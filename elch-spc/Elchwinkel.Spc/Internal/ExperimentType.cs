namespace Elchwinkel.Spc
{
    internal enum ExperimentType : byte
    {
        /// <summary>
        /// General (could be anything)
        /// </summary>
        SPCGEN = 0x00,
        /// <summary>
        /// Gas Chromatogram
        /// </summary>
        SPCGC = 0x01,
        /// <summary>
        /// General Chromatogram (same as SPCGEN and TCGRAM in ftflgs).
        /// </summary>
        SPCCGM = 0x02,
        /// <summary>
        /// HPLC Chromatogram
        /// </summary>
        SPChPLC = 0x03,
        /// <summary>
        /// FT-IR, FT-NIR, FT-Raman Spectrum  (Can also be used for scanning IR.)
        /// </summary>
        SPCFTIR = 0x04,
        /// <summary>
        /// NIR Spectrum (Usually multi-spectral data sets for calibration.)
        /// </summary>
        SPCNIR = 0x05,
        /// <summary>
        /// UV-VIS Spectrum  (Can be used for single scanning UV-VIS-NIR.)
        /// </summary>
        SPCUV = 0x06,
        /// <summary>
        /// Not Defined – Do not use.
        /// </summary>
        NotDefined = 0x07,
        /// <summary>
        /// X-ray Diffraction Spectrum
        /// </summary>
        SPCXRY = 0x08,
        /// <summary>
        /// Mass Spectrum  (Can be GC-MS, Continuum, Centroid or TOF.)
        /// </summary>
        SPCMS = 0x09,
        /// <summary>
        /// NMR Spectrum
        /// </summary>
        SPCNMR = 0x0A,
        /// <summary>
        /// Raman Spectrum (Usually Diode Array, CCD, etc. not for FT-Raman.)
        /// </summary>
        SPCRMN = 0x0B,
        /// <summary>
        /// Fluorescence Spectrum
        /// </summary>
        SPCFLR = 0x0C,
        /// <summary>
        /// Atomic Spectrum
        /// </summary>
        SPCATM = 0x0D,
        /// <summary>
        /// Chromatography Diode Array Spectra
        /// </summary>
        SPCDAD = 0x0E,
    }
}