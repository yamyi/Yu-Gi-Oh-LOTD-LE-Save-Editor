

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services
{

    public static class YDKReader
    {
        public static Deck Parse(string path, CardDatabase db)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("YDK file not found.", path);

            var main = new List<Card>();
            var extra = new List<Card>();
            var side = new List<Card>();

            List<Card> current = new List<Card>();

            foreach (var rawLine in File.ReadAllLines(path))
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("#main", StringComparison.OrdinalIgnoreCase))
                {
                    current = main;
                    continue;
                }

                if (line.StartsWith("#extra", StringComparison.OrdinalIgnoreCase))
                {
                    current = extra;
                    continue;
                }

                if (line.StartsWith("!side", StringComparison.OrdinalIgnoreCase))
                {
                    current = side;
                    continue;
                }

                if (int.TryParse(line, out int passcode))
                {
                    Card? card = db.GetById(passcode);
                    if (card is not null)
                        current.Add(card);
                }
            }

            return new Deck(main, extra, side);
        }
    }
}
