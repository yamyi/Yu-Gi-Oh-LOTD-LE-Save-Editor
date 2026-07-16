namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

partial class ThemeEditorForm
{
    // ── Header: name + start-from-preset ────────────────────────────────────
    private Label lblNameTitle = null!;
    private TextBox txtName = null!;
    private Label lblBaseTitle = null!;
    private ComboBox cboBase = null!;
    private CheckBox chkIsDark = null!;

    // ── Background image ─────────────────────────────────────────────────────
    private Label lblBgTitle = null!;
    private TextBox txtBgPath = null!;
    private Button btnBrowseBg = null!;
    private Button btnClearBg = null!;
    private Label lblOpacityTitle = null!;
    private TrackBar trkOpacity = null!;
    private Label lblOpacityValue = null!;

    // ── Scrollable swatch list ───────────────────────────────────────────────
    private Panel pnlSwatches = null!;
    private Label lblSecBACKGROUNDS = null!;
    private Label lblSwAppBg = null!;
    private Button swAppBg = null!;
    private Label lblSwSidebarBg = null!;
    private Button swSidebarBg = null!;
    private Label lblSwCardBg = null!;
    private Button swCardBg = null!;
    private Label lblSwCardHover = null!;
    private Button swCardHover = null!;
    private Label lblSwCardSelected = null!;
    private Button swCardSelected = null!;
    private Label lblSwTopbarBg = null!;
    private Button swTopbarBg = null!;
    private Label lblSwInputBg = null!;
    private Button swInputBg = null!;
    private Label lblSwActionBtnBg = null!;
    private Button swActionBtnBg = null!;
    private Label lblSecBORDERS = null!;
    private Label lblSwBorder = null!;
    private Button swBorder = null!;
    private Label lblSwBorderHover = null!;
    private Button swBorderHover = null!;
    private Label lblSwBorderSelected = null!;
    private Button swBorderSelected = null!;
    private Label lblSecTEXT = null!;
    private Label lblSwTextPrimary = null!;
    private Button swTextPrimary = null!;
    private Label lblSwTextSecondary = null!;
    private Button swTextSecondary = null!;
    private Label lblSwTextMuted = null!;
    private Button swTextMuted = null!;
    private Label lblSwTextDim = null!;
    private Button swTextDim = null!;
    private Label lblSwTextGold = null!;
    private Button swTextGold = null!;
    private Label lblSwTextNavActive = null!;
    private Button swTextNavActive = null!;
    private Label lblSwTextNav = null!;
    private Button swTextNav = null!;
    private Label lblSwTextInfo = null!;
    private Button swTextInfo = null!;
    private Label lblSwTextCardName = null!;
    private Button swTextCardName = null!;
    private Label lblSwTextDescription = null!;
    private Button swTextDescription = null!;
    private Label lblSecDECKSTATS = null!;
    private Label lblSwMainGreen = null!;
    private Button swMainGreen = null!;
    private Label lblSwMainBg = null!;
    private Button swMainBg = null!;
    private Label lblSwMainBorder = null!;
    private Button swMainBorder = null!;
    private Label lblSwExtraBlue = null!;
    private Button swExtraBlue = null!;
    private Label lblSwExtraBg = null!;
    private Button swExtraBg = null!;
    private Label lblSwExtraBorder = null!;
    private Button swExtraBorder = null!;
    private Label lblSwSideRed = null!;
    private Button swSideRed = null!;
    private Label lblSwSideBg = null!;
    private Button swSideBg = null!;
    private Label lblSwSideBorder = null!;
    private Button swSideBorder = null!;
    private Label lblSwStatAtk = null!;
    private Button swStatAtk = null!;
    private Label lblSwStatDef = null!;
    private Button swStatDef = null!;
    private Label lblSwStatMuted = null!;
    private Button swStatMuted = null!;
    private Label lblSwStatusOk = null!;
    private Button swStatusOk = null!;
    private Label lblSecBUTTONS = null!;
    private Label lblSwGoldBtnBg = null!;
    private Button swGoldBtnBg = null!;
    private Label lblSwGoldBtnFg = null!;
    private Button swGoldBtnFg = null!;
    private Label lblSwGoldBtnHover = null!;
    private Button swGoldBtnHover = null!;
    private Label lblSwButtonNeutralBg = null!;
    private Button swButtonNeutralBg = null!;
    private Label lblSwButtonNeutralHover = null!;
    private Button swButtonNeutralHover = null!;
    private Label lblSwDangerFg = null!;
    private Button swDangerFg = null!;
    private Label lblSwDangerBorder = null!;
    private Button swDangerBorder = null!;
    private Label lblSwDangerHoverBg = null!;
    private Button swDangerHoverBg = null!;
    private Label lblSwAccentRedHover = null!;
    private Button swAccentRedHover = null!;
    private Label lblSecMISC = null!;
    private Label lblSwOverlayBackdrop = null!;
    private Button swOverlayBackdrop = null!;
    private Label lblSwPortraitBg = null!;
    private Button swPortraitBg = null!;
    private Label lblSwAccentDivider = null!;
    private Button swAccentDivider = null!;
    private Label lblSwOfflinePanelBg = null!;
    private Button swOfflinePanelBg = null!;

