using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using YuGiOhSaveEditor.Wpf.Controls;
using YuGiOhSaveEditor.Wpf.Services;

namespace YuGiOhSaveEditor.Wpf.Views;

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
    /// build's equivalent was DeckSlotsPage.SlotDataChanged.</summary>
    public event EventHandler? SaveDataChanged;

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
            Multiselect = false,
        };
        if (dlg.ShowDialog(owner) != true) return;

        Deck deck;
        List<YdkSkippedCard> skipped;
        try
        {
            deck = YDKReader.Parse(dlg.FileName, AppContext.CardDb, out skipped);
        }
        catch (Exception ex)
        {
            AppMessageBox.Show(owner, $"Couldn't read that .ydk file:\n{ex.Message}", "Import Deck",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (deck.main.Count > 60 || deck.extra.Count > 15 || deck.side.Count > 15)
        {
            AppMessageBox.Show(owner,
                $"That deck is too large (Main {deck.main.Count}/60, Extra {deck.extra.Count}/15, Side {deck.side.Count}/15).",
                "Import Deck", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!slot.IsEmpty)
        {
            var confirm = AppMessageBox.Show(owner,
                $"Slot #{slot.SlotNumber} already has a deck ('{slot.Name}'). Overwrite it?",
                "Import Deck", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm != MessageBoxResult.Yes) return;
        }

        string name = Path.GetFileNameWithoutExtension(dlg.FileName);
        if (name.Length > 32) name = name[..32];

        AppContext.Undo.Snapshot();
        try
        {
            AppContext.SlotIo.WriteSlot(AppContext.State.SaveBytes, slot.SlotNumber, deck, name, slot.OwnerId);
        }
        catch (ArgumentException ex)
        {
            AppMessageBox.Show(owner, $"Couldn't write that deck to the slot:\n{ex.Message}", "Import Deck",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        AppContext.State.Slots = AppContext.SlotIo.ReadAllSlots(AppContext.State.SaveBytes).ToArray();
        SaveDataChanged?.Invoke(this, EventArgs.Empty);

        if (skipped.Count > 0)
        {
            // List every unresolved passcode back to the user - a stale or
            // foreign-format .ydk (unsupported cards, TCG-only passcodes,
            // typos) would otherwise just silently drop cards with no sign
            // anything was wrong beyond a smaller-than-expected deck.
            string lines = string.Join("\n", skipped.Select(s => $"  {s.Passcode}  ({s.Section})"));
            AppMessageBox.Show(owner,
                $"Imported '{name}' into slot #{slot.SlotNumber}, but {skipped.Count} card(s) " +
                $"weren't found in the card database and were skipped:\n\n{lines}",
                "Import Deck", MessageBoxButton.OK, MessageBoxImage.Warning, copyText: lines);
        }
        else
        {
            AppMessageBox.Show(owner, $"Imported '{name}' into slot #{slot.SlotNumber}.", "Import Deck",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
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
