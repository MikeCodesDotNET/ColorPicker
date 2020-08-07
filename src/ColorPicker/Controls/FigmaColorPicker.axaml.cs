using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using System;

namespace ColorPickers.Controls
{
    public class FigmaColorPicker : UserControl
    {
        public FigmaColorPicker()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void NumericUpDownMouseScrolled(object sender, PointerWheelEventArgs e)
        {
            var d = e.Delta.Y;

            if (sender is NumericUpDown nmb)
            {
                var newValue = Math.Round(d >= 0 ? nmb.Value + d : nmb.Value - -d);
                 
                if (newValue >= nmb.Minimum && newValue <= nmb.Maximum)
                    nmb.Value = newValue;
            }
        }
    }
}
