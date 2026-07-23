using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// Maps a slot's deck legality (see DeckLegalityChecker) to one of the
/// theme's status brushes for the small legality dot on each tile - deck
/// size (Main &lt; 40 = warning, Main &gt; 60 or Extra/Side &gt; 15 =
/// illegal) plus banlist violations (a Forbidden card, or too many
/// Limited/Semi-Limited copies) now both feed into the same dot.
/// </summary>
public sealed class SlotLegalityBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SlotIO.SlotInfo slot)
            return Brushes.Transparent;

        string key = DeckLegalityChecker.GetStatus(slot) switch
        {
            SlotLegality.Empty => "StatusEmptyBrush",
            SlotLegality.Illegal => "StatusIllegalBrush",
            SlotLegality.Warning => "StatusWarningBrush",
            _ => "StatusOkBrush",
        };

        return (Brush)Application.Current.Resources[key];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
