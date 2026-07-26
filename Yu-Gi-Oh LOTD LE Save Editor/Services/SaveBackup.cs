using System.Globalization;
using System.IO;

namespace YuGiOhSaveEditor.Services;

/// <summary>
/// Creates and enumerates the timestamped sibling backups MainWindow makes
/// of a save file. MainWindow.OnSaveFile calls Create right before writing
/// the in-memory (edited) bytes back to disk, so it always backs up the
/// exact on-disk state that write is about to overwrite - not on Open Save.
/// RestoreBackupWindow lists the exact same backups this class creates,
/// using the same naming convention and directory - a save at
/// "C:\saves\savegame.dat" gets backups named
/// "savegame.dat.bak_yyyyMMdd_HHmmss" (or "..._2", "..._3", ... if more than
/// one backup happens to land in the same second) right next to it.
/// </summary>
public static class SaveBackup
{
    private const int MaxBackupsPerFile = 20;
    private const string TimestampFormat = "yyyyMMdd_HHmmss";

    /// <summary>Copies path to a new timestamped backup next to it, then
    /// prunes down to the newest MaxBackupsPerFile backups for that file
    /// name. Never throws - a failed backup (locked/read-only folder, out of
    /// disk space) shouldn't block saving itself, so this only ever logs
    /// and moves on rather than surfacing an error dialog.</summary>
    public static void Create(string path)
    {
        try
        {
            string? dir = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(dir)) return; // no folder to place a sibling backup in

            string stamp = DateTime.Now.ToString(TimestampFormat);
            string backupPath = $"{path}.bak_{stamp}";

            // Reopening the same save twice within one second would otherwise
            // collide on an identical timestamp - fall back to a numbered
            // suffix rather than silently overwriting the earlier backup.
            int n = 2;
            while (File.Exists(backupPath))
            {
                backupPath = $"{path}.bak_{stamp}_{n}";
                n++;
            }

            File.Copy(path, backupPath, overwrite: false);
            AppContext.State?.Log?.Invoke($"Backed up save to {Path.GetFileName(backupPath)}.");

            // Keep only the most recent MaxBackupsPerFile backups for this
            // save file, so the folder doesn't grow forever across many Open
            // Save calls.
            foreach (string f in FindAll(path).Skip(MaxBackupsPerFile))
            {
                try { File.Delete(f); }
                catch { /* not worth failing the backup over a stale file that won't delete */ }
            }
        }
        catch
        {
            AppContext.State?.Log?.Invoke("Couldn't create a backup of the save file (continuing anyway).");
        }
    }

    /// <summary>Every existing backup for the given save path, newest first -
    /// the "bak_yyyyMMdd_HHmmss" suffix sorts the same way lexicographically
    /// as chronologically, so this is a plain descending string sort, same
    /// trick the old %LocalAppData%-based scheme used. Returns an empty list
    /// if the save's folder doesn't exist or nothing's been backed up
    /// yet.</summary>
    public static IReadOnlyList<string> FindAll(string path)
    {
        string? dir = Path.GetDirectoryName(path);
        if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return Array.Empty<string>();

        string fileName = Path.GetFileName(path);
        return Directory.GetFiles(dir, $"{fileName}.bak_*")
            .OrderByDescending(f => f)
            .ToList();
    }

    /// <summary>Parses the yyyyMMdd_HHmmss timestamp out of a backup's file
    /// name (the part right after ".bak_"), or null if it doesn't match the
    /// expected format - e.g. a stray file that just happens to match the
    /// "*.bak_*" glob. RestoreBackupWindow falls back to showing the raw
    /// file name when this returns null.</summary>
    public static DateTime? ParseTimestamp(string backupPath, string savePath)
    {
        string fileName = Path.GetFileName(backupPath);
        string prefix = Path.GetFileName(savePath) + ".bak_";
        if (!fileName.StartsWith(prefix, StringComparison.Ordinal)) return null;

        string rest = fileName.Substring(prefix.Length);
        string stampPart = rest.Length >= TimestampFormat.Length ? rest.Substring(0, TimestampFormat.Length) : rest;

        return DateTime.TryParseExact(stampPart, TimestampFormat, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var dt) ? dt : null;
    }
}
