# LOTD:LE Save File Format

A byte-level map of the Link Evolution `.dat` save file (44,008 bytes / `0xABE8`). All field-level layout classes live under `Models/Save/` (`LotdSaveFormat`, `SaveLayout`, `StatsLayout`, `MiscSaveLayout`, `CampaignSaveLayout`, `SlotLayout`, `CardCollectionLayout`); this document is the human-readable reference for the same data.

Offsets below are hex unless noted. Original reverse engineering by pixeltris/Lotd.

Legend: ✅ confirmed field &nbsp;·&nbsp; ⚠️ observed changing, not fully decoded &nbsp;·&nbsp; ❔ fully unidentified

## Top-Level Regions

| Region | Offset Range | Length | Implemented By |
|---|---|---|---|
| Header | `0x0000`–`0x0013` | 20 B | `SaveSignatureFixer` |
| Unknown lead-in | `0x0014`–`0x0023` | 16 B | – |
| Stats | `0x0024`–`0x0343` | 800 B | `StatsLayout` |
| BattlePacks | `0x0344`–`0x0FD7` | 3220 B | – |
| Misc | `0x0FD8`–`0x1B6F` | 2968 B | `MiscSaveLayout` |
| Campaign | `0x1B70`–`0x37C7` | 7256 B | `CampaignSaveLayout` |
| Decks | `0x37C8`–`0x5DC7` | 9728 B | `SlotLayout` |
| CardList | `0x5DC8`–`0xABE7` | 20000 B | `CardCollectionLayout` |

### Header (`0x00`–`0x13`, 20 bytes)

| Offset | Length | Field | Notes |
|---|---|---|---|
| `0x00` | 4 B | `Magic1` | constant `0x54CE29F9` |
| `0x04` | 4 B | `Magic2` | constant `0x04ABE802` |
| `0x08` | 4 B | `FileLength` | must equal `0xABE8` (44008) |
| `0x0C` | 4 B | `Signature` | CRC-32-like checksum over the whole file, computed with this field zeroed |
| `0x10` | 4 B | `PlayCount` | number of play sessions; also folded into the signature input |

### Unknown lead-in (`0x14`–`0x23`, 16 bytes)

Fully unidentified. Nothing in the app reads or writes it; it's carried through untouched on every save.

### Stats (`0x24`–`0x343`, 800 bytes) — 100 slots × 8 bytes, base `0x24`

Only indices 0–42 have a confirmed name, ported from pixeltris/Lotd's `StatSaveType` enum. Offset of slot *i* = `0x24 + i×8`. Indices 43–99 are reserved capacity with no known meaning and are never bulk-written.

| Index | Offset | Stat |
|---|---|---|
| 0 | `0x24` | Campaign Games |
| 1 | `0x2C` | Campaign Games (Normal) |
| 2 | `0x34` | Campaign Games (Reverse) |
| 3 | `0x3C` | Challenge Games |
| 4 | `0x44` | Multiplayer Games |
| 5 | `0x4C` | Multiplayer Games (1v1) |
| 6 | `0x54` | Multiplayer Games (Tag) |
| 7 | `0x5C` | Multiplayer Games (Ranked) |
| 8 | `0x64` | Multiplayer Games (Player Match) |
| 9 | `0x6C` | Multiplayer Games (Any Battle Pack) |
| 10 | `0x74` | Multiplayer Games (Battle Pack 1) |
| 11 | `0x7C` | Multiplayer Games (Battle Pack 2) |
| 12 | `0x84` | Multiplayer Games (Battle Pack 3) |
| 13 | `0x8C` | Campaign Wins |
| 14 | `0x94` | Campaign Wins (Normal) |
| 15 | `0x9C` | Campaign Wins (Reverse) |
| 16 | `0xA4` | Challenge Wins |
| 17 | `0xAC` | Multiplayer Wins |
| 18 | `0xB4` | Multiplayer Wins (1v1) |
| 19 | `0xBC` | Multiplayer Wins (Tag) |
| 20 | `0xC4` | Multiplayer Wins (Ranked) |
| 21 | `0xCC` | Multiplayer Wins (Player Match) |
| 22 | `0xD4` | Multiplayer Wins (Any Battle Pack) |
| 23 | `0xDC` | Multiplayer Wins (Battle Pack 1) |
| 24 | `0xE4` | Multiplayer Wins (Battle Pack 2) |
| 25 | `0xEC` | Multiplayer Wins (Battle Pack 3) |
| 26 | `0xF4` | Match Wins |
| 27 | `0xFC` | Non-Match Wins |
| 28 | `0x104` | Normal Summons |
| 29 | `0x10C` | Tribute Summons |
| 30 | `0x114` | Ritual Summons |
| 31 | `0x11C` | Fusion Summons |
| 32 | `0x124` | Xyz Summons |
| 33 | `0x12C` | Synchro Summons |
| 34 | `0x134` | Pendulum Summons |
| 35 | `0x13C` | Total Damage |
| 36 | `0x144` | Battle Damage |
| 37 | `0x14C` | Direct Damage |
| 38 | `0x154` | Effect Damage |
| 39 | `0x15C` | Reflected Damage |
| 40 | `0x164` | Chains Activated |
| 41 | `0x16C` | Decks Created |
| 42 | `0x174` | Cards Won (Campaign) — new card copies granted as post-duel rewards (loss = +1, win = +3, +0 if already at max copies) |
| 43–99 | `0x17C`–`0x343` | *(unnamed)* — reserved capacity |

