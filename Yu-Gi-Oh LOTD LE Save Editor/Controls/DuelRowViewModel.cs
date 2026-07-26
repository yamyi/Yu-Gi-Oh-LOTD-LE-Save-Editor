namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// One row in Save Editor's Campaign duel grid - the visible 1-based row
/// number, the actual 0-based duel index in the save's 50-slot array
/// (padding duels are filtered out before this list is ever built, so
/// DisplayNumber and DuelIndex diverge), the episode title + character
/// matchup + both duelists' OwnerDatabase ids (from CampaignDuelNames,
/// falling back to a plain "Duel N" title / empty matchup / id 0 for
/// anything outside the known name tables - OwnerPortraitConverter shows no
/// image for an unknown id), and the two state ComboBoxes' starting
/// selections. SaveEditorView.xaml.cs reads DuelIndex back off this (via
/// the row's DataContext) whenever either ComboBox's selection changes.
/// </summary>
public sealed class DuelRowViewModel
{
    public int DisplayNumber { get; }
    public int DuelIndex { get; }
    public string Title { get; }
    public string Matchup { get; }
    public byte OwnerA { get; }
    public byte OwnerB { get; }
    public string Forward { get; }
    public string Reverse { get; }

    /// <summary>False only for the series' first real duel (DisplayNumber 1) -
    /// its Reverse state is always Locked in the real game (Reverse mode as
    /// a whole only unlocks after clearing the entire series forward, not
    /// after any single early duel), so DuelRowTemplate hides its Reverse
    /// ComboBox entirely rather than show a dropdown that can't actually do
    /// anything meaningful. See also
    /// CampaignSaveLayout.EnsurePrerequisitesComplete, which skips this same
    /// duel for the same reason.</summary>
    public bool ShowReverse { get; }

    public DuelRowViewModel(int displayNumber, int duelIndex, string title, string matchup, byte ownerA, byte ownerB, string forward, string reverse)
    {
        DisplayNumber = displayNumber;
        DuelIndex = duelIndex;
        Title = title;
        Matchup = matchup;
        OwnerA = ownerA;
        OwnerB = ownerB;
        Forward = forward;
        Reverse = reverse;
        ShowReverse = displayNumber != 1;
    }
}
