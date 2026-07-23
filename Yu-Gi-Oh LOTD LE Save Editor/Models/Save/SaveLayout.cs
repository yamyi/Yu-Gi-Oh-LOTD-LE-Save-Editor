using System.Text;

namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Full memory map of the entire LOTD save file, top to bottom. This class
    /// doesn't re-implement any reading/writing logic — every region already has
    /// its own class (LotdSaveFormat for chunk boundaries, MiscSaveLayout,
    /// CampaignSaveLayout, SlotLayout, CardCollectionLayout, SaveSignatureFixer
    /// for the header checksum). SaveLayout just points at all of them from one
    /// place so the whole file can be understood without hunting across six
    /// files, the same way SlotLayout.cs is the one-stop map for a single deck
    /// slot's internal fields.
    ///
    /// All offsets are absolute (from byte 0) and version-dependent unless noted
    /// "fixed" — LinkEvolution (2019+) shifted everything after the header by
    /// inserting a 6th duel series (VRAINS) and enlarging the Stats/Misc/Campaign/
    /// CardList chunks relative to the original 2016 "Lotd" format. Use
    /// LotdSaveFormat.DetectVersion(bytes) to get the LotdSaveVersion for a loaded
    /// buffer. This app targets LinkEvolution saves specifically (see
    /// SlotLayout.CurrentVersion's default) — the Lotd-format numbers below are
    /// included for reference but aren't exercised anywhere in the UI.
    ///
    /// Whole-file map (offsets/lengths shown for LinkEvolution; file length 44008):
    ///   0x00000  Header             20 bytes    -&gt; SaveSignatureFixer (sub-fields below)
    ///   0x00014  Unknown lead-in    16 bytes    unidentified
    ///   0x00024  Stats              0x320 (800) unidentified internals; 100 stats x 8 bytes each
    ///   0x00344  BattlePacks        0xC94 (3220) unidentified internals; 5 packs x 644 bytes each
    ///   0x00FD8  Misc               0xB98 (2968) -&gt; MiscSaveLayout
    ///   0x01B70  Campaign           0x1C58 (7256) -&gt; CampaignSaveLayout
    ///   0x037C8  Decks              0x2600 (9728) -&gt; SlotLayout (32 slots x 0x130 bytes)
    ///   0x05DC8  CardList           0x4E20 (20000) -&gt; CardCollectionLayout (see note below)
    ///   0x0ABE8  EOF                (= GetFileLength(LinkEvolution))
    ///
    /// Lotd-format equivalents (file length 29005):
    ///   0x00000  Header             20 bytes
    ///   0x00014  Unknown lead-in    16 bytes
    ///   0x00024  Stats              0x158  (344)   43 stats x 8 bytes
    ///   0x0017C  BattlePacks        0xC94  (3220)  5 packs x 644 bytes (same size as LinkEvolution - fixed)
    ///   0x00E10  Misc               0x800  (2048)
    ///   0x01610  Campaign           0x17A0 (6048)
    ///   0x02DB0  Decks              0x2600 (9728, same as LinkEvolution - fixed)
    ///   0x053B0  CardList           0x1D9D (7581)
    ///   0x0714D  EOF                (= GetFileLength(Lotd))
    ///
    /// Header sub-fields (both versions - confirmed against pixeltris/Lotd's own
    /// GameSaveData.Load/ToArray, which writes exactly this 20-byte sequence):
    ///   0x00  4 bytes   Magic1        constant, 0x54CE29F9
    ///   0x04  4 bytes   Magic2        0x04714D02 = Lotd, 0x04ABE802 = LinkEvolution
    ///   0x08  4 bytes   FileLength    total save file size in bytes (must equal GetFileLength(version))
    ///   0x0C  4 bytes   Signature     CRC-32-like checksum over the whole file, computed with this field zeroed (see SaveSignatureFixer)
    ///   0x10  4 bytes   PlayCount     number of play sessions; also folded into the signature input
    ///
    /// Misc sub-fields (see MiscSaveLayout.cs for the byte math these offsets
    /// come from; offsets below are absolute, shown for LinkEvolution - use
    /// the Get*Offset methods below for the actual version-aware values,
    /// Lotd's numbers are smaller/shifted the same way the top-level chunks are):
    ///   0x0FE8  8 bytes             DuelPoints            int64
    ///   0x0FF0  32 bytes            UnlockedAvatars       bitfield, bit i = avatar id i (0-152 used)
    ///   0x1010  NumDeckDataSlots*4  Challenges            int32 DeulistChallengeState per deck-data slot
    ///   0x1B00  RecipeBufferBytes   UnlockedRecipes       bitfield, bit i = deck-data slot id i
    ///   0x1B58  4 bytes             ShopPacks             uint32 flags - THE CARD SHOP UNLOCKS, plus Epic Dawn (see UnlockedShopPacks; bits 1-31 confirmed real, bit 24 confirmed unused filler, bit 31 = Epic Dawn confirmed via before/after diff)
    ///   0x1B5C  4 bytes             BattlePacksFlag       uint32 flags (see UnlockedBattlePacks; bits 0-5 all confirmed real via a fully-unlocked save, but unrelated to Epic Dawn despite bit 0's original name)
    ///   0x1B60  8 bytes             (reserved/unknown)    confirmed genuinely unused - all-zero in a real fully-unlocked save
    ///   0x1B68  4 bytes             Tutorials             int32 flags (see CompleteTutorials)
    ///   0x1B6C  4 bytes             UnlockedContent       int32 flags (see UnlockedContent)
    ///
    /// Notes on unidentified/padded regions:
    ///   - Unknown lead-in (16B), Stats' per-entry internals, and BattlePacks'
    ///     per-pack internals have never been reverse-engineered in this project
    ///     or in pixeltris/Lotd - only their chunk-level size is known. Nothing
    ///     in this app reads or writes them; they're carried through untouched.
    ///   - CardList's chunk size (to EOF) doesn't equal GetNumCards(v) for
    ///     LinkEvolution (20000 bytes vs 10027 real cards) - this isn't
    ///     unidentified data, it's a padded array capacity (pixeltris/Lotd's
    ///     Constants.AbsoluteMaxNumCards = 20000, so a save can round-trip
    ///     through a newer game version that adds cards without a format
    ///     migration). GetNumCards(v) is 10027 (confirmed 2026-07-23 against a
    ///     real "all cards owned" save - 10166 was the old/wrong estimate
    ///     before that; 10341 was floated and retracted the same day). That
    ///     save showed 10027 slots at count 3 and everything else at 0 - a
    ///     clean bimodal split - and this project's own
    ///     Assets/cards/Cards.json independently maps lotd_id for all 10027
    ///     cards (the one gap, lotd_id 6053 = "7", was found and added
    ///     2026-07-23). That save also proved real cards are
    ///     scattered across the full chunk (its owned indices ran
    ///     ~4007-14964, 353 separate runs), NOT clustered in a
    ///     0..GetNumCards-1 prefix - and Cards.json's lotd_id field turns out
    ///     to already carry an index-to-card-identity mapping for all 10027
    ///     cards it knows about (min/max lotd_id 4007/14968, matching that
    ///     same range), enabling per-card (not just bulk) editing - see
    ///     Card.LotdId and the Cards tab's "unlock a specific card" search. So both
    ///     CardCollectionLayout's bulk operations (unlock all/reset all/mark
    ///     all seen) AND the Cards tab's "X/Y owned" summary scan the *entire*
    ///     chunk via GetNumCardSlots(v) - stopping at GetNumCards(v) as a scan
    ///     bound was the cause of two real bugs: the original "unlock all
    ///     cards" bug, and later an inaccurate owned-count summary that
    ///     undercounted a fully-owned save. GetNumCards(v) is now only ever
    ///     used as the summary's display denominator, never a scan bound. The
    ///     original Lotd format didn't reserve extra room, so its chunk size
    ///     and card count match exactly (7581 = 7581) and the distinction is
    ///     moot there.
    ///   - Blue Angel/Soulburner/Varis/Ai's individual shop-unlock bits remain
    ///     completely unresolved, and there's no room left to find them in:
    ///     ShopPacks bit 31 (its last spare bit) turned out to be Epic Dawn's
    ///     real flag, not a Blue Angel/VRAINS-group toggle - confirmed
    ///     2026-07-23 via a real before/after save diff isolating just Epic
    ///     Dawn, which showed exactly that one bit change (ShopPacks
    ///     0x40000002 -> 0xC0000002) with BattlePacksFlag staying 0x0 in both
    ///     saves. So ShopPacks is now fully consumed with no bit to spare, the
    ///     8-byte reserved gap is confirmed all-zero/unused, and
    ///     BattlePacksFlag has nothing to do with Epic Dawn - these 4
    ///     duelists' real gating almost certainly lives entirely outside
    ///     both fields. See UnlockedShopPacks' doc comment for the full
    ///     diff details.
    /// </summary>
    public static class SaveLayout
    {
        // ── Header (fixed - identical layout in both versions) ───────────────────
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
        public static int GetStatsOffset(LotdSaveVersion v) => LotdSaveFormat.StatsOffset; // fixed at 36/0x24 for both versions
        public static int GetStatsSize(LotdSaveVersion v) => LotdSaveFormat.GetStatsSize(v);
        public static int GetNumStats(LotdSaveVersion v) => LotdSaveFormat.GetNumStats(v);
        public const int StatEntryBytes = 8; // GetStatsSize(v) == GetNumStats(v) * StatEntryBytes for both versions

        // ── BattlePacks (unidentified internals; see LotdSaveFormat for boundaries) ─
        public static int GetBattlePacksOffset(LotdSaveVersion v) => LotdSaveFormat.GetBattlePacksOffset(v);
        public static int GetBattlePacksSize(LotdSaveVersion v) => LotdSaveFormat.GetBattlePacksSize(v);
        public const int NumBattlePacks = LotdSaveFormat.NumBattlePacks; // 5, fixed
        public const int BattlePackEntryBytes = 644; // GetBattlePacksSize(v) == NumBattlePacks * BattlePackEntryBytes for both versions

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
