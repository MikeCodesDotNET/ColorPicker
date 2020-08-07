using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ColorPickers.Converters
{
    public class HalfNumericValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is double d)
            {
                return Math.Round(d / 2);
            }
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return Math.Round(d * 2);
            }

            return value.Equals(true) ? parameter : AvaloniaProperty.UnsetValue;
           
        }

    }
}
