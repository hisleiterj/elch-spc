using System;
using System.Collections.Generic;
using System.IO;
using Elchwinkel.Spc;
using NUnit.Framework;

namespace elch_spc.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void WorksWithSingleSpectrumFile()
        {
            //Arrange
            var file = @".\TestData\Ft-ir.spc";
            //Act
            var spectra = SpcReader.Read(file);
            //Assert
            Assert.AreEqual(spectra.Memo, "FT-IR Spectrum Example\0torials: polystyrene");
            Assert.AreEqual(spectra.MetaData["SCANS"], "16");
            Assert.AreEqual(spectra.MetaData["MODEL"], "PE Spectrum 2000");
            Assert.AreEqual(spectra.Spectra.Length, 1);
            Assert.AreEqual(spectra.Spectra[0].Y[0], 95.137, 10E-3);
            Assert.AreEqual(spectra.Spectra[0].Y[spectra.Spectra[0].Length-1], 94.883, 10E-3);
        }
        [Test]
        public void WorksWithMultiSpectraFile()
        {
            //Arrange
            var file = @".\TestData\m_xyxy.spc";
            //Act
            var spectra = SpcReader.Read(file);
            //Assert
            Assert.AreEqual(spectra.Memo, "Multiple data arrays (multifile), variable X spacing, each record has different  # points in X & Y arrays");
            Assert.AreEqual(spectra.Spectra.Length, 512);
        }
        [Test]
        public void XyFileWithSharedUnevenX()
        {
            //Arrange
            var xAxes = new[] { 1, 2, 3.0, 4, 5, 6, 7, 8.5, 9, 10 };
            var spectra = new SpectrumCollection()
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
                        y: new[] {1,1,1,2,0,2,0,2.0,3.0,1}),
                },
                MetaData = new Dictionary<string, string>()
                {
                    {"Foo", "Bar" },
                    {"Bar", "Buz" }
                }
            };
            //Act
            var bytes = SpcWriter.Encode(spectra);
            var spc2 = SpcReader.Read(bytes);
            var decoded = spc2;
            //Assert
            Assert.AreEqual("Polystyrene", decoded.Memo);
            Assert.AreEqual(XZUnit.RamanShift, decoded.XUnit);
            Assert.AreEqual(YUnit.Counts, decoded.YUnit);
            Assert.AreEqual(2, decoded.Spectra[0].Y[1]);
            Assert.IsTrue(decoded.MetaData.ContainsKey("Foo"));

        }

        [Test]
        public void XyFileWithOwnX()
        {
            //Arrange
            var spectra = new SpectrumCollection()
            {
                Spectra = new[]
                {
                    new Spectrum(
                        x: new[] { 1, 2, 3.0, 4, 5, 6, 7, 8.5, 9, 10 },
                        y: new[] {1, 2, 3, 2, 4.0, 6.1, 7, 8, 9, Math.PI}),
                    new Spectrum(
                        x: new[] { 4,5,6,7,8.0,9,10,11,12,13 },
                        y: new[] {1,1,1,2,0,2,0,2.0,3.0,1}),
                }
            };
            //Act
            var bytes = SpcWriter.Encode(spectra);
            var spc2 = SpcReader.Read(bytes);
            var decoded = spc2;
            //Assert
            Assert.AreEqual(2, decoded.Spectra[0].Y[1]);
        }

        [Test]
        public void SingleFile_EvenXAxes()
        {
            //Arrange
            var xAxes = new[] { 1, 2, 3.0, 4, 5, 6, 7, 8, 9, 10 };
            var spectra = new SpectrumCollection()
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
                        y: new[] {1,1,1,2,0,2,0,2.0,3.0,1}),
                },
                MetaData = new Dictionary<string, string>()
                {
                    {"Foo", "Bar" },
                    {"Bar", "Buz" }
                }
            };
            //Act
            var bytes = SpcWriter.Encode(spectra);
            var spc2 = SpcReader.Read(bytes);
            var decoded = spc2;
            //Assert
            Assert.AreEqual("Polystyrene", decoded.Memo);
            Assert.AreEqual(XZUnit.RamanShift, decoded.XUnit);
            Assert.AreEqual(YUnit.Counts, decoded.YUnit);
            Assert.AreEqual(2, decoded.Spectra[0].Y[1]);
            Assert.AreEqual("Bar", decoded.MetaData["Foo"]);
        }
        [Test]
        public void RunsAllTestData()
        {
            foreach (var file in Directory.GetFiles(@".\TestData", "*.spc"))
            {
                try
                {
                    SpcReader.Read(file);
                }
                catch (InvalidSpcFileException)
                {
                    if (Path.GetFileName(file).Equals("m_ordz.spc")) continue;
                    Assert.Fail();
                }

            }

        }
    }
}
