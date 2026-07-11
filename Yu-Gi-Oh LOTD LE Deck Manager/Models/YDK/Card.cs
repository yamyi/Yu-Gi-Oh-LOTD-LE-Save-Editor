using System.Text.Json.Serialization;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services
{
    // ── Root wrapper ──────────────────────────────────────────────────────────────
    public sealed class CardRoot
    {
        [JsonPropertyName("data")]
        public List<Card> Data { get; init; } = new();
    }

    // ── Card ──────────────────────────────────────────────────────────────────────
    public sealed class Card
    {
        // ── Identity ──────────────────────────────────────────────────────────────
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("lotd_id")]
        public int? LotdId { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; init; } = string.Empty;

        [JsonPropertyName("humanReadableCardType")]
        public string HumanReadableCardType { get; init; } = string.Empty;

        [JsonPropertyName("frameType")]
        public string FrameType { get; init; } = string.Empty;

        [JsonPropertyName("desc")]
        public string Desc { get; init; } = string.Empty;

        [JsonPropertyName("race")]
        public string Race { get; init; } = string.Empty;

        [JsonPropertyName("archetype")]
        public string? Archetype { get; init; }

        [JsonPropertyName("ygoprodeck_url")]
        public string YgoprodeckUrl { get; init; } = string.Empty;

        // ── Monster stats (null for Spell / Trap) ─────────────────────────────────
        [JsonPropertyName("attribute")]
        public string? Attribute { get; init; }

        [JsonPropertyName("level")]
        public int? Level { get; init; }

        [JsonPropertyName("atk")]
        public int? Atk { get; init; }

        [JsonPropertyName("def")]
        public int? Def { get; init; }

        // ── Link monsters ─────────────────────────────────────────────────────────
        [JsonPropertyName("linkval")]
        public int? LinkVal { get; init; }

        [JsonPropertyName("linkmarkers")]
        public List<string>? LinkMarkers { get; init; }

        // ── Pendulum monsters ─────────────────────────────────────────────────────
        [JsonPropertyName("scale")]
        public int? Scale { get; init; }

        [JsonPropertyName("pend_desc")]
        public string? PendDesc { get; init; }

        [JsonPropertyName("monster_desc")]
        public string? MonsterDesc { get; init; }

        // ── Related data ──────────────────────────────────────────────────────────
        [JsonPropertyName("card_sets")]
        public List<CardSet>? CardSets { get; init; }

        [JsonPropertyName("card_images")]
        public List<CardImage>? CardImages { get; init; }

        [JsonPropertyName("banlist_info")]
        public BanlistInfo? BanlistInfo { get; init; }

        // ── Convenience helpers ───────────────────────────────────────────────────
        [JsonIgnore]
        public bool IsMonster => Type.Contains("Monster", StringComparison.OrdinalIgnoreCase);

        [JsonIgnore]
        public bool IsSpell => Type.Contains("Spell", StringComparison.OrdinalIgnoreCase);

        [JsonIgnore]
        public bool IsTrap => Type.Contains("Trap", StringComparison.OrdinalIgnoreCase);

        [JsonIgnore]
        public bool IsLink => LinkVal.HasValue;

        [JsonIgnore]
        public bool IsPendulum => Scale.HasValue;

        [JsonIgnore]
        public string? SmallImageUrl => CardImages?.FirstOrDefault()?.ImageUrlSmall;

        [JsonIgnore]
        public string? ImageUrl => CardImages?.FirstOrDefault()?.ImageUrl;

        public override string ToString() => $"[{Id}] {Name}";
    }

    // ── CardSet ───────────────────────────────────────────────────────────────────
    public sealed class CardSet
    {
        [JsonPropertyName("set_name")]
        public string SetName { get; init; } = string.Empty;

        [JsonPropertyName("set_code")]
        public string SetCode { get; init; } = string.Empty;

        [JsonPropertyName("set_rarity")]
        public string SetRarity { get; init; } = string.Empty;

        [JsonPropertyName("set_rarity_code")]
        public string SetRarityCode { get; init; } = string.Empty;

        [JsonPropertyName("set_price")]
        public string SetPrice { get; init; } = "0";
    }

    // ── CardImage ─────────────────────────────────────────────────────────────────
    public sealed class CardImage
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("image_url")]
        public string ImageUrl { get; init; } = string.Empty;

        [JsonPropertyName("image_url_small")]
        public string ImageUrlSmall { get; init; } = string.Empty;

        [JsonPropertyName("image_url_cropped")]
        public string ImageUrlCropped { get; init; } = string.Empty;
    }

    // ── BanlistInfo ───────────────────────────────────────────────────────────────
    public sealed class BanlistInfo
    {
        [JsonPropertyName("ban_tcg")]
        public string? BanTcg { get; init; }

        [JsonPropertyName("ban_ocg")]
        public string? BanOcg { get; init; }

        [JsonPropertyName("ban_goat")]
        public string? BanGoat { get; init; }
    }
}