using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ColorPickers.Primitives;
using System;
using AvaloniaColor = Avalonia.Media.Color;

namespace ColorPickers
{
    public class ColorPicker : UserControl
    {
        public static readonly StyledProperty<AvaloniaColor> ColorProperty = AvaloniaProperty.Register<ColorPicker, AvaloniaColor>("Color", Rgb.Default.ToColor(), true, BindingMode.TwoWay);



        //Color spectrum	
        public static readonly StyledProperty<bool> IsColorSpectrumVisibleProperty = AvaloniaProperty.Register<ColorPicker, bool>("IsColorSpectrumVisible", true, true, BindingMode.TwoWay);
        public static readonly StyledProperty<ColorSpectrumShape> ColorSpectrumShapeProperty = AvaloniaProperty.Register<ColorPicker, ColorSpectrumShape>("ColorSpectrumShape", ColorSpectrumShape.Ring, true, BindingMode.TwoWay);

        //Color preview	
        public static readonly StyledProperty<bool> IsColorPreviewVisibleProperty = AvaloniaProperty.Register<ColorPicker, bool>("IsColorPreviewVisible", true, true, BindingMode.TwoWay);

        //Color values	
        public static readonly StyledProperty<bool> IsColorSliderVisibleProperty = AvaloniaProperty.Register<ColorPicker, bool>("IsColorSliderVisible", true, true, BindingMode.TwoWay);
        public static readonly StyledProperty<bool> IsColorChannelTextInputVisibleProperty = AvaloniaProperty.Register<ColorPicker, bool>("IsColorChannelTextInputVisible", true, true, BindingMode.TwoWay);

        //Opacity values
        public static readonly StyledProperty<bool> IsAlphaEnabledProperty = AvaloniaProperty.Register<ColorPicker, bool>("IsAlphaEnabled", false, true, BindingMode.TwoWay);
        public static readonly StyledProperty<bool> IsAlphaSliderVisibleProperty = AvaloniaProperty.Register<ColorPicker, bool>("IsAlphaSliderVisible", false, true, BindingMode.TwoWay);
        public static readonly StyledProperty<bool> IsAlphaTextInputVisibleProperty = AvaloniaProperty.Register<ColorPicker, bool>("IsAlphaTextInputVisible", false, true, BindingMode.TwoWay);

        //Hex Values
        public static readonly StyledProperty<bool> IsHexInputVisibleProperty = AvaloniaProperty.Register<ColorPicker, bool>("IsHexInputVisible", true, true, BindingMode.TwoWay);



        public static readonly StyledProperty<bool> IsValueScrollingEnabledProperty = AvaloniaProperty.Register<ColorPicker, bool>("IsValueScrollingEnabled", true, true, BindingMode.TwoWay);


        public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e);
        public event ColorChangedEventHandler ColorChanged;


        ColorSpectrum colorSpectrum;
        NumericUpDown redNumericUpDown, greenNumericUpDown, blueNumericUpDown,  hueNumericUpDown, saturationNumericUpDown, valueNumericUpDown;
        TextBox hexTextBox, alphaTextBox;
        Slider thirdDimensionSlider, alphaSlider;
        Grid previewGrid, textEntryGrid;
        StackPanel colorChannelTextInputPanel;
        StackPanel alphaPanel;
        ComboBox colorRepresentationComboBox;

        public ColorPicker()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            colorSpectrum = this.Get<ColorSpectrum>("ColorSpectrum");
            colorSpectrum.ColorChanged += (sender, colorEventArgs, changeSource) => {                
                    SetValue(ColorProperty, colorEventArgs.NewColor);

                SetAndRaise(ColorProperty, ref selectedColor, colorEventArgs.NewColor);


            };

            redNumericUpDown = this.Get<NumericUpDown>("RedNumericUpDown");
            greenNumericUpDown = this.Get<NumericUpDown>("GreenNumericUpDown");
            blueNumericUpDown = this.Get<NumericUpDown>("BlueNumericUpDown");

