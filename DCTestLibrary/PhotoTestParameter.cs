using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
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
using System.Runtime.InteropServices.WindowsRuntime;

namespace DCTestLibrary
{
    public class PhotoTestParameter:PhotoTest//单一指标测试，所有通过图片进行的测试
    {
        #region Dispersiveness//边沿色散测试
        public decimal getEdgeDispersiveness(WriteableBitmap b, bool IsH)//得到边界的色散程度
        {

            List<int> al;
            List<int> ral, gal, bal;

            if (IsH)
            {
                al = getImageGrayVLine(b, b.PixelWidth / 2);
                ral = getImageRedVLine(b, b.PixelWidth / 2);
                gal = getImageGreenVLine(b, b.PixelWidth / 2);
                bal = getImageBlueVLine(b, b.PixelWidth / 2);
            }
            else
            {
                al = getImageGrayHLine(b, b.PixelHeight / 2);
                ral = getImageRedHLine(b, b.PixelHeight / 2);
                gal = getImageGreenHLine(b, b.PixelHeight / 2);
                bal = getImageBlueHLine(b, b.PixelHeight / 2);


            }
            if (Convert.ToInt32(al[0]) > Convert.ToInt32(al[al.Count - 1]))
            {
                al = ReverseList(al);
                ral = ReverseList(ral);
                gal = ReverseList(gal);
                bal = ReverseList(bal);

            }
            List<long> pl = getEdgePosition(al);//得到边界的起至位置，第一个是开始，第二个是终止
            int SP, EP;
            SP = Convert.ToInt32(pl[0]);
            EP = Convert.ToInt32(pl[1]);



            ral.RemoveRange(EP + 1, ral.Count - EP - 1);
            gal.RemoveRange(EP + 1, ral.Count - EP - 1);
            bal.RemoveRange(EP + 1, ral.Count - EP - 1);

            ral.RemoveRange(0, SP);
            gal.RemoveRange(0, SP);
            bal.RemoveRange(0, SP);

            int red, green, blue;
            decimal MaxDis = 0;
            for (int i = 0; i < ral.Count; i++)
            {
                red = Convert.ToInt32(ral[i]);
                green = Convert.ToInt32(gal[i]);
                blue = Convert.ToInt32(bal[i]);
                MaxDis = MaxDis + getRGBMaxDis(red, green, blue);

            }
            decimal edgeSwing = getListSwing(al);
            if (edgeSwing <0.1m) { return 0; }//没有亮度变化的范围，因此也就无所谓色散程度，直接返回0
            MaxDis = MaxDis / edgeSwing / ral.Count;//计算的是平均每个像素上面的最大分离程度
            al = null;
            ral = null;
            gal = null;
            bal = null;
            return MaxDis;
        }

        public List<List<int>> getCurveHDispersiveness(WriteableBitmap b)//色散的曲线包括了三条，RGB，每一个是个列表，他们合起来也是一个新的列表
        {
            List<List<int>> al = new List<List<int>>();
            al.Add(getRedHEdge(b));
            al.Add(getGreenHEdge(b));
            al.Add(getBlueHEdge(b));
            return al;
        }

        public List<List<int>> getCurveVDispersiveness(WriteableBitmap b)//色散的曲线包括了三条，RGB，每一个是个列表，他们合起来也是一个新的列表
        {
            List<List<int>> al = new List<List<int>>();
            al.Add(getRedVEdge(b));
            al.Add(getGreenVEdge(b));
            al.Add(getBlueVEdge(b));
            return al;
        }

        public List<List<int>> getCurveHDispersiveness(WriteableBitmap b,int p)//色散的曲线包括了三条，RGB，每一个是个列表，他们合起来也是一个新的列表
        {
            List<List<int>> al = new List<List<int>>();
            al.Add(getRedHEdge(b,p));
            al.Add(getGreenHEdge(b,p));
            al.Add(getBlueHEdge(b,p));
            return al;
        }

        public List<List<int>> getCurveVDispersiveness(WriteableBitmap b,int p)//色散的曲线包括了三条，RGB，每一个是个列表，他们合起来也是一个新的列表
        {
            List<List<int>> al = new List<List<int>>();
            al.Add(getRedVEdge(b,p));
            al.Add(getGreenVEdge(b,p));
            al.Add(getBlueVEdge(b,p));
            return al;
        }

        
        #endregion

        #region EdgeResoveLines//边沿分辨率测试

        int StopFrequency = 50;
        public void setEdgeResoveStopFrequency(int StopFrq)
        {
            StopFrequency = StopFrq;
        }

        public int getEdgeResoveLines(WriteableBitmap b, bool IsH, int TotleHeight)//输入的包含单个黑白边沿的图片，得出分辨率
        //IsH表示是不是水平分辨率，垂直的线是水平分辨率，否则就不是
        {
            List<int> al;
            List<double> MTFList;
            int line = 0;
            decimal rev = 0;
            decimal tr = 0;
            int c = 0;
            if (IsH)
            {
                line = Convert.ToInt32(0.3 * b.PixelWidth);
                al = getImageGrayVLine(b, line);
            }
            else
            {
                line = Convert.ToInt32(0.3 * b.PixelHeight);
                al = getImageGrayHLine(b, line);
            }
            MTFList = getMTFList(al);
            tr = Convert.ToDecimal(getMTFValue(MTFList, StopFrequency));
            if (tr != -1.0m)//表示数据有效
            {
                rev = rev + tr;
                c++;
            }


            if (IsH)
            {
                line = Convert.ToInt32(0.5 * b.PixelWidth);
                al = getImageGrayVLine(b, line);
            }
            else
            {
                line = Convert.ToInt32(0.5 * b.PixelHeight);
                al = getImageGrayHLine(b, line);
            }
            MTFList = getMTFList(al);
            tr = Convert.ToDecimal(getMTFValue(MTFList, StopFrequency));

            if (tr != -1.0m)//表示数据有效
            {
                rev = rev + tr;
                c++;
            }


            if (IsH)
            {
                line = Convert.ToInt32(0.7 * b.PixelWidth);
                al = getImageGrayVLine(b, line);
            }
            else
            {
                line = Convert.ToInt32(0.7 * b.PixelHeight);
                al = getImageGrayHLine(b, line);
            }
            MTFList = getMTFList(al);
            tr = Convert.ToDecimal(getMTFValue(MTFList, StopFrequency));

            if (tr != -1.0m)//表示数据有效
            {
                rev = rev + tr;
                c++;
            }

            if (c == 0) { return -1; }
            rev = rev / c;//求平均值
            int lwpph;
            lwpph = Convert.ToInt32(rev * 2 * TotleHeight);
            al = null;
            MTFList = null;
            return lwpph;
        }

