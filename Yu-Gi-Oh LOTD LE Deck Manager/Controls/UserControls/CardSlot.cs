
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

/// <summary>
/// A single card slot in the deck editor (60×88 px).
/// Online  → shows card image from CardImage.ImageUrl.
/// Offline → shows name / type / ATK / DEF as text.
/// Empty   → shows a faint placeholder.
/// All control creation lives in CardSlot.Designer.cs.
/// </summary>
public sealed partial class CardSlot : UserControl
{
    /// <summary>Which deck section this slot belongs to — drives its idle
    /// border color (cyan Main / magenta Extra / red Side), matching the
    /// Main/Extra deck slot border colors seen in the Duel Links deck editor
    /// screenshot. Defaults to Main so the 60 main-deck slots need no extra
    /// Designer.cs wiring.</summary>
    public enum SlotRole { Main, Extra, Side }

    // ── State ─────────────────────────────────────────────────────────────────
    private Card? _card;
    private bool _isOnline;
    private bool _isHovered;
    private bool _isSelected;
    private bool _isDropTarget;
    private Point _dragStartPoint = Point.Empty;
    private SlotRole _role = SlotRole.Main;

    // ── Events ────────────────────────────────────────────────────────────────
    public event EventHandler<Card>? CardClicked;
    public event EventHandler<Card>? CardRightClicked;
    public event EventHandler<Card>? CardDoubleClicked;

    /// <summary>Raised on the drop-target slot when another slot's card is
    /// dragged onto it. Sender is the target CardSlot, the event arg is the
    /// source CardSlot the card was dragged from — the host page decides
    /// whether to move or swap.</summary>
    public event EventHandler<CardSlot>? CardDropped;

    public CardSlot()
    {
        InitializeComponent();

        // DragLeave isn't wired per-control in the Designer file the way
        // DragEnter/DragDrop are (adding it there would mean touching every
        // child control's block) — a small recursive hookup here in
        // hand-written code covers the root and every child in one go.
        WireDragLeave(this);
    }

    private void WireDragLeave(Control c)
    {
        c.DragLeave += CardSlot_DragLeave;
        foreach (Control child in c.Controls)
            WireDragLeave(child);
    }

    // ── Public API ────────────────────────────────────────────────────────────
    public Card? Card => _card;

    public bool IsOnline
    {
        get => _isOnline;
        set { _isOnline = value; RefreshView(); }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set { _isSelected = value; Invalidate(); }
    }

    public SlotRole Role
    {
        get => _role;
        set { _role = value; Invalidate(); }
    }

    public void SetCard(Card card)
    {
        _card = card;
        RefreshView();
    }

    public void ClearCard()
    {
        _card = null;
        picCard.Image = null;
        RefreshView();
    }

    // ── View refresh ──────────────────────────────────────────────────────────
    private void RefreshView()
    {
        bool empty = _card is null;

        // Empty state
        pnlEmpty.Visible = empty;

        // Filled states
        pnlOffline.Visible = !empty && !_isOnline;
        picCard.Visible = !empty && _isOnline;

        if (empty) { Invalidate(); return; }

        // Offline text
        lblName.Text = _card!.Name;
        lblType.Text = _card.HumanReadableCardType;

        bool hasStats = _card.Atk.HasValue;
        lblStats.Visible = hasStats;
        lblStats.Text = hasStats
            ? $"ATK {_card.Atk?.ToString() ?? "?"} / DEF {_card.Def?.ToString() ?? "—"}"
            : string.Empty;

        Invalidate();
    }

    // ── Image loading (called by the host when online) ────────────────────────
    public void SetImage(Image image)
    {
        picCard.Image = image;
        picCard.Visible = _card is not null && _isOnline;
        Invalidate();
    }

