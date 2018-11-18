using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using SilverlightLFC.common;
using SilverlightLynxControls.LogicView;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.Foundation;

namespace SilverlightLynxControls.Design
{
    public interface IDesignObject  //所有可能出现在画布里面的对象的共同特征
    {
        Brush DesignObjectBorderBrush { get; set; }
        Brush DesignObjectBackBrush { get; set; }
        Brush DesignObjectHoverBrush { get; set; }
        Brush DesignObjectActiveBrush { get; set; }

        string ObjectID { get; }//严格的和DataObject对象的ID一致，支持直接从这个id加载对应的数据对象
        string ControlName { get; set; }//直接受制于DataObject，否则没有意义，也不会保存
        string ControlMemo { get; set; }

        AbstractLFCDataObject DataObject { get; set; }
        FrameworkElement getControl();
        //FrameworkElement getNewControl();//获取一个表达同一个数据对象的设计对象
        
        void LoadLogicViewInfor();
        void SaveLogicViewInfor();

        IDesignCanvas designCanvas { get; set; }//必须包含这个来计算方位
        void ReadFromObject();
        void WriteToObject();
        void ReadFromObject(string id);

        void Active();
        void DeActive();
        void ClearAllEvent();//退订全部的事件，保证可以干净的删除
        bool IsErrorData { get; set; }//表示数据错误，需要有异常显示
    }

    public interface IDesignNode: IDesignObject//包含链接点的用户控件，可以决定发出的连线的类型
    {
        List<IDesignAnchorPoint> AnchorPointList { get; }
        string LineType { get; set; }//依据线的类型，构造不同的连接线

        IDesignAnchorPoint getNearAnchorPoint(Point DiagramPosition);//找到距离点击点最近的一个锚点
        IDesignAnchorPoint getDefaultAnchorPoint();//找到距离点击点最近的一个锚点
        ViewItem LogicViewObject { get; set; }//只要是直接可视化控件支持的都是ViewItem，用来保存显示数据

        void AddRelationAsSourceObject(IDesignRelation IRelation);
        void AddRelationAsTargetObject(IDesignRelation IRelation);
        void DeleteRelation(IDesignRelation IRelation);

        void EnableControlMove();
        void DisableControlMove();

        //IDesignRelation getRelation();//节点可以决定可以发出的链接关联的数据对象
    }

    public interface IDesignRelation : IDesignObject
    {
        string LineType { get; set; }//依据线的类型的不同，有不同的绘制方式
        ViewLineItem LogicViewObject { get; set; }//只要是直接可视化控件支持的都是ViewItem，用来保存显示数据
        Point StartPoint { get; set; }
        Point EndPoint { get; set; }

        string SourceID { get; set; }
        string TargetID { get; set; }

        IDesignAnchorPoint StartAnchorPoint { get; set; }
        IDesignAnchorPoint EndAnchorPoint { get; set; }

        void DrawRelationLine();
        void DrawRelationLine(Point sp, Point ep);
        void RemoveRelationLine();
        void MoveRelationStartPoint(double dx, double dy);
        void MoveRelationEndPoint(double dx, double dy);
        void MoveRelationStartPoint(Point nsp);
        void MoveRelationEndPoint(Point nep);
    }

    public interface IDesignAnchorPoint//支持访问逻辑连线视图，只和布局有关，和数据无关
    {
        string Key { get; set; }
        List<IDesignRelation> InRelationList { get; }
        List<IDesignRelation> OutRelationList { get; }

        void AddOutRelation(IDesignRelation or);//添加关联，锚点作为源
        void AddInRelation(IDesignRelation ir);//添加关键，锚点作为目标
        void RemoveRelation(IDesignRelation dr);//删除关联

        void MoveLines(double dx, double dy);

        IDesignNode ParentConnectControl{get;set;}

        Brush AnchorPointBorderBrush { get; set; }//
        Brush AnchorPointBackBrush { get; set; }
        Brush AnchorPointHoverBrush { get; set; }
        Brush AnchorPointActiveBrush { get; set; }

        Point getDesignCanvasPoint();//获得该锚点相对设计画布
        Point ParentControlPoint { get; set; }//获取该锚点相对于设计控件的位置
    }

}
