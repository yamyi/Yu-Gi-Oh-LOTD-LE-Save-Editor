using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using YuGiOhSaveEditor.Controls;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor;

public partial class MainWindow : Window
{
    // ── Page navigation ──────────────────────────────────────────────────────
    // Every page is declared directly in MainWindow.xaml (DeckSlotsPage,
    // DeckEditorPage, ...) with x:Name - no `new SomeView()` anywhere in this
    // file. This array just collects references to those already-declared
    // instances, matching the WinForms build's pnlContent (five named page
    // controls added in Designer.cs; MainForm.cs only ever toggled .Visible).
    // Order matches the WinForms build's old index order (DeckSlots=0,
    // DeckEditor=1, CardSearch=2, SaveEditor=3, Settings=4) and SideNav's XAML
    // item order. All five are real views now (SettingsPage was the last
    // PlaceholderView stand-in).
    private FrameworkElement[] _pages = Array.Empty<FrameworkElement>();
    private int _currentPageIndex = -1;

    // ── File / dirty-tracking state (ported straight from the WinForms
    // MainForm.cs - same fields, same logic, only the UI glue differs) ──────
    private string _currentPath = string.Empty;
    private byte[]? _originalSaveBytes;

    public MainWindow()
    {
        InitializeComponent();

        _currentPath = AppContext.State?.SavePath ?? string.Empty;
        _originalSaveBytes = AppContext.State?.DirtyBaseline;

        _pages = new FrameworkElement[]
        {
            DeckSlotsPage,
            DeckEditorPage,
            CardSearchPage,
            SaveEditorPage,
            SettingsPage,
        };

        _currentPageIndex = 0; // DeckSlotsPage starts Visible in XAML already
        SideNav.SelectedIndex = 0;

        DeckSlotsPage.EditCardsRequested += (_, slot) =>
        {
            DeckEditorPage.LoadDeck(slot, AppContext.CardDb);
            // Drive navigation through SideNav.SelectedIndex (not a direct
            // ShowPage(1) call) so the nav highlight stays in sync with the
            // page that's actually showing - SideNav_SelectionChanged is
            // what actually calls ShowPage.
            SideNav.SelectedIndex = 1;
        };

        // Import Deck / Save Deck both mutate AppContext.State.SaveBytes
        // directly - refresh the Save button's dirty state the same way any
        // other mutation does.
        DeckSlotsPage.SaveDataChanged += (_, _) => SetDirty(IsActuallyDirty());
        DeckEditorPage.SaveDataChanged += (_, _) => SetDirty(IsActuallyDirty());
        SaveEditorPage.SaveDataChanged += (_, _) => SetDirty(IsActuallyDirty());

        AppContext.Undo.StackChanged += RefreshUndoRedoButtons;
        RefreshUndoRedoButtons();
    }

    protected override void OnClosed(EventArgs e)
    {
        AppContext.Undo.StackChanged -= RefreshUndoRedoButtons;
        base.OnClosed(e);
    }

