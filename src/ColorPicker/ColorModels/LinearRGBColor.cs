using ColorPicker.Implementation.Utils;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Vector = System.Collections.Generic.IReadOnlyList<double>;


namespace ColorPicker.ColorModels
{
    /// <summary>
    ///     RGB color with specified <see cref="IRGBWorkingSpace"> working space </see>, which has linear channels (not
    ///     companded)
    /// </summary>
    public readonly struct LinearRGBColor : IColorVector
    {

        /// <summary>
        ///     sRGB color space. Used when working space is not specified explicitly.
        /// </summary>
        public static readonly IRGBWorkingSpace DefaultWorkingSpace = RGBWorkingSpaces.sRGB;

        private readonly IRGBWorkingSpace _workingSpace;

        /// <param name="vector"><see cref="Vector"/>, expected 3 dimensions (range from 0 to 1) </param>
        /// <remarks> Uses <see cref="DefaultWorkingSpace"/> as working space. </remarks>
        public LinearRGBColor(Vector vector) : this(vector, DefaultWorkingSpace)
        {
        }

        /// <param name="vector"><see cref="Vector"/>, expected 3 dimensions (range from 0 to 1) </param>
        /// <param name="workingSpace">
        ///     <see cref="RGBWorkingSpaces"/>
        /// </param>
        public LinearRGBColor(Vector vector, IRGBWorkingSpace workingSpace) : this(vector[0], vector[1], vector[2], workingSpace)
        {
        }


        /// <param name="r"> Red (from 0 to 1) </param>
        /// <param name="g"> Green (from 0 to 1) </param>
        /// <param name="b"> Blue (from 0 to 1) </param>
        /// <remarks> Uses <see cref="DefaultWorkingSpace"/> as working space. </remarks>
        public LinearRGBColor(double r, double g, double b) : this(r, g, b, DefaultWorkingSpace)
        {
        }

        /// <param name="r"> Red (from 0 to 1) </param>
        /// <param name="g"> Green (from 0 to 1) </param>
        /// <param name="b"> Blue (from 0 to 1) </param>
        /// <param name="workingSpace">
        ///     <see cref="RGBWorkingSpaces"/>
        /// </param>
        public LinearRGBColor(double r, double g, double b, IRGBWorkingSpace workingSpace)
        {
            R = r.CheckRange(0, 1);
            G = g.CheckRange(0, 1);
            B = b.CheckRange(0, 1);
            _workingSpace = workingSpace;
        }

        /// <inheritdoc cref="object"/>
        public static bool operator !=(LinearRGBColor left, LinearRGBColor right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc cref="object"/>
        public static bool operator ==(LinearRGBColor left, LinearRGBColor right)
        {
            return Equals(left, right);
        }


        /// <inheritdoc cref="object"/>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public bool Equals(LinearRGBColor other) { return (R == other.R) && (G == other.G) && (B == other.B) && WorkingSpace.Equals(other.WorkingSpace); }

        /// <inheritdoc cref="object"/>
        public override bool Equals(object obj) { return (obj is LinearRGBColor other) && Equals(other); }

        /// <summary>
        ///     Creates RGB color with all channels equal
        /// </summary>
        /// <param name="value"> Grey value (from 0 to 1) </param>
        /// <remarks> Uses <see cref="DefaultWorkingSpace"/> as working space. </remarks>
        public static LinearRGBColor FromGrey(double value) { return FromGrey(value, DefaultWorkingSpace); }


        /// <summary>
        ///     Creates RGB color with all channels equal
        /// </summary>
        /// <param name="value"> Grey value (from 0 to 1) </param>
        /// <param name="workingSpace">
        ///     <see cref="RGBWorkingSpaces"/>
        /// </param>
        public static LinearRGBColor FromGrey(double value, IRGBWorkingSpace workingSpace) { return new LinearRGBColor(value, value, value, workingSpace); }

        /// <inheritdoc cref="object"/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ WorkingSpace.GetHashCode();
            }
        }


        /// <inheritdoc cref="object"/>
        public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "LinearRGB [R={0:0.##}, G={1:0.##}, B={2:0.##}]", R, G, B); }

        /// <summary>
        ///     Blue
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 1.
        /// </remarks>
        public double B { get; }

        /// <summary>
        ///     Green
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 1.
        /// </remarks>
        public double G { get; }


        /// <summary>
        ///     Red
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 1.
        /// </remarks>
        public double R { get; }


        /// <summary>
        ///     <see cref="IColorVector"/>
        /// </summary>
        public Vector Vector => new[] { R, G, B };


        /// <summary>
        ///     RGB color space <seealso cref="RGBWorkingSpaces"/>
        /// </summary>
        public IRGBWorkingSpace WorkingSpace => _workingSpace ?? DefaultWorkingSpace;

    }
}
