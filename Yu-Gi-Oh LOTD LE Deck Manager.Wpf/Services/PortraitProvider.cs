using System.Reflection;
using System.Windows.Media.Imaging;

namespace YuGiOhDeckManager.Wpf.Services
{
    /// <summary>Returns BitmapImage (WPF's native imaging type) instead of the
    /// WinForms build's System.Drawing.Image — bind straight to an
    /// Image.Source.</summary>
    public static class PortraitProvider
    {
        public static BitmapImage? GetPortrait(byte ownerId)
        {
            string resourceName = $"YuGiOhDeckManager.Wpf.Assets.characters.{ownerId}.png";
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream is null) return null;

            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad; // decode now so the stream can be disposed
            bmp.StreamSource = stream;
            bmp.EndInit();
            bmp.Freeze();
            return bmp;
        }
    }
}