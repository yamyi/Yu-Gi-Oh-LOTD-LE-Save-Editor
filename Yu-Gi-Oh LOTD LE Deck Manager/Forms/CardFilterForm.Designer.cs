namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

partial class CardFilterForm
{
    // ── Column 1 ─────────────────────────────────────────────────────────────
    private Label lblNameTitle = null!;
    private TextBox txtName = null!;
    private Label lblIdTitle = null!;
    private TextBox txtId = null!;
    private Label lblTypeTitle = null!;
    private ComboBox cboType = null!;
    private Label lblAttrTitle = null!;
    private ComboBox cboAttribute = null!;
    private Label lblRaceTitle = null!;
    private ComboBox cboRace = null!;
    // ── Column 2 ─────────────────────────────────────────────────────────────
    private Label lblArchetypeTitle = null!;
    private ComboBox cboArchetype = null!;
    private Label lblLevelTitle = null!;
    private TextBox txtMinLevel = null!;
    private Label lblLevelDash = null!;
    private TextBox txtMaxLevel = null!;
    private Label lblLinkTitle = null!;
    private TextBox txtMinLinkVal = null!;
    private Label lblLinkDash = null!;
    private TextBox txtMaxLinkVal = null!;
    private Label lblScaleTitle = null!;
    private TextBox txtMinScale = null!;
    private Label lblScaleDash = null!;
    private TextBox txtMaxScale = null!;
    // ── Ranges ───────────────────────────────────────────────────────────────
    private Label lblAtkTitle = null!;
    private TextBox txtMinAtk = null!;
    private Label lblAtkDash = null!;
    private TextBox txtMaxAtk = null!;
    private Label lblDefTitle = null!;
    private TextBox txtMinDef = null!;
    private Label lblDefDash = null!;
    private TextBox txtMaxDef = null!;
    // ── Buttons ──────────────────────────────────────────────────────────────
    private Button btnClear = null!;
    private Button btnCancel = null!;
    private Button btnApply = null!;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardFilterForm));
        lblNameTitle = new Label();
        txtName = new TextBox();
        lblIdTitle = new Label();
        txtId = new TextBox();
        lblTypeTitle = new Label();
        cboType = new ComboBox();
        lblAttrTitle = new Label();
        cboAttribute = new ComboBox();
        lblRaceTitle = new Label();
        cboRace = new ComboBox();
        lblArchetypeTitle = new Label();
        cboArchetype = new ComboBox();
        lblLevelTitle = new Label();
        txtMinLevel = new TextBox();
        lblLevelDash = new Label();
        txtMaxLevel = new TextBox();
        lblLinkTitle = new Label();
        txtMinLinkVal = new TextBox();
        lblLinkDash = new Label();
        txtMaxLinkVal = new TextBox();
        lblScaleTitle = new Label();
        txtMinScale = new TextBox();
        lblScaleDash = new Label();
        txtMaxScale = new TextBox();
        lblAtkTitle = new Label();
        txtMinAtk = new TextBox();
        lblAtkDash = new Label();
        txtMaxAtk = new TextBox();
        lblDefTitle = new Label();
        txtMinDef = new TextBox();
        lblDefDash = new Label();
        txtMaxDef = new TextBox();
        btnClear = new Button();
        btnCancel = new Button();
        btnApply = new Button();
        SuspendLayout();
        // 
        // lblNameTitle
        // 
        lblNameTitle.BackColor = Color.Transparent;
        lblNameTitle.Font = new Font("Segoe UI", 8F);
        lblNameTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblNameTitle.Location = new Point(20, 16);
        lblNameTitle.Name = "lblNameTitle";
        lblNameTitle.Size = new Size(190, 14);
        lblNameTitle.TabIndex = 0;
        lblNameTitle.Text = "NAME CONTAINS";
        // 
        // txtName
        // 
        txtName.BackColor = Color.FromArgb(24, 24, 36);
        txtName.BorderStyle = BorderStyle.FixedSingle;
        txtName.ForeColor = Color.FromArgb(238, 238, 250);
        txtName.Location = new Point(20, 32);
        txtName.Name = "txtName";
        txtName.PlaceholderText = "Any";
        txtName.Size = new Size(190, 23);
        txtName.TabIndex = 1;
        // 
        // lblIdTitle
        // 
        lblIdTitle.BackColor = Color.Transparent;
        lblIdTitle.Font = new Font("Segoe UI", 8F);
        lblIdTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblIdTitle.Location = new Point(20, 68);
        lblIdTitle.Name = "lblIdTitle";
        lblIdTitle.Size = new Size(190, 14);
        lblIdTitle.TabIndex = 2;
        lblIdTitle.Text = "CARD ID (PASSCODE)";
        // 
        // txtId
        // 
        txtId.BackColor = Color.FromArgb(24, 24, 36);
        txtId.BorderStyle = BorderStyle.FixedSingle;
        txtId.ForeColor = Color.FromArgb(238, 238, 250);
        txtId.Location = new Point(20, 84);
        txtId.Name = "txtId";
        txtId.PlaceholderText = "Any";
        txtId.Size = new Size(190, 23);
        txtId.TabIndex = 3;
        // 
        // lblTypeTitle
        // 
        lblTypeTitle.BackColor = Color.Transparent;
        lblTypeTitle.Font = new Font("Segoe UI", 8F);
        lblTypeTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblTypeTitle.Location = new Point(20, 120);
        lblTypeTitle.Name = "lblTypeTitle";
        lblTypeTitle.Size = new Size(190, 14);
        lblTypeTitle.TabIndex = 4;
        lblTypeTitle.Text = "CARD TYPE";
        // 
        // cboType
        // 
        cboType.BackColor = Color.FromArgb(24, 24, 36);
        cboType.DropDownStyle = ComboBoxStyle.DropDownList;
        cboType.ForeColor = Color.FromArgb(238, 238, 250);
        cboType.FormattingEnabled = true;
        cboType.Location = new Point(20, 136);
        cboType.Name = "cboType";
        cboType.Size = new Size(190, 23);
        cboType.TabIndex = 5;
        // 
        // lblAttrTitle
        // 
        lblAttrTitle.BackColor = Color.Transparent;
        lblAttrTitle.Font = new Font("Segoe UI", 8F);
        lblAttrTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblAttrTitle.Location = new Point(20, 172);
        lblAttrTitle.Name = "lblAttrTitle";
        lblAttrTitle.Size = new Size(190, 14);
        lblAttrTitle.TabIndex = 6;
        lblAttrTitle.Text = "ATTRIBUTE";
        // 
        // cboAttribute
        // 
        cboAttribute.BackColor = Color.FromArgb(24, 24, 36);
        cboAttribute.DropDownStyle = ComboBoxStyle.DropDownList;
        cboAttribute.ForeColor = Color.FromArgb(238, 238, 250);
        cboAttribute.FormattingEnabled = true;
        cboAttribute.Location = new Point(20, 188);
        cboAttribute.Name = "cboAttribute";
        cboAttribute.Size = new Size(190, 23);
        cboAttribute.TabIndex = 7;
        // 
        // lblRaceTitle
        // 
        lblRaceTitle.BackColor = Color.Transparent;
        lblRaceTitle.Font = new Font("Segoe UI", 8F);
        lblRaceTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblRaceTitle.Location = new Point(20, 224);
        lblRaceTitle.Name = "lblRaceTitle";
        lblRaceTitle.Size = new Size(190, 14);
        lblRaceTitle.TabIndex = 8;
        lblRaceTitle.Text = "RACE / TYPE";
        // 
        // cboRace
        // 
        cboRace.BackColor = Color.FromArgb(24, 24, 36);
        cboRace.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRace.ForeColor = Color.FromArgb(238, 238, 250);
        cboRace.FormattingEnabled = true;
        cboRace.Location = new Point(20, 240);
        cboRace.Name = "cboRace";
        cboRace.Size = new Size(190, 23);
        cboRace.TabIndex = 9;
        // 
        // lblArchetypeTitle
        // 
        lblArchetypeTitle.BackColor = Color.Transparent;
        lblArchetypeTitle.Font = new Font("Segoe UI", 8F);
        lblArchetypeTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblArchetypeTitle.Location = new Point(230, 16);
        lblArchetypeTitle.Name = "lblArchetypeTitle";
        lblArchetypeTitle.Size = new Size(190, 14);
        lblArchetypeTitle.TabIndex = 10;
        lblArchetypeTitle.Text = "ARCHETYPE";
        // 
        // cboArchetype
        // 
        cboArchetype.BackColor = Color.FromArgb(24, 24, 36);
        cboArchetype.DropDownStyle = ComboBoxStyle.DropDownList;
        cboArchetype.ForeColor = Color.FromArgb(238, 238, 250);
        cboArchetype.FormattingEnabled = true;
        cboArchetype.Location = new Point(230, 32);
        cboArchetype.Name = "cboArchetype";
        cboArchetype.Size = new Size(190, 23);
        cboArchetype.TabIndex = 11;
        // 
        // lblLevelTitle
        // 
        lblLevelTitle.BackColor = Color.Transparent;
        lblLevelTitle.Font = new Font("Segoe UI", 8F);
        lblLevelTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblLevelTitle.Location = new Point(230, 68);
        lblLevelTitle.Name = "lblLevelTitle";
        lblLevelTitle.Size = new Size(190, 14);
        lblLevelTitle.TabIndex = 12;
        lblLevelTitle.Text = "LEVEL / RANK";
        // 
        // txtMinLevel
        // 
        txtMinLevel.BackColor = Color.FromArgb(24, 24, 36);
        txtMinLevel.BorderStyle = BorderStyle.FixedSingle;
        txtMinLevel.ForeColor = Color.FromArgb(238, 238, 250);
        txtMinLevel.Location = new Point(230, 84);
        txtMinLevel.Name = "txtMinLevel";
        txtMinLevel.PlaceholderText = "Min";
        txtMinLevel.Size = new Size(82, 23);
        txtMinLevel.TabIndex = 13;
        // 
        // lblLevelDash
        // 
        lblLevelDash.BackColor = Color.Transparent;
        lblLevelDash.Font = new Font("Segoe UI", 9F);
        lblLevelDash.ForeColor = Color.FromArgb(138, 138, 164);
        lblLevelDash.Location = new Point(316, 86);
        lblLevelDash.Name = "lblLevelDash";
        lblLevelDash.Size = new Size(16, 18);
        lblLevelDash.TabIndex = 14;
        lblLevelDash.Text = "–";
        lblLevelDash.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // txtMaxLevel
        // 
        txtMaxLevel.BackColor = Color.FromArgb(24, 24, 36);
        txtMaxLevel.BorderStyle = BorderStyle.FixedSingle;
        txtMaxLevel.ForeColor = Color.FromArgb(238, 238, 250);
        txtMaxLevel.Location = new Point(334, 84);
        txtMaxLevel.Name = "txtMaxLevel";
        txtMaxLevel.PlaceholderText = "Max";
        txtMaxLevel.Size = new Size(86, 23);
        txtMaxLevel.TabIndex = 15;
        // 
        // lblLinkTitle
        // 
        lblLinkTitle.BackColor = Color.Transparent;
        lblLinkTitle.Font = new Font("Segoe UI", 8F);
        lblLinkTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblLinkTitle.Location = new Point(230, 120);
        lblLinkTitle.Name = "lblLinkTitle";
        lblLinkTitle.Size = new Size(190, 14);
        lblLinkTitle.TabIndex = 16;
        lblLinkTitle.Text = "LINK RATING";
        // 
        // txtMinLinkVal
        // 
        txtMinLinkVal.BackColor = Color.FromArgb(24, 24, 36);
        txtMinLinkVal.BorderStyle = BorderStyle.FixedSingle;
        txtMinLinkVal.ForeColor = Color.FromArgb(238, 238, 250);
        txtMinLinkVal.Location = new Point(230, 136);
        txtMinLinkVal.Name = "txtMinLinkVal";
        txtMinLinkVal.PlaceholderText = "Min";
        txtMinLinkVal.Size = new Size(82, 23);
        txtMinLinkVal.TabIndex = 17;
        // 
        // lblLinkDash
        // 
        lblLinkDash.BackColor = Color.Transparent;
        lblLinkDash.Font = new Font("Segoe UI", 9F);
        lblLinkDash.ForeColor = Color.FromArgb(138, 138, 164);
        lblLinkDash.Location = new Point(316, 138);
        lblLinkDash.Name = "lblLinkDash";
        lblLinkDash.Size = new Size(16, 18);
        lblLinkDash.TabIndex = 18;
        lblLinkDash.Text = "–";
        lblLinkDash.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // txtMaxLinkVal
        // 
        txtMaxLinkVal.BackColor = Color.FromArgb(24, 24, 36);
        txtMaxLinkVal.BorderStyle = BorderStyle.FixedSingle;
        txtMaxLinkVal.ForeColor = Color.FromArgb(238, 238, 250);
        txtMaxLinkVal.Location = new Point(334, 136);
        txtMaxLinkVal.Name = "txtMaxLinkVal";
        txtMaxLinkVal.PlaceholderText = "Max";
        txtMaxLinkVal.Size = new Size(86, 23);
        txtMaxLinkVal.TabIndex = 19;
        // 
        // lblScaleTitle
        // 
        lblScaleTitle.BackColor = Color.Transparent;
        lblScaleTitle.Font = new Font("Segoe UI", 8F);
        lblScaleTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblScaleTitle.Location = new Point(230, 172);
        lblScaleTitle.Name = "lblScaleTitle";
        lblScaleTitle.Size = new Size(190, 14);
        lblScaleTitle.TabIndex = 20;
        lblScaleTitle.Text = "PENDULUM SCALE";
        // 
        // txtMinScale
        // 
        txtMinScale.BackColor = Color.FromArgb(24, 24, 36);
        txtMinScale.BorderStyle = BorderStyle.FixedSingle;
        txtMinScale.ForeColor = Color.FromArgb(238, 238, 250);
        txtMinScale.Location = new Point(230, 188);
        txtMinScale.Name = "txtMinScale";
        txtMinScale.PlaceholderText = "Min";
        txtMinScale.Size = new Size(82, 23);
        txtMinScale.TabIndex = 21;
        // 
        // lblScaleDash
        // 
        lblScaleDash.BackColor = Color.Transparent;
        lblScaleDash.Font = new Font("Segoe UI", 9F);
        lblScaleDash.ForeColor = Color.FromArgb(138, 138, 164);
        lblScaleDash.Location = new Point(316, 190);
        lblScaleDash.Name = "lblScaleDash";
        lblScaleDash.Size = new Size(16, 18);
        lblScaleDash.TabIndex = 22;
        lblScaleDash.Text = "–";
        lblScaleDash.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // txtMaxScale
        // 
        txtMaxScale.BackColor = Color.FromArgb(24, 24, 36);
        txtMaxScale.BorderStyle = BorderStyle.FixedSingle;
        txtMaxScale.ForeColor = Color.FromArgb(238, 238, 250);
        txtMaxScale.Location = new Point(334, 188);
        txtMaxScale.Name = "txtMaxScale";
        txtMaxScale.PlaceholderText = "Max";
        txtMaxScale.Size = new Size(86, 23);
        txtMaxScale.TabIndex = 23;
        // 
        // lblAtkTitle
        // 
        lblAtkTitle.BackColor = Color.Transparent;
        lblAtkTitle.Font = new Font("Segoe UI", 8F);
        lblAtkTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblAtkTitle.Location = new Point(20, 276);
        lblAtkTitle.Name = "lblAtkTitle";
        lblAtkTitle.Size = new Size(190, 14);
        lblAtkTitle.TabIndex = 24;
        lblAtkTitle.Text = "ATK RANGE";
        // 
        // txtMinAtk
        // 
        txtMinAtk.BackColor = Color.FromArgb(24, 24, 36);
        txtMinAtk.BorderStyle = BorderStyle.FixedSingle;
        txtMinAtk.ForeColor = Color.FromArgb(238, 238, 250);
        txtMinAtk.Location = new Point(20, 292);
        txtMinAtk.Name = "txtMinAtk";
        txtMinAtk.PlaceholderText = "Min";
        txtMinAtk.Size = new Size(82, 23);
        txtMinAtk.TabIndex = 25;
        // 
        // lblAtkDash
        // 
        lblAtkDash.BackColor = Color.Transparent;
        lblAtkDash.Font = new Font("Segoe UI", 9F);
        lblAtkDash.ForeColor = Color.FromArgb(138, 138, 164);
        lblAtkDash.Location = new Point(106, 294);
        lblAtkDash.Name = "lblAtkDash";
        lblAtkDash.Size = new Size(16, 18);
        lblAtkDash.TabIndex = 26;
        lblAtkDash.Text = "–";
        lblAtkDash.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // txtMaxAtk
        // 
        txtMaxAtk.BackColor = Color.FromArgb(24, 24, 36);
        txtMaxAtk.BorderStyle = BorderStyle.FixedSingle;
        txtMaxAtk.ForeColor = Color.FromArgb(238, 238, 250);
        txtMaxAtk.Location = new Point(124, 292);
        txtMaxAtk.Name = "txtMaxAtk";
        txtMaxAtk.PlaceholderText = "Max";
        txtMaxAtk.Size = new Size(86, 23);
        txtMaxAtk.TabIndex = 27;
        // 
        // lblDefTitle
        // 
        lblDefTitle.BackColor = Color.Transparent;
        lblDefTitle.Font = new Font("Segoe UI", 8F);
        lblDefTitle.ForeColor = Color.FromArgb(138, 138, 164);
        lblDefTitle.Location = new Point(230, 276);
        lblDefTitle.Name = "lblDefTitle";
        lblDefTitle.Size = new Size(190, 14);
        lblDefTitle.TabIndex = 28;
        lblDefTitle.Text = "DEF RANGE";
        // 
        // txtMinDef
        // 
        txtMinDef.BackColor = Color.FromArgb(24, 24, 36);
        txtMinDef.BorderStyle = BorderStyle.FixedSingle;
        txtMinDef.ForeColor = Color.FromArgb(238, 238, 250);
        txtMinDef.Location = new Point(230, 292);
        txtMinDef.Name = "txtMinDef";
        txtMinDef.PlaceholderText = "Min";
        txtMinDef.Size = new Size(82, 23);
        txtMinDef.TabIndex = 29;
        // 
        // lblDefDash
        // 
        lblDefDash.BackColor = Color.Transparent;
        lblDefDash.Font = new Font("Segoe UI", 9F);
        lblDefDash.ForeColor = Color.FromArgb(138, 138, 164);
        lblDefDash.Location = new Point(316, 294);
        lblDefDash.Name = "lblDefDash";
        lblDefDash.Size = new Size(16, 18);
        lblDefDash.TabIndex = 30;
        lblDefDash.Text = "–";
        lblDefDash.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // txtMaxDef
        // 
        txtMaxDef.BackColor = Color.FromArgb(24, 24, 36);
        txtMaxDef.BorderStyle = BorderStyle.FixedSingle;
        txtMaxDef.ForeColor = Color.FromArgb(238, 238, 250);
        txtMaxDef.Location = new Point(334, 292);
        txtMaxDef.Name = "txtMaxDef";
        txtMaxDef.PlaceholderText = "Max";
        txtMaxDef.Size = new Size(86, 23);
        txtMaxDef.TabIndex = 31;
        // 
        // btnClear
        // 
        btnClear.BackColor = Color.Transparent;
        btnClear.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnClear.FlatStyle = FlatStyle.Flat;
        btnClear.ForeColor = Color.FromArgb(190, 190, 212);
        btnClear.Location = new Point(20, 336);
        btnClear.Name = "btnClear";
        btnClear.Size = new Size(90, 30);
        btnClear.TabIndex = 32;
        btnClear.Text = "Clear";
        btnClear.UseVisualStyleBackColor = false;
        btnClear.Click += btnClear_Click;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.Transparent;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.ForeColor = Color.FromArgb(190, 190, 212);
        btnCancel.Location = new Point(240, 336);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(90, 30);
        btnCancel.TabIndex = 33;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = false;
        // 
        // btnApply
        // 
        btnApply.BackColor = Color.FromArgb(216, 178, 60);
        btnApply.FlatAppearance.BorderSize = 0;
        btnApply.FlatStyle = FlatStyle.Flat;
        btnApply.ForeColor = Color.FromArgb(18, 18, 28);
        btnApply.Location = new Point(340, 336);
        btnApply.Name = "btnApply";
        btnApply.Size = new Size(90, 30);
        btnApply.TabIndex = 34;
        btnApply.Text = "Apply";
        btnApply.UseVisualStyleBackColor = false;
        btnApply.Click += btnApply_Click;
        // 
        // CardFilterForm
        // 
        AcceptButton = btnApply;
        BackColor = Color.FromArgb(13, 13, 22);
        CancelButton = btnCancel;
        ClientSize = new Size(460, 390);
        Controls.Add(lblNameTitle);
        Controls.Add(txtName);
        Controls.Add(lblIdTitle);
        Controls.Add(txtId);
        Controls.Add(lblTypeTitle);
        Controls.Add(cboType);
        Controls.Add(lblAttrTitle);
        Controls.Add(cboAttribute);
        Controls.Add(lblRaceTitle);
        Controls.Add(cboRace);
        Controls.Add(lblArchetypeTitle);
        Controls.Add(cboArchetype);
        Controls.Add(lblLevelTitle);
        Controls.Add(txtMinLevel);
        Controls.Add(lblLevelDash);
        Controls.Add(txtMaxLevel);
        Controls.Add(lblLinkTitle);
        Controls.Add(txtMinLinkVal);
        Controls.Add(lblLinkDash);
        Controls.Add(txtMaxLinkVal);
        Controls.Add(lblScaleTitle);
        Controls.Add(txtMinScale);
        Controls.Add(lblScaleDash);
        Controls.Add(txtMaxScale);
        Controls.Add(lblAtkTitle);
        Controls.Add(txtMinAtk);
        Controls.Add(lblAtkDash);
        Controls.Add(txtMaxAtk);
        Controls.Add(lblDefTitle);
        Controls.Add(txtMinDef);
        Controls.Add(lblDefDash);
        Controls.Add(txtMaxDef);
        Controls.Add(btnClear);
        Controls.Add(btnCancel);
        Controls.Add(btnApply);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "CardFilterForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Filter Cards";
        ResumeLayout(false);
        PerformLayout();
    }
}
