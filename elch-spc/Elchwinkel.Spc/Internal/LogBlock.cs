using System;
using System.Collections.Generic;
using System.Linq;

namespace Elchwinkel.Spc.Internal
{
    internal class LogBlock
    {
        public static readonly LogBlock Empty = new LogBlock(LogTextData.Empty);

        internal LogBlock(LogTextData textData)
        {
            TextData = textData;
        }

        public LogBlock(IDictionary<string, string> metaData, byte[] binaryData = null)
        {
            BinaryData = binaryData;
            var txt = string.Join(Environment.NewLine, metaData.Select(pair => $"{pair.Key}={pair.Value}"));
            TextData = new LogTextData(txt);
        }

        public byte[] BinaryData { get; }
        public LogTextData TextData { get; }
    }
}