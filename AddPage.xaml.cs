using InputSimulatorStandard;
using InputSimulatorStandard.Native;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        Brush rowColor;
        Brush rowStrokeColor;
        bool[] KeyFocused;
        public string[] remap;
        public string keyMap1;
        public string keyMap2;
        public int key1, key2;
        const UInt32 WM_KEYDOWN = 0x0100;

        public static AddPage addPageInstance;
        public AddPage()
        {
            
            string[] SelectableKeys = { "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh" , "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruhhhhhh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh" };
            string[] SelectableControlKeys = { "CTRL", "ALT", "SHIFT", "LWIN"};
            InitializeComponent();
            addPageInstance = this;
            Key1DropDown.ItemsSource = SelectableControlKeys;
            Key2DropDown.ItemsSource = SelectableControlKeys;
            Key4DropDown.ItemsSource = SelectableControlKeys;
            Key5DropDown.ItemsSource = SelectableControlKeys;
            Key3DropDown.ItemsSource = KeyDictionary.keyDictionary.Values;
            Key6DropDown.ItemsSource = KeyDictionary.keyDictionary.Values;
            KeyFocused = new bool[6];
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
        }
        public void ApplyButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            remap = new string[] { Key1Text.Text, Key2Text.Text, Key3Text.Text, Key4Text.Text, Key5Text.Text, Key6Text.Text };
            for (int i = 0; i < remap.Length; i++)
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
            rowColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31");
            rowStrokeColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF34373B");
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
                Margin = new Thickness(5, 8, 5, 8)
            };
            Grid.SetRow(rowBody, MainPage.mainPageInstance.rows);
            Grid.SetColumn(rowBody, 1);
            Grid.SetColumnSpan(rowBody, 3);
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
            MainPage.mainPageInstance.BodyContainer.RowDefinitions.Add(gridRow);

            MainPage.mainPageInstance.BodyContainer.Children.Add(rowBody);
            MainPage.mainPageInstance.BodyContainer.Children.Add(keyLabel);
            MainPage.mainPageInstance.BodyContainer.Children.Add(keyLabel2);

            //Fill="#FF292C31" Height="44" Canvas.Left="72" RadiusY="10" RadiusX="10" Stroke="#FF34373B" Canvas.Top="25" Width="494"/>
            MainPage.mainPageInstance.rows += 1;
            switch(Key1Text.Text)
            {
                case "ALT": 
                    key1 = 1;
                    break;
                case "CTRL":
                    key1 = 2;
                    break;
                case "SHIFT":
                    key1 = 4;
                    break;
                case "LWIN":
                    key1 = 8;
                    break;
            }
            switch (Key2Text.Text)
            {
                case "ALT":
                    key2 = 1;
                    break;
                case "CTRL":
                    key2 = 2;
                    break;
                case "SHIFT":
                    key2 = 4;
                    break;
                case "LWIN":
                    key2 = 8;
                    break;
            }
            Console.WriteLine(key1);
            MainPage.mainPageInstance.keyboardHookManager.RegisterHotkey((KeyboardHookLibrary.ModifierKeys)key1,KeyDictionary.keyReversed[Key3Text.Text], () =>
            {
                Console.WriteLine("NEW detected");
                this.Dispatcher.Invoke(() =>
                {
                    MainPage.mainPageInstance.keySimulator.Keyboard.ModifiedKeyStroke(
                            new[] { (VirtualKeyCode)KeyDictionary.keyReversed[Key4Text.Text], (VirtualKeyCode)KeyDictionary.keyReversed[Key5Text.Text], },
                            new[] { (VirtualKeyCode)KeyDictionary.keyReversed[Key6Text.Text]});
                    //MainPage.mainPageInstance.keySimulator.Keyboard.KeyPress((VirtualKeyCode)KeyDictionary.keyDictionary[Key6Text.Text]);
                    //Win32.PostMessage(MainPage.mainPageInstance.currentWindow, WM_KEYDOWN, KeyDictionary.keyDictionary[Key6Text.Text], 0);
                });
            }, true);
            this.NavigationService.GoBack();
        }

        //Can't get KeyDown so I have to dirty my hands
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
                    if (KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "CTRL" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "L_CTRL" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "R_CTRL" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "SHIFT" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "L_SHIFT" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "R_SHIFT")
                    {
                        Key1Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)];
                    }
                }
                else
                {
                    if (KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)] == "ALT" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "L_ALT" || KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.Key)] == "R_ALT")
                    {
                        Key1Text.Text = KeyDictionary.keyDictionary[KeyInterop.VirtualKeyFromKey(e.SystemKey)];
                    }
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
        private void AddWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AddWindow.Focus();
        }

    }
}