    // ── Page navigation ───────────────────────────────────────────────────────
    private void ShowPage(int index)
    {
        if (index < 0 || index >= _pages.Length || index == _currentPageIndex)
            return;

        var oldPage = _pages[_currentPageIndex];
        var newPage = _pages[index];
        _currentPageIndex = index;

        // Quick crossfade instead of an instant Visibility flip - the one
        // thing a straight WinForms .Visible swap could never give this app
        // for free.
        newPage.Opacity = 0;
        newPage.Visibility = Visibility.Visible;

        var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(110));
        fadeOut.Completed += (_, _) =>
        {
            oldPage.Visibility = Visibility.Collapsed;
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(160));
            newPage.BeginAnimation(OpacityProperty, fadeIn);
        };
        oldPage.BeginAnimation(OpacityProperty, fadeOut);
    }

    private void SideNav_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (SideNav.SelectedIndex >= 0)
            ShowPage(SideNav.SelectedIndex);
    }

    // ── Nav collapse ──────────────────────────────────────────────────────────
    // Collapses SideNav down to a slim icon-only rail instead of hiding it
    // outright, so navigation stays one click away. SidebarBorder.Width is a
    // plain animatable double (not a GridLength), and its parent
    // ColumnDefinition is Width="Auto", so animating it reflows the content
    // column next to it for free - no runtime control creation, just an
    // animation on an already-declared named element.
    private bool _navCollapsed;
    private const double SidebarExpandedWidth = 340;
    private const double SidebarCollapsedWidth = 68;

    private void NavToggleButton_Click(object sender, RoutedEventArgs e)
    {
        _navCollapsed = !_navCollapsed;
        MenuHeaderPanel.Visibility = _navCollapsed ? Visibility.Collapsed : Visibility.Visible;

        double target = _navCollapsed ? SidebarCollapsedWidth : SidebarExpandedWidth;
        var anim = new DoubleAnimation(SidebarBorder.Width, target, TimeSpan.FromMilliseconds(180))
        {
            EasingFunction = new System.Windows.Media.Animation.QuadraticEase
            {
                EasingMode = System.Windows.Media.Animation.EasingMode.EaseInOut,
            },
        };
        SidebarBorder.BeginAnimation(WidthProperty, anim);
    }

    // ── Keyboard shortcuts ────────────────────────────────────────────────────
    // Ctrl+Z/Y/O/S for the same actions the toolbar buttons already
    // trigger - respects the same IsEnabled gating each button uses (e.g.
    // Ctrl+S does nothing if there's nothing dirty to save) rather than
    // bypassing it. If focus is inside a TextBox, WPF's own built-in
    // Ctrl+Z/Y text-undo handling already marks the key event Handled before
    // it would ever reach this Window-level handler, so this never fights
    // with in-progress text editing.
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control) return;

        switch (e.Key)
        {
            case Key.Z:
                if (UndoButton.IsEnabled) PerformUndo();
                e.Handled = true;
                break;
            case Key.Y:
                if (RedoButton.IsEnabled) PerformRedo();
                e.Handled = true;
                break;
            case Key.O:
                OnOpenSave();
                e.Handled = true;
                break;
            case Key.S:
                if (SaveButton.IsEnabled) OnSaveFile();
                e.Handled = true;
                break;
        }
    }

    // ── Undo / redo ───────────────────────────────────────────────────────────
    private void UndoButton_Click(object sender, RoutedEventArgs e) => PerformUndo();
    private void RedoButton_Click(object sender, RoutedEventArgs e) => PerformRedo();

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
    // byte[] - every page that caches anything off SaveBytes has to re-pull
    // from AppContext.State here, same as the old MainForm.AfterUndoRedo.
    // DeckEditorPage.RefreshFromState was missing entirely until now, which
    // is why Undo/Redo looked like it did nothing while that page was open -
    // the save bytes really were reverting, this view just never re-read
    // them afterward.
    private void AfterUndoRedo()
    {
        RefreshSlots(AppContext.State.SaveBytes);
        DeckEditorPage.RefreshFromState();
        SaveEditorPage.RefreshFromState();
        SetDirty(IsActuallyDirty());
    }

    private void RefreshUndoRedoButtons()
    {
        if (!Dispatcher.CheckAccess()) { Dispatcher.Invoke(RefreshUndoRedoButtons); return; }
        UndoButton.IsEnabled = AppContext.Undo.CanUndo;
        RedoButton.IsEnabled = AppContext.Undo.CanRedo;
    }

    // ── File actions ──────────────────────────────────────────────────────────
    private void OpenSaveButton_Click(object sender, RoutedEventArgs e) => OnOpenSave();
    private void SaveButton_Click(object sender, RoutedEventArgs e) => OnSaveFile();

    private void OnOpenSave()
    {
        var dlg = new OpenFileDialog
        {
            Title = "Open save file",
            Filter = "Save files (*.dat)|*.dat|All files (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = false,
        };
        if (dlg.ShowDialog(this) != true) return;

        if (!File.Exists(dlg.FileName) || Path.GetExtension(dlg.FileName).ToLowerInvariant() != ".dat")
        {
            AppMessageBox.Show(this, "Please select a valid .dat save file.", Title,
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _currentPath = dlg.FileName;
        SaveFileLabel.Text = Path.GetFileName(dlg.FileName);

        BackupSaveFile(_currentPath);

        AppContext.CardDb.Load();
        LoadSaveSlots();
    }

    // ── Auto-backup ───────────────────────────────────────────────────────────
    // Copies the .dat file to a numbered sibling backup the moment it's opened -
    // before anything in this app has a chance to touch it - given how much
    // manual byte-offset writing SlotIO/CampaignSaveLayout/etc. do directly
    // against a save file. A failed backup (locked/read-only folder, out of
    // disk space) shouldn't block opening the save itself, so this only ever
    // logs and moves on rather than showing an error dialog.
    //
    // Backups live right next to the save file itself (e.g. "savegame.dat"
    // gets "savegame.dat.bak_1", "savegame.dat.bak_2", ...) rather than under
    // %LocalAppData% - changed 2026-07-23 per user request. bak_1 is always
    // the most recent backup: each call rotates every existing bak_N up to
    // bak_(N+1) (oldest, bak_MaxBackupsPerFile, is dropped first) and then
    // writes the fresh copy as bak_1, so higher numbers are older - same
    // "keep the last N" intent as the old timestamp-based scheme, just
    // sequential instead of timestamped and colocated instead of centralized.
    private const int MaxBackupsPerFile = 20;

    private static void BackupSaveFile(string path)
    {
        try
        {
            string? dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir)) return; // no folder to place a sibling backup in

            string BackupPath(int n) => $"{path}.bak_{n}";

            string oldest = BackupPath(MaxBackupsPerFile);
            if (File.Exists(oldest))
            {
                try { File.Delete(oldest); }
                catch { /* not worth failing the backup over a stale file that won't delete */ }
            }

            // Walk from the second-oldest slot down to bak_1, freeing each
            // destination right before it's needed (bak_MaxBackupsPerFile was
            // just vacated above, so shifting bak_(Max-1) into it is always safe,
            // and so on down the chain).
            for (int i = MaxBackupsPerFile - 1; i >= 1; i--)
            {
                string from = BackupPath(i);
                if (!File.Exists(from)) continue;
                try { File.Move(from, BackupPath(i + 1), overwrite: true); }
                catch { /* leave it - a failed rotation step just means that slot repeats next time */ }
            }

            string backupPath = BackupPath(1);
            File.Copy(path, backupPath, overwrite: true);
            AppContext.State?.Log?.Invoke($"Backed up save to {Path.GetFileName(backupPath)}.");
        }
        catch
        {
            AppContext.State?.Log?.Invoke("Couldn't create a backup of the save file (continuing anyway).");
        }
    }

    private void LoadSaveSlots()
    {
        if (!File.Exists(_currentPath))
        {
            AppContext.State.Log?.Invoke("Pick a valid save file.");
            return;
        }

        byte[] bytes = File.ReadAllBytes(_currentPath);
        AppContext.State.SavePath = _currentPath;

        // Detect which on-disk save format this file uses (original "Lotd" vs
        // "Link Evolution") - see LotdSaveFormat.DetectVersion.
        AppContext.State.Version = LotdSaveFormat.DetectVersion(bytes);
        SlotLayout.CurrentVersion = AppContext.State.Version;

        _originalSaveBytes = (byte[])bytes.Clone();
        AppContext.State.DirtyBaseline = (byte[])bytes.Clone();
        AppContext.State.SaveBytes = (byte[])bytes.Clone();

        // A freshly (re)loaded save is a new baseline.
        AppContext.Undo.Clear();

        SetDirty(IsActuallyDirty());
        RefreshSlots(bytes);
        SaveEditorPage.RefreshFromState();

        int empties = AppContext.State.Slots.Count(s => s.IsEmpty);
        AppContext.State.Log?.Invoke($"Loaded {AppContext.State.Slots.Length} slots. Empty: {empties}");
    }

    private void RefreshSlots(byte[] bytes)
    {
        if (AppContext.State == null) return;

        var slots = AppContext.SlotIo.ReadAllSlots(bytes);
        AppContext.State.Slots = slots.ToArray();
        // Setting AppContext.State.Slots fires SlotsChanged, which
        // DeckSlotsView already subscribes to in its constructor, so it
        // repopulates itself here with no further wiring needed.
    }

    private void OnSaveFile()
    {
        if (AppContext.State == null) return;

        string path = _currentPath;
        if (!File.Exists(path))
        {
            AppContext.State.Log?.Invoke("Invalid save path.");
            return;
        }

        if (AppContext.State.SaveBytes == null || AppContext.State.SaveBytes.Length == 0)
        {
            AppContext.State.Log?.Invoke("No save data loaded.");
            return;
        }

        SaveSignatureFixer.FixInPlace(AppContext.State.SaveBytes);
        File.WriteAllBytes(path, AppContext.State.SaveBytes);

        _originalSaveBytes = (byte[])AppContext.State.SaveBytes.Clone();
        AppContext.State.DirtyBaseline = (byte[])AppContext.State.SaveBytes.Clone();

        SetDirty(IsActuallyDirty());
        AppContext.State.Log?.Invoke("Save written to disk.");
    }

    private void SetDirty(bool value)
    {
        SaveButton.IsEnabled = value;
        AppContext.State?.Log?.Invoke(value ? "Changes detected (unsaved)." : "All changes saved.");
    }

    private bool IsActuallyDirty()
    {
        if (AppContext.State?.SaveBytes == null || _originalSaveBytes == null) return false;
        if (AppContext.State.SaveBytes.Length != _originalSaveBytes.Length) return true;
        return !AppContext.State.SaveBytes.SequenceEqual(_originalSaveBytes);
    }
}
