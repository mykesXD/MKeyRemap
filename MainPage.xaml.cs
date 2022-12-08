using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyRemap
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        Brush rowColor;
        Brush rowStrokeColor;
        public static MainPage mainPageInstance;
        public MainPage()
        {
            InitializeComponent();
            mainPageInstance = this;

        }

        private void AddButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("WTFFFFFFFFFFFFFFFFFFFF");
            this.NavigationService.Navigate(new AddPage());
        }

        private void ImportButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindowInstance.rows += 1;
            rowColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffaacc");
            rowStrokeColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffffff");
            Rectangle rowBody = new Rectangle
            {
                Width = 100,
                Height = 50,
                Fill = rowColor,
                StrokeThickness = 1,
                Stroke = rowStrokeColor
            };
            Canvas.SetLeft(rowBody, 50.0);
            Canvas.SetTop(rowBody, 100.0);
            BodyContainer.Children.Add(rowBody);

        }
    }
}
