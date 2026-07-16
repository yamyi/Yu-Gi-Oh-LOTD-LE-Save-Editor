namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

partial class AppMessageBox
{
    private Panel pnlIcon = null!;
    private Label lblIconGlyph = null!;
    private Label lblMessage = null!;
    private Panel pnlButtons = null!;
    private FlowLayoutPanel flowButtons = null!;
    private Button btnPrimary = null!;
    private Button btnSecondary = null!;
    private Button btnTertiary = null!;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppMessageBox));
        pnlIcon = new Panel();
        lblIconGlyph = new Label();
        lblMessage = new Label();
        pnlButtons = new Panel();
        flowButtons = new FlowLayoutPanel();
        btnPrimary = new Button();
        btnSecondary = new Button();
        btnTertiary = new Button();
        pnlIcon.SuspendLayout();
        pnlButtons.SuspendLayout();
        flowButtons.SuspendLayout();
        SuspendLayout();
        // 
        // pnlIcon
        // 
        pnlIcon.BackColor = Color.FromArgb(24, 24, 36);
        pnlIcon.Controls.Add(lblIconGlyph);
        pnlIcon.Location = new Point(24, 24);
        pnlIcon.Name = "pnlIcon";
        pnlIcon.Size = new Size(48, 48);
        pnlIcon.TabIndex = 0;
        // 
        // lblIconGlyph
        // 
        lblIconGlyph.Dock = DockStyle.Fill;
        lblIconGlyph.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblIconGlyph.ForeColor = Color.FromArgb(220, 184, 84);
        lblIconGlyph.Location = new Point(0, 0);
        lblIconGlyph.Name = "lblIconGlyph";
        lblIconGlyph.Size = new Size(48, 48);
        lblIconGlyph.TabIndex = 0;
        lblIconGlyph.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblMessage
        // 
        lblMessage.BackColor = Color.Transparent;
        lblMessage.Font = new Font("Segoe UI", 9.5F);
        lblMessage.ForeColor = Color.FromArgb(238, 238, 250);
        lblMessage.Location = new Point(90, 24);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(290, 86);
        lblMessage.TabIndex = 1;
        // 
        // pnlButtons
        // 
        pnlButtons.BackColor = Color.FromArgb(22, 22, 34);
        pnlButtons.Controls.Add(flowButtons);
        pnlButtons.Dock = DockStyle.Bottom;
        pnlButtons.Location = new Point(0, 122);
        pnlButtons.Name = "pnlButtons";
        pnlButtons.Size = new Size(400, 56);
        pnlButtons.TabIndex = 2;
        // 
        // flowButtons
        // 
        flowButtons.Controls.Add(btnPrimary);
        flowButtons.Controls.Add(btnSecondary);
        flowButtons.Controls.Add(btnTertiary);
        flowButtons.Dock = DockStyle.Right;
        flowButtons.FlowDirection = FlowDirection.RightToLeft;
        flowButtons.Location = new Point(94, 0);
        flowButtons.Name = "flowButtons";
        flowButtons.Padding = new Padding(0, 13, 16, 0);
        flowButtons.Size = new Size(306, 56);
        flowButtons.TabIndex = 0;
        flowButtons.WrapContents = false;
        // 
        // btnPrimary
        // 
        btnPrimary.BackColor = Color.FromArgb(216, 178, 60);
        btnPrimary.FlatAppearance.BorderSize = 0;
        btnPrimary.FlatStyle = FlatStyle.Flat;
        btnPrimary.ForeColor = Color.FromArgb(18, 18, 28);
        btnPrimary.Location = new Point(200, 13);
        btnPrimary.Margin = new Padding(8, 0, 0, 0);
        btnPrimary.Name = "btnPrimary";
        btnPrimary.Size = new Size(90, 30);
        btnPrimary.TabIndex = 0;
        btnPrimary.UseVisualStyleBackColor = false;
        // 
        // btnSecondary
        // 
        btnSecondary.BackColor = Color.Transparent;
        btnSecondary.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnSecondary.FlatStyle = FlatStyle.Flat;
        btnSecondary.ForeColor = Color.FromArgb(190, 190, 212);
        btnSecondary.Location = new Point(102, 13);
        btnSecondary.Margin = new Padding(8, 0, 0, 0);
        btnSecondary.Name = "btnSecondary";
        btnSecondary.Size = new Size(90, 30);
        btnSecondary.TabIndex = 1;
        btnSecondary.UseVisualStyleBackColor = false;
        // 
        // btnTertiary
        // 
        btnTertiary.BackColor = Color.Transparent;
        btnTertiary.FlatAppearance.BorderColor = Color.FromArgb(60, 60, 86);
        btnTertiary.FlatStyle = FlatStyle.Flat;
        btnTertiary.ForeColor = Color.FromArgb(190, 190, 212);
        btnTertiary.Location = new Point(4, 13);
        btnTertiary.Margin = new Padding(8, 0, 0, 0);
        btnTertiary.Name = "btnTertiary";
        btnTertiary.Size = new Size(90, 30);
        btnTertiary.TabIndex = 2;
        btnTertiary.UseVisualStyleBackColor = false;
        // 
        // AppMessageBox
        // 
        BackColor = Color.FromArgb(13, 13, 22);
        ClientSize = new Size(400, 178);
        Controls.Add(lblMessage);
        Controls.Add(pnlIcon);
        Controls.Add(pnlButtons);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AppMessageBox";
        ShowIcon = false;
        ShowInTaskbar = false;
        SizeGripStyle = SizeGripStyle.Hide;
        pnlIcon.ResumeLayout(false);
        pnlButtons.ResumeLayout(false);
        flowButtons.ResumeLayout(false);
        ResumeLayout(false);
    }
}
