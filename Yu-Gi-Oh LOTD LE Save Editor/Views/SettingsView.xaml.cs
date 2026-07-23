using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using YuGiOhSaveEditor.Controls;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Views;

/// <summary>
/// App-wide preferences page. Everything on screen (OfflineModeCheckBox,
/// CacheSizeText, OpenCacheFolderButton, ClearCacheButton, VersionText) is
/// declared once in SettingsView.xaml; this class only ever reads/writes
/// AppContext.State.IsOfflineMode, calls CardImageProvider's cache helpers,
/// opens the cache folder via UrlLauncher, and sets TextBlock/CheckBox
/// properties - no controls are built here.
/// </summary>
public partial class SettingsView : UserControl
{
    private bool _suppressEvents;

    public SettingsView()
    {
        InitializeComponent();

        VersionText.Text = $"Version {GetVersionString()}";

        // Same "(re)populate on becoming visible" pattern CardSearchView
        // uses - the cache on disk can change while this page isn't showing
        // (e.g. artwork downloaded from Deck Editor/Card Search), so its
        // size is refreshed every time the user navigates back here rather
        // than only once at startup.
        IsVisibleChanged += (_, e) =>
        {
            if (e.NewValue is true) RefreshFromState();
        };
    }

    private void RefreshFromState()
    {
        _suppressEvents = true;
        OfflineModeCheckBox.IsChecked = AppContext.State.IsOfflineMode;
        _suppressEvents = false;

        RefreshCacheSize();
    }

    private void RefreshCacheSize()
    {
        long bytes = CardImageProvider.GetCacheSizeBytes();
        CacheSizeText.Text = $"Cache size: {FormatBytes(bytes)}";
    }

    private static string FormatBytes(long bytes)
    {
        const double kb = 1024;
        const double mb = kb * 1024;

        if (bytes >= mb) return $"{bytes / mb:0.0} MB";
        if (bytes >= kb) return $"{bytes / kb:0.0} KB";
        return $"{bytes} B";
    }

    private static string GetVersionString()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        return version == null ? "-" : version.ToString(3);
    }

    private void OfflineModeCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (_suppressEvents) return;
        AppContext.State.IsOfflineMode = OfflineModeCheckBox.IsChecked == true;
    }

    /// <summary>Opens the on-disk card image cache folder in Explorer -
    /// CardImageProvider.GetCacheDirectory() creates it first if it doesn't
    /// exist yet (e.g. offline mode has been on since first launch), so this
    /// always has somewhere real to open. Same UrlLauncher.TryOpen helper
    /// CardSearchView/DeckEditorView use for "VIEW ON YGOPRODECK" - it's just
    /// a Process.Start(UseShellExecute: true) wrapper, which opens a folder
    /// path in Explorer exactly the same way it opens a URL in a browser.</summary>
    private void OpenCacheFolderButton_Click(object sender, RoutedEventArgs e)
    {
        string dir = CardImageProvider.GetCacheDirectory();
        if (!UrlLauncher.TryOpen(dir))
        {
            AppMessageBox.Show(Window.GetWindow(this), "Couldn't open the cache folder.",
                "Open Cache Folder", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ClearCacheButton_Click(object sender, RoutedEventArgs e)
    {
        CardImageProvider.ClearCache();
        RefreshCacheSize();

        // Same transient-relabel feedback AppMessageBoxWindow's CopyButton
        // uses - no new control, just a temporary Content swap reverted by
        // a one-shot DispatcherTimer.
        string original = (string)ClearCacheButton.Content;
        ClearCacheButton.Content = "CLEARED!";
        ClearCacheButton.IsEnabled = false;

        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.2) };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            ClearCacheButton.Content = original;
            ClearCacheButton.IsEnabled = true;
        };
        timer.Start();
    }
}
