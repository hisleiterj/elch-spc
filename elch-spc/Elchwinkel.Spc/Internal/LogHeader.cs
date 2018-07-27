namespace Elchwinkel.Spc.Internal
{
    internal class LogHeader
    {
        public int Size { get; set; }
        public int SizeMemoryBlock { get; set; }
        public int OffsetTextSection { get; set; }
        public int SizeBinarySection { get; set; }
        public int SizePrivateBinarySection { get; set; }
        public byte[] LogSpar { get; set; } = new byte[44];
    }
}