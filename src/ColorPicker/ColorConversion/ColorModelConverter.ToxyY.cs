using ColorPicker.ColorModels;
using ColorPicker.Implementation.Conversion.xyY;
using System;
using Matrix = System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<double>>;


namespace ColorPicker.ColorConversion
{
    /// <summary>
    /// Converts between color spaces and makes sure that the color is adapted using chromatic adaptation.
    /// </summary>
    public partial class ColorModelConverter
    {
        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY(in RGBColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToxyY(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY(in LinearRGBColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToxyY(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY(in XYZColor color)
        {
            // conversion
            var converter = new xyYAndXYZConverter();
            var result = converter.Convert(color);
            return result;
        }

        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY(in LabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToxyY(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY(in LChabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToxyY(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY(in HunterLabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToxyY(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY(in LuvColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToxyY(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY(in LChuvColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToxyY(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY(in LMSColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToxyY(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE xyY color
        /// </summary>
        public xyYColor ToxyY<T>(T color) where T : IColorVector
        {
            switch (color)
            {
                case RGBColor typedColor:
                    return ToxyY(in typedColor);
                case LinearRGBColor typedColor:
                    return ToxyY(in typedColor);
                case XYZColor typedColor:
                    return ToxyY(in typedColor);
                case xyYColor typedColor:
                    return typedColor;
                case HunterLabColor typedColor:
                    return ToxyY(in typedColor);
                case LabColor typedColor:
                    return ToxyY(in typedColor);
                case LChabColor typedColor:
                    return ToxyY(in typedColor);
                case LuvColor typedColor:
                    return ToxyY(in typedColor);
                case LChuvColor typedColor:
                    return ToxyY(in typedColor);
                case LMSColor typedColor:
                    return ToxyY(in typedColor);
                default:
                    throw new ArgumentException($"Cannot accept type '{typeof(T)}'.", nameof(color));
            }
        }
    }
}

