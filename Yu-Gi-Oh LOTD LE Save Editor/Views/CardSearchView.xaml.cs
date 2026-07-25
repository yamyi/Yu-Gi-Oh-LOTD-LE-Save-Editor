using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
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

    // Every keystroke in SearchBox used to call RefreshResults() directly,
    // which re-runs CardDatabase.Filter over the whole card list AND kicks
    // off a fire-and-forget LoadImageAsync for up to ResultCap tiles - typing
    // a whole word fast fired that entire burst once per character, with
    // every earlier burst's image loads still in flight and competing for
    // I/O/CPU with the newest one. This timer coalesces those into a single
    // RefreshResults() call ~250ms after the user stops typing, same fix as
    // any standard "search-as-you-type" debounce. Only SearchBox's
    // TextChanged goes through it - the combo/checkbox filters below still
    // call RefreshResults() directly since those only fire once per action,
    // not once per keystroke.
    private readonly DispatcherTimer _searchDebounceTimer;

    // ── Zoom ─────────────────────────────────────────────────────────────────
    private const double MinZoom = 0.75;
    private const double MaxZoom = 2.0;
    private const double ZoomStep = 0.25;
    private double _zoom = 1.0;

    public CardSearchView()
    {
        InitializeComponent();
        ResultsList.ItemsSource = ResultCards;

        _searchDebounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
        _searchDebounceTimer.Tick += (_, _) =>
        {
            _searchDebounceTimer.Stop();
            RefreshResults();
        };

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
        SortCombo.ItemsSource = CardSorting.Options;
        TypeCombo.SelectedIndex = 0;
        AttributeCombo.SelectedIndex = 0;
        RaceCombo.SelectedIndex = 0;
        ArchetypeCombo.SelectedIndex = 0;
        BanStatusCombo.SelectedIndex = 0;
        SortCombo.SelectedIndex = 0; // triggers SortCombo_SelectionChanged, which sets SortDirectionToggle's default

        _suppressFilterEvents = false;
    }

    // ── Filters ──────────────────────────────────────────────────────────────

    private void Filter_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Debounced - see _searchDebounceTimer's doc comment.
        _searchDebounceTimer.Stop();
        _searchDebounceTimer.Start();
    }

    // Two overloads, same name: SelectionChanged (the four ComboBoxes) and
    // Checked/Unchecked (the CheckBox) are different delegate types
    // (SelectionChangedEventHandler vs RoutedEventHandler) - XAML resolves
    // each wired event to whichever overload's signature actually matches.
    private void Filter_Changed(object sender, SelectionChangedEventArgs e) => RefreshResults();
    private void Filter_Changed(object sender, RoutedEventArgs e) => RefreshResults();

    /// <summary>Separate from the generic Filter_Changed above - changing
    /// the sort key itself resets SortDirectionToggle to that key's
    /// sensible default (CardSorting.DefaultAscending) before refreshing,
    /// so switching from "Name" to "ATK" doesn't leave it stuck ascending
    /// (lowest ATK first) just because that happened to be Name's
    /// direction. Unconditional (not gated on _suppressFilterEvents) since
    /// setting IsChecked here is just UI state, not a query re-run -
    /// RefreshResults still no-ops on its own during suppression.</summary>
    private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SortDirectionToggle.IsChecked = CardSorting.DefaultAscending(SortCombo.SelectedItem as string);
        RefreshResults();
    }

    private void SortDirectionToggle_Click(object sender, RoutedEventArgs e) => RefreshResults();

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
        MinRankBox.Text = string.Empty;
        MaxRankBox.Text = string.Empty;
        MinLinkBox.Text = string.Empty;
        MaxLinkBox.Text = string.Empty;
        MinAtkBox.Text = string.Empty;
        MaxAtkBox.Text = string.Empty;
        MinDefBox.Text = string.Empty;
        MaxDefBox.Text = string.Empty;
        SearchDescCheckBox.IsChecked = true; // matches the default in XAML - description search is on by default
        SortCombo.SelectedIndex = 0;
        FavoritesOnlyCheckBox.IsChecked = false;
        _suppressFilterEvents = false;

        // Clearing SearchBox.Text above still raises TextChanged, which
        // would otherwise leave a pending debounced refresh to fire ~250ms
        // from now on top of the explicit one below - harmless (same
        // now-empty query either way) but redundant.
        _searchDebounceTimer.Stop();
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
            minRank: ParseInt(MinRankBox.Text), maxRank: ParseInt(MaxRankBox.Text),
            minLinkVal: ParseInt(MinLinkBox.Text), maxLinkVal: ParseInt(MaxLinkBox.Text),
            minAtk: ParseInt(MinAtkBox.Text), maxAtk: ParseInt(MaxAtkBox.Text),
            minDef: ParseInt(MinDefBox.Text), maxDef: ParseInt(MaxDefBox.Text),
            banStatus: ComboFilterValue(BanStatusCombo),
            searchDescription: SearchDescCheckBox.IsChecked == true);

        if (FavoritesOnlyCheckBox.IsChecked == true)
            results = results.Where(c => FavoritesStore.IsFavorite(c.Id));

        results = results.Sort(SortCombo.SelectedItem as string, SortDirectionToggle.IsChecked == true);

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
        PreviewContent.Visibility = Visibility.Visible;
        PreviewName.Text = card.Name;
        PreviewFavoriteToggle.IsChecked = FavoritesStore.IsFavorite(card.Id);
        PreviewPasscode.Text = CardPreviewFormatter.Passcode(card);
        SetOptionalLine(PreviewOwned, CardPreviewFormatter.OwnedCount(card));
        SetOptionalLine(PreviewRaceText, CardPreviewFormatter.RaceText(card));
        PreviewAttributeText.Text = CardPreviewFormatter.AttributeOrSubTypeText(card);
        PreviewTypeIcon.Source = card.IsMonster
            ? AttributeTypeIconProvider.TypeIcon(card.Race)
            : AttributeTypeIconProvider.CardTypeIcon(card.Type);
        PreviewAttributeIcon.Source = card.IsMonster
            ? AttributeTypeIconProvider.AttributeIcon(card.Attribute)
            : AttributeTypeIconProvider.SubTypeIcon(card.HumanReadableCardType);
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
        SetOptionalLine(PreviewShop, CardPreviewFormatter.ShopInfo(card));
        SetOptionalLine(PreviewDuelistChallenge, CardPreviewFormatter.DuelistChallengeInfo(card));

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
    /// hides the field entirely (rather than leaving it visible-but-empty)
    /// when the given card has nothing to show for that field. Takes a
    /// TextBox, not TextBlock - every preview field is a chrome-less
    /// read-only TextBox now (see SelectableTextStyle) so its text can be
    /// selected/copied like any other text in the app.</summary>
    private static void SetOptionalLine(TextBox block, string text)
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

    /// <summary>Toggling the star in the preview panel writes straight to
    /// FavoritesStore (the preview isn't backed by a CardTileViewModel), then
    /// syncs any matching tile(s) already showing in ResultCards so a
    /// result-grid star doesn't sit stale until the next search refresh -
    /// CardTileViewModel.IsFavorite's own setter also writes to
    /// FavoritesStore, but that's a harmless no-op re-write of the same
    /// value, not a second toggle.</summary>
    private void PreviewFavoriteToggle_Click(object sender, RoutedEventArgs e)
    {
        if (_previewCard == null) return;

        bool value = PreviewFavoriteToggle.IsChecked == true;
        FavoritesStore.SetFavorite(_previewCard.Id, value);

        foreach (var vm in ResultCards.Where(t => t.Card.Id == _previewCard.Id))
            vm.IsFavorite = value;
    }
}
