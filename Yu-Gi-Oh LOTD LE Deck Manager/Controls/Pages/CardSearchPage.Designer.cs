using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

partial class CardSearchPage
{
    // ── Toolbar ────────────────────────────────────────────────────────────────
    private Panel pnlToolbar;
    private TextBox txtSearch;
    private ReaLTaiizor.Controls.Button btnFilter;
    private CheckBox chkSearchDesc;
    private Label lblResultCount;
    private Panel pnlToolbarDiv;
    // ── Left results list ─────────────────────────────────────────────────────
    private Panel pnlResults;
    private ListBox lstResults;
    private Panel pnlResultsDiv;
    // ── Right preview panel ───────────────────────────────────────────────────
    private Panel pnlPreview;
    private PictureBox picPreview;
    private Label lblHint;
    private Panel pnlPreviewText;
    private Label lblPreviewName;
    private Label lblPreviewType;
    private Label lblPreviewAttr;
    private Label lblPreviewRace;
    private Label lblPreviewArchetype;
    private Label lblPreviewAtk;
    private Label lblPreviewDef;
    private Label lblPreviewDesc;
    // ── In-window zoom overlay ─────────────────────────────────────────────────
    private CardImageOverlay imgOverlay;

    private void InitializeComponent()
    {
        pnlToolbar = new Panel();
        txtSearch = new TextBox();
        btnFilter = new ReaLTaiizor.Controls.Button();
        chkSearchDesc = new CheckBox();
        lblResultCount = new Label();
        pnlToolbarDiv = new Panel();
        pnlResults = new Panel();
        lstResults = new ListBox();
        pnlResultsDiv = new Panel();
        pnlPreview = new Panel();
        picPreview = new PictureBox();
        lblHint = new Label();
        pnlPreviewText = new Panel();
        lblPreviewName = new Label();
        lblPreviewType = new Label();
        lblPreviewAttr = new Label();
        lblPreviewRace = new Label();
        lblPreviewArchetype = new Label();
        lblPreviewAtk = new Label();
        lblPreviewDef = new Label();
        lblPreviewDesc = new Label();
        imgOverlay = new CardImageOverlay();

        pnlToolbar.SuspendLayout();
        pnlResults.SuspendLayout();
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
        txtSearch.Size = new Size(220, 23);
        txtSearch.TabIndex = 0;
        txtSearch.TextChanged += txtSearch_TextChanged;
        //
        // btnFilter
        //
        btnFilter.BackColor = Color.Transparent;
        btnFilter.BorderColor = AppColors.ButtonNeutralBg;
        btnFilter.Cursor = Cursors.Hand;
        btnFilter.EnteredBorderColor = AppColors.AccentRedHover;
        btnFilter.EnteredColor = AppColors.ButtonNeutralHover;
        btnFilter.Font = new Font("Segoe UI", 8F);
        btnFilter.Image = null;
        btnFilter.ImageAlign = ContentAlignment.MiddleLeft;
        btnFilter.InactiveColor = AppColors.ButtonNeutralBg;
        btnFilter.Location = new Point(236, 10);
        btnFilter.Name = "btnFilter";
        btnFilter.PressedBorderColor = AppColors.AccentRedHover;
        btnFilter.PressedColor = AppColors.AccentRedHover;
        btnFilter.Size = new Size(60, 24);
        btnFilter.TabIndex = 1;
        btnFilter.TabStop = false;
        btnFilter.Text = "Filter";
        btnFilter.TextAlignment = StringAlignment.Center;
        btnFilter.Click += btnFilter_Click;
        //
        // chkSearchDesc
        //
        chkSearchDesc.BackColor = Color.Transparent;
        chkSearchDesc.Cursor = Cursors.Hand;
        chkSearchDesc.Font = new Font("Segoe UI", 8F);
        chkSearchDesc.ForeColor = AppColors.TextSecondary;
        chkSearchDesc.Location = new Point(304, 12);
        chkSearchDesc.Name = "chkSearchDesc";
        chkSearchDesc.Size = new Size(150, 20);
        chkSearchDesc.TabIndex = 2;
        chkSearchDesc.Text = "Search description too";
        chkSearchDesc.UseVisualStyleBackColor = false;
        chkSearchDesc.CheckedChanged += chkSearchDesc_CheckedChanged;
        //
        // lblResultCount
        //
        lblResultCount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblResultCount.BackColor = Color.Transparent;
        lblResultCount.Font = new Font("Segoe UI", 8F);
        lblResultCount.ForeColor = AppColors.TextInfo;
        lblResultCount.Location = new Point(835, 14);
        lblResultCount.Name = "lblResultCount";
        lblResultCount.Size = new Size(210, 16);
        lblResultCount.TabIndex = 3;
        lblResultCount.Text = "Type to search";
        lblResultCount.TextAlign = ContentAlignment.MiddleRight;
        //
        // pnlToolbar
        //
        pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlToolbar.BackColor = AppColors.TopbarBg;
        pnlToolbar.Controls.Add(txtSearch);
        pnlToolbar.Controls.Add(btnFilter);
        pnlToolbar.Controls.Add(chkSearchDesc);
        pnlToolbar.Controls.Add(lblResultCount);
        pnlToolbar.Location = new Point(0, 0);
        pnlToolbar.Name = "pnlToolbar";
        pnlToolbar.Size = new Size(1055, 44);
        pnlToolbar.TabIndex = 0;
        //
        // pnlToolbarDiv
        //
        pnlToolbarDiv.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlToolbarDiv.BackColor = AppColors.Border;
        pnlToolbarDiv.Location = new Point(0, 44);
        pnlToolbarDiv.Name = "pnlToolbarDiv";
        pnlToolbarDiv.Size = new Size(1055, 1);
        pnlToolbarDiv.TabIndex = 1;
        //
        // pnlResults
        //
        pnlResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlResults.BackColor = AppColors.CardBg;
        pnlResults.Controls.Add(lstResults);
        pnlResults.Location = new Point(0, 45);
        pnlResults.Name = "pnlResults";
        pnlResults.Size = new Size(280, 554);
        pnlResults.TabIndex = 2;
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
        lstResults.Size = new Size(280, 554);
        lstResults.TabIndex = 0;
        lstResults.DrawItem += lstResults_DrawItem;
        lstResults.SelectedIndexChanged += lstResults_SelectedIndexChanged;
        //
        // pnlResultsDiv
        //
        pnlResultsDiv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlResultsDiv.BackColor = AppColors.Border;
        pnlResultsDiv.Location = new Point(280, 45);
        pnlResultsDiv.Name = "pnlResultsDiv";
        pnlResultsDiv.Size = new Size(1, 554);
        pnlResultsDiv.TabIndex = 3;
        //
        // picPreview
        //
        picPreview.BackColor = AppColors.CardBg;
        picPreview.Cursor = Cursors.Hand;
        picPreview.Location = new Point(24, 20);
        picPreview.Name = "picPreview";
        picPreview.Size = new Size(240, 350);
        picPreview.SizeMode = PictureBoxSizeMode.Zoom;
        picPreview.TabIndex = 0;
        picPreview.TabStop = false;
        picPreview.Visible = false;
        picPreview.DoubleClick += picPreview_DoubleClick;
        //
        // lblHint
        //
        lblHint.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblHint.BackColor = Color.Transparent;
        lblHint.Font = new Font("Segoe UI", 10F);
        lblHint.ForeColor = AppColors.TextMuted;
        lblHint.Location = new Point(0, 0);
        lblHint.Name = "lblHint";
        lblHint.Size = new Size(774, 554);
        lblHint.TabIndex = 2;
        lblHint.Text = "Search for a card to see its details here.";
        lblHint.TextAlign = ContentAlignment.MiddleCenter;
        //
        // pnlPreviewText
        //
        pnlPreviewText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        pnlPreviewText.BackColor = Color.Transparent;
        pnlPreviewText.Controls.Add(lblPreviewName);
        pnlPreviewText.Controls.Add(lblPreviewType);
        pnlPreviewText.Controls.Add(lblPreviewAttr);
        pnlPreviewText.Controls.Add(lblPreviewRace);
        pnlPreviewText.Controls.Add(lblPreviewArchetype);
        pnlPreviewText.Controls.Add(lblPreviewAtk);
        pnlPreviewText.Controls.Add(lblPreviewDef);
        pnlPreviewText.Controls.Add(lblPreviewDesc);
        pnlPreviewText.Location = new Point(288, 20);
        pnlPreviewText.Name = "pnlPreviewText";
        pnlPreviewText.Size = new Size(462, 514);
        pnlPreviewText.TabIndex = 1;
        pnlPreviewText.Visible = false;
        //
        // lblPreviewName
        //
        lblPreviewName.AutoEllipsis = true;
        lblPreviewName.BackColor = Color.Transparent;
        lblPreviewName.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblPreviewName.ForeColor = AppColors.TextCardName;
        lblPreviewName.Location = new Point(0, 0);
        lblPreviewName.Name = "lblPreviewName";
        lblPreviewName.Size = new Size(462, 44);
        lblPreviewName.TabIndex = 0;
        //
        // lblPreviewType
        //
        lblPreviewType.BackColor = Color.Transparent;
        lblPreviewType.Font = new Font("Segoe UI", 9F);
        lblPreviewType.ForeColor = AppColors.TextInfo;
        lblPreviewType.Location = new Point(0, 48);
        lblPreviewType.Name = "lblPreviewType";
        lblPreviewType.Size = new Size(462, 20);
        lblPreviewType.TabIndex = 1;
        //
        // lblPreviewAttr
        //
        lblPreviewAttr.BackColor = Color.Transparent;
        lblPreviewAttr.Font = new Font("Segoe UI", 9F);
        lblPreviewAttr.ForeColor = AppColors.TextInfo;
        lblPreviewAttr.Location = new Point(0, 70);
        lblPreviewAttr.Name = "lblPreviewAttr";
        lblPreviewAttr.Size = new Size(462, 20);
        lblPreviewAttr.TabIndex = 2;
        //
        // lblPreviewRace
        //
        lblPreviewRace.BackColor = Color.Transparent;
        lblPreviewRace.Font = new Font("Segoe UI", 9F);
        lblPreviewRace.ForeColor = AppColors.TextInfo;
        lblPreviewRace.Location = new Point(0, 92);
        lblPreviewRace.Name = "lblPreviewRace";
        lblPreviewRace.Size = new Size(462, 20);
        lblPreviewRace.TabIndex = 3;
        //
        // lblPreviewArchetype
        //
        lblPreviewArchetype.BackColor = Color.Transparent;
        lblPreviewArchetype.Font = new Font("Segoe UI", 9F);
        lblPreviewArchetype.ForeColor = AppColors.TextInfo;
        lblPreviewArchetype.Location = new Point(0, 114);
        lblPreviewArchetype.Name = "lblPreviewArchetype";
        lblPreviewArchetype.Size = new Size(462, 20);
        lblPreviewArchetype.TabIndex = 4;
        //
        // lblPreviewAtk
        //
        lblPreviewAtk.BackColor = Color.Transparent;
        lblPreviewAtk.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPreviewAtk.ForeColor = AppColors.StatAtk;
        lblPreviewAtk.Location = new Point(0, 142);
        lblPreviewAtk.Name = "lblPreviewAtk";
        lblPreviewAtk.Size = new Size(220, 22);
        lblPreviewAtk.TabIndex = 5;
        lblPreviewAtk.Visible = false;
        //
        // lblPreviewDef
        //
        lblPreviewDef.BackColor = Color.Transparent;
        lblPreviewDef.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPreviewDef.ForeColor = AppColors.StatDef;
        lblPreviewDef.Location = new Point(230, 142);
        lblPreviewDef.Name = "lblPreviewDef";
        lblPreviewDef.Size = new Size(220, 22);
        lblPreviewDef.TabIndex = 6;
        lblPreviewDef.Visible = false;
        //
        // lblPreviewDesc
        //
        lblPreviewDesc.BackColor = Color.Transparent;
        lblPreviewDesc.Font = new Font("Segoe UI", 9F);
        lblPreviewDesc.ForeColor = AppColors.TextDescription;
        lblPreviewDesc.Location = new Point(0, 172);
        lblPreviewDesc.Name = "lblPreviewDesc";
        lblPreviewDesc.Size = new Size(462, 342);
        lblPreviewDesc.TabIndex = 7;
        //
        // pnlPreview
        //
        pnlPreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlPreview.AutoScroll = true;
        pnlPreview.BackColor = AppColors.AppBg;
        pnlPreview.Controls.Add(pnlPreviewText);
        pnlPreview.Controls.Add(picPreview);
        pnlPreview.Controls.Add(lblHint);
        pnlPreview.Location = new Point(281, 45);
        pnlPreview.Name = "pnlPreview";
        pnlPreview.Size = new Size(774, 554);
        pnlPreview.TabIndex = 4;
        pnlPreview.Paint += BackgroundPainter.Paint;
        //
        // imgOverlay
        //
        imgOverlay.Dock = DockStyle.Fill;
        imgOverlay.Location = new Point(0, 0);
        imgOverlay.Name = "imgOverlay";
        imgOverlay.Size = new Size(1055, 599);
        imgOverlay.TabIndex = 5;
        imgOverlay.Visible = false;
        //
        // CardSearchPage
        //
        BackColor = AppColors.AppBg;
        Controls.Add(pnlPreview);
        Controls.Add(pnlResultsDiv);
        Controls.Add(pnlResults);
        Controls.Add(pnlToolbarDiv);
        Controls.Add(pnlToolbar);
        Controls.Add(imgOverlay);
        Name = "CardSearchPage";
        Size = new Size(1055, 599);
        pnlToolbar.ResumeLayout(false);
        pnlToolbar.PerformLayout();
        pnlResults.ResumeLayout(false);
        pnlPreview.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
        pnlPreviewText.ResumeLayout(false);
        ResumeLayout(false);
    }
}
