using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using DCTestLibrary;
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

namespace SilverlightDCTestLibrary.Video
{
    public class WhiteBalance
    {
        public double? getWhiteBalance(Dictionary<TimeSpan, WriteableBitmap> sl)
        {
            double y=0, pb=0, pr=0;
            if (sl.Count == 0) { return null; }
            
            foreach (WriteableBitmap b in sl.Values)
            {
                var p = b.PixelBuffer.ToArray();
                for (int i = 0; i < p.Length; i+=4)
                {
                    LColor lc = new LColor(Color.FromArgb(255, p[i+2], p[i+1], p[i]));
                    lc.CalculateRGB2YPbPr();
                    y += lc.YPbPr_Y;
                    pb += lc.YPbPr_Pb;
                    pr += lc.YPbPr_Pr;
                }

            }
            return y / Math.Max(pb, pr);
        }
    }
}
