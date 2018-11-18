using System;
using System.Net;
using System.Windows;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


using System.Windows.Input;



using System.Collections.Generic;
using SilverlightLFC.common;
using Windows.UI.Xaml.Input;
using Windows.UI;
using Windows.UI.Xaml.Shapes;

namespace SilverlightLynxControls
{
    //public delegate void ProcessDrop(object o);//设置获取数据表的回调函数
    //public delegate object getProcessObject();
    public delegate void DropEventHandler(object sender, LDropCompleteEventArgs e);//定义的系统事件

    public class LDropCompleteEventArgs : EventArgs//通用的事件类，传送全部的参数
    {
        public object SenderObject;//产生这个事件的对象句柄
        public DateTime EventTime = DateTime.Now;//事件发生时刻
        public object DropObject;//传递的对象

    }
    public class ActionDragDrop
    {
        public static void BeginDrag(object sender,object DO)
        {
            ActionDragDrop.DropObject = DO;
            Sender = sender;
            IsDragging = true;
        }
        public static bool IsBeginDrag()
        {
            return IsDragging;
        }
        public static object EndDrag(bool FinishDragging)
        {
            object t = DropObject;
            if (FinishDragging)
            {
                IsDragging = false;
                Sender = null;
                DropObject = null;
            }
            return t;
        }
        public static bool IsDragging = false;
        public static object Sender;
        public static Object DropObject;
        public static IDragDrop Source;
        public static string type;
        
        public event DropEventHandler DropObjectComplete;
        bool IsArrow = true;
        bool IsDrag = false;
        Point SP;//起始点
        Panel DeskPanel;//桌面
        FrameworkElement dragicon;//拖动时候的指示图标，可以是null，那就没有指示
        public void SendEvent()//引发一个附带数据的消息
        {
            LDropCompleteEventArgs le = new LDropCompleteEventArgs();
            le.DropObject = o;//返回值
            le.SenderObject = this;//表明谁引发了消息
            if (DropObjectComplete != null)
            {
                DropObjectComplete(this, le);
            }
        }

        object o=null;
        FrameworkElement DragArea;
        FrameworkElement DropArea;
        IDragDrop ParentUC;
        //ProcessDrop TProc;
        //getProcessObject SProc;

        public Panel FindDesktop()
        {
            //UIElement ui = Application.Current.RootVisual;
            return (Panel) VisualTreeHelper.GetChild(Window.Current.Content, 0);
        }


        public ActionDragDrop(IDragDrop UC, FrameworkElement dragArea, FrameworkElement dropArea,Panel dp,FrameworkElement Icon)//构造函数
        {
            DragArea = dragArea;
            DropArea = dropArea;
            ParentUC = UC;
            if (dp == null)
            {
                dp = FindDesktop();
            }
            
            DeskPanel = dp;
            dragicon = Icon;
            //SProc = sp;
            //TProc = td;
            DragArea.PointerPressed += new PointerEventHandler(DragArea_PointerPressed);
            DragArea.PointerReleased += new PointerEventHandler(DragArea_PointerReleased);
            //DragArea.PointerExited += new PointerEventHandler(DragArea_PointerExited);
            DropArea.PointerReleased += new PointerEventHandler(DropArea_PointerReleased);
            DropArea.PointerEntered += new PointerEventHandler(DropArea_PointerEntered);
            DropArea.PointerExited += new PointerEventHandler(DropArea_PointerExited);
            DeskPanel.PointerMoved += new PointerEventHandler(DeskPanel_PointerMoved);
            DeskPanel.PointerReleased += new PointerEventHandler(DeskPanel_PointerReleased);
        }

        void DeskPanel_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            HideArrow();
            HideIcon();
            IsDrag = false;
        }

