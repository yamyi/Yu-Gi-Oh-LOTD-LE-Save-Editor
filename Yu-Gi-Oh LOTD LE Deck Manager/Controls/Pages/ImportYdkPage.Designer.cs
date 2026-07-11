namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

partial class ImportYdkPage
{
    private Panel           pnlScroll      = null!;
    private Label           lblDrop        = null!;
    private Panel           pnlDropZone    = null!;
    private Label           lblDropIcon    = null!;
    private Label           lblDropTitle   = null!;
    private Label           lblDropSub     = null!;
    private Label           lblOr          = null!;
    private Panel           pnlPathRow     = null!;
    private TextBox         txtPath        = null!;
    private Button          btnBrowse      = null!;
    private Label           lblPickerTitle = null!;
    private FlowLayoutPanel pnlSlotPicker  = null!;
    private Button          btnImport      = null!;

    private void InitializeComponent()
    {
        pnlScroll = new Panel();
        btnImport = new Button();
        pnlSlotPicker = new FlowLayoutPanel();
        lblPickerTitle = new Label();
        pnlPathRow = new Panel();
        txtPath = new TextBox();
        btnBrowse = new Button();
        lblOr = new Label();
        pnlDropZone = new Panel();
        lblDropSub = new Label();
        lblDropTitle = new Label();
        lblDropIcon = new Label();
        lblDrop = new Label();
        pnlScroll.SuspendLayout();
        pnlPathRow.SuspendLayout();
        pnlDropZone.SuspendLayout();
        SuspendLayout();
        // 
        // pnlScroll
        // 
        pnlScroll.AutoScroll = true;
        pnlScroll.BackColor = Color.FromArgb(15, 15, 26);
        pnlScroll.Controls.Add(btnImport);
        pnlScroll.Controls.Add(pnlSlotPicker);
        pnlScroll.Controls.Add(lblPickerTitle);
        pnlScroll.Controls.Add(pnlPathRow);
        pnlScroll.Controls.Add(lblOr);
        pnlScroll.Controls.Add(pnlDropZone);
        pnlScroll.Dock = DockStyle.Fill;
        pnlScroll.Location = new Point(0, 0);
        pnlScroll.Name = "pnlScroll";
        pnlScroll.Padding = new Padding(20, 16, 20, 16);
        pnlScroll.Size = new Size(1177, 595);
        pnlScroll.TabIndex = 0;
        // 
        // btnImport
        // 
        btnImport.BackColor = Color.FromArgb(201, 162, 39);
        btnImport.Cursor = Cursors.Hand;
        btnImport.FlatAppearance.BorderColor = Color.FromArgb(201, 162, 39);
        btnImport.FlatAppearance.MouseOverBackColor = Color.FromArgb(218, 179, 56);
        btnImport.FlatStyle = FlatStyle.Flat;
        btnImport.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnImport.ForeColor = Color.FromArgb(15, 15, 26);
        btnImport.Location = new Point(0, 0);
        btnImport.Name = "btnImport";
        btnImport.Size = new Size(200, 34);
        btnImport.TabIndex = 0;
        btnImport.Text = "Import into selected slot";
        btnImport.UseVisualStyleBackColor = false;
        // 
        // pnlSlotPicker
        // 
        pnlSlotPicker.BackColor = Color.Transparent;
        pnlSlotPicker.Dock = DockStyle.Top;
        pnlSlotPicker.Location = new Point(20, 204);
        pnlSlotPicker.Name = "pnlSlotPicker";
        pnlSlotPicker.Size = new Size(1137, 100);
        pnlSlotPicker.TabIndex = 1;
        // 
        // lblPickerTitle
        // 
        lblPickerTitle.BackColor = Color.Transparent;
        lblPickerTitle.Dock = DockStyle.Top;
        lblPickerTitle.Font = new Font("Segoe UI", 8.5F);
        lblPickerTitle.ForeColor = Color.FromArgb(90, 90, 122);
        lblPickerTitle.Location = new Point(20, 180);
        lblPickerTitle.Name = "lblPickerTitle";
        lblPickerTitle.Size = new Size(1137, 24);
        lblPickerTitle.TabIndex = 2;
        lblPickerTitle.Text = "Target slot — click to select";
        // 
        // pnlPathRow
        // 
        pnlPathRow.BackColor = Color.Transparent;
        pnlPathRow.Controls.Add(txtPath);
        pnlPathRow.Controls.Add(btnBrowse);
        pnlPathRow.Dock = DockStyle.Top;
        pnlPathRow.Location = new Point(20, 150);
        pnlPathRow.Name = "pnlPathRow";
        pnlPathRow.Size = new Size(1137, 30);
        pnlPathRow.TabIndex = 3;
        // 
        // txtPath
        // 
        txtPath.BackColor = Color.FromArgb(19, 19, 31);
        txtPath.BorderStyle = BorderStyle.FixedSingle;
        txtPath.Dock = DockStyle.Fill;
        txtPath.Font = new Font("Segoe UI", 9F);
        txtPath.ForeColor = Color.FromArgb(192, 192, 216);
        txtPath.Location = new Point(0, 0);
        txtPath.Name = "txtPath";
        txtPath.PlaceholderText = "C:\\Users\\…\\deck.ydk";
        txtPath.Size = new Size(1063, 23);
        txtPath.TabIndex = 0;
        // 
        // btnBrowse
        // 
        btnBrowse.BackColor = Color.FromArgb(19, 19, 31);
        btnBrowse.Cursor = Cursors.Hand;
        btnBrowse.Dock = DockStyle.Right;
        btnBrowse.FlatAppearance.BorderColor = Color.FromArgb(42, 42, 62);
        btnBrowse.FlatAppearance.MouseOverBackColor = Color.FromArgb(26, 26, 46);
        btnBrowse.FlatStyle = FlatStyle.Flat;
        btnBrowse.Font = new Font("Segoe UI", 8.5F);
        btnBrowse.ForeColor = Color.FromArgb(90, 90, 122);
        btnBrowse.Location = new Point(1063, 0);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(74, 30);
        btnBrowse.TabIndex = 1;
        btnBrowse.Text = "Browse";
        btnBrowse.UseVisualStyleBackColor = false;
        // 
        // lblOr
        // 
        lblOr.BackColor = Color.Transparent;
        lblOr.Dock = DockStyle.Top;
        lblOr.Font = new Font("Segoe UI", 8.5F);
        lblOr.ForeColor = Color.FromArgb(58, 58, 90);
        lblOr.Location = new Point(20, 126);
        lblOr.Name = "lblOr";
        lblOr.Size = new Size(1137, 24);
        lblOr.TabIndex = 4;
        lblOr.Text = "— or paste a file path —";
        lblOr.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // pnlDropZone
        // 
        pnlDropZone.BackColor = Color.FromArgb(15, 15, 26);
        pnlDropZone.Controls.Add(lblDropSub);
        pnlDropZone.Controls.Add(lblDropTitle);
        pnlDropZone.Controls.Add(lblDropIcon);
        pnlDropZone.Dock = DockStyle.Top;
        pnlDropZone.Location = new Point(20, 16);
        pnlDropZone.Margin = new Padding(0, 0, 0, 8);
        pnlDropZone.Name = "pnlDropZone";
        pnlDropZone.Padding = new Padding(0, 8, 0, 0);
        pnlDropZone.Size = new Size(1137, 110);
        pnlDropZone.TabIndex = 5;
        // 
        // lblDropSub
        // 
        lblDropSub.BackColor = Color.Transparent;
        lblDropSub.Dock = DockStyle.Top;
        lblDropSub.Font = new Font("Segoe UI", 9F);
        lblDropSub.ForeColor = Color.FromArgb(58, 58, 90);
        lblDropSub.Location = new Point(0, 80);
        lblDropSub.Name = "lblDropSub";
        lblDropSub.Size = new Size(1137, 20);
        lblDropSub.TabIndex = 0;
        lblDropSub.Text = "or browse for a file below";
        lblDropSub.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDropTitle
        // 
        lblDropTitle.BackColor = Color.Transparent;
        lblDropTitle.Dock = DockStyle.Top;
        lblDropTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblDropTitle.ForeColor = Color.FromArgb(90, 90, 122);
        lblDropTitle.Location = new Point(0, 56);
        lblDropTitle.Name = "lblDropTitle";
        lblDropTitle.Size = new Size(1137, 24);
        lblDropTitle.TabIndex = 1;
        lblDropTitle.Text = "Drop a .ydk file here";
        lblDropTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDropIcon
        // 
        lblDropIcon.BackColor = Color.Transparent;
        lblDropIcon.Dock = DockStyle.Top;
        lblDropIcon.Font = new Font("Segoe UI", 28F);
        lblDropIcon.ForeColor = Color.FromArgb(58, 58, 90);
        lblDropIcon.Location = new Point(0, 8);
        lblDropIcon.Name = "lblDropIcon";
        lblDropIcon.Size = new Size(1137, 48);
        lblDropIcon.TabIndex = 2;
        lblDropIcon.Text = "⬆";
        lblDropIcon.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDrop
        // 
        lblDrop.Location = new Point(0, 0);
        lblDrop.Name = "lblDrop";
        lblDrop.Size = new Size(100, 23);
        lblDrop.TabIndex = 0;
        // 
        // ImportYdkPage
        // 
        BackColor = Color.FromArgb(15, 15, 26);
        Controls.Add(pnlScroll);
        Name = "ImportYdkPage";
        Size = new Size(1177, 595);
        pnlScroll.ResumeLayout(false);
        pnlPathRow.ResumeLayout(false);
        pnlPathRow.PerformLayout();
        pnlDropZone.ResumeLayout(false);
        ResumeLayout(false);
    }

    private void InitializeDropZonePainting()
    {
        pnlDropZone.Paint += PnlDropZone_Paint;
    }

    private void PnlDropZone_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

        using var pen = new Pen(AppColors.Border, 1f)
        {
            DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
        };

        var rect = new Rectangle(
            0, 0,
            pnlDropZone.ClientSize.Width - 1,
            pnlDropZone.ClientSize.Height - 1
        );

        e.Graphics.DrawRectangle(pen, rect);
    }
}
