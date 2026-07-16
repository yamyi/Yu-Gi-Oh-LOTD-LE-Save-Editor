namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

partial class DetailPanel
{
    // ── Named controls referenced from DetailPanel.cs ─────────────────────────
    private Panel pnlEmpty;
    // Portrait
    private Panel pnlPortrait;
    private PictureBox picPortrait;
    private Label lblOwnerName;
    private Label lblOwnerId;
    // Header
    private Panel pnlHeader;
    private Label lblHeaderTag;
    private Label lblDeckName;
    private Label lblSlotNum;
    // Stats
    private Panel pnlStats;
    private Panel pnlMainBarRow;
    private Panel pnlMainTrack;
    private Panel pnlMainFill;
    private Label lblMainBar;
    private Panel pnlExtraBarRow;
    private Panel pnlExtraTrack;
    private Panel pnlExtraFill;
    private Label lblExtraBar;
    private Panel pnlSideBarRow;
    private Panel pnlSideTrack;
    private Panel pnlSideFill;
    private Label lblSideBar;
    // Actions
    private Panel pnlActions;
    private Button btnEditCards;
    private Button btnRename;
    private Button btnCopyTo;
    private Button btnChangeDuelist;
    private Button btnSwapWith;
    private Button btnImportYdk;
    private Button btnExportYdk;
    private Button btnClearSlot;

