using static Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services.SlotIO;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

/// <summary>Tab page 1 — Import a .ydk file into a slot.</summary>
public sealed partial class ImportYdkPage : UserControl
{
    private int _selectedSlot = -1;
    private List<SlotInfo> _slots = new();

    /// <summary>Raised after a deck is successfully imported, so the host form can
    /// reload all slots (grid + this page's picker) and refresh the dirty flag.</summary>
    public event EventHandler? DeckImported;

    public ImportYdkPage()
    {
        InitializeComponent();
        btnBrowse.Click += new EventHandler(btnBrowse_Click);
        btnImport.Click += new EventHandler(btnImport_Click);
    }

    public void LoadSlots(List<SlotInfo> slots)
    {
        _slots = slots;
        BuildSlotPicker();
    }

    private void BuildSlotPicker()
    {
        pnlSlotPicker.Controls.Clear();
        for (int i = 0; i < _slots.Count; i++)
        {
            int idx = i;
            var btn = new Button
            {
                Text      = $"{i + 1:D2}",
                FlatStyle = FlatStyle.Flat,
                Size      = new Size(38, 28),
                Font      = new Font("Segoe UI", 8F),
                BackColor = _slots[i].IsEmpty ? AppColors.AppBg : AppColors.SidebarBg,
                ForeColor = _slots[i].IsEmpty ? AppColors.TextDim : AppColors.TextMuted,
                Cursor    = Cursors.Hand,
                Margin    = new Padding(3),
                Tag       = idx,
            };
            btn.FlatAppearance.BorderColor = AppColors.Border;
            btn.FlatAppearance.BorderSize  = 1;
            btn.Click += new EventHandler(SlotPickerBtn_Click);
            pnlSlotPicker.Controls.Add(btn);
        }
    }

    private void SlotPickerBtn_Click(object? sender, EventArgs e)
    {
        if (sender is not Button btn) return;
        _selectedSlot = (int)btn.Tag!;
        foreach (Control c in pnlSlotPicker.Controls)
        {
            if (c is Button b)
            {
                bool sel = (int)b.Tag! == _selectedSlot;
                b.BackColor = sel ? AppColors.GoldBtnBg   : (_slots[(int)b.Tag!].IsEmpty ? AppColors.AppBg : AppColors.SidebarBg);
                b.ForeColor = sel ? AppColors.GoldBtnFg   : (_slots[(int)b.Tag!].IsEmpty ? AppColors.TextDim : AppColors.TextMuted);
                b.FlatAppearance.BorderColor = sel ? AppColors.GoldBtnBg : AppColors.Border;
            }
        }
    }

    private void btnBrowse_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Title = "Select .ydk file", Filter = "YDK files (*.ydk)|*.ydk" };
        if (dlg.ShowDialog() == DialogResult.OK)
            txtPath.Text = dlg.FileName;
    }

    private void btnImport_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtPath.Text))
        { AppMessageBox.Show("Please select a .ydk file.", "Import .ydk"); return; }
        if (_selectedSlot < 0)
        { AppMessageBox.Show("Please select a target slot.", "Import .ydk"); return; }

        Deck deck;
        try { deck = YDKReader.Parse(txtPath.Text, AppContext.CardDb); }
        catch (Exception ex)
        {
            AppMessageBox.Show($"Couldn't read that .ydk file:\n{ex.Message}", "Import .ydk", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (deck.main.Count > 60 || deck.extra.Count > 15 || deck.side.Count > 15)
        {
            AppMessageBox.Show(
                $"That deck doesn't fit the LOTD limits (Main ≤60, Extra ≤15, Side ≤15).\n" +
                $"Found Main {deck.main.Count}, Extra {deck.extra.Count}, Side {deck.side.Count}.",
                "Import .ydk", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var target = _slots[_selectedSlot];
        if (!target.IsEmpty)
        {
            var res = AppMessageBox.Show(
                $"Slot {target.SlotNumber} already has a deck ('{target.Name}'). Overwrite its cards with the imported deck?",
                "Import .ydk", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res != DialogResult.Yes) return;
        }

        AppContext.SlotIo.WriteSlot(AppContext.State.SaveBytes, target.SlotNumber, deck, target.Name, target.OwnerId);

        AppMessageBox.Show(
            $"Imported {Path.GetFileName(txtPath.Text)} into slot {target.SlotNumber}.",
            "Import .ydk", MessageBoxButtons.OK, MessageBoxIcon.Information);

        DeckImported?.Invoke(this, EventArgs.Empty);
    }
}
