using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using YuGiOhSaveEditor.Controls;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Views;

/// <summary>
/// Master Duel's "My Deck" screen — a grid of the 32 fixed save-file deck
/// slots, rendered entirely by the SlotIO.SlotInfo DataTemplate in
/// DeckSlotsView.xaml. LoadSlots only ever assigns the existing slot array
/// straight to ItemsSource — the same category as the WinForms build's
/// CheckedListBox.Items/DataGridView.Rows population, not runtime UI control
/// creation. SlotsList is a ListBox (not a plain ItemsControl) so single
/// click gives native single-selection — which the Import/Export buttons key
/// their IsEnabled off of — while double-click (wired via an EventSetter in
/// the ItemContainerStyle) opens the slot for editing.
/// </summary>
public partial class DeckSlotsView : UserControl
{
    /// <summary>Raised when the user double-clicks a slot tile (empty or
    /// not). MainWindow wires this to navigate to the Deck Editor page —
    /// same flow as the WinForms build's DeckSlotsPage.EditCardsRequested ->
    /// MainForm.DeckSlots_EditCards.</summary>
    public event EventHandler<SlotIO.SlotInfo>? EditCardsRequested;

    /// <summary>Raised after Import Deck actually mutates AppContext.State.SaveBytes,
    /// so MainWindow can refresh its Save button / dirty state — the WinForms
    /// build's equivalent was DeckSlotsPage.SlotDataChanged. Only ever raised
    /// by the multi-file import path now - the single-file path raises
    /// SingleDeckImportRequested instead and never touches SaveBytes itself
    /// (see ImportDeckButton_Click).</summary>
    public event EventHandler? SaveDataChanged;

    /// <summary>Raised instead of SaveDataChanged when exactly one .ydk was
    /// selected - MainWindow opens the parsed deck in Deck Editor as a
    /// pending load (DeckEditorView.LoadPendingDeck) rather than writing it
    /// to the slot immediately, so nothing here mutates
    /// AppContext.State.SaveBytes at all; that only happens once the user
    /// presses Save Deck over there.</summary>
    public event EventHandler<PendingDeckImport>? SingleDeckImportRequested;

    public sealed record PendingDeckImport(SlotIO.SlotInfo Slot, Deck Deck, string SuggestedName, List<YdkSkippedCard> Skipped);

    private SlotIO.SlotInfo? _selectedSlot;

    /// <summary>Full, unfiltered slot array - what SearchBox actually
    /// filters against. SlotsList.ItemsSource is a separate (possibly
    /// smaller) list rebuilt by ApplyFilter, since WPF's ListBox has no
    /// per-item Visibility flag the way the WinForms build's own DeckCard
    /// control did.</summary>
    private SlotIO.SlotInfo[] _allSlots = Array.Empty<SlotIO.SlotInfo>();

    private enum PendingSlotAction { None, CopyTo, SwapWith }
    private PendingSlotAction _pendingAction = PendingSlotAction.None;
    private SlotIO.SlotInfo? _pendingSourceSlot;

    public DeckSlotsView()
    {
        InitializeComponent();

        LoadSlots(AppContext.State.Slots);
        AppContext.State.SlotsChanged += LoadSlots;
    }

