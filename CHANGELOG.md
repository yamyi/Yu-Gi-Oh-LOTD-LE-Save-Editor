## Added

### Stats
- Added a new **Stats** entry: **Cards Won (Campaign)**.
  - This previously unidentified counter has been reverse-engineered and formally named.
  - Tracks new card copies awarded as post-duel rewards:
    - **Loss:** +1 card
    - **Win:** +3 cards
    - **Already own maximum copies:** +0 cards
  - The value is now visible and fully editable in the **Stats** tab.

### Campaign
- Completing a campaign duel in the editor now stamps a random **Duel Points** reward value (**1000–1800**) into that duel's internal reward field, matching what the game writes during a genuine completion.
  - **Experimental:** This field is not used for unlocking or scoring. It simply populates a value that editor-completed duels previously left at `0`.

### Challenges
- Added a new **Challenges** tab for unlocking and viewing individual duelist challenges, organized per series like the **Campaign** tab.

### Automatic Unlock Logic
- Completing all campaign duels (Forward and Reverse) for a character now automatically unlocks:
  - Their **Avatar**
  - Their **Duelist Challenge**
- Unlocking any **Duelist Challenge** now automatically unlocks the **Duelist Challenges** content gate.

### Campaign Progression
- Editing a campaign duel to anything other than **Locked** now automatically:
  - Completes every preceding duel in that series.
  - Changes their **Reverse** duels from **Locked** to **Available**, matching normal game progression.
  - **Exception:** The first Reverse duel of each series remains locked, as it does in-game.

---

## Changed

### Interface
- Reworked the **Avatars** tab to use the same per-series layout as **Campaign** and **Challenges**.
- Reordered tabs alphabetically:
  - Avatars
  - Campaign
  - Cards
  - Challenges
  - Duel Points
  - Stats
  - Unlocks

### Validation
- Added failsafes preventing manual unlocking of:
  - Avatars
  - Duelist Challenges
  - Shop/Battle Packs
- These can no longer be unlocked before their legitimate in-game campaign prerequisites have been met.

---

## Investigated

### Steam Achievements
- Investigated and identified the root cause of a long-standing issue where Steam achievements failed to unlock after using the editor to complete campaign duels.
- The affected achievements depend on a **Steamworks client-side stat** that exists entirely outside the save file and only advances after a genuine in-game victory.
- Because this stat is not stored in the save, **no save editor can unlock these achievements retroactively**.
- Users who do not wish to replay the required duels can use **Steam Achievement Manager**, which modifies the same Steamworks achievement/stat data used by the game itself.

### Save Format Research
- Mapped several previously unknown save-file fields through before/after byte analysis:
  - Each campaign duel's **last Duel Points reward** field.
  - The Campaign chunk's **last played series/duel** bookmark.
  - The **BattlePacks** chunk's internal record layout (Name, Main/Extra/Side deck counts, Duel Points cost, timestamp-like data, card-ID list) across all 5 records, now fully confirmed by direct before/after diff (Draft: Epic Dawn, Sealed: Epic Dawn, Draft: War of the Giants, Sealed: War of the Giants, Draft: War of the Giants Round 2).
  - Within each BattlePacks record: the Main/Extra/Side deck card counts, confirmed against the exact in-game deck breakdowns for all 3 Draft packs (Epic Dawn 39/6/0, War of the Giants 41/0/4, War of the Giants Round 2 45/0/0).
  - Confirmed Multiplayer duels correctly drive their own Stats counters (Games_Multiplayer/_1V1/_Ranked on a loss, with no Wins_* counter moving) and that Summons/Damage counters are true lifetime totals across every duel type, not campaign-only.
  - Partially decoded Misc's previously fully-unidentified 16-byte lead-in: an 8-byte middle section (bracketed by a constant on both sides) holds a value that flips from a `-1` sentinel to a real number on a save's first Multiplayer duel - likely a last-played Multiplayer session marker.
  - Corrected an earlier theory: BattlePacks record 4's "card-ID-list" bytes were found to also change in response to an unrelated Multiplayer duel, ruling out "this record's own private card list" - now treated as an unexplained, likely non-record-scoped region pending further investigation.
  - Relabeled the Stats tab's "Multiplayer Games/Wins (Friendly)" to "(Player Match)" - Link Evolution's only two real Multiplayer match types are Ranked and Player Match, not "Friendly."
  - Flagged Games_Multiplayer_Tag/Wins_Multiplayer_Tag as likely vestigial: Link Evolution has no Tag Duel option in Multiplayer at all, even though the slot name traces back to a real symbol in the game's own compiled binary (via pixeltris/Lotd).
  - Corrected a prior doc comment: pixeltris/Lotd's published enum already names index 42 "Cards_Earned" (this project's Cards_Won_Campaign) - it isn't a bare "Unknown" slot this project discovered from scratch, just one this project was first to behaviorally confirm via real diffs.
  - Cross-checked three community repos (MoonlitDeath/Link-Evolution-Editing-Guide, Arefu/Wolf, thomasneff/YGOLOTDPatchDraft) for anything new on the save format. No new byte-level save offsets turned up (none of the three document Link Evolution's save internals beyond what pixeltris/Lotd already covers), but two things upgraded from speculation to sourced fact: Tag Duel is confirmed a real, working engine mode (a genuine `modeTagDuelAddress` flag exists) that's simply unreachable through Link Evolution's shipped UI - explaining Games_Multiplayer_Tag/Wins_Multiplayer_Tag without requiring it to be dead data - and this project's own Campaign chunk math for the original 2016 format was independently corroborated by Arefu/Wolf's one-line save structure note.
  - Checked three more pages of the same wiki (File List, FAQ, Missing ID List). No new save-format info, but strong independent corroboration: the FAQ confirms both the 32-deck-slot limit and the ~700-deck-data-slot capacity this project already uses, and cross-referencing the wiki's 513-entry "IDs referenced in code but not in game" list against this project's own Cards.json found 130 candidate IDs inside the real card range - all 130 correctly absent, 0 false positives.
