using System.IO;
using System.Reflection;
using System.Text.Json;


namespace YuGiOhDeckManager.Wpf.Services
{
    public sealed class CardDatabase
    {
        private  List<Card> cards=new List<Card>();
        private  Dictionary<int, Card> byId = new Dictionary<int, Card>();
        private  Dictionary<int, Card> byLotdId=new Dictionary<int, Card>();

        private CardDatabase(List<Card> cards_list)
        {
            cards = cards_list;
        }

        public CardDatabase()
        {
        }

        public void Load()
        {
            // Idempotent: MainForm calls this on every construction, including
            // the soft restart a theme change triggers (see Program.cs) — no
            // need to re-parse the embedded card JSON if it's already loaded.
            if (cards.Count > 0) return;

            string resourceName = "YuGiOhDeckManager.Wpf.Assets.cards.Cards.json";

            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException(
                    $"Embedded resource '{resourceName}' not found. " +
                    $"Available resources: {string.Join(", ", assembly.GetManifestResourceNames())}");

            var root = JsonSerializer.Deserialize<CardRoot>(stream)
                ?? throw new InvalidDataException("Failed to parse Cards.json");

            var rootcards = root.Data;

            cards = rootcards;

            // Index by every passcode a card can be referenced by — its own
            // id plus every alternate-art id listed in card_images (e.g.
            // Number 39: Utopia is 84013237, but its alt art 84013238 should
            // resolve to the same Card). A plain ToDictionary(c => c.Id)
            // would miss those alt-art passcodes when importing a .ydk that
            // references them.
            byId = new Dictionary<int, Card>();
            foreach (var c in cards)
            {
                byId[c.Id] = c;

                if (c.CardImages is null) continue;
                foreach (var img in c.CardImages)
                    byId[img.Id] = c;
            }

            byLotdId = cards
                .Where(c => c.LotdId.HasValue)
                .ToDictionary(c => c.LotdId!.Value);
        }

        // O(1) lookups
        public Card? GetById(int id) => byId.GetValueOrDefault(id);
        public Card? GetByLotdId(int lotdId)
        {
            return byLotdId.GetValueOrDefault(lotdId);
        }

        // Exact ID lookup — O(1)

        // Name contains — case-insensitive. Pass includeDescription to also
        // match against the card's rules text.
        public IEnumerable<Card> Search(string query, bool includeDescription = false) =>
            cards.Where(c =>
                c.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                (includeDescription && c.Desc.Contains(query, StringComparison.OrdinalIgnoreCase)));

        // Combine multiple filters — covers every commonly-searched card
        // property (name/description, passcode, type/attribute/race/archetype,
        // level, ATK/DEF range, link rating, pendulum scale).
        public IEnumerable<Card> Filter(
            string? name = null,
            string? type = null,
            string? attribute = null,
            string? race = null,
            string? archetype = null,
            int? id = null,
            int? minLevel = null,
            int? maxLevel = null,
            int? minAtk = null,
            int? maxAtk = null,
            int? minDef = null,
            int? maxDef = null,
            int? minLinkVal = null,
            int? maxLinkVal = null,
            int? minScale = null,
            int? maxScale = null,
            bool searchDescription = false)
        {
            IEnumerable<Card> q = cards;

            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(c =>
                    c.Name.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                    (searchDescription && c.Desc.Contains(name, StringComparison.OrdinalIgnoreCase)));

            if (id.HasValue)
                q = q.Where(c => c.Id == id.Value);

            if (!string.IsNullOrWhiteSpace(type))
                q = q.Where(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(attribute))
                q = q.Where(c => c.Attribute?.Equals(attribute, StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(race))
                q = q.Where(c => c.Race.Equals(race, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(archetype))
                q = q.Where(c => c.Archetype?.Equals(archetype, StringComparison.OrdinalIgnoreCase) == true);

            if (minLevel.HasValue)
                q = q.Where(c => c.Level >= minLevel);

            if (maxLevel.HasValue)
                q = q.Where(c => c.Level <= maxLevel);

            if (minAtk.HasValue)
                q = q.Where(c => c.Atk >= minAtk);

            if (maxAtk.HasValue)
                q = q.Where(c => c.Atk <= maxAtk);

            if (minDef.HasValue)
                q = q.Where(c => c.Def >= minDef);

            if (maxDef.HasValue)
                q = q.Where(c => c.Def <= maxDef);

            if (minLinkVal.HasValue)
                q = q.Where(c => c.LinkVal >= minLinkVal);

            if (maxLinkVal.HasValue)
                q = q.Where(c => c.LinkVal <= maxLinkVal);

            if (minScale.HasValue)
                q = q.Where(c => c.Scale >= minScale);

            if (maxScale.HasValue)
                q = q.Where(c => c.Scale <= maxScale);

            return q;
        }
        // Filtered searches — call on a background thread if needed

        public IEnumerable<Card> GetByType(string type) =>
            cards.Where(c => c.Type.Equals(type, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<Card> GetByArchetype(string archetype) =>
            cards.Where(c => c.Archetype?.Equals(archetype, StringComparison.OrdinalIgnoreCase) == true);

        // Distinct value lists for populating filter UI dropdowns.
        public IReadOnlyList<string> DistinctTypes() =>
            cards.Select(c => c.Type).Where(t => !string.IsNullOrWhiteSpace(t)).Distinct().OrderBy(t => t).ToList();

        public IReadOnlyList<string> DistinctAttributes() =>
            cards.Select(c => c.Attribute).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().OrderBy(a => a).ToList()!;

        public IReadOnlyList<string> DistinctRaces() =>
            cards.Select(c => c.Race).Where(r => !string.IsNullOrWhiteSpace(r)).Distinct().OrderBy(r => r).ToList();

        public IReadOnlyList<string> DistinctArchetypes() =>
            cards.Select(c => c.Archetype).Where(a => !string.IsNullOrWhiteSpace(a)).Distinct().OrderBy(a => a).ToList()!;
    }
}