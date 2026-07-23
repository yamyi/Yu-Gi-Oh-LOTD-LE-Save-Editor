using System.Windows;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Restore the user's saved Offline Mode/disk cache/cache size
        // preferences before anything else reads AppState, so they don't
        // silently reset to defaults every launch (AppSettings.cs).
        AppSettings.Load();

        // Load the embedded card database once at startup — same idempotent
        // Load() call the WinForms app made from MainForm's constructor.
        AppContext.CardDb.Load();
    }
}
