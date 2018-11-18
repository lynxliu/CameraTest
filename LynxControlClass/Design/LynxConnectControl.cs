using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using SilverlightLynxControls.LogicView;
using SilverlightLFC.common;
using SilverlightLynxControls.ObjectMonitorUI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Input;

namespace SilverlightLynxControls.Design
{
    public class LynxConnectControl : LynxDesignObject, IDesignNode, IObjectMonitor//可连接图标单元
    {//不可以改变大小，但是可以链接，支持拖放移动
        public LynxConnectControl()
        {
            ToolTipService.SetToolTip(this, ControlMemo);
            Canvas.SetZIndex(this, 3);//默认是3的zindex
            if ((ControlMemo == "") || (ControlMemo == null)) { ControlMemo = "设计元素"; }
            DesignObjectBorderBrush = new SolidColorBrush(Colors.Orange);

            EnableActive();
        }
 
        double getDistance(Point p0,Point p1)//p是相对于设计画布的位置，本函数求其和该锚点的中心点的距离
        {
            double d = -1;
            d = ((p0.X - p1.X) * (p0.X - p1.X) + (p0.Y - p1.Y) * (p0.Y - p1.Y));
            return Math.Sqrt(d);
        }
        public FrameworkElement ActiveArea;//可激活区域
        public FrameworkElement MoveArea;//移动区域

        public List<LynxConnectControl> prevControlList=new List<LynxConnectControl>();
        public List<LynxConnectControl> nextControlList = new List<LynxConnectControl>();
        public List<LynxArrowLine> InLine = new List<LynxArrowLine>();
        public List<LynxArrowLine> OutLine = new List<LynxArrowLine>();

        bool IsMove = false;
        bool isMouseCaptured = false;
        double mouseVerticalPosition, mouseHorizontalPosition;

        public override void ClearAllEvent()
        {
            base.ClearAllEvent();
            DisableActive();
            DisableControlMove();
            DisableMonitor();
        }//需要子类实现

