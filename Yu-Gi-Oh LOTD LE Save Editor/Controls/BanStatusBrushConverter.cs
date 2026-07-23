using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// Maps CardPreviewFormatter.BanStatus's text ("Forbidden"/"Limited"/
/// "Semi-Limited"/"Unlimited") to a theme brush for the preview panel's
/// PreviewBanStatus TextBlock - reuses the same status brushes
/// SlotLegalityBrushConverter already uses for the deck-slot legality dot
/// (StatusIllegalBrush/StatusWarningBrush/StatusOkBrush), plus
/// TextPrimaryBrush (near-white) for the common Unlimited case, so an
/// unrestricted card doesn't read as a warning.
/// </summary>
public sealed class BanStatusBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string key = value as string switch
        {
            "Forbidden" => "StatusIllegalBrush",
            "Limited" => "StatusWarningBrush",
            "Semi-Limited" => "StatusOkBrush",
            _ => "TextPrimaryBrush",
        };
        return (Brush)Application.Current.Resources[key];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
