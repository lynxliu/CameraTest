using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;




namespace SilverlightLynxControls
{
    public class LynxArrow//支持绘制箭头
    {//每个线，如果需要箭头，需要包含一个这个对象
        public LynxArrow()//强制给出画布
        {
        }
        public LynxArrow(Panel p)//强制给出画布
        {
            ParentPanel = p;
        }
        public Line LeftArrowLine;
        public Line RightArrowLine;
        public Panel ParentPanel;

        public double ArrowLineLength = 15;
        public double ArrowLineWidth = 3;
        public Brush StrokeBrush =new SolidColorBrush(Colors.Blue);

        double getArc(Line l)
        {
            double tg = (l.Y2 - l.Y1) / (l.X2 - l.X1);
            return Math.Atan(tg);
        }

        public void DrawArrow(Line MainLine)//依据一根线,在末端绘制箭头
        {
            if (ParentPanel == null) { return; }
            StrokeBrush = MainLine.Stroke;
            double LineArc = getArc(MainLine);
            DrawLineCap(new Point(MainLine.X2, MainLine.Y2), LineArc);
        }

        public void DrawArrow(Canvas c,Line MainLine)//依据一根线,在末端绘制箭头
        {
            ParentPanel = c;
            DrawArrow(MainLine);
        }

        void DrawLineCap(Point StartPoint, double LineArc)//从某点开始以约定的长度和角度绘制箭头
        {
            double a1, a2;

            a1 = LineArc + Math.PI * 3 / 4;
            a2 = LineArc - Math.PI * 3 / 4;

            double ex1, ey1, ex2, ey2;
            ex1 = StartPoint.X + ArrowLineLength * Math.Cos(a1);
            ey1 = StartPoint.Y - ArrowLineLength * Math.Sin(a1);
            ex2 = StartPoint.X + ArrowLineLength * Math.Cos(a2);
            ey2 = StartPoint.Y - ArrowLineLength * Math.Sin(a2);

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

            LeftArrowLine.X1 = StartPoint.X;
            LeftArrowLine.X2 = ex1;
            LeftArrowLine.Y1 = StartPoint.Y;
            LeftArrowLine.Y2 = ey1;

            RightArrowLine.X1 = StartPoint.X;
            RightArrowLine.X2 = ex2;
            RightArrowLine.Y1 = StartPoint.Y;
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
