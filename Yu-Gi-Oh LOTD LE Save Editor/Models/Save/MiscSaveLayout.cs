namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Duelists whose shop packs can be unlocked. Bits 1-26 copied verbatim from
    /// pixeltris/Lotd's UnlockedShopPacks flags enum (Lotd/SaveData/MiscSaveData.cs)
    /// — this is a bitfield, one bit per duelist. Bits 1-23 are confirmed correct:
    /// they line up exactly in order with the real Yu-Gi-Oh/GX/5D's/ZEXAL shop
    /// pack list. "Pendulum" (bit 24) doesn't match any real duelist name -
    /// pixeltris's own comment says any of bits 24-26 unlocks the whole ARC-V
    /// group, which was accurate for the original 2016 game's 3-duelist ARC-V
    /// roster, but is unverified for Link Evolution's larger one.
    ///
    /// Shay/DeclanAkaba/YuyaSakaki/Playmaker (bits 27-30) were originally added
    /// here as an experimental guess (not present in pixeltris/Lotd - Link
    /// Evolution added them to the shop after that enum was last
    /// reverse-engineered, and these 4 bits were sitting completely
    /// unused/unnamed) but have since been CONFIRMED CORRECT against a real
    /// save/in-game purchase check - all four unlock properly.
    ///
    /// Blue Angel/Soulburner/Varis/Ai and Epic Dawn (also sold as a "shop
    /// pack" of sorts, per the game's framing of Battle Packs) are now all
    /// fully resolved - just not in this field. Two rounds of real-save
    /// diffing plus one live in-game confirmation (all 2026-07-23) settled it:
    ///   - Its ShopPacks value is 0xFEFFFFFE: every bit from 1-23 and 25-31 is
    ///     set, while bits 0 and 24 (Pendulum) are clear. That confirms bit 0
    ///     is genuinely never used, and confirms Pendulum (bit 24) really is
    ///     unused filler in Link Evolution - not a "ARC-V group toggle" the
    ///     way it was in the original 2016 game.
    ///   - Bit 31 turned out NOT to be a Blue Angel/VRAINS-group bit at all -
    ///     a byte-diff of two saves differing only in "does/doesn't own Epic
    ///     Dawn" showed exactly one bit change anywhere in the whole file's
    ///     unlock-related fields: ShopPacks gained bit 31 (0x40000002 ->
    ///     0xC0000002), with UnlockedBattlePacks staying 0x0 in both saves.
    ///     So bit 31 IS Epic Dawn's real unlock flag (see EpicDawn below) -
    ///     it just isn't stored in UnlockedBattlePacks, despite being a
    ///     "Battle Pack" in the game's own UI.
    ///   - A second before/after diff isolating Blue Angel's pack found the
    ///     mirror image: ShopPacks didn't move a single bit (still
    ///     0xC0000002 in both saves) - Blue Angel's flag isn't here either.
    ///     It's UnlockedBattlePacks.BlueAngel (bit 0 of that field) instead -
    ///     see that enum's doc comment.
    ///   - With bit 31 confirmed as Epic Dawn, ShopPacks is a fully consumed
    ///     32-bit field (0 unused, 24 unused filler, 1-23/25-31 all named) -
    ///     there was never room here for Blue Angel or the other 2 remaining
    ///     VRAINS duelists (Soulburner/Varis/Ai) to begin with. The reserved
    ///     8-byte gap after BattlePacksFlag remains confirmed all-zero in
    ///     every real save seen so far. Soulburner/Varis/Ai turned out to be
    ///     UnlockedBattlePacks' remaining bits 3-5, CONFIRMED via live in-game
    ///     testing after being wired up as a numbering hypothesis - see that
    ///     enum's doc comment. That closes out every one of Link Evolution's
    ///     33 shop duelists.
    /// </summary>
    [Flags]
    public enum UnlockedShopPacks : uint
    {
        None = 0,
        GrandpaMuto = 1 << 1,
        MaiValentine = 1 << 2,
        Bakura = 1 << 3,
        JoeyWheeler = 1 << 4,
        SetoKaiba = 1 << 5,
        Yugi = 1 << 6,
        AlexisRhodes = 1 << 7,
        BastionMisawa = 1 << 8,
        ChazzPrinceton = 1 << 9,
        SyrusTruesdale = 1 << 10,
        JesseAnderson = 1 << 11,
        JadenYuki = 1 << 12,
        TetsuTrudge = 1 << 13,
        LeoLuna = 1 << 14,
        AkizaIzinski = 1 << 15,
        JackAtlas = 1 << 16,
        Crow = 1 << 17,
        YuseiFudo = 1 << 18,
        CathyKatherine = 1 << 19,
        Quinton = 1 << 20,
        KiteTenjo = 1 << 21,
        Shark = 1 << 22,
        YumaTsukumo = 1 << 23,
        // Confirmed unused filler in Link Evolution - clear in a real
        // fully-unlocked save (see class doc comment). Left defined for
        // backward compatibility/documentation only, never set by this app.
        Pendulum = 1 << 24,
        GongStrong = 1 << 25,
        ZuzuBoyle = 1 << 26,
        // Confirmed against a real save - see class doc comment above.
        Shay = 1 << 27,
        DeclanAkaba = 1 << 28,
        YuyaSakaki = 1 << 29,
        Playmaker = 1 << 30,
        // CONFIRMED CORRECT via a real before/after save diff (see class doc
        // comment) - despite being a "Battle Pack" in-game, its flag lives
        // here in ShopPacks, not in UnlockedBattlePacks.
        EpicDawn = 1u << 31,
        All = GrandpaMuto | MaiValentine | Bakura | JoeyWheeler | SetoKaiba | Yugi
            | AlexisRhodes | BastionMisawa | ChazzPrinceton | SyrusTruesdale | JesseAnderson | JadenYuki
            | TetsuTrudge | LeoLuna | AkizaIzinski | JackAtlas | Crow | YuseiFudo
            | CathyKatherine | Quinton | KiteTenjo | Shark | YumaTsukumo
            | GongStrong | ZuzuBoyle | Shay | DeclanAkaba | YuyaSakaki | Playmaker
            | EpicDawn // = 0xFEFFFFFE, byte-for-byte the real save's fully-unlocked value
    }

    /// <summary>
    /// Copied verbatim from pixeltris/Lotd's UnlockedBattlePacks (Lotd/SaveData/
    /// MiscSaveData.cs) - only 2 bits, WarOfTheGiants and WarOfTheGiantsRound2.
    /// This field turns out to have NOTHING to do with "Battle Pack: Epic
    /// Dawn" despite the name (see UnlockedShopPacks.EpicDawn) - but it DOES
    /// hold one of the previously-unresolved VRAINS shop duelists: Blue Angel.
    ///
    /// WarOfTheGiants/WarOfTheGiantsRound2 (bits 2-1, in that order - see
    /// below) are CONFIRMED CORRECT against a real save - toggling either one
    /// alone, in isolation, unlocks that pack in-game with no other bits
    /// required. Initially the two were assigned bit 1 = WarOfTheGiants /
    /// bit 2 = WarOfTheGiantsRound2, but live in-game testing found that
    /// backwards - corrected 2026-07-23 to bit 1 = WarOfTheGiantsRound2 /
    /// bit 2 = WarOfTheGiants. Bit 0 was originally guessed to be Epic Dawn
    /// and confirmed wrong that way (writing it alone did nothing in-game); a
    /// real before/after save diff isolating Epic Dawn specifically
    /// (2026-07-23) then proved this whole field is unrelated to Epic Dawn -
    /// BattlePacksFlag was 0x0 in BOTH the "doesn't own Epic Dawn" and "owns
    /// Epic Dawn" saves, with the only actual change being bit 31 of ShopPacks.
    ///
    /// Bit 0 = Blue Angel, CONFIRMED via a second before/after save diff
    /// (2026-07-23) isolating "doesn't have Blue Angel's pack" vs. "has it":
    /// BattlePacksFlag went 0x0 -> 0x5 (bits 0 AND 2), not just bit 0 alone -
    /// but the user confirmed a War of the Giants pack (bit 2, already
    /// independently confirmed and unrelated) was ALSO unlocked in that same
    /// window, which fully accounts for bit 2's change. Bit 0 is the one
    /// attributable to Blue Angel specifically. So despite being sold as a
    /// normal duelist shop pack in-game, Blue Angel's real flag lives here,
    /// not in ShopPacks - the mirror image of Epic Dawn (sold as a Battle
    /// Pack, but actually a ShopPacks bit).
    ///
    /// That left bits 3-5, matched to Soulburner/Varis/Ai purely on a "3
    /// remaining bits, 3 remaining VRAINS shop duelists, in their in-game
    /// shop order" hypothesis - and CONFIRMED CORRECT via live in-game testing
    /// (2026-07-23): all three unlock properly. So as of that date, this
    /// field's all 6 bits (plus ShopPacks' bit 31 for Epic Dawn) fully account
    /// for every one of Link Evolution's 33 shop duelists - nothing left
    /// unresolved in the shop-pack unlock system.
    /// </summary>
    [Flags]
    public enum UnlockedBattlePacks
    {
        // CONFIRMED via a real before/after save diff isolating Blue Angel's
        // pack specifically (see class doc comment) - despite being sold as a
        // normal duelist shop pack in-game, its real flag lives here.
        BlueAngel = 1 << 0,
        // Swapped 2026-07-23 - live in-game testing found these two backwards
        // from the original guess (bit 1 was labeled WarOfTheGiants, bit 2
        // WarOfTheGiantsRound2; it's the other way around).
        WarOfTheGiantsRound2 = 1 << 1,
        WarOfTheGiants = 1 << 2,
        // CONFIRMED via live in-game testing (2026-07-23) - originally a
        // numbering hypothesis ("3 remaining bits, 3 remaining VRAINS shop
        // duelists, in shop order"), now verified correct: all three unlock
        // properly in-game.
        Soulburner = 1 << 3,
        Varis = 1 << 4,
        Ai = 1 << 5,
        All = BlueAngel | WarOfTheGiants | WarOfTheGiantsRound2 | Soulburner | Varis | Ai // = 0x3F
    }

    [Flags]
    public enum UnlockedContent
    {
        None = 0,
        DuelistChallenges = 1 << 0,
        BattlePack = 1 << 1,
        CardShop = 1 << 2,
        All = DuelistChallenges | BattlePack | CardShop
    }

    [Flags]
    public enum CompleteTutorials
    {
        None = 0,
        Tut01 = 1 << 1,
        Tut02 = 1 << 2,
        Tut03 = 1 << 3,
        Tut04 = 1 << 4,
        Tut05 = 1 << 5,
        Tut06 = 1 << 6,
        Tut07 = 1 << 7,
        Tut08 = 1 << 8,
        Tut09 = 1 << 9,
        Tut10 = 1 << 10,
        Tut11 = 1 << 11,
        Tut12 = 1 << 12,
        Tut13 = 1 << 13,
        Tut14 = 1 << 14,
        Tut15 = 1 << 15,
        Tut16 = 1 << 17,
        Tut17 = 1 << 18,
        Tut18 = 1 << 19,
        Tut19 = 1 << 20,
        Tut20 = 1 << 21,
    }

    public enum DeulistChallengeState
    {
        Locked = 0,
        Available = 1,
        Failed = 2,
        Complete = 3
    }

    /// <summary>
    /// Reads/writes the "Misc" save chunk directly against the raw save byte[] —
    /// duel points, unlocked avatars, duelist-challenge states, unlocked deck
    /// recipes, unlocked shop/battle packs, completed tutorials, and the
    /// padlocked-content flags. Field layout ported verbatim from
    /// pixeltris/Lotd's MiscSaveData.Load/Save (Lotd/SaveData/MiscSaveData.cs).
    ///
    /// Byte layout, relative to LotdSaveFormat.GetMiscDataOffset(version):
    ///   +0    16 bytes   reserved/unknown
    ///   +16   8 bytes    DuelPoints (int64)
    ///   +24   32 bytes   UnlockedAvatars bitfield (bit i = avatar id i, 0-152 used)
    ///   +56   N*4 bytes  Challenges (int32 DeulistChallengeState per deck-data slot)
    ///   ...   R bytes    UnlockedRecipes bitfield (bit i = deck-data slot id i)
    ///   ...   4 bytes    UnlockedShopPacks (uint32 flags)
    ///   ...   4 bytes    UnlockedBattlePacks (uint32 flags)
    ///   ...   8 bytes    reserved/unknown (2 guessed shop-pack locations tried and ruled out here - see UnlockedShopPacks' doc comment)
    ///   ...   4 bytes    CompleteTutorials (int32 flags)
    ///   ...   4 bytes    UnlockedContent (int32 flags)
    /// </summary>
    public static class MiscSaveLayout
    {
        // Offsets below are public (not just an implementation detail of this
        // class) so SaveLayout.cs can reference them directly for its whole-file
        // map instead of duplicating the byte math - see SaveLayout's "Misc
        // sub-fields" doc section for where each of these actually lands.
        public const int DuelPointsRelOffset = 16;
        public const int UnlockedAvatarsRelOffset = 24;
        public const int ChallengesRelOffset = 56;

        public static int GetUnlockedRecipesRelOffset(LotdSaveVersion v) =>
            ChallengesRelOffset + LotdSaveFormat.GetNumDeckDataSlots(v) * 4;

        public static int GetShopPacksRelOffset(LotdSaveVersion v) =>
            GetUnlockedRecipesRelOffset(v) + LotdSaveFormat.GetRecipeBufferBytes(v);

        public static int GetBattlePacksFlagRelOffset(LotdSaveVersion v) =>
            GetShopPacksRelOffset(v) + 4;

        public static int GetTutorialsRelOffset(LotdSaveVersion v) =>
            GetBattlePacksFlagRelOffset(v) + 4 + 8; // +8 = reserved bytes after the pack flags - both halves tried as a guessed shop-pack location and ruled out, left untouched now

        public static int GetUnlockedContentRelOffset(LotdSaveVersion v) =>
            GetTutorialsRelOffset(v) + 4;

        private static int MiscBase(LotdSaveVersion v) => LotdSaveFormat.GetMiscDataOffset(v);

        // ── Duel points ───────────────────────────────────────────────────────
        public static long GetDuelPoints(byte[] save, LotdSaveVersion v) =>
            ReadI64(save, MiscBase(v) + DuelPointsRelOffset);

        public static void SetDuelPoints(byte[] save, LotdSaveVersion v, long value) =>
            WriteI64(save, MiscBase(v) + DuelPointsRelOffset, value);

        // ── Avatars (bit-packed, 153 meaningful bits in a 32-byte buffer) ───────
        public static bool GetAvatarUnlocked(byte[] save, LotdSaveVersion v, int avatarIndex)
        {
            int byteIndex = avatarIndex / 8, bitIndex = avatarIndex % 8;
            int offset = MiscBase(v) + UnlockedAvatarsRelOffset + byteIndex;
            if ((uint)offset >= (uint)save.Length) return false;
            return (save[offset] & (1 << bitIndex)) != 0;
        }

        public static void SetAvatarUnlocked(byte[] save, LotdSaveVersion v, int avatarIndex, bool unlocked)
        {
            int byteIndex = avatarIndex / 8, bitIndex = avatarIndex % 8;
            int offset = MiscBase(v) + UnlockedAvatarsRelOffset + byteIndex;
            if ((uint)offset >= (uint)save.Length) return;
            if (unlocked) save[offset] |= (byte)(1 << bitIndex);
            else save[offset] &= (byte)~(1 << bitIndex);
        }

        public static void SetAllAvatarsUnlocked(byte[] save, LotdSaveVersion v, bool unlocked)
        {
            for (int i = 0; i < LotdSaveFormat.NumAvatarSlots; i++)
                SetAvatarUnlocked(save, v, i, unlocked);
        }

        // ── Duelist challenges ───────────────────────────────────────────────────
        public static DeulistChallengeState GetChallengeState(byte[] save, LotdSaveVersion v, int slotIndex) =>
            (DeulistChallengeState)ReadI32(save, MiscBase(v) + ChallengesRelOffset + slotIndex * 4);

        public static void SetChallengeState(byte[] save, LotdSaveVersion v, int slotIndex, DeulistChallengeState state) =>
            WriteI32(save, MiscBase(v) + ChallengesRelOffset + slotIndex * 4, (int)state);

        public static void SetAllChallenges(byte[] save, LotdSaveVersion v, DeulistChallengeState state)
        {
            int n = LotdSaveFormat.GetNumDeckDataSlots(v);
            for (int i = 0; i < n; i++)
                SetChallengeState(save, v, i, state);
        }

        // ── Unlocked deck recipes (bit-packed) ───────────────────────────────────
        public static bool GetRecipeUnlocked(byte[] save, LotdSaveVersion v, int slotIndex)
        {
            int byteIndex = slotIndex / 8, bitIndex = slotIndex % 8;
            int offset = MiscBase(v) + GetUnlockedRecipesRelOffset(v) + byteIndex;
            if ((uint)offset >= (uint)save.Length) return false;
            return (save[offset] & (1 << bitIndex)) != 0;
        }

        public static void SetRecipeUnlocked(byte[] save, LotdSaveVersion v, int slotIndex, bool unlocked)
        {
            int byteIndex = slotIndex / 8, bitIndex = slotIndex % 8;
            int offset = MiscBase(v) + GetUnlockedRecipesRelOffset(v) + byteIndex;
            if ((uint)offset >= (uint)save.Length) return;
            if (unlocked) save[offset] |= (byte)(1 << bitIndex);
            else save[offset] &= (byte)~(1 << bitIndex);
        }

        public static void SetAllRecipesUnlocked(byte[] save, LotdSaveVersion v, bool unlocked)
        {
            int n = LotdSaveFormat.GetNumDeckDataSlots(v);
            for (int i = 0; i < n; i++)
                SetRecipeUnlocked(save, v, i, unlocked);
        }

        // ── Shop packs / battle packs / content / tutorials (plain flag words) ──
        public static UnlockedShopPacks GetShopPacks(byte[] save, LotdSaveVersion v) =>
            (UnlockedShopPacks)ReadU32(save, MiscBase(v) + GetShopPacksRelOffset(v));

        public static void SetShopPacks(byte[] save, LotdSaveVersion v, UnlockedShopPacks value) =>
            WriteU32(save, MiscBase(v) + GetShopPacksRelOffset(v), (uint)value);

        public static UnlockedBattlePacks GetBattlePacksFlag(byte[] save, LotdSaveVersion v) =>
            (UnlockedBattlePacks)ReadU32(save, MiscBase(v) + GetBattlePacksFlagRelOffset(v));

        public static void SetBattlePacksFlag(byte[] save, LotdSaveVersion v, UnlockedBattlePacks value) =>
            WriteU32(save, MiscBase(v) + GetBattlePacksFlagRelOffset(v), (uint)value);

        public static CompleteTutorials GetTutorials(byte[] save, LotdSaveVersion v) =>
            (CompleteTutorials)ReadI32(save, MiscBase(v) + GetTutorialsRelOffset(v));

        public static void SetTutorials(byte[] save, LotdSaveVersion v, CompleteTutorials value) =>
            WriteI32(save, MiscBase(v) + GetTutorialsRelOffset(v), (int)value);

        public static UnlockedContent GetUnlockedContent(byte[] save, LotdSaveVersion v) =>
            (UnlockedContent)ReadI32(save, MiscBase(v) + GetUnlockedContentRelOffset(v));

        public static void SetUnlockedContent(byte[] save, LotdSaveVersion v, UnlockedContent value) =>
            WriteI32(save, MiscBase(v) + GetUnlockedContentRelOffset(v), (int)value);

        /// <summary>
        /// Unlocks everything gated behind the early-game "padlock" — matches
        /// pixeltris/Lotd's GameSaveData.UnlockPadlockedContent() helper.
        /// </summary>
        public static void UnlockPadlockedContent(byte[] save, LotdSaveVersion v)
        {
            SetUnlockedContent(save, v, UnlockedContent.All);
            SetShopPacks(save, v, UnlockedShopPacks.All);
            SetBattlePacksFlag(save, v, UnlockedBattlePacks.All);
        }

        // ── Raw helpers ───────────────────────────────────────────────────────
        private static long ReadI64(byte[] b, int offset)
        {
            if (offset < 0 || offset + 8 > b.Length) return 0;
            return BitConverter.ToInt64(b, offset);
        }

        private static void WriteI64(byte[] b, int offset, long value)
        {
            if (offset < 0 || offset + 8 > b.Length) return;
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, b, offset, 8);
        }

        private static int ReadI32(byte[] b, int offset)
        {
            if (offset < 0 || offset + 4 > b.Length) return 0;
            return b[offset] | (b[offset + 1] << 8) | (b[offset + 2] << 16) | (b[offset + 3] << 24);
        }

        private static void WriteI32(byte[] b, int offset, int value)
        {
            if (offset < 0 || offset + 4 > b.Length) return;
            b[offset] = (byte)(value & 0xFF);
            b[offset + 1] = (byte)((value >> 8) & 0xFF);
            b[offset + 2] = (byte)((value >> 16) & 0xFF);
            b[offset + 3] = (byte)((value >> 24) & 0xFF);
        }

        private static uint ReadU32(byte[] b, int offset) => (uint)ReadI32(b, offset);

        private static void WriteU32(byte[] b, int offset, uint value) => WriteI32(b, offset, (int)value);
    }
}
