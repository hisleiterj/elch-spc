using System;
using System.Text;

namespace Elchwinkel.Spc.Internal
{
    internal class LogBlockParser
    {
        public static LogBlock Parse(byte[] bytes, int offset)
        {
            if (offset == 0) return LogBlock.Empty;
            var header = ParseHeader(bytes, offset);
            var txt = Encoding.ASCII.GetString(bytes, offset + header.OffsetTextSection,
                header.Size - header.OffsetTextSection);
            var textData = new LogTextData(txt);
            return new LogBlock(textData);
        }

        private static LogHeader ParseHeader(byte[] bytes, int offset)
        {
            return new LogHeader
            {
                Size = BitConverter.ToInt32(bytes, offset + 0),
                SizeMemoryBlock = BitConverter.ToInt32(bytes, offset + 4),
                OffsetTextSection = BitConverter.ToInt32(bytes, offset + 8),
                SizeBinarySection = BitConverter.ToInt32(bytes, offset + 12),
                SizePrivateBinarySection = BitConverter.ToInt32(bytes, offset + 16)
            };
        }
    }
}