using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;





namespace SLPhotoTest.UIControl
{
    public partial class LPhotoHeaderItem : UserControl
    {
        public LPhotoHeaderItem()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            TargetPhoto = null;
        }
        public WriteableBitmap TargetPhoto=null;
        public LMultiPhotoHeader TargetControl;
        public string TabName
        {
            get { return textBlock1.Text; }
            set { textBlock1.Text = value; }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            //(Parent as Panel).Children.Remove(this);
            //Clear();
            if (TargetPhoto != null)
            {
                TargetControl.RemovePhoto(TargetPhoto);
            }
            else
            {
                (Parent as Panel).Children.Remove(this);

            }
        }
    }
}
