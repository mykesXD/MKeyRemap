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
        private LowLevelKeyboardListener _listener;
        Process[] processes;
        //Process[] processlist = Process.GetProcesses().Where(p => (long)p.MainWindowHandle != 0).ToArray();
        IntPtr currentWindow;
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
            CompositionTarget.Rendering += MainEventTimer;
            processes = Process.GetProcessesByName("chrome");
            /*foreach (Process theprocess in processlist)
            {
                Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);
            }*/
        }
        public void MainEventTimer(object sender, EventArgs e)
        {
            //Console.WriteLine(GetTitle(Win32.GetForegroundWindow()));
            Console.WriteLine(rows);

            currentWindow = Win32.GetForegroundWindow();

            
        }
        static string GetTitle(IntPtr handle, int length = 128)
        {
            StringBuilder builder = new StringBuilder(length);
            Win32.GetWindowText(handle, builder, length + 1);
            return builder.ToString();
        }
        public void CloseButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            KeyRemapWindow.Visibility = Visibility.Hidden;
        }

        private void KeyRemapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new MainPage());
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += _listener_OnKeyPressed;

            _listener.HookKeyboard();
        }
        public void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            if(e.KeyPressed.ToString() == "Home")
            {
                const UInt32 WM_KEYDOWN = 0x0100;
                const int VK_F5 = 0x74;
                Win32.PostMessage(currentWindow, WM_KEYDOWN, VK_F5, 0);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _listener.UnHookKeyboard();
        }

        private void AddButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
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
            ContainerCanvas.Children.Add(rowBody);
        }
    }
}