        #region Control Move//和控件移动相关的事件
        public void DisableControlMove()//禁止控件移动
        {
            if (MoveArea != null)
            {
                MoveArea.PointerPressed -= new PointerEventHandler(mv_PointerPressed);
                MoveArea.PointerMoved -= new PointerEventHandler(mv_PointerMoved);
                MoveArea.PointerReleased -= new PointerEventHandler(mv_PointerReleased);
                MoveArea.PointerEntered -= new PointerEventHandler(mv_PointerEntered);
                MoveArea.PointerExited -= new PointerEventHandler(mv_PointerExited);
            }
        }
        public void EnableControlMove()//允许控件移动
        {
            if (MoveArea != null)
            {
                MoveArea.PointerPressed += new PointerEventHandler(mv_PointerPressed);
                MoveArea.PointerMoved += new PointerEventHandler(mv_PointerMoved);
                MoveArea.PointerReleased += new PointerEventHandler(mv_PointerReleased);
                MoveArea.PointerEntered += new PointerEventHandler(mv_PointerEntered);
                MoveArea.PointerExited += new PointerEventHandler(mv_PointerExited);
            }
        }
        void mv_PointerExited(object sender, PointerRoutedEventArgs e)
        {

        }
        void mv_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }
        void mv_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            IsMove = false;
            isMouseCaptured = false;

        }
        void mv_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!isMouseCaptured) { return; }

            if (IsMove)
            {
                Panel p = designCanvas.getCanvas();

                double deltaV = e.GetCurrentPoint(p).Position.Y - mouseVerticalPosition;
                double startTop = Convert.ToDouble(this.GetValue(Canvas.TopProperty));
                double newTop = deltaV + startTop;
                this.SetValue(Canvas.TopProperty, newTop);

                


                double deltaH = e.GetCurrentPoint(p).Position.X - mouseHorizontalPosition;
                double startLeft = Convert.ToDouble(this.GetValue(Canvas.LeftProperty));
                double newLeft = deltaH + startLeft;
                this.SetValue(Canvas.LeftProperty, newLeft);

                foreach (IDesignAnchorPoint lp in AnchorPointList)
                {
                    lp.MoveLines(deltaH, deltaV);
                }
                mouseHorizontalPosition = mouseHorizontalPosition + deltaH;
                mouseVerticalPosition = mouseVerticalPosition + deltaV;
            }
        }
        void mv_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            IsMove = true;
            isMouseCaptured = true;

            ActionActive.Active(designCanvas.getCanvas());
            mouseVerticalPosition = e.GetCurrentPoint(designCanvas.getCanvas()).Position.Y;
            mouseHorizontalPosition = e.GetCurrentPoint(designCanvas.getCanvas()).Position.X;
        }
        #endregion
        
        #region ControlActive//和控件激活相关
        void ActiveArea_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            designCanvas.DeActiveAll();
            Canvas.SetZIndex(this, 5);//设计元素激活，都激活到zindex5的层次
            designCanvas.CurrentActiveDesignObject = this;//当前控件
        }

        public void EnableActive()
        {
            if (ActiveArea != null)
            {
                ActiveArea.PointerEntered += new PointerEventHandler(ActiveArea_PointerEntered);
                ActiveArea.PointerExited += new PointerEventHandler(ActiveArea_PointerExited);
                ActiveArea.PointerPressed += new PointerEventHandler(ActiveArea_PointerPressed);
            }
        }

        void ActiveArea_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            
            BorderBrush = null;
            Background = DesignObjectHoverBrush;
            designCanvas.CurrentHoverDesignObject = null;
        }

        void ActiveArea_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            BorderBrush = DesignObjectBorderBrush;
            Background = DesignObjectBackBrush;
            designCanvas.CurrentHoverDesignObject = this;
        }

        public void DisableActive()
        {
            if (ActiveArea != null)
            {
                ActiveArea.PointerEntered -= new PointerEventHandler(ActiveArea_PointerEntered);
                ActiveArea.PointerExited -= new PointerEventHandler(ActiveArea_PointerExited);
                ActiveArea.PointerPressed -= new PointerEventHandler(ActiveArea_PointerPressed);
            }
        }

        public override void Active()
        {
            BorderBrush = DesignObjectBorderBrush;
            Background = DesignObjectActiveBrush;
            if (designCanvas == null) { return; }
            if (designCanvas.CurrentActiveDesignObject != null) { designCanvas.CurrentActiveDesignObject.DeActive(); }
            designCanvas.CurrentActiveDesignObject = this;//当前控件
        }

        public override void DeActive()
        {
            BorderBrush = DesignObjectBorderBrush;
            Background = DesignObjectBackBrush;
            if (designCanvas == null) { return; }
            designCanvas.CurrentActiveDesignObject = null;//当前控件
        }
        #endregion
 
        List<IDesignAnchorPoint> _AnchorPointList = new List<IDesignAnchorPoint>();
        public List<IDesignAnchorPoint> AnchorPointList
        {
            get { return _AnchorPointList; }
        }
        public IDesignAnchorPoint getAnchorpointByKey(string n)
        {
            foreach (IDesignAnchorPoint a in AnchorPointList)
            {
                if (a.Key == n)
                {
                    return a;
                }
            }
            return null;
        }

        //public virtual void ReadFromObject(string id) { }//需要子类实现的方法,从ID恢复对象数据
        public override void ReadFromObject()
        {
            DataContext = DataObject;//全部使用数据绑定
            //if (DataObject == null) { return; }
            //DisableMonitor();
            //System.Reflection.PropertyInfo pi=DataObject.GetType().GetProperty("Name");
            //if ( pi!= null)
            //{
            //    ControlName = pi.GetValue(DataObject, null).ToString();
            //}
            //pi = DataObject.GetType().GetProperty("Memo");
            //if (pi != null)
            //{
            //    ToolTipService.SetToolTip(this, pi.GetValue(DataObject, null).ToString());
            //    ControlMemo = pi.GetValue(DataObject, null).ToString();
            //}
            //EnableMonitor();
        }
        //public virtual void WriteToObject(){}

        #region LogicViewIO//把设计时刻的数据保存下来，或者加载

        public ViewItem LogicViewObject
        {
            get;
            set;
        }

        public override void LoadLogicViewInfor()
        {
            if (LogicViewObject == null) { return; }
            this.Height = LogicViewObject.Height;
            this.Width = LogicViewObject.Width;
            Canvas.SetLeft(this, LogicViewObject.Left);
            Canvas.SetTop(this, LogicViewObject.Top);

            if ((LogicViewObject.DataObjectID != null) && (LogicViewObject.DataObjectID != ""))
            {
                ReadFromObject(LogicViewObject.DataObjectID);
            }
        }
        public override void SaveLogicViewInfor()
        {
            if (LogicViewObject == null) { LogicViewObject=new ViewItem(); }
            LogicViewObject.ControlType = this.GetType().FullName;
            //LogicViewObject.Name = Name;
            //LogicViewObject.Memo = Memo;
            LogicViewObject.Height = this.Height;
            LogicViewObject.Left = Canvas.GetLeft(this);
            LogicViewObject.Top = Canvas.GetTop(this);
            LogicViewObject.Width = this.Width;
            if (DataObject != null)
            {
                LogicViewObject.DataObjectID = DataObject.ObjectID;
                LogicViewObject.DataObjectType = DataObject.GetType().FullName;
            }

        }
        #endregion

        public string LineType
        {
            get;
            set;
        }

        public IDesignAnchorPoint getNearAnchorPoint(Point DiagramPosition)
        {
            if (this.AnchorPointList.Count == 0)
            {
                return null;
            }
            double min = getDistance(DiagramPosition, getAnchorPointCenter(AnchorPointList[0]));
            IDesignAnchorPoint Anchor = AnchorPointList[0];

            for (int i = 1; i < AnchorPointList.Count; i++)
            {
                IDesignAnchorPoint dap = AnchorPointList[i];
                double td = getDistance(DiagramPosition, getAnchorPointCenter(dap));
                if (td < min)
                {
                    min = td;
                    Anchor = dap;
                }
            }
            return Anchor;
        }
        public IDesignAnchorPoint getDefaultAnchorPoint()
        {
            if (AnchorPointList.Count > 0)
            {
                return AnchorPointList[0];
            }
            return null;
        }

        Point getAnchorPointCenter(IDesignAnchorPoint ap)
        {
            double x = Canvas.GetLeft(this) + Canvas.GetLeft(ap.ParentConnectControl.getControl()) + ap.ParentConnectControl.getControl().Width / 2;
            double y = Canvas.GetTop(this) + Canvas.GetTop(ap.ParentConnectControl.getControl()) + ap.ParentConnectControl.getControl().Height / 2;
            return new Point(x, y);
        }

        public void AddRelationAsSourceObject(IDesignRelation IRelation)
        {
            IDesignAnchorPoint Anchor = getNearAnchorPoint(IRelation.StartPoint);
            if (Anchor == null) { return; }
            Anchor.AddOutRelation(IRelation);
        }

        public void AddRelationAsTargetObject(IDesignRelation IRelation)
        {
            IDesignAnchorPoint Anchor = getNearAnchorPoint(IRelation.EndPoint);
            if (Anchor == null) { return; }
            Anchor.AddInRelation(IRelation);
        }

        public void DeleteRelation(IDesignRelation IRelation)//删除任何指定的关联
        {
            for (int i = 0; i < AnchorPointList.Count; i++)
            {
                IDesignAnchorPoint dap = AnchorPointList[i];
                dap.RemoveRelation(IRelation);
            }
        }

        //public virtual IDesignRelation getRelation() { return null; }



        bool IsEnableMonitor = false;
        public void EnableMonitor()
        {
            if (!IsEnableMonitor)
            {
                DataObject.ObjctChanged += new SilverlightLFC.common.LFCObjectChanged(DataObject_ObjctChanged);
            }
            IsEnableMonitor = true;
        }

        void DataObject_ObjctChanged(object sender, SilverlightLFC.common.LFCObjectChangedArgs e)
        {
            ReadFromObject();
        }

        public void DisableMonitor()
        {
            if (IsEnableMonitor)
            {
                DataObject.ObjctChanged -= new SilverlightLFC.common.LFCObjectChanged(DataObject_ObjctChanged);
            }
            IsEnableMonitor = false;
        }
    }
}
