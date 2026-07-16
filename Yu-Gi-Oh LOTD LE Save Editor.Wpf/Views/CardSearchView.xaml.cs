using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YuGiOhSaveEditor.Wpf.Controls;
using YuGiOhSaveEditor.Wpf.Services;

namespace YuGiOhSaveEditor.Wpf.Views;

/// <summary>
/// Browse-only card database screen - search + an advanced filter panel
/// (Type/Attribute/Race/Archetype + Level/ATK/DEF ranges, straight off
/// CardDatabase.Filter's own parameters) over a results grid, with a
/// left-side preview panel. No deck is attached here, so clicking a card
/// only ever previews it - there's nothing to add it to. Every tile comes
/// from ResultTileTemplate in the XAML; this class only ever populates
/// ResultCards (an ObservableCollection&lt;CardTileViewModel&gt;) and sets
/// properties on already-declared elements - no controls are built here.
/// </summary>
public partial class CardSearchView : UserControl
{
    public ObservableCollection<CardTileViewModel> ResultCards { get; } = new();

    private Card? _previewCard;
    private bool _comboOptionsLoaded;
    private bool _suppressFilterEvents;

    private const int ResultCap = 300;

    public CardSearchView()
    {
        InitializeComponent();
        ResultsList.ItemsSource = ResultCards;

        // CardDatabase.Load() only ever runs once a save file is opened
        // (MainWindow.OnOpenSave), so this page has to (re)populate itself
        // whenever it's actually navigated to rather than once in the
        // constructor - by the time this UserControl is constructed at
        // MainWindow startup, AppContext.CardDb is very likely still empty.
        IsVisibleChanged += (_, e) =>
        {
            if (e.NewValue is true) RefreshForCardDb();
        };
    }

    private void RefreshForCardDb()
    {
        bool hasData = AppContext.CardDb.DistinctTypes().Count > 0;
        NoDbPlaceholder.Visibility = hasData ? Visibility.Collapsed : Visibility.Visible;
        ResultsList.Visibility = hasData ? Visibility.Visible : Visibility.Collapsed;

        if (hasData && !_comboOptionsLoaded)
        {
            PopulateFilterCombos();
            _comboOptionsLoaded = true;
        }

        if (hasData) RefreshResults();
    }

    private void PopulateFilterCombos()
    {
        _suppressFilterEvents = true;

        TypeCombo.ItemsSource = new[] { "All Types" }.Concat(AppContext.CardDb.DistinctTypes()).ToList();
        AttributeCombo.ItemsSource = new[] { "All Attributes" }.Concat(AppContext.CardDb.DistinctAttributes()).ToList();
        RaceCombo.ItemsSource = new[] { "All Races" }.Concat(AppContext.CardDb.DistinctRaces()).ToList();
        ArchetypeCombo.ItemsSource = new[] { "All Archetypes" }.Concat(AppContext.CardDb.DistinctArchetypes()).ToList();
        TypeCombo.SelectedIndex = 0;
        AttributeCombo.SelectedIndex = 0;
        RaceCombo.SelectedIndex = 0;
        ArchetypeCombo.SelectedIndex = 0;

        _suppressFilterEvents = false;
    }

    // ── Filters ──────────────────────────────────────────────────────────────

    private void Filter_TextChanged(object sender, TextChangedEventArgs e) => RefreshResults();

    // Two overloads, same name: SelectionChanged (the four ComboBoxes) and
    // Checked/Unchecked (the CheckBox) are different delegate types
    // (SelectionChangedEventHandler vs RoutedEventHandler) - XAML resolves
    // each wired event to whichever overload's signature actually matches.
    private void Filter_Changed(object sender, SelectionChangedEventArgs e) => RefreshResults();
    private void Filter_Changed(object sender, RoutedEventArgs e) => RefreshResults();

    private void ClearFilters_Click(object sender, RoutedEventArgs e)
    {
        _suppressFilterEvents = true;
        SearchBox.Text = string.Empty;
        TypeCombo.SelectedIndex = 0;
        AttributeCombo.SelectedIndex = 0;
        RaceCombo.SelectedIndex = 0;
        ArchetypeCombo.SelectedIndex = 0;
        MinLevelBox.Text = string.Empty;
        MaxLevelBox.Text = string.Empty;
        MinAtkBox.Text = string.Empty;
        MaxAtkBox.Text = string.Empty;
        MinDefBox.Text = string.Empty;
        MaxDefBox.Text = string.Empty;
        SearchDescCheckBox.IsChecked = true; // matches the default in XAML - description search is on by default
        _suppressFilterEvents = false;

        RefreshResults();
    }

