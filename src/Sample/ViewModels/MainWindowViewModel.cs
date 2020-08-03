using Avalonia;
using ColorPicker.ColorModels;
using ReactiveUI;

namespace Sample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        RGBColor _selectedColor;

        public void GoToRedCommand()
        {
            SelectedColor = new RGBColor(Avalonia.Media.Colors.Red);
        }

        public void GoToGreenCommand()
        {
            SelectedColor = new RGBColor(Avalonia.Media.Colors.Green);

        }

        public void GoToBlueCommand()
        {
            SelectedColor = new RGBColor(Avalonia.Media.Colors.Blue);

        }

        public void GoToPurpleCommand()
        {
            SelectedColor = new RGBColor(Avalonia.Media.Colors.Purple);
        }



        public RGBColor SelectedColor
        {
            get => _selectedColor;
            set { 
                this.RaiseAndSetIfChanged(ref _selectedColor, value);               
            }
        }


        public MainWindowViewModel()
        {
            SelectedColor = new RGBColor(0, 0, 0);

            
        }
    }
}
