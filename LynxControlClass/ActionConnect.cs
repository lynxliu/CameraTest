using System;
using System.Net;
using System.Windows;

using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;




namespace SilverlightLynxControls
{
    public class ActionConnect//支持使用连接线互联，可以使用带有箭头的连接线，使用锚点，可以支持设计状态
    {
        public bool IsDesign = false;//默认是false，true的时候可以支持拖放
        public bool CanDrag = false;//拖放支持，默认是支持的
        public bool CanDrop = false;

        Control o;//真正操作的可是对象

        public ActionConnect(Control obj)//链接的对象
        {
            o=obj;
            o.PointerEntered += new PointerEventHandler(o_PointerEntered);
            o.PointerExited += new PointerEventHandler(o_PointerExited);
            o.PointerPressed += new PointerEventHandler(o_PointerPressed);
            o.PointerReleased += new PointerEventHandler(o_PointerReleased);
            o.PointerMoved += new PointerEventHandler(o_PointerMoved);
        }
        Rectangle ObjectBright = new Rectangle();//对象的光芒
        Color ActiveColor = Colors.Red;

        public void Active()//激活状态
        {
            Canvas c = (Canvas)o.Parent;
            if (!c.Children.Contains(ObjectBright))
            {
                c.Children.Add(ObjectBright);
            }
            ObjectBright.Fill = new SolidColorBrush(ActiveColor);
            ObjectBright.Width = o.Width + 20;
            ObjectBright.Height = o.Height + 20;
            Canvas.SetTop(ObjectBright, Canvas.GetTop(o) - 10);
            Canvas.SetLeft(ObjectBright, Canvas.GetLeft(o) - 10);
            Canvas.SetZIndex(ObjectBright, Canvas.GetZIndex(o) - 5);
            o.Opacity = 0.8;
        }

        public void DeActive()
        {
            o.Opacity = 1;
            Canvas c = (Canvas)o.Parent;
            if (c.Children.Contains(ObjectBright))
            {
                c.Children.Remove(ObjectBright);
            }
        }

        public Point getStartPoint(Point cp)//依据当前的点的位置得到适合的起始点
        {
            return new Point();
        }

        public void Designing(Point cp)//正在拖动一条新的设计线
        {

        }

        void o_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void o_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void o_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void o_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void o_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }


    }
}
