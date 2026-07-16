using YuGiOhDeckManager.Wpf.Services;
using static YuGiOhDeckManager.Wpf.Services.SlotIO;

namespace YuGiOhDeckManager.Wpf;

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