using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using DCTestLibrary;
using System.Collections.Generic;
using System.Linq;
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
using SilverlightLFC.common;

namespace SilverlightDCTestLibrary
{
    public class ISO12233ExChart : AbstractTestChart
    {
        public override string ChartName { get { return "Super ISO12233 Chart "; } }
        public override string ChartMemo { get { return "Can test a lot of parameter"; } }

        private float ResoveBrightPercent=73.5f;//分辨率的默认数值，这个数值被修改，测试的分辨率就会改变

        public ISO12233ExChart(WriteableBitmap Photo)
        {
            //InitBWList();
            //InitColorList();
            ChartPhoto = Photo;
            mp = new MarkProcess(Photo);
            CorrectChart();
            
        }
        public ISO12233ExChart() { }
        //public new void setChart(WriteableBitmap b)
        //{
        //    base.setChart(b);
        //    CorrectISO12233Chart();
        //}
        //public void DrawSelectArea(int StartX,int StartY, int w, int h)//在分析照片上面绘制出选择的区域，便于调试
        //{
        //    if (AnalysePhoto == null)
        //    {
        //        if (ChartPhoto == null) { return; }
        //        AnalysePhoto = new WriteableBitmap(ChartPhoto);
        //    }
        //    DrawRecArea(AnalysePhoto, StartX, StartY, w, h, Colors.Red);
        //    mp.SelectedPhoto = new WriteableBitmap(AnalysePhoto);
        //}

        public long getLPResoveLines()//自动从原始的照片
        {
            //AutoBright(CorrectPhoto);
            float percent = this.ResoveBrightPercent / 100;

            WriteableBitmap xb = getAreaLResove(ChartPhoto);
            if (ProcessInfor.ContainsKey("RayleiResolutionIsLeft"))
            {
                ProcessInfor["RayleiResolutionIsLeft"] = true;
            }
            else
            {
                ProcessInfor.Add("RayleiResolutionIsLeft", true);
            }
            //AutoBright(xb);
            long l = getResoveLines(xb, percent, true);
            if (l == 0||l>1900)//如果左侧的测试没有测出一个可以的成绩
            {
                //xb.Dispose();
                xb = getAreaRResove(ChartPhoto);
                setProcessInfor("RayleiResolutionIsLeft", false);
                l = getResoveLines(xb, percent, false);
            }
            //xb.Dispose();
            return l;
            //return getLPResoveLines(CorrectPhoto, 0f);
        }

        public long getResoveLines(WriteableBitmap b, float percent, bool IsLeft)//通过最终分辨率的位置来折算实际的分辨率
        {//主要是需要判读这些曲线属于哪个级别的

            if (!IsLeft)
            {
                b=FlipYImage(b);

            }
            int ln=0;
            List<int> NumList = new List<int>();
            NumList.Add(getHBlackLineNum(b, 0.08));
            NumList.Add(getHBlackLineNum(b, 0.1));
            NumList.Add(getHBlackLineNum(b, 0.12));
            NumList.Add(getHBlackLineNum(b, 0.14));
            NumList.Add(getHBlackLineNum(b, 0.16));

            ln = (NumList.Sum() - NumList.Max() - NumList.Min()) / (NumList.Count - 2);

            long r = 0;
            decimal m = ptp.getLPResove(b, percent);
            if (m == 0)
            {
                return 0;
            }
            if ((ln < 6) && IsLeft)//说明是老的ISO12233的低分辨测试
            {
                r = Convert.ToInt64(100 + m * 500);
                ptp.setProcessInfor("ISOCardType", "100-600");
            }
            if ((ln > 6) && (!IsLeft) && (ln < 9))//说明是老的ISO12233高分辨测试
            {

                r = Convert.ToInt64(500 + m * 1500);
                ptp.setProcessInfor("ISOCardType", "500-2000");
            }
            if (ln > 8)//说明是新的ISO12233高分辨测试（低的不使用新的）
            {
                if (IsLeft)
                {
                    r = Convert.ToInt64(500 + m * 1500);
                    ptp.setProcessInfor("ISOCardType", "500-2000");
                }
                else
                {
                    r = Convert.ToInt64(1000 + m * 3000);
                    ptp.setProcessInfor("ISOCardType", "1000-4000");
                }
            }

            return r;

        }

