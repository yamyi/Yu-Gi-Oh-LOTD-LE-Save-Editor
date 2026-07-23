

using System.IO;

namespace YuGiOhSaveEditor.Services
{
    /// <summary>One .ydk line whose passcode didn't resolve to any card in
    /// CardDatabase - the passcode as written in the file, and which section
    /// (Main/Extra/Side) it was listed under. DeckSlotsView shows these back
    /// to the user after import so a stale/foreign-format .ydk doesn't just
    /// silently drop cards.</summary>
    public sealed record YdkSkippedCard(string Section, int Passcode);

    public static class YDKReader
    {
        /// <summary>Convenience overload for callers that don't care about
        /// skipped cards.</summary>
        public static Deck Parse(string path, CardDatabase db) => Parse(path, db, out _);

        public static Deck Parse(string path, CardDatabase db, out List<YdkSkippedCard> skipped)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("YDK file not found.", path);

            var main = new List<Card>();
            var extra = new List<Card>();
            var side = new List<Card>();
            skipped = new List<YdkSkippedCard>();

            List<Card> current = new List<Card>();
            string sectionName = "Main";

            foreach (var rawLine in File.ReadAllLines(path))
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("#main", StringComparison.OrdinalIgnoreCase))
                {
                    current = main;
                    sectionName = "Main";
                    continue;
                }

                if (line.StartsWith("#extra", StringComparison.OrdinalIgnoreCase))
                {
                    current = extra;
                    sectionName = "Extra";
                    continue;
                }

                if (line.StartsWith("!side", StringComparison.OrdinalIgnoreCase))
                {
                    current = side;
                    sectionName = "Side";
                    continue;
                }

                if (int.TryParse(line, out int passcode))
                {
                    Card? card = db.GetById(passcode);
                    if (card is not null)
                        current.Add(card);
                    else
                        skipped.Add(new YdkSkippedCard(sectionName, passcode));
                }
            }

            return new Deck(main, extra, side);
        }
    }
}
