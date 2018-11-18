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

namespace SilverlightDCTestLibrary.Video
{
    public class BrightConsistency
    {
        public LabMode CurrentLabMode
        {
            get { return pt.CurrentLabMode; }
            set { pt.CurrentLabMode = value; }
        }
        Dictionary<TimeSpan, WriteableBitmap> SnapList = new Dictionary<TimeSpan, WriteableBitmap>();
        PhotoTest pt = new PhotoTest();
        public List<WriteableBitmap> getSnapTestArea(WriteableBitmap b)
        {
            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            double ll = 3d * b.PixelWidth / 20-10;
            double lstep = 3.5 / 10 * b.PixelWidth;
            double tt = 3d * b.PixelHeight / 20 - 10;
            double tstep = 3.5 / 10 * b.PixelHeight;

            
            WriteableBitmap xb;
            xb= pt.getImageArea(b, Convert.ToInt32(ll), Convert.ToInt32(tt), 20, 20);
            bl.Add(xb);

            xb = pt.getImageArea(b, Convert.ToInt32(ll+lstep), Convert.ToInt32(tt), 20, 20);
            bl.Add(xb);

            xb = pt.getImageArea(b, Convert.ToInt32(ll+lstep+lstep), Convert.ToInt32(tt), 20, 20);
            bl.Add(xb);

            xb = pt.getImageArea(b, Convert.ToInt32(ll), Convert.ToInt32(tt+tstep), 20, 20);
            bl.Add(xb);

            xb = pt.getImageArea(b, Convert.ToInt32(ll + lstep), Convert.ToInt32(tt+tstep), 20, 20);
            bl.Add(xb);

            xb = pt.getImageArea(b, Convert.ToInt32(ll + lstep + lstep), Convert.ToInt32(tt+tstep), 20, 20);
            bl.Add(xb);

            xb = pt.getImageArea(b, Convert.ToInt32(ll), Convert.ToInt32(tt + tstep + tstep), 20, 20);
            bl.Add(xb);

            xb = pt.getImageArea(b, Convert.ToInt32(ll + lstep), Convert.ToInt32(tt + tstep + tstep), 20, 20);
            bl.Add(xb);

            xb = pt.getImageArea(b, Convert.ToInt32(ll + lstep + lstep), Convert.ToInt32(tt + tstep + tstep), 20, 20);
            bl.Add(xb);


            return bl;
        }

        public double getSnapBrightChanges(WriteableBitmap b)
        {
            List<double> ll = new List<double>();
            List<WriteableBitmap> al = getSnapTestArea(b);
            foreach (WriteableBitmap a in al)
            {
                ll.Add(pt.getAverageColorL(a));
            }

            double c=ll[4];
            ll.RemoveAt(4);
            double max = -1;
            foreach (double d in ll)
            {
                if (max < Math.Abs(1 - (d / c)))
                {
                    max = Math.Abs(1 - (d / c));
                }
            }
            return max;
        }

        public double? getBrightChanges(Dictionary<TimeSpan, WriteableBitmap> sl)
        {
            double max = -1;
            foreach (WriteableBitmap b in sl.Values)
            {
                double x = getSnapBrightChanges(b);
                if (max < x) { max = x; }
            }
            return max;
        }
    }
}
