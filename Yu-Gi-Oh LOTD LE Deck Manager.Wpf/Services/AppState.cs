using System;

namespace YuGiOhDeckManager.Wpf.Services
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
    }
}
