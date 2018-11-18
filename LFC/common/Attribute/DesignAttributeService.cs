using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;

namespace SilverlightLFC.common.Attribute
{
    public class DesignAttributeService//提供对支持设计属性的对象进行服务，提供列表和查询属性的方法
    {
        public string msg="";
        public bool AddCustomAttribute(IDesignCustomAttribute ida, CustomDesignAttribute v)
        {
            //if (ida.IsEnableCustomAttrbute == false) { return false; }
            foreach (CustomDesignAttribute d in ida.CustomAttributeList)
            {
                if (d.Name == v.Name)
                {
                    msg = v.Name + "已经存在";
                    return false;
                }
            }
            ida.CustomAttributeList.Add(v);
            msg = "添加完成";
            //getAttributeList(ida).Add(v);
            return true;
        }
        public bool RemoveCustomAttribute(IDesignCustomAttribute ida, CustomDesignAttribute v)
        {
            //if (ida.IsEnableCustomAttrbute == false) { return false; }
            foreach (CustomDesignAttribute d in ida.CustomAttributeList)
            {
                if (d.Name == v.Name)
                {
                    ida.CustomAttributeList.Remove(d);
                    return true;
                }
            }
            msg = "要删除对象不存在";
            return false;
        }
        //public List<CustomDesignAttribute> getCustomAttributeList(IDesignCustomAttribute ida)//获取全部属性
        //{
        //    List<CustomDesignAttribute> AttrList = new List<CustomDesignAttribute>();
        //    //foreach (CustomDesignAttribute d in ida.getStandardAttributeList())
        //    //{
        //    //    AttrList.Add(d);
        //    //}
        //    if (ida.IsEnableCustomAttrbute)
        //    {
        //        foreach (CustomDesignAttribute d in ida.CustomAttributeList)
        //        {
        //            AttrList.Add(d);
        //        }
        //    }
        //    return AttrList;
        //}
        public CustomDesignAttribute getAttribute(IDesignCustomAttribute iao, string name)//依据属性名称获得特定的属性
        {
            foreach (CustomDesignAttribute da in iao.CustomAttributeList)
            {
                if (da.Name == name)
                {
                    return da;
                }
            }
            return null;
        }

        public void setAttribute(IDesignCustomAttribute iao, string name, string value)//给属性赋值
        {
            CustomDesignAttribute da = getAttribute(iao,name);
            if (da != null)
            {
                da.Value = value;
                //da.PropertyField.SetValue(da.TargetObject, value,null);//给目标对象的目标字段赋值
            }
        }

        //public void setAttribute(CustomDesignAttribute da)//给属性赋值
        //{
        //    setAttribute(da.TargetObject, da.Name, da.Value);
        //}

        public string getAttributeInfor(IDesignCustomAttribute ida)
        {
            string s = "";
            foreach (CustomDesignAttribute da in ida.CustomAttributeList)
            {
                s = s + da.Name + ":" + da.Value.ToString()+"\n";
            }
            return s;
        }

    }

}
