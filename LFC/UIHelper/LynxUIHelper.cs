using System;
using System.Net;
using System.Windows;
using Windows.UI;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace SilverlightLFC.UIHelper
{
    public class LynxUIHelper//一些有关UI的辅助函数
    {
        public static Point? getGridCellTreePosition(Grid g, int r, int c)
        {
            if ((r >= g.RowDefinitions.Count) || (r < 0)) { return null; }
            if ((c >= g.ColumnDefinitions.Count) || (c < 0)) { return null; }
            double x = 0, y = 0;
            for (int i = 0; i < r; i++)
            {
                y = y + g.RowDefinitions[i].ActualHeight;
            }
            for (int i = 0; i < c; i++)
            {
                x = x + g.ColumnDefinitions[i].ActualWidth;
            }
            return new Point(x, y);
        }
    }
}
