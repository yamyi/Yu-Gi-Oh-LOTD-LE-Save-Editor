namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

partial class SidebarPanel
{
    // ── Named controls ─────────────────────────────────────────────────────────
    // Logo header
    private Panel pnlLogo;
    private Label lblLogoTitle;
    private Label lblLogoSub;
    private ReaLTaiizor.Controls.Button btnToggle;
    // Dividers / border
    private Panel pnlLogoDivider;
    private Panel pnlFooterDivider;
    private Panel rightBorder;
    // Nav table  (1 col — each row is a single fused icon+label button)
    private TableLayoutPanel tblNav;
    // Section labels
    private Label lblSecManage;
    private Label lblSecTools;
    private Label lblSecFile;
    // Nav buttons — icon glyph lives inside Text now, so there is no separate
    // non-interactive icon control. Dock = Fill means the whole row is
    // clickable whether the drawer is expanded (200px) or collapsed (48px).
    private ReaLTaiizor.Controls.Button btnDeckSlots;
    private ReaLTaiizor.Controls.Button btnImportYdk;
    private ReaLTaiizor.Controls.Button btnCopySwap;
    private ReaLTaiizor.Controls.Button btnDeckEditor;
    private ReaLTaiizor.Controls.Button btnCardSearch;
    private ReaLTaiizor.Controls.Button btnOpenSave;
    private ReaLTaiizor.Controls.Button btnSaveFile;
    // Footer
    private Panel pnlFooter;
    private Label lblSaveFile;
    private Label lblStatus;

