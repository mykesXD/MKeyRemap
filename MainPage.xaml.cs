using DesktopWPFAppLowLevelKeyboardHook;
using InputSimulatorStandard;
using KeyboardHookLibrary;
using KeyboardUtils;
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
        public IntPtr currentWindow;
        public string currentWindowName;
        public int rows;
        Brush rowColor;
        Brush rowStrokeColor;
        public static MainPage mainPageInstance;
        public static AddPage addPageInstance;
        public bool ValidKey;
        public string pressedKey;
        bool addPageOpen;
        string[] remap;
        public List<Guid> hotkeyIDList;
        public List<String> hotkeyWindowList;
        public List<int> lHKids; // list of unique ids for newly registered hotkeys
        public KeyboardHookManager keyboardHookManager;
        public InputSimulator keySimulator;
        public MainPage()
        {
            InitializeComponent();
            mainPageInstance = this;
            rows = 0;
            addPageOpen = false;
            CompositionTarget.Rendering += MainEventTimer;
            /*foreach (Process theprocess in processlist)
            {
                Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);Q
            }*/
            hotkeyIDList = new List<Guid>();
            hotkeyWindowList = new List<String>();

            keySimulator = new InputSimulator();
            keyboardHookManager = new KeyboardHookManager();
            keyboardHookManager.Start();
        }
        public void MainEventTimer(object sender, EventArgs e)
        {
            if (addPageOpen)
            {
                remap = AddPage.addPageInstance.remap;
            }
            currentWindowName =  GetTitle(Win32.GetForegroundWindow());
            Console.WriteLine(currentWindowName);
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

        private void DeleteButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            keyboardHookManager.UnregisterHotkey(hotkeyIDList[0]);
        }
    }
}