        public double getEdgeResoveEffect(WriteableBitmap b, bool IsH)//输入的包含单个黑白边沿的图片，得出分辨率
        //IsH表示是不是水平分辨率，垂直的线是水平分辨率，否则就不是
        {
            List<int> al;
            List<double> MTFList;
            int line = 0;
            double rev = 0;
            double tr = 0;
            int c = 0;
            if (IsH)
            {
                line = Convert.ToInt32(0.3 * b.PixelWidth);
                al = getImageGrayVLine(b, line);
            }
            else
            {
                line = Convert.ToInt32(0.3 * b.PixelHeight);
                al = getImageGrayHLine(b, line);
            }
            MTFList = getMTFList(al);
            tr = getMTFValue(MTFList, StopFrequency);
            if (tr != -1.0)//表示数据有效
            {
                rev = rev + tr;
                c++;
            }


            if (IsH)
            {
                line = Convert.ToInt32(0.5 * b.PixelWidth);
                al = getImageGrayVLine(b, line);
            }
            else
            {
                line = Convert.ToInt32(0.5 * b.PixelHeight);
                al = getImageGrayHLine(b, line);
            }
            MTFList = getMTFList(al);
            tr = getMTFValue(MTFList, StopFrequency);

            if (tr != -1.0)//表示数据有效
            {
                rev = rev + tr;
                c++;
            }


            if (IsH)
            {
                line = Convert.ToInt32(0.7 * b.PixelWidth);
                al = getImageGrayVLine(b, line);
            }
            else
            {
                line = Convert.ToInt32(0.7 * b.PixelHeight);
                al = getImageGrayHLine(b, line);
            }
            MTFList = getMTFList(al);
            tr = getMTFValue(MTFList, StopFrequency);

            if (tr != -1.0)//表示数据有效
            {
                rev = rev + tr;
                c++;
            }

            if (c == 0) { return -1; }
            rev = rev / c;//求平均值
            al = null;
            MTFList = null;
            return rev*2;
        }

        public double getEdgeResoveEffect(WriteableBitmap b, bool IsH,double Position)//输入的包含单个黑白边沿的图片，得出分辨率
        //IsH表示是不是水平分辨率，垂直的线是水平分辨率，否则就不是，Position表示选取线的位置百分比
        {
            List<int> al;
            List<double> MTFList;
            int line = 0;
            double rev = 0;
            double tr = 0;
            if (IsH)
            {
                line = Convert.ToInt32(Position * b.PixelWidth);
                al = getImageGrayVLine(b, line);
            }
            else
            {
                line = Convert.ToInt32(Position * b.PixelHeight);
                al = getImageGrayHLine(b, line);
            }
            MTFList = getMTFList(al);
            tr = getMTFValue(MTFList, StopFrequency);
            if (tr != -1.0)//表示数据有效
            {
                rev = rev + tr;
            }
            al = null;
            MTFList = null;
            return rev * 2;
        }


        protected List<double> getMTFList(List<int> al)//得到一个序列，标识MTF
        {
            if (Convert.ToInt32(al[0]) > Convert.ToInt32(al[al.Count - 1]))
            {
                al = ReverseList(al);
            }
            List<int> el = ProcEdgeList(al);//处理边界,利用其是单调增的函数
            List<decimal> LSF = getDdx(el);//获得线扩散函数
            int listlong = LSF.Count;
            double[] xr = new double[LSF.Count];

            double[] xi = new double[LSF.Count];
            for (int i = 0; i < LSF.Count; i++)
            {
                xr[i] = Convert.ToDouble(LSF[i]);
            }
            int n = 0;
            n = getMaxHz(listlong);
            FFT(xr, xi, n);
            List<double> tal = new List<double>();
            for (int i = 0; i < xr.Length / 2; i++)//MTF只有前面的一半有意义
            {
                double r, j;
                r = xr[i];
                j = xi[i];
                tal.Add(getComplexABS(r, j));
            }
            double d;
            d = tal[0];
            List<double> rl = new List<double>();
            for (int i = 0; i < tal.Count; i++)//归一化
            {
                rl.Add(tal[i] / d);
            }
            el = null;
            LSF = null;
            return rl;
        }


        protected double getMTFValue(List<double> al, int RV)//得出cy/pxl在MTF特定Value时候的数值
        {
            double d;
            int x = 0;
            double standard = RV / 100d;
            for (int i = 0; i < al.Count; i++)
            {
                d = al[i];
                if (d < standard)
                {
                    x = i;
                    break;
                }
            }
            if (x == 0)
            {
                return -1;//表示测试失败，开始的时候到最后，mtf数值都在0.5以上
            }
            return ((al[x - 1] - standard) / (al[x - 1] - al[x]) + x - 1) / al.Count;
        }

