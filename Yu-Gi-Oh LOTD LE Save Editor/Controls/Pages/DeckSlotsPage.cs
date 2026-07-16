using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;
using static Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms.MainForm;
using static Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services.SlotIO;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

/// <summary>
/// Tab page 0 — 32-slot grid on the left, detail panel on the right.
/// The detail panel (portrait, owner info, deck name, card counts, actions)
/// used to be a separate DetailPanel UserControl; it now lives directly here.
/// </summary>
public sealed partial class DeckSlotsPage : UserControl
{
    public event EventHandler<SlotInfo>? SlotSelected;
    public event EventHandler<SlotInfo>? EditCardsRequested;
    public event EventHandler<SlotInfo>? ClearRequested;

    /// <summary>Raised once the user has confirmed wiping every slot (see
    /// btnClearAllSlots_Click) — the host form performs the actual clear.</summary>
    public event EventHandler? ClearAllRequested;

    /// <summary>Raised after a slot's underlying save data is mutated in place
    /// (rename, change duelist, …) so the host form can refresh its dirty flag.</summary>
    public event EventHandler? SlotDataChanged;

    private enum PendingSlotAction { None, CopyTo, SwapWith }

    private DeckCard? _selectedCard;
    private SlotInfo? _selectedSlot;
    private PendingSlotAction _pendingAction = PendingSlotAction.None;
    private SlotInfo? _pendingSourceSlot;

    // ── Multi-select (Ctrl/Shift-click) ──────────────────────────────────────
    // Separate from _selectedSlot/_selectedCard (the "primary" selection that
    // drives the detail panel) — this is purely for bulk actions.
    private readonly HashSet<int> _multiSelectedSlotNumbers = new();
    private int? _lastPrimarySlotNumber;

    public DeckSlotsPage()
    {
        InitializeComponent();

        // Shows the active theme's background image (if any) behind the
        // page — see BackgroundPainter. No-ops entirely when no image is set.
        Paint += BackgroundPainter.Paint;

        // Built here rather than in the Designer file: the WinForms designer only
        // preserves standard component/property/event assignments in
        // InitializeComponent() and silently strips anything else (like a raw
        // array-literal) whenever it regenerates that method.
        deckCards = new DeckCard[]
        {
            deckCard01, deckCard02, deckCard03, deckCard04,
            deckCard05, deckCard06, deckCard07, deckCard08,
            deckCard09, deckCard10, deckCard11, deckCard12,
            deckCard13, deckCard14, deckCard15, deckCard16,
            deckCard17, deckCard18, deckCard19, deckCard20,
            deckCard21, deckCard22, deckCard23, deckCard24,
            deckCard25, deckCard26, deckCard27, deckCard28,
            deckCard29, deckCard30, deckCard31, deckCard32
        };

        // The extra context-menu/drag events added alongside the original
        // three (SlotSelected/EditRequested/ClearRequested, still wired
        // individually per-card in the Designer file) are wired here via a
        // loop instead — they're identical across all 32 cards, so a loop
        // in hand-written code is simpler than 32 more Designer.cs blocks.
        foreach (var card in deckCards)
        {
            card.RenameRequested += Card_RenameRequested;
            card.ChangeDuelistRequested += Card_ChangeDuelistRequested;
            card.CopyToRequested += Card_CopyToRequested;
            card.SwapToRequested += Card_SwapToRequested;
            card.ImportYdkRequested += Card_ImportYdkRequested;
            card.ExportYdkRequested += Card_ExportYdkRequested;
            card.CardDropped += Card_CardDropped;
            card.YdkFileDropped += Card_YdkFileDropped;
        }
    }

    // ── Public API ────────────────────────────────────────────────────────────
    public void LoadSlots(List<SlotInfo> slots)
    {
        // deckCards is a fixed array of 32 cards built once in the Designer;
        // just rebind each one's data instead of recreating controls.
        for (int i = 0; i < deckCards.Length && i < slots.Count; i++)
            deckCards[i].Slot = slots[i];
    }

