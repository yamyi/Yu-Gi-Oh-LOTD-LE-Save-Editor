using System.Runtime.InteropServices;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        ThemeManager.Load();

        // WinForms controls only read their colors once, at construction —
        // there's no built-in live re-skin. Rather than leave stale colors
        // on screen (or chase down every control by hand), a theme change
        // sets ThemeManager.RestartRequested and closes MainForm; this loop
        // just builds a fresh one, which picks up the new palette naturally
        // since every control reads AppColors.X (-> ThemeManager.Current)
        // at construction. AppContext's static services keep the loaded
        // save/card database in memory across the rebuild, so this is a
        // near-instant flicker, not a real app restart.
        do
        {
            ThemeManager.RestartRequested = false;
            using var form = new MainForm();
            Application.Run(form);
        }
        while (ThemeManager.RestartRequested);
    }

    /// <summary>
    /// Call this inside a Form's Load or HandleCreated event to get a dark
    /// title bar on Windows 11. Safe no-op on older Windows versions.
    /// Usage: Program.TryEnableDarkTitleBar(this);
    /// </summary>
    public static void TryEnableDarkTitleBar(Form form) =>
        TrySetImmersiveDarkMode(form, ThemeManager.Current.IsDark);

    /// <summary>Same as TryEnableDarkTitleBar, but lets the caller force a
    /// specific state instead of following the active theme.</summary>
    public static void TrySetImmersiveDarkMode(Form form, bool dark)
    {
        if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 18985)) return;
        try
        {
            int value = dark ? 1 : 0;
            DwmSetWindowAttribute(form.Handle, 20 /* DWMWA_USE_IMMERSIVE_DARK_MODE */,
                ref value, sizeof(int));
        }
        catch { /* silently ignore on unsupported builds */ }
    }

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(
        IntPtr hwnd, int attr, ref int attrValue, int attrSize);
}