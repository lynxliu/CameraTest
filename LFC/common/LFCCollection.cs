using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using SilverlightLFC.common;
using System.Collections.Generic;
using System.Reflection;

namespace SilverlightLFC.common
{
    public class LFCObjectCollection<T> where T : AbstractLFCDataObject//保存查询结果，并对结果进行处理
    {
        public LFCObjectCollection(string fkName, string fkValue)
        {
            Type t = typeof(T);
            sql = "select * from Table_" + t.Name + " where " + t.Name + "_" + fkName + "='" + fkValue + "'";
            FKeyName = fkName;
            FKeyValue = fkValue;
        }

        public LFCObjectCollection(string s)
        {
            sql = s;
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
        public string FKeyName;
        public string FKeyValue;
        public bool IsChanged = false;
        SilverlightLFC.common.LFCDataService ls = new SilverlightLFC.common.LFCDataService();
        public List<T> ol=new List<T>();
        
        public T SelectedObject;
        public T ProcObject;
        public string sql;
        public int PerPageCount = 10;
        public int CurrentPage = 1;
        public int TotlePage;

        public List<T> getObjectList()
        {
            return ol;
        }

        public int getCount()
        {
            return ol.Count;
        }

        public bool SelectObject(T o)
        {
            if (ol.Contains(o))
            {
                SelectedObject = o;
                return true;
            }
            return false;
        }

        public List<T> getList()
        {
            return ol;
        }

        public void AddObject(T o)
        {
            ol.Add(o);
            IsChanged = true;
        }
        public void RemoveObject(T o)
        {
            if (ol.Contains(o))
            {
                ol.Remove(o);
                IsChanged = true;
            }
        }
        public void RemoveAllObject()
        {
            ol.Clear();
            IsChanged = true;
        }

        public void AddRelationObject(T o)
        {
            ol.Add(o);
            FieldInfo f = ls.getFieldByName(o, FKeyName);
            if (f != null)
            {
                f.SetValue(o, FKeyValue);
            }
            if (o.DataFlag == DataOperation.Nothing)
            {
                o.DataFlag = DataOperation.Update;
            }
            IsChanged = true;
        }

        public void RemoveRelationObject(T o)//对于关联的数据对象，只打标记，不真正的删除
        {
            if (ol.Contains(o))
            {
                //ol.Remove(o);
                IsChanged = true;
                FieldInfo f = ls.getFieldByName(o, FKeyName);
                if (f != null)
                {
                    f.SetValue(o, o.getUnAvailableIDValue());
                }
                if (o.DataFlag == DataOperation.Nothing)
                {
                    o.DataFlag = DataOperation.Update;
                }
            }
        }
        public void RemoveAllRelationObject()
        {
            foreach (T o in ol)
            {
                o.DataFlag = DataOperation.Delete;
                FieldInfo f = ls.getFieldByName(o, FKeyName);
                if (f != null)
                {
                    f.SetValue(o, o.getUnAvailableIDValue());
                }
            }
            IsChanged = true;
        }
        public void Clear()
        {
            ol.Clear();
            IsChanged = false;//表明调用该方法，该类和数据库数据脱离关系
        }
    }
}
