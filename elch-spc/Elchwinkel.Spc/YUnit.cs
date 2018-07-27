using System.Collections.Generic;

namespace Elchwinkel.Spc
{
    public class YUnit
    {
        public static readonly YUnit ReferenceArbitraryEnergy = new YUnit(255, "Reference Arbitrary Energy");
        public static readonly YUnit ArbitraryIntensity = new YUnit(0, "Arbitrary Intensity");
        public static readonly YUnit Interferogram = new YUnit(1, "Interferogram");
        public static readonly YUnit Absorbance = new YUnit(2, "Absorbance");
        public static readonly YUnit KubelkaMunk = new YUnit(3, "Kubelka-Munk");
        public static readonly YUnit Counts = new YUnit(4, "Counts");
        public static readonly YUnit Volts = new YUnit(5, "Volts");
        public static readonly YUnit Degrees = new YUnit(6, "Degrees");
        public static readonly YUnit Milliamps = new YUnit(7, "Milliamps");
        public static readonly YUnit Millimeters = new YUnit(8, "Millimeters");
        public static readonly YUnit Millivolts = new YUnit(9, "Millivolts");
        public static readonly YUnit Log = new YUnit(10, "Log (1/R)");
        public static readonly YUnit Percent = new YUnit(11, "Percent");
        public static readonly YUnit Intensity = new YUnit(12, "Intensity");
        public static readonly YUnit RelativeIntensity = new YUnit(13, "Relative Intensity");
        public static readonly YUnit Energy = new YUnit(14, "Energy");
        public static readonly YUnit NotUsed15 = new YUnit(15, "Not Used");
        public static readonly YUnit Decibel = new YUnit(16, "Decibel");
        public static readonly YUnit NotUsed17 = new YUnit(17, "Not Used");
        public static readonly YUnit NotUsed18 = new YUnit(18, "Not Used");
        public static readonly YUnit TemperatureFarenheit = new YUnit(19, "Temperature (F)");
        public static readonly YUnit TemperatureCelcius = new YUnit(20, "Temperature (C)");
        public static readonly YUnit TemperatureKelvin = new YUnit(21, "Temperature (K)");
        public static readonly YUnit IndexOfRefraction = new YUnit(22, "Index of Refraction [N]");
        public static readonly YUnit ExtinctionCoeff = new YUnit(23, "Extinction Coeff. [K]");
        public static readonly YUnit Real = new YUnit(24, "Real");
        public static readonly YUnit Imaginary = new YUnit(25, "Imaginary");
        public static readonly YUnit Complex = new YUnit(26, "Complex");
        public static readonly YUnit Transmission = new YUnit(128, "Transmission");
        public static readonly YUnit Reflectance = new YUnit(129, "Reflectance");

        public static readonly YUnit ArbitraryOrSingleBeamWithValleyPeaks =
            new YUnit(130, "Arbitrary or Single Beam with Valley Peaks");

        public static readonly YUnit Emission = new YUnit(131, "Emission");

        private static readonly Dictionary<byte, YUnit> _lookup = new Dictionary<byte, YUnit>
        {
            {ReferenceArbitraryEnergy.Code, ReferenceArbitraryEnergy},
            {ArbitraryIntensity.Code, ArbitraryIntensity},
            {Interferogram.Code, Interferogram},
            {Absorbance.Code, Absorbance},
            {KubelkaMunk.Code, KubelkaMunk},
            {Counts.Code, Counts},
            {Volts.Code, Volts},
            {Degrees.Code, Degrees},
            {TemperatureFarenheit.Code, TemperatureFarenheit},
            {TemperatureCelcius.Code, TemperatureCelcius},
            {TemperatureKelvin.Code, TemperatureKelvin},
            {Milliamps.Code, Milliamps},
            {Millivolts.Code, Millivolts},
            {Log.Code, Log},
            {Percent.Code, Percent},
            {Intensity.Code, Intensity},
            {RelativeIntensity.Code, RelativeIntensity},
            {Energy.Code, Energy},
            {Millimeters.Code, Millimeters},
            {NotUsed15.Code, NotUsed15},
            {Decibel.Code, Decibel},
            {NotUsed17.Code, NotUsed17},
            {NotUsed18.Code, NotUsed18},
            {IndexOfRefraction.Code, IndexOfRefraction},
            {ExtinctionCoeff.Code, ExtinctionCoeff},
            {Real.Code, Real},
            {Imaginary.Code, Imaginary},
            {Complex.Code, Complex},
            {Transmission.Code, Transmission},
            {Reflectance.Code, Reflectance},
            {ArbitraryOrSingleBeamWithValleyPeaks.Code, ArbitraryOrSingleBeamWithValleyPeaks},
            {Emission.Code, Emission}
        };

        public YUnit(byte code, string name)
        {
            Code = code;
            Name = name;
        }

        public byte Code { get; }
        public string Name { get; }

        public static YUnit FromCode(byte code)
        {
            return _lookup[code];
        }
    }
}