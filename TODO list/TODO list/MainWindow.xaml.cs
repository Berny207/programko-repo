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

namespace TODO_list
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel viewModel = new ViewModel();
            DataContext = viewModel;
        }
        private static readonly Regex number_regex = new Regex("[^0-9]");
        private static bool IsTextNumerical(string text)
        {
            return number_regex.IsMatch(text);
        }
        public void IntegerCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumerical(e.Text);
        }
    }
}