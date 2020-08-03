using ColorPicker.ColorConversion;
using ColorPicker.ColorModels;
using System;
using Point = Avalonia.Point;

namespace ColorPicker.Wheels
{
    public class HSVWheel : ColorWheelBase
    {
        ColorModelConverter converter = new ColorModelConverter();

        private const double whiteFactor = 2.2; // Provide more accuracy around the white-point

        public override RGBColor ColorMapping(double radius, double theta, double value)
        {
            HSVColor hsv = new HSVColor((float)theta, (float)Math.Pow(radius, whiteFactor), (float)value);
            RGBColor rgb = converter.ToRGB(hsv);
            return rgb;
        }

        public override Point InverseColorMapping(IColorVector color)
        {

            double theta, rad;

            converter.ToHSV(color);
            HSVColor hsv = converter.ToHSV(color);
            theta = hsv.H;
            rad = Math.Pow(hsv.S, 1.0 / whiteFactor);

            return new Point(theta, rad);
        }
    }
}
