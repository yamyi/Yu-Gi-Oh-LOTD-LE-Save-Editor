using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// null -&gt; Visible, non-null -&gt; Collapsed. Used on card tiles' fallback
/// name/type TextBlock (CardSearchView's ResultTileTemplate,
/// DeckEditorView's CardListTileTemplate/DeckCardTileTemplate) so it only
/// shows up in place of the card art when Image is null - i.e. a card with
/// no cached art while in Offline Mode (see CardImageProvider - a cache hit
/// still loads even offline, only a genuine miss skips the network) or a
/// download/decode that failed outright. Without this, a tile with no image
/// had nothing on it identifying which card it was but a bare rarity/ban
/// badge.
/// </summary>
public sealed class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is null ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
