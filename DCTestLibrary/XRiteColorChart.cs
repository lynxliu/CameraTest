using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Collections;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;

namespace DCTestLibrary
{
    public class XRiteColorChart : AbstractTestChart
    {
        public override string ChartName { get { return "XRite Color Chart"; } }
        public override string ChartMemo { get { return "Test lots of parameter around color"; } }
        public XRiteColorChart(WriteableBitmap Photo)
        {
            ChartPhoto = Photo;
            InitBWList();
            InitColorList();
        }
        public XRiteColorChart()
        {
            InitBWList();
            InitColorList();
        }
        //public new void setChart(WriteableBitmap b)
        //{
        //    base.setChart(b);
        //    //CorrectXRite();
        //}

        public List<Color> ColorList;//保存色彩块位图理论值
        public List<Color> BWList;//保存黑白块位图理论值
        private List<Color> ValueColorList = new List<Color>();//保存色彩块位图实际值
        private List<Color> ValueBWList = new List<Color>();//保存黑白块位图实际值

        private int NoiseBrightChange = 30;//被认为是噪点的亮度差异百分比

        public void InitColorList()//初始化色块的颜色列表
        {
            ColorList = new List<Color>();
            ColorList.Add(Color.FromArgb(255,94, 28, 13));
            ColorList.Add(Color.FromArgb(255, 241, 149, 108));
            ColorList.Add(Color.FromArgb(255, 97, 119, 171));

            ColorList.Add(Color.FromArgb(255, 90, 103, 39));
            ColorList.Add(Color.FromArgb(255, 164, 131, 196));
            ColorList.Add(Color.FromArgb(255, 140, 253, 153));

            ColorList.Add(Color.FromArgb(255, 255, 116, 21));
            ColorList.Add(Color.FromArgb(255, 7, 47, 122));
            ColorList.Add(Color.FromArgb(255, 222, 29, 42));

            ColorList.Add(Color.FromArgb(255, 69, 0, 68));
            ColorList.Add(Color.FromArgb(255, 187, 255, 19));
            ColorList.Add(Color.FromArgb(255, 255, 142, 0));

            ColorList.Add(Color.FromArgb(255, 0, 0, 142));
            ColorList.Add(Color.FromArgb(255, 64, 173, 38));
            ColorList.Add(Color.FromArgb(255, 203, 0, 0));

            ColorList.Add(Color.FromArgb(255, 255, 217, 0));
            ColorList.Add(Color.FromArgb(255, 207, 3, 124));
            ColorList.Add(Color.FromArgb(255, 0, 148, 189));


        }

        public void InitBWList()//初始化灰度块的灰度值
        {
            BWList = new List<Color>();
            BWList.Add(Color.FromArgb(255, 255, 255, 255));
            BWList.Add(Color.FromArgb(255, 249, 249, 249));
            BWList.Add(Color.FromArgb(255, 180, 180, 180));
            BWList.Add(Color.FromArgb(255, 117, 117, 117));
            BWList.Add(Color.FromArgb(255, 53, 53, 53));
            BWList.Add(Color.FromArgb(255, 0, 0, 0));

        }

        public float getNoiseNum()//描绘噪点的严重程度，通过SRite的灰度块进行测试
        {
            return getNoiseNum(NoiseBrightChange);
        }

