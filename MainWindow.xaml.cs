using DesktopWPFAppLowLevelKeyboardHook;
using KeyRemap.ViewModels;
using KeyRemap.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Application = System.Windows.Application;

namespace KeyRemap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Brush rowColor;
        Brush rowStrokeColor;
        public int rows;
        Canvas ContainerCanvas { get; set; }
        public static MainWindow mainWindowInstance;
        public MainWindow()
        {

            InitializeComponent();
            mainWindowInstance = this;
            rows = 0;
            /*foreach (Process theprocess in processlist)
            {
                Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);
            }*/
        }
        private void KeyRemapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new MainPage());
        }
    }
        
}
