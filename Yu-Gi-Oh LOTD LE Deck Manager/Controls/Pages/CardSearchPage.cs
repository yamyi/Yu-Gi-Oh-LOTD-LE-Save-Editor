using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Forms;
using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Pages;

/// <summary>
/// Standalone card database browser — search the full card list, narrow it
/// with the same filter dialog used in DeckEditorPage, and preview details
/// (with full art, when online). Purely read-only: this page never touches
/// a deck slot. All control creation lives in CardSearchPage.Designer.cs.
/// </summary>
public sealed partial class CardSearchPage : UserControl
{
    private bool _isOnline;
    private CardFilterOptions? _activeFilter;
    private Card? _previewCard;

    public CardSearchPage()
    {
        InitializeComponent();

        // Shows the active theme's background image (if any) in whatever
        // space isn't covered by the toolbar/results/preview panels — see
        // BackgroundPainter. No-ops entirely when no image is set.
        Paint += BackgroundPainter.Paint;

        // Follow the app-wide Offline Mode toggle, same as DeckEditorPage.
        AppContext.State.OfflineModeChanged += OnOfflineModeChanged;
        IsOnline = !AppContext.State.IsOfflineMode;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            AppContext.State.OfflineModeChanged -= OnOfflineModeChanged;

        base.Dispose(disposing);
    }

    private void OnOfflineModeChanged(bool isOffline) => IsOnline = !isOffline;

    public bool IsOnline
    {
        get => _isOnline;
        set
        {
            _isOnline = value;

            if (_previewCard is null) return;

            if (value)
            {
                LoadPreviewImage(_previewCard);
            }
            else
            {
                picPreview.Image = null;
                picPreview.Visible = false;
            }
        }
    }

    // ── Search + filter ──────────────────────────────────────────────────────
    private void txtSearch_TextChanged(object? sender, EventArgs e) => ApplySearch();

    private void chkSearchDesc_CheckedChanged(object? sender, EventArgs e) => ApplySearch();

    // Empty query with no filter shows nothing rather than dumping the whole
    // card database into an owner-drawn list box. Results are capped either way.
    private void ApplySearch()
    {
        string q = txtSearch.Text.Trim();
        bool hasQuery = !string.IsNullOrEmpty(q);
        bool searchDesc = chkSearchDesc.Checked;

        IEnumerable<Card> results;
        if (_activeFilter is null)
        {
            results = hasQuery ? AppContext.CardDb.Search(q, searchDesc) : Enumerable.Empty<Card>();
        }
        else
        {
            results = AppContext.CardDb.Filter(
                name: hasQuery ? q : null,
                type: _activeFilter.Type,
                attribute: _activeFilter.Attribute,
                race: _activeFilter.Race,
                archetype: _activeFilter.Archetype,
                id: _activeFilter.Id,
                minLevel: _activeFilter.MinLevel,
                maxLevel: _activeFilter.MaxLevel,
                minAtk: _activeFilter.MinAtk,
                maxAtk: _activeFilter.MaxAtk,
                minDef: _activeFilter.MinDef,
                maxDef: _activeFilter.MaxDef,
                minLinkVal: _activeFilter.MinLinkVal,
                maxLinkVal: _activeFilter.MaxLinkVal,
                minScale: _activeFilter.MinScale,
                maxScale: _activeFilter.MaxScale,
                searchDescription: searchDesc);
        }

        var capped = results.Take(200).ToList();

        lstResults.Items.Clear();
        foreach (var c in capped)
            lstResults.Items.Add(c);
        lstResults.DisplayMember = nameof(Card.Name);

        lblResultCount.Text = !hasQuery && _activeFilter is null
            ? "Type to search"
            : capped.Count == 200
                ? "200+ results"
                : $"{capped.Count} result(s)";
    }

    private void btnFilter_Click(object? sender, EventArgs e)
    {
        var result = CardFilterForm.Show(AppContext.CardDb, _activeFilter);
        if (result is null) return; // cancelled

        _activeFilter = result.IsEmpty ? null : result;
        btnFilter.Text = _activeFilter is null ? "Filter" : "Filter*";

        // The dialog's own Name field is just a convenience for the same
        // toolbar search box — fold it in so there's one source of truth.
        if (!string.IsNullOrWhiteSpace(result.Name))
            txtSearch.Text = result.Name;

        ApplySearch();
    }

    // Fully custom-painted rows — a plain ListBox's default rendering doesn't
    // apply the theme consistently across selection states (unselected rows
    // drifted to the OS default black instead of staying themed), so every
    // pixel here comes from AppColors instead of relying on ForeColor alone.
    private void lstResults_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0) return;

        bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        var bg = selected ? AppColors.CardSelected : AppColors.CardBg;
        var fg = selected ? AppColors.TextPrimary : AppColors.TextSecondary;

        using (var bgBrush = new SolidBrush(bg))
            e.Graphics.FillRectangle(bgBrush, e.Bounds);

        if (lstResults.Items[e.Index] is Card card)
        {
            var textRect = new Rectangle(e.Bounds.X + 10, e.Bounds.Y, e.Bounds.Width - 14, e.Bounds.Height);
            TextRenderer.DrawText(e.Graphics, card.Name, e.Font ?? lstResults.Font, textRect, fg,
                TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
        }

        if (selected)
        {
            using var accentPen = new Pen(AppColors.BorderSelected, 2);
            e.Graphics.DrawLine(accentPen, e.Bounds.Left, e.Bounds.Top, e.Bounds.Left, e.Bounds.Bottom);
        }
    }

    // ── Preview ───────────────────────────────────────────────────────────────
    private void lstResults_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (lstResults.SelectedItem is Card card)
            ShowPreview(card);
    }

    private void ShowPreview(Card card)
    {
        _previewCard = card;

        lblPreviewName.Text = card.Name;
        lblPreviewType.Text = card.HumanReadableCardType;
        lblPreviewAttr.Text = string.IsNullOrEmpty(card.Attribute) ? string.Empty : $"Attribute: {card.Attribute}";
        lblPreviewRace.Text = string.IsNullOrEmpty(card.Race) ? string.Empty : $"Race: {card.Race}";
        lblPreviewArchetype.Text = string.IsNullOrEmpty(card.Archetype) ? string.Empty : $"Archetype: {card.Archetype}";
        lblPreviewDesc.Text = card.Desc;

        lblPreviewAtk.Text = card.Atk.HasValue ? $"ATK  {card.Atk}" : string.Empty;
        lblPreviewAtk.Visible = card.Atk.HasValue;
        lblPreviewDef.Text = card.Def.HasValue ? $"DEF  {card.Def}" : string.Empty;
        lblPreviewDef.Visible = card.Def.HasValue;

        picPreview.Image = null;
        picPreview.Visible = _isOnline;
        pnlPreviewText.Visible = true;
        lblHint.Visible = false;

        if (_isOnline) LoadPreviewImage(card);
    }

    private async void LoadPreviewImage(Card card)
    {
        var image = await CardImageProvider.GetImageAsync(card, small: false);
        if (image is null || IsDisposed || !_isOnline) return;
        if (!ReferenceEquals(_previewCard, card)) return; // user picked a different card

        picPreview.Image = image;
    }

    // Double-clicking the preview art zooms it in-window (scroll to zoom,
    // click/Esc/× to close) instead of opening a separate popup window.
    private void picPreview_DoubleClick(object? sender, EventArgs e)
    {
        if (picPreview.Image is null) return;
        imgOverlay.ShowImage(picPreview.Image, _previewCard?.Name ?? "Card");
    }
}
