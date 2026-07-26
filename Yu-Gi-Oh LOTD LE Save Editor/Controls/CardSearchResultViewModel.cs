namespace YuGiOhSaveEditor.Controls;

using YuGiOhSaveEditor.Services;

/// <summary>
/// One row in the Save Editor's Cards tab "unlock a specific card" search
/// results - a real card database entry (Card) paired with its current
/// owned count read from the loaded save. LotdId doubles as the byte index
/// CardCollectionLayout.GetCount/SetCount take - the game's own save-slot
/// index. Only cards with a LotdId are ever searchable here - see
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
