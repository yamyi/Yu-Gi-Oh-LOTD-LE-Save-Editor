using System.Text;
using System.Xml.Linq;

namespace YuGiOhSaveEditor.Services
{
    public sealed class SlotIO
    {
        public record SlotInfo(
            int SlotNumber,
            int SlotBaseOffset,
            string Name,
            byte OwnerId,
            ushort Main,
            ushort Extra,
            ushort Side,
            bool IsEmpty
        )
        {


            public string OwnerName => OwnerDatabase.GetName(OwnerId);
            public string Display => $"#{SlotNumber}: {Name} (M{Main}/E{Extra}/S{Side})";

            public override string ToString() => Display;
        }


        public List<SlotInfo> ReadAllSlots(byte[] saveBytes)
        {
            var slots = new SlotInfo[SlotLayout.SlotCount];

            for (int i = 0; i < SlotLayout.SlotCount; i++)
            {
                int slotNumber = i + 1;
                int slotBase = SlotLayout.Slot1Offset + (i * SlotLayout.SlotStrideBytes);

                ushort main = ReadU16LE(saveBytes, slotBase + SlotLayout.MainCountOffset);
                ushort extra = ReadU16LE(saveBytes, slotBase + SlotLayout.ExtraCountOffset);
                ushort side = ReadU16LE(saveBytes, slotBase + SlotLayout.SideCountOffset);


                string name = ReadFixedUtf16Field(saveBytes, slotBase + SlotLayout.NameOffset, SlotLayout.NameFieldByteLength);

                byte ownerId = ReadU8(saveBytes, slotBase + SlotLayout.OwnerIdOffset);

                bool empty = ReadU8(saveBytes, slotBase + SlotLayout.OccupancyFlagOffset) == 0;

                slots[i] = new SlotInfo(
                    SlotNumber: slotNumber,
                    SlotBaseOffset: slotBase,
                    Name: name,
                    OwnerId: ownerId,
                    Main: main,
                    Extra: extra,
                    Side: side,
                    IsEmpty: empty
                );
            }

            return slots.ToList();
        }

        public SlotInfo ReadSlot(byte[] saveBytes, int index)
        {

            int slotNumber = index;
            int slotBase = SlotLayout.Slot1Offset + ((index - 1) * SlotLayout.SlotStrideBytes);

            ushort main = ReadU16LE(saveBytes, slotBase + SlotLayout.MainCountOffset);
            ushort extra = ReadU16LE(saveBytes, slotBase + SlotLayout.ExtraCountOffset);
            ushort side = ReadU16LE(saveBytes, slotBase + SlotLayout.SideCountOffset);


            string name = ReadFixedUtf16Field(saveBytes, slotBase + SlotLayout.NameOffset, SlotLayout.NameFieldByteLength);

            byte ownerId = ReadU8(saveBytes, slotBase + SlotLayout.OwnerIdOffset);

            bool empty = ReadU8(saveBytes, slotBase + SlotLayout.OccupancyFlagOffset) == 0;

            var slot = new SlotInfo(
                SlotNumber: slotNumber,
                SlotBaseOffset: slotBase,
                Name: name,
                OwnerId: ownerId,
                Main: main,
                Extra: extra,
                Side: side,
                IsEmpty: empty
            );

            return slot;
        }
        public void CopySlotBlock(byte[] saveBytes, int srcSlotBase, int dstSlotBase)
        {
            int len = SlotLayout.SlotStrideBytes;
            if (srcSlotBase < 0 || dstSlotBase < 0) throw new ArgumentOutOfRangeException();
            if (srcSlotBase + len > saveBytes.Length) throw new ArgumentOutOfRangeException(nameof(srcSlotBase));
            if (dstSlotBase + len > saveBytes.Length) throw new ArgumentOutOfRangeException(nameof(dstSlotBase));

            Buffer.BlockCopy(saveBytes, srcSlotBase, saveBytes, dstSlotBase, len);
        }

        public void SwapSlotBlocks(byte[] saveBytes, int slotBaseA, int slotBaseB)
        {
            int len = SlotLayout.SlotStrideBytes;
            if (slotBaseA < 0 || slotBaseB < 0) throw new ArgumentOutOfRangeException();
            if (slotBaseA + len > saveBytes.Length) throw new ArgumentOutOfRangeException(nameof(slotBaseA));
            if (slotBaseB + len > saveBytes.Length) throw new ArgumentOutOfRangeException(nameof(slotBaseB));
            if (slotBaseA == slotBaseB) return;

            byte[] temp = new byte[len];
            Buffer.BlockCopy(saveBytes, slotBaseA, temp, 0, len);
            Buffer.BlockCopy(saveBytes, slotBaseB, saveBytes, slotBaseA, len);
            Buffer.BlockCopy(temp, 0, saveBytes, slotBaseB, len);
        }

        private static ushort ReadU16LE(byte[] bytes, int offset)
        {
            if (offset < 0 || offset + 1 >= bytes.Length) return 0;
            return (ushort)(bytes[offset] | (bytes[offset + 1] << 8));
        }

