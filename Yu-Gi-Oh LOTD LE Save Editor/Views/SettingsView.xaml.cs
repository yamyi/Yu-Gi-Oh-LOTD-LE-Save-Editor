using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using YuGiOhSaveEditor.Controls;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Views;

/// <summary>
/// App-wide preferences page. Everything on screen (OfflineModeCheckBox,
/// EnableCacheCheckBox, CacheSizeText, OpenCacheFolderButton,
/// ClearCacheButton, CacheLimitBox, CacheLimitApplyButton, WarmCacheButton,
/// CancelWarmCacheButton, WarmCacheStatusText, ThemeCombo, RestartNowButton,
/// ThemeRestartHintText, VersionText, ViewOnGitHubButton,
/// ViewDocumentationButton) is declared once in SettingsView.xaml;
/// this class only ever reads/writes AppContext.State.IsOfflineMode/
/// DiskCacheEnabled/CacheSizeLimitMB/ThemeName, calls CardImageProvider's
/// cache helpers, opens the cache folder via UrlLauncher, and sets
/// TextBlock/CheckBox/ComboBox/Button properties - no controls are built
/// here. Each preference-changing handler also calls AppSettings.Save() so
/// the change survives an app restart (AppSettings.Load() restores it in
/// App.xaml.cs at startup).
/// </summary>
public partial class SettingsView : UserControl
{
    private bool _suppressEvents;

    /// <summary>Non-null only while a Warm Cache run is in progress -
    /// CancelWarmCacheButton_Click cancels it, RunWarmCacheAsync's finally
    /// block clears it back to null.</summary>
    private CancellationTokenSource? _warmCacheCts;

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

    // Display name <-> AppState.ThemeName - ThemeCombo shows the former,
    // everything persisted/compared (AppState, AppSettings.json,
    // App.ApplyTheme) uses the latter.
    private static readonly (string Display, string ThemeName)[] ThemeOptions =
    {
        ("Master Duel", "MasterDuel"),
        ("Duel Links", "DuelLinks"),
    };

    private void RefreshFromState()
    {
        _suppressEvents = true;
        OfflineModeCheckBox.IsChecked = AppContext.State.IsOfflineMode;
        EnableCacheCheckBox.IsChecked = AppContext.State.DiskCacheEnabled;
        CacheLimitBox.Text = AppContext.State.CacheSizeLimitMB.ToString();

        if (ThemeCombo.ItemsSource == null) ThemeCombo.ItemsSource = ThemeOptions.Select(t => t.Display).ToList();
        int themeIndex = Array.FindIndex(ThemeOptions, t => t.ThemeName == AppContext.State.ThemeName);
        ThemeCombo.SelectedIndex = themeIndex >= 0 ? themeIndex : 0;
        _suppressEvents = false;

        CacheLimitBox.IsEnabled = AppContext.State.DiskCacheEnabled;
        CacheLimitApplyButton.IsEnabled = AppContext.State.DiskCacheEnabled;
        RefreshWarmCacheButtonEnabled();

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

    /// <summary>Persists the new theme choice immediately (same
    /// "no separate apply step" pattern as the other preferences here), but
    /// - unlike them - can't take effect on the running window (see
    /// App.OnStartup's comment on why StaticResource brushes can't be
    /// swapped live). Reveals RESTART NOW plus an explanatory line instead;
    /// picking the theme that's already active just hides them again.</summary>
    private void ThemeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_suppressEvents) return;
        if (ThemeCombo.SelectedIndex < 0) return;

        string newTheme = ThemeOptions[ThemeCombo.SelectedIndex].ThemeName;
        bool changed = newTheme != AppContext.State.ThemeName;

        AppContext.State.ThemeName = newTheme;
        AppSettings.Save();

