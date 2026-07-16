

using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;
using static Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services.SlotIO;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

/// <summary>
/// Full deck editor page.
/// Left  : DreamListBox search results.
/// Centre: 90 CardSlot controls across Main / Extra / Side grids.
/// Right : Card preview panel (image or text detail).
/// Toolbar: search box, filter, sort, deck name, save.
/// All control creation lives in DeckEditorPage.Designer.cs.
/// </summary>
public sealed partial class DeckEditorPage : UserControl
{
    // ── State ─────────────────────────────────────────────────────────────────
    private bool _isOnline;
    private readonly CardSlot[] _mainSlots;
    private readonly CardSlot[] _extraSlots;
    private readonly CardSlot[] _sideSlots;
    private CardFilterOptions? _activeFilter;
    private Card? _previewCard;
    private SlotInfo? _currentSlot;

    // ── Keyboard navigation ──────────────────────────────────────────────────
    private CardSlot[]? _focusedSection;
    private int _focusedIndex = -1;
    private const int GridColumns = 10; // matches the Designer's fixed 64px-stride layout

    // ── Events ────────────────────────────────────────────────────────────────
    public event EventHandler? SaveRequested;
    public event EventHandler? FilterRequested;
    public event EventHandler? SortRequested;

    public DeckEditorPage()
    {
        InitializeComponent();

        // Shows the active theme's background image (if any) behind the
        // page — see BackgroundPainter. No-ops entirely when no image is set.
        Paint += BackgroundPainter.Paint;

        // Populate convenience arrays after InitializeComponent has created the fields.
        _mainSlots = new[]
        {
            slotMain00, slotMain01, slotMain02, slotMain03, slotMain04,
            slotMain05, slotMain06, slotMain07, slotMain08, slotMain09,
            slotMain10, slotMain11, slotMain12, slotMain13, slotMain14,
            slotMain15, slotMain16, slotMain17, slotMain18, slotMain19,
            slotMain20, slotMain21, slotMain22, slotMain23, slotMain24,
            slotMain25, slotMain26, slotMain27, slotMain28, slotMain29,
            slotMain30, slotMain31, slotMain32, slotMain33, slotMain34,
            slotMain35, slotMain36, slotMain37, slotMain38, slotMain39,
            slotMain40, slotMain41, slotMain42, slotMain43, slotMain44,
            slotMain45, slotMain46, slotMain47, slotMain48, slotMain49,
            slotMain50, slotMain51, slotMain52, slotMain53, slotMain54,
            slotMain55, slotMain56, slotMain57, slotMain58, slotMain59,
        };

        _extraSlots = new[]
        {
            slotExtra00, slotExtra01, slotExtra02, slotExtra03, slotExtra04,
            slotExtra05, slotExtra06, slotExtra07, slotExtra08, slotExtra09,
            slotExtra10, slotExtra11, slotExtra12, slotExtra13, slotExtra14,
        };

        _sideSlots = new[]
        {
            slotSide00, slotSide01, slotSide02, slotSide03, slotSide04,
            slotSide05, slotSide06, slotSide07, slotSide08, slotSide09,
            slotSide10, slotSide11, slotSide12, slotSide13, slotSide14,
        };

        WireSlotEvents(_mainSlots);
        WireSlotEvents(_extraSlots);
        WireSlotEvents(_sideSlots);

        // Follow the app-wide Offline Mode toggle: online whenever it's
        // disabled, and re-fetch anything missing an image the moment it's
        // turned off again.
        AppContext.State.OfflineModeChanged += OnOfflineModeChanged;
        IsOnline = !AppContext.State.IsOfflineMode;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            AppContext.State.OfflineModeChanged -= OnOfflineModeChanged;

        base.Dispose(disposing);
    }

    private void OnOfflineModeChanged(bool isOffline) => IsOnline = !isOffline;

