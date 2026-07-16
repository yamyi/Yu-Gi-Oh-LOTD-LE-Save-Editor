using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

partial class DeckSlotsPage
{
    private Panel           pnlToolbar  = null!;
    private Panel           pnlBody     = null!;
    private FlowLayoutPanel pnlGrid     = null!;
    private Panel           pnlDivider  = null!;
    private TextBox txtSearch;
    private Button btnClearAllSlots;
    private Label lblMultiSelectCount;
    private Button btnBulkClear;
    private Button btnBulkExport;

    // Fixed set of 32 deck-slot cards — a save always has exactly SlotLayout.SlotCount
    // slots, so each card is declared and built individually below (not created or
    // destroyed on every LoadSlots() call). DeckSlotsPage.LoadSlots() just rebinds
    // SlotInfo onto them. The convenience "deckCards" array (for indexed access from
    // DeckSlotsPage.cs) is assembled in the constructor, not here — the WinForms
    // designer strips non-standard code like an array literal out of this method
    // whenever it regenerates it.
    private DeckCard deckCard01 = null!;
    private DeckCard deckCard02 = null!;
    private DeckCard deckCard03 = null!;
    private DeckCard deckCard04 = null!;
    private DeckCard deckCard05 = null!;
    private DeckCard deckCard06 = null!;
    private DeckCard deckCard07 = null!;
    private DeckCard deckCard08 = null!;
    private DeckCard deckCard09 = null!;
    private DeckCard deckCard10 = null!;
    private DeckCard deckCard11 = null!;
    private DeckCard deckCard12 = null!;
    private DeckCard deckCard13 = null!;
    private DeckCard deckCard14 = null!;
    private DeckCard deckCard15 = null!;
    private DeckCard deckCard16 = null!;
    private DeckCard deckCard17 = null!;
    private DeckCard deckCard18 = null!;
    private DeckCard deckCard19 = null!;
    private DeckCard deckCard20 = null!;
    private DeckCard deckCard21 = null!;
    private DeckCard deckCard22 = null!;
    private DeckCard deckCard23 = null!;
    private DeckCard deckCard24 = null!;
    private DeckCard deckCard25 = null!;
    private DeckCard deckCard26 = null!;
    private DeckCard deckCard27 = null!;
    private DeckCard deckCard28 = null!;
    private DeckCard deckCard29 = null!;
    private DeckCard deckCard30 = null!;
    private DeckCard deckCard31 = null!;
    private DeckCard deckCard32 = null!;
    private DeckCard[] deckCards = null!;

    // ── Detail panel (formerly the DetailPanel UserControl, now inline) ────────
    private Panel pnlDetail = null!;
    private Panel pnlEmpty;
    private Label lblEmptyHint;
    private TableLayoutPanel tableLayoutPanel1;
    // Portrait
    private Panel pnlPortrait;
    private PictureBox picPortrait;
    private Label lblOwnerName;
    private Label lblOwnerId;
    private Panel pnlPortraitDivider;
    // Header
    private Panel pnlHeader;
    private Label lblHeaderTag;
    private Label lblDeckName;
    private Label lblSlotNum;
    private Panel pnlHeaderDivider;
    // Stats
    private Panel pnlStats;
    private Label lblCountsTitle;
    private Panel pnlMainBarRow;
    private Label lblMainName;
    private Panel pnlMainTrack;
    private Panel pnlMainFill;
    private Label lblMainBar;
    private Panel pnlExtraBarRow;
    private Label lblExtraName;
    private Panel pnlExtraTrack;
    private Panel pnlExtraFill;
    private Label lblExtraBar;
    private Panel pnlSideBarRow;
    private Label lblSideName;
    private Panel pnlSideTrack;
    private Panel pnlSideFill;
    private Label lblSideBar;
    private Panel pnlStatsDivider;
    // Actions
    private Panel pnlActions;
    private Panel pnlActionsScroll;
    private Label lblActionsTitle;
    private Button btnEditCards;
    private Button btnChangeDuelist;
    private Button btnRename;
    private Button btnCopyTo;
    private Button btnSwapWith;
    private Button btnImportYdk;
    private Button btnImportYdkMulti;
    private Button btnExportYdk;
    private Label lblDangerTitle;
    private Button btnClearSlot;
    private Panel leftBorder;

