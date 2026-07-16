using System.Windows;
using YuGiOhDeckManager.Wpf.Services;

namespace YuGiOhDeckManager.Wpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Load the embedded card database once at startup — same idempotent
        // Load() call the WinForms app made from MainForm's constructor.
        AppContext.CardDb.Load();
    }
}
