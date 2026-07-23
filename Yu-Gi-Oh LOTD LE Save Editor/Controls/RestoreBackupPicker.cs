using System.Windows;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// Opens RestoreBackupWindow (the "Restore Backup" modal) sized to cover the
/// owner window's own bounds, same convention OwnerPicker.Show and
/// AppMessageBox.Show both follow. Pass the currently open save's full path -
/// see Services.SaveBackup for the naming convention this looks for.
/// </summary>
public static class RestoreBackupPicker
{
    public static string? Show(Window? owner, string savePath)
    {
        var win = new RestoreBackupWindow(savePath);

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
        return win.SelectedBackupPath;
    }
}
