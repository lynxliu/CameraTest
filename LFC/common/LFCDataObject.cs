using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace SilverlightLFC.common
{

    public enum DataOperation//描述对数据库应该进行的操作，包括新建删除和更新
    {//应用这个标记，此时数据尚未进行更新到数据库的操作，操作更新完成，就标记为Nothing,Load代表完成
        New, Delete, Update, Nothing,NeedRead,NeedWrite

    }

    public enum CheckFlag//查询的类型，包括成功和失败以及未知
    {
        Success, Fail, Unknown
    }
    public abstract class AbstractLFCDataObject : IAbstractLFCDataObject, INotifyPropertyChanged//默认的可以识别的父类
    {
        public SilverlightLFC.common.LFCDataService lds = new SilverlightLFC.common.LFCDataService();

        #region InitEvent

        public void InitSubObjectEvent()
        {
            FieldInfo[] fl =new List<FieldInfo>(this.GetType().GetTypeInfo().DeclaredFields).ToArray();
            foreach (FieldInfo fi in fl)
            {
                if(fi.Name.Contains("_xo_"))
                {

                    object to = fi.GetValue(this);
                    if (to != null)
                    {
                        AbstractLFCDataObject lo = to as AbstractLFCDataObject;
                        if (lo != null)
                        {
                            lo.ObjctChanged += new LFCObjectChanged(lo_ObjctChanged);
                        }
                    }
                }
                if (fi.Name.Contains("_xi_"))
                {
                    object to = fi.GetValue(this);
                    Type[] typeArguments = to.GetType().GetTypeInfo().GenericTypeArguments;

                    if (to != null)
                    {
                        EventInfo ei = to.GetType().GetRuntimeEvent("LFCListObjectChanged");
                        if (ei != null)
                        {
                            ei.AddEventHandler(this,new LFCListItemChanged(ol_Changed));
                        }

                    }
                }
            }
        }

        void ol_Changed(IAbstractLFCDataObject sender, LFCObjChanged e)
        {
            if (ObjctChanged != null)
            {
                ObjctChanged(this, new LFCObjectChangedArgs(LFCObjChanged.ObjectChanged));
            }
        }

        void lo_ObjctChanged(object sender, LFCObjectChangedArgs e)
        {
            if (ObjctChanged != null)
            {
                ObjctChanged(this,new LFCObjectChangedArgs(LFCObjChanged.ObjectChanged));
            }
        }

        #endregion

        #region Basic Function//定义数据对象的基本参数和方法

        //public Boolean RelationObjectChanged = false; //记录是否修改过该对象关联对象的数值，如果修改了，那么就为true，否则为false
        //public Boolean DependenceObjectChanged = false;//记录是否修改过该对象的依赖对象列表里面的数值，如果修改了，为true

        public event LFCObjectChanged ObjctChanged;//操作状态改变，主要是焦点的改变
        public void sendObjctChanged(FieldInfo f,object v)
        {
            if (ObjctChanged != null)
            {
                ObjctChanged(this, new LFCObjectChangedArgs(f, v));
            }
        }
        public void sendObjctChanged(LFCObjChanged f, string n)
        {
            if (ObjctChanged != null)
            {
                ObjctChanged(this, new LFCObjectChangedArgs(f, n));
            }
            if (PropertyChanged != null)//兼容silverlight的系统数据绑定功能
            {
                PropertyChanged(this, new PropertyChangedEventArgs(n));
            }
        }

        //public event LynxValueChangedHandler ValueChanged;//定义字段值修改事件
        //public void sendValueChanged(FieldInfo f, object v)//给出被修改的字段值
        //{
        //    LynxValueChangedEventArgs leq = new LynxValueChangedEventArgs(false,f,v);
        //    if (ValueChanged != null)
        //    {
        //        ValueChanged(this, leq);
        //    }
        //}
        //public void sendValueChanged()//全部都修改了，适合调用完毕WriteToObject之后的情况
        //{
        //    LynxValueChangedEventArgs leq = new LynxValueChangedEventArgs(false);
        //    if (ValueChanged != null)
        //    {
        //        ValueChanged(this, leq);
        //    }
        //}
        //public void sendDeleted()//表示对象被删除
        //{
        //    LynxValueChangedEventArgs leq = new LynxValueChangedEventArgs(true);//表示对象被删除了
        //    if (ValueChanged != null)
        //    {
        //        ValueChanged(this, leq);
        //    }
        //}

        public event LynxCommonProcessHandler DataObjectProcComplete;//定义简单的查询事件
        public void sendProcEvent(bool r, string s,object o)//给出简单的查询返回，成功，失败和返回的结果集
        {
            LynxCommonProcessEventArgs leq = new LynxCommonProcessEventArgs(r, s,o);
            if (DataObjectProcComplete != null)
            {
                DataObjectProcComplete(this, leq);
            }
             
        }

        public abstract string ObjectID//对象标识，为null的时候是代表状态未确定
        {
            get;
            set;
        }

        public abstract void ClearObjectID();//清除本表的标识

        public virtual string getDatabaseIDFieldName()//给出数据库里面的ID字段名，默认就是_ID
        {
            return "_ID";
        }

        public bool IsLoaded()//判断是不是刚刚加载，或者保存完
        {
            if (DataFlag == DataOperation.Nothing) { return true; }
            return false;
        }

        public bool IsRelationObjectChanged(AbstractLFCDataObject lo)
        {
            if (lo == null) { return false; }
            if (lo.DataFlag == DataOperation.Nothing)
            {
                return false;
            }
            return true;
        }
        public bool IsDependenceListChanged(List<AbstractLFCDataObject> lol)
        {
            if (lol == null) { return false; }
            foreach (AbstractLFCDataObject lo in lol)
            {
                if (lo.DataFlag != DataOperation.Nothing)
                {
                    return false;
                }
            }
            return true;
        }

        public string getUnAvailableIDValue()//活动默认的非法的ID
        {
            return "";
        }

        public bool IsAvailableObjectID()//判断ID字段是否已经赋值
        {
            if (ObjectID == null || ObjectID == "")//ID是空或者是null
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public AbstractLFCDataObject ParentObject;//上一级AbstractDataObject。主要是XML之间的互相包含

        //public string objectName = "";//属性对象的名称，也就是字段名

        DataOperation _DataFlag = DataOperation.New;//说明数据库的操作标志，默认就是新建
        public DataOperation DataFlag
        {
            get { return _DataFlag; }
            set { _DataFlag = value; }
        }
        //数据加载以后就清除标记
        //关联表，针对多属性操作时候保证完整性
        public Dictionary<FieldInfo, CheckFlag> RelationTableStatus = new Dictionary<FieldInfo, CheckFlag>();//关联表列表，包括名称和状态


        #endregion
        public string BaseFileURL = "";
        public void getPath(List<AbstractLFCDataObject> PathList)
        {
            if (ParentObject == null)
            {
                PathList.Insert(0, this);
                return;
            }
            else
            {

                //PathList.Insert(0,ParentObject);
                PathList.Insert(0, this);
                ParentObject.getPath(PathList);
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;
        //public void NotifyPropertyChanged(string propertyName)//激活系统的数据绑定
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

    }
}
