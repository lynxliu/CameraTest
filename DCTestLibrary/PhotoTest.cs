using System;
using System.Collections.Generic;
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
using System.Runtime.InteropServices.WindowsRuntime;
using SilverlightLFC.common;
using System.Threading.Tasks;

namespace DCTestLibrary
{
    public delegate void PhotoTestProcessHandler(object sender, PhotoTestProcessEventArgs e);//定义的test事件
    public class PhotoTestProcessEventArgs : EventArgs//通用的事件类，传送全部的参数
    {
        public string msg;
        public DateTime EventTime = DateTime.Now;//事件发生时刻
        public bool IsSuccess;//标识调用是否成功
        public object ReturnValue;//返回值
        public PhotoTestProcessEventArgs(bool b, string s)
        {
            IsSuccess = b;
            msg = s;
        }
    }
    public class PhotoTest//测试照片里面的基本参数的类，也包括基本的图像处理
    {
        LabMode _CurrentLabMode = LabMode.Undefine;//lab的颜色模式选择
        public LabMode CurrentLabMode
        {
            get
            {
                if (_CurrentLabMode == LabMode.Undefine) return ColorManager.CurrentLanMode;
                return _CurrentLabMode;
            }
            set
            {
                _CurrentLabMode = value;
            }
        }

        public event PhotoTestProcessHandler PhotoTestProc;//定义简单的查询事件
        public void sendProcEvent(bool r, string s)//给出简单的查询返回，成功，失败和返回的结果集
        {
            PhotoTestProcessEventArgs leq = new PhotoTestProcessEventArgs(r, s);
            if (PhotoTestProc != null)
            {
                PhotoTestProc(this, leq);
            }
        }

        public Dictionary<string, object> ProcessInfor=new Dictionary<string,object>();//提供中间的处理信息
        public void setProcessInfor(string key, object o)//自动设置选区，并且改变不会影响原始信息
        {
            if (ProcessInfor.ContainsKey(key))
            {
                ProcessInfor[key] = o;
            }
            else
            {
                ProcessInfor.Add(key, o);
            }
        }

        public static Point NullPoint = new Point(Double.NaN,double.NaN);
        public static bool IsNullPoint(Point p)
        {
            if (Double.IsNaN(p.X) || Double.IsNaN(p.Y)) return true;
            return false;
        }
        public PhotoTest()//包含所有的进行影像处理的库
        {
        }

        public int getBright(Color c)
        {
            return (byte)(.299 * c.R + .587 * c.G + .114 * c.B);

        }

        public WriteableBitmap Gray(WriteableBitmap b)//整个图像转为灰度图
        {
            if (b == null) { return null; }
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var p = wb.PixelBuffer.ToArray();

            for (int i = 0; i < p.Length; i+=4)
            {
                p[i] = p[i + 1] = p[i + 2] = (byte)(.299 * (int)p[i+2] + .587 * p[i+1] + .114 * p[i]); 
            }
            p.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public WriteableBitmap BWBitmap(WriteableBitmap b, int h)//整个图像转为黑白二色图，h为黑白的分界线，推荐是100
        {
            if (b == null) { return null; }
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                var v= (byte)(.299 * components[i+2] + .587 * components[i+1] + .114 * components[i]);
                if (v < h)
                {
                    components[i] = components[i+1] = components[i+2] = 0;
                }
                else
                {
                    components[i] = components[i+1] = components[i+2] = 255;
                }
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;

        }

        public WriteableBitmap BrightnessBitmap(WriteableBitmap b, int nBrightness)//把图片改变亮度
        {
            if (b == null) { return null; }
            if (nBrightness < -255 || nBrightness > 255)
                return null;
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                int nVal = (int)(components[i] + nBrightness);
                if (nVal < 0) nVal = 0;
                if (nVal > 255) nVal = 255;
                components[i] = (byte)nVal;

                nVal = (int)(components[i+1] + nBrightness);
                if (nVal < 0) nVal = 0;
                if (nVal > 255) nVal = 255;
                components[i+1] = (byte)nVal;

                nVal = (int)(components[i+2] + nBrightness);
                if (nVal < 0) nVal = 0;
                if (nVal > 255) nVal = 255;
                components[i+2] = (byte)nVal;
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public WriteableBitmap RedBitmap(WriteableBitmap b, int nBrightness)//把图片改变红色分量亮度
        {
            if (b == null) { return null; }
            if (nBrightness < -255 || nBrightness > 255)
                return null;
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                int nVal = (int)(components[i+2] + nBrightness);
                if (nVal < 0) nVal = 0;
                if (nVal > 255) nVal = 255;
                components[i+2] = (byte)nVal;
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public WriteableBitmap GreenBitmap(WriteableBitmap b, int nBrightness)//把图片改变红色分量亮度
        {
            if (b == null) { return null; }
            if (nBrightness < -255 || nBrightness > 255)
                return null;
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                int nVal = (int)(components[i+1] + nBrightness);
                if (nVal < 0) nVal = 0;
                if (nVal > 255) nVal = 255;
                components[i+1] = (byte)nVal;
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public WriteableBitmap BlueBitmap(WriteableBitmap b, int nBrightness)//把图片改变红色分量亮度
        {
            if (b == null) { return null; }
            if (nBrightness < -255 || nBrightness > 255)
                return null;
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                int nVal = (int)(components[i] + nBrightness);
                if (nVal < 0) nVal = 0;
                if (nVal > 255) nVal = 255;
                components[i] = (byte)nVal;
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public WriteableBitmap Invert(WriteableBitmap b)//图像的亮度反转
        {
            if (b == null) { return null; }
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                components[i] = (byte)(255 - components[i]);
                components[i+1] = (byte)(255 - components[i+1]);
                components[i+2] = (byte)(255 - components[i+2]);
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public WriteableBitmap Blur(WriteableBitmap b, int grade)//把图片模糊，grade为模糊程度，实际是模糊2*grade+1的矩阵
        {
            if (b == null) { return null; }
            if (grade < 0 || grade > b.PixelWidth / 2)
                return null;

            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();
            for (int y = grade; y < wb.PixelHeight - grade; y++)//遍历每一个像素
            {
                for (int x = grade; x < wb.PixelWidth - grade; x++)
                {
                    var index = y * wb.PixelWidth * 4 + (x * 4);
                    int blue = 0;
                    int green = 0;
                    int red = 0;
                    for (int k = -grade; k < grade + 1; k++)
                    {
                        for (int l = -grade; l < grade + 1; l++)
                        {
                            int sp = (y + k) * wb.PixelWidth*4 + ((x + l)*4);//计算当前位置

                            blue = blue + components[sp];
                            green = green + components[sp+1];
                            red = red + components[sp+2];

                        }
                    }
                    blue = blue / ((2 * grade + 1) * (2 * grade + 1));
                    green = green / ((2 * grade + 1) * (2 * grade + 1));
                    red = red / ((2 * grade + 1) * (2 * grade + 1));

                    components[index] = (byte)(red);
                    components[index+1] = (byte)(green);
                    components[index+2] = (byte)(blue);
                }
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public WriteableBitmap Mosaic(WriteableBitmap b, int grade)//把图片模糊，grade为模糊程度，实际是模糊2*grade+1的矩阵
        {
            if (b == null) { return null; }
            if (grade < 0 || grade > b.PixelWidth / 2)
                return null;

            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();
            for (int y = grade; y < wb.PixelHeight - grade; y = y + 2 * grade + 1)//遍历每一个像素
            {
                for (int x = grade; x < wb.PixelWidth - grade; x = x + 2 * grade + 1)
                {
                    int blue = 0;
                    int green = 0;
                    int red = 0;
                    for (int k = -grade; k < grade + 1; k++)
                    {
                        for (int l = -grade; l < grade + 1; l++)
                        {
                            int sp = (y + k) * wb.PixelWidth + (x + l);//计算当前位置

                            blue = blue + components[sp+2];
                            green = green + components[sp+1];
                            red = red + components[sp];

                        }
                    }

                    blue = blue / ((2 * grade + 1) * (2 * grade + 1));
                    green = green / ((2 * grade + 1) * (2 * grade + 1));
                    red = red / ((2 * grade + 1) * (2 * grade + 1));

                    for (int k = -grade; k < grade + 1; k++)
                    {
                        for (int l = -grade; l < grade + 1; l++)
                        {
                            int sp = (y + k) * wb.PixelWidth + (x + l);

                            components[sp] = (byte)(red);
                            components[sp+1] = (byte)(green);
                            components[sp+2] = (byte)(blue);
                        }
                    }
                }
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public List<Color> getImageColorList(WriteableBitmap b)//把图像转换为颜色列表
        {
            List<Color> al = new List<Color>();
            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i++)
            {
                Color c = new Color();
                c.R = components[i+2];
                c.G = components[i+1];
                c.B = components[i];
                al.Add(c);
            }
            return al;
        }

        public List<Color> getImageColorHLine(WriteableBitmap b, long line)//从图像里面指定的一水平行来获取一个向量。需要注意的是这个向量是个数值向量。
        {
            List<Color> al = new List<Color>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelWidth; i++)
            {
                var index=4*line * b.PixelWidth + 4*i;
                Color c = new Color();
                c.R = components[index+2];
                c.G = components[index+1];
                c.B = components[index];
                al.Add(c);
            }
            return al;
        }

        public List<int> getImageGrayHLine(WriteableBitmap b, long line)//从图像里面指定的一水平行来获取一个向量。需要注意的是这个向量是个数值向量。
        {
            List<int> al = new List<int>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelWidth; i++)
            {
                var index=line * b.PixelWidth*4 + (i*4);
                byte c = (byte)(.299 * components[index+2] + .587 * components[index+1] + .114 * components[index]);//按照特定的比例从rgb转换为灰度，实际灰度就是rgb相等                Color c = new Color();
                al.Add(c);
            }
            //wb.Invalidate();
            return al;
        }

        public List<int> getImageRedHLine(WriteableBitmap b, long line)//从图像里面指定的一水平行来获取一个向量的红色分量。需要注意的是这个向量是个数值向量。
        {
            List<int> al = new List<int>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelWidth; i++)
            {
                al.Add(components[(line * b.PixelWidth + i)*4+2]);
            }
            return al;
        }

        public List<int> getImageGreenHLine(WriteableBitmap b, long line)//从图像里面指定的一水平行来获取一个向量的绿色分量。需要注意的是这个向量是个数值向量。
        {
            List<int> al = new List<int>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelWidth; i++)
            {
                al.Add(components[(line * b.PixelWidth + i)*4+1]);
            }
            return al;
        }

        public List<int> getImageBlueHLine(WriteableBitmap b, long line)//从图像里面指定的一水平行来获取一个向量的蓝色分量。需要注意的是这个向量是个数值向量。
        {
            List<int> al = new List<int>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelWidth; i++)
            {
                al.Add(components[(line * b.PixelWidth + i)*4]);
            }
            //wb.Invalidate();
            return al;
        }

        public List<Color> getImageColorVLine(WriteableBitmap b, long line)//从图像里面指定的一列来获取一个向量。需要注意的是这个向量是个数值向量。
        {
            List<Color> al = new List<Color>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelHeight; i++)
            {
                var index = i * b.PixelWidth * 4 + line * 4;
                Color c = new Color();
                c.R = components[index+2];
                c.G = components[index+1];
                c.B = components[index];
                al.Add(c);
            }
            //wb.Invalidate();
            return al;
        }

        public List<int> getImageGrayVLine(WriteableBitmap b, long line)//从图像里面指定的一列来获取一个向量。需要注意的是这个向量是个数值向量。
        {
            List<int> al = new List<int>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelHeight; i++)
            {
                var index=(i * b.PixelWidth + line)*4;
                byte c = (byte)(.299 * components[index+2] + .587 * components[index+1] + .114 * components[index]);//按照特定的比例从rgb转换为灰度，实际灰度就是rgb相等                Color c = new Color();
                al.Add(c);
            }
            //wb.Invalidate();
            return al;
        }

        public List<int> getImageRedVLine(WriteableBitmap b, long line)//从图像里面指定的一列来获取一个红色分量的向量。需要注意的是这个向量是个数值向量。
        {
            List<int> al = new List<int>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelHeight; i++)
            {
                al.Add(components[(i * b.PixelWidth + line)*4+2]);
            }
            //wb.Invalidate();
            return al;
        }

        public List<int> getImageGreenVLine(WriteableBitmap b, long line)//从图像里面指定的一列来获取一个绿色分量的向量。需要注意的是这个向量是个数值向量。
        {
            List<int> al = new List<int>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelHeight; i++)
            {
                al.Add(components[(i * b.PixelWidth + line)*4+1]);
            }
            //wb.Invalidate();
            return al;
        }

        public List<int> getImageBlueVLine(WriteableBitmap b, long line)//从图像里面指定的一列来获取一个蓝色分量的向量。需要注意的是这个向量是个数值向量。
        {
            List<int> al = new List<int>();

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelHeight; i++)
            {
                al.Add(components[(i * b.PixelWidth + line)*4]);
            }
            //b.Invalidate();
            return al;
        }

        public long getBoderLine(WriteableBitmap b, Boolean BeginFromLeft, Boolean BoderIsBlack)//给出边界的位置
        //对边界的判读，假设边界是黑色或者白色，一旦有不是的像素，那么就进行判读，找到这个列，这就是非边界的起始点
        {
            long i = 0;
            long Step = 1;
            long r = 0;
            long BoderBright = 0;
            if (BoderIsBlack)
            {
                BoderBright = 220;
            }
            else
            {
                BoderBright = 25;
            }
            if (BeginFromLeft)
            {
                i = 0;
                Step = 1;
            }
            else
            {
                i = b.PixelWidth - 1;
                Step = -1;
            }
            for (int j = 0; j < b.PixelWidth; j++)
            {
                List<int> al = this.getImageGrayVLine(b, i);
                for (int x = 0; x < al.Count; x++)
                {
                    int Br = Convert.ToInt32(al[x]);
                    if (BoderIsBlack)
                    {
                        if (Br > BoderBright)
                        {
                            r = i;
                            x = al.Count;//终止循环
                            j = b.PixelWidth;//终止循环
                        }
                    }
                    else
                    {
                        if (Br > BoderBright)
                        {
                            r = i;
                            x = al.Count;//终止循环
                            j = b.PixelWidth;//终止循环
                        }
                    }

                }

                i = i + Step;
            }
            return r;
        }

        public List<int> CropBoder(List<int> al)//去除数组两侧的特定颜色区域，类似trim的作用，这里假设背景肯定是白色的
        //算法是双向扫描，从前向后找到第一个非白的元素，记录位置，再从后向前，找到第一个非白元素记录位置
        //然后彻底去除两个位置之外的元素。
        {
            int firstNo = 0;
            int lastNo = 0;
            for (int i = 0; i < al.Count; i++)
            {

                if (al[i] < 150)//表示已经很黑，也就是不属于背景和噪点
                {
                    firstNo = i;
                    i = al.Count;
                }
            }

            for (int i = al.Count - 1; i >= 0; i--)
            {
                if (al[i] < 150)//表示已经很黑，也就是不属于背景和噪点
                {
                    lastNo = i;
                    i = 0;
                }
            }
            //移除两侧的元素
            if (al.Count > 0)
            {
                al.RemoveRange(lastNo + 1, al.Count - lastNo - 1);
                al.RemoveRange(0, firstNo);
            }
            return al;
        }

        protected void TrimGrayList(List<int> al, int h, bool BackIsWhite, int BackBright)//去除al两侧的被淹没到背景里面的像素
        {
            int bb, eb;//bb是合法亮度的起始值，eb是结束值
            if (BackIsWhite)
            {
                bb = 0;
                eb = BackBright - h;
            }
            else
            {
                bb = BackBright + h;
                eb = 255;
            }
            //int n = al.Count;
            for (int i = 0; i < al.Count; i++)
            {
                int b;
                b = al[i];
                if ((b > bb) && (b < eb))//表示已经很黑，也就是不属于背景和噪点
                {
                    break;
                }
                al.RemoveAt(i);
                i--;
            }
            //n = al.Count;
            for (int i = al.Count - 1; i >= 0; i--)
            {
                int b;
                b = al[i];
                if ((b > bb) && (b < eb))//表示已经很黑，也就是不属于背景和噪点
                {
                    break;
                }
                al.RemoveAt(i);
                //i++;
            }
        }

