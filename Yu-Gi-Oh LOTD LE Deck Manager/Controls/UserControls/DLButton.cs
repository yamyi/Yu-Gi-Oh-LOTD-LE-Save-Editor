using System.Drawing.Drawing2D;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

/// <summary>
/// Which color role a DLButton plays — mirrors the button colors actually
/// seen across Duel Links (teal for primary CTAs like "Card Inventory" /
/// "Information", blue for everything else like "MENU" / "Auto-Build Deck" /
/// "Copy Deck" / "CLOSE", red for destructive actions).
/// </summary>
public enum DLButtonVariant
{
    Primary,
    Secondary,
    Danger,
}

/// <summary>
/// Duel Links-styled button: a rounded pill (or, with <see cref="Slanted"/>,
/// a parallelogram with slanted ends like the big vertical menu-list buttons
/// in Duel Studio/Help screens) filled with a top-to-bottom gradient plus a
/// diagonal sheen highlight across the upper-left, a bright cyan/colored
/// border, and centered white text. Fully owner-drawn (no child controls —
/// consistent with every other control in this app), so it drops in
/// anywhere a stock Button would go.
/// </summary>
public sealed class DLButton : Control
{
    private bool _isHovered;
    private bool _isPressed;
    private DLButtonVariant _variant = DLButtonVariant.Secondary;
    private bool _slanted;

    public DLButtonVariant Variant
    {
        get => _variant;
        set { _variant = value; Invalidate(); }
    }

    /// <summary>True for the diagonal-cut parallelogram shape used by the big
    /// full-width menu-list buttons (Duel Studio, Help/Etc.); false for the
    /// rounded-pill shape used everywhere else (MENU, Auto-Build Deck, etc).</summary>
    public bool Slanted
    {
        get => _slanted;
        set { _slanted = value; Invalidate(); }
    }

    public DLButton()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                  ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        Cursor = Cursors.Hand;
        Size = new Size(140, 34);
    }

    protected override void OnMouseEnter(EventArgs e) { _isHovered = true; Invalidate(); base.OnMouseEnter(e); }
    protected override void OnMouseLeave(EventArgs e) { _isHovered = false; _isPressed = false; Invalidate(); base.OnMouseLeave(e); }
    protected override void OnMouseDown(MouseEventArgs e) { _isPressed = true; Invalidate(); base.OnMouseDown(e); }
    protected override void OnMouseUp(MouseEventArgs e) { _isPressed = false; Invalidate(); base.OnMouseUp(e); }
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }

    private (Color top, Color bottom, Color border) GetPalette()
    {
        (Color baseColor, Color hoverColor, Color borderColor) = _variant switch
        {
            DLButtonVariant.Primary => (AppColors.GoldBtnBg, AppColors.GoldBtnHover, AppColors.BorderHover),
            DLButtonVariant.Danger => (AppColors.DangerHoverBg, AppColors.AccentRedHover, AppColors.DangerBorder),
            _ => (AppColors.ButtonNeutralBg, AppColors.ButtonNeutralHover, AppColors.Border),
        };

        if (!Enabled)
            return (ControlPaint.Dark(baseColor, 0.2f), ControlPaint.Dark(baseColor, 0.4f), AppColors.TextDim);

        Color top = _isPressed ? ControlPaint.Dark(baseColor, 0.1f) : (_isHovered ? hoverColor : ControlPaint.Light(baseColor, 0.15f));
        Color bottom = _isPressed ? ControlPaint.Dark(baseColor, 0.25f) : baseColor;
        return (top, bottom, _isHovered ? AppColors.BorderHover : borderColor);
    }

    private GraphicsPath BuildPath()
    {
        var path = new GraphicsPath();
        var r = new Rectangle(0, 0, Width - 1, Height - 1);

        if (_slanted)
        {
            // Parallelogram: top-left and bottom-right corners cut diagonally.
            int cut = Math.Min(Height / 2, 14);
            path.AddLine(cut, 0, r.Right, 0);
            path.AddLine(r.Right, 0, r.Right - cut, r.Bottom);
            path.AddLine(r.Right - cut, r.Bottom, 0, r.Bottom);
            path.AddLine(0, r.Bottom, cut, 0);
            path.CloseFigure();
        }
        else
        {
            int radius = Math.Min(Height, 20);
            int d = radius;
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
        }
        return path;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var (top, bottom, border) = GetPalette();
        using var path = BuildPath();

        using (var fill = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), top, bottom, LinearGradientMode.Vertical))
            g.FillPath(fill, path);

        // Diagonal sheen across the upper-left third — the subtle highlight
        // stripe every Duel Links button has.
        using (var sheenPath = new GraphicsPath())
        {
            sheenPath.AddPolygon(new[]
            {
                new Point(0, 0),
                new Point((int)(Width * 0.65), 0),
                new Point((int)(Width * 0.35), Height / 2),
                new Point(0, Height / 2),
            });
            using var region = new Region(path);
            region.Intersect(sheenPath);
            using var sheenBrush = new SolidBrush(Color.FromArgb(38, 255, 255, 255));
            g.FillRegion(sheenBrush, region);
        }

        using (var pen = new Pen(border, 1.5f))
            g.DrawPath(pen, path);

        var textColor = Enabled ? ForeColor : AppColors.TextDim;
        TextRenderer.DrawText(g, Text, Font, ClientRectangle, textColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
}