        public decimal getHDispersiveness()
        {
            if (mp.CorrectPhoto == null) { return -1; }
            WriteableBitmap cb = getAreaHEdge(mp.CorrectPhoto);
            decimal f = ptp.getEdgeDispersiveness(cb, true);//调用测试对象的对应方法
            return f;
        }

        public decimal getVDispersiveness()
        {

            if (CorrectPhoto == null) { return -1; }
            WriteableBitmap cb = getAreaVEdge(CorrectPhoto);
            decimal f = ptp.getEdgeDispersiveness(cb, false);//调用测试对象的对应方法
            return f;
        }

        public WriteableBitmap CorrectPosition(WriteableBitmap b)//专门针对ISO12233照片进行对正处理，如果对正失败，返回null
        {
            Point CP = getCenterPoint(b, 70);
            if (CP.X == 0 && CP.Y == 0)
            {
                return null;
            }
            int dx =Convert.ToInt32( CP.X - b.PixelWidth / 2);
            int dy =Convert.ToInt32( CP.Y - b.PixelHeight / 2);
            WriteableBitmap cb = MoveTrans(b, dx, dy);
            return cb;
        }

        public WriteableBitmap CorrectArcRegion(WriteableBitmap b)//获取ISO12233的验证角度的区域，无论是否新版本，统一处理
        {
            double XISOscale = 32.5 / 70.9;
            double YISOscale = 20.5 / 39.9;
            double XW = 5.9 / 39.9;
            double YH = 3.5 / 39.9;
            int xs = Convert.ToInt32(b.PixelHeight / 9 * 16 * XISOscale);
            xs = getCenterX(xs, b.PixelWidth, b.PixelHeight);
            int ys = Convert.ToInt32(b.PixelHeight * YISOscale);
            int lx = Convert.ToInt32(b.PixelHeight * XW);
            int ly = Convert.ToInt32(b.PixelHeight * YH);
            WriteableBitmap sb = getImageArea(b,xs,ys, lx, ly);//只切上面的1/4高度来检查倾斜性
            return sb;
        }

        public float getArc(WriteableBitmap b)//专门针对ISO12233照片获取倾斜角度，如果失败，-1
        {
            WriteableBitmap sb = CorrectArcRegion(b);//只切下面的1/4高度来检查倾斜性
            Point bp = getBottomPoint(sb, 100);
            float arc = -1;
            if (bp.X < sb.PixelWidth / 2)//表示最下面的点是在中线的左侧，整个角度是左倾
            {
                Point p = getRightPoint(sb, 100);//找到右侧的拐点
                if (p.X == bp.X) { return 0; }
                arc = (float)(Math.Atan((p.Y - bp.Y) / (p.X - bp.X)));
            }
            else//这个是右倾
            {
                Point p = getLeftPoint(sb, 100);//找到左侧的拐点
                if (p.X == bp.X) { return 0; }
                arc = (float)(Math.Atan((bp.Y - p.Y) / (bp.X - p.X)));
            }

            //Bitmap lb = RotateBitmap(sb, -arc);
            //sb.Dispose();

            return arc;

        }

        public WriteableBitmap Correct_ISO12233Chart(WriteableBitmap b)//专门针对ISO12233进行矫正，这个是新的ISO12233标准，旧的称为12233S
        {
            Point CP = getCenterPoint(b, 70);
            if (CP.X == 0 && CP.Y == 0)
            {
                return b;
            }
            int dx =Convert.ToInt32( CP.X - b.PixelWidth / 2);
            int dy = Convert.ToInt32( CP.Y - b.PixelHeight / 2);
            WriteableBitmap cb = MoveTrans(b, dx, dy);//对准操作
            float arc = getArc(cb);

            WriteableBitmap lb = RotateTrans(cb, -arc);
            //cb.Dispose();
            return lb;
        }

        public int getCenterX(int CX, int bw, int bh)//这是一个坐标变换，主要针对ISO12233使用不同长宽比例，进行x坐标换算
        {
            int rx;
            decimal x = Convert.ToDecimal(bw) / Convert.ToDecimal(bh);//计算照片的长宽比例，宽度除以长度
            rx = Convert.ToInt32(CX - (16m / 9m - x) / 2m * bh);
            return rx;
        }