`Games_Multiplayer_Tag`/`Wins_Multiplayer_Tag` (indices 6/19): Tag Duel isn't reachable through the shipped Multiplayer UI (it requires a third-party duel-starter tool); the slot still exists under its native pixeltris/Lotd name and reads 0 in a normally-played save.

### BattlePacks (`0x344`–`0xFD7`, 3220 bytes) — 5 records × 644 bytes

**Record index** (base = `0x344 + i×0x284`):

| Record | Offset Range | Identity |
|---|---|---|
| 0 | `0x344`–`0x5C7` | Draft: Epic Dawn |
| 1 | `0x5C8`–`0x84B` | Sealed: Epic Dawn |
| 2 | `0x84C`–`0xACF` | Draft: War of the Giants |
| 3 | `0xAD0`–`0xD53` | Sealed: War of the Giants |
| 4 | `0xD54`–`0xFD7` | Draft: War of the Giants Round 2 |

**Per-record field layout** (relative to each record's own base):

| Rel. Offset | Length | Field | Notes |
|---|---|---|---|
| `0x00` | 1 B | *(unknown)* | ❔ |
| `0x01` | 1 B | Owned flag | `0x00` = unused, `0x01` = this pack instance is owned |
| `0x02`–`0x28` | 39 B | *(unknown)* | ❔ zero in every record seen so far |
| `0x29`–~`0x67` | 63 B | Name | ASCII, null-padded, e.g. `"Battle Pack: Epic Dawn"` |
| `0x68`–`0x6A` | 3 B | *(unknown)* | ❔ |
| `0x6B` | 1 B | MainDeckCount | matches in-game Main deck count (39/41/45 across the 3 Draft packs) |
| `0x6C` | 1 B | *(unknown)* | ❔ |
| `0x6D` | 1 B | ExtraDeckCount | matches in-game Extra deck count (6/0/0) |
| `0x6E` | 1 B | *(unknown)* | ❔ |
| `0x6F` | 1 B | SideDeckCount | matches in-game Side deck count (0/4/0) |
| `0x70` | 1 B | *(unknown)* | ❔ |
| `0x71`–`0x10C` | 156 B | Card-ID-list region A | ⚠️ 2-byte tokens, values look like card IDs; not individually decoded, and not confirmed to be scoped to this record specifically (also seen changing in response to an unrelated Multiplayer duel) |
| `0x10D`–`0x124` | 24 B | *(mostly zero filler)* | ❔ |
| `0x125`–`0x146` | 34 B | 2× timestamp-like blocks | ⚠️ same ~10–12 byte shape as `SlotLayout` Created/Modified (first 2 bytes = year LE) |
| `0x147`–`0x154` | 14 B | *(unknown)* | ❔ |
| `0x155` | 1 B | *(unknown counter)* | ⚠️ seen going 0→1 |
| `0x156`–`0x158` | 3 B | *(unknown)* | ❔ |
| `0x159` | 1 B | *(unknown counter)* | ⚠️ seen going 1→2 / 1→4, varies per record |
| `0x15A`–`0x15C` | 3 B | *(unknown)* | ❔ |
| `0x15D`–`0x211` | 181 B | Card-ID-list region B | ⚠️ mirrors region A's content; not individually decoded |
| `0x212`–`0x214` | 3 B | *(unknown)* | ❔ |
| `0x215` | 1 B | Mode marker (?) | ⚠️ reads 3 in every Draft-mode record seen so far; Sealed's value is unconfirmed |
| `0x216`–`0x283` | 110 B | *(zero filler/unused)* | ❔ |

Field-level offsets beyond Main/Extra/Side deck count haven't been formalized into named constants/accessors in `SaveLayout.cs`.

### Misc (`0xFD8`–`0x1B6F`, 2968 bytes) — base `0xFD8`

Field layout ported verbatim from pixeltris/Lotd's `MiscSaveData.Load/Save`.

| Offset | Rel. Offset | Length | Field | Notes |
|---|---|---|---|---|
| `0xFD8`–`0xFE7` | `+0x00` | 16 B | *(reserved/unknown)* | ❔ bytes 0-3/12-15 are a constant bracketing an 8-byte middle section; bytes 4-7 appear to be a sentinel (-1) that changes once Multiplayer has been played |
| `0xFE8`–`0xFEF` | `+0x10` | 8 B | DuelPoints | int64 |
| `0xFF0`–`0x100F` | `+0x18` | 32 B | UnlockedAvatars | bitfield, bit *i* = avatar id *i* (0–152 used) |
| `0x1010`–`0x1AFF` | `+0x38` | 2800 B | Challenges | int32 `DeulistChallengeState` × 700 deck-data slots |
| `0x1B00`–`0x1B57` | `+0xB28` | 88 B | UnlockedRecipes | bitfield, bit *i* = deck-data slot id *i* (700 bits) |
| `0x1B58`–`0x1B5B` | `+0xB80` | 4 B | UnlockedShopPacks | uint32 flags — 30 named duelists + Epic Dawn (bit 31); bits 0 and 24 unused |
| `0x1B5C`–`0x1B5F` | `+0xB84` | 4 B | UnlockedBattlePacks | uint32 flags — BlueAngel, WotG Round 2, WotG, Soulburner, Varis, Ai |
| `0x1B60`–`0x1B67` | `+0xB88` | 8 B | *(reserved/unknown)* | ❔ all-zero in a real save |
| `0x1B68`–`0x1B6B` | `+0xB90` | 4 B | CompleteTutorials | int32 flags |
| `0x1B6C`–`0x1B6F` | `+0xB94` | 4 B | UnlockedContent | int32 flags — DuelistChallenges, BattlePack, CardShop |

### Campaign (`0x1B70`–`0x37C7`, 7256 bytes)

**Chunk-wide lead-in:**

| Offset | Length | Field | Notes |
|---|---|---|---|
| `0x1B70`–`0x1B73` | 4 B | LastPlayedSeriesIndex | int32, `LotdDuelSeries` value — a "resume where you left off" bookmark, not a counter |
| `0x1B74`–`0x1B77` | 4 B | LastPlayedDuelIndex | int32, Stage number of the duel last played in that series |

Never touched by any editor write path.

**6 series blocks** (1208 bytes each = 8-byte trailer + 50 × 24-byte duel records, base `0x1B78`):

| Series | Offset Range | Real Duels |
|---|---|---|
| Yu-Gi-Oh! | `0x1B78`–`0x202F` | index 1–32 (32 duels) |
| Yu-Gi-Oh! GX | `0x2030`–`0x24E7` | index 1–32 (32 duels) |
| Yu-Gi-Oh! 5D's | `0x24E8`–`0x299F` | index 1–32 (32 duels) |
| Yu-Gi-Oh! ZEXAL | `0x29A0`–`0x2E57` | index 1–26 (26 duels) |
| Yu-Gi-Oh! ARC-V | `0x2E58`–`0x330F` | index 1–33 (33 duels) |
| Yu-Gi-Oh! VRAINS | `0x3310`–`0x37C7` | index 1–28 (28 duels) |

The rest of each 50-slot array is unused padding.

**Per-duel record layout** (24 bytes, relative to a duel's own offset):

| Rel. Offset | Length | Field | Notes |
|---|---|---|---|
| `+0x0` | 4 B | State | int32, `LotdCampaignDuelState` (0 Locked / 1 Available / 2 AvailableAttempted / 3 Complete / 4 AvailableAlt) |
| `+0x4` | 4 B | ReverseState | int32, same enum |
| `+0x8` | 4 B | LastDuelPointsAwarded | int32, DP paid the last time this duel was won; overwritten on every win, reads 0 if never won |
| `+0xC` | 12 B | *(3× unknown int32)* | ❔ |

Duel 0 in each series is documented as unused padding but has its own State field observed flipping to Complete in lockstep with the whole series finishing — likely a per-series "100% complete" flag, not dead padding (see `CampaignSaveLayout.EnsurePrerequisitesComplete`'s known-gap note). Each series also has an 8-byte unidentified trailer right after duel 0, before duel 1 begins.

### Decks (`0x37C8`–`0x5DC7`, 9728 bytes) — 32 slots × `0x130` bytes

**Slot base offsets** (base = `0x37C8 + (n-1)×0x130`): Slot 1 = `0x37C8`, Slot 2 = `0x38F8`, Slot 3 = `0x3A28` … Slot 32 = `0x5C98`.

**Per-slot field layout** (relative to each slot's own base — identical for every slot):

| Rel. Offset | Length | Field | Notes |
|---|---|---|---|
| `0x000`–`0x03F` | 64 B | Name | UTF-16 LE, null-terminated, 32 chars max |
| `0x040`–`0x041` | 2 B | *(unknown gap)* | ❔ |
| `0x042`–`0x043` | 2 B | MainCount | uint16, max 60 |
| `0x044`–`0x045` | 2 B | ExtraCount | uint16, max 15 |
| `0x046`–`0x047` | 2 B | SideCount | uint16, max 15 |
| `0x048`–`0x0FB` | 180 B | CardData | 60×uint16 Main + 15×uint16 Extra + 15×uint16 Side |
| `0x0FC`–`0x107` | 12 B | Created | game-internal timestamp, first 2 bytes = year LE |
| `0x108`–`0x113` | 12 B | Modified | same shape as Created |
| `0x114`–`0x11F` | 12 B | *(unknown gap)* | ❔ |
| `0x120`–`0x121` | 2 B | OwnerId | uint16, index into OwnerDatabase (e.g. `0x8A` = 138 → IN4-M8) |
| `0x122`–`0x12B` | 10 B | *(unknown gap)* | ❔ |
| `0x12C` | 1 B | Occupied | `0x01` = slot has a deck, `0x00` = empty |
| `0x12D`–`0x12F` | 3 B | *(unknown gap)* | ❔ |

### CardList (`0x5DC8`–`0xABE7`, 20000 bytes)

1 byte per card slot: bits 0-2 = owned count (0-3, game cap), bit 3 = "seen" (false puts the "NEW" ribbon on a card in-game), bits 4-7 unknown.

Capacity (20,000 slots) is intentionally larger than the real card count: pixeltris/Lotd's `Constants.AbsoluteMaxNumCards = 20000` reserves room for a save to round-trip through a newer game version without a format migration.

`GetNumCards(v) = 10027` real, mapped cards. Slot index = `lotd_id` directly — real cards span `lotd_id` 4007–14968, scattered across 353 separate runs within that capacity (**not** a clean `0..10026` prefix). `Assets/cards/Cards.json` maps every one of the 10027 real lotd_ids, enabling per-card editing (not just bulk unlock/reset) via `CardCollectionLayout`.

## Shop / Battle Pack Unlock Bits

`UnlockedShopPacks` (uint32, in Misc): bits 1–23 are the Yu-Gi-Oh/GX/5D's/ZEXAL shop duelists in story order; bits 27–30 are Shay/Declan Akaba/Yuya Sakaki/Playmaker; bit 31 is Epic Dawn (despite being presented as a "Battle Pack" in-game). Bits 0 and 24 (Pendulum) are unused filler.

`UnlockedBattlePacks` (uint32, in Misc): bit 0 = Blue Angel (despite being sold as a normal shop pack in-game), bit 1 = WarOfTheGiantsRound2, bit 2 = WarOfTheGiants, bits 3–5 = Soulburner/Varis/Ai. Together with `UnlockedShopPacks`' bit 31, these two fields cover every one of Link Evolution's 33 shop duelists.

## Steam Achievements

Achievements gated on lifetime win counts depend on a Steamworks client-side stat that lives outside the save file entirely and only advances on a genuine live win — no byte pattern in the save can trigger it retroactively. [Steam Achievement Manager](https://gitlab.com/gjankowski/steam-achievement-manager) is the practical workaround for unlocking an already-earned-in-spirit achievement without replaying.

## See Also

`SaveLayoutDiagram.html` in this folder is an interactive, browsable version of the whole-file map above.
