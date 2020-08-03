using ColorPicker.Other;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Vector = System.Collections.Generic.IReadOnlyList<double>;

namespace ColorPicker.ColorModels
{
    /// <summary>
    ///     CIE L*C*h°, cylindrical form of <see cref="LabColor"> CIE L*a*b* (1976) </see>
    /// </summary>
    public readonly struct LChabColor : IColorVector
    {

        /// <summary>
        ///     D50 standard illuminant. Used when reference white is not specified explicitly.
        /// </summary>
        public static readonly XYZColor DefaultWhitePoint = Illuminants.D50;

        private readonly XYZColor? _whitePoint;

        /// <param name="vector"><see cref="Vector"/>, expected 3 dimensions </param>
        /// <remarks> Uses <see cref="DefaultWhitePoint"/> as white point. </remarks>
        public LChabColor(Vector vector) : this(vector, DefaultWhitePoint)
        {
        }

        /// <param name="vector"><see cref="Vector"/>, expected 3 dimensions </param>
        /// <param name="whitePoint"> Reference white (see <see cref="Illuminants"/>) </param>
        public LChabColor(Vector vector, XYZColor whitePoint) : this(vector[0], vector[1], vector[2], whitePoint)
        {
        }


        /// <param name="l"> L* (lightness) (from 0 to 100) </param>
        /// <param name="c"> C* (chroma) (from 0 to 100) </param>
        /// <param name="h"> h° (hue in degrees) (from 0 to 360) </param>
        public LChabColor(double l, double c, double h) : this(l, c, h, DefaultWhitePoint)
        {
        }

        /// <param name="l"> L* (lightness) (from 0 to 100) </param>
        /// <param name="c"> C* (chroma) (from 0 to 100) </param>
        /// <param name="h"> h° (hue in degrees) (from 0 to 360) </param>
        /// <param name="whitePoint"> Reference white (see <see cref="Illuminants"/>) </param>
        public LChabColor(double l, double c, double h, XYZColor whitePoint)
        {
            L = l;
            C = c;
            this.h = h;
            _whitePoint = whitePoint;
        }

        /// <inheritdoc cref="object"/>
        public static bool operator !=(LChabColor left, LChabColor right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc cref="object"/>
        public static bool operator ==(LChabColor left, LChabColor right)
        {
            return Equals(left, right);
        }


        /// <inheritdoc cref="object"/>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public bool Equals(LChabColor other) { return (L == other.L) && (C == other.C) && (h == other.h); }

        /// <inheritdoc cref="object"/>
        public override bool Equals(object obj) { return (obj is LChabColor other) && Equals(other); }

        /// <inheritdoc cref="object"/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = L.GetHashCode();
                hashCode = (hashCode * 397) ^ C.GetHashCode();
                hashCode = (hashCode * 397) ^ h.GetHashCode();
                return hashCode;
            }
        }


        /// <inheritdoc cref="object"/>
        public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "LChab [L={0:0.##}, C={1:0.##}, h={2:0.##}]", L, C, h); }

        /// <summary>
        ///     C* (chroma)
        /// </summary>
        /// <remarks>
        ///     Ranges usually from 0 to 100.
        /// </remarks>
        public double C { get; }

        /// <summary>
        ///     h° (hue in degrees)
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 360.
        /// </remarks>
        public double h { get; }


        /// <summary>
        ///     L* (lightness)
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 100.
        /// </remarks>
        public double L { get; }


        /// <summary>
        ///     Computes saturation of the color (chroma normalized by lightness)
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 100.
        /// </remarks>
        public double Saturation => SaturationLChFormulas.GetSaturation(L, C);


        /// <summary>
        ///     <see cref="IColorVector"/>
        /// </summary>
        public Vector Vector => new[] { L, C, h };


        /// <remarks>
        ///     <see cref="Illuminants"/>
        /// </remarks>
        public XYZColor WhitePoint => _whitePoint ?? DefaultWhitePoint;

    }
}