        void DragArea_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            IsDrag = false;
            HideArrow();
            HideIcon();
        }

        void DeskPanel_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsArrow)
            {
                if (IsDrag)
                {
                    DrawDragArror(e.GetCurrentPoint(null).Position);
                    if (dragicon != null)
                    {
                        Canvas.SetLeft(dragicon, e.GetCurrentPoint(null).Position.X);
                        Canvas.SetTop(dragicon, e.GetCurrentPoint(null).Position.Y);
                    }
                }
            }
        }
        Path DragArrow = new Path();
        private void DrawDragArror(Point point)
        {
            if (!DeskPanel.Children.Contains(DragArrow))
            {
                DeskPanel.Children.Add(DragArrow);
            }
            setArrow(SP, point, 7, Colors.Yellow, Colors.Yellow, Colors.Yellow);
            DragArrow.Visibility = Visibility.Visible;
            //DragArrow.X1 = SP.X;
            //DragArrow.X2 = point.X;
            //DragArrow.Y1 = SP.Y;
            //DragArrow.Y2 = point.Y;
            //DragArrow.StrokeThickness = 5;
            //DragArrow.StrokeDashCap = PenLineCap.Triangle;
            //DragArrow.Stroke = new SolidColorBrush(Colors.Yellow); 
        }
        private void ShowIcon(Point p)
        {
            if (dragicon == null) { return; }
            if (!DeskPanel.Children.Contains(dragicon))
            {
                DeskPanel.Children.Add(dragicon);
            }
            else
            {
                dragicon.Visibility = Visibility.Visible;
            }
            dragicon.Opacity = 0.7;
            Canvas.SetLeft(dragicon, SP.X);
            Canvas.SetTop(dragicon, SP.Y);
        }
        private void HideIcon()
        {
            if (dragicon == null) { return; }
            if (!DeskPanel.Children.Contains(dragicon))
            {

            }
            else
            {
                dragicon.Visibility = Visibility.Collapsed;
            }
        }
        private void HideArrow()
        {
            if (!DeskPanel.Children.Contains(DragArrow))
            {

            }
            else
            {
                DragArrow.Visibility = Visibility.Collapsed;
            }
        }


        void DropArea_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            ShowDropDeActive();
            //this.DropObjectComplete -= new DropEventHandler(ActionDragDrop_DropObjectComplete);
        }

        public void setArrow(Point SP, Point EP, double r, Color sc, Color ec, Color ForeColor)
        {
            double arc = -Math.Atan((EP.Y - SP.Y) / (EP.X - SP.X));
            bool isR = true;
            if (EP.X - SP.X > 0)
            {
                
            }
            else
            {
                isR = false;
            }
            Point[] dp = new Point[7];
            dp[0] = new Point(SP.X + Math.Cos(Math.PI / 6 + arc) * r, SP.Y - Math.Sin(Math.PI / 6 + arc) * r);
            dp[6] = new Point(SP.X + Math.Cos(-Math.PI / 6 + arc) * r, SP.Y - Math.Sin(-Math.PI / 6 + arc) * r);
            dp[1] = new Point(EP.X + Math.Cos(Math.PI / 2 + arc) * r, EP.Y - Math.Sin(Math.PI / 2 + arc) * r);
            dp[2] = new Point(EP.X + Math.Cos(Math.PI / 2 + arc) * (r + r), EP.Y - Math.Sin(Math.PI / 2 + arc) * (r + r));
            if (isR)
            {
                dp[3] = new Point(EP.X - Math.Cos(Math.PI + arc) * (r + r), EP.Y + Math.Sin(Math.PI + arc) * (r + r));
            }
            else
            {
                dp[3] = new Point(EP.X + Math.Cos(Math.PI + arc) * (r + r), EP.Y - Math.Sin(Math.PI + arc) * (r + r));
            }
            dp[4] = new Point(EP.X + Math.Cos(-Math.PI / 2 + arc) * (r + r), EP.Y - Math.Sin(-Math.PI / 2 + arc) * (r + r));
            dp[5] = new Point(EP.X + Math.Cos(-Math.PI / 2 + arc) * r, EP.Y - Math.Sin(-Math.PI / 2 + arc) * r);

            if (DeskPanel.Children.Contains(DragArrow))
            {
                DeskPanel.Children.Remove(DragArrow);
            }
            DragArrow  = new Path();
            PathGeometry pg = new PathGeometry();
            DragArrow.Data = pg;
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
            if (isR) {
                a.SweepDirection = SweepDirection.Clockwise;
                a.IsLargeArc = true;
            }
            else
            {
                a.SweepDirection = SweepDirection.Counterclockwise;
                a.IsLargeArc = true;
            }
            
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

            DragArrow.Fill = rb;
            DragArrow.Stroke = new SolidColorBrush(ForeColor);
            DragArrow.StrokeThickness = 1;
            if (!DeskPanel.Children.Contains(DragArrow))
            {
                DeskPanel.Children.Add(DragArrow);
            }
            ActionActive.Active(DragArrow);
            //Canvas.SetZIndex(DragArrow, 100);
            //DrawCanvas.Children.Add(p);
            //return p;
        }


        void DropArea_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ShowDropActive();
            //this.DropObjectComplete += new DropEventHandler(ActionDragDrop_DropObjectComplete);
        }

        private void ShowDropActive()
        {
            DropArea.Opacity = 0.7;
        }

        private void ShowDropDeActive()
        {
            DropArea.Opacity = 1d;
        }

        void DropArea_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;

            SendEvent();
            ParentUC.processDropObject(ActionDragDrop.DropObject);
            IsDrag = false;

            HideArrow();
            HideIcon();
        }

        void DragArea_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsDrag = true;//表示开始拖放
            //fe.CaptureMouse();
            ActionDragDrop.DropObject = ParentUC.getDragObject();
            ActionDragDrop.Source = ParentUC;
            FrameworkElement fe = sender as FrameworkElement;
            if (fe == null) { return; }

            SP = e.GetCurrentPoint(null).Position;
            ShowIcon(SP);

            //e.Handled = true;
        }


    }
}
