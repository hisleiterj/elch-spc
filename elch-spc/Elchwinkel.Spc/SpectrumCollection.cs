using System;
using System.Collections.Generic;

namespace Elchwinkel.Spc
{
    public class SpectrumCollection
    {
        public Spectrum[] Spectra { get; set; }
        public XZUnit XUnit { get; set; } = XZUnit.Arbitrary;
        public YUnit YUnit { get; set; } = YUnit.ArbitraryIntensity;
        public string Memo { get; set; } = string.Empty;
        public string ResolutionDescription { get; set; } = string.Empty;
        public string SourceInstrument { get; set; } = string.Empty;
        public DateTime? Date { get; set; } = DateTime.Now;
        public IDictionary<string, string> MetaData { get; set; }
        public byte[] BinaryData { get; set; }
    }
}