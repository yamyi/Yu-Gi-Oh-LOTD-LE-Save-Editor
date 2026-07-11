using System.ComponentModel;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;
using static Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services.SlotIO;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

/// <summary>
/// Static UserControl card representing one of the 32 deck slots.
/// All visual layout lives in DeckSlotCard.Designer.cs.
/// This file owns state, public API, events, border painting, and data binding.
/// </summary>
public partial class DeckCard : UserControl
{
    // ── State ─────────────────────────────────────────────────────────────────
    private SlotInfo _slot;
    private bool _isSelected;
    private bool _isHovered;
    private bool _isMultiSelected;
    private bool _isDropTarget;
    private byte? _lastPortraitOwnerId;
    private Point _dragStartPoint = Point.Empty;
    private readonly ToolTip _legalityTip = new();

    // ── Events ────────────────────────────────────────────────────────────────
    public event EventHandler<SlotInfo>? SlotSelected;
    public event EventHandler<SlotInfo>? EditRequested;
    public event EventHandler<SlotInfo>? ClearRequested;
    public event EventHandler<SlotInfo>? RenameRequested;
    public event EventHandler<SlotInfo>? ChangeDuelistRequested;
    public event EventHandler<SlotInfo>? CopyToRequested;
    public event EventHandler<SlotInfo>? SwapToRequested;
    public event EventHandler<SlotInfo>? ImportYdkRequested;
    public event EventHandler<SlotInfo>? ExportYdkRequested;

    /// <summary>Raised on the drop-target card when another card is dragged
    /// onto it (reordering). Sender is the target DeckCard, the event arg is
    /// the source DeckCard it was dragged from.</summary>
    public event EventHandler<DeckCard>? CardDropped;

    /// <summary>Raised when a .ydk file is dragged in from Explorer and
    /// dropped on this card. The event arg is the full file path.</summary>
    public event EventHandler<string>? YdkFileDropped;

    // ── Constructor ───────────────────────────────────────────────────────────
    public DeckCard()
    {

        InitializeComponent();
        Slot = new SlotInfo (1,0, "Empty",  1,  0,  0,  0,true );
        AllowDrop = true;

        // Bubble mouse/drag events from child controls up to the card so
        // hover/click/drag fire regardless of which child the cursor is
        // over — every child also gets AllowDrop so a .ydk file or another
        // card can be dropped anywhere on the tile, not just its root area.
        foreach (Control c in Controls)
            BubbleMouseEvents(c);

        MouseEnter += DeckSlotCard_MouseEnter;
        MouseLeave += DeckSlotCard_MouseLeave;
        MouseClick += DeckSlotCard_MouseClick;
        MouseDown += DeckSlotCard_MouseDown;
        MouseMove += DeckSlotCard_MouseMove;
        DragEnter += DeckSlotCard_DragEnter;
        DragDrop += DeckSlotCard_DragDrop;
        DragLeave += DeckSlotCard_DragLeave;
    }

    // ── Public API ────────────────────────────────────────────────────────────
    public SlotInfo Slot
    {
        get => _slot;
        set
        {
            _slot = value;
            if(_slot!=null)
                RefreshState();
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            RefreshBorder();
        }
    }

    /// <summary>Bulk-action multi-selection (Ctrl/Shift-click) — independent
    /// of IsSelected, which drives the detail panel.</summary>
    public bool IsMultiSelected
    {
        get => _isMultiSelected;
        set
        {
            _isMultiSelected = value;
            RefreshBorder();
        }
    }

