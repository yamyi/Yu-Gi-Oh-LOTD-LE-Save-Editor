using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

/// <summary>
/// Modal dialog for picking one of the owner/duelist characters in OwnerDatabase.
/// Several character names repeat across different IDs (e.g. "Jack Atlas" is both
/// 16 and 50), so the list always shows "ID — Name" to avoid ambiguity.
/// All control layout lives in OwnerPickerForm.Designer.cs.
/// </summary>
public sealed partial class OwnerPickerForm : Form
{
    private readonly List<OwnerListItem> _allOwners;

    private OwnerPickerForm(byte currentOwnerId)
    {
        InitializeComponent();

        _allOwners = OwnerDatabase.All
            .Select(kv => new OwnerListItem(kv.Key, kv.Value))
            .OrderBy(o => o.Name)
            .ToList();

        Filter();
        SelectCurrent(currentOwnerId);
    }

    private void txtSearch_TextChanged(object? sender, EventArgs e) => Filter();

    private void lstOwners_DoubleClick(object? sender, EventArgs e)
    {
        if (lstOwners.SelectedItem != null)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    private void Filter()
    {
        string q = txtSearch.Text.Trim();
        lstOwners.Items.Clear();

        IEnumerable<OwnerListItem> matches = string.IsNullOrEmpty(q)
            ? _allOwners
            : _allOwners.Where(o => o.Name.Contains(q, StringComparison.OrdinalIgnoreCase));

        foreach (var o in matches)
            lstOwners.Items.Add(o);
    }

    private void SelectCurrent(byte currentOwnerId)
    {
        for (int i = 0; i < lstOwners.Items.Count; i++)
        {
            if (lstOwners.Items[i] is OwnerListItem o && o.Id == currentOwnerId)
            {
                lstOwners.SelectedIndex = i;
                return;
            }
        }
    }

    /// <summary>Shows the picker modally. Returns the selected owner id, or null if cancelled.</summary>
    public static byte? Show(byte currentOwnerId)
    {
        using var form = new OwnerPickerForm(currentOwnerId);
        return form.ShowDialog() == DialogResult.OK && form.lstOwners.SelectedItem is OwnerListItem selected
            ? selected.Id
            : null;
    }

    private void lstOwners_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (lstOwners.SelectedItem != null)
        {
            lblOwnerId.Text = $"ID: {(lstOwners.SelectedItem as OwnerListItem)?.Id:D3}";
            lblOwnerName.Text = $"Name: {(lstOwners.SelectedItem as OwnerListItem)?.Name}";
            pictureBox1.Image = PortraitProvider.GetPortrait((lstOwners.SelectedItem as OwnerListItem)?.Id ?? 0);
        }
    }

    private sealed record OwnerListItem(byte Id, string Name)
    {
        public override string ToString() => $"{Id:D3} — {Name}";
    }
}
