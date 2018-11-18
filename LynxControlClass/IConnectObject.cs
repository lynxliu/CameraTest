using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using Windows.Foundation;

namespace SilverlightLynxControls
{
    public interface IConnectObject//可以被连接的对象
    {
        void AddInRelation(IConnection c);
        void AddOutRelation(IConnection c);
        void RemoveInRelation(IConnection c);
        void RemoveOutRelation(IConnection c);
        List<IConnectObject> InObjectList{get;}
        List<IConnectObject> OutObjectList{get;}
        Point getCPosition();
        List<ILine> InLines { get; }
        List<ILine> OutLines { get; }
        string ObjectID { get; set; }
        string Name { get; set; }
        string Memo { get; set; }
    }


}
