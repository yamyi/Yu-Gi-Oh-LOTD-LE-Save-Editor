using System.Xml.Linq;
using static ReaLTaiizor.Drawing.Poison.PoisonPaint;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls;

partial class CardSlot
{
    // ── Named controls ─────────────────────────────────────────────────────────
    // Empty state
    private Panel pnlEmpty;
    private Label lblEmptyIcon;
    // Online state
    private PictureBox picCard;
    // Offline state
    private Panel pnlOffline;
    private Label lblName;
    private Label lblType;
    private Label lblStats;

    private void InitializeComponent()
    {
        pnlEmpty = new Panel();
        lblEmptyIcon = new Label();
        picCard = new PictureBox();
        pnlOffline = new Panel();
        lblName = new Label();
        lblType = new Label();
        lblStats = new Label();

        pnlEmpty.SuspendLayout();
        pnlOffline.SuspendLayout();
        SuspendLayout();

        // ═════════════════════════════════════════════════════════════════════
        // pnlEmpty  — shown when no card assigned
        // ═════════════════════════════════════════════════════════════════════
        pnlEmpty.Name = "pnlEmpty";
        pnlEmpty.Location = new Point(0, 0);
        pnlEmpty.Size = new Size(60, 88);
        pnlEmpty.BackColor = AppColors.CardBg;
        pnlEmpty.Cursor = Cursors.Hand;
        pnlEmpty.AllowDrop = true;

        pnlEmpty.DragEnter += CardSlot_DragEnter;
        pnlEmpty.DragDrop += CardSlot_DragDrop;

        //   lblEmptyIcon — centred "+" glyph
        lblEmptyIcon.Name = "lblEmptyIcon";
        lblEmptyIcon.Text = "+";
        lblEmptyIcon.Font = new Font("Segoe UI", 14f, FontStyle.Regular);
        lblEmptyIcon.ForeColor = AppColors.TextDim;
        lblEmptyIcon.BackColor = Color.Transparent;
        lblEmptyIcon.Location = new Point(0, 0);
        lblEmptyIcon.Size = new Size(60, 88);
        lblEmptyIcon.TextAlign = ContentAlignment.MiddleCenter;
        lblEmptyIcon.AutoSize = false;
        lblEmptyIcon.AllowDrop = true;

        lblEmptyIcon.DragEnter += CardSlot_DragEnter;
        lblEmptyIcon.DragDrop += CardSlot_DragDrop;

        pnlEmpty.Controls.Add(lblEmptyIcon);

        // ═════════════════════════════════════════════════════════════════════
        // picCard  — shown when online + card assigned
        // ═════════════════════════════════════════════════════════════════════
        picCard.Name = "picCard";
        picCard.Location = new Point(0, 0);
        picCard.Size = new Size(60, 88);
        picCard.SizeMode = PictureBoxSizeMode.StretchImage;
        picCard.BackColor = AppColors.CardBg;
        picCard.Cursor = Cursors.Hand;
        picCard.Visible = false;
        picCard.AllowDrop = true;

        picCard.MouseEnter += CardSlot_MouseEnter;
        picCard.MouseLeave += CardSlot_MouseLeave;
        picCard.MouseClick += CardSlot_MouseClick;
        picCard.MouseDoubleClick += CardSlot_MouseDoubleClick;
        picCard.MouseDown += CardSlot_MouseDown;
        picCard.MouseMove += CardSlot_MouseMove;
        picCard.DragEnter += CardSlot_DragEnter;
        picCard.DragDrop += CardSlot_DragDrop;

        // ═════════════════════════════════════════════════════════════════════
        // pnlOffline  — shown when offline + card assigned
        //
        // Layout inside (60×88):
        //   lblName   y=4,  h=28  (2 lines, bold, wraps)
        //   lblType   y=34, h=22  (1 line, dim)
        //   lblStats  y=62, h=22  (ATK/DEF, small, bottom)
        // ═════════════════════════════════════════════════════════════════════
        pnlOffline.Name = "pnlOffline";
        pnlOffline.Location = new Point(0, 0);
        pnlOffline.Size = new Size(60, 88);
        pnlOffline.BackColor = AppColors.OfflinePanelBg;
        pnlOffline.Cursor = Cursors.Hand;
        pnlOffline.Visible = false;
        pnlOffline.AllowDrop = true;

        pnlOffline.MouseEnter += CardSlot_MouseEnter;
        pnlOffline.MouseLeave += CardSlot_MouseLeave;
        pnlOffline.MouseClick += CardSlot_MouseClick;
        pnlOffline.MouseDoubleClick += CardSlot_MouseDoubleClick;
        pnlOffline.MouseDown += CardSlot_MouseDown;
        pnlOffline.MouseMove += CardSlot_MouseMove;
        pnlOffline.DragEnter += CardSlot_DragEnter;
        pnlOffline.DragDrop += CardSlot_DragDrop;

        //   lblName  — card name, 2 lines max, bold, wraps
        lblName.Name = "lblName";
        lblName.Text = string.Empty;
        lblName.Font = new Font("Segoe UI", 5.5f, FontStyle.Bold);
        lblName.ForeColor = AppColors.TextCardName;
        lblName.BackColor = Color.Transparent;
        lblName.Location = new Point(3, 4);
        lblName.Size = new Size(54, 28);
        lblName.AutoSize = false;
        lblName.AutoEllipsis = true;
        lblName.AllowDrop = true;

        lblName.MouseEnter += CardSlot_MouseEnter;
        lblName.MouseLeave += CardSlot_MouseLeave;
        lblName.MouseClick += CardSlot_MouseClick;
        lblName.MouseDoubleClick += CardSlot_MouseDoubleClick;
        lblName.MouseDown += CardSlot_MouseDown;
        lblName.MouseMove += CardSlot_MouseMove;
        lblName.DragEnter += CardSlot_DragEnter;
        lblName.DragDrop += CardSlot_DragDrop;

        //   lblType  — human-readable card type, dim
        lblType.Name = "lblType";
        lblType.Text = string.Empty;
        lblType.Font = new Font("Segoe UI", 5f);
        lblType.ForeColor = AppColors.TextInfo;
        lblType.BackColor = Color.Transparent;
        lblType.Location = new Point(3, 34);
        lblType.Size = new Size(54, 22);
        lblType.AutoSize = false;
        lblType.AutoEllipsis = true;
        lblType.AllowDrop = true;

        lblType.MouseEnter += CardSlot_MouseEnter;
        lblType.MouseLeave += CardSlot_MouseLeave;
        lblType.MouseClick += CardSlot_MouseClick;
        lblType.MouseDoubleClick += CardSlot_MouseDoubleClick;
        lblType.MouseDown += CardSlot_MouseDown;
        lblType.MouseMove += CardSlot_MouseMove;
        lblType.DragEnter += CardSlot_DragEnter;
        lblType.DragDrop += CardSlot_DragDrop;

        //   lblStats  — ATK / DEF, anchored to bottom
        lblStats.Name = "lblStats";
        lblStats.Text = string.Empty;
        lblStats.Font = new Font("Segoe UI", 4.5f);
        lblStats.ForeColor = AppColors.StatMuted;
        lblStats.BackColor = Color.Transparent;
        lblStats.Location = new Point(3, 64);
        lblStats.Size = new Size(54, 20);
        lblStats.AutoSize = false;
        lblStats.Visible = false;
        lblStats.AllowDrop = true;

        lblStats.MouseEnter += CardSlot_MouseEnter;
        lblStats.MouseLeave += CardSlot_MouseLeave;
        lblStats.MouseClick += CardSlot_MouseClick;
        lblStats.MouseDoubleClick += CardSlot_MouseDoubleClick;
        lblStats.MouseDown += CardSlot_MouseDown;
        lblStats.MouseMove += CardSlot_MouseMove;
        lblStats.DragEnter += CardSlot_DragEnter;
        lblStats.DragDrop += CardSlot_DragDrop;

        pnlOffline.Controls.Add(lblName);
        pnlOffline.Controls.Add(lblType);
        pnlOffline.Controls.Add(lblStats);

        // ═════════════════════════════════════════════════════════════════════
        // CardSlot (UserControl root)  60×88
        // ═════════════════════════════════════════════════════════════════════
        Name = "CardSlot";
        Size = new Size(60, 88);
        BackColor = AppColors.CardBg;
        Cursor = Cursors.Hand;
        AutoScaleMode = AutoScaleMode.None;   // fixed-size control — no scaling
        AllowDrop = true;

        MouseEnter += CardSlot_MouseEnter;
        MouseLeave += CardSlot_MouseLeave;
        MouseClick += CardSlot_MouseClick;
        MouseDoubleClick += CardSlot_MouseDoubleClick;
        MouseDown += CardSlot_MouseDown;
        MouseMove += CardSlot_MouseMove;
        DragEnter += CardSlot_DragEnter;
        DragDrop += CardSlot_DragDrop;

        Controls.Add(pnlEmpty);
        Controls.Add(picCard);
        Controls.Add(pnlOffline);

        pnlEmpty.ResumeLayout(false);
        pnlOffline.ResumeLayout(false);
        ResumeLayout(false);
    }
}