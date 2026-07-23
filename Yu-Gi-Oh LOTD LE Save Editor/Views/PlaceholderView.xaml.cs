using System.Windows;
using System.Windows.Controls;

namespace YuGiOhSaveEditor.Views;

/// <summary>
/// Stand-in for a page that hasn't been rebuilt in WPF yet. Every instance is
/// declared directly in MainWindow.xaml (PageTitle set as a XAML attribute,
/// not passed to a constructor) — no runtime control creation anywhere in
/// this app, WinForms build included this rule and it still applies here.
/// MainWindow toggles Visibility/Opacity on the named instances so the shell
/// is fully functional before each real view (Deck Editor, Deck Slots, etc.)
/// replaces its placeholder.
/// </summary>
public partial class PlaceholderView : UserControl
{
    public static readonly DependencyProperty PageTitleProperty =
        DependencyProperty.Register(nameof(PageTitle), typeof(string), typeof(PlaceholderView),
            new PropertyMetadata("Page"));

    public string PageTitle
    {
        get => (string)GetValue(PageTitleProperty);
        set => SetValue(PageTitleProperty, value);
    }

    public PlaceholderView()
    {
        InitializeComponent();
    }
}
