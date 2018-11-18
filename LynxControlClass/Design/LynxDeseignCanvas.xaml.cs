using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls.Design;
using SilverlightLynxControls.LogicView;
using SilverlightLFC.common;
using SilverlightLynxControls.DragDrop;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;

namespace SilverlightLynxControls
{
    public partial class LynxDeseignCanvas : UserControl,IDesignCanvas//该类支持设计类拖放对象
    {//支持点击创建对象，拖放对象和对象的布局和设计

        #region DesignObjectList//包含所有的设计对象，节点和关联，必须保障其和显示控件严格一致
        List<IDesignNode> _NodeList = new List<IDesignNode>();
        List<IDesignRelation> _RelationList = new List<IDesignRelation>();

        public List<IDesignNode> NodeList { get { return _NodeList; } }//保存所有加入的节点对象，只读属性
        public List<IDesignRelation> RelationList { get { return _RelationList; } }//保存所有的关联对象，只读属性
        #endregion

        #region getNewDesignObject//使用委托的方式允许实体画布来依据id实例化设计控件
        public GetNewRelationControl getNewRelationControlByID;//由实际的画布决定如何依据ID和类型新建一个链接对象
        public GetNewNodeControl getNewNodeControlByID;//由实际的画布决定如何依据ID和类型新建一个结点对象
        public GetAllowedRelation getAllowedRelationListByID;//由实际的画布决定如何依据ID和类型找到所有的关联
        public GetAllowedNode getAllowedNodeListByID;//由实际的画布决定如何依据ID和类型找到所有的关联节点对象
        public IsNodeDataObject IsNodeDataObjectByID;//依据ID看是否是节点
        public IsRelationDataObject IsRelationDataObjectByID;//依据画布看是否是关联

        public bool? IsDesignNodeByObjectID(string id)//为null代表根本不存在
        {
            if (IsNodeDataObjectByID == null) { return null; }
            return IsNodeDataObjectByID(id);
        }
        public bool? IsDesignRelationByObjectID(string id)
        {
            if (IsRelationDataObjectByID == null) { return null; }
            return IsRelationDataObjectByID(id);
        }

        public IDesignRelation getNewRelationControl(string dataObjectType, string id)
        {
            if (getNewRelationControlByID == null) { return null; }
            return getNewRelationControlByID(dataObjectType, id);
        }
        public IDesignNode getNewNodeControl(string dataObjectType, string id)
        {
            if (getNewNodeControlByID == null) { return null; }
            return getNewNodeControlByID(dataObjectType, id);
        }

        public List<IDesignRelation> getAllowedRelationListByNodeID(string DataObjectType, string id)//依据ID或者类别获取所有可能的关联
        {
            if (getAllowedRelationListByID == null) { return new List<IDesignRelation>(); }
            return getAllowedRelationListByID(DataObjectType, id);
        }
        public List<IDesignNode> getAllowedNodeListByNodeID(string DataObjectType, string id)//依据ID和类别获取某个某个对象联络的全部节点
        {
            if (getAllowedNodeListByID == null) { return new List<IDesignNode>(); }
            return getAllowedNodeListByID(DataObjectType, id);
        }
        #endregion

        #region RelationDesign
        LynxConnectPoint CurrentConnectPoint { get; set; }//拖动的链接点

