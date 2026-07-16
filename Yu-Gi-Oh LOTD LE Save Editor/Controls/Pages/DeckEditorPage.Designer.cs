using System.Xml.Linq;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;
using static ReaLTaiizor.Drawing.Poison.PoisonPaint;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

partial class DeckEditorPage
{
    // ── Toolbar ────────────────────────────────────────────────────────────────
    private TextBox txtSearch;
    private DLButton btnFilter;
    private DLButton btnSort;
    private DLButton btnSave;
    private Label lblDeckName;
    private Panel pnlToolbar;
    private Panel pnlToolbarDiv;
    // ── Left search panel ──────────────────────────────────────────────────────
    private Panel pnlSearch;
    private ListBox lstResults;
    private Panel pnlSearchDiv;
    // ── Centre deck panel ──────────────────────────────────────────────────────
    private Panel pnlDeck;
    private Panel pnlDeckScroll;
    private Label lblMainCount;
    private Panel pnlMainGrid;
    private Label lblExtraCount;
    private Panel pnlExtraGrid;
    private Label lblSideCount;
    private Panel pnlSideGrid;
    // ── Right preview panel ────────────────────────────────────────────────────
    private Panel pnlPreview;
    private Panel pnlPreviewDiv;
    private PictureBox picPreview;
    private Panel pnlPreviewText;
    private Label lblPreviewName;
    private Label lblPreviewType;
    private Label lblPreviewAttr;
    private Label lblPreviewRace;
    private Label lblPreviewAtk;
    private Label lblPreviewDef;
    private Label lblPreviewDesc;
    // ── Card slots ────────────────────────────────────────────────────────────
    private CardSlot slotMain00;
    private CardSlot slotMain01;
    private CardSlot slotMain02;
    private CardSlot slotMain03;
    private CardSlot slotMain04;
    private CardSlot slotMain05;
    private CardSlot slotMain06;
    private CardSlot slotMain07;
    private CardSlot slotMain08;
    private CardSlot slotMain09;
    private CardSlot slotMain10;
    private CardSlot slotMain11;
    private CardSlot slotMain12;
    private CardSlot slotMain13;
    private CardSlot slotMain14;
    private CardSlot slotMain15;
    private CardSlot slotMain16;
    private CardSlot slotMain17;
    private CardSlot slotMain18;
    private CardSlot slotMain19;
    private CardSlot slotMain20;
    private CardSlot slotMain21;
    private CardSlot slotMain22;
    private CardSlot slotMain23;
    private CardSlot slotMain24;
    private CardSlot slotMain25;
    private CardSlot slotMain26;
    private CardSlot slotMain27;
    private CardSlot slotMain28;
    private CardSlot slotMain29;
    private CardSlot slotMain30;
    private CardSlot slotMain31;
    private CardSlot slotMain32;
    private CardSlot slotMain33;
    private CardSlot slotMain34;
    private CardSlot slotMain35;
    private CardSlot slotMain36;
    private CardSlot slotMain37;
    private CardSlot slotMain38;
    private CardSlot slotMain39;
    private CardSlot slotMain40;
    private CardSlot slotMain41;
    private CardSlot slotMain42;
    private CardSlot slotMain43;
    private CardSlot slotMain44;
    private CardSlot slotMain45;
    private CardSlot slotMain46;
    private CardSlot slotMain47;
    private CardSlot slotMain48;
    private CardSlot slotMain49;
    private CardSlot slotMain50;
    private CardSlot slotMain51;
    private CardSlot slotMain52;
    private CardSlot slotMain53;
    private CardSlot slotMain54;
    private CardSlot slotMain55;
    private CardSlot slotMain56;
    private CardSlot slotMain57;
    private CardSlot slotMain58;
    private CardSlot slotMain59;
    private CardSlot slotExtra00;
    private CardSlot slotExtra01;
    private CardSlot slotExtra02;
    private CardSlot slotExtra03;
    private CardSlot slotExtra04;
    private CardSlot slotExtra05;
    private CardSlot slotExtra06;
    private CardSlot slotExtra07;
    private CardSlot slotExtra08;
    private CardSlot slotExtra09;
    private CardSlot slotExtra10;
    private CardSlot slotExtra11;
    private CardSlot slotExtra12;
    private CardSlot slotExtra13;
    private CardSlot slotExtra14;
    private CardSlot slotSide00;
    private CardSlot slotSide01;
    private CardSlot slotSide02;
    private CardSlot slotSide03;
    private CardSlot slotSide04;
    private CardSlot slotSide05;
    private CardSlot slotSide06;
    private CardSlot slotSide07;
    private CardSlot slotSide08;
    private CardSlot slotSide09;
    private CardSlot slotSide10;
    private CardSlot slotSide11;
    private CardSlot slotSide12;
    private CardSlot slotSide13;
    private CardSlot slotSide14;
    // ── In-window zoom overlay ─────────────────────────────────────────────────
    private CardImageOverlay imgOverlay;

