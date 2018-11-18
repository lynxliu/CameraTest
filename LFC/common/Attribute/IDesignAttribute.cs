using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using System.Reflection;

namespace SilverlightLFC.common
{
    public interface IDesignCustomAttribute//支持设计器的对象应该实现的接口
    {
        //List<CustomDesignAttribute> getStandardAttributeList();//获得基本属性
        LFCObjectList<CustomDesignAttribute> CustomAttributeList { get; }//获取用户属性
        //bool IsEnableCustomAttrbute { get; }//是否允许属性自定义，该属性有类来定义，只读
        event LFCObjectChanged ObjctChanged;
    }


    public class CustomDesignAttribute:SilverlightLFC.common.AbstractLFCDataObject//可以在属性编辑器里面编辑的属性可以保存
    {//专门用于扩展属性，扩展属性只支持String格式保存，不支持其他的类型
        public string _xt_db_ID;
        override public String ObjectID
        {
            get
            {
                return this._xt_db_ID;
            }
            set
            {
                this._xt_db_ID = value;
            }
        }
        override public void ClearObjectID()
        {
            this._xt_db_ID = "";
        }
        //public byte IsReadOnly//判断是否是只读属性
        //{
        //    get { return _xt_db_IsReadOnly; }
        //    set
        //    {
        //        _xt_db_IsReadOnly = value;
        //        DataFlag = DataOperation.Update;
        //        sendObjctChanged(LFCObjChanged.ObjectChanged, "IsReadOnly");
        //    }
        //}
        //public IDesignCustomAttribute TargetObject;//目标对象，不能为空
        //public PropertyInfo PropertyField=null;//目标属性，为空表示是自定义属性
        //public string Key//判断属性唯一值的标识，key相同代表值相同
        //{
        //    get;
        //    set;
        //}
        public string Name//属性名称
        {
            get { return _xt_db_AttributeName; }
            set { _xt_db_AttributeName = value;
            DataFlag = DataOperation.Update;
            sendObjctChanged(LFCObjChanged.ObjectChanged, "Name");
            }
        }

        //public string Max//为数值型的时候最大
        //{
        //    get { return _xt_db_Max; }
        //    set
        //    {
        //        _xt_db_Max = value;
        //        DataFlag = DataOperation.Update;
        //        sendObjctChanged(LFCObjChanged.ObjectChanged, "Max");
        //    }
        //}
        //public string Min//为数值型的时候最大
        //{
        //    get { return _xt_db_Min; }
        //    set
        //    {
        //        _xt_db_Min = value;
        //        DataFlag = DataOperation.Update;
        //        sendObjctChanged(LFCObjChanged.ObjectChanged, "Min");
        //    }
        //}

        public string Value//属性值
        {
            get
            {
                return _xt_db_AttributeValue;
            }
            set
            {
                _xt_db_AttributeValue = value;
                DataFlag = DataOperation.Update;
                sendObjctChanged(LFCObjChanged.ObjectChanged, "Value");
            }
        }

        public string ValueTypeName//数值的类型
        {
            get
            {
                return _xt_db_ValueTypeName;
            }
            set
            {
                _xt_db_ValueTypeName = value;
                DataFlag = DataOperation.Update;
                sendObjctChanged(LFCObjChanged.ObjectChanged, "ValueTypeName");
            }

        }
        public string Memo//说明
        {
            get { return _xt_db_AttributeMemo; }
            set
            {
                _xt_db_AttributeMemo = value;
                DataFlag = DataOperation.Update;
                sendObjctChanged(LFCObjChanged.ObjectChanged, "Memo");
            }
        }
        //public string Type//属性类别，判断是不是自定义类型
        //{
        //    get { return _xt_db_AttributeType; }
        //    set
        //    {
        //        _xt_db_AttributeType = value;
        //        DataFlag = DataOperation.Update;
        //        sendObjctChanged(LFCObjChanged.ObjectChanged, "Type");
        //    }
        //}

        //public Dictionary<string, object> AttributeValueArea;//键值对，帮助显示取值范围的
        //专门使用combox来进行简单的显示

        public string _xt_db_AttributeName="UnNamed";
        public string _xt_db_AttributeValue;//目前自定义属性只能允许字符串
        public string _xt_db_ValueTypeName;
        public string _xt_db_AttributeMemo;
        //public string _xt_db_AttributeType="CustomerAttribute";

        //public string _xt_db_Max = long.MaxValue.ToString();
        //public string _xt_db_Min = long.MinValue.ToString();
        //public byte _xt_db_IsReadOnly = 0;
        //public getAttributeObjectAvailableList GetAttributeObjectAvailableList;//用于列表选择的对象列表，包括显示对象的详细信息
        //public createNewObject CreateNewTargetObject;//创建新的可能对象
        //public deleteObject DeleteObject;//删除可能的对象
        //public addTargetObject AddTargetObject;//把指定的对象增加到列表里面，只对列表有效
        //public removeTargetObject RemoveTargetObject;//从列表里面删除指定对象，只对列表有效

    }
    //public delegate Dictionary<string, object> getAttributeObjectAvailableList();//获取可能的对象列表
    //public delegate KeyValuePair<string, object> createNewObject();//创建新的可能对象
    //public delegate bool deleteObject(KeyValuePair<string,object> to);//删除可能的对象
    //public delegate bool addTargetObject(KeyValuePair<string,object> to);//把指定的对象增加到列表里面，只对列表有效
    //public delegate bool removeTargetObject(KeyValuePair<string, object> to);//从列表里面删除指定对象，只对列表有效
}