        public List<double> getCurveMTF(WriteableBitmap b, bool IsH)//依据已经裁切完毕的图片获得mtflist,IsH表示是不是图片包含水平的边界
        {
            List<int> Val = new List<int>();
            if (IsH)
            {
                Val = getImageGrayVLine(b, b.PixelWidth / 2);
            }
            else
            {
                Val = getImageGrayHLine(b, b.PixelHeight / 2);
            }

            List<double> dVal = getMTFList(Val);
            List<double> sal = SilverlightLFC.common.Environment.getDoubleList<decimal>(getSmoothList(dVal));
            dVal = null;
            return sal;
        }

        public List<double> getCurveMTF(WriteableBitmap b, bool IsH,double PointPercent)//依据已经裁切完毕的图片获得mtflist,IsH表示是不是图片包含水平的边界
        {
            List<int> Val = new List<int>();
            if (IsH)
            {
                Val = getImageGrayVLine(b, Convert.ToInt32(b.PixelWidth * PointPercent));
            }
            else
            {
                Val = getImageGrayHLine(b, Convert.ToInt32(b.PixelHeight * PointPercent));
            }

            List<double> dVal = getMTFList(Val);
            List<double> sal = SilverlightLFC.common.Environment.getDoubleList<decimal>(getSmoothList(dVal));
            dVal = null;
            return sal;
        }

        #endregion

        #region RayleiResolution

        public long getRayleiPosition(WriteableBitmap b,double Percent)
        {
            if (Percent < 0.01) { Percent = 0.735; }
            List<int> al;
            long r = 0;
            long left, right = 0;
            int top = 0, bottom = 0;//上下的限制边界，由前一个决定，这样可以避免外侧异常点的干扰
            left = getLeftPoint(b);
            right = getRightPoint(b);

            int ld = Convert.ToInt32(b.PixelWidth * 0.07);//从7%开始寻找极限
            al = this.getImageGrayVLine(b, left + ld);
            top = FindFirstBlack(al, 150);//初始化上边界，后面的只会越来越低
            if (top == -1) { top = 0; }//表示没有找到任何发黑的像素
            bottom = FindLastBlack(al, 150);//初始化下边界，后面只会越来越高
            for (long i = left + ld; i < right - 1; i++)
            {
                al = this.getImageGrayVLine(b, i);

                al.RemoveRange(bottom + 1, al.Count - bottom - 1);
                al.RemoveRange(0, top + 1);
                int tx = al.Count;
                this.CropBoder(al);//过滤两侧的边界

                top = top + (tx - al.Count) / 2;
                bottom = bottom - (tx - al.Count) / 2;//获取当前的边界，为下一个更窄的预处理打基础

                if (this.IsSoveLine(al, Percent))
                {
                    List<int> Nal = this.getImageGrayVLine(b, i + 1);

                    Nal.RemoveRange(bottom + 1, Nal.Count - bottom - 1);
                    Nal.RemoveRange(0, top + 1);
                    this.CropBoder(Nal);//过滤两侧的边界
                    if (IsSoveLine(Nal, Percent))
                    {
                        r = i;
                        break;
                    }
                }
            }
            if (r == 0) { return 0; }
            return r;
        }

        public decimal getLPResove(WriteableBitmap b, double Percent)//通过照片得到相机的分辨率，这里是照片是包含逐渐接近的黑白线对的局部，后面是修形的参数
        {
            //算法：第一步找到图像的左边界，和又边界。条件是左边界是白色加黑色，右边界没有则是黑色加灰色
            //循环，每一列进行分析。
            //SilverlightPhotoIO.PhotoIO.WriteImageToFile(b);
            long r = getRayleiPosition(b, Percent);
            if (r == 0) { return 0; }
            long left, right = 0;

            left = getLeftPoint(b);
            
            right = getRightPoint(b);
            setProcessInfor("RayleiResolutionLeftBorder", left);
            setProcessInfor("RayleiResolutionRightBorder", right);
            decimal x;
            x = Convert.ToDecimal((r - left)) / Convert.ToDecimal((right - left));
            if (ProcessInfor.ContainsKey("RayleiResolutionRightPosition"))
            {
                ProcessInfor["RayleiResolutionRightPosition"] = r;
            }
            else
            {
                ProcessInfor.Add("RayleiResolutionRightPosition", r);
            }
            return x;
        }

