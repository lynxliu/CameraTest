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
    public class ITEGrayscaleChart : AbstractTestChart//柯达灰度卡,进行动态范围测试
    {   //该卡一共12既灰度，一般来说，覆盖照片最黑和最白的地方，只测试上1/3处的带状区域
        //该线条找出最黑和最白的变化边界作为边界，计算明度

        public ITEGrayscaleChart(WriteableBitmap Photo)
        {
            ChartPhoto = Photo;
            mp = new MarkProcess(Photo);

        }
        public ITEGrayscaleChart() { }
        public override string ChartName { get { return "ITEGrayscaleChart"; } }
        public override string ChartMemo { get { return "Test Photo Gray Grade"; } }
        //int LatitudeBrightChange = 7;
        List<Color> ValueBWList = new List<Color>();//实际的黑白色块的取值集合

        public double ConstGradeL = 5;
        public bool IsUseConstGradeL = false;

        bool CanDifferentiate(double L1,double L2)
        {
            if (IsUseConstGradeL)
            {
                return Math.Abs(L1 - L2) > ConstGradeL;
            }
            return Math.Abs(L1 - L2) > getS(L1, L2);
        }
        public int getGrayGrade()
        {
            return getGrayGrade(ChartPhoto.PixelHeight / 3 - 10, 64,64);
        }
        public int getGrayGrade(int h, int w)
        {
            return getGrayGrade(ChartPhoto.PixelHeight / 3 - h/2, h, w);
        }
        public int getGrayGrade(int y,int h,int w)
        {
            List<double> l = getHLineAveL(y, h,w);
            int n = 1;
            for (int i = 1; i < l.Count; i++)
            {
                if (CanDifferentiate(l[i], l[i - 1])) { n++; }

            }
            return n;
        }
        public List<double> getHLineAveL(int h, int w)
        {
            return getHLineAveL(ChartPhoto.PixelHeight / 3 - h / 2, h, w);
        }
        public List<double> getHLineAveL(int y, int h,int w)
        {
            List<double> ll = new List<double>();
            WriteableBitmap cb = AutoBright(ChartPhoto);
            List<int> bl = getImageGrayHLine(cb, y);

            int l, r;
            r = FindLastWhite(bl, 20);
            l = FindFirstBlack(bl, 20);

            if (l >= r)//表示找到了下面的反向灰阶
            {
                l = FindFirstWhite(bl, 20);
                r = FindLastBlack(bl, 20);
            }
            if (r == -1 || l == -1) return new List<double>();

            int tl=r-l;
            int sl = Convert.ToInt32(tl / 11d);

            if(sl<w){
                w=sl;
            }
            List<int> tll = new List<int>();
            for (int i = l; i <= r-w; i = i + sl)
            {
                int m = Convert.ToInt32(i + (sl - w) / 2d);
                tll.Add(m);
                WriteableBitmap sb = getImageArea(ChartPhoto, i + (sl - w) / 2, y, w, h);
                ll.Add(getAverageColorL(sb));
            }

            return ll;

        }

        public double getS(List<double> l)//求取标准差
        {
            if (l.Count == 0) { return -1; }
            double a = 0;
            foreach (double d in l)
            {
                a += d;
            }
            a = a / l.Count;
            double x = 0;
            foreach (double d in l)
            {
                x += (d-a)*(d-a);
            }
            x = x / l.Count;
            return Math.Sqrt(x);
        }

        public double getS(double x,double y)//求取标准差
        {
            double a = 0;

            a = (x+y) / 2;

            double t = ((x - a) * (x - a) + (y - a) * (y - a)) / 2;

            return Math.Sqrt(t);
        }

        public override void CorrectChart()
        {

        }
    }
}
