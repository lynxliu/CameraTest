using System;
using System.Net;
using System.Windows;



using System.Windows.Input;




namespace SilverlightLynxControls.ObjectMonitorUI
{
    public interface IObjectMonitor//对对象的监控编辑UI
    {
        void ReadFromObject();//读取信息
        void WriteToObject();//写入信息
        void EnableMonitor();//开始监控
        void DisableMonitor();//取消监控

    }
}
