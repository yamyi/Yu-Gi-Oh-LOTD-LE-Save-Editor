using ReaLTaiizor.Controls;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;
using Panel = System.Windows.Forms.Panel;
using TabPage = System.Windows.Forms.TabPage;
using Button = System.Windows.Forms.Button;


namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

partial class MainForm
{
    // ── Named controls ────────────────────────────────────────────────────────
    private Panel              pnlTopbar       = null!;
    private Label              lblAppTitle     = null!;
    private Label              lblSaveFile     = null!;
    private Button             btnUndo         = null!;
    private Button             btnRedo         = null!;
    private Button             btnSave         = null!;
    private Panel              pnlTopDivider   = null!;
    private MaterialTabControl tabControl      = null!;
    private TabPage            tpDeckSlots     = null!;
    private TabPage            tpDeckEditor    = null!;
    private TabPage            tpCardSearch    = null!;
    private TabPage            tpSettings      = null!;

    // ── Sidebar / drawer (formerly SidebarPanel UserControl, now inline) ───────
    private Panel                       pnlSidebar       = null!;
    private ReaLTaiizor.Controls.Button btnToggle        = null!;
    private Panel                       pnlFooterDivider = null!;
    private Panel                       rightBorder      = null!;
    private TableLayoutPanel            tblNav           = null!;
    private Label                       lblSecManage     = null!;
    private ReaLTaiizor.Controls.Button btnDeckSlots     = null!;
    private ReaLTaiizor.Controls.Button btnDeckEditor    = null!;
    private ReaLTaiizor.Controls.Button btnCardSearch    = null!;
    private Label                       lblSecTools      = null!;
    private Label                       lblSecFile       = null!;
    private ReaLTaiizor.Controls.Button btnOpenSave      = null!;
    private ReaLTaiizor.Controls.Button btnSaveFile      = null!;
    private Panel                       pnlFooter        = null!;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        pnlTopbar = new Panel();
        btnToggle = new ReaLTaiizor.Controls.Button();
        lblAppTitle = new Label();
        lblSaveFile = new Label();
        btnUndo = new Button();
        btnRedo = new Button();
        btnSave = new Button();
        pnlTopDivider = new Panel();
        tabControl = new MaterialTabControl();
        tpDeckSlots = new TabPage();
        deckSlotsPage = new DeckSlotsPage();
        tpDeckEditor = new TabPage();
        deckEditorPage = new DeckEditorPage();
        tpCardSearch = new TabPage();
        cardSearchPage = new CardSearchPage();
        tpSettings = new TabPage();
        settingsPage = new SettingsPage();
        pnlSidebar = new Panel();
        pnlFooterDivider = new Panel();
        pnlFooter = new Panel();
        btnSettings = new ReaLTaiizor.Controls.Button();
        rightBorder = new Panel();
        tblNav = new TableLayoutPanel();
        lblSecManage = new Label();
        btnDeckSlots = new ReaLTaiizor.Controls.Button();
        btnDeckEditor = new ReaLTaiizor.Controls.Button();
        lblSecTools = new Label();
        btnCardSearch = new ReaLTaiizor.Controls.Button();
        lblSecFile = new Label();
        btnOpenSave = new ReaLTaiizor.Controls.Button();
        btnSaveFile = new ReaLTaiizor.Controls.Button();
        pnlLeftDivider = new Panel();
        pnlTopbar.SuspendLayout();
        tabControl.SuspendLayout();
        tpDeckSlots.SuspendLayout();
        tpDeckEditor.SuspendLayout();
        tpCardSearch.SuspendLayout();
        tpSettings.SuspendLayout();
        pnlSidebar.SuspendLayout();
        pnlFooter.SuspendLayout();
        tblNav.SuspendLayout();
        SuspendLayout();
        // 
        // pnlTopbar
        // 
        pnlTopbar.BackColor = Color.FromArgb(22, 22, 34);
        pnlTopbar.Controls.Add(btnToggle);
        pnlTopbar.Controls.Add(lblAppTitle);
        pnlTopbar.Controls.Add(lblSaveFile);
        pnlTopbar.Controls.Add(btnUndo);
        pnlTopbar.Controls.Add(btnRedo);
        pnlTopbar.Controls.Add(btnSave);
        pnlTopbar.Dock = DockStyle.Top;
        pnlTopbar.Location = new Point(3, 0);
        pnlTopbar.Name = "pnlTopbar";
        pnlTopbar.Size = new Size(1258, 52);
        pnlTopbar.TabIndex = 2;
        // 
        // btnToggle
        // 
        btnToggle.BackColor = Color.Transparent;
        btnToggle.BorderColor = Color.FromArgb(42, 42, 52);
        btnToggle.Cursor = Cursors.Hand;
        btnToggle.EnteredBorderColor = Color.FromArgb(206, 68, 68);
        btnToggle.EnteredColor = Color.FromArgb(42, 42, 52);
        btnToggle.Font = new Font("Segoe UI", 8F);
        btnToggle.Image = null;
        btnToggle.ImageAlign = ContentAlignment.MiddleLeft;
        btnToggle.InactiveColor = Color.FromArgb(42, 42, 52);
        btnToggle.Location = new Point(4, 14);
        btnToggle.Name = "btnToggle";
        btnToggle.PressedBorderColor = Color.FromArgb(206, 68, 68);
        btnToggle.PressedColor = Color.FromArgb(206, 68, 68);
        btnToggle.Size = new Size(24, 24);
        btnToggle.TabIndex = 2;
        btnToggle.TabStop = false;
        btnToggle.Text = "◀";
        btnToggle.TextAlignment = StringAlignment.Center;
        btnToggle.Click += btnToggle_Click;
        // 
        // lblAppTitle
        // 
        lblAppTitle.AutoSize = true;
        lblAppTitle.BackColor = Color.Transparent;
        lblAppTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblAppTitle.ForeColor = Color.FromArgb(238, 238, 250);
        lblAppTitle.Location = new Point(45, 12);
        lblAppTitle.Name = "lblAppTitle";
        lblAppTitle.Size = new Size(238, 19);
        lblAppTitle.TabIndex = 1;
        lblAppTitle.Text = "Yu-Gi-Oh  LOTD:LE  —  Deck Editor";
        // 
        // lblSaveFile
        // 
        lblSaveFile.AutoSize = true;
        lblSaveFile.BackColor = Color.Transparent;
        lblSaveFile.Font = new Font("Segoe UI", 8F);
        lblSaveFile.ForeColor = Color.FromArgb(92, 92, 116);
        lblSaveFile.Location = new Point(45, 31);
        lblSaveFile.Name = "lblSaveFile";
        lblSaveFile.Size = new Size(100, 13);
        lblSaveFile.TabIndex = 2;
        lblSaveFile.Text = "Save File Manager";
        //
        // btnUndo
        //
        btnUndo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnUndo.BackColor = Color.Transparent;
        btnUndo.Cursor = Cursors.Hand;
        btnUndo.Enabled = false;
        btnUndo.FlatAppearance.BorderColor = AppColors.ButtonNeutralBg;
        btnUndo.FlatAppearance.MouseOverBackColor = AppColors.ButtonNeutralHover;
        btnUndo.FlatStyle = FlatStyle.Flat;
        btnUndo.Font = new Font("Segoe UI", 10F);
        btnUndo.ForeColor = AppColors.TextSecondary;
        btnUndo.Location = new Point(1097, 12);
        btnUndo.Name = "btnUndo";
        btnUndo.Size = new Size(32, 30);
        btnUndo.TabIndex = 4;
        btnUndo.Text = "↶";
        btnUndo.TextAlign = ContentAlignment.MiddleCenter;
        btnUndo.UseVisualStyleBackColor = false;
        btnUndo.Click += btnUndo_Click;
        //
        // btnRedo
        //
        btnRedo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRedo.BackColor = Color.Transparent;
        btnRedo.Cursor = Cursors.Hand;
        btnRedo.Enabled = false;
        btnRedo.FlatAppearance.BorderColor = AppColors.ButtonNeutralBg;
        btnRedo.FlatAppearance.MouseOverBackColor = AppColors.ButtonNeutralHover;
        btnRedo.FlatStyle = FlatStyle.Flat;
        btnRedo.Font = new Font("Segoe UI", 10F);
        btnRedo.ForeColor = AppColors.TextSecondary;
        btnRedo.Location = new Point(1137, 12);
        btnRedo.Name = "btnRedo";
        btnRedo.Size = new Size(32, 30);
        btnRedo.TabIndex = 5;
        btnRedo.Text = "↷";
        btnRedo.TextAlign = ContentAlignment.MiddleCenter;
        btnRedo.UseVisualStyleBackColor = false;
        btnRedo.Click += btnRedo_Click;
        //
        // btnSave
        //
        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.BackColor = Color.Transparent;
        btnSave.Cursor = Cursors.Hand;
        btnSave.Enabled = false;
        btnSave.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnSave.FlatAppearance.MouseOverBackColor = Color.FromArgb(48, 46, 76);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.Font = new Font("Segoe UI", 8.5F);
        btnSave.ForeColor = Color.FromArgb(138, 138, 164);
        btnSave.Location = new Point(1177, 12);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(68, 30);
        btnSave.TabIndex = 3;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.EnabledChanged += btnSave_EnabledChanged;
        btnSave.Click += btnSave_Click;
        // 
        // pnlTopDivider
        // 
        pnlTopDivider.BackColor = Color.FromArgb(70, 46, 112);
        pnlTopDivider.Dock = DockStyle.Top;
        pnlTopDivider.Location = new Point(198, 52);
        pnlTopDivider.Name = "pnlTopDivider";
        pnlTopDivider.Size = new Size(1063, 1);
        pnlTopDivider.TabIndex = 1;
        // 
        // tabControl
        // 
        tabControl.Controls.Add(tpDeckSlots);
        tabControl.Controls.Add(tpDeckEditor);
        tabControl.Controls.Add(tpCardSearch);
        tabControl.Controls.Add(tpSettings);
        tabControl.Depth = 0;
        tabControl.Dock = DockStyle.Fill;
        tabControl.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        tabControl.Location = new Point(199, 53);
        tabControl.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
        tabControl.Multiline = true;
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(1062, 625);
        tabControl.TabIndex = 0;
        tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        // 
        // tpDeckSlots
        // 
        tpDeckSlots.BackColor = Color.FromArgb(13, 13, 22);
        tpDeckSlots.Controls.Add(deckSlotsPage);
        tpDeckSlots.Location = new Point(4, 22);
        tpDeckSlots.Name = "tpDeckSlots";
        tpDeckSlots.Size = new Size(1054, 599);
        tpDeckSlots.TabIndex = 0;
        tpDeckSlots.Text = "Deck Slots";
        // 
        // deckSlotsPage
        // 
        deckSlotsPage.BackColor = Color.FromArgb(13, 13, 22);
        deckSlotsPage.Dock = DockStyle.Fill;
        deckSlotsPage.Location = new Point(0, 0);
        deckSlotsPage.Name = "deckSlotsPage";
        deckSlotsPage.Size = new Size(1054, 599);
        deckSlotsPage.TabIndex = 0;
        // 
        // tpDeckEditor
        // 
        tpDeckEditor.BackColor = Color.FromArgb(13, 13, 22);
        tpDeckEditor.Controls.Add(deckEditorPage);
        tpDeckEditor.Location = new Point(4, 22);
        tpDeckEditor.Name = "tpDeckEditor";
        tpDeckEditor.Size = new Size(1054, 599);
        tpDeckEditor.TabIndex = 3;
        tpDeckEditor.Text = "Deck Editor";
        // 
        // deckEditorPage
        // 
        deckEditorPage.BackColor = Color.FromArgb(13, 13, 22);
        deckEditorPage.Dock = DockStyle.Fill;
        deckEditorPage.IsOnline = true;
        deckEditorPage.Location = new Point(0, 0);
        deckEditorPage.Name = "deckEditorPage";
        deckEditorPage.Size = new Size(1054, 599);
        deckEditorPage.TabIndex = 0;
        // 
        // tpCardSearch
        // 
        tpCardSearch.BackColor = Color.FromArgb(13, 13, 22);
        tpCardSearch.Controls.Add(cardSearchPage);
        tpCardSearch.Location = new Point(4, 22);
        tpCardSearch.Name = "tpCardSearch";
        tpCardSearch.Size = new Size(1054, 599);
        tpCardSearch.TabIndex = 4;
        tpCardSearch.Text = "Card Search";
        // 
        // cardSearchPage
        // 
        cardSearchPage.BackColor = Color.FromArgb(13, 13, 22);
        cardSearchPage.Dock = DockStyle.Fill;
        cardSearchPage.IsOnline = true;
        cardSearchPage.Location = new Point(0, 0);
        cardSearchPage.Name = "cardSearchPage";
        cardSearchPage.Size = new Size(1054, 599);
        cardSearchPage.TabIndex = 0;
        // 
        // tpSettings
        // 
        tpSettings.BackColor = Color.FromArgb(13, 13, 22);
        tpSettings.Controls.Add(settingsPage);
        tpSettings.Location = new Point(4, 22);
        tpSettings.Name = "tpSettings";
        tpSettings.Size = new Size(1054, 599);
        tpSettings.TabIndex = 5;
        tpSettings.Text = "Settings";
        // 
        // settingsPage
        // 
        settingsPage.BackColor = Color.FromArgb(13, 13, 22);
        settingsPage.Dock = DockStyle.Fill;
        settingsPage.Location = new Point(0, 0);
        settingsPage.Name = "settingsPage";
        settingsPage.Size = new Size(1054, 599);
        settingsPage.TabIndex = 0;
        // 
        // pnlSidebar
        // 
        pnlSidebar.BackColor = Color.FromArgb(22, 22, 34);
        pnlSidebar.Controls.Add(pnlFooterDivider);
        pnlSidebar.Controls.Add(pnlFooter);
        pnlSidebar.Controls.Add(rightBorder);
        pnlSidebar.Controls.Add(tblNav);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.Location = new Point(3, 52);
        pnlSidebar.Name = "pnlSidebar";
        pnlSidebar.Size = new Size(195, 626);
        pnlSidebar.TabIndex = 4;
        // 
        // pnlFooterDivider
        // 
        pnlFooterDivider.BackColor = Color.FromArgb(60, 60, 86);
        pnlFooterDivider.Dock = DockStyle.Bottom;
        pnlFooterDivider.Location = new Point(0, 586);
        pnlFooterDivider.Name = "pnlFooterDivider";
        pnlFooterDivider.Size = new Size(195, 1);
        pnlFooterDivider.TabIndex = 3;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(28, 28, 42);
        pnlFooter.Controls.Add(btnSettings);
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 587);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.Size = new Size(195, 39);
        pnlFooter.TabIndex = 4;
        // 
        // btnSettings
        // 
        btnSettings.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnSettings.BackColor = Color.FromArgb(28, 28, 42);
        btnSettings.BorderColor = Color.FromArgb(28, 28, 42);
        btnSettings.Cursor = Cursors.Hand;
        btnSettings.EnteredBorderColor = Color.FromArgb(40, 40, 60);
        btnSettings.EnteredColor = Color.FromArgb(40, 40, 60);
        btnSettings.Font = new Font("Segoe UI", 9.5F);
        btnSettings.Image = null;
        btnSettings.ImageAlign = ContentAlignment.MiddleLeft;
        btnSettings.InactiveColor = Color.FromArgb(28, 28, 42);
        btnSettings.Location = new Point(3, 8);
        btnSettings.Name = "btnSettings";
        btnSettings.PressedBorderColor = Color.FromArgb(206, 68, 68);
        btnSettings.PressedColor = Color.FromArgb(206, 68, 68);
        btnSettings.Size = new Size(195, 24);
        btnSettings.TabIndex = 10;
        btnSettings.TabStop = false;
        btnSettings.Text = "⚙    Settings";
        btnSettings.TextAlignment = StringAlignment.Near;
        btnSettings.Click += btnSettings_Click;
        // 
        // rightBorder
        // 
        rightBorder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        rightBorder.BackColor = Color.FromArgb(60, 60, 86);
        rightBorder.Location = new Point(199, 0);
        rightBorder.Name = "rightBorder";
        rightBorder.Size = new Size(1, 548);
        rightBorder.TabIndex = 5;
        // 
        // tblNav
        // 
        tblNav.BackColor = Color.FromArgb(28, 28, 42);
        tblNav.ColumnCount = 1;
        tblNav.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tblNav.Controls.Add(lblSecManage, 0, 0);
        tblNav.Controls.Add(btnDeckSlots, 0, 1);
        tblNav.Controls.Add(btnDeckEditor, 0, 2);
        tblNav.Controls.Add(lblSecTools, 0, 3);
        tblNav.Controls.Add(btnCardSearch, 0, 4);
        tblNav.Controls.Add(lblSecFile, 0, 5);
        tblNav.Controls.Add(btnOpenSave, 0, 6);
        tblNav.Controls.Add(btnSaveFile, 0, 7);
        tblNav.Dock = DockStyle.Fill;
        tblNav.Location = new Point(0, 0);
        tblNav.Name = "tblNav";
        tblNav.Padding = new Padding(0, 8, 0, 0);
        tblNav.RowCount = 10;
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle());
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tblNav.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        tblNav.Size = new Size(195, 626);
        tblNav.TabIndex = 2;
        // 
        // lblSecManage
        // 
        lblSecManage.BackColor = Color.Transparent;
        lblSecManage.Dock = DockStyle.Fill;
        lblSecManage.Font = new Font("Segoe UI", 7F);
        lblSecManage.ForeColor = Color.FromArgb(92, 92, 116);
        lblSecManage.Location = new Point(3, 8);
        lblSecManage.Name = "lblSecManage";
        lblSecManage.Padding = new Padding(14, 8, 0, 0);
        lblSecManage.Size = new Size(189, 30);
        lblSecManage.TabIndex = 0;
        lblSecManage.Text = "MANAGE";
        // 
        // btnDeckSlots
        // 
        btnDeckSlots.BackColor = Color.FromArgb(28, 28, 42);
        btnDeckSlots.BorderColor = Color.FromArgb(28, 28, 42);
        btnDeckSlots.Cursor = Cursors.Hand;
        btnDeckSlots.Dock = DockStyle.Fill;
        btnDeckSlots.EnteredBorderColor = Color.FromArgb(40, 40, 60);
        btnDeckSlots.EnteredColor = Color.FromArgb(40, 40, 60);
        btnDeckSlots.Font = new Font("Segoe UI", 9.5F);
        btnDeckSlots.Image = null;
        btnDeckSlots.ImageAlign = ContentAlignment.MiddleLeft;
        btnDeckSlots.InactiveColor = Color.FromArgb(28, 28, 42);
        btnDeckSlots.Location = new Point(3, 41);
        btnDeckSlots.Name = "btnDeckSlots";
        btnDeckSlots.PressedBorderColor = Color.FromArgb(206, 68, 68);
        btnDeckSlots.PressedColor = Color.FromArgb(206, 68, 68);
        btnDeckSlots.Size = new Size(189, 24);
        btnDeckSlots.TabIndex = 1;
        btnDeckSlots.TabStop = false;
        btnDeckSlots.Text = "⊞    Deck slots";
        btnDeckSlots.TextAlignment = StringAlignment.Near;
        btnDeckSlots.Click += btnDeckSlots_Click;
        // 
        // btnDeckEditor
        // 
        btnDeckEditor.BackColor = Color.FromArgb(28, 28, 42);
        btnDeckEditor.BorderColor = Color.FromArgb(28, 28, 42);
        btnDeckEditor.Cursor = Cursors.Hand;
        btnDeckEditor.Dock = DockStyle.Fill;
        btnDeckEditor.EnteredBorderColor = Color.FromArgb(40, 40, 60);
        btnDeckEditor.EnteredColor = Color.FromArgb(40, 40, 60);
        btnDeckEditor.Font = new Font("Segoe UI", 9.5F);
        btnDeckEditor.Image = null;
        btnDeckEditor.ImageAlign = ContentAlignment.MiddleLeft;
        btnDeckEditor.InactiveColor = Color.FromArgb(28, 28, 42);
        btnDeckEditor.Location = new Point(3, 71);
        btnDeckEditor.Name = "btnDeckEditor";
        btnDeckEditor.PressedBorderColor = Color.FromArgb(206, 68, 68);
        btnDeckEditor.PressedColor = Color.FromArgb(206, 68, 68);
        btnDeckEditor.Size = new Size(189, 24);
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
        lblSecTools.ForeColor = Color.FromArgb(92, 92, 116);
        lblSecTools.Location = new Point(3, 98);
        lblSecTools.Name = "lblSecTools";
        lblSecTools.Padding = new Padding(14, 8, 0, 0);
        lblSecTools.Size = new Size(189, 30);
        lblSecTools.TabIndex = 3;
        lblSecTools.Text = "TOOLS";
        // 
        // btnCardSearch
        // 
        btnCardSearch.BackColor = Color.FromArgb(24, 24, 36);
        btnCardSearch.BorderColor = Color.FromArgb(24, 24, 36);
        btnCardSearch.Cursor = Cursors.Hand;
        btnCardSearch.Dock = DockStyle.Fill;
        btnCardSearch.EnteredBorderColor = Color.FromArgb(40, 40, 60);
        btnCardSearch.EnteredColor = Color.FromArgb(40, 40, 60);
        btnCardSearch.Font = new Font("Segoe UI", 9.5F);
        btnCardSearch.Image = null;
        btnCardSearch.ImageAlign = ContentAlignment.MiddleLeft;
        btnCardSearch.InactiveColor = Color.FromArgb(24, 24, 36);
        btnCardSearch.Location = new Point(3, 131);
        btnCardSearch.Name = "btnCardSearch";
        btnCardSearch.PressedBorderColor = Color.FromArgb(206, 68, 68);
        btnCardSearch.PressedColor = Color.FromArgb(206, 68, 68);
        btnCardSearch.Size = new Size(189, 24);
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
        lblSecFile.ForeColor = Color.FromArgb(92, 92, 116);
        lblSecFile.Location = new Point(3, 158);
        lblSecFile.Name = "lblSecFile";
        lblSecFile.Padding = new Padding(14, 8, 0, 0);
        lblSecFile.Size = new Size(189, 30);
        lblSecFile.TabIndex = 7;
        lblSecFile.Text = "FILE";
        // 
        // btnOpenSave
        // 
        btnOpenSave.BackColor = Color.FromArgb(28, 28, 42);
        btnOpenSave.BorderColor = Color.FromArgb(28, 28, 42);
        btnOpenSave.Cursor = Cursors.Hand;
        btnOpenSave.Dock = DockStyle.Fill;
        btnOpenSave.EnteredBorderColor = Color.FromArgb(40, 40, 60);
        btnOpenSave.EnteredColor = Color.FromArgb(40, 40, 60);
        btnOpenSave.Font = new Font("Segoe UI", 9.5F);
        btnOpenSave.Image = null;
        btnOpenSave.ImageAlign = ContentAlignment.MiddleLeft;
        btnOpenSave.InactiveColor = Color.FromArgb(28, 28, 42);
        btnOpenSave.Location = new Point(3, 191);
        btnOpenSave.Name = "btnOpenSave";
        btnOpenSave.PressedBorderColor = Color.FromArgb(206, 68, 68);
        btnOpenSave.PressedColor = Color.FromArgb(206, 68, 68);
        btnOpenSave.Size = new Size(189, 24);
        btnOpenSave.TabIndex = 8;
        btnOpenSave.TabStop = false;
        btnOpenSave.Text = "▤    Open save";
        btnOpenSave.TextAlignment = StringAlignment.Near;
        btnOpenSave.Click += btnOpenSave_Click;
        // 
        // btnSaveFile
        // 
        btnSaveFile.BackColor = Color.FromArgb(28, 28, 42);
        btnSaveFile.BorderColor = Color.FromArgb(28, 28, 42);
        btnSaveFile.Cursor = Cursors.Hand;
        btnSaveFile.Dock = DockStyle.Fill;
        btnSaveFile.EnteredBorderColor = Color.FromArgb(40, 40, 60);
        btnSaveFile.EnteredColor = Color.FromArgb(40, 40, 60);
        btnSaveFile.Font = new Font("Segoe UI", 9.5F);
        btnSaveFile.Image = null;
        btnSaveFile.ImageAlign = ContentAlignment.MiddleLeft;
        btnSaveFile.InactiveColor = Color.FromArgb(28, 28, 42);
        btnSaveFile.Location = new Point(3, 221);
        btnSaveFile.Name = "btnSaveFile";
        btnSaveFile.PressedBorderColor = Color.FromArgb(206, 68, 68);
        btnSaveFile.PressedColor = Color.FromArgb(206, 68, 68);
        btnSaveFile.Size = new Size(189, 24);
        btnSaveFile.TabIndex = 9;
        btnSaveFile.TabStop = false;
        btnSaveFile.Text = "💾    Save file";
        btnSaveFile.TextAlignment = StringAlignment.Near;
        btnSaveFile.Click += btnSaveFile_Click;
        // 
        // pnlLeftDivider
        // 
        pnlLeftDivider.BackColor = Color.FromArgb(70, 46, 112);
        pnlLeftDivider.Dock = DockStyle.Left;
        pnlLeftDivider.Location = new Point(198, 53);
        pnlLeftDivider.Name = "pnlLeftDivider";
        pnlLeftDivider.Size = new Size(1, 625);
        pnlLeftDivider.TabIndex = 2;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(13, 13, 22);
        ClientSize = new Size(1264, 681);
        Controls.Add(tabControl);
        Controls.Add(pnlLeftDivider);
        Controls.Add(pnlTopDivider);
        Controls.Add(pnlSidebar);
        Controls.Add(pnlTopbar);
        Font = new Font("Segoe UI", 9F);
        Icon = (Icon)resources.GetObject("$this.Icon");
        KeyPreview = true;
        MinimumSize = new Size(860, 580);
        Name = "MainForm";
        Padding = new Padding(3, 0, 3, 3);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Yu-Gi-Oh LOTD:LE — Deck Editor";
        Load += MainForm_Load;
        KeyDown += MainForm_KeyDown;
        pnlTopbar.ResumeLayout(false);
        pnlTopbar.PerformLayout();
        tabControl.ResumeLayout(false);
        tpDeckSlots.ResumeLayout(false);
        tpDeckEditor.ResumeLayout(false);
        tpCardSearch.ResumeLayout(false);
        tpSettings.ResumeLayout(false);
        pnlSidebar.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        tblNav.ResumeLayout(false);
        ResumeLayout(false);
    }

    private DeckSlotsPage deckSlotsPage;
    private DeckEditorPage deckEditorPage;
    private CardSearchPage cardSearchPage;
    private SettingsPage settingsPage;
    private ReaLTaiizor.Controls.Button btnSettings;
    private Panel pnlLeftDivider;
}
