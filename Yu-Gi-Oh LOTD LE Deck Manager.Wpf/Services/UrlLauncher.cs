using System.Diagnostics;

namespace YuGiOhDeckManager.Wpf.Services;

/// <summary>
/// Opens a URL in the user's default browser - .NET (Core/5+) needs
/// UseShellExecute explicitly set to true here, unlike .NET Framework where
/// Process.Start(url) worked directly. Used by both preview panels'
/// "VIEW ON YGOPRODECK" buttons.
/// </summary>
public static class UrlLauncher
{
    public static bool TryOpen(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;

        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            return true;
        }
        catch
        {
            return false;
        }
    }
}
