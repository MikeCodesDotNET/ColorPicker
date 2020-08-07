using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ColorPickers.Converters
{
    public class ColorToSpectrumComponentValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Color color && parameter is ColorSpectrumComponent component)
            {
                switch(component)
                {
                    case ColorSpectrumComponent.Red:
                        return color.R;
                    case ColorSpectrumComponent.Green:
                        return color.G;
                    case ColorSpectrumComponent.Blue:
                        return color.B;
                    case ColorSpectrumComponent.Alpha:
                        return color.A;

                    case ColorSpectrumComponent.Hue:
                        return Math.Round(color.ToHsv().H);

                    case ColorSpectrumComponent.Saturation:
                        return Math.Round(color.ToHsv().S * 100);

                    case ColorSpectrumComponent.Value:
                        return Math.Round(color.ToHsv().V * 100);
                }
                return AvaloniaProperty.UnsetValue;
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