    // ── Public API ────────────────────────────────────────────────────────────
    public bool IsOnline
    {
        get => _isOnline;
        set
        {
            _isOnline = value;
            foreach (var s in _mainSlots) s.IsOnline = value;
            foreach (var s in _extraSlots) s.IsOnline = value;
            foreach (var s in _sideSlots) s.IsOnline = value;

            if (value) LoadVisibleImages();
        }
    }

    // ── Image loading ─────────────────────────────────────────────────────────
    // Only ever called while online. Fetches are async and get discarded
    // silently (no placeholder change) on failure or if the slot's content
    // changed, or the page went offline again, before the download finished.
    private void LoadVisibleImages()
    {
        foreach (var s in _mainSlots) FetchImageFor(s);
        foreach (var s in _extraSlots) FetchImageFor(s);
        foreach (var s in _sideSlots) FetchImageFor(s);

        if (_previewCard is not null) LoadPreviewImage(_previewCard);
    }

    private async void FetchImageFor(CardSlot slot)
    {
        var card = slot.Card;
        if (card is null) return;

        var image = await CardImageProvider.GetImageAsync(card, small: true);
        if (image is null || IsDisposed || !_isOnline) return;
        if (!ReferenceEquals(slot.Card, card)) return; // slot changed while we waited

        slot.SetImage(image);
    }

    private async void LoadPreviewImage(Card card)
    {
        var image = await CardImageProvider.GetImageAsync(card, small: false);
        if (image is null || IsDisposed || !_isOnline) return;
        if (!ReferenceEquals(_previewCard, card)) return; // user picked a different card

        SetPreviewImage(image);
    }

    /// <summary>The slot this page's grids were last loaded from, if any —
    /// used by MainForm to know whether/what to reload after an Undo/Redo.</summary>
    public SlotInfo? CurrentSlot => _currentSlot;

    public void SetDeckName(string name) =>
        lblDeckName.Text = name;

    public void SetSearchResults(IEnumerable<Card> cards)
    {
        lstResults.Items.Clear();
        foreach (var c in cards)
            lstResults.Items.Add(c);
        lstResults.DisplayMember = nameof(Card.Name);
    }

    public void UpdateSectionCounts(int main, int extra, int side)
    {
        lblMainCount.Text = $"MAIN DECK  ({main}/60)";
        lblExtraCount.Text = $"EXTRA DECK ({extra}/15)";
        lblSideCount.Text = $"SIDE DECK  ({side}/15)";
    }

    // ── Card preview ──────────────────────────────────────────────────────────
    private void ShowPreview(Card card)
    {
        _previewCard = card;

        lblPreviewName.Text = card.Name;
        lblPreviewType.Text = card.HumanReadableCardType;
        lblPreviewAttr.Text = card.Attribute ?? string.Empty;
        lblPreviewRace.Text = card.Race ?? string.Empty;
        lblPreviewAtk.Text = card.Atk.HasValue ? $"ATK  {card.Atk}" : string.Empty;
        lblPreviewDef.Text = card.Def.HasValue ? $"DEF  {card.Def}" : string.Empty;
        lblPreviewDesc.Text = card.Desc;
        lblPreviewAtk.Visible = card.Atk.HasValue;
        lblPreviewDef.Visible = card.Def.HasValue;

        picPreview.Image = null;
        picPreview.Visible = _isOnline;
        pnlPreviewText.Visible = true;

        if (_isOnline) LoadPreviewImage(card);
    }

    public void SetPreviewImage(Image image) =>
        picPreview.Image = image;

    // Double-clicking the preview art zooms it in-window (scroll to zoom,
    // click/Esc/× to close) instead of opening a separate popup window.
    private void picPreview_DoubleClick(object? sender, EventArgs e)
    {
        if (picPreview.Image is null) return;
        imgOverlay.ShowImage(picPreview.Image, _previewCard?.Name ?? "Card");
    }

