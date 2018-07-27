namespace Elchwinkel.Spc.Internal
{
    internal sealed class SubFile
    {
        public SubHeader Header { get; set; }
        public double[] XValues { get; set; }
        public double[] YValues { get; set; }
    }
}