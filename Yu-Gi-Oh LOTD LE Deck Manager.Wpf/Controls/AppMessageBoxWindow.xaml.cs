using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace YuGiOhDeckManager.Wpf.Controls;

/// <summary>
/// The actual dialog behind AppMessageBox.Show - everything on screen
/// (Card, IconBadge, TitleText, MessageText, CopyButton, Button1/2/3) is
/// declared once in AppMessageBoxWindow.xaml; this class only ever sets
/// properties on those already-existing elements (text, style, visibility,
/// tag) to configure one of a handful of icon/button combinations, the same
/// pattern every other view in this app follows - no controls are built here.
/// </summary>
public partial class AppMessageBoxWindow : Window
{
    public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

    private MessageBoxResult _escapeResult = MessageBoxResult.None;
    private MessageBoxResult _enterResult = MessageBoxResult.OK;
    private string? _copyText;

    public AppMessageBoxWindow(string message, string title, MessageBoxButton button, MessageBoxImage icon,
        string? copyText = null)
    {
        InitializeComponent();
        TitleText.Text = title;
        MessageText.Text = message;
        ConfigureIcon(icon);
        ConfigureButtons(button);

        _copyText = copyText;
        CopyButton.Visibility = copyText != null ? Visibility.Visible : Visibility.Collapsed;
    }

    private void ConfigureIcon(MessageBoxImage icon)
    {
        switch (icon)
        {
            case MessageBoxImage.Error:
                IconBadge.Background = (Brush)FindResource("StatusIllegalBrush");
                IconGlyph.Text = "!";
                break;
            case MessageBoxImage.Warning:
                IconBadge.Background = (Brush)FindResource("StatusWarningBrush");
                IconGlyph.Text = "!";
                break;
            case MessageBoxImage.Question:
                IconBadge.Background = (Brush)FindResource("AccentBrush");
                IconGlyph.Text = "?";
                break;
            case MessageBoxImage.Information:
                IconBadge.Background = (Brush)FindResource("AccentBrush");
                IconGlyph.Text = "i";
                break;
            default:
                IconBadge.Visibility = Visibility.Collapsed;
                break;
        }
    }

    /// <summary>Button1/2/3 are pre-declared in the XAML (never created here)
    /// - this just decides how many are visible, what they say, which
    /// slanted style (safe/green vs destructive/red) they use, and which
    /// MessageBoxResult each one closes the dialog with (stashed in Tag).
    /// The one real-world YesNo dialog in this app is an overwrite confirm,
    /// so Yes is always styled as the destructive choice and No as the safe
    /// one - matches the only case that exists without needing an extra
    /// parameter on every call site.</summary>
    private void ConfigureButtons(MessageBoxButton button)
    {
        var safe = (Style)FindResource("SlantedButtonStyle");
        var danger = (Style)FindResource("SlantedDangerButtonStyle");

        switch (button)
        {
            case MessageBoxButton.OKCancel:
                Setup(Button1, "OK", safe, MessageBoxResult.OK);
                Setup(Button2, "CANCEL", safe, MessageBoxResult.Cancel);
                Button3.Visibility = Visibility.Collapsed;
                _enterResult = MessageBoxResult.OK;
                _escapeResult = MessageBoxResult.Cancel;
                break;

            case MessageBoxButton.YesNo:
                Setup(Button1, "YES", danger, MessageBoxResult.Yes);
                Setup(Button2, "NO", safe, MessageBoxResult.No);
                Button3.Visibility = Visibility.Collapsed;
                _enterResult = MessageBoxResult.No;
                _escapeResult = MessageBoxResult.No;
                break;

            case MessageBoxButton.YesNoCancel:
                Setup(Button1, "YES", danger, MessageBoxResult.Yes);
                Setup(Button2, "NO", safe, MessageBoxResult.No);
                Setup(Button3, "CANCEL", safe, MessageBoxResult.Cancel);
                _enterResult = MessageBoxResult.Cancel;
                _escapeResult = MessageBoxResult.Cancel;
                break;

            default: // OK
                Setup(Button1, "OK", safe, MessageBoxResult.OK);
                Button2.Visibility = Visibility.Collapsed;
                Button3.Visibility = Visibility.Collapsed;
                _enterResult = MessageBoxResult.OK;
                _escapeResult = MessageBoxResult.OK;
                break;
        }
    }

    private static void Setup(Button b, string text, Style style, MessageBoxResult result)
    {
        b.Content = text;
        b.Style = style;
        b.Tag = result;
        b.Visibility = Visibility.Visible;
    }

    private void ActionButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button { Tag: MessageBoxResult result })
            Result = result;
        Close();
    }

    /// <summary>Copies the text AppMessageBox.Show was given (e.g. the full
    /// list of a .ydk import's skipped cards) to the clipboard and doesn't
    /// close the dialog - the button just relabels itself "COPIED!" for a
    /// moment as feedback, then reverts, same "touch an already-declared
    /// element's property" approach as everything else here.</summary>
    private void CopyButton_Click(object sender, RoutedEventArgs e)
    {
        if (_copyText == null) return;

        Clipboard.SetText(_copyText);

        CopyButton.Content = "COPIED!";
        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.2) };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            CopyButton.Content = "COPY TO CLIPBOARD";
        };
        timer.Start();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => Button1.Focus();

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Result = _escapeResult;
            Close();
        }
        else if (e.Key == Key.Enter)
        {
            Result = _enterResult;
            Close();
        }
    }
}
