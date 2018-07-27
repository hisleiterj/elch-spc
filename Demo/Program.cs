using System;
using System.Collections.Generic;
using Elchwinkel.Spc;
using System.IO;

namespace Demo
{

    class Program
    {
        static void Main(string[] args)
        {
            _ReadSpcFileAndPrintInfo(args[0]);
            _WriteSpcFile(args[1]);
        }

        private static void _ReadSpcFileAndPrintInfo(string file)
        {
            var spectra = SpcReader.Read(file);

            Console.WriteLine($"File: {Path.GetFileName(file)}");
            Console.WriteLine($"Memo: {spectra.Memo}");
            Console.WriteLine("Meta-Data-Values:");
            foreach (var kv in spectra.MetaData)
            {
                Console.WriteLine($"    {kv.Key} = {kv.Value}");
            }
        }

        private static void _WriteSpcFile(string file)
        {
            var xAxes = new[] {1, 2, 3.0, 4, 5, 60, 70, 71, 72, 73};
            var spectra = new SpectrumCollection
            {
                Memo = "Polystyrene",
                XUnit = XZUnit.RamanShift,
                YUnit = YUnit.Counts,
                Spectra = new[]
                {
                    new Spectrum(
                        x: xAxes,
                        y: new[] {1, 2, 3, 2, 4.0, 6.1, 7, 8, 9, Math.PI}),
                    new Spectrum(
                        x: xAxes,
                        y: new[] {1, 1, 1, 2, 0, 2, 0, 2.0, 3.0, 1}),
                },
                MetaData = new Dictionary<string, string>()
                {
                    {"Foo", "42"},
                    {"Bar", "Buz"}
                }
            };
            SpcWriter.ToFile(spectra, file);
        }
    }


}
