using ColorPicker.ColorModels;
using ColorPicker.Implementation.Conversion.Lab;
using ColorPicker.Implementation.Conversion.LChab;
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
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab(in RGBColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToLab(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab(in LinearRGBColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToLab(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab(in XYZColor color)
        {
            // adaptation
            var adapted = !WhitePoint.Equals(TargetLabWhitePoint) && IsChromaticAdaptationPerformed
                ? ChromaticAdaptation.Transform(color, WhitePoint, TargetLabWhitePoint)
                : color;

            // conversion
            var converter = new XYZToLabConverter(TargetLabWhitePoint);
            var result = converter.Convert(adapted);
            return result;
        }

        /// <summary>
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab(in xyYColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToLab(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab(in LChabColor color)
        {
            // conversion (preserving white point)
            var converter = LChabToLabConverter.Default;
            var unadapted = converter.Convert(color);

            if (!IsChromaticAdaptationPerformed)
                return unadapted;

            // adaptation to target lab white point (LabWhitePoint)
            var adapted = Adapt(unadapted);
            return adapted;
        }

        /// <summary>
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab(in HunterLabColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToLab(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab(in LuvColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToLab(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab(in LChuvColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToLab(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab(in LMSColor color)
        {
            var xyzColor = ToXYZ(color);
            var result = ToLab(xyzColor);
            return result;
        }

        /// <summary>
        /// Convert to CIE L*a*b* (1976) color
        /// </summary>
        public LabColor ToLab<T>(T color) where T : struct, IColorVector
        {
            switch (color)
            {
                case RGBColor typedColor:
                    return ToLab(in typedColor);
                case LinearRGBColor typedColor:
                    return ToLab(in typedColor);
                case XYZColor typedColor:
                    return ToLab(in typedColor);
                case xyYColor typedColor:
                    return ToLab(in typedColor);
                case HunterLabColor typedColor:
                    return ToLab(in typedColor);
                case LabColor typedColor:
                    return typedColor;
                case LChabColor typedColor:
                    return ToLab(in typedColor);
                case LuvColor typedColor:
                    return ToLab(in typedColor);
                case LChuvColor typedColor:
                    return ToLab(in typedColor);
                case LMSColor typedColor:
                    return ToLab(in typedColor);
                default:
                    throw new ArgumentException($"Cannot accept type '{typeof(T)}'.", nameof(color));
            }
        }
    }
}

