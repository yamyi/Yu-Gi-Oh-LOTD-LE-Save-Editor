using System.Collections.Concurrent;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services
{
    /// <summary>
    /// Downloads and caches card artwork from Card.ImageUrl / SmallImageUrl.
    /// Results are cached in memory for the running session and on disk
    /// (under %LocalAppData%) so images don't have to be re-downloaded between
    /// runs. Callers are expected to only invoke this when online mode is
    /// enabled (see AppState.IsOfflineMode) — this class does not check that
    /// itself, it just fetches whatever URL it's given.
    /// </summary>
    public static class CardImageProvider
    {
        private static readonly HttpClient http = new()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private static readonly ConcurrentDictionary<string, Task<Image?>> cache = new();

        private static readonly string cacheDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Yu-Gi-Oh LOTD LE Deck Editor", "CardImageCache");

        /// <summary>
        /// Fetches artwork for a card. Pass small=true for the compact grid
        /// thumbnails, small=false for the larger preview panel. Returns null
        /// if the card has no image URL, the download fails, or there's no
        /// network access — callers should treat null as "just leave the
        /// existing/placeholder visual in place."
        /// Safe to call repeatedly; concurrent/duplicate requests for the
        /// same card+size are coalesced into a single download.
        /// </summary>
        public static Task<Image?> GetImageAsync(Card card, bool small, CancellationToken ct = default)
        {
            string? url = small ? card.SmallImageUrl : card.ImageUrl;
            if (string.IsNullOrWhiteSpace(url))
                return Task.FromResult<Image?>(null);

            string cacheKey = $"{card.Id}_{(small ? "s" : "f")}";
            return cache.GetOrAdd(cacheKey, _ => FetchAsync(url!, cacheKey, ct));
        }

        private static async Task<Image?> FetchAsync(string url, string cacheKey, CancellationToken ct)
        {
            try
            {
                Directory.CreateDirectory(cacheDir);
                string cachePath = Path.Combine(cacheDir, cacheKey + ".jpg");

                byte[] bytes = File.Exists(cachePath)
                    ? await File.ReadAllBytesAsync(cachePath, ct)
                    : await DownloadAndCacheAsync(url, cachePath, ct);

                using var ms = new MemoryStream(bytes);
                using var raw = Image.FromStream(ms);
                return new Bitmap(raw);
            }
            catch
            {
                // Network hiccup, bad URL, corrupt cache file, etc. — let a
                // later call retry instead of remembering the failure forever.
                cache.TryRemove(cacheKey, out _);
                return null;
            }
        }

        private static async Task<byte[]> DownloadAndCacheAsync(string url, string cachePath, CancellationToken ct)
        {
            byte[] bytes = await http.GetByteArrayAsync(url, ct);
            await File.WriteAllBytesAsync(cachePath, bytes, ct);
            return bytes;
        }
    }
}
