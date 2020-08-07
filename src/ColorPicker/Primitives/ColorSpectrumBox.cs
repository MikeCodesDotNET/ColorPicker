using System;

using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ColorPickers.Helpers;

namespace ColorPickers.Primitives
{
    internal class ColorSpectrumBox : ColorSpectrumBase
    {   

        public override ColorSpectrumShape Shape => ColorSpectrumShape.Box;


        /// <summary>
        ///     The function used to draw the pixels in the color wheel.
        /// </summary>
        protected override Color ColorFunction(double x, double y)
        {
            return ColorMapping(x, y, 1.0);
        }

        protected override void DrawSpectrumBitmap(DrawingContext drawingContext)
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
                            Color color = ColorMapping(x, y, 0);

                            color_data = color.ToARGB32();

                            *(int*)bufferPtr = color_data;
                            bufferPtr += 4;
                        }
                    }
                }
            }

            drawingContext.DrawImage(writeableBitmap, Bounds);
        }



        public override void Render(DrawingContext dc)
        {
            base.Render(dc);
            DrawSpectrumBitmap(dc);
        }

        public override Color ColorMapping(double x, double y, double z)
        {
            var _h = MathHelpers.ConvertRange(0, this.Bounds.Width, 0, 360, x);
            var _s = MathHelpers.ConvertRange(0, Bounds.Height, 1, 0, y);

            Hsv hsv = new Hsv(_h, _s, 1.0);
            return hsv.ToColor();
        }

        public override Point InverseColorMapping(Color color)
        {
            Hsv hsv = color.ToHsv();
            double x, y;

            x = MathHelpers.ConvertRange(0, 360, 0, Bounds.Width, hsv.H);
            y = MathHelpers.ConvertRange(0, 100, 0, Bounds.Width, hsv.S);

            return new Point(x, y);
        }
    }
}
