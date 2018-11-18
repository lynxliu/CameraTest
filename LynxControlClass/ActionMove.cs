using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using SilverlightLynxControls.LogicView;
using SilverlightLynxControls.Design;
using Windows.UI.Xaml;
using Windows.Devices.Input;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;

namespace SilverlightLynxControls
{
    public delegate void LynxMoved(double dx,double dy);//重新布局以后的代理，用来使用一个函数指针给重新改变大小的对象以布局的机会
    //dx和dy代表作水平和垂直方向移动多少像素
    public class ActionMove
    {
        private LynxMoved lynxMoved;//回调

        public bool _EnableH=true;//水平是否有效
        public bool _EnableV=true;//垂直是否有效
        public bool _Enable = true;//水平垂直同时移动
        public bool EnableH//是否起效
        {
            get
            {
                return _EnableH;
            }
            set
            {
                _EnableH = value;
            }
        }
        public bool EnableV//是否起效
        {
            get
            {
                return _EnableV;
            }
            set
            {
                _EnableV = value;
            }
        }
        public bool Enable//是否起效
        {
            get
            {
                return _EnableH || _EnableV;
            }
            set
            {
                _EnableH = value;
                _EnableV = value;

            }
        }
        public List<FrameworkElement> MoveObjectList = new List<FrameworkElement>();//同时需要移动的对象列表
        public List<Point> MovePointList = new List<Point>();
        FrameworkElement po;
        FrameworkElement MoveArea;
        TranslateTransform tt = new TranslateTransform();
        public ActionMove(FrameworkElement moveControl) : this(moveControl, moveControl) { }

        public List<IDesignAnchorPoint> AnchorPointList = new List<IDesignAnchorPoint>();//起始点链接锚点的列表

        void MoveArea_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (_EnableH||_EnableV)
            {

            }
        }

        void MoveArea_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (_EnableH || _EnableV)
            {
            }
        }

        public ActionMove(FrameworkElement MoveControl, FrameworkElement moveArea)
            :this(MoveControl,moveArea,null)
        {

        }

        public ActionMove(FrameworkElement MoveControl, FrameworkElement moveArea,LynxMoved lMovedCallback)
        {
            po = MoveControl;
            MoveArea = moveArea;
            lynxMoved = lMovedCallback;
            MoveArea.PointerPressed += new PointerEventHandler(MoveArea_PointerPressed);
            MoveArea.PointerReleased += new PointerEventHandler(MoveArea_PointerReleased);
            MoveArea.PointerMoved += new PointerEventHandler(MoveArea_PointerMoved);
            MoveArea.PointerEntered += new PointerEventHandler(MoveArea_PointerEntered);
            MoveArea.PointerExited += new PointerEventHandler(MoveArea_PointerExited);
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
        public bool _IsMove = false;//只有

        //设置移动的边界
        double _MinLeft = -1000, _MaxLeft = 1000, _MinTop = -1000, _MaxTop = 1000;
        public double MinLeft
        {
            get { return _MinLeft; }
            set { _MinLeft = value; }
        }
        public double MaxLeft
        {
            get { return _MaxLeft; }
            set { _MaxLeft = value; }
        }
        public double MinTop
        {
            get { return _MinTop; }
            set { _MinTop = value; }
        }
        public double MaxTop
        {
            get { return _MaxTop; }
            set { _MaxTop = value; }
        }
        public void MoveArea_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (_EnableH || _EnableV)
            {
                FrameworkElement item = sender as FrameworkElement;
                IsMove = true;
                //isMouseCaptured = true;

                //e.Handled = false;
                //UserControl uc = (UserControl)po.Parent;
                Canvas p = po.Parent as Canvas;
                ActionActive.Active(po);
                mouseVerticalPosition = e.GetCurrentPoint(p).Position.Y;
                mouseHorizontalPosition = e.GetCurrentPoint(p).Position.X;
            }
        }

        public virtual void MoveArea_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
           
            //FrameworkElement item = sender as FrameworkElement;
            //item = (sender as Panel).Parent as FrameworkElement;
            
            //if (item == null) { return; }
            if (!(_EnableH || _EnableV)) { return; }
            //if (!isMouseCaptured) { return; }

            if (IsMove)
            {
                double deltaV=0, deltaH=0;
                Canvas p = po.Parent as Canvas;
                
                if (_EnableV)
                {
                    deltaV = e.GetCurrentPoint(p).Position.Y - mouseVerticalPosition;
                    double startTop = Convert.ToDouble(po.GetValue(Canvas.TopProperty));
                    double newTop = deltaV + startTop;
                    if ((newTop > _MinTop) && (newTop < _MaxTop))
                    {
                        po.SetValue(Canvas.TopProperty, newTop);
                        foreach (FrameworkElement fe in MoveObjectList)
                        {
                            fe.SetValue(Canvas.TopProperty, newTop);
                            //fe.SetValue(Canvas.LeftProperty, newLeft);
                        }
                        foreach (LynxAnchorPoint lp in AnchorPointList)
                        {
                            lp.MoveLines(0, deltaV);
                        }
                        mouseVerticalPosition = mouseVerticalPosition + deltaV;
                        //if (lynxMoved != null) { lynxMoved(); }//移动完成以后回调
                    }
                }
                if (_EnableH)
                {
                    deltaH = e.GetCurrentPoint(p).Position.X - mouseHorizontalPosition;
                    double startLeft = Convert.ToDouble(po.GetValue(Canvas.LeftProperty));
                    double newLeft = deltaH + startLeft;
                    if ((newLeft > _MinLeft) && (newLeft < _MaxLeft))
                    {
                        po.SetValue(Canvas.LeftProperty, newLeft);
                        foreach (FrameworkElement fe in MoveObjectList)
                        {
                            //fe.SetValue(Canvas.TopProperty, newTop);
                            fe.SetValue(Canvas.LeftProperty, newLeft);
                        }
                        foreach (LynxAnchorPoint lp in AnchorPointList)
                        {
                            lp.MoveLines(deltaH, 0);
                        }
                        mouseHorizontalPosition = mouseHorizontalPosition + deltaH;
                        
                    }
                }
                po.SetValue(Canvas.ZIndexProperty, MaxZIndex++);
                if (lynxMoved != null) { lynxMoved(deltaH,deltaV); }//移动完成以后回调
                
            }
        }

        public void MoveArea_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!(_EnableH || _EnableV)) { return; }
            FrameworkElement item = sender as FrameworkElement;

            IsMove = false;
            //e.Handled = false;
            //isMouseCaptured = false;
            
        }

        double mouseVerticalPosition;//中间的
        double mouseHorizontalPosition;

        public int MaxZIndex = 0;

    }
}
