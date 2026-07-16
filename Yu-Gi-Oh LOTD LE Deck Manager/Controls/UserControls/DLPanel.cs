using System.Drawing.Drawing2D;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

/// <summary>
/// Duel Links-styled container panel: rounded rectangle, navy panel fill with
/// a faint top-to-bottom gradient, a bright cyan (or custom) border, and a
/// thin diagonal sheen along the top edge — the same "info box" look used for
/// the Main Deck/Extra Deck header, the Card Inventory panel, and list rows
/// throughout Duel Links. A normal Panel subclass (unlike DLButton/
/// DLBottomTabBar) since panels are meant to host child controls like every
/// other panel in this app.
/// </summary>
public class DLPanel : Panel
{
    public Color BorderColor { get; set; } = AppColors.Border;
    public int CornerRadius { get; set; } = 10;
    public float BorderWidth { get; set; } = 1.5f;

    public DLPanel()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        BackColor = Color.Transparent;
        Padding = new Padding(10);
    }

    private GraphicsPath BuildPath()
    {
        var path = new GraphicsPath();
        var r = new Rectangle(0, 0, Width - 1, Height - 1);
        int d = Math.Max(0, Math.Min(CornerRadius, Math.Min(r.Width, r.Height) / 2));

        if (d == 0)
        {
            path.AddRectangle(r);
            return path;
        }

        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using var path = BuildPath();
        using (var fill = new LinearGradientBrush(ClientRectangle, ControlPaint.Light(AppColors.CardBg, 0.05f),
                   AppColors.CardBg, LinearGradientMode.Vertical))
            g.FillPath(fill, path);

        using (var sheenPath = new GraphicsPath())
        {
            sheenPath.AddPolygon(new[]
            {
                new Point(0, 0),
                new Point(Width, 0),
                new Point(Width, Height / 6),
                new Point(0, Height / 3),
            });
            using var region = new Region(path);
            region.Intersect(sheenPath);
            using var sheenBrush = new SolidBrush(Color.FromArgb(14, 255, 255, 255));
            g.FillRegion(sheenBrush, region);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = BuildPath();
        using var pen = new Pen(BorderColor, BorderWidth);
        g.DrawPath(pen, path);
    }
}
