using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;

namespace SilverlightLFC.common
{
    public delegate void LFCListItemChanged(IAbstractLFCDataObject o, LFCObjChanged lo);
    public enum DataListStatus//表示集合是不是已经是数据库查询
    {
        DataLinked, DataLinkedChanged, DataUnLinkedChanged, DataUnLinked
        
    }


    public class LFCObjectList<T> : ObservableCollection<T> where T : IAbstractLFCDataObject
    {
        public LFCObjectList(){}
        public LFCObjectList(List<T> tl):base(tl) { }

        public event LFCListItemChanged LFCListObjectChanged;
        void sendChangedEvent(T o, LFCObjChanged lo)
        {
            if (LFCListObjectChanged != null)
            {
                LFCListObjectChanged(o, lo);
            }
        }
        
        public DataListStatus Status = DataListStatus.DataUnLinked;
        SilverlightLFC.common.LFCDataService ls = new SilverlightLFC.common.LFCDataService();
        public new void Add(T item)
        {
            base.Add(item);
            sendChangedEvent(item, LFCObjChanged.ObjectCreated);
            if (Status == DataListStatus.DataUnLinked)
            {
                Status = DataListStatus.DataUnLinkedChanged;
            }
            if (Status == DataListStatus.DataLinked)
            {
                Status = DataListStatus.DataLinkedChanged;
            }
        }
        public new void Remove(T item)//只有没有关联的时候真的删除，否则只是打标记
        {
            if (Status == DataListStatus.DataUnLinked || Status == DataListStatus.DataUnLinkedChanged)
            {
                Status = DataListStatus.DataUnLinkedChanged;
                base.Remove(item);
                sendChangedEvent(item, LFCObjChanged.ObjectDeleted);
                return;
            }
            //item.DataFlag = DataOperation.Delete;
            if (Status == DataListStatus.DataLinked)
            {
                Status = DataListStatus.DataLinkedChanged;
            }
        }

        public new void Clear()//相当于初始化
        {
            base.Clear();
            Status = DataListStatus.DataUnLinked;
        }

        public void AddRelationObject(T o,string FKeyName,string FKeyValue)
        {
            Add(o);
            FieldInfo f = ls.getFieldByName(o, FKeyName);
            if (f != null)
            {
                f.SetValue(o, FKeyValue);
            }
            if (o.DataFlag == DataOperation.Nothing)
            {
                o.DataFlag = DataOperation.Update;
            }
            if (Status == DataListStatus.DataUnLinked)
            {
                Status = DataListStatus.DataUnLinkedChanged;
            }
            if (Status == DataListStatus.DataLinked)
            {
                Status = DataListStatus.DataLinkedChanged;
            }
        }

        public void RemoveRelationObject(T o,string FKeyName)//对于关联的数据对象，只打标记，不真正的删除
        {
            if (Contains(o))
            {
                //ol.Remove(o);
                //IsChanged = true;
                FieldInfo f = ls.getFieldByName(o, FKeyName);
                if (f != null)
                {
                    f.SetValue(o, o.getUnAvailableIDValue());
                }
                if (o.DataFlag == DataOperation.Nothing)
                {
                    o.DataFlag = DataOperation.Update;
                }
                if (Status == DataListStatus.DataUnLinked)
                {
                    Status = DataListStatus.DataUnLinkedChanged;
                }
                if (Status == DataListStatus.DataLinked)
                {
                    Status = DataListStatus.DataLinkedChanged;
                }
            }
        }
        public void LoadList(List<T> ol)
        {
            foreach (T o in ol)
            {
                Add(o);
            }
        }
        
        public event LynxCommonProcessHandler ListProcComplete;//定义简单的查询事件
        public void sendEvent(bool r, string s,object o)//给出简单的查询返回，成功，失败和返回的结果集
        {
            LynxCommonProcessEventArgs leq = new LynxCommonProcessEventArgs(r, s,o);
            if (ListProcComplete != null)
            {
                ListProcComplete(this, leq);
            }
        }

        public string getStatus()
        {
            if (Status == DataListStatus.DataLinked) { return "DataLinked"; }
            if (Status == DataListStatus.DataLinkedChanged) { return "DataLinkedChanged"; }
            if (Status == DataListStatus.DataUnLinkedChanged) { return "DataUnLinkedChanged"; }
            if (Status == DataListStatus.DataUnLinked) { return "DataUnLinked"; }
            return "DataUnLinked";
        }

        public void setStatus(string s)
        {
            if (s == "DataLinked") {Status=DataListStatus.DataLinked; }
            if (s == "DataLinkedChanged") {Status=DataListStatus.DataLinkedChanged; }
            if (s == "DataUnLinkedChanged") {Status=DataListStatus.DataUnLinkedChanged; }
            Status=DataListStatus.DataUnLinked; 

        }

        public void AddList(LFCObjectList<T> tt)
        {
            foreach (T t in tt)
            {
                Add(t);

            }
        }

        public void RemoveList(LFCObjectList<T> tt)
        {
            foreach (T t in tt)
            {
                if (Contains(t))
                {
                    Remove(t);
                }
            }
        }

        public bool IsChanged()//判断任何一个子对象是不是被修改的
        {
            foreach (T t in this)
            {
                if (t.DataFlag != DataOperation.Nothing)
                {
                    return false;
                }
            }
            return true;
        }

        public void Sort()
        {
           Sort(Comparer<T>.Default);
        }

        public void Sort(IComparer< T> comparer)
        {
            int i, j;
            T index;
            for (i = 1; i <  this.Count; i++)
            {
                index = Items[i];
                j = i;
                while ((j > 0) && (comparer.Compare(Items[j - 1], index) == 1))
                {
                    Items[j] = Items[j - 1];
                    j = j - 1;
                }
                Items[j] = index;
            }
        }

    }
}
