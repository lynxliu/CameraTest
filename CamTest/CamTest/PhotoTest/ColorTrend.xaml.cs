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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;

namespace SLPhotoTest.PhotoTest
{
    public partial class ColorTrend : UserControl
    {
        public ColorTrend()
        {
            InitializeComponent();
            am = new ActionMove(this, Title);
            dg = new DrawGraphic(CC);

            photoTestToolbar1.setTarget(lChartPhoto);
            photoTestToolbar1.autoTest = ParameterAutoTest;
            photoTestToolbar1.addPhoto = AddPhoto;
            photoTestToolbar1.removePhoto = RemovePhoto;
        }
        ActionMove am;
        List<Color> sl=new List<Color>();//标准颜色

        List<WriteableBitmap> pl=new List<WriteableBitmap>();
        DCTestLibrary.PhotoTestParameter pt = new DCTestLibrary.PhotoTestParameter();
        DrawGraphic dg ;
        public void setStandardColorList(List<Color> tl)//设置标准颜色
        {
            sl.Clear();
            foreach (Color c in tl)
            {
                sl.Add(c);
            }
        }
        void RefreshShow()
        {
            currentImage = null;
            CC.Children.Clear();
            dg.DrawColorCy(0.9f);
            stackBitmapList.Children.Clear();
            for (int i = 1; i <= pl.Count; i++)
            {
                WriteableBitmap bi = pl[i - 1];
                Image li = new Image();
                li.Width = stackBitmapList.Width / pl.Count;
                li.Height = stackBitmapList.Height;
                double w = li.Height * bi.PixelWidth / bi.PixelHeight;
                if (li.Width > w)
                {
                    li.Width = w;
                }
                li.Source = bi;
                li.Tag = sl[i-1];//附属的是原始的理论颜色
                stackBitmapList.Children.Add(li);
                li.PointerPressed += li_PointerPressed;
                dg.DrawColorMoveHue(sl[i - 1], pt.getAverageColor(bi), 0.9f);
            }
        }

