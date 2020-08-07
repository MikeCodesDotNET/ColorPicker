using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ColorPickers.Primitives
{
    public class SelectionThumb : UserControl
    {
        public static readonly StyledProperty<IBrush> OuterRingBrushProperty = AvaloniaProperty.Register<SelectionThumb, IBrush>("OuterRingBrush", new SolidColorBrush(Colors.White), true, BindingMode.TwoWay);
        public static readonly StyledProperty<IBrush> InnerRingBrushProperty = AvaloniaProperty.Register<SelectionThumb, IBrush>("InnerRingBrush", new SolidColorBrush(Colors.Black), true, BindingMode.TwoWay);

        public static readonly StyledProperty<IBrush> FillProperty = AvaloniaProperty.Register<SelectionThumb, IBrush>("Fill", new SolidColorBrush(Colors.Black), true, BindingMode.TwoWay);

        public static readonly StyledProperty<double> ThicknessProperty = AvaloniaProperty.Register<SelectionThumb, double>("Thickness", 3, true, BindingMode.TwoWay);



        private Ellipse _outerRing;
        private Ellipse _innerRing;


        public SelectionThumb()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            _outerRing = this.Get<Ellipse>("OuterRing");
            _innerRing = this.Get<Ellipse>("InnerRing");            
        }

        public IBrush OuterRingBrush
        {
            get => GetValue(OuterRingBrushProperty);
            set
            {
                SetValue(OuterRingBrushProperty, value);
                _innerRing.Stroke = value;
            }
        }

        public IBrush InnerRingBrush
        {
            get => GetValue(InnerRingBrushProperty);
            set
            {
                SetValue(InnerRingBrushProperty, value);
                _outerRing.Stroke = value;
            }

        }

        public IBrush Fill
        {
            get => GetValue(FillProperty);
            set
            {
                SetValue(FillProperty, value);
                _outerRing.Fill = value;

            }
        }

        public double Thickness
        {
            get => GetValue(ThicknessProperty);
            set
            {
                SetValue(ThicknessProperty, value);
                _outerRing.StrokeThickness = value;
            }
        }
    }

    
}
