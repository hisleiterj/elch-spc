using System.Collections.Generic;

namespace Elchwinkel.Spc
{
    public sealed class XZUnit
    {
        public static readonly XZUnit Arbitrary = new XZUnit(0, "Arbitrary");
        public static readonly XZUnit Wavenumber = new XZUnit(1, "Wavenumber (cm-1)");
        public static readonly XZUnit Micrometers = new XZUnit(2, "Micrometers");
        public static readonly XZUnit Nanometers = new XZUnit(3, "Nanometers");
        public static readonly XZUnit Seconds = new XZUnit(4, "Seconds");
        public static readonly XZUnit Minutes = new XZUnit(5, "Minutes");
        public static readonly XZUnit Hertz = new XZUnit(6, "Hertz");
        public static readonly XZUnit Kilohertz = new XZUnit(7, "Kilohertz");
        public static readonly XZUnit Megahertz = new XZUnit(8, "Megahertz");
        public static readonly XZUnit Mass = new XZUnit(9, "Mass (M/z)");
        public static readonly XZUnit PartsPerMillion = new XZUnit(10, "Parts per million");
        public static readonly XZUnit Days = new XZUnit(11, "Days");
        public static readonly XZUnit Years = new XZUnit(12, "Years");
        public static readonly XZUnit RamanShift = new XZUnit(13, "Raman Shift (cm-1)");
        public static readonly XZUnit ElectronVolts = new XZUnit(14, "Electron Volts (eV)");

        public static readonly XZUnit
            CustomTexts = new XZUnit(15, "X,Y,Z text labels in fcatxt (old 4Dh version only)");

        public static readonly XZUnit DiodeNumber = new XZUnit(16, "Diode Number");
        public static readonly XZUnit Channel = new XZUnit(17, "Channel");
        public static readonly XZUnit Degrees = new XZUnit(18, "Degrees");
        public static readonly XZUnit TemperatureFarenheit = new XZUnit(19, "Temperature (F)");
        public static readonly XZUnit TemperatureCelcius = new XZUnit(20, "Temperature (C)");
        public static readonly XZUnit TemperatureKelvin = new XZUnit(21, "Temperature (K)");
        public static readonly XZUnit DataPoints = new XZUnit(22, "Data Points");
        public static readonly XZUnit Milliseconds = new XZUnit(23, "Milliseconds (mSec)");
        public static readonly XZUnit Microseconds = new XZUnit(24, "Microseconds (uSec)");
        public static readonly XZUnit Nanoseconds = new XZUnit(25, "Nanoseconds (nSec)");
        public static readonly XZUnit Gigahertz = new XZUnit(26, "Gigahertz (GHz)");
        public static readonly XZUnit Centimeters = new XZUnit(27, "Centimeters (cm)");
        public static readonly XZUnit Meters = new XZUnit(28, "Meters (m)");
        public static readonly XZUnit Millimeters = new XZUnit(29, "Millimeters (mm)");
        public static readonly XZUnit Hours = new XZUnit(30, "Hours");
        public static readonly XZUnit DoubleInterferogram = new XZUnit(255, "Double interferogram (no display labels)");

        private static readonly Dictionary<byte, XZUnit> _lookup = new Dictionary<byte, XZUnit>
        {
            {Arbitrary.Code, Arbitrary},
            {Wavenumber.Code, Wavenumber},
            {Micrometers.Code, Micrometers},
            {Nanometers.Code, Nanometers},
            {Seconds.Code, Seconds},
            {Minutes.Code, Minutes},
            {Hertz.Code, Hertz},
            {Kilohertz.Code, Kilohertz},
            {Megahertz.Code, Megahertz},
            {Mass.Code, Mass},
            {PartsPerMillion.Code, PartsPerMillion},
            {Days.Code, Days},
            {Years.Code, Years},
            {RamanShift.Code, RamanShift},
            {ElectronVolts.Code, ElectronVolts},
            {CustomTexts.Code, CustomTexts},
            {DiodeNumber.Code, DiodeNumber},
            {Channel.Code, Channel},
            {Degrees.Code, Degrees},
            {TemperatureFarenheit.Code, TemperatureFarenheit},
            {TemperatureCelcius.Code, TemperatureCelcius},
            {TemperatureKelvin.Code, TemperatureKelvin},
            {DataPoints.Code, DataPoints},
            {Milliseconds.Code, Milliseconds},
            {Microseconds.Code, Microseconds},
            {Nanoseconds.Code, Nanoseconds},
            {Gigahertz.Code, Gigahertz},
            {Centimeters.Code, Centimeters},
            {Meters.Code, Meters},
            {Millimeters.Code, Millimeters},
            {Hours.Code, Hours},
            {DoubleInterferogram.Code, DoubleInterferogram}
        };

        public XZUnit(byte code, string name)
        {
            Code = code;
            Name = name;
        }

        public byte Code { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        public static XZUnit FromCode(byte code)
        {
            return _lookup[code];
        }
    }
}