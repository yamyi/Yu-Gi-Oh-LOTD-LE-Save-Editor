using System;
using System.Windows;
using YuGiOhSaveEditor.Services;

namespace YuGiOhSaveEditor;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Restore the user's saved Offline Mode/disk cache/cache size/theme
        // preferences BEFORE base.OnStartup - App.xaml's StartupUri is what
        // actually constructs MainWindow, and every Style/Setter in
        // Controls.xaml (and every view) resolves its {StaticResource ...}
        // brushes the first time it's parsed, which happens during that
        // construction. ThemeName has to be applied before that point or
        // it's too late - StaticResource bakes in whatever was resolvable at
        // parse time and never re-resolves afterward, unlike DynamicResource.
        AppSettings.Load();
        ApplyTheme(AppContext.State.ThemeName);

        base.OnStartup(e);

        // Load the embedded card database once at startup — same idempotent
        // Load() call the WinForms app made from MainForm's constructor.
        AppContext.CardDb.Load();
    }

    /// <summary>Layers Themes/DuelLinksTheme.xaml on top of the
    /// always-merged Themes/MasterDuelTheme.xaml when themeName is
    /// "DuelLinks" - anything DuelLinksTheme.xaml doesn't define (every
    /// brush added since the Master Duel redesign: AccentBrush, the
    /// rarity/status brushes, the diagonal-line pattern, the translucent/
    /// scrim gradients, ...) falls through to MasterDuelTheme.xaml's value
    /// instead of throwing a missing-resource error, because a merged
    /// ResourceDictionary's key lookup walks its MergedDictionaries in
    /// reverse (last-added wins for a key both define, and it falls back to
    /// an earlier entry for a key only that one defines). So this is a
    /// genuine "restore the old Duel Links look" rather than the original,
    /// stale, would-crash-if-merged-alone DuelLinksTheme.xaml - just a
    /// partial one: whatever it never defined still shows in Master Duel's
    /// colors. "MasterDuel" (or anything else unrecognized) just removes
    /// DuelLinksTheme.xaml again, leaving App.xaml's original single
    /// dictionary in place.</summary>
    public static void ApplyTheme(string themeName)
    {
        if (Current == null) return;
        var dictionaries = Current.Resources.MergedDictionaries;

        for (int i = dictionaries.Count - 1; i >= 0; i--)
        {
            if (dictionaries[i].Source?.OriginalString.EndsWith("DuelLinksTheme.xaml") == true)
                dictionaries.RemoveAt(i);
        }

        if (themeName == "DuelLinks")
        {
            // Index 1: right after MasterDuelTheme.xaml (index 0, the
            // fallback base) and before Controls.xaml (now pushed to 2).
            dictionaries.Insert(1, new ResourceDictionary
            {
                Source = new Uri("Themes/DuelLinksTheme.xaml", UriKind.Relative),
            });
        }
    }
}
