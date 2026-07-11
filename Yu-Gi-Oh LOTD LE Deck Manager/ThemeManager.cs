using System.Text.Json;
using System.Text.Json.Serialization;

// Deliberately root-namespace (not .Services), like AppContext/AppColors/Theme,
// so every page/form/control can reference ThemeManager.X unqualified without
// an extra using — sibling nested namespaces (Pages/Forms/Controls) can't see
// each other's types, but everyone can see the root namespace.
namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager;

/// <summary>
/// Owns the active Theme, the list of user-created custom themes, and
/// persistence to a small JSON file in %LocalAppData%. Reachable unqualified
/// from anywhere in the app (root namespace, same pattern as AppContext).
///
/// WinForms only reads a control's colors once, when it's constructed —
/// there's no live re-skinning of an existing control tree without touching
/// every single control. So switching themes doesn't repaint in place;
/// instead ThemeChanged fires, MainForm asks Program to rebuild itself
/// (see Program.cs's restart loop), and every control comes up fresh with
/// the new palette since Designer.cs files read AppColors.X, which now
/// reads through to Current.
/// </summary>
public static class ThemeManager
{
    private static readonly string ConfigDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "Yu-Gi-Oh LOTD LE Deck Editor");

    private static readonly string ConfigPath = Path.Combine(ConfigDir, "theme-config.json");

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        Converters = { new ColorJsonConverter() },
    };

    public static Theme Current { get; private set; } = Theme.Dark;

    public static List<Theme> CustomThemes { get; private set; } = new();

    public static IReadOnlyList<Theme> BuiltInThemes { get; } = new[] { Theme.Dark, Theme.Light };

    public static IEnumerable<Theme> AllThemes => BuiltInThemes.Concat(CustomThemes);

    /// <summary>Fired after Current changes (SetActiveTheme). MainForm
    /// subscribes and triggers a soft restart to apply it everywhere.</summary>
    public static event Action? ThemeChanged;

    /// <summary>Read by Program.cs's main loop; set true right before a
    /// theme-driven restart so the loop knows to rebuild MainForm instead
    /// of exiting the app.</summary>
    public static bool RestartRequested { get; set; }

    public static void Load()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                var data = JsonSerializer.Deserialize<ThemeConfigFile>(json, JsonOpts);
                if (data is not null)
                {
                    CustomThemes = data.CustomThemes ?? new List<Theme>();
                    foreach (var t in CustomThemes) t.IsBuiltIn = false;

                    Current = AllBuiltInAnd(CustomThemes).FirstOrDefault(t => t.Name == data.ActiveThemeName)
                              ?? Theme.Dark;

                    // Built-in themes (Dark/Light) are regenerated fresh from
                    // Theme.Dark/Theme.Light every load, which would otherwise
                    // silently drop a background image the user picked while
                    // one of them was active. The background is persisted
                    // independently of which palette is active so it survives
                    // real app restarts either way.
                    if (data.BackgroundImagePath is not null)
                        Current.BackgroundImagePath = data.BackgroundImagePath;
                    if (data.BackgroundOverlayOpacity.HasValue)
                        Current.BackgroundOverlayOpacity = data.BackgroundOverlayOpacity.Value;

                    return;
                }
            }
        }
        catch
        {
            // Corrupt or unreadable config — fall back to the default theme
            // rather than crashing app startup.
        }

        Current = Theme.Dark;
    }

    private static IEnumerable<Theme> AllBuiltInAnd(IEnumerable<Theme> custom) =>
        new[] { Theme.Dark, Theme.Light }.Concat(custom);

    /// <summary>Switches the active theme, persists the choice, and notifies
    /// subscribers. Does NOT restart anything itself — the caller (Settings
    /// page / Theme Editor) decides when to actually apply it.</summary>
    public static void SetActiveTheme(Theme theme)
    {
        Current = theme;
        Save();
        ThemeChanged?.Invoke();
    }

    /// <summary>Adds a new custom theme, or overwrites an existing one with
    /// the same name (case-sensitive match).</summary>
    public static void SaveCustomTheme(Theme theme)
    {
        theme.IsBuiltIn = false;
        int existingIndex = CustomThemes.FindIndex(t => t.Name == theme.Name);
        if (existingIndex >= 0) CustomThemes[existingIndex] = theme;
        else CustomThemes.Add(theme);

        // If we just overwrote the theme that's currently active, refresh
        // Current so it picks up the new values immediately.
        if (Current.Name == theme.Name && !Current.IsBuiltIn)
            Current = theme;

        Save();
    }

    public static void DeleteCustomTheme(Theme theme)
    {
        CustomThemes.RemoveAll(t => t.Name == theme.Name);

        if (Current.Name == theme.Name)
            SetActiveTheme(Theme.Dark);
        else
            Save();
    }

    /// <summary>Sets (or clears, with path=null) the background image on
    /// whatever theme is currently active, persists it, and restarts to
    /// show it. Independent of SetActiveTheme so "change the wallpaper"
    /// doesn't require also picking a different color palette.</summary>
    public static void SetBackgroundImage(string? path, double opacity)
    {
        Current.BackgroundImagePath = path;
        Current.BackgroundOverlayOpacity = opacity;

        // Keep a saved custom theme's own copy in sync too, so switching
        // away and back doesn't revert the image.
        if (!Current.IsBuiltIn)
        {
            int i = CustomThemes.FindIndex(t => t.Name == Current.Name);
            if (i >= 0) CustomThemes[i] = Current;
        }

        Save();
        ThemeChanged?.Invoke();
    }

    /// <summary>Writes a theme out to a standalone .json file so it can be
    /// shared — same shape/converter as the app's own theme-config.json, just
    /// a single Theme object instead of the whole config wrapper.</summary>
    public static void ExportTheme(Theme theme, string filePath) =>
        File.WriteAllText(filePath, JsonSerializer.Serialize(theme, JsonOpts));

    /// <summary>Reads a theme previously written by ExportTheme (or hand-
    /// authored in the same shape). Returns null if the file can't be parsed.
    /// The result is never marked IsBuiltIn and is renamed if it collides
    /// with an existing theme name — callers still need to SaveCustomTheme
    /// it themselves to actually add it to the library.</summary>
    public static Theme? ImportTheme(string filePath)
    {
        try
        {
            string json = File.ReadAllText(filePath);
            var theme = JsonSerializer.Deserialize<Theme>(json, JsonOpts);
            if (theme is null) return null;

            theme.IsBuiltIn = false;
            string baseName = string.IsNullOrWhiteSpace(theme.Name) ? "Imported" : theme.Name;
            theme.Name = SuggestUniqueName(baseName);
            return theme;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Generates a unique name for a "Save as new" flow, e.g.
    /// "Custom", "Custom 2", "Custom 3"...</summary>
    public static string SuggestUniqueName(string baseName)
    {
        var taken = new HashSet<string>(AllThemes.Select(t => t.Name), StringComparer.OrdinalIgnoreCase);
        if (!taken.Contains(baseName)) return baseName;

        for (int i = 2; i < 1000; i++)
        {
            string candidate = $"{baseName} {i}";
            if (!taken.Contains(candidate)) return candidate;
        }
        return $"{baseName} {Guid.NewGuid():N}"[..24];
    }

    private static void Save()
    {
        try
        {
            Directory.CreateDirectory(ConfigDir);
            var data = new ThemeConfigFile
            {
                ActiveThemeName = Current.Name,
                CustomThemes = CustomThemes,
                BackgroundImagePath = Current.BackgroundImagePath,
                BackgroundOverlayOpacity = Current.BackgroundOverlayOpacity,
            };
            File.WriteAllText(ConfigPath, JsonSerializer.Serialize(data, JsonOpts));
        }
        catch
        {
            // Best-effort persistence — a failed write just means the
            // choice won't survive a restart, not a crash.
        }
    }

    private sealed class ThemeConfigFile
    {
        public string ActiveThemeName { get; set; } = "Dark";
        public List<Theme>? CustomThemes { get; set; }
        public string? BackgroundImagePath { get; set; }
        public double? BackgroundOverlayOpacity { get; set; }
    }
}

/// <summary>Serializes System.Drawing.Color as a compact "#RRGGBB" hex
/// string instead of System.Text.Json's default (every public property of
/// the struct, including the named-color/system-color plumbing).</summary>
public sealed class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? s = reader.GetString();
        if (string.IsNullOrEmpty(s)) return Color.Black;

        s = s.TrimStart('#');
        if (s.Length < 6) return Color.Black;

        int r = Convert.ToInt32(s.Substring(0, 2), 16);
        int g = Convert.ToInt32(s.Substring(2, 2), 16);
        int b = Convert.ToInt32(s.Substring(4, 2), 16);
        return Color.FromArgb(r, g, b);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options) =>
        writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
}
