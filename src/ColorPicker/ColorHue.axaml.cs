using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ColorPicker.ColorConversion;
using ColorPicker.ColorModels;
using ColorPicker.Utilities;
using System;

namespace ColorPicker
{
    public class ColorHue : UserControl
    {

        public static readonly StyledProperty<double> ThumbSizeProperty = AvaloniaProperty.Register<ColorWheel, double>(nameof(ThumbSize));
        public static readonly StyledProperty<double> HueProperty = AvaloniaProperty.Register<ColorWheel, double>(nameof(Hue));
        public static readonly StyledProperty<double> SaturationProperty = AvaloniaProperty.Register<ColorWheel, double>(nameof(Saturation));

        public static readonly StyledProperty<double> BrightnessProperty = AvaloniaProperty.Register<ColorWheel, double>(nameof(Brightness));
        public static readonly StyledProperty<IColorVector> SelectedColorProperty = AvaloniaProperty.Register<ColorWheel, IColorVector>(nameof(SelectedColor), new RGBColor(1, 1, 1), false, Avalonia.Data.BindingMode.TwoWay);


        //UI Controls (defined in XAML)
        private Ellipse _selector;
        private Grid _grid;

        private Type colorViewClass = typeof(ColorHuePanel);
        private ColorHuePanel colorView;
        private bool isDragging = false;


        public ColorHue()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _grid = this.Get<Grid>("grid");
            _selector = this.Get<Ellipse>("hue_selector");

            _selector.PointerMoved += _selector_PointerMoved;
            _selector.PointerPressed += _selector_PointerPressed;
            _selector.PointerReleased += _selector_PointerReleased;

            ColorGradientType = typeof(ColorHuePanel);

        }

        public Type ColorGradientType
        {
            get { return typeof(ColorHue); }
            set
            {
                colorViewClass = value;
                InstantiateHue();
            }
        }


        void InstantiateHue()
        {
            //Leaving this here in case I create more color wheel types...

            if (colorView != null)
                this._grid.Children.Remove(colorView);

            if (colorViewClass != null)
            {
                colorView = (ColorHuePanel)Activator.CreateInstance(colorViewClass);
                colorView.Name = "wheel";
                colorView.ZIndex = -2;
                _grid.Children.Add(colorView);

                colorView.PointerPressed += hue_PointerPressed;
            }
        }

        //Public properties
        public double ThumbSize
        {
            get { return (double)GetValue(ThumbSizeProperty); }
            set { SetValue(ThumbSizeProperty, value); UpdateThumbSize(); }
        }

        public double Hue
        {
            get { return (double)GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }


        public double Saturation
        {
            get { return (double)GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }


        public double Brightness
        {
            get { return (double)GetValue(BrightnessProperty); }
            set { SetValue(BrightnessProperty, value); }
        }

        public IColorVector SelectedColor
        {
            get { return GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        //Pointer Events
        private void hue_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            // e.Pointer.Capture(this);
            UpdateSelectorFromPoint(e.GetPosition(this));
        }

        private void _selector_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            e.Pointer.Capture(null);
            isDragging = false;
        }

        private void _selector_PointerMoved(object sender, PointerEventArgs e)
        {
            if (isDragging)
            {
                // Calculate Theta and Rad from the mouse position
                UpdateSelectorFromPoint(e.GetPosition(this));
            }
        }

        public void _selector_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            isDragging = true;
            UpdateSelectorFromPoint(e.GetPosition(this));
        }

        ColorModelConverter converter = new ColorModelConverter();

        private void UpdateSelectorFromPoint(Point point)
        {
            var x = Math.Clamp(point.X, 0, Bounds.Width - _selector.Width);
            var y = Math.Clamp(point.Y, 0, Bounds.Height - _selector.Height);

            SelectedColor = colorView.ColorMapping(x, y);
            _selector.Margin = new Thickness(x, y, 0, 0);
            _selector.Fill = new SolidColorBrush(converter.ToRGB(SelectedColor));
        }


        private void UpdateThumbSize()
        {
            _selector.Width = ThumbSize;
            _selector.Height = ThumbSize;
        }
    }



    public class ColorHuePanel : Panel
    {

        public double ConvertRange(double originalStart, double originalEnd, double newStart, double newEnd, double value) 
        {
            double scale = (newEnd - newStart) / (originalEnd - originalStart);
            return (newStart + ((value - originalStart) * scale));
        }

        protected void DrawBackground(DrawingContext drawingContext)
        {
            int bmp_width = (int)Bounds.Width;
            int bmp_height = (int)Bounds.Height;

            if ((bmp_width <= 0) || (bmp_height <= 0))
            {
                return;
            }

          
            WriteableBitmap writeableBitmap = new WriteableBitmap(new PixelSize(bmp_width, bmp_height), new Vector(96, 96), PixelFormat.Bgra8888);

            using (ILockedFramebuffer lockedFrameBuffer = writeableBitmap.Lock())
            {
                unsafe
                {
                    IntPtr bufferPtr = new IntPtr(lockedFrameBuffer.Address.ToInt64());

                    for (int y = 0; y < bmp_height; y++)
                    {
                        for (int x = 0; x < bmp_width; x++)
                        {
                            int color_data = 0;
                            RGBColor rgbColor = (RGBColor)ColorMapping(x, y);
                                                    
                            color_data = rgbColor.ToARGB32(255);                            

                            *(int*)bufferPtr = color_data;
                            bufferPtr += 4;
                        }
                    }
                }
            }

            drawingContext.DrawImage(writeableBitmap, Bounds);
        }

        ColorModelConverter converter = new ColorModelConverter();

        private const double whiteFactor = 2.2; // Provide more accuracy around the white-point

        public IColorVector ColorMapping(double x, double y)
        {
            var _h = ConvertRange(0, this.Bounds.Width, 0, 360, x);
            var _s = ConvertRange(0, Bounds.Height, 1, 0, y);

            HSVColor hsv = new HSVColor(_h, _s, 1.0);
            RGBColor rgb = converter.ToRGB(hsv);
            return rgb;
        }

        public Point InverseColorMapping(IColorVector color)
        {         
            var hsv = converter.ToHSV(color);
            double x, y;

            x = ConvertRange(0, 360, 0, this.Bounds.Width, hsv.H);
            y = ConvertRange(0, 100, 0, this.Bounds.Width, hsv.S);

            return new Point(x, y);
        }

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            DrawBackground(dc);
        }
    }
}
