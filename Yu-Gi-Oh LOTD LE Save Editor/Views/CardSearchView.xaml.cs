using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YuGiOhSaveEditor.Controls;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Views;

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

    // ── Zoom ─────────────────────────────────────────────────────────────────
    private const double MinZoom = 0.75;
    private const double MaxZoom = 2.0;
    private const double ZoomStep = 0.25;
    private double _zoom = 1.0;

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
        BanStatusCombo.ItemsSource = new[] { "All Ban Statuses", "Forbidden", "Limited", "Semi-Limited", "Unlimited" };
        TypeCombo.SelectedIndex = 0;
        AttributeCombo.SelectedIndex = 0;
        RaceCombo.SelectedIndex = 0;
        ArchetypeCombo.SelectedIndex = 0;
        BanStatusCombo.SelectedIndex = 0;

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
        BanStatusCombo.SelectedIndex = 0;
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
            banStatus: ComboFilterValue(BanStatusCombo),
            searchDescription: SearchDescCheckBox.IsChecked == true);

        var list = results.Take(ResultCap).ToList();

        ResultCards.Clear();
        foreach (var card in list)
        {
            var vm = new CardTileViewModel(card);
            ResultCards.Add(vm);
            // Always request the image - CardImageProvider itself decides
            // whether Offline Mode allows a network fetch, but an on-disk
            // cache hit from an earlier online session should still show up.
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

        // Always request the image - CardImageProvider itself decides
        // whether Offline Mode allows a network fetch, but an on-disk cache
        // hit from an earlier online session should still show up.
        // GetPreviewImageAsync (rather than GetImageAsync directly) falls
        // back to a cached thumbnail when the full-size art isn't available -
        // stretched-up and soft, but better than a blank preview.
        var image = await CardImageProvider.GetPreviewImageAsync(card);
        if (_previewCard == card) // selection may have moved on while this was loading
            PreviewImage.Source = image;
    }

    /// <summary>Same optional-field collapse helper as DeckEditorView -
    /// hides a TextBlock entirely (rather than leaving it visible-but-empty)
    /// when the given card has nothing to show for that field.</summary>
    private static void SetOptionalLine(TextBlock block, string text)
    {
        block.Text = text;
        block.Visibility = string.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
    }

    // ── Zoom ─────────────────────────────────────────────────────────────────
    // Tiles are a fixed 76x108 (ResultTileTemplate), which reads small on a
    // big or high-DPI screen - rather than resize every tile individually,
    // ZoomIn/ZoomOutButton scale ResultsList as a whole via
    // ResultsZoomTransform, a ScaleTransform already declared in XAML as the
    // ListBox's LayoutTransform (not RenderTransform - LayoutTransform is
    // applied during measure/arrange, so the ScrollViewer inside ResultsList
    // still computes correct scroll extents at the new size instead of
    // clipping/overlapping like a post-layout RenderTransform would).
    private void ZoomInButton_Click(object sender, RoutedEventArgs e) => ApplyZoom(_zoom + ZoomStep);

    private void ZoomOutButton_Click(object sender, RoutedEventArgs e) => ApplyZoom(_zoom - ZoomStep);

    private void ApplyZoom(double zoom)
    {
        _zoom = Math.Clamp(zoom, MinZoom, MaxZoom);
        ResultsZoomTransform.ScaleX = _zoom;
        ResultsZoomTransform.ScaleY = _zoom;
        ZoomText.Text = $"{_zoom * 100:0}%";
    }

    /// <summary>Ctrl+wheel over ResultsList zooms it, same as the +/-
    /// buttons - e.Handled stops the wheel from also scrolling the list at
    /// the same time.</summary>
    private void ResultsList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control) return;
        ApplyZoom(_zoom + (e.Delta > 0 ? ZoomStep : -ZoomStep));
        e.Handled = true;
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
        if (e.ClickCount != 1 || _previewCard == null) return;
        ImageViewer.Show(Window.GetWindow(this), PreviewImage.Source, _previewCard.Name);
    }
}
