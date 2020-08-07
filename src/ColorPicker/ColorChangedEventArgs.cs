using Avalonia.Media;
using System;

namespace ColorPickers
{

    /// <summary>
    /// Occurs when the Color property has changed.
    /// </summary>
    public class ColorChangedEventArgs :EventArgs
    {
        public Color OldColor { get;  }

        public Color NewColor { get;  }

        public ColorChangedEventArgs(Color oldColor, Color newColor)
        {
            OldColor = oldColor;
            NewColor = newColor;
        }

        public ColorChangedEventArgs(Color newColor)
        {
            NewColor = newColor;
        }
    }

    public enum ColorChangeSource
    {
        SpectrumInteraction,
        NumericValueChange,
        SliderChange,
        HexChange,
        Unknown

    }
}
