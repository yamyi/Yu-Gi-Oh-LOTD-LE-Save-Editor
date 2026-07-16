namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

partial class SaveEditorPage
{
    private TabPage tpDuelPoints = null!;
    private TabPage tpCampaign = null!;
    private TabPage tpUnlocks = null!;
    private TabPage tpAvatars = null!;
    private TabPage tpCards = null!;
    private TabControl tabs = null!;

    // ── Duel Points ──────────────────────────────────────────────────────────
    private Label lblDuelPointsTitle = null!;
    private Label lblDuelPointsCurrent = null!;
    private NumericUpDown numDuelPoints = null!;
    private Button btnSetDuelPoints = null!;
    private Label lblDuelPointsHint = null!;

    // ── Campaign ─────────────────────────────────────────────────────────────
    private Label lblCampaignTitle = null!;
    private Label lblCampaignHint = null!;
    private Label lblSeriesPicker = null!;
    private ComboBox cboSeries = null!;
    private Button btnUnlockSeries = null!;
    private Button btnCompleteSeries = null!;
    private Button btnResetSeries = null!;
    private Button btnUnlockAllCampaign = null!;
    private Button btnCompleteAllCampaign = null!;
    private Button btnResetAllCampaign = null!;
    private Label lblDuelsTitle = null!;
    private DataGridView dgvDuels = null!;
    private DataGridViewTextBoxColumn colDuelNum = null!;
    private DataGridViewComboBoxColumn colDuelState = null!;
    private DataGridViewComboBoxColumn colDuelReverse = null!;

    // ── Unlocks ──────────────────────────────────────────────────────────────
    private Button btnUnlockPadlocked = null!;
    private Label lblPadlockHint = null!;
    private Label lblContentTitle = null!;
    private CheckBox chkContentChallenges = null!;
    private CheckBox chkContentBattlePack = null!;
    private CheckBox chkContentCardShop = null!;
    private Label lblBattlePacksTitle = null!;
    private CheckBox chkBpEpicDawn = null!;
    private CheckBox chkBpWotgRound2 = null!;
    private Label lblShopPacksTitle = null!;
    private Label lblShopPacksHint = null!;
    private CheckedListBox clbShopPacks = null!;
    private Label lblTutorialsTitle = null!;
    private CheckedListBox clbTutorials = null!;
    private Label lblChallengesTitle = null!;
    private Button btnUnlockChallenges = null!;
    private Button btnCompleteChallenges = null!;
    private Button btnResetChallenges = null!;
    private Label lblRecipesTitle = null!;
    private Button btnUnlockRecipes = null!;
    private Button btnLockRecipes = null!;

    // ── Avatars ──────────────────────────────────────────────────────────────
    private Label lblAvatarsTitle = null!;
    private Label lblAvatarsHint = null!;
    private Button btnAvatarsAll = null!;
    private Button btnAvatarsNone = null!;
    private CheckedListBox clbAvatars = null!;

