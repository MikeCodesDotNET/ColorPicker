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
using ColorPickers.Helpers;

using AvaloniaColor = Avalonia.Media.Color;

namespace ColorPickers.Primitives
{
    public class ColorSpectrum : UserControl
    {
        //Dependancy Properties 
        public static readonly StyledProperty<AvaloniaColor> ColorProperty = AvaloniaProperty.Register<ColorSpectrum, AvaloniaColor>("Color", Rgb.Default.ToColor(), true, BindingMode.TwoWay);

        public static readonly StyledProperty<Hsv> HsvColorProperty = AvaloniaProperty.Register<ColorSpectrum, Hsv>("HSV", Hsv.Default, true, BindingMode.TwoWay);

        public static readonly StyledProperty<int> MaxHueProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MaxHue", 360, true, BindingMode.TwoWay);
        public static readonly StyledProperty<int> MaxSaturationProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MaxSaturation", 100, true, BindingMode.TwoWay);
        public static readonly StyledProperty<int> MaxValueProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MaxValue", 100, true, BindingMode.TwoWay);

        public static readonly StyledProperty<int> MinHueProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MinHue", 0, true, BindingMode.TwoWay);
        public static readonly StyledProperty<int> MinSaturationProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MinSaturation", 0, true, BindingMode.TwoWay);
        public static readonly StyledProperty<int> MinValueProperty = AvaloniaProperty.Register<ColorSpectrum, int>("MinValue", 0, true, BindingMode.TwoWay);

        public static readonly StyledProperty<ColorSpectrumShape> ShapeProperty = AvaloniaProperty.Register<ColorSpectrum, ColorSpectrumShape>("Shape", 0, true, BindingMode.TwoWay);

        public static readonly StyledProperty<double> ThumbSizeProperty = AvaloniaProperty.Register<ColorSpectrum, double>("ThumbSize");
        public static readonly StyledProperty<double> ThumbThicknessProperty = AvaloniaProperty.Register<ColorSpectrum, double>("ThumbThickness");
        public static readonly StyledProperty<IBrush> ThumbInnerBorderBrushProperty = AvaloniaProperty.Register<ColorSpectrum, IBrush>("ThumbInnerBorderBrush");
        public static readonly StyledProperty<IBrush> ThumbOuterBorderBrushProperty = AvaloniaProperty.Register<ColorSpectrum, IBrush>("ThumbOuterBorderBrush");



        public static readonly StyledProperty<Point> SelectionEllipsePositionProperty = AvaloniaProperty.Register<ColorSpectrum, Point>("SelectionEllipsePosition");



        //Fields 
        private Grid _grid;
        private SelectionThumb _selector;

        private bool isDragging = false;


        ColorSpectrumBase _colorSpectrum;


        public delegate void ColorChangedEventHandler(object sender, ColorChangedEventArgs e, ColorChangeSource changeSource);
        public event ColorChangedEventHandler ColorChanged;



        public ColorSpectrum()
        {
            InitializeComponent();
        }



        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _selector = this.Get<SelectionThumb>("selector");
            _grid = this.Get<Grid>("grid");

            _selector.PointerMoved += _selector_PointerMoved;
            _selector.PointerPressed += _selector_PointerPressed;
            _selector.PointerReleased += _selector_PointerReleased;
        }

        private void InstantiateSpectrum(ColorSpectrumShape shape)
        {
            if (_colorSpectrum != null)
            {
                _grid.Children.Remove(_colorSpectrum);
            }

            if (shape == ColorSpectrumShape.Box)
                _colorSpectrum = new ColorSpectrumBox();
            else
                _colorSpectrum = new ColorSpectrumRing();

            
            _colorSpectrum.Name = $"ColorSpectrum{shape}";
            _colorSpectrum.PointerPressed += Wheel_PointerPressed;
            _colorSpectrum.ZIndex = -2;
            _grid.Children.Add(_colorSpectrum);

            _colorSpectrum.PointerPressed += Wheel_PointerPressed;

            UpdateSelectorFromPoint(_colorSpectrum.InverseColorMapping(Color));

        }

        public override void Render(DrawingContext context)
        {
            //UpdateSelector();
            base.Render(context);
        }

        public AvaloniaColor Color
        {
            get => GetValue(ColorProperty);
            set {
                SetValue(ColorProperty, value);
            }
        }

        public Hsv HSV
        {
            get => GetValue(HsvColorProperty);
            set
            {
                Color = value.ToColor();
                SetValue(HsvColorProperty, value);             
            }
        }


        public Point SelectionEllipsePosition
        {
            get => GetValue(SelectionEllipsePositionProperty);
            set
            {
                SetValue(SelectionEllipsePositionProperty, value);
            }
        }


        public double ThumbSize
        {
            get => GetValue(ThumbSizeProperty);
            set
            {
                SetValue(ThumbSizeProperty, value);
                UpdateThumbSize();
            }
        }

        public double ThumbThickness
        {
            get => GetValue(ThumbThicknessProperty);
            set
            {
                SetValue(ThumbThicknessProperty, value);
                _selector.Thickness = value;
            }
        }


