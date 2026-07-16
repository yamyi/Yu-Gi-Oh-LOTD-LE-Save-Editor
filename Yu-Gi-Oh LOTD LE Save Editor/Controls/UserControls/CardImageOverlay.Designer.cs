namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

partial class CardImageOverlay
{
    private Panel pnlBar = null!;
    private Label lblTitle = null!;
    private Label lblZoomHint = null!;
    private Button btnClose = null!;

    private void InitializeComponent()
    {
        pnlBar = new Panel();
        lblTitle = new Label();
        lblZoomHint = new Label();
        btnClose = new Button();

        pnlBar.SuspendLayout();
        SuspendLayout();

        //
        // lblTitle
        //
        lblTitle.BackColor = Color.Transparent;
        lblTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblTitle.ForeColor = AppColors.TextPrimary;
        lblTitle.Location = new Point(16, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(400, 40);
        lblTitle.TabIndex = 0;
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;

        //
        // lblZoomHint
        //
        lblZoomHint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblZoomHint.BackColor = Color.Transparent;
        lblZoomHint.Font = new Font("Segoe UI", 8F);
        lblZoomHint.ForeColor = AppColors.TextMuted;
        lblZoomHint.Location = new Point(444, 0);
        lblZoomHint.Name = "lblZoomHint";
        lblZoomHint.Size = new Size(300, 40);
        lblZoomHint.TabIndex = 1;
        lblZoomHint.Text = "Scroll to zoom  ·  drag to pan  ·  click image or press Esc to close";
        lblZoomHint.TextAlign = ContentAlignment.MiddleRight;

        //
        // btnClose
        //
        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClose.BackColor = Color.Transparent;
        btnClose.Cursor = Cursors.Hand;
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.FlatStyle = FlatStyle.Flat;
        btnClose.Font = new Font("Segoe UI", 12F);
        btnClose.ForeColor = AppColors.TextSecondary;
        btnClose.Location = new Point(752, 0);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(48, 40);
        btnClose.TabIndex = 2;
        btnClose.Text = "✕";
        btnClose.UseVisualStyleBackColor = false;
        btnClose.Click += btnClose_Click;

        //
        // pnlBar
        //
        pnlBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlBar.BackColor = AppColors.CardBg;
        pnlBar.Controls.Add(lblTitle);
        pnlBar.Controls.Add(lblZoomHint);
        pnlBar.Controls.Add(btnClose);
        pnlBar.Location = new Point(0, 0);
        pnlBar.Name = "pnlBar";
        pnlBar.Size = new Size(800, 40);
        pnlBar.TabIndex = 0;

        //
        // CardImageOverlay
        //
        BackColor = AppColors.OverlayBackdrop;
        Controls.Add(pnlBar);
        DoubleBuffered = true;
        Name = "CardImageOverlay";
        Size = new Size(800, 600);
        TabStop = true;
        Visible = false;
        MouseDown += CardImageOverlay_MouseDown;
        MouseMove += CardImageOverlay_MouseMove;
        MouseUp += CardImageOverlay_MouseUp;
        MouseWheel += CardImageOverlay_MouseWheel;
        KeyDown += CardImageOverlay_KeyDown;

        pnlBar.ResumeLayout(false);
        ResumeLayout(false);
    }
}
