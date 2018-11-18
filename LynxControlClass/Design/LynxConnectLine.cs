using System;
using System.Net;
using System.Windows;
using System.Reflection;
using System.Windows.Input;
using SilverlightLynxControls.LogicView;
using SilverlightLFC.common;
using SilverlightLynxControls.ObjectMonitorUI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Windows.UI;

namespace SilverlightLynxControls.Design
{
    public class LynxConnectLine : LynxDesignObject, IDesignRelation, IObjectMonitor//包含一个数据对象，可以被记录的
    {
        #region Constructor
        public LynxConnectLine()
        {
            Canvas.SetZIndex(this, 7);
            DesignObjectActiveBrush = new SolidColorBrush(Colors.Orange);
            DesignObjectBackBrush = new SolidColorBrush(Colors.Blue);
            DesignObjectBorderBrush = null;
            DesignObjectHoverBrush = new SolidColorBrush(Colors.Green);

            StartConnectPoint.setRelation( this,ConnectPointType.BeginPoint);
            EndConnectPoint.setRelation(this, ConnectPointType.EndPoint);
            StartConnectPoint.EnablePoint();
            EndConnectPoint.EnablePoint();
        }

        #endregion

        #region Fields
        //string _Name = "UnNamed";//被显示在一个TextBlock里面
        //string _Memo = "Undefined Connection";
        Point _StartPoint;
        Point _EndPoint;
        string _LineType = "Line";

        public EllipseGeometry StartPointGeo;
        public EllipseGeometry EndPointGeo;
        public virtual string TargetID
        { get; set; }
        public virtual string SourceID { get; set; }

        #region UIElement
        public Line MainLine=new Line();

        Line LeftArrowLine = new Line();
        Line RightArrowLine = new Line();

        TextBlock tb = new TextBlock();

        double MainLineWidth = 5;
        double ArrowLineLength = 15;
        double ArrowLineWidth = 3;
        Color ForeColor = Colors.Black;

        LynxConnectPoint StartConnectPoint=new LynxConnectPoint();//起始的设计点
        LynxConnectPoint EndConnectPoint = new LynxConnectPoint();//终点设计点
        #endregion

        public IDesignAnchorPoint StartAnchorPoint { get; set; }
        public IDesignAnchorPoint EndAnchorPoint { get; set; }
        #endregion

        public override void ClearAllEvent()
        {
            base.ClearAllEvent();
            DisableActive();
            DisableMonitor();

        }//需要子类实现

        public override void Active()
        {
            MainLine.Stroke = DesignObjectActiveBrush;
            LeftArrowLine.Stroke = DesignObjectActiveBrush;
            RightArrowLine.Stroke = DesignObjectActiveBrush;
            tb.Foreground = DesignObjectActiveBrush;
            Canvas.SetZIndex(MainLine, Canvas.GetZIndex(this) + 1);
            Canvas.SetZIndex(LeftArrowLine, Canvas.GetZIndex(this) + 1);
            Canvas.SetZIndex(RightArrowLine, Canvas.GetZIndex(this) + 1);
            Canvas.SetZIndex(tb, Canvas.GetZIndex(this) + 1);
            Canvas c = designCanvas.getCanvas();
            designCanvas.CurrentActiveDesignObject = this;
        }

        public override void DeActive()
        {
            MainLine.Stroke = DesignObjectBackBrush;
            LeftArrowLine.Stroke = DesignObjectBackBrush;
            RightArrowLine.Stroke = DesignObjectBackBrush;
            tb.Foreground = DesignObjectBackBrush;
            Canvas.SetZIndex(MainLine, Canvas.GetZIndex(this));
            Canvas.SetZIndex(LeftArrowLine, Canvas.GetZIndex(this));
            Canvas.SetZIndex(RightArrowLine, Canvas.GetZIndex(this));
            Canvas.SetZIndex(tb, Canvas.GetZIndex(this));
            Canvas c = designCanvas.getCanvas();
            designCanvas.CurrentActiveDesignObject = this;
        }

        public void EnableActive()
        {
            MainLine.PointerEntered += new PointerEventHandler(MainLine_PointerEntered);
            MainLine.PointerExited += new PointerEventHandler(MainLine_PointerExited);
            MainLine.PointerPressed += new PointerEventHandler(MainLine_PointerPressed);
        }

        void MainLine_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            designCanvas.CurrentActiveDesignObject = this;
        }

        public void DisableActive()
        {
            MainLine.PointerEntered -= new PointerEventHandler(MainLine_PointerEntered);
            MainLine.PointerExited -= new PointerEventHandler(MainLine_PointerExited);
            MainLine.PointerPressed -= new PointerEventHandler(MainLine_PointerPressed);
        }

