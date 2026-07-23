namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Reads/writes the "CardList" save chunk directly against the raw save byte[] —
    /// one byte per card slot, tracking how many copies are owned and whether the
    /// card has been "seen" (a false value here is what puts the "NEW" ribbon on a
    /// card in the in-game collection). Ported verbatim from pixeltris/Lotd's
    /// CardListSaveData / CardState struct (Lotd/SaveData/CardListSaveData.cs).
    ///
    /// IMPORTANT: the index used here is the card's position in this byte array,
    /// which is NOT the same id used elsewhere in the save (deck slot card ids).
    /// It DOES, however, match this project's own Assets/cards/Cards.json
    /// `lotd_id` field (confirmed 2026-07-23: a real save's owned card indices
    /// and Cards.json's lotd_id values both span ~4007-14968) - so per-card
    /// editing by name is actually possible via that field, contrary to what
    /// this comment used to claim. Only bulk operations (unlock everything,
    /// set every count, etc.) are exposed for now though, matching the scope
    /// of pixeltris/Lotd's own SetAllOwnedCardsCount helper - per-card editing
    /// would be new work, not implemented here yet.
    /// </summary>
    public static class CardCollectionLayout
    {
        public static int GetNumCards(LotdSaveVersion v) => LotdSaveFormat.GetNumCards(v);

        /// <summary>
        /// Total byte slots in the CardList chunk (CardListOffset..EOF), i.e. what
        /// pixeltris/Lotd calls GetNumCards2() - the game reads/writes this many
        /// slots on every load/save, not just GetNumCards(v). For LinkEvolution
        /// this is 20000 vs GetNumCards()'s 10027: the file reserves room for
        /// future cards beyond the real card total. Bulk operations below use
        /// this wider bound so "unlock all"/"reset all"/"mark all seen" actually
        /// cover every card the game itself considers part of the collection,
        /// instead of silently stopping short.
        ///
        /// IMPORTANT: real (non-padding) card slots are NOT the first
        /// GetNumCards(v) indices - a real save with every card owned (3x) had
        /// its owned slots scattered across indices ~4007-14964 (353 separate
        /// runs), confirmed 2026-07-23 (see this class's top doc comment for
        /// the Cards.json `lotd_id` mapping that covers the same index range),
        /// so any UI/logic that needs an "X owned out of Y" figure must scan
        /// the FULL GetNumCardSlots range for X and use GetNumCards(v) only as
        /// the Y denominator - never assume real cards live in a
        /// 0..GetNumCards-1 prefix.
        /// </summary>
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

        /// <summary>3 copies of everything in the CardList chunk, marked seen.
        /// NOT used by the Cards tab's "Unlock All Cards" button (see
        /// SetCardsCount below and its doc comment) - kept only as the raw
        /// pixeltris/Lotd-equivalent primitive, since it touches every
        /// padding slot in the chunk too.</summary>
        public static void UnlockAllCards(byte[] save, LotdSaveVersion v) =>
            SetAllOwnedCardsCount(save, v, 3, true);

        /// <summary>Sets count/seen on exactly the given card indices,
        /// leaving every other slot untouched. This is what the Cards tab's
        /// "Unlock All Cards" button actually calls, passing
        /// AppContext.CardDb.AllLotdIds() - restricting to real (Cards.json-
        /// mapped) slots rather than the full GetNumCardSlots chunk. Using
        /// UnlockAllCards/SetAllOwnedCardsCount instead would set every one of
        /// the ~9973 unmapped padding slots to owned too, which used to make
        /// the Cards tab's counter read e.g. "20000/10027" after unlocking -
        /// confirmed as a bug 2026-07-23 now that Cards.json maps all 10027
        /// real cards (see CardDatabase.AllLotdIds).</summary>
        public static void SetCardsCount(byte[] save, LotdSaveVersion v, IEnumerable<int> cardIndices, byte count, bool seen)
        {
            foreach (int i in cardIndices)
            {
                SetCount(save, v, i, count);
                if (seen) SetSeen(save, v, i, true);
            }
        }

        /// <summary>Sets the owned copy count on every ALREADY-owned card
        /// only - unowned slots (count == 0) are left untouched at 0, and
        /// each touched card's existing Seen flag is preserved rather than
        /// forced on. This is the real "set every owned card's count" ask:
        /// SetAllOwnedCardsCount (above) touches every slot in the chunk
        /// regardless of current ownership, which also turns previously-
        /// unowned padding/real-but-unowned slots "owned" - fine for "unlock
        /// all cards" (which wants exactly that), wrong for bulk-editing the
        /// count of cards you already have. Covers the full GetNumCardSlots
        /// range - see GetNumCardSlots for why real cards can't be assumed to
        /// sit in a 0..GetNumCards-1 prefix.</summary>
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
