using System;
using System.Collections.Generic;
using System.Linq;

namespace Elchwinkel.Spc.Internal
{
    internal static class Helper
    {
        public static uint CompressDateTime(DateTime dt)
        {
            return ((uint) dt.Year << 20) & ((uint) dt.Month << 16) & ((uint) dt.Day << 11) & ((uint) dt.Hour << 6) &
                   (uint) dt.Minute;
        }

        public static DateTime? ParseCompressedDateTime(uint compressed)
        {
            uint Mask(byte size)
            {
                return (1u << size) - 1u;
            }

            var year = (compressed >> 20) & Mask(12);
            var month = (compressed >> 16) & Mask(4);
            var day = (compressed >> 11) & Mask(5);
            var hour = (compressed >> 6) & Mask(5);
            var minute = (compressed >> 0) & Mask(6);
            try
            {
                return new DateTime((int) year, (int) month, (int) day, (int) hour, (int) minute, 0);
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<double> ParseXValues(byte[] bytes, int points, int offset)
        {
            for (var i = 0; i < points; i++)
            {
                var xVal = BitConverter.ToSingle(bytes, offset + i * 4);
                yield return xVal;
            }
        }

        public static double[] ParseYValues(Header header, SubHeader subHeader, byte[] bytes, int offset)
        {
            var shortPrecision = header.FileType.HasFlag(FileType.YDataWith16BitPrecision);
            int points;
            if (header.NumberOfPointsInFile == 0 && subHeader.NumberOfPoints != 0)
                points = subHeader.NumberOfPoints;
            else if (header.NumberOfPointsInFile != 0 && subHeader.NumberOfPoints == 0)
                points = header.NumberOfPointsInFile;
            else
                points = header.FileType.HasFlag(FileType.Multifile) && subHeader.NumberOfPoints != 0
                    ? subHeader.NumberOfPoints
                    : header.NumberOfPointsInFile;

            if (subHeader.ExponentForYValues == 0x80)
            {
                var values = new List<double>();
                for (var i = 0; i < header.NumberOfPointsInFile; i++)
                    values.Add(BitConverter.ToSingle(bytes, offset + i * 4));
                return values.ToArray();
            }

            var integers = new List<int>();
            for (var i = 0; i < points; i++)
            {
                var val = shortPrecision
                    ? BitConverter.ToInt16(bytes, offset + i * 2)
                    : BitConverter.ToInt32(bytes, offset + i * 4);
                integers.Add(val);
            }

            var yValues = integers.Select(i => Math.Pow(2, subHeader.ExponentForYValues) * (double) i /
                                               Math.Pow(2, 32)).ToArray();
            return yValues;
        }
    }
}