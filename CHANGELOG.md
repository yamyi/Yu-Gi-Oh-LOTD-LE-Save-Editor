# Changelog

All notable changes to this project are documented here.

## 2026-07-26

### Added
- **New Stats entry: "Cards Won (Campaign)"** — a previously-unidentified counter in the save's Stats chunk has been reverse-engineered and formally named. It tracks new card copies granted as post-duel rewards (1 for a loss, 3 for a win, 0 if you already own the max copies of whatever was rolled). Now visible and editable in the Stats tab like any other stat.
- Completing a campaign duel now stamps a random Duel Points value (1000–1800) into that duel's own internal reward field, mirroring what a genuine in-game completion writes there. *(Experimental — this doesn't change anything the game reads for unlocking or scoring; it only fills in a field that was previously always left at 0 on editor-completed duels.)*

### Investigated
- Root-caused a long-standing report of Steam achievements not unlocking after using the editor to complete campaign duels — even after subsequently finishing the final duel(s) completely legitimately. The relevant achievements are gated by a Steamworks client-side stat that lives entirely outside the save file and only advances on a genuine, live win; no save-file edit (from this or any other tool) can trigger it retroactively. If you want an affected achievement unlocked without replaying, [Steam Achievement Manager](https://gitlab.com/gjankowski/steam-achievement-manager) is the practical route — it edits that stat/achievement state directly through the same Steamworks API the game itself uses.
- Mapped several previously-unknown save-file fields via careful before/after byte comparison: each campaign duel's last-DP-reward field, and the Campaign chunk's "last played series/duel" bookmark.
- Catalogued every remaining unmapped region of the save format (a handful of Stats slots, the BattlePacks chunk, three per-duel fields in every Campaign record, and a few small gaps in the deck-slot layout) for future reference.

## 2026-07-25

### Added
- **Challenges tab** — unlock and inspect individual duelist challenges, laid out per-series just like the Campaign tab.
- **Avatars tab reworked** to the same per-series layout as Campaign and Challenges.
- Tabs reordered alphabetically: Avatars, Campaign, Cards, Challenges, Duel Points, Stats, Unlocks.
- **Automatic unlock rules:**
  - Completing all of a character's campaign duels (forward *and* reverse) now automatically unlocks their avatar and their duelist challenge.
  - Unlocking any duelist challenge automatically unlocks the Duelist Challenges content gate.
- **Failsafes** preventing manual unlocking of Avatars, Duelist Challenges, and Shop/Battle Packs before their real in-game prerequisites are actually met — you can no longer flip these on ahead of the campaign progress the game would otherwise require.
- Editing a single campaign duel to anything other than Locked now automatically completes every duel before it in that series, and bumps their Reverse duels from Locked to Available — matching what the real game would require to have reached that duel at all. (The series' very first Reverse duel is exempt, since it's always locked in-game regardless of forward progress.)

---

*Dates reflect when each change was made during development, not necessarily a packaged release.*
