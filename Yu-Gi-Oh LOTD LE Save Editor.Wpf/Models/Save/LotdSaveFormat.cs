namespace YuGiOhSaveEditor.Wpf.Services
{
    /// <summary>
    /// Which on-disk save format a .dat file uses. The game shipped in two eras
    /// with genuinely different save layouts:
    ///   - Lotd: the original 2016 "Yu-Gi-Oh! Legacy of the Duelist" (29005 bytes,
    ///     5 duel series — no VRAINS).
    ///   - LinkEvolution: the 2019+ "Link Evolution" remaster (44008 bytes,
    ///     6 duel series — adds VRAINS/Pendulum-era content).
    /// Detected automatically from the file's header magic bytes — see DetectVersion.
    /// </summary>
    public enum LotdSaveVersion
    {
        Lotd,
        LinkEvolution,
    }

    /// <summary>
    /// Full binary layout of the LOTD save file — every top-level chunk, not just
    /// the deck slots already covered by SlotLayout.cs. Ported directly from
    /// pixeltris/Lotd (Lotd/SaveData/GameSaveData.cs, Constants.cs), the source
    /// of the original reverse-engineered save-editing tool for this game.
    /// Offsets/sizes below are taken verbatim from that project.
    /// See https://github.com/pixeltris/Lotd
    ///
    /// File chunk order (all versions): header (20B) → unknown block (16B) →
    /// Stats → BattlePacks → Misc → Campaign → Decks → CardList → EOF.
    /// </summary>
    public static class LotdSaveFormat
    {
        // ── Header magic (bytes 0-7) ─────────────────────────────────────────────
        // Byte 0-3 is the same for both versions; byte 4-7 tells them apart.
        private const uint HeaderMagic1 = 0x54CE29F9;
        private const uint HeaderMagic2_Lotd = 0x04714D02;
        private const uint HeaderMagic2_LinkEvolution = 0x04ABE802;

        /// <summary>
        /// Detects which save format a loaded buffer uses by reading its header
        /// magic (bytes 4-7). Defaults to LinkEvolution if the buffer is too
        /// short or the magic doesn't match either known value.
        /// </summary>
        public static LotdSaveVersion DetectVersion(byte[]? save)
        {
            if (save == null || save.Length < 8)
                return LotdSaveVersion.LinkEvolution;

            uint magic2 = ReadU32(save, 4);
            return magic2 == HeaderMagic2_Lotd ? LotdSaveVersion.Lotd : LotdSaveVersion.LinkEvolution;
        }

        /// <summary>True if the buffer's header magic matches a known LOTD save at all.</summary>
        public static bool HasValidHeader(byte[] save)
        {
            if (save == null || save.Length < 8) return false;
            uint magic1 = ReadU32(save, 0);
            uint magic2 = ReadU32(save, 4);
            return magic1 == HeaderMagic1 && (magic2 == HeaderMagic2_Lotd || magic2 == HeaderMagic2_LinkEvolution);
        }

        // ── Fixed geometry (same across versions) ────────────────────────────────
        public const int NumBattlePacks = 5;
        public const int NumUserDecks = 32;
        public const int DuelsPerSeries = 50;

        /// <summary>Byte capacity reserved for the unlocked-avatars bitfield (32 bytes = 256 bits).</summary>
        public const int UnlockedAvatarsBufferBytes = 32;

        /// <summary>Number of avatar bits actually meaningful (ids 0-152 in chardata.bin).</summary>
        public const int NumAvatarSlots = 153;

        public const int StatsOffset = 36;

        // ── Version-dependent geometry (verbatim from GameSaveData.cs) ───────────
        public static int GetFileLength(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 29005 : 44008;

        public static int GetNumDuelSeries(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 5 : 6;

        public static int GetNumDeckDataSlots(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 477 : 700;

        public static int GetRecipeBufferBytes(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 60 : 88;

        public static int GetNumCards(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 7581 : 10166;

        public static int GetNumStats(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 43 : 100;

        public static int GetBattlePacksOffset(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 380 : 836;

        public static int GetMiscDataOffset(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 3600 : 4056;

        public static int GetCampaignDataOffset(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 5648 : 7024;

        public static int GetDecksOffset(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 11696 : 14280;

        public static int GetCardListOffset(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd ? 21424 : 24008;

        // ── Derived sizes (each chunk runs up to the start of the next) ──────────
        public static int GetStatsSize(LotdSaveVersion v) => GetBattlePacksOffset(v) - StatsOffset;
        public static int GetBattlePacksSize(LotdSaveVersion v) => GetMiscDataOffset(v) - GetBattlePacksOffset(v);
        public static int GetMiscDataSize(LotdSaveVersion v) => GetCampaignDataOffset(v) - GetMiscDataOffset(v);
        public static int GetCampaignDataSize(LotdSaveVersion v) => GetDecksOffset(v) - GetCampaignDataOffset(v);
        public static int GetDecksSize(LotdSaveVersion v) => GetCardListOffset(v) - GetDecksOffset(v);
        public static int GetCardListSize(LotdSaveVersion v) => GetFileLength(v) - GetCardListOffset(v);

        private static uint ReadU32(byte[] b, int offset) =>
            (uint)(b[offset] | (b[offset + 1] << 8) | (b[offset + 2] << 16) | (b[offset + 3] << 24));
    }
}
