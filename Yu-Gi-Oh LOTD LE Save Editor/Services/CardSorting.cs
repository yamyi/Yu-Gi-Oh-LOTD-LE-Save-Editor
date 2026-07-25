using System.Collections.Generic;
using System.Linq;

namespace YuGiOhSaveEditor.Services
{
    /// <summary>
    /// Sort orders for the Card Search / Deck Editor results grids' SortCombo
    /// + SortDirectionToggle - applied after CardDatabase.Filter, right
    /// before the ResultCap/200-card Take(). A plain extension method (not
    /// baked into CardDatabase.Filter itself) since sorting is a display
    /// concern over an already-filtered sequence, not another filter
    /// predicate.
    /// </summary>
    public static class CardSorting
    {
        /// <summary>Every option SortCombo offers, in display order - both
        /// CardSearchView and DeckEditorView populate their SortCombo from
        /// this exact list so the two stay in sync with no duplicated
        /// string literals. No "(A-Z)"/"(High-Low)" suffixes anymore - since
        /// direction is now its own SortDirectionToggle, baking a direction
        /// into the label would go stale the moment someone flips it.</summary>
        public static readonly string[] Options =
        {
            "Name",
            "Level / Rank / Link",
            "ATK",
            "DEF",
            "Passcode",
        };

        /// <summary>The sensible starting direction for a given sort key -
        /// alphabetical keys start ascending, numeric stat keys start
        /// descending (highest first, since that's usually what you're
        /// hunting for). SortCombo_SelectionChanged applies this every time
        /// the key itself changes; SortDirectionToggle's own Click handler
        /// is the only thing that can override it after that.</summary>
        public static bool DefaultAscending(string? sortKey) => sortKey is "Name" or "Passcode" or null;

        /// <summary>Applies whichever of Options was selected, in the given
        /// direction - unrecognized or null falls back to Name, same as
        /// SortCombo's default selection, so a blank/invalid value never
        /// leaves results in CardDatabase's raw (arbitrary) order.</summary>
        public static IEnumerable<Card> Sort(this IEnumerable<Card> cards, string? sortKey, bool ascending) => sortKey switch
        {
            "Level / Rank / Link" => ascending
                ? cards.OrderBy(c => c.IsLink ? c.LinkVal : c.Level)
                : cards.OrderByDescending(c => c.IsLink ? c.LinkVal : c.Level),
            "ATK" => ascending ? cards.OrderBy(c => c.Atk) : cards.OrderByDescending(c => c.Atk),
            "DEF" => ascending ? cards.OrderBy(c => c.Def) : cards.OrderByDescending(c => c.Def),
            "Passcode" => ascending ? cards.OrderBy(c => c.Id) : cards.OrderByDescending(c => c.Id),
            _ => ascending ? cards.OrderBy(c => c.Name) : cards.OrderByDescending(c => c.Name),
        };
    }
}
