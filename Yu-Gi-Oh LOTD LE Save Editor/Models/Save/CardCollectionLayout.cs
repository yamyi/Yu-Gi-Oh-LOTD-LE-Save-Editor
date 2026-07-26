namespace YuGiOhSaveEditor.Services
{
    /// <summary>Reads/writes the "CardList" save chunk: one byte per card
    /// slot, tracking copies owned and whether "seen" (false puts the "NEW"
    /// ribbon on a card in-game). The index here is the card's position in
    /// this byte array, matching Assets/cards/Cards.json's `lotd_id` field -
    /// not the id used elsewhere in the save (deck slot card ids).</summary>
    public static class CardCollectionLayout
    {
        /// <summary>Owned-count is bits 0-2 of the raw byte; the game never
        /// lets a card's count exceed 3 even though 3 bits could hold 7.</summary>
        public const byte MaxCountPerCard = 3;

        public static int GetNumCards(LotdSaveVersion v) => LotdSaveFormat.GetNumCards(v);

        /// <summary>Total byte slots in the CardList chunk (20000) - the
        /// file reserves room beyond the real card total (10027), and real
        /// cards are scattered across the full range, not a leading prefix.
        /// Bulk operations use this wider bound; GetNumCards is only ever
        /// the display denominator.</summary>
        public static int GetNumCardSlots(LotdSaveVersion v) => LotdSaveFormat.GetCardListSize(v);

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

        /// <summary>Sets every card's owned count and seen flag at once (leaves the unknown bits at 0).
        /// Covers the full CardList chunk (GetNumCardSlots), not just GetNumCards -
        /// see GetNumCardSlots for why.</summary>
        public static void SetAllOwnedCardsCount(byte[] save, LotdSaveVersion v, byte count, bool seen = true)
        {
            int offset = LotdSaveFormat.GetCardListOffset(v);
            int n = GetNumCardSlots(v);
            byte raw = (byte)((count & 7) | (seen ? 1 << 3 : 0));
            for (int i = 0; i < n && offset + i < save.Length; i++)
                save[offset + i] = raw;
        }

        /// <summary>3 copies of everything in the CardList chunk, marked
        /// seen. NOT used by the Cards tab's "Unlock All Cards" button (see
        /// SetCardsCount) - touches every padding slot too.</summary>
        public static void UnlockAllCards(byte[] save, LotdSaveVersion v) =>
            SetAllOwnedCardsCount(save, v, 3, true);

        /// <summary>Sets count/seen on exactly the given card indices,
        /// leaving every other slot untouched. What the Cards tab's "Unlock
        /// All Cards" button calls, passing AppContext.CardDb.AllLotdIds() -
        /// unlike UnlockAllCards, this doesn't touch unmapped padding
        /// slots.</summary>
        public static void SetCardsCount(byte[] save, LotdSaveVersion v, IEnumerable<int> cardIndices, byte count, bool seen)
        {
            foreach (int i in cardIndices)
            {
                SetCount(save, v, i, count);
                if (seen) SetSeen(save, v, i, true);
            }
        }

        /// <summary>Sets the owned copy count on every already-owned card
        /// only - unowned slots stay at 0, existing Seen flags are
        /// preserved. Unlike SetAllOwnedCardsCount, doesn't turn unowned
        /// slots "owned."</summary>
        public static void SetOwnedCardsCount(byte[] save, LotdSaveVersion v, byte count)
        {
            int n = GetNumCardSlots(v);
            for (int i = 0; i < n; i++)
                if (GetCount(save, v, i) > 0)
                    SetCount(save, v, i, count);
        }

        /// <summary>Marks every card as seen without touching owned counts (clears all "NEW" ribbons).
        /// Covers the full CardList chunk - see GetNumCardSlots.</summary>
        public static void MarkAllSeen(byte[] save, LotdSaveVersion v)
        {
            int n = GetNumCardSlots(v);
            for (int i = 0; i < n; i++)
                SetSeen(save, v, i, true);
        }

        /// <summary>Resets the entire collection to unowned/unseen. Covers the full
        /// CardList chunk - see GetNumCardSlots.</summary>
        public static void ResetAllCards(byte[] save, LotdSaveVersion v)
        {
            int offset = LotdSaveFormat.GetCardListOffset(v);
            int n = GetNumCardSlots(v);
            for (int i = 0; i < n && offset + i < save.Length; i++)
                save[offset + i] = 0;
        }
    }
}