            hueNumericUpDown = this.Get<NumericUpDown>("HueNumericUpDown");
            saturationNumericUpDown = this.Get<NumericUpDown>("SaturationNumericUpDown");
            valueNumericUpDown = this.Get<NumericUpDown>("ValueNumericUpDown");

            previewGrid = this.Get<Grid>("ColorPreviewRectangleGrid");

            colorChannelTextInputPanel = this.Get<StackPanel>("ColorChannelTextInputPanel");
            alphaPanel = this.Get<StackPanel>("AlphaPanel");
            colorRepresentationComboBox = this.Get<ComboBox>("ColorRepresentationComboBox");

            hexTextBox = this.Get<TextBox>("HexTextBox");
            alphaTextBox = this.Get<TextBox>("AlphaTextBox");

            thirdDimensionSlider = this.Get<Slider>("ThirdDimensionSlider");


        }


        AvaloniaColor selectedColor;
        public AvaloniaColor Color
        {
            get => GetValue(ColorProperty);
            set
            {
                colorSpectrum.Color = value;
            }
        }

        public ColorSpectrumShape ColorSpectrumShape
        {
            get => GetValue(ColorSpectrumShapeProperty);
            set
            {
                SetValue(ColorSpectrumShapeProperty, value);
                colorSpectrum.Shape = value;
            }
        }


        public bool IsHexInputVisible
        {
            get => GetValue(IsHexInputVisibleProperty);
            set { 
                SetValue(IsHexInputVisibleProperty, value);
                hexTextBox.IsVisible = value;
            }
        }


        public bool IsColorPreviewVisible
        {
            get => GetValue(IsColorPreviewVisibleProperty);
            set
            {
                SetValue(IsColorPreviewVisibleProperty, value);
                previewGrid.IsVisible = value;
            }
        }

        public bool IsColorChannelTextInputVisible
        {
            get => GetValue(IsColorChannelTextInputVisibleProperty);
            set
            {
                SetValue(IsColorChannelTextInputVisibleProperty, value);
                colorChannelTextInputPanel.IsVisible = value;
                colorRepresentationComboBox.IsVisible = value;
            }
        }

        public bool IsAlphaTextInputVisible
        {
            get => GetValue(IsAlphaTextInputVisibleProperty);
            set
            {
                SetValue(IsAlphaTextInputVisibleProperty, value);
                alphaPanel.IsVisible = value;
            }
        }



        public bool IsValueScrollingEnabled
        {
            get => GetValue(IsValueScrollingEnabledProperty);
            set
            {
                SetValue(IsValueScrollingEnabledProperty, value);
            }
        }



        public void NumericUpDownMouseScrolled(object sender, PointerWheelEventArgs e)
        {
            if (IsValueScrollingEnabled == false)
                return; 

            var d = e.Delta.Y;

            if (sender is NumericUpDown nmb)
            {
                var newValue = Math.Round(d >= 0 ? nmb.Value + d : nmb.Value - -d);

                if (newValue >= nmb.Minimum && newValue <= nmb.Maximum)
                    nmb.Value = newValue;
            }
        }



        
        void UpdateRgbNumericValues()
        {
            redNumericUpDown.Value = Color.R;
            greenNumericUpDown.Value = Color.G;
            blueNumericUpDown.Value = Color.B;
            alphaTextBox.Text = $"{Color.A / 2.55}%";

        }

        void UpdateHsvNumericValues()
        {
            var hsv = Color.ToHsv();
            hueNumericUpDown.Value = Math.Round(hsv.H);
            saturationNumericUpDown.Value = Math.Round(hsv.S * 100);
            valueNumericUpDown.Value = Math.Round(hsv.V * 100);
        }


        private void UpdateNumericValues()
        {
            UpdateRgbNumericValues();
            UpdateHsvNumericValues();

            hexTextBox.Text = Color.ToHex(false);
        }

    }
}
