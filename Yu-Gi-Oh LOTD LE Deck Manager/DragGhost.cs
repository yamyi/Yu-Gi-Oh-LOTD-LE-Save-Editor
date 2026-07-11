namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager;

/// <summary>
/// Wraps Control.DoDragDrop with a small floating "ghost" thumbnail of the
/// control being dragged, so it visually follows the cursor instead of just
/// showing the OS's plain drag icon. Still uses real OLE drag-and-drop
/// underneath (DoDragDrop / DragEnter / DragDrop keep working exactly as
/// before on the source/target controls) — this only adds the visual.
///
/// Usage: replace <c>DoDragDrop(this, DragDropEffects.Move)</c> with
/// <c>DragGhost.Begin(this, DragDropEffects.Move, this)</c>.
/// </summary>
public static class DragGhost
{
    private static GhostForm? _ghost;

    public static DragDropEffects Begin(Control source, DragDropEffects allowedEffects, object payload)
    {
        Bitmap? preview = null;
        try
        {
            if (source.Width > 0 && source.Height > 0)
            {
                preview = new Bitmap(source.Width, source.Height);
                source.DrawToBitmap(preview, new Rectangle(Point.Empty, source.Size));
            }
        }
        catch
        {
            preview = null; // fall back to no thumbnail — DoDragDrop still works fine
        }

        _ghost = new GhostForm
        {
            Size = source.Size,
            Opacity = 0.75,
            BackgroundImage = preview,
            BackgroundImageLayout = ImageLayout.Stretch,
        };
        PositionGhost();
        _ghost.Show();

        source.GiveFeedback += Source_GiveFeedback;
        try
        {
            return source.DoDragDrop(payload, allowedEffects);
        }
        finally
        {
            source.GiveFeedback -= Source_GiveFeedback;
            _ghost.Close();
            _ghost.Dispose();
            _ghost = null;
            preview?.Dispose();
        }
    }

    private static void Source_GiveFeedback(object? sender, GiveFeedbackEventArgs e) => PositionGhost();

    // Offset down-right of the actual cursor hotspot (rather than centered
    // on it) so the ghost window itself never sits directly under the
    // cursor — OLE drag-drop resolves the drop target from whatever window
    // is under the cursor, and a ghost centered there would shadow the
    // real CardSlot/DeckCard underneath.
    private static void PositionGhost()
    {
        if (_ghost is null) return;
        var pos = Cursor.Position;
        _ghost.Location = new Point(pos.X + 18, pos.Y + 18);
    }

    /// <summary>Borderless, non-activating, taskbar-free popup used purely
    /// as a visual overlay during a drag — never takes focus or input.</summary>
    private sealed class GhostForm : Form
    {
        public GhostForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            TopMost = true;
        }

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_NOACTIVATE = 0x08000000;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;
                return cp;
            }
        }
    }
}
