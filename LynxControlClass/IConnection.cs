using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;

namespace SilverlightLynxControls
{
    public interface IConnection//逻辑连接对象
    {
        void AddInRelation(IConnectObject cn);
        void AddOutRelation(IConnectObject cn);
        void RemoveInRelation(IConnectObject cn);
        void RemoveOutRelation(IConnectObject cn);
        IConnectObject InObject { get; set; }
        IConnectObject OutObject { get; set; }
        string Name { get; set; }
        string Memo { get; set; }
        IConnection getNewConnection();
        string ObjectID { get; set; }
    }
}
