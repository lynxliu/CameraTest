using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using SilverlightLynxControls;
using DCTestLibrary;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Input;
using LynxCameraTest.ViewModel;
using LynxCameraTest.Model;

namespace SLPhotoTest.PhotoTest
{
    public partial class PurplePercent : UserControl, IParameterTestView
    {
        public PurplePercent()
        {
            InitializeComponent();
            am = new ActionMove(this, this);
            dg = new DrawGraphic(DrawCanvas);
        }
        ActionMove am;
        PhotoTestParameter ptp = new PhotoTestParameter();
        List<WriteableBitmap> bl=new List<WriteableBitmap>();
        DrawGraphic dg;
        double PurplePix = 0;
        void RefreshShow()
        {
            stackBitmapList.Children.Clear();
            int n = lynxUpDown1.IntValue;
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
                this.stackBitmapList.Children.Add(li);
                li.PointerPressed += (li_MouseLeftButtonDown);
                PurplePix = PurplePix + ptp.getPurpleEdge(bi, n);
            }
        }

        public void Test(List<WriteableBitmap> b)
        {
            if (b.Count == 0 || b == null) { return; }
            bl = b;
            try
            {
                RefreshShow();

                dg.DrawColorCy(0.9f);

                int n = lynxUpDown1.IntValue;

                dg.DrawColorPoint(Colors.Purple, 0.9f, 3);
                dg.DrawColorHueArea(Colors.Purple, 0.9f, 2, n);
                if (bl.Count == 0) { return; }
                this.textPruplePercent.Text = (PurplePix / bl.Count).ToString();
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

        void li_MouseLeftButtonDown(object sender, PointerRoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            Image im = sender as Image;
            //int No = Convert.ToInt32(im.Tag);
            WriteableBitmap cb = im.Source as WriteableBitmap;

            lChartPhoto1.setPhoto(cb);
            //image.Source = bl[No - 1];
            int n = lynxUpDown1.IntValue;
            this.textPruplePercent.Text = ptp.getPurpleEdge(cb, n).ToString();
            DrawGraphic dg = new DrawGraphic(DrawCanvas);
            dg.DrawColorCy(0.9f);
            dg.DrawColorHueArea(Colors.Purple, 0.9f, 2, n);
            dg.DrawColorPoint(cb, 0.9f, 3);
        }

        //private void buttonCopy_Click(object sender, RoutedEventArgs e)
        //{
        //    CameraTestDesktop d = CameraTestDesktop.getDesktop();
        //    d.addClip(image.Source as WriteableBitmap);
        //}

        //private void buttonPaste_Click(object sender, RoutedEventArgs e)
        //{
        //    CameraTestDesktop d = CameraTestDesktop.getDesktop();
        //    image.Source = d.getClip();
        //}

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            bl.Clear();
            DrawCanvas.Children.Clear();

            dg = null;
            lChartPhoto1.Clear();
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
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

        public void InitViewModel()
        {
            if (DataContext == null) return;
            var vm = DataContext as CommonToolViewModel;
            if (vm == null) return;
            vm.Title = "Purple Pixel Percent In Border Parameter";
            vm.TestAction = ParameterAutoTest;
        }
    }
}
