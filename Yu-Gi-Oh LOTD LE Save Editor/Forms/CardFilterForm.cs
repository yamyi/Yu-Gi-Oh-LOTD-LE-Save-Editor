using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;

/// <summary>Criteria produced by CardFilterForm; null fields mean "any".
/// Covers every commonly-searched card property, not just type/attribute/race/ATK.</summary>
public sealed record CardFilterOptions(
    string? Name,
    int? Id,
    string? Type,
    string? Attribute,
    string? Race,
    string? Archetype,
    int? MinLevel,
    int? MaxLevel,
    int? MinAtk,
    int? MaxAtk,
    int? MinDef,
    int? MaxDef,
    int? MinLinkVal,
    int? MaxLinkVal,
    int? MinScale,
    int? MaxScale)
{
    public static readonly CardFilterOptions Empty =
        new(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

    public bool IsEmpty =>
        Name is null && Id is null && Type is null && Attribute is null && Race is null &&
        Archetype is null && MinLevel is null && MaxLevel is null && MinAtk is null && MaxAtk is null &&
        MinDef is null && MaxDef is null && MinLinkVal is null && MaxLinkVal is null &&
        MinScale is null && MaxScale is null;
}

/// <summary>
/// Modal dialog for narrowing a card search by any of the card's properties —
/// name, passcode, type, attribute, race, archetype, level/rank, ATK/DEF
/// range, link rating, and pendulum scale. Shared by DeckEditorPage and
/// CardSearchPage. All control layout lives in CardFilterForm.Designer.cs.
/// </summary>
public sealed partial class CardFilterForm : Form
{
    private const string AnyItem = "Any";

    private CardFilterForm(CardDatabase db, CardFilterOptions? current)
    {
        InitializeComponent();

        cboType.Items.Add(AnyItem);
        foreach (var t in db.DistinctTypes()) cboType.Items.Add(t);

        cboAttribute.Items.Add(AnyItem);
        foreach (var a in db.DistinctAttributes()) cboAttribute.Items.Add(a);

        cboRace.Items.Add(AnyItem);
        foreach (var r in db.DistinctRaces()) cboRace.Items.Add(r);

        cboArchetype.Items.Add(AnyItem);
        foreach (var ar in db.DistinctArchetypes()) cboArchetype.Items.Add(ar);

        cboType.SelectedItem = current?.Type ?? AnyItem;
        cboAttribute.SelectedItem = current?.Attribute ?? AnyItem;
        cboRace.SelectedItem = current?.Race ?? AnyItem;
        cboArchetype.SelectedItem = current?.Archetype ?? AnyItem;
        if (cboType.SelectedIndex < 0) cboType.SelectedItem = AnyItem;
        if (cboAttribute.SelectedIndex < 0) cboAttribute.SelectedItem = AnyItem;
        if (cboRace.SelectedIndex < 0) cboRace.SelectedItem = AnyItem;
        if (cboArchetype.SelectedIndex < 0) cboArchetype.SelectedItem = AnyItem;

        txtName.Text = current?.Name ?? string.Empty;
        txtId.Text = current?.Id?.ToString() ?? string.Empty;
        txtMinLevel.Text = current?.MinLevel?.ToString() ?? string.Empty;
        txtMaxLevel.Text = current?.MaxLevel?.ToString() ?? string.Empty;
        txtMinLinkVal.Text = current?.MinLinkVal?.ToString() ?? string.Empty;
        txtMaxLinkVal.Text = current?.MaxLinkVal?.ToString() ?? string.Empty;
        txtMinScale.Text = current?.MinScale?.ToString() ?? string.Empty;
        txtMaxScale.Text = current?.MaxScale?.ToString() ?? string.Empty;
        txtMinAtk.Text = current?.MinAtk?.ToString() ?? string.Empty;
        txtMaxAtk.Text = current?.MaxAtk?.ToString() ?? string.Empty;
        txtMinDef.Text = current?.MinDef?.ToString() ?? string.Empty;
        txtMaxDef.Text = current?.MaxDef?.ToString() ?? string.Empty;
    }

    /// <summary>Shows the filter dialog modally. Returns null if cancelled, or the
    /// chosen criteria (possibly CardFilterOptions.Empty, if "Clear" was pressed).</summary>
    public static CardFilterOptions? Show(CardDatabase db, CardFilterOptions? current)
    {
        using var form = new CardFilterForm(db, current);
        return form.ShowDialog() == DialogResult.OK ? form.Result : null;
    }

    private CardFilterOptions? Result { get; set; }

    private void btnApply_Click(object? sender, EventArgs e)
    {
        Result = new CardFilterOptions(
            Name: string.IsNullOrWhiteSpace(txtName.Text) ? null : txtName.Text.Trim(),
            Id: ParseInt(txtId.Text),
            Type: SelectedOrNull(cboType),
            Attribute: SelectedOrNull(cboAttribute),
            Race: SelectedOrNull(cboRace),
            Archetype: SelectedOrNull(cboArchetype),
            MinLevel: ParseInt(txtMinLevel.Text),
            MaxLevel: ParseInt(txtMaxLevel.Text),
            MinAtk: ParseInt(txtMinAtk.Text),
            MaxAtk: ParseInt(txtMaxAtk.Text),
            MinDef: ParseInt(txtMinDef.Text),
            MaxDef: ParseInt(txtMaxDef.Text),
            MinLinkVal: ParseInt(txtMinLinkVal.Text),
            MaxLinkVal: ParseInt(txtMaxLinkVal.Text),
            MinScale: ParseInt(txtMinScale.Text),
            MaxScale: ParseInt(txtMaxScale.Text));

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        Result = CardFilterOptions.Empty;
        DialogResult = DialogResult.OK;
        Close();
    }

    private static string? SelectedOrNull(ComboBox combo) =>
        combo.SelectedItem is string s && s != AnyItem ? s : null;

    private static int? ParseInt(string text) =>
        int.TryParse(text.Trim(), out int value) ? value : null;
}
