using System;
using System.Net;
using System.Windows;



using System.Windows.Input;




namespace SilverlightLFC.common
{
    public interface IDataView<T> where T:AbstractLFCDataObject//只是观察对象信息，不能编辑
    {
        T DataObject { get; set; }
        void ReadFromObject();
        void ReadFromObject(T so);
        //void WriteToObject();
    }
    public interface IDataMonitor<T> : IDataView<T> where T : AbstractLFCDataObject//同时支持观察和编辑
    {

        void WriteToObject();
    }
}
