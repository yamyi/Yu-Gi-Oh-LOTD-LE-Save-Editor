using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using YuGiOhSaveEditor.Controls;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Views;

/// <summary>
/// Master Duel-styled Deck Editor - left preview / center Main+Extra+Side
/// deck grids / right searchable Card List, all populated from
/// ObservableCollection&lt;CardTileViewModel&gt; properties bound via
/// ElementName=Root in DeckEditorView.xaml (same pattern as
/// PlaceholderView's PageTitle binding). No control is ever built in C# -
/// every tile comes from the two DataTemplates declared in the XAML, and the
/// only "extra" visual (DragGhost) is a single element declared once in the
/// XAML that drag handlers merely show/move/hide.
/// </summary>
public partial class DeckEditorView : UserControl, INotifyPropertyChanged
{
    /// <summary>Raised after Save Deck actually mutates
    /// AppContext.State.SaveBytes, so MainWindow can refresh its Save
    /// button/dirty state - same pattern as DeckSlotsView.SaveDataChanged.</summary>
    public event EventHandler? SaveDataChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<CardTileViewModel> MainDeckCards { get; } = new();
    public ObservableCollection<CardTileViewModel> ExtraDeckCards { get; } = new();
    public ObservableCollection<CardTileViewModel> SideDeckCards { get; } = new();
    public ObservableCollection<CardTileViewModel> CardListResults { get; } = new();

    // ── Deck stats (bound via ElementName=Root in the XAML) ──────────────────
    // Plain computed properties, not part of any collection - this UserControl
    // has to implement INotifyPropertyChanged itself for those bindings to
    // refresh, since ElementName bindings source directly off this instance
    // rather than through a DataContext/ViewModel.
    private int _monsterCount, _spellCount, _trapCount;
    public int MonsterCount { get => _monsterCount; private set { _monsterCount = value; OnPropertyChanged(); } }
    public int SpellCount { get => _spellCount; private set { _spellCount = value; OnPropertyChanged(); } }
    public int TrapCount { get => _trapCount; private set { _trapCount = value; OnPropertyChanged(); } }

    private string _avgLevelText = "Avg LV: -";
    public string AvgLevelText { get => _avgLevelText; private set { _avgLevelText = value; OnPropertyChanged(); } }
    private string _avgAtkText = "Avg ATK: -";
    public string AvgAtkText { get => _avgAtkText; private set { _avgAtkText = value; OnPropertyChanged(); } }
    private string _avgDefText = "Avg DEF: -";
    public string AvgDefText { get => _avgDefText; private set { _avgDefText = value; OnPropertyChanged(); } }

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private SlotIO.SlotInfo? _currentSlot;
    private Card? _previewCard;
    private bool _cardListLoaded;
    private bool _comboOptionsLoaded;
    private bool _suppressFilterEvents;

    // ── Zoom ─────────────────────────────────────────────────────────────────
    private const double MinZoom = 0.75;
    private const double MaxZoom = 2.0;
    private const double ZoomStep = 0.25;
    private double _zoom = 1.0;
    private double _deckZoom = 1.0;

    // ── Drag & drop state ────────────────────────────────────────────────────
    // Set right before DragDrop.DoDragDrop and read back by whichever list's
    // Drop handler ends up firing - DoDragDrop blocks synchronously on this
    // same instance for the whole gesture, so plain fields are enough; no
    // need to round-trip the payload through the DataObject itself.
    private Point? _dragStartPoint;
    private CardTileViewModel? _draggedVm;
    private string? _draggedSourceSection; // "CardList" / "Main" / "Extra" / "Side"

    public DeckEditorView()
    {
        InitializeComponent();
        CardListBox.ItemsSource = CardListResults;

        // Recompute the stats panel off any deck mutation, whatever the
        // source - click-to-add, drag-drop, Clear Deck, or LoadDeck's own
        // PopulateSection calls all just Add/Remove/Clear these three
        // ObservableCollections, so one CollectionChanged subscription per
        // collection covers every code path with nothing extra to call at
        // each individual add/remove site.
        MainDeckCards.CollectionChanged += DeckCards_CollectionChanged;
        ExtraDeckCards.CollectionChanged += DeckCards_CollectionChanged;
        SideDeckCards.CollectionChanged += DeckCards_CollectionChanged;
    }