    private void InitializeComponent()
    {
        pnlToolbar = new Panel();
        txtSearch = new TextBox();
        lblMultiSelectCount = new Label();
        btnBulkClear = new Button();
        btnBulkExport = new Button();
        btnClearAllSlots = new Button();
        pnlBody = new Panel();
        pnlGrid = new FlowLayoutPanel();
        deckCard01 = new DeckCard();
        deckCard02 = new DeckCard();
        deckCard03 = new DeckCard();
        deckCard04 = new DeckCard();
        deckCard05 = new DeckCard();
        deckCard06 = new DeckCard();
        deckCard07 = new DeckCard();
        deckCard08 = new DeckCard();
        deckCard09 = new DeckCard();
        deckCard10 = new DeckCard();
        deckCard11 = new DeckCard();
        deckCard12 = new DeckCard();
        deckCard13 = new DeckCard();
        deckCard14 = new DeckCard();
        deckCard15 = new DeckCard();
        deckCard16 = new DeckCard();
        deckCard17 = new DeckCard();
        deckCard18 = new DeckCard();
        deckCard19 = new DeckCard();
        deckCard20 = new DeckCard();
        deckCard21 = new DeckCard();
        deckCard22 = new DeckCard();
        deckCard23 = new DeckCard();
        deckCard24 = new DeckCard();
        deckCard25 = new DeckCard();
        deckCard26 = new DeckCard();
        deckCard27 = new DeckCard();
        deckCard28 = new DeckCard();
        deckCard29 = new DeckCard();
        deckCard30 = new DeckCard();
        deckCard31 = new DeckCard();
        deckCard32 = new DeckCard();
        pnlDetail = new Panel();
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
        btnImportYdkMulti = new Button();
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
        pnlDivider = new Panel();
        pnlToolbar.SuspendLayout();
        pnlBody.SuspendLayout();
        pnlGrid.SuspendLayout();
        pnlDetail.SuspendLayout();
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
        // pnlToolbar
        // 
        pnlToolbar.BackColor = AppColors.TopbarBg;
        pnlToolbar.Controls.Add(txtSearch);
        pnlToolbar.Controls.Add(lblMultiSelectCount);
        pnlToolbar.Controls.Add(btnBulkClear);
        pnlToolbar.Controls.Add(btnBulkExport);
        pnlToolbar.Controls.Add(btnClearAllSlots);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Location = new Point(0, 0);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1177, 44);
        pnlToolbar.TabIndex = 2;
        //
        // txtSearch
        //
        txtSearch.BackColor = AppColors.AppBg;
        txtSearch.BorderStyle = BorderStyle.None;
        txtSearch.Font = new Font("Segoe UI", 9F);
        txtSearch.ForeColor = AppColors.TextSecondary;
        txtSearch.Location = new Point(14, 14);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search deck";
        txtSearch.Size = new Size(198, 16);
        txtSearch.TabIndex = 0;
        txtSearch.TextChanged += txtSearch_TextChanged;
        //
        // lblMultiSelectCount
        //
        lblMultiSelectCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblMultiSelectCount.AutoSize = true;
        lblMultiSelectCount.BackColor = Color.Transparent;
        lblMultiSelectCount.Font = new Font("Segoe UI", 8.5F);
        lblMultiSelectCount.ForeColor = AppColors.TextMuted;
        lblMultiSelectCount.Location = new Point(600, 16);
        lblMultiSelectCount.Name = "lblMultiSelectCount";
        lblMultiSelectCount.Size = new Size(70, 15);
        lblMultiSelectCount.TabIndex = 4;
        lblMultiSelectCount.Text = "";
        //
        // btnBulkExport
        //
        btnBulkExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBulkExport.BackColor = Color.Transparent;
        btnBulkExport.Cursor = Cursors.Hand;
        btnBulkExport.Enabled = false;
        btnBulkExport.FlatAppearance.BorderColor = AppColors.ButtonNeutralBg;
        btnBulkExport.FlatAppearance.MouseOverBackColor = AppColors.ButtonNeutralHover;
        btnBulkExport.FlatStyle = FlatStyle.Flat;
        btnBulkExport.Font = new Font("Segoe UI", 9F);
        btnBulkExport.ForeColor = AppColors.TextSecondary;
        btnBulkExport.Location = new Point(713, 10);
        btnBulkExport.Name = "btnBulkExport";
        btnBulkExport.Size = new Size(140, 24);
        btnBulkExport.TabIndex = 5;
        btnBulkExport.Text = "Export selected";
        btnBulkExport.TextAlign = ContentAlignment.MiddleCenter;
        btnBulkExport.UseVisualStyleBackColor = false;
        btnBulkExport.Click += btnBulkExport_Click;
        //
        // btnBulkClear
        //
        btnBulkClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBulkClear.BackColor = Color.Transparent;
        btnBulkClear.Cursor = Cursors.Hand;
        btnBulkClear.Enabled = false;
        btnBulkClear.FlatAppearance.BorderColor = AppColors.DangerBorder;
        btnBulkClear.FlatAppearance.MouseOverBackColor = AppColors.DangerHoverBg;
        btnBulkClear.FlatStyle = FlatStyle.Flat;
        btnBulkClear.Font = new Font("Segoe UI", 9F);
        btnBulkClear.ForeColor = AppColors.DangerFg;
        btnBulkClear.Location = new Point(859, 10);
        btnBulkClear.Name = "btnBulkClear";
        btnBulkClear.Size = new Size(140, 24);
        btnBulkClear.TabIndex = 6;
        btnBulkClear.Text = "Clear selected";
        btnBulkClear.TextAlign = ContentAlignment.MiddleCenter;
        btnBulkClear.UseVisualStyleBackColor = false;
        btnBulkClear.Click += btnBulkClear_Click;
        //
        // btnClearAllSlots
        //
        btnClearAllSlots.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClearAllSlots.BackColor = AppColors.DangerBorder;
        btnClearAllSlots.Cursor = Cursors.Hand;
        btnClearAllSlots.FlatAppearance.BorderColor = AppColors.DangerBorder;
        btnClearAllSlots.FlatAppearance.MouseOverBackColor = AppColors.DangerHoverBg;
        btnClearAllSlots.FlatStyle = FlatStyle.Flat;
        btnClearAllSlots.Font = new Font("Segoe UI", 9F);
        btnClearAllSlots.ForeColor = AppColors.DangerFg;
        btnClearAllSlots.Location = new Point(1023, 10);
        btnClearAllSlots.Name = "btnClearAllSlots";
        btnClearAllSlots.Size = new Size(140, 24);
        btnClearAllSlots.TabIndex = 1;
        btnClearAllSlots.Text = "Clear all slots";
        btnClearAllSlots.TextAlign = ContentAlignment.MiddleCenter;
        btnClearAllSlots.UseVisualStyleBackColor = false;
        btnClearAllSlots.Click += btnClearAllSlots_Click;
        // 
        // pnlBody
        // 
        pnlBody.BackColor = AppColors.AppBg;
        pnlBody.Controls.Add(pnlGrid);
        pnlBody.Controls.Add(pnlDetail);
        pnlBody.Dock = DockStyle.Fill;
        pnlBody.Location = new Point(0, 45);
        pnlBody.Name = "pnlBody";
        pnlBody.Size = new Size(1177, 581);
        pnlBody.TabIndex = 0;
        // 
        // pnlGrid
        // 
        pnlGrid.AutoScroll = true;
        pnlGrid.BackColor = AppColors.AppBg;
        pnlGrid.Controls.Add(deckCard01);
        pnlGrid.Controls.Add(deckCard02);
        pnlGrid.Controls.Add(deckCard03);
        pnlGrid.Controls.Add(deckCard04);
        pnlGrid.Controls.Add(deckCard05);
        pnlGrid.Controls.Add(deckCard06);
        pnlGrid.Controls.Add(deckCard07);
        pnlGrid.Controls.Add(deckCard08);
        pnlGrid.Controls.Add(deckCard09);
        pnlGrid.Controls.Add(deckCard10);
        pnlGrid.Controls.Add(deckCard11);
        pnlGrid.Controls.Add(deckCard12);
        pnlGrid.Controls.Add(deckCard13);
        pnlGrid.Controls.Add(deckCard14);
        pnlGrid.Controls.Add(deckCard15);
        pnlGrid.Controls.Add(deckCard16);
        pnlGrid.Controls.Add(deckCard17);
        pnlGrid.Controls.Add(deckCard18);
        pnlGrid.Controls.Add(deckCard19);
        pnlGrid.Controls.Add(deckCard20);
        pnlGrid.Controls.Add(deckCard21);
        pnlGrid.Controls.Add(deckCard22);
        pnlGrid.Controls.Add(deckCard23);
        pnlGrid.Controls.Add(deckCard24);
        pnlGrid.Controls.Add(deckCard25);
        pnlGrid.Controls.Add(deckCard26);
        pnlGrid.Controls.Add(deckCard27);
        pnlGrid.Controls.Add(deckCard28);
        pnlGrid.Controls.Add(deckCard29);
        pnlGrid.Controls.Add(deckCard30);
        pnlGrid.Controls.Add(deckCard31);
        pnlGrid.Controls.Add(deckCard32);
        pnlGrid.Dock = DockStyle.Fill;
        pnlGrid.Location = new Point(0, 0);
        pnlGrid.Name = "pnlGrid";
        pnlGrid.Padding = new Padding(12);
        pnlGrid.Size = new Size(898, 581);
        pnlGrid.TabIndex = 0;
        pnlGrid.Paint += BackgroundPainter.Paint;
        //
        // deckCard01
        // 
        deckCard01.BackColor = AppColors.CardBg;
        deckCard01.IsSelected = false;
        deckCard01.Location = new Point(16, 16);
        deckCard01.Margin = new Padding(4);
        deckCard01.Name = "deckCard01";
        deckCard01.Size = new Size(204, 143);
        deckCard01.Slot = null;
        deckCard01.TabIndex = 0;
        deckCard01.SlotSelected += Card_SlotSelected;
        deckCard01.EditRequested += Card_EditRequested;
        deckCard01.ClearRequested += Card_ClearRequested;
        // 
        // deckCard02
        // 
        deckCard02.BackColor = AppColors.CardBg;
        deckCard02.IsSelected = false;
        deckCard02.Location = new Point(228, 16);
        deckCard02.Margin = new Padding(4);
        deckCard02.Name = "deckCard02";
        deckCard02.Size = new Size(204, 143);
        deckCard02.Slot = null;
        deckCard02.TabIndex = 1;
        deckCard02.SlotSelected += Card_SlotSelected;
        deckCard02.EditRequested += Card_EditRequested;
        deckCard02.ClearRequested += Card_ClearRequested;
        // 
        // deckCard03
        // 
        deckCard03.BackColor = AppColors.CardBg;
        deckCard03.IsSelected = false;
        deckCard03.Location = new Point(440, 16);
        deckCard03.Margin = new Padding(4);
        deckCard03.Name = "deckCard03";
        deckCard03.Size = new Size(204, 143);
        deckCard03.Slot = null;
        deckCard03.TabIndex = 2;
        deckCard03.SlotSelected += Card_SlotSelected;
        deckCard03.EditRequested += Card_EditRequested;
        deckCard03.ClearRequested += Card_ClearRequested;
        // 
        // deckCard04
        // 
        deckCard04.BackColor = AppColors.CardBg;
        deckCard04.IsSelected = false;
        deckCard04.Location = new Point(652, 16);
        deckCard04.Margin = new Padding(4);
        deckCard04.Name = "deckCard04";
        deckCard04.Size = new Size(204, 143);
        deckCard04.Slot = null;
        deckCard04.TabIndex = 3;
        deckCard04.SlotSelected += Card_SlotSelected;
        deckCard04.EditRequested += Card_EditRequested;
        deckCard04.ClearRequested += Card_ClearRequested;
        // 
        // deckCard05
        // 
        deckCard05.BackColor = AppColors.CardBg;
        deckCard05.IsSelected = false;
        deckCard05.Location = new Point(16, 167);
        deckCard05.Margin = new Padding(4);
        deckCard05.Name = "deckCard05";
        deckCard05.Size = new Size(204, 143);
        deckCard05.Slot = null;
        deckCard05.TabIndex = 4;
        deckCard05.SlotSelected += Card_SlotSelected;
        deckCard05.EditRequested += Card_EditRequested;
        deckCard05.ClearRequested += Card_ClearRequested;
        // 
        // deckCard06
        // 
        deckCard06.BackColor = AppColors.CardBg;
        deckCard06.IsSelected = false;
        deckCard06.Location = new Point(228, 167);
        deckCard06.Margin = new Padding(4);
        deckCard06.Name = "deckCard06";
        deckCard06.Size = new Size(204, 143);
        deckCard06.Slot = null;
        deckCard06.TabIndex = 5;
        deckCard06.SlotSelected += Card_SlotSelected;
        deckCard06.EditRequested += Card_EditRequested;
        deckCard06.ClearRequested += Card_ClearRequested;
        // 
        // deckCard07
        // 
        deckCard07.BackColor = AppColors.CardBg;
        deckCard07.IsSelected = false;
        deckCard07.Location = new Point(440, 167);
        deckCard07.Margin = new Padding(4);
        deckCard07.Name = "deckCard07";
        deckCard07.Size = new Size(204, 143);
        deckCard07.Slot = null;
        deckCard07.TabIndex = 6;
        deckCard07.SlotSelected += Card_SlotSelected;
        deckCard07.EditRequested += Card_EditRequested;
        deckCard07.ClearRequested += Card_ClearRequested;
        // 
        // deckCard08
        // 
        deckCard08.BackColor = AppColors.CardBg;
        deckCard08.IsSelected = false;
        deckCard08.Location = new Point(652, 167);
        deckCard08.Margin = new Padding(4);
        deckCard08.Name = "deckCard08";
        deckCard08.Size = new Size(204, 143);
        deckCard08.Slot = null;
        deckCard08.TabIndex = 7;
        deckCard08.SlotSelected += Card_SlotSelected;
        deckCard08.EditRequested += Card_EditRequested;
        deckCard08.ClearRequested += Card_ClearRequested;
        // 
        // deckCard09
        // 
        deckCard09.BackColor = AppColors.CardBg;
        deckCard09.IsSelected = false;
        deckCard09.Location = new Point(16, 318);
        deckCard09.Margin = new Padding(4);
        deckCard09.Name = "deckCard09";
        deckCard09.Size = new Size(204, 143);
        deckCard09.Slot = null;
        deckCard09.TabIndex = 8;
        deckCard09.SlotSelected += Card_SlotSelected;
        deckCard09.EditRequested += Card_EditRequested;
        deckCard09.ClearRequested += Card_ClearRequested;
        // 
        // deckCard10
        // 
        deckCard10.BackColor = AppColors.CardBg;
        deckCard10.IsSelected = false;
        deckCard10.Location = new Point(228, 318);
        deckCard10.Margin = new Padding(4);
        deckCard10.Name = "deckCard10";
        deckCard10.Size = new Size(204, 143);
        deckCard10.Slot = null;
        deckCard10.TabIndex = 9;
        deckCard10.SlotSelected += Card_SlotSelected;
        deckCard10.EditRequested += Card_EditRequested;
        deckCard10.ClearRequested += Card_ClearRequested;
        // 
        // deckCard11
        // 
        deckCard11.BackColor = AppColors.CardBg;
        deckCard11.IsSelected = false;
        deckCard11.Location = new Point(440, 318);
        deckCard11.Margin = new Padding(4);
        deckCard11.Name = "deckCard11";
        deckCard11.Size = new Size(204, 143);
        deckCard11.Slot = null;
        deckCard11.TabIndex = 10;
        deckCard11.SlotSelected += Card_SlotSelected;
        deckCard11.EditRequested += Card_EditRequested;
        deckCard11.ClearRequested += Card_ClearRequested;
        // 
        // deckCard12
        // 
        deckCard12.BackColor = AppColors.CardBg;
        deckCard12.IsSelected = false;
        deckCard12.Location = new Point(652, 318);
        deckCard12.Margin = new Padding(4);
        deckCard12.Name = "deckCard12";
        deckCard12.Size = new Size(204, 143);
        deckCard12.Slot = null;
        deckCard12.TabIndex = 11;
        deckCard12.SlotSelected += Card_SlotSelected;
        deckCard12.EditRequested += Card_EditRequested;
        deckCard12.ClearRequested += Card_ClearRequested;
        // 
        // deckCard13
        // 
        deckCard13.BackColor = AppColors.CardBg;
        deckCard13.IsSelected = false;
        deckCard13.Location = new Point(16, 469);
        deckCard13.Margin = new Padding(4);
        deckCard13.Name = "deckCard13";
        deckCard13.Size = new Size(204, 143);
        deckCard13.Slot = null;
        deckCard13.TabIndex = 12;
        deckCard13.SlotSelected += Card_SlotSelected;
        deckCard13.EditRequested += Card_EditRequested;
        deckCard13.ClearRequested += Card_ClearRequested;
        // 
        // deckCard14
        // 
        deckCard14.BackColor = AppColors.CardBg;
        deckCard14.IsSelected = false;
        deckCard14.Location = new Point(228, 469);
        deckCard14.Margin = new Padding(4);
        deckCard14.Name = "deckCard14";
        deckCard14.Size = new Size(204, 143);
        deckCard14.Slot = null;
        deckCard14.TabIndex = 13;
        deckCard14.SlotSelected += Card_SlotSelected;
        deckCard14.EditRequested += Card_EditRequested;
        deckCard14.ClearRequested += Card_ClearRequested;
        // 
        // deckCard15
        // 
        deckCard15.BackColor = AppColors.CardBg;
        deckCard15.IsSelected = false;
        deckCard15.Location = new Point(440, 469);
        deckCard15.Margin = new Padding(4);
        deckCard15.Name = "deckCard15";
        deckCard15.Size = new Size(204, 143);
        deckCard15.Slot = null;
        deckCard15.TabIndex = 14;
        deckCard15.SlotSelected += Card_SlotSelected;
        deckCard15.EditRequested += Card_EditRequested;
        deckCard15.ClearRequested += Card_ClearRequested;
        // 
        // deckCard16
        // 
        deckCard16.BackColor = AppColors.CardBg;
        deckCard16.IsSelected = false;
        deckCard16.Location = new Point(652, 469);
        deckCard16.Margin = new Padding(4);
        deckCard16.Name = "deckCard16";
        deckCard16.Size = new Size(204, 143);
        deckCard16.Slot = null;
        deckCard16.TabIndex = 15;
        deckCard16.SlotSelected += Card_SlotSelected;
        deckCard16.EditRequested += Card_EditRequested;
        deckCard16.ClearRequested += Card_ClearRequested;
        // 
        // deckCard17
        // 
        deckCard17.BackColor = AppColors.CardBg;
        deckCard17.IsSelected = false;
        deckCard17.Location = new Point(16, 620);
        deckCard17.Margin = new Padding(4);
        deckCard17.Name = "deckCard17";
        deckCard17.Size = new Size(204, 143);
        deckCard17.Slot = null;
        deckCard17.TabIndex = 16;
        deckCard17.SlotSelected += Card_SlotSelected;
        deckCard17.EditRequested += Card_EditRequested;
        deckCard17.ClearRequested += Card_ClearRequested;
        // 
        // deckCard18
        // 
        deckCard18.BackColor = AppColors.CardBg;
        deckCard18.IsSelected = false;
        deckCard18.Location = new Point(228, 620);
        deckCard18.Margin = new Padding(4);
        deckCard18.Name = "deckCard18";
        deckCard18.Size = new Size(204, 143);
        deckCard18.Slot = null;
        deckCard18.TabIndex = 17;
        deckCard18.SlotSelected += Card_SlotSelected;
        deckCard18.EditRequested += Card_EditRequested;
        deckCard18.ClearRequested += Card_ClearRequested;
        // 
        // deckCard19
        // 
        deckCard19.BackColor = AppColors.CardBg;
        deckCard19.IsSelected = false;
        deckCard19.Location = new Point(440, 620);
        deckCard19.Margin = new Padding(4);
        deckCard19.Name = "deckCard19";
        deckCard19.Size = new Size(204, 143);
        deckCard19.Slot = null;
        deckCard19.TabIndex = 18;
        deckCard19.SlotSelected += Card_SlotSelected;
        deckCard19.EditRequested += Card_EditRequested;
        deckCard19.ClearRequested += Card_ClearRequested;
        // 
        // deckCard20
        // 
        deckCard20.BackColor = AppColors.CardBg;
        deckCard20.IsSelected = false;
        deckCard20.Location = new Point(652, 620);
        deckCard20.Margin = new Padding(4);
        deckCard20.Name = "deckCard20";
        deckCard20.Size = new Size(204, 143);
        deckCard20.Slot = null;
        deckCard20.TabIndex = 19;
        deckCard20.SlotSelected += Card_SlotSelected;
        deckCard20.EditRequested += Card_EditRequested;
        deckCard20.ClearRequested += Card_ClearRequested;
        // 
        // deckCard21
        // 
        deckCard21.BackColor = AppColors.CardBg;
        deckCard21.IsSelected = false;
        deckCard21.Location = new Point(16, 771);
        deckCard21.Margin = new Padding(4);
        deckCard21.Name = "deckCard21";
        deckCard21.Size = new Size(204, 143);
        deckCard21.Slot = null;
        deckCard21.TabIndex = 20;
        deckCard21.SlotSelected += Card_SlotSelected;
        deckCard21.EditRequested += Card_EditRequested;
        deckCard21.ClearRequested += Card_ClearRequested;
        // 
        // deckCard22
        // 
        deckCard22.BackColor = AppColors.CardBg;
        deckCard22.IsSelected = false;
        deckCard22.Location = new Point(228, 771);
        deckCard22.Margin = new Padding(4);
        deckCard22.Name = "deckCard22";
        deckCard22.Size = new Size(204, 143);
        deckCard22.Slot = null;
        deckCard22.TabIndex = 21;
        deckCard22.SlotSelected += Card_SlotSelected;
        deckCard22.EditRequested += Card_EditRequested;
        deckCard22.ClearRequested += Card_ClearRequested;
        // 
        // deckCard23
        // 
        deckCard23.BackColor = AppColors.CardBg;
        deckCard23.IsSelected = false;
        deckCard23.Location = new Point(440, 771);
        deckCard23.Margin = new Padding(4);
        deckCard23.Name = "deckCard23";
        deckCard23.Size = new Size(204, 143);
        deckCard23.Slot = null;
        deckCard23.TabIndex = 22;
        deckCard23.SlotSelected += Card_SlotSelected;
        deckCard23.EditRequested += Card_EditRequested;
        deckCard23.ClearRequested += Card_ClearRequested;
        // 
        // deckCard24
        // 
        deckCard24.BackColor = AppColors.CardBg;
        deckCard24.IsSelected = false;
        deckCard24.Location = new Point(652, 771);
        deckCard24.Margin = new Padding(4);
        deckCard24.Name = "deckCard24";
        deckCard24.Size = new Size(204, 143);
        deckCard24.Slot = null;
        deckCard24.TabIndex = 23;
        deckCard24.SlotSelected += Card_SlotSelected;
        deckCard24.EditRequested += Card_EditRequested;
        deckCard24.ClearRequested += Card_ClearRequested;
        // 
        // deckCard25
        // 
        deckCard25.BackColor = AppColors.CardBg;
        deckCard25.IsSelected = false;
        deckCard25.Location = new Point(16, 922);
        deckCard25.Margin = new Padding(4);
        deckCard25.Name = "deckCard25";
        deckCard25.Size = new Size(204, 143);
        deckCard25.Slot = null;
        deckCard25.TabIndex = 24;
        deckCard25.SlotSelected += Card_SlotSelected;
        deckCard25.EditRequested += Card_EditRequested;
        deckCard25.ClearRequested += Card_ClearRequested;
        // 
        // deckCard26
        // 
        deckCard26.BackColor = AppColors.CardBg;
        deckCard26.IsSelected = false;
        deckCard26.Location = new Point(228, 922);
        deckCard26.Margin = new Padding(4);
        deckCard26.Name = "deckCard26";
        deckCard26.Size = new Size(204, 143);
        deckCard26.Slot = null;
        deckCard26.TabIndex = 25;
        deckCard26.SlotSelected += Card_SlotSelected;
        deckCard26.EditRequested += Card_EditRequested;
        deckCard26.ClearRequested += Card_ClearRequested;
        // 
        // deckCard27
        // 
        deckCard27.BackColor = AppColors.CardBg;
        deckCard27.IsSelected = false;
        deckCard27.Location = new Point(440, 922);
        deckCard27.Margin = new Padding(4);
        deckCard27.Name = "deckCard27";
        deckCard27.Size = new Size(204, 143);
        deckCard27.Slot = null;
        deckCard27.TabIndex = 26;
        deckCard27.SlotSelected += Card_SlotSelected;
        deckCard27.EditRequested += Card_EditRequested;
        deckCard27.ClearRequested += Card_ClearRequested;
        // 
        // deckCard28
        // 
        deckCard28.BackColor = AppColors.CardBg;
        deckCard28.IsSelected = false;
        deckCard28.Location = new Point(652, 922);
        deckCard28.Margin = new Padding(4);
        deckCard28.Name = "deckCard28";
        deckCard28.Size = new Size(204, 143);
        deckCard28.Slot = null;
        deckCard28.TabIndex = 27;
        deckCard28.SlotSelected += Card_SlotSelected;
        deckCard28.EditRequested += Card_EditRequested;
        deckCard28.ClearRequested += Card_ClearRequested;
        // 
        // deckCard29
        // 
        deckCard29.BackColor = AppColors.CardBg;
        deckCard29.IsSelected = false;
        deckCard29.Location = new Point(16, 1073);
        deckCard29.Margin = new Padding(4);
        deckCard29.Name = "deckCard29";
        deckCard29.Size = new Size(204, 143);
        deckCard29.Slot = null;
        deckCard29.TabIndex = 28;
        deckCard29.SlotSelected += Card_SlotSelected;
        deckCard29.EditRequested += Card_EditRequested;
        deckCard29.ClearRequested += Card_ClearRequested;
        // 
        // deckCard30
        // 
        deckCard30.BackColor = AppColors.CardBg;
        deckCard30.IsSelected = false;
        deckCard30.Location = new Point(228, 1073);
        deckCard30.Margin = new Padding(4);
        deckCard30.Name = "deckCard30";
        deckCard30.Size = new Size(204, 143);
        deckCard30.Slot = null;
        deckCard30.TabIndex = 29;
        deckCard30.SlotSelected += Card_SlotSelected;
        deckCard30.EditRequested += Card_EditRequested;
        deckCard30.ClearRequested += Card_ClearRequested;
        // 
        // deckCard31
        // 
        deckCard31.BackColor = AppColors.CardBg;
        deckCard31.IsSelected = false;
        deckCard31.Location = new Point(440, 1073);
        deckCard31.Margin = new Padding(4);
        deckCard31.Name = "deckCard31";
        deckCard31.Size = new Size(204, 143);
        deckCard31.Slot = null;
        deckCard31.TabIndex = 30;
        deckCard31.SlotSelected += Card_SlotSelected;
        deckCard31.EditRequested += Card_EditRequested;
        deckCard31.ClearRequested += Card_ClearRequested;
        // 
        // deckCard32
        // 
        deckCard32.BackColor = AppColors.CardBg;
        deckCard32.IsSelected = false;
        deckCard32.Location = new Point(652, 1073);
        deckCard32.Margin = new Padding(4);
        deckCard32.Name = "deckCard32";
        deckCard32.Size = new Size(204, 143);
        deckCard32.Slot = null;
        deckCard32.TabIndex = 31;
        deckCard32.SlotSelected += Card_SlotSelected;
        deckCard32.EditRequested += Card_EditRequested;
        deckCard32.ClearRequested += Card_ClearRequested;
        // 
        // pnlDetail
        // 
        pnlDetail.BackColor = AppColors.CardBg;
        pnlDetail.Controls.Add(pnlEmpty);
        pnlDetail.Controls.Add(tableLayoutPanel1);
        pnlDetail.Controls.Add(leftBorder);
        pnlDetail.Dock = DockStyle.Right;
        pnlDetail.Location = new Point(898, 0);
        pnlDetail.Name = "pnlDetail";
        pnlDetail.Size = new Size(279, 581);
        pnlDetail.TabIndex = 0;
        // 
        // pnlEmpty
        // 
        pnlEmpty.BackColor = Color.Transparent;
        pnlEmpty.Controls.Add(lblEmptyHint);
        pnlEmpty.Dock = DockStyle.Fill;
        pnlEmpty.Location = new Point(0, 0);
        pnlEmpty.Name = "pnlEmpty";
        pnlEmpty.Size = new Size(279, 581);
        pnlEmpty.TabIndex = 1;
        // 
        // lblEmptyHint
        // 
        lblEmptyHint.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblEmptyHint.BackColor = Color.Transparent;
        lblEmptyHint.Font = new Font("Segoe UI", 9F);
        lblEmptyHint.ForeColor = AppColors.TextDim;
        lblEmptyHint.Location = new Point(0, 0);
        lblEmptyHint.Name = "lblEmptyHint";
        lblEmptyHint.Size = new Size(279, 581);
        lblEmptyHint.TabIndex = 0;
        lblEmptyHint.Text = "Select a deck slot\nto view details";
        lblEmptyHint.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.AutoScroll = true;
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
        tableLayoutPanel1.Size = new Size(279, 581);
        tableLayoutPanel1.TabIndex = 3;
        // 
        // pnlActions
        // 
        pnlActions.AutoScroll = true;
        pnlActions.BackColor = Color.Transparent;
        pnlActions.Controls.Add(pnlActionsScroll);
        pnlActions.Location = new Point(3, 321);
        pnlActions.Name = "pnlActions";
        pnlActions.Size = new Size(240, 367);
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
        pnlActionsScroll.Controls.Add(btnImportYdkMulti);
        pnlActionsScroll.Controls.Add(btnExportYdk);
        pnlActionsScroll.Controls.Add(lblDangerTitle);
        pnlActionsScroll.Controls.Add(btnClearSlot);
        pnlActionsScroll.Dock = DockStyle.Fill;
        pnlActionsScroll.Location = new Point(0, 0);
        pnlActionsScroll.Name = "pnlActionsScroll";
        pnlActionsScroll.Size = new Size(240, 367);
        pnlActionsScroll.TabIndex = 0;
        // 
        // lblActionsTitle
        // 
        lblActionsTitle.BackColor = Color.Transparent;
        lblActionsTitle.Font = new Font("Segoe UI", 7F);
        lblActionsTitle.ForeColor = AppColors.TextDim;
        lblActionsTitle.Location = new Point(10, 0);
        lblActionsTitle.Name = "lblActionsTitle";
        lblActionsTitle.Size = new Size(200, 20);
        lblActionsTitle.TabIndex = 0;
        lblActionsTitle.Text = "ACTIONS";
        // 
        // btnEditCards
        // 
        btnEditCards.BackColor = AppColors.ActionBtnBg;
        btnEditCards.Cursor = Cursors.Hand;
        btnEditCards.FlatAppearance.BorderColor = AppColors.Border;
        btnEditCards.FlatAppearance.MouseOverBackColor = AppColors.CardHover;
        btnEditCards.FlatStyle = FlatStyle.Flat;
        btnEditCards.Font = new Font("Segoe UI", 9F);
        btnEditCards.ForeColor = AppColors.TextSecondary;
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
        btnChangeDuelist.BackColor = AppColors.ActionBtnBg;
        btnChangeDuelist.Cursor = Cursors.Hand;
        btnChangeDuelist.FlatAppearance.BorderColor = AppColors.Border;
        btnChangeDuelist.FlatAppearance.MouseOverBackColor = AppColors.CardHover;
        btnChangeDuelist.FlatStyle = FlatStyle.Flat;
        btnChangeDuelist.Font = new Font("Segoe UI", 9F);
        btnChangeDuelist.ForeColor = AppColors.TextSecondary;
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
        btnRename.BackColor = AppColors.ActionBtnBg;
        btnRename.Cursor = Cursors.Hand;
        btnRename.FlatAppearance.BorderColor = AppColors.Border;
        btnRename.FlatAppearance.MouseOverBackColor = AppColors.CardHover;
        btnRename.FlatStyle = FlatStyle.Flat;
        btnRename.Font = new Font("Segoe UI", 9F);
        btnRename.ForeColor = AppColors.TextSecondary;
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
        btnCopyTo.BackColor = AppColors.ActionBtnBg;
        btnCopyTo.Cursor = Cursors.Hand;
        btnCopyTo.FlatAppearance.BorderColor = AppColors.Border;
        btnCopyTo.FlatAppearance.MouseOverBackColor = AppColors.CardHover;
        btnCopyTo.FlatStyle = FlatStyle.Flat;
        btnCopyTo.Font = new Font("Segoe UI", 9F);
        btnCopyTo.ForeColor = AppColors.TextSecondary;
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
        btnSwapWith.BackColor = AppColors.ActionBtnBg;
        btnSwapWith.Cursor = Cursors.Hand;
        btnSwapWith.FlatAppearance.BorderColor = AppColors.Border;
        btnSwapWith.FlatAppearance.MouseOverBackColor = AppColors.CardHover;
        btnSwapWith.FlatStyle = FlatStyle.Flat;
        btnSwapWith.Font = new Font("Segoe UI", 9F);
        btnSwapWith.ForeColor = AppColors.TextSecondary;
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
        btnImportYdk.BackColor = AppColors.ActionBtnBg;
        btnImportYdk.Cursor = Cursors.Hand;
        btnImportYdk.FlatAppearance.BorderColor = AppColors.Border;
        btnImportYdk.FlatAppearance.MouseOverBackColor = AppColors.CardHover;
        btnImportYdk.FlatStyle = FlatStyle.Flat;
        btnImportYdk.Font = new Font("Segoe UI", 9F);
        btnImportYdk.ForeColor = AppColors.TextSecondary;
        btnImportYdk.Location = new Point(25, 194);
        btnImportYdk.Name = "btnImportYdk";
        btnImportYdk.Padding = new Padding(6, 0, 0, 0);
        btnImportYdk.Size = new Size(200, 30);
        btnImportYdk.TabIndex = 6;
        btnImportYdk.Text = "Import .ydk to slot";
        btnImportYdk.TextAlign = ContentAlignment.MiddleLeft;
        btnImportYdk.UseVisualStyleBackColor = false;
        btnImportYdk.Click += btnImportYdk_Click;
        //
        // btnImportYdkMulti
        //
        btnImportYdkMulti.BackColor = AppColors.ActionBtnBg;
        btnImportYdkMulti.Cursor = Cursors.Hand;
        btnImportYdkMulti.FlatAppearance.BorderColor = AppColors.Border;
        btnImportYdkMulti.FlatAppearance.MouseOverBackColor = AppColors.CardHover;
        btnImportYdkMulti.FlatStyle = FlatStyle.Flat;
        btnImportYdkMulti.Font = new Font("Segoe UI", 9F);
        btnImportYdkMulti.ForeColor = AppColors.TextSecondary;
        btnImportYdkMulti.Location = new Point(25, 228);
        btnImportYdkMulti.Name = "btnImportYdkMulti";
        btnImportYdkMulti.Padding = new Padding(6, 0, 0, 0);
        btnImportYdkMulti.Size = new Size(200, 30);
        btnImportYdkMulti.TabIndex = 7;
        btnImportYdkMulti.Text = "Import multiple .ydk…";
        btnImportYdkMulti.TextAlign = ContentAlignment.MiddleLeft;
        btnImportYdkMulti.UseVisualStyleBackColor = false;
        btnImportYdkMulti.Click += btnImportYdkMulti_Click;
        //
        // btnExportYdk
        //
        btnExportYdk.BackColor = AppColors.ActionBtnBg;
        btnExportYdk.Cursor = Cursors.Hand;
        btnExportYdk.FlatAppearance.BorderColor = AppColors.Border;
        btnExportYdk.FlatAppearance.MouseOverBackColor = AppColors.CardHover;
        btnExportYdk.FlatStyle = FlatStyle.Flat;
        btnExportYdk.Font = new Font("Segoe UI", 9F);
        btnExportYdk.ForeColor = AppColors.TextSecondary;
        btnExportYdk.Location = new Point(25, 262);
        btnExportYdk.Name = "btnExportYdk";
        btnExportYdk.Padding = new Padding(6, 0, 0, 0);
        btnExportYdk.Size = new Size(200, 30);
        btnExportYdk.TabIndex = 8;
        btnExportYdk.Text = "Export slot as .ydk";
        btnExportYdk.TextAlign = ContentAlignment.MiddleLeft;
        btnExportYdk.UseVisualStyleBackColor = false;
        btnExportYdk.Click += btnExportYdk_Click;
        //
        // lblDangerTitle
        //
        lblDangerTitle.BackColor = Color.Transparent;
        lblDangerTitle.Font = new Font("Segoe UI", 7F);
        lblDangerTitle.ForeColor = AppColors.TextDim;
        lblDangerTitle.Location = new Point(10, 304);
        lblDangerTitle.Name = "lblDangerTitle";
        lblDangerTitle.Size = new Size(200, 20);
        lblDangerTitle.TabIndex = 9;
        lblDangerTitle.Text = "DANGER ZONE";
        //
        // btnClearSlot
        //
        btnClearSlot.BackColor = AppColors.DangerBorder;
        btnClearSlot.Cursor = Cursors.Hand;
        btnClearSlot.FlatAppearance.BorderColor = AppColors.DangerBorder;
        btnClearSlot.FlatAppearance.MouseOverBackColor = AppColors.DangerHoverBg;
        btnClearSlot.FlatStyle = FlatStyle.Flat;
        btnClearSlot.Font = new Font("Segoe UI", 9F);
        btnClearSlot.ForeColor = AppColors.DangerFg;
        btnClearSlot.Location = new Point(25, 328);
        btnClearSlot.Name = "btnClearSlot";
        btnClearSlot.Padding = new Padding(6, 0, 0, 0);
        btnClearSlot.Size = new Size(200, 30);
        btnClearSlot.TabIndex = 10;
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
        pnlStats.Size = new Size(273, 125);
        pnlStats.TabIndex = 3;
        // 
        // lblCountsTitle
        // 
        lblCountsTitle.BackColor = Color.Transparent;
        lblCountsTitle.Font = new Font("Segoe UI", 7F);
        lblCountsTitle.ForeColor = AppColors.TextDim;
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
        pnlMainBarRow.Size = new Size(253, 25);
        pnlMainBarRow.TabIndex = 1;
        // 
        // lblMainName
        // 
        lblMainName.BackColor = Color.Transparent;
        lblMainName.Font = new Font("Segoe UI", 7F);
        lblMainName.ForeColor = AppColors.TextSecondary;
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
        lblMainBar.ForeColor = AppColors.TextSecondary;
        lblMainBar.Location = new Point(112, 0);
        lblMainBar.Name = "lblMainBar";
        lblMainBar.Size = new Size(113, 16);
        lblMainBar.TabIndex = 1;
        lblMainBar.Text = "0/60";
        lblMainBar.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlMainTrack
        // 
        pnlMainTrack.BackColor = AppColors.CardSelected;
        pnlMainTrack.Controls.Add(pnlMainFill);
        pnlMainTrack.Location = new Point(0, 18);
        pnlMainTrack.Name = "pnlMainTrack";
        pnlMainTrack.Size = new Size(225, 4);
        pnlMainTrack.TabIndex = 2;
        // 
        // pnlMainFill
        // 
        pnlMainFill.BackColor = AppColors.MainGreen;
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
        pnlExtraBarRow.Size = new Size(253, 25);
        pnlExtraBarRow.TabIndex = 2;
        // 
        // lblExtraName
        // 
        lblExtraName.BackColor = Color.Transparent;
        lblExtraName.Font = new Font("Segoe UI", 7F);
        lblExtraName.ForeColor = AppColors.TextSecondary;
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
        lblExtraBar.ForeColor = AppColors.TextSecondary;
        lblExtraBar.Location = new Point(112, 0);
        lblExtraBar.Name = "lblExtraBar";
        lblExtraBar.Size = new Size(113, 16);
        lblExtraBar.TabIndex = 1;
        lblExtraBar.Text = "0/15";
        lblExtraBar.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlExtraTrack
        // 
        pnlExtraTrack.BackColor = AppColors.CardSelected;
        pnlExtraTrack.Controls.Add(pnlExtraFill);
        pnlExtraTrack.Location = new Point(0, 18);
        pnlExtraTrack.Name = "pnlExtraTrack";
        pnlExtraTrack.Size = new Size(225, 4);
        pnlExtraTrack.TabIndex = 2;
        // 
        // pnlExtraFill
        // 
        pnlExtraFill.BackColor = AppColors.ExtraBlue;
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
        pnlSideBarRow.Size = new Size(253, 25);
        pnlSideBarRow.TabIndex = 3;
        // 
        // lblSideName
        // 
        lblSideName.BackColor = Color.Transparent;
        lblSideName.Font = new Font("Segoe UI", 7F);
        lblSideName.ForeColor = AppColors.TextSecondary;
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
        lblSideBar.ForeColor = AppColors.TextSecondary;
        lblSideBar.Location = new Point(112, 0);
        lblSideBar.Name = "lblSideBar";
        lblSideBar.Size = new Size(113, 16);
        lblSideBar.TabIndex = 1;
        lblSideBar.Text = "0/15";
        lblSideBar.TextAlign = ContentAlignment.MiddleRight;
        // 
        // pnlSideTrack
        // 
        pnlSideTrack.BackColor = AppColors.CardSelected;
        pnlSideTrack.Controls.Add(pnlSideFill);
        pnlSideTrack.Location = new Point(0, 18);
        pnlSideTrack.Name = "pnlSideTrack";
        pnlSideTrack.Size = new Size(225, 4);
        pnlSideTrack.TabIndex = 2;
        // 
        // pnlSideFill
        // 
        pnlSideFill.BackColor = AppColors.SideRed;
        pnlSideFill.Location = new Point(0, 0);
        pnlSideFill.Name = "pnlSideFill";
        pnlSideFill.Size = new Size(0, 4);
        pnlSideFill.TabIndex = 0;
        // 
        // pnlStatsDivider
        // 
        pnlStatsDivider.BackColor = AppColors.Border;
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
        pnlHeader.Size = new Size(273, 58);
        pnlHeader.TabIndex = 2;
        // 
        // lblHeaderTag
        // 
        lblHeaderTag.BackColor = Color.Transparent;
        lblHeaderTag.Font = new Font("Segoe UI", 7F);
        lblHeaderTag.ForeColor = AppColors.TextDim;
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
        lblOwnerName.ForeColor = AppColors.TextPrimary;
        lblOwnerName.Location = new Point(2, 20);
        lblOwnerName.Name = "lblOwnerName";
        lblOwnerName.Padding = new Padding(8, 0, 4, 0);
        lblOwnerName.Size = new Size(254, 20);
        lblOwnerName.TabIndex = 1;
        lblOwnerName.Text = "__";
        lblOwnerName.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlHeaderDivider
        // 
        pnlHeaderDivider.BackColor = AppColors.Border;
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
        lblOwnerId.ForeColor = AppColors.TextGold;
        lblOwnerId.Location = new Point(4, 38);
        lblOwnerId.Name = "lblOwnerId";
        lblOwnerId.Padding = new Padding(8, 0, 4, 0);
        lblOwnerId.Size = new Size(252, 16);
        lblOwnerId.TabIndex = 2;
        lblOwnerId.Text = "—";
        lblOwnerId.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlPortraitDivider
        // 
        pnlPortraitDivider.BackColor = AppColors.Border;
        pnlPortraitDivider.Dock = DockStyle.Fill;
        pnlPortraitDivider.Location = new Point(3, 119);
        pnlPortraitDivider.Name = "pnlPortraitDivider";
        pnlPortraitDivider.Size = new Size(273, 1);
        pnlPortraitDivider.TabIndex = 1;
        // 
        // pnlPortrait
        // 
        pnlPortrait.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlPortrait.BackColor = AppColors.PortraitBg;
        pnlPortrait.Controls.Add(picPortrait);
        pnlPortrait.Controls.Add(lblSlotNum);
        pnlPortrait.Controls.Add(lblDeckName);
        pnlPortrait.Location = new Point(3, 3);
        pnlPortrait.Name = "pnlPortrait";
        pnlPortrait.Size = new Size(273, 110);
        pnlPortrait.TabIndex = 0;
        // 
        // picPortrait
        // 
        picPortrait.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        picPortrait.BackColor = Color.Transparent;
        picPortrait.Location = new Point(133, 3);
        picPortrait.Name = "picPortrait";
        picPortrait.Size = new Size(123, 100);
        picPortrait.SizeMode = PictureBoxSizeMode.Zoom;
        picPortrait.TabIndex = 0;
        picPortrait.TabStop = false;
        // 
        // lblSlotNum
        // 
        lblSlotNum.BackColor = Color.Transparent;
        lblSlotNum.Font = new Font("Segoe UI", 8F);
        lblSlotNum.ForeColor = AppColors.TextGold;
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
        lblDeckName.ForeColor = AppColors.TextPrimary;
        lblDeckName.Location = new Point(4, 3);
        lblDeckName.Name = "lblDeckName";
        lblDeckName.Size = new Size(118, 86);
        lblDeckName.TabIndex = 1;
        lblDeckName.Text = "__";
        // 
        // leftBorder
        // 
        leftBorder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        leftBorder.BackColor = AppColors.Border;
        leftBorder.Location = new Point(0, 0);
        leftBorder.Name = "leftBorder";
        leftBorder.Size = new Size(1, 654);
        leftBorder.TabIndex = 0;
        // 
        // pnlDivider
        // 
        pnlDivider.BackColor = AppColors.Border;
        pnlDivider.Dock = DockStyle.Top;
        pnlDivider.Location = new Point(0, 44);
        pnlDivider.Name = "pnlDivider";
        pnlDivider.Size = new Size(1177, 1);
        pnlDivider.TabIndex = 1;
        // 
        // DeckSlotsPage
        // 
        BackColor = AppColors.AppBg;
        Controls.Add(pnlBody);
        Controls.Add(pnlDivider);
        Controls.Add(pnlToolbar);
        Name = "DeckSlotsPage";
        Size = new Size(1177, 626);
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        pnlBody.ResumeLayout(false);
        pnlGrid.ResumeLayout(false);
        pnlDetail.ResumeLayout(false);
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


}
