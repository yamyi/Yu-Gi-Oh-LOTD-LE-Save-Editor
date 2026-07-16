namespace YuGiOhSaveEditor.Wpf.Services;

/// <summary>
/// Episode title + character matchup + both duelists' OwnerDatabase ids
/// for every real (non-padding) duel in each campaign series, in
/// on-screen order (array index 0 = display duel 1 = save array index 1 -
/// index 0 of the save's 50-slot-per-series array is always an unused
/// padding slot; see CampaignSaveLayout's remarks and SaveEditorView's
/// KnownRealDuelRange). Titles/matchups supplied directly by the user from
/// the show's actual episode list; owner ids were cross-referenced against
/// OwnerDatabase - a few characters (Alexis Rhodes, Aster Phoenix, Crow
/// Hogan, Jack Atlas, Kite Tenjo) have two separate ids in that table, one
/// per arc they appear/cameo in, so the id picked here always matches the
/// arc block the duel itself belongs to. Array lengths match
/// KnownRealDuelRange exactly (32/32/32/26/33/28).
/// </summary>
public static class CampaignDuelNames
{
    public static readonly Dictionary<LotdDuelSeries, (string Title, string Matchup, byte OwnerA, byte OwnerB)[]> BySeries = new()
    {
        [LotdDuelSeries.YuGiOh] = new (string Title, string Matchup, byte OwnerA, byte OwnerB)[]
        {
            ("The Duelist Kingdom", "Yugi Muto vs. Joey Wheeler", 105, 81),
            ("The Heart of the Cards", "Yami Yugi vs. Seto Kaiba", 104, 94),
            ("The Ultimate Great Moth", "Yami Yugi vs. Weevil Underwood", 104, 101),
            ("The Harpie Lady", "Joey Wheeler vs. Mai Valentine", 81, 82),
            ("Attack from the Deep", "Yami Yugi vs. Mako Tsunami", 104, 83),
            ("The Dinosaur Duelist", "Joey Wheeler vs. Rex Raptor", 81, 92),
            ("Evil Spirit of the Ring", "Yami Yugi vs. Yami Bakura", 104, 102),
            ("PaniK Attack", "Yami Yugi vs. PaniK", 104, 90),
            ("Arena of Lost Souls", "Joey Wheeler vs. Bonz", 81, 78),
            ("The Face Off", "Seto Kaiba vs. Yami Yugi", 94, 104),
            ("A Duel of Tears", "Téa Gardner vs. Mai Valentine", 98, 82),
            ("Champion vs. Creator", "Maximillion Pegasus vs. Seto Kaiba", 84, 94),
            ("Duel Identity", "Yami Yugi vs. Mai Valentine", 104, 82),
            ("Keith's Machinations", "Joey Wheeler vs. Bandit Keith", 81, 77),
            ("Best Friends, Best Duelists", "Yami Yugi vs. Joey Wheeler", 104, 81),
            ("The Match of the Millennium", "Yami Yugi vs. Maximillion Pegasus", 104, 84),
            ("The Mystery Duelist", "Yugi Muto vs. Bandit Keith", 105, 77),
            ("Battle City Begins", "Yami Yugi vs. Seeker", 104, 93),
            ("The Master of Magicians", "Yami Yugi vs. Arkana", 104, 76),
            ("Playing with a Parasite", "Joey Wheeler vs. Weevil Underwood", 81, 101),
            ("Mime Control", "Yami Yugi vs. Strings", 104, 97),
            ("The Dark Spirit Revealed", "Yami Yugi vs. Yami Bakura", 104, 102),
            ("The Awakening of Evil", "Joey Wheeler vs. Odion", 81, 89),
            ("A Duel with Destiny", "Seto Kaiba vs. Ishizu Ishtar", 94, 80),
            ("Clash in the Colosseum", "Yami Yugi vs. Seto Kaiba", 104, 94),
            ("The Final Face Off", "Yami Yugi vs. Yami Marik", 104, 103),
            ("A New Evil", "Joey Wheeler vs. Mai Valentine", 81, 82),
            ("Deja Duel", "Seto Kaiba vs. Alister", 94, 75),
            ("Fighting for a Friend", "Joey Wheeler vs. Valon", 81, 100),
            ("Grappling with a Guardian", "Yami Yugi vs. Rafael", 104, 91),
            ("A Duel with Dartz!", "Yami Yugi vs. Dartz", 104, 79),
            ("The Final Duel", "Yugi Muto vs. Yami Yugi", 105, 104),
        },

        [LotdDuelSeries.YuGiOhGX] = new (string Title, string Matchup, byte OwnerA, byte OwnerB)[]
        {
            ("The Next King of Games", "Jaden Yuki vs. Dr. Vellian Crowler", 119, 133),
            ("A Duel in Love", "Jaden Yuki vs. Alexis Rhodes", 119, 107),
            ("The Shadow Duelist", "Jaden Yuki vs. Titan", 119, 131),
            ("For the Sake of Syrus", "Zane Truesdale vs. Jaden Yuki", 137, 119),
            ("Formula for Success", "Bastion Misawa vs. Chazz Princeton", 113, 117),
            ("Doomsday Duel", "Jaden Yuki vs. Nightshroud", 119, 124),
            ("Field of Screams", "Jaden Yuki vs. Camula", 119, 116),
            ("Duel Distractions", "Jaden Yuki vs. Tania", 119, 129),
            ("A Reason to Win", "Alexis Rhodes vs. Titan", 107, 131),
            ("Amnael's Endgame", "Jaden Yuki vs. Amnael", 119, 108),
            ("Rise of the Sacred Beasts", "Jaden Yuki vs. Kagemaru", 119, 122),
            ("Magna Chum Laude", "Dr. Vellian Crowler vs. Chumley Huffington", 133, 118),
            ("The Graduation Match", "Jaden Yuki vs. Zane Truesdale", 119, 137),
            ("A New Breed of Hero", "Aster Phoenix vs. Jaden Yuki", 109, 119),
            ("Schooling the Master", "Zane Truesdale vs. Chancellor Sheppard", 137, 127),
            ("Blinded by the Light", "Jaden Yuki vs. Chazz Princeton", 119, 117),
            ("Duel for Hire", "Maximillion Pegasus vs. Dr. Vellian Crowler", 84, 133),
            ("Heart of Ice", "Jaden Yuki vs. Alexis Rhodes", 119, 107),
            ("Tough Love", "Zane Truesdale vs. Syrus Truesdale", 137, 128),
            ("The Hand of Justice", "Jaden Yuki vs. Sartorius Kumar", 119, 126),
            ("Future Changes", "Chazz Princeton vs. Blair Flannigan", 117, 114),
            ("A Jewel of a Duel", "Jaden Yuki vs. Jesse Anderson", 119, 120),
            ("Hanging with Axel", "Jaden Yuki vs. Axel Brodie", 119, 112),
            ("Primal Instinct", "Jim \"Crocodile\" Cook vs. Tyranno Hassleberry", 121, 132),
            ("Head in the Clouds", "Adrian Gecko vs. Chazz Princeton", 106, 117),
            ("A Snake in the Grass", "Jaden Yuki vs. Prof. Thelonious Viper", 119, 130),
            ("A Dimensional Duel", "Jesse Anderson vs. Zane Truesdale", 120, 137),
            ("Unleashing the Dragon", "Jaden Yuki vs. Marcel Bonaparte", 119, 123),
            ("The Power Within", "Jaden Yuki vs. Jesse Anderson", 119, 120),
            ("Return of the Supreme King", "Jaden Yuki vs. Yubel", 119, 136),
            ("Darkness Returns", "Jaden Yuki vs. Nightshroud", 119, 124),
            ("The Legendary Duelist", "Jaden Yuki vs. Yami Yugi", 119, 104),
        },

        [LotdDuelSeries.YuGiOh5D] = new (string Title, string Matchup, byte OwnerA, byte OwnerB)[]
        {
            ("Ready, Set, Duel", "Yusei Fudo vs. Tetsu Trudge", 35, 34),
            ("A Blast from the Past", "Yusei Fudo vs. Jack Atlas", 35, 16),
            ("The Facility", "Yusei Fudo vs. Bolt Tanner", 35, 5),
            ("The Lockdown Duel", "Yusei Fudo vs. Mr. Armstrong", 35, 26),
            ("The Take Back", "Yusei Fudo vs. Tetsu Trudge", 35, 34),
            ("Welcome to the Fortune Cup", "Greiger vs. Leo", 13, 21),
            ("Surprise Surprise", "Yusei Fudo vs. Hunter Pace", 35, 15),
            ("Second Round Showdown", "Yusei Fudo vs. Greiger", 35, 13),
            ("Duel of Dragons", "Yusei Fudo vs. Akiza Izinski", 35, 1),
            ("The Fortune Cup Finale", "Yusei Fudo vs. Jack Atlas", 35, 16),
            ("Supersensory Shakedown", "Sayer vs. Leo", 31, 21),
            ("Digging Deeper", "Sayer vs. Carly Carmine", 31, 9),
            ("Mark of the Monkey", "Leo vs. Devack", 21, 11),
            ("A Whale of a Ride", "Crow Hogan vs. Greiger", 10, 13),
            ("A Score to Settle", "Yusei Fudo vs. Kalin Kessler", 35, 19),
            ("Destiny's Will", "Yusei Fudo vs. Roman Goodwin", 35, 30),
            ("Shadows of Doubt", "Jack Atlas vs. Carly Carmine", 16, 9),
            ("Truth and Consequences", "Akiza Izinski vs. Misty Tredwell", 1, 25),
            ("Signs of Doom", "Yusei Fudo vs. Rex Goodwin", 35, 29),
            ("French Twist", "Yusei Fudo vs. Sherry LeBlanc", 35, 32),
            ("The Edge of Elimination Part 1", "Yusei Fudo vs. Andre", 35, 2),
            ("The Edge of Elimination Part 2", "Yusei Fudo vs. Breo", 35, 6),
            ("The Edge of Elimination Part 3", "Yusei Fudo vs. Jean", 35, 18),
            ("Duel for Redemption", "Jack Atlas vs. Dragan", 16, 12),
            ("Tricking the Trickster", "Crow Hogan vs. Broder", 10, 7),
            ("Tricking the Trickster 2", "Yusei Fudo vs. Halldor", 35, 14),
            ("The Beginning of the End", "Jack Atlas vs. Lester", 16, 23),
            ("Dawn of the Machines", "Jack Atlas vs. Primo", 16, 27),
            ("Victory or Doom", "Yusei Fudo vs. Aporia", 35, 4),
            ("Fight for the Future", "Yusei Fudo vs. Antinomy", 35, 3),
            ("Hope", "Yusei Fudo vs. Zone", 35, 36),
            ("Future Path", "Yusei Fudo vs. Jack Atlas", 35, 16),
        },

        [LotdDuelSeries.YuGiOhZEXAL] = new (string Title, string Matchup, byte OwnerA, byte OwnerB)[]
        {
            ("Go with the Flow", "Yuma Tsukumo vs. Reginald Kastle", 174, 168),
            ("Flipping Out", "Yuma Tsukumo vs. Flip Turner", 174, 153),
            ("The Sparrow", "Yuma Tsukumo vs. Nelson Andrews", 174, 161),
            ("Feline Frenzy", "Yuma Tsukumo vs. Cathy Katherine", 174, 148),
            ("Roots of the Problem", "Bronk Stone vs. Number 96", 147, 163),
            ("Love Hurts", "Yuma Tsukumo vs. Anna Kaboom", 174, 145),
            ("Double Jeopardy", "Yuma Tsukumo vs. Dextra", 174, 149),
            ("The Dragon Awakens", "Kite Tenjo vs. Trey", 156, 171),
            ("Cosmic Chaos", "Kite Tenjo vs. Quinton", 156, 166),
            ("Swimming with Sharks", "Reginald Kastle vs. Quattro", 168, 165),
            ("Rockin' and Rollin'", "Yuma Tsukumo vs. Nistro", 174, 162),
            ("Duel of Destiny", "Yuma Tsukumo vs. Reginald Kastle", 174, 168),
            ("Sphere of Fear", "Yuma Tsukumo vs. Vetrix", 174, 173),
            ("A Trio's Challenge", "Yuma Tsukumo vs. Dr. Faker", 174, 151),
            ("Counter Offensive", "ZEXAL vs. Alito", 175, 144),
            ("Dual Duel", "Ray Shadows vs. Girag", 167, 154),
            ("Search for Shadows, Pt. 1", "Reginald Kastle vs. Dumon", 168, 152),
            ("Search for Shadows, Pt. 2", "Kite Tenjo vs. Mizar", 156, 158),
            ("Search for Shadows, Pt. 3", "Yuma Tsukumo vs. Vector", 174, 172),
            ("Barian Vengeance", "Yuma Tsukumo vs. Nistro", 174, 162),
            ("A Sea of Troubles", "Reginald Kastle vs. Rio Kastle", 168, 169),
            ("A World of Chaos", "Yuma Tsukumo vs. Number 96", 174, 163),
            ("Clash of the Emperors", "Nash vs. Vector", 160, 172),
            ("The New World", "ZEXAL 3 vs. Don Thousand", 176, 150),
            ("The Battle of Three Worlds", "Yuma Tsukumo vs. Nash", 174, 160),
            ("Forever ZEXAL", "Yuma Tsukumo vs. Astral", 174, 146),
        },

        [LotdDuelSeries.YuGiOhARCV] = new (string Title, string Matchup, byte OwnerA, byte OwnerB)[]
        {
            ("Swing Into Action", "Yuya Sakaki vs. The Sledgehammer", 72, 64),
            ("Trade Bait", "Yuya Sakaki vs. Sylvio Sawatari", 72, 62),
            ("Dueling with the Stars", "Yuya Sakaki vs. Dipper O'rion", 72, 49),
            ("You Show 'Em!", "Julia Krystal vs. Zuzu Boyle", 55, 73),
            ("Going, Going, Gong", "Gong Strong vs. Kit Blade", 47, 67),
            ("A Date With Fate", "Yuya Sakaki vs. Aura Sentia", 72, 39),
            ("Fusion Foes", "Zuzu Boyle vs. Julia Krystal", 73, 55),
            ("The Pendulum Swings Both Ways", "Yuya Sakaki vs. Sylvio Sawatari", 72, 62),
            ("Making the Cut", "Shay Obsidian vs. Sora Perse", 61, 65),
            ("A Dark Reflection", "Yugo vs. Yuto", 68, 71),
            ("Obelisk Assault", "Celina vs. Dennis McField", 40, 44),
            ("Battlefronts", "Shadow Moon vs. Obelisk Force", 66, 57),
            ("Fighting for Fun", "Yuya Sakaki vs. Sora Perse", 72, 65),
            ("City 'Scape", "Yugo vs. Officer 227", 68, 45),
            ("Crow's Crew", "Crow Hogan vs. Gong Strong", 42, 47),
            ("A Concerted Effort", "Zuzu Boyle vs. Chojiro Tokumatsu", 73, 41),
            ("Turbotainers", "Shay Obsidian vs. Dennis McField", 61, 44),
            ("Battle Birds", "Crow Hogan vs. Shay Obsidian", 42, 61),
            ("The Many Dimensions of Yuya", "Yuya Sakaki vs. Crow Hogan", 72, 42),
            ("Chain Game", "Yugo vs. Yuri", 68, 69),
            ("Friendship Finale", "Yuya Sakaki vs. Jack Atlas", 72, 50),
            ("A Vicious Cycle", "Declan Akaba vs. Jean Michel Roget", 43, 51),
            ("Rush to Revenge", "Yuya Sakaki vs. Kite Tenjo", 72, 52),
            ("Rise of the Resistance", "Alexis Rhodes vs. Obelisk Force", 37, 57),
            ("Last Laugh", "Yuya Sakaki vs. Aster Phoenix", 72, 38),
            ("All Duel Hands on Deck", "Kite Tenjo vs. Dennis McField", 52, 44),
            ("Grip of the Parasite", "Rin vs. Yugo", 60, 68),
            ("Family Face Off", "Shay Obsidian vs. Lulu", 61, 54),
            ("Duel Interrupted", "Yuri vs. Yugo", 69, 68),
            ("Time to Reunite", "Yuya Sakaki vs. Yuri", 72, 69),
            ("A Ray of Hope", "Declan Akaba vs. Z-ARC", 43, 74),
            ("One Last Duel", "Yuya Sakaki vs. Declan Akaba", 72, 43),
            ("That's a Wrap!", "Yuya Sakaki vs. Yusho Sakaki", 72, 70),
        },

        [LotdDuelSeries.YuGiOhVRAINS] = new (string Title, string Matchup, byte OwnerA, byte OwnerB)[]
        {
            ("My Name is Playmaker", "Playmaker vs. Knight of Hanoi", 143, 142),
            ("The Three Count Rings", "Playmaker vs. The Gore", 143, 141),
            ("Hanoi's Angel", "Playmaker vs. Blue Angel", 143, 140),
            ("A Storm is Coming", "Playmaker vs. Varis", 143, 189),
            ("Ghost Gal's Invitation", "Playmaker vs. Ghost Gal", 143, 183),
            ("Dueling for Answers", "Playmaker vs. Akira Zaizen", 143, 178),
            ("Under VRAINS", "Varis vs. Ghost Gal", 189, 183),
            ("Once Upon a Time", "Spectre vs. Blue Angel", 188, 140),
            ("A Bridge Too Far", "Playmaker vs. Spectre", 143, 188),
            ("Gore at War", "Varis vs. The Gore", 189, 141),
            ("Link to the Future", "Playmaker vs. Varis", 143, 189),
            ("The Bounty Hunter", "Playmaker vs. The Shepherd", 143, 179),
            ("Blue Gal's Here!", "Soulburner vs. Blue Gal", 187, 180),
            ("Tuning Rokkets", "Varis vs. Windy", 189, 191),
            ("Battle of the Siblings", "The Shepherd vs. Ghost Gal", 179, 183),
            ("Friend or Foe", "Varis vs. The Shepherd", 189, 179),
            ("Specter's Plan", "Lightning vs. Spectre", 185, 188),
            ("Windy's Revenge", "Soulburner vs. Windy", 187, 191),
            ("A New World Coming", "Bohman vs. Blue Maiden", 182, 181),
            ("Burning Soul", "Bohman vs. Soulburner", 182, 187),
            ("Anything to Win", "Varis vs. Lightning", 189, 185),
            ("A Wish Entrusted", "Playmaker vs. Bohman", 143, 182),
            ("Go Go Roboppi!", "Roboppi vs. Ghost Gal", 186, 183),
            ("Unbreakable Spirit", "Ai vs. The Gore", 177, 141),
            ("Ai's Conundrum", "Ai vs. Blue Maiden", 177, 181),
            ("Roboppi's Dream", "Soulburner vs. Roboppi", 187, 186),
            ("Back to the Beginning", "Soulburner vs. Varis", 187, 189),
            ("Separate Ways", "Playmaker vs. Ai", 143, 177),
        },
    };

    /// <summary>Looks up the title/matchup/owner-ids for a series' Nth
    /// (1-based) displayed duel. Returns null if the series or index isn't
    /// in the table above - callers fall back to a plain "Duel N" label
    /// and owner id 0 (PortraitProvider returns no image for an unknown id).</summary>
    public static (string Title, string Matchup, byte OwnerA, byte OwnerB)? Get(LotdDuelSeries series, int displayNumber)
    {
        if (!BySeries.TryGetValue(series, out var duels)) return null;
        int idx = displayNumber - 1;
        if (idx < 0 || idx >= duels.Length) return null;
        return duels[idx];
    }
}
