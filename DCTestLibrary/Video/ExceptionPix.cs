using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using DCTestLibrary;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Windows.UI;
using SilverlightLFC.common;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SilverlightDCTestLibrary.Video
{
    public class ExceptionPix
    {
        public LabMode CurrentLabMode
        {
            get { return pt.CurrentLabMode; }
            set { pt.CurrentLabMode = value; }
        }
        //记录亮度低于20%的点和亮度高于基础80%的点
        PhotoTest pt = new PhotoTest();
        double hPercent = 0.2;
        int w, h;
        int EqualLength = 1;//表示位置在3*3范围内都算作是一个坏点

        public List<PixInfor> ExceptionPixList = new List<PixInfor>();

        public List<PixInfor> getExceptionPix(WriteableBitmap b)
        {
            List<PixInfor> pl = new List<PixInfor>();
            double l = pt.getAverageColorL(b);
            byte[] p = b.PixelBuffer.ToArray();
            for (int y = 0; y < b.PixelHeight; y++)//遍历每一个像素
            {
                for (int x = 0; x < b.PixelWidth; x++)
                {
                    byte[] components = new byte[4];
                    components = BitConverter.GetBytes(p[y * b.PixelWidth + x]);
                    Color c = new Color();
                    c.R = components[2];
                    c.G = components[1];
                    c.B = components[0];
                    double cpl = ColorManager.getLabL(c,CurrentLabMode);

                    if ((cpl < l * (1 - hPercent))||(cpl > l * (1 + hPercent)))
                    {
                        PixInfor pi = new PixInfor();
                        pi.colorValue = c;
                        pi.XPosition = x;
                        pi.YPosition = y;
                        pl.Add(pi);
                    }


                }
            }
            return pl;
        }

        public List<PixInfor> getExceptionPix(Dictionary<TimeSpan, WriteableBitmap> bl)
        {
            ExceptionPixList.Clear();
            foreach (WriteableBitmap b in bl.Values)
            {
                w = b.PixelWidth;
                h = b.PixelHeight;
                List<PixInfor> pl = getExceptionPix(b);
                AddPixList(pl);
                if (ExceptionPixList.Count > 1000)
                {
                    throw new Exception("太多异常点");
                }
            }
            return ExceptionPixList;
        }

        public double? getExceptionPixPercent(Dictionary<TimeSpan, WriteableBitmap> bl)
        {
            getExceptionPix(bl);

            return ExceptionPixList.Count/(w*Convert.ToDouble(h));
        }


        public void AddPixList(List<PixInfor> pl)
        {
            foreach (PixInfor p in pl)
            {
                if(!IsContains(p)){
                    ExceptionPixList.Add(p);
                }
            }
        }

        bool IsContains(PixInfor p)
        {
            foreach (PixInfor cp in ExceptionPixList)
            {
                if (IsSame(p, cp))
                {
                    return true;
                }

            }
            return false;
        }

        bool IsSame(PixInfor p1, PixInfor p2)
        {
            if ((Math.Abs(p1.XPosition - p2.XPosition) < EqualLength) && (Math.Abs(p1.YPosition - p2.YPosition) < EqualLength))
            {
                return true;
            }
            return false;
        }
    }
}
