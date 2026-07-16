using System.Reflection;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

/// <summary>
/// Modal color/background-image editor for a Theme. Works on a private clone
/// (_draft) so nothing is written to ThemeManager until Save is pressed —
/// mirrors CardFilterForm's Show(...) pattern (static factory, nullable
/// result). Every swatch Button carries the Theme property name it edits in
/// its Tag; clicking one uses reflection to read/write that single Color
/// property on _draft, which keeps this file's logic generic instead of
/// needing 47 near-identical click handlers. All control layout lives in
/// ThemeEditorForm.Designer.cs.
/// </summary>
public sealed partial class ThemeEditorForm : Form
{
    private readonly Theme _draft;
    private readonly bool _isExistingCustom;
    private readonly List<(Button Swatch, PropertyInfo Prop)> _swatches = new();

    private ThemeEditorForm(Theme startingTheme)
    {
        InitializeComponent();

        _isExistingCustom = !startingTheme.IsBuiltIn &&
                             ThemeManager.CustomThemes.Any(t => t.Name == startingTheme.Name);

        // Starting from a built-in preset always forks into a new custom
        // theme (built-ins can't be overwritten); starting from an existing
        // custom theme keeps editing it in place (Save overwrites unless
        // the name is changed).
        _draft = startingTheme.IsBuiltIn
            ? startingTheme.Clone(ThemeManager.SuggestUniqueName(startingTheme.Name + " Custom"))
            : startingTheme.Clone(startingTheme.Name);

        // Collect every swatch button (Tag = Theme property name) once, via
        // reflection, instead of hand-writing 47 getter/setter branches.
        foreach (Control c in pnlSwatches.Controls)
        {
            if (c is Button btn && btn.Tag is string propName)
            {
                var prop = typeof(Theme).GetProperty(propName);
                if (prop is not null) _swatches.Add((btn, prop));
            }
        }

        cboBase.Items.Add("(current values)");
        foreach (var t in ThemeManager.AllThemes) cboBase.Items.Add(t.Name);
        cboBase.SelectedIndex = 0;

        txtName.Text = _draft.Name;
        chkIsDark.Checked = _draft.IsDark;
        txtBgPath.Text = _draft.BackgroundImagePath ?? string.Empty;
        trkOpacity.Value = (int)Math.Round(Math.Clamp(_draft.BackgroundOverlayOpacity, 0, 1) * 100);
        lblOpacityValue.Text = $"{trkOpacity.Value}%";

        btnDelete.Visible = _isExistingCustom;

        RefreshSwatches();
    }

    /// <summary>True if Save was pressed with "apply immediately" checked.
    /// Read by Show(...) — deliberately AFTER the dialog has fully closed,
    /// since applying a theme triggers MainForm to close itself for a soft
    /// restart (see ThemeManager/Program.cs), and doing that while this
    /// modal dialog's own message loop is still active would race with it.</summary>
    private bool ApplyImmediately { get; set; }

    /// <summary>Shows the editor modally, starting from startingTheme's
    /// values. Returns the finalized (already-persisted) Theme if Save was
    /// pressed, or null if cancelled/deleted. Applying a theme (Save with
    /// "apply immediately", or Delete falling back to Dark) restarts the
    /// main window (see ThemeManager/Program.cs) — that only happens here,
    /// after ShowDialog has returned, never while this modal is still up.</summary>
    public static Theme? Show(IWin32Window owner, Theme startingTheme)
    {
        using var form = new ThemeEditorForm(startingTheme);
        var dialogResult = form.ShowDialog(owner);

        if (dialogResult == DialogResult.Abort) // Delete was pressed
        {
            ThemeManager.DeleteCustomTheme(startingTheme);
            return null;
        }

        if (dialogResult != DialogResult.OK) return null;

        if (form.ApplyImmediately)
            ThemeManager.SetActiveTheme(form._draft);

        return form._draft;
    }

    private void RefreshSwatches()
    {
        foreach (var (swatch, prop) in _swatches)
            swatch.BackColor = (Color)prop.GetValue(_draft)!;
    }

    private void swatch_Click(object? sender, EventArgs e)
    {
        if (sender is not Button btn) return;

        var prop = _swatches.FirstOrDefault(s => s.Swatch == btn).Prop;
        if (prop is null) return;

        using var dlg = new ColorDialog
        {
            Color = (Color)prop.GetValue(_draft)!,
            FullOpen = true,
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        prop.SetValue(_draft, dlg.Color);
        btn.BackColor = dlg.Color;
    }

    private void cboBase_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboBase.SelectedIndex <= 0) return; // "(current values)" — no-op

        string chosenName = (string)cboBase.SelectedItem!;
        var preset = ThemeManager.AllThemes.FirstOrDefault(t => t.Name == chosenName);
        if (preset is null) return;

        // Copy every color + IsDark from the preset into the draft, but keep
        // the name/background-image the user already set up.
        foreach (var (_, prop) in _swatches)
            prop.SetValue(_draft, prop.GetValue(preset));

        _draft.IsDark = preset.IsDark;
        chkIsDark.Checked = preset.IsDark;

        RefreshSwatches();
    }

    private void btnBrowseBg_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Choose a background image",
            Filter = "Image files (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files (*.*)|*.*",
            CheckFileExists = true,
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;

        _draft.BackgroundImagePath = dlg.FileName;
        txtBgPath.Text = dlg.FileName;
    }

    private void btnClearBg_Click(object? sender, EventArgs e)
    {
        _draft.BackgroundImagePath = null;
        txtBgPath.Text = string.Empty;
    }

    private void trkOpacity_Scroll(object? sender, EventArgs e)
    {
        _draft.BackgroundOverlayOpacity = trkOpacity.Value / 100.0;
        lblOpacityValue.Text = $"{trkOpacity.Value}%";
    }

    // Actual deletion happens in Show(), after this dialog has fully closed —
    // ThemeManager.DeleteCustomTheme can trigger a MainForm restart if the
    // deleted theme was active, which must never happen while this modal's
    // own message loop is still running.
    private void btnDelete_Click(object? sender, EventArgs e)
    {
        var res = AppMessageBox.Show(
            $"Delete the custom theme \"{_draft.Name}\"? This can't be undone.",
            "Delete Theme", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (res != DialogResult.Yes) return;

        DialogResult = DialogResult.Abort; // distinct from OK/Cancel — signals "delete" to Show()
        Close();
    }

    private void btnSave_Click(object? sender, EventArgs e)
    {
        string name = txtName.Text.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            AppMessageBox.Show("Give this theme a name before saving.", "Save Theme", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (ThemeManager.BuiltInThemes.Any(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            AppMessageBox.Show(
                $"\"{name}\" is a built-in theme name and can't be reused. Pick a different name.",
                "Save Theme", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _draft.Name = name;
        _draft.IsDark = chkIsDark.Checked;
        _draft.IsBuiltIn = false;

        // Persist only — applying it live (if requested) happens in Show(),
        // after this dialog has fully closed. See ApplyImmediately's doc.
        ThemeManager.SaveCustomTheme(_draft);
        ApplyImmediately = chkApplyNow.Checked;

        DialogResult = DialogResult.OK;
        Close();
    }
}
