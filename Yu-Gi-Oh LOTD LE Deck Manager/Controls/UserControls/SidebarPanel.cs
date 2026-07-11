using ReaLTaiizor.Controls;
using Button = ReaLTaiizor.Controls.Button;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

/// <summary>
/// Left navigation sidebar / drawer.
/// Expanded = 200 px (icon + label). Collapsed = 48 px (icon only, with tooltips).
/// Icon + label are fused into a single Button per nav row (no separate icon
/// Label sitting on top of it), so the entire row stays clickable in both states.
/// All control layout lives in SidebarPanel.Designer.cs.
/// </summary>
public sealed partial class SidebarPanel : UserControl
{
    private const int ExpandedWidth = 200;
    private const int CollapsedWidth = 48;
    private const int AnimationIntervalMs = 12;
    private const int AnimationDurationMs = 160;

    // Gap between the icon glyph and the label when expanded. Tune to taste —
    // exact pixel alignment depends on ReaLTaiizor's text metrics.
    private const string IconLabelGap = "    ";

    // Icon-only collapsed rows have room to breathe, so the glyph gets its own
    // larger font instead of reusing the small label font size.
    private static readonly Font ExpandedNavFont = new("Segoe UI", 12F);
    private static readonly Font CollapsedNavFont = new("Segoe UI", 16F);

    private bool _collapsed;
    private Button? _activeBtn;

    private readonly System.Windows.Forms.Timer _animTimer;
    private readonly ToolTip _collapsedTips = new();
    private readonly Dictionary<Button, (string Icon, string Label)> _navItems = new();
    private int _animStartWidth;
    private int _animTargetWidth;
    private DateTime _animStartTime;

    public event EventHandler<string>? NavItemClicked;

    /// <summary>Raised once a collapse/expand animation finishes.</summary>
    public event EventHandler<bool>? CollapsedChanged;

    public bool IsCollapsed => _collapsed;

