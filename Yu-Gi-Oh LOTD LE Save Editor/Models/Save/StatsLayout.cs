namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// The 42 known campaign/duel counters tracked in the Stats chunk (see
    /// SaveLayout's whole-file map — fixed at LotdSaveFormat.StatsOffset,
    /// 0x24/36, in both save formats). Ported from pixeltris/Lotd's
    /// StatSaveType enum (Lotd/SaveData/StatSaveData.cs), which the user's own
    /// WinForms reference tool (Arefu/Wolf's Elroy.SaveStatManager) matches
    /// member-for-member and in the same order — that ordering is exactly the
    /// on-disk slot order, so StatType's integer value IS the stat's index in
    /// the chunk (0-based).
    ///
    /// pixeltris' enum has one further "Unknown" 43rd member after
    /// Decks_Created ("There seems to be one additional value. Is this
    /// actually part of the stat data or something separate?") — deliberately
    /// left out here since it isn't a real, nameable stat, matching Elroy's
    /// own tool which also stops at Decks_Created.
    ///
    /// LinkEvolution reserves 100 stat slots total (LotdSaveFormat.GetNumStats),
    /// not just 43 — the same "wider on-disk capacity than known fields" pattern
    /// already seen in CardCollectionLayout (GetNumCardSlots vs GetNumCards).
    /// Slots 42-99 have no known real-world meaning (presumably newer counters
    /// added for Link Evolution's expanded content) and are intentionally left
    /// untouched: StatType/GetKnownStats only expose indices 0-41, and nothing
    /// in this class ever bulk-writes across the full GetNumStats(v) range.
    /// </summary>
    public enum StatType
    {
        Games_Campaign = 0,
        Games_Campaign_Normal = 1,
        Games_Campaign_Reverse = 2,
        Games_Challenge = 3,
        Games_Multiplayer = 4,
        Games_Multiplayer_1V1 = 5,
        Games_Multiplayer_Tag = 6,
        Games_Multiplayer_Ranked = 7,
        Games_Multiplayer_Friendly = 8,
        Games_Multiplayer_Battlepack_Any = 9,
        Games_Multiplayer_Battlepack_1 = 10,
        Games_Multiplayer_Battlepack_2 = 11,
        Games_Multiplayer_Battlepack_3 = 12,
        Wins_Campaign = 13,
        Wins_Campaign_Normal = 14,
        Wins_Campaign_Reverse = 15,
        Wins_Challenge = 16,
        Wins_Multiplayer = 17,
        Wins_Multiplayer_1V1 = 18,
        Wins_Multiplayer_Tag = 19,
        Wins_Multiplayer_Ranked = 20,
        Wins_Multiplayer_Friendly = 21,
        Wins_Multiplayer_Battlepack_Any = 22,
        Wins_Multiplayer_Battlepack_1 = 23,
        Wins_Multiplayer_Battlepack_2 = 24,
        Wins_Multiplayer_Battlepack_3 = 25,
        Wins_Match = 26,
        Wins_Nonmatch = 27,
        Summons_Normal = 28,
        Summons_Tribute = 29,
        Summons_Ritual = 30,
        Summons_Fusion = 31,
        Summons_Xyz = 32,
        Summons_Synchro = 33,
        Summons_Pendulum = 34,
        Damage_Any = 35,
        Damage_Battle = 36,
        Damage_Direct = 37,
        Damage_Effect = 38,
        Damage_Reflect = 39,
        Chains = 40,
        Decks_Created = 41,
    }

    /// <summary>
    /// Reads/writes the Stats save chunk directly against the raw save byte[].
    /// Each slot is 8 bytes (int64), base offset LotdSaveFormat.StatsOffset —
    /// confirmed twice over: once via SaveLayout/LotdSaveFormat's own
    /// GetStatsSize(v)/GetNumStats(v) math (800/100 and 344/43 both divide out
    /// to 8 bytes/stat), and again by the user's pasted Elroy.SaveStatManager
    /// code, which reads each stat as ReadBytes(8) at a running position that
    /// starts at absolute offset 0x24 (=36=StatsOffset) and writes back with
    /// BitConverter.GetBytes((long)value) — genuinely an 8-byte/int64 field on
    /// disk, even though Elroy's own UI only surfaces the low 4 bytes of it
    /// (BitConverter.ToInt32 on the same 8-byte read) into a NumericUpDown.
    /// This class uses long end-to-end instead, so stat values beyond int32
    /// range round-trip correctly.
    /// </summary>
    public static class StatsLayout
    {
        public const int EntryBytes = SaveLayout.StatEntryBytes; // 8

        /// <summary>Number of stats with a known name/meaning (see StatType) —
        /// NOT the same as GetNumStats(v) (43/100 on-disk slots). See this
        /// class's doc comment for why the gap is left alone.</summary>
        public static int NumKnownStats => Enum.GetValues<StatType>().Length; // 42

        public static int GetOffset(LotdSaveVersion v, int index) =>
            LotdSaveFormat.StatsOffset + index * EntryBytes;

        public static int GetOffset(LotdSaveVersion v, StatType stat) => GetOffset(v, (int)stat);

        public static long Get(byte[] save, LotdSaveVersion v, int index)
        {
            int offset = GetOffset(v, index);
            if (offset < 0 || offset + EntryBytes > save.Length) return 0;
            return BitConverter.ToInt64(save, offset);
        }

        public static long Get(byte[] save, LotdSaveVersion v, StatType stat) => Get(save, v, (int)stat);

        public static void Set(byte[] save, LotdSaveVersion v, int index, long value)
        {
            int offset = GetOffset(v, index);
            if (offset < 0 || offset + EntryBytes > save.Length) return;
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, save, offset, EntryBytes);
        }

        public static void Set(byte[] save, LotdSaveVersion v, StatType stat, long value) => Set(save, v, (int)stat, value);

        /// <summary>Human-readable label for a known stat, for the Stats tab's list.</summary>
        public static string GetLabel(StatType stat) => stat switch
        {
            StatType.Games_Campaign => "Campaign Games",
            StatType.Games_Campaign_Normal => "Campaign Games (Normal)",
            StatType.Games_Campaign_Reverse => "Campaign Games (Reverse)",
            StatType.Games_Challenge => "Challenge Games",
            StatType.Games_Multiplayer => "Multiplayer Games",
            StatType.Games_Multiplayer_1V1 => "Multiplayer Games (1v1)",
            StatType.Games_Multiplayer_Tag => "Multiplayer Games (Tag)",
            StatType.Games_Multiplayer_Ranked => "Multiplayer Games (Ranked)",
            StatType.Games_Multiplayer_Friendly => "Multiplayer Games (Friendly)",
            StatType.Games_Multiplayer_Battlepack_Any => "Multiplayer Games (Any Battle Pack)",
            StatType.Games_Multiplayer_Battlepack_1 => "Multiplayer Games (Battle Pack 1)",
            StatType.Games_Multiplayer_Battlepack_2 => "Multiplayer Games (Battle Pack 2)",
            StatType.Games_Multiplayer_Battlepack_3 => "Multiplayer Games (Battle Pack 3)",
            StatType.Wins_Campaign => "Campaign Wins",
            StatType.Wins_Campaign_Normal => "Campaign Wins (Normal)",
            StatType.Wins_Campaign_Reverse => "Campaign Wins (Reverse)",
            StatType.Wins_Challenge => "Challenge Wins",
            StatType.Wins_Multiplayer => "Multiplayer Wins",
            StatType.Wins_Multiplayer_1V1 => "Multiplayer Wins (1v1)",
            StatType.Wins_Multiplayer_Tag => "Multiplayer Wins (Tag)",
            StatType.Wins_Multiplayer_Ranked => "Multiplayer Wins (Ranked)",
            StatType.Wins_Multiplayer_Friendly => "Multiplayer Wins (Friendly)",
            StatType.Wins_Multiplayer_Battlepack_Any => "Multiplayer Wins (Any Battle Pack)",
            StatType.Wins_Multiplayer_Battlepack_1 => "Multiplayer Wins (Battle Pack 1)",
            StatType.Wins_Multiplayer_Battlepack_2 => "Multiplayer Wins (Battle Pack 2)",
            StatType.Wins_Multiplayer_Battlepack_3 => "Multiplayer Wins (Battle Pack 3)",
            StatType.Wins_Match => "Match Wins",
            StatType.Wins_Nonmatch => "Non-Match Wins",
            StatType.Summons_Normal => "Normal Summons",
            StatType.Summons_Tribute => "Tribute Summons",
            StatType.Summons_Ritual => "Ritual Summons",
            StatType.Summons_Fusion => "Fusion Summons",
            StatType.Summons_Xyz => "Xyz Summons",
            StatType.Summons_Synchro => "Synchro Summons",
            StatType.Summons_Pendulum => "Pendulum Summons",
            StatType.Damage_Any => "Total Damage",
            StatType.Damage_Battle => "Battle Damage",
            StatType.Damage_Direct => "Direct Damage",
            StatType.Damage_Effect => "Effect Damage",
            StatType.Damage_Reflect => "Reflected Damage",
            StatType.Chains => "Chains Activated",
            StatType.Decks_Created => "Decks Created",
            _ => stat.ToString(),
        };
    }
}
