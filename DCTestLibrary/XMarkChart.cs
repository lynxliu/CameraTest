using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Collections;
using System.Windows;
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

namespace DCTestLibrary
{
    public class XMarkChart : AbstractTestChart
    {
        public override string ChartName { get { return "XMark Chart"; } }
        public override string ChartMemo { get { return "Design to test almost all parameter only with one photo"; } }
        public XMarkChart()
        {
            InitBWList();
            InitColorList();
        }
        public XMarkChart(WriteableBitmap Photo)
        {
            InitBWList();
            InitColorList();
            ChartPhoto = Photo;
            //CorrectXMark();
        }
        //public new void setChart(WriteableBitmap b)
        //{

        //    base.setChart(b);
        //    CorrectXMark();
        //}
        //private WriteableBitmap ChartPhoto;//基本的照片
        public string PhotoMode = "CMYK Print";
        //private WriteableBitmap CorrectPhoto;//记录以及纠正了的照片
        //private WriteableBitmap AnalysePhoto;//记录了选择区以后的Photo
        public WriteableBitmap CropPhoto;//记录已经被纠正和裁减了的照片
        //public bool IsAnalyse = false;//标识是不是在调试的状态
        //public WriteableBitmap AreaPhoto
        //{
        //    get
        //    {
        //        return AnalysePhoto;
        //    }
        //}

        //public WriteableBitmap SourcePhoto
        //{
        //    get
        //    {
        //        return ChartPhoto;
        //    }
        //    set
        //    {
        //        ChartPhoto = value;
        //        CorrectXMark();
               
        //    }
        //}

        private int MTFPercenr=50;
        private int NoiseBrightChange = 30;//被确认为噪点的亮度异常百分比
        private int LatitudeBrightChange = 7;//可以被识别出来的亮度等级的亮度差

        public override void BeginAnalyse()
        {
            IsAnalyse = true;
            mp.SelectedPhoto = WriteableBitmapHelper.Clone(CropPhoto);
        }//开始调试状态，主要是保存选取区域


        public int StandardBlackValue;//照片的基本黑色值，0-255之间
        public int StandardWhiteValue;//照片的基本白色值，0-255之间
        public List<Color> ColorList;//保存色彩块位图
        public List<Color> BWList;//保存黑白块位图
        public List<decimal> ResoveList;//保存分辨率块位图，增序
        public List<decimal> WaveList;//保存正弦波列表，增序

        private List<Color> ValueColorList = new List<Color>();//实际的色彩块取值的集合
        private List<Color> ValueBWList = new List<Color>();//实际的黑白色块的取值集合
        //private ArrayList ValueResoveList = new ArrayList();//实际的分辨率块的mtf集合。区别于从边沿得到的mtf
        
        public int TrueLongMM = 240;//默认的初始化长边的长度
        private void InitTrueLongMM()//设置默认的边长，这是从角上的颜色来确认的
        {
            TrueLongMM = 240;
        }

        public void InitColorList()//初始化色块的颜色列表
        {
            ColorList = new List<Color>();
            ColorList.Add(Color.FromArgb(255, 255, 0, 0));
            ColorList.Add(Color.FromArgb(255, 0, 0, 255));
            ColorList.Add(Color.FromArgb(255, 0, 255, 0));

            ColorList.Add(Color.FromArgb(255, 255, 255, 0));
            ColorList.Add(Color.FromArgb(255, 0, 255, 255));
            ColorList.Add(Color.FromArgb(255, 255, 0, 255));
            //前面的6个是rgb模式的，仅仅在直接拍摄屏幕的情况下面使用
            ColorList.Add(Color.FromArgb(255, 230, 30, 25));
            ColorList.Add(Color.FromArgb(255, 106, 184, 45));
            ColorList.Add(Color.FromArgb(255, 25, 72, 157));

            ColorList.Add(Color.FromArgb(255, 0, 160, 233));
            ColorList.Add(Color.FromArgb(255, 228, 0, 127));
            ColorList.Add(Color.FromArgb(255, 255, 241, 0));

            ColorList.Add(Color.FromArgb(255, 29, 32, 136));
            ColorList.Add(Color.FromArgb(255, 0, 153, 68));
            ColorList.Add(Color.FromArgb(255, 230, 0, 17));

            ColorList.Add(Color.FromArgb(255, 239, 234, 58));
            ColorList.Add(Color.FromArgb(255, 113, 199, 213));
            ColorList.Add(Color.FromArgb(255, 171, 83, 155));


        }

        public void InitBWList()//初始化灰度块的灰度值
        {
            BWList = new List<Color>();
            for (int i = 0; i < 22; i++)
            {
                BWList.Add(Color.FromArgb(255, Convert.ToByte(i * 12), Convert.ToByte(i * 12), Convert.ToByte(i * 12)));
            }
        }

        public void InitResoveList()//初始化分辨率测试线条的空间频率：pl/mm
        {
            ResoveList = new List<decimal>();
            decimal d = 1;
            for (int i = 0; i < 12; i++)
            {
                ResoveList.Add(d);
                d = d + 0.5m;
            }
        }

        public void InitWaveList()//初始化正弦曲线的空间频率：pl/mm
        {
            WaveList = new List<decimal>();
            WaveList.Add(0.05m);
            WaveList.Add(0.1m);
            WaveList.Add(0.3m);
            WaveList.Add(0.5m);
            WaveList.Add(0.7m);

        }

        //public List<decimal> getResoveList()//使用黑白线对的方式，得到分辨率
        //{
        //    WriteableBitmap subB;
        //    decimal mtf;
        //    List<decimal> mtfList = new List<decimal>();//加载各个mtf的列表，和每一个空间频率对应
        //    for (int i = 1; i < 12; i++)
        //    {
        //        subB = getAreaResove(this.CropPhoto, i);
        //        List<int> al;
        //        mtf = 0;
        //        for (int h = 0; h < subB.PixelHeight; h++)
        //        {
        //            al = getImageGrayHLine(subB, h);
        //            mtf = mtf + this.getMTF(al);
        //        }
        //        mtf = mtf / subB.PixelHeight;
        //        mtfList.Add(mtf);
                
