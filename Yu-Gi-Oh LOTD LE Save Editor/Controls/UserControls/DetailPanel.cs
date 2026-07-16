using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;
using static Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services.SlotIO;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

/// <summary>
/// Right-hand detail panel.
/// Displays portrait, owner info, deck name, card counts + bars, and action buttons.
/// All control creation lives in DetailPanel.Designer.cs.
/// </summary>
public sealed partial class DetailPanel : UserControl
{
    // ── Events ────────────────────────────────────────────────────────────────
    public event EventHandler<SlotInfo>? EditCardsRequested;
    public event EventHandler<SlotInfo>? RenameRequested;
    public event EventHandler<SlotInfo>? CopyToRequested;
    public event EventHandler<SlotInfo>? ChangeDuelistRequested;
    public event EventHandler<SlotInfo>? SwapWithRequested;
    public event EventHandler<SlotInfo>? ImportYdkRequested;
    public event EventHandler<SlotInfo>? ExportYdkRequested;
    public event EventHandler<SlotInfo>? ClearRequested;

    private SlotInfo? _slot;

    public DetailPanel()
    {
        InitializeComponent();
    }

    // ── Public API ────────────────────────────────────────────────────────────
    public void ShowSlot(SlotInfo slot)
    {
        _slot = slot;
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

        UpdateCounts(slot.Main, slot.Extra, slot.Side);
    }

    public void ClearSelection()
    {
        _slot = null;
        pnlEmpty.Visible = true;
    }

    // ── Count update ──────────────────────────────────────────────────────────
    private void UpdateCounts(int main, int extra, int side)
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

    // ── Button click handlers (wired in Designer) ─────────────────────────────
    private void btnEditCards_Click(object? sender, EventArgs e) => EditCardsRequested?.Invoke(this, _slot!);
    private void btnRename_Click(object? sender, EventArgs e) => RenameRequested?.Invoke(this, _slot!);
    private void btnCopyTo_Click(object? sender, EventArgs e) => CopyToRequested?.Invoke(this, _slot!);
    private void btnChangeDuelist_Click(object? sender, EventArgs e) => ChangeDuelistRequested?.Invoke(this, _slot!);
    private void btnSwapWith_Click(object? sender, EventArgs e) => SwapWithRequested?.Invoke(this, _slot!);
    private void btnImportYdk_Click(object? sender, EventArgs e) => ImportYdkRequested?.Invoke(this, _slot!);
    private void btnExportYdk_Click(object? sender, EventArgs e) => ExportYdkRequested?.Invoke(this, _slot!);
    private void btnClearSlot_Click(object? sender, EventArgs e) => ClearRequested?.Invoke(this, _slot!);

    private void lblOwnerName_Click(object sender, EventArgs e)
    {

    }
}