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
using System.Threading;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Xaml.Input;

namespace SilverlightLynxControls
{
    public class ActionLink//相互关联的对象，使用连接线链接
    {

        public ActionLink(Panel p)
        {
            po = p;
            if (IsEdit)
            {
                InitLine();
                po.PointerEntered += new PointerEventHandler(po_PointerEntered);
                po.PointerExited += new PointerEventHandler(po_PointerExited);
                po.PointerPressed += new PointerEventHandler(po_PointerPressed);
                po.PointerReleased += new PointerEventHandler(po_PointerReleased);
                po.PointerMoved += new PointerEventHandler(po_PointerMoved);
            }
        }

        void po_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsBeginDrag)
            {

            }
        }

        void po_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            IsBeginDrag = false;

        }

        void po_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsBeginDrag = true;

            BeginDrawLine(e.GetCurrentPoint(null).Position);
        }
        bool IsBeginDrag = false;
        //bool IsDropActive = false;
        private void BeginDrawLine(Point tp)
        {
            Line dl = new Line();
            dl.X1 = getCPosition().X;
            dl.Y1 = getCPosition().Y;
            dl.X2 = tp.X;
            dl.Y2 = tp.Y;
        }

        void po_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            InActive();
        }

        void po_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

            Active();
        }
        public bool IsSelect;
        public bool IsActive = false;
        public Line getNewLine()
        {
            Line l = new Line();
            l.StrokeThickness = 3;
            l.Stroke = new SolidColorBrush(Colors.Red);
            return l;
        }

        public void InActive()
        {
            po.Children.Remove(rl);
            IsActive = false;
        }

        Line ll, rl, tl, bl;
        public void InitLine()
        {
            ll = getNewLine();
            ll.X1 = 0;
            ll.X2 = 0;
            ll.Y1 = 0;
            ll.Y2 = po.Height;

            rl = getNewLine();
            rl.X1 = po.Width;
            rl.X2 = po.Width;
            rl.Y1 = 0;
            rl.Y2 = po.Height;

            tl = getNewLine();
            tl.X1 = 0;
            tl.X2 = po.Width;
            tl.Y1 = 0;
            tl.Y2 = 0;

            bl = getNewLine();
            bl.X1 = 0;
            bl.X2 = po.Width;
            bl.Y1 = po.Height;
            bl.Y2 = po.Height;
        }
        public void Active()
        {
            IsActive = true;
            if (!po.Children.Contains(ll))
            {
                po.Children.Add(ll);
            }
            if (!po.Children.Contains(rl))
            {
                po.Children.Add(rl);
            }
            if (!po.Children.Contains(tl))
            {
                po.Children.Add(tl);
            }
            if (!po.Children.Contains(bl))
            {
                po.Children.Add(bl);
            }
        }
        public bool IsEdit = true;
        public Point CPosition;//连接点
        public List<ILine> InLines = new List<ILine>();
        public List<ILine> OutLines = new List<ILine>();
        public List<IConnectObject> InObjectList = new List<IConnectObject>();
        public List<IConnectObject> OutObjectList = new List<IConnectObject>();

        public Panel po;

        public void setPosition(Point p)
        {
            po.SetValue(Canvas.LeftProperty, p.X - po.ActualWidth / 2);
            po.SetValue(Canvas.TopProperty, p.Y - po.ActualHeight / 2);

        }

        public void DrawNode(Panel p)
        {
            if (p.Children.Contains(po)) { }
            else
            {
                p.Children.Add(po);
                po.SetValue(Canvas.ZIndexProperty, 10);
            }
        }

        public void MoveOutLines()
        {
            foreach (ILine l in OutLines)
            {
                l.Move(getCPosition(), l.EndPoint);
            }
        }

        public void MoveInLines()
        {
            foreach (ILine l in InLines)
            {
                l.Move(l.StartPoint, getCPosition());
            }
        }

        public void MoveLines()
        {
            MoveOutLines();
            MoveInLines();
        }
        public void DrawOutObject(Panel p)//每一个都只能绘制流出的线
        {
            int i = 0;

            foreach (IConnectObject to in OutObjectList)
            {
                Point sp = getCPosition();

                Point np = new Point();
                np.X = sp.X + 250;
                np.Y = sp.Y + 120 * i;

                //to.linkAction.setPosition(np);
                //to.linkAction.DrawNode(p);
                //l.SetValue(Canvas.ZIndexProperty, 1);
                i++;
            }
            //this.SetValue(Canvas.ZIndexProperty, 10);
        }
        public void DrawOutLine(Panel p)//每一个都只能绘制流出的线
        {
            int i = 0;

            foreach (IConnectObject to in OutObjectList)
            {
                Point sp = getCPosition();

                //to.linkAction.setPosition(np);
                Point ep = to.getCPosition();
                ILine l = new ILine();
                l.StartPoint = sp;
                l.EndPoint = ep;
                l.Memo = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

                OutLines.Add(l);
                to.InLines.Add(l);
                l.Draw(p);
                //to.linkAction.DrawNode(p);

                i++;
            }
            //this.SetValue(Canvas.ZIndexProperty, 10);
        }

        public void AddNext(IConnectObject so, IConnectObject to)
        {
            so.OutObjectList.Add(to);
        }

        public Point getCPosition()
        {
            //Dispatcher.BeginInvoke(delegate
            //{
            //    //get and use ActualWidth/ActualHeight 
            //});
            //UpdateLayout();
            po.UpdateLayout();

            Point p = new Point();
            p.Y = (double)po.GetValue(Canvas.TopProperty) + po.ActualHeight / 2;
            p.X = (double)po.GetValue(Canvas.LeftProperty) + po.ActualWidth / 2;
            return p;
        }

        public Point getLPosition(FrameworkElement e)
        {
            Point p = new Point();
            p.Y = (double)e.GetValue(Canvas.TopProperty) + e.ActualHeight / 2;
            p.X = (double)e.GetValue(Canvas.LeftProperty);
            return p;
        }

        public Point getRPosition(FrameworkElement e)
        {
            Point p = new Point();
            p.Y = (double)e.GetValue(Canvas.TopProperty) + e.ActualHeight / 2;
            p.X = (double)e.GetValue(Canvas.LeftProperty) + e.ActualWidth;
            return p;
        }

        public Point getTPosition(FrameworkElement e)
        {
            Point p = new Point();
            p.Y = (double)e.GetValue(Canvas.TopProperty);
            p.X = (double)e.GetValue(Canvas.LeftProperty) + e.ActualWidth / 2;
            return p;
        }

        public Point getBPosition(FrameworkElement e)
        {
            Point p = new Point();
            p.Y = (double)e.GetValue(Canvas.TopProperty) + e.ActualHeight;
            p.X = (double)e.GetValue(Canvas.LeftProperty) + e.ActualWidth / 2;
            return p;
        }


        public void at_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //base.at_PointerMoved(sender, e);
            MoveLines();
        }


    }
}