        protected void TrimColorList(List<Color> al, int h, bool BackIsWhite, Color BackColor)//去除al两侧的被淹没到背景里面的像素
        {
            int n = al.Count;
            for (int i = 0; i < n; i++)
            {
                Color b;
                b = (Color)al[i];
                if (BackIsWhite)
                {
                    if (IsWhite(b, h))
                    {
                        al.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (IsBlack(b, h))
                    {
                        al.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            n = al.Count;
            for (int i = n - 1; i >= 0; i--)
            {
                Color b;
                b = (Color)al[i];
                if (BackIsWhite)
                {
                    if (IsWhite(b, h))
                    {
                        al.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (IsBlack(b, h))
                    {
                        al.RemoveAt(i);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        protected List<int> getSoveUnit(List<int> al, int BlackLines)//此函数用来从一个数组里面切出一个黑色到黑色的部分
        {//现在是根据有几条黑线来直接切分，最好的方式是可以自己识别，有难度。除非先做一次平滑处理，否则局部的起伏直接影响全局。
            List<int> Ual = new List<int>();
            int Tar = BlackLines - 1;
            int Num = al.Count / Tar;
            //Array x[];
            //x= Ual.ToArray();
            //Ual = al.CopyTo(0, x, 0, Num);
            return Ual;
        }
        //考虑
        //游程算法，要先确认数组的两侧多少像素是无效的。问题是如何来确定这一点。看来还是计算导数比较合理
        //通过各点计算导数，确定对称的倒数
        //采用三联邻域法来计算，避免出现问题，也就是通过两个相邻的像素来判读本点的趋向。理论上面，
        //由于拍摄是在光照充足的范围进行的，因此，认为不会出现毛刺，后面的像素肯定可以反应影像的趋向。
        protected List<long> DeepCropBoder(List<int> al)//删除两侧全部的干扰数据，依据分析出来的平均夸距
        {
            return new List<long>();
        }

        public WriteableBitmap FlipYImage(WriteableBitmap b)
        {//水平翻转图像
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var p = b.PixelBuffer.ToArray();
            var tp = wb.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelHeight; i++)
            {
                int of = i * b.PixelWidth*4;
                for (int j = 0; j < b.PixelWidth; j+=4)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        tp[of + j+k] = p[of + b.PixelWidth - 1 - j+k];
                    }
                }
            }
            tp.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public WriteableBitmap FlipXImage(WriteableBitmap b)
        {//垂直翻转图像
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var p = b.PixelBuffer.ToArray();
            var tp = wb.PixelBuffer.ToArray();
            for (int i = 0; i < b.PixelHeight; i++)
            {
                int of = i * b.PixelWidth*4;
                int fof = (b.PixelHeight - 1 - i) * b.PixelWidth*4;
                for (int j = 0; j < b.PixelWidth; j++)
                {

                    tp[of + j] = p[fof + j];
                }
            }
            tp.CopyTo(wb.PixelBuffer);
            return wb;
        }

        protected long AverageValue(List<long> al)//求平均数
        {
            if (al.Count == 0) { return 0; }
            long l = 0;
            for (int i = 0; i < al.Count; i++)
            {
                l = l + al[i];
            }
            return l / al.Count;
        }

        protected long AverageValue(List<int> al)//求平均数
        {
            if (al.Count == 0) { return 0; }
            long l = 0;
            for (int i = 0; i < al.Count; i++)
            {
                l = l + al[i];
            }
            return l / al.Count;
        }

        protected bool ConfirmTrend(List<int> al, int sp,int d, int step, bool IsIncreaseTrend)//趋势的确认，避免异常波动的影响
        {//d是波动的幅度，在起始点后面step范围里面，任何一个点和原始点的差异大于顺趋势的d，就确认趋势
            int b = al[sp];
            for (int i = 1; i < step; i++)
            {
                if (sp + i < al.Count)
                {
                    int c = al[sp + i];
                    if (IsIncreaseTrend)
                    {
                        if (b < c - d) { return true; }
                    }
                    else
                    {
                        if (b > c + d) { return true; }
                    }
                }
            }
            return false;
        }

        protected void AnalyseArray(Dictionary<long, int> htMAX, Dictionary<long, int> htMIN, List<int> al)//通过该函数可以找出所有的顶点的位置，后面就是分析这些顶点哪些是真的，哪些是假的
        {
            //如果出现的是相等的平台，记录的是变化的后缘
            //需要注意的是需要检查黑色和白色出现的相对位置，只有和前面的位置等距才是合法的点。
            //为了避免噪点影响，条件改变时候差异应该达到最终差异的5%才可以确认趋势
            htMAX.Clear();
            htMIN.Clear();
            Boolean IsIncrease = true;
            int step=5;
            int d=5;
            for (int i = 0; i < al.Count - 1; i++)
            {

                int cur = al[i];
                int next = al[i + 1];

                if ((cur <= next) && IsIncrease)
                {
                    IsIncrease = true;
                }
                if ((cur > next) && IsIncrease)
                {
                    if (ConfirmTrend(al, i, d, step, false))
                    {
                        htMAX.Add(i, cur);
                        IsIncrease = false;
                    }

                }
                if ((cur > next) && (IsIncrease == false))
                {
                    IsIncrease = false;
                }
                if ((cur <= next) && (IsIncrease == false))
                {
                    if (ConfirmTrend(al, i, d, step, true))
                    {
                        htMIN.Add(i, cur);
                        IsIncrease = true;
                    }
                }
            }
        }

        protected void AnalyseArray(List<int> alMax, List<int> alMin, List<long> alMaxP, List<long> alMinP, List<int> al)//通过该函数可以找出所有的顶点的位置，后面就是分析这些顶点哪些是真的，哪些是假的
        {
            //如果出现的是相等的平台，记录的是变化的后缘
            //需要注意的是需要检查黑色和白色出现的相对位置，只有和前面的位置等距才是合法的点。

            Boolean IsIncrease = true;
            for (int i = 0; i < al.Count - 1; i++)
            {

                int cur = al[i];
                int next = al[i + 1];
                if ((cur <= next) && IsIncrease)
                {
                    IsIncrease = true;
                }
                if ((cur > next) && IsIncrease)
                {
                    alMax.Add(cur);
                    alMaxP.Add(i);
                    IsIncrease = false;

                }
                if ((cur > next) && (IsIncrease == false))
                {
                    IsIncrease = false;
                }
                if ((cur <= next) && (IsIncrease == false))
                {
                    alMin.Add(cur);
                    alMinP.Add(i);
                    IsIncrease = true;
                }
            }
        }

        protected int getSoveLineDistance(Dictionary<long, int> ht)//得到分辨率线的平均间距
        {
            if (ht.Count == 0) { return 0; }
            List<int> DisList = new List<int>();
            int x0 = 0;
            List<long> kal = new List<long>(ht.Keys);
            for (int i = 0; i < ht.Count; i++)
            {
                int x1 = Convert.ToInt32(kal[i]);
                DisList.Add(x1 - x0);
                x0 = x1;
            }//依次记录其间距

            int ave = 0;
            for (int i = 0; i < DisList.Count; i++)
            {
                ave = ave + Convert.ToInt32(DisList[i]);
            }
            ave = ave / DisList.Count;//得到平均距离，距离平均距离超过50的就排斥掉，既包括小于的，也包括大于的
            return ave;
        }

        protected void AnalyseChangePix(Dictionary<long, int> htChange, List<int> al, int h)//直接获得所有急剧变化的像素，原理是后面的像素和前面的像素差很明显，大于阀值h
        {
            //这是因为实际的标版上面，都是非黑即白的东西，不存在过度灰度
            htChange.Clear();
            for (int i = 0; i < al.Count - 1; i++)
            {
                int cur = al[i];
                int next = al[i + 1];
                if ((cur <= next - h) || (cur > next + h))
                {
                    htChange.Add(i, cur);
                }
            }
        }
         
        protected int getHBlackLineNum(WriteableBitmap b,double per)//得到黑色水平线条的数字
        {//检测左侧边界向右一点的像素，找出最大值的数字
            //per是位置的监测点
            long left = getLeftPoint(b);
            Dictionary<long, int> hMAX, hMIN;
            hMAX = new Dictionary<long, int>();
            hMIN = new Dictionary<long, int>();
            List<int> al = new List<int>();
            int ld = Convert.ToInt32(b.PixelWidth * per);
            al = this.getImageGrayVLine(b, left + ld);
            al = this.CropBoder(al);
            this.AnalyseArray(hMAX, hMIN, al);
            //完成了初步的筛选，但是要验证其间距是合理的，分析的依据是平均间距、

            //int r = 0;//定义波峰到波峰的平均间距
            List<int> DisList = new List<int>();
            int x0 = 0;
            DisList.Clear();
            List<long> kal = new List<long>(hMIN.Keys);//加载所有黑线的位置，由于加入的关系，应该是顺序加载的
            kal.Sort();

            for (int i = 0; i < kal.Count - 1; i++)
            {
                x0 = Convert.ToInt32(kal[i]);
                int x1 = Convert.ToInt32(kal[i + 1]);
                DisList.Add(x1 - x0);
            }//依次记录其间距

            //需要注意的是，因为边界选择采用的是灵敏算法，因此不会漏掉，只会更多，所以，分析标准是各个间距不得小于最大间距的75%，否则判定为噪声
            int ave = 0;
            if (DisList.Count == 0) { return 0; }
            for (int i = 0; i < DisList.Count; i++)
            {
                ave = ave + Convert.ToInt32(DisList[i]);
            }
            ave = ave / DisList.Count;//得到平均距离，距离平均距离超过50的就排斥掉，既包括小于的，也包括大于的

            int max = Convert.ToInt32(DisList[0]);
            for (int i = 0; i < DisList.Count; i++)
            {
                if (max < Convert.ToInt32(DisList[i]))
                {
                    max = Convert.ToInt32(DisList[i]);
                }
            }
            for (int i = 0; i < DisList.Count; i++)
            {
                if (max * 0.75 > Convert.ToInt32(DisList[i]))
                {
                    DisList.RemoveAt(i);
                }
            }
            int count = 0;
            count = DisList.Count + 1;
            return count;
        }

        protected long getLeftPoint(WriteableBitmap b)//得到左边界，这个边界应该是对正以后的边界
        {
            List<int> al;
            int PC = 0;
            for (int i = 0; i < b.PixelWidth; i++)
            {
                PC = 0;
                al = this.getImageGrayVLine(b, i);
                for (int j = 0; j < al.Count; j++)
                {

                    if (Convert.ToInt32(al[j]) < 100)//表示该点是黑色的
                    {
                        PC++;
                    }
                }
                if (PC > 3)//大于阀值的超过5像素
                {
                    return i;
                }
            }
            return 0;
        }

        protected long getRightPoint(WriteableBitmap b)//得到右边界，注意，必须是中央比两侧更黑，不能中央平均更白
            //一旦进入黑块就是中央不比两侧更黑了
        {
            List<int> al;
            int PC = 0;
            int i = 0;

            int TopEnd,BottomBegin,centBegin, centEnd;//中间亮度大代表不合法
            TopEnd = Convert.ToInt32(b.PixelHeight * 0.2);
            BottomBegin = Convert.ToInt32(b.PixelHeight * 0.8);
            centBegin = Convert.ToInt32(b.PixelHeight * 0.3);
            centEnd = Convert.ToInt32(b.PixelHeight * 0.7);

            for (i = b.PixelWidth - 1; i >= 0; i--)
            {
                PC = 0;
                al = this.getImageGrayVLine(b, i);

                double bb=0, cb=0;
                for (int j = 0; j < TopEnd;j++)
                {
                    bb = bb + al[j];
                }
                for (int j = BottomBegin; j < al.Count; j++)
                {
                    bb = bb + al[j];
                }
                bb = bb / (TopEnd + al.Count-BottomBegin);

                for (int j = centBegin; j < centEnd; j++)
                {
                    cb = cb + al[j];
                }
                cb = cb / (centEnd - centBegin);
                if (cb > bb-5) { continue; }

                for (int j = 0; j < al.Count; j++)
                {

                    if (Convert.ToInt64(al[j]) > 100)
                    {
                        PC++;
                    }
                }
                if (PC > 3)
                {
                    return i;
                }
            }
            return 0;
        }

        protected Boolean IsLeftWider(WriteableBitmap b)//判断左右方哪一方比较宽
        {
            long left, right = 0;
            left = this.getLeftPoint(b);
            right = this.getRightPoint(b);
            List<int> all, alr;
            all = this.getImageGrayVLine(b, left + 1);
            this.CropBoder(all);//过滤两侧的边界
            alr = this.getImageGrayVLine(b, right - 1);
            this.CropBoder(alr);
            if (all.Count >= alr.Count)
            {
                return true;
            }
            else { return false; }
        }
        public WriteableBitmap getImagePercentArea(WriteableBitmap b, double StartXPercent, double StartYPercent, double wPercent, double hPercent)//从已知的图像里面取出指定的部分,这个制定的部分是个矩形，给的是百分比坐标
        {
            int StartX = Convert.ToInt32(b.PixelWidth * StartXPercent);
            int StartY = Convert.ToInt32(b.PixelHeight * StartYPercent);
            int w = Convert.ToInt32(b.PixelWidth * wPercent);
            int h = Convert.ToInt32(b.PixelHeight * hPercent);


            return getImageArea(b,StartX,StartY,w,h);

        }

        public WriteableBitmap getImageArea(WriteableBitmap b, int StartX, int StartY, int w, int h)//从已知的图像里面取出指定的部分,这个制定的部分是个矩形
        {
            if (StartX < 0) { StartX = 0; }
            if (StartY < 0)
            {
                StartY = 0;
            }
            if (StartX > b.PixelWidth - 1)
            {
                StartX = b.PixelWidth - 1;
            }
            if (StartY > b.PixelHeight - 1)
            {
                StartY = b.PixelHeight - 1;
            }
            if (StartX + w > b.PixelWidth - 1) { w = b.PixelWidth - 1 - StartX; }
            if (StartY + h > b.PixelHeight - 1) { h = b.PixelHeight - 1 - StartY; }
            WriteableBitmap wb = new WriteableBitmap(w, h);
            var p = b.PixelBuffer.ToArray();
            var tp = wb.PixelBuffer.ToArray();
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    tp[(i * w + j) * 4] = p[((i + StartY) * b.PixelWidth + StartX + j) * 4];
                    tp[(i * w + j) * 4+1] = p[((i + StartY) * b.PixelWidth + StartX + j) * 4+1];
                    tp[(i * w + j) * 4+2] = p[((i + StartY) * b.PixelWidth + StartX + j) * 4+2];
                    tp[(i * w + j)*4+3] = p[((i + StartY) * b.PixelWidth + StartX + j)*4+3];
                }
            }
            tp.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public Color getAverageColor(WriteableBitmap b)//计算位图的平均颜色，很简单，直接rgb分别平均就可以了，返回颜色值
        {
            long rc = 0, gc = 0, bc = 0;
            Color c = new Color();
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i+=4)
            {
                bc = bc + Convert.ToInt64(components[i]);
                gc = gc + Convert.ToInt64(components[i+1]);
                rc = rc + Convert.ToInt64(components[i+2]);
            }
            long Num = 0;
            Num = b.PixelWidth * b.PixelHeight;
            rc = Convert.ToInt32(Convert.ToDecimal(rc) / Num);
            gc = Convert.ToInt32(Convert.ToDecimal(gc) / Num);
            bc = Convert.ToInt32(Convert.ToDecimal(bc) / Num);
            c.B = Convert.ToByte(bc);
            c.G = Convert.ToByte(gc);
            c.R = Convert.ToByte(rc);
            return c;
        }

        public double getAverageBright(WriteableBitmap b)//计算位图的平均颜色，很简单，直接rgb分别平均就可以了，返回颜色值
        {
            long rc = 0, gc = 0, bc = 0;
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i+=4)
            {
                bc = bc + Convert.ToInt64(components[i]);
                gc = gc + Convert.ToInt64(components[i+1]);
                rc = rc + Convert.ToInt64(components[i+2]);
            }
            long Num = 0;
            Num = b.PixelWidth * b.PixelHeight;
            rc = Convert.ToInt32(Convert.ToDouble(rc) / Num);
            gc = Convert.ToInt32(Convert.ToDouble(gc) / Num);
            bc = Convert.ToInt32(Convert.ToDouble(bc) / Num);
            return  .299 * rc + .587 * gc + .114 * bc;
        }

        public double getAverageColorL(WriteableBitmap b)//计算位图的平均明度，很简单，每个像素求明度然后平均
        {
            List<double> dl = getImageLList(b);
            if (dl.Count == 0) { return -1; }
            double x = 0;
            foreach (double d in dl)
            {
                x += d;
            }
            return x / dl.Count;


        }

        public List<Color> getAverageColorList(List<WriteableBitmap> bl)
        {
            List<Color> cl = new List<Color>();
            foreach (WriteableBitmap b in bl)
            {
                cl.Add(getAverageColor(b));
            }
            return cl;
        }

        public List<Double> getAverageBrightList(List<WriteableBitmap> bl)
        {
            List<Double> cl = new List<Double>();
            foreach (WriteableBitmap b in bl)
            {
                cl.Add(getAverageBright(b));
            }
            return cl;
        }

        protected Color HSVToRGB(int H, int S, int V)//HSV到RGB的色彩变幻
        //H取值是0-360，S、V取值都是1到100，会自动处理
        {
            int R, G, B;
            H = Convert.ToInt32(Convert.ToDecimal(H) / 360 * 255);
            S = Convert.ToInt32(Convert.ToDecimal(S) / 100 * 255);
            V = Convert.ToInt32(Convert.ToDecimal(V) / 100 * 255);

            if (S == 0)
            {
                R = 0;
                G = 0;
                B = 0;
            }

            decimal fractionalSector;
            decimal sectorNumber;
            decimal sectorPos;
            sectorPos = (Convert.ToDecimal(H) / 255 * 360) / 60;
            sectorNumber = Convert.ToInt32(Math.Floor(Convert.ToDouble(sectorPos)));
            fractionalSector = sectorPos - sectorNumber;

            decimal p;
            decimal q;
            decimal t;

            decimal r = 0;
            decimal g = 0;
            decimal b = 0;
            decimal ss = Convert.ToDecimal(S) / 255;
            decimal vv = Convert.ToDecimal(V) / 255;


            p = vv * (1 - ss);
            q = vv * (1 - (ss * fractionalSector));
            t = vv * (1 - (ss * (1 - fractionalSector)));

            switch (Convert.ToInt32(sectorNumber))
            {
                case 0:

                    r = vv;
                    g = t;
                    b = p;
                    break;

                case 1:
                    r = q;
                    g = vv;
                    b = p;
                    break;
                case 2:

                    r = p;
                    g = vv;
                    b = t;
                    break;
                case 3:

                    r = p;
                    g = q;
                    b = vv;
                    break;
                case 4:

                    r = t;
                    g = p;
                    b = vv;
                    break;
                case 5:

                    r = vv;
                    g = p;
                    b = q;
                    break;
            }
            R = Convert.ToInt32(r * 255);
            G = Convert.ToInt32(g * 255);
            B = Convert.ToInt32(b * 255);
            Color c = new Color();
            c.R = Convert.ToByte(R);
            c.G = Convert.ToByte(G);
            c.B = Convert.ToByte(B);
            return c;
        }

        public WriteableBitmap getRedImage(WriteableBitmap b)//获取图像的红色分量影像，相当于透过红色的滤镜观察影像
        {
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i+=4)
            {
                components[i] = 0;
                components[i+1] = 0;
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }
        public WriteableBitmap getGreenImage(WriteableBitmap b)//获取图像的绿色分量影像，相当于透过绿色的滤镜观察影像
        {
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i+=4)
            {
                components[i] = 0;
                //components[1] = 0;
                components[i+2] = 0;
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }
        public WriteableBitmap getBlueImage(WriteableBitmap b)//获取图像的蓝色分量影像，相当于透过蓝色的滤镜观察影像
        {
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i+=4)
            {
                components[i+1] = 0;
                components[i+2] = 0;
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }
        public WriteableBitmap getGrayImage(WriteableBitmap b, Boolean IsCloseBlack, int h)//获取图像的灰色分量影像，h是阀值，如果是CloseBlack，那么就取接近黑色的部分保留(<h的像素)，其他部分清白（255），反之保留接近白色的（>255-h），其他部分清黑（0）
        {
            Byte Gray, red, blue, green;
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i += 4)
            {
                blue = components[i];
                green = components[i+1];
                red = components[i+2];
                Gray = (byte)(.299 * red + .587 * green + .114 * blue);
                if (IsCloseBlack)
                {
                    if (Gray > h)
                    {
                        components[i] = components[i+1] = components[i+2] = 255;//直接删除这些像素的信息，变成白色
                    }
                }
                else
                {
                    if (Gray < 255 - h)
                    {
                        components[i] = components[i+1] = components[i+2] = 0;
                    }
                }
            }
            components.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public List<double> getImageLList(WriteableBitmap b)//获取图像的明度分量影像
        {
            Byte red, blue, green;
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var components = wb.PixelBuffer.ToArray();
            List<double> dl = new List<double>();
            for (int i = 0; i < components.Length; i+=4)
            {
                blue = components[i];
                green = components[i+1];
                red = components[i+2];
                LColor lc = new LColor(Color.FromArgb(255, red, green, blue));
                dl.Add(lc.getL(CurrentLabMode));
            }
            return dl;
        }

        protected float getAberration(WriteableBitmap b)//得到畸变的程度，这里假设是黑白的影像,这个是纠正了图像以后的成绩
        {//首先是找到中心点，然后利用阀值去掉上下两边，直接分析左右的边的鼓出程度
            //分析的方法是先割裂出左右两个影像，针对每一个找出其黑线的起始和终止的列，然后进行平均
            //最终返回的是鼓出的部分和全长的比值。
            b=AutoBright(b);
            //b=Correct_XMarkAberrationChart_Photo(b);
            float r = 0;
            float sw = 0.2f;//子图像的边界畸变覆盖的区域
            Point lbPoint = PhotoTest.NullPoint;
            Point lePoint = PhotoTest.NullPoint;//左侧的起始位置和终止位置
            Point rbPoint = PhotoTest.NullPoint;
            Point rePoint = PhotoTest.NullPoint;
            //Bitmap Fiterb = this.getGrayImage(b, true, 80);//得到特定阀值的灰度图
            WriteableBitmap Fiterb = b;
            WriteableBitmap Leftb = this.getImageArea(Fiterb, 0, 0, Convert.ToInt32(sw * Fiterb.PixelWidth), Fiterb.PixelHeight);//得到图像的左侧裁剪区，约为1/3
            WriteableBitmap Rightb = this.getImageArea(Fiterb, Convert.ToInt32((1 - sw) * Fiterb.PixelWidth), 0, Convert.ToInt32(sw * Fiterb.PixelWidth), Fiterb.PixelHeight);
            //
            List<int> al;
            int h = 125;
            for (int i = 0; i < Leftb.PixelHeight; i++)
            {
                al = this.getImageGrayHLine(Leftb, i);//水平切分，直到发现黑色像素
                for (int j = 0; j < al.Count; j++)//从左向右
                {
                    int temp = Convert.ToInt32(al[j]);
                    if (temp < h)
                    {
                        lbPoint = new Point(j, i);

                        i = Leftb.PixelHeight;
                        break;
                    }
                }
            }
            for (int i = Leftb.PixelHeight - 1; i >= 0; i--)
            {
                al = this.getImageGrayHLine(Leftb, i);//水平切分，直到发现黑色像素
                for (int j = 0; j < al.Count; j++)//从左向右
                {
                    int temp = Convert.ToInt32(al[j]);
                    if (temp < h)
                    {
                        lePoint = new Point(j, i);

                        i = -1;
                        break;
                    }
                }
            }

            for (int i = 0; i < Rightb.PixelHeight; i++)
            {
                al = this.getImageGrayHLine(Rightb, i);//水平切分，直到发现黑色像素
                for (int j = al.Count - 1; j >= 0; j--)//从右向左
                {
                    int temp = Convert.ToInt32(al[j]);
                    if (temp < h)
                    {
                        rbPoint = new Point(j, i);

                        i = Rightb.PixelHeight;
                        break;
                    }
                }
            }
            for (int i = Rightb.PixelHeight - 1; i >= 0; i--)
            {
                al = this.getImageGrayHLine(Rightb, i);//水平切分，直到发现黑色像素
                for (int j = al.Count - 1; j >= 0; j--)//从右向左
                {
                    int temp = Convert.ToInt32(al[j]);
                    if (temp < h)
                    {
                        rePoint = new Point(j, i);

                        i = -1;
                        break;
                    }
                }
            }
            int subw = Leftb.PixelWidth;

            //Leftb.Dispose();
            //Rightb.Dispose();
            //下面开始计算左右侧的最鼓点，由原始图片进行
            Point lQPoint = PhotoTest.NullPoint;
            Point rQPoint = PhotoTest.NullPoint;
            for (int i = 0; i < subw; i++)
            {
                al = this.getImageGrayVLine(Fiterb, i);//垂直切分，直到发现黑色像素
                for (int j = 0; j < al.Count; j++)//从上向下
                {
                    int temp = Convert.ToInt32(al[j]);
                    if (temp < h)
                    {
                        lQPoint = new Point(i, j);

                        i = Fiterb.PixelWidth;
                        break;
                    }
                }

            }
            for (int i = Fiterb.PixelWidth - 1; i >= Fiterb.PixelWidth - subw; i--)
            {
                al = this.getImageGrayVLine(Fiterb, i);//垂直切分，直到发现黑色像素
                for (int j = 0; j < al.Count; j++)//从上向下
                {
                    int temp = Convert.ToInt32(al[j]);
                    if (temp < h)
                    {
                        rQPoint = new Point(i, j);

                        i = -1;
                        break;
                    }
                }

            }

            int w = Convert.ToInt32(Fiterb.PixelWidth * (1 - sw));//用来矫正在右侧图的x值，相对于原始图的位置

            double tQ, bQ, Q, TL;
            tQ = Math.Abs(lbPoint.X - rbPoint.X - w);//上面的x轴距离
            bQ = Math.Abs(lePoint.X - rePoint.X - w);//下面的x轴距离
            Q = (tQ + bQ) / 2;//求出上下两组点之间的平均x轴距离
            TL = Math.Abs(rQPoint.X - lQPoint.X);//计算出沿着x轴最宽处的距离
            //平均的鼓出程度比上宽度的一半
            r = Convert.ToSingle(TL) / Convert.ToSingle(Q) - 1;//意义是两侧最宽处宽出来的部分比上总宽

            return r;
        }

        protected long getColorPixNum(WriteableBitmap b, Color c, int h)//检索某个图片里面特定颜色的像素数
        //h是左右的阀值，单位是度，所有的隶属关系都使用HSV/HSB模式来进行度量和处理
        {
            long PCount = 0;
            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                Color c0 = Color.FromArgb(255, components[i+2], components[i+1], components[i]);//查找第一个白色的点，用于校准位置
                float h0 = Math.Abs(PhotoTest.getHue(c0) - PhotoTest.getHue(c));//这个阀值是在色相环上面的，因此无论浓度和亮度，关键是角度
                if (h0 < h)
                {
                    PCount++;
                }
            }
            return PCount;


        }

        protected decimal getNoiseLevel(WriteableBitmap b, int h)//描绘影像的背景噪声，方法是看每个像素的差值和平均值之间的距离是不是超过阀值，超过就是噪点，最后除以像素数
        {
            Color AveC = this.getAverageColor(b);
            decimal res = 0;
            decimal Sum = 0;

            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                Color c = new Color();
                c.B = components[i];
                c.G = components[i+1];
                c.R = components[i+2];
                if (getColorDistance(c, AveC) > h)//偏离平均值过大被认为是噪点
                {
                    Sum++;
                }
            }

            long Num = 0;
            Num = b.PixelWidth * b.PixelHeight;
            res = Sum / Num;//平均每个像素的噪点数
            return res;
        }

        protected decimal getColorDistance(Color SourceC, Color TargetC)//计算两个颜色的距离，在rgb空间里面的距离
        {
            double x = (SourceC.R - TargetC.R) * (SourceC.R - TargetC.R) + (SourceC.G - TargetC.G) * (SourceC.G - TargetC.G) + (SourceC.B - TargetC.B) * (SourceC.B - TargetC.B);
            double d = Math.Sqrt(x);
            return Convert.ToDecimal(d);
        }
        
        protected WriteableBitmap getEdgeBitmap(WriteableBitmap b)//获得包含边界的影像，实际是删除两侧的不合理的那些纯黑和纯白像素
        {
            int left = 0, right = 0;//由于边沿可能是倾斜的，因此，针对每一行，left选择最小的，right选择最大的
            //BitmapData bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);//给出锁定内存，直接访问内存区
            //int stride = bmData.Stride;
            //System.IntPtr Scan0 = bmData.Scan0;
            for (int y = 0; y < b.PixelHeight; ++y)//行的变化
            {
                List<int> al;
                al = this.getImageGrayHLine(b, y);
                for (int i = 0; i < al.Count; i++)
                {
                    if ((Convert.ToInt32(al[i]) < 220) && (left > i))//注意，这里的阀值是35，也就是说边界是灰度达到了35才算的
                    {
                        left = i;
                    }
                    if ((Convert.ToInt32(al[i]) > 35) && (right < i))
                    {
                        right = i;
                    }
                }
            }
            WriteableBitmap BRIO = this.getImageArea(b, left, 0, right - left, b.PixelHeight);
            return BRIO;
        }

        protected float getDispersiveness(WriteableBitmap b)//测试色散，在沿着边沿，看rgb的分离情况，照片要包含一个边沿
        //获取边沿内的最大色彩离散值，然后去比上边沿的整个宽度。
        //先计算每一列的最大色散，然后进行各个列的平均，然后除以整个边界的宽度。核心是找到那个边界，找到边界的跨度
        {
            WriteableBitmap tb;
            tb = this.getEdgeBitmap(b);//选择包含了边界的图像部分，主要是去掉两端的那些纯黑和纯白的点，阀值35
            int AveDisCow = 0;
            float DispersivenessPerEdge;

            var components = tb.PixelBuffer.ToArray();

            for (int y = 0; y < tb.PixelHeight; ++y)//行的变化
            {
                int Dispersiveness = 0;
                for (int x = 0; x < tb.PixelWidth; ++x)//列的变化
                {
                    int i = (y * tb.PixelWidth + x)*4;
                    Color c = new Color();
                    c.B = components[i];
                    c.G = components[i+1];
                    c.R = components[i+2];
                    int tempD = 0;
                    tempD = this.getPointMaxDispersiveness(c.R, c.G, c.B);//当前的色散值是rgb差值中间最大的
                    if (Dispersiveness < tempD)//获取最大的色散值
                    {
                        Dispersiveness = tempD;
                    }

                    AveDisCow = AveDisCow + Dispersiveness;
                }
            }

            DispersivenessPerEdge = (Convert.ToSingle(AveDisCow) / Convert.ToSingle(tb.PixelHeight));//计算色散比例
            return DispersivenessPerEdge;
        }

        protected int getPointMaxDispersiveness(int r, int g, int b)//实际就是求RGB色散，这三者的最大差值
        {
            int d1, d2, d3;
            d1 = Math.Abs(r - g);
            d2 = Math.Abs(g - b);
            d3 = Math.Abs(b - r);
            int reV = Math.Max(d1, d2);
            reV = Math.Max(reV, d3);
            return reV;
        }

        protected double LeanArc(WriteableBitmap b)//依据第一个边界上面的横点判读照片的倾角。实际上进行纠正的时候，应该首先纠正角度
        {
            List<int> al;
            List<long> Xal = new List<long>(), Yal = new List<long>();
            int lesspointY = b.PixelHeight, lesspointX = 0;
            for (int i = 0; i < b.PixelHeight; i++)
            {
                al = this.getImageGrayVLine(b, i);
                long l = this.FindFirstBlack(al, 100);
                if (l != -1)
                {
                    Xal.Add(i);
                    Yal.Add(l);
                }
            }
            long h = 0, w = 0;
            if (Yal.Count != 0)
            {
                for (int i = 0; i < Yal.Count; i++)
                {
                    if (lesspointY > Convert.ToInt32(Yal[i]))
                    {
                        lesspointY = Convert.ToInt32(Yal[i]);
                        lesspointX = Convert.ToInt32(Xal[i]);
                    }
                }
                if (lesspointX < b.PixelWidth / 2)
                {
                    h = Convert.ToInt64(Yal[Yal.Count - 1]) - lesspointY;//此时h是正数，标识顺时针旋转的角度
                    w = Convert.ToInt64(Xal[Xal.Count - 1]) - lesspointX;
                }
                else
                {
                    h = lesspointY - Convert.ToInt64(Yal[0]);//此时h是负数，标识反时针旋转的角度
                    w = Convert.ToInt64(Xal[Xal.Count - 1]) - lesspointX;
                }

            }
            double r = Convert.ToDouble(h) / Convert.ToDouble(w);
            double row = Math.Atan(r);
            return row;
        }

        protected Point Excursion(WriteableBitmap b)//依据第一个边界判断拍摄的偏移。
        {
            Point p = new Point(0, 0);
            List<int> al;
            List<long> Xal = new List<long>(), Yal = new List<long>();
            for (int i = 0; i < b.PixelWidth; i++)
            {
                al = this.getImageGrayVLine(b, i);
                long l = this.FindFirstBlack(al, 100);
                if (l != -1)
                {
                    Xal.Add(i);
                    Yal.Add(l);
                }
            }

            if (Convert.ToInt32(Yal[0]) > b.PixelHeight / 2)//标识第一个点到下面去了，此时应该以Y方向最小值为基准点
            {
                int LessY = Convert.ToInt32(Yal[0]);
                int LessX = Convert.ToInt32(Xal[0]);
                for (int j = 0; j < Yal.Count; j++)
                {
                    if (LessY > Convert.ToInt32(Yal[j]))
                    {
                        LessY = Convert.ToInt32(Yal[j]);
                        LessX = Convert.ToInt32(Xal[j]);
                    }
                }
                p.X = LessX;
                p.Y = LessY;
            }
            else
            {
                p.X = Convert.ToInt32(Xal[0]);
                p.Y = Convert.ToInt32(Yal[0]);
            }
            return p;
        }

        protected int FindFirstBlack(List<int> al, int h)//找到一个数组列表里面的第一个发黑的像素的位置。
        {
            for (int i = 0; i < al.Count; i++)
            {
                if (Convert.ToInt32(al[i]) < h)
                {
                    return i;
                }
            }
            return -1;//标识没有找到发黑的像素
        }

        protected int FindLastBlack(List<int> al, int h)//找到一个数组列表里面的最后发黑的像素的位置。
        {
            for (int i = al.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(al[i]) < h)
                {
                    return i;
                }
            }
            return -1;//标识没有找到发黑的像素
        }

        protected int FindFirstWhite(List<int> al, int h)//找到一个数组列表里面的第一个发白的像素的位置。
        {
            for (int i = 0; i < al.Count; i++)
            {
                if (Convert.ToInt32(al[i]) > 255 - h)
                {
                    return i;
                }
            }
            return -1;//标识没有找到发白的像素
        }

        protected int FindLastWhite(List<int> al, int h)//找到一个数组列表里面的最后发白的像素的位置。
        {
            for (int i = al.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(al[i]) > 255 - h)
                {
                    return i;
                }
            }
            return -1;//标识没有找到发白的像素
        }

