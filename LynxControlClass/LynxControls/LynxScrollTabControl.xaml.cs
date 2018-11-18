using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;




namespace SilverlightLynxControls.LynxControls
{
    public partial class LynxScrollTabControl : UserControl
    {
        public LynxScrollTabControl()
        {
            InitializeComponent();
        }
        Canvas c = new Canvas();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int x = mm.Children.IndexOf(b);

            if (mm.Children.Contains(c))
            {
                mm.Children.Remove(c);
            }
            mm.Children.Insert(x, c);

            // TODO: Add event handler implementation here.
        }
    }
}