    // ── Border painting ───────────────────────────────────────────────────────
    // Rounded-square slot with a role-colored idle border (cyan Main / magenta
    // Extra / red Side), matching the Duel Links deck editor's Main/Extra/Side
    // deck slot styling — hover/selected/drop-target accents stay shared
    // across every role, same as the reference screenshots.
    private Color IdleBorderColor => _role switch
    {
        SlotRole.Extra => AppColors.ExtraBorder,
        SlotRole.Side => AppColors.SideBorder,
        _ => AppColors.MainBorder,
    };

    private System.Drawing.Drawing2D.GraphicsPath BuildPath()
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        var r = new Rectangle(0, 0, Width - 1, Height - 1);
        int d = Math.Max(0, Math.Min(6, Math.Min(r.Width, r.Height) / 2));

        if (d == 0) { path.AddRectangle(r); return path; }

        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        Color border = _isDropTarget ? AppColors.StatusOk
                     : _isSelected ? AppColors.BorderSelected
                     : _isHovered ? AppColors.BorderHover
                     : IdleBorderColor;

        using var path = BuildPath();

        // Clip the whole control to the rounded shape so BackColor/the card
        // image/text don't show square corners poking past the border line.
        Region = new Region(path);

        using var pen = new Pen(border, _isDropTarget || _isSelected ? 2f : 1.25f);
        g.DrawPath(pen, path);
    }

    // ── Mouse handlers ────────────────────────────────────────────────────────
    private void CardSlot_MouseEnter(object? sender, EventArgs e)
    {
        _isHovered = true;
        Invalidate();
    }

    private void CardSlot_MouseLeave(object? sender, EventArgs e)
    {
        if (!ClientRectangle.Contains(PointToClient(Cursor.Position)))
        {
            _isHovered = false;
            Invalidate();
        }
    }

    private void CardSlot_MouseClick(object? sender, MouseEventArgs e)
    {
        if (_card is null) return;

        if (e.Button == MouseButtons.Left)
            CardClicked?.Invoke(this, _card);
        else if (e.Button == MouseButtons.Right)
            CardRightClicked?.Invoke(this, _card);
    }

    private void CardSlot_MouseDoubleClick(object? sender, MouseEventArgs e)
    {
        if (_card is null) return;

        if (e.Button == MouseButtons.Left)
            CardDoubleClicked?.Invoke(this, _card);
    }

    // ── Drag & drop ───────────────────────────────────────────────────────────
    // Only filled slots can be dragged FROM (nothing to move out of an empty
    // one), but every state — including empty — can be dropped ONTO.
    private void CardSlot_MouseDown(object? sender, MouseEventArgs e)
    {
        _dragStartPoint = _card is not null && e.Button == MouseButtons.Left
            ? e.Location
            : Point.Empty;
    }

    private void CardSlot_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_card is null || e.Button != MouseButtons.Left || _dragStartPoint == Point.Empty)
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

    private void CardSlot_DragEnter(object? sender, DragEventArgs e)
    {
        e.Effect = e.Data is not null
            && e.Data.GetDataPresent(typeof(CardSlot))
            && e.Data.GetData(typeof(CardSlot)) is CardSlot source
            && source != this
                ? DragDropEffects.Move
                : DragDropEffects.None;

        _isDropTarget = e.Effect != DragDropEffects.None;
        Invalidate();
    }

    // Mirrors CardSlot_MouseLeave's defensive check — a drag hovering between
    // two overlapping child controls fires Leave-then-Enter in the same
    // instant, so only clear the highlight once the cursor has truly left
    // this slot's bounds.
    private void CardSlot_DragLeave(object? sender, EventArgs e)
    {
        if (!ClientRectangle.Contains(PointToClient(Cursor.Position)))
        {
            _isDropTarget = false;
            Invalidate();
        }
    }

    private void CardSlot_DragDrop(object? sender, DragEventArgs e)
    {
        _isDropTarget = false;
        Invalidate();

        if (e.Data?.GetData(typeof(CardSlot)) is CardSlot source && source != this)
            CardDropped?.Invoke(this, source);
    }
}