        RestartNowButton.Visibility = changed ? Visibility.Visible : Visibility.Collapsed;
        ThemeRestartHintText.Visibility = changed ? Visibility.Visible : Visibility.Collapsed;
        ThemeRestartHintText.Text = changed
            ? "Saved. Restart the app for the new look to take effect."
            : string.Empty;
    }

    /// <summary>Relaunches the app from its own executable, then shuts this
    /// instance down - the same "soft restart" idea the WinForms build's
    /// Program.cs used for a theme change, ported here since WPF has no way
    /// to re-resolve already-parsed StaticResource brushes on a running
    /// window. Doesn't try to reopen whatever save file was loaded - that's
    /// exactly the same "start fresh" experience the app already gives on
    /// every normal launch.</summary>
    private void RestartNowButton_Click(object sender, RoutedEventArgs e)
    {
        string? exePath = Environment.ProcessPath;
        if (!string.IsNullOrEmpty(exePath))
            System.Diagnostics.Process.Start(exePath);

        Application.Current.Shutdown();
    }

    private void OfflineModeCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (_suppressEvents) return;
        AppContext.State.IsOfflineMode = OfflineModeCheckBox.IsChecked == true;
        RefreshWarmCacheButtonEnabled();
        AppSettings.Save();
    }

    /// <summary>Toggles CardImageProvider's disk cache off (every request
    /// re-contacts the network instead) or back on - see
    /// CardImageProvider's class doc comment for exactly what changes.
    /// The size-limit box/button and Warm Cache are meaningless without a
    /// disk cache to limit or warm, so they follow this checkbox too.</summary>
    private void EnableCacheCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (_suppressEvents) return;
        AppContext.State.DiskCacheEnabled = EnableCacheCheckBox.IsChecked == true;

        CacheLimitBox.IsEnabled = AppContext.State.DiskCacheEnabled;
        CacheLimitApplyButton.IsEnabled = AppContext.State.DiskCacheEnabled;
        RefreshWarmCacheButtonEnabled();
        AppSettings.Save();
    }

    private void RefreshWarmCacheButtonEnabled()
    {
        if (_warmCacheCts != null) return; // a run is in progress - WarmCacheButton is Collapsed, leave it alone
        WarmCacheButton.IsEnabled = AppContext.State.DiskCacheEnabled && !AppContext.State.IsOfflineMode;
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

    private const string GitHubUrl = "https://github.com/yamyi/Yu-Gi-Oh-LOTD-LE-Save-Editor";
    private const string DocumentationUrl = GitHubUrl + "/tree/master/Documentation";

    private void ViewOnGitHubButton_Click(object sender, RoutedEventArgs e)
    {
        if (!UrlLauncher.TryOpen(GitHubUrl))
        {
            AppMessageBox.Show(Window.GetWindow(this), "Couldn't open that link in your browser.",
                "View on GitHub", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ViewDocumentationButton_Click(object sender, RoutedEventArgs e)
    {
        if (!UrlLauncher.TryOpen(DocumentationUrl))
        {
            AppMessageBox.Show(Window.GetWindow(this), "Couldn't open that link in your browser.",
                "Documentation", MessageBoxButton.OK, MessageBoxImage.Error);
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

    /// <summary>Applies a new AppState.CacheSizeLimitMB (0/blank/invalid = unlimited)
    /// and immediately runs an eviction pass against it
    /// (CardImageProvider.EnforceCacheLimitNow), rather than waiting for the
    /// next download to notice the cache is now over the new limit - so
    /// shrinking the limit below the cache's current size takes effect right
    /// away instead of looking like nothing happened.</summary>
    private void CacheLimitApplyButton_Click(object sender, RoutedEventArgs e)
    {
        if (!int.TryParse(CacheLimitBox.Text, out int mb) || mb < 0) mb = 0;
        CacheLimitBox.Text = mb.ToString();

        AppContext.State.CacheSizeLimitMB = mb;
        CardImageProvider.EnforceCacheLimitNow();
        RefreshCacheSize();
        AppSettings.Save();

        // Same transient-relabel feedback ClearCacheButton uses.
        string original = (string)CacheLimitApplyButton.Content;
        CacheLimitApplyButton.Content = "APPLIED!";
        CacheLimitApplyButton.IsEnabled = false;

        var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1.2) };
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            CacheLimitApplyButton.Content = original;
            CacheLimitApplyButton.IsEnabled = AppContext.State.DiskCacheEnabled;
        };
        timer.Start();
    }

    // Rough estimates from a real run, not computed live (sizing every card
    // ahead of time would mean its own round of network requests). The
    // thumbnails-only figure is what the user actually asked to warn about;
    // the full-size figure is the combined total when IncludeFullSizeCheckBox
    // is also checked (see CardImageProvider.WarmCacheAsync's doc comment for
    // why thumbnails-only is the default).
    private const int ThumbnailWarmEstimateMB = 150;
    private const int FullWarmEstimateMB = 1761;

    /// <summary>Confirms via AppMessageBox first, quoting whichever estimate
    /// matches IncludeFullSizeCheckBox's current state, then hands off to
    /// StartWarmCache if the user says yes. Declining leaves everything
    /// untouched.</summary>
    private void WarmCacheButton_Click(object sender, RoutedEventArgs e)
    {
        if (_warmCacheCts != null) return; // already running - shouldn't be reachable while WarmCacheButton is Collapsed

        bool includeFullSize = IncludeFullSizeCheckBox.IsChecked == true;
        int estimateMB = includeFullSize ? FullWarmEstimateMB : ThumbnailWarmEstimateMB;
        string what = includeFullSize ? "thumbnail and full-size preview art" : "thumbnail art";

        var result = AppMessageBox.Show(Window.GetWindow(this),
            $"Downloading {what} for every card in the database uses roughly {estimateMB} MB of network data and disk space (less if some of it's already cached). Continue?",
            "Warm Cache", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result != MessageBoxResult.Yes) return;

        StartWarmCache(includeFullSize);
    }

    private void StartWarmCache(bool includeFullSize)
    {
        _warmCacheCts = new CancellationTokenSource();
        WarmCacheButton.Visibility = Visibility.Collapsed;
        CancelWarmCacheButton.Visibility = Visibility.Visible;
        IncludeFullSizeCheckBox.IsEnabled = false;
        WarmCacheStatusText.Text = "Starting...";

        var progress = new Progress<(int done, int total)>(p =>
            WarmCacheStatusText.Text = $"Downloading art... {p.done} / {p.total}");

        _ = RunWarmCacheAsync(includeFullSize, progress, _warmCacheCts.Token);
    }

    private void CancelWarmCacheButton_Click(object sender, RoutedEventArgs e) => _warmCacheCts?.Cancel();

    private async Task RunWarmCacheAsync(bool includeFullSize, IProgress<(int done, int total)> progress, CancellationToken ct)
    {
        try
        {
            await CardImageProvider.WarmCacheAsync(AppContext.CardDb.AllCards, includeFullSize, progress, ct);
            WarmCacheStatusText.Text = includeFullSize
                ? "Cache warmed - every card's art is now available offline."
                : "Cache warmed - every card's thumbnail is now available offline.";
        }
        catch (OperationCanceledException)
        {
            WarmCacheStatusText.Text = "Cache warming cancelled.";
        }
        finally
        {
            WarmCacheButton.Visibility = Visibility.Visible;
            CancelWarmCacheButton.Visibility = Visibility.Collapsed;
            IncludeFullSizeCheckBox.IsEnabled = true;
            _warmCacheCts = null;
            RefreshWarmCacheButtonEnabled();
            RefreshCacheSize();
        }
    }
}
