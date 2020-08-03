
using ColorPicker.ColorModels;

namespace ColorPicker.Utilities
{
    public static class Extensions
    {
        public static int ToARGB32(this RGBColor color, byte alpha) 
        {

            byte red = (byte)(color.R * 255);
            byte green = (byte)(color.G * 255);
            byte blue = (byte)(color.B * 255);


            return (alpha << 24) | (red << 16) | (green << 8) | (blue << 0); 
        }
    }
}
