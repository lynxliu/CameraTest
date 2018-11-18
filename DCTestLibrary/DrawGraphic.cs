using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using SilverlightLFC.common;

namespace DCTestLibrary
{
    public class DrawGraphic//是给绘制曲线使用的类
    {//也包括编辑图片的类，特别是坐标转换
        public DrawGraphic()
        {
        }
        public DrawGraphic(Canvas c)
        {
            double w, h;
            if (c.ActualHeight != 0 && c.ActualHeight != Double.NaN)
            {
                h = c.ActualHeight;
            }
            else
            {
                h = c.Height;
            }
            if (c.ActualWidth != 0 && c.ActualWidth != Double.NaN)
            {
                w = c.ActualWidth;
            }
            else
            {
                w = c.Width;
            }
            DrawCanvas = c;
            DrawArea = new Rectangle();
            DrawArea.Width = w * 0.9;
            DrawArea.Height = h * 0.9;
            DrawArea.SetValue(Canvas.LeftProperty, w * 0.05);
        }
        public string status;//记录所有的调用状态
        public Color ForeColor = Colors.Red;
        public Canvas DrawCanvas;//代表画布，实际的绘制都是在这里进行的
        public Rectangle DrawArea;//代表实际的绘图区域。
        public Color AxisColor = Colors.Black;
        public Color TextColor = Colors.Blue;
        private double XMax = 0, YMax = 255, XMin = 0, YMin = 0;
        private List<double> XMarkList, YMarkList;

        //private float XLborderPercent = 0.1f;//是指X左边沿占到的比例
        //private float YBborderPercent = 0.1f;//是指Y下边沿占到的比例
        //private float XRborderPercent = 0.05f;//是指X右边沿占到的比例
        //private float YTborderPercent = 0.05f;//是指Y上边沿占到的比例

        public void DrawLineCap(Point StartPoint, double CapSize, double LineArc, bool IsArc)//从某点开始以约定的长度和角度绘制箭头
        {
            double a1, a2;
            if (IsArc)
            {//弧度给的角
                a1 = LineArc + Math.PI * 3 / 4;
                a2 = LineArc - Math.PI * 3 / 4;
            }
            else
            {
                a1 = LineArc + 135;
                a2 = LineArc - 135;

                a1 = a1 / 180 * Math.PI;
                a2 = a2 / 180 * Math.PI;
            }
            double ex1, ey1, ex2, ey2;
            ex1 = StartPoint.X + CapSize * Math.Cos(a1);
            ey1 = StartPoint.Y - CapSize * Math.Sin(a1);
            ex2 = StartPoint.X + CapSize * Math.Cos(a2);
            ey2 = StartPoint.Y - CapSize * Math.Sin(a2);

            Line l1, l2;
            l1 = new Line();
            l1.StrokeThickness = 3;
            l1.Stroke = new SolidColorBrush(AxisColor);
            l2 = new Line();
            l2.StrokeThickness = 3;
            l2.Stroke = new SolidColorBrush(AxisColor);

            l1.X1 = StartPoint.X;
            l1.X2 = ex1;
            l1.Y1 = StartPoint.Y;
            l1.Y2 = ey1;

            l2.X1 = StartPoint.X;
            l2.X2 = ex2;
            l2.Y1 = StartPoint.Y;
            l2.Y2 = ey2;

            DrawCanvas.Children.Add(l1);
            DrawCanvas.Children.Add(l2);
        }

        public void DrawLine(double x0, double y0, double x1, double y1, bool IsArrow, double w, Brush b)
        {
            Line l = new Line();
            l.StrokeThickness = w;
            l.Stroke = b;
            l.X1 = x0;
            l.X2 = x1;
            l.Y1 = y0;
            l.Y2 = y1;

            DrawCanvas.Children.Add(l);
            if (IsArrow)
            {
                double arc = Math.Atan((y1 - y0) / (x1 - x0));
                DrawLineCap(new Point(l.X2, l.Y2), 7, arc, true);
            }
            //DrawLineCap(new Point(l.X2, l.Y2), 7, 0);
        }

        public void DrawX()//绘制X轴,实际就是在画布的底部绘制一条直线，加上箭头的线帽
        {
            Line l = new Line();
            l.StrokeThickness = 5;
            l.Stroke = new SolidColorBrush(Colors.Blue);
            l.X1 = (double)DrawArea.GetValue(Canvas.LeftProperty);
            l.X2 = l.X1 + DrawArea.ActualWidth;
            l.Y1 = (double)DrawArea.GetValue(Canvas.TopProperty) + DrawArea.ActualHeight;
            l.Y2 = l.Y1;

            DrawCanvas.Children.Add(l);
            DrawLineCap(new Point(l.X2, l.Y2), 7, 0, false);
        }

        public void DrawY()
        {
            Line l = new Line();
            l.StrokeThickness = 5;
            l.Stroke = new SolidColorBrush(Colors.Blue);
            l.X1 = (double)DrawArea.GetValue(Canvas.LeftProperty);
            l.X2 = l.X1;
            l.Y1 = (double)DrawArea.GetValue(Canvas.TopProperty) + DrawArea.ActualHeight;
            l.Y2 = (double)DrawArea.GetValue(Canvas.TopProperty);
            DrawCanvas.Children.Add(l);
            DrawLineCap(new Point(l.X2, l.Y2), 7, 90, false);
        }

        public void AddXLable(string s, string dimension)//绘制X轴的名称
        {
            s = s + "(" + dimension + ")";
            TextBlock tb = new TextBlock();
            tb.Foreground = new SolidColorBrush(TextColor);
            tb.FontStyle = FontStyle.Italic;
            tb.FontSize = 12;
            tb.FontWeight = FontWeights.Bold;
            tb.Text = s;
            DrawCanvas.Children.Add(tb);
            tb.SetValue(Canvas.LeftProperty, (double)DrawArea.GetValue(Canvas.LeftProperty) + DrawArea.Width + 2);
            tb.SetValue(Canvas.TopProperty, (double)DrawArea.GetValue(Canvas.TopProperty) + DrawArea.Height);
        }

        public void AddYLable(string s, string dimension)
        {
            s = s + "(" + dimension + ")";
            TextBlock tb = new TextBlock();
            tb.Foreground = new SolidColorBrush(TextColor);
            tb.FontStyle = FontStyle.Italic;
            tb.FontSize = 12;
            tb.FontWeight = FontWeights.Bold;
            tb.Text = s;
            DrawCanvas.Children.Add(tb);
            tb.SetValue(Canvas.LeftProperty, (double)DrawArea.GetValue(Canvas.LeftProperty));
            tb.SetValue(Canvas.TopProperty, (double)DrawArea.GetValue(Canvas.TopProperty) - DrawArea.Height - 2);

        }

        public void DrawXMark(List<double> MarkList)
        {
            XMarkList = MarkList;
            DrawXMark();
        }

        private void DrawXMark()
        {
            if (XMarkList.Count == 0) { return; }
            int step = Convert.ToInt32(Convert.ToDecimal(DrawArea.Width / (XMarkList.Count - 1)));
            Brush br = new SolidColorBrush(ForeColor);
            double Scale = 1;
            XMin = XMarkList[0];
            if (XMax == 0)
            {
                XMax = XMarkList[XMarkList.Count - 1];
            }

            if (XMax != XMarkList[XMarkList.Count - 1])
            {
                Scale = (XMarkList[XMarkList.Count - 1]-XMarkList[0]) / (XMax-XMin);
            }

            for (int i = 0; i < XMarkList.Count; i++)
            {
                string s = XMarkList[i].ToString();
                Line l = new Line();
                l.Stroke = new SolidColorBrush(AxisColor);
                l.StrokeThickness = 2;

                l.X1 = (double)DrawArea.GetValue(Canvas.LeftProperty) + i * step * Scale;
                l.X2 = l.X1;
                l.Y1 = (double)DrawArea.GetValue(Canvas.TopProperty) + DrawArea.Height;
                l.Y2 = l.Y1 - 10;
                DrawCanvas.Children.Add(l);

                TextBlock tb = new TextBlock();
                tb.Foreground = new SolidColorBrush(TextColor);
                tb.FontStyle = FontStyle.Italic;
                tb.FontSize = 12;
                tb.FontWeight = FontWeights.Bold;
                tb.Text = s;
                DrawCanvas.Children.Add(tb);
                tb.SetValue(Canvas.LeftProperty, l.X1);
                tb.SetValue(Canvas.TopProperty, l.Y1 + 7);

            }
        }

