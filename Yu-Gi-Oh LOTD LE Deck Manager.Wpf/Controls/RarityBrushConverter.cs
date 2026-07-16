using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace YuGiOhDeckManager.Wpf.Controls;

/// <summary>Maps a "N"/"R"/"SR"/"UR" rarity tier string (see
/// DeckEditorHelpers.GetRarityTier) to the matching theme brush.</summary>
public sealed class RarityBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string key = value as string switch
        {
            "R" => "RarityRBrush",
            "SR" => "RaritySRBrush",
            "UR" => "RarityURBrush",
            _ => "RarityNBrush",
        };
        return (Brush)Application.Current.Resources[key];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
