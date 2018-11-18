using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;




namespace SilverlightLynxControls
{
    public class ActionRotate
    {
        FrameworkElement po;
        FrameworkElement RotateArea;
        RotateTransform tt = new RotateTransform();
        public ActionRotate(Panel RootPanel)
            : this(RootPanel, RootPanel){}

        void MoveControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //RotateArea.Cursor = Cursors.Arrow;
        }

        void MoveControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //RotateArea.Cursor = Cursors.Hand;
        }
        public ActionRotate(Panel RootPanel, FrameworkElement rotateArea)
        {
            po = RootPanel;
            TransformGroup tg;
            Transform t = po.RenderTransform;
            if (t == null)
            {
                tg = new TransformGroup();
                tg.Children.Add(tt);
                po.RenderTransform = tg;
            }
            else
            {
                if (t.GetType().Name.EndsWith("TransformGroup"))
                {
                    tg = (TransformGroup)t;
                    tg.Children.Add(tt);
                }
                else
                {
                    tg = new TransformGroup();
                    tg.Children.Add(t);
                    tg.Children.Add(tt);
                    po.RenderTransform = tg;
                }
            }
            Point cp = getCPoint();
            tt.CenterX = cp.X;
            tt.CenterY = cp.Y;
            RotateArea = rotateArea;
            RotateArea.PointerPressed += new PointerEventHandler(at_PointerPressed);
            RotateArea.PointerReleased += new PointerEventHandler(at_PointerReleased);
            RotateArea.PointerMoved += new PointerEventHandler(at_PointerMoved);
            RotateArea.PointerEntered += new PointerEventHandler(MoveControl_PointerEntered);
            RotateArea.PointerExited += new PointerEventHandler(MoveControl_PointerExited);
        }

        public bool IsMove
        {
            get
            {
                return _IsMove;
            }
            set
            {
                _IsMove = value;
            }
        }
        public bool _IsMove = false;

        public Point getCPoint()
        {
            Point p = new Point();
            double x = Canvas.GetLeft(po);
            double y = Canvas.GetTop(po);
            x = x + po.ActualWidth / 2;
            y = y + po.ActualHeight / 2;
            p.X = x;
            p.Y = y;
            return p;
        }

        public double getAgl(Point p0,Point p1)
        {
            Point cp=getCPoint();
            double t0 = (cp.Y - p0.Y) / (p0.X - cp.X);
            double t1 = (cp.Y - p1.Y) / (p1.X - cp.X);
            return Math.Atan(t1) - Math.Atan(t0);
        }

        public void at_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            IsMove = true;
            isMouseCaptured = true;

            e.Handled = true;
            Panel p = (Panel)po.Parent;

            mouseVerticalPosition = e.GetCurrentPoint(p).Position.Y;
            mouseHorizontalPosition = e.GetCurrentPoint(p).Position.X;
        }
        //double tx, ty;
        public virtual void at_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
         
            FrameworkElement item = sender as FrameworkElement;
            item = (sender as Panel).Parent as FrameworkElement;
 
            if (item == null) { return; }
            if (!isMouseCaptured) { return; }

            if (IsMove)
            {
                Panel p = (Panel)po.Parent;
                double a = getAgl(new Point(mouseHorizontalPosition, mouseVerticalPosition), new Point(e.GetCurrentPoint(p).Position.X, e.GetCurrentPoint(p).Position.Y));
                tt.Angle = tt.Angle+a;
                mouseVerticalPosition = e.GetCurrentPoint(p).Position.Y;
                mouseHorizontalPosition = e.GetCurrentPoint(p).Position.X;
            }
        }

        public void at_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            //e.Handled = true;
            isMouseCaptured = false;

            if (sender != null) { _IsMove=false; }
        }
        bool isMouseCaptured;
        double mouseVerticalPosition;//中间的
        double mouseHorizontalPosition;

    }
}
