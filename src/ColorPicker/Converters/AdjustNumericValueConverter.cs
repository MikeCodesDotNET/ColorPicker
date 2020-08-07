
using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;


namespace ColorPickers.Converters
{
    /// <summary>
    /// Converts a binding value object from <see cref="object"/> to <see cref="bool"/> True if value is equal to parameter otherwise return False.
    /// </summary>
    public class AdjustNumericValueConverter : IValueConverter
    {
        /// <summary>
        /// Convert value to boolean, true if matches parameter.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target.</param>
        /// <param name="parameter">A user-defined parameter.</param>
        /// <param name="culture">The culture to use.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsNumber(value) && IsNumber(parameter))
            {
                double original = double.Parse(value.ToString());
                var adjustmentVal = double.Parse(parameter.ToString());

                return original + adjustmentVal;
            }
            return AvaloniaProperty.UnsetValue;
        }

        /// <summary>
        /// Convert boolean to parameter value, returning parameter if true.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the target.</param>
        /// <param name="parameter">A user-defined parameter.</param>
        /// <param name="culture">The culture to use.</param>
        /// <returns>The converted value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return (bool)value ? parameter : AvaloniaProperty.UnsetValue;
            }
            return AvaloniaProperty.UnsetValue;
        }


        private bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }
}
