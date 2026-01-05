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
        public MainWindow()
        {
            InitializeComponent();
            Rectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            
        }
        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
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
        }
    }
}