    public void RefreshCard(SlotInfo slot)
    {
        slot = AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, slot.SlotNumber);

        if (slot.SlotNumber >= 1 && slot.SlotNumber <= deckCards.Length)
            deckCards[slot.SlotNumber - 1].Slot = slot;   // setter refreshes the card itself

        pnlDetail.Refresh();
    }

    public void ClearDetail()
    {
        ClearDetailSelection();
        _selectedCard = null;
    }

    /// <summary>Re-reads the detail panel's currently-shown slot straight
    /// from AppContext.State.SaveBytes — used after an Undo/Redo swaps the
    /// whole save buffer out from under whatever was selected.</summary>
    public void RefreshSelectedDetail()
    {
        if (_selectedSlot is null) return;
        ShowDetailSlot(AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, _selectedSlot.SlotNumber));
    }

    // ── Card handlers ─────────────────────────────────────────────────────────
    // slot can be null here if a card is clicked before any save has been loaded
    // (the 32 cards now exist from startup, unbound, rather than being created on
    // LoadSlots) — ignore clicks on cards with no data yet.
    private void Card_SlotSelected(object? sender, SlotInfo slot)
    {
        if (slot is null) return;

        if (_pendingAction != PendingSlotAction.None)
        {
            CompletePendingAction(slot);
            return;
        }

        // Ctrl-click toggles this slot in the bulk-action multi-selection
        // without touching the primary selection/detail panel. Shift-click
        // extends the multi-selection as a contiguous range from the last
        // primary pick and also moves the primary selection here, same as
        // most file-explorer-style grids.
        if (ModifierKeys.HasFlag(Keys.Control))
        {
            ToggleMultiSelect(slot.SlotNumber);
            return;
        }

        if (ModifierKeys.HasFlag(Keys.Shift) && _lastPrimarySlotNumber is int anchor)
        {
            SelectMultiRange(anchor, slot.SlotNumber);
        }
        else
        {
            _multiSelectedSlotNumbers.Clear();
            RefreshMultiSelectVisuals();
        }

        if (_selectedCard != null) _selectedCard.IsSelected = false;
        _selectedCard = sender as DeckCard;
        if (_selectedCard != null) _selectedCard.IsSelected = true;
        _lastPrimarySlotNumber = slot.SlotNumber;
        ShowDetailSlot(slot);
        SlotSelected?.Invoke(this, slot);
    }

    // ── Multi-select ──────────────────────────────────────────────────────────
    private void ToggleMultiSelect(int slotNumber)
    {
        if (!_multiSelectedSlotNumbers.Remove(slotNumber))
            _multiSelectedSlotNumbers.Add(slotNumber);

        _lastPrimarySlotNumber = slotNumber;
        RefreshMultiSelectVisuals();
    }

    private void SelectMultiRange(int fromSlot, int toSlot)
    {
        int lo = Math.Min(fromSlot, toSlot);
        int hi = Math.Max(fromSlot, toSlot);

        _multiSelectedSlotNumbers.Clear();
        for (int n = lo; n <= hi; n++)
            _multiSelectedSlotNumbers.Add(n);

        RefreshMultiSelectVisuals();
    }

    private void RefreshMultiSelectVisuals()
    {
        foreach (var card in deckCards)
        {
            var slot = card.Slot;
            card.IsMultiSelected = slot is not null && _multiSelectedSlotNumbers.Contains(slot.SlotNumber);
        }

        int count = _multiSelectedSlotNumbers.Count;
        lblMultiSelectCount.Text = count > 0 ? $"{count} selected" : string.Empty;
        btnBulkClear.Enabled = count > 0;
        btnBulkExport.Enabled = count > 0;
    }

    // Clears every slot in the current multi-selection (owner, name, cards).
    private void btnBulkClear_Click(object? sender, EventArgs e)
    {
        if (_multiSelectedSlotNumbers.Count == 0) return;

        var res = AppMessageBox.Show(
            $"Clear {_multiSelectedSlotNumbers.Count} selected slot(s)? Names, duelists, and every card in them will be removed.",
            "Clear Selected Slots", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (res != DialogResult.Yes) return;

        AppContext.Undo.Snapshot();

        foreach (int slotNumber in _multiSelectedSlotNumbers)
        {
            var slot = AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, slotNumber);
            AppContext.SlotIo.UpdateSlotOwner(AppContext.State.SaveBytes, slot, 0);
            AppContext.SlotIo.UpdateName(AppContext.State.SaveBytes, slot, string.Empty);
            AppContext.SlotIo.ClearDeck(AppContext.State.SaveBytes, slotNumber);
            RefreshCard(slot);
        }

        _multiSelectedSlotNumbers.Clear();
        RefreshMultiSelectVisuals();
        SlotDataChanged?.Invoke(this, EventArgs.Empty);
    }

    // Exports every non-empty slot in the current multi-selection to its own
    // .ydk file in a chosen folder, named after the deck.
    private void btnBulkExport_Click(object? sender, EventArgs e)
    {
        var targets = _multiSelectedSlotNumbers
            .Select(n => AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, n))
            .Where(s => !s.IsEmpty)
            .ToList();

        if (targets.Count == 0)
        {
            AppMessageBox.Show("None of the selected slots have a deck to export.", "Export Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dlg = new FolderBrowserDialog { Description = "Choose a folder to export the selected decks into" };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        int written = 0;
        foreach (var slot in targets)
        {
            var deck = AppContext.SlotIo.ReadDeck(AppContext.State.SaveBytes, slot, AppContext.CardDb);
            string safeName = string.Join("_", slot.Name.Split(Path.GetInvalidFileNameChars()));
            if (string.IsNullOrWhiteSpace(safeName)) safeName = $"Slot{slot.SlotNumber}";
            string path = Path.Combine(dlg.SelectedPath, $"{safeName}.ydk");
            YDKWriter.Write(path, deck);
            written++;
        }

        AppMessageBox.Show($"Exported {written} deck(s) to {dlg.SelectedPath}.", "Export Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ── Keyboard navigation (routed here from MainForm's global KeyDown) ─────
    // Arrow keys move the primary selection across the grid; Delete clears
    // the currently selected slot (reusing the same confirm-and-clear flow
    // as the detail panel's Clear button).
    public void HandleGridKeyDown(KeyEventArgs e)
    {
        if (deckCards.Length == 0) return;

        if (e.KeyCode == Keys.Delete)
        {
            if (_selectedSlot is not null)
                ClearRequested?.Invoke(this, _selectedSlot);
            e.Handled = true;
            return;
        }

        int columns = ColumnsPerRow();
        int currentIndex = _selectedSlot is not null ? _selectedSlot.SlotNumber - 1 : 0;

        int newIndex = e.KeyCode switch
        {
            Keys.Left => currentIndex - 1,
            Keys.Right => currentIndex + 1,
            Keys.Up => currentIndex - columns,
            Keys.Down => currentIndex + columns,
            _ => currentIndex
        };

        if (newIndex == currentIndex || newIndex < 0 || newIndex >= deckCards.Length)
            return;

        e.Handled = true;
        var card = deckCards[newIndex];
        pnlGrid.ScrollControlIntoView(card);
        if (card.Slot is null) return;

        // Plain move-and-select, deliberately bypassing Card_SlotSelected's
        // Ctrl/Shift branching — those are mouse-click gestures, and arrow
        // keys shouldn't reinterpret whatever modifier the user happens to
        // be holding as a multi-select action.
        _multiSelectedSlotNumbers.Clear();
        RefreshMultiSelectVisuals();

        if (_selectedCard != null) _selectedCard.IsSelected = false;
        _selectedCard = card;
        card.IsSelected = true;
        _lastPrimarySlotNumber = card.Slot.SlotNumber;
        ShowDetailSlot(card.Slot);
        SlotSelected?.Invoke(this, card.Slot);
    }

    // pnlGrid is a FlowLayoutPanel (wraps based on available width, not a
    // fixed column count), so "how many cards per row" is computed from the
    // actual laid-out positions rather than assumed.
    private int ColumnsPerRow()
    {
        if (deckCards.Length == 0) return 1;
        int firstRowTop = deckCards[0].Top;
        int columns = deckCards.Count(c => c.Top == firstRowTop);
        return Math.Max(1, columns);
    }

    private void Card_EditRequested(object? sender, SlotInfo slot)
    {
        if (slot is not null) EditCardsRequested?.Invoke(this, slot);
    }

    private void Card_ClearRequested(object? sender, SlotInfo slot)
    {
        if (slot is not null) ClearRequested?.Invoke(this, slot);
    }

    // ── Context-menu passthroughs (right-click on a card, not the selected slot) ─
    // These reuse the same action methods as the detail-panel buttons, but act
    // on whichever slot was actually right-clicked rather than _selectedSlot.
    private void Card_RenameRequested(object? sender, SlotInfo slot)
    {
        if (slot is not null) OnRename(slot);
    }

    private void Card_ChangeDuelistRequested(object? sender, SlotInfo slot)
    {
        if (slot is not null) OnChangeDuelist(slot);
    }

    private void Card_CopyToRequested(object? sender, SlotInfo slot)
    {
        if (slot is not null) BeginPickSlot(PendingSlotAction.CopyTo, slot);
    }

    private void Card_SwapToRequested(object? sender, SlotInfo slot)
    {
        if (slot is not null) BeginPickSlot(PendingSlotAction.SwapWith, slot);
    }

    private void Card_ImportYdkRequested(object? sender, SlotInfo slot)
    {
        if (slot is not null) OnImportYdk(slot);
    }

    private void Card_ExportYdkRequested(object? sender, SlotInfo slot)
    {
        if (slot is not null) OnExportYdk(slot);
    }

    // ── Drag-to-reorder (deck slot pushed past its neighbors) ────────────────
    private void Card_CardDropped(object? sender, DeckCard source)
    {
        if (sender is not DeckCard target || source == target) return;
        if (source.Slot is null || target.Slot is null) return;

        int fromIndex = source.Slot.SlotNumber - 1;
        int toIndex = target.Slot.SlotNumber - 1;
        if (fromIndex < 0 || toIndex < 0) return;

        AppContext.Undo.Snapshot();
        PushReorderSlots(fromIndex, toIndex);

        int lo = Math.Min(fromIndex, toIndex);
        int hi = Math.Max(fromIndex, toIndex);
        for (int i = lo; i <= hi; i++)
            RefreshCard(deckCards[i].Slot!);

        // Keep the detail panel in sync if the currently-shown slot was among
        // the ones shifted by the reorder.
        if (_selectedSlot is not null)
        {
            int selIndex = _selectedSlot.SlotNumber - 1;
            if (selIndex >= lo && selIndex <= hi)
                ShowDetailSlot(AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, _selectedSlot.SlotNumber));
        }

        SlotDataChanged?.Invoke(this, EventArgs.Empty);
    }

    // Moves the slot at fromIndex to toIndex, pushing every slot in between
    // over by one — a chain of adjacent pairwise byte-block swaps on the raw
    // save data (mirrors DeckEditorPage's in-memory PushReorder, but operates
    // on save-file bytes via SlotIo.SwapSlotBlocks instead of a CardSlot[]).
    private void PushReorderSlots(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex) return;

        int step = toIndex > fromIndex ? 1 : -1;
        for (int i = fromIndex; i != toIndex; i += step)
        {
            int slotBaseA = SlotLayout.GetSlotBase(i + 1);
            int slotBaseB = SlotLayout.GetSlotBase(i + step + 1);
            AppContext.SlotIo.SwapSlotBlocks(AppContext.State.SaveBytes, slotBaseA, slotBaseB);
        }
    }

    // ── Drag-and-drop .ydk import (Explorer file dropped on a slot) ──────────
    private void Card_YdkFileDropped(object? sender, string filePath)
    {
        if (sender is not DeckCard card || card.Slot is null) return;
        ImportYdkFile(filePath, card.Slot);
    }

    // ── Detail panel (formerly DetailPanel.cs) ───────────────────────────────
    private void ShowDetailSlot(SlotInfo slot)
    {
        _selectedSlot = slot;
        pnlEmpty.Visible = false;

        // Portrait
        picPortrait.Image = null;
        if (slot.OwnerId != 0)
        {
            try { picPortrait.Image = PortraitProvider.GetPortrait(slot.OwnerId); }
            catch { /* fall back to no image */ }
        }

        lblOwnerName.Text = slot.OwnerName;
        lblOwnerId.Text = slot.OwnerId != 0 ? $"ID #{slot.OwnerId:D3}" : string.Empty;
        lblDeckName.Text = slot.IsEmpty ? "Empty slot" : slot.Name;
        lblSlotNum.Text = $"Slot {slot.SlotNumber:D2}";

        UpdateDetailCounts(slot.Main, slot.Extra, slot.Side);
    }

    private void ClearDetailSelection()
    {
        _selectedSlot = null;
        pnlEmpty.Visible = true;
    }

    private void UpdateDetailCounts(int main, int extra, int side)
    {
        SetBar(pnlMainFill, lblMainBar, main, 60);
        SetBar(pnlExtraFill, lblExtraBar, extra, 15);
        SetBar(pnlSideFill, lblSideBar, side, 15);
    }

    private static void SetBar(Panel fill, Label lbl, int count, int max)
    {
        int pct = max == 0 ? 0 : Math.Min(100, (int)Math.Round(count / (double)max * 100));
        fill.Width = (int)((fill.Parent?.Width ?? 200) * pct / 100.0);
        lbl.Text = $"{count}/{max}";
    }

    // ── Detail panel action buttons (wired in Designer) ──────────────────────
    private void btnEditCards_Click(object? sender, EventArgs e)
    {
        CancelPendingAction();
        EditCardsRequested?.Invoke(this, _selectedSlot!);
    }

    private void btnRename_Click(object? sender, EventArgs e)
    {
        CancelPendingAction();
        OnRename(_selectedSlot!);
    }

    private void btnCopyTo_Click(object? sender, EventArgs e) => BeginPickSlot(PendingSlotAction.CopyTo, _selectedSlot!);

    private void btnChangeDuelist_Click(object? sender, EventArgs e)
    {
        CancelPendingAction();
        OnChangeDuelist(_selectedSlot!);
    }

    private void btnSwapWith_Click(object? sender, EventArgs e) => BeginPickSlot(PendingSlotAction.SwapWith, _selectedSlot!);

    private void btnImportYdk_Click(object? sender, EventArgs e)
    {
        CancelPendingAction();
        OnImportYdk(_selectedSlot!);
    }

    private void btnImportYdkMulti_Click(object? sender, EventArgs e)
    {
        CancelPendingAction();
        OnImportYdkMulti(_selectedSlot!);
    }

    private void btnExportYdk_Click(object? sender, EventArgs e)
    {
        CancelPendingAction();
        OnExportYdk(_selectedSlot!);
    }

    private void btnClearSlot_Click(object? sender, EventArgs e)
    {
        CancelPendingAction();
        ClearRequested?.Invoke(this, _selectedSlot!);
    }

    // ── Copy to Slot / Swap With Slot ────────────────────────────────────────
    private void BeginPickSlot(PendingSlotAction action, SlotInfo source)
    {
        _pendingAction = action;
        _pendingSourceSlot = source;

        string verb = action == PendingSlotAction.CopyTo ? "copy" : "swap";
        AppMessageBox.Show(
            $"Click the deck slot you want to {verb} with '{source.Name}'.\nClick slot {source.SlotNumber} again to cancel.",
            action == PendingSlotAction.CopyTo ? "Copy to Slot" : "Swap With Slot",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void CancelPendingAction()
    {
        _pendingAction = PendingSlotAction.None;
        _pendingSourceSlot = null;
    }

    private void CompletePendingAction(SlotInfo target)
    {
        var action = _pendingAction;
        var source = _pendingSourceSlot!;
        CancelPendingAction();

        if (target.SlotNumber == source.SlotNumber)
            return; // clicking the source slot again cancels

        if (action == PendingSlotAction.CopyTo)
        {
            if (!target.IsEmpty)
            {
                var res = AppMessageBox.Show(
                    $"Slot {target.SlotNumber} already has a deck ('{target.Name}'). Overwrite it with '{source.Name}'?",
                    "Copy to Slot", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res != DialogResult.Yes) return;
            }

            AppContext.Undo.Snapshot();
            AppContext.SlotIo.CopySlotBlock(AppContext.State.SaveBytes, source.SlotBaseOffset, target.SlotBaseOffset);
            RefreshCard(target);
        }
        else
        {
            AppContext.Undo.Snapshot();
            AppContext.SlotIo.SwapSlotBlocks(AppContext.State.SaveBytes, source.SlotBaseOffset, target.SlotBaseOffset);
            RefreshCard(source);
            RefreshCard(target);
            ShowDetailSlot(AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, source.SlotNumber));
        }

        SlotDataChanged?.Invoke(this, EventArgs.Empty);
    }

    // ── Clear all slots ──────────────────────────────────────────────────────
    private void btnClearAllSlots_Click(object? sender, EventArgs e)
    {
        CancelPendingAction();

        var res = AppMessageBox.Show(
            "This will permanently clear all 32 deck slots — names, duelists, and every card. This cannot be undone.\n\n" +
            "Are you sure you want to continue?",
            "Clear All Slots", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (res != DialogResult.Yes) return;

        ClearAllRequested?.Invoke(this, EventArgs.Empty);
    }

    // ── Search ────────────────────────────────────────────────────────────────
    // Matches on the deck's own name OR any card inside it (Main/Extra/Side)
    // — so "find which slot has Blue-Eyes White Dragon" works the same as
    // searching by deck name.
    private void txtSearch_TextChanged(object? sender, EventArgs e)
    {
        string q = txtSearch.Text.Trim().ToLowerInvariant();
        foreach (var card in deckCards)
            card.Visible = string.IsNullOrEmpty(q) || SlotMatches(card.Slot, q);
    }

    private static bool SlotMatches(SlotInfo? slot, string query)
    {
        if (slot is null) return false;
        if (slot.Name.ToLowerInvariant().Contains(query)) return true;
        if (slot.IsEmpty || AppContext.State.SaveBytes is null) return false;

        var (mainCount, extraCount, sideCount, mainAll, extraAll, sideAll) =
            AppContext.SlotIo.ReadLOTDDeckCards(AppContext.State.SaveBytes, slot);

        return ContainsMatch(mainAll, mainCount, query)
            || ContainsMatch(extraAll, extraCount, query)
            || ContainsMatch(sideAll, sideCount, query);
    }

    private static bool ContainsMatch(ushort[] lotdIds, int count, string query)
    {
        for (int i = 0; i < count && i < lotdIds.Length; i++)
        {
            if (lotdIds[i] == 0) continue;
            var card = AppContext.CardDb.GetByLotdId(lotdIds[i]);
            if (card is not null && card.Name.ToLowerInvariant().Contains(query))
                return true;
        }
        return false;
    }

    // ── Actions ───────────────────────────────────────────────────────────────
    private void OnRename(SlotInfo slot)
    {
        string? name = Prompt.Show("Rename deck", "Enter new deck name:", slot.Name);
        if (name is null) return;

        AppContext.Undo.Snapshot();
        AppContext.SlotIo.UpdateName(AppContext.State.SaveBytes, slot, name);

        slot = AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, slot.SlotNumber);
        RefreshCard(slot);
        ShowDetailSlot(slot);
        SlotDataChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnChangeDuelist(SlotInfo slot)
    {
        byte? newOwnerId = OwnerPickerForm.Show(slot.OwnerId);
        if (newOwnerId is null) return;

        AppContext.Undo.Snapshot();
        AppContext.SlotIo.UpdateSlotOwner(AppContext.State.SaveBytes, slot, newOwnerId.Value);

        slot = AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, slot.SlotNumber);
        RefreshCard(slot);
        ShowDetailSlot(slot);
        SlotDataChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnImportYdk(SlotInfo target)
    {
        using var dlg = new OpenFileDialog { Title = "Import YDK", Filter = "YDK files (*.ydk)|*.ydk" };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        ImportYdkFile(dlg.FileName, target);
    }

    // Shared by both the OpenFileDialog-driven import (OnImportYdk) and
    // dragging a .ydk file from Explorer onto a card (Card_YdkFileDropped) —
    // parse/validate/confirm-overwrite/write/refresh, identical either way.
    private void ImportYdkFile(string filePath, SlotInfo target)
    {
        Deck deck;
        try { deck = YDKReader.Parse(filePath, AppContext.CardDb); }
        catch (Exception ex)
        {
            AppMessageBox.Show($"Couldn't read that .ydk file:\n{ex.Message}", "Import .ydk", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (deck.main.Count > 60 || deck.extra.Count > 15 || deck.side.Count > 15)
        {
            AppMessageBox.Show(
                $"That deck doesn't fit the LOTD limits (Main ≤60, Extra ≤15, Side ≤15).\n" +
                $"Found Main {deck.main.Count}, Extra {deck.extra.Count}, Side {deck.side.Count}.",
                "Import .ydk", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!target.IsEmpty)
        {
            var res = AppMessageBox.Show(
                $"Slot {target.SlotNumber} already has a deck ('{target.Name}'). Overwrite its cards with the imported deck?",
                "Import .ydk", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != DialogResult.Yes) return;
        }
        string name = Path.GetFileNameWithoutExtension(filePath);
        if (name.Length > 32)
            name = name.Substring(0, 32);
        AppContext.Undo.Snapshot();
        AppContext.SlotIo.WriteSlot(AppContext.State.SaveBytes, target.SlotNumber, deck, name, 138);

        var updated = AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, target.SlotNumber);
        RefreshCard(updated);
        ShowDetailSlot(updated);
        SlotDataChanged?.Invoke(this, EventArgs.Empty);

        AppMessageBox.Show(
            $"Imported {Path.GetFileName(filePath)} into slot {target.SlotNumber}.",
            "Import .ydk", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Imports several .ydk files at once, one per slot, starting at whatever
    // slot is currently selected and moving forward (slot N, N+1, N+2, …).
    // Everything is parsed and validated up front so a bad file in the
    // middle of the batch can't leave a half-applied import — it's just
    // skipped, its target slot left untouched, while the rest still import.
    private void OnImportYdkMulti(SlotInfo startSlot)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Import multiple .ydk files",
            Filter = "YDK files (*.ydk)|*.ydk",
            Multiselect = true
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        var files = dlg.FileNames.OrderBy(f => Path.GetFileName(f), StringComparer.OrdinalIgnoreCase).ToArray();
        if (files.Length == 0) return;

        int firstSlot = startSlot.SlotNumber;
        int lastSlot = firstSlot + files.Length - 1;

        if (lastSlot > SlotLayout.SlotCount)
        {
            AppMessageBox.Show(
                $"Selected {files.Length} files, which would need slots {firstSlot}–{lastSlot}, " +
                $"but there are only {SlotLayout.SlotCount} slots.\nChoose fewer files or start from an earlier slot.",
                "Import multiple .ydk", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var plans = new List<(string File, int SlotNumber, Deck Deck)>();
        var failures = new List<string>();

        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i];
            int slotNumber = firstSlot + i;

            Deck deck;
            try { deck = YDKReader.Parse(file, AppContext.CardDb); }
            catch (Exception ex)
            {
                failures.Add($"{Path.GetFileName(file)}: {ex.Message}");
                continue;
            }

            if (deck.main.Count > 60 || deck.extra.Count > 15 || deck.side.Count > 15)
            {
                failures.Add(
                    $"{Path.GetFileName(file)}: doesn't fit the LOTD limits " +
                    $"(Main {deck.main.Count}/60, Extra {deck.extra.Count}/15, Side {deck.side.Count}/15).");
                continue;
            }

            plans.Add((file, slotNumber, deck));
        }

        if (plans.Count == 0)
        {
            AppMessageBox.Show(
                "None of the selected files could be imported:\n\n" + string.Join("\n", failures),
                "Import multiple .ydk", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var occupiedTargets = plans
            .Where(p => !AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, p.SlotNumber).IsEmpty)
            .Select(p => p.SlotNumber)
            .ToList();

        if (occupiedTargets.Count > 0)
        {
            var res = AppMessageBox.Show(
                $"This will overwrite {occupiedTargets.Count} existing deck(s) in slot(s) {string.Join(", ", occupiedTargets)}. Continue?",
                "Import multiple .ydk", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != DialogResult.Yes) return;
        }

        AppContext.Undo.Snapshot();

        SlotInfo? lastWritten = null;
        foreach (var (file, slotNumber, deck) in plans)
        {
            string name = Path.GetFileNameWithoutExtension(file);
            if (name.Length > 32) name = name.Substring(0, 32);

            AppContext.SlotIo.WriteSlot(AppContext.State.SaveBytes, slotNumber, deck, name, 138);

            var updated = AppContext.SlotIo.ReadSlot(AppContext.State.SaveBytes, slotNumber);
            RefreshCard(updated);
            lastWritten = updated;
        }

        if (lastWritten is not null) ShowDetailSlot(lastWritten);
        SlotDataChanged?.Invoke(this, EventArgs.Empty);

        int usedMin = plans.Min(p => p.SlotNumber);
        int usedMax = plans.Max(p => p.SlotNumber);
        string summary = $"Imported {plans.Count} of {files.Length} file(s) into slot(s) {usedMin}–{usedMax}.";
        if (failures.Count > 0)
            summary += "\n\nSkipped:\n" + string.Join("\n", failures);

        AppMessageBox.Show(summary, "Import multiple .ydk", MessageBoxButtons.OK,
            failures.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
    }

    private static void OnExportYdk(SlotInfo slot)
    {
        if (slot.IsEmpty)
        {
            AppMessageBox.Show("This slot is empty — nothing to export.", "Export .ydk", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dlg = new SaveFileDialog { Title = "Export .ydk", Filter = "YDK files (*.ydk)|*.ydk", DefaultExt = "ydk", FileName = slot.Name + ".ydk" };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        var deck = AppContext.SlotIo.ReadDeck(AppContext.State.SaveBytes, slot, AppContext.CardDb);
        YDKWriter.Write(dlg.FileName, deck);

        AppMessageBox.Show(
            $"Exported '{slot.Name}' to {Path.GetFileName(dlg.FileName)}.",
            "Export .ydk", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
