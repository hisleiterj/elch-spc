# elch-spc
A C#/.net library to read and write spc spectrum files (aka. Thermo Fisher/Scientific software GRAMS/AI spectroscopy format).
## Usage
```c#
//Example: How to Load a spc file

//Loading spc file into memory
var spc = SpcReader.Read("test.spc");
//Printing 'Memo'/'Comment' string to console
Console.Writeline(spc.Memo);
//Accessing the y-values of first spectrum
double[] yValues = spc.Spectra[0].Y;
```
## Acknowledgements
Test Files were taken from https://github.com/wingardium/spc-sdk