    private void DeckCards_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => RefreshStats();

    private void RefreshStats()
    {
        var all = MainDeckCards.Concat(ExtraDeckCards).Concat(SideDeckCards).Select(vm => vm.Card).ToList();

        MonsterCount = all.Count(c => c.IsMonster);
        SpellCount = all.Count(c => c.IsSpell);
        TrapCount = all.Count(c => c.IsTrap);

        var monsters = all.Where(c => c.IsMonster).ToList();
        var withLevel = monsters.Where(c => c.Level.HasValue).ToList();
        var withAtk = monsters.Where(c => c.Atk.HasValue).ToList();
        var withDef = monsters.Where(c => c.Def.HasValue).ToList();

        AvgLevelText = withLevel.Count > 0 ? $"Avg LV: {withLevel.Average(c => c.Level!.Value):0.#}" : "Avg LV: -";
        AvgAtkText = withAtk.Count > 0 ? $"Avg ATK: {withAtk.Average(c => c.Atk!.Value):0}" : "Avg ATK: -";
        AvgDefText = withDef.Count > 0 ? $"Avg DEF: {withDef.Average(c => c.Def!.Value):0}" : "Avg DEF: -";
    }

    /// <summary>Loads the given slot's Main/Extra/Side decks - same
    /// signature as the WinForms build's DeckEditorPage.LoadDeck(SlotInfo,
    /// CardDatabase), called from MainWindow's EditCardsRequested handler.</summary>
    public void LoadDeck(SlotIO.SlotInfo slot, CardDatabase cardDb)
    {
        _currentSlot = slot;
        DeckNameBox.Text = string.IsNullOrWhiteSpace(slot.Name) ? $"Deck {slot.SlotNumber}" : slot.Name;
        MainDeckCards.Clear();
        ExtraDeckCards.Clear();
        SideDeckCards.Clear();
        ClearPreview();

        if (AppContext.State?.SaveBytes != null)
        {
            var (_, _, _, mainAll, extraAll, sideAll) = AppContext.SlotIo.ReadLOTDDeckCards(AppContext.State.SaveBytes, slot);
            PopulateSection(mainAll, MainDeckCards, cardDb);
            PopulateSection(extraAll, ExtraDeckCards, cardDb);
            PopulateSection(sideAll, SideDeckCards, cardDb);
        }

        if (!_cardListLoaded)
        {
            _cardListLoaded = true;
            PopulateFilterCombos();
            RefreshSearchResults();
        }
    }

