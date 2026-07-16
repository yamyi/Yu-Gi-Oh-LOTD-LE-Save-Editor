using System.Windows;
using System.Windows.Media;

namespace YuGiOhDeckManager.Wpf.Controls;

/// <summary>
/// Opens ImageViewerWindow (zoom/pan card-art viewer) sized to cover the
/// entire screen, not just the app's own window - unlike AppMessageBox's
/// "dimmed backdrop only over this window" convention, the artwork viewer
/// is meant to be a full-bleed, distraction-free view, so it's sized to
/// SystemParameters.PrimaryScreen* instead of the owner window's bounds.
/// Both preview panels' PreviewImage double-click handlers call this
/// directly with whatever ImageSource is already loaded in the preview -
/// no extra download, just a bigger, zoomable view of the same artwork.
/// </summary>
public static class ImageViewer
{
    public static void Show(Window? owner, ImageSource? image, string title)
    {
        if (image == null) return;

        var win = new ImageViewerWindow(image, title)
        {
            WindowStartupLocation = WindowStartupLocation.Manual,
            Left = 0,
            Top = 0,
            Width = SystemParameters.PrimaryScreenWidth,
            Height = SystemParameters.PrimaryScreenHeight
        };

        if (owner != null && owner.IsLoaded)
            win.Owner = owner;

        win.ShowDialog();
    }
}