        private static byte ReadU8(byte[] bytes, int offset)
        {
            if (offset < 0 || offset >= bytes.Length) return 0;
            return bytes[offset];
        }

        private static string ReadFixedUtf16Field(byte[] bytes, int offset, int byteLen)
        {
            if (offset < 0 || offset + byteLen > bytes.Length) return "";
            string s = Encoding.Unicode.GetString(bytes, offset, byteLen);
            int nul = s.IndexOf('\0');
            if (nul >= 0) s = s[..nul];
            return s;
        }

        public (ushort MainCount, ushort ExtraCount, ushort SideCount, ushort[] MainAll, ushort[] ExtraAll, ushort[] SideAll)
            ReadLOTDDeckCards(byte[] saveBytes, SlotInfo slot)
        {
            int slotbase = slot.SlotBaseOffset;

            ushort mainCount = ReadU16LE(saveBytes, slotbase+SlotLayout.MainCountOffset);
            ushort extraCount = ReadU16LE(saveBytes, slotbase + SlotLayout.ExtraCountOffset);
            ushort sideCount = ReadU16LE(saveBytes, slotbase + SlotLayout.SideCountOffset);

            

            var mainAll = new ushort[60];
            var extraAll = new ushort[15];
            var sideAll = new ushort[15];

            int cardDataOffset = slotbase + SlotLayout.CardDataOffset;

            for (int i = 0; i < 60; i++, cardDataOffset += 2) mainAll[i] = ReadU16LE(saveBytes, cardDataOffset);
            for (int i = 0; i < 15; i++, cardDataOffset += 2) extraAll[i] = ReadU16LE(saveBytes, cardDataOffset);
            for (int i = 0; i < 15; i++, cardDataOffset += 2) sideAll[i] = ReadU16LE(saveBytes, cardDataOffset);

            return (mainCount, extraCount, sideCount, mainAll, extraAll, sideAll);
        }

        /// <summary>Reads a slot's cards back out as real Card objects (via LOTD id lookup),
        /// e.g. for exporting to a .ydk file. Unrecognized/zero ids are dropped.</summary>
        public Deck ReadDeck(byte[] saveBytes, SlotInfo slot, CardDatabase db)
        {
            var (mainCount, extraCount, sideCount, mainAll, extraAll, sideAll) = ReadLOTDDeckCards(saveBytes, slot);

            List<Card> ToCards(ushort[] all, int count) =>
                all.Take(count)
                   .Where(id => id != 0)
                   .Select(id => db.GetByLotdId(id))
                   .Where(card => card is not null)
                   .Select(card => card!)
                   .ToList();

            return new Deck(ToCards(mainAll, mainCount), ToCards(extraAll, extraCount), ToCards(sideAll, sideCount));
        }

        public void WriteSlot(byte[] saveBytes, int index, ushort[] mainCards, ushort[] extraCards, ushort[] sideCards, string name, byte ownerId)
        {
            if (mainCards.Length > 60) throw new ArgumentException("Main deck cannot exceed 60 cards.", nameof(mainCards));
            if (extraCards.Length > 15) throw new ArgumentException("Extra deck cannot exceed 15 cards.", nameof(extraCards));
            if (sideCards.Length > 15) throw new ArgumentException("Side deck cannot exceed 15 cards.", nameof(sideCards));

            int slotBase = SlotLayout.Slot1Offset + ((index - 1) * SlotLayout.SlotStrideBytes);
            int countsOffset = slotBase + SlotLayout.MainCountOffset;

            // Write counts
            WriteU16LE(saveBytes, countsOffset, (ushort)mainCards.Length);
            WriteU16LE(saveBytes, countsOffset + 2, (ushort)extraCards.Length);
            WriteU16LE(saveBytes, countsOffset + 4, (ushort)sideCards.Length);

            // Write cards (fixed-size slots, zero-padded)
            int pos = countsOffset + 6;

            for (int i = 0; i < 60; i++, pos += 2)
                WriteU16LE(saveBytes, pos, i < mainCards.Length ? mainCards[i] : (ushort)0);

            for (int i = 0; i < 15; i++, pos += 2)
                WriteU16LE(saveBytes, pos, i < extraCards.Length ? extraCards[i] : (ushort)0);

            for (int i = 0; i < 15; i++, pos += 2)
                WriteU16LE(saveBytes, pos, i < sideCards.Length ? sideCards[i] : (ushort)0);

            // Write name and owner
            WriteFixedUtf16Field(saveBytes, slotBase + SlotLayout.NameOffset, SlotLayout.NameFieldByteLength, name);
            WriteU8(saveBytes, slotBase + SlotLayout.OwnerIdOffset, ownerId);

            WriteU8(saveBytes, slotBase + SlotLayout.OccupancyFlagOffset, 1);
            WriteFixedTimestamps(saveBytes, slotBase);
        }

