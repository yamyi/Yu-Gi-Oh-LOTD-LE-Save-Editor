namespace YuGiOhSaveEditor.Services
{
    /// <summary>On-disk save format version. Only Link Evolution (2019+) is
    /// supported.</summary>
    public enum LotdSaveVersion
    {
        LinkEvolution,
    }

    /// <summary>
    /// Full binary layout of the Link Evolution save file - every top-level
    /// chunk, not just the deck slots already covered by SlotLayout.cs.
    /// Ported from pixeltris/Lotd (Lotd/SaveData/GameSaveData.cs,
    /// Constants.cs). See https://github.com/pixeltris/Lotd
    ///
    /// Chunk order: header (20B) -&gt; unknown block (16B) -&gt; Stats -&gt;
    /// BattlePacks -&gt; Misc -&gt; Campaign -&gt; Decks -&gt; CardList -&gt; EOF.
    /// </summary>
    public static class LotdSaveFormat
    {
        // ── Header magic (bytes 0-7) ─────────────────────────────────────────────
        private const uint HeaderMagic1 = 0x54CE29F9;
        private const uint HeaderMagic2 = 0x04ABE802;

        /// <summary>Always returns LinkEvolution - kept so call sites don't
        /// need to change if another format is ever added.</summary>
        public static LotdSaveVersion DetectVersion(byte[]? save) => LotdSaveVersion.LinkEvolution;

        /// <summary>True if the buffer's header magic matches a Link Evolution save.</summary>
        public static bool HasValidHeader(byte[] save)
        {
            if (save == null || save.Length < 8) return false;
            return ReadU32(save, 0) == HeaderMagic1 && ReadU32(save, 4) == HeaderMagic2;
        }

        // ── Fixed geometry ────────────────────────────────────────────────────────
        public const int NumBattlePacks = 5;
        public const int NumUserDecks = 32;
        public const int DuelsPerSeries = 50;

        /// <summary>Byte capacity reserved for the unlocked-avatars bitfield (32 bytes = 256 bits).</summary>
        public const int UnlockedAvatarsBufferBytes = 32;

        /// <summary>Number of avatar bits actually meaningful (ids 0-152 in chardata.bin).</summary>
        public const int NumAvatarSlots = 153;

        public const int StatsOffset = 36;

        // ── Geometry (verbatim from GameSaveData.cs) ─────────────────────────────
        public static int GetFileLength(LotdSaveVersion v) => 44008;
        public static int GetNumDuelSeries(LotdSaveVersion v) => 6;
        public static int GetNumDeckDataSlots(LotdSaveVersion v) => 700;
        public static int GetRecipeBufferBytes(LotdSaveVersion v) => 88;

        /// <summary>Real (non-padding) card count. Real cards are scattered
        /// throughout the full 20000-slot CardList chunk, not clustered in a
        /// 0..N-1 prefix - see CardCollectionLayout's bulk methods, which
        /// scan the full chunk rather than assuming this count is a
        /// contiguous range.</summary>
        public static int GetNumCards(LotdSaveVersion v) => 10027;

        public static int GetNumStats(LotdSaveVersion v) => 100;
        public static int GetBattlePacksOffset(LotdSaveVersion v) => 836;
        public static int GetMiscDataOffset(LotdSaveVersion v) => 4056;
        public static int GetCampaignDataOffset(LotdSaveVersion v) => 7024;
        public static int GetDecksOffset(LotdSaveVersion v) => 14280;
        public static int GetCardListOffset(LotdSaveVersion v) => 24008;

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
