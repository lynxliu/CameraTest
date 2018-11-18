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
using DCTestLibrary;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SilverlightDCTestLibrary.Video
{
    public class SNRTest//计算视频的信噪比
    {
        //public LabMode CurrentLabMode = LabMode.Photoshop;//lab的颜色模式选择
        Dictionary<TimeSpan, WriteableBitmap> SnapList = new Dictionary<TimeSpan,WriteableBitmap>();
        public double? SignValue=null;//如果有这个信号，就用这个信号计算


        public SignNoiseType currentNoiseType;

        int SnapCount = 0;
        int PixWidth = -1;
        int PixHeight = -1;
        bool CharkSnapList()
        {
            SnapCount = 0;
            if (SnapList == null || SnapList.Count == 0) { return false; }
            PixWidth = -1;
            PixHeight = -1;
            
            foreach (WriteableBitmap b in SnapList.Values)
            {
                if (PixWidth == -1) { PixWidth = b.PixelWidth; }
                if (PixHeight == -1) { PixHeight = b.PixelHeight; }
                if (PixWidth != b.PixelWidth) { return false; }
                if (PixHeight != b.PixelHeight) { return false; }
            }
            SnapCount = SnapList.Count;
            return true;
        }

        void PrepareSign()
        {

        }

        public double? getSNR(Dictionary<TimeSpan, WriteableBitmap> sl, LabMode CurrentLabMode)
        {
            SnapList = sl;
            if (!CharkSnapList()) { return null; }
            double AllAve = 0;//总平均
            double FrameAve = 0;//帧平均
            Dictionary<TimeSpan, double> FrameAveList = new Dictionary<TimeSpan, double>();
            long tp=0;
            
            List<double> PixAveList = new List<double>();//空间平均的列表

            if (SignValue == null)
            {
                foreach (KeyValuePair<TimeSpan, WriteableBitmap> tb in sl)
                {
                    tp = tp + tb.Value.PixelBuffer.Length;
                    for (int i = 0; i < tb.Value.PixelBuffer.Length; i++)
                    {
                        FrameAve = 0;
                        byte[] components = new byte[4];
                        components = BitConverter.GetBytes(tb.Value.PixelBuffer.ToArray()[i]);
                        double v = 0;

                        if (currentNoiseType == SignNoiseType.Gray)
                        {
                            v = .299 * components[2] + .587 * components[1] + .114 * components[0];
                        }
                        if (currentNoiseType == SignNoiseType.L)
                        {
                            v = ColorManager.getLabL(Color.FromArgb(255, components[2], components[1], components[0]),CurrentLabMode);
                        }
                        if (currentNoiseType == SignNoiseType.R)
                        {
                            v = components[2];
                        }
                        if (currentNoiseType == SignNoiseType.G)
                        {
                            v = components[1];
                        }
                        if (currentNoiseType == SignNoiseType.B)
                        {
                            v = components[0];
                        }
                        AllAve = AllAve + v;
                        FrameAve = FrameAve + v;
                        if (i >= PixAveList.Count)
                        {
                            PixAveList.Add(v);
                        }
                        else
                        {
                            PixAveList[i] = PixAveList[i] + v;
                        }
                    }
                    FrameAve = FrameAve / tb.Value.PixelBuffer.ToArray().Length;
                    FrameAveList.Add(tb.Key, FrameAve);
                }
                AllAve = AllAve / tp;//获取所有帧的平均值
            }
            else
            {
                foreach (KeyValuePair<TimeSpan, WriteableBitmap> tb in sl)
                {
                    tp = tp + tb.Value.PixelBuffer.ToArray().Length;
                    for (int i = 0; i < tb.Value.PixelBuffer.ToArray().Length; i++)
                    {

                        if (i >= PixAveList.Count)
                        {
                            PixAveList.Add(SignValue.Value);
                        }
                        else
                        {
                            PixAveList[i] = PixAveList[i] + SignValue.Value;
                        }
                    }
                    FrameAve = SignValue.Value;
                    FrameAveList.Add(tb.Key, FrameAve);
                }
                AllAve = SignValue.Value;

            }
            double s1=0, s2=0;
            foreach (KeyValuePair<TimeSpan, WriteableBitmap> tb in sl)
            {
                s1 = s1 + (FrameAveList[tb.Key] - AllAve) * (FrameAveList[tb.Key] - AllAve);
                for (int i = 0; i < tb.Value.PixelBuffer.ToArray().Length;i++ )
                {
                    byte[] components = new byte[4];
                    components = BitConverter.GetBytes(tb.Value.PixelBuffer.ToArray()[i]);
                    double v = 0;

                    if (currentNoiseType == SignNoiseType.Gray)
                    {
                        v = .299 * components[2] + .587 * components[1] + .114 * components[0];
                    }
                    if (currentNoiseType == SignNoiseType.L)
                    {
                        v = ColorManager.getLabL(Color.FromArgb(255, components[2], components[1], components[0]), CurrentLabMode);
                    }
                    if (currentNoiseType == SignNoiseType.R)
                    {
                        v = components[2];
                    }
                    if (currentNoiseType == SignNoiseType.G)
                    {
                        v = components[1];
                    }
                    if (currentNoiseType == SignNoiseType.B)
                    {
                        v = components[0];
                    }
                    s2 = s2 + (PixAveList[i] / sl.Count - v) * (PixAveList[i] / sl.Count - v);

                }
            }
            s1 = s1 / FrameAveList.Count;
            s2 = s2 / FrameAveList.Count / PixAveList.Count;
            return 20 * Math.Log10(AllAve / Math.Sqrt(s1 + s2));
        }
    }


}
