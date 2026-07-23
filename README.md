# Yu-Gi-Oh LOTD LE Save Editor

A Windows desktop app for viewing and editing save files from *Yu-Gi-Oh! Legacy of the Duelist: Link Evolution*. It reads and writes the game's `.dat` save format directly, so changes made here are just as valid as ones made in-game.

Built with C# / WPF on .NET 8. Every screen is declared entirely in XAML — no controls are ever created at runtime in code.

## Features

**32-slot deck browser (Deck Slots).** See every deck slot in the save at once, each showing the duelist portrait, deck name, Main/Extra/Side card counts, and a small legality dot. Search by deck name or by any card inside the deck. Select a slot to Rename it, Copy To or Swap With another slot, Change Duelist, Clear the slot entirely, or Import/Export its `.ydk`. Export All Decks dumps every non-empty slot to its own `.ydk` file in a chosen folder in one go.

**Full deck editor (Deck Editor).** Search the entire card database and filter by type/attribute/race/archetype/level/ATK/DEF/Link rating/Pendulum scale (as min–max ranges), then build out Main/Extra/Side decks slot by slot. Double-click a search result to add it to the deck; double-click a deck card to remove it. A live stats panel shows monster/spell/trap breakdowns, average level, and attribute/type distribution as you edit. Card art can be viewed online or the app can run fully offline with text-only cards.

**Drag-and-drop.** Drag a card to reorder it within a deck section, or drag a `.ydk` file in from Explorer and drop it onto a slot to import it directly. A thumbnail follows the cursor while dragging.

**Card Search.** Browse and inspect the full card database independently of any particular deck, with the same filtering options as Deck Editor.

