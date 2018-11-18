using System;
using System.Net;
using System.Windows;



using System.Windows.Input;




namespace SilverlightLynxControls
{
    public interface IDragDrop//支持拖放的基本接口，一个是要能够有被拖放的对象，一个是要能够处理拖放过来的对象
    {
        ActionDragDrop dragdropAction
        {
            get;

        }
        object getDragObject();//准备被拖放的对象
        void processDropObject(object dragObject);//处理拖放过来的对象
    }
}
