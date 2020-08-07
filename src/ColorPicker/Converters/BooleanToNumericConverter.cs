using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ColorPickers.Converters
{
    public class BooleanToNumericConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool cond = (bool)value;
            return cond ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            // TODO - Probably not a good idea to compare doubles
            double val = (double)value;

            if(val == TrueValue)
            {
                return true;
            }

            if(val == FalseValue)
            {
                return false;
            }

            return AvaloniaProperty.UnsetValue;
        }

        public double FalseValue { get; set; }

        public double TrueValue { get; set; }

    }
}
