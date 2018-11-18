using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;

using SilverlightLynxControls;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;

namespace SLPhotoTest.PhotoEdit
{
    public partial class PhotoBrightCurve : UserControl
    {
        public PhotoBrightCurve()
        {
            InitializeComponent();
            dg = new DrawGraphic(DrawCanvas);
            acm = new ActionMove(this, this);
        }
        ActionMove acm;

        Image TargetImage;
        DrawGraphic dg;
        PhotoEditCanvas Target;
        public void setTarget(PhotoEditCanvas pc)//
        {
            //TargetImage = img;
            Target = pc;
            pc.PointerPressed += new PointerEventHandler(img_PointerPressed);
            pc.PointerReleased += new PointerEventHandler(img_PointerReleased);
            pc.PointerMoved += new PointerEventHandler(img_PointerMoved);
        }

        void img_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            TargetImage = Target.SelectLayer.getImage();
            if (begin)
            {
                ComboBoxItem ci = comboBoxSelectType.SelectedItem as ComboBoxItem;
                if (ci.Content.ToString() == "Path")
                {
                    ActiveDrawPath(e.GetCurrentPoint(TargetImage).Position);
                }
                if (ci.Content.ToString() == "Line")
                {
                    ActiveDrawLine(e.GetCurrentPoint(TargetImage).Position);
                }
                if (ci.Content.ToString() == "Area")
                {
                    ActiveDrawArea(e.GetCurrentPoint(TargetImage).Position);
                }
            }
        }

        Point sp;
        void img_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (Target == null) { return; }
            if (Target.SelectLayer == null) { return; }
            if (Target.SelectLayer.GetType().Name != "PhotoLayer") { return; }//只要针对位图图层,并且是当前图层,才可以进行跟踪
            TargetImage = Target.SelectLayer.getImage();

            if (TargetImage == null) { return; }
            sp = e.GetCurrentPoint(TargetImage).Position;//这个点,只是相对显示图来说的
            ComboBoxItem ci = comboBoxSelectType.SelectedItem as ComboBoxItem;
            if (ci.Content.ToString() == "Path")
            {

                CreatePath(sp);
            }
            if (ci.Content.ToString() == "Line")
            {

                CreateLine(sp);
            }

