using InputSimulatorStandard.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        Brush rowColor;
        Brush rowStrokeColor;
        bool[] KeyFocused;
        public List<String> remap;
        public int key1, key2;
        public static AddPage addPageInstance;
        public Guid hotkeyID;
        public int delay;
        public List<IntPtr> processHWNDList;
        public List<String> processNameList;
        public List<ImageBrush> processIconList;
        public List<System.Drawing.Bitmap> processIcon;

        public string windowToActivate;
        public AddPage()
        {

            processHWNDList = new List<IntPtr>();
            processNameList = new List<String>();
            processIconList = new List<ImageBrush>();
            processIcon = new List<System.Drawing.Bitmap>();
            windowToActivate = "*Everywhere*";
            var icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("pack://application:,,,/image/Globe.ico")).Stream);
            var globe = icon.ToBitmap();
            processNameList.Add("*Everywhere*");
            processIconList.Add(new ImageBrush(Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));
            processIcon.Add(globe);
            foreach (KeyValuePair<IntPtr, string> window in Win32.GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;

                IntPtr procId;
                Win32.GetWindowThreadProcessId(handle, out procId);
                var p = Process.GetProcessById((int)procId).MainModule.FileVersionInfo.FileDescription;
                if (p == "")
                {
                    processNameList.Add(title);
                    //Console.WriteLine("{0}^", title);
                }
                else
                {
                    processNameList.Add(p);
                    //Console.WriteLine(p);
                }
                using (System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetProcessById((int)procId).MainModule.FileName))
                {
                    processIcon.Add(ico.ToBitmap());
                    processIconList.Add(new ImageBrush(Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));
                }
            }
            //base64String = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAATXSURBVFhHvZZZTxtXFMfnI/ARUAKJ+lJZVQ3GxARVjdoECANhsSG0fqoUVS0uq1jMGIxrMIttwBgwEVFVKU8VlcEYbPCwmS3QIelbXuYjTMBUfTw9dxib8TYqIuRIP92R8Z3ff869cw11G8WUsjmTNRz724v3fNT7z8K+999c6U+fpmylO2x/ySb0aDdgx3MhsvupggyWbr2yPdyCAQzQVbQOO1MYIA4J4rnFINaSLcsgigdLMIDuMsA2ijOBgVj2YwaxlrAWK8rj9OtYDBCG7UkUprAljTuTF5w0/WY1oGNNCD71FcyDCHRqQrA1gUIFpFtQ1IKRy5l//nez18hdqy2MjqXJ06bSV7yBAdZg032hCDXXwKleNr5zzje+E+Yb3gLBZ+BM0v0Vi9GyKouOFSy6CKRixgDtmlXYdKFIAepH1Wve+igMPiKWo+cskidj4bueiyKeebCB7b7Cgq0no1m7Du2Fq8A6Y4pQ392bB0OeFxzl2zBneJuET3/KevXpS0LkTAa5nF5tGNqLAlORcRQpQH1/7yU05fug6f4cjFZGYVZ/eokhAS8PQU45XF++r3gd1zk7vZowQ74fGYuBEhhgHkgXGvNnwfiZD2b0pzxCxgTe+lNhto6jTaWLOT3aEEeezoySLAjmoshDMS3WxmgMlMAl8GEA7ADSkO8F8rQkhLeeQ3Ey9idRrq1gBd/vNTxmQ9BbHE4CP+M7NcGkJdsYQZECGGBODEDGJoRMIiGm6zkBgVRs3+xAm2YFOguD0C0FEcMUhVnSIdEqq3VHDJSgmvJRLEOaR3lqOZWnjhMQ8NT9JYMDV/Ub3GBB3OWkG6ukI05pWlqFh2OgBPUc116ONE8sT+2JcQqlIrXJo6sKQ2hJiNVm6esZKzyEIgUwwAyKr5DmidWiDhj7vorAJEqzMVFzonhohewxUIJqxI0XpyF/OhGgWx1SteNam9RLwHzNElFW3DUnWQ+t0K/nEGctcY1y6ZpqyJuGOIa7HjEA2cm4yYQOTRDacJ2bv/RD/6NNIhKRieXjopPeTzu01mwoVgADeFDuQfkU1N1x4w8IyjVBnsg7sAOkC62FAfgZQ9jLopz72TEQXBlwVx/zqSFWB89BCcqQNwX6vEmUu6A+zw0dhSs82d1JFKwIpgI/7aS5HFf1MedEGYFI49fO6sRnAo605E8PYJWQrqn6uxNQi/Jnd8bhpy/+QGEA2x4QRwnepFlMPBUJgTLeia9iNsaRseo34r4IoiQ4kILsM4rIq3Id8EL1WhQns5zxcLHTXO541TE/jq9iKmPSOFJ5CINle86V/nNQgmq67xN++Px3aC1YTqJFvZz1cCFFQoxhiLGqI5Sm46g8AuuTvTRhKpRJFczF932hpWAJ4rSqA4qHS7wcdFQ1Sh8JCKQy/PQQBh5HIWA5V0S6FSUGMan9xl/U/sQv2f+pUfqQHkFhKkMYoJ8EYFCkgHSbm9VwxYHJgWsux15xAJbHu7Dcdy5xlhHpFjevEfqQIW2PY6/YB+bbXVgynynBS9M/TqGYGXp6AHbEVr4PfSRAL4oy8oFd7BTSTs4bl718/xVpv7VsD8z4v4O/5yyFD6zfHLvWPrt22coPWDEAdsDfjdJLFvxdtyyOF0OzOfbKo4UZ43v+z64zZ/ZWU9R/5o+npwCuo3YAAAAASUVORK5CYII=";
            //Console.WriteLine(base64String);
            delay = 200;
            string[] SelectableKeys = { "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh" , "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruhhhhhh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh" };
            string[] SelectableControlKeys = {"L-CTRL", "L-ALT", "L-SHIFT", "L-WIN", "R-CTRL", "R-ALT", "R-SHIFT", "R-WIN" };
            InitializeComponent();
            addPageInstance = this;
            WindowDropDown.ItemsSource = processNameList;
            Key1DropDown.ItemsSource = SelectableControlKeys;
            Key2DropDown.ItemsSource = SelectableControlKeys;
            Key4DropDown.ItemsSource = SelectableControlKeys;
            Key5DropDown.ItemsSource = SelectableControlKeys;
            Key3DropDown.ItemsSource = KeyDictionary.keyDictionary.Values;
            Key6DropDown.ItemsSource = KeyDictionary.keyDictionary.Values;
            KeyFocused = new bool[6];
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
            if (MainPage.mainPageInstance.editPageOpen)
            {
                windowToActivate = MainPage.mainPageInstance.hotkeyWindowList[MainPage.mainPageInstance.selectedRowIndex];
                WindowName.Text = windowToActivate;
                WindowIcon.Fill = MainPage.mainPageInstance.hotkeyIconList[MainPage.mainPageInstance.selectedRowIndex];
                Key1Text.Text = MainPage.mainPageInstance.hotkeyList[MainPage.mainPageInstance.selectedRowIndex][0];
                Key2Text.Text = MainPage.mainPageInstance.hotkeyList[MainPage.mainPageInstance.selectedRowIndex][1];
                Key3Text.Text = MainPage.mainPageInstance.hotkeyList[MainPage.mainPageInstance.selectedRowIndex][2];
                Key4Text.Text = MainPage.mainPageInstance.hotkeyList[MainPage.mainPageInstance.selectedRowIndex][3];
                Key5Text.Text = MainPage.mainPageInstance.hotkeyList[MainPage.mainPageInstance.selectedRowIndex][4];
                Key6Text.Text = MainPage.mainPageInstance.hotkeyList[MainPage.mainPageInstance.selectedRowIndex][5];
            }
        }
        public void rowBody_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rowBody = Util.FindVisualParent<Rectangle>(sender as Rectangle);
            MainPage.mainPageInstance.selectedRowIndex = (int)rowBody.GetValue(Grid.RowProperty);

            for (int i = 0; i < MainPage.mainPageInstance.rows; i++)
            {
                var pain = Util.ChildrenInRow(MainPage.mainPageInstance.BodyContainer, i);
                Rectangle painer = (Rectangle)pain.ToList()[4];
                painer.StrokeThickness = 0;
            }
            rowBody.StrokeThickness = 1;
        }
        public void ApplyButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Builds the Key remap text used for Homepage
            remap = new List<string> { Key1Text.Text, Key2Text.Text, Key3Text.Text, Key4Text.Text, Key5Text.Text, Key6Text.Text };
            string keyMap1 = "";
            string keyMap2 = "";
            bool hotkeyAlreadyRegistered = false;
            for (int i = 0; i < remap.Count; i++)
            {
                if (i < 3) {
                    if (remap[i] != " ")
                    {
                        keyMap1 += remap[i];
                        if (i != 2)
                        {
                            keyMap1 += "+";
                        }
                    }
                }
                else
                {
                    if (remap[i] != " ")
                    {
                        keyMap2 += remap[i];
                        if (i != 5)
                        {
                            keyMap2 += "+";
                        }
                    }
                }
            }
            //Prevents same hotkey with same activation window from getting registered
            if (MainPage.mainPageInstance.realHotkeyList.Contains(keyMap1))
            {
                if (MainPage.mainPageInstance.hotkeyWindowList[MainPage.mainPageInstance.realHotkeyList.IndexOf(keyMap1)] == WindowName.Text)
                {
                    hotkeyAlreadyRegistered = true;
                }
                else
                {
                    hotkeyAlreadyRegistered = false;
                }
            }
            else {
                hotkeyAlreadyRegistered = false; ;
            }
            if (hotkeyAlreadyRegistered)
            {
                Console.WriteLine("ALREADY REGISTERED");
            }
            else
            {
                if (MainPage.mainPageInstance.editPageOpen)
                {
                    Console.WriteLine("EDITING");
                    foreach (Keymap k in MainPage.mainPageInstance.keyMapList)
                    {
                        Console.WriteLine(MainPage.mainPageInstance.selectedRowIndex);

                        if (k.row - 1 == MainPage.mainPageInstance.selectedRowIndex)
                        {
                            try
                            {
                                k.Unregister();
                            }
                            catch
                            {
                                k.key1 = Key1Text.Text;
                                k.key2 = Key2Text.Text;
                                k.key3 = Key3Text.Text;
                                k.key4 = Key4Text.Text;
                                k.key5 = Key5Text.Text;
                                k.key6 = Key6Text.Text;
                                k.keyMap1 = keyMap1;
                                k.keyMap2 = keyMap2;
                                k.window = WindowName.Text;
                                Console.WriteLine("CHANGED WINDOW: {0}", k.window);
                                k.Register();
                            }
                        }
                    }
                    //MainPage.mainPageInstance.keyboardHookManager.UnregisterHotkey(MainPage.mainPageInstance.hotkeyIDList[MainPage.mainPageInstance.selectedRowIndex]);
                    var pain = Util.ChildrenInRow(MainPage.mainPageInstance.BodyContainer, MainPage.mainPageInstance.selectedRowIndex);
                    Rectangle icon = (Rectangle)pain.ToList()[1];
                    Label label1 = (Label)pain.ToList()[2];
                    Label label2 = (Label)pain.ToList()[3];
                    icon.Fill = WindowIcon.Fill;
                    label1.Content = keyMap1;
                    label2.Content = keyMap2;
                    MainPage.mainPageInstance.hotkeyList[MainPage.mainPageInstance.selectedRowIndex] = remap;
                    MainPage.mainPageInstance.hotkeyWindowList[MainPage.mainPageInstance.selectedRowIndex] = WindowName.Text;
                    MainPage.mainPageInstance.hotkeyIconList[MainPage.mainPageInstance.selectedRowIndex] = WindowIcon.Fill;
                }
                else
                {
                    byte[] bytes;
                    using (var ms = new MemoryStream())
                    {
                        if (WindowDropDown.SelectedIndex == -1)
                        {
                            processIcon[0].Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        }
                        else
                        {
                            processIcon[WindowDropDown.SelectedIndex].Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        }
                        bytes = ms.ToArray();
                    }
                    var base64String = Convert.ToBase64String(bytes);
                    Row row = new Row(keyMap1,keyMap2, WindowIcon.Fill);
                    row.Create();
                    Keymap keymap = new Keymap(Key1Text.Text, Key2Text.Text, Key3Text.Text, Key4Text.Text, Key5Text.Text, Key6Text.Text, keyMap1, keyMap2, WindowName.Text, base64String, MainPage.mainPageInstance.rows);
                    keymap.Register();
                    MainPage.mainPageInstance.realHotkeyList.Add(keyMap1);
                    MainPage.mainPageInstance.keyMapList.Add(keymap);
                    MainPage.mainPageInstance.hotkeyIDList.Add(hotkeyID);
                    MainPage.mainPageInstance.hotkeyList.Add(remap);
                    MainPage.mainPageInstance.hotkeyWindowList.Add(WindowName.Text);
                    MainPage.mainPageInstance.hotkeyIconList.Add(WindowIcon.Fill);
                }

                Console.WriteLine("{0} HOTKEY ID", hotkeyID);
                MainPage.mainPageInstance.editPageOpen = false;
                var keyMapJson = JsonConvert.SerializeObject(MainPage.mainPageInstance.keyMapList,Formatting.Indented);
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string filePath = System.IO.Path.Combine(path, "SaveFile.json");
                using(StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(keyMapJson);
                }
                this.NavigationService.GoBack();
            }
        }

        private void Key1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Key1DropDown.Visibility = Visibility.Visible;
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
            KeyFocused[0] = true;
            Console.WriteLine("CLICKED");
        }
        private void Key2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Key2DropDown.Visibility = Visibility.Visible;
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
            KeyFocused[1] = true;
            Console.WriteLine("CLICKED");
        }
        private void Key3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(MainPage.mainPageInstance.pressedKey);
            Key3DropDown.Visibility = Visibility.Visible;
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
            KeyFocused[2] = true;
            Console.WriteLine("CLICKED");
        }
        private void Key4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Key4DropDown.Visibility = Visibility.Visible;
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
            KeyFocused[3] = true;
            Console.WriteLine("CLICKED");
        }

        private void Key5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Key5DropDown.Visibility = Visibility.Visible;
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
            KeyFocused[4] = true;
            Console.WriteLine("CLICKED");
        }

        private void Key6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Key6DropDown.Visibility = Visibility.Visible;
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
            KeyFocused[5] = true;
            Console.WriteLine("CLICKED");
        }

        private void Key1DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Key1Text.Text = Key1DropDown.SelectedItem.ToString();
            Key1DropDown.Visibility = Visibility.Hidden;
        }
        private void Key2DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Key2Text.Text = Key2DropDown.SelectedItem.ToString();
            Key2DropDown.Visibility = Visibility.Hidden;
        }
        private void Key3DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(Key3DropDown.SelectedItem);
            Key3Text.Text = Key3DropDown.SelectedItem.ToString();
            Key3DropDown.Visibility = Visibility.Hidden;
        }
        private void Key4DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Key4Text.Text = Key4DropDown.SelectedItem.ToString();
            Key4DropDown.Visibility = Visibility.Hidden;
        }

        private void Key5DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Key5Text.Text = Key5DropDown.SelectedItem.ToString();
            Key5DropDown.Visibility = Visibility.Hidden;
        }
        private void Key6DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Key6Text.Text = Key6DropDown.SelectedItem.ToString();
            Key6DropDown.Visibility = Visibility.Hidden;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e) {
            if (KeyFocused[0])
            {
                if (e.SystemKey.ToString() == "None")
                {
                    if ( KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "L-CTRL" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "R-CTRL" ||
                        KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "L-SHIFT" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "R-SHIFT" ||
                        KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "L-WIN" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "R-WIN")
                    {
                        Key1Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                    }
                }
                else
                {
                    Key1Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
            Console.WriteLine(KeyInterop.VirtualKeyFromKey(e.Key));
            KeyFocused[1] = false;
            Key1DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[1])
            {
                if (e.SystemKey.ToString() == "None")
                {
                    Key2Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                }
                else
                {
                    Key2Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                Console.WriteLine(KeyInterop.VirtualKeyFromKey(e.Key));
                KeyFocused[1] = false;
                Key2DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[2])
            {
                if (e.SystemKey.ToString() == "None")
                {
                    Key3Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                }
                else
                {
                    Key3Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                Console.WriteLine(KeyInterop.VirtualKeyFromKey(e.Key));
                KeyFocused[2] = false;
                Key3DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[3])
            {
                if (e.SystemKey.ToString() == "None")
                {
                    Key4Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                }
                else
                {
                    Key4Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                Console.WriteLine(KeyInterop.VirtualKeyFromKey(e.Key));
                KeyFocused[3] = false;
                Key4DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[4])
            {
                if (e.SystemKey.ToString() == "None")
                {
                    Key5Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                }
                else
                {
                    Key5Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                Console.WriteLine(KeyInterop.VirtualKeyFromKey(e.Key));
                KeyFocused[4] = false;
                Key5DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[5])
            {
                if (e.SystemKey.ToString() == "None")
                {
                    Key6Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                }
                else
                {
                    Key6Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                Console.WriteLine(KeyInterop.VirtualKeyFromKey(e.Key));
                KeyFocused[5] = false;
                Key6DropDown.Visibility = Visibility.Hidden;
            }
                    }

        private void CancelButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainPage.mainPageInstance.editPageOpen = false;
            this.NavigationService.GoBack();
        }

        private void WindowList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowDropDown.Visibility = Visibility.Visible;
        }

        private void WindowDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WindowName.Text = WindowDropDown.SelectedItem.ToString();
            windowToActivate = WindowName.Text;
            WindowIcon.Fill = processIconList[WindowDropDown.SelectedIndex];
            WindowDropDown.Visibility = Visibility.Hidden;
        }

        private void AddWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AddWindow.Focus();
        }

    }
}
