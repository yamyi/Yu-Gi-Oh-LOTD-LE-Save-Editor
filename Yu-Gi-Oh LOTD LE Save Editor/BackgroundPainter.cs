namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager;

/// <summary>
/// Draws the active theme's background image (cover-fit, tinted by
/// BackgroundOverlayOpacity) behind a control, or does nothing — leaving
/// whatever solid AppColors.X the control was already painted with — when
/// no image is set. Hook it up to a control's public Paint event (works on
/// any stock Panel/UserControl/Form straight from a Designer.cs, no
/// subclassing needed): <c>somePanel.Paint += BackgroundPainter.Paint;</c>
///
/// Every hooked control draws its own slice of ONE continuous image, sized
/// to cover the top-level Form's client area and positioned by each
/// control's offset from that form's origin — so the wallpaper lines up
/// seamlessly across panels/pages instead of each one stretching its own
/// independent copy.
/// </summary>
public static class BackgroundPainter
{
    private static Image? _cachedImage;
    private static string? _cachedPath;

    public static void Paint(object? sender, PaintEventArgs e)
    {
        if (sender is not Control target) return;

        var theme = ThemeManager.Current;
        if (string.IsNullOrWhiteSpace(theme.BackgroundImagePath)) return;
        if (!TryGetImage(theme.BackgroundImagePath, out var img)) return;

        var g = e.Graphics;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        var form = target.FindForm();
        Size referenceSize = form?.ClientSize ?? target.ClientSize;
        if (referenceSize.Width <= 0 || referenceSize.Height <= 0) return;

        double scale = Math.Max(
            (double)referenceSize.Width / img.Width,
            (double)referenceSize.Height / img.Height);
        int w = (int)(img.Width * scale);
        int h = (int)(img.Height * scale);
        int imgX = (referenceSize.Width - w) / 2;
        int imgY = (referenceSize.Height - h) / 2;

        Point offset = Point.Empty;
        if (form is not null && !ReferenceEquals(form, target) && target.IsHandleCreated && form.IsHandleCreated)
        {
            var targetScreen = target.PointToScreen(Point.Empty);
            var formScreen = form.PointToScreen(Point.Empty);
            offset = new Point(targetScreen.X - formScreen.X, targetScreen.Y - formScreen.Y);
        }

        g.DrawImage(img, new Rectangle(imgX - offset.X, imgY - offset.Y, w, h));

        // Tint with the control's own BackColor so it keeps its role's
        // identity (a card-toned panel stays card-toned, the app root stays
        // AppBg-toned) even with an image showing through it.
        int alpha = (int)(Math.Clamp(theme.BackgroundOverlayOpacity, 0, 1) * 255);
        if (alpha > 0)
        {
            using var overlay = new SolidBrush(Color.FromArgb(alpha, target.BackColor));
            g.FillRectangle(overlay, target.ClientRectangle);
        }
    }

    private static bool TryGetImage(string path, out Image image)
    {
        if (_cachedPath != path)
        {
            _cachedImage?.Dispose();
            _cachedImage = null;
            _cachedPath = path;
            try
            {
                if (File.Exists(path))
                    _cachedImage = Image.FromFile(path);
            }
            catch
            {
                // Missing/corrupt/locked file — fall back to solid colors.
                _cachedImage = null;
            }
        }

        image = _cachedImage!;
        return _cachedImage is not null;
    }
}
