using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ColorPickers.Converters
{
    public class ColorToHexConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Color color)
            {
                return color.ToHex(false);
            }
            
            return AvaloniaProperty.UnsetValue;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string hex)
            {
                return hex.ToColor();
            }
            return AvaloniaProperty.UnsetValue;

        }

    }
}