            if (ci.Content.ToString() == "Area")
            {

                CreateRect(sp);
            }
            begin = true;
            //e.Handled = true;
        }
        DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
        void img_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (Target == null) { return; }
            if (Target.SelectLayer == null) { return; }
            if (Target.SelectLayer.GetType().Name != "PhotoLayer") { return; }//只要针对位图图层,并且是当前图层,才可以进行跟踪
            TargetImage = Target.SelectLayer.getImage();

            if (TargetImage == null) { return; }

            ComboBoxItem ci = comboBoxSelectType.SelectedItem as ComboBoxItem;
            if (ci.Content.ToString() == "Path")
            {
                DrawPathBrightness();
                DeActiveDrawPath();
            }
            if (ci.Content.ToString() == "Line")
            {
                DrawLineBrightness();
                DeActiveDrawLine();
            }

            if (ci.Content.ToString() == "Area")
            {
                DrawAreaBrightness();
                DeActiveDrawArea();
            }
            begin = false;
            //IsLine = false;
            
            //e.Handled = true;
        }
        Path p;
        Line l;
        Rectangle r;

        public void CreatePath(Point cp)
        {
            p = new Path();
            sp = cp;
            PathGeometry pg = new PathGeometry();
            pg.FillRule = FillRule.Nonzero;
            p.Data = pg;
            PathFigure pf = new PathFigure();
            p.StrokeDashArray = new DoubleCollection();
            p.StrokeDashArray.Add(2);
            p.StrokeDashArray.Add(1);
            pg.Figures.Add(pf);
            pf.Segments.Clear();
            pf.StartPoint = cp;
            p.Fill = new SolidColorBrush(Colors.Transparent);
            p.Stroke = new SolidColorBrush(Colors.Blue);
            p.StrokeThickness = 3;
        }


        public void CreateLine(Point cp)
        {
            l = new Line();
            l.X1 = cp.X;
            l.Y1 = cp.Y;
            l.X2=cp.X;
            l.Y2=cp.X;
            l.Stroke = new SolidColorBrush(Colors.Blue);
            l.StrokeThickness = 3;

        }

        public void CreateRect(Point cp)
        {

                r = new Rectangle();
                sp = cp;
                r.Fill = new SolidColorBrush(Colors.Transparent);
                r.Stroke = new SolidColorBrush(Colors.Blue);
                r.StrokeThickness = 3;



        }

        public void ActiveDrawPath(Point cp)
        {
            if (Target == null) { return; }
            if (Target.SelectLayer == null) { return; }
            if (p == null) { return; }
            PhotoLayer pl = Target.SelectLayer;
            Canvas drawCanvas = pl.getTempObjectCanvas();
            PathGeometry pg = p.Data as PathGeometry;
            
            PathFigure pf = pg.Figures[0];
            if (pf == null) { return; }
            LineSegment ls = new LineSegment();
            ls.Point = cp;
            pf.Segments.Add(ls);
            if (drawCanvas.Children.Contains(p)) { }
            else
            {
                drawCanvas.Children.Add(p);
            }

        }

        public void ActiveDrawLine(Point cp)
        {
            if (Target == null) { return; }
            if (Target.SelectLayer == null) { return; }
            if (p == null) { return; }
            PhotoLayer pl = Target.SelectLayer;
            Canvas drawCanvas = pl.getTempObjectCanvas();
            l.X2=cp.X;
            l.Y2=cp.Y;
            if (drawCanvas.Children.Contains(l)) { }
            else
            {
                drawCanvas.Children.Add(l);
            }
        }

        public void ActiveDrawArea(Point cp)
        {
            if (Target == null) { return; }
            if (Target.SelectLayer == null) { return; }
            if (p == null) { return; }
            PhotoLayer pl = Target.SelectLayer;
            Canvas drawCanvas = pl.getTempObjectCanvas();
                double bl=0,bt=0,w=0,h=0;
                if(cp.X>sp.X)
                {
                    bl=sp.X;
                    w = cp.X - sp.X;
                }else
                {
                    bl = cp.X;
                    w=sp.X - cp.X;
                }
                if (cp.Y > sp.Y)
                {
                    bt = sp.Y;
                    h = cp.Y - sp.Y;
                }
                else
                {
                    bt = cp.Y;
                    h = sp.Y - cp.Y;
                }
                r.Width = w;
                r.Height = h;
                Canvas.SetLeft(r, bl);
                Canvas.SetTop(r, bt);
            if (drawCanvas.Children.Contains(r)) { }
            else
            {
                drawCanvas.Children.Add(r);
            }


        }

        public void DeActiveDrawPath()
        {
            if (Target == null) { return; }
            if (Target.SelectLayer == null) { return; }
            if (p == null) { return; }
            PhotoLayer pl = Target.SelectLayer;
            Canvas drawCanvas = pl.getTempObjectCanvas();
            if (drawCanvas.Children.Contains(p)) { drawCanvas.Children.Remove(p); }
            p = null;
        }

        public void DeActiveDrawLine()
        {
            if (Target == null) { return; }
            if (Target.SelectLayer == null) { return; }
            if (p == null) { return; }
            PhotoLayer pl = Target.SelectLayer;
            Canvas drawCanvas = pl.getTempObjectCanvas();
            if (drawCanvas.Children.Contains(l)) { drawCanvas.Children.Remove(l); }
            l = null;
        }

        public void DeActiveDrawArea()
        {
            if (Target == null) { return; }
            if (Target.SelectLayer == null) { return; }
            if (p == null) { return; }
            PhotoLayer pl = Target.SelectLayer;
            Canvas drawCanvas = pl.getTempObjectCanvas();
            if (drawCanvas.Children.Contains(r)) { drawCanvas.Children.Remove(r); }
            r = null;
        }


        public List<Color> getPathLine()
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            Point sp,cp;
            List<Color> cl = new List<Color>();
            if (p == null) { return null; }
            PathGeometry pg = p.Data as PathGeometry;
            if (pg == null) { return null; }
            foreach (PathFigure pf in pg.Figures)
            {
                sp = DrawGraphic.getImagePosition(pf.StartPoint, TargetImage);
                
                foreach (LineSegment ls in pf.Segments)
                {
                    cp = DrawGraphic.getImagePosition(ls.Point, TargetImage);
                    List<Color> scl=pt.getLine(TargetImage.Source as WriteableBitmap,sp,cp);
                    foreach (Color cc in scl)
                    {
                        cl.Add(cc);
                    }
                    sp = new Point(cp.X, cp.Y);
                }
            }
            return cl;
        }
        public void Draw(Point cp)
        {
            if (begin)//表示是第一次有个起始点
            {
                PathGeometry pg = p.Data as PathGeometry;

                LineSegment l = new LineSegment();

                l.Point = cp;
                pg.Figures[0].Segments.Add(l);

            }

        }
        bool begin = false;

        public void DrawPathBrightness()
        {
            if (p == null) { return ; }
            List<Point> pl = getPathPointList();
            List<Point> ipl = getImagePathPointList(pl);
            List<Color> cl = getPathColor(ipl);
            List<double> il = getBrightness(cl);
            dg.DrawLines(il);
            double d=0;
            if (ipl.Count < 2) { return; }
            Point sp = ipl[0];
            for (int i = 1; i < ipl.Count;i++ )
            {
                d = d + Math.Sqrt((ipl[i].X - sp.X) * (ipl[i].X - sp.X) + (ipl[i].Y - sp.Y) * (ipl[i].Y - sp.Y));
                sp = ipl[i];

            }
            textBlockValue.Text = il.Count.ToString();
        }

        public void DrawLineBrightness()
        {
            if (l == null) { return; }
            List<Point> pl = new List<Point>();
            pl.Add(new Point(l.X1, l.Y1));
            pl.Add(new Point(l.X2, l.Y2));
            List<Point> ipl = getImagePathPointList(pl);
            List<Color> cl = getPathColor(ipl);
            List<double> il = getBrightness(cl);
            dg.DrawLines(il);
            textBlockValue.Text = il.Count.ToString();

        }

        public void DrawAreaBrightness()
        {
            if (r == null) { return; }
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            DrawGraphic dg = new DrawGraphic(DrawCanvas);
            WriteableBitmap sb=TargetImage.Source as WriteableBitmap;
            Point isp=DrawGraphic.getImagePosition(sp, TargetImage);
            double iw=DrawGraphic.getImageDistance(r.Width,TargetImage);
            double ih=DrawGraphic.getImageDistance(r.Height,TargetImage);
            WriteableBitmap cb = pt.getImageArea(sb, (int)isp.X, (int)isp.Y, (int)iw, (int)ih);

            List<int> tl = pt.getBrightPixNum(cb);
            dg.ForeColor = Colors.Blue;
            dg.DrawBrightPixNumHistogram(tl);
            textBlockValue.Text = (iw * ih).ToString();
        }

        public List<double> getBrightness(List<Color> cl)
        {
            List<double> l = new List<double>();
            foreach (Color c in cl)
            {
                double d = .299 * c.R + .587 * c.G + .114 * c.B;
                l.Add(d);
            }
            return l;
        }

        public List<Color> getPathColor(List<Point> ipl)
        {
            if (ipl.Count == 0) { return null; }
            List<Color> cl = new List<Color>();
            Point sp=ipl[0];
            for(int i=1;i<ipl.Count;i++){
                List<Color> tcl = pt.getLine((WriteableBitmap)TargetImage.Source, sp, ipl[i]);
                foreach (Color c in tcl)
                {
                    cl.Add(c);
                }
                sp = ipl[i];
            }
            return cl;
        }

        public List<Point> getImagePathPointList(List<Point> pl)
        {
            List<Point> ipl = new List<Point>();
            foreach(Point tp in pl)
            {
                Point ip = DrawGraphic.getImagePosition(tp, TargetImage);
                ipl.Add(ip);
            }
            return ipl;
        }

        public List<Point> getPathPointList()
        {

            PathGeometry pg = p.Data as PathGeometry;
            PathFigure x = pg.Figures[0];
            List<Point> pl = new List<Point>();
            pl.Add(x.StartPoint);
            foreach (LineSegment ps in x.Segments)
            {
                Point tp = ps.Point;
                pl.Add(tp);
            }
            return pl;
        }

        private void buttonOption_Click(object sender, RoutedEventArgs e)
        {
            if (buttonOption.Content.ToString() == "Begin")
            {
                TargetImage.PointerPressed += new PointerEventHandler(img_PointerPressed);
                TargetImage.PointerReleased += new PointerEventHandler(img_PointerReleased);
                TargetImage.PointerMoved += new PointerEventHandler(img_PointerMoved);
                buttonOption.Content = "End";
            }
            else
            {
                buttonOption.Content = "Begin";
                TargetImage.PointerPressed -= new PointerEventHandler(img_PointerPressed);
                TargetImage.PointerReleased -= new PointerEventHandler(img_PointerReleased);
                TargetImage.PointerMoved -= new PointerEventHandler(img_PointerMoved);
            }
        }
        //bool IsLine = false;
        //private void buttonLine_Click(object sender, RoutedEventArgs e)
        //{
        //    IsLine = true;
        //}

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
        }
    }
}
