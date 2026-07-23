using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// Maps a CardTileViewModel.BanStatus string ("Forbidden"/"Limited"/
/// "Semi-Limited", or null) to the matching corner-badge icon under
/// Assets/icons/ (added alongside Undo.png/Redo.png as plain build
/// Resources, so a pack URI works the same way). Returns null for an
/// unrestricted card - bound directly to an Image.Source with no
/// Visibility plumbing needed, since a null Source just renders nothing.
/// </summary>
public sealed class BanIconConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string? fileName = value as string switch
        {
            "Forbidden" => "forbidden.png",
            "Limited" => "limited.png",
            "Semi-Limited" => "semi-limited.png",
            _ => null,
        };

        return fileName == null
            ? null
            : new BitmapImage(new Uri($"pack://application:,,,/Assets/icons/{fileName}"));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
