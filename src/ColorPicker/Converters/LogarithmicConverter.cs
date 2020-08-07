using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ColorPickers.Converters
{
    /// <summary>
    ///     Turns a linear-scaled slider (0.0 to 1.0) into a logarithmic value, defined by Minimum and Maximum. The
    ///     logarithm is in base 10, so for best results Minimum & Maximum should be a power of 10.
    /// </summary>
    public class LogarithmicConverter : IValueConverter
    {

        private double linMax;

        private double linMin;
        private double logMax;
        private double logMin;

        public object Convert(object _value, Type targetType, object parameter, CultureInfo culture)
        {
            double scale = logMax - logMin;
            double value = (double)_value;

            return (Math.Log10(value) - logMin) / scale;
        }

        public object ConvertBack(object _value, Type targetType, object parameter, CultureInfo culture)
        {
            double scale = logMax - logMin;
            double value = (double)_value;

            return Math.Pow(10.0, logMin + (scale * value));
        }

        public double Maximum
        {
            get => linMax;
            set
            {
                linMax = value;
                logMax = Math.Log10(value);
            }
        }

        public double Minimum
        {
            get => linMin;
            set
            {
                linMin = value;
                logMin = Math.Log10(value);
            }
        }

    }
}
