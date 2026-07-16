
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;
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

    // ── Page navigation ──────────────────────────────────────────────────────
    // Every page control lives directly in pnlContent, stacked Dock=Fill on
    // top of one another (see Designer.cs) — ShowPage(int) just toggles which
    // one is Visible, mirroring how a mobile app swaps the current screen
    // instead of a desktop multi-tab strip. bottomNav (DLBottomTabBar) drives
    // this and stays in sync whenever code elsewhere jumps pages directly
    // (e.g. DeckSlots_EditCards jumping to the Deck Editor tab).
    private Control[] _pages = Array.Empty<Control>();
    private int _currentPageIndex = -1;

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

        _pages = new Control[] { deckSlotsPage, deckEditorPage, cardSearchPage, saveEditorPage, settingsPage };

        bottomNav.SetTabs(new[]
        {
            new DLTabItem("Deck Slots"),
            new DLTabItem("Deck Editor", elevated: true),
            new DLTabItem("Card Search"),
            new DLTabItem("Save Editor"),
            new DLTabItem("Settings"),
        });
        ShowPage(0);

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

        // SaveEditorPage writes straight into AppContext.State.SaveBytes too
        // (duel points, campaign, unlocks, card collection) — same dirty
        // tracking as the other editors. It snapshots undo itself before
        // each edit.
        saveEditorPage.DataChanged += (_, _) =>
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
    // this instance and let Program.cs's loop spin up a fresh one. The app
    // now ships a single fixed Duel Links look (no more Theme Editor access
    // point), so in practice this only fires if something else in the code
    // calls ThemeManager — kept for that safety net.
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

    // ── Page navigation ───────────────────────────────────────────────────────
    private void ShowPage(int index)
    {
        if (index < 0 || index >= _pages.Length || index == _currentPageIndex)
        {
            if (index >= 0 && index < _pages.Length)
                bottomNav.SelectedIndex = index;
            return;
        }

        for (int i = 0; i < _pages.Length; i++)
            _pages[i].Visible = i == index;

        _currentPageIndex = index;
        bottomNav.SelectedIndex = index;
    }

    private void BottomNav_SelectedIndexChanged(object? sender, EventArgs e) => ShowPage(bottomNav.SelectedIndex);

    private void NavigateTo(string key)
    {
        switch (key)
        {
            case "DeckSlots": ShowPage(0); break;
            case "DeckEditor": ShowPage(1); break;
            case "CardSearch": ShowPage(2); break;
            case "SaveEditor": ShowPage(3); break;
            case "Settings": ShowPage(4); break;
            case "OpenSave": OnOpenSave(); break;
            case "SaveFile": OnSaveFile(); break;
        }
    }

    // ── Topbar actions ────────────────────────────────────────────────────────
    private void btnOpenSave_Click(object? sender, EventArgs e) => OnOpenSave();
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
        saveEditorPage.RefreshFromState();
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

        if (_currentPageIndex == 0)
            deckSlotsPage.HandleGridKeyDown(e);
        else if (_currentPageIndex == 1)
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
        ShowPage(1); // DeckEditor tab index
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

        // Detect which on-disk save format this file uses (original "Lotd" vs
        // "Link Evolution" — they put the deck-slot/campaign/misc/card-list
        // chunks at different offsets). Everything that reads those chunks
        // (SlotLayout, MiscSaveLayout, CampaignSaveLayout, CardCollectionLayout)
        // reads AppContext.State.Version / SlotLayout.CurrentVersion rather
        // than assuming a fixed layout.
        AppContext.State.Version = LotdSaveFormat.DetectVersion(bytes);
        SlotLayout.CurrentVersion = AppContext.State.Version;

        _originalSaveBytes = (byte[])bytes.Clone();
        AppContext.State.DirtyBaseline = (byte[])bytes.Clone();
         AppContext.State.SaveBytes = (byte[])bytes.Clone();

        // A freshly (re)loaded save is a new baseline — any prior undo
        // history refers to bytes that no longer apply.
        AppContext.Undo.Clear();

        SetDirty(IsActuallyDirty());

        RefreshSlots(bytes);
        saveEditorPage.RefreshFromState();

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
}
