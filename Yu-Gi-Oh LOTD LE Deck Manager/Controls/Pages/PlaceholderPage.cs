namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Controls.Pages;

/// <summary>
/// Generic placeholder for tabs not yet implemented.
/// Shows a centred icon, title and subtitle.
/// </summary>
public sealed class PlaceholderPage : UserControl
{
    public PlaceholderPage(string icon, string title, string subtitle)
    {
        Dock      = DockStyle.Fill;
        BackColor = AppColors.AppBg;

        var pnl = new TableLayoutPanel
        {
            Dock        = DockStyle.Fill,
            ColumnCount = 1,
            RowCount    = 3,
            BackColor   = Color.Transparent,
        };
        pnl.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        pnl.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        pnl.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        var iconLbl = new Label
        {
            Text      = icon,
            Font      = new Font("Segoe UI", 32F),
            ForeColor = AppColors.TextDim,
            TextAlign = ContentAlignment.MiddleCenter,
            Dock      = DockStyle.Fill,
            AutoSize  = false,
            BackColor = Color.Transparent,
        };

        var inner = new Panel { BackColor = Color.Transparent, Dock = DockStyle.Fill, AutoSize = true };

        var titleLbl = new Label
        {
            Text      = title,
            Font      = new Font("Segoe UI", 13F, FontStyle.Bold),
            ForeColor = AppColors.TextMuted,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize  = false,
            Width     = 400,
            Height    = 28,
            BackColor = Color.Transparent,
        };
        var subLbl = new Label
        {
            Text      = subtitle,
            Font      = new Font("Segoe UI", 9.5F),
            ForeColor = AppColors.TextDim,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoSize  = false,
            Width     = 400,
            Height    = 20,
            BackColor = Color.Transparent,
        };
        inner.Controls.Add(subLbl);
        inner.Controls.Add(titleLbl);

        pnl.Controls.Add(new Panel { BackColor = Color.Transparent }, 0, 0);
        pnl.Controls.Add(inner, 0, 1);
        pnl.Controls.Add(new Panel { BackColor = Color.Transparent }, 0, 2);
        Controls.Add(pnl);

        // Centre icon in the top spacer cell at runtime
        Load += (_, _) =>
        {
            iconLbl.Size     = new Size(80, 80);
            iconLbl.Location = new Point((pnl.Width - 80) / 2, (pnl.Height / 2) - 100);
            Controls.Add(iconLbl);
            titleLbl.Left = subLbl.Left = (pnl.Width - 400) / 2;
        };
    }
}
