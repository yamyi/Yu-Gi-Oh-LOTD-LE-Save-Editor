namespace YuGiOhSaveEditor.Services;

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

    /// <summary>This card's in-game ban status - "Forbidden"/"Limited"/
    /// "Semi-Limited"/"Unlimited" - read straight from LotdBanlist, the
    /// curated list that's actually correct for this game/format. No more
    /// separate TCG:/OCG: split (Cards.json's own generic banlist_info
    /// field is real-world TCG/OCG data, not this format's list, and this
    /// app has no OCG-specific mode to justify showing it alongside).</summary>
    public static string BanStatus(Card card) => LotdBanlist.GetStatus(card.LotdId) ?? "Unlimited";

    /// <summary>Which Card Shop booster pack(s) sell this card and at what
    /// pack-local rarity, e.g. "Shop: Yugi (Rare)" or, for a card sold in
    /// more than one pack, "Shop: Grandpa Muto (Common), Yugi (Rare)".
    /// Empty for the small number of cards with no shop_packs data (see
    /// Card.ShopPacks' doc comment) - callers collapse the TextBlock in that
    /// case, same as every other optional preview line.</summary>
    public static string ShopInfo(Card card) =>
        card.ShopPacks is { Count: > 0 }
            ? $"Shop: {string.Join(", ", card.ShopPacks.Select(p => $"{p.Pack} ({p.Rarity})"))}"
            : string.Empty;
}