        public void DrawYMark(List<double> MarkList)
        {
            YMarkList = MarkList;
            DrawYMark();
        }

        private void DrawYMark()
        {
            int step = Convert.ToInt32(Convert.ToDecimal(DrawArea.Height / (YMarkList.Count - 1)));
            Brush br = new SolidColorBrush(ForeColor);
            double Scale = 1;
            if (YMax == 0)
            {
                YMax = YMarkList[YMarkList.Count - 1];
            }
            if (YMax != YMarkList[YMarkList.Count - 1])
            {
                Scale = YMarkList[YMarkList.Count - 1] / YMax;
            }

            for (int i = 0; i < YMarkList.Count; i++)
            {
                string s = YMarkList[i].ToString();
                Line l = new Line();
                l.Stroke = new SolidColorBrush(AxisColor);
                l.StrokeThickness = 2;

                l.X1 = (double)DrawArea.GetValue(Canvas.LeftProperty);
                l.X2 = l.X1 + 10;
                l.Y1 = DrawArea.Height - i * step * Scale + (double)DrawArea.GetValue(Canvas.TopProperty);
                l.Y2 = l.Y1;
                DrawCanvas.Children.Add(l);
                //AddYLable(s, "");

                TextBlock tb = new TextBlock();
                tb.Foreground = new SolidColorBrush(TextColor);
                tb.FontStyle = FontStyle.Italic;
                tb.FontSize = 12;
                tb.FontWeight = FontWeights.Bold;
                tb.Text = s;
                DrawCanvas.Children.Add(tb);
                tb.SetValue(Canvas.LeftProperty, l.X1 + 7);
                tb.SetValue(Canvas.TopProperty, l.Y1);

            }
        }

        public void DrawTitle(string Title)
        {

            TextBlock tb = new TextBlock();
            tb.Foreground = new SolidColorBrush(TextColor);
            tb.FontSize = 12;
            //tb.FontWeight = FontWeights.Bold;
            tb.Text = Title + "(" + DateTime.Now.ToString() + ")";
            DrawCanvas.Children.Add(tb);
            tb.SetValue(Canvas.LeftProperty, (double)DrawArea.GetValue(Canvas.LeftProperty) + DrawArea.Width / 2 - 5);
            tb.SetValue(Canvas.TopProperty, (double)DrawArea.GetValue(Canvas.TopProperty) + 5);

        }

        public void DrawBrightLines(List<int> al)//依据一个亮度曲线，而且是充满画布的方式填充
        {
            if (al.Count < 2) { return; }//最少需要两个点才可以连线
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }
            double w, h;
            if (DrawCanvas.ActualHeight != 0 && DrawCanvas.ActualHeight != Double.NaN)
            {
                h = DrawCanvas.ActualHeight;
            }
            else
            {
                h = DrawCanvas.Height;
            }
            if (DrawCanvas.ActualWidth != 0 && DrawCanvas.ActualWidth != Double.NaN)
            {
                w = DrawCanvas.ActualWidth;
            }
            else
            {
                w = DrawCanvas.Width;
            }
            Brush b = new SolidColorBrush(ForeColor);
            Point[] ps = new Point[al.Count];
            Path p = new Path();
            p.StrokeThickness = 1;
            p.Stroke = new SolidColorBrush(ForeColor);
            PathGeometry pg = new PathGeometry();
            p.Data = pg;
            PathFigure pf = new PathFigure();
            pg.Figures.Add(pf);
            pf.StartPoint = new Point(0d, h * (1 - (255d - al[0]) / 255d));
            double step = w / (al.Count - 1);
            for (int i = 1; i < al.Count; i++)
            {
                LineSegment ls = new LineSegment();
                Point tp;
                double x, y;
                y = (255d - al[i]) / 255d;
                y = h * (1 - y);
                x = i * step;
                tp = new Point(x, y);
                ls.Point = tp;
                pf.Segments.Add(ls);

            }

            DrawCanvas.Children.Add(p);
        }

        public void InitYMark(List<double> al)
        {
            if (al.Count == 0) { return; }
            YMax = YMin = al[0];
            if (YMax < al[0])
            {
                YMax = al[0];
            }
            if (YMin > al[0])
            {
                YMin = al[0];
            }
            foreach (double d in al)
            {
                if (YMax < d)
                {
                    YMax = d;
                }
                if (YMin > d)
                {
                    YMin = d;
                }
            }
        }

        public void DrawLines(List<double> al, double yMin, double yMax)//依据一个列表，绘制曲线
        {
            YMax = yMax;
            YMin = yMin;
            if (al.Count < 2) { return; }//最少需要两个点才可以连线
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }

            Point[] ps = new Point[al.Count];
            Path p = new Path();
            p.StrokeThickness = 1;
            p.Stroke = new SolidColorBrush(ForeColor);
            PathGeometry pg = new PathGeometry();
            p.Data = pg;
            PathFigure pf = new PathFigure();
            pg.Figures.Add(pf);
            pf.StartPoint = new Point((double)DrawArea.GetValue(Canvas.LeftProperty), DrawArea.Height - (al[0] - YMin) / (YMax - YMin) * DrawArea.Height + (double)DrawArea.GetValue(Canvas.TopProperty));
            double step = 1.0 / (al.Count - 1);
            for (int i = 1; i < al.Count; i++)
            {
                LineSegment ls = new LineSegment();
                Point tp;
                double x, y;
                y = al[i];
                y = (y - YMin) / (YMax - YMin) * DrawArea.Height;
                y = DrawArea.Height - y;
                x = i * step * DrawArea.Width;
                tp = new Point(x + (double)DrawArea.GetValue(Canvas.LeftProperty), y + (double)DrawArea.GetValue(Canvas.TopProperty));
                ls.Point = tp;
                pf.Segments.Add(ls);

            }

