﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;
using SilverlightLynxControls;

using SilverlightLFC.common;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml.Input;

namespace SLPhotoTest.PhotoTest
{
    public partial class Wave : UserControl
    {
        public Wave()
        {
            InitializeComponent();
            am = new ActionMove(this, Title);
            photoTestToolbar1.setTarget(lChartPhoto1);
            photoTestToolbar1.autoTest = ParameterAutoTest;
            photoTestToolbar1.addPhoto = AddPhoto;
            photoTestToolbar1.removePhoto = RemovePhoto;
            lChartPhoto1.getImage().PointerPressed += image_PointerPressed;

        }
        ActionMove am;
        List<Color> cl = new List<Color>();
        List<WriteableBitmap> bl = new List<WriteableBitmap>();
        PhotoTestParameter pt = new PhotoTestParameter();
        void RefreshShow()
        {
            stackBitmapList.Children.Clear();
            for (int i = 1; i <= bl.Count; i++)
            {
                WriteableBitmap bi = bl[i - 1];
                Image li = new Image();
                li.Source = bi;
                li.Tag = i;
                li.Width = stackBitmapList.Width / 5;
                stackBitmapList.Children.Add(li);
                li.PointerPressed += li_PointerPressed;
                cl.Add(pt.getAverageColor(bi));
            }
        }
        public void Test(List<WriteableBitmap> b)
        {
            if (b.Count == 0 || b == null) { return; }
            bl = b;
            try
            {
                this.textBlockWaveQ.Text = pt.getWhiteBalance(b).ToString();

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
        //System.Windows.Media.Imaging.WriteableBitmap cb;
        PhotoTestParameter ptp = new PhotoTestParameter();
        void LoadPhoto(WriteableBitmap TargetPhoto)
        {

        }
        void li_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Image im = sender as Image;
            int no = Convert.ToInt32(im.Tag);

            lChartPhoto1.setPhoto(im.Source as WriteableBitmap);

            canvasBright.Children.Clear();
            DrawGraphic dgb = new DrawGraphic(canvasBright);
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            List<int> hl = pt.getImageGrayHLine(im.Source as WriteableBitmap, (im.Source as WriteableBitmap).PixelHeight / 2);
            dgb.DrawBrightLines(hl);

            List<List<decimal>> al = new List<List<decimal>>();
            al = ptp.getCurveWaveQ(im.Source as WriteableBitmap, no);
            DrawCanvas.Children.Clear();
            DrawGraphic dg = new DrawGraphic(DrawCanvas);

            dg.DrawX();
            List<double> MarkList = new List<double>();
            int step = (al[0]).Count / 5;
            for (int i = 0; i < al[0].Count; i = i + step)
            {
                MarkList.Add(i);
            }
            dg.DrawXMark(MarkList);
            MarkList.Clear();
            for (int i = 0; i < 255; i = i + 40)
            {
                MarkList.Add(i);

            }
            dg.DrawY();
            dg.DrawYMark(MarkList);

            List<decimal> Sal, Val;
            Sal = al[0];
            Val = al[1];
            dg.ForeColor = Colors.Blue;
            dg.DrawLines(SilverlightLFC.common.Environment.getDoubleList<decimal>(Sal));
            dg.ForeColor = Colors.Red;
            dg.DrawLines(SilverlightLFC.common.Environment.getDoubleList<decimal>(Val));
            dg.DrawTitle("成像品质");

            textBlockCurrentBrightDis.Text = ptp.getWaveQ(im.Source as WriteableBitmap, no).ToString();
        }

        void image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (lChartPhoto1.getPhoto() == null) { return; }
            double h = e.GetCurrentPoint(lChartPhoto1.getImage()).Position.Y;
            int H = Convert.ToInt32(lChartPhoto1.getPhoto().PixelHeight * (1 - h / lChartPhoto1.getImage().Height));
            canvasBright.Children.Clear();
            DrawGraphic dgb = new DrawGraphic(canvasBright);
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            List<int> hl = pt.getImageGrayHLine(lChartPhoto1.getPhoto(), H);
            dgb.DrawBrightLines(hl);
            //throw new NotImplementedException();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            photoTestToolbar1 = null;

            bl.Clear();
            stackBitmapList.Children.Clear();
            DrawCanvas.Children.Clear();
            canvasBright.Children.Clear();
            lChartPhoto1.getImage().PointerPressed -= (image_PointerPressed);
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
    }
}
