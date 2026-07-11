
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Properties;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;
using static Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services.SlotIO;
using Button= System.Windows.Forms.Button;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

public partial class MainForm : Form
{
    // ── Data ──────────────────────────────────────────────────────────────────
    private string currentpath;
    private byte[]? _originalSaveBytes;
    private bool _isDirty;

    // ── Sidebar / drawer state (formerly SidebarPanel UserControl) ─────────────
    // Expanded = 200 px (icon + label). Collapsed = 48 px (icon only, with tooltips).
    // Icon + label are fused into a single Button per nav row (no separate icon
    // Label sitting on top of it), so the entire row stays clickable in both states.
    private const int SidebarExpandedWidth = 200;
    private const int SidebarCollapsedWidth = 48;
    private const int SidebarAnimationIntervalMs = 12;
    private const int SidebarAnimationDurationMs = 160;

    // Gap between the icon glyph and the label when expanded. Tune to taste —
    // exact pixel alignment depends on ReaLTaiizor's text metrics.
    private const string SidebarIconLabelGap = "    ";

    // Icon-only collapsed rows have room to breathe, so the glyph gets its own
    // larger font instead of reusing the small label font size.
    private static readonly Font SidebarExpandedNavFont = new("Segoe UI", 12F);
    private static readonly Font SidebarCollapsedNavFont = new("Segoe UI", 16F);

    private bool _sidebarCollapsed;
    private ReaLTaiizor.Controls.Button? _activeNavBtn;

    private readonly System.Windows.Forms.Timer _sidebarAnimTimer = new() { Interval = SidebarAnimationIntervalMs };
    private readonly ToolTip _sidebarCollapsedTips = new();
    private readonly Dictionary<ReaLTaiizor.Controls.Button, (string Icon, string Label)> _navItems = new();
    private int _sidebarAnimStartWidth;
    private int _sidebarAnimTargetWidth;
    private DateTime _sidebarAnimStartTime;

