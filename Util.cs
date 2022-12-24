using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KeyRemap
{
    public class Util
    {
        public static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }
        public static IEnumerable<UIElement> ChildrenInRow(Grid grid, int row)
        {
            return grid.Children.Cast<UIElement>().Where(c => Grid.GetRow(c) == row);
        }
        public static bool ContainsAll<T>(IEnumerable<T> source, IEnumerable<T> values)
        {
            return values.All(value => source.Contains(value));
        }
    }
}
