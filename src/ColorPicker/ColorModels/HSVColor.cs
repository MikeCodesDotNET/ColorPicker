using System;
using System.Diagnostics.CodeAnalysis;
using Vector = System.Collections.Generic.IReadOnlyList<double>;


namespace ColorPicker.ColorModels
{
    public readonly struct HSVColor : IColorVector, IEquatable<HSVColor>
    {
        /// <param name="h"> Hue (from 0 to 1) </param>
        /// <param name="s"> Saturation (from 0 to 1) </param>
        /// <param name="v"> Value / Brightness (from 0 to 1) </param>
        public HSVColor(double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }

        /// <summary>
        ///     Hue
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 360
        /// </remarks>
        public double H { get; }

        /// <summary>
        ///     Saturation
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 1.
        /// </remarks>
        public double S { get; }

        /// <summary>
        ///     Value / Brightness
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 1.
        /// </remarks>
        public double V { get; }



        public Vector Vector => new[] { H, S, V };

        public bool Equals([AllowNull] HSVColor other)
        {
            throw new NotImplementedException();
        }
    }
}
