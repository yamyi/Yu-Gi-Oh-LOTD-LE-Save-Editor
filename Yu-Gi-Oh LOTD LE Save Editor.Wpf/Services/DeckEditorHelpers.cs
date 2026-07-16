namespace YuGiOhSaveEditor.Wpf.Services;

/// <summary>
/// Small static helpers ported from the WinForms build's DeckEditorPage -
/// Card itself has no "is this an Extra Deck monster" flag or single-rarity
/// field, so these live here rather than being bolted onto the ported Card
/// model.
/// </summary>
public static class DeckEditorHelpers
{
    /// <summary>Ported verbatim from the WinForms build's
    /// DeckEditorPage.IsExtraDeckCard - Fusion/Synchro/XYZ/Link monsters go
    /// in the Extra Deck, everything else (including Pendulum Normal/Effect
    /// monsters) goes in Main.</summary>
    public static bool IsExtraDeckCard(Card card) =>
        card.Type.Contains("Fusion", StringComparison.OrdinalIgnoreCase) ||
        card.Type.Contains("Synchro", StringComparison.OrdinalIgnoreCase) ||
        card.Type.Contains("XYZ", StringComparison.OrdinalIgnoreCase) ||
        card.Type.Contains("Link", StringComparison.OrdinalIgnoreCase);

    /// <summary>Card has no single-rarity field of its own - only real-world
    /// CardSets, each with its own print rarity. This buckets the first
    /// known set's rarity into the N/R/SR/UR tiers the theme's rarity
    /// brushes use, purely as color language (this app has no Master
    /// Duel-style craft-point economy). Cards with no set data default to
    /// "N".</summary>
    public static string GetRarityTier(Card card)
    {
        string? rarity = card.CardSets?.FirstOrDefault()?.SetRarity;
        if (string.IsNullOrWhiteSpace(rarity)) return "N";

        string r = rarity.ToLowerInvariant();
        if (r.Contains("secret") || r.Contains("ultimate") || r.Contains("ghost") ||
            r.Contains("starlight") || r.Contains("collector") || r.Contains("quarter") ||
            r.Contains("gold") || r.Contains("platinum") || r.Contains("ultra"))
            return "UR";
        if (r.Contains("super")) return "SR";
        if (r.Contains("rare")) return "R";
        return "N";
    }
}
