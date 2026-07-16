
internal static class GraphicsExtensions
{
    public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, int radius)
    {
        using var path = RoundedPath(rect, radius);
        g.FillPath(brush, path);
    }

    public static void DrawRoundedRectangle(this Graphics g, Pen pen, Rectangle rect, int radius)
    {
        using var path = RoundedPath(rect, radius);
        g.DrawPath(pen, path);
    }

    private static System.Drawing.Drawing2D.GraphicsPath RoundedPath(Rectangle r, int radius)
    {
        int d = radius * 2;
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    public static Rectangle Deflate(this Rectangle r, int x, int y) =>
        new(r.X + x, r.Y + y, r.Width - x * 2, r.Height - y * 2);


}