        public void WriteSlot(byte[] saveBytes, int index, Deck deck, string name, byte ownerId)
        {
            if (deck.main.Count > 60) throw new ArgumentException("Main deck cannot exceed 60 cards.", nameof(deck.main));
            if (deck.extra.Count > 15) throw new ArgumentException("Extra deck cannot exceed 15 cards.", nameof(deck.extra));
            if (deck.side.Count > 15) throw new ArgumentException("Side deck cannot exceed 15 cards.", nameof(deck.side));

            int slotBase = SlotLayout.Slot1Offset + ((index - 1) * SlotLayout.SlotStrideBytes);

            ushort[] mainCards = deck.Get_Lotd_Main();
            ushort[] extraCards = deck.Get_Lotd_Extra();
            ushort[] sideCards = deck.Get_Lotd_Side();

            int countsOffset = slotBase + SlotLayout.MainCountOffset;
            // Write counts
            WriteU16LE(saveBytes, countsOffset, (ushort)mainCards.Length);
            WriteU16LE(saveBytes, countsOffset + 2, (ushort)extraCards.Length);
            WriteU16LE(saveBytes, countsOffset + 4, (ushort)sideCards.Length);

            // Write cards (fixed-size slots, zero-padded)
            int pos = countsOffset + 6;

            for (int i = 0; i < 60; i++, pos += 2)
                WriteU16LE(saveBytes, pos, i < mainCards.Length ? mainCards[i] : (ushort)0);

            for (int i = 0; i < 15; i++, pos += 2)
                WriteU16LE(saveBytes, pos, i < extraCards.Length ? extraCards[i] : (ushort)0);

            for (int i = 0; i < 15; i++, pos += 2)
                WriteU16LE(saveBytes, pos, i < sideCards.Length ? sideCards[i] : (ushort)0);

            // Write name and owner
            WriteFixedUtf16Field(saveBytes, slotBase + SlotLayout.NameOffset, SlotLayout.NameFieldByteLength, name);
            WriteU8(saveBytes, slotBase + SlotLayout.OwnerIdOffset, ownerId);
            WriteU8(saveBytes, slotBase + SlotLayout.OccupancyFlagOffset, 1);
            WriteFixedTimestamps(saveBytes, slotBase);

        }
        private static void WriteU16LE(byte[] bytes, int offset, ushort value)
        {
            if (offset < 0 || offset + 1 >= bytes.Length) return;
            bytes[offset] = (byte)(value & 0xFF);
            bytes[offset + 1] = (byte)(value >> 8);
        }
        private static void WriteU8(byte[] bytes, int offset, byte value)
        {
            if ((uint)offset >= (uint)bytes.Length) return;
            bytes[offset] = value;
        }

        private static void WriteFixedUtf16Field(byte[] bytes, int offset, int byteLen, string value)
        {
            if ((uint)offset + (uint)byteLen > (uint)bytes.Length) return;

            // Clear old name (critical for shorter names)
            Array.Clear(bytes, offset, byteLen);

            if (string.IsNullOrEmpty(value))
                return;

            byte[] encoded = Encoding.Unicode.GetBytes(value);

            int len = Math.Min(encoded.Length, byteLen);
            Buffer.BlockCopy(encoded, 0, bytes, offset, len);
        }

        public void UpdateSlotOwner(
            byte[] saveBytes,
            SlotInfo slot,
            byte newOwnerId)
        {
            int baseOffset = slot.SlotBaseOffset;

            // OwnerId (0x120)
            WriteU8(
                saveBytes,
                baseOffset + SlotLayout.OwnerIdOffset,
                newOwnerId
            );
        }

        public void UpdateName(
           byte[] saveBytes,
           SlotInfo slot,
           string newName)
        {
            int baseOffset = slot.SlotBaseOffset;

            // Name (0x00)
            WriteFixedUtf16Field(
                saveBytes,
                baseOffset + SlotLayout.NameOffset,
                SlotLayout.NameFieldByteLength,
                newName
            );

        }




        public void WriteFixedTimestamps(byte[] saveBytes, int slotBase)
        {
            byte[] Timestamp1 =
            { 0xEA, 0x07, 0x00, 0x0C, 0x3D, 0x77, 0x7E, 0x00, 0x1D, 0x01, 0x00, 0x00 };

           byte[] Timestamp2 =
            { 0xEA, 0x07, 0x00, 0x0C, 0x3D, 0x77, 0x9E, 0x00, 0xFE, 0x01, 0x00, 0x00 };

        Buffer.BlockCopy(Timestamp1, 0, saveBytes, slotBase + SlotLayout.CreatedTimestampOffset, 12);
            Buffer.BlockCopy(Timestamp2, 0, saveBytes, slotBase + SlotLayout.ModifiedTimestampOffset, 12);
        }


        public void ClearDeck(
            byte[] saveBytes,
            int index)
        {
            int slotBase = SlotLayout.Slot1Offset + ((index - 1) * SlotLayout.SlotStrideBytes);
            // Clear counts (Main, Extra, Side)
            Array.Clear(saveBytes, slotBase, SlotLayout.SlotStrideBytes);

        }
    }
}