        //    }
        //    return mtfList;
        //}

        public decimal getPixPerMM()//给出像素和毫米的等效关系
        {
            decimal x = Convert.ToDecimal(mp.CorrectPhoto.PixelWidth) / Convert.ToDecimal(TrueLongMM);
            return x;
        }

        public int getHalfWaveLength(List<int> al)//找到半波长，先找到第一个最亮的点，然后再找到
        {
            return 0;
        }

        public int getAverageMax(List<int> al, int HalfWaveLength)
        {
            return 0;
        }

        //public decimal getResove(float percent)//给出分辨率，percent是阀值
        //{//原始图像长20cm，空间频率从0.02-1，对数累进，不能分辨的时候，就是其极限值，最终转换为lp/pw

        //    List<decimal> al = getResoveList();
        //    int cp = 0;
        //    for (int i = 0; i < 21; i++)
        //    {
        //        decimal x = al[i];
        //        if (x < Convert.ToDecimal(percent))
        //        {
        //            cp = i - 1;
        //            break;
        //        }
        //    }
        //    decimal f = Convert.ToDecimal(ResoveList[cp]);//找出实际的空间频率
        //    decimal lpph = CorrectPhoto.PixelHeight * f / getPixPerMM() * 2;//给出的是线对，所以乘2
        //    return lpph;
        //}

        public void ProcBWValue()//分析记录实际的颜色值，把数值保存在ValueColorList里面
        {
            ValueBWList.Clear();
            WriteableBitmap subB = null;
            Color c;
            for (int i = 1; i < 23; i++)
            {
                subB = getAreaBW(i);
                c = getAverageColor(subB);
                ValueBWList.Add(c);
                //subB.Dispose();
            }
        }

        public void ProcColorValue()//分析记录实际的颜色值，把数值保存在ValueColorList里面
        {
            ValueColorList.Clear();
            WriteableBitmap subB = null;
            Color c;
            for (int i = 1; i < 19; i++)//仅仅测试7-18，前面6个是RGB的，后面才是CMYK模式的，表示了真实的色差
            {
                subB = getAreaColor(i);
                c = getAverageColor(subB);
                ValueColorList.Add(c);
                //subB.Dispose();
            }

        }

        public decimal getColorDis()//给出色差测试的结果
        {
            List<float> al = new List<float>();
            decimal x = 0;
            WriteableBitmap subB = null;
            Color c;
            for (int i = 7; i < 19; i++)//仅仅测试7-18，前面6个是RGB的，后面才是CMYK模式的，表示了真实的色差
            {

                subB = getAreaColor(i);
                c = getAverageColor(subB);
                Color c0 = (Color)ColorList[i - 1];
                float f = Math.Abs(PhotoTest.getHue(c0) - PhotoTest.getHue(c));//这里的色差仅仅看色相的差异，不管亮度和饱和度的差异
                al.Add(f);
                //subB.Dispose();
            }

            for (int i = 0; i < al.Count; i++)//求平均值
            {
                decimal dx=Convert.ToDecimal(al[i]);
                if(dx>180){//大于180度，就从色相环的另一个方向去绕更快
                    dx=360-dx;
                }
                x = x + dx;
            }
            x = x / al.Count;
            return x;
        }

        public decimal getNoiseNum()//噪点数量,阀值为30
        {
            return getNoiseNum(NoiseBrightChange);
        }
        public decimal getNoiseNum(int h)//噪点数量,h代表噪点异常的百分比，如果为0，默认是30
        {
            int x = 0;
            int pixNum = 0;
            WriteableBitmap subB = null;
            for (int i = 1; i < 22; i++)
            {
                subB = getAreaBW(i);
                x = x + ptp.getNoiseNum(subB, h / 100f);
                pixNum = pixNum + subB.PixelWidth * subB.PixelHeight;
                //subB.Dispose();
            }

            return Convert.ToDecimal(x) / Convert.ToDecimal(pixNum);

        }

        public decimal getWhiteBanlance()//白平衡
        {
            decimal x = 0;
            WriteableBitmap subB = null;
            for (int i = 1; i < 23; i++)
            {
                subB = getAreaBW(i);
                x = x + Convert.ToDecimal(getWriteBanlance(subB));
                //subB.Dispose();
            }

            x = x / 22;
            return x;

        }

        public float getVEdgeDispersiveness()//测试色散，在沿着边沿，看rgb的分离情况，这里的bm已经是包含边界的选取图
        //获取边沿内的最大色彩离散值，然后去比上边沿的整个宽度。
        //先计算每一列的最大色散，然后进行各个列的平均，然后除以整个边界的宽度。核心是找到那个边界，找到边界的跨度
        {
            WriteableBitmap bm = getAreaVEdge();
            float f = Convert.ToSingle(ptp.getEdgeDispersiveness(bm, false));//调用基类的对应方法
            //bm.Dispose();
            return f;
        }

        public float getHEdgeDispersiveness()//测试色散，在沿着边沿，看rgb的分离情况，这里的bm已经是包含边界的选取图
        //获取边沿内的最大色彩离散值，然后去比上边沿的整个宽度。
        //先计算每一列的最大色散，然后进行各个列的平均，然后除以整个边界的宽度。核心是找到那个边界，找到边界的跨度
        {
            WriteableBitmap bm = getAreaVEdge();
            float f = Convert.ToSingle(ptp.getEdgeDispersiveness(bm, true));//调用基类的对应方法
            //bm.Dispose();
            return f;
        }

        public List<double> getBWBrightList()//求出各个黑白块的平均亮度列表
        {
            List<double> al = new List<double>();//记录各个灰度色块的平均灰度值，动态范围就是看灰度值的数量
            WriteableBitmap subB = null;
            for (int i = 1; i < 23; i++)
            {
                subB = getAreaBW(i);
                double c = getAverageBright(subB);
                //double b = 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;//最终能否分辨的亮度等级还是使用rgb实现
                al.Add(c);
                //subB.Dispose();
            }
            return al;
        }