    private void InitializeComponent()
    {
        txtSearch = new TextBox();
        btnFilter = new DLButton();
        btnSort = new DLButton();
        btnSave = new DLButton();
        lblDeckName = new Label();
        pnlToolbar = new Panel();
        pnlToolbarDiv = new Panel();
        pnlSearch = new Panel();
        lstResults = new ListBox();
        pnlSearchDiv = new Panel();
        pnlDeck = new Panel();
        pnlDeckScroll = new Panel();
        lblMainCount = new Label();
        pnlMainGrid = new Panel();
        slotMain00 = new CardSlot();
        slotMain01 = new CardSlot();
        slotMain02 = new CardSlot();
        slotMain03 = new CardSlot();
        slotMain04 = new CardSlot();
        slotMain05 = new CardSlot();
        slotMain06 = new CardSlot();
        slotMain07 = new CardSlot();
        slotMain08 = new CardSlot();
        slotMain09 = new CardSlot();
        slotMain10 = new CardSlot();
        slotMain11 = new CardSlot();
        slotMain12 = new CardSlot();
        slotMain13 = new CardSlot();
        slotMain14 = new CardSlot();
        slotMain15 = new CardSlot();
        slotMain16 = new CardSlot();
        slotMain17 = new CardSlot();
        slotMain18 = new CardSlot();
        slotMain19 = new CardSlot();
        slotMain20 = new CardSlot();
        slotMain21 = new CardSlot();
        slotMain22 = new CardSlot();
        slotMain23 = new CardSlot();
        slotMain24 = new CardSlot();
        slotMain25 = new CardSlot();
        slotMain26 = new CardSlot();
        slotMain27 = new CardSlot();
        slotMain28 = new CardSlot();
        slotMain29 = new CardSlot();
        slotMain30 = new CardSlot();
        slotMain31 = new CardSlot();
        slotMain32 = new CardSlot();
        slotMain33 = new CardSlot();
        slotMain34 = new CardSlot();
        slotMain35 = new CardSlot();
        slotMain36 = new CardSlot();
        slotMain37 = new CardSlot();
        slotMain38 = new CardSlot();
        slotMain39 = new CardSlot();
        slotMain40 = new CardSlot();
        slotMain41 = new CardSlot();
        slotMain42 = new CardSlot();
        slotMain43 = new CardSlot();
        slotMain44 = new CardSlot();
        slotMain45 = new CardSlot();
        slotMain46 = new CardSlot();
        slotMain47 = new CardSlot();
        slotMain48 = new CardSlot();
        slotMain49 = new CardSlot();
        slotMain50 = new CardSlot();
        slotMain51 = new CardSlot();
        slotMain52 = new CardSlot();
        slotMain53 = new CardSlot();
        slotMain54 = new CardSlot();
        slotMain55 = new CardSlot();
        slotMain56 = new CardSlot();
        slotMain57 = new CardSlot();
        slotMain58 = new CardSlot();
        slotMain59 = new CardSlot();
        lblExtraCount = new Label();
        pnlExtraGrid = new Panel();
        slotExtra00 = new CardSlot();
        slotExtra01 = new CardSlot();
        slotExtra02 = new CardSlot();
        slotExtra03 = new CardSlot();
        slotExtra04 = new CardSlot();
        slotExtra05 = new CardSlot();
        slotExtra06 = new CardSlot();
        slotExtra07 = new CardSlot();
        slotExtra08 = new CardSlot();
        slotExtra09 = new CardSlot();
        slotExtra10 = new CardSlot();
        slotExtra11 = new CardSlot();
        slotExtra12 = new CardSlot();
        slotExtra13 = new CardSlot();
        slotExtra14 = new CardSlot();
        lblSideCount = new Label();
        pnlSideGrid = new Panel();
        slotSide00 = new CardSlot();
        slotSide01 = new CardSlot();
        slotSide02 = new CardSlot();
        slotSide03 = new CardSlot();
        slotSide04 = new CardSlot();
        slotSide05 = new CardSlot();
        slotSide06 = new CardSlot();
        slotSide07 = new CardSlot();
        slotSide08 = new CardSlot();
        slotSide09 = new CardSlot();
        slotSide10 = new CardSlot();
        slotSide11 = new CardSlot();
        slotSide12 = new CardSlot();
        slotSide13 = new CardSlot();
        slotSide14 = new CardSlot();
        pnlPreview = new Panel();
        picPreview = new PictureBox();
        pnlPreviewText = new Panel();
        lblPreviewName = new Label();
        lblPreviewType = new Label();
        lblPreviewAttr = new Label();
        lblPreviewRace = new Label();
        lblPreviewAtk = new Label();
        lblPreviewDef = new Label();
        lblPreviewDesc = new Label();
        pnlPreviewDiv = new Panel();
        imgOverlay = new CardImageOverlay();
        pnlToolbar.SuspendLayout();
        pnlSearch.SuspendLayout();
        pnlDeck.SuspendLayout();
        pnlDeckScroll.SuspendLayout();
        pnlMainGrid.SuspendLayout();
        pnlExtraGrid.SuspendLayout();
        pnlSideGrid.SuspendLayout();
        pnlPreview.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
        pnlPreviewText.SuspendLayout();
        SuspendLayout();
        // 
        // txtSearch
        // 
        txtSearch.BackColor = AppColors.InputBg;
        txtSearch.BorderStyle = BorderStyle.FixedSingle;
        txtSearch.Font = new Font("Segoe UI", 9F);
        txtSearch.ForeColor = AppColors.TextPrimary;
        txtSearch.Location = new Point(8, 10);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search cards…";
        txtSearch.Size = new Size(180, 23);
        txtSearch.TabIndex = 0;
        txtSearch.TextChanged += txtSearch_TextChanged;
        //
        // btnFilter
        //
        btnFilter.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnFilter.Location = new Point(196, 8);
        btnFilter.Name = "btnFilter";
        btnFilter.Size = new Size(70, 28);
        btnFilter.TabIndex = 1;
        btnFilter.Text = "Filter";
        btnFilter.Variant = DLButtonVariant.Secondary;
        btnFilter.Click += btnFilter_Click;
        //
        // btnSort
        //
        btnSort.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnSort.Location = new Point(272, 8);
        btnSort.Name = "btnSort";
        btnSort.Size = new Size(70, 28);
        btnSort.TabIndex = 2;
        btnSort.Text = "Sort";
        btnSort.Variant = DLButtonVariant.Secondary;
        btnSort.Click += btnSort_Click;
        //
        // btnSave
        //
        btnSave.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnSave.Location = new Point(348, 8);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(70, 28);
        btnSave.TabIndex = 3;
        btnSave.Text = "Save";
        btnSave.Variant = DLButtonVariant.Primary;
        btnSave.Click += btnSave_Click;
        //
        // lblDeckName
        //
        lblDeckName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblDeckName.BackColor = Color.Transparent;
        lblDeckName.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDeckName.ForeColor = AppColors.TextGold;
        lblDeckName.Location = new Point(426, 10);
        lblDeckName.Name = "lblDeckName";
        lblDeckName.Size = new Size(414, 24);
        lblDeckName.TabIndex = 4;
        lblDeckName.Text = "— No deck loaded —";
        lblDeckName.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlToolbar
        // 
        pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlToolbar.BackColor = AppColors.TopbarBg;
        pnlToolbar.Controls.Add(txtSearch);
        pnlToolbar.Controls.Add(btnFilter);
        pnlToolbar.Controls.Add(btnSort);
        pnlToolbar.Controls.Add(btnSave);
        pnlToolbar.Controls.Add(lblDeckName);
        pnlToolbar.Location = new Point(0, 0);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1200, 44);
        pnlToolbar.TabIndex = 0;
        // 
        // pnlToolbarDiv
        // 
        pnlToolbarDiv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlToolbarDiv.BackColor = AppColors.Border;
        pnlToolbarDiv.Location = new Point(0, 44);
        pnlToolbarDiv.Name = "pnlToolbarDiv";
        pnlToolbarDiv.Size = new Size(1200, 1);
        pnlToolbarDiv.TabIndex = 1;
        // 
        // pnlSearch
        // 
        pnlSearch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlSearch.BackColor = AppColors.InputBg;
        pnlSearch.Controls.Add(lstResults);
        pnlSearch.Location = new Point(0, 45);
        pnlSearch.Name = "pnlSearch";
        pnlSearch.Size = new Size(220, 755);
        pnlSearch.TabIndex = 2;
        // 
        // lstResults
        // 
        lstResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lstResults.BackColor = AppColors.CardBg;
        lstResults.BorderStyle = BorderStyle.None;
        lstResults.DrawMode = DrawMode.OwnerDrawFixed;
        lstResults.Font = new Font("Segoe UI", 9.5F);
        lstResults.ForeColor = AppColors.TextSecondary;
        lstResults.IntegralHeight = false;
        lstResults.ItemHeight = 26;
        lstResults.Location = new Point(0, 0);
        lstResults.Name = "lstResults";
        lstResults.Size = new Size(220, 755);
        lstResults.TabIndex = 0;
        lstResults.DrawItem += lstResults_DrawItem;
        lstResults.SelectedIndexChanged += lstResults_SelectedIndexChanged;
        lstResults.DoubleClick += lstResults_DoubleClick;
        // 
        // pnlSearchDiv
        // 
        pnlSearchDiv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlSearchDiv.BackColor = AppColors.Border;
        pnlSearchDiv.Location = new Point(220, 45);
        pnlSearchDiv.Name = "pnlSearchDiv";
        pnlSearchDiv.Size = new Size(1, 755);
        pnlSearchDiv.TabIndex = 3;
        // 
        // pnlDeck
        // 
        pnlDeck.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlDeck.BackColor = AppColors.AppBg;
        pnlDeck.Controls.Add(pnlDeckScroll);
        pnlDeck.Location = new Point(221, 45);
        pnlDeck.Name = "pnlDeck";
        pnlDeck.Size = new Size(759, 755);
        pnlDeck.TabIndex = 4;
        pnlDeck.Paint += BackgroundPainter.Paint;
        // 
        // pnlDeckScroll
        // 
        pnlDeckScroll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlDeckScroll.AutoScroll = true;
        pnlDeckScroll.BackColor = Color.Transparent;
        pnlDeckScroll.Controls.Add(lblMainCount);
        pnlDeckScroll.Controls.Add(pnlMainGrid);
        pnlDeckScroll.Controls.Add(lblExtraCount);
        pnlDeckScroll.Controls.Add(pnlExtraGrid);
        pnlDeckScroll.Controls.Add(lblSideCount);
        pnlDeckScroll.Controls.Add(pnlSideGrid);
        pnlDeckScroll.Location = new Point(0, 0);
        pnlDeckScroll.Name = "pnlDeckScroll";
        pnlDeckScroll.Size = new Size(759, 755);
        pnlDeckScroll.TabIndex = 0;
        // 
        // lblMainCount
        // 
        lblMainCount.BackColor = Color.Transparent;
        lblMainCount.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblMainCount.ForeColor = AppColors.TextGold;
        lblMainCount.Location = new Point(8, 8);
        lblMainCount.Name = "lblMainCount";
        lblMainCount.Size = new Size(740, 20);
        lblMainCount.TabIndex = 0;
        lblMainCount.Text = "MAIN DECK  (0/60)";
        // 
        // pnlMainGrid
        // 
        pnlMainGrid.BackColor = Color.Transparent;
        pnlMainGrid.Controls.Add(slotMain00);
        pnlMainGrid.Controls.Add(slotMain01);
        pnlMainGrid.Controls.Add(slotMain02);
        pnlMainGrid.Controls.Add(slotMain03);
        pnlMainGrid.Controls.Add(slotMain04);
        pnlMainGrid.Controls.Add(slotMain05);
        pnlMainGrid.Controls.Add(slotMain06);
        pnlMainGrid.Controls.Add(slotMain07);
        pnlMainGrid.Controls.Add(slotMain08);
        pnlMainGrid.Controls.Add(slotMain09);
        pnlMainGrid.Controls.Add(slotMain10);
        pnlMainGrid.Controls.Add(slotMain11);
        pnlMainGrid.Controls.Add(slotMain12);
        pnlMainGrid.Controls.Add(slotMain13);
        pnlMainGrid.Controls.Add(slotMain14);
        pnlMainGrid.Controls.Add(slotMain15);
        pnlMainGrid.Controls.Add(slotMain16);
        pnlMainGrid.Controls.Add(slotMain17);
        pnlMainGrid.Controls.Add(slotMain18);
        pnlMainGrid.Controls.Add(slotMain19);
        pnlMainGrid.Controls.Add(slotMain20);
        pnlMainGrid.Controls.Add(slotMain21);
        pnlMainGrid.Controls.Add(slotMain22);
        pnlMainGrid.Controls.Add(slotMain23);
        pnlMainGrid.Controls.Add(slotMain24);
        pnlMainGrid.Controls.Add(slotMain25);
        pnlMainGrid.Controls.Add(slotMain26);
        pnlMainGrid.Controls.Add(slotMain27);
        pnlMainGrid.Controls.Add(slotMain28);
        pnlMainGrid.Controls.Add(slotMain29);
        pnlMainGrid.Controls.Add(slotMain30);
        pnlMainGrid.Controls.Add(slotMain31);
        pnlMainGrid.Controls.Add(slotMain32);
        pnlMainGrid.Controls.Add(slotMain33);
        pnlMainGrid.Controls.Add(slotMain34);
        pnlMainGrid.Controls.Add(slotMain35);
        pnlMainGrid.Controls.Add(slotMain36);
        pnlMainGrid.Controls.Add(slotMain37);
        pnlMainGrid.Controls.Add(slotMain38);
        pnlMainGrid.Controls.Add(slotMain39);
        pnlMainGrid.Controls.Add(slotMain40);
        pnlMainGrid.Controls.Add(slotMain41);
        pnlMainGrid.Controls.Add(slotMain42);
        pnlMainGrid.Controls.Add(slotMain43);
        pnlMainGrid.Controls.Add(slotMain44);
        pnlMainGrid.Controls.Add(slotMain45);
        pnlMainGrid.Controls.Add(slotMain46);
        pnlMainGrid.Controls.Add(slotMain47);
        pnlMainGrid.Controls.Add(slotMain48);
        pnlMainGrid.Controls.Add(slotMain49);
        pnlMainGrid.Controls.Add(slotMain50);
        pnlMainGrid.Controls.Add(slotMain51);
        pnlMainGrid.Controls.Add(slotMain52);
        pnlMainGrid.Controls.Add(slotMain53);
        pnlMainGrid.Controls.Add(slotMain54);
        pnlMainGrid.Controls.Add(slotMain55);
        pnlMainGrid.Controls.Add(slotMain56);
        pnlMainGrid.Controls.Add(slotMain57);
        pnlMainGrid.Controls.Add(slotMain58);
        pnlMainGrid.Controls.Add(slotMain59);
        pnlMainGrid.Location = new Point(8, 32);
        pnlMainGrid.Name = "pnlMainGrid";
        pnlMainGrid.Size = new Size(740, 548);
        pnlMainGrid.TabIndex = 1;
        // 
        // slotMain00
        // 
        slotMain00.BackColor = AppColors.CardBg;
        slotMain00.IsOnline = false;
        slotMain00.IsSelected = false;
        slotMain00.Location = new Point(0, 0);
        slotMain00.Name = "slotMain00";
        slotMain00.Size = new Size(60, 88);
        slotMain00.TabIndex = 0;
        // 
        // slotMain01
        // 
        slotMain01.BackColor = AppColors.CardBg;
        slotMain01.IsOnline = false;
        slotMain01.IsSelected = false;
        slotMain01.Location = new Point(64, 0);
        slotMain01.Name = "slotMain01";
        slotMain01.Size = new Size(60, 88);
        slotMain01.TabIndex = 1;
        // 
        // slotMain02
        // 
        slotMain02.BackColor = AppColors.CardBg;
        slotMain02.IsOnline = false;
        slotMain02.IsSelected = false;
        slotMain02.Location = new Point(128, 0);
        slotMain02.Name = "slotMain02";
        slotMain02.Size = new Size(60, 88);
        slotMain02.TabIndex = 2;
        // 
        // slotMain03
        // 
        slotMain03.BackColor = AppColors.CardBg;
        slotMain03.IsOnline = false;
        slotMain03.IsSelected = false;
        slotMain03.Location = new Point(192, 0);
        slotMain03.Name = "slotMain03";
        slotMain03.Size = new Size(60, 88);
        slotMain03.TabIndex = 3;
        // 
        // slotMain04
        // 
        slotMain04.BackColor = AppColors.CardBg;
        slotMain04.IsOnline = false;
        slotMain04.IsSelected = false;
        slotMain04.Location = new Point(256, 0);
        slotMain04.Name = "slotMain04";
        slotMain04.Size = new Size(60, 88);
        slotMain04.TabIndex = 4;
        // 
        // slotMain05
        // 
        slotMain05.BackColor = AppColors.CardBg;
        slotMain05.IsOnline = false;
        slotMain05.IsSelected = false;
        slotMain05.Location = new Point(320, 0);
        slotMain05.Name = "slotMain05";
        slotMain05.Size = new Size(60, 88);
        slotMain05.TabIndex = 5;
        // 
        // slotMain06
        // 
        slotMain06.BackColor = AppColors.CardBg;
        slotMain06.IsOnline = false;
        slotMain06.IsSelected = false;
        slotMain06.Location = new Point(384, 0);
        slotMain06.Name = "slotMain06";
        slotMain06.Size = new Size(60, 88);
        slotMain06.TabIndex = 6;
        // 
        // slotMain07
        // 
        slotMain07.BackColor = AppColors.CardBg;
        slotMain07.IsOnline = false;
        slotMain07.IsSelected = false;
        slotMain07.Location = new Point(448, 0);
        slotMain07.Name = "slotMain07";
        slotMain07.Size = new Size(60, 88);
        slotMain07.TabIndex = 7;
        // 
        // slotMain08
        // 
        slotMain08.BackColor = AppColors.CardBg;
        slotMain08.IsOnline = false;
        slotMain08.IsSelected = false;
        slotMain08.Location = new Point(512, 0);
        slotMain08.Name = "slotMain08";
        slotMain08.Size = new Size(60, 88);
        slotMain08.TabIndex = 8;
        // 
        // slotMain09
        // 
        slotMain09.BackColor = AppColors.CardBg;
        slotMain09.IsOnline = false;
        slotMain09.IsSelected = false;
        slotMain09.Location = new Point(576, 0);
        slotMain09.Name = "slotMain09";
        slotMain09.Size = new Size(60, 88);
        slotMain09.TabIndex = 9;
        // 
        // slotMain10
        // 
        slotMain10.BackColor = AppColors.CardBg;
        slotMain10.IsOnline = false;
        slotMain10.IsSelected = false;
        slotMain10.Location = new Point(0, 92);
        slotMain10.Name = "slotMain10";
        slotMain10.Size = new Size(60, 88);
        slotMain10.TabIndex = 10;
        // 
        // slotMain11
        // 
        slotMain11.BackColor = AppColors.CardBg;
        slotMain11.IsOnline = false;
        slotMain11.IsSelected = false;
        slotMain11.Location = new Point(64, 92);
        slotMain11.Name = "slotMain11";
        slotMain11.Size = new Size(60, 88);
        slotMain11.TabIndex = 11;
        // 
        // slotMain12
        // 
        slotMain12.BackColor = AppColors.CardBg;
        slotMain12.IsOnline = false;
        slotMain12.IsSelected = false;
        slotMain12.Location = new Point(128, 92);
        slotMain12.Name = "slotMain12";
        slotMain12.Size = new Size(60, 88);
        slotMain12.TabIndex = 12;
        // 
        // slotMain13
        // 
        slotMain13.BackColor = AppColors.CardBg;
        slotMain13.IsOnline = false;
        slotMain13.IsSelected = false;
        slotMain13.Location = new Point(192, 92);
        slotMain13.Name = "slotMain13";
        slotMain13.Size = new Size(60, 88);
        slotMain13.TabIndex = 13;
        // 
        // slotMain14
        // 
        slotMain14.BackColor = AppColors.CardBg;
        slotMain14.IsOnline = false;
        slotMain14.IsSelected = false;
        slotMain14.Location = new Point(256, 92);
        slotMain14.Name = "slotMain14";
        slotMain14.Size = new Size(60, 88);
        slotMain14.TabIndex = 14;
        // 
        // slotMain15
        // 
        slotMain15.BackColor = AppColors.CardBg;
        slotMain15.IsOnline = false;
        slotMain15.IsSelected = false;
        slotMain15.Location = new Point(320, 92);
        slotMain15.Name = "slotMain15";
        slotMain15.Size = new Size(60, 88);
        slotMain15.TabIndex = 15;
        // 
        // slotMain16
        // 
        slotMain16.BackColor = AppColors.CardBg;
        slotMain16.IsOnline = false;
        slotMain16.IsSelected = false;
        slotMain16.Location = new Point(384, 92);
        slotMain16.Name = "slotMain16";
        slotMain16.Size = new Size(60, 88);
        slotMain16.TabIndex = 16;
        // 
        // slotMain17
        // 
        slotMain17.BackColor = AppColors.CardBg;
        slotMain17.IsOnline = false;
        slotMain17.IsSelected = false;
        slotMain17.Location = new Point(448, 92);
        slotMain17.Name = "slotMain17";
        slotMain17.Size = new Size(60, 88);
        slotMain17.TabIndex = 17;
        // 
        // slotMain18
        // 
        slotMain18.BackColor = AppColors.CardBg;
        slotMain18.IsOnline = false;
        slotMain18.IsSelected = false;
        slotMain18.Location = new Point(512, 92);
        slotMain18.Name = "slotMain18";
        slotMain18.Size = new Size(60, 88);
        slotMain18.TabIndex = 18;
        // 
        // slotMain19
        // 
        slotMain19.BackColor = AppColors.CardBg;
        slotMain19.IsOnline = false;
        slotMain19.IsSelected = false;
        slotMain19.Location = new Point(576, 92);
        slotMain19.Name = "slotMain19";
        slotMain19.Size = new Size(60, 88);
        slotMain19.TabIndex = 19;
        // 
        // slotMain20
        // 
        slotMain20.BackColor = AppColors.CardBg;
        slotMain20.IsOnline = false;
        slotMain20.IsSelected = false;
        slotMain20.Location = new Point(0, 184);
        slotMain20.Name = "slotMain20";
        slotMain20.Size = new Size(60, 88);
        slotMain20.TabIndex = 20;
        // 
        // slotMain21
        // 
        slotMain21.BackColor = AppColors.CardBg;
        slotMain21.IsOnline = false;
        slotMain21.IsSelected = false;
        slotMain21.Location = new Point(64, 184);
        slotMain21.Name = "slotMain21";
        slotMain21.Size = new Size(60, 88);
        slotMain21.TabIndex = 21;
        // 
        // slotMain22
        // 
        slotMain22.BackColor = AppColors.CardBg;
        slotMain22.IsOnline = false;
        slotMain22.IsSelected = false;
        slotMain22.Location = new Point(128, 184);
        slotMain22.Name = "slotMain22";
        slotMain22.Size = new Size(60, 88);
        slotMain22.TabIndex = 22;
        // 
        // slotMain23
        // 
        slotMain23.BackColor = AppColors.CardBg;
        slotMain23.IsOnline = false;
        slotMain23.IsSelected = false;
        slotMain23.Location = new Point(192, 184);
        slotMain23.Name = "slotMain23";
        slotMain23.Size = new Size(60, 88);
        slotMain23.TabIndex = 23;
        // 
        // slotMain24
        // 
        slotMain24.BackColor = AppColors.CardBg;
        slotMain24.IsOnline = false;
        slotMain24.IsSelected = false;
        slotMain24.Location = new Point(256, 184);
        slotMain24.Name = "slotMain24";
        slotMain24.Size = new Size(60, 88);
        slotMain24.TabIndex = 24;
        // 
        // slotMain25
        // 
        slotMain25.BackColor = AppColors.CardBg;
        slotMain25.IsOnline = false;
        slotMain25.IsSelected = false;
        slotMain25.Location = new Point(320, 184);
        slotMain25.Name = "slotMain25";
        slotMain25.Size = new Size(60, 88);
        slotMain25.TabIndex = 25;
        // 
        // slotMain26
        // 
        slotMain26.BackColor = AppColors.CardBg;
        slotMain26.IsOnline = false;
        slotMain26.IsSelected = false;
        slotMain26.Location = new Point(384, 184);
        slotMain26.Name = "slotMain26";
        slotMain26.Size = new Size(60, 88);
        slotMain26.TabIndex = 26;
        // 
        // slotMain27
        // 
        slotMain27.BackColor = AppColors.CardBg;
        slotMain27.IsOnline = false;
        slotMain27.IsSelected = false;
        slotMain27.Location = new Point(448, 184);
        slotMain27.Name = "slotMain27";
        slotMain27.Size = new Size(60, 88);
        slotMain27.TabIndex = 27;
        // 
        // slotMain28
        // 
        slotMain28.BackColor = AppColors.CardBg;
        slotMain28.IsOnline = false;
        slotMain28.IsSelected = false;
        slotMain28.Location = new Point(512, 184);
        slotMain28.Name = "slotMain28";
        slotMain28.Size = new Size(60, 88);
        slotMain28.TabIndex = 28;
        // 
        // slotMain29
        // 
        slotMain29.BackColor = AppColors.CardBg;
        slotMain29.IsOnline = false;
        slotMain29.IsSelected = false;
        slotMain29.Location = new Point(576, 184);
        slotMain29.Name = "slotMain29";
        slotMain29.Size = new Size(60, 88);
        slotMain29.TabIndex = 29;
        // 
        // slotMain30
        // 
        slotMain30.BackColor = AppColors.CardBg;
        slotMain30.IsOnline = false;
        slotMain30.IsSelected = false;
        slotMain30.Location = new Point(0, 276);
        slotMain30.Name = "slotMain30";
        slotMain30.Size = new Size(60, 88);
        slotMain30.TabIndex = 30;
        // 
        // slotMain31
        // 
        slotMain31.BackColor = AppColors.CardBg;
        slotMain31.IsOnline = false;
        slotMain31.IsSelected = false;
        slotMain31.Location = new Point(64, 276);
        slotMain31.Name = "slotMain31";
        slotMain31.Size = new Size(60, 88);
        slotMain31.TabIndex = 31;
        // 
        // slotMain32
        // 
        slotMain32.BackColor = AppColors.CardBg;
        slotMain32.IsOnline = false;
        slotMain32.IsSelected = false;
        slotMain32.Location = new Point(128, 276);
        slotMain32.Name = "slotMain32";
        slotMain32.Size = new Size(60, 88);
        slotMain32.TabIndex = 32;
        // 
        // slotMain33
        // 
        slotMain33.BackColor = AppColors.CardBg;
        slotMain33.IsOnline = false;
        slotMain33.IsSelected = false;
        slotMain33.Location = new Point(192, 276);
        slotMain33.Name = "slotMain33";
        slotMain33.Size = new Size(60, 88);
        slotMain33.TabIndex = 33;
        // 
        // slotMain34
        // 
        slotMain34.BackColor = AppColors.CardBg;
        slotMain34.IsOnline = false;
        slotMain34.IsSelected = false;
        slotMain34.Location = new Point(256, 276);
        slotMain34.Name = "slotMain34";
        slotMain34.Size = new Size(60, 88);
        slotMain34.TabIndex = 34;
        // 
        // slotMain35
        // 
        slotMain35.BackColor = AppColors.CardBg;
        slotMain35.IsOnline = false;
        slotMain35.IsSelected = false;
        slotMain35.Location = new Point(320, 276);
        slotMain35.Name = "slotMain35";
        slotMain35.Size = new Size(60, 88);
        slotMain35.TabIndex = 35;
        // 
        // slotMain36
        // 
        slotMain36.BackColor = AppColors.CardBg;
        slotMain36.IsOnline = false;
        slotMain36.IsSelected = false;
        slotMain36.Location = new Point(384, 276);
        slotMain36.Name = "slotMain36";
        slotMain36.Size = new Size(60, 88);
        slotMain36.TabIndex = 36;
        // 
        // slotMain37
        // 
        slotMain37.BackColor = AppColors.CardBg;
        slotMain37.IsOnline = false;
        slotMain37.IsSelected = false;
        slotMain37.Location = new Point(448, 276);
        slotMain37.Name = "slotMain37";
        slotMain37.Size = new Size(60, 88);
        slotMain37.TabIndex = 37;
        // 
        // slotMain38
        // 
        slotMain38.BackColor = AppColors.CardBg;
        slotMain38.IsOnline = false;
        slotMain38.IsSelected = false;
        slotMain38.Location = new Point(512, 276);
        slotMain38.Name = "slotMain38";
        slotMain38.Size = new Size(60, 88);
        slotMain38.TabIndex = 38;
        // 
        // slotMain39
        // 
        slotMain39.BackColor = AppColors.CardBg;
        slotMain39.IsOnline = false;
        slotMain39.IsSelected = false;
        slotMain39.Location = new Point(576, 276);
        slotMain39.Name = "slotMain39";
        slotMain39.Size = new Size(60, 88);
        slotMain39.TabIndex = 39;
        // 
        // slotMain40
        // 
        slotMain40.BackColor = AppColors.CardBg;
        slotMain40.IsOnline = false;
        slotMain40.IsSelected = false;
        slotMain40.Location = new Point(0, 368);
        slotMain40.Name = "slotMain40";
        slotMain40.Size = new Size(60, 88);
        slotMain40.TabIndex = 40;
        // 
        // slotMain41
        // 
        slotMain41.BackColor = AppColors.CardBg;
        slotMain41.IsOnline = false;
        slotMain41.IsSelected = false;
        slotMain41.Location = new Point(64, 368);
        slotMain41.Name = "slotMain41";
        slotMain41.Size = new Size(60, 88);
        slotMain41.TabIndex = 41;
        // 
        // slotMain42
        // 
        slotMain42.BackColor = AppColors.CardBg;
        slotMain42.IsOnline = false;
        slotMain42.IsSelected = false;
        slotMain42.Location = new Point(128, 368);
        slotMain42.Name = "slotMain42";
        slotMain42.Size = new Size(60, 88);
        slotMain42.TabIndex = 42;
        // 
        // slotMain43
        // 
        slotMain43.BackColor = AppColors.CardBg;
        slotMain43.IsOnline = false;
        slotMain43.IsSelected = false;
        slotMain43.Location = new Point(192, 368);
        slotMain43.Name = "slotMain43";
        slotMain43.Size = new Size(60, 88);
        slotMain43.TabIndex = 43;
        // 
        // slotMain44
        // 
        slotMain44.BackColor = AppColors.CardBg;
        slotMain44.IsOnline = false;
        slotMain44.IsSelected = false;
        slotMain44.Location = new Point(256, 368);
        slotMain44.Name = "slotMain44";
        slotMain44.Size = new Size(60, 88);
        slotMain44.TabIndex = 44;
        // 
        // slotMain45
        // 
        slotMain45.BackColor = AppColors.CardBg;
        slotMain45.IsOnline = false;
        slotMain45.IsSelected = false;
        slotMain45.Location = new Point(320, 368);
        slotMain45.Name = "slotMain45";
        slotMain45.Size = new Size(60, 88);
        slotMain45.TabIndex = 45;
        // 
        // slotMain46
        // 
        slotMain46.BackColor = AppColors.CardBg;
        slotMain46.IsOnline = false;
        slotMain46.IsSelected = false;
        slotMain46.Location = new Point(384, 368);
        slotMain46.Name = "slotMain46";
        slotMain46.Size = new Size(60, 88);
        slotMain46.TabIndex = 46;
        // 
        // slotMain47
        // 
        slotMain47.BackColor = AppColors.CardBg;
        slotMain47.IsOnline = false;
        slotMain47.IsSelected = false;
        slotMain47.Location = new Point(448, 368);
        slotMain47.Name = "slotMain47";
        slotMain47.Size = new Size(60, 88);
        slotMain47.TabIndex = 47;
        // 
        // slotMain48
        // 
        slotMain48.BackColor = AppColors.CardBg;
        slotMain48.IsOnline = false;
        slotMain48.IsSelected = false;
        slotMain48.Location = new Point(512, 368);
        slotMain48.Name = "slotMain48";
        slotMain48.Size = new Size(60, 88);
        slotMain48.TabIndex = 48;
        // 
        // slotMain49
        // 
        slotMain49.BackColor = AppColors.CardBg;
        slotMain49.IsOnline = false;
        slotMain49.IsSelected = false;
        slotMain49.Location = new Point(576, 368);
        slotMain49.Name = "slotMain49";
        slotMain49.Size = new Size(60, 88);
        slotMain49.TabIndex = 49;
        // 
        // slotMain50
        // 
        slotMain50.BackColor = AppColors.CardBg;
        slotMain50.IsOnline = false;
        slotMain50.IsSelected = false;
        slotMain50.Location = new Point(0, 460);
        slotMain50.Name = "slotMain50";
        slotMain50.Size = new Size(60, 88);
        slotMain50.TabIndex = 50;
        // 
        // slotMain51
        // 
        slotMain51.BackColor = AppColors.CardBg;
        slotMain51.IsOnline = false;
        slotMain51.IsSelected = false;
        slotMain51.Location = new Point(64, 460);
        slotMain51.Name = "slotMain51";
        slotMain51.Size = new Size(60, 88);
        slotMain51.TabIndex = 51;
        // 
        // slotMain52
        // 
        slotMain52.BackColor = AppColors.CardBg;
        slotMain52.IsOnline = false;
        slotMain52.IsSelected = false;
        slotMain52.Location = new Point(128, 460);
        slotMain52.Name = "slotMain52";
        slotMain52.Size = new Size(60, 88);
        slotMain52.TabIndex = 52;
        // 
        // slotMain53
        // 
        slotMain53.BackColor = AppColors.CardBg;
        slotMain53.IsOnline = false;
        slotMain53.IsSelected = false;
        slotMain53.Location = new Point(192, 460);
        slotMain53.Name = "slotMain53";
        slotMain53.Size = new Size(60, 88);
        slotMain53.TabIndex = 53;
        // 
        // slotMain54
        // 
        slotMain54.BackColor = AppColors.CardBg;
        slotMain54.IsOnline = false;
        slotMain54.IsSelected = false;
        slotMain54.Location = new Point(256, 460);
        slotMain54.Name = "slotMain54";
        slotMain54.Size = new Size(60, 88);
        slotMain54.TabIndex = 54;
        // 
        // slotMain55
        // 
        slotMain55.BackColor = AppColors.CardBg;
        slotMain55.IsOnline = false;
        slotMain55.IsSelected = false;
        slotMain55.Location = new Point(320, 460);
        slotMain55.Name = "slotMain55";
        slotMain55.Size = new Size(60, 88);
        slotMain55.TabIndex = 55;
        // 
        // slotMain56
        // 
        slotMain56.BackColor = AppColors.CardBg;
        slotMain56.IsOnline = false;
        slotMain56.IsSelected = false;
        slotMain56.Location = new Point(384, 460);
        slotMain56.Name = "slotMain56";
        slotMain56.Size = new Size(60, 88);
        slotMain56.TabIndex = 56;
        // 
        // slotMain57
        // 
        slotMain57.BackColor = AppColors.CardBg;
        slotMain57.IsOnline = false;
        slotMain57.IsSelected = false;
        slotMain57.Location = new Point(448, 460);
        slotMain57.Name = "slotMain57";
        slotMain57.Size = new Size(60, 88);
        slotMain57.TabIndex = 57;
        // 
        // slotMain58
        // 
        slotMain58.BackColor = AppColors.CardBg;
        slotMain58.IsOnline = false;
        slotMain58.IsSelected = false;
        slotMain58.Location = new Point(512, 460);
        slotMain58.Name = "slotMain58";
        slotMain58.Size = new Size(60, 88);
        slotMain58.TabIndex = 58;
        // 
        // slotMain59
        // 
        slotMain59.BackColor = AppColors.CardBg;
        slotMain59.IsOnline = false;
        slotMain59.IsSelected = false;
        slotMain59.Location = new Point(576, 460);
        slotMain59.Name = "slotMain59";
        slotMain59.Size = new Size(60, 88);
        slotMain59.TabIndex = 59;
        // 
        // lblExtraCount
        // 
        lblExtraCount.BackColor = Color.Transparent;
        lblExtraCount.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblExtraCount.ForeColor = AppColors.ExtraBorder;
        lblExtraCount.Location = new Point(8, 592);
        lblExtraCount.Name = "lblExtraCount";
        lblExtraCount.Size = new Size(740, 20);
        lblExtraCount.TabIndex = 2;
        lblExtraCount.Text = "EXTRA DECK (0/15)";
        // 
        // pnlExtraGrid
        // 
        pnlExtraGrid.BackColor = Color.Transparent;
        pnlExtraGrid.Controls.Add(slotExtra00);
        pnlExtraGrid.Controls.Add(slotExtra01);
        pnlExtraGrid.Controls.Add(slotExtra02);
        pnlExtraGrid.Controls.Add(slotExtra03);
        pnlExtraGrid.Controls.Add(slotExtra04);
        pnlExtraGrid.Controls.Add(slotExtra05);
        pnlExtraGrid.Controls.Add(slotExtra06);
        pnlExtraGrid.Controls.Add(slotExtra07);
        pnlExtraGrid.Controls.Add(slotExtra08);
        pnlExtraGrid.Controls.Add(slotExtra09);
        pnlExtraGrid.Controls.Add(slotExtra10);
        pnlExtraGrid.Controls.Add(slotExtra11);
        pnlExtraGrid.Controls.Add(slotExtra12);
        pnlExtraGrid.Controls.Add(slotExtra13);
        pnlExtraGrid.Controls.Add(slotExtra14);
        pnlExtraGrid.Location = new Point(8, 616);
        pnlExtraGrid.Name = "pnlExtraGrid";
        pnlExtraGrid.Size = new Size(740, 180);
        pnlExtraGrid.TabIndex = 3;
        // 
        // slotExtra00
        // 
        slotExtra00.BackColor = AppColors.CardBg;
        slotExtra00.IsOnline = false;
        slotExtra00.IsSelected = false;
        slotExtra00.Role = CardSlot.SlotRole.Extra;
        slotExtra00.Location = new Point(0, 0);
        slotExtra00.Name = "slotExtra00";
        slotExtra00.Size = new Size(60, 88);
        slotExtra00.TabIndex = 0;
        // 
        // slotExtra01
        // 
        slotExtra01.BackColor = AppColors.CardBg;
        slotExtra01.IsOnline = false;
        slotExtra01.IsSelected = false;
        slotExtra01.Role = CardSlot.SlotRole.Extra;
        slotExtra01.Location = new Point(64, 0);
        slotExtra01.Name = "slotExtra01";
        slotExtra01.Size = new Size(60, 88);
        slotExtra01.TabIndex = 1;
        // 
        // slotExtra02
        // 
        slotExtra02.BackColor = AppColors.CardBg;
        slotExtra02.IsOnline = false;
        slotExtra02.IsSelected = false;
        slotExtra02.Role = CardSlot.SlotRole.Extra;
        slotExtra02.Location = new Point(128, 0);
        slotExtra02.Name = "slotExtra02";
        slotExtra02.Size = new Size(60, 88);
        slotExtra02.TabIndex = 2;
        // 
        // slotExtra03
        // 
        slotExtra03.BackColor = AppColors.CardBg;
        slotExtra03.IsOnline = false;
        slotExtra03.IsSelected = false;
        slotExtra03.Role = CardSlot.SlotRole.Extra;
        slotExtra03.Location = new Point(192, 0);
        slotExtra03.Name = "slotExtra03";
        slotExtra03.Size = new Size(60, 88);
        slotExtra03.TabIndex = 3;
        // 
        // slotExtra04
        // 
        slotExtra04.BackColor = AppColors.CardBg;
        slotExtra04.IsOnline = false;
        slotExtra04.IsSelected = false;
        slotExtra04.Role = CardSlot.SlotRole.Extra;
        slotExtra04.Location = new Point(256, 0);
        slotExtra04.Name = "slotExtra04";
        slotExtra04.Size = new Size(60, 88);
        slotExtra04.TabIndex = 4;
        // 
        // slotExtra05
        // 
        slotExtra05.BackColor = AppColors.CardBg;
        slotExtra05.IsOnline = false;
        slotExtra05.IsSelected = false;
        slotExtra05.Role = CardSlot.SlotRole.Extra;
        slotExtra05.Location = new Point(320, 0);
        slotExtra05.Name = "slotExtra05";
        slotExtra05.Size = new Size(60, 88);
        slotExtra05.TabIndex = 5;
        // 
        // slotExtra06
        // 
        slotExtra06.BackColor = AppColors.CardBg;
        slotExtra06.IsOnline = false;
        slotExtra06.IsSelected = false;
        slotExtra06.Role = CardSlot.SlotRole.Extra;
        slotExtra06.Location = new Point(384, 0);
        slotExtra06.Name = "slotExtra06";
        slotExtra06.Size = new Size(60, 88);
        slotExtra06.TabIndex = 6;
        // 
        // slotExtra07
        // 
        slotExtra07.BackColor = AppColors.CardBg;
        slotExtra07.IsOnline = false;
        slotExtra07.IsSelected = false;
        slotExtra07.Role = CardSlot.SlotRole.Extra;
        slotExtra07.Location = new Point(448, 0);
        slotExtra07.Name = "slotExtra07";
        slotExtra07.Size = new Size(60, 88);
        slotExtra07.TabIndex = 7;
        // 
        // slotExtra08
        // 
        slotExtra08.BackColor = AppColors.CardBg;
        slotExtra08.IsOnline = false;
        slotExtra08.IsSelected = false;
        slotExtra08.Role = CardSlot.SlotRole.Extra;
        slotExtra08.Location = new Point(512, 0);
        slotExtra08.Name = "slotExtra08";
        slotExtra08.Size = new Size(60, 88);
        slotExtra08.TabIndex = 8;
        // 
        // slotExtra09
        // 
        slotExtra09.BackColor = AppColors.CardBg;
        slotExtra09.IsOnline = false;
        slotExtra09.IsSelected = false;
        slotExtra09.Role = CardSlot.SlotRole.Extra;
        slotExtra09.Location = new Point(576, 0);
        slotExtra09.Name = "slotExtra09";
        slotExtra09.Size = new Size(60, 88);
        slotExtra09.TabIndex = 9;
        // 
        // slotExtra10
        // 
        slotExtra10.BackColor = AppColors.CardBg;
        slotExtra10.IsOnline = false;
        slotExtra10.IsSelected = false;
        slotExtra10.Role = CardSlot.SlotRole.Extra;
        slotExtra10.Location = new Point(0, 92);
        slotExtra10.Name = "slotExtra10";
        slotExtra10.Size = new Size(60, 88);
        slotExtra10.TabIndex = 10;
        // 
        // slotExtra11
        // 
        slotExtra11.BackColor = AppColors.CardBg;
        slotExtra11.IsOnline = false;
        slotExtra11.IsSelected = false;
        slotExtra11.Role = CardSlot.SlotRole.Extra;
        slotExtra11.Location = new Point(64, 92);
        slotExtra11.Name = "slotExtra11";
        slotExtra11.Size = new Size(60, 88);
        slotExtra11.TabIndex = 11;
        // 
        // slotExtra12
        // 
        slotExtra12.BackColor = AppColors.CardBg;
        slotExtra12.IsOnline = false;
        slotExtra12.IsSelected = false;
        slotExtra12.Role = CardSlot.SlotRole.Extra;
        slotExtra12.Location = new Point(128, 92);
        slotExtra12.Name = "slotExtra12";
        slotExtra12.Size = new Size(60, 88);
        slotExtra12.TabIndex = 12;
        // 
        // slotExtra13
        // 
        slotExtra13.BackColor = AppColors.CardBg;
        slotExtra13.IsOnline = false;
        slotExtra13.IsSelected = false;
        slotExtra13.Role = CardSlot.SlotRole.Extra;
        slotExtra13.Location = new Point(192, 92);
        slotExtra13.Name = "slotExtra13";
        slotExtra13.Size = new Size(60, 88);
        slotExtra13.TabIndex = 13;
        // 
        // slotExtra14
        // 
        slotExtra14.BackColor = AppColors.CardBg;
        slotExtra14.IsOnline = false;
        slotExtra14.IsSelected = false;
        slotExtra14.Role = CardSlot.SlotRole.Extra;
        slotExtra14.Location = new Point(256, 92);
        slotExtra14.Name = "slotExtra14";
        slotExtra14.Size = new Size(60, 88);
        slotExtra14.TabIndex = 14;
        // 
        // lblSideCount
        // 
        lblSideCount.BackColor = Color.Transparent;
        lblSideCount.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSideCount.ForeColor = AppColors.SideBorder;
        lblSideCount.Location = new Point(8, 808);
        lblSideCount.Name = "lblSideCount";
        lblSideCount.Size = new Size(740, 20);
        lblSideCount.TabIndex = 4;
        lblSideCount.Text = "SIDE DECK  (0/15)";
        // 
        // pnlSideGrid
        // 
        pnlSideGrid.BackColor = Color.Transparent;
        pnlSideGrid.Controls.Add(slotSide00);
        pnlSideGrid.Controls.Add(slotSide01);
        pnlSideGrid.Controls.Add(slotSide02);
        pnlSideGrid.Controls.Add(slotSide03);
        pnlSideGrid.Controls.Add(slotSide04);
        pnlSideGrid.Controls.Add(slotSide05);
        pnlSideGrid.Controls.Add(slotSide06);
        pnlSideGrid.Controls.Add(slotSide07);
        pnlSideGrid.Controls.Add(slotSide08);
        pnlSideGrid.Controls.Add(slotSide09);
        pnlSideGrid.Controls.Add(slotSide10);
        pnlSideGrid.Controls.Add(slotSide11);
        pnlSideGrid.Controls.Add(slotSide12);
        pnlSideGrid.Controls.Add(slotSide13);
        pnlSideGrid.Controls.Add(slotSide14);
        pnlSideGrid.Location = new Point(8, 832);
        pnlSideGrid.Name = "pnlSideGrid";
        pnlSideGrid.Size = new Size(740, 180);
        pnlSideGrid.TabIndex = 5;
        // 
        // slotSide00
        // 
        slotSide00.BackColor = AppColors.CardBg;
        slotSide00.IsOnline = false;
        slotSide00.IsSelected = false;
        slotSide00.Role = CardSlot.SlotRole.Side;
        slotSide00.Location = new Point(0, 0);
        slotSide00.Name = "slotSide00";
        slotSide00.Size = new Size(60, 88);
        slotSide00.TabIndex = 0;
        // 
        // slotSide01
        // 
        slotSide01.BackColor = AppColors.CardBg;
        slotSide01.IsOnline = false;
        slotSide01.IsSelected = false;
        slotSide01.Role = CardSlot.SlotRole.Side;
        slotSide01.Location = new Point(64, 0);
        slotSide01.Name = "slotSide01";
        slotSide01.Size = new Size(60, 88);
        slotSide01.TabIndex = 1;
        // 
        // slotSide02
        // 
        slotSide02.BackColor = AppColors.CardBg;
        slotSide02.IsOnline = false;
        slotSide02.IsSelected = false;
        slotSide02.Role = CardSlot.SlotRole.Side;
        slotSide02.Location = new Point(128, 0);
        slotSide02.Name = "slotSide02";
        slotSide02.Size = new Size(60, 88);
        slotSide02.TabIndex = 2;
        // 
        // slotSide03
        // 
        slotSide03.BackColor = AppColors.CardBg;
        slotSide03.IsOnline = false;
        slotSide03.IsSelected = false;
        slotSide03.Role = CardSlot.SlotRole.Side;
        slotSide03.Location = new Point(192, 0);
        slotSide03.Name = "slotSide03";
        slotSide03.Size = new Size(60, 88);
        slotSide03.TabIndex = 3;
        // 
        // slotSide04
        // 
        slotSide04.BackColor = AppColors.CardBg;
        slotSide04.IsOnline = false;
        slotSide04.IsSelected = false;
        slotSide04.Role = CardSlot.SlotRole.Side;
        slotSide04.Location = new Point(256, 0);
        slotSide04.Name = "slotSide04";
        slotSide04.Size = new Size(60, 88);
        slotSide04.TabIndex = 4;
        // 
        // slotSide05
        // 
        slotSide05.BackColor = AppColors.CardBg;
        slotSide05.IsOnline = false;
        slotSide05.IsSelected = false;
        slotSide05.Role = CardSlot.SlotRole.Side;
        slotSide05.Location = new Point(320, 0);
        slotSide05.Name = "slotSide05";
        slotSide05.Size = new Size(60, 88);
        slotSide05.TabIndex = 5;
        // 
        // slotSide06
        // 
        slotSide06.BackColor = AppColors.CardBg;
        slotSide06.IsOnline = false;
        slotSide06.IsSelected = false;
        slotSide06.Role = CardSlot.SlotRole.Side;
        slotSide06.Location = new Point(384, 0);
        slotSide06.Name = "slotSide06";
        slotSide06.Size = new Size(60, 88);
        slotSide06.TabIndex = 6;
        // 
        // slotSide07
        // 
        slotSide07.BackColor = AppColors.CardBg;
        slotSide07.IsOnline = false;
        slotSide07.IsSelected = false;
        slotSide07.Role = CardSlot.SlotRole.Side;
        slotSide07.Location = new Point(448, 0);
        slotSide07.Name = "slotSide07";
        slotSide07.Size = new Size(60, 88);
        slotSide07.TabIndex = 7;
        // 
        // slotSide08
        // 
        slotSide08.BackColor = AppColors.CardBg;
        slotSide08.IsOnline = false;
        slotSide08.IsSelected = false;
        slotSide08.Role = CardSlot.SlotRole.Side;
        slotSide08.Location = new Point(512, 0);
        slotSide08.Name = "slotSide08";
        slotSide08.Size = new Size(60, 88);
        slotSide08.TabIndex = 8;
        // 
        // slotSide09
        // 
        slotSide09.BackColor = AppColors.CardBg;
        slotSide09.IsOnline = false;
        slotSide09.IsSelected = false;
        slotSide09.Role = CardSlot.SlotRole.Side;
        slotSide09.Location = new Point(576, 0);
        slotSide09.Name = "slotSide09";
        slotSide09.Size = new Size(60, 88);
        slotSide09.TabIndex = 9;
        // 
        // slotSide10
        // 
        slotSide10.BackColor = AppColors.CardBg;
        slotSide10.IsOnline = false;
        slotSide10.IsSelected = false;
        slotSide10.Role = CardSlot.SlotRole.Side;
        slotSide10.Location = new Point(0, 92);
        slotSide10.Name = "slotSide10";
        slotSide10.Size = new Size(60, 88);
        slotSide10.TabIndex = 10;
        // 
        // slotSide11
        // 
        slotSide11.BackColor = AppColors.CardBg;
        slotSide11.IsOnline = false;
        slotSide11.IsSelected = false;
        slotSide11.Role = CardSlot.SlotRole.Side;
        slotSide11.Location = new Point(64, 92);
        slotSide11.Name = "slotSide11";
        slotSide11.Size = new Size(60, 88);
        slotSide11.TabIndex = 11;
        // 
        // slotSide12
        // 
        slotSide12.BackColor = AppColors.CardBg;
        slotSide12.IsOnline = false;
        slotSide12.IsSelected = false;
        slotSide12.Role = CardSlot.SlotRole.Side;
        slotSide12.Location = new Point(128, 92);
        slotSide12.Name = "slotSide12";
        slotSide12.Size = new Size(60, 88);
        slotSide12.TabIndex = 12;
        // 
        // slotSide13
        // 
        slotSide13.BackColor = AppColors.CardBg;
        slotSide13.IsOnline = false;
        slotSide13.IsSelected = false;
        slotSide13.Role = CardSlot.SlotRole.Side;
        slotSide13.Location = new Point(192, 92);
        slotSide13.Name = "slotSide13";
        slotSide13.Size = new Size(60, 88);
        slotSide13.TabIndex = 13;
        // 
        // slotSide14
        // 
        slotSide14.BackColor = AppColors.CardBg;
        slotSide14.IsOnline = false;
        slotSide14.IsSelected = false;
        slotSide14.Role = CardSlot.SlotRole.Side;
        slotSide14.Location = new Point(256, 92);
        slotSide14.Name = "slotSide14";
        slotSide14.Size = new Size(60, 88);
        slotSide14.TabIndex = 14;
        // 
        // pnlPreview
        // 
        pnlPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        pnlPreview.AutoScroll = true;
        pnlPreview.BackColor = AppColors.CardBg;
        pnlPreview.Controls.Add(picPreview);
        pnlPreview.Controls.Add(pnlPreviewText);
        pnlPreview.Location = new Point(981, 45);
        pnlPreview.Name = "pnlPreview";
        pnlPreview.Size = new Size(219, 755);
        pnlPreview.TabIndex = 6;
        // 
        // picPreview
        // 
        picPreview.BackColor = AppColors.AppBg;
        picPreview.Cursor = Cursors.Hand;
        picPreview.Location = new Point(10, 8);
        picPreview.Name = "picPreview";
        picPreview.Size = new Size(199, 160);
        picPreview.SizeMode = PictureBoxSizeMode.Zoom;
        picPreview.TabIndex = 0;
        picPreview.TabStop = false;
        picPreview.Visible = false;
        picPreview.DoubleClick += picPreview_DoubleClick;
        // 
        // pnlPreviewText
        // 
        pnlPreviewText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlPreviewText.BackColor = Color.Transparent;
        pnlPreviewText.Controls.Add(lblPreviewName);
        pnlPreviewText.Controls.Add(lblPreviewType);
        pnlPreviewText.Controls.Add(lblPreviewAttr);
        pnlPreviewText.Controls.Add(lblPreviewRace);
        pnlPreviewText.Controls.Add(lblPreviewAtk);
        pnlPreviewText.Controls.Add(lblPreviewDef);
        pnlPreviewText.Controls.Add(lblPreviewDesc);
        pnlPreviewText.Location = new Point(0, 172);
        pnlPreviewText.Name = "pnlPreviewText";
        pnlPreviewText.Size = new Size(219, 560);
        pnlPreviewText.TabIndex = 1;
        pnlPreviewText.Visible = false;
        // 
        // lblPreviewName
        // 
        lblPreviewName.AutoEllipsis = true;
        lblPreviewName.BackColor = Color.Transparent;
        lblPreviewName.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblPreviewName.ForeColor = AppColors.TextCardName;
        lblPreviewName.Location = new Point(8, 4);
        lblPreviewName.Name = "lblPreviewName";
        lblPreviewName.Size = new Size(203, 36);
        lblPreviewName.TabIndex = 0;
        // 
        // lblPreviewType
        // 
        lblPreviewType.BackColor = Color.Transparent;
        lblPreviewType.Font = new Font("Segoe UI", 7.5F);
        lblPreviewType.ForeColor = AppColors.TextInfo;
        lblPreviewType.Location = new Point(8, 44);
        lblPreviewType.Name = "lblPreviewType";
        lblPreviewType.Size = new Size(203, 16);
        lblPreviewType.TabIndex = 1;
        // 
        // lblPreviewAttr
        // 
        lblPreviewAttr.BackColor = Color.Transparent;
        lblPreviewAttr.Font = new Font("Segoe UI", 7.5F);
        lblPreviewAttr.ForeColor = AppColors.TextInfo;
        lblPreviewAttr.Location = new Point(8, 62);
        lblPreviewAttr.Name = "lblPreviewAttr";
        lblPreviewAttr.Size = new Size(203, 16);
        lblPreviewAttr.TabIndex = 2;
        // 
        // lblPreviewRace
        // 
        lblPreviewRace.BackColor = Color.Transparent;
        lblPreviewRace.Font = new Font("Segoe UI", 7.5F);
        lblPreviewRace.ForeColor = AppColors.TextInfo;
        lblPreviewRace.Location = new Point(8, 80);
        lblPreviewRace.Name = "lblPreviewRace";
        lblPreviewRace.Size = new Size(203, 16);
        lblPreviewRace.TabIndex = 3;
        // 
        // lblPreviewAtk
        // 
        lblPreviewAtk.BackColor = Color.Transparent;
        lblPreviewAtk.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
        lblPreviewAtk.ForeColor = AppColors.StatAtk;
        lblPreviewAtk.Location = new Point(8, 98);
        lblPreviewAtk.Name = "lblPreviewAtk";
        lblPreviewAtk.Size = new Size(96, 16);
        lblPreviewAtk.TabIndex = 4;
        lblPreviewAtk.Visible = false;
        // 
        // lblPreviewDef
        // 
        lblPreviewDef.BackColor = Color.Transparent;
        lblPreviewDef.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
        lblPreviewDef.ForeColor = AppColors.StatDef;
        lblPreviewDef.Location = new Point(112, 98);
        lblPreviewDef.Name = "lblPreviewDef";
        lblPreviewDef.Size = new Size(96, 16);
        lblPreviewDef.TabIndex = 5;
        lblPreviewDef.Visible = false;
        // 
        // lblPreviewDesc
        // 
        lblPreviewDesc.BackColor = Color.Transparent;
        lblPreviewDesc.Font = new Font("Segoe UI", 7F);
        lblPreviewDesc.ForeColor = AppColors.TextDescription;
        lblPreviewDesc.Location = new Point(8, 124);
        lblPreviewDesc.Name = "lblPreviewDesc";
        lblPreviewDesc.Size = new Size(203, 420);
        lblPreviewDesc.TabIndex = 6;
        // 
        // pnlPreviewDiv
        // 
        pnlPreviewDiv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        pnlPreviewDiv.BackColor = AppColors.Border;
        pnlPreviewDiv.Location = new Point(980, 45);
        pnlPreviewDiv.Name = "pnlPreviewDiv";
        pnlPreviewDiv.Size = new Size(1, 755);
        pnlPreviewDiv.TabIndex = 5;
        //
        // imgOverlay
        //
        imgOverlay.Dock = DockStyle.Fill;
        imgOverlay.Location = new Point(0, 0);
        imgOverlay.Name = "imgOverlay";
        imgOverlay.Size = new Size(1200, 800);
        imgOverlay.TabIndex = 7;
        imgOverlay.Visible = false;
        //
        // DeckEditorPage
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppColors.AppBg;
        Controls.Add(pnlToolbar);
        Controls.Add(pnlToolbarDiv);
        Controls.Add(pnlSearch);
        Controls.Add(pnlSearchDiv);
        Controls.Add(pnlDeck);
        Controls.Add(pnlPreviewDiv);
        Controls.Add(pnlPreview);
        Controls.Add(imgOverlay);
        Name = "DeckEditorPage";
        Size = new Size(1200, 800);
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        pnlSearch.ResumeLayout(false);
        pnlDeck.ResumeLayout(false);
        pnlDeckScroll.ResumeLayout(false);
        pnlMainGrid.ResumeLayout(false);
        pnlExtraGrid.ResumeLayout(false);
        pnlSideGrid.ResumeLayout(false);
        pnlPreview.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
        pnlPreviewText.ResumeLayout(false);
        ResumeLayout(false);
    }
}