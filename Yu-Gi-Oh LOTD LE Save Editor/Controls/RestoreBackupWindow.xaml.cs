using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// The actual dialog behind RestoreBackupPicker.Show - everything on screen
/// (BackupsList, RestoreButton, CancelButton, NoBackupsPlaceholder) is
/// declared once in RestoreBackupWindow.xaml; this class only ever populates
/// BackupsList's ItemsSource (a plain List&lt;BackupListItem&gt;, the same
/// "data objects into ItemsSource" category as every other list in this app)
/// and reads back whichever row got selected - no controls are built here.
/// </summary>
public partial class RestoreBackupWindow : Window
{
    /// <summary>Set once the dialog closes via RESTORE or a double-click;
    /// stays null if the user cancels or hits Escape.</summary>
    public string? SelectedBackupPath { get; private set; }

    public RestoreBackupWindow(string savePath)
    {
        InitializeComponent();

        var items = SaveBackup.FindAll(savePath)
            .Select(p => new BackupListItem(
                p,
                SaveBackup.ParseTimestamp(p, savePath) is DateTime dt
                    ? dt.ToString("MMM d, yyyy h:mm:ss tt")
                    : Path.GetFileName(p),
                FormatBytes(TryGetLength(p))))
            .ToList();

        BackupsList.ItemsSource = items;
        NoBackupsPlaceholder.Visibility = items.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        BackupsList.Visibility = items.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
    }

    private static long TryGetLength(string path)
    {
        try { return new FileInfo(path).Length; }
        catch { return 0; }
    }

    private static string FormatBytes(long bytes)
    {
        const double kb = 1024;
        const double mb = kb * 1024;

        if (bytes >= mb) return $"{bytes / mb:0.0} MB";
        if (bytes >= kb) return $"{bytes / kb:0.0} KB";
        return $"{bytes} B";
    }

    private void BackupsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        => RestoreButton.IsEnabled = BackupsList.SelectedItem != null;

    private void BackupsList_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (BackupsList.SelectedItem != null) Confirm();
    }

    private void RestoreButton_Click(object sender, RoutedEventArgs e) => Confirm();

    private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

    private void Confirm()
    {
        if (BackupsList.SelectedItem is BackupListItem item)
            SelectedBackupPath = item.FullPath;
        Close();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => BackupsList.Focus();

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Close();
        }
        else if (e.Key == Key.Enter && BackupsList.SelectedItem != null)
        {
            Confirm();
        }
    }

    private sealed record BackupListItem(string FullPath, string DisplayTime, string DisplaySize);
}
