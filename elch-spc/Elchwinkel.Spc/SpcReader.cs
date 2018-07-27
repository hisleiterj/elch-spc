using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Elchwinkel.Spc.Internal;
using static Elchwinkel.Spc.Internal.Constants;

namespace Elchwinkel.Spc
{
    public static class SpcReader
    {
        public static SpectrumCollection Read(string filePath)
        {
            return Read(File.ReadAllBytes(filePath));
        }

        private static Header _ParseHeader(byte[] bytes)
        {
            using (var br = new BinaryReader(new MemoryStream(bytes)))
            {
                var fileType = (FileType) br.ReadByte();
                var fileVersion = (FileVersion) br.ReadByte();
                if (fileVersion != FileVersion.NewFormat)
                    throw new InvalidSpcFileException("Only New Format (FileVersion=0x4B) is supported!");
                return new Header
                {
                    FileType = fileType,
                    Version = fileVersion,
                    ExperimentType = (ExperimentType) br.ReadByte(),
                    ExponentForYValues = br.ReadByte(),
                    NumberOfPointsInFile = br.ReadInt32(),
                    FirstXCoordinate = br.ReadDouble(),
                    LastXCoordinate = br.ReadDouble(),
                    NumberOfSubfiles = br.ReadInt32(),
                    XUnitsTypeCode = br.ReadByte(),
                    YUnitsTypeCode = br.ReadByte(),
                    ZUnitsTypeCode = br.ReadByte(),
                    PostingDisposition = br.ReadByte(),
                    CompressedDate = br.ReadUInt32(),
                    ResolutionDescriptionText = Encoding.ASCII.GetString(br.ReadBytes(9)).TrimEnd('\0'),
                    SourceInstrumentDescriptionText = Encoding.ASCII.GetString(br.ReadBytes(9)).TrimEnd('\0'),
                    PeakPointNumberForInterferograms = br.ReadInt16(),
                    Spare = Enumerable.Range(0, 8).Select(i => br.ReadSingle()).ToArray(),
                    Memo = Encoding.ASCII.GetString(br.ReadBytes(130)).TrimEnd('\0'),
                    XyzAxisStrings = Encoding.ASCII.GetString(br.ReadBytes(30)),
                    ByteOffsetToLogBlock = br.ReadInt32(),
                    FileModificationFlag = br.ReadInt32(),
                    ProcessingCode = br.ReadByte(),
                    CalibrationLevel = br.ReadByte(),
                    SubMethodSampleInjectionNumber = br.ReadInt16(),
                    FloatingDataMultiplierConcentrationFactor = br.ReadSingle(),
                    MethodFile = Encoding.ASCII.GetString(br.ReadBytes(48)).TrimEnd('\0'),
                    ZSubfileIncrementForEvenZMultifiles = br.ReadSingle(),
                    NumberOfWPlanes = br.ReadInt32(),
                    WPlaneIncrement = br.ReadSingle(),
                    WAxisUnitsCode = br.ReadByte(),
                    Reserved = Encoding.ASCII.GetString(br.ReadBytes(187)).TrimEnd('\0')
                };
            }
        }

        private static SubHeader _ParseSubHeader(byte[] bytes, int offset)
        {
            return new SubHeader
            {
                Flags = bytes[offset + 0],
                ExponentForYValues = bytes[offset + 1],
                IndexNumber = BitConverter.ToInt16(bytes, offset + 2),
                StartingZValue = BitConverter.ToSingle(bytes, offset + 4),
                EndingZValue = BitConverter.ToSingle(bytes, offset + 8),
                NoiseValue = BitConverter.ToSingle(bytes, offset + 12),
                NumberOfPoints = BitConverter.ToInt32(bytes, offset + 16),
                NumberOfCoAddedScans = BitConverter.ToInt32(bytes, offset + 20),
                WAxisValue = BitConverter.ToSingle(bytes, offset + 24),
                Reserved = new byte[4]
            };
        }

