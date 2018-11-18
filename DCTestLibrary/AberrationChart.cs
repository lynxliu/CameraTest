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
    public class AberrationChart : AbstractTestChart//畸变测试，线性拟合，求出成像膨胀系数
    {//从中央和上下两边，各自取一个水平线，输出相同的黑线位置，比较膨胀的程度
        //先找到中央红线，然后向左右搜索。取中央线条的搜索数
        public override string ChartName { get { return "Aberration Chart"; } }
        public override string ChartMemo { get { return "Test canmera aberration"; } }

        public AberrationChart(WriteableBitmap Photo)
        {
            //InitBWList();
            //InitColorList();
            ChartPhoto = Photo;
            mp = new MarkProcess(Photo);
       
        }
        public AberrationChart() { }
        //public new void setChart(WriteableBitmap b)
        //{
        //    base.setChart(b);
        //    //CorrectISO12233Chart();
        //}

        bool IsSColor(Color sc,Color tc,int h)
        {
            if((Math.Abs(sc.R-tc.R)<h)&&(Math.Abs(sc.G-tc.G)<h)&&(Math.Abs(sc.B-tc.B)<h))
            {
                return true;
            }
            return false;
        }
        public List<int> ProcessColorList(List<Color> cl)//去除红色像素
        {
            List<int> tpl = new List<int>();
            for(int i=0;i< cl.Count;i++)
            {
                Color c=cl[i];
                if ((c.R < 35) && (c.G < 35) && (c.B < 35))
                {
                    tpl.Add(i);
                }
            }
            return tpl;
        }

        public double getAberration()//获得的还是中央鼓出或者凹陷距离和真正应该距离的比
        {
            List<Color> tl = getTopLine();
            List<Color> bl = getBottomLine();
            //List<Color> tml = getTMiddleLine();
            List<Color> ml = getBMiddleLine();
            int ci = getBlackPointNum(ml);
            int ti = getBlackPointNum(tl);
            int di = ti - ci;

            int ms = FindBlackPointPsition(ml, 0);
            int me = FindBlackPointPsition(ml, ci - 1);
            int ts = FindBlackPointPsition(tl, di / 2);
            int te = FindBlackPointPsition(tl, ti - di / 2);
            int bs = FindBlackPointPsition(bl, di / 2);
            int be = FindBlackPointPsition(bl, ti - di / 2);

            double md = me - ms;
            double td = te - ts;
            double bd = be - bs;
            return (md - (td + bd) / 2d) / ((td + bd) / 2d);
        }

        public int getTopBlackLineNum()
        {
            List<Color> cl = getTopLine();
            return getBlackPointNum(cl);
        }

        public int getCenterBlackLineNum()
        {
            List<Color> cl = getBMiddleLine();
            return getBlackPointNum(cl);
        }

        public int getBottomBlackLineNum()
        {
            List<Color> cl = getBottomLine();
            return getBlackPointNum(cl);
        }

        public int getTopBlackLinePix()
        {
            int n = getTopBlackLineNum();
            List<Color> cl = getTopLine();
            int ms = FindBlackPointPsition(cl, 0);
            int me = FindBlackPointPsition(cl, n);
            return me - ms;
        }

        public int getCenterBlackLinePix()
        {
            int n = getCenterBlackLineNum();
            List<Color> cl = getBMiddleLine();
            int ms = FindBlackPointPsition(cl, 0);
            int me = FindBlackPointPsition(cl, n);
            return me - ms;
        }

        public int getBottomBlackLinePix()
        {
            int n = getBottomBlackLineNum();
            List<Color> cl = getBottomLine();
            int ms = FindBlackPointPsition(cl, 0);
            int me = FindBlackPointPsition(cl, n);
            return me - ms;
        }

        public int getLineLength(int h,int s,int e)//获取特定高度水平线中间，第几根黑线到第几根黑线直接距离
        {
            List<Color> cl = getImageColorHLine(ChartPhoto, h);
            int ms = FindBlackPointPsition(cl, s);
            int me = FindBlackPointPsition(cl, e);
            return me - ms;
        }

        public List<Color> getTopLine()
        {
            int h = Convert.ToInt32(165 / 2856d * ChartPhoto.PixelHeight);
            return getImageColorHLine(ChartPhoto, h);

        }

        public List<Color> getBottomLine()
        {
            int h = Convert.ToInt32(2673 / 2856d * ChartPhoto.PixelHeight);
            return getImageColorHLine(ChartPhoto, h);
        }

        //public List<Color> getTMiddleLine()
        //{
        //    int h = Convert.ToInt32(165 / 2856d * SourcePhoto.PixelHeight);
        //    return getImageColorHLine(SourcePhoto, h);
        //}

        public List<Color> getBMiddleLine()
        {
            int h = Convert.ToInt32(1460 / 2856d * ChartPhoto.PixelHeight);
            return getImageColorHLine(ChartPhoto, h);
        }

        bool IsBlack(Color c)//RGB都小于60，并且RGB之间差异不大于12
        {
            if ((c.R <= 60) && (c.G <= 60) && (c.B <= 60))
            {
                if (Math.Max(Math.Max(c.R, c.G), c.B) - Math.Min(Math.Min(c.R, c.G), c.B)<12)
                {
                    return true;
                }
            }
            return false;
        }

        public int getBlackPointNum(List<Color> cl)
        {
            bool BeginBlack = false;
            int x = -1;
            for (int i = 0; i < cl.Count; i++)
            {
                Color c = cl[i];
                if (IsBlack(c))
                {
                    if (BeginBlack == false)
                    {
                        BeginBlack = true; x++;
                    }
                }
                else
                {
                    BeginBlack = false;
                }
            }
            return x;
        }


        public int FindBlackPointPsition(List<Color> cl,int t)
        {
            bool BeginBlack = false;
            int x = -1;
            for (int i = 0; i < cl.Count; i++)
            {
                Color c = cl[i];
                if (IsBlack(c))
                {
                    if (BeginBlack == false)
                    {
                        BeginBlack = true; x++;
                        if (x == t) { return i; }
                    }
                }
                else
                {
                    BeginBlack = false;
                }
            }
            return x;
        }

        public override void CorrectChart()
        {
            
        }
    }
}
