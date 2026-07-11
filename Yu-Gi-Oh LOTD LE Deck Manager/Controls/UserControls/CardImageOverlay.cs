namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

/// <summary>
/// In-window "lightbox" for viewing card art at full size. Add one instance
/// to a page's Controls (Dock = Fill, added last so it lands on top), leave
/// it hidden, and call ShowImage(...) when the user wants to zoom in —
/// instead of opening a separate OS window. Scroll the mouse wheel to zoom;
/// hold the left mouse button and drag to pan around; click the image (without
/// dragging), the × button, or press Escape to dismiss.
/// All control creation lives in CardImageOverlay.Designer.cs.
/// </summary>
public sealed partial class CardImageOverlay : UserControl
{
    private Image? _image;
    private double _zoom = 1.0;
    private const double MinZoom = 0.5;
    private const double MaxZoom = 4.0;
    private const double ZoomStep = 1.1;

    // Pan state. _panOffset accumulates drag movement and is applied on top
    // of the centered image position in OnPaint. _mouseDownAt records where
    // the current press started (fixed for the whole gesture) so MouseUp can
    // tell a genuine drag apart from a plain click-to-close; _panAnchor is
    // updated every MouseMove to compute the incremental delta.
    private bool _isPanning;
    private Point _panOffset;
    private Point _panAnchor;
    private Point _mouseDownAt;
    private const int ClickDragThreshold = 4;

    public CardImageOverlay()
    {
        InitializeComponent();
    }

    /// <summary>Shows the overlay with the given image, on top of whatever
    /// else is on the hosting page.</summary>
    public void ShowImage(Image image, string title)
    {
        _image = image;
        _zoom = 1.0;
        _panOffset = Point.Empty;
        _isPanning = false;
        lblTitle.Text = title;

        Visible = true;
        BringToFront();
        Focus();
        Invalidate();
    }

    public void CloseOverlay()
    {
        Visible = false;
        _image = null;
        _isPanning = false;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (_image is null) return;

        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        int areaTop = pnlBar.Height;
        int areaHeight = Math.Max(0, ClientSize.Height - areaTop);

        int w = (int)(_image.Width * _zoom);
        int h = (int)(_image.Height * _zoom);
        int x = (ClientSize.Width - w) / 2 + _panOffset.X;
        int y = areaTop + (areaHeight - h) / 2 + _panOffset.Y;

        g.DrawImage(_image, new Rectangle(x, y, w, h));
    }

    private void CardImageOverlay_MouseWheel(object? sender, MouseEventArgs e)
    {
        if (_image is null) return;

        double factor = e.Delta > 0 ? ZoomStep : 1 / ZoomStep;
        _zoom = Math.Clamp(_zoom * factor, MinZoom, MaxZoom);
        Invalidate();
    }

    private void CardImageOverlay_MouseDown(object? sender, MouseEventArgs e)
    {
        if (_image is null || e.Button != MouseButtons.Left) return;

        _isPanning = true;
        _mouseDownAt = e.Location;
        _panAnchor = e.Location;
        Cursor = Cursors.SizeAll;
    }

    private void CardImageOverlay_MouseMove(object? sender, MouseEventArgs e)
    {
        if (!_isPanning) return;

        _panOffset.X += e.X - _panAnchor.X;
        _panOffset.Y += e.Y - _panAnchor.Y;
        _panAnchor = e.Location;
        Invalidate();
    }

    // A press-and-release that never moved beyond the click threshold closes
    // the overlay, same as before; anything that moved further is treated as
    // a completed pan and leaves the overlay open.
    private void CardImageOverlay_MouseUp(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        _isPanning = false;
        Cursor = Cursors.Default;

        int dx = e.X - _mouseDownAt.X;
        int dy = e.Y - _mouseDownAt.Y;
        if (Math.Abs(dx) <= ClickDragThreshold && Math.Abs(dy) <= ClickDragThreshold)
            CloseOverlay();
    }

    private void CardImageOverlay_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Escape) CloseOverlay();
    }

    private void btnClose_Click(object? sender, EventArgs e) => CloseOverlay();
}
