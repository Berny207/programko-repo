using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RGB_míchátko
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool loaded = false;
        public MainWindow()
        {
            InitializeComponent();
            loaded = true;
            Rectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            
        }
        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(loaded == false) { return; }
			byte red = Convert.ToByte(double.Round(SliderRed.Value));
            byte green = Convert.ToByte(double.Round(SliderGreen.Value));
            byte blue = Convert.ToByte(double.Round(SliderBlue.Value));
            TextBoxRed.Text = red.ToString();
            TextBoxGreen.Text = green.ToString();
            TextBoxBlue.Text = blue.ToString();
            Rectangle.Fill = new SolidColorBrush(Color.FromRgb(red, green, blue));

            LabelColor.Content = $"#{Convert.ToHexString([red, green, blue])}";
        }
        private static readonly Regex number_regex = new Regex("[^1-9]");
        private static bool IsTextNumerical(string text)
        {
            return number_regex.IsMatch(text);
        }
        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumerical(e.Text);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if(loaded == false) { return; }
			byte red = 0;
            byte green = 0;
            byte blue = 0;
			TextBox source = (TextBox)sender;
            if (source.Name == "TextBoxRed")
            {
                updateTextBox(source, SliderRed);
			}
            else if (source.Name == "TextBoxGreen")
            {
				updateTextBox(source, SliderGreen);
			}
            else if (source.Name == "TextBoxBlue")
            {
				updateTextBox(source, SliderBlue);
			}
		}

        private void updateTextBox(TextBox source, Slider boundSlider)
        {
			if (source.Text == "") { return; }
			long input = Convert.ToUInt32(source.Text);
			if (input > 255) { input = 255; source.Text = "255"; MessageBox.Show("Cannot input value higher than 255."); }
			boundSlider.Value = input;
            SliderValueChanged(source, null);
		}
    }
}