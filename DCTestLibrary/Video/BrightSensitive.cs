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

namespace SilverlightDCTestLibrary.Video
{
    public class BrightSensitive
    {
        //取中央的10%像素计算平均值，记录起始亮度和当亮度变为一半时候的亮度
        public LabMode CurrentLabMode
        {
            get { return pt.CurrentLabMode; }
            set { pt.CurrentLabMode = value; }
        }
        public double TargetPercent = 0.5;
        public double CenterPercent = 0.1;

        public double BaseLight = 100;
        PhotoTest pt = new PhotoTest();
        public double? getCurrentBright(WriteableBitmap b)
        {
            if (b == null) return null;
            WriteableBitmap cb = pt.getImageArea(b, Convert.ToInt32(b.PixelWidth * (1 - CenterPercent) / 2), Convert.ToInt32(b.PixelHeight * (1 - CenterPercent) / 2), Convert.ToInt32(b.PixelWidth * CenterPercent), Convert.ToInt32(b.PixelHeight * CenterPercent));
            return pt.getAverageColorL(cb);
        }

        public TimeSpan? getCurrentBright(Dictionary<TimeSpan, WriteableBitmap> sl)
        {
            if (sl.Count == 0) { return null; }
            TimeSpan? preTime=null;
            TimeSpan? CurrentTime = null;
            double? preV = null;
            double? cv = null;
            //preTime = sl.Keys[0];
            
            foreach (KeyValuePair<TimeSpan, WriteableBitmap> kv in sl)
            {
                CurrentTime = kv.Key;
                
                cv = getCurrentBright(kv.Value);
                if (cv == null) { return null; }
                if (cv.Value > BaseLight * TargetPercent)
                {
                    preTime = kv.Key;
                    preV = cv;
                }
                else
                {
                    break;
                }
            }

            if (CurrentTime == null||preTime==null)
            {
                return null;//没有找到合理的间隔
            }

            TimeSpan xp = CurrentTime.Value - preTime.Value;
            double d = preV.Value - cv.Value;
            TimeSpan xt = TimeSpan.FromMilliseconds(preTime.Value.TotalMilliseconds + (TargetPercent * BaseLight - cv.Value) / (preV.Value - cv.Value) * (xp.TotalMilliseconds));
            return xt;

        }
    }
}
