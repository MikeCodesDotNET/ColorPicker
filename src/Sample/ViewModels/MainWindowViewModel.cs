using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ColorPickers;
using ReactiveUI;

namespace Sample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        Color _selectedColor;

        public void GoToRedCommand()
        {
            SelectedColor = Avalonia.Media.Colors.Red;
        }

        public void GoToGreenCommand()
        {
            SelectedColor = Avalonia.Media.Colors.Green;

        }

        public void GoToBlueCommand()
        {
            SelectedColor = Avalonia.Media.Colors.Blue;

        }

        public void GoToPurpleCommand()
        {
            SelectedColor = Avalonia.Media.Colors.Purple;
        }



        public Color SelectedColor
        {
            get => _selectedColor;
            set { 
                this.RaiseAndSetIfChanged(ref _selectedColor, value);               
            }
        }


        public void ShowPickerDialog()
        {
            var window = new Window();
            //var content = new ColorPickers.Controls.FigmaColorPicker();
            //window.Content = content;
            //window.Width = content.Width;
            //window.Height = content.Height;
            //window.Title = "Color Picker";
            window.Show();
        }

        public MainWindowViewModel()
        {
            SelectedColor = Rgb.Default.ToColor();

        }
    }
}
