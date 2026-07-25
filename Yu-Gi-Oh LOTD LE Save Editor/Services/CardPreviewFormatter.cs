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

    /// <summary>Text for the first icon+text pair on the type/attribute line -
    /// the monster's Race (e.g. "Dragon") next to its Type icon. Empty for
    /// Spell/Trap cards, which only use the second icon+text slot (see
    /// AttributeOrSubTypeText) - callers collapse both the icon and this
    /// text, plus the " / " separator between them, in that case.</summary>
    public static string RaceText(Card card) => card.IsMonster ? card.Race : string.Empty;

    /// <summary>Text for the second icon+text pair - the monster's
    /// Attribute (e.g. "LIGHT") next to its Attribute icon, or for a
    /// Spell/Trap card, its full subtype name (e.g. "Field Spell") next to
    /// the subtype icon instead (there's no first pair in that case - see
    /// RaceText).</summary>
    public static string AttributeOrSubTypeText(Card card) => card.IsMonster
        ? card.Attribute ?? string.Empty
        : card.HumanReadableCardType;

    /// <summary>The monster subtype qualifier (e.g. "Effect Monster",
    /// "Fusion Pendulum Effect Monster") - shown as its own line since
    /// TypeRace only carries Race/Attribute for monsters.</summary>
    public static string MonsterType(Card card) =>
        card.IsMonster ? card.HumanReadableCardType : string.Empty;

    public static string Archetype(Card card) =>
        string.IsNullOrEmpty(card.Archetype) ? string.Empty : $"Archetype: {card.Archetype}";

    /// <summary>"LV 4" for a normal/effect/etc. monster, "Rank 4" for an Xyz
    /// monster (ygoprodeck stores Rank in the same Level field - FrameType is
    /// what actually says "xyz"), or "Link-2" for a Link monster (which has
    /// no Level at all - LinkVal is its own field). Only one of these ever
    /// applies to a given card, so this is the single line PreviewLevel
    /// shows regardless of which kind of monster it is.</summary>
    public static string Level(Card card)
    {
        if (card.IsLink) return $"Link-{card.LinkVal}";
        if (!card.Level.HasValue) return string.Empty;
        return card.IsXyz ? $"Rank {card.Level}" : $"LV {card.Level}";
    }

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

    /// <summary>Which duelist(s)' dedicated Duelist Challenge decklist
    /// contains this card, e.g. "Duelist Challenge: Yugi Muto, Seto Kaiba" -
    /// staple cards (Mystical Space Typhoon, Dark Hole, etc.) show up in
    /// dozens of decks, so the line caps at DuelistChallengeDisplayCap names
    /// and appends "+N more" rather than listing every one of them. Empty
    /// for cards with no duelist_challenges data (see Card.DuelistChallenges'
    /// doc comment) - callers collapse the TextBlock in that case, same as
    /// every other optional preview line.</summary>
    private const int DuelistChallengeDisplayCap = 5;

    public static string DuelistChallengeInfo(Card card)
    {
        if (card.DuelistChallenges is not { Count: > 0 } duelists) return string.Empty;

        var shown = duelists.Take(DuelistChallengeDisplayCap);
        string list = string.Join(", ", shown);
        int remaining = duelists.Count - DuelistChallengeDisplayCap;

        return remaining > 0
            ? $"Duelist Challenge: {list} (+{remaining} more)"
            : $"Duelist Challenge: {list}";
    }

    /// <summary>How many copies of this specific card the currently loaded
    /// save owns, e.g. "Owned: 2 / 3" - reads AppContext.State.SaveBytes
    /// directly (same live save CardCollectionLayout/Save Editor's Cards tab
    /// reads/writes) rather than taking it as a parameter, since every other
    /// formatter method here only needs the Card itself and callers already
    /// call this once per preview anyway. Empty when there's no save loaded
    /// yet or this card has no LotdId (i.e. it isn't part of the save's card
    /// table at all) - callers collapse the TextBlock in that case, same as
    /// every other optional preview line.</summary>
    public static string OwnedCount(Card card)
    {
        if (AppContext.State.SaveBytes is not { Length: > 0 } || card.LotdId is not int lotdId)
            return string.Empty;

        int count = CardCollectionLayout.GetCount(AppContext.State.SaveBytes, AppContext.State.Version, lotdId);
        return $"Owned: {count} / {CardCollectionLayout.MaxCountPerCard}";
    }
}