    // ── Slot events ───────────────────────────────────────────────────────────
    private void WireSlotEvents(CardSlot[] slots)
    {
        foreach (var slot in slots)
        {
            slot.CardClicked += OnSlotClicked;
            slot.CardRightClicked += OnSlotRightClicked;
            slot.CardDoubleClicked += OnSlotDoubleClicked;
            slot.CardDropped += OnCardDropped;
        }
    }

    // Dragging within the same section (Main/Extra/Side) pushes every card
    // between the source and target over by one, like reordering a list,
    // instead of just swapping two positions. Dragging across sections
    // (rare — the grids are visually separate) falls back to a plain
    // two-way swap since "push" doesn't make sense across different card
    // pools with different size limits.
    private void OnCardDropped(object? sender, CardSlot source)
    {
        if (sender is not CardSlot target || ReferenceEquals(source, target)) return;
        if (source.Card is null) return;

        var section = _mainSlots.Contains(source) && _mainSlots.Contains(target) ? _mainSlots
            : _extraSlots.Contains(source) && _extraSlots.Contains(target) ? _extraSlots
            : _sideSlots.Contains(source) && _sideSlots.Contains(target) ? _sideSlots
            : null;

        if (section is not null)
            PushReorder(section, Array.IndexOf(section, source), Array.IndexOf(section, target));
        else
            SwapTwo(source, target);

        RefreshCounts();
        CardMoved?.Invoke(this, EventArgs.Empty);
    }

    // Shifts every slot between fromIndex and toIndex over by one (freeing
    // up toIndex), then drops the moved card there — same feel as
    // reordering a playlist or a row of desktop icons.
    private void PushReorder(CardSlot[] slots, int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || toIndex < 0 || fromIndex == toIndex) return;

        var movingCard = slots[fromIndex].Card;
        if (movingCard is null) return;

        int step = fromIndex < toIndex ? 1 : -1;
        for (int i = fromIndex; i != toIndex; i += step)
        {
            var next = slots[i + step].Card;
            if (next is not null) slots[i].SetCard(next);
            else slots[i].ClearCard();
        }
        slots[toIndex].SetCard(movingCard);

