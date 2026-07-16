using System.Windows;

namespace YuGiOhDeckManager.Wpf.Controls;

/// <summary>
/// Drop-in replacement for the WinForms build's ad-hoc Prompt.Show(title,
/// prompt, defaultValue) - same signature, same "returns the typed text, or
/// null if cancelled" contract, styled like every other modal in this app
/// (AppMessageBox.Show / OwnerPicker.Show) instead of a raw WinForms Form.
/// </summary>
public static class AppInputBox
{
    public static string? Show(Window? owner, string title, string prompt, string defaultValue = "")
    {
        var win = new AppInputBoxWindow(title, prompt, defaultValue);

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
        return win.Result;
    }
}
