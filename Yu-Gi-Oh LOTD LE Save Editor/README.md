# Yu-Gi-Oh LOTD:LE Deck Manager

A Windows desktop app for viewing and editing decks in *Yu-Gi-Oh! Legacy of the Duelist: Link Evolution* save files. It reads and writes the game's `.dat` save format directly, so changes made here are just as valid as ones made in-game.

Built with C# / WinForms on .NET 8.

## Features

**32-slot deck browser.** See every deck slot in the save at once, each showing the duelist portrait, deck name, and Main/Extra/Side card counts. Click a slot to see full details in the side panel; right-click for a full action menu (edit, rename, change duelist, copy to another slot, swap with another slot, import `.ydk`, export `.ydk`, clear).

**Full deck editor.** Search the entire card database, filter by type/attribute/race/archetype/level/ATK/DEF/Link rating/Pendulum scale (as min–max ranges), and build out Main/Extra/Side decks slot by slot. Double-click a search result to add it to the deck; double-click a deck slot to remove it. Card art can be viewed online or the app can run fully offline with text-only cards.

**Drag-and-drop everywhere.** Drag a card to reorder it within a deck section — everything between the old and new position shifts over by one, like reordering a playlist, instead of just swapping two cards. The same push-to-reorder behavior works for whole deck slots on the browser grid. While dragging, a small thumbnail follows the cursor and the slot you're hovering over highlights green so the drop target is always obvious. Drag a `.ydk` file in from Explorer and drop it directly onto a slot to import it.

**Undo / redo.** Every edit — renames, duelist changes, copy/swap, imports, clears, drag-reorders, and deck editor saves — can be undone with `Ctrl+Z` and redone with `Ctrl+Y` (or `Ctrl+Shift+Z`), or via the buttons in the top bar. Undo history resets whenever a save file is (re)loaded.

**Multi-select and bulk actions.** `Ctrl`+click to toggle individual deck slots into a selection, `Shift`+click to select a range. With a selection active, bulk-clear or bulk-export (each deck to its own `.ydk` file in a chosen folder) every selected slot at once.

**Keyboard navigation.** Arrow keys move the selection around the deck-slot grid or the deck editor's card grid; `Delete` clears the selected slot or removes the focused card. Disabled automatically while typing in a search box.

