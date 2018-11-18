using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;




namespace SilverlightLynxControls.Design
{
    public partial class LynxConnectPoint : UserControl
    {
        public LynxConnectPoint()//被连接线包含，位于连接线的端点，可以响应鼠标进行连接线拖拽，并可以固定到锚点
        {
            InitializeComponent();
            Height = 2 * R;
            Width = 2 * R;
            DesignConnectPoint.Width = Width;
            DesignConnectPoint.Height = Height;
        }

        public double R = 5;
        public bool IsEnable { get; set; }
        IDesignRelation _ConnectRelation;//给定连接的同时，必须给定该点的类型
        public IDesignRelation ConnectRelation { get { return _ConnectRelation; } }
        public ConnectPointType PointType = ConnectPointType.BeginPoint;//默认是连接的起始点
        //public LynxAnchorPoint ConnectAnchorPoint { get; set; }//该点链接的锚点

        public void SetAnchorPoint(LynxAnchorPoint lap)
        {
            if (PointType == ConnectPointType.BeginPoint)
            {
                if (lap == null)
                {
                    ConnectRelation.SourceID = "";
                }
                else
                {
                    ConnectRelation.SourceID = lap.ParentConnectControl.ObjectID;
                }
            }
            if (PointType == ConnectPointType.EndPoint)
            {
                if (lap == null)
                {
                    ConnectRelation.TargetID = "";
                }
                else
                {
                    ConnectRelation.TargetID = lap.ParentConnectControl.ObjectID;
                }
            }
        }

        public void setRelation(IDesignRelation ir,ConnectPointType pt)
        {
            PointType = pt;
            _ConnectRelation = ir;
        }

        public void EnablePoint()
        {
            //DesignConnectPoint.PointerEntered += new PointerEventHandler(DesignConnectPoint_PointerEntered);
            //DesignConnectPoint.PointerExited += new PointerEventHandler(DesignConnectPoint_PointerExited);
            DesignConnectPoint.PointerPressed += new PointerEventHandler(DesignConnectPoint_PointerPressed);
            //DesignConnectPoint.PointerReleased += new PointerEventHandler(DesignConnectPoint_PointerReleased);
            //DesignConnectPoint.PointerMoved += new PointerEventHandler(DesignConnectPoint_PointerMoved);
        }
 
        void DesignConnectPoint_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point p=e.GetCurrentPoint(ConnectRelation.designCanvas.getCanvas()).Position;
            double dx, dy;
            dx = p.X + 3 - Canvas.GetLeft(this);
            dy = p.Y + 3 - Canvas.GetTop(this);
            if (PointType == ConnectPointType.BeginPoint)
            {
                ConnectRelation.MoveRelationStartPoint(dx, dy);
            }
            if (PointType == ConnectPointType.EndPoint)
            {
                ConnectRelation.MoveRelationEndPoint(dx,dy);
            }
            Canvas.SetLeft(this, p.X + 3);//把鼠标直接移出本对象的范围
            Canvas.SetTop(this, p.Y + 3);
            ConnectRelation.designCanvas.BeginRelationDesign(p, this);
        }

        public void DisablePoint()
        {
            //DesignConnectPoint.PointerEntered -= new PointerEventHandler(DesignConnectPoint_PointerEntered);
            //DesignConnectPoint.PointerExited -= new PointerEventHandler(DesignConnectPoint_PointerExited);
            DesignConnectPoint.PointerPressed -= new PointerEventHandler(DesignConnectPoint_PointerPressed);
            //DesignConnectPoint.PointerReleased -= new PointerEventHandler(DesignConnectPoint_PointerReleased);
            //DesignConnectPoint.PointerMoved -= new PointerEventHandler(DesignConnectPoint_PointerMoved);

        }

        

    }
    public enum ConnectPointType
    {
        BeginPoint,EndPoint
    }
}
