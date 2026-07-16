using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace YuGiOhDeckManager.Wpf.Controls;

/// <summary>
/// True -&gt; Collapsed, False -&gt; Visible — the inverse of the built-in
/// BooleanToVisibilityConverter. Used to hide a slot tile's deck-info block
/// when SlotInfo.IsEmpty is true (the built-in converter handles the
/// opposite case: showing the "Empty" placeholder).
/// </summary>
public sealed class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is true ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
