using System.Reflection;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services
{
    public static class PortraitProvider
    {
        public static Image? GetPortrait(byte ownerId)
        {
            string resourceName = $"Yu_Gi_Oh_LOTD_LE_Deck_Manager.Assets.characters.{ownerId}.png";
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            return stream is null ? null : Image.FromStream(stream);
        }
    }
}