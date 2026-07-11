namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

partial class SettingsPage
{
    private Panel pnlGeneral = null!;
    private Label lblGeneralTitle = null!;
    private CheckBox chkOfflineMode = null!;
    private Label lblOfflineHint = null!;
    private Panel pnlAppearance = null!;
    private Label lblAppearanceTitle = null!;
    private ComboBox cboTheme = null!;
    private Button btnCustomizeTheme = null!;
    private Label lblThemeHint = null!;
    private Label lblBgSectionTitle = null!;
    private Label lblBgStatus = null!;
    private Button btnChooseBg = null!;
    private Button btnClearBg = null!;
    private Label lblThemeFileTitle = null!;
    private Button btnExportTheme = null!;
    private Button btnImportTheme = null!;
    private Label lblThemeFileHint = null!;

    private void InitializeComponent()
    {
        pnlGeneral = new Panel();
        lblGeneralTitle = new Label();
        chkOfflineMode = new CheckBox();
        lblOfflineHint = new Label();
        pnlAppearance = new Panel();
        lblAppearanceTitle = new Label();
        cboTheme = new ComboBox();
        btnCustomizeTheme = new Button();
        lblThemeHint = new Label();
        lblBgSectionTitle = new Label();
        lblBgStatus = new Label();
        btnChooseBg = new Button();
        btnClearBg = new Button();
        lblThemeFileTitle = new Label();
        btnExportTheme = new Button();
        btnImportTheme = new Button();
        lblThemeFileHint = new Label();
        pnlGeneral.SuspendLayout();
        pnlAppearance.SuspendLayout();
        SuspendLayout();
        //
        // pnlGeneral
        //
        pnlGeneral.BackColor = Color.Transparent;
        pnlGeneral.Controls.Add(lblGeneralTitle);
        pnlGeneral.Controls.Add(chkOfflineMode);
        pnlGeneral.Controls.Add(lblOfflineHint);
        pnlGeneral.Location = new Point(24, 24);
        pnlGeneral.Name = "pnlGeneral";
        pnlGeneral.Size = new Size(420, 100);
        pnlGeneral.TabIndex = 0;
        //
        // lblGeneralTitle
        //
        lblGeneralTitle.BackColor = Color.Transparent;
        lblGeneralTitle.Font = new Font("Segoe UI", 7F);
        lblGeneralTitle.ForeColor = AppColors.TextDim;
        lblGeneralTitle.Location = new Point(0, 0);
        lblGeneralTitle.Name = "lblGeneralTitle";
        lblGeneralTitle.Size = new Size(400, 16);
        lblGeneralTitle.TabIndex = 0;
        lblGeneralTitle.Text = "GENERAL";
        //
        // chkOfflineMode
        //
        chkOfflineMode.BackColor = Color.Transparent;
        chkOfflineMode.Font = new Font("Segoe UI", 9.5F);
        chkOfflineMode.ForeColor = AppColors.TextPrimary;
        chkOfflineMode.Location = new Point(0, 24);
        chkOfflineMode.Name = "chkOfflineMode";
        chkOfflineMode.Size = new Size(400, 24);
        chkOfflineMode.TabIndex = 1;
        chkOfflineMode.Text = "Offline mode";
        chkOfflineMode.UseVisualStyleBackColor = false;
        chkOfflineMode.CheckedChanged += chkOfflineMode_CheckedChanged;
        //
        // lblOfflineHint
        //
        lblOfflineHint.BackColor = Color.Transparent;
        lblOfflineHint.Font = new Font("Segoe UI", 8.5F);
        lblOfflineHint.ForeColor = AppColors.TextMuted;
        lblOfflineHint.Location = new Point(22, 50);
        lblOfflineHint.Name = "lblOfflineHint";
        lblOfflineHint.Size = new Size(390, 40);
        lblOfflineHint.TabIndex = 2;
        lblOfflineHint.Text = "Skip fetching card images and data from the network. Pages that support it will read this the next time they need online data.";
        //
        // pnlAppearance
        //
        pnlAppearance.BackColor = Color.Transparent;
        pnlAppearance.Controls.Add(lblAppearanceTitle);
        pnlAppearance.Controls.Add(cboTheme);
        pnlAppearance.Controls.Add(btnCustomizeTheme);
        pnlAppearance.Controls.Add(lblThemeHint);
        pnlAppearance.Controls.Add(lblBgSectionTitle);
        pnlAppearance.Controls.Add(lblBgStatus);
        pnlAppearance.Controls.Add(btnChooseBg);
        pnlAppearance.Controls.Add(btnClearBg);
        pnlAppearance.Controls.Add(lblThemeFileTitle);
        pnlAppearance.Controls.Add(btnExportTheme);
        pnlAppearance.Controls.Add(btnImportTheme);
        pnlAppearance.Controls.Add(lblThemeFileHint);
        pnlAppearance.Location = new Point(24, 140);
        pnlAppearance.Name = "pnlAppearance";
        pnlAppearance.Size = new Size(420, 220);
        pnlAppearance.TabIndex = 1;
        //
        // lblAppearanceTitle
        //
        lblAppearanceTitle.BackColor = Color.Transparent;
        lblAppearanceTitle.Font = new Font("Segoe UI", 7F);
        lblAppearanceTitle.ForeColor = AppColors.TextDim;
        lblAppearanceTitle.Location = new Point(0, 0);
        lblAppearanceTitle.Name = "lblAppearanceTitle";
        lblAppearanceTitle.Size = new Size(400, 16);
        lblAppearanceTitle.TabIndex = 0;
        lblAppearanceTitle.Text = "APPEARANCE";
        //
        // cboTheme
        //
        cboTheme.BackColor = AppColors.InputBg;
        cboTheme.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTheme.ForeColor = AppColors.TextPrimary;
        cboTheme.FormattingEnabled = true;
        cboTheme.Location = new Point(0, 24);
        cboTheme.Name = "cboTheme";
        cboTheme.Size = new Size(240, 23);
        cboTheme.TabIndex = 1;
        cboTheme.SelectedIndexChanged += cboTheme_SelectedIndexChanged;
        //
        // btnCustomizeTheme
        //
        btnCustomizeTheme.BackColor = Color.Transparent;
        btnCustomizeTheme.FlatAppearance.BorderColor = AppColors.Border;
        btnCustomizeTheme.FlatAppearance.BorderSize = 1;
        btnCustomizeTheme.FlatStyle = FlatStyle.Flat;
        btnCustomizeTheme.ForeColor = AppColors.TextSecondary;
        btnCustomizeTheme.Location = new Point(250, 23);
        btnCustomizeTheme.Name = "btnCustomizeTheme";
        btnCustomizeTheme.Size = new Size(150, 25);
        btnCustomizeTheme.TabIndex = 2;
        btnCustomizeTheme.Text = "Customize theme...";
        btnCustomizeTheme.UseVisualStyleBackColor = false;
        btnCustomizeTheme.Click += btnCustomizeTheme_Click;
        //
        // lblThemeHint
        //
        lblThemeHint.BackColor = Color.Transparent;
        lblThemeHint.Font = new Font("Segoe UI", 8.5F);
        lblThemeHint.ForeColor = AppColors.TextMuted;
        lblThemeHint.Location = new Point(0, 58);
        lblThemeHint.Name = "lblThemeHint";
        lblThemeHint.Size = new Size(400, 32);
        lblThemeHint.TabIndex = 3;
        lblThemeHint.Text = "Switching or customizing a theme briefly reloads the window to apply it everywhere.";
        //
        // lblBgSectionTitle
        //
        lblBgSectionTitle.BackColor = Color.Transparent;
        lblBgSectionTitle.Font = new Font("Segoe UI", 7F);
        lblBgSectionTitle.ForeColor = AppColors.TextDim;
        lblBgSectionTitle.Location = new Point(0, 96);
        lblBgSectionTitle.Name = "lblBgSectionTitle";
        lblBgSectionTitle.Size = new Size(300, 16);
        lblBgSectionTitle.TabIndex = 4;
        lblBgSectionTitle.Text = "BACKGROUND IMAGE";
        //
        // lblBgStatus
        //
        lblBgStatus.AutoEllipsis = true;
        lblBgStatus.BackColor = Color.Transparent;
        lblBgStatus.Font = new Font("Segoe UI", 8.5F);
        lblBgStatus.ForeColor = AppColors.TextSecondary;
        lblBgStatus.Location = new Point(0, 116);
        lblBgStatus.Name = "lblBgStatus";
        lblBgStatus.Size = new Size(240, 32);
        lblBgStatus.TabIndex = 5;
        lblBgStatus.Text = "None";
        //
        // btnChooseBg
        //
        btnChooseBg.BackColor = Color.Transparent;
        btnChooseBg.FlatAppearance.BorderColor = AppColors.Border;
        btnChooseBg.FlatAppearance.BorderSize = 1;
        btnChooseBg.FlatStyle = FlatStyle.Flat;
        btnChooseBg.ForeColor = AppColors.TextSecondary;
        btnChooseBg.Location = new Point(250, 114);
        btnChooseBg.Name = "btnChooseBg";
        btnChooseBg.Size = new Size(80, 25);
        btnChooseBg.TabIndex = 6;
        btnChooseBg.Text = "Choose...";
        btnChooseBg.UseVisualStyleBackColor = false;
        btnChooseBg.Click += btnChooseBg_Click;
        //
        // btnClearBg
        //
        btnClearBg.BackColor = Color.Transparent;
        btnClearBg.FlatAppearance.BorderColor = AppColors.DangerBorder;
        btnClearBg.FlatAppearance.BorderSize = 1;
        btnClearBg.FlatStyle = FlatStyle.Flat;
        btnClearBg.ForeColor = AppColors.DangerFg;
        btnClearBg.Location = new Point(336, 114);
        btnClearBg.Name = "btnClearBg";
        btnClearBg.Size = new Size(80, 25);
        btnClearBg.TabIndex = 7;
        btnClearBg.Text = "Clear image";
        btnClearBg.UseVisualStyleBackColor = false;
        btnClearBg.Click += btnClearBg_Click;
        //
        // lblThemeFileTitle
        //
        lblThemeFileTitle.BackColor = Color.Transparent;
        lblThemeFileTitle.Font = new Font("Segoe UI", 7F);
        lblThemeFileTitle.ForeColor = AppColors.TextDim;
        lblThemeFileTitle.Location = new Point(0, 150);
        lblThemeFileTitle.Name = "lblThemeFileTitle";
        lblThemeFileTitle.Size = new Size(300, 16);
        lblThemeFileTitle.TabIndex = 8;
        lblThemeFileTitle.Text = "THEME FILE";
        //
        // btnExportTheme
        //
        btnExportTheme.BackColor = Color.Transparent;
        btnExportTheme.FlatAppearance.BorderColor = AppColors.Border;
        btnExportTheme.FlatAppearance.BorderSize = 1;
        btnExportTheme.FlatStyle = FlatStyle.Flat;
        btnExportTheme.ForeColor = AppColors.TextSecondary;
        btnExportTheme.Location = new Point(0, 170);
        btnExportTheme.Name = "btnExportTheme";
        btnExportTheme.Size = new Size(110, 25);
        btnExportTheme.TabIndex = 9;
        btnExportTheme.Text = "Export...";
        btnExportTheme.UseVisualStyleBackColor = false;
        btnExportTheme.Click += btnExportTheme_Click;
        //
        // btnImportTheme
        //
        btnImportTheme.BackColor = Color.Transparent;
        btnImportTheme.FlatAppearance.BorderColor = AppColors.Border;
        btnImportTheme.FlatAppearance.BorderSize = 1;
        btnImportTheme.FlatStyle = FlatStyle.Flat;
        btnImportTheme.ForeColor = AppColors.TextSecondary;
        btnImportTheme.Location = new Point(118, 170);
        btnImportTheme.Name = "btnImportTheme";
        btnImportTheme.Size = new Size(110, 25);
        btnImportTheme.TabIndex = 10;
        btnImportTheme.Text = "Import...";
        btnImportTheme.UseVisualStyleBackColor = false;
        btnImportTheme.Click += btnImportTheme_Click;
        //
        // lblThemeFileHint
        //
        lblThemeFileHint.BackColor = Color.Transparent;
        lblThemeFileHint.Font = new Font("Segoe UI", 8.5F);
        lblThemeFileHint.ForeColor = AppColors.TextMuted;
        lblThemeFileHint.Location = new Point(236, 170);
        lblThemeFileHint.Name = "lblThemeFileHint";
        lblThemeFileHint.Size = new Size(184, 44);
        lblThemeFileHint.TabIndex = 11;
        lblThemeFileHint.Text = "Save your custom theme to share it, or load one someone sent you.";
        //
        // SettingsPage
        //
        BackColor = AppColors.AppBg;
        Controls.Add(pnlGeneral);
        Controls.Add(pnlAppearance);
        Name = "SettingsPage";
        Size = new Size(1055, 599);
        pnlGeneral.ResumeLayout(false);
        pnlAppearance.ResumeLayout(false);
        ResumeLayout(false);
    }
}
