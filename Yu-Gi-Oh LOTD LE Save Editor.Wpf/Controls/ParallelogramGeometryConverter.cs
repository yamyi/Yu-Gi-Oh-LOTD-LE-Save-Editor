using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace YuGiOhSaveEditor.Wpf.Controls;

/// <summary>
/// Builds the diagonal-cut parallelogram shape used by Duel Links' big
/// full-width menu-list buttons (Duel Studio, Help/Etc.) from a control's
/// live ActualWidth/ActualHeight — bound via a MultiBinding in
/// Themes/Controls.xaml's SlantedButtonStyle so the shape always matches the
/// button's real size instead of being hardcoded.
/// </summary>
public sealed class ParallelogramGeometryConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 || values[0] is not double w || values[1] is not double h || w <= 0 || h <= 0)
            return Geometry.Empty;

        double cut = Math.Min(h / 2.0, 14.0);

        var figure = new PathFigure { StartPoint = new System.Windows.Point(cut, 0), IsClosed = true };
        figure.Segments.Add(new LineSegment(new System.Windows.Point(w, 0), true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(w - cut, h), true));
        figure.Segments.Add(new LineSegment(new System.Windows.Point(0, h), true));

        var geometry = new PathGeometry();
        geometry.Figures.Add(figure);
        geometry.Freeze();
        return geometry;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
