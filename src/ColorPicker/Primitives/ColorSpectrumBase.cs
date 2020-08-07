using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

using System.Diagnostics;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using HA = Avalonia.Layout.HorizontalAlignment;
using Point = Avalonia.Point;
using VA = Avalonia.Layout.VerticalAlignment;


namespace ColorPickers.Primitives
{
    internal abstract class ColorSpectrumBase : Panel
    {

        public abstract ColorSpectrumShape Shape { get; }

        public static readonly StyledProperty<int> MaxHueProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MaxHue", 360, true, BindingMode.TwoWay);
        public static readonly StyledProperty<int> MaxSaturationProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MaxSaturation", 100, true, BindingMode.TwoWay);
        public static readonly StyledProperty<int> MaxValueProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MaxValue", 100, true, BindingMode.TwoWay);

        public static readonly StyledProperty<int> MinHueProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MinHue", 0, true, BindingMode.TwoWay);
        public static readonly StyledProperty<int> MinSaturationProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MinSaturation", 0, true, BindingMode.TwoWay);
        public static readonly StyledProperty<int> MinValueProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MinValue", 0, true, BindingMode.TwoWay);

        public int MaxHue
        {
            get => GetValue(MaxHueProperty);
            set
            {
                value = Math.Clamp(value, 1, 360);
                SetValue(MaxHueProperty, value);
                InvalidateVisual();
            }
        }

        public int MaxSaturation
        {
            get => GetValue(MaxSaturationProperty);
            set
            {
                value = Math.Clamp(value, 1, 100);
                SetValue(MaxSaturationProperty, value);
                InvalidateVisual();
            }
        }

        public int MaxValue
        {
            get => GetValue(MaxValueProperty);
            set
            {
                value = Math.Clamp(value, 1, 100);
                SetValue(MaxValueProperty, value);
                InvalidateVisual();
            }
        }

        public int MinHue
        {
            get => GetValue(MinHueProperty);
            set
            {
                value = Math.Clamp(value, 0, 359);
                SetValue(MinHueProperty, value);
                InvalidateVisual();
            }
        }

        public int MinSaturation
        {
            get => GetValue(MinSaturationProperty);
            set
            {
                value = Math.Clamp(value, 0, 99);
                SetValue(MinSaturationProperty, value);
                InvalidateVisual();
            }
        }

        public int MinValue
        {
            get => GetValue(MaxValueProperty);
            set
            {
                value = Math.Clamp(value, 0, 99);
                SetValue(MinValueProperty, value);
                InvalidateVisual();
            }
        }



        protected Shape borderShape { get; set; }

        protected virtual Color ColorFunction(double x, double y)
        {
            return ColorMapping(x, y, 1.0);
        }

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            DrawSpectrumBitmap(dc);
        }

        /// <summary>
        /// Responsible for drawing the background image. For a ring, this would be the HSV dial. 
        /// </summary>
        protected abstract void DrawSpectrumBitmap(DrawingContext drawingContext);
       

        public abstract Color ColorMapping(double x, double y, double z);

        public abstract Point InverseColorMapping(Color color);

    }
}