    private void RefreshResults()
    {
        if (_suppressFilterEvents || !_comboOptionsLoaded) return;

        string query = SearchBox.Text?.Trim() ?? string.Empty;
        var results = AppContext.CardDb.Filter(
            name: string.IsNullOrEmpty(query) ? null : query,
            type: ComboFilterValue(TypeCombo),
            attribute: ComboFilterValue(AttributeCombo),
            race: ComboFilterValue(RaceCombo),
            archetype: ComboFilterValue(ArchetypeCombo),
            minLevel: ParseInt(MinLevelBox.Text), maxLevel: ParseInt(MaxLevelBox.Text),
            minAtk: ParseInt(MinAtkBox.Text), maxAtk: ParseInt(MaxAtkBox.Text),
            minDef: ParseInt(MinDefBox.Text), maxDef: ParseInt(MaxDefBox.Text),
            searchDescription: SearchDescCheckBox.IsChecked == true);

        var list = results.Take(ResultCap).ToList();

        ResultCards.Clear();
        foreach (var card in list)
        {
            var vm = new CardTileViewModel(card);
            ResultCards.Add(vm);
            if (!AppContext.State.IsOfflineMode)
                _ = vm.LoadImageAsync(small: true);
        }

        ResultCountText.Text = list.Count == ResultCap
            ? $"{ResultCap}+ cards - refine your search"
            : $"{list.Count} card{(list.Count == 1 ? "" : "s")}";
    }

    private static string? ComboFilterValue(ComboBox combo) => combo.SelectedIndex <= 0 ? null : combo.SelectedItem as string;

    private static int? ParseInt(string? text) => int.TryParse(text, out int value) ? value : null;

    // ── Selection / preview ─────────────────────────────────────────────────
    // Same "preview only, never stays selected" behavior established in
    // Deck Editor - a click here just shows the card, it doesn't select it.

    private void ResultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ResultsList.SelectedItem is CardTileViewModel vm)
        {
            _ = ShowPreviewAsync(vm.Card);
            ResultsList.SelectedIndex = -1;
        }
    }

    private async Task ShowPreviewAsync(Card card)
    {
        _previewCard = card;
        PreviewPlaceholder.Visibility = Visibility.Collapsed;
        PreviewName.Text = card.Name;
        PreviewPasscode.Text = CardPreviewFormatter.Passcode(card);
        PreviewTypeRace.Text = CardPreviewFormatter.TypeRace(card);
        SetOptionalLine(PreviewMonsterType, CardPreviewFormatter.MonsterType(card));
        SetOptionalLine(PreviewArchetype, CardPreviewFormatter.Archetype(card));

        if (card.IsMonster)
        {
            PreviewStatsPanel.Visibility = Visibility.Visible;
            PreviewLevel.Text = CardPreviewFormatter.Level(card);
            PreviewAtk.Text = CardPreviewFormatter.Atk(card);
            PreviewDef.Text = CardPreviewFormatter.Def(card);
        }
        else
        {
            PreviewStatsPanel.Visibility = Visibility.Collapsed;
        }

        SetOptionalLine(PreviewScale, CardPreviewFormatter.Scale(card));
        SetOptionalLine(PreviewLinkMarkers, CardPreviewFormatter.LinkMarkers(card));
        SetOptionalLine(PreviewBanStatus, CardPreviewFormatter.BanStatus(card));

        PreviewDesc.Text = card.Desc;
        ViewOnYgoprodeckButton.IsEnabled = !string.IsNullOrWhiteSpace(card.YgoprodeckUrl);
        PreviewImage.Source = null;

        if (!AppContext.State.IsOfflineMode)
        {
            var image = await CardImageProvider.GetImageAsync(card, small: false);
            if (_previewCard == card) // selection may have moved on while this was loading
                PreviewImage.Source = image;
        }
    }

    /// <summary>Same optional-field collapse helper as DeckEditorView -
    /// hides a TextBlock entirely (rather than leaving it visible-but-empty)
    /// when the given card has nothing to show for that field.</summary>
    private static void SetOptionalLine(TextBlock block, string text)
    {
        block.Text = text;
        block.Visibility = string.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
    }

    private void ViewOnYgoprodeckButton_Click(object sender, RoutedEventArgs e)
    {
        if (_previewCard == null) return;

        if (!UrlLauncher.TryOpen(_previewCard.YgoprodeckUrl))
        {
            AppMessageBox.Show(Window.GetWindow(this), "Couldn't open that link in your browser.",
                "View on ygoprodeck", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void PreviewImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2 || _previewCard == null) return;
        ImageViewer.Show(Window.GetWindow(this), PreviewImage.Source, _previewCard.Name);
    }
}