        protected Boolean IsSoveLine(List<int> al, double sv)//直接判读数列里面的极大值和极小值，看其中的比，如果两个次小值都大于判据，则表示合格。
        //条件，背景是白色的，前端是黑色的，首先要检测是否进入了目标区域，目标区外面的像素不考虑
        {
            //输入的数列已经去掉了边界，只要求出最黑的地方和两个最白的地方就可以了。
            //算法是找出最黑的和最白的，如果比例小于判据，就认为可以分辨，否则就是不可
            Dictionary<long, int> htMAX = new Dictionary<long, int>();
            Dictionary<long, int> htMIN = new Dictionary<long, int>();
            decimal tx = this.DeepAnalyseArray(htMAX, htMIN, al);//避免后面的黑线条对于分析产生干扰，同时消除噪声

            if (sv < 0.1)
            {
                sv = 0.735f;//瑞丽判据
            }
            htMAX = null;
            htMIN = null;
            if (tx > Convert.ToDecimal(sv))//表示最小的极值点平均值已经大于最大极值点平均值的73.5%
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        protected decimal DeepAnalyseArray(Dictionary<long, int> htMAX, Dictionary<long, int> htMIN, List<int> al)//通过该函数可以找出所有的顶点的位置，后面就是分析这些顶点哪些是真的，哪些是假的
        {   //需要预先知道夸距，包括峰谷之间的和波峰直接的。前面的5个已经可以给出精确的平均峰谷距离，然后就可以依据这个距离来测算整个的涵盖范围。
            //如果出现的是相等的平台，记录的是变化的后缘
            //需要注意的是需要检查黑色和白色出现的相对位置，只有和前面的位置等距才是合法的点。
            htMAX.Clear();
            htMIN.Clear();

            AnalyseArray(htMAX, htMIN, al);//最敏感的记录，包含了大量的噪声，后面的任务主要是去除噪声

            List<int> minVal = new List<int>(htMIN.Values);
            List<int> maxVal = new List<int>(htMAX.Values);

            int aveMax = Convert.ToInt32(AverageValue(maxVal));//计算初始的值的平均值
            int aveMin = Convert.ToInt32(AverageValue(minVal));
            //minVal.Sort();
            //maxVal.Sort();

            for (int i = 0; i < maxVal.Count; i++)
            {
                int tem = Convert.ToInt32(maxVal[i]);
                if (aveMax - tem > 20)//和平均值差异过大的极值点被认为是噪点，删除之
                {
                    maxVal.RemoveAt(i);
                }
            }

            for (int i = 0; i < minVal.Count; i++)
            {
                int tem = Convert.ToInt32(minVal[i]);
                if (tem - aveMin > 20)
                {
                    minVal.RemoveAt(i);
                }
            }
            aveMax = Convert.ToInt32(AverageValue(maxVal));//再次从新计算新平均值
            aveMin = Convert.ToInt32(AverageValue(minVal));

            if (aveMax ==0 && aveMin==0)
            {
                return 1;//表示极大点和极小点已经无法形成差异，都是0
            }
            decimal xd = Convert.ToDecimal(aveMin) / Convert.ToDecimal(aveMax);
            return xd;
        }

        public decimal getContrast(List<int> al)
        {
            Dictionary<long, int> htMAX = new Dictionary<long, int>();
            Dictionary<long, int> htMIN = new Dictionary<long, int>();
            decimal tx = this.DeepAnalyseArray(htMAX, htMIN, al);//避免后面的黑线条对于分析产生干扰，同时消除噪声
            htMAX = null;
            htMIN = null;
            return tx;
        }

        #endregion

        #region Noise
        public double getNoise(WriteableBitmap b, float drift)
        {
            int n = getNoiseNum(b, drift);
            return Convert.ToDouble(n) / b.PixelBuffer.ToArray().Length/4;
        }
        
        public  int getNoiseNum(WriteableBitmap b, float driftPercent)//得到符合标准，也就是超过阀值的噪点的数量
        //阀值是一个浮点数，实际是rgb偏移的允许比例
        //bm是单一色块，driftPercent是阀值
        {
            int NoiseCount = 0;
            Color AveC = this.getAverageColor(b);

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                Color c = new Color();
                c.B = components[i];
                c.G = components[i+1];
                c.R = components[i+2];
                if (IsNoise(c, AveC, driftPercent))//偏离平均值过大被认为是噪点
                {
                    NoiseCount++;
                }
            }

            return NoiseCount;
        }

        public Boolean IsNoise(Color SourcrC, Color AveColor, float DriftPercent)//判断一个颜色在可以允许的情况下是不是属于噪点
        //实际是求RGB的最大差异是不是超出了允许的范围
        {
            Boolean flag = false;
            int lr, lg, lb;
            int hr, hg, hb;
            lr = Convert.ToInt32(AveColor.R * (1 - DriftPercent));
            lg = Convert.ToInt32(AveColor.G * (1 - DriftPercent));
            lb = Convert.ToInt32(AveColor.B * (1 - DriftPercent));
            hr = Convert.ToInt32(AveColor.R * (1 + DriftPercent));
            hg = Convert.ToInt32(AveColor.G * (1 + DriftPercent));
            hb = Convert.ToInt32(AveColor.B * (1 + DriftPercent));
            if (SourcrC.R > hr)
            {
                flag = true;
            }
            if (SourcrC.G > hg)
            {
                flag = true;
            }
            if (SourcrC.B > hb)
            {
                flag = true;
            }
            if (SourcrC.R < lr)
            {
                flag = true;
            }
            if (SourcrC.G < lg)
            {
                flag = true;
            }
            if (SourcrC.B < lb)
            {
                flag = true;
            }
            return flag;
        }

        public List<PixInfor> getNoiseInfor(WriteableBitmap b, float driftPercent)//得到符合标准，也就是超过阀值的噪点的数量
        //阀值是一个浮点数，实际是rgb偏移的允许比例
        //bm是单一色块，driftPercent是阀值
        {
            List<PixInfor> pl = new List<PixInfor>();
            Color AveC = this.getAverageColor(b);

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelHeight; i++)
            {
                for (int j = 0; j < b.PixelWidth; j++)
                {
                    var index = i * b.PixelWidth * 4 + (j * 4);
                    Color c = new Color();
                    c.B = components[index];
                    c.G = components[index+1];
                    c.R = components[index+2];
                    if (IsNoise(c, AveC, driftPercent))//偏离平均值过大被认为是噪点
                    {
                        PixInfor pi = new PixInfor();
                        pi.colorValue = c;
                        pi.XPosition = j;
                        pi.YPosition = i;
                        pl.Add(pi);

                    }
                }
            }

            return pl;
        }

        public void ReduceChangedBrightPix(List<PixInfor> pl, WriteableBitmap b, int h)//逐步删除不一致的点
        {
            foreach (PixInfor pi in pl)
            {
                if (IsSameBright(pi, b, h) == true)
                {

                }
                else
                {
                    pl.Remove(pi);
                }
            }
        }

        public bool IsSameBright(PixInfor pi, WriteableBitmap b,int h)//判断在某个阀值下该点的亮度是否和图片对应位置的点的亮度一致
        {
            byte[] components = new byte[4];
            components = BitConverter.GetBytes(b.PixelBuffer.ToArray()[pi.XPosition+pi.YPosition*b.PixelWidth]);
            Color c = new Color();
            c.B = components[0];
            c.G = components[1];
            c.R = components[2];
            if ((Math.Abs(pi.colorValue.R - c.R) < h) && (Math.Abs(pi.colorValue.G - c.G) < h) && (Math.Abs(pi.colorValue.B - c.B) < h))
            {
                return true;
            }
            return false;

        }
        public bool IsSame(PixInfor a, PixInfor b,int ph,int bh)
        {
            if ((Math.Abs(a.XPosition - b.XPosition) < ph) && (Math.Abs(a.YPosition - b.YPosition) < ph))
            {
                if ((Math.Abs(a.colorValue.R - b.colorValue.R) < bh) && (Math.Abs(a.colorValue.G - b.colorValue.G) < bh) && (Math.Abs(a.colorValue.B - b.colorValue.B) < bh))
                {
                    return true;
                }
            }
            return false;
        }


