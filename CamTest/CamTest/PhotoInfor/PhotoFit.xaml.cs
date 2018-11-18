using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;




namespace SLPhotoTest.PhotoInfor
{
    public partial class PhotoFit : UserControl
    {
        public PhotoFit()
        {
            InitializeComponent();
        }
        public void setTarget(LChartPhoto l)
        {
            lc = l;
            lynxUpDownXPix.IntValue = l.getPhoto().PixelWidth;
            lynxUpDownYPix.IntValue = l.getPhoto().PixelHeight;
        }
        LChartPhoto lc;
        DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
        private void UpStep_Click(object sender, RoutedEventArgs e)
        {
            if (lc == null) { return; }
            if (lc.getPhoto() == null) { return; }
            var r = pt.MoveBitmap(lc.getPhoto(), 0, -1 * lynxUpDownMovePix.IntValue);
            if(r!=null&&r.Result!=null)
                lc.setPhoto(r.Result);
        }

        private void LeftStep_Click(object sender, RoutedEventArgs e)
        {
            if (lc == null) { return; }
            if (lc.getPhoto() == null) { return; }
            var r = pt.MoveBitmap(lc.getPhoto(), -1 * lynxUpDownMovePix.IntValue, 0);
            if(r!=null&&r.Result!=null)
                lc.setPhoto(r.Result);
        }

        private void DownStep_Click(object sender, RoutedEventArgs e)
        {
            if (lc == null) { return; }
            if (lc.getPhoto() == null) { return; }
            var r = pt.MoveBitmap(lc.getPhoto(), 0, lynxUpDownMovePix.IntValue);
            if(r!=null&&r.Result!=null)
                lc.setPhoto(r.Result);
        }

        private void RightStep_Click(object sender, RoutedEventArgs e)
        {
            if (lc == null) { return; }
            if (lc.getPhoto() == null) { return; }
            var r = pt.MoveBitmap(lc.getPhoto(), lynxUpDownMovePix.IntValue, 0);
            if(r!=null&&r.Result!=null)
                lc.setPhoto(r.Result);
        }

        private void buttonReversRotate_Click(object sender, RoutedEventArgs e)
        {
            if (lc == null) { return; }
            if (lc.getPhoto() == null) { return; }
            var r = pt.RotateBitmap(lc.getPhoto(), -1 * lynxUpDownAngle.DoubleValue);
            if (r != null && r.Result != null)
                lc.setPhoto(r.Result);
        }

        private void buttonRotate_Click(object sender, RoutedEventArgs e)
        {
            if (lc == null) { return; }
            if (lc.getPhoto() == null) { return; }
            var r = pt.RotateBitmap(lc.getPhoto(), lynxUpDownAngle.DoubleValue);
            if (r != null && r.Result != null)
                lc.setPhoto(r.Result);
        }

        private void buttonScale_Click(object sender, RoutedEventArgs e)
        {
            if (lc == null) { return; }
            if (lc.getPhoto() == null) { return; }
            float fx,fy;
            fx = 1f;
            fy = 1f;
            if (lynxUpDownXPix.IntValue != lc.getPhoto().PixelWidth)
            {
                fx = Convert.ToSingle(lynxUpDownXPix.IntValue) / lc.getPhoto().PixelWidth;
            }
            if (lynxUpDownYPix.IntValue != lc.getPhoto().PixelHeight)
            {
                fy = Convert.ToSingle(lynxUpDownYPix.IntValue) / lc.getPhoto().PixelHeight;
            }
            var r = pt.ScaleBitmap(lc.getPhoto(), fx, fy);
            if (r != null && r.Result != null)
                lc.setPhoto(r.Result);
        }

        private void lynxUpDownXPix_valueChanged(object sender, SilverlightLynxControls.LUpDownValueChangeArgs e)
        {
            if (lc == null) { return; }
            if (lc.getPhoto() == null) { return; }
            if (checkBox1.IsChecked == true)
            {
                float fx;
                if (lynxUpDownXPix.IntValue != lc.getPhoto().PixelWidth)
                {
                    
                    fx = Convert.ToSingle(lynxUpDownXPix.IntValue) / lc.getPhoto().PixelWidth;
                    lynxUpDownYPix.IntValue =Convert.ToInt32( fx * lc.getPhoto().PixelHeight);
                }

            }
        }

        private void lynxUpDownYPix_valueChanged(object sender, SilverlightLynxControls.LUpDownValueChangeArgs e)
        {
            if (lc == null) { return; }
            if (lc.getPhoto() == null) { return; }
            if (checkBox1.IsChecked == true)
            {
                if (lynxUpDownYPix.IntValue != lc.getPhoto().PixelHeight)
                {
                    float fy;
                    fy = Convert.ToSingle(lynxUpDownYPix.IntValue) / lc.getPhoto().PixelHeight;
                    lynxUpDownXPix.IntValue = Convert.ToInt32(fy * lc.getPhoto().PixelWidth);
                }

            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            (Parent as Panel).Children.Remove(this);
        }
    }
}
