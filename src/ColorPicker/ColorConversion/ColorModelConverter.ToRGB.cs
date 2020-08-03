using ColorPicker.ColorModels;
using ColorPicker.Implementation.Conversion.RGB;
using System;
using Matrix = System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<double>>;


namespace ColorPicker.ColorConversion
{
    public partial class ColorModelConverter
    {
        private XYZToLinearRGBConverter _lastXYZToLinearRGBConverter;

        private XYZToLinearRGBConverter GetXYZToLinearRGBConverter(IRGBWorkingSpace workingSpace)
        {
            if (_lastXYZToLinearRGBConverter != null &&
                _lastXYZToLinearRGBConverter.TargetRGBWorkingSpace.Equals(workingSpace))
                return _lastXYZToLinearRGBConverter;

            return _lastXYZToLinearRGBConverter = new XYZToLinearRGBConverter(workingSpace);
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in LinearRGBColor color)
        {
            // conversion
            var converter = LinearRGBToRGBConverter.Default;

            var result = converter.Convert(color);
            return result;
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in HSVColor color)
        {
            
            float r = 0;
            float g = 0;
            float b = 0;

            int hi = (int)Math.Floor(color.H / 60) % 6;
            var f = color.H / 60 - Math.Floor(color.H / 60);

            var _v = color.V * 255;
            int v = (int)_v;
            int p = (int)(_v * (1.0 - color.S));
            int q = (int)(_v * (1.0 - f * color.S));
            int t = (int)(_v * (1.0 - (1.0 - f) * color.S));

            switch (hi)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                case 5: r = v; g = p; b = q; break;
            }
            r /= 255.0f;
            g /= 255.0f;
            b /= 255.0f;

            if (r <= 0)
                r = 0;

            if (g <= 0)
                g = 0;

            if (b <= 0)
                b = 0;

            return new RGBColor(r, g, b);
        }




        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in XYZColor color)
        {
            // conversion
            var linear = ToLinearRGB(color);

            // companding to RGB
            var result = ToRGB(linear);
            return result;
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in xyYColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToRGB(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in LabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToRGB(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in LChabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToRGB(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in HunterLabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToRGB(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in LuvColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToRGB(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in LChuvColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToRGB(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB(in LMSColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToRGB(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to RGB color
        /// </summary>
        public RGBColor ToRGB<T>(T color) where T : IColorVector
        {
            switch (color)
            {
                case RGBColor typedColor:
                    return typedColor;
                case LinearRGBColor typedColor:
                    return ToRGB(in typedColor);
                case XYZColor typedColor:
                    return ToRGB(in typedColor);
                case xyYColor typedColor:
                    return ToRGB(in typedColor);
                case HunterLabColor typedColor:
                    return ToRGB(in typedColor);
                case LabColor typedColor:
                    return ToRGB(in typedColor);
                case LChabColor typedColor:
                    return ToRGB(in typedColor);
                case LuvColor typedColor:
                    return ToRGB(in typedColor);
                case LChuvColor typedColor:
                    return ToRGB(in typedColor);
                case LMSColor typedColor:
                    return ToRGB(in typedColor);
                case HSVColor typedColor:
                    return ToRGB(in typedColor);
                default:
                    throw new ArgumentException($"Cannot accept type '{typeof(T)}'.", nameof(color));
            }
        }
    
    }
}