        public WriteableBitmap getAreaLResove(WriteableBitmap b)
        {
            int LsratrX, LstartY, LW, LH;
            LsratrX = Convert.ToInt32(20.9 / 70.9 * (b.PixelHeight / 9 * 16));
            LsratrX = getCenterX(LsratrX, b.PixelWidth, b.PixelHeight);
            LstartY = Convert.ToInt32(18.8 / 39.9 * b.PixelHeight);
            LW = Convert.ToInt32(12.7 / 70.9 * (b.PixelHeight / 9 * 16));
            LH = Convert.ToInt32(2.2 / 39.9 * b.PixelHeight);

                if (IsAnalyse) { DrawSelectArea(LsratrX, LstartY, LW, LH); }

                WriteableBitmap lb = getImageArea(b, LsratrX, LstartY, LW, LH);
                if (mp.SelectedArea.ContainsKey("RayleiResolutionLArea"))
                {
                    mp.SelectedArea["RayleiResolutionLArea"] = WriteableBitmapHelper.Clone(lb);
                }
                else
                {
                    mp.SelectedArea.Add("RayleiResolutionLArea", WriteableBitmapHelper.Clone(lb));
                }
                return lb;

        }

        public WriteableBitmap getAreaRResove(WriteableBitmap b)//获取ISO12233测试卡的侧视图区域
        {
            int RstartX, RstartY, RH, RW;
            RstartX = Convert.ToInt32(37.5 / 70.9 * (b.PixelHeight / 9 * 16));
            RstartY = Convert.ToInt32(19.3 / 39.9 * b.PixelHeight);
            RstartX = getCenterX(RstartX, b.PixelWidth, b.PixelHeight);
            RW = Convert.ToInt32(12.7 / 70.9 * (b.PixelHeight / 9 * 16));
            RH = Convert.ToInt32(1.4 / 39.9 * b.PixelHeight);
                if (IsAnalyse) { DrawSelectArea(RstartX, RstartY, RW, RH); }
                WriteableBitmap rb = getImageArea(b,RstartX, RstartY, RW, RH);
                setSelectArea("RayleiResolutionRArea", rb);
                return rb;
        }

        public long getLLPResoveLines(WriteableBitmap b, float percent)//最终的通过ISO12233进行测试
        {
            //AutoBright(b);
            if (percent == 0)
            {
                percent = this.ResoveBrightPercent/100;
            }
            if (b == null) { return 0; }
            WriteableBitmap xb = getAreaLResove(b);
            setProcessInfor("RayleiResolutionIsLeft", true);

            //AutoBright(xb);
            long l = getResoveLines(xb, percent,true);

            return l;
        }
        public long getRPResoveLines(WriteableBitmap b, float percent)//最终的通过ISO12233进行测试
        {
            //AutoBright(b);
            if (percent == 0)
            {
                percent = this.ResoveBrightPercent / 100;
            }
            if (b == null) { return 0; }

            WriteableBitmap xb = getAreaRResove(b);
                if (ProcessInfor.ContainsKey("RayleiResolutionIsLeft"))
                {
                    ProcessInfor["RayleiResolutionIsLeft"] = false;
                }
                else
                {
                    ProcessInfor.Add("RayleiResolutionIsLeft", false);
                }
                long l = getResoveLines(xb, percent, false);
            return l;
        }

        public int getCCR(WriteableBitmap b)//得到ISO12233里面中央园的直径
        {
            int x, y;
            x = b.PixelWidth / 2;
            y = b.PixelHeight / 2;
            int rx, ry;
            List<int> alx = getImageGrayHLine(b, x);
            List<int> aly = getImageGrayVLine(b, y);
            rx = getBorderPointPosition(alx, x, true, 100);
            ry = getBorderPointPosition(aly, y, true, 100);
            int dx = x - rx;
            int dy = y - ry;
            if (Math.Abs(dx - dx) < 3)
            {
                return dx;
            }
            return 0;
        }

