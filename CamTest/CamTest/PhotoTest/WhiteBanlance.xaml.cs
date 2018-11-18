using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;
using SilverlightLynxControls;

using SilverlightLFC.common;
using SLPhotoTest.ChartTest;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;

namespace SLPhotoTest.PhotoTest
{
    public partial class WhiteBanlance : UserControl
    {
        public WhiteBanlance()
        {
            InitializeComponent();
            am = new ActionMove(this, Title);
            photoTestToolbar1.setTarget(lChartPhoto1);
            photoTestToolbar1.autoTest = ParameterAutoTest;
            photoTestToolbar1.addPhoto = AddPhoto;
            photoTestToolbar1.removePhoto = RemovePhoto;
            dg = new DrawGraphic(DrawCanvas);
        }
        ActionMove am;
        List<Color> cl = new List<Color>();
        List<WriteableBitmap> bl = new List<WriteableBitmap>();
        PhotoTestParameter pt = new PhotoTestParameter();
        DrawGraphic dg;
        void RefreshShow()
        {
            stackBitmapList.Children.Clear();
            for (int i = 1; i <= bl.Count; i++)
            {
                WriteableBitmap bi = bl[i - 1];
                Image li = new Image();
                li.Source = bi;
                li.Width = stackBitmapList.Width / bl.Count;
                li.Height = stackBitmapList.Height;
                double w = li.Height * bi.PixelWidth / bi.PixelHeight;
                if (li.Width > w)
                {
                    li.Width = w;
                }
                li.Tag = i;
                stackBitmapList.Children.Add(li);
                li.PointerPressed += li_PointerPressed;
                cl.Add(pt.getAverageColor(bi));
                dg.DrawColorPoint(cl[i - 1], 0.9f, 3);
            }
        }
        public void Test(List<WriteableBitmap> b)
        {
            if (b.Count == 0 || b == null) { return; }
            bl = b;
            try
            {
                textBlockWhiteBalance.Text = pt.getWhiteBalance(b).ToString();

                DrawCanvas.Children.Clear();
                dg.DrawColorCy(0.9f);
                RefreshShow();
            }
            catch (Exception xe)//未知的异常
            {
                if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
                {
                    SilverlightLFC.common.Environment.ShowMessage(xe.Message);
                }
                else
                {
                    SilverlightLFC.common.Environment.ShowMessage("测试错误,请检查照片");
                }
            }
        }
        void LoadPhoto(WriteableBitmap TargetPhoto)
        {
            if (TargetPhoto == null) { return; }
            lChartPhoto1.setPhoto( TargetPhoto);
            Color c = pt.getAverageColor(TargetPhoto);
            textBlockR.Text = c.R.ToString();
            textBlockG.Text = c.G.ToString();
            textBlockB.Text = c.B.ToString();

            double rg = (Convert.ToDouble(c.R) / Convert.ToDouble(c.G));
            double bg = (Convert.ToDouble(c.R) / Convert.ToDouble(c.G));
            textBlockJBBG.Text = bg.ToString();
            textBlockJBRG.Text = rg.ToString();
            if (rg > 0.8 && rg < 1.2 && bg > 0.8 && bg < 1.2)
                ChartTestHelper.setGBSign(true, gridGB);
            else
                ChartTestHelper.setGBSign(false, gridGB);


            DrawGraphic dg = new DrawGraphic(DrawCanvas);
            dg.ForeColor = Colors.Blue;
            dg.DrawColorPoint(c, 0.9f, 3);
        }

        void li_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Image im = sender as Image;
            //int No = Convert.ToInt32(im.Tag);
            //if (No >= bl.Count) { return; }
            LoadPhoto(im.Source as WriteableBitmap);
        }
        //private void buttonCopy_Click(object sender, RoutedEventArgs e)
        //{
        //    CameraTestDesktop d = CameraTestDesktop.getDesktop();
        //    d.addClip(image.Source as WriteableBitmap);
        //}

        //private void buttonPaste_Click(object sender, RoutedEventArgs e)
        //{
        //    CameraTestDesktop d = CameraTestDesktop.getDesktop();
        //    //image.Source = d.getClip();
        //    LoadPhoto(d.getClip());
        //}

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            bl.Clear();
            DrawCanvas.Children.Clear();
            stackBitmapList.Children.Clear();
            photoTestToolbar1 = null;
            dg = null;
            lChartPhoto1.Clear();

            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            DrawGraphic dg = new DrawGraphic(DrawCanvas);
            dg.DrawColorCy(0.9f);
        }

        private void buttonAll_Click(object sender, RoutedEventArgs e)
        {
            Test(bl);
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

        }
        public void ParameterAutoTest()
        {
            Test(bl);
        }

        public void AddPhoto(WriteableBitmap photo)
        {
            if (!bl.Contains(photo))
            {
                bl.Add(photo);
                RefreshShow();
            }
        }

        public void RemovePhoto(WriteableBitmap photo)
        {
            if (bl.Contains(photo))
            {
                bl.Remove(photo);
                RefreshShow();
            }
        }

    }
}
