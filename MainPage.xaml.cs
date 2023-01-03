using ImageProcessor;
using ImageProcessor.Imaging.Formats;
using InputSimulatorStandard;
using KeyboardHookLibrary;
using KeyboardUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public List<Keymap> sortedList;
        public List<RowDefinition> rowList;
        public bool ActivateButton;
        public Bind bind;
        public int prevSelectedRowIndex;
        public MainPage()
        {
            InitializeComponent();
            mainPageInstance = this;
            rows = 0;
            prevWindowName = "";
            prevSelectedRowIndex = -1;
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
            sortedList = new List<Keymap>();
            rowList = new List<RowDefinition>();
            bind = new Bind();
            DataContext = bind;
            selectedRowIndex = -1;
            ActivateButton = false;
            CompositionTarget.Rendering += MainEventTimer;
            foreach (KeyValuePair<IntPtr, string> window in Win32.GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;

                IntPtr procId;
                Win32.GetWindowThreadProcessId(handle,out procId);
                try
                {
                    var p = Process.GetProcessById((int)procId).MainModule.FileVersionInfo.FileDescription;
                    if (p == "")
                    {
                        processNameList.Add(title);
                        Console.WriteLine("{0}^", title);
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
                            Console.WriteLine("{0},FAIL", p);
                        }
                    }
                    using (System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetProcessById((int)procId).MainModule.FileName))
                    {

                        processIconList.Add(new ImageBrush(Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));
                    }
                }
                catch
                {
                    Console.WriteLine("SYSTEM FILE");
                }
            }
            hotkeyWindowList = new List<String>();
            keySimulator = new InputSimulator();
            keyboardHookManager = new KeyboardHookManager();
            keyboardHookManager.Start();
            try
            {
                var path = Environment.CurrentDirectory;
                string filePath = System.IO.Path.Combine(path, "SaveFile.json");
                string content = "";
                using (StreamReader sr = new StreamReader(filePath))
                {
                    content = sr.ReadToEnd();
                }
                keyMapList = JsonConvert.DeserializeObject<IEnumerable<Keymap>>(content).ToList();
                foreach (Keymap keymap in keyMapList)
                {
                    Console.WriteLine("DADA {0}", keymap.window);
                    var brush = new ImageBrush(Util.BitmapToBitmapSource(Util.Base64StringToBitmap(keymap.icon)));
                    Row row = new Row(keymap.keyMap1, keymap.keyMap2, brush);
                    row.Create();
                    //keymap.Register();
                    List<string> remap = new List<String> { keymap.key1, keymap.key2, keymap.key3, keymap.key4, keymap.key5, keymap.key6 };
                    hotkeyList.Add(remap);
                    hotkeyWindowList.Add(keymap.window);
                    hotkeyIconList.Add(new ImageBrush(Util.BitmapToBitmapSource(Util.Base64StringToBitmap(keymap.icon))));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                Console.WriteLine(e);
            }
        }
        public void MainEventTimer(object sender, EventArgs e)
        {
            currentWindowName = GetTitle(Win32.GetForegroundWindow());
            //Console.WriteLine(BodyContainer.RowDefinitions.Count);
            //Console.WriteLine(editPageOpen);
            // Running same hotkeys that has different activation windows
            // Works when activation windows are specified but when adding *Everywhere* activation, it overrides the others
            // Sorted the list to make activations with *Everywhere* to be first so it gets overrided by specified window hotkeys
            if (currentWindowName != prevWindowName)
            {
                sortedList = keyMapList.OrderBy(o => o.window).ToList();
                Console.WriteLine(currentWindowName);
                foreach (Keymap keymap in sortedList)
                {
                    if (currentWindowName.Contains(keymap.window) || keymap.window.Contains("Everywhere") || currentWindowName.Contains("KeyRemap"))
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
            if (selectedRowIndex != prevSelectedRowIndex)
            {
                if (selectedRowIndex != -1)
                {
                    ActivateButtons();
                    Console.WriteLine("ACTIVATE");
                }
                else
                {
                    DeactivateButtons();
                    Console.WriteLine("DEACTIVATE");
                }
                prevSelectedRowIndex = selectedRowIndex;
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
            addPageOpen = true;
            NavigationService.Navigate(new AddPage());
            selectedRowIndex -= 1;
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
        private void EditButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedRowIndex != -1)
            {
                editPageOpen = true;
                NavigationService.Navigate(new AddPage());
                selectedRowIndex -= 1;
            }
        }
        private void DeleteButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedRowIndex != -1)
            {
                Console.WriteLine("DELETE CLICKED");
                BodyContainer.RowDefinitions.RemoveRange(0, BodyContainer.RowDefinitions.Count);
                BodyContainer.Children.RemoveRange(0, BodyContainer.Children.Count);
                keyboardHookManager.UnregisterAll();
                realHotkeyList.Clear();
                keyMapList.RemoveAt(selectedRowIndex);
                rows = 0;
                var keyMapJson = JsonConvert.SerializeObject(MainPage.mainPageInstance.keyMapList, Formatting.Indented);
                var path = Environment.CurrentDirectory;
                string filePath = System.IO.Path.Combine(path, "SaveFile.json");
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(keyMapJson);
                }
                try
                {
                    path = Environment.CurrentDirectory;
                    filePath = System.IO.Path.Combine(path, "SaveFile.json");
                    string content = "";
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        content = sr.ReadToEnd();
                    }
                    keyMapList = JsonConvert.DeserializeObject<IEnumerable<Keymap>>(content).ToList();
                    sortedList = keyMapList.OrderBy(o => o.window).ToList();
                    foreach (Keymap keymap in sortedList)
                    {
                        Console.WriteLine("DADA {0}", keymap.window);
                        var brush = new ImageBrush(Util.BitmapToBitmapSource(Util.Base64StringToBitmap(keymap.icon)));
                        Row row = new Row(keymap.keyMap1, keymap.keyMap2, brush);
                        row.Create();
                        //keymap.Register();
                        List<string> remap = new List<String> { keymap.key1, keymap.key2, keymap.key3, keymap.key4, keymap.key5, keymap.key6 };
                        hotkeyList.Add(remap);
                        hotkeyWindowList.Add(keymap.window);
                        hotkeyIconList.Add(new ImageBrush(Util.BitmapToBitmapSource(Util.Base64StringToBitmap(keymap.icon))));
                    }
                }
                catch
                {
                    Console.WriteLine("DELETE FAIL");
                }
                selectedRowIndex -= 1;
            }
        }

        private void BodyBackground_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < rows; i++)
            {
                var pain = Util.ChildrenInRow(BodyContainer, i);
                Rectangle painer = (Rectangle)pain.ToList()[0];
                Rectangle painer2 = (Rectangle)pain.ToList()[2];
                painer2.StrokeThickness = 1;
                painer2.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31");
                painer.Fill = Brushes.Transparent;
            }
            selectedRowIndex = -1;
        }
        private void ContainerCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < rows; i++)
            {
                var pain = Util.ChildrenInRow(BodyContainer, i);
                Rectangle painer = (Rectangle)pain.ToList()[0];
                Rectangle painer2 = (Rectangle)pain.ToList()[2];
                painer2.StrokeThickness = 1;
                painer2.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31");
                painer.Fill = Brushes.Transparent;
            }
            selectedRowIndex = -1;

        }

        private void EditButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selectedRowIndex != -1)
            {
                EditButton.Cursor = Cursors.Hand;
                bind.BinderEdit = true;
            }
            else
            {
                EditButton.Cursor = Cursors.Arrow;
            }
        }

        private void EditButton_MouseLeave(object sender, MouseEventArgs e)
        {
            bind.BinderEdit = false;
            EditButton.Cursor = Cursors.Arrow;
        }

        private void DeleteButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (selectedRowIndex != -1)
            {
                DeleteButton.Cursor = Cursors.Hand;
                bind.BinderDelete = true;
            }
            else
            {
                DeleteButton.Cursor = Cursors.Arrow;
            }
        }

        private void DeleteButton_MouseLeave(object sender, MouseEventArgs e)
        {
            DeleteButton.Cursor = Cursors.Arrow;
            bind.BinderDelete = false;

        }
        public void ActivateButtons()
        {
            EditButton.Opacity = 1;
            DeleteButton.Opacity = 1;
        }
        public void DeactivateButtons()
        {
            EditButton.Opacity = 0.2;
            DeleteButton.Opacity = 0.2;
        }

        private void Handle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("DRAG");
            MainWindow.mainWindowInstance.DragMove();
        }

        private void SettingsButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedRowIndex = -1;
            NavigationService.Navigate(new SettingsPage());
        }

        private void MainPage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < rows; i++)
            {
                var pain = Util.ChildrenInRow(BodyContainer, i);
                Rectangle painer = (Rectangle)pain.ToList()[0];
                Rectangle painer2 = (Rectangle)pain.ToList()[2];
                //MainPage.mainPageInstance.selectedRowIndex -= 1;
                painer2.StrokeThickness = 1;
                painer2.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31");
                painer.Fill = Brushes.Transparent;
            }
            selectedRowIndex = -1;
        }
    }
}
