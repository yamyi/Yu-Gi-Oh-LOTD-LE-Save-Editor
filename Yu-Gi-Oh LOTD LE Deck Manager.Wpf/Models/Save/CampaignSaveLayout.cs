namespace YuGiOhDeckManager.Wpf.Services
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
    ///   +0   8 bytes    reserved/unknown (2x int32)
    ///   then for each duel series (5 or 6, in LotdDuelSeries order):
    ///     duel[0]: 24 bytes (State int32, ReverseState int32, 4x unknown int32)
    ///     +8 bytes reserved/unknown (only right after duel index 0)
    ///     duel[1..49]: 24 bytes each
    ///   i.e. each series occupies 8 + 50*24 = 1208 bytes.
    /// </summary>
    public static class CampaignSaveLayout
    {
        public const int DuelsPerSeries = LotdSaveFormat.DuelsPerSeries;
        private const int DuelRecordBytes = 24;
        private const int SeriesLeadInBytes = 8;   // the 2 unknown ints at the very start of the chunk
        private const int Duel0TrailerBytes = 8;   // the 2 unknown ints right after duel index 0

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

        public static LotdCampaignDuelState GetState(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex) =>
            (LotdCampaignDuelState)ReadI32(save, GetDuelOffset(v, seriesIndex, duelIndex));

        public static void SetState(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex, LotdCampaignDuelState state) =>
            WriteI32(save, GetDuelOffset(v, seriesIndex, duelIndex), (int)state);

        public static LotdCampaignDuelState GetReverseState(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex) =>
            (LotdCampaignDuelState)ReadI32(save, GetDuelOffset(v, seriesIndex, duelIndex) + 4);

        public static void SetReverseState(byte[] save, LotdSaveVersion v, int seriesIndex, int duelIndex, LotdCampaignDuelState state) =>
            WriteI32(save, GetDuelOffset(v, seriesIndex, duelIndex) + 4, (int)state);

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