    // ── Footer ────────────────────────────────────────────────────────────────
    private CheckBox chkApplyNow = null!;
    private Button btnDelete = null!;
    private Button btnCancel = null!;
    private Button btnSave = null!;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThemeEditorForm));
        lblNameTitle = new Label();
        txtName = new TextBox();
        lblBaseTitle = new Label();
        cboBase = new ComboBox();
        chkIsDark = new CheckBox();
        lblBgTitle = new Label();
        txtBgPath = new TextBox();
        btnBrowseBg = new Button();
        btnClearBg = new Button();
        lblOpacityTitle = new Label();
        trkOpacity = new TrackBar();
        lblOpacityValue = new Label();
        pnlSwatches = new Panel();
        lblSecBACKGROUNDS = new Label();
        lblSwAppBg = new Label();
        swAppBg = new Button();
        lblSwSidebarBg = new Label();
        swSidebarBg = new Button();
        lblSwCardBg = new Label();
        swCardBg = new Button();
        lblSwCardHover = new Label();
        swCardHover = new Button();
        lblSwCardSelected = new Label();
        swCardSelected = new Button();
        lblSwTopbarBg = new Label();
        swTopbarBg = new Button();
        lblSwInputBg = new Label();
        swInputBg = new Button();
        lblSwActionBtnBg = new Label();
        swActionBtnBg = new Button();
        lblSecBORDERS = new Label();
        lblSwBorder = new Label();
        swBorder = new Button();
        lblSwBorderHover = new Label();
        swBorderHover = new Button();
        lblSwBorderSelected = new Label();
        swBorderSelected = new Button();
        lblSecTEXT = new Label();
        lblSwTextPrimary = new Label();
        swTextPrimary = new Button();
        lblSwTextSecondary = new Label();
        swTextSecondary = new Button();
        lblSwTextMuted = new Label();
        swTextMuted = new Button();
        lblSwTextDim = new Label();
        swTextDim = new Button();
        lblSwTextGold = new Label();
        swTextGold = new Button();
        lblSwTextNavActive = new Label();
        swTextNavActive = new Button();
        lblSwTextNav = new Label();
        swTextNav = new Button();
        lblSwTextInfo = new Label();
        swTextInfo = new Button();
        lblSwTextCardName = new Label();
        swTextCardName = new Button();
        lblSwTextDescription = new Label();
        swTextDescription = new Button();
        lblSecDECKSTATS = new Label();
        lblSwMainGreen = new Label();
        swMainGreen = new Button();
        lblSwMainBg = new Label();
        swMainBg = new Button();
        lblSwMainBorder = new Label();
        swMainBorder = new Button();
        lblSwExtraBlue = new Label();
        swExtraBlue = new Button();
        lblSwExtraBg = new Label();
        swExtraBg = new Button();
        lblSwExtraBorder = new Label();
        swExtraBorder = new Button();
        lblSwSideRed = new Label();
        swSideRed = new Button();
        lblSwSideBg = new Label();
        swSideBg = new Button();
        lblSwSideBorder = new Label();
        swSideBorder = new Button();
        lblSwStatAtk = new Label();
        swStatAtk = new Button();
        lblSwStatDef = new Label();
        swStatDef = new Button();
        lblSwStatMuted = new Label();
        swStatMuted = new Button();
        lblSwStatusOk = new Label();
        swStatusOk = new Button();
        lblSecBUTTONS = new Label();
        lblSwGoldBtnBg = new Label();
        swGoldBtnBg = new Button();
        lblSwGoldBtnFg = new Label();
        swGoldBtnFg = new Button();
        lblSwGoldBtnHover = new Label();
        swGoldBtnHover = new Button();
        lblSwButtonNeutralBg = new Label();
        swButtonNeutralBg = new Button();
        lblSwButtonNeutralHover = new Label();
        swButtonNeutralHover = new Button();
        lblSwDangerFg = new Label();
        swDangerFg = new Button();
        lblSwDangerBorder = new Label();
        swDangerBorder = new Button();
        lblSwDangerHoverBg = new Label();
        swDangerHoverBg = new Button();
        lblSwAccentRedHover = new Label();
        swAccentRedHover = new Button();
        lblSecMISC = new Label();
        lblSwOverlayBackdrop = new Label();
        swOverlayBackdrop = new Button();
        lblSwPortraitBg = new Label();
        swPortraitBg = new Button();
        lblSwAccentDivider = new Label();
        swAccentDivider = new Button();
        lblSwOfflinePanelBg = new Label();
        swOfflinePanelBg = new Button();
        chkApplyNow = new CheckBox();
        btnDelete = new Button();
        btnCancel = new Button();
        btnSave = new Button();
        ((System.ComponentModel.ISupportInitialize)trkOpacity).BeginInit();
        pnlSwatches.SuspendLayout();
        SuspendLayout();
        // 
        // lblNameTitle
        // 
        lblNameTitle.BackColor = Color.Transparent;
        lblNameTitle.Font = new Font("Segoe UI", 8F);
        lblNameTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblNameTitle.Location = new Point(20, 12);
        lblNameTitle.Name = "lblNameTitle";
        lblNameTitle.Size = new Size(300, 14);
        lblNameTitle.TabIndex = 0;
        lblNameTitle.Text = "THEME NAME";
        // 
        // txtName
        // 
        txtName.BackColor = Color.FromArgb(24, 24, 36);
        txtName.BorderStyle = BorderStyle.FixedSingle;
        txtName.ForeColor = Color.FromArgb(238, 238, 250);
        txtName.Location = new Point(20, 28);
        txtName.Name = "txtName";
        txtName.Size = new Size(300, 23);
        txtName.TabIndex = 1;
        // 
        // lblBaseTitle
        // 
        lblBaseTitle.BackColor = Color.Transparent;
        lblBaseTitle.Font = new Font("Segoe UI", 8F);
        lblBaseTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblBaseTitle.Location = new Point(340, 12);
        lblBaseTitle.Name = "lblBaseTitle";
        lblBaseTitle.Size = new Size(240, 14);
        lblBaseTitle.TabIndex = 2;
        lblBaseTitle.Text = "START FROM PRESET";
        // 
        // cboBase
        // 
        cboBase.BackColor = Color.FromArgb(24, 24, 36);
        cboBase.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBase.ForeColor = Color.FromArgb(238, 238, 250);
        cboBase.FormattingEnabled = true;
        cboBase.Location = new Point(340, 28);
        cboBase.Name = "cboBase";
        cboBase.Size = new Size(186, 23);
        cboBase.TabIndex = 3;
        cboBase.SelectedIndexChanged += cboBase_SelectedIndexChanged;
        // 
        // chkIsDark
        // 
        chkIsDark.BackColor = Color.Transparent;
        chkIsDark.ForeColor = Color.FromArgb(190, 190, 212);
        chkIsDark.Location = new Point(20, 60);
        chkIsDark.Name = "chkIsDark";
        chkIsDark.Size = new Size(300, 20);
        chkIsDark.TabIndex = 4;
        chkIsDark.Text = "Dark theme (affects title bar)";
        chkIsDark.UseVisualStyleBackColor = false;
        // 
        // lblBgTitle
        // 
        lblBgTitle.BackColor = Color.Transparent;
        lblBgTitle.Font = new Font("Segoe UI", 8F);
        lblBgTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblBgTitle.Location = new Point(20, 90);
        lblBgTitle.Name = "lblBgTitle";
        lblBgTitle.Size = new Size(300, 14);
        lblBgTitle.TabIndex = 5;
        lblBgTitle.Text = "BACKGROUND IMAGE (OPTIONAL)";
        // 
        // txtBgPath
        // 
        txtBgPath.BackColor = Color.FromArgb(24, 24, 36);
        txtBgPath.BorderStyle = BorderStyle.FixedSingle;
        txtBgPath.ForeColor = Color.FromArgb(238, 238, 250);
        txtBgPath.Location = new Point(20, 106);
        txtBgPath.Name = "txtBgPath";
        txtBgPath.PlaceholderText = "None";
        txtBgPath.ReadOnly = true;
        txtBgPath.Size = new Size(360, 23);
        txtBgPath.TabIndex = 6;
        // 
        // btnBrowseBg
        // 
        btnBrowseBg.BackColor = Color.Transparent;
        btnBrowseBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnBrowseBg.FlatStyle = FlatStyle.Flat;
        btnBrowseBg.ForeColor = Color.FromArgb(190, 190, 212);
        btnBrowseBg.Location = new Point(386, 105);
        btnBrowseBg.Name = "btnBrowseBg";
        btnBrowseBg.Size = new Size(90, 25);
        btnBrowseBg.TabIndex = 7;
        btnBrowseBg.Text = "Browse...";
        btnBrowseBg.UseVisualStyleBackColor = false;
        btnBrowseBg.Click += btnBrowseBg_Click;
        // 
        // btnClearBg
        // 
        btnClearBg.BackColor = Color.Transparent;
        btnClearBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnClearBg.FlatStyle = FlatStyle.Flat;
        btnClearBg.ForeColor = Color.FromArgb(138, 138, 164);
        btnClearBg.Location = new Point(482, 105);
        btnClearBg.Name = "btnClearBg";
        btnClearBg.Size = new Size(64, 25);
        btnClearBg.TabIndex = 8;
        btnClearBg.Text = "Clear";
        btnClearBg.UseVisualStyleBackColor = false;
        btnClearBg.Click += btnClearBg_Click;
        // 
        // lblOpacityTitle
        // 
        lblOpacityTitle.BackColor = Color.Transparent;
        lblOpacityTitle.Font = new Font("Segoe UI", 8F);
        lblOpacityTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblOpacityTitle.Location = new Point(20, 138);
        lblOpacityTitle.Name = "lblOpacityTitle";
        lblOpacityTitle.Size = new Size(400, 14);
        lblOpacityTitle.TabIndex = 9;
        lblOpacityTitle.Text = "OVERLAY TINT (HIGHER = IMAGE LESS VISIBLE)";
        // 
        // trkOpacity
        // 
        trkOpacity.BackColor = Color.FromArgb(13, 13, 22);
        trkOpacity.Location = new Point(20, 154);
        trkOpacity.Maximum = 100;
        trkOpacity.Name = "trkOpacity";
        trkOpacity.Size = new Size(300, 45);
        trkOpacity.TabIndex = 10;
        trkOpacity.TickFrequency = 10;
        trkOpacity.Scroll += trkOpacity_Scroll;
        // 
        // lblOpacityValue
        // 
        lblOpacityValue.BackColor = Color.Transparent;
        lblOpacityValue.Font = new Font("Segoe UI", 8.5F);
        lblOpacityValue.ForeColor = Color.FromArgb(190, 190, 212);
        lblOpacityValue.Location = new Point(326, 162);
        lblOpacityValue.Name = "lblOpacityValue";
        lblOpacityValue.Size = new Size(60, 18);
        lblOpacityValue.TabIndex = 11;
        lblOpacityValue.Text = "55%";
        // 
        // pnlSwatches
        // 
        pnlSwatches.AutoScroll = true;
        pnlSwatches.BackColor = Color.FromArgb(28, 28, 42);
        pnlSwatches.BorderStyle = BorderStyle.FixedSingle;
        pnlSwatches.Controls.Add(lblSecBACKGROUNDS);
        pnlSwatches.Controls.Add(lblSwAppBg);
        pnlSwatches.Controls.Add(swAppBg);
        pnlSwatches.Controls.Add(lblSwSidebarBg);
        pnlSwatches.Controls.Add(swSidebarBg);
        pnlSwatches.Controls.Add(lblSwCardBg);
        pnlSwatches.Controls.Add(swCardBg);
        pnlSwatches.Controls.Add(lblSwCardHover);
        pnlSwatches.Controls.Add(swCardHover);
        pnlSwatches.Controls.Add(lblSwCardSelected);
        pnlSwatches.Controls.Add(swCardSelected);
        pnlSwatches.Controls.Add(lblSwTopbarBg);
        pnlSwatches.Controls.Add(swTopbarBg);
        pnlSwatches.Controls.Add(lblSwInputBg);
        pnlSwatches.Controls.Add(swInputBg);
        pnlSwatches.Controls.Add(lblSwActionBtnBg);
        pnlSwatches.Controls.Add(swActionBtnBg);
        pnlSwatches.Controls.Add(lblSecBORDERS);
        pnlSwatches.Controls.Add(lblSwBorder);
        pnlSwatches.Controls.Add(swBorder);
        pnlSwatches.Controls.Add(lblSwBorderHover);
        pnlSwatches.Controls.Add(swBorderHover);
        pnlSwatches.Controls.Add(lblSwBorderSelected);
        pnlSwatches.Controls.Add(swBorderSelected);
        pnlSwatches.Controls.Add(lblSecTEXT);
        pnlSwatches.Controls.Add(lblSwTextPrimary);
        pnlSwatches.Controls.Add(swTextPrimary);
        pnlSwatches.Controls.Add(lblSwTextSecondary);
        pnlSwatches.Controls.Add(swTextSecondary);
        pnlSwatches.Controls.Add(lblSwTextMuted);
        pnlSwatches.Controls.Add(swTextMuted);
        pnlSwatches.Controls.Add(lblSwTextDim);
        pnlSwatches.Controls.Add(swTextDim);
        pnlSwatches.Controls.Add(lblSwTextGold);
        pnlSwatches.Controls.Add(swTextGold);
        pnlSwatches.Controls.Add(lblSwTextNavActive);
        pnlSwatches.Controls.Add(swTextNavActive);
        pnlSwatches.Controls.Add(lblSwTextNav);
        pnlSwatches.Controls.Add(swTextNav);
        pnlSwatches.Controls.Add(lblSwTextInfo);
        pnlSwatches.Controls.Add(swTextInfo);
        pnlSwatches.Controls.Add(lblSwTextCardName);
        pnlSwatches.Controls.Add(swTextCardName);
        pnlSwatches.Controls.Add(lblSwTextDescription);
        pnlSwatches.Controls.Add(swTextDescription);
        pnlSwatches.Controls.Add(lblSecDECKSTATS);
        pnlSwatches.Controls.Add(lblSwMainGreen);
        pnlSwatches.Controls.Add(swMainGreen);
        pnlSwatches.Controls.Add(lblSwMainBg);
        pnlSwatches.Controls.Add(swMainBg);
        pnlSwatches.Controls.Add(lblSwMainBorder);
        pnlSwatches.Controls.Add(swMainBorder);
        pnlSwatches.Controls.Add(lblSwExtraBlue);
        pnlSwatches.Controls.Add(swExtraBlue);
        pnlSwatches.Controls.Add(lblSwExtraBg);
        pnlSwatches.Controls.Add(swExtraBg);
        pnlSwatches.Controls.Add(lblSwExtraBorder);
        pnlSwatches.Controls.Add(swExtraBorder);
        pnlSwatches.Controls.Add(lblSwSideRed);
        pnlSwatches.Controls.Add(swSideRed);
        pnlSwatches.Controls.Add(lblSwSideBg);
        pnlSwatches.Controls.Add(swSideBg);
        pnlSwatches.Controls.Add(lblSwSideBorder);
        pnlSwatches.Controls.Add(swSideBorder);
        pnlSwatches.Controls.Add(lblSwStatAtk);
        pnlSwatches.Controls.Add(swStatAtk);
        pnlSwatches.Controls.Add(lblSwStatDef);
        pnlSwatches.Controls.Add(swStatDef);
        pnlSwatches.Controls.Add(lblSwStatMuted);
        pnlSwatches.Controls.Add(swStatMuted);
        pnlSwatches.Controls.Add(lblSwStatusOk);
        pnlSwatches.Controls.Add(swStatusOk);
        pnlSwatches.Controls.Add(lblSecBUTTONS);
        pnlSwatches.Controls.Add(lblSwGoldBtnBg);
        pnlSwatches.Controls.Add(swGoldBtnBg);
        pnlSwatches.Controls.Add(lblSwGoldBtnFg);
        pnlSwatches.Controls.Add(swGoldBtnFg);
        pnlSwatches.Controls.Add(lblSwGoldBtnHover);
        pnlSwatches.Controls.Add(swGoldBtnHover);
        pnlSwatches.Controls.Add(lblSwButtonNeutralBg);
        pnlSwatches.Controls.Add(swButtonNeutralBg);
        pnlSwatches.Controls.Add(lblSwButtonNeutralHover);
        pnlSwatches.Controls.Add(swButtonNeutralHover);
        pnlSwatches.Controls.Add(lblSwDangerFg);
        pnlSwatches.Controls.Add(swDangerFg);
        pnlSwatches.Controls.Add(lblSwDangerBorder);
        pnlSwatches.Controls.Add(swDangerBorder);
        pnlSwatches.Controls.Add(lblSwDangerHoverBg);
        pnlSwatches.Controls.Add(swDangerHoverBg);
        pnlSwatches.Controls.Add(lblSwAccentRedHover);
        pnlSwatches.Controls.Add(swAccentRedHover);
        pnlSwatches.Controls.Add(lblSecMISC);
        pnlSwatches.Controls.Add(lblSwOverlayBackdrop);
        pnlSwatches.Controls.Add(swOverlayBackdrop);
        pnlSwatches.Controls.Add(lblSwPortraitBg);
        pnlSwatches.Controls.Add(swPortraitBg);
        pnlSwatches.Controls.Add(lblSwAccentDivider);
        pnlSwatches.Controls.Add(swAccentDivider);
        pnlSwatches.Controls.Add(lblSwOfflinePanelBg);
        pnlSwatches.Controls.Add(swOfflinePanelBg);
        pnlSwatches.Location = new Point(20, 206);
        pnlSwatches.Name = "pnlSwatches";
        pnlSwatches.Size = new Size(560, 400);
        pnlSwatches.TabIndex = 12;
        // 
        // lblSecBACKGROUNDS
        // 
        lblSecBACKGROUNDS.BackColor = Color.Transparent;
        lblSecBACKGROUNDS.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSecBACKGROUNDS.ForeColor = Color.FromArgb(220, 184, 84);
        lblSecBACKGROUNDS.Location = new Point(8, 8);
        lblSecBACKGROUNDS.Name = "lblSecBACKGROUNDS";
        lblSecBACKGROUNDS.Size = new Size(540, 24);
        lblSecBACKGROUNDS.TabIndex = 0;
        lblSecBACKGROUNDS.Text = "BACKGROUNDS";
        // 
        // lblSwAppBg
        // 
        lblSwAppBg.BackColor = Color.Transparent;
        lblSwAppBg.Font = new Font("Segoe UI", 8.5F);
        lblSwAppBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwAppBg.Location = new Point(20, 35);
        lblSwAppBg.Name = "lblSwAppBg";
        lblSwAppBg.Size = new Size(320, 18);
        lblSwAppBg.TabIndex = 1;
        lblSwAppBg.Text = "App Background";
        // 
        // swAppBg
        // 
        swAppBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swAppBg.FlatStyle = FlatStyle.Flat;
        swAppBg.Location = new Point(456, 32);
        swAppBg.Name = "swAppBg";
        swAppBg.Size = new Size(70, 22);
        swAppBg.TabIndex = 2;
        swAppBg.Tag = "AppBg";
        swAppBg.UseVisualStyleBackColor = false;
        swAppBg.Click += swatch_Click;
        // 
        // lblSwSidebarBg
        // 
        lblSwSidebarBg.BackColor = Color.Transparent;
        lblSwSidebarBg.Font = new Font("Segoe UI", 8.5F);
        lblSwSidebarBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwSidebarBg.Location = new Point(20, 61);
        lblSwSidebarBg.Name = "lblSwSidebarBg";
        lblSwSidebarBg.Size = new Size(320, 18);
        lblSwSidebarBg.TabIndex = 3;
        lblSwSidebarBg.Text = "Sidebar Background";
        // 
        // swSidebarBg
        // 
        swSidebarBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swSidebarBg.FlatStyle = FlatStyle.Flat;
        swSidebarBg.Location = new Point(456, 58);
        swSidebarBg.Name = "swSidebarBg";
        swSidebarBg.Size = new Size(70, 22);
        swSidebarBg.TabIndex = 4;
        swSidebarBg.Tag = "SidebarBg";
        swSidebarBg.UseVisualStyleBackColor = false;
        swSidebarBg.Click += swatch_Click;
        // 
        // lblSwCardBg
        // 
        lblSwCardBg.BackColor = Color.Transparent;
        lblSwCardBg.Font = new Font("Segoe UI", 8.5F);
        lblSwCardBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwCardBg.Location = new Point(20, 87);
        lblSwCardBg.Name = "lblSwCardBg";
        lblSwCardBg.Size = new Size(320, 18);
        lblSwCardBg.TabIndex = 5;
        lblSwCardBg.Text = "Card Background";
        // 
        // swCardBg
        // 
        swCardBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swCardBg.FlatStyle = FlatStyle.Flat;
        swCardBg.Location = new Point(456, 84);
        swCardBg.Name = "swCardBg";
        swCardBg.Size = new Size(70, 22);
        swCardBg.TabIndex = 6;
        swCardBg.Tag = "CardBg";
        swCardBg.UseVisualStyleBackColor = false;
        swCardBg.Click += swatch_Click;
        // 
        // lblSwCardHover
        // 
        lblSwCardHover.BackColor = Color.Transparent;
        lblSwCardHover.Font = new Font("Segoe UI", 8.5F);
        lblSwCardHover.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwCardHover.Location = new Point(20, 113);
        lblSwCardHover.Name = "lblSwCardHover";
        lblSwCardHover.Size = new Size(320, 18);
        lblSwCardHover.TabIndex = 7;
        lblSwCardHover.Text = "Card Hover";
        // 
        // swCardHover
        // 
        swCardHover.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swCardHover.FlatStyle = FlatStyle.Flat;
        swCardHover.Location = new Point(456, 110);
        swCardHover.Name = "swCardHover";
        swCardHover.Size = new Size(70, 22);
        swCardHover.TabIndex = 8;
        swCardHover.Tag = "CardHover";
        swCardHover.UseVisualStyleBackColor = false;
        swCardHover.Click += swatch_Click;
        // 
        // lblSwCardSelected
        // 
        lblSwCardSelected.BackColor = Color.Transparent;
        lblSwCardSelected.Font = new Font("Segoe UI", 8.5F);
        lblSwCardSelected.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwCardSelected.Location = new Point(20, 139);
        lblSwCardSelected.Name = "lblSwCardSelected";
        lblSwCardSelected.Size = new Size(320, 18);
        lblSwCardSelected.TabIndex = 9;
        lblSwCardSelected.Text = "Card Selected";
        // 
        // swCardSelected
        // 
        swCardSelected.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swCardSelected.FlatStyle = FlatStyle.Flat;
        swCardSelected.Location = new Point(456, 136);
        swCardSelected.Name = "swCardSelected";
        swCardSelected.Size = new Size(70, 22);
        swCardSelected.TabIndex = 10;
        swCardSelected.Tag = "CardSelected";
        swCardSelected.UseVisualStyleBackColor = false;
        swCardSelected.Click += swatch_Click;
        // 
        // lblSwTopbarBg
        // 
        lblSwTopbarBg.BackColor = Color.Transparent;
        lblSwTopbarBg.Font = new Font("Segoe UI", 8.5F);
        lblSwTopbarBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTopbarBg.Location = new Point(20, 165);
        lblSwTopbarBg.Name = "lblSwTopbarBg";
        lblSwTopbarBg.Size = new Size(320, 18);
        lblSwTopbarBg.TabIndex = 11;
        lblSwTopbarBg.Text = "Topbar Background";
        // 
        // swTopbarBg
        // 
        swTopbarBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTopbarBg.FlatStyle = FlatStyle.Flat;
        swTopbarBg.Location = new Point(456, 162);
        swTopbarBg.Name = "swTopbarBg";
        swTopbarBg.Size = new Size(70, 22);
        swTopbarBg.TabIndex = 12;
        swTopbarBg.Tag = "TopbarBg";
        swTopbarBg.UseVisualStyleBackColor = false;
        swTopbarBg.Click += swatch_Click;
        // 
        // lblSwInputBg
        // 
        lblSwInputBg.BackColor = Color.Transparent;
        lblSwInputBg.Font = new Font("Segoe UI", 8.5F);
        lblSwInputBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwInputBg.Location = new Point(20, 191);
        lblSwInputBg.Name = "lblSwInputBg";
        lblSwInputBg.Size = new Size(320, 18);
        lblSwInputBg.TabIndex = 13;
        lblSwInputBg.Text = "Input Background";
        // 
        // swInputBg
        // 
        swInputBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swInputBg.FlatStyle = FlatStyle.Flat;
        swInputBg.Location = new Point(456, 188);
        swInputBg.Name = "swInputBg";
        swInputBg.Size = new Size(70, 22);
        swInputBg.TabIndex = 14;
        swInputBg.Tag = "InputBg";
        swInputBg.UseVisualStyleBackColor = false;
        swInputBg.Click += swatch_Click;
        // 
        // lblSwActionBtnBg
        // 
        lblSwActionBtnBg.BackColor = Color.Transparent;
        lblSwActionBtnBg.Font = new Font("Segoe UI", 8.5F);
        lblSwActionBtnBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwActionBtnBg.Location = new Point(20, 217);
        lblSwActionBtnBg.Name = "lblSwActionBtnBg";
        lblSwActionBtnBg.Size = new Size(320, 18);
        lblSwActionBtnBg.TabIndex = 15;
        lblSwActionBtnBg.Text = "Action Button Background";
        // 
        // swActionBtnBg
        // 
        swActionBtnBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swActionBtnBg.FlatStyle = FlatStyle.Flat;
        swActionBtnBg.Location = new Point(456, 214);
        swActionBtnBg.Name = "swActionBtnBg";
        swActionBtnBg.Size = new Size(70, 22);
        swActionBtnBg.TabIndex = 16;
        swActionBtnBg.Tag = "ActionBtnBg";
        swActionBtnBg.UseVisualStyleBackColor = false;
        swActionBtnBg.Click += swatch_Click;
        // 
        // lblSecBORDERS
        // 
        lblSecBORDERS.BackColor = Color.Transparent;
        lblSecBORDERS.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSecBORDERS.ForeColor = Color.FromArgb(220, 184, 84);
        lblSecBORDERS.Location = new Point(8, 250);
        lblSecBORDERS.Name = "lblSecBORDERS";
        lblSecBORDERS.Size = new Size(540, 24);
        lblSecBORDERS.TabIndex = 17;
        lblSecBORDERS.Text = "BORDERS";
        // 
        // lblSwBorder
        // 
        lblSwBorder.BackColor = Color.Transparent;
        lblSwBorder.Font = new Font("Segoe UI", 8.5F);
        lblSwBorder.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwBorder.Location = new Point(20, 277);
        lblSwBorder.Name = "lblSwBorder";
        lblSwBorder.Size = new Size(320, 18);
        lblSwBorder.TabIndex = 18;
        lblSwBorder.Text = "Border";
        // 
        // swBorder
        // 
        swBorder.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swBorder.FlatStyle = FlatStyle.Flat;
        swBorder.Location = new Point(456, 274);
        swBorder.Name = "swBorder";
        swBorder.Size = new Size(70, 22);
        swBorder.TabIndex = 19;
        swBorder.Tag = "Border";
        swBorder.UseVisualStyleBackColor = false;
        swBorder.Click += swatch_Click;
        // 
        // lblSwBorderHover
        // 
        lblSwBorderHover.BackColor = Color.Transparent;
        lblSwBorderHover.Font = new Font("Segoe UI", 8.5F);
        lblSwBorderHover.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwBorderHover.Location = new Point(20, 303);
        lblSwBorderHover.Name = "lblSwBorderHover";
        lblSwBorderHover.Size = new Size(320, 18);
        lblSwBorderHover.TabIndex = 20;
        lblSwBorderHover.Text = "Border Hover";
        // 
        // swBorderHover
        // 
        swBorderHover.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swBorderHover.FlatStyle = FlatStyle.Flat;
        swBorderHover.Location = new Point(456, 300);
        swBorderHover.Name = "swBorderHover";
        swBorderHover.Size = new Size(70, 22);
        swBorderHover.TabIndex = 21;
        swBorderHover.Tag = "BorderHover";
        swBorderHover.UseVisualStyleBackColor = false;
        swBorderHover.Click += swatch_Click;
        // 
        // lblSwBorderSelected
        // 
        lblSwBorderSelected.BackColor = Color.Transparent;
        lblSwBorderSelected.Font = new Font("Segoe UI", 8.5F);
        lblSwBorderSelected.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwBorderSelected.Location = new Point(20, 329);
        lblSwBorderSelected.Name = "lblSwBorderSelected";
        lblSwBorderSelected.Size = new Size(320, 18);
        lblSwBorderSelected.TabIndex = 22;
        lblSwBorderSelected.Text = "Border Selected";
        // 
        // swBorderSelected
        // 
        swBorderSelected.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swBorderSelected.FlatStyle = FlatStyle.Flat;
        swBorderSelected.Location = new Point(456, 326);
        swBorderSelected.Name = "swBorderSelected";
        swBorderSelected.Size = new Size(70, 22);
        swBorderSelected.TabIndex = 23;
        swBorderSelected.Tag = "BorderSelected";
        swBorderSelected.UseVisualStyleBackColor = false;
        swBorderSelected.Click += swatch_Click;
        // 
        // lblSecTEXT
        // 
        lblSecTEXT.BackColor = Color.Transparent;
        lblSecTEXT.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSecTEXT.ForeColor = Color.FromArgb(220, 184, 84);
        lblSecTEXT.Location = new Point(8, 362);
        lblSecTEXT.Name = "lblSecTEXT";
        lblSecTEXT.Size = new Size(540, 24);
        lblSecTEXT.TabIndex = 24;
        lblSecTEXT.Text = "TEXT";
        // 
        // lblSwTextPrimary
        // 
        lblSwTextPrimary.BackColor = Color.Transparent;
        lblSwTextPrimary.Font = new Font("Segoe UI", 8.5F);
        lblSwTextPrimary.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextPrimary.Location = new Point(20, 389);
        lblSwTextPrimary.Name = "lblSwTextPrimary";
        lblSwTextPrimary.Size = new Size(320, 18);
        lblSwTextPrimary.TabIndex = 25;
        lblSwTextPrimary.Text = "Primary Text";
        // 
        // swTextPrimary
        // 
        swTextPrimary.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextPrimary.FlatStyle = FlatStyle.Flat;
        swTextPrimary.Location = new Point(456, 386);
        swTextPrimary.Name = "swTextPrimary";
        swTextPrimary.Size = new Size(70, 22);
        swTextPrimary.TabIndex = 26;
        swTextPrimary.Tag = "TextPrimary";
        swTextPrimary.UseVisualStyleBackColor = false;
        swTextPrimary.Click += swatch_Click;
        // 
        // lblSwTextSecondary
        // 
        lblSwTextSecondary.BackColor = Color.Transparent;
        lblSwTextSecondary.Font = new Font("Segoe UI", 8.5F);
        lblSwTextSecondary.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextSecondary.Location = new Point(20, 415);
        lblSwTextSecondary.Name = "lblSwTextSecondary";
        lblSwTextSecondary.Size = new Size(320, 18);
        lblSwTextSecondary.TabIndex = 27;
        lblSwTextSecondary.Text = "Secondary Text";
        // 
        // swTextSecondary
        // 
        swTextSecondary.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextSecondary.FlatStyle = FlatStyle.Flat;
        swTextSecondary.Location = new Point(456, 412);
        swTextSecondary.Name = "swTextSecondary";
        swTextSecondary.Size = new Size(70, 22);
        swTextSecondary.TabIndex = 28;
        swTextSecondary.Tag = "TextSecondary";
        swTextSecondary.UseVisualStyleBackColor = false;
        swTextSecondary.Click += swatch_Click;
        // 
        // lblSwTextMuted
        // 
        lblSwTextMuted.BackColor = Color.Transparent;
        lblSwTextMuted.Font = new Font("Segoe UI", 8.5F);
        lblSwTextMuted.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextMuted.Location = new Point(20, 441);
        lblSwTextMuted.Name = "lblSwTextMuted";
        lblSwTextMuted.Size = new Size(320, 18);
        lblSwTextMuted.TabIndex = 29;
        lblSwTextMuted.Text = "Muted Text";
        // 
        // swTextMuted
        // 
        swTextMuted.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextMuted.FlatStyle = FlatStyle.Flat;
        swTextMuted.Location = new Point(456, 438);
        swTextMuted.Name = "swTextMuted";
        swTextMuted.Size = new Size(70, 22);
        swTextMuted.TabIndex = 30;
        swTextMuted.Tag = "TextMuted";
        swTextMuted.UseVisualStyleBackColor = false;
        swTextMuted.Click += swatch_Click;
        // 
        // lblSwTextDim
        // 
        lblSwTextDim.BackColor = Color.Transparent;
        lblSwTextDim.Font = new Font("Segoe UI", 8.5F);
        lblSwTextDim.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextDim.Location = new Point(20, 467);
        lblSwTextDim.Name = "lblSwTextDim";
        lblSwTextDim.Size = new Size(320, 18);
        lblSwTextDim.TabIndex = 31;
        lblSwTextDim.Text = "Dim Text";
        // 
        // swTextDim
        // 
        swTextDim.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextDim.FlatStyle = FlatStyle.Flat;
        swTextDim.Location = new Point(456, 464);
        swTextDim.Name = "swTextDim";
        swTextDim.Size = new Size(70, 22);
        swTextDim.TabIndex = 32;
        swTextDim.Tag = "TextDim";
        swTextDim.UseVisualStyleBackColor = false;
        swTextDim.Click += swatch_Click;
        // 
        // lblSwTextGold
        // 
        lblSwTextGold.BackColor = Color.Transparent;
        lblSwTextGold.Font = new Font("Segoe UI", 8.5F);
        lblSwTextGold.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextGold.Location = new Point(20, 493);
        lblSwTextGold.Name = "lblSwTextGold";
        lblSwTextGold.Size = new Size(320, 18);
        lblSwTextGold.TabIndex = 33;
        lblSwTextGold.Text = "Gold Text";
        // 
        // swTextGold
        // 
        swTextGold.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextGold.FlatStyle = FlatStyle.Flat;
        swTextGold.Location = new Point(456, 490);
        swTextGold.Name = "swTextGold";
        swTextGold.Size = new Size(70, 22);
        swTextGold.TabIndex = 34;
        swTextGold.Tag = "TextGold";
        swTextGold.UseVisualStyleBackColor = false;
        swTextGold.Click += swatch_Click;
        // 
        // lblSwTextNavActive
        // 
        lblSwTextNavActive.BackColor = Color.Transparent;
        lblSwTextNavActive.Font = new Font("Segoe UI", 8.5F);
        lblSwTextNavActive.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextNavActive.Location = new Point(20, 519);
        lblSwTextNavActive.Name = "lblSwTextNavActive";
        lblSwTextNavActive.Size = new Size(320, 18);
        lblSwTextNavActive.TabIndex = 35;
        lblSwTextNavActive.Text = "Active Nav Text";
        // 
        // swTextNavActive
        // 
        swTextNavActive.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextNavActive.FlatStyle = FlatStyle.Flat;
        swTextNavActive.Location = new Point(456, 516);
        swTextNavActive.Name = "swTextNavActive";
        swTextNavActive.Size = new Size(70, 22);
        swTextNavActive.TabIndex = 36;
        swTextNavActive.Tag = "TextNavActive";
        swTextNavActive.UseVisualStyleBackColor = false;
        swTextNavActive.Click += swatch_Click;
        // 
        // lblSwTextNav
        // 
        lblSwTextNav.BackColor = Color.Transparent;
        lblSwTextNav.Font = new Font("Segoe UI", 8.5F);
        lblSwTextNav.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextNav.Location = new Point(20, 545);
        lblSwTextNav.Name = "lblSwTextNav";
        lblSwTextNav.Size = new Size(320, 18);
        lblSwTextNav.TabIndex = 37;
        lblSwTextNav.Text = "Nav Text";
        // 
        // swTextNav
        // 
        swTextNav.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextNav.FlatStyle = FlatStyle.Flat;
        swTextNav.Location = new Point(456, 542);
        swTextNav.Name = "swTextNav";
        swTextNav.Size = new Size(70, 22);
        swTextNav.TabIndex = 38;
        swTextNav.Tag = "TextNav";
        swTextNav.UseVisualStyleBackColor = false;
        swTextNav.Click += swatch_Click;
        // 
        // lblSwTextInfo
        // 
        lblSwTextInfo.BackColor = Color.Transparent;
        lblSwTextInfo.Font = new Font("Segoe UI", 8.5F);
        lblSwTextInfo.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextInfo.Location = new Point(20, 571);
        lblSwTextInfo.Name = "lblSwTextInfo";
        lblSwTextInfo.Size = new Size(320, 18);
        lblSwTextInfo.TabIndex = 39;
        lblSwTextInfo.Text = "Info Text";
        // 
        // swTextInfo
        // 
        swTextInfo.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextInfo.FlatStyle = FlatStyle.Flat;
        swTextInfo.Location = new Point(456, 568);
        swTextInfo.Name = "swTextInfo";
        swTextInfo.Size = new Size(70, 22);
        swTextInfo.TabIndex = 40;
        swTextInfo.Tag = "TextInfo";
        swTextInfo.UseVisualStyleBackColor = false;
        swTextInfo.Click += swatch_Click;
        // 
        // lblSwTextCardName
        // 
        lblSwTextCardName.BackColor = Color.Transparent;
        lblSwTextCardName.Font = new Font("Segoe UI", 8.5F);
        lblSwTextCardName.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextCardName.Location = new Point(20, 597);
        lblSwTextCardName.Name = "lblSwTextCardName";
        lblSwTextCardName.Size = new Size(320, 18);
        lblSwTextCardName.TabIndex = 41;
        lblSwTextCardName.Text = "Card Name Text";
        // 
        // swTextCardName
        // 
        swTextCardName.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextCardName.FlatStyle = FlatStyle.Flat;
        swTextCardName.Location = new Point(456, 594);
        swTextCardName.Name = "swTextCardName";
        swTextCardName.Size = new Size(70, 22);
        swTextCardName.TabIndex = 42;
        swTextCardName.Tag = "TextCardName";
        swTextCardName.UseVisualStyleBackColor = false;
        swTextCardName.Click += swatch_Click;
        // 
        // lblSwTextDescription
        // 
        lblSwTextDescription.BackColor = Color.Transparent;
        lblSwTextDescription.Font = new Font("Segoe UI", 8.5F);
        lblSwTextDescription.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwTextDescription.Location = new Point(20, 623);
        lblSwTextDescription.Name = "lblSwTextDescription";
        lblSwTextDescription.Size = new Size(320, 18);
        lblSwTextDescription.TabIndex = 43;
        lblSwTextDescription.Text = "Description Text";
        // 
        // swTextDescription
        // 
        swTextDescription.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swTextDescription.FlatStyle = FlatStyle.Flat;
        swTextDescription.Location = new Point(456, 620);
        swTextDescription.Name = "swTextDescription";
        swTextDescription.Size = new Size(70, 22);
        swTextDescription.TabIndex = 44;
        swTextDescription.Tag = "TextDescription";
        swTextDescription.UseVisualStyleBackColor = false;
        swTextDescription.Click += swatch_Click;
        // 
        // lblSecDECKSTATS
        // 
        lblSecDECKSTATS.BackColor = Color.Transparent;
        lblSecDECKSTATS.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSecDECKSTATS.ForeColor = Color.FromArgb(220, 184, 84);
        lblSecDECKSTATS.Location = new Point(8, 656);
        lblSecDECKSTATS.Name = "lblSecDECKSTATS";
        lblSecDECKSTATS.Size = new Size(540, 24);
        lblSecDECKSTATS.TabIndex = 45;
        lblSecDECKSTATS.Text = "DECK & STATS";
        // 
        // lblSwMainGreen
        // 
        lblSwMainGreen.BackColor = Color.Transparent;
        lblSwMainGreen.Font = new Font("Segoe UI", 8.5F);
        lblSwMainGreen.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwMainGreen.Location = new Point(20, 683);
        lblSwMainGreen.Name = "lblSwMainGreen";
        lblSwMainGreen.Size = new Size(320, 18);
        lblSwMainGreen.TabIndex = 46;
        lblSwMainGreen.Text = "Main Deck Accent";
        // 
        // swMainGreen
        // 
        swMainGreen.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swMainGreen.FlatStyle = FlatStyle.Flat;
        swMainGreen.Location = new Point(456, 680);
        swMainGreen.Name = "swMainGreen";
        swMainGreen.Size = new Size(70, 22);
        swMainGreen.TabIndex = 47;
        swMainGreen.Tag = "MainGreen";
        swMainGreen.UseVisualStyleBackColor = false;
        swMainGreen.Click += swatch_Click;
        // 
        // lblSwMainBg
        // 
        lblSwMainBg.BackColor = Color.Transparent;
        lblSwMainBg.Font = new Font("Segoe UI", 8.5F);
        lblSwMainBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwMainBg.Location = new Point(20, 709);
        lblSwMainBg.Name = "lblSwMainBg";
        lblSwMainBg.Size = new Size(320, 18);
        lblSwMainBg.TabIndex = 48;
        lblSwMainBg.Text = "Main Deck Background";
        // 
        // swMainBg
        // 
        swMainBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swMainBg.FlatStyle = FlatStyle.Flat;
        swMainBg.Location = new Point(456, 706);
        swMainBg.Name = "swMainBg";
        swMainBg.Size = new Size(70, 22);
        swMainBg.TabIndex = 49;
        swMainBg.Tag = "MainBg";
        swMainBg.UseVisualStyleBackColor = false;
        swMainBg.Click += swatch_Click;
        // 
        // lblSwMainBorder
        // 
        lblSwMainBorder.BackColor = Color.Transparent;
        lblSwMainBorder.Font = new Font("Segoe UI", 8.5F);
        lblSwMainBorder.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwMainBorder.Location = new Point(20, 735);
        lblSwMainBorder.Name = "lblSwMainBorder";
        lblSwMainBorder.Size = new Size(320, 18);
        lblSwMainBorder.TabIndex = 50;
        lblSwMainBorder.Text = "Main Deck Border";
        // 
        // swMainBorder
        // 
        swMainBorder.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swMainBorder.FlatStyle = FlatStyle.Flat;
        swMainBorder.Location = new Point(456, 732);
        swMainBorder.Name = "swMainBorder";
        swMainBorder.Size = new Size(70, 22);
        swMainBorder.TabIndex = 51;
        swMainBorder.Tag = "MainBorder";
        swMainBorder.UseVisualStyleBackColor = false;
        swMainBorder.Click += swatch_Click;
        // 
        // lblSwExtraBlue
        // 
        lblSwExtraBlue.BackColor = Color.Transparent;
        lblSwExtraBlue.Font = new Font("Segoe UI", 8.5F);
        lblSwExtraBlue.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwExtraBlue.Location = new Point(20, 761);
        lblSwExtraBlue.Name = "lblSwExtraBlue";
        lblSwExtraBlue.Size = new Size(320, 18);
        lblSwExtraBlue.TabIndex = 52;
        lblSwExtraBlue.Text = "Extra Deck Accent";
        // 
        // swExtraBlue
        // 
        swExtraBlue.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swExtraBlue.FlatStyle = FlatStyle.Flat;
        swExtraBlue.Location = new Point(456, 758);
        swExtraBlue.Name = "swExtraBlue";
        swExtraBlue.Size = new Size(70, 22);
        swExtraBlue.TabIndex = 53;
        swExtraBlue.Tag = "ExtraBlue";
        swExtraBlue.UseVisualStyleBackColor = false;
        swExtraBlue.Click += swatch_Click;
        // 
        // lblSwExtraBg
        // 
        lblSwExtraBg.BackColor = Color.Transparent;
        lblSwExtraBg.Font = new Font("Segoe UI", 8.5F);
        lblSwExtraBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwExtraBg.Location = new Point(20, 787);
        lblSwExtraBg.Name = "lblSwExtraBg";
        lblSwExtraBg.Size = new Size(320, 18);
        lblSwExtraBg.TabIndex = 54;
        lblSwExtraBg.Text = "Extra Deck Background";
        // 
        // swExtraBg
        // 
        swExtraBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swExtraBg.FlatStyle = FlatStyle.Flat;
        swExtraBg.Location = new Point(456, 784);
        swExtraBg.Name = "swExtraBg";
        swExtraBg.Size = new Size(70, 22);
        swExtraBg.TabIndex = 55;
        swExtraBg.Tag = "ExtraBg";
        swExtraBg.UseVisualStyleBackColor = false;
        swExtraBg.Click += swatch_Click;
        // 
        // lblSwExtraBorder
        // 
        lblSwExtraBorder.BackColor = Color.Transparent;
        lblSwExtraBorder.Font = new Font("Segoe UI", 8.5F);
        lblSwExtraBorder.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwExtraBorder.Location = new Point(20, 813);
        lblSwExtraBorder.Name = "lblSwExtraBorder";
        lblSwExtraBorder.Size = new Size(320, 18);
        lblSwExtraBorder.TabIndex = 56;
        lblSwExtraBorder.Text = "Extra Deck Border";
        // 
        // swExtraBorder
        // 
        swExtraBorder.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swExtraBorder.FlatStyle = FlatStyle.Flat;
        swExtraBorder.Location = new Point(456, 810);
        swExtraBorder.Name = "swExtraBorder";
        swExtraBorder.Size = new Size(70, 22);
        swExtraBorder.TabIndex = 57;
        swExtraBorder.Tag = "ExtraBorder";
        swExtraBorder.UseVisualStyleBackColor = false;
        swExtraBorder.Click += swatch_Click;
        // 
        // lblSwSideRed
        // 
        lblSwSideRed.BackColor = Color.Transparent;
        lblSwSideRed.Font = new Font("Segoe UI", 8.5F);
        lblSwSideRed.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwSideRed.Location = new Point(20, 839);
        lblSwSideRed.Name = "lblSwSideRed";
        lblSwSideRed.Size = new Size(320, 18);
        lblSwSideRed.TabIndex = 58;
        lblSwSideRed.Text = "Side Deck Accent";
        // 
        // swSideRed
        // 
        swSideRed.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swSideRed.FlatStyle = FlatStyle.Flat;
        swSideRed.Location = new Point(456, 836);
        swSideRed.Name = "swSideRed";
        swSideRed.Size = new Size(70, 22);
        swSideRed.TabIndex = 59;
        swSideRed.Tag = "SideRed";
        swSideRed.UseVisualStyleBackColor = false;
        swSideRed.Click += swatch_Click;
        // 
        // lblSwSideBg
        // 
        lblSwSideBg.BackColor = Color.Transparent;
        lblSwSideBg.Font = new Font("Segoe UI", 8.5F);
        lblSwSideBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwSideBg.Location = new Point(20, 865);
        lblSwSideBg.Name = "lblSwSideBg";
        lblSwSideBg.Size = new Size(320, 18);
        lblSwSideBg.TabIndex = 60;
        lblSwSideBg.Text = "Side Deck Background";
        // 
        // swSideBg
        // 
        swSideBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swSideBg.FlatStyle = FlatStyle.Flat;
        swSideBg.Location = new Point(456, 862);
        swSideBg.Name = "swSideBg";
        swSideBg.Size = new Size(70, 22);
        swSideBg.TabIndex = 61;
        swSideBg.Tag = "SideBg";
        swSideBg.UseVisualStyleBackColor = false;
        swSideBg.Click += swatch_Click;
        // 
        // lblSwSideBorder
        // 
        lblSwSideBorder.BackColor = Color.Transparent;
        lblSwSideBorder.Font = new Font("Segoe UI", 8.5F);
        lblSwSideBorder.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwSideBorder.Location = new Point(20, 891);
        lblSwSideBorder.Name = "lblSwSideBorder";
        lblSwSideBorder.Size = new Size(320, 18);
        lblSwSideBorder.TabIndex = 62;
        lblSwSideBorder.Text = "Side Deck Border";
        // 
        // swSideBorder
        // 
        swSideBorder.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swSideBorder.FlatStyle = FlatStyle.Flat;
        swSideBorder.Location = new Point(456, 888);
        swSideBorder.Name = "swSideBorder";
        swSideBorder.Size = new Size(70, 22);
        swSideBorder.TabIndex = 63;
        swSideBorder.Tag = "SideBorder";
        swSideBorder.UseVisualStyleBackColor = false;
        swSideBorder.Click += swatch_Click;
        // 
        // lblSwStatAtk
        // 
        lblSwStatAtk.BackColor = Color.Transparent;
        lblSwStatAtk.Font = new Font("Segoe UI", 8.5F);
        lblSwStatAtk.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwStatAtk.Location = new Point(20, 917);
        lblSwStatAtk.Name = "lblSwStatAtk";
        lblSwStatAtk.Size = new Size(320, 18);
        lblSwStatAtk.TabIndex = 64;
        lblSwStatAtk.Text = "ATK Stat";
        // 
        // swStatAtk
        // 
        swStatAtk.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swStatAtk.FlatStyle = FlatStyle.Flat;
        swStatAtk.Location = new Point(456, 914);
        swStatAtk.Name = "swStatAtk";
        swStatAtk.Size = new Size(70, 22);
        swStatAtk.TabIndex = 65;
        swStatAtk.Tag = "StatAtk";
        swStatAtk.UseVisualStyleBackColor = false;
        swStatAtk.Click += swatch_Click;
        // 
        // lblSwStatDef
        // 
        lblSwStatDef.BackColor = Color.Transparent;
        lblSwStatDef.Font = new Font("Segoe UI", 8.5F);
        lblSwStatDef.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwStatDef.Location = new Point(20, 943);
        lblSwStatDef.Name = "lblSwStatDef";
        lblSwStatDef.Size = new Size(320, 18);
        lblSwStatDef.TabIndex = 66;
        lblSwStatDef.Text = "DEF Stat";
        // 
        // swStatDef
        // 
        swStatDef.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swStatDef.FlatStyle = FlatStyle.Flat;
        swStatDef.Location = new Point(456, 940);
        swStatDef.Name = "swStatDef";
        swStatDef.Size = new Size(70, 22);
        swStatDef.TabIndex = 67;
        swStatDef.Tag = "StatDef";
        swStatDef.UseVisualStyleBackColor = false;
        swStatDef.Click += swatch_Click;
        // 
        // lblSwStatMuted
        // 
        lblSwStatMuted.BackColor = Color.Transparent;
        lblSwStatMuted.Font = new Font("Segoe UI", 8.5F);
        lblSwStatMuted.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwStatMuted.Location = new Point(20, 969);
        lblSwStatMuted.Name = "lblSwStatMuted";
        lblSwStatMuted.Size = new Size(320, 18);
        lblSwStatMuted.TabIndex = 68;
        lblSwStatMuted.Text = "Muted Stat";
        // 
        // swStatMuted
        // 
        swStatMuted.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swStatMuted.FlatStyle = FlatStyle.Flat;
        swStatMuted.Location = new Point(456, 966);
        swStatMuted.Name = "swStatMuted";
        swStatMuted.Size = new Size(70, 22);
        swStatMuted.TabIndex = 69;
        swStatMuted.Tag = "StatMuted";
        swStatMuted.UseVisualStyleBackColor = false;
        swStatMuted.Click += swatch_Click;
        // 
        // lblSwStatusOk
        // 
        lblSwStatusOk.BackColor = Color.Transparent;
        lblSwStatusOk.Font = new Font("Segoe UI", 8.5F);
        lblSwStatusOk.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwStatusOk.Location = new Point(20, 995);
        lblSwStatusOk.Name = "lblSwStatusOk";
        lblSwStatusOk.Size = new Size(320, 18);
        lblSwStatusOk.TabIndex = 70;
        lblSwStatusOk.Text = "Status OK";
        // 
        // swStatusOk
        // 
        swStatusOk.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swStatusOk.FlatStyle = FlatStyle.Flat;
        swStatusOk.Location = new Point(456, 992);
        swStatusOk.Name = "swStatusOk";
        swStatusOk.Size = new Size(70, 22);
        swStatusOk.TabIndex = 71;
        swStatusOk.Tag = "StatusOk";
        swStatusOk.UseVisualStyleBackColor = false;
        swStatusOk.Click += swatch_Click;
        // 
        // lblSecBUTTONS
        // 
        lblSecBUTTONS.BackColor = Color.Transparent;
        lblSecBUTTONS.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSecBUTTONS.ForeColor = Color.FromArgb(220, 184, 84);
        lblSecBUTTONS.Location = new Point(8, 1028);
        lblSecBUTTONS.Name = "lblSecBUTTONS";
        lblSecBUTTONS.Size = new Size(540, 24);
        lblSecBUTTONS.TabIndex = 72;
        lblSecBUTTONS.Text = "BUTTONS";
        // 
        // lblSwGoldBtnBg
        // 
        lblSwGoldBtnBg.BackColor = Color.Transparent;
        lblSwGoldBtnBg.Font = new Font("Segoe UI", 8.5F);
        lblSwGoldBtnBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwGoldBtnBg.Location = new Point(20, 1055);
        lblSwGoldBtnBg.Name = "lblSwGoldBtnBg";
        lblSwGoldBtnBg.Size = new Size(320, 18);
        lblSwGoldBtnBg.TabIndex = 73;
        lblSwGoldBtnBg.Text = "Gold Button Background";
        // 
        // swGoldBtnBg
        // 
        swGoldBtnBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swGoldBtnBg.FlatStyle = FlatStyle.Flat;
        swGoldBtnBg.Location = new Point(456, 1052);
        swGoldBtnBg.Name = "swGoldBtnBg";
        swGoldBtnBg.Size = new Size(70, 22);
        swGoldBtnBg.TabIndex = 74;
        swGoldBtnBg.Tag = "GoldBtnBg";
        swGoldBtnBg.UseVisualStyleBackColor = false;
        swGoldBtnBg.Click += swatch_Click;
        // 
        // lblSwGoldBtnFg
        // 
        lblSwGoldBtnFg.BackColor = Color.Transparent;
        lblSwGoldBtnFg.Font = new Font("Segoe UI", 8.5F);
        lblSwGoldBtnFg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwGoldBtnFg.Location = new Point(20, 1081);
        lblSwGoldBtnFg.Name = "lblSwGoldBtnFg";
        lblSwGoldBtnFg.Size = new Size(320, 18);
        lblSwGoldBtnFg.TabIndex = 75;
        lblSwGoldBtnFg.Text = "Gold Button Text";
        // 
        // swGoldBtnFg
        // 
        swGoldBtnFg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swGoldBtnFg.FlatStyle = FlatStyle.Flat;
        swGoldBtnFg.Location = new Point(456, 1078);
        swGoldBtnFg.Name = "swGoldBtnFg";
        swGoldBtnFg.Size = new Size(70, 22);
        swGoldBtnFg.TabIndex = 76;
        swGoldBtnFg.Tag = "GoldBtnFg";
        swGoldBtnFg.UseVisualStyleBackColor = false;
        swGoldBtnFg.Click += swatch_Click;
        // 
        // lblSwGoldBtnHover
        // 
        lblSwGoldBtnHover.BackColor = Color.Transparent;
        lblSwGoldBtnHover.Font = new Font("Segoe UI", 8.5F);
        lblSwGoldBtnHover.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwGoldBtnHover.Location = new Point(20, 1107);
        lblSwGoldBtnHover.Name = "lblSwGoldBtnHover";
        lblSwGoldBtnHover.Size = new Size(320, 18);
        lblSwGoldBtnHover.TabIndex = 77;
        lblSwGoldBtnHover.Text = "Gold Button Hover";
        // 
        // swGoldBtnHover
        // 
        swGoldBtnHover.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swGoldBtnHover.FlatStyle = FlatStyle.Flat;
        swGoldBtnHover.Location = new Point(456, 1104);
        swGoldBtnHover.Name = "swGoldBtnHover";
        swGoldBtnHover.Size = new Size(70, 22);
        swGoldBtnHover.TabIndex = 78;
        swGoldBtnHover.Tag = "GoldBtnHover";
        swGoldBtnHover.UseVisualStyleBackColor = false;
        swGoldBtnHover.Click += swatch_Click;
        // 
        // lblSwButtonNeutralBg
        // 
        lblSwButtonNeutralBg.BackColor = Color.Transparent;
        lblSwButtonNeutralBg.Font = new Font("Segoe UI", 8.5F);
        lblSwButtonNeutralBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwButtonNeutralBg.Location = new Point(20, 1133);
        lblSwButtonNeutralBg.Name = "lblSwButtonNeutralBg";
        lblSwButtonNeutralBg.Size = new Size(320, 18);
        lblSwButtonNeutralBg.TabIndex = 79;
        lblSwButtonNeutralBg.Text = "Neutral Button Background";
        // 
        // swButtonNeutralBg
        // 
        swButtonNeutralBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swButtonNeutralBg.FlatStyle = FlatStyle.Flat;
        swButtonNeutralBg.Location = new Point(456, 1130);
        swButtonNeutralBg.Name = "swButtonNeutralBg";
        swButtonNeutralBg.Size = new Size(70, 22);
        swButtonNeutralBg.TabIndex = 80;
        swButtonNeutralBg.Tag = "ButtonNeutralBg";
        swButtonNeutralBg.UseVisualStyleBackColor = false;
        swButtonNeutralBg.Click += swatch_Click;
        // 
        // lblSwButtonNeutralHover
        // 
        lblSwButtonNeutralHover.BackColor = Color.Transparent;
        lblSwButtonNeutralHover.Font = new Font("Segoe UI", 8.5F);
        lblSwButtonNeutralHover.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwButtonNeutralHover.Location = new Point(20, 1159);
        lblSwButtonNeutralHover.Name = "lblSwButtonNeutralHover";
        lblSwButtonNeutralHover.Size = new Size(320, 18);
        lblSwButtonNeutralHover.TabIndex = 81;
        lblSwButtonNeutralHover.Text = "Neutral Button Hover";
        // 
        // swButtonNeutralHover
        // 
        swButtonNeutralHover.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swButtonNeutralHover.FlatStyle = FlatStyle.Flat;
        swButtonNeutralHover.Location = new Point(456, 1156);
        swButtonNeutralHover.Name = "swButtonNeutralHover";
        swButtonNeutralHover.Size = new Size(70, 22);
        swButtonNeutralHover.TabIndex = 82;
        swButtonNeutralHover.Tag = "ButtonNeutralHover";
        swButtonNeutralHover.UseVisualStyleBackColor = false;
        swButtonNeutralHover.Click += swatch_Click;
        // 
        // lblSwDangerFg
        // 
        lblSwDangerFg.BackColor = Color.Transparent;
        lblSwDangerFg.Font = new Font("Segoe UI", 8.5F);
        lblSwDangerFg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwDangerFg.Location = new Point(20, 1185);
        lblSwDangerFg.Name = "lblSwDangerFg";
        lblSwDangerFg.Size = new Size(320, 18);
        lblSwDangerFg.TabIndex = 83;
        lblSwDangerFg.Text = "Danger Text";
        // 
        // swDangerFg
        // 
        swDangerFg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swDangerFg.FlatStyle = FlatStyle.Flat;
        swDangerFg.Location = new Point(456, 1182);
        swDangerFg.Name = "swDangerFg";
        swDangerFg.Size = new Size(70, 22);
        swDangerFg.TabIndex = 84;
        swDangerFg.Tag = "DangerFg";
        swDangerFg.UseVisualStyleBackColor = false;
        swDangerFg.Click += swatch_Click;
        // 
        // lblSwDangerBorder
        // 
        lblSwDangerBorder.BackColor = Color.Transparent;
        lblSwDangerBorder.Font = new Font("Segoe UI", 8.5F);
        lblSwDangerBorder.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwDangerBorder.Location = new Point(20, 1211);
        lblSwDangerBorder.Name = "lblSwDangerBorder";
        lblSwDangerBorder.Size = new Size(320, 18);
        lblSwDangerBorder.TabIndex = 85;
        lblSwDangerBorder.Text = "Danger Border";
        // 
        // swDangerBorder
        // 
        swDangerBorder.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swDangerBorder.FlatStyle = FlatStyle.Flat;
        swDangerBorder.Location = new Point(456, 1208);
        swDangerBorder.Name = "swDangerBorder";
        swDangerBorder.Size = new Size(70, 22);
        swDangerBorder.TabIndex = 86;
        swDangerBorder.Tag = "DangerBorder";
        swDangerBorder.UseVisualStyleBackColor = false;
        swDangerBorder.Click += swatch_Click;
        // 
        // lblSwDangerHoverBg
        // 
        lblSwDangerHoverBg.BackColor = Color.Transparent;
        lblSwDangerHoverBg.Font = new Font("Segoe UI", 8.5F);
        lblSwDangerHoverBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwDangerHoverBg.Location = new Point(20, 1237);
        lblSwDangerHoverBg.Name = "lblSwDangerHoverBg";
        lblSwDangerHoverBg.Size = new Size(320, 18);
        lblSwDangerHoverBg.TabIndex = 87;
        lblSwDangerHoverBg.Text = "Danger Hover Background";
        // 
        // swDangerHoverBg
        // 
        swDangerHoverBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swDangerHoverBg.FlatStyle = FlatStyle.Flat;
        swDangerHoverBg.Location = new Point(456, 1234);
        swDangerHoverBg.Name = "swDangerHoverBg";
        swDangerHoverBg.Size = new Size(70, 22);
        swDangerHoverBg.TabIndex = 88;
        swDangerHoverBg.Tag = "DangerHoverBg";
        swDangerHoverBg.UseVisualStyleBackColor = false;
        swDangerHoverBg.Click += swatch_Click;
        // 
        // lblSwAccentRedHover
        // 
        lblSwAccentRedHover.BackColor = Color.Transparent;
        lblSwAccentRedHover.Font = new Font("Segoe UI", 8.5F);
        lblSwAccentRedHover.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwAccentRedHover.Location = new Point(20, 1263);
        lblSwAccentRedHover.Name = "lblSwAccentRedHover";
        lblSwAccentRedHover.Size = new Size(320, 18);
        lblSwAccentRedHover.TabIndex = 89;
        lblSwAccentRedHover.Text = "Accent Red Hover";
        // 
        // swAccentRedHover
        // 
        swAccentRedHover.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swAccentRedHover.FlatStyle = FlatStyle.Flat;
        swAccentRedHover.Location = new Point(456, 1260);
        swAccentRedHover.Name = "swAccentRedHover";
        swAccentRedHover.Size = new Size(70, 22);
        swAccentRedHover.TabIndex = 90;
        swAccentRedHover.Tag = "AccentRedHover";
        swAccentRedHover.UseVisualStyleBackColor = false;
        swAccentRedHover.Click += swatch_Click;
        // 
        // lblSecMISC
        // 
        lblSecMISC.BackColor = Color.Transparent;
        lblSecMISC.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSecMISC.ForeColor = Color.FromArgb(220, 184, 84);
        lblSecMISC.Location = new Point(8, 1296);
        lblSecMISC.Name = "lblSecMISC";
        lblSecMISC.Size = new Size(540, 24);
        lblSecMISC.TabIndex = 91;
        lblSecMISC.Text = "MISC";
        // 
        // lblSwOverlayBackdrop
        // 
        lblSwOverlayBackdrop.BackColor = Color.Transparent;
        lblSwOverlayBackdrop.Font = new Font("Segoe UI", 8.5F);
        lblSwOverlayBackdrop.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwOverlayBackdrop.Location = new Point(20, 1323);
        lblSwOverlayBackdrop.Name = "lblSwOverlayBackdrop";
        lblSwOverlayBackdrop.Size = new Size(320, 18);
        lblSwOverlayBackdrop.TabIndex = 92;
        lblSwOverlayBackdrop.Text = "Overlay Backdrop";
        // 
        // swOverlayBackdrop
        // 
        swOverlayBackdrop.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swOverlayBackdrop.FlatStyle = FlatStyle.Flat;
        swOverlayBackdrop.Location = new Point(456, 1320);
        swOverlayBackdrop.Name = "swOverlayBackdrop";
        swOverlayBackdrop.Size = new Size(70, 22);
        swOverlayBackdrop.TabIndex = 93;
        swOverlayBackdrop.Tag = "OverlayBackdrop";
        swOverlayBackdrop.UseVisualStyleBackColor = false;
        swOverlayBackdrop.Click += swatch_Click;
        // 
        // lblSwPortraitBg
        // 
        lblSwPortraitBg.BackColor = Color.Transparent;
        lblSwPortraitBg.Font = new Font("Segoe UI", 8.5F);
        lblSwPortraitBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwPortraitBg.Location = new Point(20, 1349);
        lblSwPortraitBg.Name = "lblSwPortraitBg";
        lblSwPortraitBg.Size = new Size(320, 18);
        lblSwPortraitBg.TabIndex = 94;
        lblSwPortraitBg.Text = "Portrait Background";
        // 
        // swPortraitBg
        // 
        swPortraitBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swPortraitBg.FlatStyle = FlatStyle.Flat;
        swPortraitBg.Location = new Point(456, 1346);
        swPortraitBg.Name = "swPortraitBg";
        swPortraitBg.Size = new Size(70, 22);
        swPortraitBg.TabIndex = 95;
        swPortraitBg.Tag = "PortraitBg";
        swPortraitBg.UseVisualStyleBackColor = false;
        swPortraitBg.Click += swatch_Click;
        // 
        // lblSwAccentDivider
        // 
        lblSwAccentDivider.BackColor = Color.Transparent;
        lblSwAccentDivider.Font = new Font("Segoe UI", 8.5F);
        lblSwAccentDivider.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwAccentDivider.Location = new Point(20, 1375);
        lblSwAccentDivider.Name = "lblSwAccentDivider";
        lblSwAccentDivider.Size = new Size(320, 18);
        lblSwAccentDivider.TabIndex = 96;
        lblSwAccentDivider.Text = "Accent Divider";
        // 
        // swAccentDivider
        // 
        swAccentDivider.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swAccentDivider.FlatStyle = FlatStyle.Flat;
        swAccentDivider.Location = new Point(456, 1372);
        swAccentDivider.Name = "swAccentDivider";
        swAccentDivider.Size = new Size(70, 22);
        swAccentDivider.TabIndex = 97;
        swAccentDivider.Tag = "AccentDivider";
        swAccentDivider.UseVisualStyleBackColor = false;
        swAccentDivider.Click += swatch_Click;
        // 
        // lblSwOfflinePanelBg
        // 
        lblSwOfflinePanelBg.BackColor = Color.Transparent;
        lblSwOfflinePanelBg.Font = new Font("Segoe UI", 8.5F);
        lblSwOfflinePanelBg.ForeColor = Color.FromArgb(190, 190, 212);
        lblSwOfflinePanelBg.Location = new Point(20, 1401);
        lblSwOfflinePanelBg.Name = "lblSwOfflinePanelBg";
        lblSwOfflinePanelBg.Size = new Size(320, 18);
        lblSwOfflinePanelBg.TabIndex = 98;
        lblSwOfflinePanelBg.Text = "Offline Panel Background";
        // 
        // swOfflinePanelBg
        // 
        swOfflinePanelBg.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        swOfflinePanelBg.FlatStyle = FlatStyle.Flat;
        swOfflinePanelBg.Location = new Point(456, 1398);
        swOfflinePanelBg.Name = "swOfflinePanelBg";
        swOfflinePanelBg.Size = new Size(70, 22);
        swOfflinePanelBg.TabIndex = 99;
        swOfflinePanelBg.Tag = "OfflinePanelBg";
        swOfflinePanelBg.UseVisualStyleBackColor = false;
        swOfflinePanelBg.Click += swatch_Click;
        // 
        // chkApplyNow
        // 
        chkApplyNow.BackColor = Color.Transparent;
        chkApplyNow.Checked = true;
        chkApplyNow.CheckState = CheckState.Checked;
        chkApplyNow.ForeColor = Color.FromArgb(190, 190, 212);
        chkApplyNow.Location = new Point(20, 618);
        chkApplyNow.Name = "chkApplyNow";
        chkApplyNow.Size = new Size(280, 20);
        chkApplyNow.TabIndex = 101;
        chkApplyNow.Text = "Apply immediately (briefly reloads the window)";
        chkApplyNow.UseVisualStyleBackColor = false;
        // 
        // btnDelete
        // 
        btnDelete.BackColor = Color.Transparent;
        btnDelete.FlatAppearance.BorderColor = Color.FromArgb(98, 54, 54);
        btnDelete.FlatStyle = FlatStyle.Flat;
        btnDelete.ForeColor = Color.FromArgb(230, 118, 118);
        btnDelete.Location = new Point(20, 644);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(110, 32);
        btnDelete.TabIndex = 102;
        btnDelete.Text = "Delete theme";
        btnDelete.UseVisualStyleBackColor = false;
        btnDelete.Visible = false;
        btnDelete.Click += btnDelete_Click;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.Transparent;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.ForeColor = Color.FromArgb(190, 190, 212);
        btnCancel.Location = new Point(390, 644);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 32);
        btnCancel.TabIndex = 103;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = false;
        // 
        // btnSave
        // 
        btnSave.BackColor = Color.FromArgb(216, 178, 60);
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.ForeColor = Color.FromArgb(18, 18, 28);
        btnSave.Location = new Point(490, 644);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(90, 32);
        btnSave.TabIndex = 104;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Click += btnSave_Click;
        // 
        // ThemeEditorForm
        // 
        AcceptButton = btnSave;
        BackColor = Color.FromArgb(13, 13, 22);
        CancelButton = btnCancel;
        ClientSize = new Size(600, 696);
        Controls.Add(lblNameTitle);
        Controls.Add(txtName);
        Controls.Add(lblBaseTitle);
        Controls.Add(cboBase);
        Controls.Add(chkIsDark);
        Controls.Add(lblBgTitle);
        Controls.Add(txtBgPath);
        Controls.Add(btnBrowseBg);
        Controls.Add(btnClearBg);
        Controls.Add(lblOpacityTitle);
        Controls.Add(trkOpacity);
        Controls.Add(lblOpacityValue);
        Controls.Add(pnlSwatches);
        Controls.Add(chkApplyNow);
        Controls.Add(btnDelete);
        Controls.Add(btnCancel);
        Controls.Add(btnSave);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ThemeEditorForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Customize Theme";
        ((System.ComponentModel.ISupportInitialize)trkOpacity).EndInit();
        pnlSwatches.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