    private void InitializeComponent()
    {
        pnlEmpty = new Panel();
        lblEmptyHint = new Label();
        tableLayoutPanel1 = new TableLayoutPanel();
        pnlActions = new Panel();
        pnlActionsScroll = new Panel();
        lblActionsTitle = new Label();
        btnEditCards = new Button();
        btnChangeDuelist = new Button();
        btnRename = new Button();
        btnCopyTo = new Button();
        btnSwapWith = new Button();
        btnImportYdk = new Button();
        btnExportYdk = new Button();
        lblDangerTitle = new Label();
        btnClearSlot = new Button();
        pnlStats = new Panel();
        lblCountsTitle = new Label();
        pnlMainBarRow = new Panel();
        lblMainName = new Label();
        lblMainBar = new Label();
        pnlMainTrack = new Panel();
        pnlMainFill = new Panel();
        pnlExtraBarRow = new Panel();
        lblExtraName = new Label();
        lblExtraBar = new Label();
        pnlExtraTrack = new Panel();
        pnlExtraFill = new Panel();
        pnlSideBarRow = new Panel();
        lblSideName = new Label();
        lblSideBar = new Label();
        pnlSideTrack = new Panel();
        pnlSideFill = new Panel();
        pnlStatsDivider = new Panel();
        pnlHeader = new Panel();
        lblHeaderTag = new Label();
        lblOwnerName = new Label();
        pnlHeaderDivider = new Panel();
        lblOwnerId = new Label();
        pnlPortraitDivider = new Panel();
        pnlPortrait = new Panel();
        picPortrait = new PictureBox();
        lblSlotNum = new Label();
        lblDeckName = new Label();
        leftBorder = new Panel();
        pnlEmpty.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        pnlActions.SuspendLayout();
        pnlActionsScroll.SuspendLayout();
        pnlStats.SuspendLayout();
        pnlMainBarRow.SuspendLayout();
        pnlMainTrack.SuspendLayout();
        pnlExtraBarRow.SuspendLayout();
        pnlExtraTrack.SuspendLayout();
        pnlSideBarRow.SuspendLayout();
        pnlSideTrack.SuspendLayout();
        pnlHeader.SuspendLayout();
        pnlPortrait.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picPortrait).BeginInit();
        SuspendLayout();
        // 
        // pnlEmpty
        // 
        pnlEmpty.BackColor = Color.Transparent;
        pnlEmpty.Controls.Add(lblEmptyHint);
        pnlEmpty.Dock = DockStyle.Fill;
        pnlEmpty.Location = new Point(0, 0);
        pnlEmpty.Name = "pnlEmpty";
        pnlEmpty.Size = new Size(246, 654);
        pnlEmpty.TabIndex = 1;
        // 
        // lblEmptyHint
        // 
        lblEmptyHint.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblEmptyHint.BackColor = Color.Transparent;
        lblEmptyHint.Font = new Font("Segoe UI", 9F);
        lblEmptyHint.ForeColor = Color.FromArgb(58, 58, 90);
        lblEmptyHint.Location = new Point(0, 0);
        lblEmptyHint.Name = "lblEmptyHint";
        lblEmptyHint.Size = new Size(246, 654);
        lblEmptyHint.TabIndex = 0;
        lblEmptyHint.Text = "Select a deck slot\nto view details";
        lblEmptyHint.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 1;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Controls.Add(pnlActions, 0, 4);
        tableLayoutPanel1.Controls.Add(pnlStats, 0, 3);
        tableLayoutPanel1.Controls.Add(pnlHeader, 0, 2);
        tableLayoutPanel1.Controls.Add(pnlPortraitDivider, 0, 1);
        tableLayoutPanel1.Controls.Add(pnlPortrait, 0, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 6;
        tableLayoutPanel1.RowStyles.Add(new RowStyle());
        tableLayoutPanel1.RowStyles.Add(new RowStyle());
        tableLayoutPanel1.RowStyles.Add(new RowStyle());
        tableLayoutPanel1.RowStyles.Add(new RowStyle());
        tableLayoutPanel1.RowStyles.Add(new RowStyle());
        tableLayoutPanel1.RowStyles.Add(new RowStyle());
        tableLayoutPanel1.Size = new Size(246, 654);
        tableLayoutPanel1.TabIndex = 3;
        // 
        // pnlActions
        // 
        pnlActions.AutoScroll = true;
        pnlActions.BackColor = Color.Transparent;
        pnlActions.Controls.Add(pnlActionsScroll);
        pnlActions.Location = new Point(3, 321);
        pnlActions.Name = "pnlActions";
        pnlActions.Size = new Size(240, 333);
        pnlActions.TabIndex = 4;
        // 
        // pnlActionsScroll
        // 
        pnlActionsScroll.AutoScroll = true;
        pnlActionsScroll.BackColor = Color.Transparent;
        pnlActionsScroll.Controls.Add(lblActionsTitle);
        pnlActionsScroll.Controls.Add(btnEditCards);
        pnlActionsScroll.Controls.Add(btnChangeDuelist);
        pnlActionsScroll.Controls.Add(btnRename);
        pnlActionsScroll.Controls.Add(btnCopyTo);
        pnlActionsScroll.Controls.Add(btnSwapWith);
        pnlActionsScroll.Controls.Add(btnImportYdk);
        pnlActionsScroll.Controls.Add(btnExportYdk);
        pnlActionsScroll.Controls.Add(lblDangerTitle);
        pnlActionsScroll.Controls.Add(btnClearSlot);
        pnlActionsScroll.Dock = DockStyle.Fill;
        pnlActionsScroll.Location = new Point(0, 0);
        pnlActionsScroll.Name = "pnlActionsScroll";
        pnlActionsScroll.Size = new Size(240, 333);
        pnlActionsScroll.TabIndex = 0;
        // 
        // lblActionsTitle
        // 
        lblActionsTitle.BackColor = Color.Transparent;
        lblActionsTitle.Font = new Font("Segoe UI", 7F);
        lblActionsTitle.ForeColor = Color.FromArgb(58, 58, 90);
        lblActionsTitle.Location = new Point(10, 0);
        lblActionsTitle.Name = "lblActionsTitle";
        lblActionsTitle.Size = new Size(200, 20);
        lblActionsTitle.TabIndex = 0;
        lblActionsTitle.Text = "ACTIONS";
        // 
        // btnEditCards
        // 
        btnEditCards.BackColor = Color.FromArgb(15, 15, 26);
        btnEditCards.Cursor = Cursors.Hand;
        btnEditCards.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 62);
        btnEditCards.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 22, 42);
        btnEditCards.FlatStyle = FlatStyle.Flat;
        btnEditCards.Font = new Font("Segoe UI", 9F);
        btnEditCards.ForeColor = Color.FromArgb(192, 192, 216);
        btnEditCards.Location = new Point(25, 24);
        btnEditCards.Name = "btnEditCards";
        btnEditCards.Padding = new Padding(6, 0, 0, 0);
        btnEditCards.Size = new Size(200, 30);
        btnEditCards.TabIndex = 1;
        btnEditCards.Text = "Edit Deck";
        btnEditCards.TextAlign = ContentAlignment.MiddleLeft;
        btnEditCards.UseVisualStyleBackColor = false;
        btnEditCards.Click += btnEditCards_Click;
        // 
        // btnChangeDuelist
        // 
        btnChangeDuelist.BackColor = Color.FromArgb(15, 15, 26);
        btnChangeDuelist.Cursor = Cursors.Hand;
        btnChangeDuelist.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 62);
        btnChangeDuelist.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 22, 42);
        btnChangeDuelist.FlatStyle = FlatStyle.Flat;
        btnChangeDuelist.Font = new Font("Segoe UI", 9F);
        btnChangeDuelist.ForeColor = Color.FromArgb(192, 192, 216);
        btnChangeDuelist.Location = new Point(25, 58);
        btnChangeDuelist.Name = "btnChangeDuelist";
        btnChangeDuelist.Padding = new Padding(6, 0, 0, 0);
        btnChangeDuelist.Size = new Size(200, 30);
        btnChangeDuelist.TabIndex = 2;
        btnChangeDuelist.Text = "Change Duelist";
        btnChangeDuelist.TextAlign = ContentAlignment.MiddleLeft;
        btnChangeDuelist.UseVisualStyleBackColor = false;
        btnChangeDuelist.Click += btnChangeDuelist_Click;
        // 
        // btnRename
        // 
        btnRename.BackColor = Color.FromArgb(15, 15, 26);
        btnRename.Cursor = Cursors.Hand;
        btnRename.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 62);
        btnRename.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 22, 42);
        btnRename.FlatStyle = FlatStyle.Flat;
        btnRename.Font = new Font("Segoe UI", 9F);
        btnRename.ForeColor = Color.FromArgb(192, 192, 216);
        btnRename.Location = new Point(25, 92);
        btnRename.Name = "btnRename";
        btnRename.Padding = new Padding(6, 0, 0, 0);
        btnRename.Size = new Size(200, 30);
        btnRename.TabIndex = 3;
        btnRename.Text = "Rename Deck";
        btnRename.TextAlign = ContentAlignment.MiddleLeft;
        btnRename.UseVisualStyleBackColor = false;
        btnRename.Click += btnRename_Click;
        // 
        // btnCopyTo
        // 
        btnCopyTo.BackColor = Color.FromArgb(15, 15, 26);
        btnCopyTo.Cursor = Cursors.Hand;
        btnCopyTo.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 62);
        btnCopyTo.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 22, 42);
        btnCopyTo.FlatStyle = FlatStyle.Flat;
        btnCopyTo.Font = new Font("Segoe UI", 9F);
        btnCopyTo.ForeColor = Color.FromArgb(192, 192, 216);
        btnCopyTo.Location = new Point(25, 126);
        btnCopyTo.Name = "btnCopyTo";
        btnCopyTo.Padding = new Padding(6, 0, 0, 0);
        btnCopyTo.Size = new Size(200, 30);
        btnCopyTo.TabIndex = 4;
        btnCopyTo.Text = "Copy to slot…";
        btnCopyTo.TextAlign = ContentAlignment.MiddleLeft;
        btnCopyTo.UseVisualStyleBackColor = false;
        btnCopyTo.Click += btnCopyTo_Click;
        // 
        // btnSwapWith
        // 
        btnSwapWith.BackColor = Color.FromArgb(15, 15, 26);
        btnSwapWith.Cursor = Cursors.Hand;
        btnSwapWith.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 62);
        btnSwapWith.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 22, 42);
        btnSwapWith.FlatStyle = FlatStyle.Flat;
        btnSwapWith.Font = new Font("Segoe UI", 9F);
        btnSwapWith.ForeColor = Color.FromArgb(192, 192, 216);
        btnSwapWith.Location = new Point(25, 160);
        btnSwapWith.Name = "btnSwapWith";
        btnSwapWith.Padding = new Padding(6, 0, 0, 0);
        btnSwapWith.Size = new Size(200, 30);
        btnSwapWith.TabIndex = 5;
        btnSwapWith.Text = "Swap with slot…";
        btnSwapWith.TextAlign = ContentAlignment.MiddleLeft;
        btnSwapWith.UseVisualStyleBackColor = false;
        btnSwapWith.Click += btnSwapWith_Click;
        // 
        // btnImportYdk
        // 
        btnImportYdk.BackColor = Color.FromArgb(15, 15, 26);
        btnImportYdk.Cursor = Cursors.Hand;
        btnImportYdk.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 62);
        btnImportYdk.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 22, 42);
        btnImportYdk.FlatStyle = FlatStyle.Flat;
        btnImportYdk.Font = new Font("Segoe UI", 9F);
        btnImportYdk.ForeColor = Color.FromArgb(192, 192, 216);
        btnImportYdk.Location = new Point(25, 194);
        btnImportYdk.Name = "btnImportYdk";
        btnImportYdk.Padding = new Padding(6, 0, 0, 0);
        btnImportYdk.Size = new Size(200, 30);
        btnImportYdk.TabIndex = 6;
        btnImportYdk.Text = "Import .ydk here";
        btnImportYdk.TextAlign = ContentAlignment.MiddleLeft;
        btnImportYdk.UseVisualStyleBackColor = false;
        btnImportYdk.Click += btnImportYdk_Click;
        // 
        // btnExportYdk
        // 
        btnExportYdk.BackColor = Color.FromArgb(15, 15, 26);
        btnExportYdk.Cursor = Cursors.Hand;
        btnExportYdk.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 62);
        btnExportYdk.FlatAppearance.MouseOverBackColor = Color.FromArgb(22, 22, 42);
        btnExportYdk.FlatStyle = FlatStyle.Flat;
        btnExportYdk.Font = new Font("Segoe UI", 9F);
        btnExportYdk.ForeColor = Color.FromArgb(192, 192, 216);
        btnExportYdk.Location = new Point(25, 228);
        btnExportYdk.Name = "btnExportYdk";
        btnExportYdk.Padding = new Padding(6, 0, 0, 0);
        btnExportYdk.Size = new Size(200, 30);
        btnExportYdk.TabIndex = 7;
        btnExportYdk.Text = "Export as .ydk";
        btnExportYdk.TextAlign = ContentAlignment.MiddleLeft;
        btnExportYdk.UseVisualStyleBackColor = false;
        btnExportYdk.Click += btnExportYdk_Click;
        // 
        // lblDangerTitle
        // 
        lblDangerTitle.BackColor = Color.Transparent;
        lblDangerTitle.Font = new Font("Segoe UI", 7F);
        lblDangerTitle.ForeColor = Color.FromArgb(58, 58, 90);
        lblDangerTitle.Location = new Point(10, 270);
        lblDangerTitle.Name = "lblDangerTitle";
        lblDangerTitle.Size = new Size(200, 20);
        lblDangerTitle.TabIndex = 8;
        lblDangerTitle.Text = "DANGER ZONE";
        // 
        // btnClearSlot
        // 
        btnClearSlot.BackColor = Color.FromArgb(42, 24, 24);
        btnClearSlot.Cursor = Cursors.Hand;
        btnClearSlot.FlatAppearance.BorderColor = Color.FromArgb(42, 24, 24);
        btnClearSlot.FlatAppearance.MouseOverBackColor = Color.FromArgb(26, 15, 15);
        btnClearSlot.FlatStyle = FlatStyle.Flat;
        btnClearSlot.Font = new Font("Segoe UI", 9F);
        btnClearSlot.ForeColor = Color.FromArgb(122, 64, 64);
        btnClearSlot.Location = new Point(25, 294);
        btnClearSlot.Name = "btnClearSlot";
        btnClearSlot.Padding = new Padding(6, 0, 0, 0);
        btnClearSlot.Size = new Size(200, 30);
        btnClearSlot.TabIndex = 9;
        btnClearSlot.Text = "Clear slot";
        btnClearSlot.TextAlign = ContentAlignment.MiddleLeft;
        btnClearSlot.UseVisualStyleBackColor = false;
        btnClearSlot.Click += btnClearSlot_Click;
        // 
        // pnlStats
        // 
        pnlStats.BackColor = Color.Transparent;
        pnlStats.Controls.Add(lblCountsTitle);
        pnlStats.Controls.Add(pnlMainBarRow);
        pnlStats.Controls.Add(pnlExtraBarRow);
        pnlStats.Controls.Add(pnlSideBarRow);
        pnlStats.Controls.Add(pnlStatsDivider);
        pnlStats.Dock = DockStyle.Fill;
        pnlStats.Location = new Point(3, 190);
        pnlStats.Name = "pnlStats";
        pnlStats.Size = new Size(240, 125);
        pnlStats.TabIndex = 3;
        // 
        // lblCountsTitle
        // 
        lblCountsTitle.BackColor = Color.Transparent;
        lblCountsTitle.Font = new Font("Segoe UI", 7F);
        lblCountsTitle.ForeColor = Color.FromArgb(58, 58, 90);
        lblCountsTitle.Location = new Point(10, 6);
        lblCountsTitle.Name = "lblCountsTitle";
        lblCountsTitle.Size = new Size(225, 16);
        lblCountsTitle.TabIndex = 0;
        lblCountsTitle.Text = "CARD COUNTS";
        // 
        // pnlMainBarRow
        // 
        pnlMainBarRow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlMainBarRow.BackColor = Color.Transparent;
        pnlMainBarRow.Controls.Add(lblMainName);
        pnlMainBarRow.Controls.Add(lblMainBar);
        pnlMainBarRow.Controls.Add(pnlMainTrack);
        pnlMainBarRow.Location = new Point(10, 24);
        pnlMainBarRow.Name = "pnlMainBarRow";
        pnlMainBarRow.Size = new Size(220, 25);
        pnlMainBarRow.TabIndex = 1;
        // 
        // lblMainName
        // 
        lblMainName.BackColor = Color.Transparent;
        lblMainName.Font = new Font("Segoe UI", 7F);
        lblMainName.ForeColor = Color.FromArgb(192, 192, 216);
        lblMainName.Location = new Point(0, 0);
        lblMainName.Name = "lblMainName";
        lblMainName.Size = new Size(112, 16);
        lblMainName.TabIndex = 0;
        lblMainName.Text = "Main";
        // 
        // lblMainBar
        // 
        lblMainBar.BackColor = Color.Transparent;
        lblMainBar.Font = new Font("Segoe UI", 7F);
        lblMainBar.ForeColor = Color.FromArgb(192, 192, 216);
        lblMainBar.Location = new Point(112, 0);
        lblMainBar.Name = "lblMainBar";
        lblMainBar.Size = new Size(113, 16);
        lblMainBar.TabIndex = 1;
        lblMainBar.Text = "0/60";
        lblMainBar.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlMainTrack
        // 
        pnlMainTrack.BackColor = Color.FromArgb(26, 26, 46);
        pnlMainTrack.Controls.Add(pnlMainFill);
        pnlMainTrack.Location = new Point(0, 18);
        pnlMainTrack.Name = "pnlMainTrack";
        pnlMainTrack.Size = new Size(225, 4);
        pnlMainTrack.TabIndex = 2;
        // 
        // pnlMainFill
        // 
        pnlMainFill.BackColor = Color.FromArgb(90, 176, 90);
        pnlMainFill.Location = new Point(0, 0);
        pnlMainFill.Name = "pnlMainFill";
        pnlMainFill.Size = new Size(0, 4);
        pnlMainFill.TabIndex = 0;
        // 
        // pnlExtraBarRow
        // 
        pnlExtraBarRow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlExtraBarRow.BackColor = Color.Transparent;
        pnlExtraBarRow.Controls.Add(lblExtraName);
        pnlExtraBarRow.Controls.Add(lblExtraBar);
        pnlExtraBarRow.Controls.Add(pnlExtraTrack);
        pnlExtraBarRow.Location = new Point(10, 58);
        pnlExtraBarRow.Name = "pnlExtraBarRow";
        pnlExtraBarRow.Size = new Size(220, 25);
        pnlExtraBarRow.TabIndex = 2;
        // 
        // lblExtraName
        // 
        lblExtraName.BackColor = Color.Transparent;
        lblExtraName.Font = new Font("Segoe UI", 7F);
        lblExtraName.ForeColor = Color.FromArgb(192, 192, 216);
        lblExtraName.Location = new Point(0, 0);
        lblExtraName.Name = "lblExtraName";
        lblExtraName.Size = new Size(112, 16);
        lblExtraName.TabIndex = 0;
        lblExtraName.Text = "Extra";
        // 
        // lblExtraBar
        // 
        lblExtraBar.BackColor = Color.Transparent;
        lblExtraBar.Font = new Font("Segoe UI", 7F);
        lblExtraBar.ForeColor = Color.FromArgb(192, 192, 216);
        lblExtraBar.Location = new Point(112, 0);
        lblExtraBar.Name = "lblExtraBar";
        lblExtraBar.Size = new Size(113, 16);
        lblExtraBar.TabIndex = 1;
        lblExtraBar.Text = "0/15";
        lblExtraBar.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlExtraTrack
        // 
        pnlExtraTrack.BackColor = Color.FromArgb(26, 26, 46);
        pnlExtraTrack.Controls.Add(pnlExtraFill);
        pnlExtraTrack.Location = new Point(0, 18);
        pnlExtraTrack.Name = "pnlExtraTrack";
        pnlExtraTrack.Size = new Size(225, 4);
        pnlExtraTrack.TabIndex = 2;
        // 
        // pnlExtraFill
        // 
        pnlExtraFill.BackColor = Color.FromArgb(96, 96, 200);
        pnlExtraFill.Location = new Point(0, 0);
        pnlExtraFill.Name = "pnlExtraFill";
        pnlExtraFill.Size = new Size(0, 4);
        pnlExtraFill.TabIndex = 0;
        // 
        // pnlSideBarRow
        // 
        pnlSideBarRow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlSideBarRow.BackColor = Color.Transparent;
        pnlSideBarRow.Controls.Add(lblSideName);
        pnlSideBarRow.Controls.Add(lblSideBar);
        pnlSideBarRow.Controls.Add(pnlSideTrack);
        pnlSideBarRow.Location = new Point(10, 92);
        pnlSideBarRow.Name = "pnlSideBarRow";
        pnlSideBarRow.Size = new Size(220, 25);
        pnlSideBarRow.TabIndex = 3;
        // 
        // lblSideName
        // 
        lblSideName.BackColor = Color.Transparent;
        lblSideName.Font = new Font("Segoe UI", 7F);
        lblSideName.ForeColor = Color.FromArgb(192, 192, 216);
        lblSideName.Location = new Point(0, 0);
        lblSideName.Name = "lblSideName";
        lblSideName.Size = new Size(112, 16);
        lblSideName.TabIndex = 0;
        lblSideName.Text = "Side";
        // 
        // lblSideBar
        // 
        lblSideBar.BackColor = Color.Transparent;
        lblSideBar.Font = new Font("Segoe UI", 7F);
        lblSideBar.ForeColor = Color.FromArgb(192, 192, 216);
        lblSideBar.Location = new Point(112, 0);
        lblSideBar.Name = "lblSideBar";
        lblSideBar.Size = new Size(113, 16);
        lblSideBar.TabIndex = 1;
        lblSideBar.Text = "0/15";
        lblSideBar.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlSideTrack
        // 
        pnlSideTrack.BackColor = Color.FromArgb(26, 26, 46);
        pnlSideTrack.Controls.Add(pnlSideFill);
        pnlSideTrack.Location = new Point(0, 18);
        pnlSideTrack.Name = "pnlSideTrack";
        pnlSideTrack.Size = new Size(225, 4);
        pnlSideTrack.TabIndex = 2;
        // 
        // pnlSideFill
        // 
        pnlSideFill.BackColor = Color.FromArgb(176, 80, 80);
        pnlSideFill.Location = new Point(0, 0);
        pnlSideFill.Name = "pnlSideFill";
        pnlSideFill.Size = new Size(0, 4);
        pnlSideFill.TabIndex = 0;
        // 
        // pnlStatsDivider
        // 
        pnlStatsDivider.BackColor = Color.FromArgb(42, 42, 62);
        pnlStatsDivider.Location = new Point(0, 124);
        pnlStatsDivider.Name = "pnlStatsDivider";
        pnlStatsDivider.Size = new Size(245, 1);
        pnlStatsDivider.TabIndex = 4;
        // 
        // pnlHeader
        // 
        pnlHeader.BackColor = Color.Transparent;
        pnlHeader.Controls.Add(lblHeaderTag);
        pnlHeader.Controls.Add(lblOwnerName);
        pnlHeader.Controls.Add(pnlHeaderDivider);
        pnlHeader.Controls.Add(lblOwnerId);
        pnlHeader.Dock = DockStyle.Fill;
        pnlHeader.Location = new Point(3, 126);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(240, 58);
        pnlHeader.TabIndex = 2;
        // 
        // lblHeaderTag
        // 
        lblHeaderTag.BackColor = Color.Transparent;
        lblHeaderTag.Font = new Font("Segoe UI", 7F);
        lblHeaderTag.ForeColor = Color.FromArgb(58, 58, 90);
        lblHeaderTag.Location = new Point(10, 6);
        lblHeaderTag.Name = "lblHeaderTag";
        lblHeaderTag.Size = new Size(221, 14);
        lblHeaderTag.TabIndex = 0;
        lblHeaderTag.Text = "Duelist";
        // 
        // lblOwnerName
        // 
        lblOwnerName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblOwnerName.BackColor = Color.Transparent;
        lblOwnerName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblOwnerName.ForeColor = Color.FromArgb(224, 224, 240);
        lblOwnerName.Location = new Point(2, 20);
        lblOwnerName.Name = "lblOwnerName";
        lblOwnerName.Padding = new Padding(8, 0, 4, 0);
        lblOwnerName.Size = new Size(221, 20);
        lblOwnerName.TabIndex = 1;
        lblOwnerName.Text = "__";
        lblOwnerName.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlHeaderDivider
        // 
        pnlHeaderDivider.BackColor = Color.FromArgb(42, 42, 62);
        pnlHeaderDivider.Location = new Point(0, 57);
        pnlHeaderDivider.Name = "pnlHeaderDivider";
        pnlHeaderDivider.Size = new Size(245, 1);
        pnlHeaderDivider.TabIndex = 3;
        // 
        // lblOwnerId
        // 
        lblOwnerId.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblOwnerId.BackColor = Color.Transparent;
        lblOwnerId.Font = new Font("Segoe UI", 8F);
        lblOwnerId.ForeColor = Color.FromArgb(201, 162, 39);
        lblOwnerId.Location = new Point(4, 38);
        lblOwnerId.Name = "lblOwnerId";
        lblOwnerId.Padding = new Padding(8, 0, 4, 0);
        lblOwnerId.Size = new Size(219, 16);
        lblOwnerId.TabIndex = 2;
        lblOwnerId.Text = "—";
        lblOwnerId.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlPortraitDivider
        // 
        pnlPortraitDivider.BackColor = Color.FromArgb(42, 42, 62);
        pnlPortraitDivider.Dock = DockStyle.Fill;
        pnlPortraitDivider.Location = new Point(3, 119);
        pnlPortraitDivider.Name = "pnlPortraitDivider";
        pnlPortraitDivider.Size = new Size(240, 1);
        pnlPortraitDivider.TabIndex = 1;
        // 
        // pnlPortrait
        // 
        pnlPortrait.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlPortrait.BackColor = Color.FromArgb(12, 12, 24);
        pnlPortrait.Controls.Add(picPortrait);
        pnlPortrait.Controls.Add(lblSlotNum);
        pnlPortrait.Controls.Add(lblDeckName);
        pnlPortrait.Location = new Point(3, 3);
        pnlPortrait.Name = "pnlPortrait";
        pnlPortrait.Size = new Size(240, 110);
        pnlPortrait.TabIndex = 0;
        // 
        // picPortrait
        // 
        picPortrait.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        picPortrait.BackColor = Color.Transparent;
        picPortrait.Location = new Point(133, 3);
        picPortrait.Name = "picPortrait";
        picPortrait.Size = new Size(90, 100);
        picPortrait.SizeMode = PictureBoxSizeMode.Zoom;
        picPortrait.TabIndex = 0;
        picPortrait.TabStop = false;
        // 
        // lblSlotNum
        // 
        lblSlotNum.BackColor = Color.Transparent;
        lblSlotNum.Font = new Font("Segoe UI", 8F);
        lblSlotNum.ForeColor = Color.FromArgb(201, 162, 39);
        lblSlotNum.Location = new Point(4, 89);
        lblSlotNum.Name = "lblSlotNum";
        lblSlotNum.Size = new Size(123, 14);
        lblSlotNum.TabIndex = 2;
        lblSlotNum.Text = "—";
        // 
        // lblDeckName
        // 
        lblDeckName.BackColor = Color.Transparent;
        lblDeckName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblDeckName.ForeColor = Color.FromArgb(224, 224, 240);
        lblDeckName.Location = new Point(4, 3);
        lblDeckName.Name = "lblDeckName";
        lblDeckName.Size = new Size(118, 86);
        lblDeckName.TabIndex = 1;
        lblDeckName.Text = "__";
        // 
        // leftBorder
        // 
        leftBorder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        leftBorder.BackColor = Color.FromArgb(42, 42, 62);
        leftBorder.Location = new Point(0, 0);
        leftBorder.Name = "leftBorder";
        leftBorder.Size = new Size(1, 654);
        leftBorder.TabIndex = 0;
        // 
        // DetailPanel
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(19, 19, 31);
        Controls.Add(pnlEmpty);
        Controls.Add(tableLayoutPanel1);
        Controls.Add(leftBorder);
        Name = "DetailPanel";
        Size = new Size(246, 654);
        pnlEmpty.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        pnlActions.ResumeLayout(false);
        pnlActionsScroll.ResumeLayout(false);
        pnlStats.ResumeLayout(false);
        pnlMainBarRow.ResumeLayout(false);
        pnlMainTrack.ResumeLayout(false);
        pnlExtraBarRow.ResumeLayout(false);
        pnlExtraTrack.ResumeLayout(false);
        pnlSideBarRow.ResumeLayout(false);
        pnlSideTrack.ResumeLayout(false);
        pnlHeader.ResumeLayout(false);
        pnlPortrait.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picPortrait).EndInit();
        ResumeLayout(false);
    }

    private Label lblEmptyHint;
    private Panel pnlPortraitDivider;
    private Panel pnlHeaderDivider;
    private Label lblCountsTitle;
    private Label lblMainName;
    private Label lblExtraName;
    private Label lblSideName;
    private Panel pnlStatsDivider;
    private Panel pnlActionsScroll;
    private Label lblActionsTitle;
    private Label lblDangerTitle;
    private Panel leftBorder;
    private TableLayoutPanel tableLayoutPanel1;
}