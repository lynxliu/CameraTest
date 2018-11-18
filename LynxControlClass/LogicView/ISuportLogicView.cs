using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using SilverlightLynxControls.Design;

namespace SilverlightLynxControls.LogicView
{
    public interface ISuportLogicView//支持访问逻辑视图，非泛型类型，适合使用多态处理，相当于泛型类型的父类
    {
        LogicView.ViewItem LogicViewObject { get; set; }//只要是直接可视化控件支持的都是ViewItem
        void LoadLogicViewInfor();
        void SaveLogicViewInfor();
        Control getViewControl();
        ISuportLogicCanvasView designCanvas { get; set; }//必须包含这个来计算方位
        List<ISuportLogicAnchorPoint> AnchorPointList { get; }
        void ReadFromObject();
        void WriteToObject();
        void Active();
        void DeActive();
    }

    public interface ISuportLogicView<T> : ISuportLogicView where T : SilverlightLFC.common.AbstractLFCDataObject//支持访问逻辑视图
    {
        LogicView.ViewItem LogicViewObject { get; set; }//只要是直接可视化控件支持的都是ViewItem
        void LoadLogicViewInfor();
        void SaveLogicViewInfor();
        T DataObject { get; set; }
        //SilverlightLFC.common.AbstractLFCDataObject getDataObject();
        //void setDataObject(SilverlightLFC.common.AbstractLFCDataObject d);
        Control getViewControl();
        ISuportLogicCanvasView designCanvas { get; set; }//必须包含这个来计算方位
        List<ISuportLogicAnchorPoint> AnchorPointList { get; }
        void ReadFromObject();
        void WriteToObject();
        void Active();
        void DeActive();
    }

    public interface ISuportLogicLineView<T> where T : SilverlightLFC.common.AbstractLFCDataObject//支持访问逻辑连线视图
    {
        LogicView.ViewLineItem LogicViewObject { get; set; }//只要是直接可视化控件支持的都是ViewItem
        void LoadLogicViewInfor();
        void SaveLogicViewInfor();
        //ISuportLogicAnchorPoint SourceAnchorPoint { get; set; }
        //ISuportLogicAnchorPoint TargetAnchorPoint { get; set; }
        T DataObject { get; set; }
        void DrawRelationLine();
        void MoveRelationLine();
        void RemoveRelationLine();
        void ReadFromObject();
        void WriteToObject();
        
    }

    public interface ISuportLogicAnchorPoint//支持访问逻辑连线视图
    {
        LogicView.LogicViewAnchorPoint LogicViewObject { get; set; }//只要是直接可视化控件支持的都是ViewItem

        //LogicView.ViewItem ParentLogicViewObject { get; set; }//只要是直接可视化控件支持的都是ViewItem
        string Key { get; set; }
        void LoadLogicViewInfor();
        void SaveLogicViewInfor();
    }

    public interface ISuportLogicCanvasView:IDesignCanvas//支持访问逻辑画布视图，首先要是一个设计画布
    {
        LogicView.Diagram LogicViewObject { get; set; }//只要是直接可视化控件支持的都是ViewItem
        //SilverlightLFC.common.LFCObjectList<ViewItem> LogicViewItemList { get; set; }//画布里面的全部逻辑对象集合
        //SilverlightLFC.common.LFCObjectList<ViewLineItem> LogicViewLineItemList { get; set; }//画布里面的逻辑关联集合
        void LoadLogicViewInfor();
        void SaveLogicViewInfor();
        Diagram DataObject { get; set; }
        ISuportLogicView<T> getControl<T>(string id) where T:AbstractLFCDataObject;
        ISuportLogicView<T> getControl<T>(T obj) where T : AbstractLFCDataObject;
        ISuportLogicLineView<T> getRelationControl<T>(string id) where T : AbstractLFCDataObject;
        ISuportLogicLineView<T> getRelationControl<T>(T obj) where T : AbstractLFCDataObject;
    }
}
