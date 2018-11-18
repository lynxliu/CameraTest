using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using SilverlightPhotoIO;
using SilverlightLynxControls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;

namespace SLPhotoTest
{
    public partial class ClipView : UserControl
    {
        public ClipView()
        {
            InitializeComponent();
            am = new ActionMove(this,LayoutRoot);
            am.Enable = true;
        }
        ActionMove am;
        public WriteableBitmap OpenImage()
        {
            return PhotoIO.ReadImageFromFile().Result;

        }

        public void SaveImage()
        {
            if (image1.Source != null)
            {
                PhotoIO pi = new PhotoIO();
                PhotoIO.WriteImageToFile(image1.Source as WriteableBitmap);
            }
        }
        private void buttonL_Click(object sender, RoutedEventArgs e)
        {
            //if (Width > 1024) { return; }
            //this.Height = this.Height * 1.2;
            //this.Width = this.Width * 1.2;
            ImageBorder.Width = ImageBorder.Width * 1.2;
            ImageBorder.Height = ImageBorder.Height * 1.2;
            //textBlockIndexNo.Width = Width - 125 - 20;
            //Canvas.SetLeft(buttonHide, Width - 20);
        }

        private void buttonS_Click(object sender, RoutedEventArgs e)
        {
            //if (Width < 150) { return; }
            //this.Height = this.Height * 0.8;
            //this.Width = this.Width * 0.8;
            ImageBorder.Width = ImageBorder.Width * 0.8;
            ImageBorder.Height = ImageBorder.Height * 0.8;
            //textBlockIndexNo.Width = Width - 125 - 20;
        }

        private void buttonHide_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            Image i = Tag as Image;
            Visibility = Visibility.Collapsed;
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            if (d.ClipList.Children.Contains(i)) { d.ClipList.Children.Remove(i); }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap b= OpenImage();
            if (b == null) { return; }
            image1.Source = b;
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            d.addClip(b);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveImage();
        }

        private void buttonOne_Click(object sender, RoutedEventArgs e)
        {
            ImageBorder.Width = 300;
            ImageBorder.Height = 200;
        }
    }
}
