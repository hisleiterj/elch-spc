using System;

namespace Elchwinkel.Spc.Internal
{
    internal class Header
    {
        public FileType FileType { get; set; } = FileType.SingleFileEvenlySpacedXValues;
        public FileVersion Version { get; set; } = FileVersion.NewFormat;
        public ExperimentType ExperimentType { get; set; } = ExperimentType.SPCGEN;
        public byte ExponentForYValues { get; set; } = 0x80;
        public int NumberOfPointsInFile { get; set; }
        public double FirstXCoordinate { get; set; }
        public double LastXCoordinate { get; set; }
        public int NumberOfSubfiles { get; set; }
        public byte XUnitsTypeCode { get; set; }
        public XZUnit XUnit => XZUnit.FromCode(XUnitsTypeCode);
        public byte YUnitsTypeCode { get; set; }
        public YUnit YUnit => YUnit.FromCode(YUnitsTypeCode);
        public byte ZUnitsTypeCode { get; set; }
        public XZUnit ZUnit => XZUnit.FromCode(ZUnitsTypeCode);
        public byte PostingDisposition { get; set; }
        public uint CompressedDate { get; set; } = Helper.CompressDateTime(DateTime.Now);
        public DateTime? Timestamp => Helper.ParseCompressedDateTime(CompressedDate);
        public string ResolutionDescriptionText { get; set; } = string.Empty;
        public string SourceInstrumentDescriptionText { get; set; } = string.Empty;
        public short PeakPointNumberForInterferograms { get; set; }
        public float[] Spare { get; set; } = new float[8];
        public string Memo { get; set; } = string.Empty;
        public string XyzAxisStrings { get; set; } = string.Empty;
        public int ByteOffsetToLogBlock { get; set; }
        public int FileModificationFlag { get; set; }
        public byte ProcessingCode { get; set; }
        public byte CalibrationLevel { get; set; }
        public short SubMethodSampleInjectionNumber { get; set; }
        public float FloatingDataMultiplierConcentrationFactor { get; set; }
        public string MethodFile { get; set; } = string.Empty;
        public float ZSubfileIncrementForEvenZMultifiles { get; set; }
        public int NumberOfWPlanes { get; set; }
        public float WPlaneIncrement { get; set; }
        public byte WAxisUnitsCode { get; set; }
        public string Reserved { get; set; } = string.Empty;
    }
}