    public SidebarPanel()
    {
        InitializeComponent();

        // Prevent the drawer from ever being resized outside its two valid states.
        MinimumSize = new Size(CollapsedWidth, 0);
        Width = ExpandedWidth;

        _animTimer = new System.Windows.Forms.Timer { Interval = AnimationIntervalMs };
        _animTimer.Tick += AnimTimer_Tick;

        RegisterNavItems();
        SetActiveBtn(btnDeckSlots, "DeckSlots");
        ApplyCollapsedVisualState(false); // establish the expanded text/tooltip state
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _animTimer.Stop();
            _animTimer.Dispose();
            _collapsedTips.Dispose();
        }
        base.Dispose(disposing);
    }

    // ── Public API ────────────────────────────────────────────────────────────
    public void SetSaveFileName(string fileName) =>
        lblSaveFile.Text = $"📄  {fileName}";

    public void SetSlotCount(int count) =>
        lblStatus.Text = $"● {count} slots loaded";

    /// <summary>Collapse or expand the drawer with a slide animation.</summary>
    public void SetCollapsed(bool collapsed)
    {
        if (_collapsed == collapsed)
            return;

        _collapsed = collapsed;

        _animStartWidth = Width;
        _animTargetWidth = _collapsed ? CollapsedWidth : ExpandedWidth;
        _animStartTime = DateTime.UtcNow;

        ApplyCollapsedVisualState(_collapsed);

        if (!_animTimer.Enabled)
            _animTimer.Start();
    }

    // ── Collapse / expand ─────────────────────────────────────────────────────
    private void btnToggle_Click(object? sender, EventArgs e) => SetCollapsed(!_collapsed);

    private void ApplyCollapsedVisualState(bool collapsed)
    {
        lblLogoTitle.Visible = !collapsed;
        lblLogoSub.Visible = !collapsed;
        lblSaveFile.Visible = !collapsed;
        lblStatus.Visible = !collapsed;
        lblSecManage.Visible = !collapsed;
        lblSecTools.Visible = !collapsed;
        lblSecFile.Visible = !collapsed;

        btnToggle.Text = collapsed ? "▶" : "◀";

        // Re-flow each button's own text instead of hiding a separate icon
        // control — the button (and its click handler) always occupies the
        // full row, whether that row is 200px or 48px wide.
        foreach (var (btn, info) in _navItems)
        {
            btn.Text = collapsed ? info.Icon : $"{info.Icon}{IconLabelGap}{info.Label}";
            btn.TextAlignment = collapsed ? StringAlignment.Center : StringAlignment.Near;
            btn.Font = collapsed ? CollapsedNavFont : ExpandedNavFont;
        }

        SetCollapsedTooltipsEnabled(collapsed);
    }

    private void AnimTimer_Tick(object? sender, EventArgs e)
    {
        double elapsedMs = (DateTime.UtcNow - _animStartTime).TotalMilliseconds;
        double t = Math.Min(1.0, elapsedMs / AnimationDurationMs);
        double eased = 1 - Math.Pow(1 - t, 3); // ease-out cubic

        int newWidth = _animStartWidth + (int)Math.Round((_animTargetWidth - _animStartWidth) * eased);

        SuspendLayout();
        Width = newWidth;
        btnToggle.Location = new Point(Math.Max(12, newWidth - 34), 14);
        ResumeLayout(true);

        if (t >= 1.0)
        {
            _animTimer.Stop();
            Width = _animTargetWidth;
            btnToggle.Location = new Point(Math.Max(12, _animTargetWidth - 34), 14);
            CollapsedChanged?.Invoke(this, _collapsed);
        }
    }

    // ── Nav item registry ────────────────────────────────────────────────────
    private void RegisterNavItems()
    {
        _navItems[btnDeckSlots] = ("⊞", "Deck slots");
        _navItems[btnImportYdk] = ("↑", "Import .ydk");
        _navItems[btnCopySwap] = ("⇄", "Copy / swap");
        _navItems[btnDeckEditor] = ("▦", "Deck editor");
        _navItems[btnCardSearch] = ("⌕", "Card search");
        _navItems[btnOpenSave] = ("▤", "Open save");
        _navItems[btnSaveFile] = ("💾", "Save file");

        // Only useful once the label text disappears and just the glyph remains.
        foreach (var (btn, info) in _navItems)
            _collapsedTips.SetToolTip(btn, info.Label);

        SetCollapsedTooltipsEnabled(false);
    }

    private void SetCollapsedTooltipsEnabled(bool enabled) => _collapsedTips.Active = enabled;

    // ── Nav button click handlers ─────────────────────────────────────────────
    private void SetActiveBtn(Button btn, string key)
    {
        if (_activeBtn != null)
        {
            _activeBtn.InactiveColor = AppColors.CardBg;
            _activeBtn.ForeColor = AppColors.TextNav;
        }

        _activeBtn = btn;
        _activeBtn.InactiveColor = AppColors.GoldBtnBg;
        _activeBtn.ForeColor = AppColors.GoldBtnFg;

        NavItemClicked?.Invoke(this, key);
    }

    private void btnDeckSlots_Click(object? s, EventArgs e) => SetActiveBtn(btnDeckSlots, "DeckSlots");
    private void btnImportYdk_Click(object? s, EventArgs e) => SetActiveBtn(btnImportYdk, "ImportYdk");
    private void btnCopySwap_Click(object? s, EventArgs e) => SetActiveBtn(btnCopySwap, "CopySwap");
    private void btnDeckEditor_Click(object? s, EventArgs e) => SetActiveBtn(btnDeckEditor, "DeckEditor");
    private void btnCardSearch_Click(object? s, EventArgs e) => SetActiveBtn(btnCardSearch, "CardSearch");
    private void btnOpenSave_Click(object? s, EventArgs e) => NavItemClicked?.Invoke(this, "OpenSave");
    private void btnSaveFile_Click(object? s, EventArgs e) => NavItemClicked?.Invoke(this, "SaveFile");
}