    // ── Card Collection ──────────────────────────────────────────────────────
    private Label lblCardsTitle = null!;
    private Label lblCardsHint = null!;
    private Label lblCardsSummary = null!;
    private Button btnUnlockAllCards = null!;
    private Button btnMarkAllSeen = null!;
    private Label lblSetCountTo = null!;
    private NumericUpDown numBulkCount = null!;
    private Button btnApplyCount = null!;
    private Button btnResetCollection = null!;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveEditorPage));
        tpDuelPoints = new TabPage();
        lblDuelPointsTitle = new Label();
        lblDuelPointsCurrent = new Label();
        numDuelPoints = new NumericUpDown();
        btnSetDuelPoints = new Button();
        lblDuelPointsHint = new Label();
        tpCampaign = new TabPage();
        lblCampaignTitle = new Label();
        lblCampaignHint = new Label();
        lblSeriesPicker = new Label();
        cboSeries = new ComboBox();
        btnUnlockSeries = new Button();
        btnCompleteSeries = new Button();
        btnResetSeries = new Button();
        btnUnlockAllCampaign = new Button();
        btnCompleteAllCampaign = new Button();
        btnResetAllCampaign = new Button();
        lblDuelsTitle = new Label();
        dgvDuels = new DataGridView();
        colDuelNum = new DataGridViewTextBoxColumn();
        colDuelState = new DataGridViewComboBoxColumn();
        colDuelReverse = new DataGridViewComboBoxColumn();
        tpUnlocks = new TabPage();
        btnUnlockPadlocked = new Button();
        lblPadlockHint = new Label();
        lblContentTitle = new Label();
        chkContentChallenges = new CheckBox();
        chkContentBattlePack = new CheckBox();
        chkContentCardShop = new CheckBox();
        lblBattlePacksTitle = new Label();
        chkBpEpicDawn = new CheckBox();
        chkBpWotgRound2 = new CheckBox();
        lblShopPacksTitle = new Label();
        lblShopPacksHint = new Label();
        clbShopPacks = new CheckedListBox();
        lblTutorialsTitle = new Label();
        clbTutorials = new CheckedListBox();
        lblChallengesTitle = new Label();
        btnUnlockChallenges = new Button();
        btnCompleteChallenges = new Button();
        btnResetChallenges = new Button();
        lblRecipesTitle = new Label();
        btnUnlockRecipes = new Button();
        btnLockRecipes = new Button();
        tpAvatars = new TabPage();
        lblAvatarsTitle = new Label();
        lblAvatarsHint = new Label();
        btnAvatarsAll = new Button();
        btnAvatarsNone = new Button();
        clbAvatars = new CheckedListBox();
        tpCards = new TabPage();
        lblCardsTitle = new Label();
        lblCardsHint = new Label();
        lblCardsSummary = new Label();
        btnUnlockAllCards = new Button();
        btnMarkAllSeen = new Button();
        lblSetCountTo = new Label();
        numBulkCount = new NumericUpDown();
        btnApplyCount = new Button();
        btnResetCollection = new Button();
        tabs = new TabControl();
        tpDuelPoints.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numDuelPoints).BeginInit();
        tpCampaign.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvDuels).BeginInit();
        tpUnlocks.SuspendLayout();
        tpAvatars.SuspendLayout();
        tpCards.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numBulkCount).BeginInit();
        tabs.SuspendLayout();
        SuspendLayout();
        // 
        // tpDuelPoints
        // 
        tpDuelPoints.AutoScroll = true;
        tpDuelPoints.BackColor = Color.FromArgb(13, 13, 22);
        tpDuelPoints.Controls.Add(lblDuelPointsTitle);
        tpDuelPoints.Controls.Add(lblDuelPointsCurrent);
        tpDuelPoints.Controls.Add(numDuelPoints);
        tpDuelPoints.Controls.Add(btnSetDuelPoints);
        tpDuelPoints.Controls.Add(lblDuelPointsHint);
        tpDuelPoints.Location = new Point(4, 22);
        tpDuelPoints.Name = "tpDuelPoints";
        tpDuelPoints.Padding = new Padding(16);
        tpDuelPoints.Size = new Size(1047, 640);
        tpDuelPoints.TabIndex = 0;
        tpDuelPoints.Text = "Duel Points";
        // 
        // lblDuelPointsTitle
        // 
        lblDuelPointsTitle.AutoSize = true;
        lblDuelPointsTitle.BackColor = Color.Transparent;
        lblDuelPointsTitle.Font = new Font("Segoe UI", 7F);
        lblDuelPointsTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblDuelPointsTitle.Location = new Point(19, 16);
        lblDuelPointsTitle.Name = "lblDuelPointsTitle";
        lblDuelPointsTitle.Size = new Size(66, 12);
        lblDuelPointsTitle.TabIndex = 0;
        lblDuelPointsTitle.Text = "DUEL POINTS";
        // 
        // lblDuelPointsCurrent
        // 
        lblDuelPointsCurrent.AutoSize = true;
        lblDuelPointsCurrent.BackColor = Color.Transparent;
        lblDuelPointsCurrent.Font = new Font("Segoe UI", 10F);
        lblDuelPointsCurrent.ForeColor = Color.FromArgb(238, 238, 250);
        lblDuelPointsCurrent.Location = new Point(19, 41);
        lblDuelPointsCurrent.Name = "lblDuelPointsCurrent";
        lblDuelPointsCurrent.Size = new Size(77, 19);
        lblDuelPointsCurrent.TabIndex = 1;
        lblDuelPointsCurrent.Text = "Current: —";
        // 
        // numDuelPoints
        // 
        numDuelPoints.BackColor = Color.FromArgb(24, 24, 36);
        numDuelPoints.ForeColor = Color.FromArgb(238, 238, 250);
        numDuelPoints.Location = new Point(19, 67);
        numDuelPoints.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
        numDuelPoints.Name = "numDuelPoints";
        numDuelPoints.Size = new Size(140, 23);
        numDuelPoints.TabIndex = 2;
        // 
        // btnSetDuelPoints
        // 
        btnSetDuelPoints.BackColor = Color.Transparent;
        btnSetDuelPoints.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnSetDuelPoints.FlatStyle = FlatStyle.Flat;
        btnSetDuelPoints.Font = new Font("Segoe UI", 8.5F);
        btnSetDuelPoints.ForeColor = Color.FromArgb(190, 190, 212);
        btnSetDuelPoints.Location = new Point(169, 66);
        btnSetDuelPoints.Name = "btnSetDuelPoints";
        btnSetDuelPoints.Size = new Size(70, 25);
        btnSetDuelPoints.TabIndex = 3;
        btnSetDuelPoints.Text = "Set";
        btnSetDuelPoints.UseVisualStyleBackColor = false;
        btnSetDuelPoints.Click += btnSetDuelPoints_Click;
        // 
        // lblDuelPointsHint
        // 
        lblDuelPointsHint.BackColor = Color.Transparent;
        lblDuelPointsHint.Font = new Font("Segoe UI", 8.5F);
        lblDuelPointsHint.ForeColor = Color.FromArgb(138, 138, 164);
        lblDuelPointsHint.Location = new Point(19, 103);
        lblDuelPointsHint.Name = "lblDuelPointsHint";
        lblDuelPointsHint.Size = new Size(760, 32);
        lblDuelPointsHint.TabIndex = 4;
        lblDuelPointsHint.Text = "The game stores this as a plain number — set it to whatever you like.";
        // 
        // tpCampaign
        // 
        tpCampaign.AutoScroll = true;
        tpCampaign.BackColor = Color.FromArgb(13, 13, 22);
        tpCampaign.Controls.Add(lblCampaignTitle);
        tpCampaign.Controls.Add(lblCampaignHint);
        tpCampaign.Controls.Add(lblSeriesPicker);
        tpCampaign.Controls.Add(cboSeries);
        tpCampaign.Controls.Add(btnUnlockSeries);
        tpCampaign.Controls.Add(btnCompleteSeries);
        tpCampaign.Controls.Add(btnResetSeries);
        tpCampaign.Controls.Add(btnUnlockAllCampaign);
        tpCampaign.Controls.Add(btnCompleteAllCampaign);
        tpCampaign.Controls.Add(btnResetAllCampaign);
        tpCampaign.Controls.Add(lblDuelsTitle);
        tpCampaign.Controls.Add(dgvDuels);
        tpCampaign.Location = new Point(4, 22);
        tpCampaign.Name = "tpCampaign";
        tpCampaign.Padding = new Padding(16);
        tpCampaign.Size = new Size(1047, 640);
        tpCampaign.TabIndex = 1;
        tpCampaign.Text = "Campaign";
        // 
        // lblCampaignTitle
        // 
        lblCampaignTitle.AutoSize = true;
        lblCampaignTitle.BackColor = Color.Transparent;
        lblCampaignTitle.Font = new Font("Segoe UI", 7F);
        lblCampaignTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblCampaignTitle.Location = new Point(19, 16);
        lblCampaignTitle.Name = "lblCampaignTitle";
        lblCampaignTitle.Size = new Size(54, 12);
        lblCampaignTitle.TabIndex = 0;
        lblCampaignTitle.Text = "CAMPAIGN";
        // 
        // lblCampaignHint
        // 
        lblCampaignHint.BackColor = Color.Transparent;
        lblCampaignHint.Font = new Font("Segoe UI", 8.5F);
        lblCampaignHint.ForeColor = Color.FromArgb(138, 138, 164);
        lblCampaignHint.Location = new Point(19, 36);
        lblCampaignHint.Name = "lblCampaignHint";
        lblCampaignHint.Size = new Size(1000, 34);
        lblCampaignHint.TabIndex = 1;
        lblCampaignHint.Text = "Duel names aren't stored in the save (they live in the game's own data files) — duels are shown by number. The first duel of a series must stay at least \"Available\" or the series won't open in-game.";
        // 
        // lblSeriesPicker
        // 
        lblSeriesPicker.AutoSize = true;
        lblSeriesPicker.BackColor = Color.Transparent;
        lblSeriesPicker.Font = new Font("Segoe UI", 8.5F);
        lblSeriesPicker.ForeColor = Color.FromArgb(238, 238, 250);
        lblSeriesPicker.Location = new Point(19, 80);
        lblSeriesPicker.Name = "lblSeriesPicker";
        lblSeriesPicker.Size = new Size(40, 15);
        lblSeriesPicker.TabIndex = 2;
        lblSeriesPicker.Text = "Series:";
        // 
        // cboSeries
        // 
        cboSeries.BackColor = Color.FromArgb(24, 24, 36);
        cboSeries.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSeries.ForeColor = Color.FromArgb(238, 238, 250);
        cboSeries.FormattingEnabled = true;
        cboSeries.Location = new Point(79, 77);
        cboSeries.Name = "cboSeries";
        cboSeries.Size = new Size(160, 21);
        cboSeries.TabIndex = 3;
        cboSeries.SelectedIndexChanged += cboSeries_SelectedIndexChanged;
        // 
        // btnUnlockSeries
        // 
        btnUnlockSeries.BackColor = Color.Transparent;
        btnUnlockSeries.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnUnlockSeries.FlatStyle = FlatStyle.Flat;
        btnUnlockSeries.Font = new Font("Segoe UI", 8.5F);
        btnUnlockSeries.ForeColor = Color.FromArgb(190, 190, 212);
        btnUnlockSeries.Location = new Point(249, 76);
        btnUnlockSeries.Name = "btnUnlockSeries";
        btnUnlockSeries.Size = new Size(130, 25);
        btnUnlockSeries.TabIndex = 4;
        btnUnlockSeries.Text = "Unlock this series";
        btnUnlockSeries.UseVisualStyleBackColor = false;
        btnUnlockSeries.Click += btnUnlockSeries_Click;
        // 
        // btnCompleteSeries
        // 
        btnCompleteSeries.BackColor = Color.Transparent;
        btnCompleteSeries.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnCompleteSeries.FlatStyle = FlatStyle.Flat;
        btnCompleteSeries.Font = new Font("Segoe UI", 8.5F);
        btnCompleteSeries.ForeColor = Color.FromArgb(190, 190, 212);
        btnCompleteSeries.Location = new Point(384, 76);
        btnCompleteSeries.Name = "btnCompleteSeries";
        btnCompleteSeries.Size = new Size(140, 25);
        btnCompleteSeries.TabIndex = 5;
        btnCompleteSeries.Text = "Complete this series";
        btnCompleteSeries.UseVisualStyleBackColor = false;
        btnCompleteSeries.Click += btnCompleteSeries_Click;
        // 
        // btnResetSeries
        // 
        btnResetSeries.BackColor = Color.Transparent;
        btnResetSeries.FlatAppearance.BorderColor = Color.FromArgb(98, 54, 54);
        btnResetSeries.FlatStyle = FlatStyle.Flat;
        btnResetSeries.Font = new Font("Segoe UI", 8.5F);
        btnResetSeries.ForeColor = Color.FromArgb(230, 118, 118);
        btnResetSeries.Location = new Point(529, 76);
        btnResetSeries.Name = "btnResetSeries";
        btnResetSeries.Size = new Size(170, 25);
        btnResetSeries.TabIndex = 6;
        btnResetSeries.Text = "Reset this series to locked";
        btnResetSeries.UseVisualStyleBackColor = false;
        btnResetSeries.Click += btnResetSeries_Click;
        // 
        // btnUnlockAllCampaign
        // 
        btnUnlockAllCampaign.BackColor = Color.Transparent;
        btnUnlockAllCampaign.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnUnlockAllCampaign.FlatStyle = FlatStyle.Flat;
        btnUnlockAllCampaign.Font = new Font("Segoe UI", 8.5F);
        btnUnlockAllCampaign.ForeColor = Color.FromArgb(190, 190, 212);
        btnUnlockAllCampaign.Location = new Point(19, 108);
        btnUnlockAllCampaign.Name = "btnUnlockAllCampaign";
        btnUnlockAllCampaign.Size = new Size(160, 25);
        btnUnlockAllCampaign.TabIndex = 7;
        btnUnlockAllCampaign.Text = "Unlock entire campaign";
        btnUnlockAllCampaign.UseVisualStyleBackColor = false;
        btnUnlockAllCampaign.Click += btnUnlockAllCampaign_Click;
        // 
        // btnCompleteAllCampaign
        // 
        btnCompleteAllCampaign.BackColor = Color.Transparent;
        btnCompleteAllCampaign.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnCompleteAllCampaign.FlatStyle = FlatStyle.Flat;
        btnCompleteAllCampaign.Font = new Font("Segoe UI", 8.5F);
        btnCompleteAllCampaign.ForeColor = Color.FromArgb(190, 190, 212);
        btnCompleteAllCampaign.Location = new Point(184, 108);
        btnCompleteAllCampaign.Name = "btnCompleteAllCampaign";
        btnCompleteAllCampaign.Size = new Size(170, 25);
        btnCompleteAllCampaign.TabIndex = 8;
        btnCompleteAllCampaign.Text = "Complete entire campaign";
        btnCompleteAllCampaign.UseVisualStyleBackColor = false;
        btnCompleteAllCampaign.Click += btnCompleteAllCampaign_Click;
        // 
        // btnResetAllCampaign
        // 
        btnResetAllCampaign.BackColor = Color.Transparent;
        btnResetAllCampaign.FlatAppearance.BorderColor = Color.FromArgb(98, 54, 54);
        btnResetAllCampaign.FlatStyle = FlatStyle.Flat;
        btnResetAllCampaign.Font = new Font("Segoe UI", 8.5F);
        btnResetAllCampaign.ForeColor = Color.FromArgb(230, 118, 118);
        btnResetAllCampaign.Location = new Point(359, 108);
        btnResetAllCampaign.Name = "btnResetAllCampaign";
        btnResetAllCampaign.Size = new Size(190, 25);
        btnResetAllCampaign.TabIndex = 9;
        btnResetAllCampaign.Text = "Reset entire campaign to locked";
        btnResetAllCampaign.UseVisualStyleBackColor = false;
        btnResetAllCampaign.Click += btnResetAllCampaign_Click;
        // 
        // lblDuelsTitle
        // 
        lblDuelsTitle.AutoSize = true;
        lblDuelsTitle.BackColor = Color.Transparent;
        lblDuelsTitle.Font = new Font("Segoe UI", 7F);
        lblDuelsTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblDuelsTitle.Location = new Point(19, 140);
        lblDuelsTitle.Name = "lblDuelsTitle";
        lblDuelsTitle.Size = new Size(102, 12);
        lblDuelsTitle.TabIndex = 10;
        lblDuelsTitle.Text = "DUELS IN THIS SERIES";
        // 
        // dgvDuels
        // 
        dgvDuels.AllowUserToAddRows = false;
        dgvDuels.AllowUserToDeleteRows = false;
        dgvDuels.BackgroundColor = Color.FromArgb(24, 24, 36);
        dgvDuels.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvDuels.Columns.AddRange(new DataGridViewColumn[] { colDuelNum, colDuelState, colDuelReverse });
        dgvDuels.Location = new Point(19, 164);
        dgvDuels.Name = "dgvDuels";
        dgvDuels.RowHeadersVisible = false;
        dgvDuels.RowTemplate.Height = 26;
        dgvDuels.Size = new Size(1000, 330);
        dgvDuels.TabIndex = 11;
        dgvDuels.CellValueChanged += dgvDuels_CellValueChanged;
        dgvDuels.CurrentCellDirtyStateChanged += dgvDuels_CurrentCellDirtyStateChanged;
        dgvDuels.DataError += dgvDuels_DataError;
        // 
        // colDuelNum
        // 
        colDuelNum.HeaderText = "Duel";
        colDuelNum.Name = "colDuelNum";
        colDuelNum.ReadOnly = true;
        colDuelNum.Width = 60;
        // 
        // colDuelState
        // 
        colDuelState.HeaderText = "State";
        colDuelState.Items.AddRange(new object[] { "Locked", "Available", "AvailableAttempted", "Complete", "AvailableAlt" });
        colDuelState.Name = "colDuelState";
        colDuelState.Width = 170;
        // 
        // colDuelReverse
        // 
        colDuelReverse.HeaderText = "Reverse";
        colDuelReverse.Items.AddRange(new object[] { "Locked", "Available", "AvailableAttempted", "Complete", "AvailableAlt" });
        colDuelReverse.Name = "colDuelReverse";
        colDuelReverse.Width = 170;
        // 
        // tpUnlocks
        // 
        tpUnlocks.AutoScroll = true;
        tpUnlocks.BackColor = Color.FromArgb(13, 13, 22);
        tpUnlocks.Controls.Add(btnUnlockPadlocked);
        tpUnlocks.Controls.Add(lblPadlockHint);
        tpUnlocks.Controls.Add(lblContentTitle);
        tpUnlocks.Controls.Add(chkContentChallenges);
        tpUnlocks.Controls.Add(chkContentBattlePack);
        tpUnlocks.Controls.Add(chkContentCardShop);
        tpUnlocks.Controls.Add(lblBattlePacksTitle);
        tpUnlocks.Controls.Add(chkBpEpicDawn);
        tpUnlocks.Controls.Add(chkBpWotgRound2);
        tpUnlocks.Controls.Add(lblShopPacksTitle);
        tpUnlocks.Controls.Add(lblShopPacksHint);
        tpUnlocks.Controls.Add(clbShopPacks);
        tpUnlocks.Controls.Add(lblTutorialsTitle);
        tpUnlocks.Controls.Add(clbTutorials);
        tpUnlocks.Controls.Add(lblChallengesTitle);
        tpUnlocks.Controls.Add(btnUnlockChallenges);
        tpUnlocks.Controls.Add(btnCompleteChallenges);
        tpUnlocks.Controls.Add(btnResetChallenges);
        tpUnlocks.Controls.Add(lblRecipesTitle);
        tpUnlocks.Controls.Add(btnUnlockRecipes);
        tpUnlocks.Controls.Add(btnLockRecipes);
        tpUnlocks.Location = new Point(4, 22);
        tpUnlocks.Name = "tpUnlocks";
        tpUnlocks.Padding = new Padding(16);
        tpUnlocks.Size = new Size(1047, 640);
        tpUnlocks.TabIndex = 2;
        tpUnlocks.Text = "Unlocks";
        // 
        // btnUnlockPadlocked
        // 
        btnUnlockPadlocked.BackColor = Color.Transparent;
        btnUnlockPadlocked.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnUnlockPadlocked.FlatStyle = FlatStyle.Flat;
        btnUnlockPadlocked.Font = new Font("Segoe UI", 8.5F);
        btnUnlockPadlocked.ForeColor = Color.FromArgb(190, 190, 212);
        btnUnlockPadlocked.Location = new Point(19, 16);
        btnUnlockPadlocked.Name = "btnUnlockPadlocked";
        btnUnlockPadlocked.Size = new Size(220, 26);
        btnUnlockPadlocked.TabIndex = 0;
        btnUnlockPadlocked.Text = "Unlock all padlocked content";
        btnUnlockPadlocked.UseVisualStyleBackColor = false;
        btnUnlockPadlocked.Click += btnUnlockPadlocked_Click;
        // 
        // lblPadlockHint
        // 
        lblPadlockHint.BackColor = Color.Transparent;
        lblPadlockHint.Font = new Font("Segoe UI", 8.5F);
        lblPadlockHint.ForeColor = Color.FromArgb(138, 138, 164);
        lblPadlockHint.Location = new Point(19, 49);
        lblPadlockHint.Name = "lblPadlockHint";
        lblPadlockHint.Size = new Size(1000, 30);
        lblPadlockHint.TabIndex = 1;
        lblPadlockHint.Text = "Unlocks duelist challenges, the battle pack menu, and the card shop menu all at once — the flags normally locked until you clear the early campaign.";
        // 
        // lblContentTitle
        // 
        lblContentTitle.AutoSize = true;
        lblContentTitle.BackColor = Color.Transparent;
        lblContentTitle.Font = new Font("Segoe UI", 7F);
        lblContentTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblContentTitle.Location = new Point(19, 83);
        lblContentTitle.Name = "lblContentTitle";
        lblContentTitle.Size = new Size(102, 12);
        lblContentTitle.TabIndex = 2;
        lblContentTitle.Text = "UNLOCKED CONTENT";
        // 
        // chkContentChallenges
        // 
        chkContentChallenges.AutoSize = true;
        chkContentChallenges.BackColor = Color.Transparent;
        chkContentChallenges.Font = new Font("Segoe UI", 8.5F);
        chkContentChallenges.ForeColor = Color.FromArgb(238, 238, 250);
        chkContentChallenges.Location = new Point(19, 105);
        chkContentChallenges.Name = "chkContentChallenges";
        chkContentChallenges.Size = new Size(123, 19);
        chkContentChallenges.TabIndex = 3;
        chkContentChallenges.Text = "Duelist Challenges";
        chkContentChallenges.UseVisualStyleBackColor = false;
        chkContentChallenges.CheckedChanged += chkContentChallenges_CheckedChanged;
        // 
        // chkContentBattlePack
        // 
        chkContentBattlePack.AutoSize = true;
        chkContentBattlePack.BackColor = Color.Transparent;
        chkContentBattlePack.Font = new Font("Segoe UI", 8.5F);
        chkContentBattlePack.ForeColor = Color.FromArgb(238, 238, 250);
        chkContentBattlePack.Location = new Point(219, 105);
        chkContentBattlePack.Name = "chkContentBattlePack";
        chkContentBattlePack.Size = new Size(118, 19);
        chkContentBattlePack.TabIndex = 4;
        chkContentBattlePack.Text = "Battle Pack menu";
        chkContentBattlePack.UseVisualStyleBackColor = false;
        chkContentBattlePack.CheckedChanged += chkContentBattlePack_CheckedChanged;
        // 
        // chkContentCardShop
        // 
        chkContentCardShop.AutoSize = true;
        chkContentCardShop.BackColor = Color.Transparent;
        chkContentCardShop.Font = new Font("Segoe UI", 8.5F);
        chkContentCardShop.ForeColor = Color.FromArgb(238, 238, 250);
        chkContentCardShop.Location = new Point(419, 105);
        chkContentCardShop.Name = "chkContentCardShop";
        chkContentCardShop.Size = new Size(115, 19);
        chkContentCardShop.TabIndex = 5;
        chkContentCardShop.Text = "Card Shop menu";
        chkContentCardShop.UseVisualStyleBackColor = false;
        chkContentCardShop.CheckedChanged += chkContentCardShop_CheckedChanged;
        // 
        // lblBattlePacksTitle
        // 
        lblBattlePacksTitle.AutoSize = true;
        lblBattlePacksTitle.BackColor = Color.Transparent;
        lblBattlePacksTitle.Font = new Font("Segoe UI", 7F);
        lblBattlePacksTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblBattlePacksTitle.Location = new Point(19, 135);
        lblBattlePacksTitle.Name = "lblBattlePacksTitle";
        lblBattlePacksTitle.Size = new Size(116, 12);
        lblBattlePacksTitle.TabIndex = 6;
        lblBattlePacksTitle.Text = "BATTLE PACKS AVAILABLE";
        // 
        // chkBpEpicDawn
        // 
        chkBpEpicDawn.AutoSize = true;
        chkBpEpicDawn.BackColor = Color.Transparent;
        chkBpEpicDawn.Font = new Font("Segoe UI", 8.5F);
        chkBpEpicDawn.ForeColor = Color.FromArgb(238, 238, 250);
        chkBpEpicDawn.Location = new Point(19, 157);
        chkBpEpicDawn.Name = "chkBpEpicDawn";
        chkBpEpicDawn.Size = new Size(145, 19);
        chkBpEpicDawn.TabIndex = 7;
        chkBpEpicDawn.Text = "Battle Pack: Epic Dawn";
        chkBpEpicDawn.UseVisualStyleBackColor = false;
        chkBpEpicDawn.CheckedChanged += chkBpEpicDawn_CheckedChanged;
        // 
        // chkBpWotgRound2
        // 
        chkBpWotgRound2.AutoSize = true;
        chkBpWotgRound2.BackColor = Color.Transparent;
        chkBpWotgRound2.Font = new Font("Segoe UI", 8.5F);
        chkBpWotgRound2.ForeColor = Color.FromArgb(238, 238, 250);
        chkBpWotgRound2.Location = new Point(289, 157);
        chkBpWotgRound2.Name = "chkBpWotgRound2";
        chkBpWotgRound2.Size = new Size(237, 19);
        chkBpWotgRound2.TabIndex = 8;
        chkBpWotgRound2.Text = "Battle Pack 2: War of the Giants Round 2";
        chkBpWotgRound2.UseVisualStyleBackColor = false;
        chkBpWotgRound2.CheckedChanged += chkBpWotgRound2_CheckedChanged;
        // 
        // lblShopPacksTitle
        // 
        lblShopPacksTitle.AutoSize = true;
        lblShopPacksTitle.BackColor = Color.Transparent;
        lblShopPacksTitle.Font = new Font("Segoe UI", 7F);
        lblShopPacksTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblShopPacksTitle.Location = new Point(19, 187);
        lblShopPacksTitle.Name = "lblShopPacksTitle";
        lblShopPacksTitle.Size = new Size(111, 12);
        lblShopPacksTitle.TabIndex = 9;
        lblShopPacksTitle.Text = "SHOP PACKS AVAILABLE";
        // 
        // lblShopPacksHint
        // 
        lblShopPacksHint.BackColor = Color.Transparent;
        lblShopPacksHint.Font = new Font("Segoe UI", 8.5F);
        lblShopPacksHint.ForeColor = Color.FromArgb(138, 138, 164);
        lblShopPacksHint.Location = new Point(19, 207);
        lblShopPacksHint.Name = "lblShopPacksHint";
        lblShopPacksHint.Size = new Size(480, 30);
        lblShopPacksHint.TabIndex = 10;
        lblShopPacksHint.Text = "Tied to a duelist each. Any of Pendulum / Gong Strong / Zuzu Boyle unlocks all ARC-V packs together.";
        // 
        // clbShopPacks
        // 
        clbShopPacks.BackColor = Color.FromArgb(24, 24, 36);
        clbShopPacks.CheckOnClick = true;
        clbShopPacks.ForeColor = Color.FromArgb(238, 238, 250);
        clbShopPacks.Location = new Point(19, 241);
        clbShopPacks.Name = "clbShopPacks";
        clbShopPacks.Size = new Size(480, 256);
        clbShopPacks.TabIndex = 11;
        clbShopPacks.ItemCheck += clbShopPacks_ItemCheck;
        // 
        // lblTutorialsTitle
        // 
        lblTutorialsTitle.AutoSize = true;
        lblTutorialsTitle.BackColor = Color.Transparent;
        lblTutorialsTitle.Font = new Font("Segoe UI", 7F);
        lblTutorialsTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblTutorialsTitle.Location = new Point(519, 187);
        lblTutorialsTitle.Name = "lblTutorialsTitle";
        lblTutorialsTitle.Size = new Size(114, 12);
        lblTutorialsTitle.TabIndex = 12;
        lblTutorialsTitle.Text = "COMPLETED TUTORIALS";
        // 
        // clbTutorials
        // 
        clbTutorials.BackColor = Color.FromArgb(24, 24, 36);
        clbTutorials.CheckOnClick = true;
        clbTutorials.ForeColor = Color.FromArgb(238, 238, 250);
        clbTutorials.Location = new Point(519, 243);
        clbTutorials.Name = "clbTutorials";
        clbTutorials.Size = new Size(480, 256);
        clbTutorials.TabIndex = 13;
        clbTutorials.ItemCheck += clbTutorials_ItemCheck;
        // 
        // lblChallengesTitle
        // 
        lblChallengesTitle.AutoSize = true;
        lblChallengesTitle.BackColor = Color.Transparent;
        lblChallengesTitle.Font = new Font("Segoe UI", 7F);
        lblChallengesTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblChallengesTitle.Location = new Point(19, 513);
        lblChallengesTitle.Name = "lblChallengesTitle";
        lblChallengesTitle.Size = new Size(266, 12);
        lblChallengesTitle.TabIndex = 14;
        lblChallengesTitle.Text = "DUELIST CHALLENGES (BULK — NOT NAMED IN THE SAVE)";
        // 
        // btnUnlockChallenges
        // 
        btnUnlockChallenges.BackColor = Color.Transparent;
        btnUnlockChallenges.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnUnlockChallenges.FlatStyle = FlatStyle.Flat;
        btnUnlockChallenges.Font = new Font("Segoe UI", 8.5F);
        btnUnlockChallenges.ForeColor = Color.FromArgb(190, 190, 212);
        btnUnlockChallenges.Location = new Point(19, 535);
        btnUnlockChallenges.Name = "btnUnlockChallenges";
        btnUnlockChallenges.Size = new Size(150, 25);
        btnUnlockChallenges.TabIndex = 15;
        btnUnlockChallenges.Text = "Unlock all challenges";
        btnUnlockChallenges.UseVisualStyleBackColor = false;
        btnUnlockChallenges.Click += btnUnlockChallenges_Click;
        // 
        // btnCompleteChallenges
        // 
        btnCompleteChallenges.BackColor = Color.Transparent;
        btnCompleteChallenges.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnCompleteChallenges.FlatStyle = FlatStyle.Flat;
        btnCompleteChallenges.Font = new Font("Segoe UI", 8.5F);
        btnCompleteChallenges.ForeColor = Color.FromArgb(190, 190, 212);
        btnCompleteChallenges.Location = new Point(179, 535);
        btnCompleteChallenges.Name = "btnCompleteChallenges";
        btnCompleteChallenges.Size = new Size(150, 25);
        btnCompleteChallenges.TabIndex = 16;
        btnCompleteChallenges.Text = "Complete all challenges";
        btnCompleteChallenges.UseVisualStyleBackColor = false;
        btnCompleteChallenges.Click += btnCompleteChallenges_Click;
        // 
        // btnResetChallenges
        // 
        btnResetChallenges.BackColor = Color.Transparent;
        btnResetChallenges.FlatAppearance.BorderColor = Color.FromArgb(98, 54, 54);
        btnResetChallenges.FlatStyle = FlatStyle.Flat;
        btnResetChallenges.Font = new Font("Segoe UI", 8.5F);
        btnResetChallenges.ForeColor = Color.FromArgb(230, 118, 118);
        btnResetChallenges.Location = new Point(339, 535);
        btnResetChallenges.Name = "btnResetChallenges";
        btnResetChallenges.Size = new Size(180, 25);
        btnResetChallenges.TabIndex = 17;
        btnResetChallenges.Text = "Reset all challenges to locked";
        btnResetChallenges.UseVisualStyleBackColor = false;
        btnResetChallenges.Click += btnResetChallenges_Click;
        // 
        // lblRecipesTitle
        // 
        lblRecipesTitle.AutoSize = true;
        lblRecipesTitle.BackColor = Color.Transparent;
        lblRecipesTitle.Font = new Font("Segoe UI", 7F);
        lblRecipesTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblRecipesTitle.Location = new Point(19, 571);
        lblRecipesTitle.Name = "lblRecipesTitle";
        lblRecipesTitle.Size = new Size(101, 12);
        lblRecipesTitle.TabIndex = 18;
        lblRecipesTitle.Text = "DECK RECIPES (BULK)";
        // 
        // btnUnlockRecipes
        // 
        btnUnlockRecipes.BackColor = Color.Transparent;
        btnUnlockRecipes.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnUnlockRecipes.FlatStyle = FlatStyle.Flat;
        btnUnlockRecipes.Font = new Font("Segoe UI", 8.5F);
        btnUnlockRecipes.ForeColor = Color.FromArgb(190, 190, 212);
        btnUnlockRecipes.Location = new Point(19, 593);
        btnUnlockRecipes.Name = "btnUnlockRecipes";
        btnUnlockRecipes.Size = new Size(170, 25);
        btnUnlockRecipes.TabIndex = 19;
        btnUnlockRecipes.Text = "Unlock all deck recipes";
        btnUnlockRecipes.UseVisualStyleBackColor = false;
        btnUnlockRecipes.Click += btnUnlockRecipes_Click;
        // 
        // btnLockRecipes
        // 
        btnLockRecipes.BackColor = Color.Transparent;
        btnLockRecipes.FlatAppearance.BorderColor = Color.FromArgb(98, 54, 54);
        btnLockRecipes.FlatStyle = FlatStyle.Flat;
        btnLockRecipes.Font = new Font("Segoe UI", 8.5F);
        btnLockRecipes.ForeColor = Color.FromArgb(230, 118, 118);
        btnLockRecipes.Location = new Point(195, 590);
        btnLockRecipes.Name = "btnLockRecipes";
        btnLockRecipes.Size = new Size(160, 25);
        btnLockRecipes.TabIndex = 20;
        btnLockRecipes.Text = "Lock all deck recipes";
        btnLockRecipes.UseVisualStyleBackColor = false;
        btnLockRecipes.Click += btnLockRecipes_Click;
        // 
        // tpAvatars
        // 
        tpAvatars.AutoScroll = true;
        tpAvatars.BackColor = Color.FromArgb(13, 13, 22);
        tpAvatars.Controls.Add(lblAvatarsTitle);
        tpAvatars.Controls.Add(lblAvatarsHint);
        tpAvatars.Controls.Add(btnAvatarsAll);
        tpAvatars.Controls.Add(btnAvatarsNone);
        tpAvatars.Controls.Add(clbAvatars);
        tpAvatars.Location = new Point(4, 22);
        tpAvatars.Name = "tpAvatars";
        tpAvatars.Padding = new Padding(16);
        tpAvatars.Size = new Size(1047, 640);
        tpAvatars.TabIndex = 3;
        tpAvatars.Text = "Avatars";
        // 
        // lblAvatarsTitle
        // 
        lblAvatarsTitle.AutoSize = true;
        lblAvatarsTitle.BackColor = Color.Transparent;
        lblAvatarsTitle.Font = new Font("Segoe UI", 7F);
        lblAvatarsTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblAvatarsTitle.Location = new Point(19, 16);
        lblAvatarsTitle.Name = "lblAvatarsTitle";
        lblAvatarsTitle.Size = new Size(95, 12);
        lblAvatarsTitle.TabIndex = 0;
        lblAvatarsTitle.Text = "UNLOCKED AVATARS";
        // 
        // lblAvatarsHint
        // 
        lblAvatarsHint.BackColor = Color.Transparent;
        lblAvatarsHint.Font = new Font("Segoe UI", 8.5F);
        lblAvatarsHint.ForeColor = Color.FromArgb(138, 138, 164);
        lblAvatarsHint.Location = new Point(19, 36);
        lblAvatarsHint.Name = "lblAvatarsHint";
        lblAvatarsHint.Size = new Size(1000, 40);
        lblAvatarsHint.TabIndex = 1;
        lblAvatarsHint.Text = "Avatars you can assign to a deck slot as its duelist portrait. This is a separate unlock flag from the deck-slot owner picker.";
        // 
        // btnAvatarsAll
        // 
        btnAvatarsAll.BackColor = Color.Transparent;
        btnAvatarsAll.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnAvatarsAll.FlatStyle = FlatStyle.Flat;
        btnAvatarsAll.Font = new Font("Segoe UI", 8.5F);
        btnAvatarsAll.ForeColor = Color.FromArgb(190, 190, 212);
        btnAvatarsAll.Location = new Point(19, 80);
        btnAvatarsAll.Name = "btnAvatarsAll";
        btnAvatarsAll.Size = new Size(90, 25);
        btnAvatarsAll.TabIndex = 2;
        btnAvatarsAll.Text = "Unlock all";
        btnAvatarsAll.UseVisualStyleBackColor = false;
        btnAvatarsAll.Click += btnAvatarsAll_Click;
        // 
        // btnAvatarsNone
        // 
        btnAvatarsNone.BackColor = Color.Transparent;
        btnAvatarsNone.FlatAppearance.BorderColor = Color.FromArgb(98, 54, 54);
        btnAvatarsNone.FlatStyle = FlatStyle.Flat;
        btnAvatarsNone.Font = new Font("Segoe UI", 8.5F);
        btnAvatarsNone.ForeColor = Color.FromArgb(230, 118, 118);
        btnAvatarsNone.Location = new Point(119, 80);
        btnAvatarsNone.Name = "btnAvatarsNone";
        btnAvatarsNone.Size = new Size(90, 25);
        btnAvatarsNone.TabIndex = 3;
        btnAvatarsNone.Text = "Lock all";
        btnAvatarsNone.UseVisualStyleBackColor = false;
        btnAvatarsNone.Click += btnAvatarsNone_Click;
        // 
        // clbAvatars
        // 
        clbAvatars.BackColor = Color.FromArgb(24, 24, 36);
        clbAvatars.CheckOnClick = true;
        clbAvatars.ColumnWidth = 220;
        clbAvatars.ForeColor = Color.FromArgb(238, 238, 250);
        clbAvatars.Location = new Point(19, 122);
        clbAvatars.MultiColumn = true;
        clbAvatars.Name = "clbAvatars";
        clbAvatars.Size = new Size(1009, 490);
        clbAvatars.TabIndex = 4;
        clbAvatars.ItemCheck += clbAvatars_ItemCheck;
        // 
        // tpCards
        // 
        tpCards.AutoScroll = true;
        tpCards.BackColor = Color.FromArgb(13, 13, 22);
        tpCards.Controls.Add(lblCardsTitle);
        tpCards.Controls.Add(lblCardsHint);
        tpCards.Controls.Add(lblCardsSummary);
        tpCards.Controls.Add(btnUnlockAllCards);
        tpCards.Controls.Add(btnMarkAllSeen);
        tpCards.Controls.Add(lblSetCountTo);
        tpCards.Controls.Add(numBulkCount);
        tpCards.Controls.Add(btnApplyCount);
        tpCards.Controls.Add(btnResetCollection);
        tpCards.Location = new Point(4, 22);
        tpCards.Name = "tpCards";
        tpCards.Padding = new Padding(16);
        tpCards.Size = new Size(1047, 640);
        tpCards.TabIndex = 4;
        tpCards.Text = "Card Collection";
        // 
        // lblCardsTitle
        // 
        lblCardsTitle.AutoSize = true;
        lblCardsTitle.BackColor = Color.Transparent;
        lblCardsTitle.Font = new Font("Segoe UI", 7F);
        lblCardsTitle.ForeColor = Color.FromArgb(92, 92, 116);
        lblCardsTitle.Location = new Point(19, 16);
        lblCardsTitle.Name = "lblCardsTitle";
        lblCardsTitle.Size = new Size(91, 12);
        lblCardsTitle.TabIndex = 0;
        lblCardsTitle.Text = "CARD COLLECTION";
        // 
        // lblCardsHint
        // 
        lblCardsHint.BackColor = Color.Transparent;
        lblCardsHint.Font = new Font("Segoe UI", 8.5F);
        lblCardsHint.ForeColor = Color.FromArgb(138, 138, 164);
        lblCardsHint.Location = new Point(19, 36);
        lblCardsHint.Name = "lblCardsHint";
        lblCardsHint.Size = new Size(1000, 40);
        lblCardsHint.TabIndex = 1;
        lblCardsHint.Text = resources.GetString("lblCardsHint.Text");
        // 
        // lblCardsSummary
        // 
        lblCardsSummary.AutoSize = true;
        lblCardsSummary.BackColor = Color.Transparent;
        lblCardsSummary.Font = new Font("Segoe UI", 10F);
        lblCardsSummary.ForeColor = Color.FromArgb(238, 238, 250);
        lblCardsSummary.Location = new Point(19, 80);
        lblCardsSummary.Name = "lblCardsSummary";
        lblCardsSummary.Size = new Size(286, 19);
        lblCardsSummary.TabIndex = 2;
        lblCardsSummary.Text = "— / — card slots have at least 1 copy owned.";
        // 
        // btnUnlockAllCards
        // 
        btnUnlockAllCards.BackColor = Color.Transparent;
        btnUnlockAllCards.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnUnlockAllCards.FlatStyle = FlatStyle.Flat;
        btnUnlockAllCards.Font = new Font("Segoe UI", 8.5F);
        btnUnlockAllCards.ForeColor = Color.FromArgb(190, 190, 212);
        btnUnlockAllCards.Location = new Point(19, 108);
        btnUnlockAllCards.Name = "btnUnlockAllCards";
        btnUnlockAllCards.Size = new Size(270, 26);
        btnUnlockAllCards.TabIndex = 3;
        btnUnlockAllCards.Text = "Unlock all cards (3 copies, marked seen)";
        btnUnlockAllCards.UseVisualStyleBackColor = false;
        btnUnlockAllCards.Click += btnUnlockAllCards_Click;
        // 
        // btnMarkAllSeen
        // 
        btnMarkAllSeen.BackColor = Color.Transparent;
        btnMarkAllSeen.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnMarkAllSeen.FlatStyle = FlatStyle.Flat;
        btnMarkAllSeen.Font = new Font("Segoe UI", 8.5F);
        btnMarkAllSeen.ForeColor = Color.FromArgb(190, 190, 212);
        btnMarkAllSeen.Location = new Point(299, 108);
        btnMarkAllSeen.Name = "btnMarkAllSeen";
        btnMarkAllSeen.Size = new Size(270, 26);
        btnMarkAllSeen.TabIndex = 4;
        btnMarkAllSeen.Text = "Mark all cards as seen (clear \"NEW\" tags)";
        btnMarkAllSeen.UseVisualStyleBackColor = false;
        btnMarkAllSeen.Click += btnMarkAllSeen_Click;
        // 
        // lblSetCountTo
        // 
        lblSetCountTo.AutoSize = true;
        lblSetCountTo.BackColor = Color.Transparent;
        lblSetCountTo.Font = new Font("Segoe UI", 8.5F);
        lblSetCountTo.ForeColor = Color.FromArgb(238, 238, 250);
        lblSetCountTo.Location = new Point(19, 148);
        lblSetCountTo.Name = "lblSetCountTo";
        lblSetCountTo.Size = new Size(178, 15);
        lblSetCountTo.TabIndex = 5;
        lblSetCountTo.Text = "Set every card's owned count to:";
        // 
        // numBulkCount
        // 
        numBulkCount.BackColor = Color.FromArgb(24, 24, 36);
        numBulkCount.ForeColor = Color.FromArgb(238, 238, 250);
        numBulkCount.Location = new Point(249, 146);
        numBulkCount.Maximum = new decimal(new int[] { 3, 0, 0, 0 });
        numBulkCount.Name = "numBulkCount";
        numBulkCount.Size = new Size(60, 23);
        numBulkCount.TabIndex = 6;
        numBulkCount.Value = new decimal(new int[] { 3, 0, 0, 0 });
        // 
        // btnApplyCount
        // 
        btnApplyCount.BackColor = Color.Transparent;
        btnApplyCount.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnApplyCount.FlatStyle = FlatStyle.Flat;
        btnApplyCount.Font = new Font("Segoe UI", 8.5F);
        btnApplyCount.ForeColor = Color.FromArgb(190, 190, 212);
        btnApplyCount.Location = new Point(315, 145);
        btnApplyCount.Name = "btnApplyCount";
        btnApplyCount.Size = new Size(70, 25);
        btnApplyCount.TabIndex = 7;
        btnApplyCount.Text = "Apply";
        btnApplyCount.UseVisualStyleBackColor = false;
        btnApplyCount.Click += btnApplyCount_Click;
        // 
        // btnResetCollection
        // 
        btnResetCollection.BackColor = Color.Transparent;
        btnResetCollection.FlatAppearance.BorderColor = Color.FromArgb(98, 54, 54);
        btnResetCollection.FlatStyle = FlatStyle.Flat;
        btnResetCollection.Font = new Font("Segoe UI", 8.5F);
        btnResetCollection.ForeColor = Color.FromArgb(230, 118, 118);
        btnResetCollection.Location = new Point(19, 188);
        btnResetCollection.Name = "btnResetCollection";
        btnResetCollection.Size = new Size(300, 26);
        btnResetCollection.TabIndex = 8;
        btnResetCollection.Text = "Reset entire collection (0 copies, unseen)";
        btnResetCollection.UseVisualStyleBackColor = false;
        btnResetCollection.Click += btnResetCollection_Click;
        // 
        // tabs
        // 
        tabs.Controls.Add(tpDuelPoints);
        tabs.Controls.Add(tpCampaign);
        tabs.Controls.Add(tpUnlocks);
        tabs.Controls.Add(tpAvatars);
        tabs.Controls.Add(tpCards);
        tabs.Dock = DockStyle.Fill;
        tabs.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        tabs.Location = new Point(0, 0);
        tabs.Multiline = true;
        tabs.Name = "tabs";
        tabs.SelectedIndex = 0;
        tabs.Size = new Size(1055, 666);
        tabs.TabIndex = 0;
        // 
        // SaveEditorPage
        // 
        BackColor = Color.FromArgb(13, 13, 22);
        Controls.Add(tabs);
        Name = "SaveEditorPage";
        Size = new Size(1055, 666);
        tpDuelPoints.ResumeLayout(false);
        tpDuelPoints.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numDuelPoints).EndInit();
        tpCampaign.ResumeLayout(false);
        tpCampaign.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvDuels).EndInit();
        tpUnlocks.ResumeLayout(false);
        tpUnlocks.PerformLayout();
        tpAvatars.ResumeLayout(false);
        tpAvatars.PerformLayout();
        tpCards.ResumeLayout(false);
        tpCards.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numBulkCount).EndInit();
        tabs.ResumeLayout(false);
        ResumeLayout(false);
    }
}
