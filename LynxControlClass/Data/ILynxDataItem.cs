using System;
using System.Net;
using System.Windows;



using System.Windows.Input;
using Windows.UI.Xaml.Controls;




namespace SilverlightLynxControls.Data
{
    public interface ILynxDataItem
    {
        void ShowObject(Panel p, System.Collections.Generic.Dictionary<string, object> r);//读取并显示信息到指定的容器里面
    }
}
