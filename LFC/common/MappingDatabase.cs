using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.IO;

namespace SilverlightLFC.common
{
    public class MappingDatabase//该类负责映射对象和表结构
    {
        Dictionary<string, TableInfor> MapList=new Dictionary<string,TableInfor>();

        public string getTableName(string ObjName)//如果没有映射关系，那么就直接返回名称
        {
            if (MapList == null) { return ""; }
            if (MapList.ContainsKey(ObjName))
            {
                return MapList[ObjName].TableName;
            }
            else
            {
                return "";
            }
        }

        public string getTableName(AbstractDataObject o)
        {
            return getTableName(o.GetType().Name);
        }

        public bool IsMappingCol(string  ObjName)
        {
            if (MapList == null) { return false; }
            if (MapList.ContainsKey(ObjName))
            {
                if (MapList[ObjName].ColList == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsMappingCol(AbstractDataObject o)
        {
            return IsMappingCol(o.GetType().Name);
        }

        public string getMappingCol(string ObjName, string FieldName)
        {
            if (MapList == null) { return ""; }
            if (MapList.ContainsKey(ObjName))
            {
                if (MapList[ObjName] != null)
                {
                    if (MapList[ObjName].ColList != null)
                    {
                        if (MapList[ObjName].ColList.ContainsKey(FieldName))
                        {
                            return MapList[ObjName].ColList[FieldName];
                        }
                    }
                }
            }
            return "";
        }

        public string getMappingCol(AbstractDataObject o, FieldInfo f)
        {
            return getMappingCol(o.GetType().Name, f.Name);
        }

        public void ReadMapping()
        {


        }
    }

    public class TableInfor//记录表的结构
    {
        public string TableName;
        public Dictionary<string, string> ColList;
      
    }


}
