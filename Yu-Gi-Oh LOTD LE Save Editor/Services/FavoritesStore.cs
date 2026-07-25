using System.IO;
using System.Text.Json;

namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Persists the set of "favorited" cards (by Card.Id - the ygoprodeck
    /// passcode, stable across saves/slots unlike LotdId) to a small JSON
    /// file under %LocalAppData%, same folder/pattern AppSettings.cs uses -
    /// so a card starred in Card Search or Deck Editor's browse list stays
    /// starred across restarts. Loaded once, lazily, the first time
    /// anything asks; every toggle re-writes the whole file immediately -
    /// there's never more than a few hundred entries, so this is cheap and
    /// needs no batching/debounce.
    /// </summary>
    public static class FavoritesStore
    {
        private static readonly string path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Yu-Gi-Oh LOTD LE Deck Editor", "favorites.json");

        private static HashSet<int>? _favorites;

        private static HashSet<int> Favorites
        {
            get
            {
                if (_favorites != null) return _favorites;

                try
                {
                    if (File.Exists(path))
                    {
                        var ids = JsonSerializer.Deserialize<List<int>>(File.ReadAllText(path));
                        if (ids != null) return _favorites = new HashSet<int>(ids);
                    }
                }
                catch
                {
                    // Corrupt/unreadable file - start with an empty favorites
                    // set rather than failing anything that just wants to
                    // know whether a card is starred.
                }

                return _favorites = new HashSet<int>();
            }
        }

        /// <summary>Raised after a favorite is added/removed and the file
        /// write completes (or fails) - CardSearchView/DeckEditorView's
        /// "Favorites only" checkbox re-filters off this instead of every
        /// individual CardTileViewModel.IsFavorite change needing its own
        /// wiring back to the results list.</summary>
        public static event Action? Changed;

        public static bool IsFavorite(int cardId) => Favorites.Contains(cardId);

        public static void SetFavorite(int cardId, bool value)
        {
            bool changed = value ? Favorites.Add(cardId) : Favorites.Remove(cardId);
            if (!changed) return;

            try
            {
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(path, JsonSerializer.Serialize(Favorites.ToList()));
            }
            catch
            {
                // Not worth failing a favorite toggle over a write error -
                // the in-memory set still has the right value for this
                // session either way.
            }

            Changed?.Invoke();
        }
    }
}
