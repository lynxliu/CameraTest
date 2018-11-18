using System;
using System.Net;
using System.Windows;



using System.Windows.Input;




namespace SilverlightLFC.common
{
    public interface  IXMLObject//定义所有可以自主处理xml字符串的对象接口

    {
        void ReadFromXMLString(string s,string tag);
        string WriteToXMLString();
        string WriteToXMLString(string tag);
        //IXMLObject getInstance();
    }
}
