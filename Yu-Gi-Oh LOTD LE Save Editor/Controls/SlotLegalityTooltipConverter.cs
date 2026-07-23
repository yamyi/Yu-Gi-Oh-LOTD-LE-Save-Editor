using System.Globalization;
using System.Windows.Data;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// Companion to SlotLegalityBrushConverter - same DeckLegalityChecker call,
/// but returns the human-readable reason string for a tile's ToolTip
/// instead of a brush key, so hovering the legality dot explains what
/// (if anything) is actually wrong with that slot's deck.
/// </summary>
public sealed class SlotLegalityTooltipConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is SlotIO.SlotInfo slot ? DeckLegalityChecker.GetReason(slot) : string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
