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
using System.Windows.Interop;
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

        public List<IntPtr> processHWNDList;
        public List<String> processNameList;
        public List<ImageBrush> processIconList;
        public List<int> lHKids; // list of unique ids for newly registered hotkeys
        public KeyboardHookManager keyboardHookManager;
        public InputSimulator keySimulator;
        public MainPage()
        {
            InitializeComponent();
            mainPageInstance = this;
            rows = 0;
            addPageOpen = false;
            processHWNDList = new List<IntPtr>();
            processNameList = new List<String>();
            processIconList = new List<ImageBrush>();
            hotkeyIDList = new List<Guid>();
            hotkeyWindowList = new List<String>();
        CompositionTarget.Rendering += MainEventTimer;
            foreach (KeyValuePair<IntPtr, string> window in Win32.GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;

                IntPtr procId;
                Win32.GetWindowThreadProcessId(handle,out procId);
                var p = Process.GetProcessById((int)procId).MainModule.FileVersionInfo.FileDescription;
                if(p == "")
                {
                    processNameList.Add(title);
                    Console.WriteLine("{0}^",title);
                }
                else
                {
                    processNameList.Add(p);
                    try
                    {
                        Console.WriteLine(Process.GetProcessById((int)procId).MainModule.FileName);
                        Console.WriteLine(p);
                    }
                    catch
                    {
                        Console.WriteLine("{0},FAIL",p);
                    }
                }
                using (System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetProcessById((int)procId).MainModule.FileName))
                {
                    processIconList.Add(new ImageBrush(Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));
                }
            }

            hotkeyWindowList = new List<String>();
            EditButton.Fill = processIconList[2];
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
            //Console.WriteLine(currentWindowName);
        }
        static string GetTitle(IntPtr handle, int length = 128)
        {
            StringBuilder builder = new StringBuilder(length);
            Win32.GetWindowText(handle, builder, length + 1);
            IntPtr procId;
            Win32.GetWindowThreadProcessId(handle, out procId);
            var p = builder.ToString();
            try
            {
                p = Process.GetProcessById((int)procId).MainModule.FileVersionInfo.FileDescription;
            }
            catch
            {
                Console.WriteLine("SYSTEM FILE");
            }
            if (p == "")
            {
                return builder.ToString();
            }
            return p;
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

        private void EditButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
