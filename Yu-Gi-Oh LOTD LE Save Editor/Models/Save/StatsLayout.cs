namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// The 43 known campaign/duel counters tracked in the Stats chunk (see
    /// SaveLayout's whole-file map — fixed at LotdSaveFormat.StatsOffset,
    /// 0x24/36, in both save formats). Indices 0-41 ported verbatim from
    /// pixeltris/Lotd's StatSaveType enum (Lotd/SaveData/StatSaveData.cs),
    /// which the user's own WinForms reference tool (Arefu/Wolf's
    /// Elroy.SaveStatManager) matches member-for-member and in the same
    /// order — that ordering is exactly the on-disk slot order, so StatType's
    /// integer value IS the stat's index in the chunk (0-based).
    ///
    /// pixeltris' enum has one further "Unknown" 43rd member after
    /// Decks_Created ("There seems to be one additional value. Is this
    /// actually part of the stat data or something separate?"). This project
    /// has since identified it (see Cards_Won_Campaign below) and formally
    /// named it, unlike Elroy's tool which stops at Decks_Created.
    ///
    /// LinkEvolution reserves 100 stat slots total (LotdSaveFormat.GetNumStats),
    /// not just 43 — the same "wider on-disk capacity than known fields" pattern
    /// already seen in CardCollectionLayout (GetNumCardSlots vs GetNumCards).
    /// Slots 43-99 have no known real-world meaning (presumably newer counters
    /// added for Link Evolution's expanded content) and are intentionally left
    /// untouched: StatType/GetKnownStats only expose indices 0-42, and nothing
    /// in this class ever bulk-writes across the full GetNumStats(v) range.
    ///
    /// Cards_Won_Campaign (index 42, right after Decks_Created) - identified
    /// 2026-07-26 via a series of before/after save diffs: it's a count of new
    /// card copies actually granted as a post-duel reward, not a win/score
    /// counter. Per the game's own reward rule a loss normally grants 1
    /// random card and a win grants 3, but nothing is granted for any card
    /// the player already owns the max copies of - confirmed by two
    /// independent loss diffs that left it completely flat (99→99, 105→105,
    /// both with zero CardList bytes touched - the rolled cards were already
    /// maxed) and one win diff that showed +3 (99→102) alongside exactly 3
    /// CardList bytes flipping from 0 to owned. Also confirmed scoped to
    /// duel rewards specifically, not "any new card obtained": an 8-card shop
    /// booster purchase (-200 Duel Points, 8 CardList bytes touched, 7 of
    /// them brand new) left it completely flat. All of this testing was done
    /// exclusively on Yu-Gi-Oh! campaign duels, forward mode - whether
    /// Challenge or Multiplayer wins/losses also move this same slot is
    /// unconfirmed, hence the "_Campaign" in its name; treat that scope as a
    /// working assumption, not a proven fact. Like Decks_Created, it's a
    /// lifetime running total with no way to recompute it from current save
    /// state, so it's intentionally untouched by every Recalculate* method
    /// here - editable directly via the Stats tab same as any other stat, but
    /// nothing in this app derives or resets its value automatically.
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

        /// <summary>Count of new card copies granted as post-duel rewards
        /// (1 per loss, 3 per win, 0 for any card already owned at max copies)
        /// - see this enum's doc comment for the full identification writeup.
        /// Confirmed scoped to Campaign duels specifically; Challenge/
        /// Multiplayer scope is unconfirmed.</summary>
        Cards_Won_Campaign = 42,
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

        /// <summary>
        /// Recomputes the 6 campaign counters (Games_Campaign[_Normal/_Reverse],
        /// Wins_Campaign[_Normal/_Reverse]) from scratch by scanning every real
        /// duel slot in CampaignSaveLayout, rather than incrementing them.
        ///
        /// CampaignSaveLayout (raw per-duel state) and this Stats chunk are
        /// completely separate structures on disk - nothing keeps them in sync
        /// automatically. Editing duel states directly (single dropdown or the
        /// bulk series/all buttons) used to leave the two arbitrarily out of
        /// step, e.g. an entire series flagged Complete while the lifetime
        /// counters here still read 0. That mismatch is the leading suspect for
        /// saves getting stuck on the post-duel results screen after a manually
        /// edited campaign duel is actually played and won.
        ///
        /// Call this after every campaign duel-state write so the two chunks
        /// can never drift apart again. Only real duel slots (per
        /// CampaignSaveLayout.KnownRealDuelRange) are counted - the unused
        /// padding slots that bulk actions also happen to touch are ignored,
        /// same as the Campaign tab's own display filtering.
        ///
        /// AvailableAttempted counts toward Games but not Wins (attempted, not
        /// yet beaten); Complete counts toward both; Locked/Available/
        /// AvailableAlt count toward neither - matching LotdCampaignDuelState's
        /// own doc comments on what each state means in-game.
        /// </summary>
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

        /// <summary>
        /// Recomputes Games_Challenge/Wins_Challenge from scratch by scanning
        /// every duelist-challenge slot (MiscSaveLayout's Challenges array, one
        /// DeulistChallengeState per deck-data slot) - same
        /// recompute-not-increment approach as RecalculateCampaignStats, and
        /// for the same reason: the bulk Unlock/Complete/Reset Challenges
        /// buttons only ever wrote MiscSaveLayout.SetAllChallenges, never
        /// touching this Stats chunk. Failed counts toward Games only
        /// (attempted, not won); Complete counts toward both; Locked/Available
        /// count toward neither.
        ///
        /// Call this after any challenge-state write (currently only the bulk
        /// Unlock/Complete/Reset Challenges buttons - there's no per-challenge
        /// editor yet) so Games_Challenge/Wins_Challenge - and, transitively,
        /// Wins_Nonmatch - never drift from the actual challenge states.
        /// </summary>
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

        /// <summary>
        /// Wins_Nonmatch is defined as Campaign wins + Challenge wins combined
        /// (i.e. every non-match win - as opposed to Wins_Match, which is
        /// specifically best-of-three). Recomputed from whichever of
        /// Wins_Campaign/Wins_Challenge is currently on disk rather than
        /// incremented directly, so it's called from both
        /// RecalculateCampaignStats and RecalculateChallengeStats and can
        /// never end up out of step with either one.
        /// </summary>
        public static void RecalculateNonmatchWins(byte[] save, LotdSaveVersion v)
        {
            long campaignWins = Get(save, v, StatType.Wins_Campaign);
            long challengeWins = Get(save, v, StatType.Wins_Challenge);
            Set(save, v, StatType.Wins_Nonmatch, campaignWins + challengeWins);
            SyncBattlePackUnlocksFromNonmatchWins(save, v);
        }

        /// <summary>
        /// Keeps the 3 real Battle Packs' unlock flags (plus the general
        /// "Battle Pack" content gate) in sync with Wins_Nonmatch, per the
        /// user's own milestone thresholds (2026-07-25):
        ///   - Wins_Nonmatch >= 5:  unlock the Battle Pack content gate
        ///     (UnlockedContent.BattlePack) AND Epic Dawn (UnlockedShopPacks.
        ///     EpicDawn - its real flag lives in ShopPacks despite being a
        ///     "Battle Pack" in-game, see that enum's doc comment).
        ///   - Wins_Nonmatch >= 10: unlock War of the Giants
        ///     (UnlockedBattlePacks.WarOfTheGiants).
        ///   - Wins_Nonmatch >= 20: unlock War of the Giants Round 2
        ///     (UnlockedBattlePacks.WarOfTheGiantsRound2).
        /// Unlock AND relock: each flag is set to exactly match its threshold
        /// every time, so a Wins_Nonmatch drop below a threshold (e.g. via
        /// Campaign/Challenge state edits recomputing it lower) re-locks that
        /// content again - same unlock-and-relock behavior requested for the
        /// original per-pack version of this, just re-keyed off Wins_Nonmatch.
        ///
        /// Called automatically from RecalculateNonmatchWins (itself called
        /// by RecalculateCampaignStats/RecalculateChallengeStats), and
        /// directly from SaveEditorView.StatValue_LostFocus when the user
        /// edits the Wins_Nonmatch row by hand - so this can never drift from
        /// whatever Wins_Nonmatch currently reads, regardless of how it got
        /// there.
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

        /// <summary>Adds <paramref name="by"/> (default 1) to a stat's current
        /// value, for the handful of counters - currently just Decks_Created -
        /// that track a lifetime running total rather than something
        /// recomputable from the save's current state (a cleared/overwritten
        /// slot doesn't erase the fact that a deck was created there at some
        /// point). Unlike the Recalculate* methods above, this reads-then-adds,
        /// so callers must only invoke it exactly once per real occurrence.</summary>
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
            StatType.Cards_Won_Campaign => "Cards Won (Campaign)",
            _ => stat.ToString(),
        };
    }
}