    // ── Data binding ──────────────────────────────────────────────────────────
    private void RefreshState()
    {
        lblSlotNum.Text = $"SLOT {_slot.SlotNumber:D2}";

        bool filled = !_slot.IsEmpty;

        lblEmpty.Visible = !filled;
        picPortrait.Visible = filled;
        lblDeckName.Visible = filled;
        lblOwnerName.Visible = filled;
        pnlDivider.Visible = filled;
        pnlPipMain.Visible = filled;
        pnlPipExtra.Visible = filled;
        pnlPipSide.Visible = filled;

        if (!filled)
        {
            _legalityTip.SetToolTip(this, string.Empty);
            RefreshBorder();
            return;
        }

        // Text
        lblDeckName.Text = _slot.Name;
        lblOwnerName.Text = _slot.OwnerName;

        // Pip counts
        lblMainCount.Text = _slot.Main.ToString();
        lblExtraCount.Text = _slot.Extra.ToString();
        lblSideCount.Text = _slot.Side.ToString();

        // Portrait (cache by path — only reload when path changes)
        if (_lastPortraitOwnerId != _slot.OwnerId)
        {
            picPortrait.Image?.Dispose();
            picPortrait.Image = PortraitProvider.GetPortrait(_slot.OwnerId);
            _lastPortraitOwnerId = _slot.OwnerId;
        }

        _legalityTip.SetToolTip(this, LegalityDescription(_slot));
        RefreshBorder();
    }

    // ── Deck legality badge ──────────────────────────────────────────────────
    // Structural legality only (this app has no LOTD-specific banlist data to
    // check individual cards against): Illegal if a section somehow exceeds
    // the save format's own hard caps (60/15/15 — WriteSlot already prevents
    // this, so it'd only happen from a hand-edited save); Warning if the deck
    // is under the conventional 40-card main-deck minimum; Legal otherwise.
    // Empty slots show no badge at all.
    private enum DeckLegality { Empty, Warning, Illegal, Legal }

    private static DeckLegality GetLegality(SlotInfo slot)
    {
        if (slot.IsEmpty) return DeckLegality.Empty;
        if (slot.Main > 60 || slot.Extra > 15 || slot.Side > 15) return DeckLegality.Illegal;
        if (slot.Main < 40) return DeckLegality.Warning;
        return DeckLegality.Legal;
    }

    private static string LegalityDescription(SlotInfo slot) => GetLegality(slot) switch
    {
        DeckLegality.Illegal => $"Over the save format's limits (Main {slot.Main}/60, Extra {slot.Extra}/15, Side {slot.Side}/15).",
        DeckLegality.Warning => $"Main deck has {slot.Main} cards — under the usual 40-card minimum.",
        DeckLegality.Legal => $"Legal deck size (Main {slot.Main}/60, Extra {slot.Extra}/15, Side {slot.Side}/15).",
        _ => string.Empty,
    };

    private static Color LegalityColor(DeckLegality legality) => legality switch
    {
        DeckLegality.Illegal => AppColors.DangerFg,
        DeckLegality.Warning => AppColors.TextGold,
        DeckLegality.Legal => AppColors.StatusOk,
        _ => Color.Transparent,
    };

    // ── Border / background painting ─────────────────────────────────────────
    // Only the rounded border + background fill is owner-drawn; everything
    // else is a child control positioned on top.
    private void RefreshBorder() => Invalidate();

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        Color bg = _isSelected ? AppColors.CardSelected
                 : _isHovered ? AppColors.CardHover
                 : AppColors.CardBg;

        using var bgBrush = new SolidBrush(bg);
        g.FillRoundedRectangle(bgBrush, ClientRectangle.Deflate(1, 1), 6);

        // Drop-target (drag hovering over this card) takes priority over
        // every other border state so it's always obvious where a reorder
        // or .ydk drop would land.
        Color borderColor = _isDropTarget ? AppColors.StatusOk
                          : _isSelected ? AppColors.BorderSelected
                          : _isHovered ? AppColors.BorderHover
                          : AppColors.Border;

        using var borderPen = new Pen(borderColor, _isDropTarget ? 2f : 1f);
        g.DrawRoundedRectangle(borderPen, ClientRectangle.Deflate(1, 1), 6);

