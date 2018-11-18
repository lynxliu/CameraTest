using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Context;
using System.IO;
using System.Xml;
using System.Linq;

namespace SilverlightLFC.common
{
    public class LFCDataService//该类提供LFC访问Webservice的基本能力，允许不通过LFCDataObject来使用，是V2版本LFC的核心
    {//不使用静态方法，防止出现同步的问题，sl是多线程的，同步问题非常严重，此类几乎是lfc的万能类
        #region Event//定义有关内部发送事件的参数和方法
        public event ProcessEventHandler ProcessQueryComplete;//定义查询类完毕的事件，异步事件返回值
        public event ExecuteQueryEventHandler ExecuteQuery;//定义简单的查询事件
        
        public void SendQueryResult(bool r, List<Dictionary<string, object>> dt)//给出简单的查询返回，成功，失败和返回的结果集
        {
            LFCExecuteQueryEventArgs leq = new LFCExecuteQueryEventArgs(r);
            leq.DataTable = dt;
            if (ExecuteQuery != null)
            {
                ExecuteQuery(this, leq);
            }
        }

        //发送失败事件
        public void SendErrorEvent(string Msg, string FunctionName, object tag, Dictionary<string, string> CallParameter)//引发一个附带数据的失败消息
        {
            LynxProcessCompleteEventArgs le = new LynxProcessCompleteEventArgs(false);
            le.CallFunctionName = FunctionName;//调用的方法
            le.Message = Msg;//文本的说明
            le.ReturnValue = tag;//返回值
            le.SenderObject = this;//表明谁引发了消息
            le.CallParameterList = CallParameter;
            if (ProcessQueryComplete != null)
            {
                ProcessQueryComplete(this, le);
            }
        }

        //发送成功事件
        public void SendSuccessEvent(string Msg, string FunctionName, object tag, Dictionary<string, string> CallParameter)//引发一个附带数据的失败消息
        {
            LynxProcessCompleteEventArgs le = new LynxProcessCompleteEventArgs(true);
            le.CallFunctionName = FunctionName;
            le.Message = Msg;
            le.ReturnValue = tag;
            le.SenderObject = this;
            le.CallParameterList = CallParameter;
            if (ProcessQueryComplete != null)
            {
                ProcessQueryComplete(this, le);
            }
        }

        #endregion

        #region Reflection//和反射有关的功能,

        public T CloneInfor<T>(T o) where T : AbstractLFCDataObject//复制一个一模一样的对象
        {
            //T xo = (T)Activator.CreateInstance(o.GetType());//从已经加载的类型信息创建实例，创建实例，赋给实例指针
            string s = Environment.Serialize(o);
            T xo=Environment.Deserialize(s,o.GetType()) as T;
            return xo;
        }

        public Dictionary<string, object> ListAllValue(Object o)//列出对象所有的属性名和属性值
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            FieldInfo f;
            String s = "";
            s = "This Class is " + o.GetType().Name + "\n";
            s = s + "this Class is based on" + o.GetType().GetTypeInfo().BaseType.Name + "\n";
            s = s + "This Class have these fields:\n{";
            var fields = o.GetType().GetRuntimeFields().ToList();
            for (int i = 0; i < fields.Count; i++)
            {
                f = fields[i];
                String fType, FValue, fName;
                fType = f.FieldType.Name;
                fName = f.Name;
                if (f.GetValue(o) == null)
                {
                    FValue = "Null";
                }
                else
                {
                    FValue = f.GetValue(o).ToString();
                }
                s = s + fType + " " + fName + "=" + FValue + "\n";
                d.Add(fName, FValue);
            }
            s = s + "}\n";

            return d;
        }

        public void ObjectValueClear(Object o)//逐个清除对象里面的数据，保证这个对象是新的
        {
            FieldInfo[] fields = o.GetType().GetRuntimeFields().ToArray();
            for (int i = 0; i < fields.Length; i++)
            {
                int flag = 0;
                FieldInfo f = fields[i];
                Type FT = f.FieldType;
                if (FT.Name.EndsWith("String") || FT.Name.EndsWith("string"))
                {
                    f.SetValue(o, "");
                    flag = 1;
                }
                if (FT.Name.EndsWith("Date") || FT.Name.EndsWith("date") || FT.Name.EndsWith("datetime") || FT.Name.EndsWith("DateTime"))
                {
                    f.SetValue(o, new DateTime());
                    flag = 1;
                }
                if (FT.Name.EndsWith("Int") || FT.Name.EndsWith("int") || FT.Name.EndsWith("long") || FT.Name.EndsWith("Long"))
                {
                    f.SetValue(o, 0);
                    flag = 1;
                }
                if (flag == 0)
                {
                    f.SetValue(o, null);
                }
            }

        }