        public void BeginRelationDesign(Point p, LynxConnectPoint lp)
        {
            //if (IsBeginRelationDesign == -1) { return; }
            DeseignCanvas.PointerReleased += new PointerEventHandler(DeseignCanvas_RelationDesignPointerReleased);
            DeseignCanvas.PointerMoved += new PointerEventHandler(DeseignCanvas_RelationDesignPointerMoved);
            //IsBeginRelationDesign = 1;
            StartPoint = p;
            CurrentConnectPoint=lp;
        }
        public void EndRelationDesign()
        {
            DeseignCanvas.PointerReleased -= new PointerEventHandler(DeseignCanvas_RelationDesignPointerReleased);
            DeseignCanvas.PointerMoved -= new PointerEventHandler(DeseignCanvas_RelationDesignPointerMoved);
            //IsBeginRelationDesign = -1;
            CurrentConnectPoint = null;
        }
        void DeseignCanvas_RelationDesignPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (CurrentConnectPoint != null)
            {
                IDesignRelation l = CurrentConnectPoint.ConnectRelation;
                if (l == null) { EndRelationDesign(); return; }
                if (EndAnchor != null)//表示拖放链接点，并且进入了锚点区域
                {
                    if (CurrentConnectPoint.PointType == ConnectPointType.BeginPoint)//拖拽的是连接的起始点
                    {
                        l.StartAnchorPoint = EndAnchor;
                        EndAnchor.OutRelationList.Add(l);
                        l.SourceID = EndAnchor.ParentConnectControl.ObjectID;
                    }
                    if (CurrentConnectPoint.PointType == ConnectPointType.EndPoint)
                    {
                        l.EndAnchorPoint = EndAnchor;
                        EndAnchor.InRelationList.Add(l);
                        l.TargetID = EndAnchor.ParentConnectControl.ObjectID;
                    }
                    if (l.DataObject.ObjectID == "" || l.DataObject.ObjectID == null)
                    {
                        l.DataObject.ObjectID = SilverlightLFC.common.Environment.getGUID();
                    }
                }
                else//移除任何的锚点
                {
                    if (CurrentConnectPoint.PointType == ConnectPointType.BeginPoint)//拖拽的是连接的起始点
                    {
                        if (l.StartAnchorPoint != null)
                        {
                            l.StartAnchorPoint.OutRelationList.Remove(l);
                            l.StartAnchorPoint = null;
                        }
                        l.SourceID = null;
                    }
                    if (CurrentConnectPoint.PointType == ConnectPointType.EndPoint)
                    {
                        if (l.EndAnchorPoint != null)
                        {
                            l.EndAnchorPoint.InRelationList.Remove(l);
                            l.EndAnchorPoint = null;
                        }
                        l.TargetID = null;
                    }
                }
            }
            EndRelationDesign();
        }
        void DeseignCanvas_RelationDesignPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (CurrentConnectPoint!=null)
            {
                Point p = e.GetCurrentPoint(DeseignCanvas).Position;
                if (CurrentConnectPoint.PointType == ConnectPointType.BeginPoint)
                {
                    CurrentConnectPoint.ConnectRelation.MoveRelationStartPoint(p.X - StartPoint.Value.X, p.Y - StartPoint.Value.Y);
                }
                if (CurrentConnectPoint.PointType == ConnectPointType.EndPoint)
                {
                    CurrentConnectPoint.ConnectRelation.MoveRelationEndPoint(p.X - StartPoint.Value.X, p.Y - StartPoint.Value.Y);
                }
                
                StartPoint = p;
            }

        }

        #endregion

        #region DragDropDesign//拖放设计
        ActionDrop adp;
        void DataDrop()//放置一个对象，新建其设计控件
        {
            object o = LynxDragDropData.Data;
            IDesignObject ido = null;
            if (o == null) { return; }
            if (o is IDesignObject)//这个时候放置的对象是设计对象
            {
                ido = o as IDesignObject;
            }
            else
            {//放置的是数据对象不是设计对象，需要自己来获取新的设计对象
                if (o is AbstractLFCDataObject)
                {
                    ido = getNewRelationControl(o.GetType().FullName, (o as AbstractLFCDataObject).ObjectID);
                    if (ido == null)
                    {
                        ido = getNewNodeControl(o.GetType().FullName, (o as AbstractLFCDataObject).ObjectID);

                    }
                }
            }
            if (ido != null)
            {
                //ido.designCanvas = this;
                FrameworkElement fe = ido.getControl();
                //DeseignCanvas.Children.Add(fe);
                Canvas.SetLeft(fe, adp.DropPoint.X);
                Canvas.SetTop(fe, adp.DropPoint.Y);
                if (ido is IDesignNode) { AddExistNode((ido as IDesignNode),adp.DropPoint); }
                if (ido is IDesignRelation) { AddExistRelation(ido as IDesignRelation); }
            }
        }

        #endregion
        public bool IsDragDeseign = false;//判断是否开始了拖拽，决定是否绘制设计线

        public IDesignObject CurrentHoverDesignObject { get; set; }
        IDesignObject _CurrentActiveDesignObject = null;
        public IDesignObject CurrentActiveDesignObject
        {
            get
            {
                return _CurrentActiveDesignObject;
            }
            set
            {
                _CurrentActiveDesignObject = value;
                if (!DeseignCanvas.Children.Contains(textBlockCurrent)) { DeseignCanvas.Children.Add(textBlockCurrent); }
                if (value != null)
                {
                    textBlockCurrent.Text = "Current:" + value.ControlName;
                    ToolTipService.SetToolTip(textBlockCurrent, "(" + value.ObjectID + ")" + value.ControlMemo);
                
                }
                else
                {
                    textBlockCurrent.Text = "Current:NULL";
                    ToolTipService.SetToolTip(textBlockCurrent, "No Active Object");

                }
            }
        }

        public Point? StartPoint { get; set; }
        public Point? EndPoint { get; set; }
        public IDesignAnchorPoint StartAnchor { get; set; }
        public IDesignAnchorPoint EndAnchor { get; set; }

        #region DropCreateSuport//支持从外部工具栏拖放创建新对象
        //这个创建完毕就赋给合法的ObjectID，并且保存
        IDesignObject DropNode = null;

        public virtual void EnableDropCreate(IDesignObject dn)
        {
            DropNode = dn;
            DeseignCanvas.PointerReleased += new PointerEventHandler(DeseignCanvas_DropCreatePointerReleased);
            DeseignCanvas.PointerMoved += new PointerEventHandler(DeseignCanvas_DropCreatePointerMoved);
        }//允许重载来加入新的特性
        public virtual void DisableDropCreate()
        {
            DropNode = null;
            DeseignCanvas.PointerReleased -= new PointerEventHandler(DeseignCanvas_DropCreatePointerReleased);
            DeseignCanvas.PointerMoved -= new PointerEventHandler(DeseignCanvas_DropCreatePointerMoved);

        } 
        void DeseignCanvas_DropCreatePointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (DropNode != null)
            {
                CreateControl(DropNode, e.GetCurrentPoint(DeseignCanvas).Position);
                if(DropNode.DataObject.ObjectID==null||DropNode.DataObject.ObjectID=="")
                {
                    DropNode.DataObject.ObjectID = SilverlightLFC.common.Environment.getGUID();
                }
                DropNode = null;
                DisableDropCreate();
            }

        }
        void DeseignCanvas_DropCreatePointerMoved(object sender, PointerRoutedEventArgs e)
        {

            if (IsDragDeseign)
            {
                Point p = e.GetCurrentPoint(this).Position;
                ShowDesignLine(p);
            }

        }
       
        #endregion

        #region Design//拖放设计阶段

        public void EnableDesignLine(Point p)
        {
            StartPoint = p;
            DeseignCanvas.PointerMoved += new PointerEventHandler(DeseignCanvas_DesignPointerMoved);
            DeseignCanvas.PointerPressed += new PointerEventHandler(DeseignCanvas_DesignPointerReleased);
        }

        void DeseignCanvas_DesignPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            HideDesignLine();

            DeseignCanvas.PointerMoved -= new PointerEventHandler(DeseignCanvas_DesignPointerMoved);
            DeseignCanvas.PointerReleased -= new PointerEventHandler(DeseignCanvas_DesignPointerReleased);

            
            IDesignRelation l=null;
            if ((StartAnchor != null) && (EndAnchor != null) && (EndAnchor != StartAnchor))
            {
                l=null;
                if (!getNewRelationControlByID.Equals(null))//新建关联
                {
                    l = getNewRelationControl( null, null);

                }
                if (l == null)//新建失败使用默认关联
                {
                    l = new LynxConnectLine();
                    l.IsErrorData = true;
                }
                if (l != null)
                {
                    l.designCanvas = this;

                    l.StartAnchorPoint = StartAnchor;
                    l.EndAnchorPoint = EndAnchor;
                    StartAnchor.OutRelationList.Add(l);
                    EndAnchor.InRelationList.Add(l);
                    l.SourceID = StartAnchor.ParentConnectControl.ObjectID;
                    l.TargetID = EndAnchor.ParentConnectControl.ObjectID;
                }

            }

            if (l == null) { return; }//没有能够创立链接
            sendObjectOperationEvent(l, DesignOperationFlag.CreateRelation);//建立新的连接
            l.ReadFromObject();

            DeActiveAll();

            l.DrawRelationLine(StartPoint.Value, e.GetCurrentPoint(getCanvas()).Position);
            RelationList.Add(l);
            ActiveDesignObject(l);
            EndAnchor = null;
            StartAnchor = null;
            CurrentConnectPoint = null;
            
        }
        void DeseignCanvas_DesignPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (StartAnchor != null)
            {
                Point p = e.GetCurrentPoint(this).Position;
                ShowDesignLine(StartPoint.Value, p);
            }
        }
        public void DisableDesignLine()
        {
            HideDesignLine();
            StartAnchor = null;
            DeseignCanvas.PointerMoved -= new PointerEventHandler(DeseignCanvas_DesignPointerMoved);
            DeseignCanvas.PointerReleased -= new PointerEventHandler(DeseignCanvas_DesignPointerReleased);
        }

        #endregion

        #region Structure//构造函数
        public LynxDeseignCanvas()
        {
            InitializeComponent();
            adp = new ActionDrop(DeseignCanvas, null, DataDrop);//允许放置
            adp.Enable=true;
            adp.EnableDrop();
        }
        #endregion

        #region DesignLine//有关设计线的方法
        LynxArrowLine DLine = new LynxArrowLine();

        public void ShowDesignLine(Point StartPoint, Point EndPoint)
        {
            DLine.StrokeBrush = new SolidColorBrush(Colors.Yellow);
            DLine.MainLineWidth = 5;
            DLine.DrawArrowLine(DeseignCanvas, StartPoint, EndPoint);
        }

        public void ShowDesignLine(Point EndPoint)
        {
            ShowDesignLine(StartPoint.Value, EndPoint);
        }

        public void HideDesignLine()
        {
            DLine.RemoveArrowLine();
        }
        #endregion

        public Canvas getCanvas()//获得画布对象
        {
            return this.DeseignCanvas;
        }

        #region Properties
        public LogicView.Diagram LogicDiagram
        {
            get;
            set;
        }
        public bool Status
        {
            get;
            set;
        }

        #endregion

        #region event//设计事件的定义和引发
        public event DesignEventHandler DesignObjectOperation;//改变了画布内的对象,包括增加,删除和修改链接

        public void sendObjectOperationEvent(object o, DesignOperationFlag p)//发送对当前设计对象的修改事件
        {
            if (DesignObjectOperation != null)
            {
                DesignObjectOperation(this, new LynxDesignEventArgs(o, p));
            }
        }
 
        #endregion

        #region SaveLoadLogicView
        public void LoadLogicViewInfor()//读取设计数据
        {
            if (LogicDiagram == null) { return; }
            Width = LogicDiagram.Width;
            Height = LogicDiagram.Height;

            DeseignCanvas.Children.Clear();
            LFCDataService lfcs = new LFCDataService();
            foreach (ViewItem vi in LogicDiagram.NodeItemList)
            {
                IDesignNode dn = getNewNodeControl(vi.DataObjectType, vi.DataObjectID);
                if (dn == null)//表示委托方法没有能够完成加载一个数据对应的设计节点，此时建立空节点
                {
                    dn.IsErrorData = true;//决定显示时候是红色
                    Type t = Type.GetType(vi.ControlType);
                    if (t != null)
                    {
                        object co = Activator.CreateInstance(t);
                        if (co != null && co is IDesignNode)
                        {
                            dn = co as IDesignNode;
                        }
                    }

                }
                if (dn == null)
                {
                    dn = new LynxDefaultNode();
                    dn.IsErrorData = true;
                }

                DeseignCanvas.Children.Add(dn.getControl());
                dn.LogicViewObject = vi;
                dn.LoadLogicViewInfor();
                dn.designCanvas = this;
            }
            foreach (ViewLineItem vl in LogicDiagram.RelationItemList)
            {
                IDesignRelation l = null;
                if (!getNewRelationControlByID.Equals(null))
                {
                    l = getNewRelationControl(vl.DataObjectType, vl.DataObjectID);
                }

                if (l == null)
                {
                    l = new LynxConnectLine();
                    l.IsErrorData = true;
                }
                l.designCanvas = this;
                l.LogicViewObject = vl;
                l.LoadLogicViewInfor();
                l.DrawRelationLine(l.StartPoint, l.EndPoint);

                List<FrameworkElement> fel = null;
                fel = getControlList(l.SourceID);
                foreach (FrameworkElement fe in fel)
                {
                    IDesignNode idn = fe as IDesignNode;
                    if (idn != null)
                    {
                        idn.AddRelationAsSourceObject(l);
                    }
                }
                fel = getControlList(l.TargetID);
                foreach (FrameworkElement fe in fel)
                {
                    IDesignNode idn = fe as IDesignNode;
                    if (idn != null)
                    {
                        idn.AddRelationAsTargetObject(l);
                    }
                }
            }
        }

        public void SaveLogicViewInfor()//写入设计数据
        {
            if (LogicDiagram == null) { return; }
            LogicDiagram.Width = Width;
            LogicDiagram.Height = Height;

            LogicDiagram.NodeItemList.Clear();
            LogicDiagram.RelationItemList.Clear();
            foreach (FrameworkElement fe in DeseignCanvas.Children)
            {
                IDesignNode ino = fe as IDesignNode;
                if (ino != null)
                {
                    ino.SaveLogicViewInfor();
                    LogicDiagram.NodeItemList.Add(ino.LogicViewObject);
                }
                IDesignRelation iro = fe as IDesignRelation;
                if (iro != null)
                {
                    iro.SaveLogicViewInfor();
                    LogicDiagram.RelationItemList.Add(iro.LogicViewObject);
                }
                object UItag = fe.Tag;
                if ((UItag != null) && (UItag is IDesignRelation))
                {
                    IDesignRelation tagR = UItag as IDesignRelation;
                    tagR.SaveLogicViewInfor();
                    LogicDiagram.RelationItemList.Add(tagR.LogicViewObject);
                }
            }


        }
        #endregion

        #region Query//依据条件进行选择
        public List<FrameworkElement> getControlList(string id)//依据包含的业务对象ID来查找对应控件
        {
            List<FrameworkElement> lc = new List<FrameworkElement>();
            foreach (FrameworkElement fe in DeseignCanvas.Children)
            {
                IDesignObject ido = fe as IDesignObject;
                if (ido != null)
                {
                    if (ido.DataObject.ObjectID == id)
                    {
                        lc.Add(fe);
                    }
                }
            }
            return lc;
        }

        public List<IDesignObject> getDesignObjectList(string id)//得到某个id的所有设计控件
        {
            List<IDesignObject> lc = new List<IDesignObject>();
            foreach (FrameworkElement fe in DeseignCanvas.Children)
            {
                if (fe is IDesignObject)
                {
                    IDesignObject ido = fe as IDesignObject;
                    if (ido.DataObject.ObjectID == id)
                    {
                        lc.Add(ido);
                    }
                }
            }
            return lc;
        }

        #endregion

        #region AddRemove
        public void RemoveControl(string id)//删除指定id对应的控件，并且删除关联
        {
            List<FrameworkElement> fel = getControlList(id);
            foreach (FrameworkElement fe in fel)
            {
                if (fe is IDesignObject)
                {
                    RemoveControl(fe as IDesignObject);
                }
            }
        }
        public void RemoveControl(IDesignObject ido)
        {
            if (ido == null) { return; }

            FrameworkElement fe = ido.getControl();
            if ((fe != null) && (DeseignCanvas.Children.Contains(fe)))
            {
                DeseignCanvas.Children.Remove(fe);
            }
            //if (DesignObjectList.Contains(ido)) { DesignObjectList.Remove(ido); }
            if (ido is IDesignNode)
            {
                IDesignNode idn = fe as IDesignNode;
                foreach (IDesignAnchorPoint ap in idn.AnchorPointList)
                {
                    foreach (IDesignRelation tdr in ap.InRelationList)
                    {
                        tdr.RemoveRelationLine();
                    }
                    foreach (IDesignRelation tdr in ap.OutRelationList)
                    {
                        tdr.RemoveRelationLine();
                    }
                }
                sendObjectOperationEvent(idn, DesignOperationFlag.DeleteObject);//提示设计图增加了节点对象
                //idn.DisableControlMove();//不再响应事件
            }
            if (ido is IDesignRelation)
            {
                IDesignRelation tdr = ido as IDesignRelation;
                tdr.RemoveRelationLine();
                sendObjectOperationEvent(ido, DesignOperationFlag.DeleteRelation);//提示设计图增加了节点对象
            }
            ido.designCanvas = null;
            ido.ClearAllEvent();
        }
        public void CreateControl(IDesignObject idn, Point p)
        {
            idn.designCanvas = this;
            FrameworkElement fe = idn.getControl();
            Canvas.SetLeft(fe, p.X);
            Canvas.SetTop(fe, p.Y);
            DeseignCanvas.Children.Add(fe);
            //DesignObjectList.Add(idn);

            if (idn is IDesignNode)
            {
                NodeList.Add(idn as IDesignNode);
                sendObjectOperationEvent(idn, DesignOperationFlag.CreateObject);//提示设计图增加了节点对象
            }
            if (idn is IDesignRelation)
            {
                RelationList.Add(idn as IDesignRelation);
                (idn as IDesignRelation).DrawRelationLine(p, new Point(p.X + 100, p.Y + 100));
                sendObjectOperationEvent(idn, DesignOperationFlag.CreateRelation);//提示设计图增加了节点对象
            }
        }
        #endregion

        #region ActiveControl//激活设计对象
        public void DeActiveAll()
        {
            foreach (IDesignNode ido in NodeList)
            {
                //IDesignObject ido = fe as IDesignObject;
                //if (ido != null)
                //{
                    ido.DeActive();
                //}
            }
            foreach (IDesignRelation ido in RelationList)
            {
                //IDesignObject ido = fe as IDesignObject;
                //if (ido != null)
                //{
                    ido.DeActive();
                //}
            }
        }

        public void ActiveDesignObject(IDesignObject o)
        {
            //DeActiveAll();
            CurrentActiveDesignObject = o;
            o.Active();
        }
        #endregion

        private void DeseignCanvas_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key== Windows.System.VirtualKey.Delete)
            {
                if (CurrentActiveDesignObject != null)
                {
                    RemoveControl(CurrentActiveDesignObject);
                }
            }
        }

        #region AddExistObject//添加已有的对象

        public bool IsIncludeObjectIDControl(string id)//判断该id的实体是否被包含
        {
            foreach (IDesignNode idn in NodeList)
            {
                if (idn.ObjectID==id)
                {
                    return true;
                }
            }
            foreach (IDesignRelation idr in RelationList)
            {
                if (idr.ObjectID == id)
                {
                    return true;
                }
            }
            return false;
        }

        public List<IDesignObject> getObjectByID(string id)//依据指定的ID获取全部已经加载的设计对象
        {
            List<IDesignObject> idoList = new List<IDesignObject>();
            foreach (IDesignNode idn in NodeList)
            {
                if (idn.ObjectID == id)
                {
                    idoList.Add(idn);
                }
            }
            foreach (IDesignRelation idr in RelationList)
            {
                if (idr.ObjectID == id)
                {
                    idoList.Add(idr);
                }
            }
            return idoList;
        }

        public void AddExistNode(IDesignNode idn,Point p)//添加一个指定的新设计对象，不进行关联添加
        {
            DeseignCanvas.Children.Add(idn.getControl());
            idn.designCanvas = this;
            Canvas.SetLeft(idn.getControl(), p.X);//指定位置
            Canvas.SetTop(idn.getControl(), p.Y);

            NodeList.Add(idn);
        }

        public void AddExistNode(IDesignNode idn)//添加一个指定的新设计对象，不进行关联添加
        {
            AddExistNode(idn, new Point(DeseignCanvas.ActualWidth/2 - (new Random()).Next(150),DeseignCanvas.ActualHeight/2 - (new Random()).Next(150)));//在距离中心一定范围内随机布置
        }

        public void AddExistRelation(IDesignRelation idr)//添加关联，自行判断是否有关联控件存在
        {
            if (idr == null) { return; }
            if (NodeList.Count == 0) { return; }
            idr.designCanvas = this;
            IDesignNode sdn=null, tdn=null;
            if (idr.SourceID != null)
            {
                List<IDesignNode> rl=NodeList.Where(n => n.ObjectID == idr.SourceID).ToList();
                if (rl.Count > 0)
                    sdn = rl[0];
            }
            if (idr.TargetID != null)
            {
                List<IDesignNode> rl = NodeList.Where(n => n.ObjectID == idr.SourceID).ToList();
                if (rl.Count > 0)
                    tdn = rl[0];
            }
            if (sdn != null)
            {
                idr.StartAnchorPoint = sdn.getDefaultAnchorPoint();
                idr.StartPoint = idr.StartAnchorPoint.getDesignCanvasPoint();
            }
            else
            {
                idr.StartAnchorPoint = null;
                idr.StartPoint = new Point(DeseignCanvas.Width / 2 - (new Random()).Next(50), DeseignCanvas.Height / 2 - (new Random()).Next(50));
            }
            if (tdn != null)
            {
                idr.EndAnchorPoint = tdn.getDefaultAnchorPoint();
                idr.EndPoint = idr.EndAnchorPoint.getDesignCanvasPoint();
            }
            else
            {
                idr.EndAnchorPoint = null;
                idr.EndPoint = new Point(DeseignCanvas.Width / 2 + (new Random()).Next(150), DeseignCanvas.Height / 2 + (new Random()).Next(150));

            }
            FrameworkElement fe = idr.getControl();

            DeseignCanvas.Children.Add(fe);

            idr.DrawRelationLine(idr.StartPoint, idr.EndPoint);
            sendObjectOperationEvent(idr, DesignOperationFlag.CreateRelation);//提示设计图增加了节点对象
            RelationList.Add(idr);
        }
        
        public void AddExistRelation(IDesignRelation idr,IDesignNode sdn,IDesignNode tdn)//在指定的起始节点和终止节点之间添加一个关联控件
        {
            if (idr == null) { return; }
            if (NodeList.Count == 0) { return; }
            idr.designCanvas = this;
            if (sdn != null)
            {
                idr.StartAnchorPoint = sdn.getDefaultAnchorPoint();
                idr.StartPoint = idr.StartAnchorPoint.getDesignCanvasPoint();
            }
            else
            {
                idr.StartAnchorPoint = null;
                idr.StartPoint =new Point(DeseignCanvas.Width/2 - (new Random()).Next(50),DeseignCanvas.Height/2 - (new Random()).Next(50));
            }
            if (tdn != null)
            {
                idr.EndAnchorPoint = tdn.getDefaultAnchorPoint();
                idr.EndPoint = idr.EndAnchorPoint.getDesignCanvasPoint();
            }
            else
            {
                idr.EndAnchorPoint = null;
                idr.EndPoint = new Point(DeseignCanvas.Width / 2 + (new Random()).Next(150), DeseignCanvas.Height / 2 + (new Random()).Next(150));

            }
            FrameworkElement fe = idr.getControl();

            DeseignCanvas.Children.Add(fe);

            idr.DrawRelationLine(idr.StartPoint, idr.EndPoint);
            sendObjectOperationEvent(idr, DesignOperationFlag.CreateRelation);//提示设计图增加了节点对象
            RelationList.Add(idr);
        }

        public void AddExistNode(string id)//添加一个现存的Node，并且自动寻找适合的关联线，一旦有也同步添加，由于是从实体层添加的，因此关联添加
        {
            IDesignNode idn = getNewNodeControl(null, id);
            if (idn == null) { return; }
            AddExistNode(idn);
            if (idn.ObjectID != null)
            {
                List<IDesignRelation> rl = getAllowedRelationListByNodeID(null, idn.ObjectID);
                foreach (IDesignRelation r in rl)
                {
                    if (r.SourceID == null || r.TargetID == null) { continue; }//直接进入下次循环
                    List<IDesignNode> tnl = NodeList.Where(n => n.ObjectID == r.SourceID || n.ObjectID == r.TargetID).ToList();
                    if (tnl.Count != 0)
                    {
                        AddExistRelation(r.ObjectID);
                    }
                }
            }
        }

        public void AddExistRelation(string  id)//添加现有的关联，自动寻找两侧锚点位置
        {
            IDesignRelation idr = getNewRelationControl(null, id);
            if (idr == null) { return; }
            List<IDesignNode> snl=new List<IDesignNode>();
            List<IDesignNode> tnl=new List<IDesignNode>();
            if(idr.SourceID!=null){
                snl=NodeList.Where(n=>n.ObjectID==idr.SourceID).ToList();
            }
            if(idr.TargetID!=null){
                tnl=NodeList.Where(n=>n.ObjectID==idr.TargetID).ToList();
            }

            if(tnl.Count>0&&snl.Count>0)
            {
                foreach(IDesignNode sdn in snl)
                {
                    foreach(IDesignNode tdn in tnl)
                    {
                        IDesignRelation dr = getNewRelationControl(null, id);
                        if (dr == null) { return; }
                        AddExistRelation(dr, sdn, tdn);

                    }
                }
            }
            if(snl.Count==0&&tnl.Count>0)
            {
                foreach (IDesignNode tdn in tnl)
                {
                    IDesignRelation dr = getNewRelationControl(null, id);
                    if (dr == null) { return; }
                    AddExistRelation(dr, null, tdn);
                }
            }
            if(tnl.Count==0&&snl.Count>0)
            {
                foreach (IDesignNode sdn in snl)
                {
                    IDesignRelation dr = getNewRelationControl(null, id);
                    if (dr == null) { return; }
                    AddExistRelation(dr, sdn, null);
                }
            }
            if(snl.Count==0&&tnl.Count==0)
            {
                IDesignRelation dr = getNewRelationControl(null, id);
                if (dr == null) { return; }
                AddExistRelation(dr, null, null);
            }

        }

        #endregion

    }
}
