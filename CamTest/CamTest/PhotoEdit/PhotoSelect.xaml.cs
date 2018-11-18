using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using DCTestLibrary;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

namespace SLPhotoTest.PhotoEdit
{
    public partial class PhotoSelect : UserControl
    {
        public PhotoSelect()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
        }
        ActionMove acm;

        PhotoEditCanvas pc;
        public void setTarget(PhotoEditCanvas p)
        {
            pc = p;
            if (p.SelectLayer != null)
            {
                c = p.SelectLayer.getCanvas();
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }

        Point sp, ep;
        int type;//1是line，2是矩形，3是圆，4是文本，5是手绘
        DrawGraphic dg;
        Canvas c;

        Color ForeColor = Colors.Blue;
        Color FillColor = Colors.Green;
        void c_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

            if (type == 2)
            {
                ep = e.GetCurrentPoint(pc.SelectLayer).Position;
                dg.DrawRectangle(sp.X, sp.Y, ep.X, ep.Y, false, 2, new SolidColorBrush(ForeColor), new SolidColorBrush(FillColor));
                pc.SelectLayer.setSelect(dg.getRectangleGeometry(sp.X, sp.Y, ep.X, ep.Y));
            }
            if (type == 3)
            {
                ep = e.GetCurrentPoint(pc.SelectLayer).Position;
                dg.DrawEllipse(sp.X, sp.Y, ep.X, ep.Y, false, 2, new SolidColorBrush(ForeColor), new SolidColorBrush(FillColor));
                pc.SelectLayer.setSelect(dg.getEllipseGeometry(sp.X, sp.Y, ep.X, ep.Y));
            }

            if (type == 5)
            {

                begin = false;
                c.PointerMoved -= new PointerEventHandler(c_PointerMoved);
                pc.SelectLayer.setSelect(pt.Data as PathGeometry);
            }
            c.PointerPressed -= new PointerEventHandler(c_PointerPressed);
            c.PointerReleased -= new PointerEventHandler(c_PointerReleased);
            e.Handled = true;
        }

        void c_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            sp = e.GetCurrentPoint(pc.SelectLayer).Position;
            if (type == 5)
            {
                InitPath(sp);
                begin = true;
            }
            e.Handled = true;
        }

        private void buttonEllips_Click(object sender, RoutedEventArgs e)
        {
            Canvas c = pc.SelectLayer.getDrawCanvas();
            dg = new DrawGraphic(c);

            type = 3;
            c.PointerPressed += new PointerEventHandler(c_PointerPressed);
            c.PointerReleased += new PointerEventHandler(c_PointerReleased);
        }

        private void buttonRectangle_Click(object sender, RoutedEventArgs e)
        {
            Canvas c = pc.SelectLayer.getDrawCanvas();
            dg = new DrawGraphic(c);

            type = 2;
            c.PointerPressed += new PointerEventHandler(c_PointerPressed);
            c.PointerReleased += new PointerEventHandler(c_PointerReleased);
        }

        private void buttonFree_Click(object sender, RoutedEventArgs e)
        {

            Canvas c = pc.SelectLayer.getDrawCanvas();
            dg = new DrawGraphic(c);

            type = 5;
            c.PointerMoved += new PointerEventHandler(c_PointerMoved);
            c.PointerPressed += new PointerEventHandler(c_PointerPressed);
            c.PointerReleased += new PointerEventHandler(c_PointerReleased);
        }
        Path pt;

        public void InitPath(Point cp)
        {
            pt = new Path();
            sp = cp;
            PathGeometry pg = new PathGeometry();
            pg.FillRule = FillRule.Nonzero;
            pt.Data = pg;
            PathFigure pf = new PathFigure();
            pt.StrokeDashArray = new DoubleCollection();
            pt.StrokeDashArray.Add(2);
            pt.StrokeDashArray.Add(1);
            pg.Figures.Add(pf);
            pf.StartPoint = cp;
            pt.Fill = new SolidColorBrush(Colors.Transparent);
            pt.Stroke = new SolidColorBrush(Colors.Blue);
            pt.StrokeThickness = 3;
            c.Children.Add(pt);
        }
        public void Draw(Point cp)
        {
            if (begin)//表示是第一次有个起始点
            {
                PathGeometry pg = pt.Data as PathGeometry;

                LineSegment l = new LineSegment();

                l.Point = cp;
                pg.Figures[0].Segments.Add(l);

            }

        }
        bool begin = false;
        void c_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (begin)
            {
                Draw(e.GetCurrentPoint(c).Position);
            }
        }


    }
}