        public bool CorrectISO12233SChart(WriteableBitmap b)//专门对准ISO12233标准版本，首先纠正方向，和水平位置，然后纠正垂直位置。
        {
            if (!CheckAvailability(b))//表示该图的上下位置是完全错误的，不能纠正，没有测试意义
            {
                return false;
            }
            return true;
        }

        public bool CheckAvailability(WriteableBitmap b)//检查一张ISO12233的照片是否合格，标志是看上下边沿有没有卡住，如果黑像素超过总像素的10%就没有意义了
        {
            List<int> al = getImageGrayVLine(b, b.PixelWidth / 3);
            int tb = 0, bb = 0;
            for (int i = 0; i < al.Count / 2; i++)
            {
                Byte c = Convert.ToByte(al[i]);
                Color xc = Color.FromArgb(255,c, c, c);
                if (IsWhite(xc, 125))
                {
                    break;
                }
                else
                {
                    tb++;
                }
            }
            for (int i = al.Count - 1; i > al.Count / 2; i--)
            {
                Byte c = Convert.ToByte(al[i]);
                Color xc = Color.FromArgb(255,c, c, c);
                if (IsWhite(xc, 125))
                {
                    break;
                }
                else
                {
                    bb++;
                }
            }
            if ((tb * 30 > b.PixelHeight) || (bb * 30 > b.PixelHeight))//两侧的黑色像素超过总数的6%，表示照片没有能够贴到边上，每侧不超过3%
            {
                return false;
            }
            return true;


        }

        public override void CorrectChart()//使用默认的内部来进行纠正
        {
            if (ChartPhoto == null)
            {
                ChartPhoto = mp.SourcePhoto;
                mp.SelectedPhoto = CorrectISO12233Chart(ChartPhoto);
                CorrectPhoto = WriteableBitmapHelper.Clone(AnalysePhoto);
            }
            else
            {
                mp.SelectedPhoto = CorrectISO12233Chart(ChartPhoto);
                CorrectPhoto = WriteableBitmapHelper.Clone(AnalysePhoto);
            }
        }