        #endregion

        #region ColorTrend
        
        List<Color> SourceColorList = new List<Color>();
        List<WriteableBitmap> photoList=new List<WriteableBitmap>();

        public double getAverageColorTrend(List<Color> sl, List<WriteableBitmap> vl)//获得平均色彩偏移
        {
            SourceColorList = sl;
            photoList = vl;
            double td=0;
            if (vl.Count == 0) { return -1; }
            for (int i = 0; i < vl.Count; i++)
            {
                td = td + getColorHueDistance(sl[i], getAverageColor(vl[i]));
            }
            return td / vl.Count;
        }

        public double getAverageRGBColorTrend(List<Color> sl, List<WriteableBitmap> vl)//获得平均色彩偏移
        {
            SourceColorList = sl;
            photoList = vl;
            double td = 0;
            if (vl.Count == 0) { return -1; }
            for (int i = 0; i < vl.Count; i++)
            {
                td = td + Math.Sqrt(((sl[i].R - getAverageColor(vl[i]).R) * (sl[i].R - getAverageColor(vl[i]).R)) + ((sl[i].G - getAverageColor(vl[i]).G) * (sl[i].G - getAverageColor(vl[i]).G)) + ((sl[i].B - getAverageColor(vl[i]).B) * (sl[i].B - getAverageColor(vl[i]).B)));
            }
            return td / vl.Count;
        }

        public double getAverageLabColorTrend(List<Color> sl, List<WriteableBitmap> vl)//获得平均色彩偏移
        {
            SourceColorList = sl;
            photoList = vl;
            double td = 0;
            if (vl.Count == 0) { return -1; }
            for (int i = 0; i < vl.Count; i++)
            {
                LColor ls, lv;
                ls = new LColor(sl[i]);
                lv = new LColor(getAverageColor(vl[i]));
                td = td + Math.Sqrt((ls.getL(CurrentLabMode) - lv.getL(CurrentLabMode)) * (ls.getL(CurrentLabMode) - lv.getL(CurrentLabMode)) + (ls.geta(CurrentLabMode) - lv.geta(CurrentLabMode)) * (ls.geta(CurrentLabMode) - lv.geta(CurrentLabMode)) + (ls.getb(CurrentLabMode) - lv.getb(CurrentLabMode)) * (ls.getb(CurrentLabMode) - lv.getb(CurrentLabMode)));
            }
            return td / vl.Count;
        }

        public double getMaxColorTrend(List<Color> sl, List<WriteableBitmap> vl)//获得平均色彩偏移
        {
            SourceColorList = sl;
            photoList = vl;
            double td = 0;
            if (vl.Count == 0) { return -1; }
            for (int i = 0; i < vl.Count; i++)
            {
                td = Math.Max(td, getColorHueDistance(sl[i], getAverageColor(vl[i])));
            }
            return td;
        }

        public double getMaxRGBColorTrend(List<Color> sl, List<WriteableBitmap> vl)//获得平均色彩偏移
        {
            SourceColorList = sl;
            photoList = vl;
            double td = 0;
            if (vl.Count == 0) { return -1; }
            for (int i = 0; i < vl.Count; i++)
            {
                double v= Math.Sqrt(((sl[i].R - getAverageColor(vl[i]).R) * (sl[i].R - getAverageColor(vl[i]).R)) + ((sl[i].G - getAverageColor(vl[i]).G) * (sl[i].G - getAverageColor(vl[i]).G)) + ((sl[i].B - getAverageColor(vl[i]).B) * (sl[i].B - getAverageColor(vl[i]).B)));
                td = Math.Max(td,v);
            }
            return td;
        }

        public double getMaxLabColorTrend(List<Color> sl, List<WriteableBitmap> vl)//获得平均色彩偏移
        {
            SourceColorList = sl;
            photoList = vl;
            double td = 0;
            if (vl.Count == 0) { return -1; }
            for (int i = 0; i < vl.Count; i++)
            {
                LColor ls, lv;
                ls = new LColor(sl[i]);
                lv = new LColor(getAverageColor(vl[i]));
                double v = Math.Sqrt((ls.getL(CurrentLabMode) - lv.getL(CurrentLabMode)) * (ls.getL(CurrentLabMode) - lv.getL(CurrentLabMode)) + (ls.geta(CurrentLabMode) - lv.geta(CurrentLabMode)) * (ls.geta(CurrentLabMode) - lv.geta(CurrentLabMode)) + (ls.getb(CurrentLabMode) - lv.getb(CurrentLabMode)) * (ls.getb(CurrentLabMode) - lv.getb(CurrentLabMode)));
                td = Math.Max(td,v);
            }
            return td;
        }


        #endregion

        #region WhiteBalance
        
        public List<Color> getAvgColorList(List<WriteableBitmap> bl)
        {
            List<Color> cl = new List<Color>();
            foreach (WriteableBitmap b in bl)
            {
                cl.Add(getAverageColor(b));
            }
            return cl;
        }

        public double getWhiteBalance(List<WriteableBitmap> bl)
        {
            List<Color> cl = getAvgColorList(bl);
            double d = 0; ;
            if (cl.Count == 0) { return -1; }
            foreach (Color c in cl)
            {
                d = d + getSaturation(c);
            }
            return d / cl.Count;
        }

        #endregion

        #region WaveQ

