namespace YuGiOhSaveEditor.Wpf.Services;

/// <summary>
/// Formats every field on a Card into the small set of strings the two card
/// preview panels (Deck Editor + Card Search - identically-named
/// PreviewXxx TextBlocks in both XAML files) display. Pulled out into one
/// shared place instead of duplicating the same formatting twice, since
/// both views' ShowPreviewAsync need the exact same text for the exact same
/// fields. Every method returns string.Empty for a field that doesn't apply
/// to the given card (e.g. Scale on a non-Pendulum monster) - callers
/// collapse the corresponding TextBlock when the string comes back empty so
/// no blank line is left behind in the layout.
/// </summary>
public static class CardPreviewFormatter
{
    public static string Passcode(Card card) => $"Passcode: {card.Id}";

    public static string TypeRace(Card card) => card.IsMonster
        ? $"{card.Race}{(string.IsNullOrEmpty(card.Attribute) ? "" : " / " + card.Attribute)}"
        : $"[{card.HumanReadableCardType}]";

    /// <summary>The monster subtype qualifier (e.g. "Effect Monster",
    /// "Fusion Pendulum Effect Monster") - shown as its own line since
    /// TypeRace only carries Race/Attribute for monsters.</summary>
    public static string MonsterType(Card card) =>
        card.IsMonster ? card.HumanReadableCardType : string.Empty;

    public static string Archetype(Card card) =>
        string.IsNullOrEmpty(card.Archetype) ? string.Empty : $"Archetype: {card.Archetype}";

    public static string Level(Card card) =>
        card.Level.HasValue ? $"LV {card.Level}" : (card.IsLink ? $"LINK {card.LinkVal}" : "");

    public static string Atk(Card card) => $"ATK {card.Atk?.ToString() ?? "?"}";

    public static string Def(Card card) => card.IsLink ? "" : $"DEF {card.Def?.ToString() ?? "?"}";

    public static string Scale(Card card) =>
        card.Scale.HasValue ? $"Pendulum Scale: {card.Scale}" : string.Empty;

    public static string LinkMarkers(Card card) =>
        card.IsLink && card.LinkMarkers is { Count: > 0 }
            ? $"Link Markers: {string.Join(", ", card.LinkMarkers)}"
            : string.Empty;

    /// <summary>TCG/OCG banlist status (e.g. "TCG: Forbidden   OCG: Limited") -
    /// only the lists that actually have a value are included, and the whole
    /// thing comes back empty when BanlistInfo is null (i.e. the card isn't
    /// restricted anywhere).</summary>
    public static string BanStatus(Card card)
    {
        if (card.BanlistInfo is not { } b) return string.Empty;

        var parts = new List<string>();
        if (!string.IsNullOrEmpty(b.BanTcg)) parts.Add($"TCG: {b.BanTcg}");
        if (!string.IsNullOrEmpty(b.BanOcg)) parts.Add($"OCG: {b.BanOcg}");
        return parts.Count == 0 ? string.Empty : string.Join("   ", parts);
    }
}
