using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;




namespace SilverlightLynxControls
{
    public class ILine
    {
        public Point StartPoint;
        public Point EndPoint;
        public string Memo;
        public TextBlock tm = new TextBlock();

        public Panel ParentPanel;

        public Line TLine;

        public void Move(Point sp,Point ep)
        {
            if (ParentPanel != null)
            {
                TLine.X1 = sp.X;
                TLine.Y1 = sp.Y;
                TLine.X2 = ep.X;
                TLine.Y2 = ep.Y;
                StartPoint = sp;
                EndPoint = ep;
                //Draw(ParentPanel);
            }
        }

        public void Draw()
        {
            if (ParentPanel != null)
            {
                Draw(ParentPanel);
            }
        }

        public void Draw(Panel p)
        {
            Line l = new Line();
            l.X1 = StartPoint.X;
            l.Y1 = StartPoint.Y;
            l.X2 = EndPoint.X;
            l.Y2 = EndPoint.Y;
            l.StrokeThickness = 5;
            l.PointerEntered += new PointerEventHandler(l_PointerEntered);
            l.PointerExited += new PointerEventHandler(l_PointerExited);
            l.Stroke = new SolidColorBrush(Colors.Green);
            p.Children.Add(l);
            ParentPanel = p;
            TLine = l;
            l.SetValue(Canvas.ZIndexProperty, 1);
        }

        void l_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement to = (FrameworkElement)sender;
            Panel u = (Panel)to.Parent;
            u.Children.Remove(tm);
        }

        void l_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //TextBlock m= new TextBlock();
            tm.Text = Memo;
            FrameworkElement to = (FrameworkElement)sender;
            Panel u = (Panel)to.Parent;
            double mouseVerticalPosition = e.GetCurrentPoint(to).Position.Y;
            double mouseHorizontalPosition = e.GetCurrentPoint(to).Position.X;
            tm.SetValue(Canvas.TopProperty, mouseVerticalPosition);
            tm.SetValue(Canvas.LeftProperty, mouseHorizontalPosition);
            u.Children.Add(tm);
        }




    }
}