        public WriteableBitmap CorrectISO12233Chart(WriteableBitmap b)//纠正ISO12233方位，包括位置和旋转以及大小比例
        {//算法是先得到包含中心方下侧边界的图像，测定图像中点的中心方下边界位置。同时得到距离中心线左右各1.5cm的垂线和黑方块的交点，轻易计算出倾角
            //对小图纠倾，然后上面1毫米处得出水平线。从中央，向两侧找到两边位置坐标，就可以知道水平偏离中心的情况
            //左侧边界向内一毫米位置，从原始图上面取列，可以得出上下的位置坐标，算出偏离垂直中心的情况
            //对原图，先纠正（角度），再纠偏（位置）
            //最后，依据中心方的大小，来矫正缩放的比例，其边长为高度的大约0.1（0.10020）
            WriteableBitmap r, parc, ppos;
            mp.SourcePhoto = WriteableBitmapHelper.Clone(b);
            var tt = mp.SourcePhoto.PixelBuffer.ToArray();
            WriteableBitmap cb = CorrectArcRegion(b);//只切下面的1/4高度来检查倾斜性
            cb=AutoBright(cb);//给出自动白平衡
            mp.CorrectBrightPhoto = WriteableBitmapHelper.Clone(cb);
            float arc = 0;
            Point tcp = PhotoTest.NullPoint;
            Point tlp = PhotoTest.NullPoint;
            Point trp = PhotoTest.NullPoint;

            List<int> al = getImageGrayVLine(cb, cb.PixelWidth / 2);//中点的位置取垂直像素列
            int cy = FindLastBlack(al, 100);//找到越变点位置，注意是从大向小找。
            if (cy != -1)
            {
                tcp = new Point(cb.PixelWidth / 2, cy);//这是中央相交点
            }
            double bs = 1.5 / 39.9;
            int bps = Convert.ToInt32(bs * b.PixelHeight);

            List<int> lal = getImageGrayVLine(cb, cb.PixelWidth / 2 - bps);//中点的左侧位置取垂直像素列
            int ly = FindLastBlack(lal, 100);//找到越变点位置，注意是从大向小找。
            if (ly != -1)
            {
                tlp = new Point(cb.PixelWidth / 2 - bps, ly);//这是左侧相交点
                if (Math.Abs((tcp.Y - tlp.Y) / (tcp.X - tlp.X)) > 0.75)//检查，如果找到的倾斜角度超过37度，矫正失败
                {
                    tlp = new Point();
                }
            }

            List<int> ral = getImageGrayVLine(cb, cb.PixelWidth / 2 + bps);//中点的右侧位置取垂直像素列
            int ry = FindLastBlack(ral, 100);//找到越变点位置，注意是从大向小找。
            if (ry != -1)
            {
                trp = new Point(cb.PixelWidth / 2 + bps, ry);//这是右侧相交点
                if (Math.Abs((trp.Y - tcp.Y) / (trp.X - tcp.X)) > 0.75)//检查，如果找到的倾斜角度超过37度，矫正失败
                {
                    tlp = PhotoTest.NullPoint; 
                }
            }

            if (!IsNullPoint(trp) && !IsNullPoint(tlp))
            {
                arc = Convert.ToSingle((Math.Atan((trp.Y - tlp.Y) / (trp.X - tlp.X))));
            }

            if (IsNullPoint(trp))
            {
                arc = Convert.ToSingle((Math.Atan((tcp.Y - tlp.Y) / (tcp.X - tlp.X))));
            }

            if (IsNullPoint(tlp))
            {
                arc = Convert.ToSingle((Math.Atan((trp.Y - tcp.Y) / (trp.X - tcp.X))));
            }

            //cb = RotateBitmap(cb, -arc);
            //mp.CorrectRotatePhoto = WriteableBitmapHelper.Clone(cb);

            double bhs = 0.1 / 39.9;
            bps = Convert.ToInt32(bhs * b.PixelHeight);

            al = getImageGrayHLine(cb, cy - bps);//得到水平像素行

            ral = new List<int>(al);//克隆水平像素，主要是保留右侧部分
            lal = new List<int>(al);//克隆水平像素，主要是保留左侧部分
            ral.RemoveRange(0, al.Count / 2 - 1);

            lal.RemoveRange(al.Count / 2 + 1, al.Count / 2 - 1);
            if (al.Count % 2 == 1)
            {
                lal.RemoveAt(lal.Count - 1);
            }
            int lbp, rbp;
            lbp = FindLastWhite(lal, 100);
            rbp = FindFirstWhite(ral, 100);

            bool IsCorrectMoveScale = true;
            if (lbp == -1 && rbp == -1) { IsCorrectMoveScale = false; }
            int SQLength = rbp + al.Count / 2 - lbp;
            int dx, dy;
            dx = (lbp + rbp + al.Count / 2) / 2 - cb.PixelWidth / 2;

            double YISOscale = 20.5 / 39.9;
            int topy = Convert.ToInt32(YISOscale * b.PixelHeight);
            int SQRTop = cy - SQLength + topy;
            dy = ((cy + topy) * 2 - SQLength) / 2 - b.PixelHeight / 2;

            //cb.Dispose();

            this.MovePosition = new Point(dx, dy);
            this.RotateArc = arc;

            if (IsCorrectMoveScale)
            {
                ppos = MoveTrans(b, -dx, -dy);
            mp.CorrectMovePhoto = WriteableBitmapHelper.Clone(ppos);
            }
            else
            {
                ppos = b;
            }
            parc = RotateTrans(ppos, -arc);
            mp.CorrectRotatePhoto = WriteableBitmapHelper.Clone(parc);
            if (IsCorrectMoveScale)
            {
            float sd = SQLength / (b.PixelHeight * 0.1002f);
            this.ScaleNum = sd;
            r = ScaleTrans(parc, 1, 1);   //进行缩放匹配
            }
            else
            {
                r = parc;
            }
            mp.CorrectScalePhoto = WriteableBitmapHelper.Clone(r);

            return r;
        }

        public List<double> getCurveMTF()//得到特定的MTF曲线
        {
            WriteableBitmap b = getAreaVEdge(CorrectPhoto);//这里找到的是水平的边界，因此裁剪的时候垂直进行
            return ptp.getCurveMTF(b, false);
        }
        