    private void InitializeComponent()
    {
        pnlLogo = new Panel();
        lblLogoTitle = new Label();
        lblLogoSub = new Label();
        btnToggle = new ReaLTaiizor.Controls.Button();
        pnlLogoDivider = new Panel();
        pnlFooterDivider = new Panel();
        rightBorder = new Panel();
        tblNav = new TableLayoutPanel();
        lblSecManage = new Label();
        btnDeckSlots = new ReaLTaiizor.Controls.Button();
        btnDeckEditor = new ReaLTaiizor.Controls.Button();
        lblSecTools = new Label();
        btnCopySwap = new ReaLTaiizor.Controls.Button();
        btnImportYdk = new ReaLTaiizor.Controls.Button();
        btnCardSearch = new ReaLTaiizor.Controls.Button();
        lblSecFile = new Label();
        btnOpenSave = new ReaLTaiizor.Controls.Button();
        btnSaveFile = new ReaLTaiizor.Controls.Button();
        pnlFooter = new Panel();
        lblSaveFile = new Label();
        lblStatus = new Label();
        pnlLogo.SuspendLayout();
        tblNav.SuspendLayout();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // pnlLogo
        // 
        pnlLogo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlLogo.BackColor = Color.FromArgb(19, 19, 31);
        pnlLogo.Controls.Add(lblLogoTitle);
        pnlLogo.Controls.Add(lblLogoSub);
        pnlLogo.Controls.Add(btnToggle);
        pnlLogo.Location = new Point(0, 0);
        pnlLogo.Name = "pnlLogo";
        pnlLogo.Size = new Size(200, 52);
        pnlLogo.TabIndex = 0;
        // 
        // lblLogoTitle
        // 
        lblLogoTitle.BackColor = Color.Transparent;
        lblLogoTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblLogoTitle.ForeColor = Color.FromArgb(201, 162, 39);
        lblLogoTitle.Location = new Point(14, 10);
        lblLogoTitle.Name = "lblLogoTitle";
        lblLogoTitle.Size = new Size(140, 18);
        lblLogoTitle.TabIndex = 0;
        lblLogoTitle.Text = "LOTD:LE  Deck Editor";
        // 
        // lblLogoSub
        // 
        lblLogoSub.BackColor = Color.Transparent;
        lblLogoSub.Font = new Font("Segoe UI", 7.5F);
        lblLogoSub.ForeColor = Color.FromArgb(90, 90, 122);
        lblLogoSub.Location = new Point(14, 28);
        lblLogoSub.Name = "lblLogoSub";
        lblLogoSub.Size = new Size(140, 14);
        lblLogoSub.TabIndex = 1;
        lblLogoSub.Text = "Save file manager";
        // 
        // btnToggle
        // 
        btnToggle.BackColor = Color.Transparent;
        btnToggle.BorderColor = Color.FromArgb(32, 34, 37);
        btnToggle.Cursor = Cursors.Hand;
        btnToggle.EnteredBorderColor = Color.FromArgb(165, 37, 37);
        btnToggle.EnteredColor = Color.FromArgb(32, 34, 37);
        btnToggle.Font = new Font("Segoe UI", 8F);
        btnToggle.Image = null;
        btnToggle.ImageAlign = ContentAlignment.MiddleLeft;
        btnToggle.InactiveColor = Color.FromArgb(32, 34, 37);
        btnToggle.Location = new Point(166, 14);
        btnToggle.Name = "btnToggle";
        btnToggle.PressedBorderColor = Color.FromArgb(165, 37, 37);
        btnToggle.PressedColor = Color.FromArgb(165, 37, 37);
        btnToggle.Size = new Size(24, 24);
        btnToggle.TabIndex = 2;
        btnToggle.TabStop = false;
        btnToggle.Text = "◀";
        btnToggle.TextAlignment = StringAlignment.Center;
        btnToggle.Click += btnToggle_Click;
        // 
        // pnlLogoDivider
        // 
        pnlLogoDivider.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlLogoDivider.BackColor = Color.FromArgb(40, 40, 60);
        pnlLogoDivider.Location = new Point(0, 52);
        pnlLogoDivider.Name = "pnlLogoDivider";
        pnlLogoDivider.Size = new Size(200, 1);
        pnlLogoDivider.TabIndex = 1;
        // 
        // pnlFooterDivider
        // 
        pnlFooterDivider.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlFooterDivider.BackColor = Color.FromArgb(40, 40, 60);
        pnlFooterDivider.Location = new Point(0, 507);
        pnlFooterDivider.Name = "pnlFooterDivider";
        pnlFooterDivider.Size = new Size(200, 1);
        pnlFooterDivider.TabIndex = 3;
        // 
        // rightBorder
        // 
        rightBorder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        rightBorder.BackColor = Color.FromArgb(40, 40, 60);
        rightBorder.Location = new Point(199, 0);
        rightBorder.Name = "rightBorder";
        rightBorder.Size = new Size(1, 600);
        rightBorder.TabIndex = 5;
        // 
        // tblNav
        // 
        tblNav.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tblNav.BackColor = Color.FromArgb(19, 19, 31);
        tblNav.ColumnCount = 1;
        tblNav.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblNav.Controls.Add(lblSecManage, 0, 0);
        tblNav.Controls.Add(btnDeckSlots, 0, 1);
        tblNav.Controls.Add(btnDeckEditor, 0, 2);
        tblNav.Controls.Add(lblSecTools, 0, 3);
        tblNav.Controls.Add(btnCopySwap, 0, 4);
        tblNav.Controls.Add(btnImportYdk, 0, 5);
        tblNav.Controls.Add(btnCardSearch, 0, 6);
        tblNav.Controls.Add(lblSecFile, 0, 7);
        tblNav.Controls.Add(btnOpenSave, 0, 8);
        tblNav.Controls.Add(btnSaveFile, 0, 9);
        tblNav.Location = new Point(0, 53);
        tblNav.Name = "tblNav";
        tblNav.Padding = new Padding(0, 8, 0, 0);
        tblNav.RowCount = 11;
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle());
        tblNav.Size = new Size(200, 326);
        tblNav.TabIndex = 2;
        // 
        // lblSecManage
        // 
        lblSecManage.BackColor = Color.Transparent;
        lblSecManage.Dock = DockStyle.Fill;
        lblSecManage.Font = new Font("Segoe UI", 7F);
        lblSecManage.ForeColor = Color.FromArgb(58, 58, 90);
        lblSecManage.Location = new Point(3, 8);
        lblSecManage.Name = "lblSecManage";
        lblSecManage.Padding = new Padding(14, 8, 0, 0);
        lblSecManage.Size = new Size(194, 30);
        lblSecManage.TabIndex = 0;
        lblSecManage.Text = "MANAGE";
        // 
        // btnDeckSlots
        // 
        btnDeckSlots.BackColor = Color.FromArgb(19, 19, 31);
        btnDeckSlots.BorderColor = Color.FromArgb(19, 19, 31);
        btnDeckSlots.Cursor = Cursors.Hand;
        btnDeckSlots.Dock = DockStyle.Fill;
        btnDeckSlots.EnteredBorderColor = Color.FromArgb(22, 22, 42);
        btnDeckSlots.EnteredColor = Color.FromArgb(22, 22, 42);
        btnDeckSlots.Font = new Font("Segoe UI", 9.5F);
        btnDeckSlots.ForeColor = Color.FromArgb(106, 106, 138);
        btnDeckSlots.Image = null;
        btnDeckSlots.ImageAlign = ContentAlignment.MiddleLeft;
        btnDeckSlots.InactiveColor = Color.FromArgb(19, 19, 31);
        btnDeckSlots.Location = new Point(3, 41);
        btnDeckSlots.Name = "btnDeckSlots";
        btnDeckSlots.PressedBorderColor = Color.FromArgb(165, 37, 37);
        btnDeckSlots.PressedColor = Color.FromArgb(165, 37, 37);
        btnDeckSlots.Size = new Size(194, 24);
        btnDeckSlots.TabIndex = 1;
        btnDeckSlots.TabStop = false;
        btnDeckSlots.Text = "⊞    Deck slots";
        btnDeckSlots.TextAlignment = StringAlignment.Near;
        btnDeckSlots.Click += btnDeckSlots_Click;
        // 
        // btnDeckEditor
        // 
        btnDeckEditor.BackColor = Color.FromArgb(19, 19, 31);
        btnDeckEditor.BorderColor = Color.FromArgb(19, 19, 31);
        btnDeckEditor.Cursor = Cursors.Hand;
        btnDeckEditor.Dock = DockStyle.Fill;
        btnDeckEditor.EnteredBorderColor = Color.FromArgb(22, 22, 42);
        btnDeckEditor.EnteredColor = Color.FromArgb(22, 22, 42);
        btnDeckEditor.Font = new Font("Segoe UI", 9.5F);
        btnDeckEditor.ForeColor = Color.FromArgb(106, 106, 138);
        btnDeckEditor.Image = null;
        btnDeckEditor.ImageAlign = ContentAlignment.MiddleLeft;
        btnDeckEditor.InactiveColor = Color.FromArgb(19, 19, 31);
        btnDeckEditor.Location = new Point(3, 71);
        btnDeckEditor.Name = "btnDeckEditor";
        btnDeckEditor.PressedBorderColor = Color.FromArgb(165, 37, 37);
        btnDeckEditor.PressedColor = Color.FromArgb(165, 37, 37);
        btnDeckEditor.Size = new Size(194, 24);
        btnDeckEditor.TabIndex = 5;
        btnDeckEditor.TabStop = false;
        btnDeckEditor.Text = "▦    Deck editor";
        btnDeckEditor.TextAlignment = StringAlignment.Near;
        btnDeckEditor.Click += btnDeckEditor_Click;
        // 
        // lblSecTools
        // 
        lblSecTools.BackColor = Color.Transparent;
        lblSecTools.Dock = DockStyle.Fill;
        lblSecTools.Font = new Font("Segoe UI", 7F);
        lblSecTools.ForeColor = Color.FromArgb(58, 58, 90);
        lblSecTools.Location = new Point(3, 98);
        lblSecTools.Name = "lblSecTools";
        lblSecTools.Padding = new Padding(14, 8, 0, 0);
        lblSecTools.Size = new Size(194, 30);
        lblSecTools.TabIndex = 3;
        lblSecTools.Text = "TOOLS";
        // 
        // btnCopySwap
        // 
        btnCopySwap.BackColor = Color.FromArgb(19, 19, 31);
        btnCopySwap.BorderColor = Color.FromArgb(19, 19, 31);
        btnCopySwap.Cursor = Cursors.Hand;
        btnCopySwap.Dock = DockStyle.Fill;
        btnCopySwap.EnteredBorderColor = Color.FromArgb(22, 22, 42);
        btnCopySwap.EnteredColor = Color.FromArgb(22, 22, 42);
        btnCopySwap.Font = new Font("Segoe UI", 9.5F);
        btnCopySwap.ForeColor = Color.FromArgb(106, 106, 138);
        btnCopySwap.Image = null;
        btnCopySwap.ImageAlign = ContentAlignment.MiddleLeft;
        btnCopySwap.InactiveColor = Color.FromArgb(19, 19, 31);
        btnCopySwap.Location = new Point(3, 131);
        btnCopySwap.Name = "btnCopySwap";
        btnCopySwap.PressedBorderColor = Color.FromArgb(165, 37, 37);
        btnCopySwap.PressedColor = Color.FromArgb(165, 37, 37);
        btnCopySwap.Size = new Size(194, 24);
        btnCopySwap.TabIndex = 4;
        btnCopySwap.TabStop = false;
        btnCopySwap.Text = "⇄    Copy / swap";
        btnCopySwap.TextAlignment = StringAlignment.Near;
        btnCopySwap.Click += btnCopySwap_Click;
        // 
        // btnImportYdk
        // 
        btnImportYdk.BackColor = Color.FromArgb(19, 19, 31);
        btnImportYdk.BorderColor = Color.FromArgb(19, 19, 31);
        btnImportYdk.Cursor = Cursors.Hand;
        btnImportYdk.Dock = DockStyle.Fill;
        btnImportYdk.EnteredBorderColor = Color.FromArgb(22, 22, 42);
        btnImportYdk.EnteredColor = Color.FromArgb(22, 22, 42);
        btnImportYdk.Font = new Font("Segoe UI", 9.5F);
        btnImportYdk.ForeColor = Color.FromArgb(106, 106, 138);
        btnImportYdk.Image = null;
        btnImportYdk.ImageAlign = ContentAlignment.MiddleLeft;
        btnImportYdk.InactiveColor = Color.FromArgb(19, 19, 31);
        btnImportYdk.Location = new Point(3, 161);
        btnImportYdk.Name = "btnImportYdk";
        btnImportYdk.PressedBorderColor = Color.FromArgb(165, 37, 37);
        btnImportYdk.PressedColor = Color.FromArgb(165, 37, 37);
        btnImportYdk.Size = new Size(194, 24);
        btnImportYdk.TabIndex = 2;
        btnImportYdk.TabStop = false;
        btnImportYdk.Text = "↑    Import .ydk";
        btnImportYdk.TextAlignment = StringAlignment.Near;
        btnImportYdk.Click += btnImportYdk_Click;
        // 
        // btnCardSearch
        // 
        btnCardSearch.BackColor = Color.FromArgb(19, 19, 31);
        btnCardSearch.BorderColor = Color.FromArgb(19, 19, 31);
        btnCardSearch.Cursor = Cursors.Hand;
        btnCardSearch.Dock = DockStyle.Fill;
        btnCardSearch.EnteredBorderColor = Color.FromArgb(22, 22, 42);
        btnCardSearch.EnteredColor = Color.FromArgb(22, 22, 42);
        btnCardSearch.Font = new Font("Segoe UI", 9.5F);
        btnCardSearch.ForeColor = Color.FromArgb(106, 106, 138);
        btnCardSearch.Image = null;
        btnCardSearch.ImageAlign = ContentAlignment.MiddleLeft;
        btnCardSearch.InactiveColor = Color.FromArgb(19, 19, 31);
        btnCardSearch.Location = new Point(3, 191);
        btnCardSearch.Name = "btnCardSearch";
        btnCardSearch.PressedBorderColor = Color.FromArgb(165, 37, 37);
        btnCardSearch.PressedColor = Color.FromArgb(165, 37, 37);
        btnCardSearch.Size = new Size(194, 24);
        btnCardSearch.TabIndex = 6;
        btnCardSearch.TabStop = false;
        btnCardSearch.Text = "⌕    Card search";
        btnCardSearch.TextAlignment = StringAlignment.Near;
        btnCardSearch.Click += btnCardSearch_Click;
        // 
        // lblSecFile
        // 
        lblSecFile.BackColor = Color.Transparent;
        lblSecFile.Dock = DockStyle.Fill;
        lblSecFile.Font = new Font("Segoe UI", 7F);
        lblSecFile.ForeColor = Color.FromArgb(58, 58, 90);
        lblSecFile.Location = new Point(3, 218);
        lblSecFile.Name = "lblSecFile";
        lblSecFile.Padding = new Padding(14, 8, 0, 0);
        lblSecFile.Size = new Size(194, 30);
        lblSecFile.TabIndex = 7;
        lblSecFile.Text = "FILE";
        // 
        // btnOpenSave
        // 
        btnOpenSave.BackColor = Color.FromArgb(19, 19, 31);
        btnOpenSave.BorderColor = Color.FromArgb(19, 19, 31);
        btnOpenSave.Cursor = Cursors.Hand;
        btnOpenSave.Dock = DockStyle.Fill;
        btnOpenSave.EnteredBorderColor = Color.FromArgb(22, 22, 42);
        btnOpenSave.EnteredColor = Color.FromArgb(22, 22, 42);
        btnOpenSave.Font = new Font("Segoe UI", 9.5F);
        btnOpenSave.ForeColor = Color.FromArgb(106, 106, 138);
        btnOpenSave.Image = null;
        btnOpenSave.ImageAlign = ContentAlignment.MiddleLeft;
        btnOpenSave.InactiveColor = Color.FromArgb(19, 19, 31);
        btnOpenSave.Location = new Point(3, 251);
        btnOpenSave.Name = "btnOpenSave";
        btnOpenSave.PressedBorderColor = Color.FromArgb(165, 37, 37);
        btnOpenSave.PressedColor = Color.FromArgb(165, 37, 37);
        btnOpenSave.Size = new Size(194, 24);
        btnOpenSave.TabIndex = 8;
        btnOpenSave.TabStop = false;
        btnOpenSave.Text = "▤    Open save";
        btnOpenSave.TextAlignment = StringAlignment.Near;
        btnOpenSave.Click += btnOpenSave_Click;
        // 
        // btnSaveFile
        // 
        btnSaveFile.BackColor = Color.FromArgb(19, 19, 31);
        btnSaveFile.BorderColor = Color.FromArgb(19, 19, 31);
        btnSaveFile.Cursor = Cursors.Hand;
        btnSaveFile.Dock = DockStyle.Fill;
        btnSaveFile.EnteredBorderColor = Color.FromArgb(22, 22, 42);
        btnSaveFile.EnteredColor = Color.FromArgb(22, 22, 42);
        btnSaveFile.Font = new Font("Segoe UI", 9.5F);
        btnSaveFile.ForeColor = Color.FromArgb(106, 106, 138);
        btnSaveFile.Image = null;
        btnSaveFile.ImageAlign = ContentAlignment.MiddleLeft;
        btnSaveFile.InactiveColor = Color.FromArgb(19, 19, 31);
        btnSaveFile.Location = new Point(3, 281);
        btnSaveFile.Name = "btnSaveFile";
        btnSaveFile.PressedBorderColor = Color.FromArgb(165, 37, 37);
        btnSaveFile.PressedColor = Color.FromArgb(165, 37, 37);
        btnSaveFile.Size = new Size(194, 24);
        btnSaveFile.TabIndex = 9;
        btnSaveFile.TabStop = false;
        btnSaveFile.Text = "💾    Save file";
        btnSaveFile.TextAlignment = StringAlignment.Near;
        btnSaveFile.Click += btnSaveFile_Click;
        // 
        // pnlFooter
        // 
        pnlFooter.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlFooter.BackColor = Color.FromArgb(19, 19, 31);
        pnlFooter.Controls.Add(lblSaveFile);
        pnlFooter.Controls.Add(lblStatus);
        pnlFooter.Location = new Point(0, 508);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(200, 46);
        pnlFooter.TabIndex = 4;
        // 
        // lblSaveFile
        // 
        lblSaveFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblSaveFile.BackColor = Color.Transparent;
        lblSaveFile.Font = new Font("Segoe UI", 7.5F);
        lblSaveFile.ForeColor = Color.FromArgb(58, 58, 90);
        lblSaveFile.Location = new Point(14, 8);
        lblSaveFile.Name = "lblSaveFile";
        lblSaveFile.Size = new Size(178, 18);
        lblSaveFile.TabIndex = 0;
        lblSaveFile.Text = "📄  SAVEDATA_01.bin";
        // 
        // lblStatus
        // 
        lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblStatus.BackColor = Color.Transparent;
        lblStatus.Font = new Font("Segoe UI", 7.5F);
        lblStatus.ForeColor = Color.FromArgb(58, 130, 58);
        lblStatus.Location = new Point(14, 26);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(178, 16);
        lblStatus.TabIndex = 1;
        lblStatus.Text = "● 32 slots loaded";
        // 
        // SidebarPanel
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(19, 19, 31);
        Controls.Add(pnlLogo);
        Controls.Add(pnlLogoDivider);
        Controls.Add(tblNav);
        Controls.Add(pnlFooterDivider);
        Controls.Add(pnlFooter);
        Controls.Add(rightBorder);
        Name = "SidebarPanel";
        Size = new Size(200, 600);
        pnlLogo.ResumeLayout(false);
        tblNav.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

}