        public string getFieldNameWithoutPrefix(string fname)//从对象的属性名得出真实的对象属性名称，也就是去掉所有前缀的名称
        {
            string FieldName;
            if (fname.Contains("_"))
            {
                int i = fname.LastIndexOf('_');
                FieldName = fname.Substring(i + 1);
                if (fname.Contains("xl_") && (FieldName.EndsWith("List")))
                {
                    FieldName = FieldName.Substring(0, FieldName.Length - 4);
                }

                return FieldName;
            }
            else
            {
                return fname;
            }
        }

        public Type FindType(Type[] tl, string name)
        {
            foreach (Type t in tl)
            {
                if (t.FullName.EndsWith("." + name))
                {
                    return t;
                }
            }
            return null;
        }//从一个指定的类型列表里面找到特定名称的类型


        public AbstractLFCDataObject getObjectByType(Type t)
        {
            return (AbstractLFCDataObject)Activator.CreateInstance(t);
        }

        public void setFieldValue(Object o, FieldInfo fi, object value)//对字段进行赋值
        {
            if (fi.FieldType.Name == "Int32")
            {
                if (value == null)
                {
                    fi.SetValue(o, 0);
                    return;
                }
                if (value.ToString() == "")//注意null会自动被处理为0
                {
                    fi.SetValue(o, 0);
                }
                else
                {
                    fi.SetValue(o, Convert.ToInt32(value));
                }
            }
            if (fi.FieldType.Name == "Int64")
            {
                if (value == null)
                {
                    fi.SetValue(o, 0);
                    return;
                }
                if (value.ToString() == "")//注意null会自动被处理为0
                {
                    fi.SetValue(o, 0);
                }
                else
                {
                    fi.SetValue(o, Convert.ToInt64(value));
                }
            }

            if (fi.FieldType.Name == "DateTime")
            {
                if (value == null)
                {
                    DateTime d = Environment.MinTime;

                    fi.SetValue(o, d);
                    return;
                }
                if (value.ToString() == "")
                {
                    DateTime d = Environment.MinTime;
                    d = Environment.MinTime;
                    fi.SetValue(o, d);
                }
                else
                {
                    fi.SetValue(o, Convert.ToDateTime(value));
                }
            }
            if (fi.FieldType.Name == "Decimal")
            {
                if (value == null)
                {
                    fi.SetValue(o, 0m);
                    return;
                }
                if (value.ToString() == "")//注意null会自动被处理为0
                {
                    fi.SetValue(o, 0m);
                }
                else
                {
                    fi.SetValue(o, Convert.ToDecimal(value));
                }
            }
            if (fi.FieldType.Name == "Double")
            {
                if (value == null)
                {
                    fi.SetValue(o, 0d);
                    return;
                }
                if (value.ToString() == "")//注意null会自动被处理为0
                {
                    fi.SetValue(o, 0d);
                }
                else
                {
                    fi.SetValue(o, Convert.ToDouble(value));
                }
            }

            if (fi.FieldType.Name == "String")
            {
                if (value == null)
                {
                    fi.SetValue(o, "");
                    return;
                }
                fi.SetValue(o, value.ToString().Trim());//需要注意，必须加trim来删除前后没有用的东西
            }

        }

        public FieldInfo getFieldByName(Object o, string Name)//依据名称得到该对象的字段，只要包含这个名字就可以
        {
            if (!Name.StartsWith("_")) { Name = "_" + Name; }
            Type t = o.GetType();
            FieldInfo[] fi = t.GetRuntimeFields().ToArray();
            foreach (FieldInfo f in fi)
            {
                if (f.Name.EndsWith(Name))//其名称必须包含下划线，同时以给定的名称作为结尾
                {
                    return f;
                }
            }
            return null;
        }

        public void getClassTree(Type ct, List<Type> lt)//得到一个从特定类递归到基类LFCDataObject的类名列表
        {
            if (ct.Name.StartsWith("Abstract"))
            {
            }
            else
            {
                lt.Add(ct);
                getClassTree(ct.GetTypeInfo().BaseType, lt);
            }
        }

        public string getFieldAbstractName(FieldInfo f)//得到字段的真正类型，此类型是属于LFCDataObject的名称
        {
            string ObjectName = "";
            if (f.FieldType.GetTypeInfo().IsGenericType)
            {
                Type[] tl = f.FieldType.GetTypeInfo().GenericTypeArguments;
                ObjectName = getFieldNameWithoutPrefix(tl[0].Name);
            }
            else
            {
                ObjectName = f.GetType().Name;
            }
            return ObjectName;
        }
        #endregion

    }
}
