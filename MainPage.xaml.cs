using DesktopWPFAppLowLevelKeyboardHook;
using NonInvasiveKeyboardHookLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
        private LowLevelKeyboardListener _listener;
        Process[] processes;
        //Process[] processlist = Process.GetProcesses().Where(p => (long)p.MainWindowHandle != 0).ToArray();
        IntPtr currentWindow;
        public int rows;
        Brush rowColor;
        Brush rowStrokeColor;
        public static MainPage mainPageInstance;
        public static AddPage addPageInstance;
        public bool ValidKey;
        public string pressedKey;
        bool addPageOpen;
        string[] remap;
        public List<int> lHKids; // list of unique ids for newly registered hotkeys
        KeyboardHookManager keyboardHookManager;
        public MainPage()
        {
            InitializeComponent();
            mainPageInstance = this;
            rows = 0;
            addPageOpen = false;
            CompositionTarget.Rendering += MainEventTimer;
            processes = Process.GetProcessesByName("chrome");
            const UInt32 WM_KEYDOWN = 0x0100;
            const int VK_F5 = 0x74;
            /*foreach (Process theprocess in processlist)
            {
                Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);Q
            }*/
            keyboardHookManager = new KeyboardHookManager();
            keyboardHookManager.Start();
            keyboardHookManager.RegisterHotkey(0x24, () =>
            {
                Console.WriteLine("NumPad0 detected");
                Win32.PostMessage(currentWindow, WM_KEYDOWN, VK_F5, 0);
            });
        }
        public void MainEventTimer(object sender, EventArgs e)
        {
            //Console.WriteLine(GetTitle(Win32.GetForegroundWindow()));
            if (addPageOpen)
            {
                remap = AddPage.addPageInstance.remap;
            }
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
            MainWindow.mainWindowInstance.Visibility = Visibility.Hidden;
        }
        private void MenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _listener = new LowLevelKeyboardListener();
            _listener.OnKeyPressed += _listener_OnKeyPressed;
            _listener.HookKeyboard();
        }
        public void _listener_OnKeyPressed(object sender, KeyPressedArgs e)
        {
            pressedKey = e.KeyPressed.ToString();
            
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void AddButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("WTFFFFFFFFFFFFFFFFFFFF");
            addPageOpen = true;
            this.NavigationService.Navigate(new AddPage());
        }

        public void ImportButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            rows += 1;
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
