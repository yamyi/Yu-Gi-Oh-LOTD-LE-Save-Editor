namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// The 43 known campaign/duel counters in the Stats chunk (fixed at
    /// LotdSaveFormat.StatsOffset, 0x24/36). Indices 0-41 ported from
    /// pixeltris/Lotd's StatSaveType enum, exactly matching on-disk slot
    /// order. The chunk reserves 100 slots total (LotdSaveFormat.GetNumStats);
    /// slots 43-99 have no known meaning and are left untouched.
    ///
    /// Cards_Won_Campaign (index 42) is new card copies granted as a
    /// post-duel reward (1 per loss, 3 per win, 0 if already at max copies),
    /// not a win/score counter - a lifetime total, intentionally untouched
    /// by every Recalculate* method here.
    /// </summary>
    public enum StatType
    {
        Games_Campaign = 0,
        Games_Campaign_Normal = 1,
        Games_Campaign_Reverse = 2,
        Games_Challenge = 3,
        Games_Multiplayer = 4,
        Games_Multiplayer_1V1 = 5,

        /// <summary>Tag Duel isn't reachable through the shipped Multiplayer
        /// UI (requires a third-party duel-starter tool). Should always read
        /// 0 in a normally-played save.</summary>
        Games_Multiplayer_Tag = 6,
        Games_Multiplayer_Ranked = 7,

        /// <summary>Named "Friendly" in pixeltris/Lotd; the game calls it
        /// "Player Match" (see GetLabel).</summary>
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

        /// <summary>See Games_Multiplayer_Tag - same slot, win side.</summary>
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

        /// <summary>Count of new card copies granted as post-duel Campaign
        /// rewards (1 per loss, 3 per win, 0 if already at max copies).</summary>
        Cards_Won_Campaign = 42,
    }

    /// <summary>
    /// Reads/writes the Stats save chunk directly against the raw save byte[].
    /// Each slot is 8 bytes (int64), base offset LotdSaveFormat.StatsOffset.
    /// This class uses long end-to-end, so stat values beyond int32 range
    /// round-trip correctly.
    /// </summary>
    public static class StatsLayout
    {
        public const int EntryBytes = SaveLayout.StatEntryBytes; // 8

        /// <summary>Number of named stats (43) - not the same as
        /// GetNumStats(v) (100 on-disk slots).</summary>
        public static int NumKnownStats => Enum.GetValues<StatType>().Length; // 43

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

        /// <summary>Recomputes the 6 campaign counters from scratch by
        /// scanning every real duel slot in CampaignSaveLayout, so this
        /// chunk can never drift. Call after every campaign duel-state
        /// write. AvailableAttempted counts toward Games only; Complete
        /// counts toward both; Locked/Available/AvailableAlt count toward
        /// neither.</summary>
        public static void RecalculateCampaignStats(byte[] save, LotdSaveVersion v)
        {
            long forwardGames = 0, forwardWins = 0, reverseGames = 0, reverseWins = 0;
            var series = CampaignSaveLayout.GetSeries(v);

            for (int s = 0; s < series.Length; s++)
            {
                int start = 0;
                int count = CampaignSaveLayout.DuelsPerSeries;
                if (CampaignSaveLayout.KnownRealDuelRange.TryGetValue(series[s], out var range))
                {
                    start = range.RealStart;
                    count = range.RealCount;
                }

                for (int i = 0; i < count; i++)
                {
                    int d = start + i;
                    var forward = CampaignSaveLayout.GetState(save, v, s, d);
                    var reverse = CampaignSaveLayout.GetReverseState(save, v, s, d);

                    if (forward == LotdCampaignDuelState.Complete) { forwardGames++; forwardWins++; }
                    else if (forward == LotdCampaignDuelState.AvailableAttempted) forwardGames++;

                    if (reverse == LotdCampaignDuelState.Complete) { reverseGames++; reverseWins++; }
                    else if (reverse == LotdCampaignDuelState.AvailableAttempted) reverseGames++;
                }
            }

            Set(save, v, StatType.Games_Campaign_Normal, forwardGames);
            Set(save, v, StatType.Games_Campaign_Reverse, reverseGames);
            Set(save, v, StatType.Games_Campaign, forwardGames + reverseGames);

            Set(save, v, StatType.Wins_Campaign_Normal, forwardWins);
            Set(save, v, StatType.Wins_Campaign_Reverse, reverseWins);
            Set(save, v, StatType.Wins_Campaign, forwardWins + reverseWins);

            RecalculateNonmatchWins(save, v);
        }

        /// <summary>Recomputes Games_Challenge/Wins_Challenge by scanning
        /// every duelist-challenge slot. Failed counts toward Games only;
        /// Complete counts toward both. Call after any challenge-state
        /// write.</summary>
        public static void RecalculateChallengeStats(byte[] save, LotdSaveVersion v)
        {
            long games = 0, wins = 0;
            int n = LotdSaveFormat.GetNumDeckDataSlots(v);

            for (int i = 0; i < n; i++)
            {
                var state = MiscSaveLayout.GetChallengeState(save, v, i);
                if (state == DeulistChallengeState.Complete) { games++; wins++; }
                else if (state == DeulistChallengeState.Failed) games++;
            }

            Set(save, v, StatType.Games_Challenge, games);
            Set(save, v, StatType.Wins_Challenge, wins);

            RecalculateNonmatchWins(save, v);
        }

        /// <summary>Wins_Nonmatch = Campaign wins + Challenge wins (every
        /// non-match win, vs. Wins_Match's best-of-three). Recomputed from
        /// current disk values, called from both RecalculateCampaignStats
        /// and RecalculateChallengeStats.</summary>
        public static void RecalculateNonmatchWins(byte[] save, LotdSaveVersion v)
        {
            long campaignWins = Get(save, v, StatType.Wins_Campaign);
            long challengeWins = Get(save, v, StatType.Wins_Challenge);
            Set(save, v, StatType.Wins_Nonmatch, campaignWins + challengeWins);
            SyncBattlePackUnlocksFromNonmatchWins(save, v);
        }

        /// <summary>
        /// Keeps the 3 real Battle Packs' unlock flags in sync with
        /// Wins_Nonmatch: >=5 unlocks the BattlePack gate + Epic Dawn, >=10
        /// War of the Giants, >=20 War of the Giants Round 2. Unlocks AND
        /// relocks each time, so a drop below a threshold re-locks that
        /// content. Called from RecalculateNonmatchWins and directly from
        /// SaveEditorView.StatValue_LostFocus.
        /// </summary>
        public static void SyncBattlePackUnlocksFromNonmatchWins(byte[] save, LotdSaveVersion v)
        {
            long wins = Get(save, v, StatType.Wins_Nonmatch);

            var content = MiscSaveLayout.GetUnlockedContent(save, v);
            content = wins >= 5 ? content | UnlockedContent.BattlePack : content & ~UnlockedContent.BattlePack;
            MiscSaveLayout.SetUnlockedContent(save, v, content);

            var shopPacks = MiscSaveLayout.GetShopPacks(save, v);
            shopPacks = wins >= 5 ? shopPacks | UnlockedShopPacks.EpicDawn : shopPacks & ~UnlockedShopPacks.EpicDawn;
            MiscSaveLayout.SetShopPacks(save, v, shopPacks);

            var battlePacks = MiscSaveLayout.GetBattlePacksFlag(save, v);
            battlePacks = wins >= 10 ? battlePacks | UnlockedBattlePacks.WarOfTheGiants : battlePacks & ~UnlockedBattlePacks.WarOfTheGiants;
            battlePacks = wins >= 20 ? battlePacks | UnlockedBattlePacks.WarOfTheGiantsRound2 : battlePacks & ~UnlockedBattlePacks.WarOfTheGiantsRound2;
            MiscSaveLayout.SetBattlePacksFlag(save, v, battlePacks);
        }

        /// <summary>Adds <paramref name="by"/> (default 1) to a stat's
        /// current value - for lifetime-running-total counters (currently
        /// just Decks_Created) that can't be recomputed from save state.
        /// Reads-then-adds, so call exactly once per real occurrence.</summary>
        public static void Increment(byte[] save, LotdSaveVersion v, StatType stat, long by = 1) =>
            Set(save, v, stat, Get(save, v, stat) + by);

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
            StatType.Games_Multiplayer_Friendly => "Multiplayer Games (Player Match)",
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
            StatType.Wins_Multiplayer_Friendly => "Multiplayer Wins (Player Match)",
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
            StatType.Cards_Won_Campaign => "Cards Won (Campaign)",
            _ => stat.ToString(),
        };
    }
}
