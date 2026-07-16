using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

/// <summary>
/// App-wide settings tab: Offline Mode toggle (backed by
/// AppContext.State.IsOfflineMode) and Appearance (theme picker + the
/// Theme Editor). All control layout lives in SettingsPage.Designer.cs.
/// </summary>
public sealed partial class SettingsPage : UserControl
{
    private bool _suppressCheckedChanged;
    private bool _suppressThemeChanged;

    public SettingsPage()
    {
        InitializeComponent();

        // Shows the active theme's background image (if any) behind the
        // page — see BackgroundPainter. No-ops entirely when no image is set.
        Paint += BackgroundPainter.Paint;

        chkOfflineMode.Checked = AppContext.State.IsOfflineMode;

        RefreshThemeList();
        RefreshBackgroundLabel();
    }

    private void chkOfflineMode_CheckedChanged(object? sender, EventArgs e)
    {
        if (_suppressCheckedChanged) return;
        AppContext.State.IsOfflineMode = chkOfflineMode.Checked;
    }

    /// <summary>Keeps the checkbox in sync if IsOfflineMode is ever changed
    /// from somewhere other than this page.</summary>
    public void RefreshFromState()
    {
        _suppressCheckedChanged = true;
        chkOfflineMode.Checked = AppContext.State.IsOfflineMode;
        _suppressCheckedChanged = false;
    }

    // ── Appearance ────────────────────────────────────────────────────────────
    private void RefreshThemeList()
    {
        _suppressThemeChanged = true;

        cboTheme.Items.Clear();
        foreach (var t in ThemeManager.AllThemes) cboTheme.Items.Add(t.Name);
        cboTheme.SelectedItem = ThemeManager.Current.Name;
        if (cboTheme.SelectedIndex < 0 && cboTheme.Items.Count > 0)
            cboTheme.SelectedIndex = 0;

        _suppressThemeChanged = false;
    }

    // Picking a theme from the dropdown applies it immediately. This briefly
    // reloads the main window (see ThemeManager/Program.cs) — WinForms
    // controls only read their colors once, at construction, so there's no
    // way to re-skin everything already on screen without rebuilding it.
    private void cboTheme_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_suppressThemeChanged) return;
        if (cboTheme.SelectedItem is not string name) return;
        if (name == ThemeManager.Current.Name) return;

        var chosen = ThemeManager.AllThemes.FirstOrDefault(t => t.Name == name);
        if (chosen is not null)
            ThemeManager.SetActiveTheme(chosen);
    }

    private void btnCustomizeTheme_Click(object? sender, EventArgs e)
    {
        var owner = FindForm();
        var startingTheme = ThemeManager.AllThemes.FirstOrDefault(t => t.Name == (cboTheme.SelectedItem as string))
                             ?? ThemeManager.Current;

        // Show(...) handles persisting/applying/deleting internally, and
        // only triggers the (possible) main-window restart after its own
        // modal loop has fully closed — see ThemeEditorForm's doc comments.
        ThemeEditorForm.Show(owner!, startingTheme);

        RefreshThemeList();
        RefreshBackgroundLabel();
    }

    private void RefreshBackgroundLabel()
    {
        string? path = ThemeManager.Current.BackgroundImagePath;
        lblBgStatus.Text = string.IsNullOrWhiteSpace(path) ? "None" : Path.GetFileName(path);
        btnClearBg.Enabled = !string.IsNullOrWhiteSpace(path);
    }

    // Independent of which theme/palette is active — pick a wallpaper
    // without needing to open the full color editor.
    private void btnChooseBg_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Choose a background image",
            Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files (*.*)|*.*",
            CheckFileExists = true,
        };
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;

        ThemeManager.SetBackgroundImage(dlg.FileName, ThemeManager.Current.BackgroundOverlayOpacity);
    }

    private void btnClearBg_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ThemeManager.Current.BackgroundImagePath)) return;
        ThemeManager.SetBackgroundImage(null, ThemeManager.Current.BackgroundOverlayOpacity);
    }

    // ── Theme file (export/import) ───────────────────────────────────────────
    // Lets a user share a custom theme, or load one someone sent them,
    // without going through the full color-by-color editor.
    private void btnExportTheme_Click(object? sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Title = "Export theme",
            Filter = "Theme files (*.json)|*.json",
            DefaultExt = "json",
            FileName = ThemeManager.Current.Name + ".theme.json",
        };
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;

        ThemeManager.ExportTheme(ThemeManager.Current, dlg.FileName);
        AppMessageBox.Show(
            $"Exported '{ThemeManager.Current.Name}' to {Path.GetFileName(dlg.FileName)}.",
            "Export Theme", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnImportTheme_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Import theme",
            Filter = "Theme files (*.json)|*.json",
            CheckFileExists = true,
        };
        if (dlg.ShowDialog(FindForm()) != DialogResult.OK) return;

        var theme = ThemeManager.ImportTheme(dlg.FileName);
        if (theme is null)
        {
            AppMessageBox.Show("Couldn't read that theme file — it may not be a valid exported theme.",
                "Import Theme", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        ThemeManager.SaveCustomTheme(theme);

        // Same direct call as the theme dropdown (cboTheme_SelectedIndexChanged)
        // uses — safe here because this page isn't a modal dialog, unlike
        // ThemeEditorForm, which has to defer this until after ShowDialog returns.
        ThemeManager.SetActiveTheme(theme);
    }
}
