namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

partial class DeckCard
{
    // ── Child controls ────────────────────────────────────────────────────────
    // Shared (always visible)
    private Label lblSlotNum;
    // Empty-state
    private Label lblEmpty;
    // Filled-state
    private PictureBox picPortrait;
    private Label lblDeckName;
    private Label lblOwnerName;
    private Panel pnlDivider;
    // Pips
    private Panel pnlPipMain;
    private Label lblMainCount;
    private Label lblMainLabel;
    private Panel pnlPipExtra;
    private Label lblExtraCount;
    private Label lblExtraLabel;
    private Panel pnlPipSide;
    private Label lblSideCount;
    private Label lblSideLabel;

    private void InitializeComponent()
    {
        lblSlotNum = new Label();
        lblEmpty = new Label();
        picPortrait = new PictureBox();
        lblDeckName = new Label();
        lblOwnerName = new Label();
        pnlDivider = new Panel();
        pnlPipMain = new Panel();
        lblMainCount = new Label();
        lblMainLabel = new Label();
        pnlPipExtra = new Panel();
        lblExtraCount = new Label();
        lblExtraLabel = new Label();
        pnlPipSide = new Panel();
        lblSideCount = new Label();
        lblSideLabel = new Label();
        ((System.ComponentModel.ISupportInitialize)picPortrait).BeginInit();
        pnlPipMain.SuspendLayout();
        pnlPipExtra.SuspendLayout();
        pnlPipSide.SuspendLayout();
        SuspendLayout();
        // 
        // lblSlotNum
        // 
        lblSlotNum.BackColor = Color.Transparent;
        lblSlotNum.Dock = DockStyle.Top;
        lblSlotNum.Font = new Font("Segoe UI", 7F);
        lblSlotNum.ForeColor = AppColors.TextDim;
        lblSlotNum.Location = new Point(0, 0);
        lblSlotNum.Name = "lblSlotNum";
        lblSlotNum.Padding = new Padding(8, 5, 0, 0);
        lblSlotNum.Size = new Size(204, 21);
        lblSlotNum.TabIndex = 0;
        lblSlotNum.Text = "SLOT 01";
        // 
        // lblEmpty
        // 
        lblEmpty.BackColor = Color.Transparent;
        lblEmpty.Dock = DockStyle.Fill;
        lblEmpty.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblEmpty.ForeColor = AppColors.TextMuted;
        lblEmpty.Location = new Point(0, 21);
        lblEmpty.Name = "lblEmpty";
        lblEmpty.Size = new Size(204, 122);
        lblEmpty.TabIndex = 1;
        lblEmpty.Text = "— Empty —\r\n";
        lblEmpty.TextAlign = ContentAlignment.MiddleCenter;
        lblEmpty.Visible = false;
        // 
        // picPortrait
        // 
        picPortrait.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        picPortrait.BackColor = AppColors.CardSelected;
        picPortrait.Location = new Point(153, 18);
        picPortrait.Name = "picPortrait";
        picPortrait.Size = new Size(40, 40);
        picPortrait.SizeMode = PictureBoxSizeMode.Zoom;
        picPortrait.TabIndex = 2;
        picPortrait.TabStop = false;
        picPortrait.Visible = false;
        // 
        // lblDeckName
        // 
        lblDeckName.AutoEllipsis = true;
        lblDeckName.BackColor = Color.Transparent;
        lblDeckName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDeckName.ForeColor = AppColors.TextPrimary;
        lblDeckName.Location = new Point(8, 21);
        lblDeckName.Name = "lblDeckName";
        lblDeckName.Size = new Size(116, 37);
        lblDeckName.TabIndex = 3;
        lblDeckName.Text = "Blue-Eyes Chaos White Dragon";
        lblDeckName.UseMnemonic = false;
        lblDeckName.Visible = false;
        // 
        // lblOwnerName
        // 
        lblOwnerName.AutoEllipsis = true;
        lblOwnerName.BackColor = Color.Transparent;
        lblOwnerName.Font = new Font("Segoe UI", 7.5F);
        lblOwnerName.ForeColor = AppColors.TextMuted;
        lblOwnerName.Location = new Point(8, 55);
        lblOwnerName.Name = "lblOwnerName";
        lblOwnerName.Size = new Size(139, 13);
        lblOwnerName.TabIndex = 4;
        lblOwnerName.Text = "Yusei Fudo";
        lblOwnerName.UseMnemonic = false;
        lblOwnerName.Visible = false;
        // 
        // pnlDivider
        // 
        pnlDivider.BackColor = AppColors.Border;
        pnlDivider.Location = new Point(0, 69);
        pnlDivider.Name = "pnlDivider";
        pnlDivider.Size = new Size(205, 5);
        pnlDivider.TabIndex = 5;
        pnlDivider.Visible = false;
        // 
        // pnlPipMain
        // 
        pnlPipMain.BackColor = AppColors.MainBg;
        pnlPipMain.Controls.Add(lblMainCount);
        pnlPipMain.Controls.Add(lblMainLabel);
        pnlPipMain.Location = new Point(8, 87);
        pnlPipMain.Name = "pnlPipMain";
        pnlPipMain.Size = new Size(47, 48);
        pnlPipMain.TabIndex = 6;
        pnlPipMain.Visible = false;
        // 
        // lblMainCount
        // 
        lblMainCount.BackColor = Color.Transparent;
        lblMainCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblMainCount.ForeColor = AppColors.MainGreen;
        lblMainCount.Location = new Point(0, 8);
        lblMainCount.Name = "lblMainCount";
        lblMainCount.Size = new Size(47, 13);
        lblMainCount.TabIndex = 0;
        lblMainCount.Text = "0";
        lblMainCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMainLabel
        // 
        lblMainLabel.BackColor = Color.Transparent;
        lblMainLabel.Font = new Font("Segoe UI", 6.5F);
        lblMainLabel.ForeColor = AppColors.MainGreen;
        lblMainLabel.Location = new Point(0, 27);
        lblMainLabel.Name = "lblMainLabel";
        lblMainLabel.Size = new Size(47, 10);
        lblMainLabel.TabIndex = 1;
        lblMainLabel.Text = "MAIN";
        lblMainLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlPipExtra
        // 
        pnlPipExtra.BackColor = AppColors.ExtraBg;
        pnlPipExtra.Controls.Add(lblExtraCount);
        pnlPipExtra.Controls.Add(lblExtraLabel);
        pnlPipExtra.Location = new Point(77, 87);
        pnlPipExtra.Name = "pnlPipExtra";
        pnlPipExtra.Size = new Size(47, 48);
        pnlPipExtra.TabIndex = 7;
        pnlPipExtra.Visible = false;
        // 
        // lblExtraCount
        // 
        lblExtraCount.BackColor = Color.Transparent;
        lblExtraCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblExtraCount.ForeColor = AppColors.ExtraBlue;
        lblExtraCount.Location = new Point(0, 8);
        lblExtraCount.Name = "lblExtraCount";
        lblExtraCount.Size = new Size(47, 13);
        lblExtraCount.TabIndex = 0;
        lblExtraCount.Text = "0";
        lblExtraCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblExtraLabel
        // 
        lblExtraLabel.BackColor = Color.Transparent;
        lblExtraLabel.Font = new Font("Segoe UI", 6.5F);
        lblExtraLabel.ForeColor = AppColors.ExtraBlue;
        lblExtraLabel.Location = new Point(0, 27);
        lblExtraLabel.Name = "lblExtraLabel";
        lblExtraLabel.Size = new Size(47, 10);
        lblExtraLabel.TabIndex = 1;
        lblExtraLabel.Text = "EXTRA";
        lblExtraLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlPipSide
        // 
        pnlPipSide.BackColor = AppColors.SideBg;
        pnlPipSide.Controls.Add(lblSideCount);
        pnlPipSide.Controls.Add(lblSideLabel);
        pnlPipSide.Location = new Point(146, 87);
        pnlPipSide.Name = "pnlPipSide";
        pnlPipSide.Size = new Size(47, 48);
        pnlPipSide.TabIndex = 8;
        pnlPipSide.Visible = false;
        // 
        // lblSideCount
        // 
        lblSideCount.BackColor = Color.Transparent;
        lblSideCount.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblSideCount.ForeColor = AppColors.SideRed;
        lblSideCount.Location = new Point(0, 8);
        lblSideCount.Name = "lblSideCount";
        lblSideCount.Size = new Size(47, 13);
        lblSideCount.TabIndex = 0;
        lblSideCount.Text = "0";
        lblSideCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblSideLabel
        // 
        lblSideLabel.BackColor = Color.Transparent;
        lblSideLabel.Font = new Font("Segoe UI", 6.5F);
        lblSideLabel.ForeColor = AppColors.SideRed;
        lblSideLabel.Location = new Point(0, 27);
        lblSideLabel.Name = "lblSideLabel";
        lblSideLabel.Size = new Size(47, 10);
        lblSideLabel.TabIndex = 1;
        lblSideLabel.Text = "SIDE";
        lblSideLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // DeckCard
        // 
        BackColor = AppColors.CardBg;
        Controls.Add(picPortrait);
        Controls.Add(lblDeckName);
        Controls.Add(lblOwnerName);
        Controls.Add(pnlDivider);
        Controls.Add(pnlPipMain);
        Controls.Add(pnlPipExtra);
        Controls.Add(pnlPipSide);
        Controls.Add(lblEmpty);
        Controls.Add(lblSlotNum);
        Cursor = Cursors.Hand;
        DoubleBuffered = true;
        Name = "DeckCard";
        Size = new Size(204, 143);
        ((System.ComponentModel.ISupportInitialize)picPortrait).EndInit();
        pnlPipMain.ResumeLayout(false);
        pnlPipExtra.ResumeLayout(false);
        pnlPipSide.ResumeLayout(false);
        ResumeLayout(false);
    }
}