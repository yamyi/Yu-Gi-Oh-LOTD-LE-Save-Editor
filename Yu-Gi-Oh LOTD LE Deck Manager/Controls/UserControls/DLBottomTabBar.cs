using System.Drawing.Drawing2D;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

/// <summary>One tab in a DLBottomTabBar — icon-over-label, matching Duel
/// Links' bottom nav (Gate / PvP Arena / Shop / Duel Studio, with the center
/// duel button drawn bigger/circular). Data only, no runtime control
/// creation — DLBottomTabBar itself is the one control that paints every tab.</summary>
public sealed class DLTabItem
{
    public string Text { get; }
    public Image? Icon { get; }

    /// <summary>Draws this tab bigger, in a raised circle, like Duel Links'
    /// center "start a duel" button — for whichever tab is the app's single
    /// most central action (kept optional/off by default elsewhere).</summary>
    public bool Elevated { get; }

    public DLTabItem(string text, Image? icon = null, bool elevated = false)
    {
        Text = text;
        Icon = icon;
        Elevated = elevated;
    }
}

/// <summary>
/// Duel Links-styled bottom tab bar: a dark navy strip with a bright cyan
/// glow line along the top edge, evenly-spaced icon-over-label tabs (gold
/// when active, muted cyan otherwise), and support for one visually
/// "elevated" tab drawn as a raised circle above the bar — mirroring the
/// center duel button on Duel Links' own bottom nav. Fully owner-drawn, one
/// control managing every tab's hit-testing — no child controls per tab.
/// </summary>
public sealed class DLBottomTabBar : Control
{
    private readonly List<DLTabItem> _tabs = new();
    private int _selectedIndex;
    private int _hoveredIndex = -1;

    public IReadOnlyList<DLTabItem> Tabs => _tabs;

    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (value < 0 || value >= _tabs.Count || value == _selectedIndex) return;
            _selectedIndex = value;
            Invalidate();
            SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? SelectedIndexChanged;

    public DLBottomTabBar()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                  ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        Height = 64;
        Dock = DockStyle.Bottom;
        Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        Cursor = Cursors.Hand;
    }

    /// <summary>Replaces the full tab set. Call once at startup — this is
    /// data population (like CheckedListBox.Items), not runtime control
    /// creation, since DLBottomTabBar is itself the one declared control.</summary>
    public void SetTabs(IEnumerable<DLTabItem> tabs, int selectedIndex = 0)
    {
        _tabs.Clear();
        _tabs.AddRange(tabs);
        _selectedIndex = Math.Clamp(selectedIndex, 0, Math.Max(0, _tabs.Count - 1));
        Invalidate();
    }

    private Rectangle TabBounds(int index)
    {
        if (_tabs.Count == 0) return Rectangle.Empty;
        int slotWidth = Width / _tabs.Count;
        return new Rectangle(index * slotWidth, 0, slotWidth, Height);
    }

    private int HitTest(Point p)
    {
        for (int i = 0; i < _tabs.Count; i++)
            if (TabBounds(i).Contains(p)) return i;
        return -1;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        int i = HitTest(e.Location);
        if (i != _hoveredIndex) { _hoveredIndex = i; Invalidate(); }
        base.OnMouseMove(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        _hoveredIndex = -1;
        Invalidate();
        base.OnMouseLeave(e);
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
        int i = HitTest(e.Location);
        if (i >= 0) SelectedIndex = i;
        base.OnMouseClick(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (var bg = new SolidBrush(AppColors.SidebarBg))
            g.FillRectangle(bg, ClientRectangle);

        using (var glow = new Pen(AppColors.Border, 1.5f))
            g.DrawLine(glow, 0, 0, Width, 0);

        for (int i = 0; i < _tabs.Count; i++)
        {
            var tab = _tabs[i];
            var bounds = TabBounds(i);
            bool active = i == _selectedIndex;
            bool hovered = i == _hoveredIndex;

            Color fg = active ? AppColors.TextNavActive : (hovered ? AppColors.BorderHover : AppColors.TextNav);

            if (tab.Elevated)
            {
                int d = Math.Min(Height - 8, bounds.Width - 16);
                var circleRect = new Rectangle(bounds.X + (bounds.Width - d) / 2, bounds.Y - 10, d, d);
                using var circleFill = new LinearGradientBrush(circleRect,
                    active ? AppColors.GoldBtnHover : AppColors.GoldBtnBg,
                    active ? AppColors.GoldBtnBg : ControlPaint.Dark(AppColors.GoldBtnBg, 0.2f),
                    LinearGradientMode.Vertical);
                g.FillEllipse(circleFill, circleRect);
                using var circleBorder = new Pen(AppColors.BorderHover, 2f);
                g.DrawEllipse(circleBorder, circleRect);

                if (tab.Icon is not null)
                {
                    int iconSize = (int)(d * 0.55);
                    g.DrawImage(tab.Icon, circleRect.X + (d - iconSize) / 2, circleRect.Y + (d - iconSize) / 2, iconSize, iconSize);
                }

                var labelRect = new Rectangle(bounds.X, bounds.Bottom - 16, bounds.Width, 16);
                TextRenderer.DrawText(g, tab.Text, Font, labelRect, fg, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
                continue;
            }

            int iconTop = 8;
            int iconH = tab.Icon is not null ? Math.Min(24, Height - 30) : 0;
            if (tab.Icon is not null)
            {
                var iconRect = new Rectangle(bounds.X + (bounds.Width - iconH) / 2, iconTop, iconH, iconH);
                g.DrawImage(tab.Icon, iconRect);
            }

            var textRect = new Rectangle(bounds.X, iconTop + iconH + 2, bounds.Width, Height - iconTop - iconH - 4);
            TextRenderer.DrawText(g, tab.Text, Font, textRect, fg,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.EndEllipsis);

            if (active)
            {
                int barWidth = Math.Min(bounds.Width - 20, 36);
                var barRect = new Rectangle(bounds.X + (bounds.Width - barWidth) / 2, Height - 4, barWidth, 3);
                using var barBrush = new SolidBrush(AppColors.TextNavActive);
                g.FillRectangle(barBrush, barRect);
            }
        }
    }
}
