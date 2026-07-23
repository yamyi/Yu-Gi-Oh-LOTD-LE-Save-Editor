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
    ///
    /// Disk caching itself is optional (AppState.DiskCacheEnabled, also added
    /// 2026-07-23 per user request) - when turned off in Settings, GetImageAsync
    /// bypasses BOTH the disk cache AND the in-memory session dictionary below,
    /// so every single request (while online) re-contacts ygoprodeck rather
    /// than reusing anything, and nothing is written to disk. When it's on,
    /// AppState.CacheSizeLimitMB (0 = unlimited) caps the on-disk cache's
    /// total size - once a new download pushes it over that cap, the
    /// least-recently-used files are deleted first (see TouchCacheHit /
    /// EnforceCacheLimit) until it's back under.
    ///
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

        // Running total for the size-limit check, kept so enforcing the limit
        // after every single download doesn't mean rescanning the whole
        // directory every time (this cache can be tens of thousands of
        // files during/after a Warm Cache run) - -1 means "not computed yet",
        // lazily filled in by a real directory scan on first use.
        private static long _trackedCacheBytes = -1;
        private static readonly object cacheSizeLock = new();

        /// <summary>
        /// Fetches artwork for a card. Pass small=true for the compact grid
        /// thumbnails, small=false for the larger preview panel. Returns null
        /// if the card has no image URL, the download fails, or there's no
        /// network access — callers should treat null as "just leave the
        /// existing/placeholder visual in place."
        /// Safe to call repeatedly; concurrent/duplicate requests for the
        /// same card+size are coalesced into a single download - UNLESS
        /// AppState.DiskCacheEnabled is false, in which case every call goes
        /// straight to FetchAsync (see the class doc comment).
        /// </summary>
        public static Task<BitmapImage?> GetImageAsync(Card card, bool small, CancellationToken ct = default)
        {
            string? url = small ? card.SmallImageUrl : card.ImageUrl;
            if (string.IsNullOrWhiteSpace(url))
                return Task.FromResult<BitmapImage?>(null);

            string cacheKey = $"{card.Id}_{(small ? "s" : "f")}";

            if (!AppContext.State.DiskCacheEnabled)
                return FetchAsync(url!, cacheKey, ct);

            var task = cache.GetOrAdd(cacheKey, _ => FetchAsync(url!, cacheKey, ct));
            return AwaitAndEvictOnFailure(cacheKey, task);
        }

        /// <summary>Awaits a task already sitting in `cache` and evicts its
        /// entry if it resolved to null (offline miss, download failure,
        /// etc.), so the next call for the same card+size genuinely retries
        /// instead of being stuck with a memoized null for the rest of the
        /// session. This eviction has to happen out here rather than inside
        /// FetchAsync itself (which is what the code used to do): FetchAsync
        /// is the very factory delegate GetOrAdd passes to `cache.GetOrAdd`,
        /// and GetOrAdd always *runs the factory first and inserts its result
        /// into the dictionary second*. Several of FetchAsync's failure paths
        /// (an offline miss, a bad URL) never hit a genuinely-incomplete
        /// await, so the whole async method actually runs synchronously -
        /// which means a `cache.TryRemove(cacheKey, ...)` called from inside
        /// FetchAsync was racing against, and always losing to, GetOrAdd's own
        /// not-yet-happened insert: it removed a key that wasn't in the
        /// dictionary yet, a silent no-op, and the null result got permanently
        /// memoized until the app restarted and cleared the whole static
        /// dictionary. This was the root cause of a 2026-07-23 bug report:
        /// previewing a card while offline (thumbnail cached, full-size not)
        /// then switching back online never re-downloaded the full-size image
        /// until the app was relaunched. Waiting until after GetOrAdd has
        /// definitely returned - and using the conditional KeyValuePair
        /// overload of TryRemove rather than the plain by-key one - avoids
        /// both the ordering race and evicting a different, newer task a
        /// concurrent caller may have already replaced this entry with.</summary>
        private static async Task<BitmapImage?> AwaitAndEvictOnFailure(string cacheKey, Task<BitmapImage?> task)
        {
            BitmapImage? result = await task;
            if (result == null)
                cache.TryRemove(new KeyValuePair<string, Task<BitmapImage?>>(cacheKey, task));
            return result;
        }

        /// <summary>What the preview panels (CardSearchView/DeckEditorView)
        /// actually call instead of GetImageAsync(card, small: false) directly -
        /// tries the full-size image first, and if that comes back null
        /// (most commonly a genuine Offline Mode cache miss, but also covers
        /// a transient network failure or a card with no full-size URL)
        /// falls back to whatever small/tile image is already cached instead
        /// of leaving the preview blank. A stretched-up thumbnail looking
        /// soft beats no art at all - added 2026-07-23 per user request.
        /// Only ever costs one extra request beyond what GetImageAsync(card,
        /// small: false) alone would have made: if the full-size lookup
        /// failed because we're offline, the small lookup underneath is
        /// offline too and just checks its own cache; the only case this
        /// triggers a genuinely new network call is a full-size image that
        /// errored out for some other reason while online.</summary>
        public static async Task<BitmapImage?> GetPreviewImageAsync(Card card, CancellationToken ct = default)
        {
            var full = await GetImageAsync(card, small: false, ct);
            if (full != null) return full;

            return await GetImageAsync(card, small: true, ct);
        }

        private static async Task<BitmapImage?> FetchAsync(string url, string cacheKey, CancellationToken ct)
        {
            try
            {
                byte[] bytes;

                if (!AppContext.State.DiskCacheEnabled)
                {
                    // Caching turned off entirely - never touch cacheDir,
                    // just hit the network directly (or fail the same way a
                    // genuine offline miss does below).
                    if (AppContext.State.IsOfflineMode) return null;
                    bytes = await http.GetByteArrayAsync(url, ct);
                }
                else
                {
                    Directory.CreateDirectory(cacheDir);
                    string cachePath = Path.Combine(cacheDir, cacheKey + ".jpg");

                    if (File.Exists(cachePath))
                    {
                        bytes = await File.ReadAllBytesAsync(cachePath, ct);
                        TouchCacheHit(cachePath);
                    }
                    else if (AppContext.State.IsOfflineMode)
                    {
                        // Nothing on disk and the network isn't allowed right now.
                        // Eviction of this (currently memoized-as-null) cache
                        // entry happens in AwaitAndEvictOnFailure, back in
                        // GetImageAsync, after GetOrAdd has actually inserted it -
                        // doing it here would be a no-op (see that method's doc
                        // comment for why).
                        return null;
                    }
                    else
                    {
                        bytes = await DownloadAndCacheAsync(url, cachePath, ct);
                    }
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
                // Network hiccup, bad URL, corrupt cache file, etc. Eviction
                // happens in AwaitAndEvictOnFailure (see its doc comment) -
                // removing the key here would race GetOrAdd's own not-yet-
                // happened insert and silently do nothing.
                return null;
            }
        }

        private static async Task<byte[]> DownloadAndCacheAsync(string url, string cachePath, CancellationToken ct)
        {
            byte[] bytes = await http.GetByteArrayAsync(url, ct);
            await File.WriteAllBytesAsync(cachePath, bytes, ct);
            RegisterNewFile(bytes.LongLength);
            return bytes;
        }

        // ── Size-limited eviction ────────────────────────────────────────────
        // AppState.CacheSizeLimitMB is a soft cap the user sets in Settings
        // (0 = unlimited). LRU is approximated via each file's
        // LastWriteTimeUtc: a fresh download naturally gets "now", and
        // TouchCacheHit bumps it again on every subsequent cache-hit read, so
        // "oldest LastWriteTimeUtc" really does mean "least recently used"
        // rather than just "downloaded longest ago".

        /// <summary>Bumps a cached file's last-write time to now on every
        /// cache-hit read, so it looks "freshly used" to EnforceCacheLimit's
        /// oldest-first eviction instead of looking stale just because it
        /// hasn't been re-downloaded.</summary>
        private static void TouchCacheHit(string cachePath)
        {
            try { File.SetLastWriteTimeUtc(cachePath, DateTime.UtcNow); }
            catch { /* not worth failing a cache read over a metadata write */ }
        }

        private static void RegisterNewFile(long size)
        {
            lock (cacheSizeLock)
            {
                if (_trackedCacheBytes < 0) _trackedCacheBytes = ComputeDirectorySize();
                _trackedCacheBytes += size;
                EnforceCacheLimitLocked();
            }
        }

        /// <summary>Forces an immediate eviction pass against the current
        /// AppState.CacheSizeLimitMB, instead of waiting for the next new
        /// download to trigger one - Settings calls this right after the
        /// user applies a smaller limit, so lowering the limit below the
        /// cache's current size takes effect right away.</summary>
        public static void EnforceCacheLimitNow()
        {
            lock (cacheSizeLock)
            {
                _trackedCacheBytes = ComputeDirectorySize();
                EnforceCacheLimitLocked();
            }
        }

        /// <summary>Caller must hold cacheSizeLock. Deletes the
        /// least-recently-used cached files (oldest LastWriteTimeUtc first)
        /// until _trackedCacheBytes is back under the configured limit, or
        /// there's nothing left to delete.</summary>
        private static void EnforceCacheLimitLocked()
        {
            int limitMB = AppContext.State.CacheSizeLimitMB;
            if (limitMB <= 0) return; // 0 = unlimited

            long limitBytes = (long)limitMB * 1024 * 1024;
            if (_trackedCacheBytes <= limitBytes) return;
            if (!Directory.Exists(cacheDir)) return;

            IEnumerable<FileInfo> oldestFirst;
            try
            {
                oldestFirst = new DirectoryInfo(cacheDir).GetFiles().OrderBy(f => f.LastWriteTimeUtc);
            }
            catch
            {
                return; // directory vanished or is inaccessible - nothing more to do here
            }

            foreach (var file in oldestFirst)
            {
                if (_trackedCacheBytes <= limitBytes) break;

                long length;
                try
                {
                    length = file.Length;
                    file.Delete();
                }
                catch
                {
                    continue; // in use or already gone - skip it and keep evicting others
                }

                _trackedCacheBytes -= length;
            }
        }

        private static long ComputeDirectorySize()
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

        // ── Settings-page support ────────────────────────────────────────────
        // Read/cleared/warmed by SettingsView - kept here rather than
        // exposing cacheDir itself, so the on-disk layout stays this class's
        // own concern.

        private const int WarmCacheConcurrency = 8;

        /// <summary>Downloads and caches art for every given card, so it's
        /// available before switching to Offline Mode instead of relying on
        /// incidental downloads from browsing Card Search/Deck Editor.
        /// Always warms the small (tile) image - the one every grid in this
        /// app actually shows constantly - since that's ~150MB total and
        /// covers the overwhelmingly common case. includeFullSize additionally
        /// warms the full (preview) image too - the one only ever shown for a
        /// single selected card at a time - which roughly 10x's the total
        /// download (~1.6GB more) for coverage that's rarely all used in one
        /// session; SettingsView defaults this to false and lets the user
        /// opt in via a checkbox, quoting the right estimate either way.
        /// Already-cached images are cheap re-reads (see FetchAsync's
        /// cache-hit path), so running this again later only actually
        /// downloads whatever's new/newly opted-into. Throttled to
        /// WarmCacheConcurrency parallel requests so it doesn't hammer
        /// ygoprodeck's server or saturate the connection; progress reports
        /// how many of the total *cards* (not individual small/full requests)
        /// have finished so far. Respects ct - stops scheduling new downloads
        /// and throws OperationCanceledException once cancelled, same as any
        /// other Task.</summary>
        public static async Task WarmCacheAsync(IEnumerable<Card> cards, bool includeFullSize, IProgress<(int done, int total)>? progress, CancellationToken ct = default)
        {
            var list = cards.ToList();
            int done = 0;

            var options = new ParallelOptions { MaxDegreeOfParallelism = WarmCacheConcurrency, CancellationToken = ct };
            await Parallel.ForEachAsync(list, options, async (card, token) =>
            {
                await GetImageAsync(card, small: true, token);
                if (includeFullSize)
                    await GetImageAsync(card, small: false, token);

                int n = Interlocked.Increment(ref done);
                progress?.Report((n, list.Count));
            });
        }

        /// <summary>Total size, in bytes, of everything currently cached on
        /// disk. Returns 0 if the cache directory doesn't exist yet. Always a
        /// fresh directory scan (unlike the internal _trackedCacheBytes
        /// running total) - called rarely enough by the Settings page that
        /// correctness matters more than avoiding the scan.</summary>
        public static long GetCacheSizeBytes() => ComputeDirectorySize();

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

            if (Directory.Exists(cacheDir))
            {
                foreach (var file in Directory.EnumerateFiles(cacheDir))
                {
                    try { File.Delete(file); }
                    catch { /* in use or already gone - not worth failing over */ }
                }
            }

            lock (cacheSizeLock) { _trackedCacheBytes = 0; }
        }
    }
}
