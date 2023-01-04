using System.Windows;
namespace KeyRemap
{
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindowInstance;
        public MainWindow()
        {
            InitializeComponent();
            mainWindowInstance = this;
        }
        private void KeyRemapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new MainPage());
        }
    }
}
