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




namespace SilverlightLynxControls
{
    public class ActionResizeBorder//把某个Border变为ActiveResize的类，支持激活和改变大小
    {
        public Border ActiveBorder { get; set; }
        public Brush ActiveBrush = new SolidColorBrush(Colors.Red);
        public Brush DeActiveBrush = new SolidColorBrush(Colors.Yellow);

        bool _EnableResize = false;//默认不允许改变大小
        public bool EnableResize//控制是否开始可以改变大小
        {
            get { return _EnableResize; }
            set
            {
                if (ActiveBorder == null)
                {
                    throw new SilverlightLFC.common.LFCException("未设置目标Border对象", null);

                }
                _EnableResize = value;
                if (value)
                {
                    ActiveBorder.PointerMoved += new PointerEventHandler(ActiveBorder_PointerMoved);
                    ActiveBorder.PointerPressed += new PointerEventHandler(ActiveBorder_PointerPressed);
                    ActiveBorder.PointerReleased += new PointerEventHandler(ActiveBorder_PointerReleased);
                }
                else
                {
                    ActiveBorder.PointerMoved -= new PointerEventHandler(ActiveBorder_PointerMoved);
                    ActiveBorder.PointerPressed -= new PointerEventHandler(ActiveBorder_PointerPressed);
                    ActiveBorder.PointerReleased -= new PointerEventHandler(ActiveBorder_PointerReleased);

                }
            }
        }
        bool IsBeginResize = false;
        void ActiveBorder_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            IsBeginResize = false;
        }

        void ActiveBorder_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsBeginResize = true;
        }

        ResizeType getResizeType(Point cp)
        {
            if ((cp.X > -1) && (cp.X < 3))
            {
                return ResizeType.Left;
            }
            if ((cp.Y > -1) && (cp.Y < 3))
            {
                return ResizeType.Top;
            }
            if ((cp.X > ActiveBorder.Width + 1) && (cp.X < ActiveBorder.Width-3))
            {
                return ResizeType.Right;
            }
            if ((cp.X > ActiveBorder.Height + 1) && (cp.X < ActiveBorder.Height - 3))
            {
                return ResizeType.Bottom;
            }
            return ResizeType.Normal;
        }


        void ActiveBorder_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Point cp = e.GetCurrentPoint(ActiveBorder).Position;

            if (IsBeginResize)
            {

            }
        }


    }

    enum ResizeType
    {
        Left,Right,Top,Bottom,Normal
    }
}