        public decimal getWaveQ(WriteableBitmap b, int WaveNo)//得到正弦波型的质量，关键是得到正弦数列,WaveNo是波的数目，就是波峰或者波谷的数目，选择较小的一个（黑白线条少的一个）
        //注意，WaveNo是峰谷（黑白）之间，少的那个的数值，而不是多的，这是为了让每一段尽可能的长一些，包含更多的相邻的波峰和波谷
        {//首先找到图片里面的最小点和最大点，然后检查中间点和对应的曲线之间的差
            //针对每一个波长，找到里面的一个最大值和最小值位置（对应的序号）
            //然后对比正弦曲线，看中间的点值差异
            List<int> HalfWaveList=getHalfWaveList(b, WaveNo);
            decimal wd = getAveWaveDis(HalfWaveList);
            return wd;
        }

        public List<int> getHalfWaveList(WriteableBitmap b, int WaveNo)//得到图片中间的一个完整的半波形，从高处到低处
        {
            List<int> al = getImageGrayHLine(b, b.PixelHeight / 2);
            TrimGrayList(al, 35, true, Convert.ToInt32(al[0]));//去除两端的背景像素（白色）
            int WaveLong = al.Count / WaveNo;//扩大每个区域的范围，但是不能在一个里面双峰双谷，但是必须包含锋和谷
            List<int> WaveList = new List<int>();
            int boderIndex = (al.Count - WaveLong) / 2;
            for (int i = boderIndex; i < boderIndex + WaveLong; i++)//取中间的一段包含半个波长的数列
            {
                WaveList.Add(al[i]);
            }
            int minPos, maxPos;
            List<long> PosArrarlist = getMaxValuePosForList(WaveList);//得到相邻的一对波峰和波谷的数值，一个最大，一个最小
            minPos = Convert.ToInt32(PosArrarlist[0]);
            maxPos = Convert.ToInt32(PosArrarlist[1]);
            int sPos,Lcount;
            if(minPos<maxPos)
            {
                sPos=minPos;
                Lcount=maxPos-minPos+1;
            }
            else
            {
                sPos=maxPos;
                Lcount=minPos-maxPos+1;
            }
            List<int> HalfWaveList = getSubList<int>(WaveList, sPos, Lcount);//截取出半波长度的数列，并且是相邻的最大最小值之间
            if (Convert.ToInt32(HalfWaveList[0]) < Convert.ToInt32(HalfWaveList[HalfWaveList.Count - 1]))//如果是上升波，修改为下降的，也就是对应余弦
            {
                HalfWaveList = ReverseList(HalfWaveList);
            }

            return HalfWaveList;
        }

        public List<T> getSubList<T>(List<T> al, int StartNo, int Count)
        {
            List<T> tal = new List<T>();
            for (int i = StartNo; i < StartNo+Count; i++)
            {
                tal.Add(al[i]);
            }
            return tal;
        }

        public decimal getAveWaveDis(List<int> al)//依据半波长度和振幅，计算给出的数列和标准的余弦波之间距离
        {
            if (Convert.ToInt32(al[0]) < Convert.ToInt32(al[al.Count-1]))//如果是上升波，修改为下降的，也就是对应余弦
            {
                al = ReverseList(al);
            }
            int HalfWaveLong = al.Count;
            int WaveSwing = Convert.ToInt32(al[0]) - Convert.ToInt32(al[al.Count - 1]);
            int minValue = Convert.ToInt32(al[al.Count - 1]);
            List<decimal> sinAl = getAdjustWaveValueList(HalfWaveLong, WaveSwing,minValue);//得到余弦的半波型
            decimal Dis = 0;
            for (int i = 0; i < al.Count; i++)
            {
                decimal x0 = al[i];
                decimal x1 = sinAl[i];
                Dis = Dis + Convert.ToDecimal(Math.Abs(x1 - x0));
            }
            return Dis / sinAl.Count/WaveSwing;//给出的是针对每个振幅单位的一个比例
        }

        public List<decimal> getAdjustWaveValueList(int HalfWaveLong, int WaveSwing, int minValue)//依据取样精度，得到半个波长的正弦波值分立列表，参数是全波长的采样数
        {
            List<decimal> al = getSinWaveValueList(HalfWaveLong);
            List<decimal> Tal = new List<decimal>();
            for (int i = 0; i < al.Count; i++)
            {
                decimal x = al[i];
                x = x + 1;
                Tal.Add(x * WaveSwing / 2+minValue);

            }
            return Tal;
        }

        public List<decimal> getSinWaveValueList(int HalfWaveLong)//依据取样精度，得到半个波长的正弦波值分立列表，参数是全波长的采样数
        {//结果是从max到min，也就是余弦波
            List<decimal> al = new List<decimal>();
            double step = Math.PI / HalfWaveLong;
            for (int i = 0; i < HalfWaveLong; i++)
            {
                double x = 0;
                x = Math.Cos(step * i);
                al.Add(Convert.ToDecimal(x));
            }
            return al;
        }
        public List<List<decimal>> getCurveWaveQ(WriteableBitmap b, int No)//得到一个表示测试卡波型的亮度变化曲线，给出的是依据顺序由低到高的频率
        {
            List<List<decimal>> al = new List<List<decimal>>();
            List<decimal> Sal;//理论波形
            List<decimal> Val;
            int WaveSwing;
            int minValue;

            Val = getDecimalList(getHalfWaveList(b, XMarkChart.getHalfWaveCountByIndex(No)));
            WaveSwing = Convert.ToInt32(Val[0]) - Convert.ToInt32(Val[Val.Count - 1]);
            minValue = Convert.ToInt32(Val[Val.Count - 1]);
            Sal = getAdjustWaveValueList(Val.Count, WaveSwing, minValue);
            al.Add(Sal);
            al.Add(Val);

            return al;
        }


        #endregion