    /// <summary>Same combo-population approach CardSearchView uses -
    /// deferred until a save is actually loaded (LoadDeck's first call is
    /// always after MainWindow.OnOpenSave has called AppContext.CardDb.Load,
    /// since Deck Editor only ever opens via a Deck Slots double-click),
    /// rather than in the constructor, when AppContext.CardDb is still
    /// empty.</summary>
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
        _comboOptionsLoaded = true;
    }

    /// <summary>Reloads whichever slot is currently open, straight off
    /// AppContext.State.Slots/SaveBytes - the fix for Undo/Redo appearing to
    /// "do nothing" on this page. MainWindow.AfterUndoRedo already reverted
    /// AppContext.State.SaveBytes and refreshed AppContext.State.Slots by
    /// the time it calls this, but never told this view to re-read anything,
    /// so the Main/Extra/Side lists kept showing whatever was on screen
    /// before the undo/redo even though the underlying save bytes had
    /// changed. Does nothing if no slot has been opened yet (LoadDeck was
    /// never called) or that slot no longer resolves.</summary>
    public void RefreshFromState()
    {
        if (_currentSlot == null) return;

        var refreshed = AppContext.State.Slots.FirstOrDefault(s => s.SlotNumber == _currentSlot.SlotNumber);
        if (refreshed != null) LoadDeck(refreshed, AppContext.CardDb);
    }

    private static void PopulateSection(ushort[] lotdIds, ObservableCollection<CardTileViewModel> target, CardDatabase cardDb)
    {
        foreach (ushort id in lotdIds)
        {
            if (id == 0) continue;
            var card = cardDb.GetByLotdId(id);
            if (card == null) continue;

            var vm = new CardTileViewModel(card);
            target.Add(vm);
            // Always request the image - CardImageProvider itself decides
            // whether Offline Mode allows a network fetch, but an on-disk
            // cache hit from an earlier online session should still show up.
            _ = vm.LoadImageAsync(small: true);
        }
    }

    // ── Card List / search ──────────────────────────────────────────────────

    // Two overloads, same name - SelectionChanged (the four ComboBoxes) and
    // Checked/Unchecked (the CheckBox) are different delegate types, exactly
    // the same pattern CardSearchView's Filter_Changed pair uses.
    private void Filter_Changed(object sender, SelectionChangedEventArgs e) => RefreshSearchResults();
    private void Filter_Changed(object sender, RoutedEventArgs e) => RefreshSearchResults();
    private void Filter_TextChanged(object sender, TextChangedEventArgs e) => RefreshSearchResults();

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
        SearchDescCheckBox.IsChecked = true;
        _suppressFilterEvents = false;

        RefreshSearchResults();
    }

    /// <summary>Same CardDatabase.Filter call CardSearchView's RefreshResults
    /// makes, off this panel's own Type/Attribute/Race/Archetype combos and
    /// Level/ATK/DEF range boxes - the two views' card-search panels stay
    /// behaviorally identical, just reflowed for this column's narrower
    /// width (see the XAML).</summary>
    private void RefreshSearchResults()
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

        var list = results.Take(200).ToList();

        CardListResults.Clear();
        foreach (var card in list)
        {
            var vm = new CardTileViewModel(card);
            CardListResults.Add(vm);
            // Always request the image - CardImageProvider itself decides
            // whether Offline Mode allows a network fetch, but an on-disk
            // cache hit from an earlier online session should still show up.
            _ = vm.LoadImageAsync(small: true);
        }

        ResultCountText.Text = list.Count == 200 ? "200+ cards - refine your search" : $"{list.Count} card{(list.Count == 1 ? "" : "s")}";
    }

    private static string? ComboFilterValue(ComboBox combo) => combo.SelectedIndex <= 0 ? null : combo.SelectedItem as string;

    private static int? ParseInt(string? text) => int.TryParse(text, out int value) ? value : null;

    // ── Zoom ─────────────────────────────────────────────────────────────────
    // Same idea as CardSearchView's zoom controls: CardListTileTemplate's
    // tiles are a fixed 76x108, which reads small on a big/high-DPI screen,
    // so ZoomIn/ZoomOutButton scale CardListBox as a whole via
    // CardListZoomTransform (a ScaleTransform declared in XAML as its
    // LayoutTransform, not RenderTransform - LayoutTransform participates in
    // measure/arrange, so CardListBox's own ScrollViewer still computes
    // correct scroll extents at the new size).
    private void ZoomInButton_Click(object sender, RoutedEventArgs e) => ApplyZoom(_zoom + ZoomStep);

    private void ZoomOutButton_Click(object sender, RoutedEventArgs e) => ApplyZoom(_zoom - ZoomStep);

    private void ApplyZoom(double zoom)
    {
        _zoom = Math.Clamp(zoom, MinZoom, MaxZoom);
        CardListZoomTransform.ScaleX = _zoom;
        CardListZoomTransform.ScaleY = _zoom;
        ZoomText.Text = $"{_zoom * 100:0}%";
    }

    /// <summary>Ctrl+wheel over CardListBox zooms it, same as the +/-
    /// buttons - e.Handled stops the wheel from also scrolling the list at
    /// the same time.</summary>
    private void CardListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control) return;
        ApplyZoom(_zoom + (e.Delta > 0 ? ZoomStep : -ZoomStep));
        e.Handled = true;
    }

    // ── Deck cards zoom (Main/Extra/Side, shared) ───────────────────────────
    private void DeckZoomInButton_Click(object sender, RoutedEventArgs e) => ApplyDeckZoom(_deckZoom + ZoomStep);

    private void DeckZoomOutButton_Click(object sender, RoutedEventArgs e) => ApplyDeckZoom(_deckZoom - ZoomStep);

    private void ApplyDeckZoom(double zoom)
    {
        _deckZoom = Math.Clamp(zoom, MinZoom, MaxZoom);
        DeckCardsZoomTransform.ScaleX = _deckZoom;
        DeckCardsZoomTransform.ScaleY = _deckZoom;
        DeckZoomText.Text = $"{_deckZoom * 100:0}%";
    }

    /// <summary>Ctrl+wheel anywhere over the Main/Extra/Side scroll area
    /// zooms all three together, same as DeckZoomIn/OutButton.</summary>
    private void DeckScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control) return;
        ApplyDeckZoom(_deckZoom + (e.Delta > 0 ? ZoomStep : -ZoomStep));
        e.Handled = true;
    }

    private void CardListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // A click here is a "preview this" gesture, not a "select this"
        // gesture - show the preview, then immediately drop the selection so
        // the tile never sits there visually highlighted. Setting
        // SelectedIndex back to -1 re-enters this handler synchronously with
        // SelectedItem == null, which just falls through and does nothing.
        if (CardListBox.SelectedItem is CardTileViewModel vm)
        {
            _ = ShowPreviewAsync(vm.Card);
            CardListBox.SelectedIndex = -1;
        }
    }

    private void CardListItem_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is CardTileViewModel vm)
            AddCardToDeck(vm.Card);
    }

    // ── Deck list selection / removal ───────────────────────────────────────

    private void DeckList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Same "preview only, never stays selected" behavior as
        // CardListBox_SelectionChanged above.
        if (sender is ListBox list && list.SelectedItem is CardTileViewModel vm)
        {
            _ = ShowPreviewAsync(vm.Card);
            list.SelectedIndex = -1;
        }
    }

    private void DeckItem_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is CardTileViewModel vm)
            RemoveFromAnySection(vm);
    }

    private void RemoveFromAnySection(CardTileViewModel vm)
    {
        if (!MainDeckCards.Remove(vm))
            if (!ExtraDeckCards.Remove(vm))
                SideDeckCards.Remove(vm);
    }

    /// <summary>Empties Main/Extra/Side in the editor only - like every other
    /// add/remove here, nothing touches AppContext.State.SaveBytes (and so
    /// nothing needs an Undo.Snapshot) until Save Deck is actually clicked.</summary>
    private void ClearDeckButton_Click(object sender, RoutedEventArgs e)
    {
        if (MainDeckCards.Count == 0 && ExtraDeckCards.Count == 0 && SideDeckCards.Count == 0) return;

        var owner = Window.GetWindow(this);
        var result = AppMessageBox.Show(owner, "Remove every card from the Main, Extra, and Side Deck?",
            "Clear Deck", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (result != MessageBoxResult.Yes) return;

        MainDeckCards.Clear();
        ExtraDeckCards.Clear();
        SideDeckCards.Clear();
        ClearPreview();
    }

    // ── Preview panel ────────────────────────────────────────────────────────

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
        SetOptionalLine(PreviewShop, CardPreviewFormatter.ShopInfo(card));

        PreviewDesc.Text = card.Desc;
        AddToDeckButton.IsEnabled = true;
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

    /// <summary>Shared helper for the preview panel's optional fields
    /// (Archetype, Pendulum Scale, Link Markers, ban status, monster
    /// subtype) - collapses the TextBlock entirely when there's nothing to
    /// show instead of leaving a blank line in the StackPanel.</summary>
    private static void SetOptionalLine(TextBlock block, string text)
    {
        block.Text = text;
        block.Visibility = string.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
    }

    private void ClearPreview()
    {
        _previewCard = null;
        PreviewPlaceholder.Visibility = Visibility.Visible;
        PreviewName.Text = string.Empty;
        PreviewPasscode.Text = string.Empty;
        PreviewTypeRace.Text = string.Empty;
        PreviewStatsPanel.Visibility = Visibility.Collapsed;
        SetOptionalLine(PreviewMonsterType, string.Empty);
        SetOptionalLine(PreviewArchetype, string.Empty);
        SetOptionalLine(PreviewScale, string.Empty);
        SetOptionalLine(PreviewLinkMarkers, string.Empty);
        SetOptionalLine(PreviewBanStatus, string.Empty);
        SetOptionalLine(PreviewShop, string.Empty);
        PreviewDesc.Text = string.Empty;
        PreviewImage.Source = null;
        AddToDeckButton.IsEnabled = false;
        ViewOnYgoprodeckButton.IsEnabled = false;
    }

    private void AddToDeckButton_Click(object sender, RoutedEventArgs e)
    {
        if (_previewCard != null) AddCardToDeck(_previewCard);
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

    // ── Add / remove ─────────────────────────────────────────────────────────
    // A double-click (or the preview panel's Add button) tries the card's
    // natural section first (Extra for Fusion/Synchro/XYZ/Link, Main
    // otherwise), falls back to the Side Deck if that's full, and does
    // nothing at all if every section is full - no blocking MessageBox
    // anymore, since drag-and-drop already gives an explicit, deliberate
    // placement when that matters. Standard 3-copies-per-card limit, counted
    // across Main+Extra+Side combined (not per section).

    /// <summary>How many copies of this exact card (by Card.Id) are already
    /// somewhere in the deck, added up across all three sections.</summary>
    private int CountInDeck(int cardId) =>
        MainDeckCards.Count(vm => vm.Card.Id == cardId) +
        ExtraDeckCards.Count(vm => vm.Card.Id == cardId) +
        SideDeckCards.Count(vm => vm.Card.Id == cardId);

    private bool CanAddAnotherCopy(Card card) => CountInDeck(card.Id) < 3;

    private void AddCardToDeck(Card card)
    {
        if (!CanAddAnotherCopy(card)) return; // already 3 copies in the deck - do nothing

        bool isExtra = DeckEditorHelpers.IsExtraDeckCard(card);
        var primary = isExtra ? ExtraDeckCards : MainDeckCards;
        int primaryLimit = isExtra ? 15 : 60;

        ObservableCollection<CardTileViewModel> target;
        if (primary.Count < primaryLimit)
            target = primary;
        else if (SideDeckCards.Count < 15)
            target = SideDeckCards;
        else
            return; // Main/Extra and Side are all full - do nothing.

        var vm = new CardTileViewModel(card);
        target.Add(vm);
        // Always request the image - CardImageProvider itself decides
        // whether Offline Mode allows a network fetch, but an on-disk cache
        // hit from an earlier online session should still show up.
        _ = vm.LoadImageAsync(small: true);
    }

    // ── Drag & drop ──────────────────────────────────────────────────────────
    // One pair of handlers drives drag-start for all four lists (CardListBox
    // plus the three deck ListBoxes) - which list a row belongs to is read
    // off its ListBox's own Tag ("CardList"/"Main"/"Extra"/"Side") via
    // ItemsControlFromItemContainer, so nothing here needs to be duplicated
    // per list.

    private void ItemContainer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _dragStartPoint = e.GetPosition(null);
    }

    private void ItemContainer_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_dragStartPoint == null || e.LeftButton != MouseButtonState.Pressed) return;
        if (sender is not FrameworkElement container || container.DataContext is not CardTileViewModel vm) return;

        var current = e.GetPosition(null);
        if (Math.Abs(current.X - _dragStartPoint.Value.X) < SystemParameters.MinimumHorizontalDragDistance &&
            Math.Abs(current.Y - _dragStartPoint.Value.Y) < SystemParameters.MinimumVerticalDragDistance)
            return;

        if (ItemsControl.ItemsControlFromItemContainer(container) is not ListBox sourceList) return;

        // Consumed - guards against DoDragDrop's own nested message pump
        // re-entering this handler for the same still-held mouse button.
        _dragStartPoint = null;
        _draggedVm = vm;
        _draggedSourceSection = sourceList.Tag as string ?? string.Empty;

        ShowDragGhost(vm);
        DragDrop.DoDragDrop(container, new DataObject(typeof(CardTileViewModel), vm), DragDropEffects.Move);
        HideDragGhost();

        _draggedVm = null;
        _draggedSourceSection = null;
    }

    /// <summary>Keeps DragGhost centered on the cursor for the whole
    /// gesture - "the card moves with the cursor". Runs on the Preview
    /// (tunneling) pass at the root, before whichever list is actually under
    /// the cursor gets its own DragOver, and never marks the event handled so
    /// that routing continues normally.</summary>
    private void Root_PreviewDragOver(object sender, DragEventArgs e)
    {
        if (DragGhost.Visibility != Visibility.Visible) return;

        var p = e.GetPosition(DragSurface);
        Canvas.SetLeft(DragGhost, p.X - DragGhost.Width / 2);
        Canvas.SetTop(DragGhost, p.Y - DragGhost.Height / 2);
    }

    private void ShowDragGhost(CardTileViewModel vm)
    {
        DragGhostImage.Source = vm.Image;
        DragGhost.Visibility = Visibility.Visible;
        DragGhost.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 0.95, TimeSpan.FromMilliseconds(120)));
        DragGhostScale.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation(0.8, 1.0, TimeSpan.FromMilliseconds(120)));
        DragGhostScale.BeginAnimation(ScaleTransform.ScaleYProperty, new DoubleAnimation(0.8, 1.0, TimeSpan.FromMilliseconds(120)));
    }

    private void HideDragGhost()
    {
        var fade = new DoubleAnimation(DragGhost.Opacity, 0, TimeSpan.FromMilliseconds(100));
        fade.Completed += (_, _) => DragGhost.Visibility = Visibility.Collapsed;
        DragGhost.BeginAnimation(OpacityProperty, fade);
    }

    /// <summary>Shared DragOver for all four lists. Reports Move only when
    /// the drop would actually be accepted - a same-section reorder always
    /// qualifies, a move into a different section only if that section can
    /// hold the card's type (see IsSectionCompatible), and a fresh add from
    /// the Card List only if the deck doesn't already have 3 copies of that
    /// card - giving a "no drop" cursor rather than silently swallowing the
    /// drop only once it lands. Section capacity is still only checked in
    /// the Drop handlers themselves.
    /// e.Handled is only set when this is actually our own internal
    /// CardTileViewModel drag - an unrelated drag (e.g. dropping a .dat save
    /// file onto the window, see MainWindow's Window_DragEnter/Window_Drop)
    /// bubbles past these lists untouched instead of having its cursor
    /// feedback swallowed here.</summary>
    private void List_DragOver(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(typeof(CardTileViewModel))) return;

        e.Effects = DragDropEffects.None;
        if (sender is ListBox targetList && _draggedVm != null)
        {
            string targetSection = targetList.Tag as string ?? string.Empty;
            bool sameSection = _draggedSourceSection == targetSection;
            bool typeOk = sameSection || IsSectionCompatible(targetSection, _draggedVm.Card);
            bool copyOk = sameSection || _draggedSourceSection != "CardList" || CanAddAnotherCopy(_draggedVm.Card);
            if (typeOk && copyOk)
                e.Effects = DragDropEffects.Move;
        }
        e.Handled = true;
    }

    private void DeckList_Drop(object sender, DragEventArgs e)
    {
        if (_draggedVm == null || sender is not ListBox targetList) return;

        string targetSection = targetList.Tag as string ?? string.Empty;
        var targetCollection = GetSectionCollection(targetSection);
        if (targetCollection == null) return;

        int index = ComputeInsertionIndex(targetList, e.GetPosition(targetList));

        if (_draggedSourceSection == targetSection)
        {
            // Pure reorder within the same section - card count doesn't
            // change, so no capacity check applies.
            int oldIndex = targetCollection.IndexOf(_draggedVm);
            if (oldIndex < 0) return;
            if (index > oldIndex) index--;
            index = Math.Clamp(index, 0, targetCollection.Count - 1);
            targetCollection.Move(oldIndex, index);
        }
        else
        {
            if (!IsSectionCompatible(targetSection, _draggedVm.Card)) return; // wrong card type for this section - do nothing

            int limit = GetSectionLimit(targetSection);
            if (targetCollection.Count >= limit) return; // full - drop does nothing

            CardTileViewModel moved;
            if (_draggedSourceSection == "CardList")
            {
                if (!CanAddAnotherCopy(_draggedVm.Card)) return; // already 3 copies in the deck - do nothing

                // Card List is the master index, not an inventory - dragging
                // out of it only ever adds a fresh instance (same as
                // AddCardToDeck); the source list itself is never touched.
                moved = new CardTileViewModel(_draggedVm.Card);
                // Always request the image - CardImageProvider itself decides
                // whether Offline Mode allows a network fetch, but an on-disk
                // cache hit from an earlier online session should still show up.
                _ = moved.LoadImageAsync(small: true);
            }
            else
            {
                RemoveFromSection(_draggedSourceSection, _draggedVm);
                moved = _draggedVm;
            }

            index = Math.Clamp(index, 0, targetCollection.Count);
            targetCollection.Insert(index, moved);
        }

        e.Handled = true;
    }

    private void CardList_Drop(object sender, DragEventArgs e)
    {
        // Dragging a deck card onto the Card List removes it from its deck
        // section but is never added to the Card List itself - that list is
        // always just the full/searched card index, not affected by deck
        // contents.
        if (_draggedVm != null && _draggedSourceSection is "Main" or "Extra" or "Side")
            RemoveFromSection(_draggedSourceSection, _draggedVm);

        e.Handled = true;
    }

    private ObservableCollection<CardTileViewModel>? GetSectionCollection(string section) => section switch
    {
        "Main" => MainDeckCards,
        "Extra" => ExtraDeckCards,
        "Side" => SideDeckCards,
        _ => null,
    };

    private static int GetSectionLimit(string section) => section is "Extra" or "Side" ? 15 : 60;

    /// <summary>Main only takes non-Extra cards and Extra only takes
    /// Fusion/Synchro/XYZ/Link cards - Side isn't restricted either way
    /// (matches real deck-building rules; only requested for Main/Extra).</summary>
    private static bool IsSectionCompatible(string section, Card card) => section switch
    {
        "Main" => !DeckEditorHelpers.IsExtraDeckCard(card),
        "Extra" => DeckEditorHelpers.IsExtraDeckCard(card),
        _ => true,
    };

    private void RemoveFromSection(string? section, CardTileViewModel vm) => GetSectionCollection(section ?? string.Empty)?.Remove(vm);

    /// <summary>Finds where in a WrapPanel-paneled ListBox a drop point falls,
    /// row by row, so a card dropped mid-deck lands at that position and
    /// pushes everything after it along - Insert/Move on the bound
    /// ObservableCollection does the actual "pushing".</summary>
    private static int ComputeInsertionIndex(ListBox list, Point dropPoint)
    {
        int count = list.Items.Count;
        for (int i = 0; i < count; i++)
        {
            if (list.ItemContainerGenerator.ContainerFromIndex(i) is not FrameworkElement container)
                continue;

            Point topLeft;
            try { topLeft = container.TranslatePoint(new Point(0, 0), list); }
            catch (InvalidOperationException) { continue; } // not connected to this visual tree yet

            var rect = new Rect(topLeft, new Size(container.ActualWidth, container.ActualHeight));

            if (dropPoint.Y < rect.Top)
                return i; // start of a new row above the drop point - insert here

            if (dropPoint.Y >= rect.Top && dropPoint.Y < rect.Bottom && dropPoint.X < rect.Left + rect.Width / 2)
                return i; // same row, before this item's midpoint
        }

        return count; // after everything
    }

    // ── Save ─────────────────────────────────────────────────────────────────

    private void SaveDeckButton_Click(object sender, RoutedEventArgs e)
    {
        var owner = Window.GetWindow(this);
        if (_currentSlot == null || AppContext.State?.SaveBytes == null) return;

        var allCards = MainDeckCards.Concat(ExtraDeckCards).Concat(SideDeckCards).ToList();
        var unresolvable = allCards.Where(vm => vm.Card.LotdId is null).ToList();
        if (unresolvable.Count > 0)
        {
            string names = string.Join(", ", unresolvable.Take(5).Select(vm => vm.Card.Name));
            AppMessageBox.Show(owner,
                $"{unresolvable.Count} card(s) in this deck can't be saved to the save file (no in-game id): {names}{(unresolvable.Count > 5 ? "…" : "")}",
                "Save Deck", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var deck = new Deck(
            MainDeckCards.Select(vm => vm.Card).ToList(),
            ExtraDeckCards.Select(vm => vm.Card).ToList(),
            SideDeckCards.Select(vm => vm.Card).ToList());

        string name = DeckNameBox.Text?.Trim() ?? string.Empty;
        if (string.IsNullOrEmpty(name)) name = $"Deck {_currentSlot.SlotNumber}";
        if (name.Length > 32) name = name[..32];

        AppContext.Undo.Snapshot();
        AppContext.SlotIo.WriteSlot(AppContext.State.SaveBytes, _currentSlot.SlotNumber, deck, name, _currentSlot.OwnerId);

        _currentSlot = AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, _currentSlot.SlotNumber);
        AppContext.State.Slots = AppContext.SlotIo.ReadAllSlots(AppContext.State.SaveBytes).ToArray();
        SaveDataChanged?.Invoke(this, EventArgs.Empty);

        AppMessageBox.Show(owner, $"Saved deck to slot #{_currentSlot.SlotNumber}.", "Save Deck",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