        public double getLatitude()//利用黑白块之间的平均级差作为标准
        {
            //ArrayList al = getBWBrightList();//记录各个灰度色块的平均灰度值，动态范围就是看灰度值的数量
            //decimal h = Convert.ToSingle(al[21]) - Convert.ToSingle(al[0]);
            //h = h / 21;//这个就是平均亮度差。

            return getLatitude(LatitudeBrightChange);//RGB亮度差异7以下可以认为无法识别，特别是在黑的地方
        }

        public double getLatitude(double h)//动态范围，获取灰度级别的平均值，看极差，极差大于一定程度就表示可以识别，默认h是表示级差
        {
            List<double> al = getBWBrightList();//记录各个灰度色块的平均灰度值，动态范围就是看灰度值的数量
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

        public override void CorrectChart()//矫正，设置内部的图像
        {
            if (ChartPhoto == null)
            {
                ChartPhoto = mp.SourcePhoto;
            }
            else
            {
                CorrectXMark(ChartPhoto);
            }
        }

        public WriteableBitmap CorrectXMark(WriteableBitmap b)//矫正，并且返回矫正以后的图像
        {//首先使用中心圆进行位置矫正
            //再检查上面的两个角圆的圆心，利用其来矫正角度
            //然后检查左右的对称性
            //然后利用中心园检查上下的对称
            //检查中心园的直径，建立像素和大小的关系

            Point CP = getCenterPoint(b, 100);//找到真正的图片中心点
            if (CP.X == Double.NaN || CP.Y == Double.NaN)
            {
                return null;
            }
            if (CP.X == 0 && CP.Y == 0)
            {
                return b;
            }
            int dx = Convert.ToInt32(CP.X - b.PixelWidth / 2);
            int dy = Convert.ToInt32(CP.Y - b.PixelHeight / 2);
            WriteableBitmap cb = MoveTrans(b, -dx, -dy);//对准操作
            mp.CorrectMovePhoto = WriteableBitmapHelper.Clone(cb);

            Point LTP, RTP, LBP;
            LTP = getLTPoint(cb);
            RTP = getRTPoint(cb);

            LTP = getTrueCircleCenter(cb, LTP);
            RTP = getTrueCircleCenter(cb, RTP);
            float arc = getArc(LTP, RTP);//得到倾斜的角度

            WriteableBitmap rb = RotateTrans(cb, -arc);//矫正角度
            //cb.Dispose();
            CorrectPhoto = SilverlightLFC.common.WriteableBitmapHelper.Clone(rb);//给出纠正角度的
            mp.CorrectRotatePhoto = SilverlightLFC.common.WriteableBitmapHelper.Clone(CorrectPhoto);
            //不进行缩放，而是矫正定位，让其他的方法定位的区域依据四角的标定点进行。
            //mp.CorrectScalePhoto = mp.CorrectRotatePhoto;

            LBP = getLBPoint(rb);
            LBP = getTrueCircleCenter(rb, LBP);

            int CropW = Convert.ToInt32(RTP.X - LTP.X);
            int CropH = Convert.ToInt32(LBP.Y - LTP.Y);

            WriteableBitmap CropB;
            CropB = getImageArea(rb,Convert.ToInt32(LTP.X),Convert.ToInt32(LTP.Y), CropW, CropH);
            //rb.Dispose();
            CropPhoto = SilverlightLFC.common.WriteableBitmapHelper.Clone(CropB);//在本对象记录下裁剪过的图像
            mp.SelectedPhoto = SilverlightLFC.common.WriteableBitmapHelper.Clone(CropB);
            mp.CroppedPhoto = WriteableBitmapHelper.Clone(CropB);
            return CropB;

        }

        public Point getTrueCircleCenter(WriteableBitmap b, Point CP)//得到一个标准圆的真正圆心
        {
            Point TCP = PhotoTest.NullPoint;
            int cpx = Convert.ToInt32(CP.X);
            int cpy = Convert.ToInt32(CP.Y);
            Color c0=new Color();
            byte[] components = b.PixelBuffer.ToArray();
            var index = cpx * 4 + (cpy * b.PixelWidth * 4);

            c0.B = components[index];
            c0.G = components[index+1];
            c0.R = components[index+2];

            int h = 120;
            if (IsWhite(c0, 150))//检验特定位置的颜色必须是白色
            {
                List<int> Hal, Val;

                Hal = getImageGrayHLine(b, cpy);
                Val = getImageGrayVLine(b, cpx);
                int tx = getBorderPointPosition(Hal, cpx, true, h);
                int tx1 = getBorderPointPosition(Hal, cpx, false, h);
                int tw = tx1 - tx;//给出两边越变点之间的距离
                int ty = getBorderPointPosition(Val, cpy, true, h);
                int ty1 = getBorderPointPosition(Val, cpy, false, h);
                int th = ty1 - ty;//给出两个越变点之间距离
                int cx = tx + tw / 2;
                int cy = ty + th / 2;
                TCP = new Point(cx, cy);//给出真正的中心点坐标

            }
            return TCP;
        }

        public decimal getAberration()//给出畸变数据，利用内部保持的整个照片
        {
            if (CorrectPhoto == null)
            {
                if (mp.SourcePhoto == null)
                {
                    return -1;
                }
                else
                {
                    ChartPhoto = mp.SourcePhoto;
                }
            }
            return getAberration(CorrectPhoto);//注意，这里不能使用裁剪的照片

        }

        public new decimal getAberration(WriteableBitmap b)//给出畸变数据，b为整个照片
        {//对正以后的照片，获取左右20%的宽度和高度区域进行分析，以加快测试的速度
            Point lsp = new Point(0, Convert.ToInt32(b.PixelHeight * 0.4));
            Point rsp = new Point(Convert.ToInt32(b.PixelWidth * 0.8), Convert.ToInt32(b.PixelHeight * 0.4));
            WriteableBitmap b0 = getImageArea(b,Convert.ToInt32(lsp.X),Convert.ToInt32(lsp.Y), Convert.ToInt32(b.PixelWidth * 0.2), Convert.ToInt32(b.PixelHeight * 0.2));
            WriteableBitmap b1 = getImageArea(b, Convert.ToInt32(rsp.X), Convert.ToInt32(rsp.Y), Convert.ToInt32(b.PixelWidth * 0.2), Convert.ToInt32(b.PixelHeight * 0.2));
            List<int> BorderList = new List<int>();
            bool IsOut;
            int lv = 0;
            int rv = 0;
            for (int i = 0; i < b0.PixelHeight; i++)
            {
                int sp = 0;
                List<int> al = getImageGrayHLine(b0, i);
                for (int j = 0; j < al.Count; j++)//需要忽略那些边界上面数值为0 的点，因为那是平移得到的补偿用的“假点”
                {
                    if (Convert.ToInt32(al[j]) != 0)
                    {
                        break;
                    }
                    else
                    {
                        sp++;
                    }
                }
                int m = getBorderPointPosition(al, sp, false, 100);
                BorderList.Add(m);
            }
            int CP = Convert.ToInt32(BorderList[BorderList.Count / 2]);
            if (CP > Convert.ToInt32(BorderList[0]))//表示中央的位置大，图像边界收缩
            {
                IsOut = false;
                int tmax = Convert.ToInt32(BorderList[0]);
                for (int i = 0; i < BorderList.Count; i++)
                {
                    if (tmax < Convert.ToInt32(BorderList[i]))
                    {
                        tmax = Convert.ToInt32(BorderList[i]);
                    }
                }
                lv = tmax;

            }
            else//表示图像外凸，需要取最小位置值
            {
                IsOut = true;
                int tmin = Convert.ToInt32(BorderList[0]);
                for (int i = 0; i < BorderList.Count; i++)
                {
                    if (tmin > Convert.ToInt32(BorderList[i]))
                    {
                        tmin = Convert.ToInt32(BorderList[i]);
                    }
                }
                lv = tmin;


            }
            BorderList.Clear();
            for (int i = 0; i < b1.PixelHeight; i++)
            {
                int sp = 0;
                List<int> al = getImageGrayHLine(b1, i);
                for (int j = al.Count - 1; j > -1; j--)
                {
                    if (Convert.ToInt32(al[j]) != 0)
                    {
                        break;
                    }
                    else
                    {
                        sp++;
                    }
                }
                int m = getBorderPointPosition(al, al.Count-sp-1, true, 100);//这里是从右向左扫描
                BorderList.Add(m);
            }
            if (IsOut)
            {
                int tmax = Convert.ToInt32(BorderList[0]);
                for (int i = 0; i < BorderList.Count; i++)
                {
                    if (tmax < Convert.ToInt32(BorderList[i]))
                    {
                        tmax = Convert.ToInt32(BorderList[i]);
                    }
                }
                rv = Convert.ToInt32(b.PixelWidth * 0.8 + tmax);
            }
            else
            {
                int tmin = Convert.ToInt32(BorderList[0]);
                for (int i = 0; i < BorderList.Count; i++)
                {
                    if (tmin > Convert.ToInt32(BorderList[i]))
                    {
                        tmin = Convert.ToInt32(BorderList[i]);
                    }

                }
                rv = Convert.ToInt32(b.PixelWidth * 0.8 + tmin);
            }
            //b0.Dispose();
            //b1.Dispose();
            decimal v = Convert.ToDecimal(rv - lv);
            Point leftCirclePoint, rightCirclePoint;
            leftCirclePoint = getCircleCenterPoint(b, Convert.ToInt32(1d / 24 * b.PixelWidth), Convert.ToInt32(1d / 18 * b.PixelHeight));
            rightCirclePoint = getCircleCenterPoint(b, Convert.ToInt32(23d / 24 * b.PixelWidth), Convert.ToInt32(1d / 18 * b.PixelHeight));
            setProcessInfor("AberrationLCP", leftCirclePoint.X);
            setProcessInfor("AberrationRCP", rightCirclePoint.X);
            setProcessInfor("AberrationLVP", lv);
            setProcessInfor("AberrationRVP", rv);
            decimal l = Convert.ToDecimal(rightCirclePoint.X - leftCirclePoint.X);
            return (v - l) / l;//膨胀的百分比，如果是负数，表示收缩
        }

        public int ConerSqrHalfBoder = 1;//测试卡的角块边长的一半
        public int TrueH = 18;//测试卡的实际高度
        public int TrueW = 24;//测试卡的实际宽度
        public Point getLTPoint(WriteableBitmap b)//得到图上的左上圆心
        {
            int x = Convert.ToInt32(b.PixelWidth * ConerSqrHalfBoder / TrueW);
            int y = Convert.ToInt32(b.PixelHeight * ConerSqrHalfBoder / TrueH);
            return new Point(x, y);
        }

        public Point getRTPoint(WriteableBitmap b)//得到图上的右上圆心
        {
            int x = Convert.ToInt32(b.PixelWidth * (TrueW - ConerSqrHalfBoder) / TrueW);
            int y = Convert.ToInt32(b.PixelHeight * ConerSqrHalfBoder / TrueH);
            return new Point(x, y);
        }

        public Point getLBPoint(WriteableBitmap b)//得到图上的左下圆心
        {
            int x = Convert.ToInt32(b.PixelWidth * ConerSqrHalfBoder / TrueW);
            int y = Convert.ToInt32(b.PixelHeight * (TrueH - ConerSqrHalfBoder) / TrueH);
            return new Point(x, y);
        }

        public Point getRBPoint(WriteableBitmap b)//得到图上的右下圆心
        {
            int x = Convert.ToInt32(b.PixelWidth * (TrueW - ConerSqrHalfBoder) / TrueW);
            int y = Convert.ToInt32(b.PixelHeight * (TrueH - ConerSqrHalfBoder) / TrueH);
            return new Point(x, y);
        }

        public void InitBWValue()
        {
            WriteableBitmap b;
            b = getAreaWhite(CropPhoto);
            Color c = getAverageColor(b);
            StandardWhiteValue = Convert.ToInt32(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);

            b = getAreaBlack(CropPhoto);
            c = getAverageColor(b);
            StandardBlackValue = Convert.ToInt32(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);

            //b.Dispose();
        }

        public void getAreaPurple(WriteableBitmap b)
        {
            WriteableBitmap sb = getImageArea(b, 0, 0, b.PixelWidth / 10, b.PixelHeight / 10);
            setSelectArea("AreaPurple_1", sb);

            sb = getImageArea(b, Convert.ToInt32(b.PixelWidth * 0.9), 0, b.PixelWidth / 10, b.PixelHeight / 10);
            setSelectArea("AreaPurple_2", sb);

            sb = getImageArea(b, 0, Convert.ToInt32(b.PixelHeight * 0.9), b.PixelWidth / 10, b.PixelHeight / 10);
            setSelectArea("AreaPurple_3", sb);

            sb = getImageArea(b, Convert.ToInt32(b.PixelWidth * 0.9), Convert.ToInt32(b.PixelHeight * 0.9), b.PixelWidth / 10, b.PixelHeight / 10);
            setSelectArea("AreaPurple_4", sb);

        }

        public WriteableBitmap getAreaWhite(WriteableBitmap b)//找出标准白色块，其实就是中央圆内部的一个很小区域
        {
            decimal l = 0.495m;
            decimal t = 0.495m;
            decimal w = 0.01m;
            decimal h = 0.01m;
            int ax, ay, ah, aw;

            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            ax = Convert.ToInt32(b.PixelWidth * l);
            ay = Convert.ToInt32(b.PixelHeight * t);

            WriteableBitmap subB;
            subB = this.getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("StandardWhite", subB);
            return subB;
        }

        public WriteableBitmap getAreaBlack(WriteableBitmap b)//找出标准黑色块，是水平黑色矩形的一个很小区域
        {
            if (b == null) { return null; }
            double l = 0.495;
            double t = 0.38;
            double w = 0.01;
            double h = 0.01;
            int ax, ay, ah, aw;

            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            ax = Convert.ToInt32(b.PixelWidth * l);
            ay = Convert.ToInt32(b.PixelHeight * t);

            WriteableBitmap subB;
            subB = this.getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("StandardBlack", subB);
            return subB;
        }

        public WriteableBitmap getAreaColor(int No)//使用内部保存的处理过的图像
        {

            if (CropPhoto == null)
            {
                CropPhoto = mp.SourcePhoto;
                //return null;
            }
            return getAreaColor(CropPhoto, No);
        }

        public WriteableBitmap getAreaColor(WriteableBitmap b, int No)//依据序列获取图片上面的色彩测试区，这里的图像是裁剪过的
        {
            if (b == null) { return null; }
            double Tw = 0.083;//两个色块之间的间距宽度
            double Th = 0.2;//两行之间的间距高度
            double l = 0.24;
            double t = 0.48;
            double w = 0.015;
            double h = 0.045;
            int ax, ay, ah, aw;
            ax = 0;
            ay = 0;
            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            if (No == 9)
            {
                ax = Convert.ToInt32(b.PixelWidth * 0.89);
                ay = Convert.ToInt32(b.PixelHeight * 0.33);
            }
            if ((No < 1) || (No > 18))//表示不合法
            {
                return null;
            }
            if ((No > 0) && (No < 4))//1--3个色块
            {
                ax = Convert.ToInt32(b.PixelWidth * (l + (No - 1) * Tw));
                ay = Convert.ToInt32(b.PixelHeight * t);
            }
            if ((No > 3) && (No < 9))
            {
                ax = Convert.ToInt32(b.PixelWidth * (l + No * Tw));
                ay = Convert.ToInt32(b.PixelHeight * t);
            }

            if ((No > 9) && (No < 19))
            {
                ax = Convert.ToInt32(b.PixelWidth * (l + (No - 10) * Tw));
                ay = Convert.ToInt32(b.PixelHeight * (t + Th));
            }
            WriteableBitmap subB;
            subB = this.getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("AreaColor_"+No.ToString(), subB);
            return subB;
        }

        public WriteableBitmap getAreaWave(int No)//使用内部保存的处理过的图像
        {
            if (CropPhoto == null)
            {
                CropPhoto = mp.SourcePhoto;
            }
            return getAreaWave(CropPhoto, No);
        }

        public WriteableBitmap getAreaWave(WriteableBitmap b, int No)//依据序号获取正弦波区块，注意这里的bitmap是裁剪过的
        {
            if (b == null) { return null; }
            double Tw = 0.16;
            double l = 0.1;
            double t = 0.188;
            double w = 0.16;
            double h = 0.04;
            int ax, ay, ah, aw;
            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            if ((No < 1) || (No > 5))
            {
                return null;
            }
            int seq = No;//这是为了保证是按照序号的增序，得到频率更高的部分
            if (No == 2)
            {
                seq = 5;
            }
            if (No == 3)
            {
                seq = 2;
            }
            if (No == 5)
            {
                seq = 3;
            }
            ax = Convert.ToInt32(b.PixelWidth * (l + (seq - 1) * Tw));
            ay = Convert.ToInt32(b.PixelHeight * t);

            WriteableBitmap subB;
            subB = this.getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("AreaWave_" + No.ToString(), subB);
            return subB;

        }

        //public WriteableBitmap getAreaResove(int No)
        //{
        //    if (CropPhoto == null)
        //    {
        //        return null;
        //    }
        //    return getAreaResove(CropPhoto, No);

        //}

        //public WriteableBitmap getAreaResove(WriteableBitmap b, int No)//获取测试分辨率的区域
        //{
        //    double l1 = 0.2;
        //    double l2 = 0.61;
        //    double t1 = 0.3;
        //    double t2 = 0.39;

        //    double w = 0.07;
        //    double h = 0.03;
        //    int ax, ay, ah, aw;
        //    ax = 0;
        //    ay = 0;
        //    aw = Convert.ToInt32(b.PixelWidth * w);
        //    ah = Convert.ToInt32(b.PixelHeight * h);
        //    if ((No < 1) || (No > 12))
        //    {
        //        return null;
        //    }
        //    int seq = No;
        //    if (No == 2)
        //    {
        //        seq = 7;
        //    }
        //    if (No == 3)
        //    {
        //        seq = 6;
        //    }
        //    if (No == 4)
        //    {
        //        seq = 12;
        //    }
        //    if (No == 5)
        //    {
        //        seq = 2;
        //    }
        //    if (No == 6)
        //    {
        //        seq = 8;
        //    }
        //    if (No == 7)
        //    {
        //        seq = 5;
        //    }
        //    if (No == 8)
        //    {
        //        seq = 11;
        //    }
        //    if (No == 9)
        //    {
        //        seq = 3;
        //    }
        //    if (No == 10)
        //    {
        //        seq = 9;
        //    }
        //    if (No == 11)
        //    {
        //        seq = 4;
        //    }
        //    if (No == 12)
        //    {
        //        seq = 10;
        //    }
        //    if (seq < 4)
        //    {
        //        ax = Convert.ToInt32(l1 * b.PixelWidth + (seq - 1) * aw);
        //        ay = Convert.ToInt32(t1 * b.PixelHeight);
        //    }

        //    if ((seq > 3) && (seq < 7))
        //    {
        //        ax = Convert.ToInt32(l2 * b.PixelWidth + (seq - 4) * aw);
        //        ay = Convert.ToInt32(t1 * b.PixelHeight);
        //    }

        //    if ((seq > 6) && (seq < 10))
        //    {
        //        ax = Convert.ToInt32(l1 * b.PixelWidth + (seq - 7) * aw);
        //        ay = Convert.ToInt32(t2 * b.PixelHeight);
        //    }

        //    if (seq > 9)
        //    {
        //        ax = Convert.ToInt32(l2 * b.PixelWidth + (seq - 10) * aw);
        //        ay = Convert.ToInt32(t2 * b.PixelHeight);
        //    }
        //    WriteableBitmap subB;
        //    subB = this.getImageArea(b,ax, ay, aw, ah);
        //    if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
        //    return subB;
        //}

        public WriteableBitmap getAreaBW(int No)//使用内部保存的处理过的图像
        {
            if (CropPhoto == null)
            {
                CropPhoto = mp.SourcePhoto;
            }
            return getAreaBW(CropPhoto, No);
        }

        public WriteableBitmap getAreaBW(WriteableBitmap b, int No)//依据序号获取灰度区块
        {
            if (b == null) { return null; }
            double Tw = 0.0423;

            double l = 0.038;
            double t = 0.81;
            double w = 0.016;
            double h = 0.07;
            int ax, ay, ah, aw;

            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            if ((No < 1) || (No > 22))
            {
                return null;
            }

            ax = Convert.ToInt32(b.PixelWidth * (l + (No - 1) * Tw));
            ay = Convert.ToInt32(b.PixelHeight * t);

            WriteableBitmap subB;
            subB = this.getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("AreaBW_" + No.ToString(), subB);
            return subB;

        }

        public WriteableBitmap getAreaVEdge()
        {
            if (CropPhoto == null)
            {
                CropPhoto = mp.SourcePhoto;
            }
            return getAreaVEdge(CropPhoto);
        }

        public WriteableBitmap getAreaHEdge()
        {
            if (CropPhoto == null)
            {
                CropPhoto = mp.SourcePhoto;
            }
            return getAreaHEdge(CropPhoto);
        }

        public WriteableBitmap getAreaHEdge(WriteableBitmap b)//获取边界水平色散、分辨率测试区
        {
            if (b == null) { return null; }
            double l = 0.47;
            double t = 0.3;
            double w = 0.06;
            double h = 0.09;
            int ax, ay, ah, aw;

            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            ax = Convert.ToInt32(b.PixelWidth * l);
            ay = Convert.ToInt32(b.PixelHeight * t);

            WriteableBitmap subB;
            subB = this.getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("AreaHEdge", subB);
            return subB;
        }

        public WriteableBitmap getAreaVEdge(WriteableBitmap b)//获取边界垂直分辨率、色散测试区
        {
            if (b == null) { return null; }
            double l = 0.09;
            double t = 0.45;
            double w = 0.055;
            double h = 0.11;
            int ax, ay, ah, aw;

            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            ax = Convert.ToInt32(b.PixelWidth * l);
            ay = Convert.ToInt32(b.PixelHeight * t);

            WriteableBitmap subB;
            subB = getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("AreaVEdge", subB);
            return subB;
        }

        public WriteableBitmap getAreaRainbow()
        {
            if (CropPhoto == null)
            {
                CropPhoto = mp.SourcePhoto;
            }
            return getAreaRainbow(CropPhoto);

        }

        public WriteableBitmap getAreaRainbow(WriteableBitmap b)//获取彩虹色彩区
        {
            if (b == null) { return null; }
            double l = 0.18;
            double t = 0.595;
            double w = 0.36;
            double h = 0.025;
            int ax, ay, ah, aw;
            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            ax = Convert.ToInt32(b.PixelWidth * l);
            ay = Convert.ToInt32(b.PixelHeight * t);

            WriteableBitmap subB;
            subB = this.getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("AreaRainbow", subB);
            return subB;
        }

        public WriteableBitmap getAreaVBright()
        {
            if (CropPhoto == null)
            {
                CropPhoto = mp.SourcePhoto;
            }
            return getAreaVBright(CropPhoto);

        }

        public WriteableBitmap getAreaVBright(WriteableBitmap b)
        {
            if (b == null) { return null; }
            double l = 0.965;
            double t = 0.14;
            double w = 0.01;
            double h = 0.47;
            int ax, ay, ah, aw;
            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            ax = Convert.ToInt32(b.PixelWidth * l);
            ay = Convert.ToInt32(b.PixelHeight * t);

            WriteableBitmap subB;
            subB = getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("AreaVBright", subB);
            return subB;
        }

        public WriteableBitmap getAreaHBright()
        {
            if (CropPhoto == null)
            {
                CropPhoto = mp.SourcePhoto;
            }
            return getAreaHBright(CropPhoto);

        }

        public WriteableBitmap getAreaHBright(WriteableBitmap b)
        {
            if (b == null) { return null; }
            double l = 0.553;
            double t = 0.595;
            double w = 0.42;
            double h = 0.01;
            int ax, ay, ah, aw;
            aw = Convert.ToInt32(b.PixelWidth * w);
            ah = Convert.ToInt32(b.PixelHeight * h);
            ax = Convert.ToInt32(b.PixelWidth * l);
            ay = Convert.ToInt32(b.PixelHeight * t);

            WriteableBitmap subB;
            subB = this.getImageArea(b,ax, ay, aw, ah);
            if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
            setSelectArea("AreaHBright", subB);
            return subB;
        }

        //public WriteableBitmap getAreaBlackCC()
        //{
        //    if (CropPhoto == null)
        //    {
        //        return null;
        //    }
        //    return getAreaBlackCC(CropPhoto);

        //}

        //public WriteableBitmap getAreaBlackCC(WriteableBitmap b)
        //{
        //    double l = 0.462;
        //    double t = 0.369;
        //    double w = 0.06;
        //    double h = 0.087;
        //    int ax, ay, ah, aw;
        //    aw = Convert.ToInt32(b.PixelWidth * w);
        //    ah = Convert.ToInt32(b.PixelHeight * h);
        //    ax = Convert.ToInt32(b.PixelWidth * l);
        //    ay = Convert.ToInt32(b.PixelHeight * t);

        //    WriteableBitmap subB;
        //    subB = this.getImageArea(b,ax, ay, aw, ah);
        //    if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
        //    return subB;
        //}

        //public WriteableBitmap getAreaWhiteCC()
        //{
        //    if (CropPhoto == null)
        //    {
        //        return null;
        //    }
        //    return getAreaWhiteCC(CropPhoto);

        //}

        //public WriteableBitmap getAreaWhiteCC(WriteableBitmap b)
        //{
        //    double l = 0.47;
        //    double t = 0.456;
        //    double w = 0.06;
        //    double h = 0.09;
        //    int ax, ay, ah, aw;
        //    aw = Convert.ToInt32(b.PixelWidth * w);
        //    ah = Convert.ToInt32(b.PixelHeight * h);
        //    ax = Convert.ToInt32(b.PixelWidth * l);
        //    ay = Convert.ToInt32(b.PixelHeight * t);

        //    WriteableBitmap subB;
        //    subB = this.getImageArea(b,ax, ay, aw, ah);
        //    if (IsAnalyse) { DrawSelectArea(new Point(ax, ay), aw, ah); }
        //    return subB;
        //}

        public decimal getBrightChanges()//测试亮度一致性
        {
            return getBrightChanges(CropPhoto);
        }

        public decimal getBrightChanges(WriteableBitmap b)//测试亮度一致性
        {//使用中央点的亮度减去四角点的亮度平均值，需要使用CropPhoto
            Color c = GetPixel(b, b.PixelWidth / 2, b.PixelHeight / 2);
            Color clt = GetPixel(b,0, 0);
            Color crt = GetPixel(b,b.PixelWidth - 1, 0);
            Color clb = GetPixel(b,0, b.PixelHeight - 1);
            Color crb = GetPixel(b,b.PixelWidth - 1, b.PixelHeight - 1);
            float lt = PhotoTest.getBrightness(clt);
            float lb = PhotoTest.getBrightness(clb);
            float rt = PhotoTest.getBrightness(crt);
            float rb = PhotoTest.getBrightness(crb);
            float bb = lt + lb + rt + rb;
            bb = bb / 4;
            setProcessInfor("BrightChanges_BorderBright", bb);
            setProcessInfor("BrightChanges_LT", lt);
            setProcessInfor("BrightChanges_LB", lb);
            setProcessInfor("BrightChanges_RT", rt);
            setProcessInfor("BrightChanges_RB", rb);
            float cb = PhotoTest.getBrightness(c);
            setProcessInfor("BrightChanges_CBright", cb);
            return Convert.ToDecimal(Math.Abs( cb- bb) /cb);//相当于四周变暗的比例
        }

        public int getHEdgeResoveLines()//得到中央分辨率
        {
            WriteableBitmap b = getAreaHEdge();
            int x = ptp.getEdgeResoveLines(b, true, ChartPhoto.PixelHeight);
            return x;
        }
        public int getVEdgeResoveLines()//得到边沿分辨率
        {
            WriteableBitmap b = getAreaVEdge();
            int x = ptp.getEdgeResoveLines(b, false, ChartPhoto.PixelHeight);
            return x;
        }

        public static int getHalfWaveCountByIndex(int i)
        {
            if (i == 1) { return 1; }
            if (i == 2) { return 3; }
            if (i == 3) { return 9; }
            if (i == 4) { return 15; }
            if (i == 5) { return 21; }
            return -1;
        }
        public decimal getWaveQ()//得到正弦波型的质量，关键是得到正弦数列
        {
            WriteableBitmap b = getAreaWave(1);
            decimal wq = ptp.getWaveQ(b, 1);

            b = getAreaWave(2);
            wq = wq + ptp.getWaveQ(b, 3);

            b = getAreaWave(3);
            wq = wq + ptp.getWaveQ(b, 9);

            b = getAreaWave(4);
            wq = wq + ptp.getWaveQ(b, 15);

            b = getAreaWave(5);
            wq = wq + ptp.getWaveQ(b, 21);

            wq = wq / 5;
            return wq;
        }


        public List<double> getCurveMTF()//使用内部的保持图片来提供XMark照片的mtf曲线列表
        {
            WriteableBitmap b = getAreaHEdge();//这里找到的是水平的边界，因此裁剪的时候垂直进行
            return ptp.getCurveMTF(b,true);
        }

        //public List<List<int>> getCurveVDispersiveness()//色散的曲线包括了三条，RGB，每一个是个列表，他们合起来也是一个新的列表
        //{
        //    WriteableBitmap b = getAreaVEdge();//这里找到的是垂直的边界，色散使用边上的那个进行测试
        //    List<List<int>> al = new List<List<int>>();
        //    al.Add(getRedVEdge(b));
        //    al.Add(getGreenVEdge(b));
        //    al.Add(getBlueVEdge(b));
        //    return al;
        //}



        public List<List<Color>> getCurveColorDis()//形成一个由小的理论值、实际值对组成的大列表
        {
            ProcColorValue();
            List<List<Color>> Val = new List<List<Color>>();
            for (int i = 6; i < 18; i++)
            {
                List<Color> ColorPare = new List<Color>();
                ColorPare.Add(ColorList[i]);
                ColorPare.Add(ValueColorList[i]);
                Val.Add(ColorPare);
            }
            return Val;

        }

        public List<decimal> getCurveLatitude()//给出实际的黑白区域的亮度变化
        {
            ProcBWValue();
            List<decimal> al = new List<decimal>();
            for (int i = 0; i < ValueBWList.Count; i++)
            {
                decimal d = Convert.ToDecimal(PhotoTest.getBrightness(ValueBWList[i]) * 255);
                al.Add(d);
            }

            return al;
        }

        public List<List<int>> getCurveBrightnessChange()//给出白平衡的变化曲线
        {
            List<int> hal, val;
            List<List<int>> al;
            WriteableBitmap subB = getAreaHBright();
            hal = getImageGrayHLine(subB, subB.PixelHeight / 2);
            subB = getAreaVBright();
            val = getImageGrayVLine(subB, subB.PixelWidth / 2);
            al = new List<List<int>>();
            al.Add(hal);
            al.Add(val);
            return al;
        }

        public List<Color> getCurveWhiteBalance()//给出黑白块的色彩处理结果，经过换算就可以得到白平衡
        {
            ProcBWValue();
            return ValueBWList;
        }

        public List<List<decimal>> getCurveWaveQ()//得到一个表示测试卡波型的亮度变化曲线，给出的是依据顺序由低到高的频率
        {
            List<List<decimal>> al = new List<List<decimal>>();
            WriteableBitmap b;
            List<decimal> Sal;
            List<decimal> Val;
            int WaveSwing;
            int minValue;

            b = getAreaWave(1);
            Val = getDecimalList( ptp.getHalfWaveList(b, 1));
            WaveSwing = Convert.ToInt32(Val[0]) - Convert.ToInt32(Val[Val.Count - 1]);
            minValue = Convert.ToInt32(Val[Val.Count - 1]);
            Sal = ptp.getAdjustWaveValueList(Val.Count, WaveSwing,minValue);
            al.Add(Sal);
            al.Add(Val);

            b = getAreaWave(2);
            Val = getDecimalList(ptp.getHalfWaveList(b, 3));
            WaveSwing = Convert.ToInt32(Val[0]) - Convert.ToInt32(Val[Val.Count - 1]);
            minValue = Convert.ToInt32(Val[Val.Count - 1]);
            Sal = ptp.getAdjustWaveValueList(Val.Count, WaveSwing, minValue);
            al.Add(Sal);
            al.Add(Val);

            b = getAreaWave(3);
            Val = getDecimalList(ptp.getHalfWaveList(b, 9));
            WaveSwing = Convert.ToInt32(Val[0]) - Convert.ToInt32(Val[Val.Count - 1]);
            minValue = Convert.ToInt32(Val[Val.Count - 1]);
            Sal = ptp.getAdjustWaveValueList(Val.Count, WaveSwing, minValue);
            al.Add(Sal);
            al.Add(Val);

            b = getAreaWave(4);
            Val = getDecimalList(ptp.getHalfWaveList(b, 15));
            WaveSwing = Convert.ToInt32(Val[0]) - Convert.ToInt32(Val[Val.Count - 1]);
            minValue = Convert.ToInt32(Val[Val.Count - 1]);
            Sal = ptp.getAdjustWaveValueList(Val.Count, WaveSwing, minValue);
            al.Add(Sal);
            al.Add(Val);

            b = getAreaWave(5);
            Val = getDecimalList(ptp.getHalfWaveList(b, 21));
            WaveSwing = Convert.ToInt32(Val[0]) - Convert.ToInt32(Val[Val.Count - 1]);
            minValue = Convert.ToInt32(Val[Val.Count - 1]);
            Sal = ptp.getAdjustWaveValueList(Val.Count, WaveSwing, minValue);
            al.Add(Sal);
            al.Add(Val);

            return al;
        }

        public List<Color> getCurveRainbow()
        {
            WriteableBitmap b = getAreaRainbow();
            List<Color> al = getImageColorHLine(b, b.PixelHeight / 2);
            TrimColorList(al,5,true,(Color)al[0]);
            return al;
        }

        public int NoiseSwing//设置噪点的参数
        {
            get
            {
                return NoiseBrightChange; ;
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

        public int ResoveType
        {
            get
            {
                return MTFPercenr; ;
            }
            set
            {
                if ((value > 0) && (value < 100))
                {
                    MTFPercenr = value;
                }
                else
                {
                    MTFPercenr = 50;
                }
            }
        }

        public int StepSwing
        {
            get
            {
                return LatitudeBrightChange;
            }
            set
            {
                if ((value > 0) && (value < 100))
                {
                    LatitudeBrightChange = value;
                }
                else
                {
                    LatitudeBrightChange = 7;
                }
            }
        }

        public double getPurplePercent()
        {
            if (ChartPhoto == null)
            {
                return -1;
            }
            return getPurplePercent(ChartPhoto);
        }

        public double getPurplePercent(WriteableBitmap b)
        {
            double d;
            WriteableBitmap sb = getImageArea(b, 0, 0, b.PixelWidth / 10, b.PixelHeight / 10);
            d = getPurpleEdge(sb, 15);
            setSelectArea("AreaPurple_1", sb);

            sb = getImageArea(b, Convert.ToInt32(b.PixelWidth*0.9), 0, b.PixelWidth / 10, b.PixelHeight / 10);
            d = d + getPurpleEdge(sb, 15);
            setSelectArea("AreaPurple_2", sb);

            sb = getImageArea(b, 0, Convert.ToInt32(b.PixelHeight*0.9), b.PixelWidth / 10, b.PixelHeight / 10);
            d = d + getPurpleEdge(sb, 15);
            setSelectArea("AreaPurple_3", sb);

            sb = getImageArea(b, Convert.ToInt32(b.PixelWidth * 0.9), Convert.ToInt32(b.PixelHeight * 0.9), b.PixelWidth / 10, b.PixelHeight / 10);
            d = d + getPurpleEdge(sb, 15);
            setSelectArea("AreaPurple_4", sb);

            return d / 4;
        }


    }
}
