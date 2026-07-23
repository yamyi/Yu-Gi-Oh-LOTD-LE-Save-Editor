namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// One row in Save Editor's Stats tab - a known stat's display label, its
/// StatsLayout slot index, and its current value formatted as a string for
/// the row's TextBox. Same plain-data-object-bound-via-ItemsSource pattern as
/// DuelRowViewModel/CheckableItem: SaveEditorView.xaml.cs reads Index back off
/// this (via the editing TextBox's DataContext) whenever a row loses focus,
/// so no runtime control creation is needed - StatsList's ItemsControl just
/// stamps out StatRowTemplate once per entry in StatItems.
/// </summary>
public sealed class StatRowViewModel
{
    public string Label { get; }
    public int Index { get; }
    public string Value { get; }

    public StatRowViewModel(string label, int index, long value)
    {
        Label = label;
        Index = index;
        Value = value.ToString();
    }
}
