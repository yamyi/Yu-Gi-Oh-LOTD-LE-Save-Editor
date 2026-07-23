
namespace YuGiOhSaveEditor.Services
{

    /// <summary>
    /// Port of Wolf/Elroy signature logic:
    /// - Increment UInt32 at 0x10
    /// - Zero signature field UInt32 at 0x0C
    /// - Compute signature over entire file with CRC-like table, seed 0xFFFFFFFF
    /// - Write signature back to 0x0C
    /// </summary>
    public static class SaveSignatureFixer
    {
        private const int SignatureOffset = 0x0C; // 12
        private const int CounterOffset = 0x10;   // 16

        private static readonly uint[] Table = BuildTable();

        public static void FixInPlace(byte[] saveBytes)
        {
            if (saveBytes == null) throw new ArgumentNullException(nameof(saveBytes));
            if (saveBytes.Length < 0x20) throw new ArgumentException("Save file too small.", nameof(saveBytes));

            // Increment counter at 0x10 (UInt32 LE)
            uint counter = ReadUInt32(saveBytes, CounterOffset);
            counter++;
            WriteUInt32(saveBytes, CounterOffset, counter);

            // Zero signature field before computing
            WriteUInt32(saveBytes, SignatureOffset, 0);

            // Compute signature
            uint sig = ComputeSignature(saveBytes);

            // Write signature back
            WriteUInt32(saveBytes, SignatureOffset, sig);
        }

        private static uint ComputeSignature(byte[] bytes)
        {
            uint v = 0xFFFFFFFF;

            for (int i = 0; i < bytes.Length; i++)
            {
                uint b = bytes[i];
                v = Table[(v ^ b) & 0xFF] ^ (v >> 8);
            }

            return v;
        }

        private static uint[] BuildTable()
        {
            var table = new uint[256];

            for (uint i = 0; i < 256; i++)
            {
                uint v = i;
                for (int j = 0; j < 8; j++)
                {
                    if ((v & 1) == 1)
                        v = (v >> 1) ^ 0xEDB88320;
                    else
                        v >>= 1;
                }
                table[i] = v;
            }

            return table;
        }

        private static uint ReadUInt32(byte[] bytes, int offset)
        {
            return (uint)(
                bytes[offset] |
                (bytes[offset + 1] << 8) |
                (bytes[offset + 2] << 16) |
                (bytes[offset + 3] << 24)
            );
        }

        private static void WriteUInt32(byte[] bytes, int offset, uint value)
        {
            bytes[offset] = (byte)(value & 0xFF);
            bytes[offset + 1] = (byte)((value >> 8) & 0xFF);
            bytes[offset + 2] = (byte)((value >> 16) & 0xFF);
            bytes[offset + 3] = (byte)((value >> 24) & 0xFF);
        }
    }
}
