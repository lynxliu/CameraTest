using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;

namespace SilverlightLFC.common
{
    public class ObjectUIStatus//帮助绑定UI对象加载数据对象的状态
    {//主要是帮助实现延时加载和处理
        public static ObjectUIStatus getObjectUIStatus(AbstractLFCDataObject o)
        {
            ObjectUIStatus t = new ObjectUIStatus();
            t.DataObject = o;
            t.IsLoaded = true;
            return t;
        }
        public string Status = "";
        public bool IsLoaded=false;
        public bool IsObjectChangeds=false;
        public bool IsRelationChanged = false;
        public bool IsRelationObjectListLoaded=false;
        public AbstractLFCDataObject DataObject;
        public Dictionary<string, string> RelationObjectListLoadStatus = new Dictionary<string, string>();//加载关联对象时候，加载的状态
        public void AddRelationObjectListLoadJob(string ObjName)
        {
            RelationObjectListLoadStatus.Add(ObjName, "UnLoad");
        }
        public void FinishRelationObjectListLoadJob(string ObjName)
        {
            if(RelationObjectListLoadStatus.ContainsKey(ObjName))
            {
                RelationObjectListLoadStatus.Remove(ObjName);
            }
        }
        public bool IsAllRelationListLoaded()
        {
            foreach(KeyValuePair<string,string> vp in RelationObjectListLoadStatus)
            {
                if (vp.Value == "UnLoad")
                {
                    return false;
                }

            }
            RelationObjectListLoadStatus.Clear();//一旦全部加载完成就清除
            return true;
        }
    }
}
