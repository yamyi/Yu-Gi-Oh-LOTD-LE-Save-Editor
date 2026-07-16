namespace YuGiOhSaveEditor.Wpf.Services
{
    /// <summary>
    /// Duelists whose shop packs can be unlocked. Values copied verbatim from
    /// pixeltris/Lotd's UnlockedShopPacks flags enum (Lotd/SaveData/MiscSaveData.cs) —
    /// this is a bitfield, one bit per duelist.
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
        // Any of the following three unlocks all ARC-V packs.
        Pendulum = 1 << 24,
        GongStrong = 1 << 25,
        ZuzuBoyle = 1 << 26,
        All = 0xFFFFFFFF
    }

    [Flags]
    public enum UnlockedBattlePacks
    {
        None = 0,
        WarOfTheGiants = 1 << 0,
        WarOfTheGiantsRound2 = 1 << 1,
        All = WarOfTheGiants | WarOfTheGiantsRound2
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
    ///   ...   8 bytes    reserved/unknown
    ///   ...   4 bytes    CompleteTutorials (int32 flags)
    ///   ...   4 bytes    UnlockedContent (int32 flags)
    /// </summary>
    public static class MiscSaveLayout
    {
        private const int DuelPointsRelOffset = 16;
        private const int UnlockedAvatarsRelOffset = 24;
        private const int ChallengesRelOffset = 56;

        private static int GetUnlockedRecipesRelOffset(LotdSaveVersion v) =>
            ChallengesRelOffset + LotdSaveFormat.GetNumDeckDataSlots(v) * 4;

        private static int GetShopPacksRelOffset(LotdSaveVersion v) =>
            GetUnlockedRecipesRelOffset(v) + LotdSaveFormat.GetRecipeBufferBytes(v);

        private static int GetBattlePacksFlagRelOffset(LotdSaveVersion v) =>
            GetShopPacksRelOffset(v) + 4;

        private static int GetTutorialsRelOffset(LotdSaveVersion v) =>
            GetBattlePacksFlagRelOffset(v) + 4 + 8; // +8 = reserved bytes after the pack flags

        private static int GetUnlockedContentRelOffset(LotdSaveVersion v) =>
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
