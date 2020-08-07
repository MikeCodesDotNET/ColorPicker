using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ColorPickers.Controls
{
    public class ColorHue : UserControl
    {

        public static readonly StyledProperty<double> BrightnessProperty = AvaloniaProperty.Register<ColorWheel, double>(nameof(Brightness));
        public static readonly StyledProperty<double> HueProperty = AvaloniaProperty.Register<ColorWheel, double>(nameof(Hue));
        public static readonly StyledProperty<double> SaturationProperty = AvaloniaProperty.Register<ColorWheel, double>(nameof(Saturation));
        public static readonly StyledProperty<Color> SelectedColorProperty = AvaloniaProperty.Register<ColorWheel, Color>(nameof(SelectedColor), Rgb.Default.ToColor(), false, BindingMode.TwoWay);

        public static readonly StyledProperty<double> ThumbSizeProperty = AvaloniaProperty.Register<ColorWheel, double>(nameof(ThumbSize));
        private Grid _grid;


        private Ellipse _selector;
        private ColorHueGradientPanel colorView;

        private Type colorViewClass = typeof(ColorHueGradientPanel);
        private bool isDragging = false;


        public ColorHue()
        {
            InitializeComponent();
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

        private void hue_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            UpdateSelectorFromPoint(e.GetPosition(this));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            _grid = this.Get<Grid>("grid");
            _selector = this.Get<Ellipse>("hue_selector");

            _selector.PointerMoved += _selector_PointerMoved;
            _selector.PointerPressed += _selector_PointerPressed;
            _selector.PointerReleased += _selector_PointerReleased;

            ColorGradientType = typeof(ColorHueGradientPanel);
        }


        private void InstantiateHue()
        {
            if(colorView != null)
            {
                _grid.Children.Remove(colorView);
            }

            if(colorViewClass != null)
            {
                colorView = (ColorHueGradientPanel)Activator.CreateInstance(colorViewClass);
                colorView.Name = "wheel";
                colorView.ZIndex = -2;
                _grid.Children.Add(colorView);

                colorView.PointerPressed += hue_PointerPressed;
            }
        }

        private void UpdateSelectorFromPoint(Point point)
        {
            double x = Math.Clamp(point.X, 0, Bounds.Width - _selector.Width);
            double y = Math.Clamp(point.Y, 0, Bounds.Height - _selector.Height);

            SelectedColor = colorView.ColorMapping(x, y);
            _selector.Margin = new Thickness(x, y, 0, 0);
            _selector.Fill = new SolidColorBrush(SelectedColor);
        }


        private void UpdateThumbSize()
        {
            _selector.Width = ThumbSize;
            _selector.Height = ThumbSize;
        }

        public void _selector_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            isDragging = true;
            UpdateSelectorFromPoint(e.GetPosition(this));
        }


        public double Brightness { get => GetValue(BrightnessProperty); set => SetValue(BrightnessProperty, value); }

        public Type ColorGradientType
        {
            get => typeof(ColorHue);
            set
            {
                colorViewClass = value;
                InstantiateHue();
            }
        }

        public double Hue { get => GetValue(HueProperty); set => SetValue(HueProperty, value); }


        public double Saturation { get => GetValue(SaturationProperty); set => SetValue(SaturationProperty, value); }

        public Color SelectedColor { get => GetValue(SelectedColorProperty); set => SetValue(SelectedColorProperty, value); }

        public double ThumbSize
        {
            get => GetValue(ThumbSizeProperty);
            set
            {
                SetValue(ThumbSizeProperty, value);
                UpdateThumbSize();
            }
        }

    }


    internal class ColorHueGradientPanel : Panel
    {
        protected void DrawBackground(DrawingContext drawingContext)
        {
            int bmp_width = (int)Bounds.Width;
            int bmp_height = (int)Bounds.Height;

            if((bmp_width <= 0) || (bmp_height <= 0))
            {
                return;
            }

            WriteableBitmap writeableBitmap = new WriteableBitmap(new PixelSize(bmp_width, bmp_height), new Vector(96, 96), PixelFormat.Bgra8888);

            using(ILockedFramebuffer lockedFrameBuffer = writeableBitmap.Lock())
            {
                unsafe
                {
                    IntPtr bufferPtr = new IntPtr(lockedFrameBuffer.Address.ToInt64());

                    for(int y = 0; y < bmp_height; y++)
                    {
                        for(int x = 0; x < bmp_width; x++)
                        {
                            int color_data = 0;
                            Color color = ColorMapping(x, y);
                            color_data = color.ToARGB32();

                            *(int*)bufferPtr = color_data;
                            bufferPtr += 4;
                        }
                    }
                }
            }

            drawingContext.DrawImage(writeableBitmap, Bounds);
        }

        public Color ColorMapping(double x, double y)
        {
            double _h = ConvertRange(0, Bounds.Width, 0, 360, x);
            double _s = ConvertRange(0, Bounds.Height, 1, 0, y);

            Hsv hsv = new Hsv(_h, _s, 1.0);
            return hsv.ToColor();
        }

        public double ConvertRange(double originalStart, double originalEnd, double newStart, double newEnd, double value)
        {
            double scale = (newEnd - newStart) / (originalEnd - originalStart);
            return newStart + ((value - originalStart) * scale);
        }

        public Point InverseColorMapping(Rgb rgb)
        {
            Hsv hsv = rgb.ToHsv();
            double x, y;

            x = ConvertRange(0, 360, 0, Bounds.Width, hsv.H);
            y = ConvertRange(0, 100, 0, Bounds.Width, hsv.S);

            return new Point(x, y);
        }

        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            DrawBackground(dc);
        }

    }
}
