namespace Elchwinkel.Spc.Internal
{
    internal class SubHeader
    {
        public byte Flags { get; set; }
        public byte ExponentForYValues { get; set; } = 0x80;
        public short IndexNumber { get; set; }
        public float StartingZValue { get; set; }
        public float EndingZValue { get; set; }
        public float NoiseValue { get; set; }
        public int NumberOfPoints { get; set; }
        public int NumberOfCoAddedScans { get; set; }
        public float WAxisValue { get; set; }
        public byte[] Reserved { get; set; } = new byte[4];
    }
}