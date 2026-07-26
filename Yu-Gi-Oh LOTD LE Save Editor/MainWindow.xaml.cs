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

        // Single-file .ydk import (DeckSlotsView.ImportDeckButton_Click) -
        // opens the parsed deck in Deck Editor as a pending, unsaved load
        // instead of writing straight to the slot, same navigation pattern
        // as EditCardsRequested above. Multi-file imports never raise this;
        // they keep the original immediate-overwrite behavior and fire
        // SaveDataChanged instead (wired below).
        DeckSlotsPage.SingleDeckImportRequested += (_, import) =>
        {
            DeckEditorPage.LoadPendingDeck(import.Slot, import.Deck, import.SuggestedName);
            SideNav.SelectedIndex = 1;
        };

        // Import Deck / Save Deck both mutate AppContext.State.SaveBytes
        // directly - refresh the Save button's dirty state the same way any
        // other mutation does. They also both now bump StatsLayout's
        // Decks_Created counter on a first-time (previously empty) slot
        // write, so SaveEditorPage's Stats tab needs a refresh too, or it'll
        // keep showing a stale value until something else (e.g. undo/redo)
        // happens to refresh it first.
        DeckSlotsPage.SaveDataChanged += (_, _) => { SetDirty(IsActuallyDirty()); SaveEditorPage.RefreshFromState(); };
        DeckEditorPage.SaveDataChanged += (_, _) => { SetDirty(IsActuallyDirty()); SaveEditorPage.RefreshFromState(); };
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

    // Reverting SideNav.SelectedIndex below (when the user declines to
    // discard unsaved changes) re-raises this same event - _suppressNavGuard
    // skips the gate on that re-entrant call so it doesn't just prompt again,
    // and lets it fall through to ShowPage, which is a no-op anyway since
    // the index isn't actually changing.
    private bool _suppressNavGuard;

    private void SideNav_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (SideNav.SelectedIndex < 0) return;

        // Index 1 = DeckEditorPage (see _pages' comment for the fixed order).
        // Only relevant when it's the page actually being left - navigating
        // *into* it (e.g. DeckSlotsPage.EditCardsRequested below) always has
        // _currentPageIndex != 1 at this point, so it's never gated.
        if (!_suppressNavGuard && _currentPageIndex == 1 && DeckEditorPage.HasUnsavedChanges)
        {
            var result = AppMessageBox.Show(this,
                "This deck has unsaved changes. Switching tabs will discard them. Continue anyway?",
                "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
            {
                _suppressNavGuard = true;
                SideNav.SelectedIndex = _currentPageIndex;
                _suppressNavGuard = false;
                return;
            }
        }

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

        OpenSaveFile(dlg.FileName);
    }

    // ── Drag-and-drop open ────────────────────────────────────────────────────
    // Dropping a .dat file anywhere on the window opens it, same as OPEN SAVE
    // - AllowDrop/DragEnter/DragOver/Drop are wired on the Window itself in
    // MainWindow.xaml, not on any one page, so this works no matter which
    // page happens to be showing. DragEnter and DragOver share one handler
    // (WPF re-raises DragOver continuously while hovering, and the "is this a
    // droppable .dat" check never changes mid-drag) - only Drop needs its own.
    private void Window_DragEnter(object sender, DragEventArgs e)
    {
        e.Effects = TryGetDroppedDatPath(e) != null ? DragDropEffects.Copy : DragDropEffects.None;
        e.Handled = true;
    }

    private void Window_Drop(object sender, DragEventArgs e)
    {
        string? path = TryGetDroppedDatPath(e);
        e.Handled = true;
        if (path != null) OpenSaveFile(path);
    }

    /// <summary>Null unless the drag payload is exactly one file and it has a
    /// .dat extension - multi-file drops and non-.dat files (e.g. a .ydk
    /// meant for Deck Editor/Deck Slots' own import, or a stray image) are
    /// left alone rather than guessing which one was intended.</summary>
    private static string? TryGetDroppedDatPath(DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return null;
        if (e.Data.GetData(DataFormats.FileDrop) is not string[] { Length: 1 } files) return null;

        string path = files[0];
        return Path.GetExtension(path).Equals(".dat", StringComparison.OrdinalIgnoreCase) ? path : null;
    }

    /// <summary>Shared by the Open Save dialog, drag-and-drop, and (once a
    /// save is already open) the Restore Backup flow's implicit "load these
    /// bytes as the current save" step - validates the path and loads it.
    /// Doesn't back up the file itself - that now happens in OnSaveFile,
    /// right before the on-disk copy actually gets overwritten (see its own
    /// doc comment), rather than on every open regardless of whether the
    /// user ever saves. Not used by RestoreFromBackup itself, which
    /// intentionally does NOT touch _currentPath (see its own doc
    /// comment).</summary>
    private void OpenSaveFile(string path)
    {
        if (!File.Exists(path) || Path.GetExtension(path).ToLowerInvariant() != ".dat")
        {
            AppMessageBox.Show(this, "Please select a valid .dat save file.", Title,
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _currentPath = path;
        SaveFileLabel.Text = Path.GetFileName(path);

        AppContext.CardDb.Load();
        LoadSaveSlots();
    }

    // ── Restore backup ───────────────────────────────────────────────────────
    // "RESTORE BACKUP" in the top bar opens RestoreBackupPicker over whatever
    // save is currently open (see Services.SaveBackup for how backups are
    // named/found) and, if the user picks one, loads its bytes into the
    // editor the same way any other Save Editor mutation works: Undo.Snapshot
    // before the change, then the same refresh sequence Undo/Redo itself uses
    // (AfterUndoRedo) - so Ctrl+Z undoes the restore, and nothing is written
    // back to the real .dat on disk until Save is pressed. This deliberately
    // does NOT call OpenSaveFile/SaveBackup.Create - restoring isn't "opening
    // a different file", it's swapping the in-memory buffer for an old one,
    // exactly like Undo does.
    private void RestoreBackupButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_currentPath) || !File.Exists(_currentPath))
        {
            AppMessageBox.Show(this, "Open a save file first.", "Restore Backup",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        string? backupPath = RestoreBackupPicker.Show(this, _currentPath);
        if (backupPath == null) return;

        RestoreFromBackup(backupPath);
    }

    private void RestoreFromBackup(string backupPath)
    {
        if (AppContext.State == null) return;

        byte[] bytes;
        try
        {
            bytes = File.ReadAllBytes(backupPath);
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(this, $"Couldn't read that backup: {ex.Message}", "Restore Backup",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        AppContext.Undo.Snapshot();
        AppContext.State.SaveBytes = bytes;
        AfterUndoRedo();
        AppContext.State.Log?.Invoke($"Restored from backup {Path.GetFileName(backupPath)}.");
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

        // Back up whatever's currently on disk right before it gets
        // overwritten below - moved here (from OpenSaveFile) so opening a
        // save you never end up saving doesn't leave a backup behind; this
        // still captures the exact same "last on-disk state before this
        // write" snapshot Restore Backup expects, just triggered by Save
        // instead of Open.
        SaveBackup.Create(path);

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
