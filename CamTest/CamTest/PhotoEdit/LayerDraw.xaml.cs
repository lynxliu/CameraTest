using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using DCTestLibrary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation;

namespace SLPhotoTest.PhotoEdit
{
    public partial class LayerDraw : UserControl
    {
        public LayerDraw()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
        }
        ActionMove acm;
        PhotoEditCanvas pc;
        public void setTarget(PhotoEditCanvas p)
        {
            pc = p;

        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }

        Point sp,ep;
        int type;//1是line，2是矩形，3是圆，4是文本，5是手绘
        DrawGraphic dg;
        Canvas c ;
        private void buttonLine_Click(object sender, RoutedEventArgs e)
        {
            c = pc.SelectLayer.getDrawCanvas();
            if (c == null) { return; }
            dg = new DrawGraphic(c);

            type = 1;
            c.PointerPressed += new PointerEventHandler(c_PointerPressed);
            c.PointerReleased += new PointerEventHandler(c_PointerReleased);
        }
        Color ForeColor = Colors.Blue;
        //Color FillColor = Colors.Green;
        void c_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (type == 1)
            {
                ep = e.GetCurrentPoint(pc.SelectLayer).Position;
                dg.DrawLine(sp.X, sp.Y, ep.X, ep.Y, false, Convert.ToInt32 (textW.Text),new SolidColorBrush( ForeColor));
            }
            if (type == 2)
            {
                ep = e.GetCurrentPoint(pc.SelectLayer).Position;
                dg.DrawRectangle(sp.X, sp.Y, ep.X, ep.Y, checkBox1.IsChecked == true, Convert.ToInt32(textW.Text), new SolidColorBrush(ForeColor), new SolidColorBrush(ForeColor));
            }
            if (type == 3)
            {
                ep = e.GetCurrentPoint(pc.SelectLayer).Position;
                dg.DrawEllipse(sp.X, sp.Y, ep.X, ep.Y, checkBox1.IsChecked == true, Convert.ToInt32(textW.Text), new SolidColorBrush(ForeColor), new SolidColorBrush(ForeColor));
            }
            if (type == 4)
            {
                ep = e.GetCurrentPoint(pc.SelectLayer).Position;
                TextBox tb= dg.DrawEditText(sp.X, sp.Y, ep.X, ep.Y, "Text", new SolidColorBrush(ForeColor),ForeFont);
                tb.LostFocus += new RoutedEventHandler(tb_LostFocus);
            }
            if (type == 5)
            {

                begin = false;
                c.PointerMoved -= new PointerEventHandler(c_PointerMoved);
                
            }
            c.PointerPressed -= new PointerEventHandler(c_PointerPressed);
            c.PointerReleased -= new PointerEventHandler(c_PointerReleased);
            e.Handled = true;
        }

        void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            dg.DrawEditText(Canvas.GetLeft(tb), Canvas.GetTop(tb), Canvas.GetLeft(tb) + tb.Width, Canvas.GetTop(tb)+tb.Height, tb.Text, new SolidColorBrush(ForeColor), ForeFont);
            c.Children.Remove(tb);
            //throw new NotImplementedException();
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

        private void buttonText_Click(object sender, RoutedEventArgs e)
        {
            Canvas c = pc.SelectLayer.getDrawCanvas();
            dg = new DrawGraphic(c);

            type = 4;
            c.PointerPressed += new PointerEventHandler(c_PointerPressed);
            c.PointerReleased += new PointerEventHandler(c_PointerReleased);
        }
        ColorSelect cs = new ColorSelect();
        private void buttonColor_Click(object sender, RoutedEventArgs e)
        {
            SilverlightLFC.common.Environment.ShowChildWindow(Parent as Panel, cs, SelectColor_Closed);

        }

        void SelectColor_Closed(object sender,object args)
        {
            ForeColor = cs.getSelectColor();    
            //throw new NotImplementedException();
        }
        FontSelect fs = new FontSelect();
        private void buttonFont_Click(object sender, RoutedEventArgs e)
        {
            SilverlightLFC.common.Environment.ShowChildWindow(LayoutRoot, fs, FontColor_Closed);
            //ChildWindow cw = new ChildWindow();
            //cw.Content = fs;
            //cw.Show();
            //cw.Closed += new EventHandler(FontColor_Closed);
        }
        TextFont ForeFont;
        void FontColor_Closed(object sender, object e)
        {
            ForeFont = fs.getSelectFont();
            //throw new NotImplementedException();
        }

        private void buttonBS_Click(object sender, RoutedEventArgs e)
        {
            PhotoLayer c = pc.SelectLayer;
            c.BeginSelect();
        }

        private void buttonES_Click(object sender, RoutedEventArgs e)
        {
            PhotoLayer c = pc.SelectLayer;
            c.EndSelect();
        }
    }
}
