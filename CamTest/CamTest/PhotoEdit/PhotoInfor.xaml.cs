using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using SilverlightLynxControls;
using DCTestLibrary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;

namespace SLPhotoTest.PhotoEdit
{
    public partial class PhotoInfor : UserControl
    {
        public PhotoInfor()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
        }
        ActionMove acm;

        

        //public void setTarget(Image  im)
        //{
        //    TargetImage = im;
        //    WriteableBitmap  b=TargetImage.Source as WriteableBitmap;
        //    if (b != null)
        //    {
        //        textBoxPixH.Text = b.PixelHeight.ToString();
        //        textBoxPixW.Text = b.PixelWidth.ToString();
        //        DrawHis();
        //    }
        //}
        PhotoEditCanvas pc;
        public void setTarget(PhotoEditCanvas  p)
        {
            pc = p;
            //WriteableBitmap b = pc.SelectLayer.getPhoto();
            //if (b != null)
            //{
            //    textBoxPixH.Text = b.PixelHeight.ToString();
            //    textBoxPixW.Text = b.PixelWidth.ToString();
            //    DrawHis();
            //}
        }

        public void DrawHis()
        {
            WriteableBitmap b=null;
            if (pc.SelectLayer != null)
            {
                b = pc.SelectLayer.getPhoto();
            }
            if (b != null)
            {
                DrawGraphic dg = new DrawGraphic(canvasBH);
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                List<int> bl = pt.getBrightPixNum(b);
                dg.DrawBrightPixNumHistogram(bl);
            }
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
                        WriteableBitmap b=null;
            if (pc.SelectLayer != null)
            {
                b = pc.SelectLayer.getPhoto();
            }
            if (b != null)
            {
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                float sx, sy;
                sx = Convert.ToSingle(textBoxPixH.Text) / b.PixelHeight;
                sy = Convert.ToSingle(textBoxPixW.Text) / b.PixelWidth;
                b = await pt.ScaleBitmap(b, sx, sy);
                pc.SelectLayer.setPhoto(b,pc.ScalePercent);
            }
        }

        private void buttonAutoBright_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap b = null;
            if (pc.SelectLayer != null)
            {
                b = pc.SelectLayer.getPhoto();
            }
            if (b != null)
            {
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                b = pt.AutoBright(b);
                pc.SelectLayer.setPhoto(b, pc.ScalePercent);
            }

        }

        private void buttonHis_Click(object sender, RoutedEventArgs e)
        {
            DrawHis();
        }

        private void buttonRead_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap b = null;
            if (pc.SelectLayer != null)
            {
                b = pc.SelectLayer.getPhoto();
            }
            if (b != null)
            {
                textBoxPixH.Text = b.PixelHeight.ToString();
                textBoxPixW.Text = b.PixelWidth.ToString();
                DrawHis();
            }
        }
        
    }
}
