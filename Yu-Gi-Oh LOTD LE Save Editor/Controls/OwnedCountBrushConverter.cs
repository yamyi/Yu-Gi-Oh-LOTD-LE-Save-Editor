using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// Maps CardPreviewFormatter.OwnedCount's text ("Owned: N / 3") to a status
/// brush for the preview panel's Owned pill - same red/amber/green language
/// the deck-slot legality dot already uses, so "fully owned" reads as an
/// unambiguous green badge and "not owned at all" reads as a dim/empty one
/// instead of just more plain grey text sitting next to everything else.
/// </summary>
public sealed class OwnedCountBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string key = ParseOwned(value as string) switch
        {
            0 => "StatusEmptyBrush",
            3 => "StatusOkBrush",
            _ => "StatusWarningBrush",
        };
        return (Brush)Application.Current.Resources[key];
    }

    /// <summary>Pulls the "N" out of "Owned: N / 3" - returns -1 (falls
    /// through to the amber "partial" case, same as any unexpected format)
    /// if the text doesn't look like that at all, e.g. while it's still
    /// empty/uninitialized before a save is loaded.</summary>
    private static int ParseOwned(string? text)
    {
        if (string.IsNullOrEmpty(text)) return -1;
        var parts = text.Split(':', '/');
        return parts.Length >= 2 && int.TryParse(parts[1], out int n) ? n : -1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
