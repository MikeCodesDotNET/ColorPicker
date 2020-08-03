
using Avalonia.Media;
using ColorPicker.Implementation.Utils;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Vector = System.Collections.Generic.IReadOnlyList<double>;


namespace ColorPicker.ColorModels
{
    /// <summary>
    ///     RGB color with specified <see cref="IRGBWorkingSpace"> working space </see>
    /// </summary>
    public readonly struct RGBColor : IColorVector, IEquatable<RGBColor>
    {

        /// <summary>
        ///     sRGB color space. Used when working space is not specified explicitly.
        /// </summary>
        public static readonly IRGBWorkingSpace DefaultWorkingSpace = RGBWorkingSpaces.sRGB;

        private readonly IRGBWorkingSpace _workingSpace;

        /// <param name="vector"><see cref="Vector"/>, expected 3 dimensions (range from 0 to 1) </param>
        /// <remarks> Uses <see cref="DefaultWorkingSpace"/> as working space. </remarks>
        public RGBColor(Vector vector) : this(vector, DefaultWorkingSpace)
        {
        }

        /// <param name="vector"><see cref="Vector"/>, expected 3 dimensions (range from 0 to 1) </param>
        /// <param name="workingSpace">
        ///     <see cref="RGBWorkingSpaces"/>
        /// </param>
        public RGBColor(Vector vector, IRGBWorkingSpace workingSpace) : this(vector[0], vector[1], vector[2], workingSpace)
        {
        }


        /// <param name="r"> Red (from 0 to 1) </param>
        /// <param name="g"> Green (from 0 to 1) </param>
        /// <param name="b"> Blue (from 0 to 1) </param>
        /// <remarks> Uses <see cref="DefaultWorkingSpace"/> as working space. </remarks>
        public RGBColor(double r, double g, double b) : this(r, g, b, DefaultWorkingSpace)
        {
        }

        /// <param name="r"> Red (from 0 to 1) </param>
        /// <param name="g"> Green (from 0 to 1) </param>
        /// <param name="b"> Blue (from 0 to 1) </param>
        /// <param name="workingSpace">
        ///     <see cref="RGBWorkingSpaces"/>
        /// </param>
        public RGBColor(double r, double g, double b, IRGBWorkingSpace workingSpace)
        {
            R = r.CheckRange(0, 1);
            G = g.CheckRange(0, 1);
            B = b.CheckRange(0, 1);
            _workingSpace = workingSpace;
        }

        /// <inheritdoc cref="object"/>
        public static bool operator !=(RGBColor left, RGBColor right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc cref="object"/>
        public static bool operator ==(RGBColor left, RGBColor right)
        {
            return Equals(left, right);
        }


        /// <inheritdoc cref="object"/>
        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public bool Equals(RGBColor other) { return (R == other.R) && (G == other.G) && (B == other.B) && WorkingSpace.Equals(other.WorkingSpace); }

        /// <inheritdoc cref="object"/>
        public override bool Equals(object obj) { return (obj is RGBColor other) && Equals(other); }

        /// <summary>
        ///     Creates RGB color with all channels equal
        /// </summary>
        /// <param name="value"> Grey value (from 0 to 1) </param>
        /// <remarks> Uses <see cref="DefaultWorkingSpace"/> as working space. </remarks>
        public static RGBColor FromGrey(double value) { return FromGrey(value, DefaultWorkingSpace); }


        /// <summary>
        ///     Creates RGB color with all channels equal
        /// </summary>
        /// <param name="value"> Grey value (from 0 to 1) </param>
        /// <param name="workingSpace">
        ///     <see cref="RGBWorkingSpaces"/>
        /// </param>
        public static RGBColor FromGrey(double value, IRGBWorkingSpace workingSpace) { return new RGBColor(value, value, value, workingSpace); }


        /// <summary>
        ///     Creates RGB color from 8-bit channels
        /// </summary>
        /// <remarks> Uses <see cref="DefaultWorkingSpace"/> as working space. </remarks>
        public static RGBColor FromRGB8bit(byte red, byte green, byte blue) { return FromRGB8bit(red, green, blue, DefaultWorkingSpace); }

        /// <summary>
        ///     Creates RGB color from 8-bit channels
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="workingSpace">
        ///     <see cref="RGBWorkingSpaces"/>
        /// </param>
        public static RGBColor FromRGB8bit(byte red, byte green, byte blue, IRGBWorkingSpace workingSpace)
        { return new RGBColor(red / 255d, green / 255d, blue / 255d, workingSpace); }

        /// <inheritdoc cref="object"/>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ WorkingSpace.GetHashCode();
            }
        }


        public static implicit operator Avalonia.Media.Color(RGBColor c)
        {
            return new Avalonia.Media.Color(255, (byte)(c.R * 255), (byte)(c.G * 255), (byte)(c.B * 255));
        }


        /// <summary>
        /// Convert from <see cref="System.Drawing.Color" />.
        /// </summary>
        public static explicit operator RGBColor(Color color) => new RGBColor(color);



        public override string ToString() { return string.Format(CultureInfo.InvariantCulture, "RGB [R={0:0.##}, G={1:0.##}, B={2:0.##}]", R, G, B); }



        /// <summary>
        ///     Red
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 1.
        /// </remarks>
        public double R { get; }

        /// <summary>
        ///     Green
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 1.
        /// </remarks>
        public double G { get; }

        /// <summary>
        ///     Blue
        /// </summary>
        /// <remarks>
        ///     Ranges from 0 to 1.
        /// </remarks>
        public double B { get; }




/// <remarks>Uses <see cref="DefaultWorkingSpace" /> as working space.</remarks>
        public RGBColor(Color color)
            : this(color, DefaultWorkingSpace)
        {
        }

        /// <param name="color"></param>
        /// <param name="workingSpace">
        /// <see cref="RGBWorkingSpaces" />
        /// </param>
        public RGBColor(Color color, IRGBWorkingSpace workingSpace)
            : this((double)color.R / 255, (double)color.G / 255, (double)color.B / 255, workingSpace)
        {
        }





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
