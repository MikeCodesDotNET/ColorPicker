
using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

using Avalonia.Media.Imaging;

using Avalonia.Platform;
using ColorPicker.ColorModels;
using ColorPicker.Utilities;

using HA = Avalonia.Layout.HorizontalAlignment;
using Point = Avalonia.Point;
using VA = Avalonia.Layout.VerticalAlignment;


namespace ColorPicker.Wheels
{
    public abstract class ColorWheelBase : Panel
    {

        public static StyledProperty<double> InnerRadiusProperty = AvaloniaProperty.Register<HSVWheel, double>(nameof(InnerRadius));

        private Ellipse border;

        /// <summary>
        ///     The function used to draw the pixels in the color wheel.
        /// </summary>
        private RGBColor ColorFunction(double r, double theta)
        {
            return ColorMapping(r, theta, 1.0);
        }

        protected void DrawHsvDial(DrawingContext drawingContext)
        {
            float cx = ((float)Bounds.Width) / 2.0f;
            float cy = ((float)Bounds.Height) / 2.0f;

            float outer_radius = Math.Min(cx, cy);
            ActualOuterRadius = outer_radius;

            int bmp_width = (int)Bounds.Width;
            int bmp_height = (int)Bounds.Height;

            if((bmp_width <= 0) || (bmp_height <= 0))
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if(border == null)
            {
                border = new Ellipse();
                border.Fill = new SolidColorBrush(Colors.Transparent);
                border.Stroke = new SolidColorBrush(Colors.Black);
                border.StrokeThickness = 3;
                border.IsHitTestVisible = false;
                border.Opacity = 50;
                Children.Add(border);
                border.HorizontalAlignment = HA.Center;
                border.VerticalAlignment = VA.Center;
            }

            border.Width = Math.Min(bmp_width, bmp_height) + (border.StrokeThickness / 2);
            border.Height = Math.Min(bmp_width, bmp_height) + (border.StrokeThickness / 2);

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

                            double dx = x - cx;
                            double dy = y - cy;
                            double pr = Math.Sqrt((dx * dx) + (dy * dy));

                            if(pr <= outer_radius)
                            {
                                double pa = Math.Atan2(dx, dy);
                                RGBColor rgbColor = ColorFunction(pr / outer_radius, ((pa + Math.PI) * 180.0) / Math.PI);

                                double aadelta = pr - (outer_radius - 1.0);
                                byte alpha = 255;
                                if(aadelta >= 0.0)
                                {
                                    alpha = (byte)(255 - (aadelta * 255));
                                }

                                color_data = rgbColor.ToARGB32(alpha);
                            }

                            *(int*)bufferPtr = color_data;
                            bufferPtr += 4;
                        }
                    }
                }
            }

            drawingContext.DrawImage(writeableBitmap, Bounds);

            stopwatch.Stop();
            Debug.WriteLine($"YO! This puppy took {stopwatch.ElapsedMilliseconds} MS to complete");
        }

        /// <summary>
        ///     The color mapping between Rad/Theta and RGB
        /// </summary>
        /// <param name="r"> Radius/Saturation, between 0 and 1 </param>
        /// <param name="theta"> Angle/Hue, between 0 and 360 </param>
        /// <returns> The RGB colour </returns>
        public virtual RGBColor ColorMapping(double radius, double theta, double value) { return new RGBColor(1.0f, 1.0f, 1.0f); }

        public virtual Point InverseColorMapping(IColorVector color) { return new Point(0, 0); }

        public static void OnPropertyChanged(AvaloniaObject obj, AvaloniaPropertyChangedEventArgs args)
        {
            HSVWheel ctl = obj as HSVWheel;
            ctl.InvalidateVisual();
        }


        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            DrawHsvDial(dc);
        }


        public double ActualInnerRadius => ActualOuterRadius * InnerRadius;


        public double ActualOuterRadius { get; private set; }


        public double InnerRadius
        {
            get => GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

    }
}

