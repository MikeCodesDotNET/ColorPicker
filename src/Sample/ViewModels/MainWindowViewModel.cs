using Avalonia;
using ColorPicker.Structures;
using ReactiveUI;

namespace Sample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        RGBColor _selectedColor;

        public RGBColor SelectedColor
        {
            get => _selectedColor;
            set { 
                this.RaiseAndSetIfChanged(ref _selectedColor, value);               
            }
        }


        public MainWindowViewModel()
        {
            SelectedColor = new RGBColor();

            
        }
    }
}
