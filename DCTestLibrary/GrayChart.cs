using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using DCTestLibrary;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using SilverlightLFC.common;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SilverlightDCTestLibrary
{
    public class GrayChart : AbstractTestChart
    {
        public override string ChartName { get { return "Gray Chart"; } }
        public override string ChartMemo { get { return "Test canmera bright changes"; } }
        public GrayChart(WriteableBitmap Photo)
        {
            //InitBWList();
            //InitColorList();
            ChartPhoto = Photo;
            mp = new MarkProcess(Photo);
            //CorrectISO12233Chart();
            
        }
        public GrayChart() { }
        //public new void setChart(WriteableBitmap b)
        //{
        //    base.setChart(b);
        //    //CorrectISO12233Chart();
        //}

        public Dictionary<string, List<int>> getBrightChanges()//获得照片的八向亮度变化曲线
        {
            List<int> hl = getImageGrayHLine(ChartPhoto, ChartPhoto.PixelHeight / 2);
            List<int> vl = getImageGrayVLine(ChartPhoto, ChartPhoto.PixelWidth / 2);
            List<Color> cl = getLine(ChartPhoto, new Point(0, 0), new Point(ChartPhoto.PixelWidth, ChartPhoto.PixelHeight));
            List<Color> rcl = getLine(ChartPhoto, new Point(ChartPhoto.PixelWidth, 0), new Point(0, ChartPhoto.PixelHeight));

            List<int> bl=new List<int>();
            List<int> rbl=new List<int>();
            foreach (Color c in cl)
            {
                bl.Add(Convert.ToInt32(.299 * c.R + .587 * c.G + .114 * c.B));
            }
            foreach (Color c in rcl)
            {
                rbl.Add(Convert.ToInt32(.299 * c.R + .587 * c.G + .114 * c.B));
            }
            Dictionary<string, List<int>> dl = new Dictionary<string, List<int>>();
            dl.Add("HLine", hl);
            dl.Add("VLine", vl);
            dl.Add("LLine", bl);
            dl.Add("RLine", rbl);
            return dl;
        }

        public double getBrightChangesValue()
        {
            WriteableBitmap cb = getImageArea(ChartPhoto, Convert.ToInt32(ChartPhoto.PixelWidth * 0.475), Convert.ToInt32(ChartPhoto.PixelHeight * 0.475), Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
            WriteableBitmap ltb = getImageArea(ChartPhoto, 0, 0, Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
            WriteableBitmap rtb = getImageArea(ChartPhoto, Convert.ToInt32(ChartPhoto.PixelWidth * 0.95), 0, Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
            WriteableBitmap lbb = getImageArea(ChartPhoto, 0, Convert.ToInt32(ChartPhoto.PixelHeight * 0.95), Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
            WriteableBitmap rbb = getImageArea(ChartPhoto, Convert.ToInt32(ChartPhoto.PixelWidth * 0.95), Convert.ToInt32(ChartPhoto.PixelHeight * 0.95), Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
            //sendTestStepInfor(true, "Select Finish", 0.4);

            Color cc = getAverageColor(cb);
            Color ltc = getAverageColor(ltb);
            Color rtc = getAverageColor(rtb);
            Color lbc = getAverageColor(lbb);
            Color rbc = getAverageColor(rbb);
            //sendTestStepInfor(true, "Caculate Color Finish", 0.8);

            double bc = .299 * cc.R + .587 * cc.G + .114 * cc.B;
            double blt = .299 * ltc.R + .587 * ltc.G + .114 * ltc.B;
            double brt = .299 * rtc.R + .587 * rtc.G + .114 * rtc.B;
            double blb = .299 * lbc.R + .587 * lbc.G + .114 * lbc.B;
            double brb = .299 * rbc.R + .587 * rbc.G + .114 * rbc.B;

            return (bc - ((blt + brt + blb + brb) / 4)) / bc;
        }

        public WriteableBitmap getBrightChangesImage(int n)//绘制亮度变化曲线，共n级
        {
            if (ChartPhoto == null) { throw new SilverlightLFC.common.LFCException("没有设置测试卡照片", null); }
            WriteableBitmap cb = getImageArea(ChartPhoto, Convert.ToInt32(ChartPhoto.PixelWidth * 0.475), Convert.ToInt32(ChartPhoto.PixelHeight * 0.475), Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
            WriteableBitmap ltb = getImageArea(ChartPhoto, 0, 0, Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
            WriteableBitmap rtb = getImageArea(ChartPhoto, Convert.ToInt32(ChartPhoto.PixelWidth * 0.95), 0, Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
            WriteableBitmap lbb = getImageArea(ChartPhoto, 0, Convert.ToInt32(ChartPhoto.PixelHeight * 0.95), Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
            WriteableBitmap rbb = getImageArea(ChartPhoto, Convert.ToInt32(ChartPhoto.PixelWidth * 0.95), Convert.ToInt32(ChartPhoto.PixelHeight * 0.95), Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));

            Color cc = getAverageColor(cb);
            Color ltc = getAverageColor(ltb);
            Color rtc = getAverageColor(rtb);
            Color lbc = getAverageColor(lbb);
            Color rbc = getAverageColor(rbb);

            double bc = .299 * cc.R + .587 * cc.G + .114 * cc.B;
            double blt = .299 * ltc.R + .587 * ltc.G + .114 * ltc.B;
            double brt = .299 * rtc.R + .587 * rtc.G + .114 * rtc.B;
            double blb = .299 * lbc.R + .587 * lbc.G + .114 * lbc.B;
            double brb = .299 * rbc.R + .587 * rbc.G + .114 * rbc.B;

            double bb = (blt + brt + blb + brb) / 4;//平均四角亮度
            double th = bc - bb;//亮度差
            double ph = th / n;//平均每级的亮度

            WriteableBitmap vb = WriteableBitmapHelper.Clone(ChartPhoto);//复制一份
            var components=vb.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                Color c = new Color();
                c.B = components[i];
                c.G = components[i+1];
                c.R = components[i+2];
                c.A = components[i+3];
                double cbright = 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
                Byte tbright = DrawPixToGradeBright(cbright, bc, ph, n);
                components[i] = tbright;
                components[i+1] = tbright;
                components[i+2] = tbright;
                components[i+3] = 255;

            }

            return vb;
        }

        Byte DrawPixToGradeBright(double CurrentBright,double StartBright,double h,int Grade)//对给定的像素，依据本身亮度进行分级绘制其亮度
        {
            double ch = Math.Abs(CurrentBright - StartBright);
            int i = Convert.ToInt32(ch/h);
            if (i > Grade) { i = Grade; }
            return Convert.ToByte(255d / Grade * (Grade-i));
        }

        //public WriteableBitmap getBrightChangesImage(int n)//绘制亮度变化曲线，共n级
        //{
        //    if (ChartPhoto == null) { throw new SilverlightLFC.common.LFCException("没有设置测试卡照片", null); }
        //    WriteableBitmap cb = getImageArea(ChartPhoto, Convert.ToInt32(ChartPhoto.PixelWidth * 0.475), Convert.ToInt32(ChartPhoto.PixelHeight * 0.475), Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
        //    WriteableBitmap ltb = getImageArea(ChartPhoto, 0, 0, Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
        //    WriteableBitmap rtb = getImageArea(ChartPhoto, Convert.ToInt32(ChartPhoto.PixelWidth * 0.95), 0, Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
        //    WriteableBitmap lbb = getImageArea(ChartPhoto, 0, Convert.ToInt32(ChartPhoto.PixelHeight * 0.95), Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
        //    WriteableBitmap rbb = getImageArea(ChartPhoto, Convert.ToInt32(ChartPhoto.PixelWidth * 0.95), Convert.ToInt32(ChartPhoto.PixelHeight * 0.95), Convert.ToInt32(ChartPhoto.PixelWidth * 0.05), Convert.ToInt32(ChartPhoto.PixelHeight * 0.05));
        //    //sendTestStepInfor(true, "Caculate Color Finish", 0.2);
            
        //    Color cc = getAverageColor(cb);
        //    Color ltc = getAverageColor(ltb);
        //    Color rtc = getAverageColor(rtb);
        //    Color lbc = getAverageColor(lbb);
        //    Color rbc = getAverageColor(rbb);
        //    //sendTestStepInfor(true, "Caculate Color Finish", 0.4);

        //    double bc = .299 * cc.R + .587 * cc.G + .114 * cc.B;
        //    double blt = .299 * ltc.R + .587 * ltc.G + .114 * ltc.B;
        //    double brt = .299 * rtc.R + .587 * rtc.G + .114 * rtc.B;
        //    double blb = .299 * lbc.R + .587 * lbc.G + .114 * lbc.B;
        //    double brb = .299 * rbc.R + .587 * rbc.G + .114 * rbc.B;

        //    double bb=(blt + brt + blb + brb) / 4;
        //    double th = bc - bb;
        //    double ph = th / n;
        //    //sendTestStepInfor(true, "Caculate Color Finish", 0.5);

        //    WriteableBitmap vb = new WriteableBitmap(ChartPhoto);

        //    for(int i=0;i<n;i++)
        //    {
        //        //getFloodBrightEdge(vb, new Point(ChartPhoto.PixelWidth / 2, ChartPhoto.PixelHeight / 2), ph * (n - i));
        //        DrawBrightChangedCurve(vb, new Point(ChartPhoto.PixelWidth / 2, ChartPhoto.PixelHeight / 2), ph * (n - i), Colors.Red);
        //    }
        //    return vb;
        //     //查找亮度差异不大于h的边沿，给出内部的像素数，同时边沿标记特定颜色


        //}
        //bool IsBeginChanged(Color c, double StartBright, double h)
        //{
        //    double b = getBright(c);
        //    if (Math.Abs(b - StartBright) > h * BorderPercent)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //bool IsEndChanged(Color c, double StartBright, double h)
        //{
        //    double b = getBright(c);
        //    if (Math.Abs(b - StartBright) > h * BorderPercent)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //double BorderPercent = 0.2;//只有越过了BorderPercent×h的亮度区才可以说是变化了
        //void DrawYBrightChangedPoint(WriteableBitmap b, int x0,int startY,double StartBright, double h, Color DrawColor)
        //{
        //    int minY=0, maxY=0;
        //    for (int i = startY; i >= 0; i--)
        //    {
        //        bool bb = IsBeginChanged(GetPixel(b, x0, i), StartBright, h);
        //        if ((bb) && minY == 0) { minY = i; }
        //        bb = IsEndChanged(GetPixel(b, x0, i), StartBright, h);
        //        if ((bb) && maxY == 0) { minY = i; }
                
        //    }
        //    SetPixel(b, x0, (minY + maxY) / 2, DrawColor);
        //    minY = 0; maxY = 0;
        //    for (int i = startY; i <b.PixelHeight; i++)
        //    {
        //        bool bb = IsBeginChanged(GetPixel(b, x0, i), StartBright, h);
        //        if ((bb) && minY == 0) { minY = i; }
        //        bb = IsEndChanged(GetPixel(b, x0, i), StartBright, h);
        //        if ((bb) && maxY == 0) { minY = i; }
        //    }
        //    SetPixel(b, x0, (minY + maxY) / 2, DrawColor);
        //}
        //void DrawBrightChangedCurve(WriteableBitmap b, Point StartPoint,double h,Color DrawColor)
        //{
        //    int x0=Convert.ToInt32(StartPoint.X);
        //    int y0=Convert.ToInt32(StartPoint.Y);
        //    Color StartColor = GetPixel(b,x0 ,y0 );
        //    double StartBright = getBright(StartColor);

        //    for (int i = x0; i >= 0; i--)
        //    {
        //        bool bb = IsBeginChanged(GetPixel(b, i, y0), StartBright, h);
        //        if (bb) { SetPixel(b, i, y0, DrawColor); break; }
        //        else
        //        {
        //            DrawYBrightChangedPoint(b, i, y0, StartBright, h, DrawColor);
        //        }
        //    }
        //    for (int i = x0; i <b.PixelWidth; i++)
        //    {
        //        bool bb = IsBeginChanged(GetPixel(b, i, y0), StartBright, h);
        //        if (bb) { SetPixel(b, i, y0, DrawColor); break; }
        //        else
        //        {
        //            DrawYBrightChangedPoint(b, i, y0, StartBright, h, DrawColor);
        //        }
        //    }


        //}

        public double getSNR()
        {
            if (ChartPhoto == null) { throw new SilverlightLFC.common.LFCException("没有设置测试卡照片", null); }
            return getImageSNR(ChartPhoto,SignNoiseType.Gray);
        }

        public override void CorrectChart()
        {
            
        }
    }
}