    public MainForm()
    {
        InitializeComponent();
        AppContext.CardDb.Load();

        // Restore file/dirty state across a theme-change soft restart (see
        // Program.cs) — AppContext.State survives the rebuild even though
        // this instance's own fields start blank.
        currentpath = AppContext.State?.SavePath ?? string.Empty;
        _originalSaveBytes = AppContext.State?.DirtyBaseline;

        ThemeManager.ThemeChanged += OnThemeChanged;

        // Sidebar setup (previously done inside SidebarPanel's own constructor).
        pnlSidebar.MinimumSize = new Size(SidebarCollapsedWidth, 0);
        pnlSidebar.Width = SidebarExpandedWidth;
        _sidebarAnimTimer.Tick += SidebarAnimTimer_Tick;
        RegisterNavItems();
        SetActiveNavButton(btnDeckSlots, "DeckSlots");
        ApplySidebarCollapsedVisualState(false); // establish the expanded text/tooltip state




        // Hook DeckSlotsPage events
        deckSlotsPage.EditCardsRequested += new EventHandler<SlotInfo>(DeckSlots_EditCards);
        deckSlotsPage.ClearRequested += new EventHandler<SlotInfo>(DeckSlots_Clear);
        deckSlotsPage.ClearAllRequested += DeckSlots_ClearAll;
        deckSlotsPage.SlotDataChanged += (_, _) => SetDirty(IsActuallyDirty());

        // DeckEditorPage writes straight into AppContext.State.SaveBytes on
        // Save — refresh the Deck Slots grid so it reflects the edit, and
        // mark the app dirty so the user knows to hit Save File.
        deckEditorPage.SaveRequested += (_, _) =>
        {
            RefreshSlots(AppContext.State.SaveBytes);
            SetDirty(IsActuallyDirty());
        };

        // Undo/redo — global, since edits can come from either page.
        AppContext.Undo.StackChanged += RefreshUndoRedoButtons;
        RefreshUndoRedoButtons();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _sidebarAnimTimer.Stop();
            _sidebarAnimTimer.Dispose();
            _sidebarCollapsedTips.Dispose();
            ThemeManager.ThemeChanged -= OnThemeChanged;
            AppContext.Undo.StackChanged -= RefreshUndoRedoButtons;
        }
        base.Dispose(disposing);
    }

    // ── Form events ───────────────────────────────────────────────────────────
    private void MainForm_Load(object? sender, EventArgs e)
    {
        Program.TryEnableDarkTitleBar(this);
    }

    // ── Theming ───────────────────────────────────────────────────────────────
    // WinForms controls only read AppColors.X once, at construction, so the
    // only reliable way to re-skin everything is to rebuild the form. Close
    // this instance and let Program.cs's loop spin up a fresh one.
    private void OnThemeChanged()
    {
        if (InvokeRequired) { BeginInvoke(OnThemeChanged); return; }

        ThemeManager.RestartRequested = true;
        Close();
    }

    // ── Background image ─────────────────────────────────────────────────────
    // Shared with every page/panel that opts in — see BackgroundPainter for
    // why this needs to be more than "just paint it on the Form": most of
    // the client area is covered by opaque, individually-colored panels
    // (toolbars, card grids, list boxes) that this alone can't show through.
    protected override void OnPaintBackground(PaintEventArgs e)
    {
        base.OnPaintBackground(e);
        BackgroundPainter.Paint(this, e);
    }


    // ── Sidebar / drawer ─────────────────────────────────────────────────────

    private void NavigateTo(string key)
    {
        switch (key)
        {
            case "DeckSlots": tabControl.SelectedIndex = 0; break;
            case "DeckEditor": tabControl.SelectedIndex = 1; break;
            case "CardSearch": tabControl.SelectedIndex = 2; break;
            case "Settings": tabControl.SelectedIndex = 3; break;
            case "OpenSave": OnOpenSave(); break;
            case "SaveFile": OnSaveFile(); break;
        }
    }

    private void TabControl_SelectedIndexChanged(object? sender, EventArgs e)
    {
        string[] keys = { "DeckSlots", "ImportYdk", "CopySwap", "DeckEditor", "CardSearch", "Settings" };
        //if (tabControl.SelectedIndex >= 0 && tabControl.SelectedIndex < keys.Length)
          //  drawer.SetActiveItem(keys[tabControl.SelectedIndex]);
    }


    // ── Sidebar collapse / expand ────────────────────────────────────────────
    private void btnToggle_Click(object? sender, EventArgs e) => SetSidebarCollapsed(!_sidebarCollapsed);

    /// <summary>Collapse or expand the drawer with a slide animation.</summary>
    private void SetSidebarCollapsed(bool collapsed)
    {
        if (_sidebarCollapsed == collapsed)
            return;

        _sidebarCollapsed = collapsed;

        _sidebarAnimStartWidth = pnlSidebar.Width;
        _sidebarAnimTargetWidth = _sidebarCollapsed ? SidebarCollapsedWidth : SidebarExpandedWidth;
        _sidebarAnimStartTime = DateTime.UtcNow;

        ApplySidebarCollapsedVisualState(_sidebarCollapsed);

        if (!_sidebarAnimTimer.Enabled)
            _sidebarAnimTimer.Start();
    }

    private void ApplySidebarCollapsedVisualState(bool collapsed)
    {
        lblSecManage.Visible = !collapsed;
        lblSecTools.Visible = !collapsed;
        lblSecFile.Visible = !collapsed;

        btnToggle.Text = collapsed ? "▶" : "◀";

        // Re-flow each button's own text instead of hiding a separate icon
        // control — the button (and its click handler) always occupies the
        // full row, whether that row is 200px or 48px wide.
        foreach (var (btn, info) in _navItems)
        {
            btn.Text = collapsed ? info.Icon : $"{info.Icon}{SidebarIconLabelGap}{info.Label}";
            btn.TextAlignment = collapsed ? StringAlignment.Center : StringAlignment.Near;
            btn.Font = collapsed ? SidebarCollapsedNavFont : SidebarExpandedNavFont;
        }

        SetSidebarCollapsedTooltipsEnabled(collapsed);
    }

    private void SidebarAnimTimer_Tick(object? sender, EventArgs e)
    {
        double elapsedMs = (DateTime.UtcNow - _sidebarAnimStartTime).TotalMilliseconds;
        double t = Math.Min(1.0, elapsedMs / SidebarAnimationDurationMs);
        double eased = 1 - Math.Pow(1 - t, 3); // ease-out cubic

        int newWidth = _sidebarAnimStartWidth + (int)Math.Round((_sidebarAnimTargetWidth - _sidebarAnimStartWidth) * eased);

        pnlSidebar.SuspendLayout();
        pnlSidebar.Width = newWidth;
        pnlSidebar.ResumeLayout(true);

        if (t >= 1.0)
        {
            _sidebarAnimTimer.Stop();
            pnlSidebar.Width = _sidebarAnimTargetWidth;
        }
    }

    // ── Nav item registry ────────────────────────────────────────────────────
    private void RegisterNavItems()
    {
        _navItems[btnDeckSlots] = ("⊞", "Deck slots");
        _navItems[btnDeckEditor] = ("▦", "Deck editor");
        _navItems[btnCardSearch] = ("⌕", "Card search");
        _navItems[btnOpenSave] = ("▤", "Open save");
        _navItems[btnSaveFile] = ("💾", "Save file");
        _navItems[btnSettings] = ("⚙", "Settings");

        // Only useful once the label text disappears and just the glyph remains.
        foreach (var (btn, info) in _navItems) {
            _sidebarCollapsedTips.SetToolTip(btn, info.Label);
            btn.ForeColor = AppColors.TextNav;
            btn.InactiveColor = AppColors.CardBg;
        }
        SetActiveNavButton(btnDeckSlots, "DeckSlots");
        SetSidebarCollapsedTooltipsEnabled(false);
    }

    private void SetSidebarCollapsedTooltipsEnabled(bool enabled) => _sidebarCollapsedTips.Active = enabled;

    // ── Nav button click handlers ─────────────────────────────────────────────
    private void SetActiveNavButton(ReaLTaiizor.Controls.Button btn, string key)
    {
        if (_activeNavBtn != null)
        {
            _activeNavBtn.InactiveColor = AppColors.CardBg;
            _activeNavBtn.ForeColor = AppColors.TextNav;
        }

        _activeNavBtn = btn;
        _activeNavBtn.InactiveColor = AppColors.GoldBtnBg;
        _activeNavBtn.ForeColor = AppColors.GoldBtnFg;

        NavigateTo(key);
    }

    private void btnDeckSlots_Click(object? s, EventArgs e) => SetActiveNavButton(btnDeckSlots, "DeckSlots");
    private void btnDeckEditor_Click(object? s, EventArgs e) => SetActiveNavButton(btnDeckEditor, "DeckEditor");
    private void btnCardSearch_Click(object? s, EventArgs e) => SetActiveNavButton(btnCardSearch, "CardSearch");
    private void btnOpenSave_Click(object? s, EventArgs e) => NavigateTo("OpenSave");
    private void btnSaveFile_Click(object? s, EventArgs e) => NavigateTo("SaveFile");
    private void btnSettings_Click(object? s, EventArgs e) => SetActiveNavButton(btnSettings,"Settings");

    // ── Topbar actions ────────────────────────────────────────────────────────
    private void btnSave_Click(object? sender, EventArgs e) => OnSaveFile();

    // ── Undo / redo ───────────────────────────────────────────────────────────
    // Global (not per-page) since a mutation can come from either DeckSlots
    // or DeckEditor — both write into the same AppContext.State.SaveBytes.
    private void btnUndo_Click(object? sender, EventArgs e) => PerformUndo();
    private void btnRedo_Click(object? sender, EventArgs e) => PerformRedo();

    private void PerformUndo()
    {
        if (!AppContext.Undo.Undo()) return;
        AfterUndoRedo();
    }

    private void PerformRedo()
    {
        if (!AppContext.Undo.Redo()) return;
        AfterUndoRedo();
    }

    // The undo stack swaps AppContext.State.SaveBytes for a whole different
    // byte[] — every page showing data derived from it needs to re-pull from
    // scratch rather than assuming its in-memory grids are still current.
    private void AfterUndoRedo()
    {
        RefreshSlots(AppContext.State.SaveBytes);
        deckSlotsPage.RefreshSelectedDetail();
        deckEditorPage.ReloadCurrentDeck();
        SetDirty(IsActuallyDirty());
    }

    private void RefreshUndoRedoButtons()
    {
        if (InvokeRequired) { BeginInvoke(RefreshUndoRedoButtons); return; }
        btnUndo.Enabled = AppContext.Undo.CanUndo;
        btnRedo.Enabled = AppContext.Undo.CanRedo;
    }

    // Global keyboard shortcuts (Form.KeyPreview = true — see Designer.cs) —
    // Ctrl+Z / Ctrl+Y (and Ctrl+Shift+Z as an alt-redo) for undo/redo, plus
    // arrow-key/Delete grid navigation routed to whichever tab is active.
    // Skipped entirely while a text box has focus so typing/cursor movement
    // in a search box isn't hijacked.
    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (IsTextInputFocused(this)) return;

        if (e.Control && e.KeyCode == Keys.Z && !e.Shift)
        {
            PerformUndo();
            e.Handled = true;
            return;
        }

        if ((e.Control && e.KeyCode == Keys.Y) || (e.Control && e.Shift && e.KeyCode == Keys.Z))
        {
            PerformRedo();
            e.Handled = true;
            return;
        }

        bool isGridKey = e.KeyCode is Keys.Left or Keys.Right or Keys.Up or Keys.Down or Keys.Delete;
        if (!isGridKey) return;

        if (tabControl.SelectedIndex == 0)
            deckSlotsPage.HandleGridKeyDown(e);
        else if (tabControl.SelectedIndex == 1)
            deckEditorPage.HandleGridKeyDown(e);
    }

    private static bool IsTextInputFocused(Control root)
    {
        Control? c = root;
        while (c is ContainerControl cc && cc.ActiveControl is not null)
            c = cc.ActiveControl;
        return c is TextBoxBase or ComboBox;
    }

    // ── DeckSlots page handlers ───────────────────────────────────────────────
    private void DeckSlots_EditCards(object? sender, SlotInfo slot)
    {
        deckEditorPage.LoadDeck(slot, AppContext.CardDb);
        tabControl.SelectedIndex = 1;              // DeckEditor tab index
    }

    private void DeckSlots_Clear(object? sender, SlotInfo slot)
    {
        var res = AppMessageBox.Show(
            $"Clear slot {slot.SlotNumber} ({slot.Name})?",
            "Clear Slot", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (res != DialogResult.Yes) return;

        AppContext.Undo.Snapshot();
        AppContext.SlotIo.UpdateSlotOwner( AppContext.State.SaveBytes, slot,0);
        AppContext.SlotIo.UpdateName( AppContext.State.SaveBytes, slot,string.Empty);
        AppContext.SlotIo.ClearDeck( AppContext.State.SaveBytes, slot.SlotNumber);

        deckSlotsPage.RefreshCard(slot);
        deckSlotsPage.ClearDetail();
        SetDirty(IsActuallyDirty());
    }

    private void DeckSlots_ClearAll(object? sender, EventArgs e)
    {
        if (AppContext.State?.SaveBytes == null || AppContext.State.SaveBytes.Length == 0)
        {
            AppContext.State?.Log?.Invoke("No save data loaded.");
            return;
        }

        AppContext.Undo.Snapshot();
        foreach (var slot in AppContext.State.Slots)
        {
            AppContext.SlotIo.UpdateSlotOwner(AppContext.State.SaveBytes, slot, 0);
            AppContext.SlotIo.UpdateName(AppContext.State.SaveBytes, slot, string.Empty);
            AppContext.SlotIo.ClearDeck(AppContext.State.SaveBytes, slot.SlotNumber);
        }

        RefreshSlots(AppContext.State.SaveBytes);
        deckSlotsPage.ClearDetail();
        SetDirty(IsActuallyDirty());

        AppContext.State.Log?.Invoke("Cleared all deck slots.");
    }

    // ── File actions ──────────────────────────────────────────────────────────
    private void OnOpenSave()
    {

        using var dlg = new OpenFileDialog
        {
            Title = "Open save file",
            Filter = "Save files (*.dat)|*.dat|All files (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = false
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        if (File.Exists(dlg.FileName) == false
            || Path.GetExtension(dlg.FileName).ToLowerInvariant() != ".dat")
        {
            AppMessageBox.Show("Please select a valid .dat save file.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        else
        {
            currentpath = dlg.FileName;
            string name = Path.GetFileName(dlg.FileName);
            lblSaveFile.Text = name;

        }
        AppContext.CardDb.Load();
        LoadSaveSlots();
    }
    private void LoadSaveSlots()
    {

        if (!File.Exists(currentpath))
        {
             AppContext.State.Log?.Invoke("Pick a valid save file.");
            return;
        }

        byte[] bytes = File.ReadAllBytes(currentpath);

         AppContext.State.SavePath = currentpath;



        _originalSaveBytes = (byte[])bytes.Clone();
        AppContext.State.DirtyBaseline = (byte[])bytes.Clone();
         AppContext.State.SaveBytes = (byte[])bytes.Clone();

        // A freshly (re)loaded save is a new baseline — any prior undo
        // history refers to bytes that no longer apply.
        AppContext.Undo.Clear();

        SetDirty(IsActuallyDirty());

        RefreshSlots(bytes);

        int empties =  AppContext.State.Slots.Count(s => s.IsEmpty);


         AppContext.State.Log?.Invoke(
            $"Loaded { AppContext.State.Slots.Length} slots. Empty: {empties}");
    }
    private void RefreshSlots(byte[] bytes)
    {
        if (AppContext.State == null)
            return;

        List<SlotInfo> slots = AppContext.SlotIo.ReadAllSlots(bytes);

        AppContext.State.Slots = slots.ToArray();   // ← new: actually persist the loaded slots

        deckSlotsPage.LoadSlots(slots);
    
    }
    private void OnSaveFile()
    {
        if ( AppContext.State == null)
            return;

        string path = currentpath;

        if (!File.Exists(path))
        {
             AppContext.State.Log?.Invoke("Invalid save path.");
            return;
        }

        if ( AppContext.State.SaveBytes == null ||  AppContext.State.SaveBytes.Length == 0)
        {
             AppContext.State.Log?.Invoke("No save data loaded.");
            return;
        }

        SaveSignatureFixer.FixInPlace( AppContext.State.SaveBytes);

        File.WriteAllBytes(path,  AppContext.State.SaveBytes);

        _originalSaveBytes = (byte[]) AppContext.State.SaveBytes.Clone();
        AppContext.State.DirtyBaseline = (byte[]) AppContext.State.SaveBytes.Clone();

        SetDirty(IsActuallyDirty());

         AppContext.State.Log?.Invoke("Save written to disk.");
    }


    private void SetDirty(bool value)
    {
        _isDirty = value;

        btnSave.Enabled = value;

         AppContext.State?.Log?.Invoke(value
            ? "Changes detected (unsaved)."
            : "All changes saved.");
    }

    private bool IsActuallyDirty()
    {
        if ( AppContext.State?.SaveBytes == null || _originalSaveBytes == null)
            return false;

        if ( AppContext.State.SaveBytes.Length != _originalSaveBytes.Length)
            return true;

        return ! AppContext.State.SaveBytes.SequenceEqual(_originalSaveBytes);
    }


    // ── Prompt helper (kept here; used by DeckSlotsPage too via internal access) ─
    internal static class Prompt
    {
        public static string? Show(string title, string prompt, string defaultValue = "")
        {
            using var form = new Form
            {
                Text = title,
                Size = new Size(340, 130),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = AppColors.SidebarBg,
            };
            var lbl = new Label { Text = prompt, Left = 12, Top = 12, Width = 300, AutoSize = true, ForeColor = AppColors.TextSecondary };
            var txt = new TextBox { Left = 12, Top = 34, Width = 300, Text = defaultValue, BackColor = AppColors.AppBg, ForeColor = AppColors.TextPrimary, BorderStyle = BorderStyle.FixedSingle };
            var ok = new Button { Text = "OK", Left = 148, Top = 62, Width = 80, DialogResult = DialogResult.OK, BackColor = AppColors.GoldBtnBg, ForeColor = AppColors.GoldBtnFg, FlatStyle = FlatStyle.Flat };
            var cancel = new Button { Text = "Cancel", Left = 232, Top = 62, Width = 80, DialogResult = DialogResult.Cancel, BackColor = Color.Transparent, ForeColor = AppColors.TextMuted, FlatStyle = FlatStyle.Flat };
            ok.FlatAppearance.BorderSize = cancel.FlatAppearance.BorderSize = 0;
            form.Controls.AddRange(new Control[] { lbl, txt, ok, cancel });
            form.AcceptButton = ok;
            form.CancelButton = cancel;
            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }
    }

    private void btnSave_EnabledChanged(object sender, EventArgs e)
    {
        if(btnSave.Enabled)
            btnSave.BackColor = AppColors.GoldBtnBg;
        else
            btnSave.BackColor = Color.Transparent;
    }
}