        private static int _ParseSubFile(Header header, byte[] bytes, int offset, out SubFile subFile)
        {
            var shortPrecision = header.FileType.HasFlag(FileType.YDataWith16BitPrecision);


            var subHeader = _ParseSubHeader(bytes, offset);
            offset += SUBHEADER_LENGTH;
            if (header.FileType.HasFlag(FileType.OwnXArray))
            {
                var x = Helper.ParseXValues(bytes, subHeader.NumberOfPoints, offset).Select(Convert.ToDouble).ToArray();
                offset += subHeader.NumberOfPoints *
                          (header.FileType.HasFlag(FileType.YDataWith16BitPrecision) ? 2 : 4);
                var y = Helper.ParseYValues(header, subHeader, bytes, offset);
                subFile = new SubFile
                {
                    Header = subHeader,
                    XValues = x,
                    YValues = y
                };
                return SUBHEADER_LENGTH + subHeader.NumberOfPoints * 4 +
                       subHeader.NumberOfPoints * (shortPrecision ? 2 : 4);
            }

            IEnumerable<double> xValues = null;
            if (header.FileType.HasFlag(FileType.XYFile))
            {
                xValues = Array.Empty<double>();
            }
            else
            {
                var x0 = header.FirstXCoordinate;
                var xn = header.LastXCoordinate;
                var delta = (xn - x0) / (header.NumberOfPointsInFile - 1);
                var xx = new List<double>();
                for (var i = 0; i < header.NumberOfPointsInFile; i++) xx.Add(x0 + i * delta);

                xValues = xx;
            }

            var yValues = Helper.ParseYValues(header, subHeader, bytes, offset);
            subFile = new SubFile
            {
                Header = subHeader,
                XValues = xValues.ToArray(),
                YValues = yValues
            };
            return SUBHEADER_LENGTH + header.NumberOfPointsInFile * 4;
        }

        public static SpectrumCollection Read(byte[] bytes)
        {
            var header = _ParseHeader(bytes);
            var offset = HEADER_LENGTH;

            var fileType = header.FileType;
            IEnumerable<double> xValues = null;
            if (fileType.HasFlag(FileType.XYFile) && !fileType.HasFlag(FileType.OwnXArray))
            {
                xValues = Helper.ParseXValues(bytes, header.NumberOfPointsInFile, offset);
                offset += header.NumberOfPointsInFile * 4;
            }

            var subFiles = new List<SubFile>();
            for (var i = 0; i < header.NumberOfSubfiles; i++)
            {
                offset += _ParseSubFile(header, bytes, offset, out var subFile);
                if (fileType.HasFlag(FileType.XYFile) && !fileType.HasFlag(FileType.OwnXArray))
                    subFile.XValues = xValues.ToArray();
                subFiles.Add(subFile);
            }

            var logBlock = LogBlock.Empty;
            if (header.ByteOffsetToLogBlock != 0)
            {
                if (offset != header.ByteOffsetToLogBlock)
                    throw new InvalidSpcFileException($"Invalid Log-File Offset");
                logBlock = LogBlockParser.Parse(bytes, offset);
            }

            var spcFile = new SpcFile
            {
                Header = header,
                SubFiles = subFiles,
                LogBlock = logBlock
            };

            return new SpectrumCollection
            {
                Memo = spcFile.Header.Memo,
                BinaryData = spcFile.LogBlock.BinaryData,
                Date = spcFile.Header.Timestamp,
                ResolutionDescription = spcFile.Header.ResolutionDescriptionText,
                SourceInstrument = spcFile.Header.SourceInstrumentDescriptionText,
                XUnit = spcFile.Header.XUnit,
                YUnit = spcFile.Header.YUnit,
                MetaData = spcFile.LogBlock.TextData.KeyValues,
                Spectra = spcFile.SubFiles.Select(file => new Spectrum(file.XValues, file.YValues)).ToArray()
            };
        }
    }
}