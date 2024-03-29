﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KeyRemap
{
    public partial class AddPage : Page
    {
        private bool[] KeyFocused;
        public List<String> remap;
        public int key1, key2;
        public static AddPage addPageInstance;
        public Guid hotkeyID;
        public int delay;
        public List<IntPtr> processHWNDList;
        public List<String> processNameList;
        public List<ImageBrush> processIconList;
        public List<System.Drawing.Bitmap> processIcon;
        public Bind bind;
        public string windowToActivate;
        public List<ListBox> KeyDropDownList;
        public List<TextBlock> keyTextList;
        public AddPage()
        {
            InitializeComponent();
            bind = new Bind();
            bind.BinderKey = false;
            DataContext = bind;
            processHWNDList = new List<IntPtr>();
            processNameList = new List<String>();
            processIconList = new List<ImageBrush>();
            processIcon = new List<System.Drawing.Bitmap>();
            windowToActivate = "*EVERYWHERE*";
            var icon = new System.Drawing.Icon("./image/Globe.ico");
            var globe = icon.ToBitmap();
            processNameList.Add("*EVERYWHERE*");
            processIconList.Add(new ImageBrush(Util.BitmapToBitmapSource(globe)));
            processIcon.Add(globe);
            foreach (KeyValuePair<IntPtr, string> window in Win32.GetOpenWindows())
            {
                IntPtr handle = window.Key;
                string title = window.Value;

                IntPtr procId;
                Win32.GetWindowThreadProcessId(handle, out procId);
                try
                {
                    var p = Process.GetProcessById((int)procId).MainModule.FileVersionInfo.FileDescription;
                    if (p == null && !processNameList.Contains(title))
                    {
                        processNameList.Add(title);
                    }
                    else
                    {
                        if (!processNameList.Contains(p))
                        {
                            processNameList.Add(p);
                        }
                    }
                    using (System.Drawing.Icon ico = System.Drawing.Icon.ExtractAssociatedIcon(Process.GetProcessById((int)procId).MainModule.FileName))
                    {
                        processIconList.Add(new ImageBrush(Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));
                        processIcon.Add(Util.BitmapFromSource(Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));
                    }
                }
                catch
                {
                    Console.WriteLine("Couldn't retrieve window: System Window");
                }
            }
            delay = 200;
            string[] SelectableControlKeys = { "CTRL", "ALT", "SHIFT", "WIN" };
            addPageInstance = this;
            WindowDropDown.ItemsSource = processNameList;
            Key1DropDown.ItemsSource = SelectableControlKeys;
            Key2DropDown.ItemsSource = SelectableControlKeys;
            Key4DropDown.ItemsSource = SelectableControlKeys;
            Key5DropDown.ItemsSource = SelectableControlKeys;
            Key3DropDown.ItemsSource = KeyDictionary.dropDownList;
            Key6DropDown.ItemsSource = KeyDictionary.dropDownList;
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
            KeyDropDownList = new List<ListBox> { Key1DropDown, Key2DropDown, Key3DropDown, Key4DropDown, Key5DropDown, Key6DropDown, WindowDropDown };
            keyTextList = new List<TextBlock> { Key1Text, Key2Text, Key3Text, Key4Text, Key5Text, Key6Text };
        }

        public void ApplyButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Builds the Key remap text used for Homepage
            remap = new List<string> { Key1Text.Text, Key2Text.Text, Key3Text.Text, Key4Text.Text, Key5Text.Text, Key6Text.Text };
            string keyMap1 = "";
            string keyMap2 = "";
            bool hotkeyAlreadyRegistered = false;
            bool hotkeyEmpty = false;
            for (int i = 0; i < remap.Count; i++)
            {
                if (i < 3)
                {
                    if (remap[i] != " ")
                    {
                        keyMap1 += remap[i];
                        if (i != 2)
                        {
                            keyMap1 += " + ";
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
                            keyMap2 += " + ";
                        }
                    }
                }
            }
            //Prevents same hotkey with same activation window from getting registered
            if (Key3Text.Text == " " || Key6Text.Text == " ")
            {
                hotkeyEmpty = true;
            }
            if (MainPage.mainPageInstance.realHotkeyList.Contains(keyMap1))
            {
                if (MainPage.mainPageInstance.editPageOpen && (MainPage.mainPageInstance.hotkeyWindowList[MainPage.mainPageInstance.realHotkeyList.IndexOf(keyMap1)] != WindowName.Text))
                {
                    hotkeyAlreadyRegistered = false;
                }
                else if (MainPage.mainPageInstance.hotkeyWindowList[MainPage.mainPageInstance.realHotkeyList.IndexOf(keyMap1)] == WindowName.Text)
                {
                    hotkeyAlreadyRegistered = true;
                }
                else
                {
                    hotkeyAlreadyRegistered = false;
                }
            }
            else
            {
                hotkeyAlreadyRegistered = false;
            }
            if (hotkeyEmpty)
            {
                DispatcherTimer t = new DispatcherTimer();
                //Set the timer interval to the length of the animation.
                t.Interval = new TimeSpan(0, 0, 2);
                t.Tick += (EventHandler)delegate (object snd, EventArgs ea)
                {
                    // The animation will be over now, collapse the label.
                    ErrorMessage.IsHitTestVisible = false;
                    // Get rid of the timer.
                    ((DispatcherTimer)snd).Stop();
                };
                ErrorMessage.Text = "Key empty";
                ErrorMessage.IsHitTestVisible = true;
                t.Start();
            }
            else if (hotkeyAlreadyRegistered)
            {
                DispatcherTimer t = new DispatcherTimer();
                //Set the timer interval to the length of the animation.
                t.Interval = new TimeSpan(0, 0, 2);
                t.Tick += (EventHandler)delegate (object snd, EventArgs ea)
                {
                    // The animation will be over now, collapse the label.
                    ErrorMessage.IsHitTestVisible = false;
                    // Get rid of the timer.
                    ((DispatcherTimer)snd).Stop();
                };
                ErrorMessage.Text = "Key already registered";
                ErrorMessage.IsHitTestVisible = true;
                t.Start();
            }
            else
            {
                if (MainPage.mainPageInstance.editPageOpen)
                {
                    var k = MainPage.mainPageInstance.keyMapList[MainPage.mainPageInstance.selectedRowIndex];
                    try
                    {
                        k.Unregister();
                    }
                    catch
                    {
                        Console.WriteLine("Unregister failed in Edit");
                    }
                    k.key1 = Key1Text.Text;
                    k.key2 = Key2Text.Text;
                    k.key3 = Key3Text.Text;
                    k.key4 = Key4Text.Text;
                    k.key5 = Key5Text.Text;
                    k.key6 = Key6Text.Text;
                    k.keyMap1 = keyMap1;
                    k.keyMap2 = keyMap2;
                    k.window = WindowName.Text;
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
                    k.icon = base64String;
                    //MainPage.mainPageInstance.keyboardHookManager.UnregisterHotkey(MainPage.mainPageInstance.hotkeyIDList[MainPage.mainPageInstance.selectedRowIndex]);
                    var pain = Util.ChildrenInRow(MainPage.mainPageInstance.BodyContainer, MainPage.mainPageInstance.selectedRowIndex);
                    Rectangle painer = (Rectangle)pain.ToList()[0];
                    Rectangle icon = (Rectangle)pain.ToList()[1];
                    Rectangle painer2 = (Rectangle)pain.ToList()[2];
                    Label label1 = (Label)pain.ToList()[3];
                    Label label2 = (Label)pain.ToList()[5];
                    painer2.StrokeThickness = 1;
                    painer2.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31");
                    painer.Fill = Brushes.Transparent;
                    icon.Fill = WindowIcon.Fill;
                    label1.Content = keyMap1;
                    label2.Content = keyMap2;
                    k.Register();
                    MainPage.mainPageInstance.hotkeyList[MainPage.mainPageInstance.selectedRowIndex] = remap;
                    MainPage.mainPageInstance.hotkeyWindowList[MainPage.mainPageInstance.selectedRowIndex] = WindowName.Text;
                    MainPage.mainPageInstance.hotkeyIconList[MainPage.mainPageInstance.selectedRowIndex] = WindowIcon.Fill;
                    MainPage.mainPageInstance.realHotkeyList[MainPage.mainPageInstance.selectedRowIndex] = keyMap1;
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
                    Row row = new Row(keyMap1, keyMap2, WindowIcon.Fill);
                    row.Create();
                    Keymap keymap = new Keymap(Key1Text.Text, Key2Text.Text, Key3Text.Text, Key4Text.Text, Key5Text.Text, Key6Text.Text, keyMap1, keyMap2, WindowName.Text, base64String);
                    keymap.Register();
                    MainPage.mainPageInstance.realHotkeyList.Add(keyMap1);
                    MainPage.mainPageInstance.keyMapList.Add(keymap);
                    MainPage.mainPageInstance.hotkeyList.Add(remap);
                    MainPage.mainPageInstance.hotkeyWindowList.Add(WindowName.Text);
                    MainPage.mainPageInstance.hotkeyIconList.Add(WindowIcon.Fill);
                }
                MainPage.mainPageInstance.editPageOpen = false;

                Save save = new Save(MainPage.mainPageInstance.delay, MainPage.mainPageInstance.keyMapList);
                var keyMapJson = JsonConvert.SerializeObject(save, Formatting.Indented);
                var path = Environment.CurrentDirectory;
                string filePath = System.IO.Path.Combine(path, "SaveFile.json");
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write(keyMapJson);
                }
                MainPage.mainPageInstance.selectedRowIndex = -1;
                this.NavigationService.GoBack();
            }
        }

        private void Key1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Key1DropDown.Visibility == Visibility.Visible)
            {
                Key1DropDown.Visibility = Visibility.Hidden;
            }
            else
            {
                Key1DropDown.Visibility = Visibility.Visible;
            }
            for (int i = 0; i < KeyFocused.Length; i++)
            {
                KeyFocused[i] = false;
                if (i != 0)
                {
                    KeyDropDownList[i].Visibility = Visibility.Hidden;
                }
            }
            KeyDropDownList.Last().Visibility = Visibility.Hidden;
            KeyFocused[0] = true;
        }

        private void Key2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Key2DropDown.Visibility == Visibility.Visible)
            {
                Key2DropDown.Visibility = Visibility.Hidden;
            }
            else
            {
                Key2DropDown.Visibility = Visibility.Visible;
            }
            for (int i = 0; i < KeyFocused.Length; i++)
            {
                KeyFocused[i] = false;
                if (i != 1)
                {
                    KeyDropDownList[i].Visibility = Visibility.Hidden;
                }
            }
            KeyDropDownList.Last().Visibility = Visibility.Hidden;
            KeyFocused[1] = true;
        }

        private void Key3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Key3DropDown.Visibility == Visibility.Visible)
            {
                Key3DropDown.Visibility = Visibility.Hidden;
            }
            else
            {
                Key3DropDown.Visibility = Visibility.Visible;
            }
            bind.BinderKey = true;
            for (int i = 0; i < KeyFocused.Length; i++)
            {
                KeyFocused[i] = false;
                if (i != 2)
                {
                    KeyDropDownList[i].Visibility = Visibility.Hidden;
                }
            }
            KeyDropDownList.Last().Visibility = Visibility.Hidden;
            KeyFocused[2] = true;
        }

        private void Key4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Key4DropDown.Visibility == Visibility.Visible)
            {
                Key4DropDown.Visibility = Visibility.Hidden;
            }
            else
            {
                Key4DropDown.Visibility = Visibility.Visible;
            }
            for (int i = 0; i < KeyFocused.Length; i++)
            {
                KeyFocused[i] = false;
                if (i != 3)
                {
                    KeyDropDownList[i].Visibility = Visibility.Hidden;
                }
            }
            KeyDropDownList.Last().Visibility = Visibility.Hidden;
            KeyFocused[3] = true;
        }

        private void Key5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Key5DropDown.Visibility == Visibility.Visible)
            {
                Key5DropDown.Visibility = Visibility.Hidden;
            }
            else
            {
                Key5DropDown.Visibility = Visibility.Visible;
            }
            for (int i = 0; i < KeyFocused.Length; i++)
            {
                KeyFocused[i] = false;
                if (i != 4)
                {
                    KeyDropDownList[i].Visibility = Visibility.Hidden;
                }
            }
            KeyDropDownList.Last().Visibility = Visibility.Hidden;
            KeyFocused[4] = true;
        }

        private void Key6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Key6DropDown.Visibility == Visibility.Visible)
            {
                Key6DropDown.Visibility = Visibility.Hidden;
            }
            else
            {
                Key6DropDown.Visibility = Visibility.Visible;
            }
            for (int i = 0; i < KeyFocused.Length; i++)
            {
                KeyFocused[i] = false;
                if (i != 5)
                {
                    KeyDropDownList[i].Visibility = Visibility.Hidden;
                }
            }
            KeyDropDownList.Last().Visibility = Visibility.Hidden;
            KeyFocused[5] = true;
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

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyFocused[0])
            {
                e.Handled = true;
                if (e.SystemKey.ToString() == "None")
                {
                    if (KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "CTRL" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "SHIFT" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "WIN" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "WIN")
                    {
                        Key1Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                    }
                }
                else
                {
                    Key1Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                KeyFocused[1] = false;
                Key1DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[1])
            {
                e.Handled = true;
                if (e.SystemKey.ToString() == "None")
                {
                    if (KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "CTRL" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "SHIFT" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "WIN" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "WIN")
                    {
                        Key2Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                    }
                }
                else
                {
                    Key2Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                KeyFocused[1] = false;
                Key2DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[2])
            {
                e.Handled = true;
                if (e.SystemKey.ToString() == "None")
                {
                    if (KeyDictionary.dropDownList.Contains(KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)]))
                        Key3Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                }
                else
                {
                    Key3Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                KeyFocused[2] = false;
                Key3DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[3])
            {
                e.Handled = true;
                if (e.SystemKey.ToString() == "None")
                {
                    if (KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "CTRL" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "SHIFT" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "WIN" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "WIN")
                    {
                        Key4Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                    }
                }
                else
                {
                    Key4Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                KeyFocused[3] = false;
                Key4DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[4])
            {
                e.Handled = true;
                if (e.SystemKey.ToString() == "None")
                {
                    if (KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "CTRL" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "SHIFT" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "WIN" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "WIN")
                    {
                        Key5Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                    }
                }
                else
                {
                    Key5Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                KeyFocused[4] = false;
                Key5DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[5])
            {
                e.Handled = true;
                if (e.SystemKey.ToString() == "None")
                {
                    if (KeyDictionary.dropDownList.Contains(KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)]))
                        Key6Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                }
                else
                {
                    Key6Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                }
                KeyFocused[5] = false;
                Key6DropDown.Visibility = Visibility.Hidden;
            }
        }

        private void CancelButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(MainPage.mainPageInstance.selectedRowIndex != -1) {
                var pain = Util.ChildrenInRow(MainPage.mainPageInstance.BodyContainer, MainPage.mainPageInstance.selectedRowIndex);
                Rectangle painer = (Rectangle)pain.ToList()[0];
                Rectangle painer2 = (Rectangle)pain.ToList()[2];
                painer2.StrokeThickness = 1;
                painer2.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31");
                painer.Fill = Brushes.Transparent;
                MainPage.mainPageInstance.editPageOpen = false;
            }
            MainPage.mainPageInstance.selectedRowIndex = -1;
            this.NavigationService.GoBack();
        }

        private void WindowList_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < KeyFocused.Length; i++)
            {
                KeyFocused[i] = false;
                KeyDropDownList[i].Visibility = Visibility.Hidden;
            }
            if (WindowDropDown.Visibility == Visibility.Visible)
            {
                WindowDropDown.Visibility = Visibility.Hidden;
            }
            else
            {
                WindowDropDown.Visibility = Visibility.Visible;
            }
        }

        private void WindowDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WindowName.Text = WindowDropDown.SelectedItem.ToString();
            windowToActivate = WindowName.Text;
            WindowIcon.Fill = processIconList[WindowDropDown.SelectedIndex];
            WindowDropDown.Visibility = Visibility.Hidden;
        }

        private void Handle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindowInstance.DragMove();
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            bind.BinderSettingsOpened = false;
            bind.BinderSettingsOpened = true;
        }

        private void AddWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < KeyFocused.Length; i++)
            {
                if (KeyFocused[i])
                {
                    keyTextList[i].Text = " ";
                    KeyDropDownList[i].Visibility = Visibility.Hidden;
                }
                else
                {
                    KeyFocused[i] = false;
                    KeyDropDownList[i].Visibility = Visibility.Hidden;
                }
            }
            WindowDropDown.Visibility = Visibility.Hidden;
        }

        private void AddWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AddWindow.Focus();
        }
    }
}