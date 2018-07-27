using System;
using System.IO;
using System.Linq;
using Elchwinkel.Spc.Internal;

namespace Elchwinkel.Spc
{
    public static class SpcWriter
    {
        public static void ToFile(SpectrumCollection collection, string file)
        {
            var bytes = Encode(collection);
            File.WriteAllBytes(file, bytes);
        }
        private static bool _HaveSameXAxes(Spectrum[] spectra)
        {
            var first = spectra[0].X;
            return spectra.All(s => s.X.SequenceEqual(first));
        }
        private static SpcFile _ToSpcFile(SpectrumCollection collection)
        {
            var spectra = collection.Spectra;
            var fileType = (spectra.Length == 1 && spectra[0].IsXEvenSpaced)
                ? FileType.SingleFileEvenlySpacedXValues
                : (FileType.Multifile | FileType.XYFile | FileType.OwnXArray);
            if (spectra.Length > 1 && spectra[0].IsXEvenSpaced && _HaveSameXAxes(spectra))
            {
                fileType = FileType.Multifile;
            }
            if (spectra.Length > 1 && !spectra[0].IsXEvenSpaced && _HaveSameXAxes(spectra))
            {
                fileType = FileType.Multifile | FileType.XYFile;
            }
            return new SpcFile
            {
                Header = new Header
                {
                    NumberOfPointsInFile = spectra[0].Y.Length,
                    NumberOfSubfiles = spectra.Length,
                    FileType = fileType,
                    Memo = collection.Memo,
                    ExponentForYValues = 0x80,
                    XUnitsTypeCode = collection.XUnit.Code,
                    YUnitsTypeCode = collection.YUnit.Code,
                    FirstXCoordinate = spectra[0].X[0],
                    LastXCoordinate = spectra[0].X[spectra[0].Length - 1]
                },
                SubFiles = spectra.Select((spectrum, i) => new SubFile
                {
                    Header = new SubHeader
                    {
                        NumberOfPoints = spectrum.Length,
                        IndexNumber = (short)i
                    },
                    XValues = spectrum.X,
                    YValues = spectrum.Y

                }).ToArray(),
                LogBlock = collection.MetaData == null ? LogBlock.Empty : new LogBlock(collection.MetaData)
            };
        }
        public static byte[] Encode(SpectrumCollection collection)
        {
            var spc = _ToSpcFile(collection);

            var ms = new MemoryStream();
            var logOffset = Constants.HEADER_LENGTH;
            using (var bw = new BinaryWriter(ms))
            {
                var bb = spc.SubFiles.SelectMany(file => _EncodeSubFile(spc.Header, file)).ToArray();
                var fileType = spc.Header.FileType;
                if (fileType.HasFlag(FileType.XYFile) && !fileType.HasFlag(FileType.OwnXArray))
                {
                    bb = spc.SubFiles[0].XValues.SelectMany(d => BitConverter.GetBytes((float) d)).Concat(bb).ToArray();
                }

                logOffset += bb.Length;
                if (spc.LogBlock != LogBlock.Empty)
                {
                    spc.Header.ByteOffsetToLogBlock = logOffset;
                }

                bw.Write(_EncodeHeader(spc.Header));
                bw.Write(bb);

                if (spc.LogBlock != LogBlock.Empty)
                {
                    bw.Write(LogBlockWriter.Encode(spc.LogBlock));
                }
                return ms.ToArray();
            }
           
        }
        private static byte[] _EncodeHeader(Header h)
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write((byte)h.FileType);
                    bw.Write((byte)h.Version);
                    bw.Write((byte)h.ExperimentType);
                    bw.Write((byte)h.ExponentForYValues);
                    bw.Write(h.NumberOfPointsInFile);
                    bw.Write(h.FirstXCoordinate);
                    bw.Write(h.LastXCoordinate);
                    bw.Write(h.NumberOfSubfiles);
                    bw.Write(h.XUnitsTypeCode);
                    bw.Write(h.YUnitsTypeCode);
                    bw.Write(h.ZUnitsTypeCode);
                    bw.Write(h.PostingDisposition);
                    bw.Write(h.CompressedDate);
                    bw.WriteFixedSizedString(h.ResolutionDescriptionText, 9);
                    bw.WriteFixedSizedString(h.SourceInstrumentDescriptionText, 9);
                    bw.Write(h.PeakPointNumberForInterferograms);
                    foreach (var f in h.Spare)
                    {
                        bw.Write(f);
                    }
                    bw.WriteFixedSizedString(h.Memo, 130);
                    bw.WriteFixedSizedString(h.XyzAxisStrings, 30);
                    bw.Write(h.ByteOffsetToLogBlock);
                    bw.Write(h.FileModificationFlag);
                    bw.Write(h.ProcessingCode);
                    bw.Write(h.CalibrationLevel);
                    bw.Write(h.SubMethodSampleInjectionNumber);
                    bw.Write(h.FloatingDataMultiplierConcentrationFactor);
                    bw.WriteFixedSizedString(h.MethodFile, 48);
                    bw.Write(h.ZSubfileIncrementForEvenZMultifiles);
                    bw.Write(h.NumberOfWPlanes);
                    bw.Write(h.WPlaneIncrement);
                    bw.Write(h.WAxisUnitsCode);
                    bw.WriteFixedSizedString(h.Reserved, 187);
                    return ms.ToArray();
                }
            }
        }

        private static byte[] _EncodeSubFile(Header h, SubFile subfile)
        {
            var ms = new MemoryStream();
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(_EncodeSubHeader(subfile.Header));
                if (h.FileType.HasFlag(FileType.OwnXArray))
                {
                    foreach (var x in subfile.XValues)
                    {
                        bw.Write((float)x);
                    }
                }

                foreach (var yValue in subfile.YValues)
                {
                    bw.Write((float)yValue);
                }
                return ms.ToArray();
            }
        }
        private static byte[] _EncodeSubHeader(SubHeader sh)
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(sh.Flags);
                    bw.Write(sh.ExponentForYValues);
                    bw.Write(sh.IndexNumber);
                    bw.Write(sh.StartingZValue);
                    bw.Write(sh.EndingZValue);
                    bw.Write(sh.NoiseValue);
                    bw.Write(sh.NumberOfPoints);
                    bw.Write(sh.NumberOfCoAddedScans);
                    bw.Write(sh.WAxisValue);
                    bw.Write(sh.Reserved);
                    return ms.ToArray();
                }
            }
        }


    }
}