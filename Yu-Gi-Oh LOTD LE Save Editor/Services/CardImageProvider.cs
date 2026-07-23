using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Windows.Media.Imaging;

namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Downloads and caches card artwork from Card.ImageUrl / SmallImageUrl.
    /// Results are cached in memory for the running session and on disk
    /// (under %LocalAppData%) so images don't have to be re-downloaded between
    /// runs. Callers should call this unconditionally, online or offline -
    /// this class itself decides whether the network is allowed (see
    /// AppState.IsOfflineMode): an on-disk cache hit is always returned
    /// regardless of offline mode, and only a genuine cache miss falls back
    /// to "return null without downloading" while offline. This way a card
    /// whose art was already cached during an earlier online session still
    /// shows up even after switching to Offline Mode - changed 2026-07-23,
    /// previously offline mode skipped this class entirely at every call
    /// site, which also hid already-cached art for no reason.
    /// Returns BitmapImage (WPF's native imaging type) instead of the WinForms
    /// build's System.Drawing.Bitmap — bind straight to an Image.Source.
    /// </summary>
    public static class CardImageProvider
    {
        private static readonly HttpClient http = new()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private static readonly ConcurrentDictionary<string, Task<BitmapImage?>> cache = new();

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
        public static Task<BitmapImage?> GetImageAsync(Card card, bool small, CancellationToken ct = default)
        {
            string? url = small ? card.SmallImageUrl : card.ImageUrl;
            if (string.IsNullOrWhiteSpace(url))
                return Task.FromResult<BitmapImage?>(null);

            string cacheKey = $"{card.Id}_{(small ? "s" : "f")}";
            return cache.GetOrAdd(cacheKey, _ => FetchAsync(url!, cacheKey, ct));
        }

        private static async Task<BitmapImage?> FetchAsync(string url, string cacheKey, CancellationToken ct)
        {
            try
            {
                Directory.CreateDirectory(cacheDir);
                string cachePath = Path.Combine(cacheDir, cacheKey + ".jpg");

                byte[] bytes;
                if (File.Exists(cachePath))
                {
                    bytes = await File.ReadAllBytesAsync(cachePath, ct);
                }
                else if (AppContext.State.IsOfflineMode)
                {
                    // Nothing on disk and the network isn't allowed right now -
                    // don't memoize this as a permanent result (same as the
                    // catch block below), so a later call - e.g. after the
                    // user turns Offline Mode back off - retries instead of
                    // being stuck returning null for the rest of the session.
                    cache.TryRemove(cacheKey, out _);
                    return null;
                }
                else
                {
                    bytes = await DownloadAndCacheAsync(url, cachePath, ct);
                }

                using var ms = new MemoryStream(bytes);
                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.CacheOption = BitmapCacheOption.OnLoad; // decode now so ms can be disposed
                bmp.StreamSource = ms;
                bmp.EndInit();
                bmp.Freeze(); // makes it safe to hand to any thread/control
                return bmp;
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

        // ── Settings-page support ────────────────────────────────────────────
        // Read/cleared by SettingsView - kept here rather than exposing
        // cacheDir itself, so the on-disk layout stays this class's own
        // concern.

        /// <summary>Total size, in bytes, of everything currently cached on
        /// disk. Returns 0 if the cache directory doesn't exist yet.</summary>
        public static long GetCacheSizeBytes()
        {
            if (!Directory.Exists(cacheDir)) return 0;

            long total = 0;
            foreach (var file in Directory.EnumerateFiles(cacheDir))
            {
                try { total += new FileInfo(file).Length; }
                catch { /* file could vanish mid-enumeration; just skip it */ }
            }
            return total;
        }

        /// <summary>Returns the on-disk cache folder, creating it first if it
        /// doesn't exist yet - so the Settings page's "Open Cache Folder"
        /// button always has somewhere real to open, even before any artwork
        /// has ever been downloaded.</summary>
        public static string GetCacheDirectory()
        {
            Directory.CreateDirectory(cacheDir);
            return cacheDir;
        }

        /// <summary>Deletes every cached image on disk and forgets the
        /// in-memory results too, so the next request for any card
        /// re-downloads its artwork from scratch.</summary>
        public static void ClearCache()
        {
            cache.Clear();

            if (!Directory.Exists(cacheDir)) return;
            foreach (var file in Directory.EnumerateFiles(cacheDir))
            {
                try { File.Delete(file); }
                catch { /* in use or already gone - not worth failing over */ }
            }
        }
    }
}
