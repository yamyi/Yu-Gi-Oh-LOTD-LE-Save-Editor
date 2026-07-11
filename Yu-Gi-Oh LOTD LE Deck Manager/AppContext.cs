using Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services;
using static Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services.SlotIO;

namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager;

/// <summary>
/// Global app-level state accessible from any form or page.
/// </summary>
public static class AppContext
{
    public static List<SlotInfo> slots = new List<SlotInfo>();
    public static AppState State { get; } = new AppState();
    public static SlotIO SlotIo { get; } = new SlotIO();
    public static CardDatabase CardDb { get; } = new CardDatabase();
    public static UndoManager Undo { get; } = new UndoManager();

    
}