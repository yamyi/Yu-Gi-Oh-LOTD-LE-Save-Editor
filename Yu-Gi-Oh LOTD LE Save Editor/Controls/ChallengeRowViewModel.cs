namespace YuGiOhSaveEditor.Controls;

/// <summary>
/// One row in Save Editor's new Challenges tab - a single named duelist's
/// Duelist Challenge, resolved from DuelistChallengeSlots (CharacterId ->
/// Challenges-array slot index) plus OwnerDatabase for the display name and
/// OwnerPortraitConverter for the portrait, same pipeline the Campaign duel
/// chips already use for their two duelist portraits. A handful of
/// duelists have two distinct challenges sharing one display name (e.g.
/// Alexis Rhodes' GX and ARC-V appearances) - SaveEditorView.RefreshChallengesTab
/// appends " (#CharacterId)" to Label whenever that happens, so every row on
/// screen is still unambiguous even though this class itself doesn't know
/// about the collision.
/// </summary>
public sealed class ChallengeRowViewModel
{
    public string Label { get; }
    public byte CharacterId { get; }
    public int SlotIndex { get; }
    public string State { get; }

    public ChallengeRowViewModel(string label, byte characterId, int slotIndex, string state)
    {
        Label = label;
        CharacterId = characterId;
        SlotIndex = slotIndex;
        State = state;
    }
}
