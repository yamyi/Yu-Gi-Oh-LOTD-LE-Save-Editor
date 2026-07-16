namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

partial class OwnerPickerForm
{
    private TextBox txtSearch = null!;
    private ListBox lstOwners = null!;
    private Button btnOk = null!;
    private Button btnCancel = null!;

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OwnerPickerForm));
        txtSearch = new TextBox();
        lstOwners = new ListBox();
        btnOk = new Button();
        btnCancel = new Button();
        pictureBox1 = new PictureBox();
        lblOwnerId = new Label();
        lblOwnerName = new Label();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        SuspendLayout();
        // 
        // txtSearch
        // 
        txtSearch.BackColor = Color.FromArgb(13, 13, 22);
        txtSearch.BorderStyle = BorderStyle.FixedSingle;
        txtSearch.ForeColor = Color.FromArgb(238, 238, 250);
        txtSearch.Location = new Point(146, 105);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Search duelist…";
        txtSearch.Size = new Size(315, 23);
        txtSearch.TabIndex = 0;
        txtSearch.TextChanged += txtSearch_TextChanged;
        // 
        // lstOwners
        // 
        lstOwners.BackColor = Color.FromArgb(13, 13, 22);
        lstOwners.BorderStyle = BorderStyle.FixedSingle;
        lstOwners.ForeColor = Color.FromArgb(238, 238, 250);
        lstOwners.ItemHeight = 15;
        lstOwners.Location = new Point(12, 146);
        lstOwners.Name = "lstOwners";
        lstOwners.Size = new Size(449, 287);
        lstOwners.TabIndex = 1;
        lstOwners.SelectedIndexChanged += lstOwners_SelectedIndexChanged;
        lstOwners.DoubleClick += lstOwners_DoubleClick;
        // 
        // btnOk
        // 
        btnOk.BackColor = Color.FromArgb(216, 178, 60);
        btnOk.DialogResult = DialogResult.OK;
        btnOk.FlatAppearance.BorderSize = 0;
        btnOk.FlatStyle = FlatStyle.Flat;
        btnOk.ForeColor = Color.FromArgb(18, 18, 28);
        btnOk.Location = new Point(381, 439);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(80, 30);
        btnOk.TabIndex = 2;
        btnOk.Text = "OK";
        btnOk.UseVisualStyleBackColor = false;
        // 
        // btnCancel
        // 
        btnCancel.BackColor = Color.Transparent;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.ForeColor = Color.FromArgb(138, 138, 164);
        btnCancel.Location = new Point(12, 439);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(80, 30);
        btnCancel.TabIndex = 3;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = false;
        // 
        // pictureBox1
        // 
        pictureBox1.BorderStyle = BorderStyle.FixedSingle;
        pictureBox1.Location = new Point(12, 12);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Size = new Size(128, 128);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.TabIndex = 4;
        pictureBox1.TabStop = false;
        // 
        // lblOwnerId
        // 
        lblOwnerId.BackColor = Color.Transparent;
        lblOwnerId.Font = new Font("Segoe UI", 8F);
        lblOwnerId.ForeColor = Color.FromArgb(220, 184, 84);
        lblOwnerId.Location = new Point(146, 78);
        lblOwnerId.Name = "lblOwnerId";
        lblOwnerId.Size = new Size(123, 14);
        lblOwnerId.TabIndex = 2;
        lblOwnerId.Text = "—";
        // 
        // lblOwnerName
        // 
        lblOwnerName.BackColor = Color.Transparent;
        lblOwnerName.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblOwnerName.ForeColor = Color.FromArgb(238, 238, 250);
        lblOwnerName.Location = new Point(146, 9);
        lblOwnerName.Name = "lblOwnerName";
        lblOwnerName.Size = new Size(315, 69);
        lblOwnerName.TabIndex = 1;
        lblOwnerName.Text = "__";
        // 
        // OwnerPickerForm
        // 
        AcceptButton = btnOk;
        BackColor = Color.FromArgb(28, 28, 42);
        CancelButton = btnCancel;
        ClientSize = new Size(473, 481);
        Controls.Add(lblOwnerId);
        Controls.Add(lblOwnerName);
        Controls.Add(pictureBox1);
        Controls.Add(txtSearch);
        Controls.Add(lstOwners);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "OwnerPickerForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Change Duelist";
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private PictureBox pictureBox1;
    private Label lblOwnerId;
    private Label lblOwnerName;
}
