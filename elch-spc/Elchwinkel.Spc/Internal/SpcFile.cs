using System.Collections.Generic;

namespace Elchwinkel.Spc.Internal
{
    internal class SpcFile
    {
        public Header Header { get; set; }
        public IReadOnlyList<SubFile> SubFiles { get; set; }
        public LogBlock LogBlock { get; set; } = LogBlock.Empty;
    }
}