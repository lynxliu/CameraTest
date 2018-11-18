using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using SilverlightLynxControls;
using DCTestLibrary;
using SLPhotoTest.PhotoEdit;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;

namespace SLPhotoTest.PhotoInfor
{
    public partial class PhotoInfor : UserControl
    {
        public PhotoInfor()
        {
            InitializeComponent();
            //acm = new ActionMove(this, this);
        }
        //ActionMove acm;

        LChartPhoto pc;
        public void setTarget(LChartPhoto p)
        {
            pc = p;

        }

        public void DrawHis()
        {
            WriteableBitmap b=null;
            if (pc != null)
            {
                b = pc.getPhoto();
            }
            if (b != null)
            {
                DrawGraphic dg = new DrawGraphic(canvasBH);
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                List<int> bl = pt.getBrightPixNum(b);
                dg.DrawBrightPixNumHistogram(bl);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
                        WriteableBitmap b=null;
            if (pc != null)
            {
                b = pc.getPhoto();
            }
            if (b != null)
            {
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                float sx, sy;
                sx = Convert.ToSingle(textBoxPixH.Text) / b.PixelHeight;
                sy = Convert.ToSingle(textBoxPixW.Text) / b.PixelWidth;
                b = pt.ScaleBitmap(b, sx, sy).Result;
                pc.setPhoto(b);
            }
        }

        private void buttonAutoBright_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap b = null;
            if (pc != null)
            {
                b = pc.getPhoto();
            }
            if (b != null)
            {
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                b = pt.AutoBright(b);
                pc.setPhoto(b);
            }

        }

        private void buttonHis_Click(object sender, RoutedEventArgs e)
        {
            DrawHis();
        }

        private void buttonRead_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap b = null;
            if (pc != null)
            {
                b = pc.getPhoto();
            }
            if (b != null)
            {
                textBoxPixH.Text = b.PixelHeight.ToString();
                textBoxPixW.Text = b.PixelWidth.ToString();
                DrawHis();
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            (Parent as Panel).Children.Remove(this);
        }
        
    }
}
