namespace YuGiOhSaveEditor.Wpf.Controls;

/// <summary>
/// One row in a checklist (Save Editor's Shop Packs / Tutorials / Avatars
/// panels) - a label, whether it's currently checked, its index within its
/// own underlying list (ShopPackLabels position, tutorial number - 1, or
/// avatar id), and which of the three lists it belongs to, so one shared
/// DataTemplate and one shared Checked/Unchecked handler in
/// SaveEditorView.xaml.cs can serve all three without duplicating XAML.
/// </summary>
public sealed class CheckableItem
{
    public string Label { get; }
    public int Index { get; }
    public string Kind { get; } // "ShopPack" / "Tutorial" / "Avatar"
    public bool IsChecked { get; }

    public CheckableItem(string label, int index, string kind, bool isChecked)
    {
        Label = label;
        Index = index;
        Kind = kind;
        IsChecked = isChecked;
    }
}