        public int MaxHue
        {
            get => _colorSpectrum.MaxHue;
            set
            {
                _colorSpectrum.MaxHue = value;
            }
        }

        public int MaxSaturation
        {
            get => _colorSpectrum.MaxSaturation;
            set
            {
                _colorSpectrum.MaxSaturation = value;
            }
        }

        public int MaxValue
        {
            get => _colorSpectrum.MaxValue;
            set
            {
                _colorSpectrum.MaxValue = value;
            }
        }

        public int MinHue
        {
            get => _colorSpectrum.MinHue;
            set
            {
                _colorSpectrum.MinHue = value;
            }
        }

        public int MinSaturation
        {
            get => _colorSpectrum.MinSaturation;
            set
            {
                _colorSpectrum.MinSaturation = value;
            }
        }

        public int MinValue
        {
            get => _colorSpectrum.MinValue;
            set
            {
                _colorSpectrum.MinValue = value;
            }
        }

        public ColorSpectrumShape Shape
        {
            get => GetValue(ShapeProperty);
            set
            {
                SetValue(ShapeProperty, value);
                InstantiateSpectrum(value); ;
            }
        }


        public IBrush ThumbInnerBorderBrush
        {
            get => GetValue(ThumbInnerBorderBrushProperty);
            set
            {               
                SetValue(ThumbInnerBorderBrushProperty, value);
                _selector.InnerRingBrush = value;
            }
        }

        public IBrush ThumbOuterBorderBrush
        {
            get => GetValue(ThumbOuterBorderBrushProperty);
            set
            {
                SetValue(ThumbOuterBorderBrushProperty, value);
                _selector.OuterRingBrush = value;
            }
        }




        private void _selector_PointerMoved(object sender, PointerEventArgs e)
        {
            if(isDragging)
            {
                UpdateSelectorFromPoint(e.GetPosition(this));
            }
        }

        private void _selector_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            e.Pointer.Capture(null);
            isDragging = false;
        }









        private void UpdateSelector()
        {
            if (_colorSpectrum is ColorSpectrumRing wheel)
            {
                if (!double.IsNaN(wheel.Theta) && !double.IsNaN(wheel.Rad))
                {
                    double cx = Bounds.Width / 2.0;
                    double cy = Bounds.Height / 2.0;

                    double radius = ((wheel.ActualOuterRadius - wheel.ActualInnerRadius) * wheel.Rad) + wheel.ActualInnerRadius;

                    if (radius < wheel.ActualInnerRadius + float.Epsilon)
                    {
                        radius = 0.0;
                    }

                    double angle = wheel.Theta + 180.0f;

                    double x = radius * Math.Sin((angle * Math.PI) / 180.0);
                    double y = radius * Math.Cos((angle * Math.PI) / 180.0);

                    double mx = (cx + x) - (_selector.Bounds.Width / 2);
                    double my = (cy + y) - (_selector.Bounds.Height / 2);

                    HSV = new Hsv(wheel.Theta, wheel.Rad, 1);

                    _selector.Margin = new Thickness(mx, my, 0, 0);
                    _selector.Fill = new SolidColorBrush(Color);
                }
            } 
            


        }


        private void UpdateSelectorFromPoint(Point point)
        {
            switch(Shape)
            {
                case ColorSpectrumShape.Ring:
                    UpdateSelectorFromPointForRing(point);
                    break;
                case ColorSpectrumShape.Box:
                    UpdateSelectorFromPointForBox(point);
                    break;
            }

        }


       private void UpdateSelectorFromPointForRing(Point point)
       {       
            if(_colorSpectrum is ColorSpectrumRing wheel)
            {
                wheel.Theta = wheel.CalculateTheta(point);
                wheel.Rad = wheel.CalculateR(point);
                Color = _colorSpectrum.ColorMapping(wheel.Rad, wheel.Theta, 1.0);
                ColorChanged?.Invoke(this, new ColorChangedEventArgs(Color), ColorChangeSource.SpectrumInteraction);
                //UpdateSelector();              
            }
        }

        private void UpdateSelectorFromPointForBox(Point point)
        {
            if (_colorSpectrum.Shape == ColorSpectrumShape.Box)
            {
                double x;
                double y;

                if (Bounds.Width - _selector.Width >= 0)
                    x = Math.Clamp(point.X, 0, Bounds.Width - _selector.Width);
                else
                    x = 0;

                if (Bounds.Height - _selector.Height >= 0)
                    y = Math.Clamp(point.Y, 0, Bounds.Height - _selector.Height);               
                else
                    y = 0;


                Color = _colorSpectrum.ColorMapping(x, y, 0);
                ColorChanged?.Invoke(this, new ColorChangedEventArgs(Color), ColorChangeSource.SpectrumInteraction);

                _selector.Margin = new Thickness(x, y, 0, 0);
                _selector.Fill = new SolidColorBrush(Color);
            }
        }



        private void UpdateThumbSize()
        {
            _selector.Width = ThumbSize;
            _selector.Height = ThumbSize;
        }


        private void Wheel_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            UpdateSelectorFromPoint(e.GetPosition(this));
        }

        public void _selector_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            isDragging = true;
            UpdateSelectorFromPoint(e.GetPosition(this));
        }

    }

}
