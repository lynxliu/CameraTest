using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;




namespace SLPhotoTest
{
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            (Parent as Panel).Children.Remove(this);
        }
    }
}
