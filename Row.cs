using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
    public class Row
    {
        public string keyMap1;
        public string keyMap2;
        public Brush icon;
        public Row(string keymap1, string keymap2, Brush icon)
        {
            this.keyMap1 = keymap1;
            this.keyMap2 = keymap2;
            this.icon = icon;
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
        public void Create()
        {
            MainPage.mainPageInstance.realHotkeyList.Add(keyMap1);
            Console.WriteLine("NEW REGISTER");
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
                Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31"),
                StrokeThickness = 1,
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF34373B"),
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
                Fill = icon,
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
            MainPage.mainPageInstance.BodyContainer.RowDefinitions.Add(gridRow);
            MainPage.mainPageInstance.rowList.Add(gridRow);
            MainPage.mainPageInstance.BodyContainer.Children.Add(rowBody);
            MainPage.mainPageInstance.BodyContainer.Children.Add(keyIcon);
            MainPage.mainPageInstance.BodyContainer.Children.Add(keyLabel);
            MainPage.mainPageInstance.BodyContainer.Children.Add(keyLabel2);
            MainPage.mainPageInstance.BodyContainer.Children.Add(rowBackground);
            MainPage.mainPageInstance.rows += 1;
        }
    }
}
