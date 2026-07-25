using System.IO;
using System.Text.Json;

namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Persists the handful of Settings-page preferences
    /// (AppState.IsOfflineMode / DiskCacheEnabled / CacheSizeLimitMB /
    /// ThemeName) to a small JSON file under %LocalAppData%, so they survive
    /// an app restart instead of silently resetting to their defaults every
    /// launch - added 2026-07-23 per user report ("the settings (offline
    /// mode, cache size, etc...) are reset after closing the app").
    /// App.xaml.cs calls Load() once at startup, before anything else reads
    /// AppState (ThemeName in particular has to be read before base.OnStartup
    /// constructs MainWindow - see App.ApplyTheme); SettingsView's change
    /// handlers (OfflineModeCheckBox_Changed, EnableCacheCheckBox_Changed,
    /// CacheLimitApplyButton_Click, ThemeCombo_SelectionChanged) each call
    /// Save() right after updating AppState so the file is always in sync
    /// with whatever's showing on screen - there's no separate "apply" step
    /// for persistence itself.
    /// </summary>
    public static class AppSettings
    {
        private static readonly string path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Yu-Gi-Oh LOTD LE Deck Editor", "settings.json");

        private sealed class Data
        {
            public bool IsOfflineMode { get; set; }
            public bool DiskCacheEnabled { get; set; } = true;
            public int CacheSizeLimitMB { get; set; }
            public string ThemeName { get; set; } = "MasterDuel";
        }

        /// <summary>Reads settings.json into AppContext.State if it exists.
        /// Missing file (first run) or a corrupt/unreadable one both just
        /// leave AppState at its normal defaults - this is a convenience
        /// cache of user preferences, not something worth ever failing
        /// startup over.</summary>
        public static void Load()
        {
            try
            {
                if (!File.Exists(path)) return;

                var data = JsonSerializer.Deserialize<Data>(File.ReadAllText(path));
                if (data == null) return;

                AppContext.State.IsOfflineMode = data.IsOfflineMode;
                AppContext.State.DiskCacheEnabled = data.DiskCacheEnabled;
                AppContext.State.CacheSizeLimitMB = data.CacheSizeLimitMB;
                if (!string.IsNullOrWhiteSpace(data.ThemeName)) AppContext.State.ThemeName = data.ThemeName;
            }
            catch
            {
                // Corrupt/unreadable file - start with AppState's built-in defaults.
            }
        }

        /// <summary>Writes AppContext.State's current preference values to
        /// settings.json. Called after each individual preference change
        /// rather than e.g. only on app exit, so a crash or force-close
        /// doesn't lose whatever was last set.</summary>
        public static void Save()
        {
            try
            {
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

                var data = new Data
                {
                    IsOfflineMode = AppContext.State.IsOfflineMode,
                    DiskCacheEnabled = AppContext.State.DiskCacheEnabled,
                    CacheSizeLimitMB = AppContext.State.CacheSizeLimitMB,
                    ThemeName = AppContext.State.ThemeName,
                };
                File.WriteAllText(path, JsonSerializer.Serialize(data));
            }
            catch
            {
                // Not worth failing a settings change over a write error -
                // the in-memory AppState value still took effect either way.
            }
        }
    }
}
