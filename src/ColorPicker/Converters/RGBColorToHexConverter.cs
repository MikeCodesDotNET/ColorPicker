using Avalonia;
using Avalonia.Data.Converters;
using ColorPicker.ColorConversion;
using ColorPicker.ColorModels;
using System;
using System.Globalization;

namespace ColorPicker.Converters
{
    public class RGBColorToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is IColorVector color)
            {
                var rgb = new ColorModelConverter().ToRGB(color);

                return String.Format("#{0:x2}{1:x2}{2:x2}", (byte)(rgb.R * 255), (byte)(rgb.G * 255), (byte)(rgb.B * 255));
            }
            else
            {
                return AvaloniaProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}
