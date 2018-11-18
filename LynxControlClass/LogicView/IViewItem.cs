using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using SilverlightLFC.common;
using Windows.Foundation;
using Windows.UI;

namespace SilverlightLynxControls.LogicView
{
    public interface IViewItem//所有可以被包含在Diagram里面的视图数据，基本功能就是记录数据，编辑功能由别的对象完成
    {//所有记录图的实现都需要实现这个接口

        string ObjectID { get; }
        string Name { get; set; }
        string Memo { get; set; }
        string ControlType { get; set; }
        string DataObjectType { get; set; }
        string DataObjectID { get; set; }

        string WriteToXMLString();
        void LoadFromXMLString(string s);
    }

    public interface IViewNodeItem : IViewItem
    {
        double Width { get; set; }
        double Height { get; set; }
        double Top { get; set; }
        double Left { get; set; }
        //LFCObjectList<LogicViewAnchorPoint> AnchorPointList { get; }//Node里面可以有不同的锚点
    }

    public interface IViewRelationItem : IViewItem//所有可以被包含在Diagram里面的视图数据，基本就是记录数据
    {//所有记录图的实现都需要实现这个接口

        Point SourcePoint { get; set; }
        Point TargetPoint { get; set; }
        string SourceNodeID { get; set; }
        string TargetNodeID { get; set; }
        Color LineColor { get; set; }
        string LineType { get; set; }
    }

    public interface IDiagram
    {
        string ObjectID { get; }

        double Width { get; set; }
        double Height { get; set; }
        double Zoom { get; set; }
        List<IViewNodeItem> NodeItemList { get; }
        List<IViewRelationItem> RelationItemList { get; }
    }
}