        private void MainLine_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnabled)
            {
                Active();
            }
        }

        private void MainLine_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnabled)
            {
                DeActive();
            }
        }


        public void DrawRelationLine() { DrawRelationLine(StartPoint, EndPoint); }

        public void RemoveRelationLine() 
        {
            if (designCanvas.getCanvas().Children.Contains(MainLine)) { designCanvas.getCanvas().Children.Remove(MainLine); }
            if (designCanvas.getCanvas().Children.Contains(tb)) { designCanvas.getCanvas().Children.Remove(tb); }
            if (designCanvas.getCanvas().Children.Contains(LeftArrowLine)) { designCanvas.getCanvas().Children.Remove(LeftArrowLine); }
            if (designCanvas.getCanvas().Children.Contains(RightArrowLine)) { designCanvas.getCanvas().Children.Remove(RightArrowLine); }
            if (designCanvas.getCanvas().Children.Contains(StartConnectPoint)) { designCanvas.getCanvas().Children.Remove(StartConnectPoint); }
            if (designCanvas.getCanvas().Children.Contains(EndConnectPoint)) { designCanvas.getCanvas().Children.Remove(EndConnectPoint); }
            //tb.PointerPressed -= new PointerEventHandler(tb_PointerPressed);
            //if (designCanvas.DesignObjectList.Contains(this)) { designCanvas.DesignObjectList.Remove(this); }
            if (StartAnchorPoint != null && StartAnchorPoint.InRelationList.Contains(this))
            {
                StartAnchorPoint.InRelationList.Remove(this);
            }
            if (EndAnchorPoint != null && EndAnchorPoint.OutRelationList.Contains(this))
            {
                EndAnchorPoint.OutRelationList.Remove(this);
            }
            DisableActive();
        }

        public void DrawRelationLine(Point sp, Point ep)//从特定的起始点到终点绘制关联
        {//必须把自己作为Tag附加到MainLine上面
            StartPoint = sp;
            EndPoint = ep;

            if (MainLine == null) { MainLine = new Line(); }
            if (designCanvas.getCanvas().Children.Contains(MainLine)) { }
            else
            {
                designCanvas.getCanvas().Children.Add(MainLine);
            }
            MainLine.Stroke = new SolidColorBrush(ForeColor);
            MainLine.StrokeThickness = MainLineWidth;
            MainLine.X1 = StartPoint.X;
            MainLine.X2 = EndPoint.X;
            MainLine.Y1 = StartPoint.Y;
            MainLine.Y2 = EndPoint.Y;
            MainLine.Tag = this;//必须的，把自己附加在主线上面，这也是从界面层识别的时候必要的步骤
            Canvas.SetZIndex(MainLine, Canvas.GetZIndex(this));

            double tx, ty;
            tx = (StartPoint.X + EndPoint.X) / 2;
            ty = (StartPoint.Y + EndPoint.Y) / 2;

            if (designCanvas.getCanvas().Children.Contains(tb)) { }
            else
            {
                designCanvas.getCanvas().Children.Add(tb);
            }

            
            ToolTipService.SetToolTip(this, ControlMemo);
            tb.Text = ControlName;
            //tb.PointerPressed += new PointerEventHandler(tb_PointerPressed);
            Canvas.SetLeft(tb, tx);
            Canvas.SetTop(tb, ty);
            Canvas.SetZIndex(tb, Canvas.GetZIndex(this));

            DrawArrow(MainLine);
            MainLine.Tag = this;
            EnableActive();

            if (designCanvas.getCanvas().Children.Contains(StartConnectPoint)) { }
            else
            {
                designCanvas.getCanvas().Children.Add(StartConnectPoint);
                Canvas.SetLeft(StartConnectPoint, StartPoint.X-StartConnectPoint.Width/2);
                Canvas.SetTop(StartConnectPoint, StartPoint.Y-StartConnectPoint.Height/2);
                Canvas.SetZIndex(StartConnectPoint, Canvas.GetZIndex(this) + 1);
            }
            if (designCanvas.getCanvas().Children.Contains(EndConnectPoint)) { }
            else
            {
                designCanvas.getCanvas().Children.Add(EndConnectPoint);
                Canvas.SetLeft(EndConnectPoint, EndPoint.X - EndConnectPoint.Width/2);
                Canvas.SetTop(EndConnectPoint, EndPoint.Y - EndConnectPoint.Height/2);
                Canvas.SetZIndex(EndConnectPoint, Canvas.GetZIndex(this) + 1);
            }
        }

        void MoveArrowLine()//依据新的位置转动箭头
        {
            double a1, a2;
            double a = getArc(MainLine);
            a1 = a + Math.PI*3 / 4;
            a2 = a - Math.PI*3 / 4;

            double ex1, ey1, ex2, ey2;
            ex1 = EndPoint.X + ArrowLineLength * Math.Cos(a1);
            ey1 = EndPoint.Y - ArrowLineLength * Math.Sin(a1);
            ex2 = EndPoint.X + ArrowLineLength * Math.Cos(a2);
            ey2 = EndPoint.Y - ArrowLineLength * Math.Sin(a2);

            LeftArrowLine.X1 = EndPoint.X;
            LeftArrowLine.X2 = ex1;
            LeftArrowLine.Y1 = EndPoint.Y;
            LeftArrowLine.Y2 = ey1;

            RightArrowLine.X1 = EndPoint.X;
            RightArrowLine.X2 = ex2;
            RightArrowLine.Y1 = EndPoint.Y;
            RightArrowLine.Y2 = ey2;
        }

        public void MoveRelationStartPoint(double dx, double dy)
        {
            _StartPoint.X = StartPoint.X + dx;
            _StartPoint.Y = StartPoint.Y + dy;
            MainLine.X1 = _StartPoint.X;
            MainLine.Y1 = _StartPoint.Y;
            Canvas.SetLeft(tb, Canvas.GetLeft(tb)+dx/2);
            Canvas.SetTop(tb, Canvas.GetTop(tb) + dy / 2);
            MoveArrowLine();
            Canvas.SetLeft(StartConnectPoint, Canvas.GetLeft(StartConnectPoint) + dx);
            Canvas.SetTop(StartConnectPoint, Canvas.GetTop(StartConnectPoint) + dy);
            //DrawLine(StartPoint, EndPoint);
        }
        public void MoveRelationEndPoint(double dx, double dy)
        {
            _EndPoint.X = EndPoint.X + dx;
            _EndPoint.Y = EndPoint.Y + dy;
            MainLine.X2 = _EndPoint.X;
            MainLine.Y2 = _EndPoint.Y;
            Canvas.SetLeft(tb, Canvas.GetLeft(tb) + dx / 2);
            Canvas.SetTop(tb, Canvas.GetTop(tb) + dy / 2);
            MoveArrowLine();
            Canvas.SetLeft(EndConnectPoint, Canvas.GetLeft(EndConnectPoint) + dx);
            Canvas.SetTop(EndConnectPoint, Canvas.GetTop(EndConnectPoint) + dy);
        }
        public void MoveRelationStartPoint(Point nsp)
        {
            double dx, dy;
            dx = nsp.X - StartPoint.X;
            dy = nsp.Y = StartPoint.Y;
            MoveRelationStartPoint(dx, dy);
        }
        public void MoveRelationEndPoint(Point nep)
        {
            double dx, dy;
            dx = nep.X - EndPoint.X;
            dy = nep.Y = EndPoint.Y;
            MoveRelationStartPoint(dx, dy);
        }

        double getArc(Line l)//获得一个直线的倾角
        {
            if (l.Y1 == l.Y2) { return 0; }
            if (l.X2 == l.X1) { return Math.PI / 2; }
            double tg = -(l.Y2 - l.Y1) / (l.X2 - l.X1);
            double a = Math.Atan(tg);
            if (l.X2 >= l.X1) { }
            else
            {
                a = a + Math.PI;
            }
            return a;
        }

        void DrawArrow(Line l)
        {
            double LineArc = getArc(l);
            DrawLineCap(new Point(l.X2, l.Y2), LineArc);
        }

        void DrawLineCap(Point StartPoint, double LineArc)//从某点开始以约定的长度和角度绘制箭头
        {
            double a1, a2;

            a1 = LineArc + Math.PI  *0.75;
            a2 = LineArc - Math.PI  *0.75;

            double ex1, ey1, ex2, ey2;
            ex1 = EndPoint.X + ArrowLineLength * Math.Cos(a1);
            ey1 = EndPoint.Y - ArrowLineLength * Math.Sin(a1);
            ex2 = EndPoint.X + ArrowLineLength * Math.Cos(a2);
            ey2 = EndPoint.Y - ArrowLineLength * Math.Sin(a2);

            if (LeftArrowLine == null)
            {
                LeftArrowLine = new Line();
            }
            if (RightArrowLine == null)
            {
                RightArrowLine = new Line();
            }

            LeftArrowLine.StrokeThickness = ArrowLineWidth;
            LeftArrowLine.Stroke = new SolidColorBrush(ForeColor);

            RightArrowLine.StrokeThickness = ArrowLineWidth;
            RightArrowLine.Stroke = new SolidColorBrush(ForeColor);

            LeftArrowLine.X1 = EndPoint.X;
            LeftArrowLine.X2 = ex1;
            LeftArrowLine.Y1 = EndPoint.Y;
            LeftArrowLine.Y2 = ey1;

            RightArrowLine.X1 = EndPoint.X;
            RightArrowLine.X2 = ex2;
            RightArrowLine.Y1 = EndPoint.Y;
            RightArrowLine.Y2 = ey2;

            if (!designCanvas.getCanvas().Children.Contains(LeftArrowLine))
            {
                designCanvas.getCanvas().Children.Add(LeftArrowLine);
            }
            if (!designCanvas.getCanvas().Children.Contains(RightArrowLine))
            {
                designCanvas.getCanvas().Children.Add(RightArrowLine);
            }
            Canvas.SetZIndex(LeftArrowLine, Canvas.GetZIndex(this));
            Canvas.SetZIndex(RightArrowLine, Canvas.GetZIndex(this));
        }

        public override void LoadLogicViewInfor()
        {
            if (LogicViewObject == null) { return; }
            TargetID = LogicViewObject.TargetNodeID;
            SourceID = LogicViewObject.SourceNodeID;
            StartPoint = LogicViewObject.SourcePoint;
            EndPoint = LogicViewObject.TargetPoint;
            //Name = LogicViewObject.Name;
            //Memo = LogicViewObject.Memo;
            MainLineWidth = LogicViewObject.Width;
            ForeColor = LogicViewObject.LineColor;
            LineType=LogicViewObject.LineTypeName;
            ReadFromObject(LogicViewObject.DataObjectID);
        }

        public override void SaveLogicViewInfor()
        {
            if (LogicViewObject == null) { LogicViewObject=new ViewLineItem(); }
            LogicViewObject.SourcePoint = StartPoint;
            LogicViewObject.TargetPoint = EndPoint;
            //LogicViewObject.Name = Name;
            //LogicViewObject.Memo = Memo;
            LogicViewObject.Width = MainLineWidth;
            LogicViewObject.ControlType = this.GetType().FullName;
            LogicViewObject.LineTypeName = LineType;
            LogicViewObject.LineColor =ForeColor;
            //vl._xt_RelationID = DataObject.ObjectID;
            LogicViewObject.TargetNodeID = TargetID;
            LogicViewObject.SourceNodeID = SourceID;
            LogicViewObject.DataObjectID = DataObject.ObjectID;
            LogicViewObject.DataObjectType = DataObject.GetType().FullName;
        }
        //public virtual void ReadFromObject(string id) { }//需要子类实现的方法,从ID恢复对象数据
        public override void ReadFromObject()
        {
            if (DataObject == null) { return; }
            DisableMonitor();
            if (DataObject.GetType().GetRuntimeProperty("Name") != null)
            {
                tb.Text = DataObject.GetType().GetRuntimeProperty("Name").GetValue(DataObject, null).ToString();
                ControlName = DataObject.GetType().GetRuntimeProperty("Name").GetValue(DataObject, null).ToString();
            }

            if (DataObject.GetType().GetRuntimeProperty("Memo") != null)
            {
                ToolTipService.SetToolTip(this, DataObject.GetType().GetRuntimeProperty("Memo").GetValue(DataObject, null).ToString());
                ControlMemo = DataObject.GetType().GetRuntimeProperty("Memo").GetValue(DataObject, null).ToString();
            }
            EnableMonitor();
            //DataObject.InObject=this.
        }

        public override void WriteToObject()
        {
            if (DataObject == null) { return; }
            if (DataObject.GetType().GetRuntimeProperty("Name") != null)
            {
                DataObject.GetType().GetRuntimeProperty("Name").SetValue(DataObject, tb.Text, null);
            }
        }

        #region Properties
        public ViewLineItem LogicViewObject
        {
            get;
            set;
        }
        public new string Name
        {
            get
            {
                return tb.Text;
            }
            set
            {
                tb.Text = value;
            }
        }
        //public string Memo
        //{
        //    get
        //    {
        //        return _Memo;
        //    }
        //    set
        //    {
        //        _Memo = value;
        //    }
        //}

        //AbstractLFCDataObject _DataObject = null;
        //void DataObject_ObjctChanged(object sender, LFCObjectChangedArgs e)
        //{
        //    ReadFromObject();
        //}
        //public virtual AbstractLFCDataObject DataObject
        //{
        //    get { return _DataObject; }
        //    set
        //    {
        //        if ((value is AbstractLFCDataObject) && (value != _DataObject))
        //        {
        //            if (_DataObject != null)
        //            {
        //                _DataObject.ObjctChanged -= new LFCObjectChanged(DataObject_ObjctChanged);
        //            }
        //            _DataObject = value;
        //            _DataObject.ObjctChanged += new LFCObjectChanged(DataObject_ObjctChanged);
        //        }
        //    }
        //}
        public string LineType
        {
            get
            {
                return _LineType;
            }
            set
            {
                _LineType = value;
            }
        }
        public Point StartPoint
        {
            get
            {
                return _StartPoint;
            }
            set
            {
                _StartPoint = value;
            }
        }
        public Point EndPoint
        {
            get
            {
                return _EndPoint;
            }
            set
            {
                _EndPoint = value;
            }
        }
        #endregion

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
