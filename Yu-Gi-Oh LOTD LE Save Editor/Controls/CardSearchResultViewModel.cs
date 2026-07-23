namespace YuGiOhSaveEditor.Controls;

using YuGiOhSaveEditor.Services;

/// <summary>
/// One row in the Save Editor's Cards tab "unlock a specific card" search
/// results - a real card database entry (Card) paired with its current
/// owned count read from the loaded save. LotdId doubles as the byte index
/// CardCollectionLayout.GetCount/SetCount take - confirmed 2026-07-23 to be
/// the game's own save-slot index (a real fully-owned save's owned indices
/// exactly match Cards.json's lotd_id set - all 10027, after the one gap,
/// lotd_id 6053 = "7", was found and added the same day).
/// Only cards with a LotdId are ever searchable here - see
/// SaveEditorView.xaml.cs's RefreshCardSearchResults, which filters
/// AppContext.CardDb.Search to LotdId.HasValue before building these.
/// </summary>
public sealed class CardSearchResultViewModel
{
    public int LotdId { get; }
    public string Name { get; }
    public string Subtitle { get; }
    public int OwnedCount { get; }
    public string OwnedText => $"Owned: {OwnedCount}/3";

    public CardSearchResultViewModel(Card card, int ownedCount)
    {
        LotdId = card.LotdId!.Value; // caller guarantees HasValue - see class doc comment
        Name = card.Name;
        Subtitle = string.IsNullOrWhiteSpace(card.HumanReadableCardType) ? card.Type : card.HumanReadableCardType;
        OwnedCount = ownedCount;
    }
}
