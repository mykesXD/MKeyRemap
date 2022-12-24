using InputSimulatorStandard.Native;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        public string windowToActivate;
        public AddPage()
        {

            processHWNDList = new List<IntPtr>();
            processNameList = new List<String>();
            processIconList = new List<ImageBrush>();
            windowToActivate = "*Everywhere*";
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
                    Console.WriteLine("{0}^", title);
                }
                else
                {
                    processNameList.Add(p);
                    Console.WriteLine(p);
                }
                using (System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetProcessById((int)procId).MainModule.FileName))
                {
                    processIconList.Add(new ImageBrush(Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));
                }
            }
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
            remap = new List<String> { Key1Text.Text, Key2Text.Text, Key3Text.Text, Key4Text.Text, Key5Text.Text, Key6Text.Text };
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
                MainPage.mainPageInstance.realHotkeyList.Add(keyMap1);
                Console.WriteLine("NEW REGISTER");
                rowColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31");
                rowStrokeColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF34373B");
                Rectangle rowBackground = new Rectangle
                {
                    Name = string.Format("RemapBackground{0}", MainPage.mainPageInstance.rows),
                    Width = 588,
                    Height = 56,
                    Fill = Brushes.Transparent,
                    StrokeThickness = 0,
                    Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#0094FF"),
                    RadiusX = 10,
                    RadiusY = 10,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 2, 2, 2),
                };

                rowBackground.MouseLeftButtonDown += rowBody_MouseLeftButtonDown;

                Grid.SetRow(rowBackground, MainPage.mainPageInstance.rows);
                Grid.SetColumn(rowBackground, 0);
                Grid.SetColumnSpan(rowBackground, 4);
                Rectangle rowBody = new Rectangle
                {
                    Name = string.Format("RemapRow{0}", MainPage.mainPageInstance.rows),
                    Width = 494,
                    Height = 44,
                    Fill = rowColor,
                    StrokeThickness = 1,
                    Stroke = rowStrokeColor,
                    RadiusX = 10,
                    RadiusY = 10,
                    Margin = new Thickness(5, 8, 5, 8),
                };
                Grid.SetRow(rowBody, MainPage.mainPageInstance.rows);
                Grid.SetColumn(rowBody, 1);
                Grid.SetColumnSpan(rowBody, 3);
                Rectangle keyIcon = new Rectangle
                {
                    Name = "keyIcon",
                    Width = 30,
                    Height = 30,
                    Fill = WindowIcon.Fill,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Grid.SetRow(keyIcon, MainPage.mainPageInstance.rows);
                Grid.SetColumn(keyIcon, 0);
                Label keyLabel = new Label
                {
                    Name = "keyLabel",
                    Content = keyMap1,
                    FontSize = 20,
                    Foreground = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Grid.SetRow(keyLabel, MainPage.mainPageInstance.rows);
                Grid.SetColumn(keyLabel, 1);

                Label keyLabel2 = new Label
                {
                    Name = "keyLabel2",
                    Content = keyMap2,
                    FontSize = 20,
                    Foreground = new SolidColorBrush(Colors.White),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Grid.SetRow(keyLabel2, MainPage.mainPageInstance.rows);
                Grid.SetColumn(keyLabel2, 3);

                RowDefinition gridRow = new RowDefinition();
                gridRow.Height = new GridLength(60);
                if (MainPage.mainPageInstance.editPageOpen)
                {
                    foreach (Keymap k in MainPage.mainPageInstance.keyMapList)
                    {
                        if (k.row == MainPage.mainPageInstance.selectedRowIndex)
                        {
                            k.Unregister();
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
                }
                else
                {
                    MainPage.mainPageInstance.BodyContainer.RowDefinitions.Add(gridRow);
                    MainPage.mainPageInstance.BodyContainer.Children.Add(rowBody);
                    MainPage.mainPageInstance.BodyContainer.Children.Add(keyIcon);
                    MainPage.mainPageInstance.BodyContainer.Children.Add(keyLabel);
                    MainPage.mainPageInstance.BodyContainer.Children.Add(keyLabel2);
                    MainPage.mainPageInstance.BodyContainer.Children.Add(rowBackground);
                    MainPage.mainPageInstance.rows += 1;
                }
                //Fill="#FF292C31" Height="44" Canvas.Left="72" RadiusY="10" RadiusX="10" Stroke="#FF34373B" Canvas.Top="25" Width="494"/>
                Keymap keyremap = new Keymap(Key1Text.Text, Key2Text.Text, Key3Text.Text, Key4Text.Text, Key5Text.Text, Key6Text.Text, WindowName.Text,MainPage.mainPageInstance.rows);
                keyremap.Register();
                MainPage.mainPageInstance.keyMapList.Add(keyremap);
                if (MainPage.mainPageInstance.editPageOpen)
                {
                    MainPage.mainPageInstance.hotkeyIDList[MainPage.mainPageInstance.selectedRowIndex] = hotkeyID;
                    MainPage.mainPageInstance.hotkeyList[MainPage.mainPageInstance.selectedRowIndex] = remap;
                    MainPage.mainPageInstance.hotkeyWindowList[MainPage.mainPageInstance.selectedRowIndex] = WindowName.Text;
                    MainPage.mainPageInstance.hotkeyIconList[MainPage.mainPageInstance.selectedRowIndex] = WindowIcon.Fill;
                }
                else
                {
                    MainPage.mainPageInstance.hotkeyIDList.Add(hotkeyID);
                    MainPage.mainPageInstance.hotkeyList.Add(remap);
                    MainPage.mainPageInstance.hotkeyWindowList.Add(WindowName.Text);
                    MainPage.mainPageInstance.hotkeyIconList.Add(WindowIcon.Fill);
                }
                Console.WriteLine("{0} HOTKEY ID", hotkeyID);
                MainPage.mainPageInstance.editPageOpen = false;
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
