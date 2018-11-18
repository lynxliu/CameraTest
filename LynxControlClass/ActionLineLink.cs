using System;
using System.Net;
using System.Windows;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


using System.Windows.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI;




namespace SilverlightLynxControls
{
    public class ActionLineLink:ActionLink
    {
        public ActionLineLink(Panel p)
            : base(p)
        {
            po = p;
            if (IsEdit)
            {
                InitLine();

                po.PointerPressed += po_PointerPressed;
                po.PointerReleased += po_PointerReleased;
                po.PointerMoved += po_PointerMoved;
            }
        }

        void po_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (IsDrawing)
            {
                FrameworkElement item = sender as FrameworkElement;

                EndPoint.Y = e.GetCurrentPoint((UIElement)item.Parent).Position.Y;
                EndPoint.X = e.GetCurrentPoint((UIElement)item.Parent).Position.X;
                if (TLine == null) { }
                else
                {
                    po.Children.Remove(TLine);
                }
                Draw(po);
            }
        }

        void po_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

            FrameworkElement item = sender as FrameworkElement;

            IMove im = sender as IMove;
            IsDrawing = false;
        }

        void po_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            StartPoint.Y = e.GetCurrentPoint((UIElement)item.Parent).Position.Y;
            StartPoint.X = e.GetCurrentPoint((UIElement)item.Parent).Position.X;
            IsDrawing = true;
        }
        public string Status;
        public Point StartPoint;
        public Point EndPoint;


        //public Panel ParentPanel;
        public Line TLine;

        public void Draw(Panel p)
        {
            Line l = new Line();
            l.X1 = StartPoint.X;
            l.Y1 = StartPoint.Y;
            l.X2 = EndPoint.X;
            l.Y2 = EndPoint.Y;
            l.StrokeThickness = 3;
            //l.PointerEntered += new PointerEventHandler(l_PointerEntered);
            //l.PointerExited += new PointerEventHandler(l_PointerExited);
            l.Stroke = new SolidColorBrush(Colors.Red);
            p.Children.Add(l);
            //ParentPanel = p;
            TLine = l;
            l.SetValue(Canvas.ZIndexProperty, 1);
        }
        public bool IsDrawing = false;

    }
}
