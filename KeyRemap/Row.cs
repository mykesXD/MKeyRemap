using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
            bool allRemoved = true;
            for (int i = 0; i < MainPage.mainPageInstance.rows; i++)
            {
                var pain = Util.ChildrenInRow(MainPage.mainPageInstance.BodyContainer, i);
                Rectangle painer = (Rectangle)pain.ToList()[0];
                Rectangle painer2 = (Rectangle)pain.ToList()[2];
                if (i == MainPage.mainPageInstance.selectedRowIndex && painer2.StrokeThickness == 1)
                {
                    painer2.StrokeThickness = 0;
                    painer2.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#2774CD");
                    painer.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#2774CD");
                    allRemoved = false;
                }
                else
                {
                    //MainPage.mainPageInstance.selectedRowIndex -= 1;
                    painer2.StrokeThickness = 1;
                    painer2.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31");
                    painer.Fill = Brushes.Transparent;
                }
            }
            if (allRemoved)
            {
                MainPage.mainPageInstance.selectedRowIndex = -1;
            }
        }
        public void rowBody_MouseEnter(object sender, MouseEventArgs e)
        {
            Rectangle rowBody = Util.FindVisualParent<Rectangle>(sender as Rectangle);
            if (rowBody.StrokeThickness == 2)
            {
                rowBody.StrokeThickness = 2;
            }
            else
            {
                rowBody.StrokeThickness = 0.5;
            }
        }
        public void rowBody_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle rowBody = Util.FindVisualParent<Rectangle>(sender as Rectangle);
            if (rowBody.StrokeThickness == 2)
            {
                rowBody.StrokeThickness = 2;
            }
            else
            {
                rowBody.StrokeThickness = 0;
            }
        }
        public void Create()
        {
            MainPage.mainPageInstance.realHotkeyList.Add(keyMap1);
            Console.WriteLine("NEW REGISTER");
            Rectangle rowBackground = new Rectangle
            {
                Name = string.Format("RemapBackground{0}", MainPage.mainPageInstance.rows),
                Width = 616,
                Height = 56,
                Fill = Brushes.Transparent,
                StrokeThickness = 0,
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#0094FF"),
                RadiusX = 10,
                RadiusY = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 2, 0, 2),
            };

            rowBackground.MouseLeftButtonDown += rowBody_MouseLeftButtonDown;
            rowBackground.MouseEnter += rowBody_MouseEnter;
            rowBackground.MouseLeave += rowBody_MouseLeave;

            Grid.SetRow(rowBackground, MainPage.mainPageInstance.rows);
            Grid.SetColumn(rowBackground, 0);
            Grid.SetColumnSpan(rowBackground, 4);
            Rectangle rowBody = new Rectangle
            {
                Name = string.Format("RemapRow{0}", MainPage.mainPageInstance.rows),
                Width = 560,
                Height = 44,
                Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF292C31"),
                StrokeThickness = 1,
                Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF34373B"),
                RadiusX = 10,
                RadiusY = 10,
                Margin = new Thickness(0, 8, 4, 8),
                IsHitTestVisible = false,
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
                IsHitTestVisible = false,
            };
            Grid.SetRow(keyIcon, MainPage.mainPageInstance.rows);
            Grid.SetColumn(keyIcon, 0);
            Label keyLabel = new Label
            {
                Name = "keyLabel",
                Content = keyMap1,
                FontSize = 20,
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Source Code Pro Medium"),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                IsHitTestVisible = false,
            };
            Grid.SetRow(keyLabel, MainPage.mainPageInstance.rows);
            Grid.SetColumn(keyLabel, 1);
            Rectangle arrow = new Rectangle
            {
                Name = string.Format("Arrow{0}", MainPage.mainPageInstance.rows),
                Width = 28,
                Height = 30,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/image/arrow.png"))),
            };
            Grid.SetRow(arrow, MainPage.mainPageInstance.rows);
            Grid.SetColumn(arrow, 2);

            Label keyLabel2 = new Label
            {
                Name = "keyLabel2",
                Content = keyMap2,
                FontSize = 20,
                FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./fonts/#Source Code Pro Medium"),
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                IsHitTestVisible = false,
            };
            Grid.SetRow(keyLabel2, MainPage.mainPageInstance.rows);
            Grid.SetColumn(keyLabel2, 3);

            RowDefinition gridRow = new RowDefinition();
            gridRow.Height = new GridLength(60);
            MainPage.mainPageInstance.BodyContainer.RowDefinitions.Add(gridRow);
            MainPage.mainPageInstance.rowList.Add(gridRow);
            MainPage.mainPageInstance.BodyContainer.Children.Add(rowBackground);
            MainPage.mainPageInstance.BodyContainer.Children.Add(keyIcon);
            MainPage.mainPageInstance.BodyContainer.Children.Add(rowBody);
            MainPage.mainPageInstance.BodyContainer.Children.Add(keyLabel);
            MainPage.mainPageInstance.BodyContainer.Children.Add(arrow);
            MainPage.mainPageInstance.BodyContainer.Children.Add(keyLabel2);
            if(MainPage.mainPageInstance.rows >= 6)
            {
                MainPage.mainPageInstance.ScrollContainer.Height += 60;
            }
            MainPage.mainPageInstance.BodyContainer.Height += 60;
            MainPage.mainPageInstance.rows += 1;
        }
    }
}
