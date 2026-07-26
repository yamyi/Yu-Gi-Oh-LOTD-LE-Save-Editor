namespace YuGiOhSaveEditor.Services
{
    /// <summary>Duelists whose shop packs can be unlocked, one bit per
    /// duelist. Bit 0 and bit 24 (Pendulum) are unused filler. Bit 31
    /// (EpicDawn) is Epic Dawn's flag despite being framed as a "Battle
    /// Pack" in-game. Blue Angel/Soulburner/Varis/Ai live in
    /// UnlockedBattlePacks instead.</summary>
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
        // Unused filler in Link Evolution.
        Pendulum = 1 << 24,
        GongStrong = 1 << 25,
        ZuzuBoyle = 1 << 26,
        Shay = 1 << 27,
        DeclanAkaba = 1 << 28,
        YuyaSakaki = 1 << 29,
        Playmaker = 1 << 30,
        // Epic Dawn's real unlock flag - despite being a "Battle Pack" in-game,
        // it lives here in ShopPacks, not in UnlockedBattlePacks.
        EpicDawn = 1u << 31,
        All = GrandpaMuto | MaiValentine | Bakura | JoeyWheeler | SetoKaiba | Yugi
            | AlexisRhodes | BastionMisawa | ChazzPrinceton | SyrusTruesdale | JesseAnderson | JadenYuki
            | TetsuTrudge | LeoLuna | AkizaIzinski | JackAtlas | Crow | YuseiFudo
            | CathyKatherine | Quinton | KiteTenjo | Shark | YumaTsukumo
            | GongStrong | ZuzuBoyle | Shay | DeclanAkaba | YuyaSakaki | Playmaker
            | EpicDawn // = 0xFEFFFFFE
    }

    /// <summary>WarOfTheGiants/WarOfTheGiantsRound2 plus the remaining VRAINS
    /// shop duelists this field holds: Blue Angel, Soulburner, Varis, Ai.
    /// Unrelated to "Battle Pack: Epic Dawn" despite the name - see
    /// UnlockedShopPacks.EpicDawn.</summary>
    [Flags]
    public enum UnlockedBattlePacks
    {
        // Despite being sold as a normal duelist shop pack in-game, Blue
        // Angel's real flag lives here, not in ShopPacks.
        BlueAngel = 1 << 0,
        WarOfTheGiantsRound2 = 1 << 1,
        WarOfTheGiants = 1 << 2,
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

    /// <summary>Reads/writes the "Misc" save chunk: duel points, unlocked
    /// avatars, duelist-challenge states, unlocked deck recipes, unlocked
    /// shop/battle packs, completed tutorials, padlocked-content flags.
    /// Full byte layout: see Documentation/SaveFormat.md.</summary>
    public static class MiscSaveLayout
    {
        // Public so SaveLayout.cs can reference them directly for its whole-file map.
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
            GetBattlePacksFlagRelOffset(v) + 4 + 8; // +8 = reserved bytes after the pack flags

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

        // ── Shop/Battle packs derived from Campaign story progress ──────────────

        /// <summary>Public so SaveEditorView can pass it to
        /// CanManuallyUnlockShopPack's lookup below.</summary>
        public enum PackFlagKind { Shop, Battle }

        /// <summary>Which real Campaign stage unlocks which shop/battle pack
        /// duelist. Stage numbers equal the raw CampaignSaveLayout duelIndex
        /// directly. Grandpa Muto (Yu-Gi-Oh! Stage 2) also unlocks the shop's
        /// gate itself (UnlockedContent.CardShop).</summary>
        private static readonly (LotdDuelSeries Series, int Stage, string Duelist, PackFlagKind Kind, uint Flag, bool AlsoUnlocksCardShopGate)[] CampaignShopPackUnlocks =
        {
            (LotdDuelSeries.YuGiOh, 2, "Grandpa Muto", PackFlagKind.Shop, (uint)UnlockedShopPacks.GrandpaMuto, true),
            (LotdDuelSeries.YuGiOh, 3, "Mai Valentine", PackFlagKind.Shop, (uint)UnlockedShopPacks.MaiValentine, false),
            (LotdDuelSeries.YuGiOh, 5, "Bakura", PackFlagKind.Shop, (uint)UnlockedShopPacks.Bakura, false),
            (LotdDuelSeries.YuGiOh, 8, "Joey Wheeler", PackFlagKind.Shop, (uint)UnlockedShopPacks.JoeyWheeler, false),
            (LotdDuelSeries.YuGiOh, 11, "Seto Kaiba", PackFlagKind.Shop, (uint)UnlockedShopPacks.SetoKaiba, false),
            (LotdDuelSeries.YuGiOh, 13, "Yugi Muto", PackFlagKind.Shop, (uint)UnlockedShopPacks.Yugi, false),

            (LotdDuelSeries.YuGiOhGX, 1, "Alexis Rhodes", PackFlagKind.Shop, (uint)UnlockedShopPacks.AlexisRhodes, false),
            (LotdDuelSeries.YuGiOhGX, 3, "Bastion Misawa", PackFlagKind.Shop, (uint)UnlockedShopPacks.BastionMisawa, false),
            (LotdDuelSeries.YuGiOhGX, 5, "Chazz Princeton", PackFlagKind.Shop, (uint)UnlockedShopPacks.ChazzPrinceton, false),
            (LotdDuelSeries.YuGiOhGX, 7, "Syrus Truesdale", PackFlagKind.Shop, (uint)UnlockedShopPacks.SyrusTruesdale, false),
            (LotdDuelSeries.YuGiOhGX, 9, "Jesse Anderson", PackFlagKind.Shop, (uint)UnlockedShopPacks.JesseAnderson, false),
            (LotdDuelSeries.YuGiOhGX, 12, "Jaden Yuki", PackFlagKind.Shop, (uint)UnlockedShopPacks.JadenYuki, false),

            (LotdDuelSeries.YuGiOh5D, 1, "Tetsu Trudge", PackFlagKind.Shop, (uint)UnlockedShopPacks.TetsuTrudge, false),
            (LotdDuelSeries.YuGiOh5D, 4, "Leo & Luna", PackFlagKind.Shop, (uint)UnlockedShopPacks.LeoLuna, false),
            (LotdDuelSeries.YuGiOh5D, 6, "Akiza Izinski", PackFlagKind.Shop, (uint)UnlockedShopPacks.AkizaIzinski, false),
            (LotdDuelSeries.YuGiOh5D, 8, "Jack Atlas", PackFlagKind.Shop, (uint)UnlockedShopPacks.JackAtlas, false),
            (LotdDuelSeries.YuGiOh5D, 12, "Crow Hogan", PackFlagKind.Shop, (uint)UnlockedShopPacks.Crow, false),
            (LotdDuelSeries.YuGiOh5D, 15, "Yusei Fudo", PackFlagKind.Shop, (uint)UnlockedShopPacks.YuseiFudo, false),

            (LotdDuelSeries.YuGiOhZEXAL, 1, "Cathy Katherine", PackFlagKind.Shop, (uint)UnlockedShopPacks.CathyKatherine, false),
            (LotdDuelSeries.YuGiOhZEXAL, 4, "Quinton", PackFlagKind.Shop, (uint)UnlockedShopPacks.Quinton, false),
            (LotdDuelSeries.YuGiOhZEXAL, 7, "Kite Tenjo", PackFlagKind.Shop, (uint)UnlockedShopPacks.KiteTenjo, false),
            (LotdDuelSeries.YuGiOhZEXAL, 11, "Shark", PackFlagKind.Shop, (uint)UnlockedShopPacks.Shark, false),
            (LotdDuelSeries.YuGiOhZEXAL, 15, "Yuma Tsukumo", PackFlagKind.Shop, (uint)UnlockedShopPacks.YumaTsukumo, false),

            (LotdDuelSeries.YuGiOhARCV, 1, "Gong Strong", PackFlagKind.Shop, (uint)UnlockedShopPacks.GongStrong, false),
            (LotdDuelSeries.YuGiOhARCV, 6, "Zuzu Boyle", PackFlagKind.Shop, (uint)UnlockedShopPacks.ZuzuBoyle, false),
            (LotdDuelSeries.YuGiOhARCV, 11, "Shay Obsidian", PackFlagKind.Shop, (uint)UnlockedShopPacks.Shay, false),
            (LotdDuelSeries.YuGiOhARCV, 15, "Declan Akaba", PackFlagKind.Shop, (uint)UnlockedShopPacks.DeclanAkaba, false),
            (LotdDuelSeries.YuGiOhARCV, 21, "Yuya Sakaki", PackFlagKind.Shop, (uint)UnlockedShopPacks.YuyaSakaki, false),

            (LotdDuelSeries.YuGiOhVRAINS, 1, "Playmaker", PackFlagKind.Shop, (uint)UnlockedShopPacks.Playmaker, false),
            // Blue Angel/Soulburner/Varis/Ai are sold as normal duelist shop
            // packs in-game, but their real flags live in UnlockedBattlePacks,
            // not UnlockedShopPacks - see that enum's doc comment.
            (LotdDuelSeries.YuGiOhVRAINS, 8, "Blue Angel", PackFlagKind.Battle, (uint)UnlockedBattlePacks.BlueAngel, false),
            (LotdDuelSeries.YuGiOhVRAINS, 13, "Soulburner", PackFlagKind.Battle, (uint)UnlockedBattlePacks.Soulburner, false),
            (LotdDuelSeries.YuGiOhVRAINS, 21, "Varis", PackFlagKind.Battle, (uint)UnlockedBattlePacks.Varis, false),
            (LotdDuelSeries.YuGiOhVRAINS, 25, "Ai", PackFlagKind.Battle, (uint)UnlockedBattlePacks.Ai, false),
        };

        /// <summary>Recomputes every duelist shop/battle pack flag (plus the
        /// Grandpa Muto -> CardShop gate) from the Campaign chunk's current
        /// duel states. Unlocks and re-locks with the milestone stage's
        /// forward state. Call after every Campaign duel-state write.</summary>
        public static void SyncShopPacksFromCampaignState(byte[] save, LotdSaveVersion v)
        {
            var shopPacks = GetShopPacks(save, v);
            var battlePacks = GetBattlePacksFlag(save, v);
            var content = GetUnlockedContent(save, v);

            var series = CampaignSaveLayout.GetSeries(v);
            foreach (var entry in CampaignShopPackUnlocks)
            {
                int seriesIndex = Array.IndexOf(series, entry.Series);
                if (seriesIndex < 0) continue;

                bool complete = CampaignSaveLayout.GetState(save, v, seriesIndex, entry.Stage) == LotdCampaignDuelState.Complete;

                if (entry.Kind == PackFlagKind.Shop)
                    shopPacks = complete ? shopPacks | (UnlockedShopPacks)entry.Flag : shopPacks & ~(UnlockedShopPacks)entry.Flag;
                else
                    battlePacks = complete ? battlePacks | (UnlockedBattlePacks)entry.Flag : battlePacks & ~(UnlockedBattlePacks)entry.Flag;

                if (entry.AlsoUnlocksCardShopGate)
                    content = complete ? content | UnlockedContent.CardShop : content & ~UnlockedContent.CardShop;
            }

            SetShopPacks(save, v, shopPacks);
            SetBattlePacksFlag(save, v, battlePacks);
            SetUnlockedContent(save, v, content);
        }

        /// <summary>True if it's safe to manually unlock a story-stage-gated
        /// shop/battle pack duelist right now. Packs with no
        /// CampaignShopPackUnlocks entry (Epic Dawn/War of the Giants[/Round
        /// 2], gated on Wins_Nonmatch instead - see
        /// StatsLayout.SyncBattlePackUnlocksFromNonmatchWins) always return
        /// true.</summary>
        public static bool CanManuallyUnlockShopPack(byte[] save, LotdSaveVersion v, PackFlagKind kind, uint flag)
        {
            var series = CampaignSaveLayout.GetSeries(v);
            foreach (var entry in CampaignShopPackUnlocks)
            {
                if (entry.Kind != kind || entry.Flag != flag) continue;

                int seriesIndex = Array.IndexOf(series, entry.Series);
                if (seriesIndex < 0) return true; // series not present in this save version - nothing to gate on

                return CampaignSaveLayout.GetState(save, v, seriesIndex, entry.Stage) == LotdCampaignDuelState.Complete;
            }

            return true; // not a story-stage-gated pack (Epic Dawn/War of the Giants[/Round2]) - no campaign gate here
        }

        // ── Duelist Challenges derived from Campaign story progress ─────────────

        /// <summary>One-directional: bumps a duelist's Challenge from Locked
        /// to Available once every real Campaign duel (forward and reverse)
        /// featuring them reads Complete. Never re-locks or downgrades an
        /// already-unlocked challenge. A duelist with zero known Campaign
        /// appearances is left untouched. Call after every Campaign
        /// duel-state write.</summary>
        public static void SyncChallengesFromCampaignState(byte[] save, LotdSaveVersion v)
        {
            var series = CampaignSaveLayout.GetSeries(v);
            int numSlots = LotdSaveFormat.GetNumDeckDataSlots(v);

            foreach (var entry in DuelistChallengeSlots.Entries)
            {
                if (entry.SlotIndex >= numSlots) continue; // slot doesn't exist in this save format - see DuelistChallengeSlots' doc comment
                if (GetChallengeState(save, v, entry.SlotIndex) != DeulistChallengeState.Locked) continue; // already unlocked/attempted/completed - never downgrade

                var (foundAny, allComplete) = GetCampaignCompletionStatus(save, v, series, entry.CharacterId);
                if (foundAny && allComplete)
                    SetChallengeState(save, v, entry.SlotIndex, DeulistChallengeState.Available);
            }
        }

        /// <summary>Same rule as SyncChallengesFromCampaignState, applied to
        /// UnlockedAvatars. Only covers avatar ids 0-152 (UnlockedAvatars is
        /// a fixed 153-slot bitfield) - characters past id 152 have no
        /// avatar slot to unlock. Call after every Campaign duel-state
        /// write.</summary>
        public static void SyncAvatarsFromCampaignState(byte[] save, LotdSaveVersion v)
        {
            var series = CampaignSaveLayout.GetSeries(v);

            for (int avatarId = 0; avatarId < LotdSaveFormat.NumAvatarSlots; avatarId++)
            {
                if (GetAvatarUnlocked(save, v, avatarId)) continue; // already unlocked - never downgrade

                var (foundAny, allComplete) = GetCampaignCompletionStatus(save, v, series, (byte)avatarId);
                if (foundAny && allComplete)
                    SetAvatarUnlocked(save, v, avatarId, true);
            }
        }

        /// <summary>True if it's safe to manually unlock/complete a
        /// character's avatar or challenge right now. Only governs the
        /// unlock direction - locking/resetting is never blocked.</summary>
        public static bool CanManuallyUnlock(byte[] save, LotdSaveVersion v, byte characterId)
        {
            var series = CampaignSaveLayout.GetSeries(v);
            var (_, allComplete) = GetCampaignCompletionStatus(save, v, series, characterId);
            return allComplete;
        }

        /// <summary>Scans every real Campaign duel appearance for
        /// characterId; reports whether any were found (FoundAny) and
        /// whether all read Complete on both forward and reverse
        /// (AllComplete, vacuously true when FoundAny is false).</summary>
        private static (bool FoundAny, bool AllComplete) GetCampaignCompletionStatus(byte[] save, LotdSaveVersion v, LotdDuelSeries[] series, byte characterId)
        {
            bool foundAny = false;

            for (int s = 0; s < series.Length; s++)
            {
                if (!CampaignDuelNames.BySeries.TryGetValue(series[s], out var duels)) continue;

                for (int i = 0; i < duels.Length; i++)
                {
                    var (_, _, ownerA, ownerB) = duels[i];
                    if (ownerA != characterId && ownerB != characterId) continue;

                    foundAny = true;
                    int duelIndex = i + 1; // display number == raw duelIndex - see CampaignDuelNames.Get
                    bool complete = CampaignSaveLayout.GetState(save, v, s, duelIndex) == LotdCampaignDuelState.Complete
                                 && CampaignSaveLayout.GetReverseState(save, v, s, duelIndex) == LotdCampaignDuelState.Complete;
                    if (!complete) return (true, false);
                }
            }

            return (foundAny, true);
        }

        /// <summary>One-directional: unlocks the Duelist Challenges content
        /// gate the moment any real named challenge reads anything other
        /// than Locked. Never re-locks. Call after every Challenge-state
        /// write.</summary>
        public static void SyncChallengeGateFromChallenges(byte[] save, LotdSaveVersion v)
        {
            int numSlots = LotdSaveFormat.GetNumDeckDataSlots(v);
            bool anyUnlocked = DuelistChallengeSlots.Entries
                .Where(entry => entry.SlotIndex < numSlots)
                .Any(entry => GetChallengeState(save, v, entry.SlotIndex) != DeulistChallengeState.Locked);

            if (!anyUnlocked) return;

            var content = GetUnlockedContent(save, v);
            if ((content & UnlockedContent.DuelistChallenges) != 0) return;
            SetUnlockedContent(save, v, content | UnlockedContent.DuelistChallenges);
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
