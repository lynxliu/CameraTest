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
using Windows.UI.Xaml.Input;        

namespace SilverlightLynxControls
{
    public class ActionResize//变换对象的大小，主要是针对特定的用户控件
    {
        bool _IsEnableLeft=true;
        bool _IsEnableRight=true;
        bool _IsEnableTop=true;
        bool _IsEnableBottom=true;

        public void setEnableLeft(bool b)
        {
            if (b) { ActiveLeftBorder(); }
            else
            {
                UnActiveLeftBorder();
            }
        }
        public void setEnableRight(bool b)
        {
            if (b) { ActiveRightBorder(); }
            else
            {
                UnActiveRightBorder();
            }
        }
        public void setEnableTop(bool b)
        {
            if (b) { ActiveTopBorder(); }
            else
            {
                UnActiveTopBorder();
            }
        }
        public void setEnableBottom(bool b)
        {
            if (b) { ActiveBottomBorder(); }
            else
            {
                UnActiveBottomBorder();
            }
        }
        public bool Enable
        {
            get//只要一个方向允许改变形状，就选有效
            {
                return _IsEnableBottom || _IsEnableLeft || _IsEnableRight || _IsEnableTop;
            }
            set
            {
                _IsEnableLeft=value;
                _IsEnableRight = value;
                _IsEnableTop = value;
                _IsEnableBottom = value;
                if (value) {
                    ActiveLeftBorder();
                    ActiveRightBorder();
                    ActiveTopBorder();
                    ActiveBottomBorder();
                } else {
                    UnActiveLeftBorder();
                    UnActiveRightBorder();
                    UnActiveTopBorder();
                    UnActiveBottomBorder();
                }
               
            }
        }
        public void Clear()//清除全部的事件订阅和增加的对象
        {
            UnActiveLeftBorder();
            UnActiveRightBorder();
            UnActiveBottomBorder();
            UnActiveTopBorder();
            ll = null;
            rl = null;
            bl = null;
            tl = null;
        }

        public double _MinWidth=5;
        public double MinWidth { set { _MinWidth = value; } }
        public double _MaxWidth=1200;
        public double MaxWidth { set { _MaxWidth = value; } }
        public double _MinHeight=5;
        public double MinHeight { set { _MinHeight = value; } }
        public double _MaxHeight = 800;
        public double MaxHeight { set { _MaxHeight = value; } }

        Panel so, fo;
        int BoderWieth = 3;
        //int MinDis = 15;
        Brush BorderRender = new SolidColorBrush(Colors.Red);
        Line ll = new Line();
        Line rl = new Line();
        Line tl = new Line();
        Line bl = new Line();
        double L, R, T, B;
        //double tw, th;
        double x0, y0;

        UserControl uc;
        public void Reset()
        {
            InitBoder();
            setClip();
        }

        public ActionResize(Panel RootPanel)//RootPanel必须是控件下的顶级Panel
        :this(RootPanel,RootPanel)
        {

        }

        public ActionResize(Panel RootPanel, Panel ClipForm) : this(RootPanel, ClipForm, null) { }//支持一个剪裁Panel

        public ActionResize(Panel RootPanel, Panel ClipForm, LynxResized lResizedCallback)//支持回调，改变大小以后可以重新布局
            : this((UserControl)RootPanel.Parent, RootPanel, ClipForm, lResizedCallback)
        { }

        public ActionResize(UserControl U, Panel RootPanel, Panel ClipForm, LynxResized lResizedCallback)//支持回调，改变大小以后可以重新布局
        {
            so = RootPanel;
            fo = ClipForm;
            uc = U;
            lynxResized = lResizedCallback;
            InitBoder();
        }

        public void setClip()//设置剪裁区
        {
            RectangleGeometry c = new RectangleGeometry();
            c.Rect = new Rect(0, 0, fo.Width, fo.Height);
            fo.Clip = c;
        }

        public void setSize(double w,double h)
        {
            ll.X1 = 0;
            ll.X2 = 0;
            ll.Y1 = 0;
            ll.Y2 = h;

            rl.X1 = w;
            rl.X2 = w;
            rl.Y1 = 0;
            rl.Y2 = h;

            tl.X1 = 0;
            tl.X2 = w;
            tl.Y1 = 0;
            tl.Y2 = 0;

            bl.X1 = 0;
            bl.X2 = w;
            bl.Y1 = h;
            bl.Y2 = h;

        }

