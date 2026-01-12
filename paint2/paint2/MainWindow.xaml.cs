using Malovani;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
using Color = System.Windows.Media.Color;

namespace paint2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Color SelectedColor { get; set; } = Color.FromRgb(0, 0, 0);
        public double BrushWidth { get; set; } = 10;
        public MainWindow()
        {
            InitializeComponent();
            ButtonColorPicker.Background = new SolidColorBrush(SelectedColor);
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "Image files | *.bmp; *.jpg; *.gif; *.png; *.tif | All files | *.*"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.Value)
            {
                // Open document
                string filename = dlg.FileName;
            }
        }
        private void LoadMenuItem_Click(Object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".png"; // Default file extension
            dlg.Filter = "Image files | *.bmp; *.jpg; *.gif; *.png; *.tif | All files | *.*"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result.Value)
            {
                // Open document
                string filename = dlg.FileName;
            }
        }
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            RGBColorPicker rGBColorPicker = new RGBColorPicker();
            rGBColorPicker.ShowDialog(); // musí si vybrat barvu

            if(rGBColorPicker.Success == true)
            {
                SelectedColor = rGBColorPicker.Color;
                ButtonColorPicker.Background = new SolidColorBrush(SelectedColor);
            }
        }

        private void BrushWidthSlider_Changed(object sender, EventArgs e)
        {
            BrushWidth = SliderBrushWidth.Value;
        }
        private void MouseButtonDown(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released) { return; }
            // Získání aktuální pozice myši
            System.Windows.Point currentPoint = e.GetPosition(MainCanvas);

            // Vytvoření kruhu (Ellipse)
            Ellipse circle = new Ellipse
            {
                Width = BrushWidth,
                Height = BrushWidth,
                Fill = new SolidColorBrush(SelectedColor) // Barva kruhu
            };

            // Nastavení pozice kruhu
            Canvas.SetLeft(circle, currentPoint.X - BrushWidth);
            Canvas.SetTop(circle, currentPoint.Y - BrushWidth);

            // Přidání kruhu na Canvas
            MainCanvas.Children.Add(circle);
        }
    }
}