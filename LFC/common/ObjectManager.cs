using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using SilverlightLFC.common;
using System.Collections.Generic;

namespace SilverlightLFC.common
{
    public abstract class ObjectManager//给一个简单的查询事件
    {
        public Dictionary<string, object> ParameterList = new Dictionary<string, object>();
        public event LynxCommonProcessHandler ProcComplete;//定义简单的查询事件
        public void sendProcEvent(bool r, string s,object ReturnValue)//给出简单的查询返回，成功，失败和返回的结果集
        {
            LynxCommonProcessEventArgs leq = new LynxCommonProcessEventArgs(r,s,ReturnValue);
            if (ProcComplete != null)
            {
                ProcComplete(this, leq);
            }
        }
        //public SilverlightLFC.common.LFCDataService ls = new SilverlightLFC.common.LFCDataService();
        //public List<Dictionary<string, object>> _DataTable;//缓存数据表
        //public abstract List<T> getResult<T>();
        //public abstract void setResult<T>(List<T> ls);

        //void ls_ProcessQueryComplete<T>(object sender, SilverlightLFC.common.LynxProcessCompleteEventArgs e)
        //{
        //    ls.ProcessQueryComplete -= new SilverlightLFC.common.ProcessEventHandler(ls_ProcessQueryComplete<T>);

        //    setResult<T>( (List<T>)e.ReturnValue);
        //    sendProcEvent(e.IsSuccess, "");
        //}

        //public void InitQueryOrganise<T>(string keyName,string keyValue)where T:AbstractLFCDataObject
        //{
        //    ls.ProcessQueryComplete += new SilverlightLFC.common.ProcessEventHandler(ls_ProcessQueryComplete<T>);
        //    ls.AsynchronousGetObjectListByFKey<T>(keyValue, keyName);
        //}
    }
}
