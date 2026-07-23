namespace YuGiOhSaveEditor.Services;

/// <summary>
/// The actual LOTD:LE-relevant TCG banlist, keyed by lotd_id - the game's
/// own internal card id, the same value SlotIO stores in each save-file
/// deck slot and CardDatabase.GetByLotdId/Card.LotdId resolve by. Supplied
/// directly by the user as the authoritative list, replacing the earlier
/// approach of trusting each card's generic Cards.json banlist_info.ban_tcg
/// field (which is keyed by YGOPRODeck passcode, not lotd_id, and doesn't
/// necessarily reflect this specific format/version's list).
///
/// DeckLegalityChecker calls GetStatus(card.LotdId) for every card in a
/// deck and treats the three tiers exactly as before: a Forbidden card
/// anywhere in the deck is illegal, more than 1 Limited copy is illegal,
/// more than 2 Semi-Limited copies is illegal.
/// </summary>
public static class LotdBanlist
{
    public static readonly HashSet<int> Forbidden = new()
    {
        4518, 4678, 4812, 4844, 4851, 4860, 4885, 4891, 4898, 4900,
        4901, 4907, 4910, 4911, 4913, 4966, 4984, 5123, 5127, 5195,
        5285, 5371, 5375, 5399, 5406, 5433, 5446, 5477, 5539, 5605,
        5657, 5671, 5724, 5906, 5916, 5945, 6078, 6444, 6458, 6654,
        6708, 7386, 7407, 7435, 7557, 7601, 7714, 8085, 8090, 8159,
        8318, 8440, 8472, 8733, 9095, 9153, 9455, 9742, 9821, 9860,
        9874, 9918, 9957, 10480, 10519, 10520, 10521, 10544, 11022, 11159,
        11173, 11207, 11296, 11518, 11556, 11651, 11725, 11840, 11851, 11938,
        11960, 11973, 12111, 12354, 12412, 12441, 12638, 12786, 12788, 12801,
        12906, 12908, 12938, 13077, 13082, 13507, 13554, 13597, 13598, 13671,
        13836, 13903, 13923, 14054, 14130, 14248, 14285,
    };

    public static readonly HashSet<int> Limited = new()
    {
        4023, 4024, 4025, 4026, 4027, 4343, 4426, 4597, 4818, 4821,
        4842, 4895, 4960, 5236, 5328, 5530, 5537, 5576, 5740, 5788,
        5846, 5990, 6042, 6075, 6161, 6511, 6615, 6682, 6707, 6904,
        7187, 7384, 7423, 7452, 7561, 7568, 7747, 7970, 8197, 8272,
        8602, 8709, 8823, 8824, 9178, 9256, 9271, 9350, 9597, 9778,
        9822, 10001, 10458, 10463, 10522, 11318, 11319, 11486, 11500, 11809,
        11834, 11940, 12047, 12074, 12094, 12108, 12169, 12466, 12689, 12690,
        12749, 12754, 12794, 12827, 12855, 12907, 12933, 12936, 12967, 12978,
        12983, 13011, 13030, 13092, 13107, 13292, 13419, 13463, 13508, 13616,
        13622, 13668, 13674, 13675, 13679, 13814, 13896, 13907, 13979, 14240,
        14244, 14314, 14442,
    };

    public static readonly HashSet<int> SemiLimited = new()
    {
        4817, 6799, 7445, 7570, 8187, 8352, 9702, 11168, 12923, 12950,
        13047, 13230, 13288, 13522, 13908, 13930, 13981, 14070,
    };

    /// <summary>Returns "Forbidden"/"Limited"/"Semi-Limited" (matching the
    /// same display strings DeckLegalityChecker used from the old
    /// banlist_info-based lookup) for a given lotd_id, or null if that card
    /// isn't on the list at all (i.e. unrestricted).</summary>
    public static string? GetStatus(int? lotdId)
    {
        if (lotdId is not int id) return null;
        if (Forbidden.Contains(id)) return "Forbidden";
        if (Limited.Contains(id)) return "Limited";
        if (SemiLimited.Contains(id)) return "Semi-Limited";
        return null;
    }
}
