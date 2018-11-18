using System;
using System.Net;
using System.Windows;



using System.Windows.Input;




namespace SilverlightLynxControls.LynxControls
{
    public interface IDataControl//数据显示界面控件，可以从实体对象读取信息和写入信息
    {
        SilverlightLFC.common.AbstractLFCDataObject DataObject { get; set; }
        void ReadFromObject();
        void WriteToObject();
    }

    public interface IModelDataControl//数据显示界面控件，可以从实体对象读取信息和写入信息
    {
        IModelData DataObject { get; set; }
        void ReadFromObject();
        void WriteToObject();
    }

    public interface IModelData//数据显示界面控件，可以从实体对象读取信息和写入信息
    {
        string Name { get; set; }
        string Memo { get; set; }
        string ObjectID { get; set; }
    }
}
