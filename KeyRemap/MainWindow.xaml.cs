using System;
using System.Linq;
using System.Windows;
namespace KeyRemap
{
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindowInstance;
        public string[] args;
        public MainWindow()
        {
            InitializeComponent();
            mainWindowInstance = this;
            args = Environment.GetCommandLineArgs();
            if (args.Count() > 1)
            {
                if (args[1] == "-startup")
                {
                    this.Hide();
                    this.ShowInTaskbar = false;
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Minimized;
                }
            }
        }
        private void KeyRemapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new MainPage());
        }
    }
}
