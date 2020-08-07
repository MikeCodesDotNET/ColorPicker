using System;
using System.Diagnostics;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ColorPickers.Helpers;
using HA = Avalonia.Layout.HorizontalAlignment;
using Point = Avalonia.Point;
using VA = Avalonia.Layout.VerticalAlignment;


namespace ColorPickers.Primitives
{
    internal class ColorSpectrumRing : ColorSpectrumBase
    {
        public static StyledProperty<double> InnerRadiusProperty = AvaloniaProperty.Register<ColorSpectrumBase, double>(nameof(InnerRadius));

        public double ActualInnerRadius => ActualOuterRadius * InnerRadius;
        public double ActualOuterRadius { get; private set; }
        public double InnerRadius { get => GetValue(InnerRadiusProperty); set => SetValue(InnerRadiusProperty, value); }

        double theta;
        public double Theta
        {
            get { return theta; }
            set { theta = MathHelpers.Mod(value); }
        }

        double rad;

        public double Rad
        {
            get { return rad; }
            set { rad = value; }
        }

        public override ColorSpectrumShape Shape => ColorSpectrumShape.Ring;


        /// <summary>
        ///     The function used to draw the pixels in the color wheel.
        /// </summary>
        protected override Color ColorFunction(double x, double y)
        {
            return ColorMapping(x, y, 1.0);
        }

        protected override void DrawSpectrumBitmap(DrawingContext drawingContext)
        {
            float cx = ((float)Bounds.Width) / 2.0f;
            float cy = ((float)Bounds.Height) / 2.0f;

            float outer_radius = Math.Min(cx, cy);
            ActualOuterRadius = outer_radius;

            int bmp_width = (int)Bounds.Width;
            int bmp_height = (int)Bounds.Height;

            if ((bmp_width <= 0) || (bmp_height <= 0))
            {
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (borderShape == null)
            {
                borderShape = new Ellipse();
                borderShape.Fill = new SolidColorBrush(Colors.Transparent);

                borderShape.Stroke = (SolidColorBrush)Application.Current.FindResource("ThemeControlHighlightLowBrush");
                borderShape.StrokeThickness = 6;
                borderShape.IsHitTestVisible = false;
                borderShape.Opacity = 50;
                Children.Add(borderShape);
                borderShape.HorizontalAlignment = HA.Center;
                borderShape.VerticalAlignment = VA.Center;
            }

            borderShape.Width = Math.Min(bmp_width, bmp_height) + (borderShape.StrokeThickness / 2);
            borderShape.Height = Math.Min(bmp_width, bmp_height) + (borderShape.StrokeThickness / 2);

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

                            double dx = x - cx;
                            double dy = y - cy;
                            double pr = Math.Sqrt((dx * dx) + (dy * dy));

                            if (pr <= outer_radius)
                            {
                                double pa = Math.Atan2(dx, dy);
                                Color color = ColorFunction(pr / outer_radius, ((pa + Math.PI) * 180.0) / Math.PI);

                                double aadelta = pr - (outer_radius - 1.0);
                                byte alpha = 255;
                                if (aadelta >= 0.0)
                                {
                                    alpha = (byte)(255 - (aadelta * 255));
                                }

                                color_data = color.ToARGB32();
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


        private const double whiteFactor = 2.2;


        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            DrawSpectrumBitmap(dc);
        }

        public override Color ColorMapping(double x, double y, double z)
        {
            var h = y;
            var s = x;
            var v = z;

            Hsv hsv = new Hsv((float)h, (float)Math.Pow(s, whiteFactor), (float)v);
            return hsv.ToColor();
        }

        public override Point InverseColorMapping(Color color)
        {
            double theta, rad;
            Hsv hsv = color.ToHsv();
            theta = hsv.H;
            rad = Math.Pow(hsv.S, 1.0 / whiteFactor);

            return new Point(theta, rad);
        }


        internal double CalculateTheta(Point point)
        {
            double cx = Bounds.Width / 2;
            double cy = Bounds.Height / 2;

            double dx = point.X - cx;
            double dy = point.Y - cy;

            double angle = Math.Atan2(dx, dy) / Math.PI * 180.0;

            // Theta is offset by 180 degrees, so red appears at the top
            return MathHelpers.Mod(angle - 180.0);
        }

        internal double CalculateR(Point point)
        {
            double cx = Bounds.Width / 2;
            double cy = Bounds.Height / 2;

            double dx = point.X - cx;
            double dy = point.Y - cy;

            double dist = Math.Sqrt(dx * dx + dy * dy);

            return Math.Min(dist, ActualOuterRadius) / ActualOuterRadius;
        }

    }
}
