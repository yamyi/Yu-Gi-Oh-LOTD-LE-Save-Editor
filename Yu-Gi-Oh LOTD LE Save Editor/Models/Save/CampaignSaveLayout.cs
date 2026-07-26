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
    /// Field layout ported verbatim from pixeltris/Lotd's CampaignSaveData.Load/Save.
    /// Full byte layout: see Documentation/SaveFormat.md.
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

        /// <summary>Real duel ranges per series within the fixed 50-slot
        /// array - first real duel index and count; rest is unused padding.
        /// Single source of truth for Campaign tab UI filtering and stat
        /// recalculation below.</summary>
        public static readonly Dictionary<LotdDuelSeries, (int RealStart, int RealCount)> KnownRealDuelRange = new()
        {
            { LotdDuelSeries.YuGiOh, (1, 32) },
            { LotdDuelSeries.YuGiOhGX, (1, 32) },
            { LotdDuelSeries.YuGiOh5D, (1, 32) },
            { LotdDuelSeries.YuGiOhZEXAL, (1, 26) },
            { LotdDuelSeries.YuGiOhARCV, (1, 33) },
            { LotdDuelSeries.YuGiOhVRAINS, (1, 28) },
        };

        public static LotdDuelSeries[] GetSeries(LotdSaveVersion v) => new[]
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

        /// <summary>Index (LotdDuelSeries value) of the series last played -
        /// a "resume where you left off" bookmark, read-only.</summary>
        public static int GetLastPlayedSeriesIndex(byte[] save, LotdSaveVersion v) =>
            ReadI32(save, LotdSaveFormat.GetCampaignDataOffset(v));

        /// <summary>Duel index (Stage number) last played within
        /// GetLastPlayedSeriesIndex's series. Read-only.</summary>
        public static int GetLastPlayedDuelIndex(byte[] save, LotdSaveVersion v) =>
            ReadI32(save, LotdSaveFormat.GetCampaignDataOffset(v) + 4);

        public static LotdCampaignDuelState GetState(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex) =>
            (LotdCampaignDuelState)ReadI32(save, GetDuelOffset(v, seriesIndex, duelIndex));

        /// <summary>Inclusive random range for the DP value stamped into a
        /// duel's LastDuelPointsAwarded field whenever SetState marks it
        /// Complete.</summary>
        private const int RandomDpRewardMin = 1000;
        private const int RandomDpRewardMax = 1800;

        /// <summary>Sets a duel's forward State. On Complete, also stamps a
        /// fresh random DP value into LastDuelPointsAwarded, re-rolling every
        /// call. Only applies to forward completions.</summary>
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

        /// <summary>DP reward paid the last time this duel was won (bytes
        /// 8-11 of the record). Overwritten on every win; reads 0 if never
        /// won through live gameplay.</summary>
        public static int GetLastDuelPointsAwarded(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex) =>
            ReadI32(save, GetDuelOffset(v, seriesIndex, duelIndex) + DuelPointsAwardedRelOffset);

        /// <summary>Direct setter for LastDuelPointsAwarded, for stamping a
        /// specific value instead of SetState's random one.</summary>
        public static void SetLastDuelPointsAwarded(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex, int value) =>
            WriteI32(save, GetDuelOffset(v, seriesIndex, duelIndex) + DuelPointsAwardedRelOffset, value);

        /// <summary>
        /// Sets every duel (forward, and optionally reverse) in one series to
        /// the given state. Duel indexes 0 and 1 are always forced to at
        /// least Available on a reset - leaving either Locked makes the
        /// whole series unclickable.
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

        /// <summary>Enforces that every real duel before <paramref
        /// name="duelIndex"/> is at least as progressed as the game would
        /// require: forward duels forced to Complete, reverse duels bumped
        /// Locked -> Available only (never downgraded). Reverse duel index 1
        /// is always skipped, since Reverse mode only unlocks after clearing
        /// the entire series forward. Call from
        /// SaveEditorView.DuelStateChanged only.
        ///
        /// Known gap: never touches duel index 0, which the game appears to
        /// use as a per-series "100% complete" flag - SetSeries/SetAllSeries
        /// write it, this per-duel cascade does not.</summary>
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
