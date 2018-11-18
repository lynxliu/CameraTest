using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SilverlightLynxControls
{
    public partial class ChildWindowAbout : UserControl
    {
        public bool DialogResult { get; set; }
        public void Close()
        {
            var p = Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }
        public void Show()
        {
            Show((Panel) VisualTreeHelper.GetChild(Window.Current.Content, 0));
        }
        public void Show(Panel p)
        {
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            p.Children.Add(this);
        }
        public ChildWindowAbout()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

