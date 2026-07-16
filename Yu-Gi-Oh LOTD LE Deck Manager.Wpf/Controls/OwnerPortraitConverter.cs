using System.Globalization;
using System.Windows.Data;
using YuGiOhDeckManager.Wpf.Services;

namespace YuGiOhDeckManager.Wpf.Controls;

/// <summary>
/// Binds a slot's OwnerId (byte) straight to its portrait BitmapImage via
/// PortraitProvider. Declared once as a XAML resource (see
/// DeckSlotsView.xaml) and reused by every tile's Image.Source binding —
/// no per-tile code-behind needed.
/// </summary>
public sealed class OwnerPortraitConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is byte ownerId ? PortraitProvider.GetPortrait(ownerId) : null;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
