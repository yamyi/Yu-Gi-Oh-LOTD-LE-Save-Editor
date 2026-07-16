namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services
{
    /// <summary>
    /// Reads/writes the "CardList" save chunk directly against the raw save byte[] —
    /// one byte per card slot, tracking how many copies are owned and whether the
    /// card has been "seen" (a false value here is what puts the "NEW" ribbon on a
    /// card in the in-game collection). Ported verbatim from pixeltris/Lotd's
    /// CardListSaveData / CardState struct (Lotd/SaveData/CardListSaveData.cs).
    ///
    /// IMPORTANT: the index used here is the card's position in this byte array,
    /// which is NOT the same id used elsewhere in the save (deck slot card ids) or
    /// in this project's Cards.json (`lotd_id`). The game derives that mapping from
    /// its own installed archive files (CARD_Indx_*, CARD_Prop.bin), which aren't
    /// available to this project — so per-card editing by name isn't possible here.
    /// Only bulk operations (unlock everything, set every count, etc.) are exposed,
    /// matching the scope of pixeltris/Lotd's own SetAllOwnedCardsCount helper.
    /// </summary>
    public static class CardCollectionLayout
    {
        public static int GetNumCards(LotdSaveVersion v) => LotdSaveFormat.GetNumCards(v);

        /// <summary>Raw byte layout: bits 0-2 = owned count (0-3), bit 3 = seen, bits 4-7 = unknown.</summary>
        public static byte GetRawValue(byte[] save, LotdSaveVersion v, int cardIndex)
        {
            int offset = LotdSaveFormat.GetCardListOffset(v) + cardIndex;
            if ((uint)offset >= (uint)save.Length) return 0;
            return save[offset];
        }

        public static void SetRawValue(byte[] save, LotdSaveVersion v, int cardIndex, byte rawValue)
        {
            int offset = LotdSaveFormat.GetCardListOffset(v) + cardIndex;
            if ((uint)offset >= (uint)save.Length) return;
            save[offset] = rawValue;
        }

        public static byte GetCount(byte[] save, LotdSaveVersion v, int cardIndex) =>
            (byte)(GetRawValue(save, v, cardIndex) & 7);

        public static bool GetSeen(byte[] save, LotdSaveVersion v, int cardIndex) =>
            ((GetRawValue(save, v, cardIndex) >> 3) & 1) != 0;

        public static void SetCount(byte[] save, LotdSaveVersion v, int cardIndex, byte count)
        {
            byte raw = GetRawValue(save, v, cardIndex);
            raw &= 0xF8;
            raw |= (byte)(count & 7);
            SetRawValue(save, v, cardIndex, raw);
        }

        public static void SetSeen(byte[] save, LotdSaveVersion v, int cardIndex, bool seen)
        {
            byte raw = GetRawValue(save, v, cardIndex);
            raw &= 0xF7;
            if (seen) raw |= 1 << 3;
            SetRawValue(save, v, cardIndex, raw);
        }

        /// <summary>Sets every card's owned count and seen flag at once (leaves the unknown bits at 0).</summary>
        public static void SetAllOwnedCardsCount(byte[] save, LotdSaveVersion v, byte count, bool seen = true)
        {
            int offset = LotdSaveFormat.GetCardListOffset(v);
            int n = GetNumCards(v);
            byte raw = (byte)((count & 7) | (seen ? 1 << 3 : 0));
            for (int i = 0; i < n && offset + i < save.Length; i++)
                save[offset + i] = raw;
        }

        /// <summary>Matches pixeltris/Lotd's UnlockAllCards() — 3 copies of everything, marked seen.</summary>
        public static void UnlockAllCards(byte[] save, LotdSaveVersion v) =>
            SetAllOwnedCardsCount(save, v, 3, true);

        /// <summary>Marks every card as seen without touching owned counts (clears all "NEW" ribbons).</summary>
        public static void MarkAllSeen(byte[] save, LotdSaveVersion v)
        {
            int n = GetNumCards(v);
            for (int i = 0; i < n; i++)
                SetSeen(save, v, i, true);
        }

        /// <summary>Resets the entire collection to unowned/unseen.</summary>
        public static void ResetAllCards(byte[] save, LotdSaveVersion v)
        {
            int offset = LotdSaveFormat.GetCardListOffset(v);
            int n = GetNumCards(v);
            for (int i = 0; i < n && offset + i < save.Length; i++)
                save[offset + i] = 0;
        }
    }
}
