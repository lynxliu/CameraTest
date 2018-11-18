using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;

using SilverlightLynxControls;
using Windows.UI.Xaml.Input;
using Windows.UI;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;

namespace SLPhotoTest.PhotoInfor
{
    public partial class PhotoBrightCurve : UserControl
    {
        public PhotoBrightCurve()
        {
            InitializeComponent();
            dg = new DrawGraphic(DrawCanvas);//初始化绘图对象
            //acm = new ActionMove(this, this);
        }
        //ActionMove acm;//允许移动

        Image TargetImage//针对的目标图像
        {
            get
            {
                if (Target == null) { return null; }
                return Target.getImage();
            }
        }
        Point sp,ep;
        Canvas DrawSelectCanvas//绘制选取区域的画布
        {
            get
            {
                if (Target == null) { return null; }
                return Target.getDrawObjectCanvas();
            }
        }

        DrawGraphic dg;//绘制曲线的对象
        LChartPhoto Target;//分析的目标

        Color getTargetColor()//依据comboBoxChanel决定边界的颜色
        {
            ComboBoxItem ci = comboBoxChanel.SelectedItem as ComboBoxItem;
            if (ci.Tag.ToString() == "R")
            {
                return Colors.Red;
            }
            if (ci.Tag.ToString() == "G")
            {
                return Colors.Green;
            }
            if (ci.Tag.ToString() == "B")
            {
                return Colors.Blue;
            }
            return Colors.Black;
        }
        public void setTarget(LChartPhoto pc)//设置提取信息的目标
        {
            Target = pc;
        }
        DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();

        void img_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (begin)
            {
                ComboBoxItem ci = comboBoxSelectType.SelectedItem as ComboBoxItem;
                if (ci.Tag.ToString() == "Path")
                {
                    ActiveDrawPath(e.GetCurrentPoint(TargetImage).Position);
                }
                if (ci.Tag.ToString() == "Line")
                {
                    ActiveDrawLine(e.GetCurrentPoint(TargetImage).Position);
                }
                if (ci.Tag.ToString() == "Area")
                {
                    ActiveDrawArea(e.GetCurrentPoint(TargetImage).Position);
                }
            }
        }

        
        void img_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (Target == null) { return; }
            Target.DisableMove();
            if (TargetImage == null) { return; }
            //TargetImage.CaptureMouse();
            sp = e.GetCurrentPoint(DrawSelectCanvas).Position;//这个点,只是相对显示图来说的
            ComboBoxItem ci = comboBoxSelectType.SelectedItem as ComboBoxItem;
            if (ci.Tag.ToString() == "Path")
            {

                CreatePath(sp, getTargetColor());
            }
            if (ci.Tag.ToString() == "Line")
            {

                CreateLine(sp, getTargetColor());
            }

