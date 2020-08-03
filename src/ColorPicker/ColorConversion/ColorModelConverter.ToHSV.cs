using ColorPicker.ColorModels;
using ColorPicker.Implementation.Conversion.RGB;
using System;
using Matrix = System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<double>>;


namespace ColorPicker.ColorConversion
{
    public partial class ColorModelConverter
    {

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV(in LinearRGBColor color)
        {

            // conversion
            float max = (float)Math.Max(color.R, Math.Max(color.G, color.B));
            float min = (float)Math.Min(color.R, Math.Min(color.G, color.B));


            double value = max;
            double sat = 0;
            double hue = 0;

            float delta = max - min;

            if (max > float.Epsilon)
            {
                sat = delta / max;
            }
            else
            {
                // r = g = b = 0
                sat = 0;
                hue = float.NaN; // Undefined
                return new HSVColor(hue, sat, value);
            }

            if (color.R == max)
                hue = (color.G - color.B) / delta;    // Between yellow and magenta
            else if (color.G == max)
                hue = 2 + (color.B - color.R) / delta; // Between cyan and yellow
            else
                hue = 4 + (color.R - color.G) / delta; // Between magenta and cyan

            hue *= 60.0f; // degrees
            if (hue < 0)
                hue += 360;

            return new HSVColor(hue, sat, value);
        }

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV(in XYZColor color)
        {
            // conversion
            var linear = ToLinearRGB(color);

            // companding to ToHSV
            var result = ToHSV(linear);
            return result;
        }

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV(in xyYColor color)
        {
            var rgbColor = ToRGB(color);
            var result = ToHSV(rgbColor);
            return result;
        }

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV(in LabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToHSV(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV(in LChabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToHSV(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV(in HunterLabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToHSV(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV(in LuvColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToHSV(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV(in LChuvColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToHSV(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV(in LMSColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToHSV(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to HSV color
        /// </summary>
        public HSVColor ToHSV<T>(T color) where T : IColorVector
        {
            switch (color)
            {
                case HSVColor typedColor:
                    return typedColor;
                case RGBColor typedColor:
                    return ToHSV(typedColor);
                case LinearRGBColor typedColor:
                    return ToHSV(in typedColor);
                case XYZColor typedColor:
                    return ToHSV(in typedColor);
                case xyYColor typedColor:
                    return ToHSV(in typedColor);
                case HunterLabColor typedColor:
                    return ToHSV(in typedColor);
                case LabColor typedColor:
                    return ToHSV(in typedColor);
                case LChabColor typedColor:
                    return ToHSV(in typedColor);
                case LuvColor typedColor:
                    return ToHSV(in typedColor);
                case LChuvColor typedColor:
                    return ToHSV(in typedColor);
                case LMSColor typedColor:
                    return ToHSV(in typedColor);
                default:
                    throw new ArgumentException($"Cannot accept type '{typeof(T)}'.", nameof(color));
            }
        }
    
    }
}

