using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Reflection;

namespace SilverlightLFC.common
{
    public class LynxDesignAttribute : System.Attribute
    {
        public double? Max { get; set; }
        public double? Min { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        bool ReadOnly = false;
        public bool IsReadyOnly
        {
            get { return ReadOnly; }
            set { ReadOnly = value; }
        }

        public string Author = "Lynx(lynxliu2002@hotmail.com)";
        public decimal Version = 0.7m;
        public DateTime CreateTime = new DateTime(2011, 7, 17);

        Dictionary<string, object> _ValueArea = new Dictionary<string, object>();
        public Dictionary<string,object> ValueArea
        {
            get { return _ValueArea; }
            set { _ValueArea = value; }
        }

        //获取任意的对象定义的内置特征属性，属性作为键值
        public static Dictionary<PropertyInfo, LynxDesignAttribute> getEditDesignAttribute(object o)
        {
            Dictionary<PropertyInfo, LynxDesignAttribute> tpl = new Dictionary<PropertyInfo, LynxDesignAttribute>();
            PropertyInfo[] pl = o.GetType().GetRuntimeProperties().ToArray();
            foreach (PropertyInfo p in pl)
            {
                System.Attribute[] definedAttributes =p.GetCustomAttributes(typeof(LynxDesignAttribute)).ToArray();
                if (definedAttributes.Length > 0)
                {
                    tpl.Add(p, definedAttributes[0] as LynxDesignAttribute);
                }
            }
            return tpl;
        }


    }
}
