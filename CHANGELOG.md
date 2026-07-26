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
