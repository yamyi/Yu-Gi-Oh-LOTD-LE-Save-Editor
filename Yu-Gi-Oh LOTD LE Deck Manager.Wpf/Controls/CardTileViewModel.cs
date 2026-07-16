using System.ComponentModel;
using System.Windows.Media.Imaging;
using YuGiOhDeckManager.Wpf.Services;

namespace YuGiOhDeckManager.Wpf.Controls;

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

    public CardTileViewModel(Card card)
    {
        Card = card;
        RarityTier = DeckEditorHelpers.GetRarityTier(card);
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