        public void Test(List<WriteableBitmap> b)
        {
            if (b.Count == 0 || b == null) { return; }
            pl = b;
            try
            {
                //pt.CurrentLabMode = getCurrentLabMode();
                RefreshShow();
                textBlockAverageDifferentHSB.Text = pt.getAverageColorTrend(sl, pl).ToString();
                textBlockAverageDifferentRGB.Text = pt.getAverageRGBColorTrend(sl, pl).ToString();
                textBlockAverageDifferentLab.Text = pt.getAverageLabColorTrend(sl, pl).ToString();
                double gbr= pt.getMaxLabColorTrend(sl, pl);
                textBlockMaxDifferentLab.Text =gbr.ToString();
                textBlockMaxDifferentRGB.Text = pt.getMaxRGBColorTrend(sl, pl).ToString();
                textBlockMaxDifferentHSB.Text = pt.getMaxColorTrend(sl, pl).ToString();

                if (gbr > 35)
                {
                    ChartTestHelper.setGBSign(false, gridGB);
                }
                else
                {
                    ChartTestHelper.setGBSign(true, gridGB);
                }
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
        void ShowSourceInfor(Color tc)
        {
            rectangleS.Fill = new SolidColorBrush(tc);


            textBlockTheoryGRB_R.Text = tc.R.ToString();
            textBlockTheoryGRB_G.Text = tc.G.ToString();
            textBlockTheoryGRB_B.Text = tc.B.ToString();
            textBlockTheoryHSB_H.Text = DCTestLibrary.PhotoTest.getHue(tc).ToString();
            textBlockTheoryHSB_S.Text = DCTestLibrary.PhotoTest.getSaturation(tc).ToString();
            textBlockTheoryHSB_B.Text = DCTestLibrary.PhotoTest.getBrightness(tc).ToString();
            textBlockTheoryLab_L.Text = DCTestLibrary.ColorManager.getLabL(tc,pt.CurrentLabMode).ToString();
            textBlockTheoryLab_a.Text = DCTestLibrary.ColorManager.getLaba(tc, pt.CurrentLabMode).ToString();
            textBlockTheoryLab_b.Text = DCTestLibrary.ColorManager.getLabb(tc, pt.CurrentLabMode).ToString();

            
        }
        void ShowTrueColorInfor(Color cc)
        {

            textBlockCurrentGRB_R.Text = cc.R.ToString();
            textBlockCurrentGRB_G.Text = cc.G.ToString();
            textBlockCurrentGRB_B.Text = cc.B.ToString();
            textBlockCurrentHSB_H.Text = DCTestLibrary.PhotoTest.getHue(cc).ToString();
            textBlockCurrentHSB_S.Text = DCTestLibrary.PhotoTest.getSaturation(cc).ToString();
            textBlockCurrentHSB_B.Text = DCTestLibrary.PhotoTest.getBrightness(cc).ToString();
            textBlockCurrentLab_L.Text = DCTestLibrary.ColorManager.getLabL(cc, pt.CurrentLabMode).ToString();
            textBlockCurrentLab_a.Text = DCTestLibrary.ColorManager.getLaba(cc, pt.CurrentLabMode).ToString();
            textBlockCurrentLab_b.Text = DCTestLibrary.ColorManager.getLabb(cc, pt.CurrentLabMode).ToString();
        }

        void ShowTrueColorDiffrent(Color cc,Color tc)
        {

            textBlockDiffGRB_R.Text = (cc.R-tc.R).ToString();
            textBlockDiffGRB_G.Text = (cc.G - tc.G).ToString();
            textBlockDiffGRB_B.Text = (cc.B - tc.B).ToString();
            textBlockDiffHSB_H.Text = (DCTestLibrary.PhotoTest.getHue(cc) - DCTestLibrary.PhotoTest.getHue(tc)).ToString();
            textBlockDiffHSB_S.Text = (DCTestLibrary.PhotoTest.getSaturation(cc) - DCTestLibrary.PhotoTest.getSaturation(tc)).ToString();
            textBlockDiffHSB_B.Text = (DCTestLibrary.PhotoTest.getBrightness(cc) - DCTestLibrary.PhotoTest.getBrightness(tc)).ToString();
            textBlockDiffLab_L.Text = (DCTestLibrary.ColorManager.getLabL(cc, pt.CurrentLabMode) - DCTestLibrary.ColorManager.getLabL(tc, pt.CurrentLabMode)).ToString();
            textBlockDiffLab_a.Text = (DCTestLibrary.ColorManager.getLaba(cc, pt.CurrentLabMode) - DCTestLibrary.ColorManager.getLaba(tc, pt.CurrentLabMode)).ToString();
            textBlockDiffLab_b.Text = (DCTestLibrary.ColorManager.getLabb(cc, pt.CurrentLabMode) - DCTestLibrary.ColorManager.getLabb(tc, pt.CurrentLabMode)).ToString();

            textBlockCurrentDifferentHSB.Text = pt.getColorHueDistance(cc, tc).ToString();
            textBlockCurrentDifferentRGB.Text = pt.getColorRGBDistance(cc, tc).ToString();
            textBlockCurrentDifferentLab.Text = pt.getColorLabDistance(cc, tc).ToString();
        }
        Color sc = Colors.White;//默认颜色
        Color cc = Colors.Transparent;//当前颜色
        void li_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

            Image im = sender as Image;
            currentImage = im;
            Color? tl = im.Tag as Color?;
            
            if (tl != null)
            {
                sc = tl.Value;
            }
            ShowSourceInfor(sc);

            WriteableBitmap tb = im.Source as WriteableBitmap;
            if (tb == null) { return; }
            lChartPhoto.setPhoto(tb);
            cc = pt.getAverageColor(tb);
            ShowTrueColorInfor(cc);

            ShowTrueColorDiffrent(cc, sc);

            DrawGraphic dg = new DrawGraphic(CC);
            CC.Children.Clear();
            dg.DrawColorCy(0.9f);
            dg.DrawColorMoveHue(sc, cc, 0.9f);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            pl.Clear();
            stackBitmapList.Children.Clear();
            CC.Children.Clear();
            photoTestToolbar1 = null;
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            DrawGraphic dg = new DrawGraphic(CC);
            CC.Children.Clear();
            dg.DrawColorCy(0.9f);

        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ParameterAutoTest()
        {
            Test(pl);
        }

        public void AddPhoto(WriteableBitmap photo)
        {
            if (!pl.Contains(photo)){
                pl.Add(photo);
                RefreshShow();
            }
        }

        public void RemovePhoto(WriteableBitmap photo)
        {
            if (pl.Contains(photo))
            {
                pl.Remove(photo);
                RefreshShow();
            } 
            
        }
        Image currentImage = null;

        private void buttonChangeHSB_Click(object sender, RoutedEventArgs e)
        {
            Color cc = Color.FromArgb(255, Convert.ToByte(textBlockCurrentGRB_R.Text), Convert.ToByte(textBlockCurrentGRB_G.Text), Convert.ToByte(textBlockCurrentGRB_B.Text));
            LColor lc = new LColor();
            lc.setColorByHSB(lynxUpDownHSB_H.DoubleValue,lynxUpDownHSB_S.DoubleValue,lynxUpDownHSB_B.DoubleValue);

            Color sc = lc.getColor();
            rectangleS.Fill = new SolidColorBrush(sc);
            ShowSourceInfor(sc);
            ShowTrueColorDiffrent(cc, sc);
            if (currentImage != null)
            {
                currentImage.Tag = sc;//变更其理论颜色
            }
        }

        private void buttonChangeRGB_Click(object sender, RoutedEventArgs e)
        {
            Color cc= Color.FromArgb(255,Convert.ToByte(textBlockCurrentGRB_R.Text),Convert.ToByte(textBlockCurrentGRB_G.Text),Convert.ToByte(textBlockCurrentGRB_B.Text));
            Color sc = Color.FromArgb(Convert.ToByte(255), Convert.ToByte(lynxUpDownRGB_R.IntValue), Convert.ToByte(lynxUpDownRGB_R.IntValue), Convert.ToByte(lynxUpDownRGB_R.IntValue));
            rectangleS.Fill = new SolidColorBrush(sc);
            ShowSourceInfor(sc);
            ShowTrueColorDiffrent(cc,sc);
            if (currentImage != null)
            {
                currentImage.Tag = sc;//变更其理论颜色
            }
        }
        //LabMode currentLabMode = LabMode.CIE;
        private void buttonChangeLab_Click(object sender, RoutedEventArgs e)
        {
            Color cc = Color.FromArgb(255, Convert.ToByte(textBlockCurrentGRB_R.Text), Convert.ToByte(textBlockCurrentGRB_G.Text), Convert.ToByte(textBlockCurrentGRB_B.Text));
            LColor lc = new LColor();
            lc.setColorByLab(lynxUpDownLab_L.DoubleValue, lynxUpDownLab_a.DoubleValue, lynxUpDownLab_b.DoubleValue, pt.CurrentLabMode);

            Color sc = lc.getColor();
            rectangleS.Fill = new SolidColorBrush(sc);
            ShowSourceInfor(sc);
            ShowTrueColorDiffrent(cc, sc);
            if (currentImage != null)
            {
                currentImage.Tag = sc;//变更其理论颜色
            }
        }

        //private LabMode getCurrentLabMode()
        //{
        //    if (comboBoxLabMode.SelectedIndex == 0)
        //    {
        //        return LabMode.CIE;
        //    }
        //    else
        //    {
        //        return LabMode.Photoshop;
        //    }
        //}

        private void comboBoxLabMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxLabMode==null)return;
            if (comboBoxLabMode.SelectedIndex == 0)
            {
                pt.CurrentLabMode = LabMode.CIE;
            }
            if (comboBoxLabMode.SelectedIndex == 1)
            {
                pt.CurrentLabMode = LabMode.Photoshop;
            }
            if (comboBoxLabMode.SelectedIndex == 2)
            {
                pt.CurrentLabMode = LabMode.Undefine;
            }

            textBlockAverageDifferentLab.Text = pt.getAverageLabColorTrend(sl, pl).ToString();
            double gbr = pt.getMaxLabColorTrend(sl, pl);
            textBlockMaxDifferentLab.Text = gbr.ToString();
            ShowSourceInfor(sc);
            ShowTrueColorInfor(cc);

            ShowTrueColorDiffrent(cc, sc);
        }
    }
}
