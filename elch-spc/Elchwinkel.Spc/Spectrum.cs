using System;

namespace Elchwinkel.Spc
{
    public sealed class Spectrum
    {
        public Spectrum(double[] x, double[] y)
        {
            if(x.Length != y.Length) throw new ArgumentException($"X and Y Arrays must be of same size but was {x.Length} vs {y.Length}.");
            X = x;
            Y = y;
            IsXEvenSpaced = _IsEvenSpaced(x);
        }

        public double[] X { get; }
        public double[] Y { get; }
        public int Length => Y.Length;
        internal bool IsXEvenSpaced { get; }

        private static bool _IsEvenSpaced(double[] values)
        {
            var delta = values[1] - values[0];
            for (int i = 0; i < values.Length - 1; i++)
            {
                var delta2 = values[i+1] - values[i];
                if (Math.Abs(delta2 - delta) > 10E-8) return false;
            }

            return true;
        }

    }
}