using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// The actual dialog behind OwnerPicker.Show - everything on screen
/// (SearchBox, OwnersList, OkButton, CancelButton) is declared once in
/// OwnerPickerWindow.xaml; this class only ever repopulates OwnersList's
/// ItemsSource (a plain ObservableCollection&lt;OwnerListItem&gt;, the same
/// "data objects into ItemsSource" category as every other list in this
/// app) and reads back whichever row got selected - no controls are built
/// here.
/// </summary>
public partial class OwnerPickerWindow : Window
{
    /// <summary>Set once the dialog closes via SELECT or a double-click;
    /// stays null if the user cancels or hits Escape.</summary>
    public byte? SelectedOwnerId { get; private set; }

    private readonly List<OwnerListItem> _allOwners;
    private readonly ObservableCollection<OwnerListItem> _filtered = new();

    public OwnerPickerWindow(byte currentOwnerId)
    {
        InitializeComponent();

        // Only offer avatars the player has actually unlocked in this save -
        // ids >= NumAvatarSlots aren't real avatars at all (see
        // LotdSaveFormat.NumAvatarSlots's doc comment: chardata.bin only
        // defines ids 0-152), and of those, MiscSaveLayout.GetAvatarUnlocked
        // reads the same UnlockedAvatars bitfield the Avatars tab itself
        // manages. Falls back to the unfiltered list if there's somehow no
        // save loaded (shouldn't happen - this dialog only opens from an
        // already-loaded deck slot) so the picker never ends up empty.
        var save = AppContext.State.SaveBytes;
        var version = AppContext.State.Version;

        _allOwners = OwnerDatabase.All
            .Where(kv => save == null
                || (kv.Key < LotdSaveFormat.NumAvatarSlots && MiscSaveLayout.GetAvatarUnlocked(save, version, kv.Key)))
            .Select(kv => new OwnerListItem(kv.Key, kv.Value))
            .OrderBy(o => o.Name)
            .ToList();

        OwnersList.ItemsSource = _filtered;
        ApplyFilter();
        SelectCurrent(currentOwnerId);
    }

    private void ApplyFilter()
    {
        string q = SearchBox.Text?.Trim() ?? string.Empty;
        IEnumerable<OwnerListItem> matches = string.IsNullOrEmpty(q)
            ? _allOwners
            : _allOwners.Where(o => o.Name.Contains(q, StringComparison.OrdinalIgnoreCase));

        _filtered.Clear();
        foreach (var o in matches) _filtered.Add(o);
    }

    private void SelectCurrent(byte currentOwnerId)
    {
        var match = _filtered.FirstOrDefault(o => o.Id == currentOwnerId);
        if (match == null) return;

        OwnersList.SelectedItem = match;
        OwnersList.ScrollIntoView(match);
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

    private void OwnersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        OkButton.IsEnabled = OwnersList.SelectedItem != null;

        if (OwnersList.SelectedItem is OwnerListItem item)
        {
            PreviewImage.Source = PortraitProvider.GetPortrait(item.Id);
            PreviewNameText.Text = item.Name;
            PreviewIdText.Text = $"ID: {item.Id}";
        }
        else
        {
            PreviewImage.Source = null;
            PreviewNameText.Text = "Select a duelist";
            PreviewIdText.Text = string.Empty;
        }
    }

    private void OwnersList_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (OwnersList.SelectedItem != null) Confirm();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e) => Confirm();

    private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

    private void Confirm()
    {
        if (OwnersList.SelectedItem is OwnerListItem item)
            SelectedOwnerId = item.Id;
        Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => SearchBox.Focus();

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Close();
        }
        else if (e.Key == Key.Enter && OwnersList.SelectedItem != null)
        {
            Confirm();
        }
    }

    private sealed record OwnerListItem(byte Id, string Name);
}
