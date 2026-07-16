using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;
using Panel = System.Windows.Forms.Panel;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

partial class MainForm
{
    // ── Top bar ──────────────────────────────────────────────────────────────
    private Panel    pnlTopbar     = null!;
    private Panel    pnlTopGlow    = null!;
    private Label    lblAppTitle   = null!;
    private Label    lblSaveFile   = null!;
    private DLButton btnOpenSave   = null!;
    private DLButton btnUndo       = null!;
    private DLButton btnRedo       = null!;
    private DLButton btnSave       = null!;

    // ── Content + bottom tab bar (Duel Links-style; replaces the old left
    // sidebar/drawer entirely) ───────────────────────────────────────────────
    private Panel          pnlContent = null!;
    private DLBottomTabBar bottomNav  = null!;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        pnlTopbar = new Panel();
        lblAppTitle = new Label();
        lblSaveFile = new Label();
        btnUndo = new DLButton();
        btnRedo = new DLButton();
        btnOpenSave = new DLButton();
        btnSave = new DLButton();
        pnlTopGlow = new Panel();
        pnlContent = new Panel();
        settingsPage = new SettingsPage();
        saveEditorPage = new SaveEditorPage();
        cardSearchPage = new CardSearchPage();
        deckEditorPage = new DeckEditorPage();
        deckSlotsPage = new DeckSlotsPage();
        bottomNav = new DLBottomTabBar();
        pnlTopbar.SuspendLayout();
        pnlContent.SuspendLayout();
        SuspendLayout();
        // 
        // pnlTopbar
        // 
        pnlTopbar.BackColor = Color.FromArgb(7, 11, 20);
        pnlTopbar.Controls.Add(lblAppTitle);
        pnlTopbar.Controls.Add(lblSaveFile);
        pnlTopbar.Controls.Add(btnUndo);
        pnlTopbar.Controls.Add(btnRedo);
        pnlTopbar.Controls.Add(btnOpenSave);
        pnlTopbar.Controls.Add(btnSave);
        pnlTopbar.Dock = DockStyle.Top;
        pnlTopbar.Location = new Point(0, 0);
        pnlTopbar.Name = "pnlTopbar";
        pnlTopbar.Size = new Size(1264, 56);
        pnlTopbar.TabIndex = 2;
        // 
        // lblAppTitle
        // 
        lblAppTitle.AutoSize = true;
        lblAppTitle.BackColor = Color.Transparent;
        lblAppTitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        lblAppTitle.ForeColor = Color.FromArgb(240, 246, 250);
        lblAppTitle.Location = new Point(16, 8);
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
        lblSaveFile.ForeColor = Color.FromArgb(78, 100, 130);
        lblSaveFile.Location = new Point(16, 30);
        lblSaveFile.Name = "lblSaveFile";
        lblSaveFile.Size = new Size(86, 13);
        lblSaveFile.TabIndex = 2;
        lblSaveFile.Text = "No save loaded";
        // 
        // btnUndo
        // 
        btnUndo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnUndo.BackColor = Color.Transparent;
        btnUndo.Enabled = false;
        btnUndo.Font = new Font("Segoe UI", 11F);
        btnUndo.ForeColor = Color.White;
        btnUndo.Location = new Point(946, 12);
        btnUndo.Name = "btnUndo";
        btnUndo.Size = new Size(38, 32);
        btnUndo.Slanted = false;
        btnUndo.TabIndex = 4;
        btnUndo.Text = "↶";
        btnUndo.Variant = DLButtonVariant.Secondary;
        btnUndo.Click += btnUndo_Click;
        // 
        // btnRedo
        // 
        btnRedo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnRedo.BackColor = Color.Transparent;
        btnRedo.Enabled = false;
        btnRedo.Font = new Font("Segoe UI", 11F);
        btnRedo.ForeColor = Color.White;
        btnRedo.Location = new Point(992, 12);
        btnRedo.Name = "btnRedo";
        btnRedo.Size = new Size(38, 32);
        btnRedo.Slanted = false;
        btnRedo.TabIndex = 5;
        btnRedo.Text = "↷";
        btnRedo.Variant = DLButtonVariant.Secondary;
        btnRedo.Click += btnRedo_Click;
        // 
        // btnOpenSave
        // 
        btnOpenSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnOpenSave.BackColor = Color.Transparent;
        btnOpenSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnOpenSave.ForeColor = Color.White;
        btnOpenSave.Location = new Point(1040, 12);
        btnOpenSave.Name = "btnOpenSave";
        btnOpenSave.Size = new Size(100, 32);
        btnOpenSave.Slanted = false;
        btnOpenSave.TabIndex = 3;
        btnOpenSave.Text = "Open Save";
        btnOpenSave.Variant = DLButtonVariant.Secondary;
        btnOpenSave.Click += btnOpenSave_Click;
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.BackColor = Color.Transparent;
        btnSave.Enabled = false;
        btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(1150, 12);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(98, 32);
        btnSave.Slanted = false;
        btnSave.TabIndex = 6;
        btnSave.Text = "Save";
        btnSave.Variant = DLButtonVariant.Primary;
        btnSave.Click += btnSave_Click;
        // 
        // pnlTopGlow
        // 
        pnlTopGlow.BackColor = Color.FromArgb(42, 188, 220);
        pnlTopGlow.Dock = DockStyle.Top;
        pnlTopGlow.Location = new Point(0, 56);
        pnlTopGlow.Name = "pnlTopGlow";
        pnlTopGlow.Size = new Size(1264, 2);
        pnlTopGlow.TabIndex = 1;
        // 
        // pnlContent
        // 
        pnlContent.BackColor = Color.FromArgb(7, 12, 22);
        pnlContent.Controls.Add(settingsPage);
        pnlContent.Controls.Add(saveEditorPage);
        pnlContent.Controls.Add(cardSearchPage);
        pnlContent.Controls.Add(deckEditorPage);
        pnlContent.Controls.Add(deckSlotsPage);
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.Location = new Point(0, 58);
        pnlContent.Name = "pnlContent";
        pnlContent.Size = new Size(1264, 451);
        pnlContent.TabIndex = 0;
        // 
        // settingsPage
        // 
        settingsPage.BackColor = Color.FromArgb(7, 12, 22);
        settingsPage.Dock = DockStyle.Fill;
        settingsPage.Location = new Point(0, 0);
        settingsPage.Name = "settingsPage";
        settingsPage.Size = new Size(1264, 451);
        settingsPage.TabIndex = 4;
        // 
        // saveEditorPage
        // 
        saveEditorPage.BackColor = Color.FromArgb(7, 12, 22);
        saveEditorPage.Dock = DockStyle.Fill;
        saveEditorPage.Location = new Point(0, 0);
        saveEditorPage.Name = "saveEditorPage";
        saveEditorPage.Size = new Size(1264, 451);
        saveEditorPage.TabIndex = 3;
        // 
        // cardSearchPage
        // 
        cardSearchPage.BackColor = Color.FromArgb(7, 12, 22);
        cardSearchPage.Dock = DockStyle.Fill;
        cardSearchPage.IsOnline = true;
        cardSearchPage.Location = new Point(0, 0);
        cardSearchPage.Name = "cardSearchPage";
        cardSearchPage.Size = new Size(1264, 451);
        cardSearchPage.TabIndex = 2;
        // 
        // deckEditorPage
        // 
        deckEditorPage.BackColor = Color.FromArgb(7, 12, 22);
        deckEditorPage.Dock = DockStyle.Fill;
        deckEditorPage.IsOnline = true;
        deckEditorPage.Location = new Point(0, 0);
        deckEditorPage.Name = "deckEditorPage";
        deckEditorPage.Size = new Size(1264, 451);
        deckEditorPage.TabIndex = 1;
        // 
        // deckSlotsPage
        // 
        deckSlotsPage.BackColor = Color.FromArgb(7, 12, 22);
        deckSlotsPage.Dock = DockStyle.Fill;
        deckSlotsPage.Location = new Point(0, 0);
        deckSlotsPage.Name = "deckSlotsPage";
        deckSlotsPage.Size = new Size(1264, 451);
        deckSlotsPage.TabIndex = 0;
        // 
        // bottomNav
        // 
        bottomNav.Dock = DockStyle.Bottom;
        bottomNav.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        bottomNav.Location = new Point(0, 509);
        bottomNav.Name = "bottomNav";
        bottomNav.SelectedIndex = 0;
        bottomNav.Size = new Size(1264, 172);
        bottomNav.TabIndex = 1;
        bottomNav.SelectedIndexChanged += BottomNav_SelectedIndexChanged;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(7, 12, 22);
        ClientSize = new Size(1264, 681);
        Controls.Add(pnlContent);
        Controls.Add(bottomNav);
        Controls.Add(pnlTopGlow);
        Controls.Add(pnlTopbar);
        Font = new Font("Segoe UI", 9F);
        Icon = (Icon)resources.GetObject("$this.Icon");
        KeyPreview = true;
        MinimumSize = new Size(860, 580);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Yu-Gi-Oh LOTD:LE — Deck Editor";
        Load += MainForm_Load;
        KeyDown += MainForm_KeyDown;
        pnlTopbar.ResumeLayout(false);
        pnlTopbar.PerformLayout();
        pnlContent.ResumeLayout(false);
        ResumeLayout(false);
    }

    private DeckSlotsPage deckSlotsPage;
    private DeckEditorPage deckEditorPage;
    private CardSearchPage cardSearchPage;
    private SaveEditorPage saveEditorPage;
    private SettingsPage settingsPage;
}
