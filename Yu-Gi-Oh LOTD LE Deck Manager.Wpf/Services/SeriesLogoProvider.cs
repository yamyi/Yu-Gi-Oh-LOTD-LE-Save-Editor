using System.Windows.Media.Imaging;

namespace YuGiOhDeckManager.Wpf.Services;

/// <summary>
/// Maps a campaign series to its logo file in Assets/icons (added directly
/// to that folder, which is already wildcard-included as WPF Resource
/// items in the .csproj - see the Undo/Redo icon comment there). Loaded via
/// a pack URI, same as any other Resource-build-action image, rather than
/// GetManifestResourceStream (that pattern is only used for the
/// EmbeddedResource character/card assets in PortraitProvider/CardImageProvider).
/// Currently unused: SaveEditorView's Campaign tab ended up using each
/// series' logo as a static XAML Image on its own selector button (six
/// fixed pack-URI Images, one per button) instead of one Image whose
/// Source gets swapped from code-behind - simpler once the selector became
/// "click a logo" instead of a ComboBox. Left here in case a future view
/// wants a single dynamically-swapped series logo.
/// </summary>
public static class SeriesLogoProvider
{
    private static readonly Dictionary<LotdDuelSeries, string> FileNames = new()
    {
        [LotdDuelSeries.YuGiOh] = "DM.png",
        [LotdDuelSeries.YuGiOhGX] = "GX.png",
        [LotdDuelSeries.YuGiOh5D] = "5DS.png",
        [LotdDuelSeries.YuGiOhZEXAL] = "ZEXAL.png",
        [LotdDuelSeries.YuGiOhARCV] = "ARC-V.png",
        [LotdDuelSeries.YuGiOhVRAINS] = "VRAINS.png",
    };

    public static BitmapImage? GetLogo(LotdDuelSeries series)
    {
        if (!FileNames.TryGetValue(series, out var file)) return null;

        var bmp = new BitmapImage();
        bmp.BeginInit();
        bmp.UriSource = new Uri($"pack://application:,,,/Assets/icons/{file}", UriKind.Absolute);
        bmp.CacheOption = BitmapCacheOption.OnLoad;
        bmp.EndInit();
        bmp.Freeze();
        return bmp;
    }
}
