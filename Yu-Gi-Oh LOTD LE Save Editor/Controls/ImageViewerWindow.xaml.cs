using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// The actual window behind ImageViewer.Show - everything on screen
/// (Surface, ViewerImage + its ImageScale/ImageTranslate transforms,
/// TitleText, CloseButton) is declared once in ImageViewerWindow.xaml; this
/// class only ever adjusts properties on those already-existing elements
/// (Source, Text, ScaleX/ScaleY, X/Y) - no controls built here, same
/// pattern as AppMessageBoxWindow.
/// </summary>
public partial class ImageViewerWindow : Window
{
    private const double MinScale = 0.5;
    private const double MaxScale = 6.0;
    private const double ZoomStep = 0.15;

    private bool _isDragging;
    private Point _dragStart;

    public ImageViewerWindow(ImageSource? image, string title)
    {
        InitializeComponent();
        ViewerImage.Source = image;
        TitleText.Text = title;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => CloseButton.Focus();

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape) Close();
    }

    /// <summary>Mouse-wheel zoom, centered on the image (RenderTransformOrigin
    /// is 0.5,0.5 in the XAML) rather than the cursor position - simpler than
    /// cursor-anchored zoom math and still lets the user pan afterward to
    /// bring any part of a zoomed-in card into view.</summary>
    private void Surface_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        double factor = e.Delta > 0 ? 1 + ZoomStep : 1 - ZoomStep;
        double newScale = Math.Clamp(ImageScale.ScaleX * factor, MinScale, MaxScale);
        ImageScale.ScaleX = newScale;
        ImageScale.ScaleY = newScale;
    }

    private void Surface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        // Grid (like Image) isn't a Control, so it has no MouseDoubleClick
        // event of its own - a double-click is handled right here instead
        // of via a separate handler.
        if (e.ClickCount == 2)
        {
            ResetView();
            return;
        }

        _isDragging = true;
        _dragStart = e.GetPosition(Surface);
        Surface.CaptureMouse();
    }

    private void Surface_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging) return;

        var pos = e.GetPosition(Surface);
        ImageTranslate.X += pos.X - _dragStart.X;
        ImageTranslate.Y += pos.Y - _dragStart.Y;
        _dragStart = pos;
    }

    private void Surface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isDragging = false;
        Surface.ReleaseMouseCapture();
    }

    private void Surface_MouseRightButtonDown(object sender, MouseButtonEventArgs e) => Close();

    /// <summary>Resets zoom and pan back to the fitted starting view - a
    /// quick "un-stick" if the user has zoomed/panned somewhere awkward.</summary>
    private void ResetView()
    {
        ImageScale.ScaleX = 1;
        ImageScale.ScaleY = 1;
        ImageTranslate.X = 0;
        ImageTranslate.Y = 0;
    }
}
