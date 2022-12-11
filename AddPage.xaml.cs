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

        public static AddPage addPageInstance;
        public AddPage()
        {
            string[] SelectableKeys = { "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh" , "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruhhhhhh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh", "Bruh" };
            string[] SelectableControlKeys = { "Ctrl, Alt, Shift, Win"};
            InitializeComponent();
            addPageInstance = this;
            Key1DropDown.ItemsSource = SelectableControlKeys;
            Key3DropDown.ItemsSource = SelectableKeys;
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