        // Draw portrait border on top of the PictureBox (rounded clip not
        // possible on a plain PictureBox, so we paint it in OnPaint instead).
        // _slot can still be null here: the 32 cards now exist as soon as
        // DeckSlotsPage is constructed (including in the VS designer surface),
        // before LoadSlots() ever assigns real data to them.
        if (_slot is not null && !_slot.IsEmpty)
        {
            var portraitRect = picPortrait.Bounds;
            using var pBorder = new Pen(AppColors.Border, 1f);
            g.DrawRoundedRectangle(pBorder, portraitRect, 4);

            // Deck legality dot — top-right corner, empty slots get none.
            var legality = GetLegality(_slot);
            if (legality != DeckLegality.Empty)
            {
                var dotRect = new Rectangle(Width - 14, 6, 8, 8);
                using var dotBrush = new SolidBrush(LegalityColor(legality));
                g.FillEllipse(dotBrush, dotRect);
            }
        }

        // Multi-select badge — small filled checkmark circle, top-left.
        if (_isMultiSelected)
        {
            var badgeRect = new Rectangle(6, 6, 12, 12);
            using var badgeBrush = new SolidBrush(AppColors.GoldBtnBg);
            g.FillEllipse(badgeBrush, badgeRect);
            using var checkPen = new Pen(AppColors.GoldBtnFg, 1.5f);
            g.DrawLine(checkPen, badgeRect.X + 2, badgeRect.Y + 6, badgeRect.X + 5, badgeRect.Y + 9);
            g.DrawLine(checkPen, badgeRect.X + 5, badgeRect.Y + 9, badgeRect.X + 10, badgeRect.Y + 3);
        }
    }

    // ── Mouse handlers ────────────────────────────────────────────────────────
    private void DeckSlotCard_MouseEnter(object? sender, EventArgs e)
    {
        _isHovered = true;
        Invalidate();
    }

    private void DeckSlotCard_MouseLeave(object? sender, EventArgs e)
    {
        // Only truly leaving if the cursor went outside the card bounds.
        if (!ClientRectangle.Contains(PointToClient(Cursor.Position)))
        {
            _isHovered = false;
            Invalidate();
        }
    }

    private void DeckSlotCard_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            SlotSelected?.Invoke(this, _slot);
        else if (e.Button == MouseButtons.Right)
            ShowContextMenu();
    }

    // ── Context menu ─────────────────────────────────────────────────────────
    private void ShowContextMenu()
    {
        var menu = new ContextMenuStrip();
        menu.BackColor = AppColors.SidebarBg;
        menu.ForeColor = AppColors.TextSecondary;

        var editItem = new ToolStripMenuItem("Edit cards");
        var renameItem = new ToolStripMenuItem("Rename...");
        var changeDuelistItem = new ToolStripMenuItem("Change duelist...");
        var copyToItem = new ToolStripMenuItem("Copy to...");
        var swapToItem = new ToolStripMenuItem("Swap with...");
        var importItem = new ToolStripMenuItem("Import .ydk...");
        var exportItem = new ToolStripMenuItem("Export .ydk...") { Enabled = !_slot.IsEmpty };
        var clearItem = new ToolStripMenuItem("Clear slot") { ForeColor = AppColors.DangerFg };

        editItem.Click += (_, _) => EditRequested?.Invoke(this, _slot);
        renameItem.Click += (_, _) => RenameRequested?.Invoke(this, _slot);
        changeDuelistItem.Click += (_, _) => ChangeDuelistRequested?.Invoke(this, _slot);
        copyToItem.Click += (_, _) => CopyToRequested?.Invoke(this, _slot);
        swapToItem.Click += (_, _) => SwapToRequested?.Invoke(this, _slot);
        importItem.Click += (_, _) => ImportYdkRequested?.Invoke(this, _slot);
        exportItem.Click += (_, _) => ExportYdkRequested?.Invoke(this, _slot);
        clearItem.Click += (_, _) => ClearRequested?.Invoke(this, _slot);

        menu.Items.Add(editItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(renameItem);
        menu.Items.Add(changeDuelistItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(copyToItem);
        menu.Items.Add(swapToItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(importItem);
        menu.Items.Add(exportItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(clearItem);
        menu.Show(this, new Point(Width - 10, 10));
    }

    // ── Drag out (reorder) ────────────────────────────────────────────────────
    // Only filled slots can be dragged FROM (nothing meaningful to reorder
    // out of an empty one), but every state — including empty — can be
    // dropped ONTO, same as CardSlot.
    private void DeckSlotCard_MouseDown(object? sender, MouseEventArgs e)
    {
        _dragStartPoint = !_slot.IsEmpty && e.Button == MouseButtons.Left
            ? e.Location
            : Point.Empty;
    }

    private void DeckSlotCard_MouseMove(object? sender, MouseEventArgs e)
    {
        if ( e.Button != MouseButtons.Left || _dragStartPoint == Point.Empty || _slot.IsEmpty )
            return;

        var size = SystemInformation.DragSize;
        var dragZone = new Rectangle(
            _dragStartPoint.X - size.Width / 2,
            _dragStartPoint.Y - size.Height / 2,
            size.Width, size.Height);

        if (dragZone.Contains(e.Location)) return;

        _dragStartPoint = Point.Empty;
        DragGhost.Begin(this, DragDropEffects.Move, this);
    }

    // ── Drag in (reorder target, or a .ydk file from Explorer) ───────────────
    private void DeckSlotCard_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data is null) { e.Effect = DragDropEffects.None; return; }

        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            e.Effect = files is { Length: > 0 } && Array.Exists(files, IsYdkFile)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
        }
        else
        {
            e.Effect = e.Data.GetDataPresent(typeof(DeckCard))
                && e.Data.GetData(typeof(DeckCard)) is DeckCard source
                && source != this
                    ? DragDropEffects.Move
                    : DragDropEffects.None;
        }

        _isDropTarget = e.Effect != DragDropEffects.None;
        Invalidate();
    }

    // Mirrors CardSlot's MouseLeave defensive check — a drag hovering between
    // two overlapping child controls (a label sitting on a panel, say) fires
    // Leave-then-Enter in the same instant, so only actually clear the
    // highlight once the cursor is truly outside this card's bounds.
    private void DeckSlotCard_DragLeave(object? sender, EventArgs e)
    {
        if (!ClientRectangle.Contains(PointToClient(Cursor.Position)))
        {
            _isDropTarget = false;
            Invalidate();
        }
    }

    private void DeckSlotCard_DragDrop(object? sender, DragEventArgs e)
    {
        _isDropTarget = false;
        Invalidate();

        if (e.Data is null) return;

        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            var ydk = files?.FirstOrDefault(IsYdkFile);
            if (ydk is not null) YdkFileDropped?.Invoke(this, ydk);
            return;
        }

        if (e.Data.GetData(typeof(DeckCard)) is DeckCard source && source != this)
            CardDropped?.Invoke(this, source);
    }

    private static bool IsYdkFile(string path) =>
        string.Equals(Path.GetExtension(path), ".ydk", StringComparison.OrdinalIgnoreCase);

    // ── Child-event bubbling ──────────────────────────────────────────────────
    // Ensures hover/click/drag on any child label or pip fires on the card
    // itself — the "this" inside each handler is always this DeckCard
    // instance (they're its own methods), regardless of which child raised
    // the event, so drag payloads and hit targets stay correct either way.
    private void BubbleMouseEvents(Control c)
    {
        c.MouseEnter += DeckSlotCard_MouseEnter;
        c.MouseLeave += DeckSlotCard_MouseLeave;
        c.MouseClick += DeckSlotCard_MouseClick;
        c.MouseDown += DeckSlotCard_MouseDown;
        c.MouseMove += DeckSlotCard_MouseMove;

        c.AllowDrop = true;
        c.DragEnter += DeckSlotCard_DragEnter;
        c.DragDrop += DeckSlotCard_DragDrop;
        c.DragLeave += DeckSlotCard_DragLeave;

        foreach (Control child in c.Controls)
            BubbleMouseEvents(child);
    }

    // ── Cleanup ───────────────────────────────────────────────────────────────
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            picPortrait?.Image?.Dispose();
            _legalityTip.Dispose();
        }

        base.Dispose(disposing);
    }


}