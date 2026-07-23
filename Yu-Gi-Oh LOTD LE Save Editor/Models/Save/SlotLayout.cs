
namespace YuGiOhSaveEditor.Services
{

    /// <summary>
    /// Memory layout of a single deck slot in the LOTD save file.
    ///
    /// The deck-slot array sits at a different absolute offset depending on
    /// which save format is loaded — see Slot1Offset/CurrentVersion below and
    /// LotdSaveFormat.cs (offsets there are ported from pixeltris/Lotd, the
    /// original reverse-engineered save-editing tool for this game). The
    /// field-level layout within a single 0x130-byte slot block was confirmed
    /// independently by binary diffing two saves (savegame.dat vs ful_1.dat)
    /// and is the same across both save formats.
    ///
    /// Slot array layout:
    ///   Slot 1 offset = Slot1Offset (version-dependent, see below)
    ///   slotBase            = countsOffset - CountsOffset
    ///   Each subsequent slot = slotBase + (n * SlotStrideBytes)
    /// 
    /// Full slot map (relative to slotBase):
    ///   0x000  Name          (64 bytes, UTF-16 LE, null-terminated, 32 chars max)
    ///   0x040  Main count    (uint16 LE)
    ///   0x042  Extra count   (uint16 LE)
    ///   0x044  Side count    (uint16 LE)
    ///   0x046  Card data     (180 bytes: 60 main + 15 extra + 15 side, each uint16 LE)
    ///   0x0FC  Created       (12 bytes, game-internal timestamp format)
    ///   0x108  Modified      (12 bytes, game-internal timestamp format)
    ///   0x120  OwnerId       (uint16 LE, from OwnerDatabase, e.g. 0x8A = 138 -> IN4-M8)
    ///   0x12C  Occupied      (byte: 0x01 = slot has a deck, 0x00 = empty)
    /// </summary>
    public static class SlotLayout
    {
        // ── Array geometry ────────────────────────────────────────────────────────

        /// Total number of deck slots in the save file
        public const int SlotCount = 32;

        /// Distance in bytes between the start of consecutive slot blocks.
        /// slotBase(n) = slotBase(0) + n * SlotStrideBytes
        public const int SlotStrideBytes = 0x130; // 304 bytes

        // ── Slot base anchor ─────────────────────────────────────────────────────

        /// Which save format is currently loaded — set by MainForm right after a
        /// save file is read (see LotdSaveFormat.DetectVersion). Defaults to
        /// LinkEvolution so behavior is unchanged for anyone who was already
        /// relying on the previous hardcoded offset before version detection
        /// existed.
        public static LotdSaveVersion CurrentVersion { get; set; } = LotdSaveVersion.LinkEvolution;

        /// Absolute offset of the first slot's counts field. Version-dependent —
        /// the original "Lotd" (2016) save format and the "Link Evolution"
        /// (2019+) format put the deck-slot array at different offsets because
        /// Link Evolution inserted a 6th duel series (VRAINS) and larger
        /// battle-pack/misc/card-list chunks ahead of it. See LotdSaveFormat.cs.
        public static int Slot1Offset => LotdSaveFormat.GetDecksOffset(CurrentVersion);


        // ── Fields (all offsets relative to slotBase) ─────────────────────────────

        /// Deck name. Fixed-width UTF-16 LE field, null-terminated.
        /// Capacity: 64 bytes = 32 UTF-16 characters.
        public const int NameOffset = 0x000;
        public const int NameFieldByteLength = 64;
        public const int NameMaxChars = NameFieldByteLength / 2; // 32


        /// Deck composition counts. Three consecutive uint16 LE values.
        ///   +0x042  Main  count (max 60)
        ///   +0x044  Extra count (max 15)
        ///   +0x046  Side  count (max 15)
        public const int MainCountOffset = 0x042; // relative to countsOffset
        public const int ExtraCountOffset = 0x044;
        public const int SideCountOffset = 0x046;


        /// Card ID array. Starts immediately after the three count fields.
        /// Layout: 60 × uint16 main, 15 × uint16 extra, 15 × uint16 side.
        /// Unused slots are zero-padded to the fixed maximum size.
        public const int CardDataOffset = 0x048;
        public const int MaxMainCards = 60;
        public const int MaxExtraCards = 15;
        public const int MaxSideCards = 15;
        public const int CardDataByteLength = (MaxMainCards + MaxExtraCards + MaxSideCards) * sizeof(ushort); // 180


        /// Game-internal creation timestamp (12 bytes).
        /// Byte layout (confirmed from binary diff):
        ///   [0-1]  Year  (uint16 LE, e.g. 0x07EA = 2026)
        ///   [1-11] Unknown game-internal fields — use fixed values from a real save
        public const int CreatedTimestampOffset = 0x0FC;
        public const int TimestampByteLength = 12;


        /// Game-internal last-modified timestamp (12 bytes). Same format as Created.
        public const int ModifiedTimestampOffset = 0x108;


        /// Owner character ID (uint16 LE).
        /// Index into the game's character table (e.g. 0x008A = 138 -> IN4-M8).
        public const int OwnerIdOffset = 0x120;


        /// Slot occupancy flag (single byte).
        /// 0x01 = slot contains a deck and will be shown in-game.
        /// 0x00 = slot is empty (game ignores all other fields).
        public const int OccupancyFlagOffset = 0x12C;

        // ── Fixed timestamp blobs (extracted from a known-good save) ──────────────


        /// Fixed creation timestamp copied from a verified real save.
        /// Used when writing a new deck slot from scratch.

        public static readonly byte[] FixedCreatedTimestamp =
            { 0xEA, 0x07, 0x00, 0x0C, 0x3D, 0x77, 0x7E, 0x00, 0x1D, 0x01, 0x00, 0x00 };


        /// Fixed modified timestamp copied from a verified real save.

        public static readonly byte[] FixedModifiedTimestamp =
            { 0xEA, 0x07, 0x00, 0x0C, 0x3D, 0x77, 0x9E, 0x00, 0xFE, 0x01, 0x00, 0x00 };

        // ── Derived helpers ───────────────────────────────────────────────────────

        /// Returns the absolute slotBase offset for a 1-based slot index.</summary>
        public static int GetSlotBase(int slotIndex)
        {
            if (slotIndex < 1 || slotIndex > SlotCount)
                throw new ArgumentOutOfRangeException(nameof(slotIndex), $"Slot must be 1–{SlotCount}.");

            int countsOffset = Slot1Offset + (slotIndex - 1) * SlotStrideBytes;
            return countsOffset;
        }

        /// Returns the absolute countsOffset for a 1-based slot index.</summary>
        public static int GetCountsOffset(int slotIndex) =>
            GetSlotBase(slotIndex);
    }
}