**Deck legality badges.** Each filled deck slot shows a small colored dot: green for a deck within the conventional 40–60 card Main-deck range (and within the save format's 60/15/15 caps), amber if the Main deck is under 40 cards, red if a section somehow exceeds the save format's hard limits. Hover a slot for the full breakdown.

**Full theming engine.** Switch between built-in Dark and Light themes, or build your own — every one of the ~47 named colors in the app (backgrounds, borders, text, buttons, pips, status colors) is editable in the Theme Editor. Set a custom background image for the whole app, with adjustable overlay opacity so text stays readable over it. Export a theme to a `.json` file to share it, or import one someone sent you, from Settings → Appearance.

**Cross-slot search.** The deck-slot browser's search box matches on deck name *or* any card inside the deck — so you can find "which slot has Blue-Eyes White Dragon" as easily as searching by deck name.

**Save editor.** A dedicated tab for everything in the save file besides decks: set Duel Points directly; unlock, complete, or reset campaign duels per-series or across the whole campaign (forward and reverse duels separately); toggle unlocked shop packs, battle packs, avatars, and completed tutorials; bulk-manage duelist challenges and deck recipes; and bulk-edit the card collection (owned count 0–3, "seen"/NEW state). The app detects on load whether a save is the original *Legacy of the Duelist* format or the newer *Link Evolution* format and reads/writes the correct byte offsets for each — see "How the save format works" below. Per-card editing by name isn't available, since that mapping lives in the game's own installed data files rather than the save.

## Getting started

1. Open `Yu-Gi-Oh LOTD LE Deck Manager.slnx` in Visual Studio 2022 (17.8+) or JetBrains Rider. The [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) is required.
2. Restore NuGet packages (just [ReaLTaiizor](https://www.nuget.org/packages/ReaLTaiizor) — pulled automatically on first build).
3. Build and run (`F5`, or `dotnet run --project "Yu-Gi-Oh LOTD LE Deck Manager"` from the repo root).
4. In the app, use **Open save** to load a `.dat` save file, browse/edit decks, then **Save file** to write changes back.

The card database (`Assets/cards/Cards.json`) and duelist portraits (`Assets/characters/*.png`) are embedded/copied resources and ship with the repo — no extra setup needed.

### Offline mode

Settings → General → *Offline mode* skips fetching card art over the network; deck slots and the card search list fall back to text-only cards (name/type/ATK/DEF). Useful if you don't have a network connection or just prefer a lighter UI.

## Project structure

```
Yu-Gi-Oh LOTD LE Deck Manager/
├── Yu-Gi-Oh LOTD LE Deck Manager.csproj
├── Program.cs                    ← Entry point; soft-restart loop for theme changes
├── AppContext.cs                 ← Global static services (save state, card DB, undo)
├── AppColors.cs                  ← Theme-aware color roles used by every control
├── Theme.cs / ThemeManager.cs    ← Theme model, persistence, built-in Dark/Light palettes
├── BackgroundPainter.cs          ← Shared "wallpaper" background-image renderer
├── DragGhost.cs                  ← Cursor-following drag thumbnail helper
│
├── Controls/
│   ├── Pages/                    ← DeckSlotsPage, DeckEditorPage, CardSearchPage, SaveEditorPage, SettingsPage
│   └── UserControls/             ← CardSlot, DeckCard, CardImageOverlay, and friends
│
├── Forms/
│   ├── MainForm.cs               ← Top-level window: sidebar nav, top bar, undo/redo, file I/O
│   ├── ThemeEditorForm.cs        ← Full color-by-color theme editor
│   ├── CardFilterForm.cs         ← Advanced card search filter dialog
│   └── OwnerPickerForm.cs        ← Duelist picker for "change duelist"
│
├── Models/
│   ├── Save/                     ← SlotLayout, SlotIO, OwnerDatabase, SaveSignatureFixer,
│   │                                LotdSaveFormat, MiscSaveLayout, CampaignSaveLayout, CardCollectionLayout
│   └── YDK/                      ← Card, Deck, CardDatabase, YDKReader/Writer
│
├── Services/
│   ├── AppState.cs               ← Loaded save bytes, slot list, dirty-tracking baseline
│   └── UndoManager.cs            ← Full-buffer snapshot undo/redo stack
│
└── Assets/
    ├── cards/Cards.json          ← Embedded card database
    └── characters/{id}.png       ← Duelist portraits, one per owner ID (1–191)
```

## How the save format works

`Models/Save/LotdSaveFormat.cs` detects which of the two save formats is loaded — the original *Legacy of the Duelist* (29005 bytes, 5 duel series) or *Link Evolution* (44008 bytes, 6 duel series, adds VRAINS) — from the file's header magic bytes, and exposes the byte offset/size of every top-level chunk (Stats, Battle Packs, Misc, Campaign, Decks, Card List) for whichever format is active. Everything else reads through that rather than assuming a fixed layout.

`Models/Save/SlotLayout.cs` documents the deck-slot block itself: 32 fixed-size blocks, each holding a name, owner ID, Main/Extra/Side counts, and up to 90 card IDs (60 Main + 15 Extra + 15 Side), all little-endian — its base offset now comes from `LotdSaveFormat` instead of being hardcoded. `SlotIO.cs` builds on that for reading/writing/copying/swapping whole slot blocks. `MiscSaveLayout.cs` and `CampaignSaveLayout.cs` cover Duel Points, avatar/shop-pack/battle-pack/tutorial unlocks, duelist challenges, deck recipes, and campaign duel states; `CardCollectionLayout.cs` covers the per-card owned-count/seen byte array. `SaveSignatureFixer.cs` patches the checksum/signature field the game expects before a save is written back to disk.

These offsets and struct layouts are ported from [pixeltris/Lotd](https://github.com/pixeltris/Lotd), the original reverse-engineered save-editing tool for this game.

Card lookups go through `Models/YDK/CardDatabase.cs`, which maps between the game's own internal `LotdId` and the broader YGOPRODeck-style card data bundled in `Cards.json` — only cards with a known `LotdId` can actually be written into a save slot.

## Known limitations

- No installer — run from source or a self-built binary.
- Deck legality badges check structural size limits only; the app has no LOTD-specific banlist data, so it can't flag individual banned/limited cards.
- Compiling requires a Windows machine with the .NET 8 SDK and WinForms workload — there's no cross-platform build target.
- The Save Editor's card collection and duelist-challenge/deck-recipe sections are bulk-only — the save tracks those by an internal index the game derives from its own installed data files, not by name, so editing a single named card or challenge isn't possible here.
