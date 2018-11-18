using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using DCTestLibrary;
using SilverlightLynxControls;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using LynxCameraTest.ViewModel;
using LynxCameraTest.Model;

namespace SLPhotoTest.PhotoTest
{
    public partial class Latitude : UserControl, IParameterTestView
    {
        public Latitude()
        {
            InitializeComponent();
            am = new ActionMove(this, this);
            dg = new DrawGraphic(DrawCanvas);
        }
        ActionMove am;
        PhotoTestParameter ptp = new PhotoTestParameter();
        List<WriteableBitmap> bl=new List<WriteableBitmap>();
        DrawGraphic dg;
        List<double> al = new List<double>();
        void RefreshShow()
        {
            al.Clear();
            stackImages.Children.Clear();
            for (int i = 1; i <= bl.Count; i++)
            {
                WriteableBitmap bi = bl[i - 1];
                Image li = new Image();
                li.Source = bi;
                //li.Tag = i;
                li.Width = stackImages.Width / bl.Count;
                li.Height = stackImages.Height;
                double w = li.Height * bi.PixelWidth / bi.PixelHeight;
                if (li.Width > w)
                {
                    li.Width = w;
                }
                stackImages.Children.Add(li);
                li.PointerPressed += (li_MouseLeftButtonDown);
                double ab = ptp.getAverageBright(bi);
                al.Add(ab);
                dg.DrawBrightHistogram(al);
                
            }
        }
        public void Test(List<WriteableBitmap> b)
        {
            if (b.Count == 0 || b == null) { return; }
            bl = b;
            try
            {
                RefreshShow();
                textBlockLatitude.Text = ptp.getLatitude(bl, lynxUpDown1.IntValue).ToString();
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
            Image im = sender as Image;
            int No = Convert.ToInt32(im.Tag);

            lChartPhoto1.Photo=(im.Source as WriteableBitmap);

            textBlockBright.Text = ptp.getAverageBright(im.Source as WriteableBitmap).ToString();

        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            bl.Clear();
            //lChartPhoto1.Clear();
            //photoTestToolbar1 = null;
            dg = null;
            stackImages.Children.Clear();
            DrawCanvas.Children.Clear();
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void buttonDefault_Click(object sender, RoutedEventArgs e)
        {
            lynxUpDown1.LongValue = 7;
        }

        //private void buttonCaculate_Click(object sender, RoutedEventArgs e)
        //{
        //    this.textBlockLatitude.Text = ptp.getLatitude(bl, lynxUpDown1.IntValue).ToString();
        //}

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

        private void buttonSNR_Click(object sender, RoutedEventArgs e)
        {
            var wb = lChartPhoto1.Photo;
            if(wb!=null)
                SNR.Text =ptp.getImageSNR(wb).ToString();
        }
        public void InitViewModel()
        {
            if (DataContext == null) return;
            var vm = DataContext as CommonToolViewModel;
            if (vm == null) return;
            vm.Title = "Latitude Parameter";
            vm.TestAction = ParameterAutoTest;
        }
    }
}
