using System.Windows;

namespace YuGiOhDeckManager.Wpf.Controls;

/// <summary>
/// Drop-in replacement for System.Windows.MessageBox.Show, styled to match
/// Master Duel's own confirm/alert dialogs (rounded dark card, icon badge +
/// title, framed message, slanted action buttons) instead of the native
/// Windows message box. Signature-compatible with the calls this app already
/// made against MessageBox.Show, so every call site only needed
/// "MessageBox.Show" swapped for "AppMessageBox.Show".
/// </summary>
public static class AppMessageBox
{
    public static MessageBoxResult Show(Window? owner, string message, string title,
        MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None,
        string? copyText = null)
    {
        var dlg = new AppMessageBoxWindow(message, title, button, icon, copyText);

        if (owner != null && owner.IsLoaded)
        {
            // Sized to the owner's own bounds (not the whole screen) so the
            // dimmed backdrop only covers this app's window, then the dialog
            // card centers itself within that via the XAML's
            // HorizontalAlignment/VerticalAlignment=Center.
            dlg.Owner = owner;
            dlg.WindowStartupLocation = WindowStartupLocation.Manual;
            dlg.Left = owner.Left;
            dlg.Top = owner.Top;
            dlg.Width = owner.ActualWidth;
            dlg.Height = owner.ActualHeight;
        }
        else
        {
            dlg.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        dlg.ShowDialog();
        return dlg.Result;
    }
}