        //public List<List<int>> getCurveHDispersiveness()//色散的曲线包括了三条，RGB，每一个是个列表，他们合起来也是一个新的列表
        //{
        //    WriteableBitmap b = getHAreaDispersiveness(CorrectPhoto);//这里找到的是水平的边界，因此裁剪的时候垂直进行
        //    List<List<int>> al = new List<List<int>>();
        //    al.Add(getRedHEdge(b));
        //    al.Add(getGreenHEdge(b));
        //    al.Add(getBlueHEdge(b));
        //    return al;
        //}

        //public List<List<int>> getCurveVDispersiveness()//色散的曲线包括了三条，RGB，每一个是个列表，他们合起来也是一个新的列表
        //{
        //    WriteableBitmap b = getVAreaDispersiveness(CorrectPhoto);//这里找到的是水平的边界，因此裁剪的时候垂直进行
        //    List<List<int>> al = new List<List<int>>();
        //    al.Add(getRedVEdge(b));
        //    al.Add(getGreenVEdge(b));
        //    al.Add(getBlueVEdge(b));
        //    return al;
        //}

        public int getHEdgeResoveLines()//得到分辨率，使用线扩散函数计算
        {
            
            WriteableBitmap b = getAreaHEdge(CorrectPhoto);
            //b=AutoBright(b);
            int x = ptp.getEdgeResoveLines(b, true, ChartPhoto.PixelHeight);
            return x;
        }

        public int getVEdgeResoveLines()//得到分辨率，使用线扩散函数计算
        {
            WriteableBitmap b = getAreaVEdge(CorrectPhoto);
            //b=AutoBright(b);
            int x = ptp.getEdgeResoveLines(b, false, ChartPhoto.PixelHeight);
            return x;
        }

        public float ResovePercent
        {
            get
            {
                return ResoveBrightPercent; ;
            }
            set
            {
                if ((value > 0) && (value < 100))
                {
                    ResoveBrightPercent = value;
                }
                else
                {
                    ResoveBrightPercent = 73.5f;
                }
            }
        }

        public WriteableBitmap getAreaVEdge()
        {
            if (ChartPhoto == null)
            {
                ChartPhoto = mp.SourcePhoto;
            }
            return getAreaVEdge(ChartPhoto);
        }

        public WriteableBitmap getAreaHEdge()
        {
            if (ChartPhoto == null)
            {
                ChartPhoto = mp.SourcePhoto;
            }
            return getAreaHEdge(ChartPhoto);
        }

        public WriteableBitmap getAreaHEdge(WriteableBitmap b)//在ISO12233图片里面选择一个区域来计算色散
        {
            int sx = Convert.ToInt32(b.PixelHeight / 9 * 16 * 37.5 / 71);
            sx = getCenterX(sx, b.PixelWidth, b.PixelHeight);
            int sy = Convert.ToInt32(b.PixelHeight * 25.2 / 39.9);
            int h = Convert.ToInt32(b.PixelHeight * 1.6 / 39.9);
            int w = Convert.ToInt32(b.PixelHeight * 3.5 / 39.9);
            WriteableBitmap tb = getImageArea(b, sx, sy, w, h);
            if (IsAnalyse) { DrawSelectArea(sx, sy, w, h); }
            setSelectArea("AreaHEdge", tb);

            return tb;
        }

        public WriteableBitmap getAreaVEdge(WriteableBitmap b)//在ISO12233图片里面选择一个区域来计算色散
        {
            int sx = Convert.ToInt32(b.PixelHeight / 9 * 16 * 34.5 / 71);
            sx = getCenterX(sx, b.PixelWidth, b.PixelHeight);
            int sy = Convert.ToInt32(b.PixelHeight * 4.1 / 39.9);
            int h = Convert.ToInt32(b.PixelHeight * 4.9 / 39.9);
            int w = Convert.ToInt32(b.PixelHeight * 1.2 / 39.9);
            WriteableBitmap tb = getImageArea(b, sx, sy, w, h);
            if (IsAnalyse) { DrawSelectArea(sx, sy, w, h); }

            setSelectArea("AreaVEdge", tb);
            return tb;
        }
    }
}
