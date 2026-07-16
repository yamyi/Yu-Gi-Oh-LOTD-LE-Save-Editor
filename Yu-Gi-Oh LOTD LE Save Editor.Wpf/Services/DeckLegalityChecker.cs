namespace YuGiOhSaveEditor.Wpf.Services;

public enum SlotLegality
{
    Empty,
    Illegal,
    Warning,
    Ok,
}

/// <summary>
/// Deck-legality check for a save slot, shown as the small dot on each Deck
/// Slots tile (see SlotLegalityBrushConverter/SlotLegalityTooltipConverter).
/// Originally just the WinForms build's deck-size thresholds (Main &lt; 40 =
/// warning, Main &gt; 60 or Extra/Side &gt; 15 = illegal); now also flags
/// banlist violations - a Forbidden card anywhere in the deck, more than 1
/// Limited copy, or more than 2 Semi-Limited copies of the same card (TCG
/// list; OCG isn't checked separately since this app has no per-region
/// setting). Counts are tallied by Card.Id (passcode) across
/// Main+Extra+Side combined, matching the same 3-copies-per-card rule Deck
/// Editor enforces when adding cards.
/// </summary>
public static class DeckLegalityChecker
{
    public static SlotLegality GetStatus(SlotIO.SlotInfo slot)
    {
        if (slot.IsEmpty) return SlotLegality.Empty;

        if (slot.Main > 60 || slot.Extra > 15 || slot.Side > 15)
            return SlotLegality.Illegal;

        if (FindBanlistViolation(slot) != null)
            return SlotLegality.Illegal;

        if (slot.Main < 40)
            return SlotLegality.Warning;

        return SlotLegality.Ok;
    }

    /// <summary>Short human-readable explanation for the tile's tooltip -
    /// every size/banlist problem found, or a plain "Deck is legal."</summary>
    public static string GetReason(SlotIO.SlotInfo slot)
    {
        if (slot.IsEmpty) return "Empty slot.";

        var reasons = new List<string>();
        if (slot.Main > 60) reasons.Add($"Main Deck has {slot.Main}/60 cards (over the limit).");
        if (slot.Extra > 15) reasons.Add($"Extra Deck has {slot.Extra}/15 cards (over the limit).");
        if (slot.Side > 15) reasons.Add($"Side Deck has {slot.Side}/15 cards (over the limit).");

        string? banReason = FindBanlistViolation(slot);
        if (banReason != null) reasons.Add(banReason);

        if (reasons.Count > 0) return string.Join(" ", reasons);

        if (slot.Main < 40) return $"Main Deck has only {slot.Main}/40 cards (below the minimum).";

        return "Deck is legal.";
    }

    /// <summary>Returns a description of the first banlist violation found
    /// (there may be more than one - only the first is reported, to keep the
    /// tooltip short), or null if the deck doesn't currently have
    /// SaveBytes loaded, or has none.</summary>
    private static string? FindBanlistViolation(SlotIO.SlotInfo slot)
    {
        if (AppContext.State?.SaveBytes == null) return null;

        var deck = AppContext.SlotIo.ReadDeck(AppContext.State.SaveBytes, slot, AppContext.CardDb);

        var counts = new Dictionary<int, int>();
        foreach (var card in deck.main.Concat(deck.extra).Concat(deck.side))
            counts[card.Id] = counts.GetValueOrDefault(card.Id) + 1;

        foreach (var (cardId, count) in counts)
        {
            var card = AppContext.CardDb.GetById(cardId);
            string? status = card?.BanlistInfo?.BanTcg;
            if (status == null) continue;

            int limit = status switch
            {
                "Forbidden" => 0,
                "Limited" => 1,
                "Semi-Limited" => 2,
                _ => 3,
            };

            if (count > limit)
                return $"Contains {count}x {card!.Name} ({status}, max {limit}).";
        }

        return null;
    }
}
