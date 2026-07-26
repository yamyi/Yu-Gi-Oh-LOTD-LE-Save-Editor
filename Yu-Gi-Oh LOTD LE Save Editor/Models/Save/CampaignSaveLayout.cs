namespace YuGiOhSaveEditor.Services
{
    /// <summary>Values copied verbatim from pixeltris/Lotd's DuelSeries enum.</summary>
    public enum LotdDuelSeries
    {
        YuGiOh = 0,
        YuGiOhGX = 1,
        YuGiOh5D = 2,
        YuGiOhZEXAL = 3,
        YuGiOhARCV = 4,
        YuGiOhVRAINS = 5
    }

    /// <summary>Values copied verbatim from pixeltris/Lotd's CampaignDuelState enum.</summary>
    public enum LotdCampaignDuelState
    {
        /// <summary>Locked / unavailable (padlock icon, no '!' mark).</summary>
        Locked = 0,
        /// <summary>Available, not yet attempted ('!' mark, characters blacked out).</summary>
        Available = 1,
        /// <summary>Available, attempt made but not completed (characters visible, '!' mark).</summary>
        AvailableAttempted = 2,
        /// <summary>Completed (characters visible, no '!' mark).</summary>
        Complete = 3,
        /// <summary>Available, alternate presentation. Anything 4+ behaves the same.</summary>
        AvailableAlt = 4,
    }

    /// <summary>
    /// Reads/writes the "Campaign" save chunk directly against the raw save byte[].
    /// Field layout ported verbatim from pixeltris/Lotd's CampaignSaveData.Load/Save
    /// (Lotd/SaveData/CampaignSaveData.cs).
    ///
    /// Byte layout, relative to LotdSaveFormat.GetCampaignDataOffset(version):
    ///   +0   8 bytes    chunk-wide lead-in (2x int32) - "last played" bookmark,
    ///                    not a counter (corrected 2026-07-25 - see below):
    ///                      +0: index of the series last played (LotdDuelSeries
    ///                          value - stayed 0/YuGiOh across a before/after
    ///                          diff where only YuGiOh duels were played)
    ///                      +4: index (Stage number/duelIndex) of the duel last
    ///                          played within that series - user confirmed this
    ///                          is why it read 31→32 in that same diff: the
    ///                          last duel played really was Stage 32, not a
    ///                          running total of duels won. Likely just a
    ///                          "resume where you left off" bookmark the game
    ///                          uses for its own UI, not an achievement gate.
    ///                    Never touched by any editor write path - GetState/
    ///                    SetState/EnsurePrerequisitesComplete only ever write
    ///                    individual duel records, not this lead-in.
    ///   then for each duel series (5 or 6, in LotdDuelSeries order):
    ///     duel[0]: 24 bytes - State int32 (+0), ReverseState int32 (+4),
    ///       LastDuelPointsAwarded int32 (+8, see GetLastDuelPointsAwarded
    ///       below - DP paid the last time this duel was won),
    ///       3x unknown int32 (+12/+16/+20)
    ///     +8 bytes reserved/unknown (only right after duel index 0)
    ///     duel[1..49]: same 24-byte layout each
    ///   i.e. each series occupies 8 + 50*24 = 1208 bytes.
    /// </summary>
    public static class CampaignSaveLayout
    {
        public const int DuelsPerSeries = LotdSaveFormat.DuelsPerSeries;
        private const int DuelRecordBytes = 24;
        private const int SeriesLeadInBytes = 8;   // the 2 unknown ints at the very start of the chunk
        private const int Duel0TrailerBytes = 8;   // the 2 unknown ints right after duel index 0

        /// <summary>Offset, relative to a duel record's start (i.e. relative to
        /// GetDuelOffset's return value), of the DP-reward-paid marker - see
        /// GetDuelPointsAwarded.</summary>
        private const int DuelPointsAwardedRelOffset = 8;

        /// <summary>Confirmed-real duel ranges, per series, within the fixed
        /// 50-slot save array - 0-based index of the first real duel, and how
        /// many real duels follow it. Everything outside that range is unused
        /// padding (still writable, but not a duel the game ever actually
        /// shows or plays). Series not listed here should be treated as the
        /// full unfiltered 50. Single source of truth for both the Campaign
        /// tab's UI filtering (SaveEditorView) and stat recalculation below -
        /// ported verbatim from the WinForms build's
        /// SaveEditorPage.KnownRealDuelRange.</summary>
        public static readonly Dictionary<LotdDuelSeries, (int RealStart, int RealCount)> KnownRealDuelRange = new()
        {
            { LotdDuelSeries.YuGiOh, (1, 32) },
            { LotdDuelSeries.YuGiOhGX, (1, 32) },
            { LotdDuelSeries.YuGiOh5D, (1, 32) },
            { LotdDuelSeries.YuGiOhZEXAL, (1, 26) },
            { LotdDuelSeries.YuGiOhARCV, (1, 33) },
            { LotdDuelSeries.YuGiOhVRAINS, (1, 28) },
        };

        public static LotdDuelSeries[] GetSeries(LotdSaveVersion v) =>
            v == LotdSaveVersion.Lotd
                ? new[]
                {
                    LotdDuelSeries.YuGiOh, LotdDuelSeries.YuGiOhGX, LotdDuelSeries.YuGiOh5D,
                    LotdDuelSeries.YuGiOhZEXAL, LotdDuelSeries.YuGiOhARCV,
                }
                : new[]
                {
                    LotdDuelSeries.YuGiOh, LotdDuelSeries.YuGiOhGX, LotdDuelSeries.YuGiOh5D,
                    LotdDuelSeries.YuGiOhZEXAL, LotdDuelSeries.YuGiOhARCV, LotdDuelSeries.YuGiOhVRAINS,
                };

        /// <summary>Absolute byte offset of a given series+duel's 24-byte record.</summary>
        public static int GetDuelOffset(LotdSaveVersion v, int seriesIndex, int duelIndex)
        {
            int pos = LotdSaveFormat.GetCampaignDataOffset(v) + SeriesLeadInBytes;
            pos += seriesIndex * (DuelsPerSeries * DuelRecordBytes + Duel0TrailerBytes);
            pos += duelIndex * DuelRecordBytes;
            if (duelIndex > 0) pos += Duel0TrailerBytes;
            return pos;
        }

        /// <summary>
        /// Index (LotdDuelSeries value) of the series the player last played a
        /// duel in - first int32 of the chunk-wide lead-in. Read-only/purely
        /// observational: this looks like a "resume where you left off"
        /// bookmark the game maintains for its own UI, not something the
        /// editor should ever need to set. See the class doc comment for how
        /// this was pinned down.
        /// </summary>
        public static int GetLastPlayedSeriesIndex(byte[] save, LotdSaveVersion v) =>
            ReadI32(save, LotdSaveFormat.GetCampaignDataOffset(v));

        /// <summary>
        /// Duel index (Stage number) of the duel the player last played,
        /// within GetLastPlayedSeriesIndex's series - second int32 of the
        /// chunk-wide lead-in. Read-only/observational, same reasoning as
        /// GetLastPlayedSeriesIndex.
        /// </summary>
        public static int GetLastPlayedDuelIndex(byte[] save, LotdSaveVersion v) =>
            ReadI32(save, LotdSaveFormat.GetCampaignDataOffset(v) + 4);

        public static LotdCampaignDuelState GetState(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex) =>
            (LotdCampaignDuelState)ReadI32(save, GetDuelOffset(v, seriesIndex, duelIndex));

        /// <summary>Inclusive random range (2026-07-25 experiment) for the DP
        /// value stamped into a duel's LastDuelPointsAwarded field every time
        /// SetState marks it Complete - see SetState's doc comment.</summary>
        private const int RandomDpRewardMin = 1000;
        private const int RandomDpRewardMax = 1800;

        /// <summary>
        /// Sets a duel's forward State. Whenever <paramref name="state"/> is
        /// Complete, this also stamps a fresh random value in
        /// [RandomDpRewardMin, RandomDpRewardMax] into that duel's
        /// LastDuelPointsAwarded field (bytes 8-11 of the record) -
        /// 2026-07-25 experiment to test whether populating that field (which
        /// real gameplay always sets on a win, but editor-completions never
        /// did before now) changes how the game/Steam treats an
        /// editor-completed duel. Re-rolls every call, even if the duel was
        /// already Complete - this intentionally does NOT try to detect
        /// "no-op, already Complete" and skip the reroll, since the point is
        /// to guarantee the field is never left at 0 for any duel this
        /// method ever marks Complete, via any caller (single-duel dropdown,
        /// bulk SetSeries/SetAllSeries, EnsurePrerequisitesComplete's
        /// cascade). Only applies to forward completions - SetReverseState
        /// is left untouched since the confirmed field only came from a
        /// forward-duel win; whether reverse completions should get their
        /// own reward stamp (same field, or one of the 3 still-unknown
        /// trailing int32s) is unconfirmed.
        /// </summary>
        public static void SetState(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex, LotdCampaignDuelState state)
        {
            WriteI32(save, GetDuelOffset(v, seriesIndex, duelIndex), (int)state);
            if (state == LotdCampaignDuelState.Complete)
            {
                int reward = Random.Shared.Next(RandomDpRewardMin, RandomDpRewardMax + 1); // Next's upper bound is exclusive
                SetLastDuelPointsAwarded(save, v, seriesIndex, duelIndex, reward);
            }
        }

        public static LotdCampaignDuelState GetReverseState(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex) =>
            (LotdCampaignDuelState)ReadI32(save, GetDuelOffset(v, seriesIndex, duelIndex) + 4);

        public static void SetReverseState(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex, LotdCampaignDuelState state) =>
            WriteI32(save, GetDuelOffset(v, seriesIndex, duelIndex) + 4, (int)state);

        /// <summary>
        /// The DP reward the game paid out the last time this specific duel
        /// was won - the first of the 4 "unknown int32" fields in each
        /// 24-byte duel record (bytes 8-11). Confirmed 2026-07-25: a
        /// before/after byte-diff of a real duel win showed this field go
        /// 0 → 1003, exactly matching that session's Duel Points gain
        /// (MiscSaveLayout.GetDuelPoints increased by exactly 1003); user
        /// then confirmed by direct testing that it tracks "last reward
        /// paid," i.e. it gets overwritten every time the duel is won again
        /// (not a one-time "first win" flag - replays update it too). Reads
        /// 0 for any duel never won through live gameplay. As of 2026-07-25,
        /// SetState now also stamps a random value in here whenever it marks
        /// a duel Complete (see SetState's doc comment) - an experiment to
        /// test whether populating this field changes how editor-completed
        /// duels are treated in-game. SetReverseState/EnsurePrerequisitesComplete's
        /// reverse-duel bumps still never touch it.
        /// </summary>
        public static int GetLastDuelPointsAwarded(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex) =>
            ReadI32(save, GetDuelOffset(v, seriesIndex, duelIndex) + DuelPointsAwardedRelOffset);

        /// <summary>Direct setter for LastDuelPointsAwarded - see
        /// GetLastDuelPointsAwarded. Used by SetState's Complete-stamping
        /// logic; exposed publicly in case a caller wants to stamp a specific
        /// value rather than the random one SetState rolls.</summary>
        public static void SetLastDuelPointsAwarded(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex, int value) =>
            WriteI32(save, GetDuelOffset(v, seriesIndex, duelIndex) + DuelPointsAwardedRelOffset, value);

        /// <summary>
        /// Sets every duel (forward, and optionally reverse) in one series to the
        /// given state. Duel indexes 0 and 1 are always forced to at least
        /// Available on a reset — index 0 is an unused padding slot that still
        /// has to stay Available for the series to be clickable at all, and
        /// index 1 is the real first duel of the series (confirmed in-game);
        /// leaving either Locked makes the whole series unclickable.
        /// </summary>
        public static void SetSeries(byte[] save, LotdSaveVersion v, int seriesIndex, LotdCampaignDuelState state,
            LotdCampaignDuelState? reverseState = null)
        {
            for (int d = 0; d < DuelsPerSeries; d++)
            {
                LotdCampaignDuelState forward = (d == 0 || d == 1) && state == LotdCampaignDuelState.Locked
                    ? LotdCampaignDuelState.Available
                    : state;
                SetState(save, v, seriesIndex, d, forward);
                if (reverseState.HasValue)
                    SetReverseState(save, v, seriesIndex, d, reverseState.Value);
            }
        }

        /// <summary>Applies SetSeries to every series in the save (whole-campaign bulk action).</summary>
        public static void SetAllSeries(byte[] save, LotdSaveVersion v, LotdCampaignDuelState state,
            LotdCampaignDuelState? reverseState = null)
        {
            var series = GetSeries(v);
            for (int s = 0; s < series.Length; s++)
                SetSeries(save, v, s, state, reverseState);
        }

        /// <summary>
        /// Enforces that every real duel before <paramref name="duelIndex"/>
        /// in a series is at least as progressed as the game would require
        /// to have reached <paramref name="duelIndex"/> at all - user-specified
        /// rule (2026-07-25): "if a campaign normal or reverse duel is
        /// marked as anything besides locked, all the campaign duels before
        /// it should be complete and their reverse to available." Forward
        /// duels before it are forced to Complete unconditionally (any
        /// lesser forward state there would be an impossible real-game state
        /// once a later duel is reachable at all); reverse duels before it
        /// are bumped from Locked to Available only - never downgraded if
        /// already further along (Complete, etc.), so this can never erase
        /// real recorded reverse-mode progress.
        ///
        /// Reverse duel index 1 (the series' very first real duel) is always
        /// skipped - user-specified exception: "this does not apply to the
        /// first reverse duel of each series since it is always locked" (in
        /// the real game, Reverse mode as a whole only unlocks after
        /// clearing the ENTIRE series forward, not after any single early
        /// duel, so this simple per-duel cascade can't correctly model when
        /// it should unlock and deliberately leaves it alone - see also
        /// DuelRowViewModel.ShowReverse, which hides its ComboBox in the UI
        /// for the same reason).
        ///
        /// Call from SaveEditorView.DuelStateChanged only (the single
        /// per-duel dropdown edit) - the bulk series/all buttons already set
        /// every duel in their range to one uniform state and have their own
        /// well-understood semantics ("Unlock" = Available everywhere,
        /// "Complete" = Complete everywhere, "Reset" = Locked everywhere),
        /// which this cascade must not interfere with.
        /// </summary>
        /// <summary>
        /// KNOWN GAP (2026-07-26, not yet acted on): this loop starts at i=1
        /// and never touches duel index 0. A before/after diff of a real,
        /// live campaign completion showed duel index 0's own State field
        /// (GetState(v, seriesIndex, 0)) flip from AvailableAttempted(2) to
        /// Complete(3) at the exact moment the series' last real duel was
        /// won - moving in lockstep with the whole series finishing, not with
        /// any single duel. That strongly suggests index 0 (documented above
        /// as "unused padding") is actually used by the game as a per-series
        /// "is this series 100% complete" flag using the same State enum,
        /// not dead padding. SetSeries/SetAllSeries already happen to write
        /// it (their d==0 special-case only forces it to Available on a
        /// Locked reset, otherwise it takes whatever bulk state was picked),
        /// but this per-duel cascade - the path a single-dropdown edit takes
        /// - does not, so completing a series one duel at a time through the
        /// dropdown can leave this flag stuck at a stale value even once
        /// every real duel reads Complete. Left un-wired for now pending a
        /// decision on whether to bump it here too once <paramref
        /// name="duelIndex"/> reaches the series' last real duel.
        /// </summary>
        public static void EnsurePrerequisitesComplete(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex)
        {
            for (int i = 1; i < duelIndex; i++)
            {
                SetState(save, v, seriesIndex, i, LotdCampaignDuelState.Complete);

                if (i == 1) continue; // first reverse duel is always locked - see doc comment above

                if (GetReverseState(save, v, seriesIndex, i) == LotdCampaignDuelState.Locked)
                    SetReverseState(save, v, seriesIndex, i, LotdCampaignDuelState.Available);
            }
        }

        private static int ReadI32(byte[] b, int offset)
        {
            if (offset < 0 || offset + 4 > b.Length) return 0;
            return b[offset] | (b[offset + 1] << 8) | (b[offset + 2] << 16) | (b[offset + 3] << 24);
        }

        private static void WriteI32(byte[] b, int offset, int value)
        {
            if (offset < 0 || offset + 4 > b.Length) return;
            b[offset] = (byte)(value & 0xFF);
            b[offset + 1] = (byte)((value >> 8) & 0xFF);
            b[offset + 2] = (byte)((value >> 16) & 0xFF);
            b[offset + 3] = (byte)((value >> 24) & 0xFF);
        }
    }
}