        public float getNoiseNum(int h)//描绘噪点的严重程度，通过SRite的灰度块进行测试
        {
            float x = 0;
            int pixNum = 0;
            WriteableBitmap subB = null;

            subB = getAreaSubCorrect(ChartPhoto, 4, 1);
            x = x + ptp.getNoiseNum(subB, h / 100f);
            pixNum = pixNum + subB.PixelWidth * subB.PixelHeight;
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 2);
            x = x + ptp.getNoiseNum(subB, h / 100f);
            pixNum = pixNum + subB.PixelWidth * subB.PixelHeight;
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 3);
            x = x + ptp.getNoiseNum(subB, h / 100f);
            pixNum = pixNum + subB.PixelWidth * subB.PixelHeight;
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 4);
            x = x + ptp.getNoiseNum(subB, h / 100f);
            pixNum = pixNum + subB.PixelWidth * subB.PixelHeight;
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 5);
            x = x + ptp.getNoiseNum(subB, h / 100f);
            pixNum = pixNum + subB.PixelWidth * subB.PixelHeight;
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 6);
            x = x + ptp.getNoiseNum(subB, h / 100f);
            pixNum = pixNum + subB.PixelWidth * subB.PixelHeight;
            //subB.Dispose();

            return x / Convert.ToSingle(pixNum);
        }

        public WriteableBitmap getAreaColor(int no)
        {
            int row;
            int col = no % 6;
            if (col == 0)
            {
                row = no / 6;
                col = 6;
            }
            else
            {
                row = no / 6+1;
            }
            return getAreaSubCorrect(ChartPhoto, row, col);
        }

        public WriteableBitmap getAreaSubCorrect(WriteableBitmap b, int row, int col)//获取特定行列里面经过矫正的区域
        {
            //b.Save("D:\\temp\\xb.jpg");
            int cx = Convert.ToInt32(b.PixelWidth * 0.0733);
            int cy = Convert.ToInt32(b.PixelHeight * 0.835);
            var p = b.PixelBuffer.ToArray();

            Color cc = Color.FromArgb(255, p[cy * b.PixelWidth + cx + 2], p[cy * b.PixelWidth + cx + 1], p[cy * b.PixelWidth + cx]);//查找第一个白色的点，用于校准位置
            if (IsWhite(cc, 50))//表示是正确的点
            {
                List<int> al = getImageGrayHLine(b, cy);
                int lp = getBorderPointPosition(al, cx, true, 120);//低于150就算是黑色了
                int rp = getBorderPointPosition(al, cx, false, 120);
                al = getImageGrayVLine(b, cx);
                int tp = getBorderPointPosition(al, cy, true, 120);
                int bp = getBorderPointPosition(al, cy, false, 120);
                int height = bp - tp;//找到一个色块的真实高度和宽度，用像素表示
                int width = rp - lp;
                int TAreaH = Convert.ToInt32(height / 2.5);
                int TAreaW = Convert.ToInt32(width / 2.5);
                cx = lp + TAreaW;
                cy = tp + TAreaH;

                int HGridWidth = Convert.ToInt32(0.52 / 4.1 * width);
                int VGridWidth = Convert.ToInt32(0.52 / 4.0 * height);
                cx = cx + (col - 1) * (HGridWidth + width);
                cy = cy + (row - 4) * (VGridWidth + height);
                int sx = cx - Convert.ToInt32(TAreaW / 2.5);
                int sy = cy - Convert.ToInt32(TAreaH / 2.5);
                //p = new Point(sx, sy);
                WriteableBitmap tb = getImageArea(b,sx,sy, TAreaW, TAreaH);
                if (IsAnalyse) { DrawSelectArea(cx,cy, TAreaW, TAreaH); }
                setSelectArea("Area_"+row.ToString()+"_"+col.ToString(), tb);
                return tb;
            }
            
            return null;
        }

        public void ProcColorValue()//分析记录实际的颜色值，把数值保存在ValueColorList里面
        {
            ValueColorList.Clear();
            WriteableBitmap subB = null;
            Color c;
            for (int i = 1; i < 19; i++)//仅仅测试前面18个
            {
                subB = getAreaColor(i);
                c = getAverageColor(subB);
                ValueColorList.Add(c);
                //subB.Dispose();
            }
        }

        public decimal getColorDistance()//测试色差，使用XRite的色标卡
        {
            List<float> al = new List<float>();
            decimal x = 0;
            WriteableBitmap subB = null;
            Color c,c0;
            float f = 0f;

            subB = getAreaSubCorrect(ChartPhoto, 1, 1);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[0];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 1, 2);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[1];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 1, 3);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[2];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 1, 4);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[3];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 1, 5);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[4];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 1, 6);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[5];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 2, 1);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[6];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 2, 2);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[7];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 2, 3);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[8];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 2, 4);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[9];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 2, 5);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[10];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 2, 6);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[11];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 3, 1);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[12];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 3, 2);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[13];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 3, 3);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[14];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 3, 4);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[15];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 3, 5);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[16];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 3, 6);
            c = getAverageColor(subB);
            c0 = (Color)ColorList[17];
            f = Math.Abs(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            al.Add(f);
            //subB.Dispose();

            for (int i = 0; i < al.Count; i++)
            {

                decimal dx = Convert.ToDecimal(al[i]);
                if (dx > 180)
                {
                    dx = 360-dx;
                }
                x = x + dx;
            }
            x = x / al.Count;
            return x;
        }

        public void ProcBWValue()//分析记录实际的颜色值，把数值保存在ValueColorList里面
        {
            ValueBWList.Clear();
            WriteableBitmap subB = null;
            Color c;
            for (int i = 19; i < 25; i++)//仅仅测试19-24，前面6个是RGB的，后面才是CMYK模式的，表示了真实的色差
            {
                subB = getAreaColor(i);
                c = getAverageColor(subB);
                ValueBWList.Add(c);
                //subB.Dispose();
            }
        }

        public float getWhiteBanlance()
        {
            float x = 0;
            WriteableBitmap subB = null;

            subB = getAreaSubCorrect(ChartPhoto, 4, 1);
            x = x + getWriteBanlance(subB);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 2);
            x = x + getWriteBanlance(subB);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 3);
            x = x + getWriteBanlance(subB);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 4);
            x = x + getWriteBanlance(subB);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 5);
            x = x + getWriteBanlance(subB);
            //subB.Dispose();

            subB = getAreaSubCorrect(ChartPhoto, 4, 6);
            x = x + getWriteBanlance(subB);
            //subB.Dispose();

            x = x / 6;
            return x;

        }

        public List<List<Color>> getCurveColorDis()
        {
            ProcColorValue();
            List<List<Color>> Val = new List<List<Color>>();
            for (int i = 0; i < 18; i++)
            {
                List<Color> ColorPare = new List<Color>();
                ColorPare.Add(ColorList[i]);//先加入原始的颜色，后加入实际读取的颜色
                ColorPare.Add(ValueColorList[i]);
                Val.Add(ColorPare);
            }
            return Val;
        }

        public List<decimal> getCurveLatitude()
        {
            ProcBWValue();
            List<decimal> al = new List<decimal>();
            for (int i = 0; i < ValueBWList.Count; i++)
            {
                al.Add(Convert.ToDecimal(PhotoTest.getBrightness(ValueBWList[i]) * 255));
            }

            return al;
        }

        public List<Color> getCurveWhiteBalance()
        {
            ProcBWValue();
            return ValueBWList;
        }

        public int NoiseSwing
        {
            get
            {
                return NoiseBrightChange;
            }
            set
            {
                if ((value > 0) && (value < 100))
                {
                    NoiseBrightChange = value;
                }
                else
                {
                    NoiseBrightChange = 30;
                }
            }
        }


        public override void CorrectChart()
        {
            
        }
    }
}