        //下面是图像的缩放、旋转和平移操作
        public void CopyPixel(int sourceX, int sourceY, int targetX, int targetY, WriteableBitmap source, WriteableBitmap target)
        {//copy a pixel from source bitmap point to target
            
            var sp = source.PixelBuffer.ToArray();
            var tp = target.PixelBuffer.ToArray();
            var sourceIndex = sourceY * source.PixelWidth * 4 + sourceX * 4;
            var targetIndex = targetY * 4 * target.PixelWidth + targetX * 4;
            if (sourceIndex < 0 || sourceIndex > sp.Length - 3) return;
            if (targetIndex < 0 || targetIndex > tp.Length - 3) return;
            for (int i = 0; i < 4; i++)
            {
                tp[targetIndex + i] = sp[sourceIndex + i];
            }
            tp.CopyTo(target.PixelBuffer);
        }

        public WriteableBitmap MoveTrans(WriteableBitmap b, int dx, int dy)
        {
            WriteableBitmap target = new WriteableBitmap(b.PixelWidth, b.PixelHeight);
            var sp = b.PixelBuffer.ToArray();
            var tp = target.PixelBuffer.ToArray();
            for (int y = Math.Max(0,dy); y < Math.Min(b.PixelHeight,b.PixelHeight+dy); y++)
            {
                for (int x = Math.Max(0,dx); x < Math.Min(b.PixelWidth,b.PixelWidth+dx); x++)
                {
                    //CopyPixel(x, y, x + dx, y + dy, b, target);
                    //if (x + dx < b.PixelWidth&&y+dy<b.PixelHeight)
                    //{
                        var sourceIndex = y * b.PixelWidth * 4 + x * 4;
                        var targetIndex = (y + dy) * 4 * target.PixelWidth + (x + dx) * 4;
                        if (sourceIndex >= 0 && sourceIndex <= sp.Length - 3
                            &&targetIndex >= 0 && targetIndex <= tp.Length - 3)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                tp[targetIndex + i] = sp[sourceIndex + i];
                            }
                        }
                    //}
                }
            }
            tp.CopyTo(target.PixelBuffer);
            return target;
        }
        public WriteableBitmap ScaleTrans(WriteableBitmap b, double sx, int sy)
        {
            WriteableBitmap target = new WriteableBitmap(b.PixelWidth, b.PixelHeight);
            for (int y = 0; y < b.PixelHeight; y++)
            {
                for (int x = 0; x < b.PixelWidth; x++)
                {//scale center is w/2,h/2
                    CopyPixel(x, y, Convert.ToInt32((x-b.PixelWidth/2)*sx+b.PixelWidth/2),Convert.ToInt32((y-b.PixelHeight/2) *sy+b.PixelHeight/2), b, target);
                }
            }
            return target;
        }
        public WriteableBitmap RotateTrans(WriteableBitmap b, double angel)
        {
            WriteableBitmap target = new WriteableBitmap(b.PixelWidth, b.PixelHeight);
            for (int y = 0; y < b.PixelHeight; y++)
            {
                for (int x = 0; x < b.PixelWidth; x++)
                {
                    //rotate center is w/2,h/2

                    double a = Math.Atan((y - b.PixelHeight / 2) / (x - (b.PixelWidth / 2d)));
                    a += angel;
                    double r = Math.Sqrt(x * x + y * y);
                    CopyPixel(x, y, Convert.ToInt32(r * Math.Cos(a)+(b.PixelWidth/2)), Convert.ToInt32(r * Math.Sin(a)+(b.PixelHeight/2)), b, target);
                }
            }
            return target;
        }

        //public async Task<WriteableBitmap> MoveBitmap(WriteableBitmap b, int dx, int dy)//平移图像，然后重建
        //{
        //    WriteableBitmap tb = new WriteableBitmap(b.PixelWidth, b.PixelHeight);

        //    var components = tb.PixelBuffer.ToArray();
        //    var source = b.PixelBuffer.ToArray();
        //    for (int x = Math.Max(0,dx); x < Math.Min(b.PixelWidth,b.PixelWidth+dx); x++)
        //    {
        //        for(int y=Math.Max(0,dy);y<Math.Min(b.PixelHeight,b.PixelHeight+dy);y++)
        //        {

        //        }
        //        var targetPix=
        //        int nVal = (int)(components[i + 2] + nBrightness);
        //        if (nVal < 0) nVal = 0;
        //        if (nVal > 255) nVal = 255;
        //        components[i + 2] = (byte)nVal;
        //    }
        //    TranslateTransform f = new TranslateTransform();
        //    f.X = dx;
        //    f.Y = dy;
        //    Image im = new Image();
        //    im.Source = b;
        //    im.Width = b.PixelWidth;
        //    im.Height = b.PixelHeight;
        //    im.RenderTransform = f;
        //    WriteableBitmap tb = await WriteableBitmapHelper.Snapshot(im);
        //    return tb;
        //}

        //public async Task<WriteableBitmap> RotateBitmap(WriteableBitmap b, double arc)//这些操作使用了Graphics进行，然后再从Graphics重建图像
        //{
        //    RotateTransform f = new RotateTransform();
        //    f.CenterX = b.PixelWidth / 2;
        //    f.CenterY = b.PixelHeight / 2;
        //    f.Angle = arc;
        //    Image im = new Image();
        //    im.Source = b;
        //    im.Width = b.PixelWidth;
        //    im.Height = b.PixelHeight;
        //    im.RenderTransform = f;
        //    WriteableBitmap tb = await WriteableBitmapHelper.Snapshot(im);
        //    return tb;

        //}

        //public async Task<WriteableBitmap> ScaleBitmap(WriteableBitmap b, float scalex, float scaley)//这些操作使用了Graphics进行，然后再从Graphics重建图像
        //{
        //    Image im = new Image();
        //    im.Source = b;
        //    im.Width = b.PixelWidth*scalex;
        //    im.Height = b.PixelHeight*scaley;
        //    im.Stretch = Stretch.Fill;
        //    WriteableBitmap tb =await WriteableBitmapHelper.Snapshot(im);
        //    return tb;
        //}

        public async Task<WriteableBitmap> FadeBitmap(WriteableBitmap b, int Alpha)//图片设置一个前景的半透明膜
        {
            WriteableBitmap tb = new WriteableBitmap(b.PixelWidth, b.PixelHeight);
            var source = b.PixelBuffer.ToArray();
            var target = tb.PixelBuffer.ToArray();
            for (int i = 0; i < source.Length; i += 4)
            {
                target[i] = source[i];
                target[i+1] = source[i+1];
                target[i+2] = source[i+2];
                target[i+3] = (byte)Alpha;
            }
            target.CopyTo(tb.PixelBuffer);
            return tb;
        }

        //从一个边界找到所有的亮度越变点，然后测量这些点和角的距离，距离最近的就是边界相交的点

        protected Point getCornerPoint(WriteableBitmap b, Color BackColor)//得到拐角点，需要注意的是这应该是个小的区域，里面有单一的拐角点
        {
            List<Point> al = getEdgePointList(b, BackColor);
            int min = 0;
            for (int i = 0; i < al.Count; i++)
            {
                Point p0 = (Point)al[i];
                Point p1 = (Point)al[min];
                double d0 = (p0.X * p0.X) + (p0.Y * p0.Y);
                double d1 = (p1.X * p1.X) + (p1.Y * p1.Y);
                if (d0 > d1)
                {
                }
                else
                {
                    min = i;//每次min记录的都是较小的那个位置的序号
                }
            }
            Point p = (Point)al[min];
            return p;
        }

        protected List<Point> getEdgePointList(WriteableBitmap b, Color BackColor)//找出一个区域里面的所有边界。基础是从背景到前景的越变点集合
        {
            BWBitmap(b, 100);
            if (BackColor == Colors.Black)
            {
                Invert(b);
            }//保证背景是白色，前景是黑色，也就是寻找的是白变黑的点
            List<Point> pl = new List<Point>();
            for (int i = 0; i < b.PixelHeight; i++)
            {
                List<int> al = this.getImageGrayHLine(b, i);
                int y = getEdgePoint(al, true, 2);
                if (y > 0)
                {
                    Point p = new Point(i, y);
                    pl.Add(p);
                }
            }
            return pl;
        }

        protected int getEdgePoint(List<int> al, bool IsWToB, int noise)//寻找一个行或者列里面的越变点，同时，检查后面的几个像素，避免噪点的影响
        {
            int backB = 0;
            if (IsWToB)
            {
                backB = 255;
            }
            for (int i = 0; i < al.Count; i++)
            {
                if (backB == Convert.ToInt32(al[i]))
                {

                }
                else
                {
                    bool flag = true;
                    for (int j = 0; j < noise + 1; j++)
                    {//进行噪声检测，后面的点如果变回背景就认为是噪声
                        if (Convert.ToInt32(al[i + j]) == backB)
                        {
                            flag = false;
                        }
                    }
                    if (flag)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        protected bool IsWhite(Color c, int h)//判断一个rgb颜色是不是白色，并且是在一定的允许色差范围呢
        {
            int w = Convert.ToInt32(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);
            if (255 - w < h)
            {
                return true;
            }
            return false;
        }

        protected bool IsBlack(Color c, int h)//判断一个rgb颜色是不是黑色，并且是在一定的允许色差范围呢
        {
            int w = Convert.ToInt32(0.299 * c.R + 0.587 * c.G + 0.114 * c.B);
            if (w < h)
            {
                return true;
            }
            return false;
        }

        protected bool IsColor(Color c, Color TargetColor, int h)//判断一个rgb颜色是不是特定色彩，并且是在一定的允许色差范围呢
        {
            int w = Convert.ToInt32(Math.Sqrt((c.R - TargetColor.R) * (c.R - TargetColor.R) + (c.G - TargetColor.G) * (c.G - TargetColor.G) + (c.B - TargetColor.B) * (c.B - TargetColor.B)));
            if (w < h)
            {
                return true;
            }
            return false;
        }

        protected int getBorderPointPosition(List<int> GrayLine, int SourcePoint, bool IsReverse, int h)//从某点开始到某点结束，找出跃变点的位置，并且，越变点要保持2像素
        {
            Boolean BeginIsBlack;
            int temp = Convert.ToInt32(GrayLine[SourcePoint]);//初始化中心点颜色，也就是起始点的颜色
            if (temp < 125)//中心点的颜色简化处理，和阀值无关，只有黑白
            {
                BeginIsBlack = true;
            }
            else
            {
                BeginIsBlack = false;
            }
            if (IsReverse)//表示是从中央，向编号小的端查找
            {
                for (int i = SourcePoint - 1; i >= 0; i--)
                {
                    int x = Convert.ToInt32(GrayLine[i]);

                    if ((x < h) && (!BeginIsBlack))//当然像素是黑色，并且起始点不是黑色，表示检测到了黑色边界
                    {
                        return i;

                    }
                    if ((x > 255 - h) && (BeginIsBlack))//当前像素是白色，并且起始点是黑色
                    {
                        return i;

                    }
                }
            }
            else//表示从中央，向后查找越变点，查找的方法完全一样，只是方向相反
            {
                for (int i = SourcePoint; i < GrayLine.Count; i++)
                {
                    int x = Convert.ToInt32(GrayLine[i]);
                    if ((x < h) && (!BeginIsBlack))
                    {
                        return i;
                    }
                    if ((x > 255 - h) && (BeginIsBlack))
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        public Point getCenterPoint(WriteableBitmap b, int h)//得到图像的中心点，前提是照片中央是个白色的园或者正方形，外面是黑框
        {//算法是找到落入白圈的一个点，然后获取其在纵横方向上面和空框的交汇点坐标，这样，纵横弦的中点就是真正的圆心的坐标
            int width = b.PixelWidth;
            int height = b.PixelHeight;
            //int width = 0;
            //int height = 0;
            int cw = width / 2;
            int ch = height / 2;
            var components = b.PixelBuffer.ToArray();
            var index = (components.Length / 2 - 1) * 4;
            Color pc = new Color();
            pc.B = components[index];
            pc.G = components[index+1];
            pc.R = components[index+2];

            if (IsWhite(pc, 150))//表示中点是白色，如果不是，表示已经出了检索的允许范围，这里已知图像意义的中央是白色的，并且有一个对称边界，如园或者正方形黑框
            {
                List<int> Hal, Val;
                Hal = getImageGrayHLine(b, ch);
                Val = getImageGrayVLine(b, cw);
                int tx = getBorderPointPosition(Hal, cw, true, h);
                int tx1 = getBorderPointPosition(Hal, cw, false, h);
                int tw = tx1 - tx;//给出两边越变点之间的距离
                int ty = getBorderPointPosition(Val, ch, true, h);
                int ty1 = getBorderPointPosition(Val, ch, false, h);
                int th = ty1 - ty;//给出两个越变点之间距离
                int cx = tx + tw / 2;
                int cy = ty + th / 2;
                Point c = new Point(cx, cy);//给出真正的中心点坐标
                return c;
            }
            return PhotoTest.NullPoint;//表示中心点的位置不是白色，错误（影像太歪了，出了许可范围）
        }

        public Point getBrightChangeCenterPoint(WriteableBitmap b, int h)//得到图像的中心点，图像里面包含任意的圆形区域，寻找一个边界亮度跃迁点
        {//h是识别亮度跃迁的数值
            int width = b.PixelWidth;
            int height = b.PixelHeight;

            Color cl = GetPixel(b, width / 2, height / 2);
            double cg=getBright(cl);

                List<int> Hal, Val;
                Hal = getImageGrayHLine(b, height/2);
                Val = getImageGrayVLine(b, width/2);
                int tx = getBrightChangeBorderPointPosition(Hal, width / 2, true, h);
                int tx1 = getBrightChangeBorderPointPosition(Hal, width / 2, false, h);
                int tw = tx1 - tx;//给出两边越变点之间的距离
                int ty = getBrightChangeBorderPointPosition(Val, height / 2, true, h);
                int ty1 = getBrightChangeBorderPointPosition(Val, height / 2, false, h);
                int th = ty1 - ty;//给出两个越变点之间距离
                int cx = tx + tw / 2;
                int cy = ty + th / 2;
                Point c = new Point(cx, cy);//给出真正的中心点坐标
                return c;

            //return PhotoTest.NullPoint;//表示中心点的位置不是白色，错误（影像太歪了，出了许可范围）
        }

        protected int getBrightChangeBorderPointPosition(List<int> GrayLine, int SourcePoint, bool IsReverse, int h)//从某点开始到某点结束，找出跃变点的位置，并且，越变点要保持2像素
        {
            int temp = Convert.ToInt32(GrayLine[SourcePoint]);//初始化中心点颜色，也就是起始点的颜色

            if (IsReverse)//表示是从中央，向编号小的端查找
            {
                for (int i = SourcePoint - 1; i >= 0; i--)
                {
                    int x = Convert.ToInt32(GrayLine[i]);

                    if (Math.Abs(x-temp) > h)//当然像素是黑色，并且起始点不是黑色，表示检测到了黑色边界
                    {
                        return i;
                    }
                }
            }
            else//表示从中央，向后查找越变点，查找的方法完全一样，只是方向相反
            {
                for (int i = SourcePoint; i < GrayLine.Count; i++)
                {
                    int x = Convert.ToInt32(GrayLine[i]);
                    if (Math.Abs(x - temp) > h)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        protected int getBoderPoint(List<int> al, bool BackgroundIsWhite, int h)//判断某一行里面有没有边界点
        {
            for (int i = 0; i < al.Count; i++)
            {
                int t = Convert.ToInt32(al[i]);
                if ((BackgroundIsWhite) && (t < h))
                {
                    return i;
                }
                if (!(BackgroundIsWhite) && (t > 255 - h))
                {
                    return i;
                }
            }
            return -1;//标识没有找到边界点
        }

        protected Point getTopPoint(WriteableBitmap b, int h)//扫描线从上到下，找到越变点，自动确定环境背景点
        {
            Boolean BeginIsBlack;
            Point p = PhotoTest.NullPoint;
            var components = b.PixelBuffer.ToArray();

            Color temp = new Color();
            temp.B = components[0];
            temp.G = components[1];
            temp.R = components[2];
            //Color temp = b.GetPixel(0, 0);//初始化中心点颜色，也就是起始点的颜色
            if (this.IsBlack(temp, 125))//起始点的颜色简化处理，和阀值无关，只有黑白
            {
                BeginIsBlack = true;
            }
            else
            {
                BeginIsBlack = false;
            }
            for (int i = 0; i < b.PixelHeight; i++)
            {
                List<int> al = this.getImageGrayHLine(b, i);
                int x = 0;
                if (BeginIsBlack)
                {
                    x = getBoderPoint(al, false, h);
                }
                else
                {
                    x = getBoderPoint(al, true, h);
                }
                if (x != -1)
                {
                    p = new Point(x, i);
                }
            }
            return p;
        }

        protected Point getBottomPoint(WriteableBitmap b, int h)//扫描线从下到上，找到越变点，自动确定环境背景点
        {
            Boolean BeginIsBlack;
            var components = b.PixelBuffer.ToArray();

            Color temp = new Color();
            temp.B = components[components.Length - (b.PixelWidth*4)];
            temp.G = components[components.Length - (b.PixelWidth*4)+1];
            temp.R = components[components.Length - (b.PixelWidth*4)+2];
            //Color temp = b.GetPixel(0, b.Height - 1);//初始化中心点颜色，也就是起始点的颜色
            Point p = PhotoTest.NullPoint;
            if (this.IsBlack(temp, 125))//起始点的颜色简化处理，和阀值无关，只有黑白
            {
                BeginIsBlack = true;
            }
            else
            {
                BeginIsBlack = false;
            }
            for (int i = b.PixelHeight - 1; i >= 0; i--)
            {
                List<int> al = this.getImageGrayHLine(b, i);
                int x = 0;
                if (BeginIsBlack)
                {
                    x = getBoderPoint(al, false, h);
                }
                else
                {
                    x = getBoderPoint(al, true, h);
                }
                if (x != -1)
                {
                    p = new Point(x, i);
                }
            }
            return p;
        }

        protected Point getLeftPoint(WriteableBitmap b, int h)//扫描线从左到右，找到越变点，自动确定环境背景点
        {
            Boolean BeginIsBlack;
            //Point p = Point.Empty;
            //Color temp = b.GetPixel(0, 0);//初始化中心点颜色，也就是起始点的颜色
            var components = b.PixelBuffer.ToArray();

            Color temp = new Color();
            temp.B = components[0];
            temp.G = components[1];
            temp.R = components[2];
            //Color temp = b.GetPixel(0, b.Height - 1);//初始化中心点颜色，也就是起始点的颜色
            Point p = PhotoTest.NullPoint;
            if (this.IsBlack(temp, 125))//起始点的颜色简化处理，和阀值无关，只有黑白
            {
                BeginIsBlack = true;
            }
            else
            {
                BeginIsBlack = false;
            }
            for (int i = 0; i < b.PixelHeight; i++)
            {
                List<int> al = this.getImageGrayVLine(b, i);
                int y = 0;
                if (BeginIsBlack)
                {
                    y = getBoderPoint(al, false, h);
                }
                else
                {
                    y = getBoderPoint(al, true, h);
                }
                if (y != -1)
                {
                    p = new Point(i, y);
                }
            }
            return p;
        }

        protected Point getRightPoint(WriteableBitmap b, int h)//扫描线从右到左，找到越变点，自动确定环境背景点
        {
            Boolean BeginIsBlack;
            //Color temp = b.GetPixel(b.Width - 1, 0);//初始化中心点颜色，也就是起始点的颜色
            //Point p = Point.Empty;
            var components = b.PixelBuffer.ToArray();

            Color temp = new Color();
            temp.B = components[(b.PixelWidth*4 - 4)];
            temp.G = components[(b.PixelWidth * 4 - 4)];
            temp.R = components[(b.PixelWidth * 4 - 4)];
            //Color temp = b.GetPixel(0, b.Height - 1);//初始化中心点颜色，也就是起始点的颜色
            Point p = PhotoTest.NullPoint;
            if (this.IsBlack(temp, 125))//起始点的颜色简化处理，和阀值无关，只有黑白
            {
                BeginIsBlack = true;
            }
            else
            {
                BeginIsBlack = false;
            }
            for (int i = b.PixelHeight - 1; i >= 0; i--)
            {
                List<int> al = this.getImageGrayVLine(b, i);
                int y = 0;
                if (BeginIsBlack)
                {
                    y = getBoderPoint(al, false, h);
                }
                else
                {
                    y = getBoderPoint(al, true, h);
                }
                if (y != -1)
                {
                    p = new Point(i, y);
                }
            }
            return p;
        }

        protected double getArc(WriteableBitmap b, int h)//找到图片倾斜的角度，为纠正打下基础，纠正的时候先纠正坐标位置，再去纠正角度
        {//正的角度表示右倾，负的是左倾
            Point tp = getTopPoint(b, h);
            Point lp = getLeftPoint(b, h);
            double d = Math.Sqrt((tp.X - lp.X) * (tp.X - lp.X) + (tp.Y - lp.Y) * (tp.Y - lp.Y));
            double arc = 0;
            if (d < 5)//两个点的距离小于5像素，认为是一个点，正方形不存在歪的现象
            {
                return 0;
            }
            if (tp.X < b.PixelWidth / 2)//表示最上面的点是在中线的左侧，整个角度是右倾
            {
                arc = Math.Atan((tp.X - lp.X) / (lp.Y - tp.Y));
            }
            else//这个是左倾
            {
                arc = Math.Atan((tp.Y - lp.Y) / (tp.X - lp.X));
            }
            return arc;
        }

        protected float getArc(Point lp, Point rp)//利用左右两个点位置差，求出两者的夹角
        {
            if (lp.X == rp.X) { return 0; }
            float arc = -(float)(Math.Atan((rp.Y - lp.Y) / (rp.X - lp.X)));//假定的情况是逆时针是负，顺时针是正
            return arc;
        }

        public double getColorHueDistance(Color c, Color c0)//给出色相环上面的色调差异
        {
            float d=(PhotoTest.getHue(c) - PhotoTest.getHue(c0));
            d = d % 360;
            if (d < 0) d = d + 360;
            if (d > 180) d = d-360;
            return d;
        }

        public double getColorRGBDistance(Color c, Color c0)//给出RGB空间差异
        {
            return Math.Sqrt((c.R - c0.R) * (c.R - c0.R) + (c.G - c0.G) * (c.G - c0.G) + (c.B - c0.B) * (c.B - c0.B));
        }
        public double getColorLabDistance(Color c, Color c0)//给出Lab间差异
        {
            LColor lc, lc0;
            lc = new LColor(c);
            lc0 = new LColor(c0);
            return Math.Sqrt((lc.getL(CurrentLabMode) - lc0.getL(CurrentLabMode)) * (lc.getL(CurrentLabMode) - lc0.getL(CurrentLabMode)) + (lc.geta(CurrentLabMode) - lc0.geta(CurrentLabMode)) * (lc.geta(CurrentLabMode) - lc0.geta(CurrentLabMode)) + (lc.getb(CurrentLabMode) - lc0.getb(CurrentLabMode)) * (lc.getb(CurrentLabMode) - lc0.getb(CurrentLabMode)));
        }
        public decimal getSD(WriteableBitmap b)//得到图片的均方差，用来衡量色度的一致性，也就刻画了画面的粗糙程度，但是未必是噪点，是不是噪点要看阀值
        {//这是比噪点更精细的指标
            Color ac = getAverageColor(b);//得到色度期望
            //int red = 0, green = 0, blue = 0;
            decimal SD = 0;
            //开始计算均方差，是每个像素和期望之间差的平方相加，然后平均，再开方
            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                Color tc = new Color();
                tc.B = components[i];
                tc.G = components[i+1];
                tc.R = components[i+2];
                decimal tsd = getColorDistance(tc, ac);
                SD = SD + (tsd * tsd);
            }
            //long Num = 0;
            //Num = b.PixelWidth * b.PixelHeight;
            SD = SD / b.PixelWidth * b.PixelHeight;
            //SD = Math.Sqrt(SD);
            return Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(SD)));
        }

        public decimal getBrightnessConsistancy(WriteableBitmap b)//获取照片的亮度一致性信息。实际是求整个图像像素和中心点亮度之间的均方差
        {
            //Color c = b.GetPixel(b.Width / 2, b.Height / 2);
            var components = b.PixelBuffer.ToArray();

            var index = (components.Length / 4 / 2 - 1)*4;
            Color c = new Color();
            c.B = components[index];
            c.G = components[index+1];
            c.R = components[index+2];
            float cb = PhotoTest.getBrightness(c);
            //int red = 0, green = 0, blue = 0;
            double SD = 0;
            //开始计算均方差，是每个像素和期望之间差的平方相加，然后平均，再开方

            for (int i = 0; i < components.Length; i+=4)
            {
                Color tc = new Color();
                tc.B = components[i];
                tc.G = components[i+1];
                tc.R = components[i+2];

                float tb = PhotoTest.getBrightness(tc) - cb;
                SD = SD + (tb * tb);
            }

            //long Num = 0;
            //Num = b.Width * b.Height;
            SD = SD / components.Length/4;
            //SD = Math.Sqrt(SD);
            return Convert.ToDecimal(Math.Sqrt(SD));
        }

        public double getPurpleEdge(WriteableBitmap b, int h)//得到紫边像素的比例
        {
            long pnum = getColorPixNum(b, Colors.Purple, h);//色相环紫色周围30度夹角内的像素，都算是紫边
            double pe = Convert.ToDouble(pnum) / (b.PixelWidth * b.PixelHeight);
            return pe;
        }

        public float getWriteBanlance(WriteableBitmap b)//获取灰色色块的白平衡测试值，实际就是色彩的饱和度，饱和度越是高，距离白平衡越远
        {
            Color ac = getAverageColor(b);//获取整个图像的平均颜色
            return PhotoTest.getSaturation(ac);
        }

        public WriteableBitmap AutoBright(WriteableBitmap b)//对图像进行自动白平衡，对改善图像的品质至关重要
        {
            var sourceArray = b.PixelBuffer.ToArray();
            WriteableBitmap tb = new WriteableBitmap(b.PixelWidth, b.PixelHeight);
            var targetArray = tb.PixelBuffer.ToArray();
            //下面这些变量定义的是R,G,B三个分量亮度的最大值和最小值
            int Max_R_bright = 0;
            int Min_R_bright = 255;
            int Max_B_bright = 0;
            int Min_B_bright = 255;
            int Max_G_bright = 0;
            int Min_G_bright = 255;
            //Bitmap b = bm.Clone(new Rectangle(0,0,bm.Width,bm.Height),bm.PixelFormat);
            int width = b.PixelWidth;
            int height = b.PixelHeight;

            //这一部分主要是统计出图象R,G,B三个颜色的最大值和最小值分别是多少
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var index = (y * width + x) * 4;
                    Color tc = new Color();
                    tc.B = sourceArray[index];
                    tc.G = sourceArray[index+1];
                    tc.R = sourceArray[index+2];
                    Max_B_bright = Math.Max(tc.B, Max_B_bright);
                    Min_B_bright = Math.Min(tc.B, Min_B_bright);
                    Max_G_bright = Math.Max(tc.G, Max_G_bright);
                    Min_G_bright = Math.Min(tc.G, Min_G_bright);
                    Max_R_bright = Math.Max(tc.R, Max_R_bright);
                    Min_R_bright = Math.Min(tc.R, Min_R_bright);
                }
            }
            //pixel = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // 处理像素 B, G, R 亮度三分量
                    //下面主要就是进行颜色的处理了.R,G,B,三种颜色分别做处理.
                    //用到了公式 (Color - Min_Color) * 255/(Max_color - Min_Color)
                    //Color代表当前颜色 Min_Color代表颜色的最小值 Max_Color代表颜色的最大值
                    var index = (y * width + x) * 4;
                    targetArray[index] = Convert.ToByte((sourceArray[index] - Min_B_bright) * 255 / ((Max_B_bright) - Min_B_bright));
                    targetArray[index + 1] = Convert.ToByte((sourceArray[index+1] - Min_G_bright) * 255 / ((Max_G_bright) - Min_G_bright));
                    targetArray[index + 2] = Convert.ToByte((sourceArray[index+2] - Min_R_bright) * 255 / ((Max_R_bright) - Min_R_bright));
                }
            }
            targetArray.CopyTo(tb.PixelBuffer);
            return tb;
        }

        protected String ProcMessage = "";//公告变量，表示最终的处理结果

        protected Point MovePosition;//矫正的时候需要移动的距离
        protected float RotateArc;//矫正的时候倾斜的角度
        protected float ScaleNum;//矫正的时候缩放倍数

        protected Point getCircleCenterPoint(WriteableBitmap b, int x0, int y0)//得到特定园的圆心坐标位置
        {
            Point p0 = PhotoTest.NullPoint;
            Color c0=new Color();
            var components = b.PixelBuffer.ToArray();

            var index=(y0*b.PixelWidth+x0)*4;
            c0.B = components[index];
            c0.G = components[index+1];
            c0.R = components[index+2];

            int h = 120;
            if (IsWhite(c0, 120))
            {
                List<int> Hal, Val;
                Hal = getImageGrayHLine(b, y0);
                Val = getImageGrayVLine(b, x0);
                int tx = getBorderPointPosition(Hal, x0, true, h);
                int tx1 = getBorderPointPosition(Hal, x0, false, h);
                int tw = tx1 - tx;//给出两边越变点之间的距离
                int ty = getBorderPointPosition(Val, y0, true, h);
                int ty1 = getBorderPointPosition(Val, y0, false, h);
                int th = ty1 - ty;//给出两个越变点之间距离
                int cx = tx + tw / 2;
                int cy = ty + th / 2;
                p0 = new Point(cx, cy);//给出真正的中心点坐标

            }
            return p0;
        }

        protected decimal getGraySinDistance(List<int> al, int m)//得到正弦灰度值，其值是和空间频率相关的
        {
            decimal pl = 1 / m;//给出波长
            for (int i = 0; i < al.Count; i++)
            {
                int t = Convert.ToInt32(al[i]);//获取当前的实际强度值
                double d = i / al.Count;//获取当前的位置比例
                d = d / 6.28;
                d = Math.Sin(d);
                m = m + Math.Abs(t - Convert.ToInt32(d));
            }
            return m;
        }

        protected float getEdgeDispersiveness(WriteableBitmap b)//测试色散，在沿着边沿，看rgb的分离情况，这里的bm已经是包含边界的选取图
        //获取边沿内的最大色彩离散值，然后去比上边沿的整个宽度。
        //先计算每一列的最大色散，然后进行各个列的平均，然后除以整个边界的宽度。核心是找到那个边界，找到边界的跨度
        {
            var pixelArray = b.PixelBuffer.ToArray();
            int AveDisCow = 0;
            float DispersivenessPerEdge;
            for (int i = 0; i < b.PixelHeight; i++)
            {
                int Dispersiveness = 0;
                for (int j = 0; j < b.PixelWidth; j++)
                {
                    var index=(i * b.PixelWidth + j)*4;
                    int tempD = 0;
                    tempD = this.getPointMaxDispersiveness(pixelArray[index+2], pixelArray[index+1], pixelArray[index]);
                    if (Dispersiveness < tempD)
                    {
                        Dispersiveness = tempD;
                    }
                    AveDisCow = AveDisCow + Dispersiveness;
                }
            }
            DispersivenessPerEdge = Convert.ToSingle(AveDisCow) / pixelArray.Length/4;
            return DispersivenessPerEdge;
        }

        protected List<int> getListDis(List<int> al)//计算指定数列的级差
        {
            List<int> a = new List<int>();
            for (int i = 0; i < al.Count - 1; i++)
            {
                int x0, x1;
                x0 = Convert.ToInt32(al[i]);
                x1 = Convert.ToInt32(al[i + 1]);
                a.Add(x1 - x0);
            }
            return a;
        }

        protected decimal getMTF(List<int> al)//得到某个队列里面的
        {
            decimal max, min;
            if (al.Count == 0) { return 0m; }
            max = Convert.ToDecimal(al[0]);
            min = Convert.ToDecimal(al[0]);
            for (int i = 0; i < al.Count; i++)
            {
                int x = Convert.ToInt32(al[i]);
                if (min > x)
                {
                    min = x;
                }
                if (max < x)
                {
                    max = x;
                }
            }
            decimal mtf = (max - min) / (min + max);
            return mtf;
        }

        protected decimal getBitmapMTF(WriteableBitmap b)//找一个正弦灰度图像的mtf
        {
            List<int> al;
            decimal mtf = 0;
            for (int i = 0; i < b.PixelHeight; i++)
            {
                al = getImageGreenHLine(b, i);
                mtf = mtf + getMTF(al);
            }
            mtf = mtf / b.PixelHeight;
            return mtf;
        }
        //soveLines要看mtf为0.03的空间频率

        protected float getBrightChange(WriteableBitmap CB, WriteableBitmap BB)//中心区域和四周区域的亮度差
        {
            //直接给出两个区域的亮度差
            Color c1 = getAverageColor(CB);
            Color c2 = getAverageColor(BB);
            float d = Math.Abs(PhotoTest.getBrightness(c1) - PhotoTest.getBrightness(c2));
            return d;//返回灰度差异百分比
        }

        protected void AnalyseWave(List<int> MaxAl, List<int> MinAl, List<int> DataList, int WindowWide)//依据波长，分析每一个波长里面的极值，其实也是是波的波峰和波谷数值
        {//MaxAl是返回波峰的数值
            //MinAl返回波谷的数值
            //DataList是源数据列表
            //WindowWide是波长，相当于滑动窗口
            if (MaxAl == null) { MaxAl = new List<int>(); }
            if (MinAl == null) { MinAl = new List<int>(); }
            int WaveNo = DataList.Count / WindowWide;
            int min, max;
            for (int i = 0; i < WaveNo; i++)
            {
                min = Convert.ToInt32(DataList[WaveNo * i]);
                max = Convert.ToInt32(DataList[WaveNo * i]);
                for (int j = WaveNo * i; j < WaveNo * (i + 1); j++)
                {
                    int currentvalue = Convert.ToInt32(DataList[j]);
                    if (max < currentvalue)
                    {
                        max = currentvalue;
                    }
                    if (min > currentvalue)
                    {
                        min = currentvalue;
                    }
                }
                MaxAl.Add(max);
                MinAl.Add(min);
            }
        }

        protected List<Color> getAvarageColorVLine(WriteableBitmap b)//对垂直线上面的各个像素颜色进行平均，得出一个平均颜色的队列
        {
            var components = b.PixelBuffer.ToArray();
            List<Color> al = new List<Color>();

                long red, green, blue;//实际上，rgb都是字节，在内存里面连续排列
                for (int y = 0; y < b.PixelWidth; ++y)//列的变化
                {
                    red = 0;
                    green = 0;
                    blue = 0;
                    for (int x = 0; x < b.PixelHeight; ++x)//行的变化
                    {
                        var index=(x*b.PixelWidth+y)*4;
                        blue = blue + Convert.ToInt32(components[index]);
                        green = green + Convert.ToInt32(components[index+1]);
                        red = red + Convert.ToInt32(components[index+2]);
                    }

                    blue = blue / b.PixelHeight;
                    green = green / b.PixelHeight;
                    red = red / b.PixelHeight;
                    al.Add(Color.FromArgb(255, Convert.ToByte(red), Convert.ToByte(green), Convert.ToByte(blue)));
                }
            return al;
        }
        
        protected void swap(ref double a, ref double b)
        {
            double t;
            t = a;
            a = b;
            b = t;
        }

        protected void bitrp(double[] xreal, double[] ximag, int n)
        {
            // 位反转置换 Bit-reversal Permutation
            int i, j, a, b, p;

            for (i = 1, p = 0; i < n; i *= 2)
            {
                p++;
            }
            for (i = 0; i < n; i++)
            {
                a = i;
                b = 0;
                for (j = 0; j < p; j++)
                {
                    //b = (b << 1) + (a & 1);    // b = b * 2 + a % 2;
                    //a >>= 1;        // a = a / 2;
                    b = b * 2 + a % 2;
                    a = a / 2;
                }
                if (b > i)
                {
                    swap(ref xreal[i], ref xreal[b]);
                    swap(ref ximag[i], ref ximag[b]);
                }
            }
        }

        protected void FFT(double[] xreal, double[] ximag, int n)
        {
            // 快速傅立叶变换，将复数 x 变换后仍保存在 x 中，xreal, ximag 分别是 x 的实部和虚部
            const int N = 1024;
            double[] wreal = new double[N / 2];
            double[] wimag = new double[N / 2];
            double treal, timag, ureal, uimag, arg;
            int m, k, j, t, index1, index2;

            bitrp(xreal, ximag, n);

            // 计算 1 的前 n / 2 个 n 次方根的共轭复数 W'j = wreal [j] + i * wimag [j] , j = 0, 1, ... , n / 2 - 1
            arg = -2 * Math.PI / n;
            treal = Math.Cos(arg);
            timag = Math.Sin(arg);
            wreal[0] = 1.0;
            wimag[0] = 0.0;
            for (j = 1; j < n / 2; j++)
            {
                wreal[j] = wreal[j - 1] * treal - wimag[j - 1] * timag;
                wimag[j] = wreal[j - 1] * timag + wimag[j - 1] * treal;
            }

            for (m = 2; m <= n; m *= 2)
            {
                for (k = 0; k < n; k += m)
                {
                    for (j = 0; j < m / 2; j++)
                    {
                        index1 = k + j;
                        index2 = index1 + m / 2;
                        t = n * j / m;    // 旋转因子 w 的实部在 wreal [] 中的下标为 t
                        treal = wreal[t] * xreal[index2] - wimag[t] * ximag[index2];
                        timag = wreal[t] * ximag[index2] + wimag[t] * xreal[index2];
                        ureal = xreal[index1];
                        uimag = ximag[index1];
                        xreal[index1] = ureal + treal;
                        ximag[index1] = uimag + timag;
                        xreal[index2] = ureal - treal;
                        ximag[index2] = uimag - timag;
                    }
                }
            }
        }

        protected void IFFT(double[] xreal, double[] ximag, int n)
        {
            // 快速傅立叶逆变换
            const int N = 1024;
            double[] wreal = new double[N / 2];
            double[] wimag = new double[N / 2];
            double treal, timag, ureal, uimag, arg;
            int m, k, j, t, index1, index2;

            bitrp(xreal, ximag, n);

            // 计算 1 的前 n / 2 个 n 次方根 Wj = wreal [j] + i * wimag [j] , j = 0, 1, ... , n / 2 - 1
            arg = 2 * Math.PI / n;
            treal = Math.Cos(arg);
            timag = Math.Sin(arg);
            wreal[0] = 1.0;
            wimag[0] = 0.0;
            for (j = 1; j < n / 2; j++)
            {
                wreal[j] = wreal[j - 1] * treal - wimag[j - 1] * timag;
                wimag[j] = wreal[j - 1] * timag + wimag[j - 1] * treal;
            }

            for (m = 2; m <= n; m *= 2)
            {
                for (k = 0; k < n; k += m)
                {
                    for (j = 0; j < m / 2; j++)
                    {
                        index1 = k + j;
                        index2 = index1 + m / 2;
                        t = n * j / m;    // 旋转因子 w 的实部在 wreal [] 中的下标为 t
                        treal = wreal[t] * xreal[index2] - wimag[t] * ximag[index2];
                        timag = wreal[t] * ximag[index2] + wimag[t] * xreal[index2];
                        ureal = xreal[index1];
                        uimag = ximag[index1];
                        xreal[index1] = ureal + treal;
                        ximag[index1] = uimag + timag;
                        xreal[index2] = ureal - treal;
                        ximag[index2] = uimag - timag;
                    }
                }
            }

            for (j = 0; j < n; j++)
            {
                xreal[j] /= n;
                ximag[j] /= n;
            }
        }

        public List<decimal> getDdx<T>(List<T> fal)//求离散函数的微分
        {
            List<decimal> al = new List<decimal>();
            for (int i = 0; i < fal.Count - 2; i++)
            {
                decimal d = (Convert.ToDecimal(fal[i + 2]) - Convert.ToDecimal(fal[i])) / 2;
                //if (d < 0) { d = 0; }
                al.Add(d);

            }
            return al;
        }

        protected List<double> FFTListR(List<double> al)
        {
            double[] xr = new double[al.Count];

            double[] xi = new double[al.Count];
            for (int i = 0; i < al.Count; i++)
            {
                xr[i] = al[i];
            }
            FFT(xr, xi, getMaxHz(al.Count));
            List<double> tal = new List<double>(xr);
            return tal;
        }//得到fft的实数部分

        protected List<double> FFTListV(List<double> al)
        {
            double[] xr = new double[al.Count];
            double[] xi = new double[al.Count];
            for (int i = 0; i < al.Count; i++)
            {
                xr[i] = al[i];
            }
            FFT(xr, xi, getMaxHz(al.Count));
            return new List<double>(xi);
        }//得到fft的虚数部分

        protected int getMaxHz(int l)//给出小于l的最大的2 的幂的数值，最大到128
        {
            int n = 0;
            if (l >= 128)
            {
                n = 128;
            }
            if ((l < 128) && (l >= 64))
            {
                n = 64;
            }
            if ((l < 64) && (l >= 32))
            {
                n = 32;
            }
            if ((l < 32) && (l >= 16))
            {
                n = 16;
            }
            if ((l < 16) && (l >= 8))
            {
                n = 8;
            }
            if ((l < 8) && (l >= 4))
            {
                n = 4;
            }
            if ((l < 4) && (l >= 2))
            {
                n = 2;
            }
            return n;
        }
        
        protected double getComplexABS(double xreal, double ximag)//计算复数的绝对值
        {
            return Math.Sqrt(xreal * xreal + ximag * ximag);
        }

        protected List<decimal> getSmoothList<T>(List<T> al)//对曲线做平滑，避免出现尖点
        {
            if (al.Count == 0) { return null; }
            //if (al.Count <= 2) { return al; }
            List<decimal> sal = new List<decimal>();
            sal.Add(Convert.ToDecimal(al[0]));
            for (int i = 1; i < al.Count - 1; i++)
            {
                decimal x = (Convert.ToDecimal(al[i - 1]) + Convert.ToDecimal(al[i])) / 2 + Convert.ToDecimal(al[i]) + (Convert.ToDecimal(al[i]) + Convert.ToDecimal(al[i + 1])) / 2;
                x = x / 3;
                sal.Add(x);
            }
            if (al.Count >= 2)
            {
                sal.Add(Convert.ToDecimal(al[al.Count - 1]));
            }
            return sal;
        }

        protected List<decimal> MoreProcEdgeList(List<int> al)//处理原始的边界，只取亮度变化在10%到90%之间的部分
        {
            List<decimal> vl = new List<decimal>();
            if (al.Count == 0) { return null; }
            decimal dmin = Convert.ToDecimal(al[0]);
            decimal dmax = Convert.ToDecimal(al[0]);
            for (int i = 0; i < 10; i++)
            {
                dmin = dmin + Convert.ToDecimal(al[i]);
            }
            dmin = dmin / 10;

            for (int i = al.Count - 1; i > al.Count - 11; i--)
            {
                dmax = dmax + Convert.ToDecimal(al[i]);
            }
            dmax = dmax / 10;

            decimal h = dmax - dmin;
            dmin = dmin + h * 0.1m;
            dmax = dmax - h * 0.1m;
            decimal v;
            for (int i = 0; i < al.Count; i++)
            {
                v = Convert.ToDecimal(al[i]);
                if (v < dmin)
                {
                    v = dmin;
                }
                if (v > dmax)
                {
                    v = dmax;
                }
                vl.Add(v);
            }
            return vl;
        }

        protected void ProcLowBorderPosition(List<int> al, int StartPosition)//处理低端的像素（更暗）
        {
            decimal ave = 0;
            int c = 0;
            for (int i = StartPosition; i > -1; i--)
            {
                c++;
                ave = ave + Convert.ToInt32(al[i]);
            }
            ave = ave / c;
            for (int i = StartPosition; i > -1; i--)
            {
                //int x = Convert.ToInt32(al[i]);
                //int x0 = Convert.ToInt32(al[i - 1]);
                //if (x0 > x)
                //{
                //    al[i - 1] = x;
                //}
                al[i] = Convert.ToInt32(ave);
            }
        }

        protected void ProcHighBorderPosition(List<int> al, int StartPosition)
        {
            decimal ave = 0;
            int c = 0;

            for (int i = StartPosition; i < al.Count; i++)
            {
                c++;
                ave = ave + Convert.ToInt32(al[i]);
            }
            ave = ave / c;

            for (int i = StartPosition; i < al.Count; i++)
            {
                al[i] = Convert.ToInt32(ave);

            }

            //for (int i = StartPosition; i <al.Count-1; i++)
            //{
            //    int x = Convert.ToInt32(al[i]);
            //    int x0 = Convert.ToInt32(al[i + 1]);
            //    if (x0 < x)
            //    {
            //        al[i + 1] = x;
            //    }
            //}
        }

        protected int getLowBorderPosition(List<int> al, int StartPosition)//处理低端的像素
        {
            int p = 0;
            for (int i = StartPosition; i > 1; i--)//逆向查找
            {
                int x = Convert.ToInt32(al[i]);
                int x0 = Convert.ToInt32(al[i - 1]);
                if (x0 > x)
                {
                    p = i;
                    break;
                }
            }
            return p;
        }

        protected int getHighBorderPosition(List<int> al, int StartPosition)
        {
            int p = al.Count - 1;
            for (int i = StartPosition; i < al.Count - 1; i++)
            {
                int x = Convert.ToInt32(al[i]);
                int x0 = Convert.ToInt32(al[i + 1]);
                if (x0 < x)
                {
                    p = i;
                    break;
                }
            }
            return p;
        }

        protected decimal getListSwing(List<int> al)//得到数列数值变化区域，也就是数列数值的变化幅度
        {
            //List<long> pl = new List<long>();//先放小的位置，再放大的位置
            if (al.Count == 0) { return -1; }
            decimal dmin = 0;
            decimal dmax = 0;
            for (int i = 0; i < 10; i++)
            {
                dmin = dmin + Convert.ToDecimal(al[i]);
            }
            dmin = dmin / 10;

            for (int i = al.Count - 1; i > al.Count - 11; i--)
            {
                dmax = dmax + Convert.ToDecimal(al[i]);
            }
            dmax = dmax / 10;

            decimal h = Math.Abs(dmax - dmin);//最大值和最小值的差
            return h;
        }

        protected List<long> getEdgePosition(List<int> al)//得到边界的前后边界点，边界的定义是从跃升10%灰度值的地点向两端搜索，直到找到拐点
        {
            List<long> pl = new List<long>();//先放小的位置，再放大的位置
            if (al.Count == 0) { return null; }
            //decimal dmin = Convert.ToDecimal(al[0]);
            //decimal dmax = Convert.ToDecimal(al[0]);
            decimal dmin = 0m;
            decimal dmax = 0m;
            for (int i = 0; i < 10; i++)
            {
                dmin = dmin + Convert.ToDecimal(al[i]);
            }
            dmin = dmin / 10;

            for (int i = al.Count - 1; i > al.Count - 11; i--)
            {
                dmax = dmax + Convert.ToDecimal(al[i]);
            }
            dmax = dmax / 10;

            decimal h = Math.Abs(dmax - dmin);//总的亮度差，也就是前后十个像素平均亮度的差异
            //dmin = dmin + h * 0.1;
            //dmax = dmax - h * 0.1;
            decimal v;
            int LowPos = 0;
            int HighPos = 0;


            for (int i = 0; i < al.Count; i++)
            {
                v = Convert.ToDecimal(al[i]);
                if (v > dmin + h * 0.25m)//满足条件，已经充分大，可以逆向找起始点
                {
                    LowPos = getLowBorderPosition(al, i);
                    break;
                }
            }
            for (int i = al.Count-1; i >0; i--)
            {
                v = Convert.ToDecimal(al[i]);
                if (v < dmax - h * 0.25m)
                {
                    HighPos = getHighBorderPosition(al, i);
                    break;
                }

            }
            pl.Add(LowPos);
            pl.Add(HighPos);
            return pl;

        }

        protected List<int> ProcEdgeList(List<int> al)//处理原始的边界，识别出边界以后，边界两侧的都取同一个数值，只有超越差值5%的点保留
        {
            List<int> vl = new List<int>();
            if (al.Count < 5) { return null; }//针对跃变边界，至少要有5个像素

            int cx = Convert.ToInt32(al.Count * 0.2);
            if (cx > 10) { cx = 10; }

            decimal dmin = Convert.ToDecimal(al[0]);
            decimal dmax = Convert.ToDecimal(al[0]);
            for (int i = 0; i < cx; i++)
            {
                dmin = dmin + Convert.ToDecimal(al[i]);
            }
            dmin = dmin / cx;

            for (int i = al.Count - 1; i > al.Count - 1 - cx; i--)
            {
                dmax = dmax + Convert.ToDecimal(al[i]);
            }
            dmax = dmax / cx;

            decimal h = Math.Abs(dmax - dmin);
            //dmin = dmin + h * 0.1;
            //dmax = dmax - h * 0.1;
            decimal v;
            for (int i = 0; i < al.Count; i++)
            {
                v = Convert.ToDecimal(al[i]);
                if (v > dmin + h * 0.02m)
                {
                    ProcLowBorderPosition(al, i);
                    break;
                }
            }
            for (int i = al.Count-1; i >0; i--)
            {
                v = Convert.ToDecimal(al[i]);
                if (v < dmax - (h * 0.02m))
                {
                    ProcHighBorderPosition(al, i);
                    break;
                }

            }
            for (int i = 0; i < al.Count; i++)
            {
                vl.Add(al[i]);
            }
            return vl;
        }

        protected List<T> ReverseList<T>(List<T> al)//数列反向
        {
            List<T> tal = new List<T>();
            for (int i = 0; i < al.Count; i++)
            {
                tal.Add(al[al.Count - i - 1]);
            }
            return tal;
        }

        protected WriteableBitmap getCorrectXMarkChartArea(WriteableBitmap b)
        {
            decimal XISOscale = 0.5m;
            decimal YISOscale = 0.67m;
            int subxs = Convert.ToInt32(b.PixelWidth / 2 * (1 - XISOscale));
            int subys = Convert.ToInt32(b.PixelHeight / 2 * (1 - YISOscale));
            int dis = Convert.ToInt32(b.PixelHeight * YISOscale);

            WriteableBitmap sb = getImageArea(b, subxs, subys, dis, dis);
            return sb;
        }

        protected WriteableBitmap Correct_XMarkAberrationChart_Photo(WriteableBitmap b)//完成对XMark格式的标准版的对准，对正纠正
        {//首先是对正，保证图像中心就是图片的中心，然后修正角度，最后完成对Bitmap的修改。

            Point CP = getCenterPoint(b, 100);
            int dx = Convert.ToInt32(CP.X - b.PixelWidth / 2);
            int dy = Convert.ToInt32(CP.Y - b.PixelHeight / 2);
            WriteableBitmap cb = MoveTrans(b, -dx, -dy);
            WriteableBitmap sb = getCorrectXMarkChartArea(cb);
            double arc = getArc(sb, 120);
            //sb.Dispose();
            WriteableBitmap lb = RotateTrans(cb, -arc);
            //cb.Dispose();
            return lb;
        }

        protected List<int> getRedHEdge(WriteableBitmap b)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageRedVLine(b, b.PixelHeight / 2);
            return Val;
        }

        protected List<int> getRedVEdge(WriteableBitmap b)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageRedHLine(b, b.PixelWidth / 2);
            return Val;
        }

        protected List<int> getGreenHEdge(WriteableBitmap b)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageGreenVLine(b, b.PixelHeight / 2);
            return Val;
        }

        protected List<int> getGreenVEdge(WriteableBitmap b)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageGreenHLine(b, b.PixelWidth / 2);
            return Val;
        }

        protected List<int> getBlueHEdge(WriteableBitmap b)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageBlueVLine(b, b.PixelHeight / 2);
            return Val;
        }

        protected List<int> getBlueVEdge(WriteableBitmap b)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageBlueHLine(b, b.PixelWidth / 2);
            return Val;
        }

        protected List<int> getRedHEdge(WriteableBitmap b,int p)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageRedVLine(b, p);
            return Val;
        }

        protected List<int> getRedVEdge(WriteableBitmap b, int p)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageRedHLine(b, p);
            return Val;
        }

        protected List<int> getGreenHEdge(WriteableBitmap b, int p)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageGreenVLine(b, p);
            return Val;
        }

        protected List<int> getGreenVEdge(WriteableBitmap b, int p)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageGreenHLine(b, p);
            return Val;
        }

        protected List<int> getBlueHEdge(WriteableBitmap b,int p)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageBlueVLine(b, p);
            return Val;
        }

        protected List<int> getBlueVEdge(WriteableBitmap b, int p)//依据一个边界给出一个R的亮度变化曲线
        {
            List<int> Val = new List<int>();
            Val = getImageBlueHLine(b, p);
            return Val;
        }

        //protected List<int> getGreenEdge(WriteableBitmap b, bool IsH)//依据一个边界给出一个G的亮度变化曲线
        //{
        //    List<int> Val = new List<int>();
        //    if (IsH)
        //    {
        //        Val = getImageGreenVLine(b, b.PixelWidth / 2);
        //    }
        //    else
        //    {
        //        Val = getImageGreenHLine(b, b.PixelHeight / 2);
        //    }

        //    return Val;
        //}

        //protected List<int> getBlueEdge(WriteableBitmap b, bool IsH)//依据一个边界给出一个B的亮度变化曲线
        //{
        //    List<int> Val = new List<int>();
        //    if (IsH)
        //    {
        //        Val = getImageBlueVLine(b, b.PixelWidth / 2);
        //    }
        //    else
        //    {
        //        Val = getImageBlueHLine(b, b.PixelHeight / 2);
        //    }

        //    return Val;
        //}

        protected decimal getRGBMaxDis(decimal r, decimal g, decimal b)//得到RGB的最大距离
        {
            decimal d1, d2, d3;
            decimal d;
            d1 = Math.Abs(r - g);
            d2 = Math.Abs(r - b);
            d3 = Math.Abs(g - b);
            if (d1 > d2)
            {
                if (d1 > d3)
                {
                    d = d1;
                }
                else
                {
                    d = d3;
                }
            }
            else
            {
                if (d2 > d3)
                {
                    d = d2;
                }
                else { d = d3; }
            }
            return d;
        }

        public void DrawRecArea(WriteableBitmap b, int StartX, int StartY, int w, int h, Color c)//在分析照片上面绘制出选择的区域，便于调试
        {
            if (StartX < 0) StartX = 0;
            if (StartX > b.PixelWidth - 1) StartX = b.PixelWidth - 1;
            if (StartY < 0) StartY = 0;
            if (StartY > b.PixelHeight - 1) StartY = b.PixelHeight - 1;
            if (w < 0) w = 0;
            if (StartX + w > b.PixelWidth - 1) w = (b.PixelWidth - 1) - StartX;
            if (h < 0) h = 0;
            if (StartY + h > b.PixelHeight - 1) h = b.PixelHeight - 1 - StartY;
            var components = b.PixelBuffer.ToArray();
            Point LeftTop = new Point(StartX, StartY);
            Point RightTop = new Point(StartX + w, StartY);
            Point LeftBottom = new Point(StartX, StartY + h);

            for (int i = 0; i < w; i+=4)
            {
                components[(StartX*4+StartY*b.PixelWidth*4)+ i] = (byte)(c.B);
                components[(StartX*4+StartY*b.PixelWidth*4)+i+1] = (byte)(c.G);
                components[(StartX * 4 + StartY * b.PixelWidth * 4) + i + 2] = (byte)(c.R);
                components[(StartX * 4 + StartY * b.PixelWidth * 4) + i + 3] = (byte)(255);

                components[(StartX * 4 + (StartY + h) * b.PixelWidth * 4) + i] = (byte)(c.B);
                components[(StartX * 4 + (StartY + h) * b.PixelWidth * 4) + i + 1] = (byte)(c.G);
                components[(StartX * 4 + (StartY + h) * b.PixelWidth * 4) + i + 2] = (byte)(c.R);
                components[(StartX * 4 + (StartY + h) * b.PixelWidth * 4) + i + 3] = (byte)(255);

            }
            for (int i = 0; i < h; i+=4)
            {
                components[(StartX*4+StartY*b.PixelWidth*4)+i*b.PixelWidth*4] = (byte)(c.B);
                components[(StartX * 4 + StartY * b.PixelWidth * 4) + i * b.PixelWidth * 4+1] = (byte)(c.G);
                components[(StartX * 4 + StartY * b.PixelWidth * 4) + i * b.PixelWidth * 4+2] = (byte)(c.R);
                components[(StartX * 4 + StartY * b.PixelWidth * 4) + i * b.PixelWidth * 4+3] = (byte)(255);

                components[((StartX+w) * 4 + StartY * b.PixelWidth * 4) + i * b.PixelWidth * 4] = (byte)(c.B);
                components[((StartX + w) * 4 + StartY * b.PixelWidth * 4) + i * b.PixelWidth * 4 + 1] = (byte)(c.G);
                components[((StartX + w) * 4 + StartY * b.PixelWidth * 4) + i * b.PixelWidth * 4 + 2] = (byte)(c.R);
                components[((StartX + w) * 4 + StartY * b.PixelWidth * 4) + i * b.PixelWidth * 4 + 3] = (byte)(255);

            }
            components.CopyTo(b.PixelBuffer);
            //return tb;
        }

        public static float getHue(Color c)
        {
            LColor lc = new LColor(c);
            return Convert.ToSingle(lc.HSB_H);
            //float hue;
            //int minval = Math.Min(c.R, Math.Min(c.G, c.B));
            //int maxval = Math.Max(c.R, Math.Max(c.G, c.B));
            //if (maxval == minval)
            //{
            //    return 0.0f;
            //}
            //else
            //{
            //    float diff = (float)(maxval - minval);
            //    float rnorm = (maxval - c.R) / diff;
            //    float gnorm = (maxval - c.G) / diff;
            //    float bnorm = (maxval - c.B) / diff;

            //    hue = 0.0f;

            //    if (c.R == maxval)
            //    {
            //        hue = 60.0f * (6.0f + bnorm - gnorm);
            //    }

            //    if (c.G == maxval)
            //    {
            //        hue = 60.0f * (2.0f + rnorm - bnorm);
            //    }

            //    if (c.B == maxval)
            //    {
            //        hue = 60.0f * (4.0f + gnorm - rnorm);
            //    }

            //    if (hue > 360.0f)
            //    {
            //        hue = hue - 360.0f;
            //    }
            //}
            //return hue;
        }

        public static float getSaturation(Color c)
        {
            LColor lc = new LColor(c);
            return Convert.ToSingle(lc.HSB_S);
            //int minval = Math.Min(c.R, Math.Min(c.G, c.B));
            //int maxval = Math.Max(c.R, Math.Max(c.G, c.B));
            //if (maxval == minval)
            //{
            //    return 0.0f;
            //}
            //else
            //{
            //    int sum = maxval + minval;

            //    if (sum > 255)
            //    {
            //        sum = 510 - sum;
            //    }

            //    return (float)(maxval - minval) / sum;
            //}
        }

        public static float getBrightness(Color c)
        {
            LColor lc = new LColor(c);
            return Convert.ToSingle(lc.HSB_B);
            //int minval = Math.Min(c.R, Math.Min(c.G, c.B));
            //int maxval = Math.Max(c.R, Math.Max(c.G, c.B));

            ////bri 
            //return (float)(maxval + minval) / 510;
        }

        /// <summary> 
        /// HSB用float数据类型表示 
        /// </summary> 
        public void RGB2HSB(int r, int g, int b, out float hue, out float sat, out float bri)
        {
            LColor lc = new LColor();
            lc.setColorByRGB(r, g, b);
            hue = Convert.ToSingle(lc.HSB_H);
            sat = Convert.ToSingle(lc.HSB_S);
            bri = Convert.ToSingle(lc.HSB_B);
            //int minval = Math.Min(r, Math.Min(g, b));
            //int maxval = Math.Max(r, Math.Max(g, b));

            ////bri 
            //bri = (float)(maxval + minval) / 510;

            ////sat 
            //if (maxval == minval)
            //{
            //    sat = 0.0f;
            //}
            //else
            //{
            //    int sum = maxval + minval;

            //    if (sum > 255)
            //    {
            //        sum = 510 - sum;
            //    }

            //    sat = (float)(maxval - minval) / sum;
            //}

            ////hue 
            //if (maxval == minval)
            //{
            //    hue = 0.0f;
            //}
            //else
            //{
            //    float diff = (float)(maxval - minval);
            //    float rnorm = (maxval - r) / diff;
            //    float gnorm = (maxval - g) / diff;
            //    float bnorm = (maxval - b) / diff;

            //    hue = 0.0f;

            //    if (r == maxval)
            //    {
            //        hue = 60.0f * (6.0f + bnorm - gnorm);
            //    }

            //    if (g == maxval)
            //    {
            //        hue = 60.0f * (2.0f + rnorm - bnorm);
            //    }

            //    if (b == maxval)
            //    {
            //        hue = 60.0f * (4.0f + gnorm - rnorm);
            //    }

            //    if (hue > 360.0f)
            //    {
            //        hue = hue - 360.0f;
            //    }
            //}
        }

        public Color GetPixel(WriteableBitmap b,int x, int y)//得到图像指定xy坐标上面像素点的颜色
        {
            if (x < 0 || y < 0 || x >= b.PixelWidth || y >= b.PixelHeight) { return new Color(); }
            var components = b.PixelBuffer.ToArray();

            Color c = new Color();
            c.B = components[(x+y*b.PixelWidth)*4];
            c.G = components[(x + y * b.PixelWidth) * 4+1];
            c.R = components[(x + y * b.PixelWidth) * 4+2];
            c.A = components[(x + y * b.PixelWidth) * 4+3];
            return c;
        }

        //public Color GetColorFromInt(int BitInt)//依据编码的整型得到对应颜色
        //{
        //    byte[] components = new byte[4];
        //    components = BitConverter.GetBytes(BitInt);
        //    Color c = new Color();
        //    c.B = components[0];
        //    c.G = components[1];
        //    c.R = components[2];
        //    c.A = components[3];
        //    return c;
        //}

        public void SetPixel(WriteableBitmap b, int x, int y,Color c)//给指定的像素指定的颜色
        {
            var components = b.PixelBuffer.ToArray();
            int index = x + y * b.PixelWidth;
            if (index < 0 || index > components.Length) return;
            
            components[index] = c.B;
            components[index+1] = c.G;
            components[index+2] = c.R;
            components[index+3] = c.A;
            components.CopyTo(b.PixelBuffer);
        }

        public List<decimal> getDecimalList<T>(List<T> al)//类型转换
        {
            List<decimal> l = new List<decimal>();
            for (int i = 0; i < al.Count; i++)
            {
                l.Add(Convert.ToDecimal(al[i]));
            }

            return l;
        }

        public List<long> getMaxValuePosForList(List<int> al)//得到数列的极值的位置
        {
            List<long> pal = new List<long>();
            int max, min, maxpos, minpos;
            max = Convert.ToInt32(al[0]);
            min = Convert.ToInt32(al[0]);
            maxpos = 0;
            minpos = 0;
            for (int i = 0; i < al.Count; i++)
            {
                int x = Convert.ToInt32(al[i]);
                if (max < x)
                {
                    max = x;
                    maxpos = i;
                }
                if (min > x)
                {
                    min = x;
                    minpos = i;
                }
            }
            pal.Add(minpos);
            pal.Add(maxpos);
            return pal;
        }

        public double getImageSNR(WriteableBitmap b,SignNoiseType SNRType=SignNoiseType.Gray)
        {//先求平均值，然后求平均值和每个点的差，求标准差
            double Ave=0;
            for (int i = 0; i < b.PixelBuffer.ToArray().Length; i++)
            {
                byte[] components = new byte[4];
                components = BitConverter.GetBytes(b.PixelBuffer.ToArray()[i]);
                double v = 0;

                if (SNRType == SignNoiseType.Gray)
                    v = .299 * components[2] + .587 * components[1] + .114 * components[0];

                if (SNRType == SignNoiseType.L)
                    v = ColorManager.getLabL(Color.FromArgb(255, components[2], components[1], components[0]), CurrentLabMode);

                if (SNRType == SignNoiseType.R)
                    v = components[2];

                if (SNRType == SignNoiseType.G)
                    v = components[1];

                if (SNRType == SignNoiseType.B)
                    v = components[0];

                Ave += v;
            }
            Ave = Ave / b.PixelBuffer.ToArray().Length;
            double dv = 0;
            for (int i = 0; i < b.PixelBuffer.ToArray().Length; i++)
            {
                byte[] components = new byte[4];
                components = BitConverter.GetBytes(b.PixelBuffer.ToArray()[i]);
                double v = 0;

                if (SNRType == SignNoiseType.Gray)
                    v = .299 * components[2] + .587 * components[1] + .114 * components[0];

                if (SNRType == SignNoiseType.L)
                    v = ColorManager.getLabL(Color.FromArgb(255, components[2], components[1], components[0]), CurrentLabMode);

                if (SNRType == SignNoiseType.R)
                    v = components[2];

                if (SNRType == SignNoiseType.G)
                    v = components[1];

                if (SNRType == SignNoiseType.B)
                    v = components[0];

                dv += (v - Ave) * (v - Ave);
            }
            if (dv == 0) return -1;
            return 20 * Math.Log10(Ave / Math.Sqrt(dv));
        }

        //public List<Color> getLine(WriteableBitmap b, Point sp, Point ep)//在图片上获得一个直线的像素，采集该直线被映射到图片上面的点
        //{
        //    List<Color> cl = new List<Color>();
        //    int[] p = b.PixelBuffer.ToArray();
        //    int sx = Convert.ToInt32(sp.X);
        //    int sy = Convert.ToInt32(sp.Y);
        //    int ex = Convert.ToInt32(ep.X);
        //    int ey = Convert.ToInt32(ep.Y);
        //    int dx = Math.Abs(ex - sx);
        //    int dy = Math.Abs(ey - sy);
        //    //int px = dx / dy;
        //    //int py = dy / dx;

        //    if (dx >= dy)//表示像素水平展开
        //    {
        //        int px = Convert.ToInt32(Convert.ToDouble(dx) / (dy+1));
        //        if ((sx <= ex) && (sy <= ey))
        //        {
        //            for (int j = 0; j < dy + 1; j++)
        //            {
        //                for (int i = 0; i < px; i++)
        //                {
        //                    byte[] components = new byte[4];
        //                    components = BitConverter.GetBytes(p[sx + i + (sy + j-1) * b.PixelWidth]);
        //                    Color c = new Color();
        //                    c.B = components[0];
        //                    c.G = components[1];
        //                    c.R = components[2];
        //                    cl.Add(c);
        //                }
        //                sx = sx + px;
        //            }
        //        }
        //        if ((sx <= ex) && (sy > ey))
        //        {
        //            for (int j = 0; j < dy + 1; j++)
        //            {
        //                for (int i = 0; i < px; i++)
        //                {
        //                    byte[] components = new byte[4];
        //                    components = BitConverter.GetBytes(p[sx + i + (sy - j-1) * b.PixelWidth]);
        //                    Color c = new Color();
        //                    c.B = components[0];
        //                    c.G = components[1];
        //                    c.R = components[2];
        //                    cl.Add(c);
        //                }
        //                sx = sx + px;
        //            }
        //        }

        //        if ((sx > ex) && (sy <= ey))
        //        {
        //            for (int j = 0; j < dy + 1; j++)
        //            {
        //                for (int i = 0; i < px; i++)
        //                {
        //                    byte[] components = new byte[4];
        //                    components = BitConverter.GetBytes(p[sx - i + (sy + j-1) * b.PixelWidth]);
        //                    Color c = new Color();
        //                    c.B = components[0];
        //                    c.G = components[1];
        //                    c.R = components[2];
        //                    cl.Add(c);
        //                }
        //                sx = sx + px;
        //            }
        //        }
        //        if ((sx > ex) && (sy > ey))
        //        {
        //            for (int j = 0; j < dy + 1; j++)
        //            {
        //                for (int i = 0; i < px; i++)
        //                {
        //                    byte[] components = new byte[4];
        //                    components = BitConverter.GetBytes(p[sx - i + (sy - j-1) * b.PixelWidth]);
        //                    Color c = new Color();
        //                    c.B = components[0];
        //                    c.G = components[1];
        //                    c.R = components[2];
        //                    cl.Add(c);
        //                }
        //                sx = sx + px;
        //            }
        //        }
        //    }

        //    if (dx < dy)//表示像素垂直展开
        //    {
        //        int py = Convert.ToInt32(Convert.ToDouble(dy) / (dx+1));
        //        if ((sx <= ex) && (sy <= ey))
        //        {
        //            for (int j = 0; j < dx + 1; j++)
        //            {
        //                for (int i = 0; i < py; i++)
        //                {
        //                    byte[] components = new byte[4];
        //                    components = BitConverter.GetBytes(p[sx + j + (sy + i-1) * b.PixelWidth]);
        //                    Color c = new Color();
        //                    c.B = components[0];
        //                    c.G = components[1];
        //                    c.R = components[2];
        //                    cl.Add(c);
        //                }
        //                sy = sy + py;
        //            }
        //        }
        //        if ((sx <= ex) && (sy > ey))
        //        {
        //            for (int j = 0; j < dx + 1; j++)
        //            {
        //                for (int i = 0; i < py; i++)
        //                {
        //                    byte[] components = new byte[4];
        //                    components = BitConverter.GetBytes(p[sx + j + (sy - i-1) * b.PixelWidth]);
        //                    Color c = new Color();
        //                    c.B = components[0];
        //                    c.G = components[1];
        //                    c.R = components[2];
        //                    cl.Add(c);
        //                }
        //                sy = sy + py;
        //            }
        //        }
        //        if ((sx > ex) && (sy <= ey))
        //        {
        //            for (int j = 0; j < dx + 1; j++)
        //            {
        //                for (int i = 0; i < py; i++)
        //                {
        //                    byte[] components = new byte[4];
        //                    components = BitConverter.GetBytes(p[sx - j + (sy + i-1) * b.PixelWidth]);
        //                    Color c = new Color();
        //                    c.B = components[0];
        //                    c.G = components[1];
        //                    c.R = components[2];
        //                    cl.Add(c);
        //                }
        //                sy = sy + py;
        //            }
        //        }
        //        if ((sx > ex) && (sy > ey))
        //        {
        //            for (int j = 0; j < dx + 1; j++)
        //            {
        //                for (int i = 0; i < py; i++)
        //                {
        //                    byte[] components = new byte[4];
        //                    components = BitConverter.GetBytes(p[sx - j + (sy - i-1) * b.PixelWidth]);
        //                    Color c = new Color();
        //                    c.B = components[0];
        //                    c.G = components[1];
        //                    c.R = components[2];
        //                    cl.Add(c);
        //                }
        //                sy = sy + py;
        //            }
        //        }
        //    }

        //    return cl;
        //}
        public List<Point> getLinePointWithDDA(int x0, int y0, int xEnd, int yEnd)
        {
            List<Point> pl = new List<Point>();
            int dx = xEnd - x0;
            int dy = yEnd - y0;
            double x = x0;
            double y = y0;

            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            double xIncrement = Convert.ToDouble(dx) / steps;
            double yIncrement = Convert.ToDouble(dy) / steps;

            pl.Add(new Point(x, y));
            for (int i = 0; i <= steps; ++i)
            {
                x += xIncrement;
                y += yIncrement;
                pl.Add(new Point(Convert.ToInt32(x), Convert.ToInt32(y)));
            }
            return pl;
        }

        public List<Color> getLine(WriteableBitmap b, Point sp, Point ep)//在图片上获得一个直线的像素，采集该直线被映射到图片上面的点
        {
            List<Color> cl = new List<Color>();
            int dx = Convert.ToInt32(ep.X - sp.X);
            int dy = Convert.ToInt32(ep.Y - sp.Y);
            int x = Convert.ToInt32(sp.X);
            int y = Convert.ToInt32(sp.Y);

            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            cl.Add(GetPixel(b,x,y));
            if (steps == 0) { return cl; }
            int xIncrement = Convert.ToInt32(Convert.ToDouble(dx) / steps);
            int yIncrement = Convert.ToInt32(Convert.ToDouble(dy) / steps);
            
            
            for (int i = 0; i <= steps; ++i)
            {
                x += xIncrement;
                y += yIncrement;
                cl.Add(GetPixel(b, x, y));
            }

            return cl;
        }

        public void setLine(WriteableBitmap b, Point sp, Point ep,Color c)//在图片上面绘制一条直线
        {
            int dx = Convert.ToInt32(ep.X - sp.X);
            int dy = Convert.ToInt32(ep.Y - sp.Y);
            int x = Convert.ToInt32(sp.X);
            int y = Convert.ToInt32(sp.Y);

            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));

            int xIncrement = Convert.ToInt32(Convert.ToDouble(dx) / steps);
            int yIncrement = Convert.ToInt32(Convert.ToDouble(dy) / steps);

            SetPixel(b, x, y,c);
            for (int i = 0; i <= steps; ++i)
            {
                x += xIncrement;
                y += yIncrement;
                SetPixel(b, x, y,c);
            }
        }

        public void setLine(WriteableBitmap b, Point sp, Point ep, Color c,int w)
        {
            int s = w / 2;
            int f = -1;
            for (int i = 0; i < s+1; i++)
            {
                Point SP = new Point();
                SP.X = sp.X + (f * i);
                SP.Y = sp.Y + (f * i);
                Point EP = new Point();
                EP.X = ep.X + (f * i);
                EP.Y = ep.Y + (f * i);
                setLine(b, SP, EP, c);
            }
        }

        public List<int> getBrightPixNum(WriteableBitmap b)//获取图片的亮度像素直方图数据
        {
            List<int> nl = new List<int>();
            for (int i = 0; i < 256; i++)
            {
                nl.Add(0);
            }
            var components = b.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i += 4)
            {
                byte R = components[i+2];
                byte G = components[i+1];
                byte B = components[i];
                int x = (int)(.299 * R + .587 * G + .114 * B);
                nl[x]++;
            }
            return nl;
        }

        public List<int> getRedPixNum(WriteableBitmap b)
        {
            List<int> nl = new List<int>();
            for (int i = 0; i < 256; i++)
            {
                nl.Add(0);
            }
            var components = b.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i+=4)
            {
                int x = (int)(components[i+2]);
                nl[x]++;
            }
            return nl;
        }

        public List<int> getGreenPixNum(WriteableBitmap b)
        {
            List<int> nl = new List<int>();
            for (int i = 0; i < 256; i++)
            {
                nl.Add(0);
            }
            var components = b.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i+=4)
            {
                int x = (int)(components[i+1]);
                nl[x]++;
            }
            return nl;
        }

        public List<int> getBluePixNum(WriteableBitmap b)
        {
            List<int> nl = new List<int>();
            for (int i = 0; i < 256; i++)
            {
                nl.Add(0);
            }
            var components = b.PixelBuffer.ToArray();

            for (int i = 0; i < components.Length; i+=4)
            {
                int x = (int)(components[i]);
                nl[x]++;
            }
            return nl;
        }

        public WriteableBitmap SetContrast(WriteableBitmap b, double contrast)//设置对比度
        {
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            WriteableBitmap wb = WriteableBitmapHelper.Clone(b);
            var dd=wb.PixelBuffer.ToArray();
            for (int i = 0; i <dd .Length; i+=4)
            {
                    double pR = (double)dd[i+2] / 255.0;
                    pR -= 0.5;
                    pR *= contrast;
                    pR += 0.5;
                    pR *= 255;
                    if (pR < 0) pR = 0;
                    if (pR > 255) pR = 255;

                    double pG = (double)dd[i+1] / 255.0;
                    pG -= 0.5;
                    pG *= contrast;
                    pG += 0.5;
                    pG *= 255;
                    if (pG < 0) pG = 0;
                    if (pG > 255) pG = 255;

                    double pB = (double)dd[i] / 255.0;
                    pB -= 0.5;
                    pB *= contrast;
                    pB += 0.5;
                    pB *= 255;
                    if (pB < 0) pB = 0;
                    if (pB > 255) pB = 255;

                    dd[i+2] = (byte)pR;
                    dd[i+1] = (byte)pG;
                    dd[i] = (byte)pB;
            }
            dd.CopyTo(wb.PixelBuffer);
            return wb;
        }

        public void FloodFill(WriteableBitmap img, Point location, Color backColor, Color fillColor)
        {
            int width = img.PixelWidth;
            int height = img.PixelHeight;
            if (location.X < 0 || location.X >= width || location.Y < 0 || location.Y >= height) return;
            if (backColor == fillColor) return;

            if (GetPixel(img, (int)location.X, (int)location.Y) != backColor) return;
            Stack<Point> points = new Stack<Point>();
            points.Push(location);
            int ww = width - 1;
            int hh = height - 1;
            while (points.Count > 0)
            {
                Point p = points.Pop();
                SetPixel(img, (int)p.X, (int)p.Y, fillColor);
                //img[p.Y, p.X] = fillColor;
                if (p.X > 0 && GetPixel(img, (int)p.X - 1,(int)p.Y) == backColor)
                {
                    SetPixel(img, (int)p.X-1, (int)p.Y, fillColor);
                    //img[p.Y, p.X - 1] = fillColor;
                    points.Push(new Point(p.X - 1, p.Y));
                }
                if (p.X < ww && GetPixel(img, (int)p.X + 1, (int)p.Y) == backColor)
                {
                    SetPixel(img, (int)p.X + 1, (int)p.Y, fillColor);
                    //img[p.Y, p.X + 1] = fillColor;
                    points.Push(new Point(p.X + 1, p.Y));
                }
                if (p.Y > 0 && GetPixel(img, (int)p.X, (int)p.Y-1) == backColor)
                {
                    SetPixel(img, (int)p.X, (int)p.Y-1, fillColor);
                    //img[p.Y - 1, p.X] = fillColor;
                    points.Push(new Point(p.X, p.Y - 1));
                }
                if (p.Y < hh && GetPixel(img, (int)p.X, (int)p.Y+1) == backColor)
                {
                    SetPixel(img, (int)p.X, (int)p.Y+1, fillColor);
                    //img[p.Y + 1, p.X] = fillColor;
                    points.Push(new Point(p.X, p.Y + 1));
                }
            }
        }
        
        bool IsEdge(WriteableBitmap img, Point location,int sb, double h)
        {
            Color c = GetPixel(img, (int)location.X, (int)location.Y);
            if (c.A == 0) { return false; }
            int x = getBright(c);
            if (Math.Abs(x - sb) > h) { return true; }
            return false;
        }

        public int getFloodBrightEdge(WriteableBitmap SourceImage,out WriteableBitmap OutImage, Point location, double h)//查找亮度差异不大于h的边沿，给出内部的像素数，同时边沿标记特定颜色
        {
            Color xc = Colors.Orange;
            int width = SourceImage.PixelWidth;
            int height = SourceImage.PixelHeight;
            OutImage = WriteableBitmapHelper.Clone(SourceImage);
            if (location.X < 0 || location.X >= width || location.Y < 0 || location.Y >= height) return -1;//点选择非法
            Color backColor = GetPixel(SourceImage, (int)location.X, (int)location.Y);
            int x = getBright(backColor);

            Stack<IntPoint> points = new Stack<IntPoint>();
            //List<IntPoint> ePoint = new List<IntPoint>();
            List<int> ProcessList = new List<int>();//全部的处理点
            Dictionary<int, int> ProcessDic = new Dictionary<int, int>();

            points.Push(IntPoint.FromPoint(location));
            ProcessList.Add((int)(location.X+location.Y*width));
            int ww = width - 1;
            int hh = height - 1;
            int n = 0;
            
            while (points.Count > 0)
            {
                IntPoint p = points.Pop();
                SetPixel(OutImage, (int)p.X, (int)p.Y, Colors.Transparent);
                Color fillColor = Colors.Transparent;
                if (p.X > 0)
                {
                    //Point tp = new Point(p.X - 1, p.Y);
                    Color tc = GetPixel(OutImage, (int)p.X - 1, (int)p.Y);
                    if (tc != Colors.Transparent)
                    {
                        if (IsEdge(OutImage, new Point(p.X - 1, p.Y), x, h))
                        {
                            SetPixel(OutImage, (int)p.X - 1, (int)p.Y, xc);
                        }
                        else
                        {
                            n++;
                            SetPixel(OutImage, (int)p.X, (int)p.Y, fillColor);
                            points.Push(new IntPoint(p.X-1, p.Y));
                        }
                    }
                }
                if (p.X < ww)
                {
                    //Point tp = new Point(p.X + 1, p.Y);
                    Color tc = GetPixel(OutImage, (int)p.X + 1, (int)p.Y);
                    if (tc != Colors.Transparent)
                    {
                        if (IsEdge(OutImage, new Point(p.X + 1, p.Y), x, h))
                        {
                            SetPixel(OutImage, p.X + 1, p.Y, xc);
                        }
                        else
                        {
                            n++;
                            SetPixel(OutImage, (int)p.X, (int)p.Y, fillColor);
                            points.Push(new IntPoint(p.X + 1, p.Y));
                        }
                    }
                }
                if (p.Y > 0)
                {                    
                    //Point tp = new Point(p.X , p.Y- 1);
                    Color tc = GetPixel(OutImage, (int)p.X, (int)p.Y - 1);
                    if (tc != Colors.Transparent)
                    {
                        if (IsEdge(OutImage, new Point(p.X, p.Y - 1), x, h))
                        {
                            SetPixel(OutImage, (int)p.X, (int)p.Y - 1, xc);
                        }
                        else
                        {
                            n++;
                            SetPixel(OutImage, (int)p.X, (int)p.Y, fillColor);
                            points.Push(new IntPoint(p.X, p.Y - 1));
                        }
                    }
                }
                if (p.Y < hh)
                {                   
                    //Point tp = new Point(p.X , p.Y+1);
                    Color tc = GetPixel(OutImage, (int)p.X, (int)p.Y + 1);
                    if (tc != Colors.Transparent)
                    {
                        if (IsEdge(OutImage, new Point(p.X, p.Y + 1), x, h))
                        {
                            SetPixel(OutImage, (int)p.X, (int)p.Y + 1, xc);
                        }
                        else
                        {
                            n++;
                            SetPixel(OutImage, (int)p.X, (int)p.Y, fillColor);
                            points.Push(IntPoint.FromPoint(p.X, p.Y + 1));
                        }
                    }

                }
            }
            return n;
        }

        public Point? PixToPoint(WriteableBitmap b, FrameworkElement fe, int x, int y, bool IsUniform)
        {
            double cw, ch;
            if (fe.ActualWidth > 0) { cw = fe.ActualWidth; } else { cw = fe.Width; }
            if (cw == 0) { return null; }
            if (fe.ActualHeight > 0) { ch = fe.ActualHeight; } else { ch = fe.Height; }
            if (ch == 0) { return null; }
            double mx = b.PixelWidth / cw;
            double my = b.PixelHeight / ch;
            double hp = Convert.ToDouble(x) / b.PixelWidth;
            double vp = Convert.ToDouble(y) / b.PixelHeight;
            Point p = new Point();
            p.X = hp * cw;
            p.Y = vp * ch;
            if (IsUniform)
            {
                double tx, ty, fx, fy;
                if (mx > my)//水平的像素密度大，也就是说，垂直也需要按照大密度来计算，才可以显示完整，并保持纵横比
                {
                    ty = b.PixelHeight / mx;
                    fy = ch - ty;
                    p.Y = p.Y + fy / 2;
                }
                else
                {
                    tx = b.PixelWidth / my;
                    fx = cw - tx;
                    p.X = p.X + fx / 2;
                }
            }

            return p;
        }

        public Point? PointToPix(WriteableBitmap b, FrameworkElement fe, Point cp, bool IsUniform)
        {//b是图片，fe是包裹它的界面控件，此函数完成把一个界面控件点转换为图像上面的点
            double cw, ch;
            if (fe.ActualWidth > 0) { cw = fe.ActualWidth; } else { cw = fe.Width; }
            if (cw == 0) { return null; }
            if (fe.ActualHeight > 0) { ch = fe.ActualHeight; } else { ch = fe.Height; }
            if (ch == 0) { return null; }
            double hp = cp.X / cw;
            double vp = cp.Y / ch;
            double mx = b.PixelWidth / cw;
            double my = b.PixelHeight / ch;
            Point p = new Point();
            p.X = hp * b.PixelWidth;
            p.Y = vp * b.PixelHeight;

            if (IsUniform)
            {
                double tx, ty, fx, fy;
                if (mx > my)//水平的像素密度大，也就是说，垂直也需要按照大密度来计算，才可以显示完整，并保持纵横比
                {
                    ty = b.PixelHeight / mx;
                    fy = ch - ty;
                    p.Y = p.Y - fy / 2;
                }
                else
                {
                    tx = b.PixelWidth / my;
                    fx = cw - tx;
                    p.X = p.X - fx / 2;
                }
            }
            return p;
        }

        public string ImageToString(WriteableBitmap b)//把WriteableBitmap存为字符串
        {//字符串前两位是x，y，后面是分号分割的数字
            string s = "";
            //s = "<WriteableBitmap>";
            //s =s+ "<WriteableBitmapWidth>" + b.PixelWidth.ToString() + "</WriteableBitmapWidth>";
            //s = s+"<WriteableBitmapHeight>" + b.PixelHeight.ToString() + "</WriteableBitmapHeight>";
            //s=s+"<WriteableBitmapData>";
            s = b.PixelWidth.ToString() + ";" + b.PixelHeight.ToString() + ";";
            foreach (int x in b.PixelBuffer.ToArray())
            {
                s = s + x.ToString() + ";";
            }
            //s = s + "</WriteableBitmapData>";
            //s = s+"</WriteableBitmap>";
            return s;
        }

        public WriteableBitmap StringToImage(String s)//把字符串转为WriteableBitmap
        {
            if ((s == "") || (s == null)) { return null; }
            int x, y;
            string ss,ts;
            int i = s.IndexOf(";");
            if (i < 0) { return null; }
            ss = s.Substring(0, i );
            ts = s.Substring(i + 1);
            x = Convert.ToInt32(ss);
            i = ts.IndexOf(";");
            if (i < 0) { return null; }
            ss = ts.Substring(0, i);
            ts = ts.Substring(i + 1);
            y = Convert.ToInt32(ss);

            WriteableBitmap b = new WriteableBitmap(x, y);
            String[] sl = ts.Split(';');
            for (int c=0;c<sl.Length-1;c++)//最后一个是个分号分隔的空字符串，需要去掉
            {
                string xs = sl[c];
                
                b.PixelBuffer.ToArray()[i] = Convert.ToByte(xs);
            }
            return b;
        }

    }
    public struct IntPoint
    {
        public int X;
        public int Y;
        public Point ToPoint()
        {
            return new Point(X, Y);
        }
        public IntPoint(Point p)
        {
            X = Convert.ToInt32(p.X);
            Y = Convert.ToInt32(p.Y);
        }
        public IntPoint(double px,double py)
        {
            X = Convert.ToInt32(px);
            Y = Convert.ToInt32(py);
        }
        public static IntPoint FromPoint(Point p)
        {
            return new IntPoint(p);

        }
        public static IntPoint FromPoint(double px, double py)
        {
            return new IntPoint(px,py);

        }
    }

    public enum SignNoiseType
    {
        Gray, L, R, G, B
    }
}
