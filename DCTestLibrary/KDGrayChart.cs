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

namespace SilverlightDCTestLibrary
{
    public class KDGrayChart : AbstractTestChart//柯达灰度卡,进行动态范围测试
    {   //该卡一共20既灰度，一般来说，覆盖照片最黑和最白的地方

        public KDGrayChart(WriteableBitmap Photo)
        {
            ChartPhoto = Photo;
            mp = new MarkProcess(Photo);
          
        }
        public KDGrayChart() { }
        public override string ChartName { get { return "KDGrayChart"; } }
        public override string ChartMemo { get { return "Test Photo Latitude"; } }
        int LatitudeBrightChange = 7;
        List<Color> ValueBWList = new List<Color>();//实际的黑白色块的取值集合
        //public WriteableBitmap getAreaBW(int No)//使用内部保存的处理过的图像
        //{
        //    return getAreaBW(SourcePhoto, No);
        //}

        public List<double> getHLine(long y)
        {
            List<int> bl = getImageGrayHLine(ChartPhoto, y);
            int max=0, min=255;
            foreach(int x in bl){
                if (x > max) { max = x; }
                if (x < min) { min = x; }
            }
            List<double> dl = new List<double>();
            foreach (int x in bl)
            {
                double d = (x - min) / Convert.ToDouble(max - min) * 255;
                dl.Add(d);
            }
            List<double> TrimList = new List<double>();

            int bp=0,ep=0;
            for (int i = 0; i < dl.Count;i++ )
            {
                if (dl[i] > 200) { bp = i; break; }
            }
            for (int i = dl.Count-1; i >0; i--)
            {
                if (dl[i] < 50 ) { ep = i; break; }
            }
            for (int i = bp; i < ep; i++)
            {
                TrimList.Add(dl[i]);
            }
            int step = Convert.ToInt32(TrimList.Count / 60d);//平分为20份

            List<double> sbl = new List<double>();
            for (int i = 0; i < 20; i++)
            {
                sbl.Add(getAverage(TrimList, step, i));
            }
            return sbl;
        }

        public double getAverage(List<double> sl, int step, int no)
        {
            if (no == 0) { no = 1; }
            else { no = no * 3 + 1; }
            double td = 0;
            for (int i = no * step; i < no * step+step; i++)
            {
                td = td + sl[i];
            }
            return td / step;
        }

        //public WriteableBitmap getAreaBW(WriteableBitmap b, int No)//依据序号获取灰度区块
        //{
        //    double Tw = 0.0423;

        //    double l = 0.038;
        //    double t = 0.81;
        //    double w = 0.016;
        //    double h = 0.07;
        //    int ax, ay, ah, aw;

        //    aw = Convert.ToInt32(b.PixelWidth * w);
        //    ah = Convert.ToInt32(b.PixelHeight * h);
        //    if ((No < 1) || (No > 22))
        //    {
        //        return null;
        //    }

        //    ax = Convert.ToInt32(b.PixelWidth * (l + (No - 1) * Tw));
        //    ay = Convert.ToInt32(b.PixelHeight * t);

        //    WriteableBitmap subB;
        //    subB = this.getImageArea(b, ax, ay, aw, ah);
        //    if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
        //    setSelectArea("AreaBW_" + No.ToString(), subB);
        //    return subB;

        //}
        //public List<double> getBWBrightList()//求出各个黑白块的平均亮度列表
        //{
        //    List<double> al = new List<double>();//记录各个灰度色块的平均灰度值，动态范围就是看灰度值的数量
        //    int x = 0;
        //    WriteableBitmap subB = null;
        //    for (int i = 1; i < 23; i++)
        //    {
        //        subB = getAreaBW(i);
        //        double c = getAverageBright(subB);
        //        //double b = 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;//最终能否分辨的亮度等级还是使用rgb实现
        //        al.Add(c);
        //        //subB.Dispose();
        //    }
        //    return al;
        //}

        public double getLatitude()//利用黑白块之间的平均级差作为标准
        {
            //ArrayList al = getBWBrightList();//记录各个灰度色块的平均灰度值，动态范围就是看灰度值的数量
            //decimal h = Convert.ToSingle(al[21]) - Convert.ToSingle(al[0]);
            //h = h / 21;//这个就是平均亮度差。

            return getLatitude(LatitudeBrightChange);//RGB亮度差异7以下可以认为无法识别，特别是在黑的地方
        }

        public double getLatitude(double h)//动态范围，获取灰度级别的平均值，看极差，极差大于一定程度就表示可以识别，默认h是表示级差
        {
            return getLatitude(ChartPhoto.PixelHeight/2, h);
        }

        public double getLatitude(int y,double h)//动态范围，获取灰度级别的平均值，看极差，极差大于一定程度就表示可以识别，默认h是表示级差
        {
            List<double> al = getHLine(y);//记录各个灰度色块的平均灰度值，动态范围就是看灰度值的数量
            int x = 0;
            for (int i = 0; i < al.Count - 1; i++)
            {
                double b0, b1;
                b0 = al[i];
                b1 = al[i + 1];
                if (Math.Abs(b0 - b1) > h)
                {
                    x++;
                }
            }
            return x;
        }


        //public List<decimal> getCurveLatitude()//给出实际的黑白区域的亮度变化
        //{
        //    ProcBWValue();
        //    List<decimal> al = new List<decimal>();
        //    for (int i = 0; i < ValueBWList.Count; i++)
        //    {
        //        decimal d = Convert.ToDecimal(PhotoTest.getBrightness(ValueBWList[i]) * 255);
        //        al.Add(d);
        //    }

        //    return al;
        //}

        //public void ProcBWValue()//分析记录实际的颜色值，把数值保存在ValueColorList里面
        //{
        //    ValueBWList.Clear();
        //    WriteableBitmap subB = null;
        //    Color c;
        //    for (int i = 1; i < 23; i++)
        //    {
        //        subB = getAreaBW(i);
        //        c = getAverageColor(subB);
        //        ValueBWList.Add(c);
        //        //subB.Dispose();
        //    }
        //}

        //public void Clear()
        //{

        //}

        public override void CorrectChart()
        {
            
        }
    }
}