            if (ci.Tag.ToString() == "Area")
            {

                CreateRect(sp, getTargetColor());
            }
            begin = true;
        }
        
        void img_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (Target == null) { return; }

            if (TargetImage == null) { return; }
            //TargetImage.ReleaseMouseCapture();
            ep = e.GetCurrentPoint(DrawSelectCanvas).Position;//这个点,只是相对显示图来说的
            ComboBoxItem ci = comboBoxSelectType.SelectedItem as ComboBoxItem;
            if (ci.Tag.ToString() == "Path")
            {
                DrawPathBrightness();
                
            }
            if (ci.Tag.ToString() == "Line")
            {
                DrawLineBrightness();
                
            }

            if (ci.Tag.ToString() == "Area")
            {
                DrawAreaBrightness();
                
            }
            begin = false;
        }

        void ShowBrightInfor(List<Color> cl)
        {
            List<double> bl = new List<double>();
            Color tc = getTargetColor();
            if (tc == Colors.Black)
            {
                foreach (Color xc in cl)
                {
                    bl.Add(ColorManager.getB(xc));
                }
            }
            if (tc == Colors.Red)
            {
                foreach (Color xc in cl)
                {
                    bl.Add(xc.R);
                }
            }
            if (tc == Colors.Green)
            {
                foreach (Color xc in cl)
                {
                    bl.Add(xc.G);
                }
            }
            if (tc == Colors.Blue)
            {
                foreach (Color xc in cl)
                {
                    bl.Add(xc.B);
                }
            }
            ShowBrightInfor(bl);
        }

        void ShowBrightInfor(List<double> bl)//已经因为选择的类型而筛选
        {
            if (bl.Count == 0) { return; }
            double ave = bl.Average();
            double changes=0;
            double max = bl.Max();
            double min = bl.Min();

            foreach (double d in bl)
            {
                changes = changes + ((d - ave) * (d - ave));
            }
            changes = Math.Sqrt(changes/bl.Count);

            textBright.Text = ave.ToString();
            ToolTipService.SetToolTip(textBright, ave.ToString());

            textBrightChanges.Text = changes.ToString();
            ToolTipService.SetToolTip(textBrightChanges, changes.ToString());

            textBrightMax.Text = max.ToString();
            ToolTipService.SetToolTip(textBrightMax, max.ToString());

            textBrightMin.Text = min.ToString();
            ToolTipService.SetToolTip(textBrightMin, min.ToString());
        }

        Polyline selectPath;//折线
        Line selectLine;
        Rectangle selectRec;
        void ClearSelect()
        {
            if (DrawSelectCanvas == null) { return; }
            List<FrameworkElement> ml = new List<FrameworkElement>();
            foreach (FrameworkElement fe in DrawSelectCanvas.Children)
            {
                if (fe is Image) { }
                else
                {
                    ml.Add(fe);
                }
            }
            foreach (FrameworkElement fe in ml)
            {
                DrawSelectCanvas.Children.Remove(fe);
            }
            ml.Clear();
            selectPath = null;
            selectLine = null;
            selectRec = null;
        }
        public void CreatePath(Point cp, Color bc)//新建路径
        {
            selectPath = new Polyline();
            sp = cp;
            selectPath.Points.Add(cp);
            PathGeometry pg = new PathGeometry();
            //pg.FillRule = FillRule.Nonzero;
            //selectPath.Data = pg;
            //PathFigure pf = new PathFigure();
            //selectPath.StrokeDashArray = new DoubleCollection();
            //selectPath.StrokeDashArray.Add(2);
            //selectPath.StrokeDashArray.Add(1);
            //pg.Figures.Add(pf);
            //pf.Segments.Clear();
            //pf.StartPoint = cp;
            //selectPath.Fill = new SolidColorBrush(Colors.Transparent);
            selectPath.Stroke = new SolidColorBrush(bc);
            selectPath.StrokeThickness = 3;
        }

        public void CreateLine(Point cp, Color bc)
        {
            selectLine = new Line();
            selectLine.X1 = cp.X;
            selectLine.Y1 = cp.Y;
            selectLine.X2=cp.X;
            selectLine.Y2=cp.X;
            selectLine.StrokeThickness = 3;
            selectLine.Stroke = new SolidColorBrush(bc);
        }

        public void CreateRect(Point cp,Color bc)
        {
            selectRec = new Rectangle();
            sp = cp;
            selectRec.Fill = new SolidColorBrush(Colors.Transparent);
            selectRec.StrokeThickness = 3;
            selectRec.Stroke = new SolidColorBrush(bc);
        }

        public void ActiveDrawPath(Point cp)
        {
            if (Target == null) { return; }

            if (selectPath == null) { return; }
            if (DrawSelectCanvas == null) { return; }
            selectPath.Points.Add(cp);
            //PathGeometry pg = selectPath.Data as PathGeometry;
            
            //PathFigure pf = pg.Figures[0];
            //if (pf == null) { return; }
            //LineSegment ls = new LineSegment();
            //ls.Point = cp;
            //pf.Segments.Add(ls);
            if (!DrawSelectCanvas.Children.Contains(selectPath)) { DrawSelectCanvas.Children.Add(selectPath); }


        }

        public void ActiveDrawLine(Point cp)
        {
            if (Target == null) { return; }

            if (selectLine == null) { return; }
            selectLine.X2=cp.X;
            selectLine.Y2=cp.Y;
            if (!DrawSelectCanvas.Children.Contains(selectLine)) { DrawSelectCanvas.Children.Add(selectLine); }
        }

        public void ActiveDrawArea(Point cp)//绘制矩形区域
        {
            if (Target == null) { return; }

            if (selectRec == null) { return; }

            selectRec.Width = Math.Abs(cp.X - sp.X);
            selectRec.Height = Math.Abs(cp.Y - sp.Y);
            Canvas.SetLeft(selectRec, Math.Min(cp.X,sp.X));
            Canvas.SetTop(selectRec, Math.Min(cp.Y,sp.Y));
            if (!DrawSelectCanvas.Children.Contains(selectRec)) { DrawSelectCanvas.Children.Add(selectRec); }
        }

        //public void DeActiveDrawPath()
        //{
        //    if (Target == null) { return; }

        //    if (selectPath == null) { return; }

        //    Canvas drawCanvas = Target.getCanvas();
        //    if (drawCanvas.Children.Contains(selectPath)) { drawCanvas.Children.Remove(selectPath); }
        //    selectPath = null;
        //}

        //public void DeActiveDrawLine()
        //{
        //    if (Target == null) { return; }

        //    if (selectPath == null) { return; }

        //    Canvas drawCanvas = Target.getCanvas();
        //    if (drawCanvas.Children.Contains(selectLine)) { drawCanvas.Children.Remove(selectLine); }
        //    selectLine = null;
        //}

        //public void DeActiveDrawArea()
        //{
        //    if (Target == null) { return; }

        //    if (selectPath == null) { return; }

        //    Canvas drawCanvas = Target.getCanvas();
        //    if (drawCanvas.Children.Contains(selectRec)) { drawCanvas.Children.Remove(selectRec); }
        //    selectRec = null;
        //}


        public List<Color> getPathLine()
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            Point sp,cp;
            List<Color> cl = new List<Color>();
            if (selectPath == null) { return null; }
            if (selectPath.Points.Count == 0) { return null; }
            sp = DrawGraphic.getImagePosition(selectPath.Points[0], TargetImage);
            for (int i = 1; i < selectPath.Points.Count;i++ )
            {
                cp = DrawGraphic.getImagePosition(selectPath.Points[i], TargetImage);
                List<Color> scl = pt.getLine(TargetImage.Source as WriteableBitmap, sp, cp);
                foreach (Color cc in scl)
                {
                    cl.Add(cc);
                }
                sp = cp;
            }
            //PathGeometry pg = selectPath.Data as PathGeometry;
            //if (pg == null) { return null; }
            //foreach (PathFigure pf in pg.Figures)
            //{
            //    sp = DrawGraphic.getImagePosition(pf.StartPoint, TargetImage);
                
            //    foreach (LineSegment ls in pf.Segments)
            //    {
            //        cp = DrawGraphic.getImagePosition(ls.Point, TargetImage);

            //        sp = new Point(cp.X, cp.Y);
            //    }
            //}
            return cl;
        }
        //public void Draw(Point cp)
        //{
        //    if (begin)//表示是第一次有个起始点
        //    {
        //        PathGeometry pg = selectPath.Data as PathGeometry;

        //        LineSegment l = new LineSegment();

        //        l.Point = cp;
        //        pg.Figures[0].Segments.Add(l);

        //    }

        //}
        bool begin = false;

        public void DrawPathBrightness()
        {
            if (selectPath == null) { return ; }
            List<Point> pl = getPathPointList();
            List<Point> ipl = getImagePathPointList(pl);
            List<Color> cl = getPathColor(ipl);
            List<double> il = getBrightness(cl);
            dg.DrawLines(il);//绘制曲线
            double d=0;
            if (ipl.Count < 2) { return; }
            Point sp = ipl[0];
            for (int i = 1; i < ipl.Count;i++ )//求出长度
            {
                d = d + Math.Sqrt((ipl[i].X - sp.X) * (ipl[i].X - sp.X) + (ipl[i].Y - sp.Y) * (ipl[i].Y - sp.Y));
                sp = ipl[i];

            }
            textPointCountValue.Text = il.Count.ToString();//给出点数
            ShowBrightInfor(cl);
        }

        public void DrawLineBrightness()
        {
            if (selectLine == null) { return; }
            List<Point> pl = new List<Point>();
            pl.Add(new Point(selectLine.X1, selectLine.Y1));
            pl.Add(new Point(selectLine.X2, selectLine.Y2));
            List<Point> ipl = getImagePathPointList(pl);
            List<Color> cl = getPathColor(ipl);
            List<double> il = getBrightness(cl);
            dg.DrawLines(il);
            textPointCountValue.Text = il.Count.ToString();
            ShowBrightInfor(cl);
        }

        public void DrawAreaBrightness()
        {
            if (selectRec == null) { return; }
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            DrawGraphic dg = new DrawGraphic(DrawCanvas);
            WriteableBitmap sb=TargetImage.Source as WriteableBitmap;
            Point isp=DrawGraphic.getImagePosition(sp, TargetImage);
            Point iep = DrawGraphic.getImagePosition(ep, TargetImage);
            double iw=DrawGraphic.getImageDistance(Math.Abs(iep.X-isp.X),TargetImage);
            double ih = DrawGraphic.getImageDistance(Math.Abs(iep.Y - isp.Y), TargetImage);
            WriteableBitmap cb = pt.getImageArea(sb, (int)isp.X, (int)isp.Y, (int)iw, (int)ih);

            List<int> tl = pt.getBrightPixNum(cb);
            dg.ForeColor = Colors.Blue;
            dg.DrawBrightPixNumHistogram(tl);
            textPointCountValue.Text = (iw * ih).ToString();
            List<Color> cl = pt.getImageColorList(cb);
            ShowBrightInfor(cl);
        }

        public List<double> getBrightness(List<Color> cl)//从颜色获取目标亮度列表，依据选择的通道
        {
            List<double> l = new List<double>();
            Color tc = getTargetColor();
            dg.ForeColor = tc;
            if (tc == Colors.Black)
            {
                foreach (Color c in cl)
                {
                    double d = .299 * c.R + .587 * c.G + .114 * c.B;
                    l.Add(d);
                }
            }
            if (tc == Colors.Red)
            {
                foreach (Color c in cl)
                {
                    double d = c.R;
                    l.Add(d);
                }
            }
            if (tc == Colors.Green)
            {
                foreach (Color c in cl)
                {
                    double d =c.G;
                    l.Add(d);
                }
            }
            if (tc == Colors.Blue)
            {
                foreach (Color c in cl)
                {
                    double d = c.B;
                    l.Add(d);
                }
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
            //PathGeometry pg = selectPath.Data as PathGeometry;
            //PathFigure x = pg.Figures[0];
            List<Point> pl = new List<Point>();
            foreach (Point p in selectPath.Points)
            {
                pl.Add(p);
            }
            //pl.Add(x.StartPoint);
            //foreach (LineSegment ps in x.Segments)
            //{
            //    Point tp = ps.Point;
            //    pl.Add(tp);
            //}
            return pl;
        }

        Brush ActiveBrush = new SolidColorBrush(Colors.Red);
        private void buttonOption_Click(object sender, RoutedEventArgs e)
        {
            if (Target == null) { return; }

            if (stackBeginEnd.Background == null)
            {
                stackBeginEnd.Background = ActiveBrush;
                TargetImage.PointerPressed += new PointerEventHandler(img_PointerPressed);
                TargetImage.PointerReleased += new PointerEventHandler(img_PointerReleased);
                TargetImage.PointerMoved += new PointerEventHandler(img_PointerMoved);
                TextBeginEnd.Text = "结束";
            }
            else
            {
                stackBeginEnd.Background = null;
                TextBeginEnd.Text = "开始";
                TargetImage.PointerPressed -= new PointerEventHandler(img_PointerPressed);
                TargetImage.PointerReleased -= new PointerEventHandler(img_PointerReleased);
                TargetImage.PointerMoved -= new PointerEventHandler(img_PointerMoved);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            ClearSelect();
            DrawCanvas.Children.Clear();
            
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            if (stackBeginEnd.Background != null)
            {
                stackBeginEnd.Background = null;
                TextBeginEnd.Text = "开始";
                TargetImage.PointerPressed -= new PointerEventHandler(img_PointerPressed);
                TargetImage.PointerReleased -= new PointerEventHandler(img_PointerReleased);
                TargetImage.PointerMoved -= new PointerEventHandler(img_PointerMoved);
            }
            (Parent as Panel).Children.Remove(this);

        }
    }
}