        public void InitBoder()
        {
            if (_IsEnableLeft)
            {
                ActiveLeftBorder();
            }
            if (_IsEnableRight)
            {
                ActiveRightBorder();
            }
            if (_IsEnableTop)
            {
                ActiveTopBorder(); 
            }
            if (_IsEnableBottom)
            {
                ActiveBottomBorder();
            }
            //setClip();

        }

        private void ActiveLeftBorder()//激活左侧边界
        {
            if (!so.Children.Contains(ll))
            {
                
                so.Children.Add(ll);

                ll.PointerEntered += ll_PointerEntered;
                ll.PointerExited += ll_PointerExited;
                ll.PointerMoved += ll_PointerMoved;
                ll.PointerPressed += ll_PointerPressed;
                ll.PointerReleased += ll_PointerReleased;
            }
            ll.StrokeThickness = BoderWieth;
            ll.Stroke = BorderRender;
            ll.X1 = 0;
            ll.X2 = 0;
            ll.Y1 = 0;
            ll.Y2 = so.Height;

            
        }

        void ll_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_IsEnableLeft) { return; }
            BeginDrag = false;

            ll.Y1 = 0;
            ll.Y2 = so.ActualHeight;
            ll.X1 = 0;
            ll.X2 = 0;
            tl.X1 = 0;
            tl.X2 = so.ActualWidth;
            bl.X1 = 0;
            bl.X2 = so.ActualWidth;
            rl.X1 = so.ActualWidth - BoderWieth;
            rl.X2 = so.ActualWidth - BoderWieth;
        }

        void ll_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_IsEnableLeft) { return; }
            BeginDrag = true;
            Panel p = (Panel)uc.Parent;
            R = Canvas.GetLeft(uc) + uc.ActualWidth;
            x0 = e.GetCurrentPoint(null).Position.X;
            y0 = e.GetCurrentPoint(null).Position.Y;
        }

        void ll_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_IsEnableLeft) { return; }
            if (BeginDrag)
            {
                double dx = e.GetCurrentPoint(null).Position.X - x0;
                x0 = e.GetCurrentPoint(null).Position.X;
                Panel p = (Panel)uc.Parent;
                double d = R - e.GetCurrentPoint(p).Position.X;
                if ((d < _MinWidth) || (d > _MaxWidth))//比最小值大
                {
                    return;
                }
                fo.Width = fo.Width - dx;

                //if (d < MinDis) { d = MinDis; }
                so.Parent.SetValue(Canvas.LeftProperty, R - d);
                so.Width = d;
                uc.Width = d;
                ll.Y1 = 0;
                ll.Y2 = so.ActualHeight;
                ll.X1 = 0;
                ll.X2 = 0;
                tl.X1 = 0;
                tl.X2 = so.ActualWidth;
                bl.X1 = 0;
                bl.X2 = so.ActualWidth;
                rl.X1 = so.ActualWidth;
                rl.X2 = so.ActualWidth;

                setClip();
                if (lynxResized != null) { lynxResized(); }//回调，通知布局对象修改了布局
            }
        }

        void ll_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_IsEnableLeft) { return; }

        }

        void ll_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (!_IsEnableLeft) { return; }
            //tempCursor = uc.Cursor;
            //ll.Cursor = Cursors.SizeWE;
        }

        private void UnActiveLeftBorder()//取消左侧边界
        {
            if (!so.Children.Contains(ll)) { return; }

            ll.PointerEntered -= ll_PointerEntered;
            ll.PointerExited -= ll_PointerExited;
            ll.PointerMoved -= ll_PointerMoved;
            ll.PointerPressed -= ll_PointerPressed;
            ll.PointerReleased -= ll_PointerReleased; 
            so.Children.Remove(ll);
        }

        private void ActiveRightBorder()//初始化左侧边界
        {
            if (!so.Children.Contains(rl))
            {
                rl.PointerEntered += new PointerEventHandler(rl_PointerEntered);
                rl.PointerExited += new PointerEventHandler(rl_PointerExited);
                rl.PointerMoved += new PointerEventHandler(rl_PointerMoved);
                rl.PointerReleased += new PointerEventHandler(rl_PointerReleased);
                rl.PointerPressed += new PointerEventHandler(rl_PointerPressed);

                so.Children.Add(rl);
            }

            rl.StrokeThickness = BoderWieth;
            rl.Stroke = BorderRender;
            rl.X1 = so.Width;
            rl.X2 = so.Width;
            rl.Y1 = 0;
            rl.Y2 = so.Height;


        }

        private void UnActiveRightBorder()//取消左侧边界
        {
            if (!so.Children.Contains(rl)) { return; }

            rl.PointerEntered -= new PointerEventHandler(rl_PointerEntered);
            rl.PointerExited -= new PointerEventHandler(rl_PointerExited);
            rl.PointerMoved -= new PointerEventHandler(rl_PointerMoved);
            rl.PointerReleased -= new PointerEventHandler(rl_PointerReleased);
            rl.PointerPressed -= new PointerEventHandler(rl_PointerPressed);
            so.Children.Remove(rl);
        }

        void rl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableRight) { return; }
            if (BeginDrag) {  }
            else
            {

                //so.Cursor = Cursors.Arrow;
            }

        }

        void rl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableRight) { return; }


        }

        void rl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableRight) { return; }
            BeginDrag = true;
            Panel p = (Panel)uc.Parent;
            L = Canvas.GetLeft(uc);
            x0 = e.GetCurrentPoint(null).Position.X;
            y0 = e.GetCurrentPoint(null).Position.Y;

        }

        void rl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableRight) { return; }
            //e.Handled = true;
            BeginDrag = false;
            rl.Y1 = 0;
            rl.Y2 = so.Height;
            rl.X1 = so.Width - BoderWieth;
            rl.X2 = so.Width - BoderWieth;
            tl.X2 = so.Width;
            bl.X2 = so.Width;
        }

        void rl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableRight) { return; }
            if (BeginDrag)
            {
                double dx = e.GetCurrentPoint(null).Position.X - x0;
                x0 = e.GetCurrentPoint(null).Position.X;
                Panel p = (Panel)uc.Parent;
                double d = e.GetCurrentPoint(p).Position.X - L;
                if ((d < _MinWidth) || (d > _MaxWidth))//比最小值大
                {
                    return;
                }
                fo.Width = fo.Width + dx;

                //if (d < MinDis) { d = MinDis; }
                so.Width = d;
                uc.Width = d;
                Canvas.SetLeft(so, L);
                rl.Y1 = 0;
                rl.Y2 = so.Height;
                rl.X1 = so.Width;
                rl.X2 = so.Width;
                tl.X2 = so.Width;
                bl.X2 = so.Width;
                setClip();
                if (lynxResized != null) { lynxResized(); }//回调，通知布局对象修改了布局
            }
        }

        private void ActiveTopBorder()//初始化左侧边界
        {
            if (!so.Children.Contains(tl))
            {
                tl.PointerMoved += new PointerEventHandler(tl_PointerMoved);
                tl.PointerReleased += new PointerEventHandler(tl_PointerReleased);
                tl.PointerPressed += new PointerEventHandler(tl_PointerPressed);
                tl.PointerEntered += new PointerEventHandler(tl_PointerEntered);
                tl.PointerExited += new PointerEventHandler(tl_PointerExited);
                so.Children.Add(tl);
            }
            tl.StrokeThickness = BoderWieth;
            tl.Stroke = BorderRender;
            tl.X1 = 0;
            tl.X2 = so.Width;
            tl.Y1 = 0;
            tl.Y2 = 0;
            //so.Children.Add(tl);

        }

        private void UnActiveTopBorder()//取消左侧边界
        {
            if (!so.Children.Contains(tl)) { return; }

            tl.PointerEntered -= new PointerEventHandler(tl_PointerEntered);
            tl.PointerExited -= new PointerEventHandler(tl_PointerExited);
            tl.PointerMoved -= new PointerEventHandler(tl_PointerMoved);
            tl.PointerReleased -= new PointerEventHandler(tl_PointerReleased);
            tl.PointerPressed -= new PointerEventHandler(tl_PointerPressed);
            so.Children.Remove(tl);
        }

        void tl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableTop) { return; }

        }

        void tl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableTop) { return; }

        }

        void tl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableTop) { return; }
            BeginDrag = true;
            Panel p = (Panel)uc.Parent;

            B = Canvas.GetTop(uc) + uc.ActualHeight;

            x0 = e.GetCurrentPoint(null).Position.X;
            y0 = e.GetCurrentPoint(null).Position.Y;
        }

        void tl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableTop) { return; }
            BeginDrag = false;

            tl.Y1 = 0;
            tl.Y2 = 0;
            tl.X1 = 0;
            tl.X2 = so.ActualWidth;
            rl.Y2 = so.ActualHeight;
            ll.Y2 = so.ActualHeight;
            bl.Y1 = so.ActualHeight - BoderWieth;
            bl.Y2 = so.ActualHeight - BoderWieth;
            //uc.ReleaseMouseCapture();
        }

        void tl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableTop) { return; }
            if (BeginDrag)
            {
                double dy = e.GetCurrentPoint(null).Position.Y - y0;
                y0 = e.GetCurrentPoint(null).Position.Y;
                Panel p = (Panel)uc.Parent;
                double d = B - e.GetCurrentPoint(p).Position.Y;
                //if (d < MinDis) { d = MinDis; }
                //Canvas.SetTop(uc, B - d);

                if ((d < _MinHeight) || (d > _MaxHeight)) { return; }
                fo.Height = fo.Height - dy;
                so.Parent.SetValue(Canvas.TopProperty, B - d);
                so.Height = d;
                uc.Height = d;
                tl.Y1 = 0;
                tl.Y2 = 0;
                tl.X1 = 0;
                tl.X2 = so.ActualWidth;
                rl.Y2 = so.ActualHeight;
                ll.Y2 = so.ActualHeight;
                bl.Y1 = so.ActualHeight;
                bl.Y2 = so.ActualHeight;
                setClip();
                if (lynxResized != null) { lynxResized(); }//回调，通知布局对象修改了布局
            }
        }

        private void ActiveBottomBorder()//初始化左侧边界
        {
            if (!so.Children.Contains(bl))
            {
                bl.PointerMoved += new PointerEventHandler(bl_PointerMoved);
                bl.PointerReleased += new PointerEventHandler(bl_PointerReleased);
                bl.PointerPressed += new PointerEventHandler(bl_PointerPressed);
                bl.PointerEntered += new PointerEventHandler(bl_PointerEntered);
                bl.PointerExited += new PointerEventHandler(bl_PointerExited);
                so.Children.Add(bl);
            }
            bl.StrokeThickness = BoderWieth;
            bl.Stroke = BorderRender;
            bl.X1 = 0;
            bl.X2 = so.Width;
            bl.Y1 = so.Height;
            bl.Y2 = so.Height;
            //so.Children.Add(bl);

        }

        private void UnActiveBottomBorder()//取消左侧边界
        {
            if (!so.Children.Contains(bl)) { return; }

            bl.PointerEntered -= new PointerEventHandler(bl_PointerEntered);
            bl.PointerExited -= new PointerEventHandler(bl_PointerExited);
            bl.PointerMoved -= new PointerEventHandler(bl_PointerMoved);
            bl.PointerReleased -= new PointerEventHandler(bl_PointerReleased);
            bl.PointerPressed -= new PointerEventHandler(bl_PointerPressed);
            so.Children.Remove(bl);
        }

        void bl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableBottom) { return; }

        }

        void bl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableBottom) { return; }

        }

        void bl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableBottom) { return; }
            if (BeginDrag)
            {
                double dy = e.GetCurrentPoint(null).Position.Y - y0;
                y0 = e.GetCurrentPoint(null).Position.Y;
                Panel p = (Panel)uc.Parent;
                double d = e.GetCurrentPoint(p).Position.Y - T;
                if ((d < _MinHeight) || (d > _MaxHeight)) { return; }

                fo.Height = fo.Height + dy;

                
                
                //if (d < MinDis) { d = MinDis; }
                so.SetValue(Canvas.TopProperty, T);
                so.Height = d;
                uc.Height = d;
                bl.Y1 = so.ActualHeight;
                bl.Y2 = so.ActualHeight;
                bl.X1 = 0;
                bl.X2 = so.ActualWidth;
                rl.Y2 = so.ActualHeight;
                ll.Y2 = so.ActualHeight;
                setClip();
                if (lynxResized != null) { lynxResized(); }//回调，通知布局对象修改了布局
            }
        }

        void bl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableBottom) { return; }
            BeginDrag = true;
            //e.Handled = true;
            UserControl uc = (UserControl)so.Parent;
            Panel p = (Panel)uc.Parent;
            //T = e.GetPosition(p).Y - so.ActualHeight;
            T = Canvas.GetTop(uc);
            x0 = e.GetCurrentPoint(null).Position.X;
            y0 = e.GetCurrentPoint(null).Position.Y;
        }

        void bl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsEnableBottom) { return; }
            BeginDrag = false;

            bl.Y1 = so.ActualHeight - BoderWieth;
            bl.Y2 = so.ActualHeight - BoderWieth;
            bl.X1 = 0;
            bl.X2 = so.ActualWidth;
            rl.Y2 = so.ActualHeight;
            ll.Y2 = so.ActualHeight;
        }

        bool BeginDrag = false;
        //private Canvas PhotoEditFramework;
        private LynxResized lynxResized;


    }

    public delegate void LynxResized();//重新布局以后的代理，用来使用一个函数指针给重新改变大小的对象以布局的机会
}