        #region Latitude
        public int getLatitude(List<WriteableBitmap> bl,double h)//动态范围，获取灰度级别的平均值，看极差，极差大于一定程度就表示可以识别，默认h是表示级差
        {
            List<double> al = getAverageBrightList(bl);//记录各个灰度色块的平均灰度值，动态范围就是看灰度值的数量
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
        #endregion

        #region Purple
        public double getgetPurplePercent(WriteableBitmap b,int h)
        {
            long pnum = getColorPixNum(b, Colors.Purple, h);//色相环紫色周围30度夹角内的像素，都算是紫边
            double pe = pnum / (b.PixelWidth * b.PixelHeight);
            return pe;
        }

        #endregion

        #region AntiShake
        public double getShake(List<PixInfor> pl)//找出最大的抖动距离
        {
            if(pl.Count<2){return -1;}
            double d = (pl[0].XPosition - pl[1].XPosition) * (pl[0].XPosition - pl[1].XPosition) + (pl[0].YPosition - pl[1].YPosition) * (pl[0].YPosition - pl[1].YPosition);
            if(pl.Count==2){return Math.Sqrt(d);}
            for (int i = 2; i < pl.Count; i++)
            {
                double td = (pl[0].XPosition - pl[i].XPosition) * (pl[0].XPosition - pl[i].XPosition) + (pl[0].YPosition - pl[i].YPosition) * (pl[0].YPosition - pl[i].YPosition);
                if (td > d)
                {
                    d = td;
                }
            }
            return Math.Sqrt(d);
        }

        public double getAntiShakePercent(List<PixInfor> NoAnti, List<PixInfor> Anti)//获得防抖效率
        {
            double nd = getShake(NoAnti);
            double d = getShake(Anti);
            return (nd - d) / nd;
        }
        #endregion

        #region BadPix//坏点测试
        int step = 1;//相邻点的定义
        double h = 1.5;//判断一致的阀值
        public void setStep(int v) { step = v; }
        public void setH(double v) { h = v; }
        public List<PixInfor> getEqualPix(List<WriteableBitmap> BitmapList)
        {
            if (BitmapList.Count < 2) { return null; }
            int w, h;
            w = BitmapList[0].PixelWidth;
            h = BitmapList[0].PixelHeight;
            foreach (WriteableBitmap b in BitmapList)
            {
                if (b.PixelWidth != w) { return null; }
                if (b.PixelHeight != h) { return null; }
            }
            List<PixInfor> pl = getEqualPix(BitmapList[0], BitmapList[1]);
            if (pl == null) { return null; }
            for (int i = 2; i < BitmapList.Count; i++)
            {
                List<PixInfor> tpl = getEqualPix(BitmapList[i], pl);
                pl = new List<PixInfor>(tpl);
                tpl.Clear();
                tpl = null;
            }
            return pl;
        }
        public List<PixInfor> getEqualPix(WriteableBitmap SourceBitmap, WriteableBitmap TargetBitmap)
        {
            if (SourceBitmap.PixelHeight != TargetBitmap.PixelHeight) { return null; }

            if (SourceBitmap.PixelWidth != TargetBitmap.PixelWidth) { return null; }
            List<PixInfor> pl = new List<PixInfor>();
            for (int i = 0; i < SourceBitmap.PixelHeight; i++)
            {
                for (int j = 0; j < SourceBitmap.PixelWidth; j++)
                {
                    if (IsEpualPix(SourceBitmap, TargetBitmap,j,i))
                    {
                        PixInfor pi = new PixInfor();
                        pi.colorValue = GetPixel(SourceBitmap,j,i);
                        pi.XPosition = i % SourceBitmap.PixelWidth;
                        pi.YPosition = i / SourceBitmap.PixelWidth;
                        pi.PhotoH = SourceBitmap.PixelHeight;
                        pi.PhotoW = SourceBitmap.PixelWidth;
                        pl.Add(pi);
                    }
                }
            }
            SourceBitmap = null;
            TargetBitmap = null;
            return pl;
        }
        public List<PixInfor> getEqualPix(WriteableBitmap SourceBitmap, List<PixInfor> TargetPixList)
        {
            List<PixInfor> pl=new List<PixInfor>();
            foreach (PixInfor pi in TargetPixList)
            {
                bool x = IsEpualPix(SourceBitmap, pi);
                if (x) { pl.Add(pi); }
                //if (!x) { pl.Add(pi); }
            }
            SourceBitmap = null;
            TargetPixList = null;
            return pl;
        }
        List<int> getStepIndex(int w, int h, int x, int y)
        {
            List<int> il = new List<int>();
            int x0, y0,x1,y1;
            if (x - step < 0) { x0 = 0; } else { x0 = x - step; }
            if (x + step > w-1) { x1 = w-1; } else { x1 = x + step; }
            if (y - step < 0) { y0 = 0; } else { y0 = y - step; }
            if (y + step > h-1) { y1 = h-1; } else { y1 = y + step; }
            for (int i = y0; i <= y1; i++)
            {
                for (int j = x0; j <= x1; j++)
                {
                    il.Add(w * i + j);
                }
            }
            return il;
        }
        bool IsEpualPix(WriteableBitmap b, PixInfor pi)//判断一个图像和特定的PixInfor是否一致
        {
            List<int> il=getStepIndex(b.PixelWidth,b.PixelHeight,pi.XPosition,pi.YPosition);
            foreach (int i in il)
            {
                if (IsEpualPix(b, pi)) { return true; }
            }
            return false;
        }
        bool IsEpualPix(WriteableBitmap source,WriteableBitmap target, int X, int Y)//判断一个像素整型是否和特定的PixInfor一致
        {//RGB任何的分量小于阀值都算是相等
            Color sc = GetPixel(source, X, Y);
            Color tc = GetPixel(target, X, Y);
            if (Math.Abs(sc.R - tc.R) < h) { return true; }
            if (Math.Abs(sc.G - tc.G) < h) { return true; }
            if (Math.Abs(sc.B - tc.B) < h) { return true; }
            return false;
        }
        bool IsEpualPix(WriteableBitmap photo, int X,int Y, PixInfor pi)//判断一个像素整型是否和特定的PixInfor一致
        {//RGB任何的分量小于阀值都算是相等
            Color c = GetPixel(photo,X,Y);
            if (Math.Abs(c.R - pi.colorValue.R) < h) { return true; }
            if (Math.Abs(c.G - pi.colorValue.G) < h) { return true; }
            if (Math.Abs(c.B - pi.colorValue.B) < h) { return true; }
            return false;
        }
        #endregion

        #region GBBadPix
        public List<PixInfor> getGBExceptionPix(WriteableBitmap b,double h)//依据国标获得异常点
        {
            List<double> dl = getImageLList(b);
            double d = 0;
            foreach(double x in dl)
            {
                d = d + x;
            }
            d = d / dl.Count;
            List<PixInfor> pl = new List<PixInfor>();
            for (int i = 0; i < dl.Count;i++ )
            {
                if (Math.Abs(dl[i] - d) >= h)
                {
                    PixInfor p = new PixInfor();
                    p.YPosition = i / b.PixelWidth;
                    p.XPosition = i % b.PixelWidth;
                    p.colorValue = GetPixel(b, p.XPosition, p.YPosition);
                }
            }
            return pl;
        }
        public List<PixInfor> getGBBadPix(WriteableBitmap b, double expH ,int minStep,int maxStep,double h)//依据国标获得坏点
        {
            List<PixInfor> dl = getGBExceptionPix(b, expH);
            List<PixInfor> bl = new List<PixInfor>();
            foreach (PixInfor p in dl)
            {
                if (IsBadPix(b, p, minStep, maxStep, h))
                {
                    bl.Add(p);
                }
            }
            return bl;
        }
        public List<PixInfor> getGBBadPix(List<WriteableBitmap> bl, double expH, int minStep, int maxStep, double h)//依据国标获得坏点
        {

            List<PixInfor> l = new List<PixInfor>();
            foreach (WriteableBitmap b in bl)
            {
                List<PixInfor> pl = getGBBadPix(b, expH, minStep, maxStep, h);
                foreach (PixInfor p in pl)
                {
                    l.Add(p);
                }
            }
            return l;
        }

        public bool IsBadPix(WriteableBitmap b, PixInfor p,int minStep,int maxStep,double h)//默认min是1，max是4,h是阀值
        {
            WriteableBitmap minB = getImageArea(b, p.XPosition - minStep, p.YPosition - minStep, minStep * 2 + 1, minStep * 2 + 1);
            WriteableBitmap maxB = getImageArea(b, p.XPosition - maxStep, p.YPosition - maxStep, maxStep * 2 + 1, maxStep * 2 + 1);
            double minL = getAverageColorL(minB);
            double maxL = getAverageColorL(maxB);
            if (Math.Abs(minL - maxL) >= h) { return true; }
            return false;
        }

        #endregion

        #region GBBrightChanged

        public double getGBBrightChangedValue(WriteableBitmap b,int n)
        {
            List<WriteableBitmap> bl = getGBBrightChangedTestArea(b, n);
            List<double> l = getGBBrightChanged(bl);

            double max=double.MinValue, min=double.MaxValue;
            foreach (double d in l)
            {
                if (d > max) max = d;
                if (d < min) min = d;
            }
            return min / max;
        }

        public List<WriteableBitmap> getGBBrightChangedTestArea(WriteableBitmap b,int n)
        {
            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            double Hstep=b.PixelWidth/n;
            double Vstep=b.PixelHeight/n;
            for (int i = 0; i < 11; i++)
            {
                WriteableBitmap xb = getImageArea(b, Convert.ToInt32(i * Hstep), Convert.ToInt32(i * Vstep), Convert.ToInt32(Hstep), Convert.ToInt32(Vstep));
                bl.Add(xb);
            }
            return bl;
        }

        public List<double> getGBBrightChanged(List<WriteableBitmap> bl)
        {
            List<double> l = new List<double>();
            foreach (WriteableBitmap xb in bl)
            {
                l.Add(getAverageColorL(xb));
            }
            return l;
        }

        #endregion

        #region GBAberration

        public double getGBAberration(double TestLength,double TheoryLength,double Scale)
        {
            return 1 - (TestLength / Scale / TheoryLength);
        }

        #endregion

        #region GBDEv
        public double getGBDEv(WriteableBitmap b,double gm,double c)//gm是矫正值，c是比例系数
        {
            WriteableBitmap cb = getImageArea(b, b.PixelWidth / 4, b.PixelHeight / 4, b.PixelWidth / 2, b.PixelHeight / 2);
            double l = getAverageColorL(cb);

            if (c < 0.001) { c = 3.322; }
            return c / gm * Math.Log10(l / 127);
        }
        public double getDEv(WriteableBitmap b, double gm, double c)//gm是矫正值，c是比例系数
        {
            double l = getAverageColorL(b);

            if (c < 0.001) { c = 3.322; }
            return c / gm * Math.Log10(l / 127);
        }
        #endregion
    }



    public class PixInfor//像素的详细信息
    {
        public int XPosition = -1;
        public int YPosition = -1;
        public int PhotoW = -1;
        public int PhotoH = -1;
        public TimeSpan CurrentTimeLong;//适用于视频截图
        public Color colorValue;
        public LColor LColorValue
        {
            get;
            set;
        }

        public string getDescription()//转换基本描述信息
        {
            string s = "";
            s = "Color(" + colorValue.R.ToString() + "," + colorValue.G.ToString() + "," + colorValue.R.ToString() + ")";
            s = s + ";";
            s = s + "Position(" + XPosition.ToString() + "," + YPosition.ToString() + ")";
            return s;
        }
    }
}
