using System;

namespace Elchwinkel.Spc.Internal
{
    [Flags]
    internal enum FileType : byte
    {
        SingleFileEvenlySpacedXValues = 0x0,
        YDataWith16BitPrecision = 0x01,
        UseExperimentExtension = 0x02,
        Multifile = 0x04,
        MultifileRandomZOrder = 0x08,
        MultifileZOrder = 0x10,
        UseCustomAxisLabels = 0x20,
        OwnXArray = 0x40,
        XYFile = 0x80,
    }
}