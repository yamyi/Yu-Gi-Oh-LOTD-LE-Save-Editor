using System.ComponentModel;
using System.Windows.Media.Imaging;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// Wraps a single Card for display as one grid tile (Card List panel or a
/// Main/Extra/Side deck slot). One instance = one physical copy - the same
/// Card can appear as up to three separate CardTileViewModel instances
/// across Main+Extra+Side combined (see DeckEditorView.CountInDeck for the
/// 3-copy cap), same as the WinForms build's one-slot-per-instance model.
/// Implements INotifyPropertyChanged only for Image, the only property that
/// changes after construction - it's loaded asynchronously so scrolling
/// through hundreds of cards doesn't block on network/disk I/O per tile.
/// </summary>
public sealed class CardTileViewModel : INotifyPropertyChanged
{
    public Card Card { get; }
    public string RarityTier { get; }

    /// <summary>"Forbidden"/"Limited"/"Semi-Limited" per LotdBanlist, or null
    /// if this card isn't restricted - drives the small ban-status corner
    /// icon on the tile (see BanIconConverter), same idea as RarityTier's
    /// corner badge.</summary>
    public string? BanStatus { get; }

    private BitmapImage? _image;
    public BitmapImage? Image
    {
        get => _image;
        private set
        {
            _image = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
        }
    }

    /// <summary>Backed by FavoritesStore (a small JSON file, not anything
    /// per-save) rather than a plain field - the star toggle on a tile is a
    /// two-way binding straight to this property (see FavoriteToggleStyle),
    /// so setting it here is the one and only place a favorite actually
    /// gets persisted.</summary>
    private bool _isFavorite;
    public bool IsFavorite
    {
        get => _isFavorite;
        set
        {
            if (_isFavorite == value) return;
            _isFavorite = value;
            FavoritesStore.SetFavorite(Card.Id, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFavorite)));
        }
    }

    public CardTileViewModel(Card card)
    {
        Card = card;
        RarityTier = DeckEditorHelpers.GetRarityTier(card);
        BanStatus = LotdBanlist.GetStatus(card.LotdId);
        _isFavorite = FavoritesStore.IsFavorite(card.Id);
    }

    public async Task LoadImageAsync(bool small)
    {
        try
        {
            Image = await CardImageProvider.GetImageAsync(Card, small);
        }
        catch
        {
            // Offline/network hiccup - tile just keeps its placeholder look.
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
