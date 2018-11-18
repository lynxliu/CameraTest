using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using DCTestLibrary;
using SilverlightLynxControls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI;

namespace SLPhotoTest.PhotoInfor
{
    public partial class PhotoColorInfor : UserControl
    {
        public PhotoColorInfor()
        {
            InitializeComponent();
            dg = new DrawGraphic(DrawCanvas);
            ShowCC();
            //acm = new ActionMove(this, this);
        }
        //ActionMove acm;

        Image si;
        DrawGraphic dg;
        LChartPhoto Target;
        public void setTarget(LChartPhoto pc)//
        {
            Target = pc;
            //si = img;
            Target.PointerPressed += img_PointerPressed;
            Target.PointerReleased += img_PointerReleased;
        }
        Point sp;
        void img_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            si=Target.getImage();
            //double dx = Canvas.GetLeft(si)+Canvas.GetLeft(Target.SelectLayer);
            //double dy = Canvas.GetTop(si) + Canvas.GetTop(Target.SelectLayer);
            if (si.Source != null)
            {
                Target.DisableMove();
                sp = e.GetCurrentPoint(si).Position;
            }
        }
        DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
        WriteableBitmap sb=null;
        void img_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

            si = Target.getImage();
            if (si.Source == null)
            { return; }
            //double dx = Canvas.GetLeft(si) + Canvas.GetLeft(Target.SelectLayer);
            //double dy = Canvas.GetTop(si) + Canvas.GetTop(Target.SelectLayer);
            Point ep = e.GetCurrentPoint(si).Position;

            if (Math.Abs(sp.X - ep.X) < 1.1 && Math.Abs(sp.Y - ep.Y) < 1.1)
            {
                Point tp = DrawGraphic.getImagePosition(sp, si);
                Color c = pt.GetPixel(si.Source as WriteableBitmap, Convert.ToInt32(tp.X), Convert.ToInt32(tp.Y));
                ShowColor(c);
                sb = null;
                sb=new WriteableBitmap(1,1);
                pt.SetPixel(sb,0,0,c);
                ColorImg.Source = sb;
                //ColorImg.
            }
            else
            {
                int sx,sy,w,h;
                Point isp=DrawGraphic.getImagePosition(sp, si);
                Point iep=DrawGraphic.getImagePosition(ep, si);
                if(isp.X<iep.X){
                    sx=Convert.ToInt32(isp.X);
                    w=Convert.ToInt32(iep.X-isp.X);
                }else{
                    sx=Convert.ToInt32(iep.X);
                    w=Convert.ToInt32(isp.X-iep.X);
                }
                if(isp.Y<iep.Y){
                    sy=Convert.ToInt32(isp.Y);
                    h=Convert.ToInt32(iep.Y-isp.Y);
                }
                else{
                    sy=Convert.ToInt32(iep.Y);
                    h=Convert.ToInt32(isp.Y-iep.Y);
                }
                
                if (w == 0) { w = 1; }
                if(h==0){h=1;}
                sb = null;
                sb = pt.getImageArea(si.Source as WriteableBitmap, sx, sy, w, h);
                ColorImg.Source = sb;
                Color c = pt.getAverageColor(sb);
                ShowColor(c);
            }
            sp = DrawGraphic.getImagePosition(sp, si);
        }

        public void ShowCC()
        {
            
            DrawCanvas.Children.Clear();
            dg.DrawColorCy(0.9f);
        }

        public void ShowColor(Color c)
        {
            textB.Text = c.B.ToString();
            textBr.Text = DCTestLibrary.PhotoTest.getBrightness(c).ToString();
            textG.Text = c.G.ToString();
            textH.Text = DCTestLibrary.PhotoTest.getHue(c).ToString();
            textR.Text = c.R.ToString();
            textS.Text = DCTestLibrary.PhotoTest.getSaturation(c).ToString();
            dg.DrawColorPoint(c, 0.9f, 3);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            ShowCC();
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            if (Target != null)
            {
                Target.PointerPressed -= img_PointerPressed;
                Target.PointerReleased -= img_PointerReleased;
            }
            (Parent as Panel).Children.Remove(this);
        }
    }
}
