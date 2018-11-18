using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml;

namespace SilverlightLFC.common
{
    public abstract class AbstractDataObject//支持SilveLight的父类，完成支持数据库访问和xml串行化
    {
        #region Event//定义有关内部发送事件的参数和方法
            public event ProcessEventHandler ProcessQueryComplete;//定义查询类完毕的事件，异步事件返回值

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

        #region Basic Function//定义数据对象的基本参数和方法

            public Boolean Changed = false; //记录是否修改过对象的数值，如果修改了，那么就为true，否则为false

            public abstract string ObjectID//对象标识
        {
            get;
            set;
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

            public string procmessage;//用来记录对象处理信息的辅助变量，对象的方法处理结果参数可以记录到此

            public string getProcMessage()
        {
            return procmessage;
        }//得到当前处理的消息

            public DateTime _db_xa_createTime = DateTime.Now;//对象建立的时间，所有的数据记录都需要包含
            public DateTime _db_xa_lastUpdateTime;//最终的修改时间，所有的数据记录都需要包含

            public DateTime CreateTime
            {
                get
                {
                    if (_db_xa_createTime < common.Environment.MinTime)
                    {
                        _db_xa_createTime = common.Environment.MinTime;
                    }
                    return _db_xa_createTime;
                }
                set
                {
                    _db_xa_createTime = value;
                    Changed = true;
                }
            }

            public DateTime LastUpdateTime
            {
                get
                {
                    if (_db_xa_lastUpdateTime < common.Environment.MinTime)
                    {
                        _db_xa_lastUpdateTime = common.Environment.MinTime;
                    }
                    return _db_xa_lastUpdateTime;
                }
                set
                {
                    _db_xa_lastUpdateTime = value;
                    Changed = true;
                }
            }

        #endregion

        #region Reflection//和反射有关的功能

            public AbstractDataObject CloneInfor()//复制一个一模一样的对象
            {
                AbstractDataObject xo = (AbstractDataObject)Activator.CreateInstance(GetType());//从已经加载的类型信息创建实例，创建实例，赋给实例指针
                string s = WriteToXMLString();
                xo.ReadFromXMLString(s);
                return xo;
            }

            public Dictionary<string, object> ListAllValue()//列出对象所有的属性名和属性值
            {
                Dictionary<string, object> d = new Dictionary<string, object>();
                FieldInfo f;
                String s = "";
                s = "This Class is " + this.GetType().Name + "\n";
                s = s + "this Class is based on" + this.GetType().GetTypeInfo().BaseType.Name + "\n";
                s = s + "This Class have these fields:\n{";
                FieldInfo[] fields = this.GetType().GetRuntimeFields().ToArray();
                for (int i = 0; i < fields.Length; i++)
                {
                    f = fields[i];
                    String fType, FValue, fName;
                    fType = f.FieldType.Name;
                    fName = f.Name;
                    if (f.GetValue(this) == null)
                    {
                        FValue = "Null";
                    }
                    else
                    {
                        FValue = f.GetValue(this).ToString();
                    }
                    s = s + fType + " " + fName + "=" + FValue + "\n";
                    d.Add(fName, FValue);
                }
                s = s + "}\n";

                return d;
            }

            public void ObjectValueClear()//逐个清除对象里面的数据，保证这个对象是新的
            {
                FieldInfo[] fields = this.GetType().GetRuntimeFields().ToArray();
                for (int i = 0; i < fields.Length; i++)
                {
                    int flag = 0;
                    FieldInfo f = fields[i];
                    Type FT = f.FieldType;
                    if (FT.Name.EndsWith("String") || FT.Name.EndsWith("string"))
                    {
                        f.SetValue(this, "");
                        flag = 1;
                    }
                    if (FT.Name.EndsWith("Date") || FT.Name.EndsWith("date") || FT.Name.EndsWith("datetime") || FT.Name.EndsWith("DateTime"))
                    {
                        f.SetValue(this, new DateTime());
                        flag = 1;
                    }
                    if (FT.Name.EndsWith("Int") || FT.Name.EndsWith("int") || FT.Name.EndsWith("long") || FT.Name.EndsWith("Long"))
                    {
                        f.SetValue(this, 0);
                        flag = 1;
                    }
                    if (flag == 0)
                    {
                        f.SetValue(this, null);
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
                    return null;
                }
            }

            public Type FindType(Type[] tl,string name)
            {
                foreach (Type t in tl)
                {
                    if (t.FullName.EndsWith("."+name))
                    {
                        return t;
                    }
                }
                return null;
            }//从一个指定的类型列表里面找到特定名称的类型

            public AbstractDataObject getObjectByTypeName(string Name)//从名称加载类对象，默认是和本对象在一个命名空间里面的
            {
                var o= Activator.CreateInstance(Type.GetType(Name)) ;
                if (o != null) return o as AbstractDataObject;
                return null;
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
                if (fi.FieldType.Name == "Single")
                {
                    if (value == null)
                    {
                        fi.SetValue(o, 0f);
                        return;
                    }
                    if (value.ToString() == "")//注意null会自动被处理为0
                    {
                        fi.SetValue(o, 0f);
                    }
                    else
                    {
                        fi.SetValue(o, Convert.ToSingle(value));
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

            public void setFieldValue(FieldInfo fi, object value)//对本对象字段进行赋值
            {
                setFieldValue(this, fi, value);
            }

            public FieldInfo getFieldByName(string Name)//依据名称得到该对象的字段，只要包含这个名字就可以
            {
                if (!Name.StartsWith("_")) { Name = "_" + Name; }
                Type t = this.GetType();
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

            public void getClassTree(Type ct, List<Type> lt)//得到一个从特定类递归到基类AbstractDataObject的类名列表
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

            public string getFieldAbstractName(FieldInfo f)//得到字段的真正类型，此类型是属于AbstractDataObject的名称
            {
                string ObjectName = "";
                if (f.FieldType.IsConstructedGenericType)
                {
                    Type[] tl = f.FieldType.GenericTypeArguments;
                    ObjectName = getFieldNameWithoutPrefix(tl[0].Name);
                }
                else
                {
                    ObjectName = f.GetType().Name;
                }
                return ObjectName;
            }
        #endregion

        #region XMLOperation//有关XML的操作

            public bool IsXMLAttribute(string name)//判断是不是一个属性的名字
        {
            if (name.Contains("xa_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

            public bool IsXMLTag(string name)//判断是不是一个标签的名字
        {
            if (name.Contains("xt_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

            public bool IsXMLSubObject(string name)//判断是不是一个子对象的名字
        {
            if (name.Contains("xo_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

            public bool IsXMLSubObjectList(string name)//判断是不是一个子对象列表的名字
        {
            if (name.Contains("xl_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

            public bool IsStringList(string name)//判断是不是一个字符串对象列表的名字
        {
            if (name.Contains("sl_"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

            public AbstractDataObject ParentObject;//上一级AbstractDataObject。主要是XML之间的互相包含

            public void getPath(List<AbstractDataObject> PathList)//得到对象之间的包含关系
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

            public string WriteXMLDataList(List<AbstractDataObject> l)//得到一个对象集合的XML表示
        {
            if (l.Count == 0) { return ""; }
            AbstractDataObject t = l[0];
            string TypeName = t.GetType().Name;
            string s = "<List_" + TypeName + ">";
            for (int i = 0; i < 15; i++)
            {
                t = l[i];
                s = s + t.WriteToXMLString();
            }
            s = s + "</List_" + TypeName + ">";

            return s;
        }

            public List<AbstractDataObject> ReadXMLDataList(string XMLTableList)//XMLTableList是列表字符串，符合xml格式，i是需要加载第几个对象
            {
                //XmlDocument x = new XmlDocument();
                //x.LoadXml(XMLTableList);
                //XmlNodeList xl = x.GetElementsByTagName(this.GetType().Name);
                //if (xl.Count <= i) { return; }
                //XmlNode n = xl[i];
                //LoadFromXMLString(n.OuterXml);//只有OuterXML才是带元素标签的，才是完整的
                if (!XMLTableList.StartsWith("<List_" + this.GetType().Name + ">")) { return null; }//必须带完整的List标签
                if (!XMLTableList.EndsWith("</List_" + this.GetType().Name + ">")) { return null; }

                Type t = this.GetType();
                FieldInfo[] fi = t.GetRuntimeFields().ToArray();
                StringReader xs = new StringReader(XMLTableList);
                XmlReader reader = XmlReader.Create(xs);

                bool tb = reader.ReadToFollowing(this.GetType().Name);
                if (!tb) { return null; }
                List<AbstractDataObject> xl = new List<AbstractDataObject>();
                bool IsEnd = false;
                do//至少执行一次，因为已经发现了一个数据
                {
                    Type st = GetType();
                    AbstractDataObject xo = (AbstractDataObject)Activator.CreateInstance(st);
                    string ss = reader.ReadOuterXml();

                    xo.ReadFromXMLString(ss);
                    xl.Add(xo);

                    //reader.Skip();
                    if (reader.NodeType == XmlNodeType.EndElement) { IsEnd = true; }
                } while (!IsEnd);

                return xl;
            }

            public string _xa_objectName = "";//对象的名称

            public string WriteToXMLString()//依据对象信息创建新的xml字符串，同时，这也是串行化对象的方法
            //原则是，属性归属性，标签归标签，子对象归子对象，子列表归子列表
            {
                string sAttr = "";
                string sInterXML = "";
                Type t = this.GetType();
                FieldInfo[] fi = t.GetRuntimeFields().ToArray();
                for (int i = 0; i < fi.Length; i++)
                {
                    string tname = fi[i].Name;
                    if (this.IsStringList(tname))
                    {
                        //如果是子对象列表的保存处理
                        List<Object> al;
                        if ((fi[i].GetValue(this) == null) || ((List<Object>)(fi[i].GetValue(this))).Count == 0)//表示这个列表是空的，什么都没有，这个时候初始化为空列表
                        {
                        }
                        else
                        {

                            al = (List<Object>)fi[i].GetValue(this);
                            for (int j = 0; j < al.Count; j++)
                            {
                                //AbstractDataObject ao = (AbstractDataObject)al[j];
                                //ao.xa_objectName = tname.Substring(tname.LastIndexOf("_") + 1);//提示该类型的数据对应父类的那个属性
                                string v = al[j].ToString();
                                sInterXML = sInterXML + "<" + getFieldNameWithoutPrefix(tname) + " objectName='" + tname.Substring(tname.LastIndexOf("_") + 1) + "'>" + v + "</" + getFieldNameWithoutPrefix(tname) + ">";
                            }
                        }
                    }
                    if (this.IsXMLSubObjectList(tname))
                    {
                        //如果是子对象列表的保存处理
                        List<Object> al;
                        if ((fi[i].GetValue(this) == null) || ((List<Object>)(fi[i].GetValue(this))).Count == 0)//表示这个列表是空的，什么都没有，这个时候初始化为空列表
                        {
                        }
                        else
                        {

                            al = (List<Object>)fi[i].GetValue(this);
                            for (int j = 0; j < al.Count; j++)
                            {
                                AbstractDataObject ao = (AbstractDataObject)al[j];
                                ao._xa_objectName = tname.Substring(tname.LastIndexOf("_") + 1);//提示该类型的数据对应父类的那个属性
                                sInterXML = sInterXML + ao.WriteToXMLString();
                            }
                        }
                    }
                    if (this.IsXMLSubObject(tname))
                    {
                        //如果是子对象的保存处理,核心是处理完以后，能够用子对象的datanode更新自己的datanode的对应节点
                        //先更新子对象，再获取其datanode进行赋值
                        if (fi[i].GetValue(this) == null) { }//如果值是null表示没有赋值过
                        else
                        {
                            AbstractDataObject ao = (AbstractDataObject)fi[i].GetValue(this);
                            ao._xa_objectName = tname.Substring(tname.LastIndexOf("_") + 1);//提示该类型的数据对应父类的那个属性
                            sInterXML = sInterXML + ao.WriteToXMLString();
                        }
                    }
                    if (this.IsXMLTag(tname))
                    {
                        //如果是标签的保存处理
                        string v = null;
                        if (fi[i].GetValue(this) == null) { }
                        else
                        {
                            v = fi[i].GetValue(this).ToString();
                        }
                        sInterXML = sInterXML + "<" + getFieldNameWithoutPrefix(tname) + ">" + v + "</" + getFieldNameWithoutPrefix(tname) + ">";
                    }
                    if (this.IsXMLAttribute(tname))
                    {
                        //如果是属性的保存处理
                        string v = null;
                        if (fi[i].GetValue(this) == null) { }
                        else
                        {
                            v = fi[i].GetValue(this).ToString();
                        }
                        sAttr = sAttr + " " + getFieldNameWithoutPrefix(tname) + "=\"" + v + "\"";

                    }

                }
                string s = "<" + t.Name + sAttr + ">" + sInterXML + "</" + t.Name + ">";
                return s;
            }

            public void ReadFromXMLString(string s)//最高效率的访问XML文本的方法SAX，串行的一次性访问，并且加载其中的信息
        {
            Type t = this.GetType();
            FieldInfo[] fi = t.GetRuntimeFields().ToArray();

            //List<FieldInfo> li = new List<FieldInfo>(fi);//保留该类的全部可能字段
            List<FieldInfo> lixt = new List<FieldInfo>();
            List<FieldInfo> lixo = new List<FieldInfo>();
            List<FieldInfo> lixl = new List<FieldInfo>();
            List<FieldInfo> lixa = new List<FieldInfo>();
            List<FieldInfo> lisl = new List<FieldInfo>();
            foreach (FieldInfo f in fi)
            {
                string FieldName = f.Name;
                if (FieldName.Contains("xl_"))
                {
                    lixl.Add(f);
                }
                if (FieldName.Contains("xo_"))
                {
                    lixo.Add(f);
                }
                if (FieldName.Contains("xt_"))
                {
                    lixt.Add(f);
                }
                if (FieldName.Contains("xa_"))
                {
                    lixa.Add(f);
                }
                if (FieldName.Contains("sl_"))
                {
                    lisl.Add(f);
                }

            }

            StringReader xs = new StringReader(s);
            XmlReader reader = XmlReader.Create(xs);
            if (!reader.ReadToFollowing(this.GetType().Name)) { return; }//找到该类的标签，如果没有，那么没法加载
            //如果找到了该类的标签，那么就到该类的第一个元素
            if (reader.HasAttributes)//加载该标签的属性
            {
                string an, av;
                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToAttribute(i);
                    an = reader.Name;
                    av = reader.Value;
                    foreach (FieldInfo f in lixa)
                    {
                        if (getFieldNameWithoutPrefix(f.Name) == an)
                        {
                            setFieldValue(f, av);
                            break;
                            //f.SetValue(this, av);
                        }
                    }
                }

            }

            bool IsNotEnd = reader.Read();
            while (IsNotEnd)//进入当前的对象进行读取，主要是子标签
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        FieldInfo tf = null;
                        string tag = reader.Name;

                        if (!reader.HasAttributes)//表示这是一个普通的Tag
                        {
                            tf = getXMLTagFieldByName(tag, lixt);
                            if (tf != null)
                            {
                                lixt.Remove(tf);
                                setFieldValue(tf, reader.ReadElementContentAsString());
                                //reader.ReadElementContentAsString();
                            }
                            else
                            {
                                reader.ReadOuterXml();
                            }
                        }
                        else
                        //表示这是一个xo或者xl或者sl
                        {
                            reader.MoveToAttribute("objectName");
                            string ObjName = reader.Value;
                            reader.MoveToElement();
                            //if (ObjName == "") { ObjName = "List"; }
                            tf = getXMLObjectFieldByName(tag, ObjName, lixo);//先尝试有没有子对象
                            if (tf == null)//表示没有这个子对象
                            {

                                tf = getXMLListFieldByName(tag, ObjName, lixl);//再尝试有没有对象列表
                                if (tf == null)
                                {
                                    tf = getXMLStringListFieldByName(tag, ObjName, lisl);//再尝试有没有字符串列表
                                    if (tf == null)
                                    {
                                        reader.ReadOuterXml();
                                    }
                                    else
                                    {
                                        List<Object> xl;
                                        if (tf.GetValue(this) == null)
                                        {
                                            xl = new List<Object>();
                                            tf.SetValue(this, xl);
                                        }
                                        else
                                        {
                                            xl = (List<Object>)tf.GetValue(this);
                                        }
                                        string v = reader.ReadElementContentAsString();
                                        xl.Add(v);
                                        //tf.SetValue(this, xl);

                                    }
                                }
                                else//表示是个xl
                                {
                                    //lixl.Remove(tf);
                                    List<Object> xl;
                                    if (tf.GetValue(this) == null)
                                    {
                                        xl = new List<Object>();
                                        tf.SetValue(this, xl);
                                    }
                                    else
                                    {
                                        xl = (List<Object>)tf.GetValue(this);
                                    }
                                    string ctag = reader.Name;
                                    AbstractDataObject xo = getObjectByTypeName(ctag);
                                    string ss = reader.ReadOuterXml();
                                    //xo._XMLString = ss;
                                    xo.ReadFromXMLString(ss);
                                    xl.Add(xo);
                                    xo.ParentObject = this;


                                }
                            }
                            else
                            {
                                lixo.Remove(tf);
                                string ctag = reader.Name;
                                string ss = reader.ReadOuterXml();
                                AbstractDataObject xo = getObjectByTypeName(ctag);
                                xo.ReadFromXMLString(ss);
                                //xo._XMLString = ss;
                                tf.SetValue(this, xo);
                                xo.ParentObject = this;

                            }
                        }
                        //if (tf == null)
                        //{
                        //    //reader.ReadToNextSibling(reader.Name);
                        //    reader.ReadOuterXml();
                        //}//表示虽然XML文件里面有，但是对象里面没有这个信息，直接忽略
                        //else
                        //{
                        //li.Remove(tf);
                        //if (tf.Name.Contains("xa_"))
                        //{
                        //    //tf.SetValue(this, reader.ReadElementContentAsString());
                        //    reader.ReadElementContentAsString();

                        //    //reader.ReadEndElement();
                        //}

                        //if (tf.Name.Contains("xt_"))
                        //{
                        //    //tf.SetValue(this, reader.ReadElementContentAsString());
                        //    setXMLFieldValue(tf, reader.ReadElementContentAsString());

                        //    //reader.ReadEndElement();
                        //}
                        //if (tf.Name.Contains("xo_"))
                        //{
                        //    string ctag = reader.Name;
                        //    string ss = reader.ReadOuterXml();
                        //    //String NameSpace = this.GetType().Namespace;
                        //    //Type st = Type.GetType(NameSpace + "." + reader.Name);//从名称获得类型信息，如果获取失败t就是null
                        //    //AbstractDataObject xo = (AbstractDataObject)Activator.CreateInstance(st);//从已经加载的类型信息创建实例，创建实例，赋给实例指针
                        //    AbstractDataObject xo = getObjectByTypeName(ctag);
                        //    xo.ReadFromXMLString(ss);
                        //    xo._XMLString = ss;
                        //    tf.SetValue(this, xo);
                        //    xo.ParentObject = this;
                        //    //reader.Skip();
                        //    if (reader.NodeType == XmlNodeType.EndElement) { IsNotEnd = false; }

                        //    //reader.ReadEndElement();

                        //}
                        //if (tf.Name.Contains("xl_"))
                        //{
                        //    //List<AbstractDataObject> xl;
                        //    ArrayList xl;
                        //    if (tf.GetValue(this) == null)
                        //    {
                        //        xl = new ArrayList();
                        //    }
                        //    else
                        //    {
                        //        xl = (ArrayList)tf.GetValue(this);
                        //    }
                        //    string ctag = reader.Name;
                        //    //String NameSpace = this.GetType().Namespace;
                        //    //Type st = Type.GetType(NameSpace + "." + reader.Name);//从名称获得类型信息，如果获取失败t就是null
                        //    //AbstractDataObject xo = (AbstractDataObject)Activator.CreateInstance(st);//从已经加载的类型信息创建实例，创建实例，赋给实例指针
                        //    AbstractDataObject xo = getObjectByTypeName(ctag);
                        //    string ss = reader.ReadOuterXml();
                        //    xo._XMLString = ss;
                        //    xo.ReadFromXMLString(ss);
                        //    xl.Add(xo);
                        //    //reader.ReadEndElement();
                        //    xo.ParentObject = this;

                        //    tf.SetValue(this, xl);
                        //    //reader.Skip();
                        //    if (reader.NodeType == XmlNodeType.EndElement) { IsNotEnd = false; }

                        //}
                        //foreach (FieldInfo f in fi)
                        //{
                        //    if (getFieldName(f.Name) == reader.Name)
                        //    {
                        //        setXMLFieldValue(f, reader.Value);
                        //    }
                        //}
                        //if (reader.HasAttributes)//标识还有额外的属性
                        //{
                        //    string an, av;
                        //    for (int i = 0; i < reader.AttributeCount; i++)
                        //    {
                        //        reader.MoveToAttribute(i);
                        //        an = reader.Name;
                        //        av = reader.Value;
                        //        foreach (FieldInfo f in fi)
                        //        {
                        //            if (getFieldName(f.Name) == an)
                        //            {
                        //                setXMLFieldValue(f, av);
                        //            }
                        //        }
                        //    }
                        //}
                        //}
                        //IsNotEnd = reader.Read();
                        break;
                    case XmlNodeType.Text:
                        reader.ReadOuterXml();
                        break;
                    case XmlNodeType.XmlDeclaration:
                        reader.ReadOuterXml();
                        break;

                    case XmlNodeType.ProcessingInstruction:
                        reader.ReadOuterXml();
                        break;
                    case XmlNodeType.Comment:
                        reader.ReadOuterXml();
                        break;
                    case XmlNodeType.Attribute:
                        reader.Read();
                        break;
                    case XmlNodeType.Whitespace:
                        reader.Read();
                        break;

                    case XmlNodeType.EndElement:
                        IsNotEnd = false;
                        break;
                }

            }
            //_XMLString = "";//加载完毕就清空了
            //Changed = false;
        }

            public FieldInfo getXMLTagFieldByName(string Name, List<FieldInfo> fi)//依据名称得到一个XML字段
            {
                if ((Name == null) || (Name == "")) { return null; }
                foreach (FieldInfo f in fi)
                {
                    if (f.Name.Contains("xt_") && f.Name.Contains(Name))
                    {
                        return f;
                    }

                }
                return null;
            }

            public FieldInfo getXMLListFieldByName(string Name, string objName, List<FieldInfo> fi)//依据名称得到一个XML列表字段
            {//objName必须也是从AbstractDataObject类继承的
                if (objName == null) { objName = ""; }
                if ((Name == null) || (Name == "")) { return null; }
                //Type t = this.GetType();
                //FieldInfo[] fi = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default);
                foreach (FieldInfo f in fi)
                {
                    string FieldType = f.FieldType.Name;
                    string FieldName = f.Name;
                    if (FieldName.Contains("xl_"))//只依赖前缀来进行判断
                    {
                        if (FieldName.Contains(objName) && (FieldName.Contains(Name)))
                        {
                            return f;
                        }

                        if ((objName == "") && (FieldName.Contains(Name)))
                        {
                            return f;
                        }
                    }

                }
                return null;
            }

            public FieldInfo getXMLObjectFieldByName(string Name, string objName, List<FieldInfo> fi)//依据名称得到一个XML对象字段
            {
                if (objName == null) { objName = ""; }
                if ((Name == null) || (Name == "")) { return null; }
                foreach (FieldInfo f in fi)
                {
                    string FieldType = f.FieldType.Name;
                    string FieldName = f.Name;
                    if ((FieldType == Name) && (FieldName.Contains("xo_")))
                    {
                        if ((objName == "") && (FieldName.Contains(Name)))
                        {
                            return f;
                        }
                        if (FieldName.Contains(objName))
                        {
                            return f;
                        }

                    }
                }
                return null;
            }

            public FieldInfo getXMLStringListFieldByName(string Name, string objName, List<FieldInfo> fi)//得到一个字符串列表字段
            {
                if (objName == null) { objName = ""; }
                if ((Name == null) || (Name == "")) { return null; }
                //Type t = this.GetType();
                //FieldInfo[] fi = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default);
                foreach (FieldInfo f in fi)
                {
                    string FieldType = f.FieldType.Name;
                    string FieldName = f.Name;
                    if ((FieldType == "ArrayList") && (FieldName.Contains("sl_")))
                    {
                        if (FieldName.Contains(objName) && (FieldName.Contains(Name)))
                        {
                            return f;
                        }

                    }

                }
                return null;
            }

        #endregion

        #region DatabaseOperation//数据库操作标志相关操作，数据库关联列表状态的操作

            public DataOperation DatabaseFlag;//说明数据库的操作标志

            //关联表，针对多属性操作时候保证完整性
            Dictionary<FieldInfo, CheckFlag> RelationTableStatus = new Dictionary<FieldInfo, CheckFlag>();//关联表列表，包括名称和状态

            //检查是否完成
            public bool IsRelationTableTestComplete()//判断关联的列表对象是不是完成了检查
            {
                foreach (KeyValuePair<FieldInfo, CheckFlag> r in RelationTableStatus)
                {
                    if (r.Value == CheckFlag.Unknown)
                    {
                        return false;
                    }
                }
                return true;
            }

            //检查是否成功
            public bool IsRelationTableTestSuccess()//判断关联的列表对象是完成检查以后是不是都是成功的
            {
                foreach (KeyValuePair<FieldInfo, CheckFlag> r in RelationTableStatus)
                {
                    if (r.Value != CheckFlag.Success)
                    {
                        return false;
                    }
                }
                return true;
            }                       

            //依据一个列表初始化检查字段列表
            public virtual void InitRelationTableStatus()
            {
                List<System.Reflection.FieldInfo> fl = getRelationObjectFieldList();
                RelationTableStatus.Clear();
                foreach (FieldInfo f in fl)
                {
                    RelationTableStatus.Add(f, CheckFlag.Unknown);
                }
            }
            public virtual void InitDependenceTableStatus()
            {
                List<System.Reflection.FieldInfo> fl = getDependenceObjectFieldList();
                RelationTableStatus.Clear();
                foreach (FieldInfo f in fl)
                {

                        RelationTableStatus.Add(f, CheckFlag.Unknown);

                }
            }

            //获取分号分隔字符串的最后一个子串
            public string getLastCallParameterString(string CPString)
            {
                int i = CPString.LastIndexOf(";");
                if (i == -1)
                {
                    return CPString;
                }
                else
                {
                    return CPString.Substring(i+1);
                }
            }

            //设置回调函数的函数名
            public void setCallParameterCallFunctionName(Dictionary<string, string> cp, string FunctionName)
            {
                setCallParameterKVPair(cp, "CallFunctionName", FunctionName);
            }
        
            //得到回调函数的最后调用函数名，同时去除最后一个调用者
            public string getCallParameterCallFunctionName(Dictionary<string, string> cp)
            {
                if (cp.ContainsKey("CallFunctionName"))
                {
                    string s=cp["CallFunctionName"];
                    if (s.IndexOf(";") < 0)
                    {
                        return s;
                    }
                    else
                    {
                        cp["CallFunctionName"] = cp["CallFunctionName"].Substring(0, cp["CallFunctionName"].Length - 1 - cp["CallFunctionName"].LastIndexOf(";"));
                        return s.Substring(s.LastIndexOf(";"));
                    }
                }
                return "";
            }

            //设置回调函数的具体参数名
            public void setCallParameterKVPair(Dictionary<string, string> cp, string Key,string Value)
            {
                
                if (cp == null)
                {
                    cp = new Dictionary<string, string>();
                }
                if (cp.ContainsKey(Key))
                {
                    cp[Key] = cp[Key] + ";" + Value;
                }
                else
                {
                    cp.Add(Key, Value);//加入自己的调用标识
                }
            }

            //得到回调函数的最后调用参数，同时去除最后一个参数
            public string getCallParameterByKey(Dictionary<string, string> cp, string Key)
            {
                if (cp.ContainsKey("Key"))
                {
                    string s = cp["Key"];
                    if (s.IndexOf(";") < 0)
                    {
                        return s;
                    }
                    else
                    {
                        cp["Key"] = cp["Key"].Substring(0, cp["Key"].Length - 1 - cp["Key"].LastIndexOf(";"));
                        return s.Substring(s.LastIndexOf(";"));
                    }
                }
                return "";
            }

            //复制回调函数参数的新实例，避免并发的时候因为传递地址因此的错误
            public Dictionary<string, string> getNewCallParameter(Dictionary<string, string> cp)
            {
                Dictionary<string, string> tp = new Dictionary<string, string>();
                if (cp == null) { return tp; }
                foreach (KeyValuePair<string, string> v in cp)
                {
                    tp.Add(v.Key, v.Value);
                }
                return tp;
            }

            //设置该对象的数据映射表
            bool HaveDatabaseTableDetected = false;
            string DefDataTableName = "";//默认的数据表名称
            public string getDatabaseTableName()//返回映射表名
            {
                if (!HaveDatabaseTableDetected)//只查询一次
                {
                    DefDataTableName = MappingTable.MappingData.getTableName(this);
                    HaveColMapping = MappingTable.MappingData.IsMappingCol(this);
                    HaveColMappingDetected = true;
                    HaveDatabaseTableDetected = true;
                }
                if (DefDataTableName == "")//给默认的名字
                {
                    DefDataTableName= "Table_" + this.GetType().Name;
                }
                return DefDataTableName;
            }
            public string getDatabaseTableName(string TypeName)
            {
                string s = MappingTable.MappingData.getTableName(TypeName);

                if (s == "")//给默认的名字
                {
                    s = "Table_" + TypeName;
                }
                return s;
            }
            bool HaveColMappingDetected = false;//判断字段是否被测试过
            bool HaveColMapping = false;
            public string getDatabaseColName(FieldInfo Field)//返回映射的字段名
            {
                return getDatabaseColName(Field.Name);
            }
            public string getDatabaseColName(string FieldName)//返回映射的字段名
            {
                if (!HaveColMappingDetected)
                {
                    HaveColMapping = MappingTable.MappingData.IsMappingCol(this);
                }
                if (HaveColMapping)
                {
                    string s= MappingTable.MappingData.getMappingCol(this.GetType().Name, FieldName);
                    if (s == "")
                    {
                        return this.GetType().Name + "_" + getFieldNameWithoutPrefix(FieldName);
                    }
                    else
                    {
                        return s;
                    }
                }
                else
                {
                    return this.GetType().Name + "_" + getFieldNameWithoutPrefix(FieldName);
                }
            }
            public string getDatabaseColName(string TypeName,string FieldName)//返回映射的字段名
            {
                string s = MappingTable.MappingData.getMappingCol(TypeName, FieldName);
                if (s == "")
                {
                    return TypeName + "_" + getFieldNameWithoutPrefix(FieldName);
                }
                else
                {
                    return s;
                }
            }

        #endregion

        #region DatabaseOperationSuport//基本的对数据库的操作支持函数

            //自动设置数据库继承的ID和外键，规则是上一层的对象ID是下一层对象的外键，从子表指向父表
            public string AutoSetDatabaseInheritID()
            {
                return AutoSetDatabaseInheritID(GetType());
            }
            public string AutoSetDatabaseInheritID(Type t)
            {
                String ClassName = t.Name;
                String SuperClassName = t.GetTypeInfo().BaseType.Name;
                string id = Environment.getGUID();
                if (SuperClassName.StartsWith("Abstract"))//这表示父类已经没有这个属性了
                {
                    FieldInfo[] fields = t.GetRuntimeFields().ToArray();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if ((fields[i].Name.EndsWith("_ID")) && (fields[i].Name.Contains("db_")))
                        {
                            fields[i].SetValue(this, id);
                            return id;
                        }
                    }
                }
                else
                {
                    string pid = AutoSetDatabaseInheritID(t.GetTypeInfo().BaseType);
                    if (pid == "") { return ""; }
                    FieldInfo[] fields = t.GetRuntimeFields().ToArray();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if ((fields[i].Name.EndsWith("_ID")) && (fields[i].Name.Contains("db_")))
                        {
                            fields[i].SetValue(this, id);
                        }
                        if ((fields[i].Name.EndsWith("_"+SuperClassName+"ID")) && (fields[i].Name.Contains("db_")))
                        {
                            fields[i].SetValue(this, pid);
                        }
                        
                    }
                    return id;
                }
                return "";
            }

            //自动设置针对数据库的，继承字段的属性
            public void AutoSetDatabaseInheritField()
            {
                AutoSetDatabaseInheritField(GetType());
            }
            public void AutoSetDatabaseInheritField(Type t)//这个是为了设置属性的时候，设置SubType属性，该属性非常特殊，标识了这条记录属于哪个子类的记录
            {//如果其父类是AbstractDatabaseObject，就不做处理，否则，其处理方法为用自己的类名，去设置上一级对象的Type属性的值
                String ClassName = t.Name;
                String SuperClassName = t.GetTypeInfo().BaseType.Name;
                if (SuperClassName.StartsWith("Abstract"))//这表示父类已经没有这个属性了
                {
                }
                else
                {//在父类的字段里面查找和设置
                    FieldInfo[] fields = t.GetTypeInfo().BaseType.GetRuntimeFields().ToArray();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if ((fields[i].Name.EndsWith("_SubType")) && (fields[i].Name.Contains("db_")))
                        {
                            fields[i].SetValue(this, ClassName);
                        }
                    }
                    AutoSetDatabaseInheritField(t.GetTypeInfo().BaseType);
                }
            }

            //查找用于记录关联类类别的特殊属性字段
            public FieldInfo getSubTypeField(Type t)
            {
                string fName = "SubType";
                FieldInfo[] f = t.GetRuntimeFields().ToArray();
                foreach (FieldInfo fi in f)
                {
                    if (fi.Name.EndsWith("_" + fName) && (fi.Name.Contains("db_"))) { return fi; }
                }
                return null;
            }
            public FieldInfo getFKIDField(Type t)//查找某表的外键，也就是用于记录ID的字段
            {
                string fName = "ID";
                FieldInfo[] f = t.GetRuntimeFields().ToArray();
                foreach (FieldInfo fi in f)
                {
                    if (fi.Name.EndsWith("_" + fName) && (fi.Name.Contains("_"))) { return fi; }
                }
                return null;
            }
        
            //把一条完整记录里面的信息保存到本对象里面
            public void ReadDataTableValue(Dictionary<string, object> DT){//以对象为准，表格信息可以超越对象的信息
                FieldInfo[] fields = this.GetType().GetRuntimeFields().ToArray();
                string ClassName = this.GetType().Name;
                foreach (FieldInfo f in fields)
                { //注意这里的加载数据是以对象为主，未必所有的表里面的数据都被加载

                    if (IsDataBaseField(f.Name))
                    { //表示是数据表属性，需要加载数据表里面的数据
                        string fieldName = "_" + getFieldNameWithoutPrefix(f.Name);
                        if (DT.ContainsKey(ClassName + fieldName))//表示表里面有这个字段，如果没有，则不加载
                        {
                            if (DT[ClassName + fieldName] != null)
                            {
                                setFieldValue(f, DT[ClassName + fieldName]);
                                //f.SetValue(this, DT[0][ClassName + fieldName]);
                            }
                        }
                    }
                }
            }

            //把一条完整的记录信息加载到对象里面，Dictionary<string, object>就是代表一条记录的各个列
            public void LoadDataTableValue(Dictionary<string, object> DT, AbstractDataObject ao)//把一条完整记录里面的信息保存到指定的对象里面
            {
                FieldInfo[] fields = ao.GetType().GetRuntimeFields().ToArray();
                string ClassName = ao.GetType().Name;
                foreach (FieldInfo f in fields)
                { //注意这里的加载数据是以对象为主，未必所有的表里面的数据都被加载

                    if (IsDataBaseField(f.Name))
                    { //表示是数据表属性，需要加载数据表里面的数据
                        string fieldName = "_" + getFieldNameWithoutPrefix(f.Name);
                        if (DT.ContainsKey(ClassName + fieldName))//表示表里面有这个字段，如果没有，则不加载
                        {
                            if (DT[ClassName + fieldName] != null)
                            {
                                setFieldValue(ao, f, DT[ClassName + fieldName]);
                                //f.SetValue(this, DT[0][ClassName + fieldName]);
                            }
                        }
                    }
                }
            }
            public void LoadDataTableValue<T>(Dictionary<string, object> DT, T ao) where T : AbstractDataObject//把一条完整记录里面的信息保存到指定的对象里面
            {
                FieldInfo[] fields = ao.GetType().GetRuntimeFields().ToArray();
                string ClassName = ao.GetType().Name;
                foreach (FieldInfo f in fields)
                { //注意这里的加载数据是以对象为主，未必所有的表里面的数据都被加载

                    if (IsDataBaseField(f.Name))
                    { //表示是数据表属性，需要加载数据表里面的数据
                        string fieldName = "_" + getFieldNameWithoutPrefix(f.Name);
                        if (DT.ContainsKey(ClassName + fieldName))//表示表里面有这个字段，如果没有，则不加载
                        {
                            if (DT[ClassName + fieldName] != null)
                            {
                                setFieldValue(ao, f, DT[ClassName + fieldName]);
                                //f.SetValue(this, DT[0][ClassName + fieldName]);
                            }
                        }
                    }
                }
            }

            //得到父类的ID字段名称
            private String getSuperClassID(Type t)//如果父类不是AbstractdatabaseObject，则返回父对象的ID值
            {
                String supObjID = "0";
                String supClassRealName = t.GetTypeInfo().BaseType.Name;
                if (supClassRealName.StartsWith("Abstract"))
                {
                }
                else
                {
                    try
                    {
                        //String FID = "_" + supClassRealName + "ID";
                        FieldInfo f = getDatabaseBaseClassIDField(t);//获得父类的标识
                        //FieldInfo f = t.GetField(FID, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                        if (f.GetValue(this) != null)
                        {
                            supObjID = f.GetValue(this).ToString();
                        }
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                return supObjID;
            }
            public FieldInfo getDatabaseBaseClassIDField(Type t)//查找记录了父类ID的字段，只有在数据库符合映射规范才有效
            {
                Type bt = t.GetTypeInfo().BaseType;
                string fName = bt.Name + "ID";
                FieldInfo[] f = t.GetRuntimeFields().ToArray();
                foreach (FieldInfo fi in f)
                {
                    if (fi.Name.EndsWith("_" + fName) && (fi.Name.Contains("db_"))) { return fi; }
                }
                return null;
            }
        
            //判断是否是数据库的字段
            public bool IsDataBaseField(string name)//如果字段的前缀包含了db_那么标识这个属性会保存到数据库里面
            {
                if (name.Contains("db_"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //判断在数据库操作里面某个数据类型是不是要加逗号
            public Boolean SQLIsAddComma(Type t)
            //实际上在SQL里面只有字符串和日期要加逗号，其他类型包括布尔均不需要
            {
                String AttributeType = t.Name;

                if (AttributeType.EndsWith("date"))
                {
                    return true;
                }
                if (AttributeType.EndsWith("Date"))
                {
                    return true;
                }
                if (AttributeType.EndsWith("string"))
                {
                    return true;
                }
                if (AttributeType.EndsWith("String"))
                {
                    return true;
                }
                if (AttributeType.EndsWith("dateTime"))
                {
                    return true;
                }
                if (AttributeType.EndsWith("DateTime"))
                {
                    return true;
                }

                return false;
            }

            //得到依赖对象列表
            public List<System.Reflection.FieldInfo> getDependenceObjectFieldList()//得到从属类的集合
            {//映射的是一对多的关系，本类是一，属性类是多，可以对应多个，因此是每个属性都是列表，总体又形成一个列表
                List<System.Reflection.FieldInfo> rf = new List<System.Reflection.FieldInfo>();
                FieldInfo[] f = this.GetType().GetRuntimeFields().ToArray();
                foreach (FieldInfo fi in f)
                {
                    if (fi.Name.Contains("_DependenceList_"))
                    {
                        rf.Add(fi);
                    }
                }
                return rf;
            }
            public bool IsHaveDependenceObjectField()//得到一个默认的从属类的依赖类名集合
            {//映射的是多对一的关系，本类是多，属性类是一，可以对应多个，因此是列表
                List<System.Reflection.FieldInfo> rf = getDependenceObjectFieldList();
                if (rf.Count > 0)
                {
                    return true;
                }
                return false;
            }

            //得到有效的依赖对象
            public List<AbstractDataObject> getAvailableDependenceObjectList(List<AbstractDataObject> al)//得到一个当前有效的集合，为对象显示使用的
            {
                List<AbstractDataObject> rl = new List<AbstractDataObject>();
                foreach (AbstractDataObject o in al)
                {
                    if (o.DatabaseFlag != DataOperation.Delete)
                    {
                        rl.Add(o);
                    }
                }
                return rl;
            }
            public List<T> getAvailableDependenceObjectList<T>(List<T> al) where T:AbstractDataObject//得到一个当前有效的集合，为对象显示使用的
            {
                List<T> rl = new List<T>();
                foreach (T o in al)
                {
                    if (o.DatabaseFlag != DataOperation.Delete)
                    {
                        rl.Add(o);
                    }
                }
                return rl;
            }

            //得到关联对象
            public List<System.Reflection.FieldInfo> getRelationObjectFieldList()//得到一个默认的从属类的依赖类名集合
            {//映射的是多对一的关系，本类是多，属性类是一，可以对应多个，因此是列表
                List<System.Reflection.FieldInfo> rf = new List<System.Reflection.FieldInfo>();
                FieldInfo[] f = this.GetType().GetRuntimeFields().ToArray();
                foreach (FieldInfo fi in f)
                {
                    if (fi.Name.Contains("_Relation_"))
                    {
                        rf.Add(fi);
                    }
                }
                return rf;
            }
            
            //判断是否存在关联对象的字段
            public bool IsHaveRelationObjectField()//得到一个默认的从属类的依赖类名集合
            {//映射的是多对一的关系，本类是多，属性类是一，可以对应多个，因此是列表
                List<System.Reflection.FieldInfo> rf = getRelationObjectFieldList();
                if (rf.Count > 0)
                {
                    return true;
                }
                return false;
            }
        
            //拼接选择查询语句，因为特定的继承映射关系，所以需要递归类的继承关系再拼接
            public string getObjectSelectSQL(string ColName, string ColValue, Type ColType)//直接生成简化版的单一查询SQL
            {
                List<Type> ls = new List<Type>();
                getClassTree(this.GetType(), ls);
                string s = "select * from ";
                string sw = " where ";
                //s = s + " Table_" + this.GetType().Name;
                s = s + getDatabaseTableName();
                if (SQLIsAddComma(ColType))
                {
                    //sw = sw + this.GetType().Name + "_" + ColName + "='" + ColValue + "' ";
                    sw = sw + getDatabaseColName(ColName) + "='" + ColValue + "' ";
                }
                else
                {
                    sw = sw + getDatabaseColName(ColName) + "=" + ColValue + " ";
                }
                for (int i = 1; i < ls.Count; i++)
                {
                    string sss = ls[i - 1].Name;
                    string ss = ls[i].Name;
                    //string ssb = ls[i + 1];
                    s = s + "," +getDatabaseTableName( ss);
                    sw = sw + " and " + getDatabaseTableName(sss) + "." + sss + "_" + ss + "ID=" + getDatabaseTableName(ss) + "." + ss + "_ID";
                }
                s = s + sw;
                return s;
            }
            public string getObjectSelectSQL(Type ObjType, string ColName, string ColValue, Type FieldType)//直接生成简化版的单一查询SQL
            {
                List<Type> ls = new List<Type>();
                getClassTree(ObjType, ls);
                string s = "select * from ";
                string sw = " where ";
                s = s + getDatabaseTableName(ObjType.Name);
                if (SQLIsAddComma(FieldType))
                {
                    sw = sw + getDatabaseColName(ObjType.Name,ColName) + "='" + ColValue + "' ";
                }
                else
                {
                    sw = sw + getDatabaseColName(ObjType.Name, ColName) + "=" + ColValue + " ";
                }
                for (int i = 1; i < ls.Count; i++)
                {
                    string sss = ls[i - 1].Name;
                    string ss = ls[i].Name;
                    //string ssb = ls[i + 1];
                    s = s + ",Table_" + ss;
                    sw = sw + " and " + getDatabaseTableName(sss) + "." + sss + "_" + ss + "ID=" + getDatabaseTableName(ss) + "." + ss + "_ID";
                }
                s = s + sw;
                return s;
            }
            public string getRelationObjectSelectSQL(Type ObjType, string RelationTableName, string Rid)//直接生成简化版的单一查询SQL
            {
                List<Type> ls = new List<Type>();
                getClassTree(ObjType, ls);
                string s = "select * from ";
                string sw = " where ";
                s = s + " "+getDatabaseTableName(ObjType.Name);
                string RelationTablePIDCol = getDatabaseColName(RelationTableName, ObjType.Name);
                sw = sw + RelationTableName + "." + RelationTableName + "_" + this.GetType().Name + "ID='" + Rid + "' and " + RelationTableName + "." + RelationTableName + "_" + ObjType.Name + "ID=" + ObjType.Name + "." + ObjType.Name + "_ID ";

                for (int i = 1; i < ls.Count; i++)
                {
                    string sss = ls[i - 1].Name;
                    string ss = ls[i].Name;
                    //string ssb = ls[i + 1];
                    s = s + "," + getDatabaseTableName(ss);
                    sw = sw + " and " + getDatabaseTableName(sss) + "." + sss + "_" + ss + "ID=" + getDatabaseTableName(ss) + "." + ss + "_ID";
                }
                s = s + sw;
                return s;
            }
           
        #endregion


    }
}

