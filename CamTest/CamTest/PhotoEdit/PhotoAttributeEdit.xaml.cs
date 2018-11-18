using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;




namespace SLPhotoTest.PhotoEdit
{
    public partial class PhotoAttributeEdit : UserControl
    {
        public PhotoAttributeEdit()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Panel p = Parent as Panel;
            PhotoEditCanvas pc = new PhotoEditCanvas();
            pc.Width = lynxUpDownW.IntValue;
            pc.Height = lynxUpDownH.IntValue;
            p.Children.Add(pc);
            p.Children.Remove(this);
        }
    }
}
