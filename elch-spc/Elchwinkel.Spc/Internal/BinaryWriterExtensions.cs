using System;
using System.IO;
using System.Text;

namespace Elchwinkel.Spc.Internal
{
    internal static class BinaryWriterExtensions
    {
        public static void WriteFixedSizedString(this BinaryWriter bw, string str, int size)
        {
            var buf = new byte[size];
            var bytes = Encoding.ASCII.GetBytes(str);
            Array.Copy(bytes, 0, buf, 0, Math.Min(size, bytes.Length));
            bw.Write(buf);
        }
    }
}