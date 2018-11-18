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
using Windows.UI.Xaml.Media;
using Windows.UI;
using PhotoTestControl;
using LynxCameraTest.ViewModel;
using LynxCameraTest.Model;

namespace SLPhotoTest.PhotoTest
{
    public partial class BadPixTest : UserControl, IParameterTestView
    {
        public BadPixTest()
        {
            InitializeComponent();
            am = new ActionMove(this, this);
            dg = new DrawGraphic(lChartPhotoBadPix.getDrawObjectCanvas());
            //am1 = new ActionMove(ProcPhoto, ProcPhoto);
            //am1.Enable = false;
        }
        ActionMove am;
        PhotoTestParameter ptp = new PhotoTestParameter();
        List<WriteableBitmap> bl=new List<WriteableBitmap>();
        DrawGraphic dg;
        List<PixInfor> BadPixList = new List<PixInfor>();
        List<PixInfor> JBBadPixList = new List<PixInfor>();//按照国标测试时候的数据

        SolidColorBrush JBFailBrush = new SolidColorBrush(Colors.Red);
        SolidColorBrush JBSuccessBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));

        public void Test()
        {
            if (bl.Count > 0)
            {
                Test(bl);
            }
        }
        void ShowGBBadInfor()
        {
            textBlockJBBadPixNum.Text = JBBadPixList.Count.ToString();
            int c = 0;
            foreach (PixInfor p in JBBadPixList)
            {
                if (p.XPosition > (lChartPhotoBadPix.Photo).PixelWidth * 0.25 && p.XPosition < (lChartPhotoBadPix.Photo).PixelWidth * 0.75 && p.YPosition > lChartPhotoBadPix.getPhoto().PixelHeight * 0.25 && p.YPosition < lChartPhotoBadPix.getPhoto().PixelHeight * 0.75)
                {
                    c++;
                }
            }
            textBlockJBCenterBadPixNum.Text = c.ToString();
            if (JBBadPixList.Count > 2 || c > 0)
            {
                ChartTestHelper.setGBSign(false, gridGB);
            }
            else
            {
                ChartTestHelper.setGBSign(true, gridGB);
            }
        }
        void ShowBadInfor()
        {
            textBlockNoiseNum.Text = BadPixList.Count.ToString();
            string s = string.Format("{0:G17} ", (Convert.ToDouble(BadPixList.Count) / (bl[0].PixelHeight*bl[0].PixelWidth) * 100));
            textBlockNoisePercent.Text = s + "%";
        }

        void ShowBadCombInfor(List<PixInfor> pl, ComboBox cb)//显示疑似坏点
        {

        
            cb.Items.Clear();
            int tc = 3000;//最多加3000
            if (pl.Count < tc) { tc = pl.Count; }
            for (int i=0;i<tc;i++){
                PixInfor pi = pl[i];

                ComboBoxItem ci = new ComboBoxItem();
                ci.Content = pi.getDescription();
                cb.Items.Add(ci);
            }
        }

        void ShowGBBadPosition()
        {
            if (bl.Count < 2) { return; }
            List<PixInfor> pl = new List<PixInfor>();
            if (JBBadPixList.Count > 3000)
            {
                pl = JBBadPixList.GetRange(0, 3000);
            }
            else
            {
                pl = JBBadPixList;
            }
            if (BadPixList != null)
            {
                dg.DrawPixPosition(pl, bl[0].PixelWidth, bl[0].PixelHeight);
            }
        }


        void ShowBadPosition()
        {
            if (bl.Count < 2) { return; }
            List<PixInfor> pl = new List<PixInfor>();
            if (BadPixList.Count > 3000)
            {
                pl = BadPixList.GetRange(0, 3000);
            }
            else
            {
                pl = BadPixList;
            }
            if (BadPixList != null)
            {
                dg.DrawPixPosition(pl, bl[0].PixelWidth, bl[0].PixelHeight);
            }
        }
        void RefreshShowList()//依据bl重新显示列表
        {
            stackBitmapList.Children.Clear();
            if (bl.Count == 0) { return; }
            double w, h;
            w = bl[0].PixelWidth;
            h = bl[0].PixelHeight;
            for (int i = 0; i < bl.Count; i++)
            {
                WriteableBitmap bi = bl[i];
                if ((bi.PixelHeight - h > 1) || (bi.PixelWidth - w > 1))
                {
                    SilverlightLFC.common.Environment.ShowMessage("第" + i.ToString() + "照片的尺寸不一致，将忽略此照片！");
                }
                Image li = new Image();
                li.Source = bi;
                li.Width = stackBitmapList.Width / bl.Count;
                li.Height = stackBitmapList.Height;
                double tw = li.Height * bi.PixelWidth / bi.PixelHeight;
                if (li.Width >tw)
                {
                    li.Width = tw;
                }
                this.stackBitmapList.Children.Add(li);
                li.Tapped += li_Tapped;

            }
        }

        void li_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Image im = sender as Image;
            if (im == null) { return; }

            selectBitmap(im.Source as WriteableBitmap);
        }
        void selectBitmap(WriteableBitmap b)
        {
            lChartPhotoBadPix.Photo=(b);
            lChartPhotoBadPix.Opacity = 0.5;
        }
        public void Test(List<WriteableBitmap> b)
        {
            if (b.Count == 0||b==null) { return; }
            bl = b;
            //NoisePixList.Clear();
            //NoisePixList = ptp.getNoiseInfor(bl[0], 10);//获得初始的噪点数值
            JBBadPixList = ptp.getGBBadPix(bl, lynxUpDownExceptionH.IntValue, lynxUpDownMinStep.IntValue, lynxUpDownMaxStep.IntValue,lynxUpDownBadH.IntValue);

            BadPixList=ptp.getEqualPix(bl);
            if (BadPixList == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("必须使用同样分辨率的照片进行测试！");
                return;
            }
            try
            {
                lChartPhotoBadPix.getDrawObjectCanvas().Children.Clear();
                ShowBadCombInfor(BadPixList, comboBoxBabPixInfor);
                ShowBadCombInfor(JBBadPixList, comboBoxJBBabPixInfor);
                ShowGBBadInfor();
                ShowBadInfor();
                ShowBadPosition();
                ShowGBBadPosition();
                selectBitmap(b[0]);
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

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            stackBitmapList.Children.Clear();
            bl.Clear();

            dg = null;
            lChartPhotoBadPix.Clear();
            lChartPhotoBadPix = null;
            ptp = null;
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        public void AddPhoto(WriteableBitmap photo)
        {
            if (!bl.Contains(photo))
            {
                bl.Add(photo);
                RefreshShowList();
            }
        }

        public void RemovePhoto(WriteableBitmap photo)
        {
            if (bl.Contains(photo))
            {
                bl.Remove(photo);
                RefreshShowList();
            }

        }

        private void buttonBadPixInfor_Click(object sender, RoutedEventArgs e)
        {
            ptp.setStep(lynxUpDown2.IntValue);
            ptp.setH(lynxUpDown1.DoubleValue);
            BadPixList = ptp.getEqualPix(bl);
            if (BadPixList == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("必须使用同样分辨率的照片进行测试！");
                return;
            }
            lChartPhotoBadPix.getDrawObjectCanvas().Children.Clear();
            ShowBadInfor();
            ShowBadCombInfor(BadPixList,comboBoxBabPixInfor);
            ShowBadPosition();
        }

        private void buttonShowGBBadPix_Click(object sender, RoutedEventArgs e)
        {
            lChartPhotoBadPix.Photo=null;
            ShowGBBadPosition();
        }

        private void buttonShowBadPix_Click(object sender, RoutedEventArgs e)
        {
            lChartPhotoBadPix.Photo = null;
            ShowBadPosition();
        }

        public void InitViewModel()
        {
            if (DataContext == null) return;
            var vm = DataContext as CommonToolViewModel;
            if (vm == null) return;
            vm.Title = "Bad Pixel Test Tool";
            vm.TestAction = Test;

        }
    }
}