    public void LoadSlots(SlotIO.SlotInfo[] slots)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => LoadSlots(slots));
            return;
        }

        _allSlots = slots;
        ApplyFilter();
    }

    // ── Search ───────────────────────────────────────────────────────────────
    // Ported from the WinForms build's DeckSlotsPage.txtSearch/SlotMatches -
    // matches a slot's own deck name OR any card inside its Main/Extra/Side,
    // so "which of my decks run Blue-Eyes White Dragon" works the same as
    // searching by deck name.

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

    private void ApplyFilter()
    {
        string query = SearchBox.Text?.Trim() ?? string.Empty;
        SlotsList.ItemsSource = string.IsNullOrEmpty(query)
            ? _allSlots
            : _allSlots.Where(s => SlotMatches(s, query)).ToList();
    }

    private static bool SlotMatches(SlotIO.SlotInfo slot, string query)
    {
        if (slot.Name.Contains(query, StringComparison.OrdinalIgnoreCase)) return true;
        if (slot.IsEmpty || AppContext.State?.SaveBytes == null) return false;

        var deck = AppContext.SlotIo.ReadDeck(AppContext.State.SaveBytes, slot, AppContext.CardDb);
        return deck.main.Concat(deck.extra).Concat(deck.side)
            .Any(card => card.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
    }

    private void SlotsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // A pending Copy To/Swap With pick takes over the very next slot
        // click instead of normal selection - see BeginPickSlot. Clicking
        // the source slot again deselects it (SlotItem_PreviewMouseLeftButtonDown's
        // toggle-off), landing here with SelectedItem == null, which is
        // exactly the "click it again to cancel" gesture.
        if (_pendingAction != PendingSlotAction.None)
        {
            if (SlotsList.SelectedItem is SlotIO.SlotInfo target)
                CompletePendingAction(target);
            else
                CancelPendingAction();
            return;
        }

        _selectedSlot = SlotsList.SelectedItem as SlotIO.SlotInfo;
        bool hasSelection = _selectedSlot != null;
        bool hasDeck = hasSelection && !_selectedSlot!.IsEmpty;
        ImportDeckButton.IsEnabled = hasSelection;
        ExportDeckButton.IsEnabled = hasDeck;
        ChangeDuelistButton.IsEnabled = hasDeck;
        RenameButton.IsEnabled = hasDeck;
        CopyToButton.IsEnabled = hasDeck;
        SwapWithButton.IsEnabled = hasDeck;
        ClearSlotButton.IsEnabled = hasDeck;
    }

    private void SlotItem_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is SlotIO.SlotInfo slot)
            EditCardsRequested?.Invoke(this, slot);
    }

    /// <summary>Re-clicking an already-selected tile deselects it. Runs on
    /// the Preview (tunneling) pass, before the Selector's own
    /// MouseLeftButtonDown handling would otherwise leave it selected, so
    /// marking the event handled here fully replaces the default behavior
    /// for this one case. Only applies to genuine single clicks
    /// (ClickCount == 1) - the second click of a double-click on an
    /// already-selected tile used to be swallowed here too, which both
    /// deselected the tile and starved SlotItem_DoubleClick of the click
    /// pair it needs to fire, so a double-click only ever "worked" if the
    /// tile happened to already be unselected going in.</summary>
    private void SlotItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount > 1) return;

        if (sender is ListBoxItem { IsSelected: true } item)
        {
            item.IsSelected = false;
            e.Handled = true;
        }
    }

    // ── Import / Export ─────────────────────────────────────────────────────
    // Ported from the WinForms build's DeckSlotsPage.ImportYdkFile/OnExportYdk,
    // unified into one consistent flow (the WinForms build had two import call
    // sites that disagreed on whether to snapshot for undo — this always does).

    private void ImportDeckButton_Click(object sender, RoutedEventArgs e)
    {
        var slot = _selectedSlot;
        var owner = Window.GetWindow(this);
        if (slot == null || AppContext.State?.SaveBytes == null) return;

        var dlg = new OpenFileDialog
        {
            Title = "Import .ydk",
            Filter = "YDK files (*.ydk)|*.ydk|All files (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = true,
        };
        if (dlg.ShowDialog(owner) != true) return;

        string[] files = dlg.FileNames;
        if (files.Length == 0) return;

        // A single selected file no longer overwrites the slot immediately -
        // it opens in Deck Editor as a pending load instead, and nothing
        // here touches AppContext.State.SaveBytes at all until the user
        // presses Save Deck there. Multiple files keep the original
        // immediate-overwrite behavior below unchanged.
        if (files.Length == 1)
        {
            ImportSingleDeckAsPending(owner, slot, files[0]);
            return;
        }

        // Multiple files fill consecutive slots starting at the one
        // currently selected (#5, #6, #7, ... for three files starting on
        // #5), clipped to however many slots actually exist from here to
        // #32.
        int firstSlot = slot.SlotNumber;
        int available = SlotLayout.SlotCount - firstSlot + 1;
        if (files.Length > available)
        {
            AppMessageBox.Show(owner,
                $"Only {available} slot(s) available starting from #{firstSlot}, but {files.Length} files " +
                $"were selected. Only the first {available} will be imported.",
                "Import Deck", MessageBoxButton.OK, MessageBoxImage.Warning);
            files = files.Take(available).ToArray();
        }

        var targetSlots = Enumerable.Range(firstSlot, files.Length)
            .Select(n => AppContext.State.Slots.First(s => s.SlotNumber == n))
            .ToList();

        // One upfront overwrite confirmation covering every targeted slot,
        // instead of interrupting with a Yes/No popup per file. (files.Length
        // is always >= 2 here - the single-file case returned above.)
        var occupied = targetSlots.Where(s => !s.IsEmpty).ToList();
        if (occupied.Count > 0)
        {
            string message = $"{occupied.Count} of the target slots already have a deck: " +
                  $"{string.Join(", ", occupied.Select(s => $"#{s.SlotNumber} ('{s.Name}')"))}. Overwrite them?";
            var confirm = AppMessageBox.Show(owner, message, "Import Deck",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;
        }

        AppContext.Undo.Snapshot();

        var imported = new List<string>();
        var failed = new List<string>();
        var skippedBySlot = new Dictionary<int, List<YdkSkippedCard>>();

        for (int i = 0; i < files.Length; i++)
        {
            var targetSlot = targetSlots[i];
            string file = files[i];

            Deck deck;
            List<YdkSkippedCard> skipped;
            try
            {
                deck = YDKReader.Parse(file, AppContext.CardDb, out skipped);
            }
            catch (Exception ex)
            {
                failed.Add($"#{targetSlot.SlotNumber} ({Path.GetFileName(file)}): {ex.Message}");
                continue;
            }

            if (deck.main.Count > 60 || deck.extra.Count > 15 || deck.side.Count > 15)
            {
                failed.Add($"#{targetSlot.SlotNumber} ({Path.GetFileName(file)}): deck too large " +
                           $"(Main {deck.main.Count}/60, Extra {deck.extra.Count}/15, Side {deck.side.Count}/15).");
                continue;
            }

            string name = Path.GetFileNameWithoutExtension(file);
            if (name.Length > 32) name = name[..32];

            // Empty slots and slots with no duelist set (OwnerId 0) get a
            // default owner instead of importing with "no duelist" - 138 is
            // just a sensible default portrait/name, not tied to any game
            // logic. A slot that already has a real owner keeps it.
            byte ownerId = (targetSlot.IsEmpty || targetSlot.OwnerId == 0) ? (byte)138 : targetSlot.OwnerId;

            try
            {
                AppContext.SlotIo.WriteSlot(AppContext.State.SaveBytes, targetSlot.SlotNumber, deck, name, ownerId);
            }
            catch (ArgumentException ex)
            {
                failed.Add($"#{targetSlot.SlotNumber} ({Path.GetFileName(file)}): {ex.Message}");
                continue;
            }

            imported.Add($"#{targetSlot.SlotNumber} '{name}'");
            if (skipped.Count > 0) skippedBySlot[targetSlot.SlotNumber] = skipped;
        }

        AppContext.State.Slots = AppContext.SlotIo.ReadAllSlots(AppContext.State.SaveBytes).ToArray();
        SaveDataChanged?.Invoke(this, EventArgs.Empty);

        ShowMultiImportSummary(owner, files.Length, imported, failed, skippedBySlot);
    }

    /// <summary>Single-file import path - parses and validates the .ydk the
    /// same way the multi-file path below does, but never touches
    /// AppContext.State.SaveBytes itself. Instead it raises
    /// SingleDeckImportRequested so MainWindow can hand the parsed deck to
    /// Deck Editor as a pending load (DeckEditorView.LoadPendingDeck) -
    /// nothing is actually written to slot until the user presses Save Deck
    /// there, so there's no overwrite confirmation needed here even if the
    /// slot already has a deck.</summary>
    private void ImportSingleDeckAsPending(Window? owner, SlotIO.SlotInfo slot, string file)
    {
        Deck deck;
        List<YdkSkippedCard> skipped;
        try
        {
            deck = YDKReader.Parse(file, AppContext.CardDb, out skipped);
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(owner, $"Couldn't import that deck:\n{ex.Message}", "Import Deck",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (deck.main.Count > 60 || deck.extra.Count > 15 || deck.side.Count > 15)
        {
            AppMessageBox.Show(owner,
                $"Couldn't import that deck: deck too large (Main {deck.main.Count}/60, Extra {deck.extra.Count}/15, " +
                $"Side {deck.side.Count}/15).",
                "Import Deck", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        string name = Path.GetFileNameWithoutExtension(file);
        if (name.Length > 32) name = name[..32];

        SingleDeckImportRequested?.Invoke(this, new PendingDeckImport(slot, deck, name, skipped));

        if (skipped.Count > 0)
        {
            // List every unresolved passcode back to the user - a stale or
            // foreign-format .ydk (unsupported cards, TCG-only passcodes,
            // typos) would otherwise just silently drop cards with no sign
            // anything was wrong beyond a smaller-than-expected deck.
            string lines = string.Join("\n", skipped.Select(s => $"  {s.Passcode}  ({s.Section})"));
            AppMessageBox.Show(owner,
                $"Opened '{name}' in Deck Editor, but {skipped.Count} card(s) weren't found in the card " +
                $"database and were skipped:\n\n{lines}\n\nNothing is saved to slot #{slot.SlotNumber} until you press Save Deck.",
                "Import Deck", MessageBoxButton.OK, MessageBoxImage.Warning, copyText: lines);
        }
    }

    /// <summary>Reports the outcome of ImportDeckButton_Click's multi-file
    /// path - one combined summary instead of a popup per file. The
    /// single-file path has its own summary handling now (see
    /// ImportSingleDeckAsPending), since it no longer writes anything to a
    /// slot itself.</summary>
    private static void ShowMultiImportSummary(Window? owner, int fileCount, List<string> imported,
        List<string> failed, Dictionary<int, List<YdkSkippedCard>> skippedBySlot)
    {
        var lines2 = new List<string> { $"Imported {imported.Count} of {fileCount} deck(s)." };

        if (imported.Count > 0)
            lines2.Add("Slots: " + string.Join(", ", imported));

        if (failed.Count > 0)
        {
            lines2.Add(string.Empty);
            lines2.Add("Failed:");
            lines2.AddRange(failed.Select(f => $"  {f}"));
        }

        if (skippedBySlot.Count > 0)
        {
            lines2.Add(string.Empty);
            lines2.Add("Some cards weren't found in the card database and were skipped:");
            lines2.AddRange(skippedBySlot.Select(kv => $"  Slot #{kv.Key}: {kv.Value.Count} card(s)"));
        }

        string summary = string.Join("\n", lines2);
        AppMessageBox.Show(owner, summary, "Import Deck", MessageBoxButton.OK,
            failed.Count > 0 ? MessageBoxImage.Warning : MessageBoxImage.Information,
            copyText: failed.Count > 0 || skippedBySlot.Count > 0 ? summary : null);
    }

    private void ExportDeckButton_Click(object sender, RoutedEventArgs e)
    {
        var slot = _selectedSlot;
        var owner = Window.GetWindow(this);
        if (slot == null || AppContext.State?.SaveBytes == null) return;

        if (slot.IsEmpty)
        {
            AppMessageBox.Show(owner, $"Slot #{slot.SlotNumber} is empty — nothing to export.", "Export Deck",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var dlg = new SaveFileDialog
        {
            Title = "Export .ydk",
            Filter = "YDK files (*.ydk)|*.ydk|All files (*.*)|*.*",
            DefaultExt = "ydk",
            FileName = $"{slot.Name}.ydk",
        };
        if (dlg.ShowDialog(owner) != true) return;

        try
        {
            var deck = AppContext.SlotIo.ReadDeck(AppContext.State.SaveBytes, slot, AppContext.CardDb);
            YDKWriter.Write(dlg.FileName, deck);
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(owner, $"Couldn't write that .ydk file:\n{ex.Message}", "Export Deck",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        AppMessageBox.Show(owner, $"Exported slot #{slot.SlotNumber} to {Path.GetFileName(dlg.FileName)}.", "Export Deck",
            MessageBoxButton.OK, MessageBoxImage.Information);
    }

    // ── Change Duelist ───────────────────────────────────────────────────────
    // Ported from the WinForms build's DeckSlotsPage.OnChangeDuelist -
    // OwnerPickerWindow does the actual id lookup/search, this just applies
    // whatever id comes back to the save bytes via SlotIO.UpdateSlotOwner
    // (already existed, just never had a caller). Like Import/Export, no
    // confirmation dialog - the portrait on the tile itself is the feedback.

    private void ChangeDuelistButton_Click(object sender, RoutedEventArgs e)
    {
        var slot = _selectedSlot;
        var owner = Window.GetWindow(this);
        if (slot == null || AppContext.State?.SaveBytes == null) return;

        byte? newOwnerId = OwnerPicker.Show(owner, slot.OwnerId);
        if (newOwnerId is null) return;

        AppContext.Undo.Snapshot();
        AppContext.SlotIo.UpdateSlotOwner(AppContext.State.SaveBytes, slot, newOwnerId.Value);

        AppContext.State.Slots = AppContext.SlotIo.ReadAllSlots(AppContext.State.SaveBytes).ToArray();
        SaveDataChanged?.Invoke(this, EventArgs.Empty);
    }

    // ── Rename ────────────────────────────────────────────────────────────────
    // Ported from the WinForms build's DeckSlotsPage.OnRename - Prompt.Show
    // is now AppInputBox.Show, everything else is identical.

    private void RenameButton_Click(object sender, RoutedEventArgs e)
    {
        var slot = _selectedSlot;
        var owner = Window.GetWindow(this);
        if (slot == null || AppContext.State?.SaveBytes == null) return;

        string? name = AppInputBox.Show(owner, "Rename Deck", "Enter a new name for this deck:", slot.Name);
        if (name is null) return;
        if (name.Length > 32) name = name[..32];

        AppContext.Undo.Snapshot();
        AppContext.SlotIo.UpdateName(AppContext.State.SaveBytes, slot, name);

        AppContext.State.Slots = AppContext.SlotIo.ReadAllSlots(AppContext.State.SaveBytes).ToArray();
        SaveDataChanged?.Invoke(this, EventArgs.Empty);
    }

    // ── Copy To Slot / Swap With Slot ────────────────────────────────────────
    // Ported from the WinForms build's BeginPickSlot/CompletePendingAction -
    // clicking either button arms a pending pick instead of acting
    // immediately; the following click on any slot tile (routed through
    // SlotsList_SelectionChanged above) supplies the destination.
    // CopySlotBlock/SwapSlotBlocks already existed in SlotIO, just never had
    // a caller. PendingActionBanner (declared in the XAML, Collapsed by
    // default) is the persistent "waiting for a click" reminder - the
    // WinForms build only ever explained this via a one-off popup.

    private void CopyToButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedSlot != null) BeginPickSlot(PendingSlotAction.CopyTo, _selectedSlot);
    }

    private void SwapWithButton_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedSlot != null) BeginPickSlot(PendingSlotAction.SwapWith, _selectedSlot);
    }

    private void BeginPickSlot(PendingSlotAction action, SlotIO.SlotInfo source)
    {
        _pendingAction = action;
        _pendingSourceSlot = source;

        string verb = action == PendingSlotAction.CopyTo ? "copy" : "swap";
        PendingActionText.Text =
            $"Click the deck slot you want to {verb} with '{source.Name}' (slot #{source.SlotNumber}). " +
            $"Click slot #{source.SlotNumber} again to cancel.";
        PendingActionBanner.Visibility = Visibility.Visible;
    }

    private void CancelPendingAction()
    {
        _pendingAction = PendingSlotAction.None;
        _pendingSourceSlot = null;
        PendingActionBanner.Visibility = Visibility.Collapsed;
    }

    private void CompletePendingAction(SlotIO.SlotInfo target)
    {
        var action = _pendingAction;
        var source = _pendingSourceSlot!;
        CancelPendingAction();

        if (target.SlotNumber == source.SlotNumber) return; // clicking the source again cancels
        if (AppContext.State?.SaveBytes == null) return;

        var owner = Window.GetWindow(this);

        if (action == PendingSlotAction.CopyTo)
        {
            if (!target.IsEmpty)
            {
                var confirm = AppMessageBox.Show(owner,
                    $"Slot #{target.SlotNumber} already has a deck ('{target.Name}'). Overwrite it with '{source.Name}'?",
                    "Copy to Slot", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirm != MessageBoxResult.Yes) return;
            }

            AppContext.Undo.Snapshot();
            AppContext.SlotIo.CopySlotBlock(AppContext.State.SaveBytes, source.SlotBaseOffset, target.SlotBaseOffset);
        }
        else
        {
            AppContext.Undo.Snapshot();
            AppContext.SlotIo.SwapSlotBlocks(AppContext.State.SaveBytes, source.SlotBaseOffset, target.SlotBaseOffset);
        }

        AppContext.State.Slots = AppContext.SlotIo.ReadAllSlots(AppContext.State.SaveBytes).ToArray();
        SaveDataChanged?.Invoke(this, EventArgs.Empty);
    }

    // ── Clear Slot ───────────────────────────────────────────────────────────
    // AppContext.SlotIo.ClearDeck already existed (zeroes the slot's whole
    // byte block - name, owner, occupancy flag, every card) but had no
    // caller anywhere in the Wpf app, same as UpdateSlotOwner/CopySlotBlock/
    // SwapSlotBlocks before Change Duelist/Copy To/Swap With picked them up.

    private void ClearSlotButton_Click(object sender, RoutedEventArgs e)
    {
        var slot = _selectedSlot;
        var owner = Window.GetWindow(this);
        if (slot == null || AppContext.State?.SaveBytes == null) return;

        var confirm = AppMessageBox.Show(owner,
            $"Clear slot #{slot.SlotNumber} ('{slot.Name}')? This removes its name, duelist, and every card.",
            "Clear Slot", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        if (confirm != MessageBoxResult.Yes) return;

        AppContext.Undo.Snapshot();
        AppContext.SlotIo.ClearDeck(AppContext.State.SaveBytes, slot.SlotNumber);

        AppContext.State.Slots = AppContext.SlotIo.ReadAllSlots(AppContext.State.SaveBytes).ToArray();
        SaveDataChanged?.Invoke(this, EventArgs.Empty);
    }

    // ── Export All Decks ─────────────────────────────────────────────────────
    // New, not in the WinForms build - a bulk version of Export Deck that
    // walks every non-empty slot into one chosen folder instead of one .ydk
    // at a time. OpenFolderDialog is the .NET 8 WPF folder picker (same
    // Microsoft.Win32 namespace as OpenFileDialog/SaveFileDialog above, no
    // new dependency needed).

    private void ExportAllButton_Click(object sender, RoutedEventArgs e)
    {
        var owner = Window.GetWindow(this);
        if (AppContext.State?.SaveBytes == null) return;

        var nonEmpty = AppContext.State.Slots.Where(s => !s.IsEmpty).ToList();
        if (nonEmpty.Count == 0)
        {
            AppMessageBox.Show(owner, "There are no decks to export - every slot is empty.", "Export All Decks",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var dlg = new OpenFolderDialog { Title = "Choose a folder to export all decks into" };
        if (dlg.ShowDialog(owner) != true) return;

        int exported = 0;
        var failures = new List<string>();
        foreach (var slot in nonEmpty)
        {
            try
            {
                var deck = AppContext.SlotIo.ReadDeck(AppContext.State.SaveBytes, slot, AppContext.CardDb);
                string path = Path.Combine(dlg.FolderName, $"{slot.SlotNumber:D2} - {SanitizeFileName(slot.Name)}.ydk");
                YDKWriter.Write(path, deck);
                exported++;
            }
            catch (Exception ex)
            {
                failures.Add($"Slot #{slot.SlotNumber}: {ex.Message}");
            }
        }

        string message = $"Exported {exported} of {nonEmpty.Count} deck(s) to {dlg.FolderName}.";
        if (failures.Count > 0)
        {
            string failureText = string.Join("\n", failures);
            AppMessageBox.Show(owner, $"{message}\n\n{failures.Count} failed:\n{failureText}", "Export All Decks",
                MessageBoxButton.OK, MessageBoxImage.Warning, copyText: failureText);
        }
        else
        {
            AppMessageBox.Show(owner, message, "Export All Decks", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private static string SanitizeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return string.IsNullOrWhiteSpace(name) ? "Deck" : name;
    }
}
