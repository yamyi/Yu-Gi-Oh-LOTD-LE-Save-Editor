# Yu-Gi-Oh! Legacy of the Duelist: Link Evolution Save Editor

<p align="center">
 <img width="1919" height="1032" alt="image" src="https://github.com/user-attachments/assets/c64eb849-8552-4fe2-bee4-c1029d2efdee" width="1000">
</p>

<p align="center">

![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=.net)
![Platform](https://img.shields.io/badge/Platform-Windows-0078D6?logo=windows)
![Framework](https://img.shields.io/badge/UI-WPF-0C54C2)
![License](https://img.shields.io/badge/License-MIT-green)

</p>

A modern Windows editor for **Yu-Gi-Oh! Legacy of the Duelist: Link Evolution** save files.

Edit decks, unlock content, modify card collections, inspect statistics, manage campaigns, and browse the complete card database from one native WPF application.

Built with **C#**, **WPF**, and **.NET 8**, the editor reads and writes the game's native `.dat` save format directly, meaning every save produced is identical to one created by the game itself.

---

# Contents

- [Features](#features)
- [Screenshots](#screenshots)
- [Getting Started](#getting-started)
- [Offline Mode](#offline-mode)
- [Project Structure](#project-structure)
- [Save Format](#how-the-save-format-works)
- [Known Limitations](#known-limitations)
- [Credits](#credits)
- [License](#licensing)

---

# Features

The editor isn't just a deck editor—it's a complete save editor covering virtually every editable part of the game's save file.

## 🃏 Deck Management

### 32-Slot Deck Browser

Browse every deck slot simultaneously with rich information displayed for each deck:

- Duelist portrait
- Deck name
- Main / Extra / Side counts
- Legality indicator
- Search by deck name
- Search by contained cards

Each slot can be:

- Renamed
- Copied
- Swapped
- Cleared
- Assigned to another duelist
- Imported from `.ydk`
- Exported to `.ydk`

Export every populated deck in a single operation.

---

### Full Deck Editor

Build decks using the entire card database.

Powerful filtering includes:

- Monster Type
- Attribute
- Race
- Archetype
- Level
- Link Rating
- Pendulum Scale
- ATK / DEF
- Min / Max ranges

Features include:

- Double-click to add cards
- Double-click to remove cards
- Drag-and-drop reordering
- Live deck statistics
- Attribute distribution
- Type breakdown
- Average level calculations

Card artwork can be displayed online or the application can operate entirely offline.

---

### Drag & Drop

Deck editing feels natural.

- Reorder cards with drag-and-drop
- Import `.ydk` files by dropping directly onto deck slots
- Visual drag thumbnail follows the cursor

---

## 🔍 Card Database

Browse the complete Link Evolution card database independently from any deck.

Everything available inside the Deck Editor is also available here:

- Full search
- Advanced filters
- Card previews
- Banlist status
- Shop Pack information
- Duelist Challenge information
- Owned copies
- Card artwork

Perfect for exploring cards without modifying a deck.

---

## 💾 Save Editing

The Save Editor exposes nearly every editable portion of the save file.

| Section | Description |
|---------|-------------|
| 👤 Avatars | Unlock duelist avatars, browsable per-series with automatic unlock once a duelist's full campaign (forward + reverse) is complete |
| 🏆 Campaign | Toggle completion of campaign duels and reverse duels, per series |
| 🎖️ Challenges | Unlock and inspect individual Duelist Challenges, browsable per-series, with the same automatic-unlock rule as Avatars |
| 💰 Duel Points | Edit total Duel Points |
| 📈 Statistics | Edit lifetime game statistics |
| 📦 Unlocks | Unlock Shop Packs, Battle Packs and Tutorials |
| 🃏 Card Collection | Edit owned cards and copy counts |

Avatars, Challenges, and Shop/Battle Pack unlocks all include failsafes that block manually unlocking something before its real in-game prerequisite is actually met, so the editor can't be used to reach an impossible in-game state.

### Card Collection Features

The collection editor provides accurate statistics over the entire save file.

It displays:

- Number of owned cards
- Total copies owned
- Complete collection progress

Utilities include:

- Unlock every card
- Set owned copy counts
- Search individual cards
- Modify specific ownership values

Unlike many save editors, these values are calculated by scanning the complete card table rather than only its populated prefix.

---

## ⚡ Quality-of-Life Features

### Unlimited Undo / Redo

Every editing operation is undoable.

Supported operations include:

- Deck editing
- Imports
- Exports
- Renames
- Copy / Swap
- Drag operations
- Save editing
- Unlock changes

Keyboard shortcuts:

| Shortcut | Action |
|----------|--------|
| Ctrl+Z | Undo |
| Ctrl+Shift+Z | Redo |
| Ctrl+Y | Redo |
| Ctrl+O | Open Save |
| Ctrl+S | Save |

Undo history resets whenever a save file is reloaded.

---

### Deck Legality Checking

Each populated deck displays a legality indicator.

| Color | Meaning |
|--------|---------|
| 🟢 Green | Legal |
| 🟡 Amber | Main deck below 40 cards |
| 🔴 Red | Illegal deck size or banlist violation |

Checks include:

- Main Deck size
- Extra Deck size
- Side Deck size
- Forbidden cards
- Limited cards
- Semi-Limited cards

using the bundled Link Evolution banlist.

---

### Automatic Backups

> [!TIP]
> Every time you save, the original save is automatically backed up before any changes are written.

Features include:

- Timestamped backups
- Automatic rotation
- Keeps the newest 20 backups
- Restore previous backups directly inside the editor
- Restores are themselves undoable

---

### Drag & Drop Save Opening

Simply drag a `.dat` save anywhere onto the application window to open it.

---

### Settings

Customize how the editor behaves.

Available options include:

- Offline Mode
- Disk image cache
- Maximum cache size
- Warm Cache
- Clear Cache
- Open Cache Folder
- Version information

The Warm Cache tool can download artwork for the entire game database before going offline.

- Thumbnails only (~150 MB)
- Full-size artwork (~1.7 GB)

All settings are automatically saved and restored the next time the application starts.

---

# Screenshots

## Deck Browser

<p align="center">
  <img src="https://github.com/user-attachments/assets/ea362f01-791e-4c06-b40e-bbdd32a57095" width="100%">
</p>

Browse every deck in the save at a glance, search by deck name or contained cards, and quickly rename, copy, swap, import, export, or clear individual deck slots.

---

## Deck Editor

<p align="center">
<img src="https://github.com/user-attachments/assets/03a0830f-dc70-4642-bca6-4395570140e8" width="100%">
</p>

Build decks using the complete card database with advanced filtering, drag-and-drop editing, live deck statistics, and online or offline card artwork.

---

## Card Search

<p align="center">
  <img src="https://github.com/user-attachments/assets/34cf137c-7a84-48fc-8a39-6120dedc06b4" width="100%">
</p>

Explore the entire card database independently of any deck using the same powerful search and filtering tools available inside the Deck Editor.

---

## Save Editor

<p align="center">
  <img src="https://github.com/user-attachments/assets/7cfe1e7a-c1a3-46c9-bf22-8bf4d6f00333" width="100%">
</p>

Modify campaign progress, Duel Points, unlocks, avatars, statistics, and the complete card collection from one unified editor.

---

# Getting Started

## Download (Recommended)

Grab the latest prebuilt `.exe` from the [Releases](../../releases) page — no build tools or .NET SDK required. Just download and run it; see [Requirements to Run](#requirements-to-run) below for what needs to already be on your machine.

Prefer to build it yourself, or want to modify the source? See [Building From Source](#building-from-source) below.

---

## Requirements to Run

The prebuilt `.exe` is framework-dependent, so your machine needs the **.NET 8 Desktop Runtime** installed (not the full SDK):

https://dotnet.microsoft.com/download/dotnet/8.0

No other installation or setup is required — everything else the editor needs is embedded in the executable itself.

---

## Building From Source

Before building the project, ensure you have:

- Windows
- Visual Studio 2022 (17.8+) **or** JetBrains Rider
- .NET 8 SDK

Download the latest SDK from:

https://dotnet.microsoft.com/download/dotnet/8.0

---

## Building

Clone the repository and either:

- Open `Yu-Gi-Oh LOTD LE Save Editor.slnx` in Visual Studio or Rider

or run

```bash
dotnet run --project "Yu-Gi-Oh LOTD LE Save Editor"
```

from the repository root.

---

## Using the Editor

1. Launch the application.
2. Open a `.dat` save (`Ctrl + O`).
3. Browse or edit decks.
4. Modify save data.
5. Press `Ctrl + S` to write your changes.

> [!IMPORTANT]
> Saving automatically creates a timestamped backup before anything is written to disk.

---

## Included Assets

No additional setup is required.

The application embeds:

- Card database (`Assets/cards/Cards.json`)
- Duelist portraits
- Icons
- Themes
- Resources

Everything required to run the editor ships with the project.

---

# Card Database

Every card entry contains considerably more than just card text.

Each card includes:

- Card information
- Banlist status
- Shop Pack availability
- Duelist Challenge appearances
- Artwork
- Internal Link Evolution ID

This allows the editor to provide context that isn't available in-game.

---

## Shop Pack Information

Every card stores the Card Shop booster packs that can award it.

Example:

```
Shop:
Yugi (Rare)
```

Cards can belong to multiple packs, each with its own rarity.

The bundled database covers all **33** Link Evolution shop packs.

Only two cards remain without a confirmed source:

- Synchro Chase
- The Unhappy Girl

---

## Duelist Challenge Information

Cards also record every Duelist Challenge deck that contains them.

Example:

```
Duelist Challenge

Yugi Muto
Seto Kaiba
Joey Wheeler
```

Cards appearing in many challenge decks automatically collapse into:

```
+12 more
```

to keep previews readable.

The bundled data covers every Duelist Challenge deck in the game.

---

## Owned Card Counter

When a save is loaded, card previews also display:

```
Owned: X / 3
```

This value updates live directly from the loaded save file.

---

## Icons

Preview panels include official icons for:

- Attribute
- Monster Type

The project bundles:

- Official TCG/OCG Attribute icons
- Yu-Gi-Oh! Master Duel Monster Type icons

making previews much easier to scan.

---

# Offline Mode

> [!NOTE]
> Offline Mode only prevents downloading new artwork.

Already cached artwork continues to display normally.

Cards without cached artwork automatically fall back to text-only previews.

For completely offline usage, use **Warm Cache** beforehand to download every card image.

Available options:

- Thumbnail cache (~150 MB)
- Full-resolution cache (~1.7 GB)

---

# Project Structure

<details>

<summary><strong>Repository Layout</strong></summary>

```text
Yu-Gi-Oh LOTD LE Save Editor
│
├── App.xaml(.cs)
├── MainWindow.xaml(.cs)
├── AppContext.cs
│
├── Views/
│   ├── DeckSlotsView
│   ├── DeckEditorView
│   ├── CardSearchView
│   ├── SaveEditorView
│   └── SettingsView
│
├── Controls/
│
├── Themes/
│
├── Models/
│   ├── Save/
│   └── YDK/
│
├── Services/
│
└── Assets/
```

</details>

---

## Project Highlights

### Views

Contains every major application page.

- Deck Browser
- Deck Editor
- Card Search
- Save Editor
- Settings

---

### Models

Responsible for reading and writing both save files and `.ydk` decks.

Includes:

- Save layouts
- Card database
- Deck serialization
- Save signatures

---

### Services

Application-wide functionality including:

- Undo manager
- Deck legality checker
- Card image provider
- Portrait provider
- Global application state

---

### Assets

Bundles everything required by the editor:

- Card database
- Portraits
- Icons
- Themes
- Backgrounds

No runtime downloads are required beyond optional card artwork.

---

# How the Save Format Works

<details>

<summary><strong>Implementation Details</strong></summary>

The project separates every region of the save into dedicated layout classes.

### LotdSaveFormat

Defines version-specific information including:

- Deck slot counts
- Card table sizes
- Statistics counts
- Save offsets

for both supported save formats.

---

### SlotLayout / SlotIO

Responsible for reading and writing deck slots.

Each slot contains:

- Deck name
- Duelist owner
- Card counts
- Card IDs

The save signature is automatically repaired before writing.

---

### CardCollectionLayout

Handles the game's card ownership table.

Unlike many editors, this implementation scans the entire save table rather than assuming cards occupy one contiguous block.

This guarantees:

- Accurate owned-card totals
- Correct copy counts
- Safe bulk operations

even though Link Evolution scatters real cards throughout the save.

---

### MiscSaveLayout

Handles:

- Duel Points
- Shop Pack unlocks
- Battle Pack unlocks

Every bit has been verified against real saves.

---

### CampaignSaveLayout

Stores campaign duel completion.

---

### StatsLayout

Stores the game's lifetime statistics including:

- Duels played
- Duels won
- Cards traded
- Decks created
- and many more.

---

The card database maps Link Evolution's internal card IDs to the richer YGOPRODeck dataset, allowing decks and save files to reference the same underlying card information.

</details>

---
# Known Limitations

Although the editor supports nearly every aspect of the save format, a few limitations remain.

- No installer is provided, only a portable `.exe` attached to each [Release](../../releases) — there's no Start Menu shortcut, uninstaller entry, or auto-update check.
- Deck legality uses the bundled **Link Evolution** banlist only. There is currently no option to validate against alternate TCG or OCG formats.
- Building the project from source requires Windows with the **.NET 8 SDK** and the WPF workload (`Microsoft.NET.Sdk` + `UseWPF`) — not required if you're just running the prebuilt `.exe`.
- Steam achievements gated on lifetime win counts (e.g. completing a full campaign series) can't be triggered by editing the save file, even if every duel is set to Complete. Those achievements are tracked by a Steamworks client-side stat that only advances on a genuine live win in-game — the save file has no influence over it. [Steam Achievement Manager](https://gitlab.com/gjankowski/steam-achievement-manager) is the practical workaround if you want an already-earned-in-spirit achievement unlocked without replaying.

---

# Credits

This project would not have been possible without the work of several other communities and projects.

## Save Format Research

### pixeltris/Lotd

The original reverse engineering of the Link Evolution save format.

Every save offset, enum, data structure, and bitfield ultimately traces back to their research, which has been ported, expanded, and verified against real save files throughout this project.

Without their work, this editor would not exist.

GitHub:
https://github.com/pixeltris/Lotd

---

## Card Database

### YGOPRODeck

Provides the card database used by the editor, including:

- Card names
- Effect text
- Statistics
- Banlist information
- API
- Card pages

Website:

https://ygoprodeck.com/

---

## Icons

### Yugipedia

Source of the bundled:

- Attribute icons
- Yu-Gi-Oh! Master Duel Monster Type icons

Website:

https://yugipedia.com/

---

## Additional Data

### GameFAQs Community Guides

Several fan-maintained guides were used to enrich the bundled database.

These include:

- Card Shop booster pack availability
- Duelist Challenge deck contents

Special thanks to everyone who compiled and maintained these resources.

Card Shop Guide

https://gamefaqs.gamespot.com/pc/280088-yu-gi-oh-legacy-of-the-duelist-link-evolution/faqs/79850

Character Deck Guide

https://gamefaqs.gamespot.com/pc/280088-yu-gi-oh-legacy-of-the-duelist-link-evolution/faqs/79842

---

## Konami / 4K Media

Finally, thanks to Konami and 4K Media for creating **Yu-Gi-Oh! Legacy of the Duelist: Link Evolution**, without which this project obviously would not exist.

---

# Licensing

The source code in this repository is released under the **MIT License**.

See the included **LICENSE** file for the full license text.

You are free to:

- Use
- Modify
- Fork
- Redistribute
- Use commercially

provided that the original copyright notice remains intact.

---

## Third-Party Assets

The MIT license applies **only** to the original source code contained in this repository.

Several bundled assets remain the property of their respective owners.

### Card Database

`Assets/cards/Cards.json`

Contains card information sourced from **YGOPRODeck**.

---

### Duelist Portraits

`Assets/characters/*.png`

Extracted from **Yu-Gi-Oh! Legacy of the Duelist: Link Evolution**.

---

### Icons

`Assets/icons/Attribute-*.png`

`Assets/icons/Type-*-MADU.png`

Official Attribute icons and Monster Type icons sourced from Yugipedia.

---

## Disclaimer

This is an unofficial fan-made project.

It is **not** produced by, endorsed by, sponsored by, or affiliated with:

- Konami
- Konami Digital Entertainment
- 4K Media
- YGOPRODeck

All Yu-Gi-Oh! names, artwork, trademarks, and copyrights belong to their respective owners.

If you redistribute this project beyond personal use, you should either:

- replace the bundled game assets with your own, or
- retain the same attribution and non-affiliation notice.

---

<p align="center">

Built with ❤️ using C#, WPF, and .NET 8.

If you found this project useful, consider giving the repository a ⭐.

</p>
