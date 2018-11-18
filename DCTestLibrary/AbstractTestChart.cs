using SilverlightLFC.common;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace DCTestLibrary
{

    public abstract class AbstractTestChart:PhotoTest
    {
        public abstract string ChartName { get;}
        public abstract string ChartMemo { get;}
        public abstract void CorrectChart();
        public event TestStep TestProcessStep;

        public void sendTestStepInfor(bool b, string s, double d)
        {
            if (TestProcessStep != null)
            {
                TestProcessStep(this, new TestStepArgs(b, s, d));
            }
        }

        public void Clear()//释放所有的资源
        {
            //ChartPhoto = null;
            //AnalysePhoto = null;
            //CorrectPhoto = null;
            if (mp != null)
            {
                mp.Clear();
            }
            GC.Collect();
        }

        public MarkProcess mp = new MarkProcess(null);
        //public WriteableBitmap ChartPhoto;//基本的测试卡照片
        //public WriteableBitmap AnalysePhoto;//分析调试用的照片
        //public WriteableBitmap CorrectPhoto;//矫正以后照片
        public bool IsAnalyse = true;//标识是不是在调试的状态
        public PhotoTestParameter ptp = new PhotoTestParameter();//用来进行测试的指标代码
        public WriteableBitmap AnalysePhoto
        {
            get
            {
                return mp.SelectedPhoto;
            }
        }
        public WriteableBitmap ChartPhoto//原始照片
        {
            get
            {
                return mp.SourcePhoto;
            }
            set
            {
                //ChartPhoto = value;
                mp.SourcePhoto = value;
            }
        }
        public WriteableBitmap CorrectPhoto
        {
            get
            {
                return mp.CorrectPhoto;
            }
            set
            {
                //ChartPhoto = value;
                mp.CorrectPhoto = value;
            }
        }
        //public WriteableBitmap SourcePhoto
        //{
        //    get
        //    {
        //        return mp.SourcePhoto;
        //    }
        //    set
        //    {
        //        //ChartPhoto = value;
        //        mp.SourcePhoto = value;
        //    }
        //}

        public virtual void BeginAnalyse()
        {
            IsAnalyse = true;
            //if (ChartPhoto == null) { ChartPhoto = mp.SourcePhoto; }
            //AnalysePhoto = new WriteableBitmap(ChartPhoto);
        }//开始调试状态，主要是保存选取区域

        public void DrawSelectArea(Point SPoint, int w, int h)//在分析照片上面绘制出选择的区域，便于调试
        {
            DrawSelectArea(Convert.ToInt32(SPoint.X), Convert.ToInt32(SPoint.Y), w, h);
        }
        public void DrawSelectArea(int StartX, int StartY, int w, int h)//在分析照片上面绘制出选择的区域，便于调试
        {
            if (AnalysePhoto == null)
            {
                if (mp.SourcePhoto == null) { return; }
                mp.SelectedPhoto = WriteableBitmapHelper.Clone(mp.SourcePhoto);
            }
            DrawRecArea(AnalysePhoto, StartX, StartY, w, h, Colors.Red);
            //mp.SelectedPhoto = new WriteableBitmap(AnalysePhoto);
        }

        public void setSelectArea(string key, WriteableBitmap b)//自动设置选区，并且改变不会影响原始信息
        {
            if (mp.SelectedArea.ContainsKey(key))
            {
                mp.SelectedArea[key] = WriteableBitmapHelper.Clone(b);
            }
            else
            {
                mp.SelectedArea.Add(key, WriteableBitmapHelper.Clone(b));
            }
        }

        public void setChart(WriteableBitmap b)
        {
            mp.SourcePhoto = b;

            //ChartPhoto = b;
        }

        public WriteableBitmap getCorrectPhoto()//得到最终的矫正图像
        {
            if (mp.CorrectPhoto != null)
            {
                return mp.CorrectPhoto;
            }

            if (mp.CorrectScalePhoto != null)
            {
                return mp.CorrectScalePhoto;
            }
            if (mp.CorrectRotatePhoto != null)
            {
                return mp.CorrectRotatePhoto;
            }
            if (mp.CorrectMovePhoto != null)
            {
                return mp.CorrectMovePhoto;
            }
            if (mp.CorrectBrightPhoto != null)
            {
                return mp.CorrectBrightPhoto;
            }
            return null;
        }
    }
    public delegate void  TestStep(object sender, TestStepArgs e);
    public class TestStepArgs//用来给需要多步进行的测试标度进度
    {
        public string Memo;//传递的说明信息
        public DateTime HappenTime=DateTime.Now;//完成时间
        public bool IsSuccess;//是否成功
        public double FinishTaskPercent;
        public string AuthorInfor = "Lynx 2010 (lynxliu2002@163.com)";
        public TestStepArgs(bool b,string s,double d)
        {
            Memo = s;
            IsSuccess = b;
            FinishTaskPercent = d;
        }
    }
}
