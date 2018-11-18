using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SilverlightLFC.common;
using SilverlightLynxControls.LogicView;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;
using Windows.UI.Xaml.Input;

namespace SilverlightLynxControls.Design
{
    public partial class LynxAnchorPoint : UserControl,IDesignAnchorPoint//具体的可以被包含的锚点
    {//锚点只有激活的时候会挂载事件和数据，这个由Node决定。绘制结束后，仅仅保持关联。
        public LynxAnchorPoint(IDesignNode idn)//直接在设计节点对象里面
        {
            InitializeComponent();
            InitBrush();
            ParentConnectControl = idn;
        }
        public LynxAnchorPoint()
        {
            InitializeComponent(); 
            InitBrush();

        }

        void InitBrush()
        {
            AnchorPointBorderBrush = new SolidColorBrush(Colors.Blue);
            AnchorPointBackBrush = new SolidColorBrush(Colors.LightGray);
            AnchorPointHoverBrush = new SolidColorBrush(Colors.Gray);
            AnchorPointActiveBrush = new SolidColorBrush(Colors.Orange);
            AnchorPoint.Background = AnchorPointBackBrush;
            AnchorPoint.BorderBrush = AnchorPointBorderBrush;

        }

        public IDesignNode ParentConnectControl { get; set; }//隶属的链接对象
        public Brush AnchorPointBorderBrush { get; set; }
        public Brush AnchorPointBackBrush { get; set; }
        public Brush AnchorPointHoverBrush { get; set; }
        public Brush AnchorPointActiveBrush { get; set; }

        IDesignCanvas designCanvas
        {
            get
            {
                return ParentConnectControl.designCanvas;
            }
        }

        List<IDesignRelation> _OutRelationList = new List<IDesignRelation>();//所有起始端被锚固的线集合
        List<IDesignRelation> _InRelationList = new List<IDesignRelation>();//所有终点端被锚固的集合;

        public List<IDesignRelation> OutRelationList { get { return _OutRelationList; } }
        public List<IDesignRelation> InRelationList { get { return _InRelationList; } }

        public bool Enable = true;

        private void AnchorPoint_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnabled)
            {
                AnchorPoint.Background = AnchorPointHoverBrush;
                designCanvas.EndAnchor = this;
            }
        }

        private void AnchorPoint_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnabled)
            {
                designCanvas.EndAnchor = null;
                AnchorPoint.Background = AnchorPointBackBrush;
            }
        }
        public Point StartPoint=new Point();
        
        private void AnchorPoint_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (designCanvas == null) { return; }
            if (IsEnabled)
            {
                designCanvas.EndAnchor = null;//代表新的开始
                designCanvas.StartAnchor = this;
                AnchorPoint.Background = AnchorPointBackBrush;
                designCanvas.EnableDesignLine(e.GetCurrentPoint(designCanvas.getCanvas()).Position);
                AnchorPoint.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        private void AnchorPoint_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            
            if (designCanvas == null) { return; }
            if (IsEnabled)
            {
                designCanvas.EndAnchor = null;//代表新的开始
                designCanvas.StartAnchor = this;
                AnchorPoint.Background = AnchorPointBackBrush;
                designCanvas.EnableDesignLine(e.GetCurrentPoint(designCanvas.getCanvas()).Position);
                AnchorPoint.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        public void MoveLines(double dx,double dy)//移动和锚点链接的连接线
        {
            foreach (IDesignRelation ll in OutRelationList)
            {
                ll.MoveRelationStartPoint(dx, dy);
            }
            foreach (IDesignRelation ll in InRelationList)
            {
                ll.MoveRelationEndPoint(dx, dy);
            }
        }

        public string _Key = "Undefine";
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }
       
        public void AddOutRelation(IDesignRelation or)
        {
            if (!OutRelationList.Contains(or))
            {
                OutRelationList.Add(or);
            }
        }

        public void AddInRelation(IDesignRelation ir)
        {
            if (!InRelationList.Contains(ir))
            {
                InRelationList.Add(ir);
            }
        }

        public void RemoveRelation(IDesignRelation dr)
        {
            if (OutRelationList.Contains(dr))
            { OutRelationList.Remove(dr); }
            if (InRelationList.Contains(dr))
            { InRelationList.Remove(dr); }
        }

        private void AnchorPoint_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsEnabled)
            {
                AnchorPoint.Background = AnchorPointHoverBrush;
                designCanvas.EndAnchor = this;
            }
        }

        Point? _ParentControlPoint = null;//因为结构不允许为null，所以使用？
        public Point ParentControlPoint//锚点相对设计控件的位置
        {
            get
            {
                if (_ParentControlPoint == null)
                {
                    double x=0, y=0;

                    x = x + Canvas.GetLeft(this);
                    y = y + Canvas.GetTop(this);
                    x = x + Width / 2;
                    y = y + Height / 2;
                    Point p = new Point(x, y);
                    _ParentControlPoint = p;
                }
                return _ParentControlPoint.Value;
            }
            set { _ParentControlPoint = value; }
        }

        public Point getDesignCanvasPoint()//获取该锚点相对设计画布的位置
        {
            Double x, y;
            x = Canvas.GetLeft(ParentConnectControl.getControl());
            y = Canvas.GetTop(ParentConnectControl.getControl());
            x = x + ParentControlPoint.X;
            y = y + ParentControlPoint.Y;
            return new Point(x, y);

        }
    }
}
