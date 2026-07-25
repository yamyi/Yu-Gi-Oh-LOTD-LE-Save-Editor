using System;

namespace YuGiOhSaveEditor.Services
{
    public sealed class AppState
    {
        public string SavePath { get; set; } = "";
        public byte[]? SaveBytes { get; set; }

        // Which on-disk save format is currently loaded (original "Lotd" vs
        // "Link Evolution") — detected from the header magic bytes right after
        // a file is read (see LotdSaveFormat.DetectVersion, called from
        // MainForm.LoadSaveSlots). Everything that depends on chunk offsets
        // (SlotLayout, MiscSaveLayout, CampaignSaveLayout, CardCollectionLayout)
        // reads this rather than assuming a fixed format.
        public LotdSaveVersion Version { get; set; } = LotdSaveVersion.LinkEvolution;

        // Dirty-tracking baseline, mirrored here (not just as a MainForm-local
        // field) so a theme-change soft restart (see Program.cs) can rebuild
        // MainForm without losing track of "have I saved since I last loaded
        // this file" — AppContext's static services survive the rebuild, but
        // a fresh MainForm's own fields would otherwise start blank.
        public byte[]? DirtyBaseline { get; set; }

        private SlotIO.SlotInfo[] _slots = Array.Empty<SlotIO.SlotInfo>();
        public SlotIO.SlotInfo[] Slots
        {
            get => _slots;
            set
            {
                _slots = value ?? Array.Empty<SlotIO.SlotInfo>();
                SlotsChanged?.Invoke(_slots);
            }
        }

        public event Action<SlotIO.SlotInfo[]>? SlotsChanged;

        public Action<string>? Log { get; set; }

        // ── App-wide preferences ─────────────────────────────────────────────────
        // Global toggle (set from the Settings page) for pages that fetch card
        // images/data online — e.g. DeckEditorPage.IsOnline should follow this.
        // Not consumed anywhere yet; wiring it up is a future step.
        private bool _isOfflineMode;
        public bool IsOfflineMode
        {
            get => _isOfflineMode;
            set
            {
                if (_isOfflineMode == value) return;
                _isOfflineMode = value;
                OfflineModeChanged?.Invoke(_isOfflineMode);
            }
        }

        public event Action<bool>? OfflineModeChanged;

        // Whether CardImageProvider is allowed to read/write its on-disk
        // image cache at all - added 2026-07-23 per user request to make
        // disk caching itself optional. When false, every image request
        // (while online) goes straight to the network and nothing is
        // written to disk, i.e. "contacts the website each time" rather than
        // persisting anything between requests. Defaults to true so behavior
        // is unchanged unless the user opts out in Settings.
        public bool DiskCacheEnabled { get; set; } = true;

        // Soft cap, in megabytes, on the on-disk image cache's total size -
        // 0 means unlimited. User-configurable in Settings; CardImageProvider
        // evicts the least-recently-used cached files (see
        // CardImageProvider.TouchCacheHit) once this is exceeded, right after
        // writing a new file. Same in-memory-only persistence as
        // IsOfflineMode/DiskCacheEnabled - resets to 0 (unlimited) on every
        // app restart rather than being saved to a settings file, since this
        // app doesn't have one yet.
        public int CacheSizeLimitMB { get; set; }

        // Which Themes/*.xaml palette is merged into Application.Resources -
        // "MasterDuel" (default, App.xaml's original hardcoded choice) or
        // "DuelLinks". Read once at startup by App.OnStartup, before
        // MainWindow/Controls.xaml ever get a chance to resolve a
        // StaticResource brush against it - WPF bakes StaticResource values
        // in at parse time, so this can't be swapped live while the app is
        // running the way IsOfflineMode/DiskCacheEnabled can. SettingsView's
        // Appearance picker sets this and calls AppSettings.Save(), then
        // prompts for a restart rather than trying to re-theme in place.
        public string ThemeName { get; set; } = "MasterDuel";
    }
}
