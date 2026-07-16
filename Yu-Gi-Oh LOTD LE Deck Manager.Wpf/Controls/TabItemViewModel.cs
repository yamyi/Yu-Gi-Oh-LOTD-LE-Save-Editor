namespace YuGiOhDeckManager.Wpf.Controls;

/// <summary>
/// One row in the side navigation list. Declared entirely in MainWindow.xaml
/// as an &lt;x:Array&gt; of these (settable properties + parameterless
/// constructor so XAML can build them directly) bound to the nav ListBox's
/// ItemsSource — no runtime construction in code-behind. SideNavListStyle in
/// Themes/Controls.xaml turns each one into a row matching Master Duel's
/// DUEL/DECK/SOLO/SHOP main menu (a diagonal-cut lime highlight bar + chevron
/// on the active row, with its Description shown alongside it — everything
/// else is a thin idle accent pipe and plain label).
/// </summary>
public sealed class TabItemViewModel
{
    public string Text { get; set; } = string.Empty;

    /// <summary>Shown to the right of the row only while it's selected —
    /// e.g. "Create powerful Decks and Cards." in the reference screenshot.</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Segoe MDL2 Assets glyph shown at a fixed early x-position in
    /// each row — the only part of a row still visible when SideNav is
    /// collapsed to its slim icon rail (MainWindow's NavToggleButton), since
    /// everything to its right just gets clipped off by the sidebar Border's
    /// ClipToBounds as it shrinks.</summary>
    public string IconGlyph { get; set; } = string.Empty;
}
