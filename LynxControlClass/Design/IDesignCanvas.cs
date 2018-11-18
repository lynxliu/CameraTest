using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using SilverlightLynxControls.LogicView;
using SilverlightLFC.common;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SilverlightLynxControls.Design
{
    public interface IDesignCanvas //设计用的画布,需要提供事件，以表示发生的事情
    {
        //List<IDesignObject> ActiveObjectList;//当前选择的设计对象，支持多选
        //public void ClearActiveObject();//清除所有选择

        List<IDesignNode> NodeList { get; }//保存所有加入的节点对象
        List<IDesignRelation> RelationList { get; }//保存所有的关联对象

        Canvas getCanvas();//获取真正的画布
        Diagram LogicDiagram//保存的实体对象
        {
            get;
            set;
        }
        bool Status{get;set;}//画布的状态
        //List<IDesignObject> DesignObjectList { get; set; }

        Point? StartPoint { get; set; }//设计线开始的位置
        Point? EndPoint { get; set; }//设计线结束位置

        //LynxConnectPoint CurrentConnectPoint { get; set; }//拖动的链接点
        //void EnableRelationDesign();//允许拖动链接的设计关联点，但是必须给出初始位置
        void BeginRelationDesign(Point p, LynxConnectPoint lp);
        void EndRelationDesign();

        IDesignAnchorPoint StartAnchor { get; set; }//开始的锚点
        IDesignAnchorPoint EndAnchor { get; set; }
        IDesignObject CurrentActiveDesignObject { get; set; }//当前激活的设计控件
        IDesignObject CurrentHoverDesignObject { get; set; }//当前处于鼠标下面的控件

        void EnableDesignLine(Point sp);
        void DisableDesignLine();
        void ShowDesignLine(Point StartPoint, Point EndPoint);//显示设计线
        void ShowDesignLine(Point EndPoint);
        void HideDesignLine();

        event DesignEventHandler DesignObjectOperation;//每个时段只能进行一项操作
        void sendObjectOperationEvent(object o,DesignOperationFlag p);

        void LoadLogicViewInfor();//从Diagram对象里面加载和还原设计图
        void SaveLogicViewInfor();//把设计图的信息保存到Diagram对象

        List<FrameworkElement> getControlList(string id);
        List<IDesignObject> getDesignObjectList(string id);

        void DeActiveAll();//不激活任何一个控件
        void ActiveDesignObject(IDesignObject o);//激活指定控件

        void EnableDropCreate(IDesignObject dn);//允许拖放创建对象
        void DisableDropCreate();

        void RemoveControl(string id);
        void RemoveControl(IDesignObject idn);
        void CreateControl(IDesignObject idn, Point p);

        IDesignRelation getNewRelationControl(string DataObjectType, string id);
        IDesignNode getNewNodeControl(string DataObjectType, string id);
        List<IDesignRelation> getAllowedRelationListByNodeID(string DataObjectType, string id);//依据ID或者类别获取所有可能的关联
        List<IDesignNode> getAllowedNodeListByNodeID(string DataObjectType, string id);//依据ID和类别获取某个某个对象联络的全部节点
        bool? IsDesignNodeByObjectID(string id);
        bool? IsDesignRelationByObjectID(string id);
        
        //IDesignRelation getCommonRelationControl(GetCommonRelationControl getRControl, string TypeName);
        //IDesignNode getCommonNodeControl(GetCommonNodeControl getNControl, string TypeName);
    }
    public delegate IDesignRelation GetNewRelationControl(string DataObjectType,string id);//得到一个新的关联对象(参数为null时候),或者已有对象的新设计对象实例
    public delegate IDesignNode GetNewNodeControl(string DataObjectType, string id);//得到一个新的关联对象(参数为null时候),或者已有对象的新设计对象实例
    public delegate List<IDesignRelation> GetAllowedRelation(string DataObjectType,string id);//依据ID或者类别获取所有可能的关联
    public delegate List<IDesignNode> GetAllowedNode(string DataObjectType,string id);//依据ID和类别获取某个某个对象联络的全部节点
    public delegate bool IsConnectNode(string sid, string tid);//判断两个对象之间是否有关联
    public delegate bool? IsNodeDataObject(string id);
    public delegate bool? IsRelationDataObject(string id);
    //public delegate IDesignRelation GetCommonRelationControl(string TypeName);//依据类型得到没有数据绑定的通用关联控件
    //public delegate IDesignNode GetCommonNodeControl(string TypeName);//依据类型得到没有数据绑定的通用节点控件

    public enum DesignOperationFlag//说明设计图上面的对象（IDesignObject）如何变化
    {
        ObjectMove,CreateObject, ModifyObject, DeleteObject, CreateRelation, ModifyRelation, DeleteRelation
    }

    public delegate void DesignEventHandler(object sender, LynxDesignEventArgs e);//定义的设计事件
    public class LynxDesignEventArgs : EventArgs//标识设计界面上面发生的事情
    {
        public object OperatorObject;//产生这个事件的对象句柄
        public DateTime EventTime = DateTime.Now;//事件发生时刻
        public DesignOperationFlag OperationFlag;//调用的操作
        public LynxDesignEventArgs(object o,DesignOperationFlag p)
        {
            OperatorObject = o;
            OperationFlag = p;
        }

    }
}
