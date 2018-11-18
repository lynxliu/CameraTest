using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;
using SilverlightLynxControls;

using SilverlightLFC.common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;

namespace SLPhotoTest.PhotoTest
{
    public partial class Noise : UserControl
    {
        public Noise()
        {
            InitializeComponent();
            am = new ActionMove(this, Title);
            dg = new DrawGraphic(DrawCanvas);
            dg1 = new DrawGraphic(lChartPhoto1.getDrawObjectCanvas());
            photoTestToolbar1.setTarget(lChartPhoto1);
            photoTestToolbar1.autoTest = ParameterAutoTest;
            photoTestToolbar1.addPhoto = AddPhoto;
            photoTestToolbar1.removePhoto = RemovePhoto;
        }
        DrawGraphic dg,dg1;
        ActionMove am;
        PhotoTestParameter ptp = new PhotoTestParameter();
        List<WriteableBitmap> bl=new List<WriteableBitmap>();

        int TotleNoisePixs = 0, TotlePixs = 0;
        List<int> BrightPixList = new List<int>();//记录各个亮度的像素各自有多少

        void RefreshShow()
        {
            TotleNoisePixs = 0;
            TotlePixs = 0;
            stackBitmapList.Children.Clear();
            BrightPixList.Clear();

            for (int i = 0; i < 256; i++) { BrightPixList.Add(0); }//初始化
            
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
                li.PointerPressed += (li_PointerPressed);
                double ab = ptp.getAverageBright(bi);
                float f = Convert.ToSingle(lynxUpDown1.LongValue) / 100;
                TotleNoisePixs = TotleNoisePixs + ptp.getNoiseNum(bi, f);
                TotlePixs = TotlePixs + bi.PixelHeight*bi.PixelWidth;
                List<int> tl = ptp.getBrightPixNum(bi);
                BrightPixList = dg.AddBrightPixNum(BrightPixList, tl);


            }
        }

        public void Test(List<WriteableBitmap> b)
        {
            if (b.Count == 0 || b == null) { return; }
            bl = b;
            try
            {
                RefreshShow();

                textBlockNoiseNum.Text = TotleNoisePixs.ToString();
                textBlockNoisePercent.Text = (Convert.ToDouble(TotleNoisePixs) / TotlePixs * 100).ToString();
                dg.DrawBrightPixNumHistogram(BrightPixList);
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
 
        void li_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Image im = sender as Image;
            WriteableBitmap cb = im.Source as WriteableBitmap;

            lChartPhoto1.setPhoto(cb);

            float f = Convert.ToSingle(lynxUpDown1.LongValue) / 100;
            int n = ptp.getNoiseNum(cb, f);
            this.textBlockNoiseNum.Text = n.ToString();
            textBlockBright.Text = ptp.getAverageBright(cb).ToString();
            textBlockNoisePercent.Text = (Convert.ToDouble(n) / cb.PixelBuffer.Length/4 * 100).ToString();

            List<int> tl = ptp.getBrightPixNum(cb);
            dg.ForeColor = Colors.Blue;
            dg.DrawBrightPixNumHistogram(tl);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            stackBitmapList.Children.Clear();
            bl.Clear();
            DrawCanvas.Children.Clear();
            lChartPhoto1.Clear();
            photoTestToolbar1 = null;
            dg = null;
            ptp = null;
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonAll_Click(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            Test(bl);
        }

        private void buttonCaculate_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap cb = lChartPhoto1.getPhoto();
            if (cb == null) { SilverlightLFC.common.Environment.ShowMessage("请选择要测试的图片"); return; }
            float f = Convert.ToSingle(lynxUpDown1.LongValue) / 100;
            int n = ptp.getNoiseNum(cb, f);
            this.textBlockNoiseNum.Text = n.ToString();
            textBlockNoisePercent.Text = (Convert.ToDouble(n) / (cb.PixelHeight*cb.PixelWidth) * 100).ToString();
            textBlockBright.Text = ptp.getAverageBright(cb).ToString();
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

        private void buttonNoisePosition_Click(object sender, RoutedEventArgs e)
        {
            lChartPhoto1.getDrawObjectCanvas().Children.Clear();
            WriteableBitmap cb = lChartPhoto1.getPhoto();
            if (cb == null) { return; }
            float f = Convert.ToSingle(lynxUpDown1.LongValue) / 100;
            List<PixInfor> pl = ptp.getNoiseInfor(cb, f);
            dg1.DrawPixPosition(pl, cb.PixelWidth, cb.PixelHeight);
        }
    }
}
