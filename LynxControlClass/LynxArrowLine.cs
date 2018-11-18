using System;
using System.Net;
using System.Windows;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


using System.Windows.Input;



using SilverlightLynxControls.LogicView;
using SilverlightLFC.common;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI;

namespace SilverlightLynxControls
{
    public class LynxArrowLine:FrameworkElement //包含一个数据对象，可以被记录的
    {
        public LynxArrowLine() { }
        public LynxArrowLine(Panel p)
        {
            ParentPanel = p;
        }

        TextBlock mb = new TextBlock();
        Line MainLine;
        Point StartPoint;
        Point EndPoint;
        Line LeftArrowLine;
        Line RightArrowLine;
        public Panel ParentPanel;

        public double MainLineWidth = 5;
        public double ArrowLineLength = 15;
        public double ArrowLineWidth = 3;
        public Brush StrokeBrush = new SolidColorBrush(Colors.Blue);

        public string Memo { get; set; }

        public void DrawArrowLine(Panel p,Point sp,Point ep) 
        { 
            ParentPanel = p;
            StartPoint = sp;
            EndPoint = ep;
            DrawArrowLine(StartPoint, EndPoint); 
        }

        public void DrawArrowLine() { DrawArrowLine(StartPoint,EndPoint); }

        public void DrawArrowLine(Point sp, Point ep)
        {
            if (ParentPanel == null) { return; }
            StartPoint = sp;
            EndPoint = ep;

            if (MainLine == null) { MainLine = new Line(); }
            if (ParentPanel.Children.Contains(MainLine)) { }
            else
            {
                ParentPanel.Children.Add(MainLine);
            }
            MainLine.Stroke = StrokeBrush;
            MainLine.StrokeThickness = MainLineWidth;
            MainLine.X1 = StartPoint.X;
            MainLine.X2 = EndPoint.X;
            MainLine.Y1 = StartPoint.Y;
            MainLine.Y2 = EndPoint.Y;
            DrawArrow(MainLine);
        }
        public void RemoveArrowLine()
        {
            if (ParentPanel == null) { return; }
            if (ParentPanel.Children.Contains(MainLine)) { ParentPanel.Children.Remove(MainLine); }
            if (ParentPanel.Children.Contains(LeftArrowLine)) { ParentPanel.Children.Remove(LeftArrowLine); }
            if (ParentPanel.Children.Contains(RightArrowLine)) { ParentPanel.Children.Remove(RightArrowLine); }

        }
        public void MoveArrowLine()
        {
            double a1, a2;
            double a = getArc(MainLine);
            a1 = a + Math.PI*3 / 4;
            a2 = a - Math.PI*3 / 4;

            double ex1, ey1, ex2, ey2;
            ex1 = EndPoint.X + ArrowLineLength * Math.Cos(a1);
            ey1 = EndPoint.Y - ArrowLineLength * Math.Sin(a1);
            ex2 = EndPoint.X + ArrowLineLength * Math.Cos(a2);
            ey2 = EndPoint.Y - ArrowLineLength * Math.Sin(a2);

            LeftArrowLine.X1 = EndPoint.X;
            LeftArrowLine.X2 = ex1;
            LeftArrowLine.Y1 = EndPoint.Y;
            LeftArrowLine.Y2 = ey1;

            RightArrowLine.X1 = EndPoint.X;
            RightArrowLine.X2 = ex2;
            RightArrowLine.Y1 = EndPoint.Y;
            RightArrowLine.Y2 = ey2;
        }

        public void MoveLineStartPoint(double dx,double dy)
        {
            StartPoint.X = StartPoint.X + dx;
            StartPoint.Y = StartPoint.Y + dy;
            MainLine.X1 = StartPoint.X;
            MainLine.Y1 = StartPoint.Y;
            MoveArrowLine();
        }

        public void MoveLineEndPoint(double dx, double dy)
        {
            EndPoint.X = EndPoint.X + dx;
            EndPoint.Y = EndPoint.Y + dy;
            MainLine.X2 = EndPoint.X;
            MainLine.Y2 = EndPoint.Y;
            MoveArrowLine();
            //DrawLine(StartPoint, EndPoint);
        }

        public void MoveLineStartPoint(Point nsp)
        {
            double dx, dy;
            dx = nsp.X - StartPoint.X;
            dy = nsp.Y = StartPoint.Y;
            MoveLineStartPoint(dx,dy);
        }

        public void MoveLineEndPoint(Point nep)
        {
            double dx, dy;
            dx = nep.X - EndPoint.X;
            dy = nep.Y = EndPoint.Y;
            MoveLineStartPoint(dx, dy);
        }

        void MainLine_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (ParentPanel.Children.Contains(mb)) { ParentPanel.Children.Remove(mb); }
        }

        void MainLine_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (ParentPanel.Children.Contains(mb)) { }
            else
            {
                ParentPanel.Children.Add(mb);
            }
            mb.Text = Memo;
            Canvas.SetLeft(mb, e.GetCurrentPoint(ParentPanel).Position.X);
            Canvas.SetTop(mb, e.GetCurrentPoint(ParentPanel).Position.Y);
        }

        public double getArc(Line l)//获得一个直线的倾角
        {
            if (l.Y1 == l.Y2) { return 0; }
            if (l.X2 == l.X1) { return Math.PI / 2; }
            double tg = -(l.Y2 - l.Y1) / (l.X2 - l.X1);
            double a = Math.Atan(tg);
            if (l.X2 >= l.X1) { }
            else
            {
                a = a + Math.PI;
            }
            return a;
        }

        void DrawArrow(Line l)
        {
            double LineArc = getArc(l);
            DrawLineCap(new Point(l.X2, l.Y2), LineArc);
        }

        void DrawLineCap(Point StartPoint, double LineArc)//从某点开始以约定的长度和角度绘制箭头
        {
            double a1, a2;

            a1 = LineArc + Math.PI  *0.75;
            a2 = LineArc - Math.PI  *0.75;

            double ex1, ey1, ex2, ey2;
            ex1 = EndPoint.X + ArrowLineLength * Math.Cos(a1);
            ey1 = EndPoint.Y - ArrowLineLength * Math.Sin(a1);
            ex2 = EndPoint.X + ArrowLineLength * Math.Cos(a2);
            ey2 = EndPoint.Y - ArrowLineLength * Math.Sin(a2);

            if (LeftArrowLine == null)
            {
                LeftArrowLine = new Line();
            }
            if (RightArrowLine == null)
            {
                RightArrowLine = new Line();
            }

            LeftArrowLine.StrokeThickness = ArrowLineWidth;
            LeftArrowLine.Stroke = StrokeBrush;

            RightArrowLine.StrokeThickness = ArrowLineWidth;
            RightArrowLine.Stroke = StrokeBrush;

            LeftArrowLine.X1 = EndPoint.X;
            LeftArrowLine.X2 = ex1;
            LeftArrowLine.Y1 = EndPoint.Y;
            LeftArrowLine.Y2 = ey1;

            RightArrowLine.X1 = EndPoint.X;
            RightArrowLine.X2 = ex2;
            RightArrowLine.Y1 = EndPoint.Y;
            RightArrowLine.Y2 = ey2;

            if (!ParentPanel.Children.Contains(LeftArrowLine))
            {
                ParentPanel.Children.Add(LeftArrowLine);
            }
            if (!ParentPanel.Children.Contains(RightArrowLine))
            {
                ParentPanel.Children.Add(RightArrowLine);
            }
        }

    }
}
