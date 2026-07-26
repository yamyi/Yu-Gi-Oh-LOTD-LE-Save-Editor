using System.Text;

namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Full memory map of the entire LOTD:LE save file, top to bottom. This
    /// class doesn't re-implement any reading/writing logic — every region
    /// already has its own class (LotdSaveFormat for chunk boundaries,
    /// MiscSaveLayout, CampaignSaveLayout, SlotLayout, CardCollectionLayout,
    /// SaveSignatureFixer for the header checksum). SaveLayout just points
    /// at all of them from one place.
    ///
    /// Full byte-level map, every chunk and sub-field: see Documentation/SaveFormat.md.
    /// </summary>
    public static class SaveLayout
    {
        // ── Header (fixed) ────────────────────────────────────────────────────────
        public const int HeaderOffset = 0x00;
        public const int HeaderLength = 20;
        public const int Magic1Offset = 0x00;
        public const int Magic2Offset = 0x04;
        public const int FileLengthOffset = 0x08;
        public const int SignatureOffset = 0x0C; // see SaveSignatureFixer
        public const int PlayCountOffset = 0x10;

        // ── Unknown lead-in (fixed) ────────────────────────────────────────────────
        public const int UnknownLeadInOffset = HeaderOffset + HeaderLength; // 0x14 / 20
        public const int UnknownLeadInLength = 16;

        // ── Stats (unidentified internals; see LotdSaveFormat for boundaries) ────
        public static int GetStatsOffset(LotdSaveVersion v) => LotdSaveFormat.StatsOffset; // fixed at 36/0x24
        public static int GetStatsSize(LotdSaveVersion v) => LotdSaveFormat.GetStatsSize(v);
        public static int GetNumStats(LotdSaveVersion v) => LotdSaveFormat.GetNumStats(v);
        public const int StatEntryBytes = 8; // GetStatsSize(v) == GetNumStats(v) * StatEntryBytes

        // ── BattlePacks (unidentified internals; see LotdSaveFormat for boundaries) ─
        public static int GetBattlePacksOffset(LotdSaveVersion v) => LotdSaveFormat.GetBattlePacksOffset(v);
        public static int GetBattlePacksSize(LotdSaveVersion v) => LotdSaveFormat.GetBattlePacksSize(v);
        public const int NumBattlePacks = LotdSaveFormat.NumBattlePacks; // 5, fixed
        public const int BattlePackEntryBytes = 644; // GetBattlePacksSize(v) == NumBattlePacks * BattlePackEntryBytes

        // ── Misc — see MiscSaveLayout.cs for field-level layout ──────────────────
        public static int GetMiscDataOffset(LotdSaveVersion v) => LotdSaveFormat.GetMiscDataOffset(v);
        public static int GetMiscDataSize(LotdSaveVersion v) => LotdSaveFormat.GetMiscDataSize(v);

        // ── Misc sub-fields (absolute offsets; see the doc comment table above) ──
        public static int GetDuelPointsOffset(LotdSaveVersion v) => GetMiscDataOffset(v) + MiscSaveLayout.DuelPointsRelOffset;
        public static int GetUnlockedAvatarsOffset(LotdSaveVersion v) => GetMiscDataOffset(v) + MiscSaveLayout.UnlockedAvatarsRelOffset;
        public static int GetChallengesOffset(LotdSaveVersion v) => GetMiscDataOffset(v) + MiscSaveLayout.ChallengesRelOffset;
        public static int GetUnlockedRecipesOffset(LotdSaveVersion v) => GetMiscDataOffset(v) + MiscSaveLayout.GetUnlockedRecipesRelOffset(v);

        /// <summary>Where the card shop unlock flags live (UnlockedShopPacks) — this
        /// is what "unlock all cards" and the Shop Packs checklist read/write.</summary>
        public static int GetShopPacksOffset(LotdSaveVersion v) => GetMiscDataOffset(v) + MiscSaveLayout.GetShopPacksRelOffset(v);

        public static int GetBattlePacksFlagOffset(LotdSaveVersion v) => GetMiscDataOffset(v) + MiscSaveLayout.GetBattlePacksFlagRelOffset(v);

        public static int GetTutorialsOffset(LotdSaveVersion v) => GetMiscDataOffset(v) + MiscSaveLayout.GetTutorialsRelOffset(v);
        public static int GetUnlockedContentOffset(LotdSaveVersion v) => GetMiscDataOffset(v) + MiscSaveLayout.GetUnlockedContentRelOffset(v);

        // ── Campaign — see CampaignSaveLayout.cs for field-level layout ──────────
        public static int GetCampaignDataOffset(LotdSaveVersion v) => LotdSaveFormat.GetCampaignDataOffset(v);
        public static int GetCampaignDataSize(LotdSaveVersion v) => LotdSaveFormat.GetCampaignDataSize(v);

        // ── Decks — see SlotLayout.cs for field-level layout ─────────────────────
        public static int GetDecksOffset(LotdSaveVersion v) => LotdSaveFormat.GetDecksOffset(v);
        public static int GetDecksSize(LotdSaveVersion v) => LotdSaveFormat.GetDecksSize(v);
        public const int NumDeckSlots = SlotLayout.SlotCount; // 32, fixed

        // ── CardList — see CardCollectionLayout.cs for field-level layout ────────
        public static int GetCardListOffset(LotdSaveVersion v) => LotdSaveFormat.GetCardListOffset(v);
        public static int GetCardListSize(LotdSaveVersion v) => LotdSaveFormat.GetCardListSize(v);
        public static int GetNumCards(LotdSaveVersion v) => LotdSaveFormat.GetNumCards(v);

        // ── EOF ────────────────────────────────────────────────────────────────
        public static int GetFileLength(LotdSaveVersion v) => LotdSaveFormat.GetFileLength(v);

        // ── Derived helpers ───────────────────────────────────────────────────────

        /// <summary>One row per top-level region of the save file, in file order.
        /// ImplementedBy names the class that actually reads/writes that region's
        /// fields, or "-" if the region has never been reverse-engineered beyond
        /// its chunk-level size.</summary>
        public readonly record struct Region(string Name, int Offset, int Length, string ImplementedBy);

        public static Region[] GetRegions(LotdSaveVersion v) => new[]
        {
            new Region("Header",          HeaderOffset,              HeaderLength,             "SaveSignatureFixer"),
            new Region("Unknown lead-in", UnknownLeadInOffset,       UnknownLeadInLength,       "-"),
            new Region("Stats",           GetStatsOffset(v),         GetStatsSize(v),           "-"),
            new Region("BattlePacks",     GetBattlePacksOffset(v),   GetBattlePacksSize(v),     "-"),
            new Region("Misc",            GetMiscDataOffset(v),      GetMiscDataSize(v),        "MiscSaveLayout"),
            new Region("Campaign",        GetCampaignDataOffset(v),  GetCampaignDataSize(v),    "CampaignSaveLayout"),
            new Region("Decks",           GetDecksOffset(v),         GetDecksSize(v),           "SlotLayout"),
            new Region("CardList",        GetCardListOffset(v),      GetCardListSize(v),        "CardCollectionLayout"),
        };

        /// <summary>Formats GetRegions as a human-readable table (start-end offset,
        /// length, region, implementing class) — handy for a quick sanity check
        /// against a hex editor when investigating a new field. Not used by any UI;
        /// a debugging convenience only.</summary>
        public static string DumpRegions(LotdSaveVersion v)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Save layout ({v}, file length {GetFileLength(v)} bytes):");
            foreach (var r in GetRegions(v))
            {
                string impl = r.ImplementedBy == "-" ? "" : $" -> {r.ImplementedBy}";
                sb.AppendLine($"  0x{r.Offset:X5}-0x{r.Offset + r.Length - 1:X5}  ({r.Length,6} bytes)  {r.Name,-14}{impl}");
            }
            sb.AppendLine($"  0x{GetFileLength(v):X5}  EOF");
            return sb.ToString();
        }
    }
}
