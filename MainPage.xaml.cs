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
        public string prevWindowName;
        public int rows;
        public int windowKeymap;
        public int selectedRowIndex;
        Brush rowColor;
        Brush rowStrokeColor;
        public static MainPage mainPageInstance;
        public static AddPage addPageInstance;
        public bool ValidKey;
        public string pressedKey;
        public bool addPageOpen;
        public bool editPageOpen;
        public List<String> remap;
        public List<Guid> hotkeyIDList;
        public List<List<String>> hotkeyList;
        public List<String> realHotkeyList;
        public List<String> hotkeyWindowList;
        public List<Brush> hotkeyIconList;
        public List<IntPtr> processHWNDList;
        public List<String> processNameList;
        public List<ImageBrush> processIconList;
        public List<int> lHKids; // list of unique ids for newly registered hotkeys
        public KeyboardHookManager keyboardHookManager;
        public InputSimulator keySimulator;
        public List<Keymap> keyMapList;
        public List<Keymap> activeKeyMapList;
        public List<String> activeWindowList;
        public MainPage()
        {
            InitializeComponent();
            mainPageInstance = this;
            rows = 0;
            prevWindowName = "";
            addPageOpen = false;
            processHWNDList = new List<IntPtr>();
            processNameList = new List<String>();
            processIconList = new List<ImageBrush>();
            hotkeyIDList = new List<Guid>();
            hotkeyList = new List<List<String>>();
            realHotkeyList = new List<String>();
            hotkeyWindowList = new List<String>();
            hotkeyIconList = new List<Brush>();
            keyMapList = new List<Keymap>();
            activeWindowList = new List<String>();
            selectedRowIndex = -1;
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
            keySimulator = new InputSimulator();
            keyboardHookManager = new KeyboardHookManager();
            keyboardHookManager.Start();
        }
        public void MainEventTimer(object sender, EventArgs e)
        {
            currentWindowName =  GetTitle(Win32.GetForegroundWindow());

            // Running same hotkeys that has different activation windows
            // Works when activation windows are specified but when adding *Everywhere* activation, it overrides the others
            // Sorted the list to make activations with *Everywhere* to be first so it gets overrided by specified window hotkeys
            if (currentWindowName != prevWindowName)
            {
                List<Keymap> sortedList = keyMapList.OrderBy(o => o.window).ToList();
                Console.WriteLine(currentWindowName);
                foreach (Keymap keymap in sortedList)
                {
                    Console.WriteLine(keymap.window);
                    if (currentWindowName.Contains(keymap.window) || keymap.window.Contains("Everywhere"))
                    {
                            keymap.Register();
                    }
                    else
                    {
                        try
                        {
                            keymap.Unregister();
                        }
                        catch
                        {
                            Console.WriteLine("EXCEPTION");
                        }
                    }
                }
                prevWindowName = currentWindowName;
            }
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
            NavigationService.Navigate(new AddPage());
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
        }

        private void EditButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            editPageOpen = true;
            NavigationService.Navigate(new AddPage());
        }

        private void BodyBackground_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < rows; i++)
            {
                var pain = Util.ChildrenInRow(BodyContainer, i);
                Rectangle painer = (Rectangle)pain.ToList()[4];
                painer.StrokeThickness = 0;
            }
        }
        private void ContainerCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < rows; i++)
            {
                var pain = Util.ChildrenInRow(BodyContainer, i);
                Rectangle painer = (Rectangle)pain.ToList()[4];
                painer.StrokeThickness = 0;
            }
        }
    }
}
