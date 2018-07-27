using System;
using System.IO;
using System.Text;

namespace Elchwinkel.Spc.Internal
{
    internal class LogBlockWriter
    {
        public static byte[] Encode(LogBlock b)
        {
            const int BLOCK_SIZE = 4096;

            var txtSection = String.IsNullOrEmpty(b?.TextData?.Raw) ? Array.Empty<byte>() : Encoding.ASCII.GetBytes(b.TextData.Raw);
            var binSection = b.BinaryData ?? Array.Empty<byte>();
            if (txtSection.Length == 0 && binSection.Length == 0) return Array.Empty<byte>();

            var size = Constants.LOGHEADER_LENGTH + binSection.Length + txtSection.Length;
            var h = new LogHeader
            {
                SizeBinarySection = binSection.Length,
                OffsetTextSection = Constants.LOGHEADER_LENGTH + binSection.Length,
                Size = size,
                SizeMemoryBlock = BLOCK_SIZE * (int)Math.Ceiling(size / (double)BLOCK_SIZE),
                SizePrivateBinarySection = 0
            };
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(h.Size);
                    bw.Write(h.SizeMemoryBlock);
                    bw.Write(h.OffsetTextSection);
                    bw.Write(h.SizeBinarySection);
                    bw.Write(h.SizePrivateBinarySection);
                    bw.Write(h.LogSpar);
                    bw.Write(binSection);
                    bw.Write(txtSection);
                    return ms.ToArray();
                }
            }
        }
    }
}