namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

/// <summary>
/// Drop-in themed replacement for System.Windows.Forms.MessageBox, styled to match
/// the app's dark navy / gold palette (see AppColors). All control layout lives in
/// AppMessageBox.Designer.cs; call AppMessageBox.Show(...) exactly like MessageBox.Show(...).
/// </summary>
public sealed partial class AppMessageBox : Form
{
    private AppMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        InitializeComponent();
        Text = caption;
        lblMessage.Text = text;
        ConfigureIcon(icon);
        ConfigureButtons(buttons);
    }

    public static DialogResult Show(string text, string caption)
        => Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.None);

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        => Show(text, caption, buttons, MessageBoxIcon.None);

    public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        using var form = new AppMessageBox(text, caption, buttons, icon);

        if (Form.ActiveForm != null)
        {
            form.StartPosition = FormStartPosition.CenterParent;
            return form.ShowDialog(Form.ActiveForm);
        }

        form.StartPosition = FormStartPosition.CenterScreen;
        return form.ShowDialog();
    }

    private void ConfigureIcon(MessageBoxIcon icon)
    {
        (string glyph, Color color) = icon switch
        {
            MessageBoxIcon.Warning => ("!", AppColors.TextGold),
            MessageBoxIcon.Error => ("×", AppColors.SideRed),
            MessageBoxIcon.Question => ("?", AppColors.TextGold),
            MessageBoxIcon.Information => ("i", AppColors.TextGold),
            _ => ("", AppColors.TextGold),
        };

        if (string.IsNullOrEmpty(glyph))
        {
            pnlIcon.Visible = false;
            int oldLeft = lblMessage.Left;
            lblMessage.Left = 24;
            lblMessage.Width += oldLeft - 24;
            return;
        }

        lblIconGlyph.Text = glyph;
        lblIconGlyph.ForeColor = color;
    }

    private void ConfigureButtons(MessageBoxButtons buttons)
    {
        switch (buttons)
        {
            case MessageBoxButtons.OKCancel:
                Configure(btnPrimary, "OK", DialogResult.OK, visible: true);
                Configure(btnSecondary, "Cancel", DialogResult.Cancel, visible: true);
                Configure(btnTertiary, "", DialogResult.None, visible: false);
                CancelButton = btnSecondary;
                break;

            case MessageBoxButtons.YesNo:
                Configure(btnPrimary, "Yes", DialogResult.Yes, visible: true);
                Configure(btnSecondary, "No", DialogResult.No, visible: true);
                Configure(btnTertiary, "", DialogResult.None, visible: false);
                break;

            case MessageBoxButtons.YesNoCancel:
                Configure(btnPrimary, "Yes", DialogResult.Yes, visible: true);
                Configure(btnSecondary, "No", DialogResult.No, visible: true);
                Configure(btnTertiary, "Cancel", DialogResult.Cancel, visible: true);
                CancelButton = btnTertiary;
                break;

            default: // OK
                Configure(btnPrimary, "OK", DialogResult.OK, visible: true);
                Configure(btnSecondary, "", DialogResult.None, visible: false);
                Configure(btnTertiary, "", DialogResult.None, visible: false);
                CancelButton = btnPrimary;
                break;
        }

        AcceptButton = btnPrimary;
    }

    private static void Configure(Button btn, string text, DialogResult result, bool visible)
    {
        btn.Text = text;
        btn.DialogResult = result;
        btn.Visible = visible;
    }
}