**Save Editor.** A tabbed editor over the rest of the save file:
- *Duel Points* — view and set the DP total.
- *Campaign* — view and toggle completion for every non-reverse campaign duel.
- *Unlocks* — Shop Packs and Battle Packs checklists (including duelist-specific packs like Blue Angel, Soulburner, Varis, Ai, War of the Giants/Round 2, and Epic Dawn) plus Tutorials, each with per-item toggles and bulk Unlock All / Clear All.
- *Avatars* — toggle which duelist avatars are unlocked.
- *Stats* — view and edit the save's tracked lifetime stats (duels played/won, cards traded, decks created, and more).
- *Cards* — an accurate "X / 10027" owned-card counter (scanning the save's full card table, not just its used prefix), a bulk "set every currently-owned card's count" tool that leaves un-owned cards alone, a one-click Unlock All Cards that owns exactly the 10,027 real cards without touching unused save padding, and a search box to look up and unlock/set the count of one specific card by name.

**Undo / redo.** Every edit — renames, duelist changes, copy/swap, clears, imports, drag-reorders, and every Save Editor edit — can be undone with `Ctrl+Z` and redone with `Ctrl+Y` (or `Ctrl+Shift+Z`), or via the top-bar buttons. `Ctrl+O` opens a save file and `Ctrl+S` saves it. Undo history resets whenever a save file is (re)loaded.

**Deck legality badges.** Each filled deck slot shows a colored dot: green for a deck within the 40–60 card Main-deck range (and within the save format's 60/15/15 caps) with no banlist violations, amber if the Main deck is under 40 cards, red if a section exceeds the save format's hard limits or contains a Forbidden card, more than one Limited copy, or more than two Semi-Limited copies of the same card (LotD banlist, from the bundled card database).

**Auto-backup & restore.** Opening a save file automatically copies it to a timestamped backup (`savegame.dat.bak_20260723_211932`) right next to the save file before anything touches it, keeping the most recent 20 backups per save file name. The top bar's **Restore Backup** button lists those backups and loads whichever one you pick back into the editor — nothing is written to disk until you Save, and the restore itself is a normal undoable edit (`Ctrl+Z` brings back what was open before).

**Drag-and-drop open.** Drop a `.dat` save file anywhere on the window to open it, same as **Open Save**.

**Settings.** Toggle Offline Mode (already-cached card art still displays offline; only genuinely uncached cards fall back to a text label — and a card preview specifically falls back further to a stretched-up thumbnail if that's all that's cached, rather than showing nothing). The on-disk image cache itself is also optional — turn it off and every image request re-contacts ygoprodeck fresh each time instead of ever touching disk — and, while it's on, an optional max size (in MB) evicts the least-recently-viewed art first once the cache grows past it. **Warm Cache** pre-downloads art for the whole card database before you go offline (confirms first): thumbnails only by default (~150 MB, covers every grid in the app), with an opt-in checkbox to also include full-size preview art (~1761 MB total). The panel also has Open Cache Folder / Clear Cache. Offline Mode, the disk cache toggle, and the cache size limit are all saved automatically and restored the next time the app opens. Version/about info is at the bottom.

## Getting started

1. Open `Yu-Gi-Oh LOTD LE Save Editor.slnx` in Visual Studio 2022 (17.8+) or JetBrains Rider. The [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) is required.
2. Build and run (`F5`, or `dotnet run --project "Yu-Gi-Oh LOTD LE Save Editor"` from the repo root).
3. In the app, use **Open save** (`Ctrl+O`) to load a `.dat` save file, browse/edit decks and save data, then **Save file** (`Ctrl+S`) to write changes back.

The card database (`Assets/cards/Cards.json`) and duelist portraits (`Assets/characters/*.png`) live directly under the project's own `Assets/` folder and are embedded into the app as build resources — no extra setup needed.

### Offline mode

Settings → Offline mode stops the app from downloading any new card art over the network. Art that's already cached on disk (including from a Warm Cache run) still displays normally; only cards with no cached art fall back to a text label in place of the image. Useful without a network connection or to save bandwidth — use **Warm Cache** in Settings beforehand to pre-download everything.

## Project structure

```
Yu-Gi-Oh LOTD LE Save Editor/
├── Yu-Gi-Oh LOTD LE Save Editor.csproj
├── App.xaml(.cs)              ← App startup, merged theme/style resource dictionaries
├── MainWindow.xaml(.cs)        ← Top-level window: sidebar nav, top bar, undo/redo, file I/O, keyboard shortcuts, auto-backup
├── AppContext.cs                ← Global static services (save state, card DB, undo)
│
├── Views/                       ← DeckSlotsView, DeckEditorView, CardSearchView, SaveEditorView, SettingsView
├── Controls/                    ← Reusable windows/converters/view-models (OwnerPickerWindow, ImageViewerWindow, AppInputBox, AppMessageBox, tile/stat/search-result view-models, geometry/brush converters)
├── Themes/                       ← MasterDuelTheme.xaml (fixed palette), Controls.xaml (shared styles)
│
├── Models/
│   ├── Save/                    ← LotdSaveFormat, SlotLayout, SlotIO, OwnerDatabase, CampaignSaveLayout, MiscSaveLayout (DP/shop/battle-pack flags), CardCollectionLayout, StatsLayout, SaveSignatureFixer
│   └── YDK/                     ← Card, Deck, CardDatabase, YDKReader/Writer
│
├── Services/                     ← DeckLegalityChecker, UndoManager, AppState, CardImageProvider, PortraitProvider, and other helpers
│
└── Assets/                       ← Backgrounds, icons, cards/Cards.json, characters/*.png
```

## How the save format works

`Models/Save/LotdSaveFormat.cs` centralizes the version-dependent offsets and sizes (deck slot count, card table size, real card count, stats count) for the two known save formats (`Lotd` and `LinkEvolution`). The other `Models/Save/*Layout.cs` files each own one region of the save:

- `SlotLayout.cs` / `SlotIO.cs` — 32 (or more, per version) fixed-size deck-slot blocks, each holding a name, owner ID, Main/Extra/Side counts, and up to 90 card IDs, all little-endian; `SaveSignatureFixer.cs` patches the save's checksum/signature before writing back to disk.
- `CardCollectionLayout.cs` — one byte per card slot across the save's full card table (owned count 0–3 + a "seen" flag). The real card count (10,027 for LinkEvolution) is smaller than the table's full capacity (20,000), and real cards are scattered throughout that table rather than occupying a contiguous prefix — every scan or bulk operation here walks the full table and only uses the real card count as a display denominator, and per-card lookups go through `Card.LotdId`, which is confirmed to be the same index this class reads/writes.
- `MiscSaveLayout.cs` — Duel Points, and the Shop Pack / Battle Pack unlock flag bits (each duelist and pack's exact bit was confirmed by diffing real before/after saves).
- `CampaignSaveLayout.cs` — per-duel campaign completion state.
- `StatsLayout.cs` — the save's ~40 tracked lifetime stats (duels played/won, cards traded, decks created, etc.), each an 8-byte counter at a fixed offset.

Card lookups elsewhere go through `Models/YDK/CardDatabase.cs`, which maps between the game's own internal `LotdId` and the broader YGOPRODeck-style card data bundled in `Cards.json` (including TCG banlist status) — only cards with a known `LotdId` can actually be written into a save slot or the card collection table.

## Known limitations

- No installer — run from source or a self-built binary.
- Deck legality checks the LotD banlist only; there's no separate TCG/OCG/region setting.
- Compiling requires a Windows machine with the .NET 8 SDK and the WPF (`Microsoft.NET.Sdk` + `UseWPF`) workload — there's no cross-platform build target.

## Licensing

This project's own source code is MIT-licensed — see [`LICENSE`](LICENSE). Use, fork, or modify it freely, including commercially, as long as the copyright notice stays attached.

That license covers the code only. Two things bundled in `Assets/` are not original work and aren't covered by it:

- `Assets/cards/Cards.json` — card names, text, and stats sourced from [YGOPRODeck](https://ygoprodeck.com/)'s API.
- `Assets/characters/*.png` — duelist portraits extracted from the game itself.

Card data, card images, and all *Yu-Gi-Oh!* trademarks/copyrights belong to **4K Media Inc., a subsidiary of Konami Digital Entertainment, Inc.** This is an unofficial fan-made tool — it is not produced by, endorsed by, or affiliated with Konami, 4K Media, or YGOPRODeck. Don't redistribute those two assets outside of this tool, and if you fork the project for anything beyond personal use, swap in your own card/portrait data or keep the same non-affiliation disclaimer.

(Not legal advice — if you plan to distribute this beyond personal/hobby use, it's worth having Konami's IP policies and YGOPRODeck's API terms in front of you directly.)
