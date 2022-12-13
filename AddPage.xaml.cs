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
        const UInt32 WM_KEYDOWN = 0x0100;
        public Dictionary<string, int> keyDictionary;

        public static AddPage addPageInstance;
        public AddPage()
        {
            
            string[] SelectableKeys = { "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh" , "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruhhhhhh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh" };
            string[] SelectableControlKeys = { "Ctrl, Alt, Shift, Win"};
            keyDictionary = new Dictionary<string, int>(){
                   {"A",65},
                   {"Add",107},
                   {"Alt",262144},
                   {"Apps",93},
                   {"Attn",246},
                   {"B",66},
                   {"Back",8},
                   {"BrowserBack",166},
                   {"BrowserFavorites",171},
                   {"BrowserForward",167},
                   {"BrowserHome",172},
                   {"BrowserRefresh",168},
                   {"BrowserSearch",170},
                   {"BrowserStop",169},
                   {"C",67},
                   {"Cancel",3},
                   {"Capital",20},
                   {"Clear",12},
                   {"Control",131072},
                   {"ControlKey",17},
                   {"Crsel",247},
                   {"D",68},
                   {"D0",48},
                   {"D1",49},
                   {"D2",50},
                   {"D3",51},
                   {"D4",52},
                   {"D5",53},
                   {"D6",54},
                   {"D7",55},
                   {"D8",56},
                   {"D9",57},
                   {"Decimal",110},
                   {"Delete",46},
                   {"Divide",111},
                   {"Down",40},
                   {"E",69},
                   {"End",35},
                   {"EraseEof",249},
                   {"Escape",27},
                   {"Execute",43},
                   {"Exsel",248},
                   {"F",70},
                   {"F1",112},
                   {"F10",121},
                   {"F11",122},
                   {"F12",123},
                   {"F13",124},
                   {"F14",125},
                   {"F15",126},
                   {"F16",127},
                   {"F17",128},
                   {"F18",129},
                   {"F19",130},
                   {"F2",113},
                   {"F20",131},
                   {"F21",132},
                   {"F22",133},
                   {"F23",134},
                   {"F24",135},
                   {"F3",114},
                   {"F4",115},
                   {"F5",116},
                   {"F6",117},
                   {"F7",118},
                   {"F8",119},
                   {"F9",120},
                   {"FinalMode",24},
                   {"G",71},
                   {"H",72},
                   {"HangulMode",21},
                   {"Help",47},
                   {"Home",36},
                   {"I",73},
                   {"IMEAccept",30},
                   {"IMEConvert",28},
                   {"IMEModeChange",31},
                   {"IMENonconvert",29},
                   {"Insert",45},
                   {"J",74},
                   {"JunjaMode",23},
                   {"K",75},
                   {"KanjiMode",25},
                   {"KeyCode",65535},
                   {"L",76},
                   {"LaunchApplication1",182},
                   {"LaunchApplication2",183},
                   {"LaunchMail",180},
                   {"LButton",1},
                   {"LControlKey",162},
                   {"Left",37},
                   {"LineFeed",10},
                   {"LMenu",164},
                   {"LShiftKey",160},
                   {"LWin",91},
                   {"M",77},
                   {"MButton",4},
                   {"MediaNextTrack",176},
                   {"MediaPlayPause",179},
                   {"MediaPreviousTrack",177},
                   {"MediaStop",178},
                   {"Menu",18},
                   {"Modifiers" , -65536},
                   {"Multiply",106},
                   {"N",78},
                   {"Next",34},
                   {"NoName",252},
                   {"None",0},
                   {"NumLock",144},
                   {"NumPad0",96},
                   {"NumPad1",97},
                   {"NumPad2",98},
                   {"NumPad3",99},
                   {"NumPad4",100},
                   {"NumPad5",101},
                   {"NumPad6",102},
                   {"NumPad7",103},
                   {"NumPad8",104},
                   {"NumPad9",105},
                   {"O",79},
                   {"Oem1",186},
                   {"Oem102",226},
                   {"Oem2",191},
                   {"Oem3",192},
                   {"Oem4",219},
                   {"Oem5",220},
                   {"Oem6",221},
                   {"Oem7",222},
                   {"Oem8",223},
                   {"P",80},
                   {"Pa1",253},
                   {"Packet",231},
                   {"Pause",19},
                   {"Play",250},
                   {"Print",42},
                   {"Prior",33},
                   {"ProcessKey",229},
                   {"Q",81},
                   {"R",82},
                   {"RButton",2},
                   {"RControlKey",163},
                   {"Return",13},
                   {"Right",39},
                   {"RMenu",165},
                   {"RShiftKey",161},
                   {"RWin",92},
                   {"S",83},
                   {"Scroll",145},
                   {"Select",41},
                   {"SelectMedia",181},
                   {"Separator",108},
                   {"Shift",65536},
                   {"ShiftKey",16},
                   {"Sleep",95},
                   {"Snapshot",44},
                   {"Space",32},
                   {"Subtract",109},
                   {"T",84},
                   {"Tab",9},
                   {"U",85},
                   {"Up",38},
                   {"V",86},
                   {"VolumeDown",174},
                   {"VolumeMute",173},
                   {"VolumeUp",175},
                   {"W",87},
                   {"X",88},
                   {"XButton1",5},
                   {"XButton2",6},
                   {"Y",89},
                   {"Z",90},
                   {"Zoom",251},
            };
            InitializeComponent();
            addPageInstance = this;
            Key1DropDown.ItemsSource = SelectableControlKeys;
            Key3DropDown.ItemsSource = keyDictionary.Keys;
            Key6DropDown.ItemsSource = SelectableKeys;
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
            MainPage.mainPageInstance.keyboardHookManager.RegisterHotkey(keyDictionary[Key3Text.Text], () =>
            {
                Console.WriteLine("NEW detected");
                this.Dispatcher.Invoke(() =>
                {
                    Win32.PostMessage(MainPage.mainPageInstance.currentWindow, WM_KEYDOWN, keyDictionary[Key6Text.Text], 0);
                });
            }, true);
            this.NavigationService.GoBack();
        }

        //Can't get KeyDown so I have to dirty my hands
        private void Key1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
            KeyFocused[0] = true;
            Console.WriteLine("CLICKED");
        }
        private void Key2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
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
            for (int i = 0; i < KeyFocused.Length; i++)
                KeyFocused[i] = false;
            KeyFocused[3] = true;
            Console.WriteLine("CLICKED");
        }

        private void Key5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
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
        private void Key3DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(Key3DropDown.SelectedItem);
            Key3Text.Text = Key3DropDown.SelectedItem.ToString();
            Key3DropDown.Visibility = Visibility.Hidden;
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyFocused[2])
            {
                Key3Text.Text = e.Key.ToString();
                Console.WriteLine(KeyInterop.VirtualKeyFromKey(e.Key));

                KeyFocused[2] = false;
                Key3DropDown.Visibility = Visibility.Hidden;
            }
            if (KeyFocused[5])
            {
                Key6Text.Text = e.Key.ToString();
                KeyFocused[2] = false;
                Key6DropDown.Visibility = Visibility.Hidden;
            }
        }
        private void AddWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AddWindow.Focus();
        }

        private void Key6DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Key6Text.Text = Key6DropDown.SelectedItem.ToString();
            Key6DropDown.Visibility = Visibility.Hidden;
        }
    }
}