        if (_isOnline)
        {
            int lo = Math.Min(fromIndex, toIndex), hi = Math.Max(fromIndex, toIndex);
            for (int i = lo; i <= hi; i++) FetchImageFor(slots[i]);
        }
    }

    private void SwapTwo(CardSlot source, CardSlot target)
    {
        var sourceCard = source.Card;
        var targetCard = target.Card;
        if (sourceCard is null) return;

        target.SetCard(sourceCard);
        if (targetCard is not null) source.SetCard(targetCard);
        else source.ClearCard();

        if (_isOnline)
        {
            FetchImageFor(target);
            if (targetCard is not null) FetchImageFor(source);
        }
    }

    public event EventHandler? CardMoved;

    // Clicking a slot with the mouse also moves the keyboard-focus pointer
    // there, so arrow-key navigation picks up from wherever the user last
    // clicked instead of always restarting at Main[0].
    private void OnSlotClicked(object? sender, Card card)
    {
        ShowPreview(card);

        if (sender is not CardSlot clicked) return;

        var section = _mainSlots.Contains(clicked) ? _mainSlots
            : _extraSlots.Contains(clicked) ? _extraSlots
            : _sideSlots.Contains(clicked) ? _sideSlots
            : null;
        if (section is null) return;

        if (_focusedSection is not null && _focusedIndex >= 0 && _focusedIndex < _focusedSection.Length)
            _focusedSection[_focusedIndex].IsSelected = false;

        _focusedSection = section;
        _focusedIndex = Array.IndexOf(section, clicked);
        clicked.IsSelected = true;
    }

    private void OnSlotRightClicked(object? sender, Card card)
    {
        // Context menu wired by host form / presenter
    }

    // Double-clicking a filled slot removes that card from the deck.
    private void OnSlotDoubleClicked(object? sender, Card card)
    {
        if (sender is not CardSlot slot) return;

        slot.ClearCard();
        RefreshCounts();
        CardRemoveRequested?.Invoke(this, card);

        if (ReferenceEquals(_previewCard, card))
        {
            _previewCard = null;
            picPreview.Image = null;
            picPreview.Visible = false;
            pnlPreviewText.Visible = false;
        }
    }

    public event EventHandler<Card>? CardRemoveRequested;

    // ── Search box ────────────────────────────────────────────────────────────
    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        ApplySearch();
        SearchTextChanged?.Invoke(this, txtSearch.Text);
    }

    public event EventHandler<string>? SearchTextChanged;

    // ── Search + filter combined ─────────────────────────────────────────────
    // Empty query with no filter shows nothing rather than dumping the whole
    // card database into an owner-drawn list box. Results are capped either way.
    private void ApplySearch()
    {
        string q = txtSearch.Text.Trim();

        IEnumerable<Card> results;
        if (_activeFilter is null)
        {
            results = string.IsNullOrEmpty(q) ? Enumerable.Empty<Card>() : AppContext.CardDb.Search(q);
        }
        else
        {
            results = AppContext.CardDb.Filter(
                name: string.IsNullOrEmpty(q) ? null : q,
                type: _activeFilter.Type,
                attribute: _activeFilter.Attribute,
                race: _activeFilter.Race,
                archetype: _activeFilter.Archetype,
                id: _activeFilter.Id,
                minLevel: _activeFilter.MinLevel,
                maxLevel: _activeFilter.MaxLevel,
                minAtk: _activeFilter.MinAtk,
                maxAtk: _activeFilter.MaxAtk,
                minDef: _activeFilter.MinDef,
                maxDef: _activeFilter.MaxDef,
                minLinkVal: _activeFilter.MinLinkVal,
                maxLinkVal: _activeFilter.MaxLinkVal,
                minScale: _activeFilter.MinScale,
                maxScale: _activeFilter.MaxScale);
        }

        SetSearchResults(results.Take(200));
    }

    // ── Filter dialog ─────────────────────────────────────────────────────────
    private void btnFilter_Click(object? s, EventArgs e)
    {
        var result = CardFilterForm.Show(AppContext.CardDb, _activeFilter);
        if (result is null) return; // cancelled

        _activeFilter = result.IsEmpty ? null : result;
        btnFilter.Text = _activeFilter is null ? "Filter" : "Filter*";

        // The dialog's own Name field is just a convenience for the same
        // toolbar search box — fold it in so there's one source of truth.
        if (!string.IsNullOrWhiteSpace(result.Name))
            txtSearch.Text = result.Name;

        ApplySearch();
        FilterRequested?.Invoke(this, e);
    }

    // Fully custom-painted rows — a plain ListBox's default rendering doesn't
    // apply the theme consistently across selection states (unselected rows
    // drifted to the OS default black instead of staying themed), so every
    // pixel here comes from AppColors instead of relying on ForeColor alone.
    private void lstResults_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        var bg = selected ? AppColors.CardSelected : AppColors.CardBg;
        var fg = selected ? AppColors.TextPrimary : AppColors.TextSecondary;

        using (var bgBrush = new SolidBrush(bg))
            e.Graphics.FillRectangle(bgBrush, e.Bounds);

        if (lstResults.Items[e.Index] is Card card)
        {
            var textRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y, e.Bounds.Width - 14, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, card.Name, e.Font ?? lstResults.Font, textRect, fg,
                TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        if (selected)
        {
            using var accentPen = new Pen(AppColors.BorderSelected, 2);
            e.Graphics.DrawLine(accentPen, e.Bounds.Left, e.Bounds.Top, e.Bounds.Left, e.Bounds.Bottom);
        }
    }

    // ── Search result click → preview ─────────────────────────────────────────
    private void lstResults_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (lstResults.SelectedItem is Card card)
            ShowPreview(card);
    }

    // ── Search result double-click → add to deck ──────────────────────────────
    // Main/Extra is auto-detected from the card's type. Hold Shift while
    // double-clicking to send it to the Side deck instead.
    private void lstResults_DoubleClick(object? sender, EventArgs e)
    {
        if (lstResults.SelectedItem is Card card)
            TryAddCard(card, addToSide: ModifierKeys.HasFlag(Keys.Shift));
    }

    public event EventHandler<Card>? CardAddRequested;

    private void TryAddCard(Card card, bool addToSide)
    {
        CardSlot[] targetSlots;
        string sectionName;

        if (addToSide)
        {
            targetSlots = _sideSlots;
            sectionName = "Side";
        }
        else if (IsExtraDeckCard(card))
        {
            targetSlots = _extraSlots;
            sectionName = "Extra";
        }
        else
        {
            targetSlots = _mainSlots;
            sectionName = "Main";
        }

        var emptySlot = targetSlots.FirstOrDefault(s => s.Card is null);
        if (emptySlot is null)
        {
            AppMessageBox.Show(
                $"{sectionName} deck is full ({targetSlots.Length}/{targetSlots.Length}).",
                "Add Card", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        emptySlot.SetCard(card);
        RefreshCounts();
        if (_isOnline) FetchImageFor(emptySlot);
        CardAddRequested?.Invoke(this, card);
    }

    private static bool IsExtraDeckCard(Card card) =>
        card.Type.Contains("Fusion", StringComparison.OrdinalIgnoreCase) ||
        card.Type.Contains("Synchro", StringComparison.OrdinalIgnoreCase) ||
        card.Type.Contains("XYZ", StringComparison.OrdinalIgnoreCase) ||
        card.Type.Contains("Link", StringComparison.OrdinalIgnoreCase);

    private void RefreshCounts()
    {
        int main = _mainSlots.Count(s => s.Card is not null);
        int extra = _extraSlots.Count(s => s.Card is not null);
        int side = _sideSlots.Count(s => s.Card is not null);
        UpdateSectionCounts(main, extra, side);
    }

    // ── Toolbar buttons ───────────────────────────────────────────────────────
    private void btnSort_Click(object? s, EventArgs e) => SortRequested?.Invoke(this, e);

    // Writes the current Main/Extra/Side grids back into the slot this deck
    // was loaded from. Only touches AppContext.State.SaveBytes (in memory) —
    // the user still has to hit the app-wide Save File button to persist
    // that to disk, same as every other edit in the app.
    private void btnSave_Click(object? sender, EventArgs e)
    {
        if (_currentSlot is null)
        {
            AppMessageBox.Show(
                "No deck is loaded. Open a slot from Deck Slots first, then come back here to edit and save it.",
                "Save Deck", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var main = _mainSlots.Select(cs => cs.Card).Where(c => c is not null).Select(c => c!).ToList();
        var extra = _extraSlots.Select(cs => cs.Card).Where(c => c is not null).Select(c => c!).ToList();
        var side = _sideSlots.Select(cs => cs.Card).Where(c => c is not null).Select(c => c!).ToList();

        // Cards without a LotdId can't be encoded into the save format —
        // catch that here instead of letting Deck.Get_Lotd_*() throw.
        var unsupported = main.Concat(extra).Concat(side)
            .Where(c => c.LotdId is null)
            .Select(c => c.Name)
            .Distinct()
            .ToList();

        if (unsupported.Count > 0)
        {
            AppMessageBox.Show(
                "These cards aren't recognized by this save format and can't be saved:\n\n" +
                string.Join("\n", unsupported) +
                "\n\nRemove them from the deck before saving.",
                "Save Deck", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var deck = new Deck(main, extra, side);
        AppContext.Undo.Snapshot();
        AppContext.SlotIo.WriteSlot(AppContext.State.SaveBytes, _currentSlot.SlotNumber, deck, _currentSlot.Name, _currentSlot.OwnerId);

        _currentSlot = AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, _currentSlot.SlotNumber);

        SaveRequested?.Invoke(this, EventArgs.Empty);

        AppMessageBox.Show(
            $"Saved to slot {_currentSlot.SlotNumber} ({_currentSlot.Name}).",
            "Save Deck", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public void LoadDeck(SlotInfo slot, CardDatabase cardDb)
    {
        _currentSlot = slot;
        SetDeckName(slot.Name);

        foreach (var s in _mainSlots) s.ClearCard();
        foreach (var s in _extraSlots) s.ClearCard();
        foreach (var s in _sideSlots) s.ClearCard();

        int mainCount = 0;
        int extraCount = 0;
        int sideCount = 0;

        var (mc, ec, sc, mainAll, extraAll, sideAll) = AppContext.SlotIo.ReadLOTDDeckCards(AppContext.State.SaveBytes, slot);

        var MainDeckList = mainAll.Take(mc).Where(id => id != 0).ToList();
        var ExtraDeckList = extraAll.Take(ec).Where(id => id != 0).ToList();
        var SideDeckList = sideAll.Take(sc).Where(id => id != 0).ToList();

        for (int i = 0; i < MainDeckList.Count && i < _mainSlots.Length; i++)
        {
            var card = cardDb.GetByLotdId(MainDeckList[i]);
            if (card is not null) { _mainSlots[i].SetCard(card); mainCount++; }
        }

        for (int i = 0; i < ExtraDeckList.Count && i < _extraSlots.Length; i++)
        {
            var card = cardDb.GetByLotdId(ExtraDeckList[i]);
            if (card is not null) { _extraSlots[i].SetCard(card); extraCount++; }
        }

        for (int i = 0; i < SideDeckList.Count && i < _sideSlots.Length; i++)
        {
            var card = cardDb.GetByLotdId(SideDeckList[i]);
            if (card is not null) { _sideSlots[i].SetCard(card); sideCount++; }
        }

        UpdateSectionCounts(mainCount, extraCount, sideCount);

        if (_isOnline) LoadVisibleImages();
    }

    /// <summary>Re-reads whatever slot is currently loaded straight from
    /// AppContext.State.SaveBytes — used after an Undo/Redo swaps the whole
    /// buffer out from under this page's in-memory grid.</summary>
    public void ReloadCurrentDeck()
    {
        if (_currentSlot is null) return;

        var refreshed = AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, _currentSlot.SlotNumber);
        LoadDeck(refreshed, AppContext.CardDb);
    }

    // ── Keyboard navigation (routed here from MainForm's global KeyDown) ─────
    // Arrow keys move a keyboard-focus highlight (reusing CardSlot.IsSelected,
    // otherwise unused by mouse interaction here) across whichever section
    // was last navigated, defaulting to the Main deck; Delete removes the
    // focused card, same as double-clicking it.
    public void HandleGridKeyDown(KeyEventArgs e)
    {
        if (_focusedSection is null)
        {
            _focusedSection = _mainSlots;
            _focusedIndex = 0;
        }

        if (e.KeyCode == Keys.Delete)
        {
            var focused = _focusedSection[_focusedIndex];
            if (focused.Card is not null)
            {
                var card = focused.Card;
                focused.ClearCard();
                RefreshCounts();
                CardRemoveRequested?.Invoke(this, card);
            }
            e.Handled = true;
            return;
        }

        var previous = _focusedSection[_focusedIndex];

        int newIndex = e.KeyCode switch
        {
            Keys.Left => _focusedIndex - 1,
            Keys.Right => _focusedIndex + 1,
            Keys.Up => _focusedIndex - GridColumns,
            Keys.Down => _focusedIndex + GridColumns,
            _ => _focusedIndex
        };

        if (newIndex == _focusedIndex) return;
        if (newIndex < 0 || newIndex >= _focusedSection.Length) return;

        e.Handled = true;
        previous.IsSelected = false;

        _focusedIndex = newIndex;
        var focusedSlot = _focusedSection[_focusedIndex];
        focusedSlot.IsSelected = true;

        if (focusedSlot.Card is not null)
            ShowPreview(focusedSlot.Card);
    }
}