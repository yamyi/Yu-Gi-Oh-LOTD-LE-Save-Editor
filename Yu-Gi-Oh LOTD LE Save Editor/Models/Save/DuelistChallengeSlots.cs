namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Maps each real duelist's Challenge Deck ID (chardata_X.bin's own field of
    /// that name) to its slot index in MiscSaveLayout's Challenges/UnlockedRecipes
    /// arrays - the missing link needed to unlock/complete a single named
    /// duelist's challenge, rather than only the existing bulk
    /// Unlock/Complete/Reset-everything buttons.
    ///
    /// Source: the user extracted chardata_X.bin from their own Link Evolution
    /// install and read it with nzxth2's YGO_LOTD_LE_CharData_Editor
    /// (https://github.com/nzxth2/YGO_LOTD_LE_CharData_Editor), which decodes
    /// each character's Challenge Deck ID field directly - not a guess or a
    /// derived value. Supplied 2026-07-25 as a 191-row CharacterID/Name/
    /// ChallengeDeckID dump; entries whose ChallengeDeckID read back as
    /// 4294967295 (0xFFFFFFFF, i.e. -1 as uint32) mean "this character has no
    /// challenge deck" (cameos, alternate forms, NPCs with no playable
    /// Duelist Challenge) and are excluded here. That leaves exactly 158 real
    /// entries - matching the user's own independent recollection of "158
    /// duelist challenges" before this data ever came in, which is the
    /// strongest confirmation available that the filtering is correct.
    ///
    /// CharacterId here is pixeltris/Lotd's chardata.bin character id space -
    /// confirmed identical to OwnerDatabase's byte keys by cross-checking all
    /// 158 names against OwnerDatabase.GetName (0 mismatches, 0 missing), so
    /// OwnerDatabase/OwnerPortraitConverter can be reused directly for this
    /// tab's labels and portraits instead of duplicating name strings here.
    /// A few names repeat across two different-era appearances of the same
    /// character (e.g. Alexis Rhodes ids 37 and 107, Crow Hogan ids 10 and 42,
    /// Jack Atlas ids 16 and 50, Kite Tenjo ids 52 and 156, Aster Phoenix ids
    /// 38 and 109, The Gore ids 141 and 184, Varis ids 189 and 190) - each is
    /// still a distinct real challenge with its own slot, just sharing a
    /// display name; the UI disambiguates by appending the CharacterId.
    /// </summary>
    public static class DuelistChallengeSlots
    {
        /// <summary>Which duel series a CharacterId belongs to, so the
        /// Challenges tab can group/filter the same way the Campaign tab
        /// does. Derived from chardata.bin's own character ordering - ids
        /// are laid out in contiguous per-series blocks (confirmed against
        /// MoonlitDeath's Character &amp; Arena ID wiki page, and cross-checked
        /// against every name in Entries below: block boundaries land
        /// exactly on the first/last real character of each series, e.g. id
        /// 36 "Zone" is 5D's last and id 37 "Alexis Rhodes" is ARC-V's
        /// first). VRAINS is split into two blocks (138-143, then 177-191)
        /// because ZEXAL's block (144-176) was inserted between them in
        /// chardata.bin - both halves map to YuGiOhVRAINS here.</summary>
        public static LotdDuelSeries GetSeries(byte characterId) => characterId switch
        {
            >= 1 and <= 36 => LotdDuelSeries.YuGiOh5D,
            >= 37 and <= 74 => LotdDuelSeries.YuGiOhARCV,
            >= 75 and <= 105 => LotdDuelSeries.YuGiOh,
            >= 106 and <= 137 => LotdDuelSeries.YuGiOhGX,
            >= 138 and <= 143 => LotdDuelSeries.YuGiOhVRAINS,
            >= 144 and <= 176 => LotdDuelSeries.YuGiOhZEXAL,
            >= 177 and <= 191 => LotdDuelSeries.YuGiOhVRAINS,
            _ => LotdDuelSeries.YuGiOh,
        };

        /// <summary>(CharacterId matching OwnerDatabase, slot index into
        /// MiscSaveLayout's Challenges/UnlockedRecipes arrays), one row per
        /// real Duelist Challenge - see this class's doc comment for
        /// provenance.</summary>
        public static readonly (byte CharacterId, int SlotIndex)[] Entries =
        {
            (1, 63), // Akiza Izinski
            (2, 64), // Andre
            (3, 65), // Antinomy
            (4, 66), // Aporia
            (5, 68), // Bolt Tanner
            (6, 69), // Breo
            (7, 70), // Broder
            (9, 71), // Carly Carmine
            (10, 72), // Crow Hogan
            (11, 73), // Devack
            (12, 74), // Dragan
            (13, 75), // Greiger
            (14, 76), // Halldor
            (15, 77), // Hunter Pace
            (16, 78), // Jack Atlas
            (18, 79), // Jean
            (19, 80), // Kalin Kessler
            (21, 81), // Leo
            (23, 82), // Lester
            (25, 83), // Misty Tredwell
            (26, 67), // Mr. Armstrong
            (27, 84), // Primo
            (29, 85), // Rex Goodwin
            (30, 86), // Roman Goodwin
            (31, 87), // Sayer
            (32, 88), // Sherry LeBlanc
            (34, 89), // Tetsu Trudge
            (35, 90), // Yusei Fudo
            (36, 91), // Zone
            (37, 160), // Alexis Rhodes
            (38, 161), // Aster Phoenix
            (39, 162), // Aura Sentia
            (40, 163), // Celina
            (41, 164), // Chojiro Tokumatsu
            (42, 165), // Crow Hogan
            (43, 166), // Declan Akaba
            (44, 167), // Dennis McField
            (45, 168), // Officer 227
            (47, 169), // Gong Strong
            (49, 170), // Dipper O'rion
            (50, 171), // Jack Atlas
            (51, 172), // Jean Michel Roget
            (52, 173), // Kite Tenjo
            (54, 174), // Lulu
            (55, 175), // Julia Krystal
            (57, 176), // Obelisk Force
            (60, 177), // Rin
            (61, 178), // Shay Obsidian
            (62, 179), // Sylvio Sawatari
            (64, 180), // The Sledgehammer
            (65, 181), // Sora Perse
            (66, 182), // Moon Shadow
            (67, 183), // Kit Blade
            (68, 184), // Yugo
            (69, 185), // Yuri
            (70, 186), // Yusho Sakaki
            (71, 187), // Yuto
            (72, 188), // Yuya Sakaki
            (73, 189), // Zuzu Boyle
            (74, 190), // Z-ARC
            (75, 268), // Alister
            (76, 245), // Arkana
            (77, 247), // Bandit Keith
            (78, 248), // Bonz
            (79, 269), // Dartz
            (80, 249), // Ishizu Ishtar
            (81, 250), // Joey Wheeler
            (82, 251), // Mai Valentine
            (83, 252), // Mako Tsunami
            (84, 255), // Maximillion Pegasus
            (89, 253), // Odion
            (90, 254), // PaniK
            (91, 270), // Rafael
            (92, 256), // Rex Raptor
            (93, 257), // Seeker
            (94, 258), // Seto Kaiba
            (97, 259), // Strings
            (98, 260), // Téa Gardner
            (100, 271), // Valon
            (101, 261), // Weevil Underwood
            (102, 246), // Yami Bakura
            (103, 262), // Yami Marik
            (104, 263), // Yami Yugi
            (105, 264), // Yugi Muto
            (106, 346), // Adrian Gecko
            (107, 347), // Alexis Rhodes
            (108, 363), // Amnael
            (109, 348), // Aster Phoenix
            (110, 349), // Masked Atticus
            (112, 350), // Axel Brodie
            (113, 351), // Bastion Misawa
            (114, 352), // Blair Flannigan
            (116, 353), // Camula
            (117, 354), // Chazz Princeton
            (118, 355), // Chumley Huffington
            (119, 357), // Jaden Yuki
            (120, 358), // Jesse Anderson
            (121, 359), // Jim Crocodile Cook
            (122, 360), // Kagemaru
            (123, 361), // Marcel Bonaparte
            (124, 362), // Nightshroud
            (126, 364), // Sartorius Kumar
            (127, 376), // Chancellor Sheppard
            (128, 365), // Syrus Truesdale
            (129, 366), // Tania
            (130, 367), // Prof. Thelonious Viper
            (131, 368), // Titan
            (132, 369), // Tyranno Hassleberry
            (133, 356), // Dr. Vellian Crowler
            (136, 370), // Yubel
            (137, 371), // Zane Truesdale
            (140, 391), // Blue Angel
            (141, 392), // The Gore
            (142, 393), // Knight of Hanoi
            (143, 394), // Playmaker
            (144, 447), // Alito
            (145, 448), // Anna Kaboom
            (146, 449), // Astral
            (147, 450), // Bronk Stone
            (148, 451), // Cathy Katherine
            (149, 453), // Dextra
            (150, 454), // Don Thousand
            (151, 455), // Dr. Faker
            (152, 456), // Dumon
            (153, 457), // Flip Turner
            (154, 458), // Girag
            (156, 459), // Kite Tenjo
            (158, 460), // Mizar
            (160, 461), // Nash
            (161, 462), // Nelson Andrews
            (162, 463), // Nistro
            (163, 452), // Number 96
            (165, 464), // Quattro
            (166, 465), // Quinton
            (167, 466), // Ray Shadows
            (168, 468), // Reginald Kastle
            (169, 467), // Rio Kastle
            (171, 469), // Trey
            (172, 470), // Vector
            (173, 471), // Vetrix
            (174, 472), // Yuma Tsukumo
            (175, 473), // ZEXAL
            (176, 474), // ZEXAL 3
            (177, 529), // Ai
            (178, 530), // Akira Zaizen
            (179, 531), // The Shepherd
            (180, 532), // Blue Gal
            (181, 533), // Blue Maiden
            (182, 534), // Bohman
            (183, 535), // Ghost Gal
            (184, 536), // The Gore
            (185, 537), // Lightning
            (186, 538), // Roboppi
            (187, 539), // Soulburner
            (188, 540), // Spectre
            (189, 541), // Varis
            (190, 542), // Varis
            (191, 543), // Windy
        };
    }
}
