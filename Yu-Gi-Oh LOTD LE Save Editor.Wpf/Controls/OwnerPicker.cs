using System.Windows;

namespace YuGiOhSaveEditor.Wpf.Controls;

/// <summary>
/// Opens OwnerPickerWindow (the "Change Duelist" modal) sized to cover the
/// owner window's own bounds, same convention AppMessageBox.Show and
/// ImageViewer.Show both follow. Ports the old WinForms build's
/// OwnerPickerForm.Show(byte) - same "pass the current owner id, get back
/// the newly picked one or null if cancelled" signature.
/// </summary>
public static class OwnerPicker
{
    public static byte? Show(Window? owner, byte currentOwnerId)
    {
        var win = new OwnerPickerWindow(currentOwnerId);

        if (owner != null && owner.IsLoaded)
        {
            win.Owner = owner;
            win.WindowStartupLocation = WindowStartupLocation.Manual;
            win.Left = owner.Left;
            win.Top = owner.Top;
            win.Width = owner.ActualWidth;
            win.Height = owner.ActualHeight;
        }
        else
        {
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        win.ShowDialog();
        return win.SelectedOwnerId;
    }
}