            DrawCanvas.Children.Add(p);
        }

        public void DrawCurve(List<double> al, double yMin, double yMax)//依据一个列表，绘制曲线，给出绝对的最大最小值，适合绘制0-255的亮度
        {
            YMax = yMax;
            YMin = yMin;
            if (al.Count < 2) { return; }//最少需要两个点才可以连线
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }
            Brush b = new SolidColorBrush(ForeColor);
            Point[] ps = new Point[al.Count];
            Path p = new Path();
            p.StrokeThickness = 1;
            p.Stroke = new SolidColorBrush(ForeColor);
            PathGeometry pg = new PathGeometry();
            p.Data = pg;
            PathFigure pf = new PathFigure();
            pg.Figures.Add(pf);
            pf.StartPoint = new Point(DrawArea.Height - (al[0] - YMin) / (YMax-YMin) * DrawArea.Height + (double)DrawArea.GetValue(Canvas.TopProperty), (double)DrawArea.GetValue(Canvas.LeftProperty));
            double step = 1 / (al.Count - 1);
            PolyBezierSegment ls = new PolyBezierSegment();
            pf.Segments.Add(ls);
            for (int i = 1; i < al.Count; i++)
            {
                Point tp;
                double x, y;
                y = al[i];
                y = (y - YMin) / (YMax - YMin) * DrawArea.Height;
                y = DrawArea.Height - y;
                x = i * step * DrawArea.Width;
                tp = new Point(x + (double)DrawArea.GetValue(Canvas.LeftProperty), y + (double)DrawArea.GetValue(Canvas.TopProperty));
                ls.Points.Add(tp);
            }

            DrawCanvas.Children.Add(p);
        }

        public void DrawLines(List<double> al)//依据一个列表，绘制曲线
        {
            InitYMark(al);
            if (al.Count < 2) { return; }//最少需要两个点才可以连线
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }

            Point[] ps = new Point[al.Count];
            Path p = new Path();
            p.StrokeThickness = 1;
            p.Stroke = new SolidColorBrush(ForeColor);
            PathGeometry pg = new PathGeometry();
            p.Data = pg;
            PathFigure pf = new PathFigure();
            pg.Figures.Add(pf);
            pf.StartPoint = new Point((double)DrawArea.GetValue(Canvas.LeftProperty), DrawArea.Height - (al[0] - YMin) / (YMax - YMin) * DrawArea.Height + (double)DrawArea.GetValue(Canvas.TopProperty));
            double step = 1.0 / (al.Count - 1);
            for (int i = 1; i < al.Count; i++)
            {
                LineSegment ls = new LineSegment();
                Point tp;
                double x, y;
                y = al[i];
                y = (y - YMin) / (YMax - YMin) * DrawArea.Height;
                y = DrawArea.Height - y;
                x = i * step * DrawArea.Width;
                tp = new Point(x + (double)DrawArea.GetValue(Canvas.LeftProperty), y + (double)DrawArea.GetValue(Canvas.TopProperty));
                ls.Point = tp;
                pf.Segments.Add(ls);

            }

            DrawCanvas.Children.Add(p);
        }

        public void DrawCurve(List<double> al)//依据一个列表，绘制曲线
        {
            InitYMark(al);
            if (al.Count < 2) { return; }//最少需要两个点才可以连线
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }
            Brush b = new SolidColorBrush(ForeColor);
            Point[] ps = new Point[al.Count];
            Path p = new Path();
            p.StrokeThickness = 1;
            p.Stroke = new SolidColorBrush(ForeColor);
            PathGeometry pg = new PathGeometry();
            p.Data = pg;
            PathFigure pf = new PathFigure();
            pg.Figures.Add(pf);
            pf.StartPoint = new Point(Canvas.GetLeft(DrawArea),DrawArea.Height - (al[0] - YMin) / (YMax - YMin) * DrawArea.Height + (double)DrawArea.GetValue(Canvas.TopProperty));
            double step = 1d / (al.Count - 1);
            PolyBezierSegment ls = new PolyBezierSegment();
            pf.Segments.Add(ls);
            for (int i = 1; i < al.Count; i++)
            {
                Point tp;
                double x, y;
                y = al[i];
                y = (y - YMin) / (YMax - YMin) * DrawArea.Height;
                y = DrawArea.Height - y;
                x = i * step * DrawArea.Width;
                tp = new Point(x + (double)DrawArea.GetValue(Canvas.LeftProperty), y + (double)DrawArea.GetValue(Canvas.TopProperty));
                ls.Points.Add(tp);
            }

            DrawCanvas.Children.Add(p);
        }

        public void DrawColorCy(Point PCenter, int r, Color c, double sa)
        {
            Path p = new Path();
            PathGeometry pg = new PathGeometry();
            p.Data = pg;
            PathFigure pf = new PathFigure();
            pg.Figures.Add(pf);
            pf.StartPoint = PCenter;

            LineSegment la = new LineSegment();
            double s = sa / 180 * Math.PI;
            double sx = PCenter.X + Math.Cos(s) * r;
            double sy = PCenter.Y + Math.Sin(s) * r;
            Point SP = new Point(sx, sy);
            la.Point = SP;
            double s1 = (sa + 1) / 180 * Math.PI;
            double sx1 = PCenter.X + Math.Cos(s1) * r;
            double sy1 = PCenter.Y + Math.Sin(s1) * r;
            Point SP1 = new Point(sx1, sy1);
            LineSegment lb = new LineSegment();
            lb.Point = PCenter;
            ArcSegment a = new ArcSegment();
            a.Size = new Size(r, r);
            a.SweepDirection = SweepDirection.Clockwise;
            a.Point = SP1;

            pf.Segments.Add(la);
            pf.Segments.Add(a);
            pf.Segments.Add(lb);

            LinearGradientBrush rb = new LinearGradientBrush();
            //rb.Center=PCenter;
            rb.MappingMode = BrushMappingMode.Absolute;
            //rb.RadiusX = r;
            //rb.RadiusY = r;
            //rb.GradientOrigin = PCenter;
            //rb.Center = PCenter;
            //rb.Center=new Point(0,0);
            //rb.GradientOrigin=new Point(0,0);
            //rb.RadiusX=1;
            //rb.RadiusY=1;
            GradientStop gc = new GradientStop();
            gc.Color = Color.FromArgb(255, 255, 255, 255); ;
            gc.Offset = 0;
            GradientStop gc1 = new GradientStop();
            gc1.Color = c;
            gc1.Offset = 1;
            rb.GradientStops.Add(gc);
            rb.GradientStops.Add(gc1);

            p.Fill = rb;
            DrawCanvas.Children.Add(p);
        }

        public void DrawColorCy(Point PCenter, int r)//绘制以PCenter点为圆心，r为半径的色相圆，圆心是白色
        {
            //可以构成一个基本的坐标系统，在上面定位各种颜色的差值
            //GraphicsPath path = new GraphicsPath();
            for (double i = 0; i < 360; i = i + 0.5)
            {
                LColor lc = new LColor();
                lc.setColorByHSB(i, 1, 1);
                //Color TC = HSVToRGB(i, 100, 100);
                DrawColorCy(PCenter, r, lc.getColor(), i);
            }
        }
        //public Color HSVToRGB(double H, double S, double V)//HSV到RGB的色彩变幻
        ////H取值是0-360，S、V取值都是1到100，会自动处理
        //{
        //    Byte R, G, B;
        //    H = Convert.ToInt32(Convert.ToDecimal(H) / 360 * 255);
        //    S = Convert.ToInt32(Convert.ToDecimal(S) / 100 * 255);
        //    V = Convert.ToInt32(Convert.ToDecimal(V) / 100 * 255);

        //    if (S == 0)
        //    {
        //        R = 0;
        //        G = 0;
        //        B = 0;
        //    }

        //    double fractionalSector;
        //    double sectorNumber;
        //    double sectorPos;
        //    sectorPos = (H / 255 * 360) / 60;
        //    sectorNumber = Math.Floor(sectorPos);
        //    fractionalSector = sectorPos - sectorNumber;

        //    double p;
        //    double q;
        //    double t;

        //    double r = 0;
        //    double g = 0;
        //    double b = 0;
        //    double ss = S / 255;
        //    double vv = V / 255;


        //    p = vv * (1 - ss);
        //    q = vv * (1 - (ss * fractionalSector));
        //    t = vv * (1 - (ss * (1 - fractionalSector)));

        //    switch (Convert.ToInt32(sectorNumber))
        //    {
        //        case 0:

        //            r = vv;
        //            g = t;
        //            b = p;
        //            break;

        //        case 1:
        //            r = q;
        //            g = vv;
        //            b = p;
        //            break;
        //        case 2:

        //            r = p;
        //            g = vv;
        //            b = t;
        //            break;
        //        case 3:

        //            r = p;
        //            g = q;
        //            b = vv;
        //            break;
        //        case 4:

        //            r = t;
        //            g = p;
        //            b = vv;
        //            break;
        //        case 5:

        //            r = vv;
        //            g = p;
        //            b = q;
        //            break;
        //    }
        //    R = Convert.ToByte(r * 255);
        //    G = Convert.ToByte(g * 255);
        //    B = Convert.ToByte(b * 255);
        //    Color c = Color.FromArgb(255, R, G, B);
        //    return c;
        //}

        public void DrawColorCy(float percent)//percent指明这个色相环的半径距离边界是百分之多少，这里利用的是整个画布
        {
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }

            //Graphics g = Graphics.FromImage(DrawCanvas);
            int r = Convert.ToInt32(DrawCanvas.Height * percent / 2);//一般来说画布的高度窄，半径以此为准
            Point p = new Point(DrawCanvas.Width / 2, DrawCanvas.Height / 2);
            DrawColorCy(p, r);
            //g.Dispose();
        }

        public void DrawColorMoveHue(Color c0, Color c, int r)//依据2种颜色，在色相环的周围绘制色相的变化
        {
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }

            double s0 = PhotoTest.getHue(c0) / 180 * Math.PI;
            double s1 = PhotoTest.getHue(c) / 180 * Math.PI;
            int y0 = Convert.ToInt32(Math.Sin(s0) * r + DrawCanvas.Height / 2);
            int x0 = Convert.ToInt32(Math.Cos(s0) * r + DrawCanvas.Width / 2);
            int y1 = Convert.ToInt32(Math.Sin(s1) * r + DrawCanvas.Height / 2);
            int x1 = Convert.ToInt32(Math.Cos(s1) * r + DrawCanvas.Width / 2);
            if ((x0 == x1) && (y0 == y1))
            {

            }
            else
            {
                //Brush b = new LinearGradientBrush(ew Point(x0, y0), new Point(x1, y1), c0, c);
                DrawColorArrow(new Point(x0, y0), new Point(x1, y1), 3, c0, c);
            }

        }

        public void DrawColorLine(Color c0, Color c, int r)//依据2种颜色，在色相环的周围绘制色相的变化
        {
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }
            LColor lc0=new LColor(c0), lc=new LColor(c);

            double s0 = lc0.HSB_H / 180 * Math.PI;
            double s1 = lc.HSB_H / 180 * Math.PI;
            double y0 = Math.Sin(s0) * r + DrawCanvas.Height / 2;
            double x0 = Math.Cos(s0) * r + DrawCanvas.Width / 2;
            double y1 = Math.Sin(s1) * r + DrawCanvas.Height / 2;
            double x1 = Math.Cos(s1) * r + DrawCanvas.Width / 2;

            DrawColorArrow(new Point(x0, y0), new Point(x1, y1), 3, c0, c);

        }

        public void DrawColorArrow(Point SP, Point EP, double r, Color sc, Color ec)
        {
            double arc = -Math.Atan((EP.Y - SP.Y) / (EP.X - SP.X));
            Point[] dp = new Point[7];
            dp[0] = new Point(SP.X + Math.Cos(Math.PI / 6 + arc) * r, SP.Y - Math.Sin(Math.PI / 6 + arc) * r);
            dp[6] = new Point(SP.X + Math.Cos(-Math.PI / 6 + arc) * r, SP.Y - Math.Sin(-Math.PI / 6 + arc) * r);
            dp[1] = new Point(EP.X + Math.Cos(Math.PI / 2 + arc) * r, EP.Y - Math.Sin(Math.PI / 2 + arc) * r);
            dp[2] = new Point(EP.X + Math.Cos(Math.PI / 2 + arc) * (r + r), EP.Y - Math.Sin(Math.PI / 2 + arc) * (r + r));
            dp[3] = new Point(EP.X + Math.Cos(Math.PI + arc) * (r + r), EP.Y - Math.Sin(Math.PI + arc) * (r + r));
            dp[4] = new Point(EP.X + Math.Cos(-Math.PI / 2 + arc) * (r + r), EP.Y - Math.Sin(-Math.PI / 2 + arc) * (r + r));
            dp[5] = new Point(EP.X + Math.Cos(-Math.PI / 2 + arc) * r, EP.Y - Math.Sin(-Math.PI / 2 + arc) * r);

            Path p = new Path();
            PathGeometry pg = new PathGeometry();
            p.Data = pg;
            PathFigure pf = new PathFigure();
            pg.Figures.Add(pf);
            pf.StartPoint = dp[0];

            LineSegment l1 = new LineSegment();
            l1.Point = dp[1];
            LineSegment l2 = new LineSegment();
            l2.Point = dp[2];
            LineSegment l3 = new LineSegment();
            l3.Point = dp[3];
            LineSegment l4 = new LineSegment();
            l4.Point = dp[4];
            LineSegment l5 = new LineSegment();
            l5.Point = dp[5];
            LineSegment l6 = new LineSegment();
            l6.Point = dp[6];
            //LineSegment l7 = new LineSegment();
            //l7.Point=dp[0];
            ArcSegment a = new ArcSegment();
            a.Size = new Size(2 * r, 2 * r);
            a.SweepDirection = SweepDirection.Counterclockwise;
            a.IsLargeArc = true;
            a.Point = dp[0];

            pf.Segments.Add(l1);
            pf.Segments.Add(l2);
            pf.Segments.Add(l3);
            pf.Segments.Add(l4);
            pf.Segments.Add(l5);
            pf.Segments.Add(l6);
            //pf.Segments.Add(l7);
            pf.Segments.Add(a);

            LinearGradientBrush rb = new LinearGradientBrush();
            //rb.Center=PCenter;
            rb.MappingMode = BrushMappingMode.Absolute;
            rb.StartPoint = SP;
            rb.EndPoint = EP;
            //rb.GradientOrigin = PCenter;
            //rb.Center = PCenter;

            GradientStop gc = new GradientStop();
            gc.Color = sc;
            gc.Offset = 0;
            GradientStop gc1 = new GradientStop();
            gc1.Color = ec;
            gc1.Offset = 1;
            rb.GradientStops.Add(gc);
            rb.GradientStops.Add(gc1);

            p.Fill = rb;
            p.Stroke = new SolidColorBrush(ForeColor);
            p.StrokeThickness = 1;
            DrawCanvas.Children.Add(p);
        }

        public void DrawColorList(List<Color> al, float percent)
        {
            int r = Convert.ToInt32(DrawCanvas.Height * percent / 2);
            for (int i = 0; i < al.Count - 1; i++)
            {
                Color c0 = (Color)al[i];
                Color c1 = (Color)al[i + 1];
                DrawColorLine(c0, c1, r);

            }
        }

        public void DrawColorMoveHue(Color c0, Color c, float percent)//依据2种颜色，在色相环的周围绘制色相的变化
        {
            int r = Convert.ToInt32(DrawCanvas.Height * percent / 2);
            DrawColorMoveHue(c0, c, r);
        }

        public void DrawColorHueArea(Color c, float percent, double d, double Area)//依据特定颜色，绘制一个Area定义的角度区域
        {
            Area = Area / 180 * Math.PI;//转成弧度
            double cc = ColorManager.getHue(c);
            double cc0, cc1 = 0;
            cc0 = cc - Area;
            cc1 = cc + Area;
            double r0 = ColorManager.getSaturation(c) * percent * DrawCanvas.Height / 2;
            double y0 = Math.Sin(cc0) * r0 + DrawCanvas.Height / 2;
            double x0 = Math.Cos(cc0) * r0 + DrawCanvas.Width / 2;
            double y1 = Math.Sin(cc1) * r0 + DrawCanvas.Height / 2;
            double x1 = Math.Cos(cc1) * r0 + DrawCanvas.Width / 2;
            DrawLine(DrawCanvas.Width / 2, DrawCanvas.Height / 2, x0, y0, false, d, new SolidColorBrush(ForeColor));
            DrawLine(DrawCanvas.Width / 2, DrawCanvas.Height / 2, x1, y1, false, d, new SolidColorBrush(ForeColor));

        }

        public void DrawColorPoint(WriteableBitmap b, float per, double pr)
        {
            var components = b.PixelBuffer.ToArray();
            for (int i = 0; i < components.Length; i+=4)
            {
                Color c = Color.FromArgb(255, components[i+2], components[i+1], components[i]);
                DrawColorPoint(c, per, pr);
            }
        }

        public void DrawColorPoint(Color c, float percent, double pr)//在色相环上面绘制点,pr是点的半径
        {
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }

            Ellipse e = new Ellipse();

            double r0 = ColorManager.getSaturation(c) * percent * DrawCanvas.Height / 2;
            double y0 = Math.Sin(ColorManager.getHue(c) / 180 * Math.PI) * r0 + DrawCanvas.Height / 2;
            double x0 = Math.Cos(ColorManager.getHue(c) / 180 * Math.PI) * r0 + DrawCanvas.Width / 2;
            y0 = y0 - pr;
            x0 = x0 - pr;
            Brush b = new SolidColorBrush(ForeColor);
            e.Stroke = b;
            e.StrokeThickness = 2;
            e.Width = 2 * pr;
            e.Height = 2 * pr;
            DrawCanvas.Children.Add(e);
            e.SetValue(Canvas.LeftProperty, x0);
            e.SetValue(Canvas.TopProperty, y0);
        }

        public void DrawStepPoint(List<double> al)//绘制台阶状曲线图
        {
            if (al.Count == 0) { return; }
            if (DrawCanvas == null)
            {
                status = "Error Init Canvas";
                return;
            }


            if (al.Count == 0) { return; }
            Point[] ps = new Point[al.Count];

            double step = 1 / al.Count - 1;
            for (int i = 0; i < al.Count; i++)
            {
                Point tp;
                double x, y;
                y = al[i];
                y = y / YMax * DrawArea.Height;
                y = DrawArea.Height - y;
                x = i * step * DrawArea.Width;
                tp = new Point(Convert.ToInt32(x) + (double)DrawArea.GetValue(Canvas.LeftProperty), Convert.ToInt32(y) + (double)DrawArea.GetValue(Canvas.TopProperty));
                ps[i] = tp;
            }
            int tl = ps.Length;
            List<Point> IPoint = new List<Point>();
            for (int i = 0; i < tl - 1; i++)
            {
                Point ip0, ip1;
                Point p0, p1;
                p0 = ps[i];
                p1 = ps[i + 1];
                ip0 = new Point((p0.X + p1.X) / 2, p0.Y);
                ip1 = new Point((p0.X + p1.X) / 2, p1.Y);
                IPoint.Add(ps[i]);
                IPoint.Add(ip0);
                IPoint.Add(ip1);
            }
            IPoint.Add(ps[tl - 1]);
            Point[] StepPoints = new Point[IPoint.Count];
            Polyline pl = new Polyline();
            for (int i = 0; i < IPoint.Count; i++)
            {
                StepPoints[i] = (Point)IPoint[i];
                pl.Points.Add((Point)IPoint[i]);
            }
            //绘制线条
            pl.Stroke = new SolidColorBrush(ForeColor);
            pl.StrokeThickness = 3;
            DrawCanvas.Children.Add(pl);
        }

        public void DrawWave(int HalfWavelong, int TotleLong)//绘制余弦曲线图
        {
            List<double> al = new List<double>();
            for (int i = 0; i < TotleLong; i++)
            {
                int x = Convert.ToInt32(Math.Cos((Convert.ToDouble(i) / HalfWavelong) * Math.PI) * 127.5 + 127.5);
                al.Add(x);
            }
            DrawLines(al);
        }

        public void DrawBrightHistogram(List<double> al)//亮度柱状图
        {
            double Step = DrawArea.ActualWidth / al.Count;
            
            Rectangle r;
            for (int i = 0; i < al.Count; i++)
            {
                double b = al[i];
                r = new Rectangle();
                r.Width = Step;
                r.Height = b / 255 * DrawArea.Height;//把亮度折算为显示区的高度
                DrawCanvas.Children.Add(r);
                Canvas.SetLeft(r, Canvas.GetLeft(DrawArea) + i * Step);
                Canvas.SetTop(r, Canvas.GetTop(DrawArea) + DrawArea.Height - r.Height);
                r.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)b, (byte)b, (byte)b));
                r.Stroke = new SolidColorBrush(Colors.Red);
            }
        }

        public int getMax(List<int> il)
        {
            if (il.Count == 0) { return Int32.MinValue; }
            int max = il[0];
            foreach (int i in il)
            {
                if (max < i) { max = i; }
            }
            return max;
        }

        public void DrawPixPosition(List<PixInfor> pl, int PhotoW, int PhotoH)
        {
            foreach (PixInfor pi in pl)
            {
                Ellipse e = new Ellipse();
                e.Width = 5;
                e.Height = 5;
                e.Fill = new SolidColorBrush(ForeColor);
                DrawCanvas.Children.Add(e);
                Canvas.SetLeft(e, DrawCanvas.Width* pi.XPosition / PhotoW );
                Canvas.SetTop(e, DrawCanvas.Height* pi.YPosition / PhotoH );
            }
        }

        public List<int> AddBrightPixNum(List<int> al, List<int> bl)
        {
            List<int> cl = new List<int>();
            if (al.Count != 256) { return null; }
            if (bl.Count != 256) { return null; }
            for (int i = 0; i < 256; i++)
            {
                cl.Add(al[i] + bl[i]);
            }
            return cl;
        }

        public void DrawBrightPixNumHistogram(List<int> al)//绘制柱状图表示不同亮度的像素数量
        {
            double Step = DrawArea.ActualWidth / al.Count;
            int max = getMax(al);
            if (max == 0) { max = 1; }
            double PixPerBright = DrawArea.ActualHeight / max;
            Rectangle r;
            int t = al.Count;

            for (int i = 0; i < 256; i++)
            {
                if (i > 255) { i = 255; }
                r = new Rectangle();
                r.Width = Step;
                r.Height = al[i] * PixPerBright;
                DrawCanvas.Children.Add(r);
                Canvas.SetLeft(r, i * Step);
                Canvas.SetTop(r, DrawArea.ActualHeight - al[i] * PixPerBright);
                r.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)i, (byte)i, (byte)i));
                r.Stroke = new SolidColorBrush(Colors.Red);
            }
        }

        public void DrawBrightPixNumHistogram(List<int> al, int startBright, int BrightStep)//绘制柱状图表示不同亮度的像素数量
        {
            double Step = DrawArea.ActualWidth / al.Count;
            Rectangle r;
            int t = al.Count * BrightStep;

            for (int i = startBright; i < t + startBright; i = i + BrightStep)
            {
                if (i > 255) { i = 255; }
                r = new Rectangle();
                r.Width = Step;
                r.Height = al[i];
                DrawCanvas.Children.Add(r);
                Canvas.SetLeft(r, Canvas.GetLeft(DrawArea) + i * Step);
                Canvas.SetTop(r, Canvas.GetTop(DrawArea) + DrawArea.ActualHeight - al[i]);
                r.Fill = new SolidColorBrush(Color.FromArgb(255, (byte)i, (byte)i, (byte)i));
                r.Stroke = new SolidColorBrush(Colors.Red);
            }
        }

        public static Point getImagePosition(Point p, Image image)//p是相对于画布来说的，转换为相对于图片的像素位置
        {
            WriteableBitmap img = image.Source as WriteableBitmap;
            if (img == null) { return new Point(double.NaN, double.NaN); }
            double x, y;
            x = p.X;
            y = p.Y;

            if (x < 0 || x > image.Width) { x = -1; }
            else
            {
                x = x / image.Width * img.PixelWidth;
            }
            if (y < 0 || y > image.Height) { y = -1; }
            else
            {
                y = y / image.Height * img.PixelHeight;
            }
            return new Point(x, y);
        }

        public static double getImageDistance(double l, Image image)//p是相对于画布来说的，转换为相对于图片的像素位置
        {
            WriteableBitmap img = image.Source as WriteableBitmap;
            if (img == null) { return double.NaN; }


            return l / image.Width * img.PixelWidth;
        }

        public static WriteableBitmap getImageArea(Rect r, Image image)//r是相对于画布来说的，获取对应的区域位图
        {
            WriteableBitmap img = image.Source as WriteableBitmap;
            if (img == null) { return null; }
            int x, y, w, h;//位图里面的实际像素

            if (r.X > image.Width) { return null; }
            else
            {
                x = Convert.ToInt32(r.X / image.Width * img.PixelWidth);
            }
            if (r.Y > image.Height) { return null; }
            else
            {
                y = Convert.ToInt32(r.Y / image.Height * img.PixelHeight);
            }
            if (r.X + r.Width > image.Width)
            {
                w = Convert.ToInt32((image.Width - r.X) / image.Width * img.PixelWidth);
            }
            else
            {
                w = Convert.ToInt32((r.Width - r.X) / image.Width * img.PixelWidth);
            }
            if (r.Y + r.Height > image.Height)
            {
                h = Convert.ToInt32((image.Height - r.Y) / image.Height * img.PixelHeight);
            }
            else
            {
                h = Convert.ToInt32((r.Height - r.Y) / image.Height * img.PixelHeight);
            }
            PhotoTest pt = new PhotoTest();
            return pt.getImageArea(img, x, y, w, h);
        }

        public void DrawEllipse(double x0, double y0, double x1, double y1, bool IsFill, double w, Brush bs, Brush bf)
        {
            Ellipse r = new Ellipse();
            double l, t;
            if (x1 >= x0)
            {
                l = x0;
                r.Width = x1 - x0;
            }
            else
            {
                l = x1;
                r.Width = x0 - x1;
            }
            if (y1 >= y0)
            {
                t = y0;
                r.Height = y1 - y0;
            }
            else
            {
                t = y1;
                r.Height = y0 - y1;
            }
            r.StrokeThickness = w;
            r.Stroke = bs;
            if (IsFill) { r.Fill = bf; }
            DrawCanvas.Children.Add(r);
            Canvas.SetLeft(r, l);
            Canvas.SetTop(r, t);
        }

        public void DrawText(double x0, double y0, double x1, double y1, string s, Brush bs)
        {
            TextBox r = new TextBox();
            double l, t;
            if (x1 >= x0)
            {
                l = x0;
                r.Width = x1 - x0;
            }
            else
            {
                l = x1;
                r.Width = x0 - x1;
            }
            if (y1 >= y0)
            {
                t = y0;
                r.Height = y1 - y0;
            }
            else
            {
                t = y1;
                r.Height = y0 - y1;
            }
            r.BorderBrush = null;
            r.Background = bs;
            DrawCanvas.Children.Add(r);
            Canvas.SetLeft(r, l);
            Canvas.SetTop(r, t);
        }

        public void DrawText(double x0, double y0, double x1, double y1, string s, Brush bs, TextFont tf)
        {
            TextBox r = new TextBox();
            r.FontFamily = tf.TextFontFamily;
            r.FontSize = tf.Size;
            r.FontStretch = tf.TextFontStretch;
            r.FontStyle = tf.TextFontStyle;
            r.FontWeight = tf.TextFontWeight;
            r.TextAlignment = tf.Alignment;
            double l, t;
            if (x1 >= x0)
            {
                l = x0;
                r.Width = x1 - x0;
            }
            else
            {
                l = x1;
                r.Width = x0 - x1;
            }
            if (y1 >= y0)
            {
                t = y0;
                r.Height = y1 - y0;
            }
            else
            {
                t = y1;
                r.Height = y0 - y1;
            }
            r.BorderBrush = null;
            r.Background = bs;
            DrawCanvas.Children.Add(r);
            Canvas.SetLeft(r, l);
            Canvas.SetTop(r, t);
        }

        public TextBox DrawEditText(double x0, double y0, double x1, double y1, string s, Brush bs, TextFont tf)
        {
            TextBox r = new TextBox();
            r.FontFamily = tf.TextFontFamily;
            r.FontSize = tf.Size;
            r.FontStretch = tf.TextFontStretch;
            r.FontStyle = tf.TextFontStyle;
            r.FontWeight = tf.TextFontWeight;
            r.TextAlignment = tf.Alignment;
            double l, t;
            if (x1 >= x0)
            {
                l = x0;
                r.Width = x1 - x0;
            }
            else
            {
                l = x1;
                r.Width = x0 - x1;
            }
            if (y1 >= y0)
            {
                t = y0;
                r.Height = y1 - y0;
            }
            else
            {
                t = y1;
                r.Height = y0 - y1;
            }
            r.BorderBrush = null;
            r.Background = bs;
            DrawCanvas.Children.Add(r);
            Canvas.SetLeft(r, l);
            Canvas.SetTop(r, t);
            return r;
        }

        public void DrawRectangle(double x0, double y0, double x1, double y1, bool IsFill, double w, Brush bs, Brush bf)
        {
            Rectangle r = new Rectangle();
            double l, t;
            if (x1 >= x0)
            {
                l = x0;
                r.Width = x1 - x0;
            }
            else
            {
                l = x1;
                r.Width = x0 - x1;
            }
            if (y1 >= y0)
            {
                t = y0;
                r.Height = y1 - y0;
            }
            else
            {
                t = y1;
                r.Height = y0 - y1;
            }
            r.StrokeThickness = w;
            r.Stroke = bs;
            if (IsFill) { r.Fill = bf; }
            DrawCanvas.Children.Add(r);
            Canvas.SetLeft(r, l);
            Canvas.SetTop(r, t);
        }

        public EllipseGeometry getEllipseGeometry(double x0, double y0, double x1, double y1)
        {

            EllipseGeometry r = new EllipseGeometry();
            double l, t;
            if (x1 >= x0)
            {
                l = x0;
                r.RadiusX = (x1 - x0) * 2;
            }
            else
            {
                l = x1;
                r.RadiusX = (x0 - x1) * 2;
            }
            if (y1 >= y0)
            {
                t = y0;
                r.RadiusY = (y1 - y0) * 2;
            }
            else
            {
                t = y1;
                r.RadiusY = (y0 - y1) * 2;
            }
            r.Center = new Point(l + r.RadiusX, t + r.RadiusY);
            return r;
        }
        public RectangleGeometry getRectangleGeometry(double x0, double y0, double x1, double y1)
        {

            RectangleGeometry r = new RectangleGeometry();
            Rect re = new Rect(new Point(x0, y0), new Point(x1, y1));
            r.Rect = re;
            return r;
        }

        public void DrawPointColorTimeLine(MediaElement me, Point p)
        {
            double lp, tp;
            lp = p.X / me.Width;
            tp = p.Y / me.Height;
        }

        public void DrawPointPositionBright(List<PixInfor> pl, Canvas cp, Canvas cb)
        {//两个画布一个显示位置，一个显示亮度变化
            //sb.Stop();
            //sb.Children.Clear();
            if (pl.Count == 0) { return; }
            //double yper = pl[0].PhotoH / cp.Height;
            //double xper = pl[0].PhotoW / cp.Width;
            Path p = new Path();
            p.Stroke = new SolidColorBrush(ForeColor);
            p.StrokeThickness = 3;
            PathGeometry g = new PathGeometry();
            g.FillRule = FillRule.EvenOdd;
            g.Figures.Clear();
            PathFigure pf = new PathFigure();
            g.Figures.Add(pf);
            p.Data = g;
            double bs = cb.Width / pl.Count;
            double bv = cb.Height / 256;
            PhotoTest pt = new PhotoTest();
            pf.StartPoint = new Point(0, cb.Height-pt.getBright(pl[0].colorValue) * bv);

            //Ellipse pe = new Ellipse();
            //pe.Fill = new SolidColorBrush(Colors.Blue);
            //pe.Width = 3;
            //pe.Height = 3;
            //double sx = pl[0].XPosition / pl[0].PhotoW * cp.Width;
            //double sy = pl[0].YPosition / pl[0].PhotoH * cp.Height;
            //Canvas.SetLeft(pe,sx-1);
            //Canvas.SetTop(pe,sy-1);
            //TranslateTransform tt=new TranslateTransform();
            //pe.RenderTransform=tt;
            //PointAnimationUsingKeyFrames da = new PointAnimationUsingKeyFrames();
            //EllipseGeometry eg = new EllipseGeometry();
            //eg.RadiusX = 3;
            //eg.RadiusY = 3;
            //Path ap = new Path();
            //ap.Data = eg;
            //ap.Fill = new SolidColorBrush(Colors.Blue);
            //eg.Center = new Point(sx, sy);
            //Storyboard.SetTarget(da, eg);
            //Storyboard.SetTargetProperty(da, new PropertyPath(EllipseGeometry.CenterProperty));
            //cp.Children.Add(ap);
            //sb.Children.Add(da);
            //da.Duration = sb.Duration;

            double minx,maxx,miny,maxy;
            minx = maxx = pl[0].XPosition;
            miny = maxy = pl[0].YPosition;
            for (int i = 0; i < pl.Count; i++)
            {
                PixInfor pi = pl[i];
                minx = Math.Min(minx, pl[i].XPosition);
                maxx = Math.Max(maxx, pl[i].XPosition);
                miny = Math.Min(miny, pl[i].YPosition);
                maxy = Math.Max(maxy, pl[i].YPosition);
                //ax = ax + pl[i].XPosition;
                //ay = ay + pl[i].YPosition;
                double tx, ty;
                tx = Convert.ToDouble(pi.XPosition) / pi.PhotoW * cp.Width;
                ty = Convert.ToDouble(pi.YPosition) / pi.PhotoH * cp.Height;
                Ellipse e = new Ellipse();
                e.Fill = new SolidColorBrush(ForeColor);
                e.Width = 3;
                e.Height = 3;
                Canvas.SetLeft(e, tx - 1);
                Canvas.SetTop(e, ty - 1);
                cp.Children.Add(e);
                if (i > 0)
                {
                    //double b = ColorManager.getBrightness(pi.colorValue);
                    LineSegment ls = new LineSegment();
                    ls.Point = new Point(i * bs, cb.Height - pt.getBright(pi.colorValue) * bv);
                    pf.Segments.Add(ls);
                }

                //PointKeyFrame kf = new LinearPointKeyFrame();
                //da.KeyFrames.Add(kf);
                //kf.KeyTime = pi.CurrentTimeLong;
                //kf.Value = new Point(tx, ty);
            }
            cb.Children.Add(p);

            double ax = (maxx+minx) / 2;
            double ay = (maxy+miny) / 2;

            double dr = 0;
            foreach (PixInfor xp in pl)
            {
                dr = Math.Max(dr, Math.Sqrt((xp.XPosition - ax) * (xp.XPosition - ax) + (xp.YPosition - ay) * (xp.YPosition - ay)));
            }
            
            Ellipse oe = new Ellipse();
            oe.Height = oe.Width = Convert.ToDouble(dr * 2) / pl[0].PhotoW * cp.Width; ;
            Canvas.SetLeft(oe, Convert.ToDouble(ax-dr) / pl[0].PhotoW * cp.Width);
            Canvas.SetTop(oe, Convert.ToDouble(ay - dr) / pl[0].PhotoH * cp.Height);
            oe.Stroke = new SolidColorBrush(Colors.Blue);
            oe.StrokeThickness = 2;
            cp.Children.Add(oe);
            ToolTipService.SetToolTip(oe, "弥散圆半径："+dr.ToString()+"像素");
        }

        public void DrawBrightCurve(Image Img, int d)//绘制等亮度曲线，Img被包含在一个canvas里面
        //d是表示绘制级差的参数，默认亮度等级差是20%
        {
            WriteableBitmap b = Img.Source as WriteableBitmap;
            if (b == null) { return; }
            Canvas c = Img.Parent as Canvas;
            if (c == null) { return; }
            Path p = new Path();
            PathGeometry pg = new PathGeometry();
            p.Data = pg;
            double MaxB = 0, MinB = 255;
            for (int i = 0; i < b.PixelBuffer.ToArray().Length; i++)
            {
                byte[] components = new byte[4];
                components = BitConverter.GetBytes(b.PixelBuffer.ToArray()[i]);
                double x = (.299 * components[2] + .587 * components[1] + .114 * components[0]);
                if (x > MaxB) { MaxB = x; }
                if (x < MinB) { MinB = x; }

            }
            int bh = Convert.ToInt32(MaxB - MinB);
            bh = bh * d / 100;

            List<int> BrightList = new List<int>();

            Dictionary<int, Dictionary<int, List<int>>> vl = new Dictionary<int, Dictionary<int, List<int>>>();
            for (int i = 0; i < d; i++)
            {
                Dictionary<int, List<int>> pl = new Dictionary<int, List<int>>();
                int tb = Convert.ToInt32(i * bh + MinB);
                vl.Add(tb, pl);
                BrightList.Add(tb);
                PathFigure pf = new PathFigure();
                pg.Figures.Add(pf);
            }

            for (int i = 0; i < b.PixelHeight; i++)
            {
                for (int j = 0; j < b.PixelWidth; j++)
                {
                    byte[] components = BitConverter.GetBytes(b.PixelBuffer.ToArray()[j + i * b.PixelWidth]);
                    Color cc = new Color();
                    cc.B = components[0];
                    cc.G = components[1];
                    cc.R = components[2];
                    int bb = Convert.ToInt32(.299 * components[2] + .587 * components[1] + .114 * components[0]);
                    if (vl.ContainsKey(bb))
                    {
                        List<int> lpl = new List<int>();
                        lpl.Add(j);
                        Dictionary<int, List<int>> tvl = new Dictionary<int, List<int>>();
                        tvl.Add(i, lpl);

                        vl[bb] = (tvl);
                    }
                }
            }
            foreach (KeyValuePair<int, Dictionary<int, List<int>>> tv in vl)
            {

                for (int i = 0; i < b.PixelHeight; i++)
                {
                    if (tv.Value.ContainsKey(i))
                    {
                        List<int> tpl = tv.Value[i];
                        if (tpl.Count > 2)
                        {
                            for (int x = 1; x < tpl.Count - 1; x++)
                            {
                                tpl.RemoveAt(x);//只保留头尾
                            }
                        }
                        if (tpl.Count == 1)
                        {
                            tpl.Add(tpl[0]);//保证每一行都是两个点
                        }
                    }

                }
            }

            //已经处理完数据，下面画图
            int ind = 0;
            foreach (int tb in BrightList)
            {
                List<Point> lpl = new List<Point>();
                List<Point> rpl = new List<Point>();
                List<int> kl = new List<int>(vl[tb].Keys);
                Dictionary<int, List<int>> cl = vl[tb];
                foreach (int k in kl)
                {
                    lpl.Add(new Point(k, cl[k][0]));
                    lpl.Add(new Point(k, cl[k][1]));
                }

                PathFigure pf = pg.Figures[ind];
                pf.StartPoint = lpl[0];
                foreach (Point lp in lpl)
                {
                    LineSegment ps = new LineSegment();
                    ps.Point = lp;
                    pf.Segments.Add(ps);
                }
                rpl.Reverse();
                foreach (Point lp in rpl)
                {
                    LineSegment ps = new LineSegment();
                    ps.Point = lp;
                    pf.Segments.Add(ps);
                }
                ind++;
            }

            p.StrokeThickness = 3;
            p.Stroke = new SolidColorBrush(ForeColor);
            c.Children.Add(p);
        }

        public async Task<List<WriteableBitmap>> VideoCapture(MediaElement me, TimeSpan BeginTime, TimeSpan EndTime, int CapturePhotoNumPerSec)
        {
            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            me.Position = BeginTime;
            TimeSpan dt = EndTime - BeginTime;
            if (dt.TotalSeconds == 0)
            {
                dt = me.NaturalDuration.TimeSpan;
            }
            if (CapturePhotoNumPerSec <= 0)
            {
                CapturePhotoNumPerSec = Convert.ToInt32(me.DefaultPlaybackRate);
            }
            int TotleFrame = Convert.ToInt32(dt.TotalSeconds * CapturePhotoNumPerSec);
            TimeSpan pt = TimeSpan.FromSeconds(1d / CapturePhotoNumPerSec);
            for (int i = 0; i < TotleFrame; i++)
            {
                //WriteableBitmap b = await WriteableBitmapHelper.Snapshot(me);
                WriteableBitmap b = new WriteableBitmap((int)me.ActualWidth,(int)me.ActualHeight);
                bl.Add(b);
                me.Position = me.Position + pt;
            }
            return bl;
        }

        public async void DrawVideoLineBright(Storyboard sb, MediaElement me, TimeSpan BeginTime, TimeSpan EndTime, int CapturePhotoNumPerSec, Point sp, Point ep, Canvas tarCanvas, ColorMode cm)
        {
            List<WriteableBitmap> cbl =await VideoCapture(me, BeginTime, EndTime, CapturePhotoNumPerSec);
            DrawVideoLineBright(sb, 1000d / CapturePhotoNumPerSec, cbl, sp, ep, cm);

        }
        public async void DrawPointColorCurve(Storyboard sb, MediaElement me, TimeSpan BeginTime, TimeSpan EndTime, int CapturePhotoNumPerSec, Point p, Canvas tarCanvas, ColorMode cm)
        {
            List<WriteableBitmap> cbl =await VideoCapture(me, BeginTime, EndTime, CapturePhotoNumPerSec);
            DrawPointColorCurve(sb, 1000d / CapturePhotoNumPerSec, cbl, p, cm);
        }
        public async void DrawAreaColorCurve(Storyboard sb, MediaElement me, TimeSpan BeginTime, TimeSpan EndTime, int CapturePhotoNumPerSec, double l, double t, double w, double h, Canvas tarCanvas, ColorMode cm)
        {
            List<WriteableBitmap> cbl =await VideoCapture(me, BeginTime, EndTime, CapturePhotoNumPerSec);
            DrawAreaColorCurve(sb, 1000d / CapturePhotoNumPerSec, cbl,l,t,w,h, cm);
        }

        void setForeColor(ColorMode cm)//依据颜色模式设置
        {
            if (cm == ColorMode.Bright)
            {
                ForeColor = Colors.Black;
            }
            if (cm == ColorMode.R)
            {
                ForeColor = Colors.Red;
            }
            if (cm == ColorMode.G)
            {
                ForeColor = Colors.Green;
            }
            if (cm == ColorMode.B)
            {
                ForeColor = Colors.Blue;
            }
        }

        List<double> getBright(List<Color> cl, ColorMode cm)//依照颜色模式，把某个颜色列表转换为对应的亮度列表
        {
             List<double> tbl = new List<double>();
                foreach (Color tc in cl)
                {
                    double cd = 0;
                    if (cm == ColorMode.Bright)
                    {
                        cd = (.299 * tc.R + .587 * tc.G + .114 * tc.B);
                    }
                    if (cm == ColorMode.R)
                    {
                        cd = tc.R;
                    }
                    if (cm == ColorMode.G)
                    {
                        cd = tc.G;
                    }
                    if (cm == ColorMode.B)
                    {
                        cd = tc.B;
                    }
                    tbl.Add(cd);
                }
            return tbl;
        }
        List<List<double>> getBrightList(List<List<Color>> cl, ColorMode cm)//转换颜色列表集到亮度列表集
        {
            List<List<double>> bl = new List<List<double>>();
            foreach (List<Color> c in cl)
            {
                bl.Add(getBright(c,cm));
            }
            return bl;
        }
        double getX(int i,int totle)//依据点的顺序找到x位置
        {
            if (totle == 0) { return 0; }
            return DrawCanvas.Width * i / totle;
        }

        double getY(double y)//依据值找到y的实际位置
        {
            return DrawCanvas.Height - y / (YMax - YMin) * DrawCanvas.Height;
        }
        
        public void DrawVideoLineBright(Storyboard sb,double CaptureTimeLong, List<WriteableBitmap> CaptureImageList, Point sp, Point ep, ColorMode cm)
        {//在固定的截图集合里面，对应线的亮度变化动画
            sb.Stop();
            sb.Children.Clear();
            setForeColor(cm);

            List<List<Color>> cl = new List<List<Color>>();
            PhotoTestParameter ptp = new PhotoTestParameter();

            for (int i = 0; i < CaptureImageList.Count; i++)
            {
                WriteableBitmap b = CaptureImageList[i];
                List<Color> pcl = ptp.getLine(b, sp, ep);
                cl.Add(pcl);
            }

            List<List<double>> bl = getBrightList(cl,cm);//颜色列表转换为亮度列表

            //初始化第一根亮度线
            Path p = new Path();
            p.StrokeThickness = 1;
            p.Stroke = new SolidColorBrush(ForeColor);
            PathGeometry pg = new PathGeometry();
            p.Data = pg;
            PathFigure pf = new PathFigure();
            pg.Figures.Add(pf);
            pf.StartPoint = new Point(0,getY(bl[0][0]));
            //double step = 1.0 / (bl[0].Count - 1);

            List<double> al = bl[0];
            for (int i = 0; i < al.Count; i++)
            {
                LineSegment ls = new LineSegment();
                Point tp;
                tp = new Point(getX(i,al.Count), getY(al[i]));
                ls.Point = tp;
                pf.Segments.Add(ls);

                PointAnimationUsingKeyFrames dp = new PointAnimationUsingKeyFrames();
                dp.Duration = new Duration(TimeSpan.FromMilliseconds(CaptureTimeLong));
                sb.Children.Add(dp);
                Storyboard.SetTarget(dp, ls);
                Storyboard.SetTargetProperty(dp, "LineSegment.PointProperty");
            }

            //依据后面的数据给亮度线点加动画
            for (int i = 1; i < bl.Count; i++)
            {
                for (int j = 0; j < bl[i].Count; j++)
                {
                    PointKeyFrame kf = new LinearPointKeyFrame();
                    kf.Value = new Point(getX(j,bl[i].Count), getY(bl[i][j]));
                    PointAnimationUsingKeyFrames dp = sb.Children[j] as PointAnimationUsingKeyFrames;
                    kf.KeyTime = TimeSpan.FromMilliseconds(CaptureTimeLong / bl.Count);
                    dp.KeyFrames.Add(kf);
                }
            }

            DrawCanvas.Children.Add(p);//增加亮度线
            sb.AutoReverse = true;
            sb.Duration = new Duration(TimeSpan.FromMilliseconds(CaptureTimeLong));
        }
        public void DrawPointColorCurve(Storyboard sb, double CaptureTimeLong, List<WriteableBitmap> CaptureImageList, Point p, ColorMode cm)
        {
            sb.Stop();
            sb.Children.Clear();
            setForeColor(cm);
            if ((p.X < 0) || (p.Y < 0)) { return; }

            List<Color> cl = new List<Color>();
            PhotoTestParameter ptp = new PhotoTestParameter();

            for (int i = 0; i < CaptureImageList.Count; i++)
            {
                WriteableBitmap b = CaptureImageList[i];
                int tc = b.PixelBuffer.ToArray()[Convert.ToInt32(b.PixelWidth * p.Y + p.X)];
                byte[] components = new byte[4];
                components = BitConverter.GetBytes(tc);

                Color c = new Color();
                c.R = components[2];
                c.G = components[1];
                c.B = components[0];

                cl.Add(c);

            }

            List<double> bl = getBright(cl, cm);
            DrawLines(bl,0,255);
        }
        public void DrawAreaColorCurve(Storyboard sb, double CaptureTimeLong, List<WriteableBitmap> CaptureImageList, double l, double t, double w, double h, ColorMode cm)
        {
            sb.Stop();
            sb.Children.Clear();

            List<Color> cl = new List<Color>();
            PhotoTestParameter ptp = new PhotoTestParameter();

            for (int i = 0; i < CaptureImageList.Count; i++)
            {
                WriteableBitmap b = CaptureImageList[i];
                b = ptp.getImageArea(b, Convert.ToInt32(l), Convert.ToInt32(t), Convert.ToInt32(w), Convert.ToInt32(h));
                Color c = ptp.getAverageColor(b);
                cl.Add(c);
            }

            List<double> bl = getBright(cl, cm);

            DrawLines(bl);
        }

        public async void DrawAntiShake(MediaElement me, TimeSpan BeginTime, TimeSpan EndTime, int CapturePhotoNumPerSec,double l, double t, double w, double h, Canvas VideoCanvas)
        {
            double pt = 1000d / CapturePhotoNumPerSec;
            List<WriteableBitmap> cbl =await VideoCapture(me, BeginTime, EndTime, CapturePhotoNumPerSec);
            DrawAntiShake(pt, cbl, l, t, w, h, VideoCanvas);
        }

        public void DrawAntiShake(double CaptureTimeLong, List<WriteableBitmap> CaptureImageList,double l, double t, double w, double h,Canvas VideoCanvas)
        {
            //sb.Stop();
            //sb.Children.Clear();

            List<PixInfor> pl = new List<PixInfor>();
            PhotoTestParameter ptp = new PhotoTestParameter();

            for (int i = 0; i < CaptureImageList.Count; i++)
            {
                WriteableBitmap b = CaptureImageList[i];
                b = ptp.getImageArea(b, Convert.ToInt32(l), Convert.ToInt32(t), Convert.ToInt32(w), Convert.ToInt32(h));
                Point CP = ptp.getBrightChangeCenterPoint(b, 20);//找到区域中心点
                PixInfor pi = new PixInfor();
                pi.PhotoW = Convert.ToInt32(b.PixelWidth);
                pi.PhotoH = Convert.ToInt32(b.PixelHeight);
                pi.XPosition = Convert.ToInt32(CP.X);
                pi.YPosition = Convert.ToInt32(CP.Y);
                pi.colorValue = ptp.GetPixel(b, pi.XPosition, pi.YPosition);
                pi.CurrentTimeLong = TimeSpan.FromMilliseconds(CaptureTimeLong / CaptureImageList.Count);
                pl.Add(pi);

            }
            //sb.Duration = TimeSpan.FromMilliseconds(CaptureTimeLong);
            DrawPointPositionBright(pl, VideoCanvas, DrawCanvas);
        }

        public double getPathFigureLong(PathFigure pf)
        {
            Point sp = pf.StartPoint;
            double d = 0;
            for (int i = 0; i < pf.Segments.Count; i++)
            {
                LineSegment ls = pf.Segments[i] as LineSegment;
                if (ls != null)
                {
                    d = d + Math.Sqrt((ls.Point.X - sp.X) * (ls.Point.X - sp.X) + (ls.Point.Y - sp.Y) * (ls.Point.Y - sp.Y));
                    sp = ls.Point;
                }
            }
            return d;
        }
    }

    public enum ColorMode
    {
        All, R, G, B, Bright
    }

    public struct TextFont
    {
        public FontFamily TextFontFamily;
        public FontWeight TextFontWeight;
        public FontStyle TextFontStyle;
        public FontStretch TextFontStretch;
        public double Size;
        public TextAlignment Alignment;
    }
}
