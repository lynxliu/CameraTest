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
    public abstract class AbstractDataObject//֧��SilveLight�ĸ��࣬���֧�����ݿ���ʺ�xml���л�
    {
        #region Event//�����й��ڲ������¼��Ĳ����ͷ���
            public event ProcessEventHandler ProcessQueryComplete;//�����ѯ����ϵ��¼����첽�¼�����ֵ

            //����ʧ���¼�
            public void SendErrorEvent(string Msg, string FunctionName, object tag, Dictionary<string, string> CallParameter)//����һ���������ݵ�ʧ����Ϣ
        {
            LynxProcessCompleteEventArgs le = new LynxProcessCompleteEventArgs(false);
            le.CallFunctionName = FunctionName;//���õķ���
            le.Message = Msg;//�ı���˵��
            le.ReturnValue = tag;//����ֵ
            le.SenderObject = this;//����˭��������Ϣ
            le.CallParameterList = CallParameter;
            if (ProcessQueryComplete != null)
            {
                ProcessQueryComplete(this, le);
            }
        }

            //���ͳɹ��¼�
            public void SendSuccessEvent(string Msg, string FunctionName, object tag, Dictionary<string, string> CallParameter)//����һ���������ݵ�ʧ����Ϣ
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

        #region Basic Function//�������ݶ���Ļ��������ͷ���

            public Boolean Changed = false; //��¼�Ƿ��޸Ĺ��������ֵ������޸��ˣ���ô��Ϊtrue������Ϊfalse

            public abstract string ObjectID//�����ʶ
        {
            get;
            set;
        }

            public bool IsAvailableObjectID()//�ж�ID�ֶ��Ƿ��Ѿ���ֵ
        {
            if (ObjectID == null || ObjectID == "")//ID�ǿջ�����null
            {
                return false;
            }
            else
            {
                return true;
            }
        }

            public string procmessage;//������¼��������Ϣ�ĸ�������������ķ����������������Լ�¼����

            public string getProcMessage()
        {
            return procmessage;
        }//�õ���ǰ�������Ϣ

            public DateTime _db_xa_createTime = DateTime.Now;//��������ʱ�䣬���е����ݼ�¼����Ҫ����
            public DateTime _db_xa_lastUpdateTime;//���յ��޸�ʱ�䣬���е����ݼ�¼����Ҫ����

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

        #region Reflection//�ͷ����йصĹ���

            public AbstractDataObject CloneInfor()//����һ��һģһ���Ķ���
            {
                AbstractDataObject xo = (AbstractDataObject)Activator.CreateInstance(GetType());//���Ѿ����ص�������Ϣ����ʵ��������ʵ��������ʵ��ָ��
                string s = WriteToXMLString();
                xo.ReadFromXMLString(s);
                return xo;
            }

            public Dictionary<string, object> ListAllValue()//�г��������е�������������ֵ
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

            public void ObjectValueClear()//������������������ݣ���֤����������µ�
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

            public string getFieldNameWithoutPrefix(string fname)//�Ӷ�����������ó���ʵ�Ķ����������ƣ�Ҳ����ȥ������ǰ׺������
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
            }//��һ��ָ���������б������ҵ��ض����Ƶ�����

            public AbstractDataObject getObjectByTypeName(string Name)//�����Ƽ��������Ĭ���Ǻͱ�������һ�������ռ������
            {
                var o= Activator.CreateInstance(Type.GetType(Name)) ;
                if (o != null) return o as AbstractDataObject;
                return null;
            }

            public void setFieldValue(Object o, FieldInfo fi, object value)//���ֶν��и�ֵ
            {
                if (fi.FieldType.Name == "Int32")
                {
                    if (value == null)
                    {
                        fi.SetValue(o, 0);
                        return;
                    }
                    if (value.ToString() == "")//ע��null���Զ�������Ϊ0
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
                    if (value.ToString() == "")//ע��null���Զ�������Ϊ0
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
                    if (value.ToString() == "")//ע��null���Զ�������Ϊ0
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
                    if (value.ToString() == "")//ע��null���Զ�������Ϊ0
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
                    if (value.ToString() == "")//ע��null���Զ�������Ϊ0
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
                    fi.SetValue(o, value.ToString().Trim());//��Ҫע�⣬�����trim��ɾ��ǰ��û���õĶ���
                }

            }

            public void setFieldValue(FieldInfo fi, object value)//�Ա������ֶν��и�ֵ
            {
                setFieldValue(this, fi, value);
            }

            public FieldInfo getFieldByName(string Name)//�������Ƶõ��ö�����ֶΣ�ֻҪ����������־Ϳ���
            {
                if (!Name.StartsWith("_")) { Name = "_" + Name; }
                Type t = this.GetType();
                FieldInfo[] fi = t.GetRuntimeFields().ToArray();
                foreach (FieldInfo f in fi)
                {
                    if (f.Name.EndsWith(Name))//�����Ʊ�������»��ߣ�ͬʱ�Ը�����������Ϊ��β
                    {
                        return f;
                    }
                }
                return null;
            }

            public void getClassTree(Type ct, List<Type> lt)//�õ�һ�����ض���ݹ鵽����AbstractDataObject�������б�
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

            public string getFieldAbstractName(FieldInfo f)//�õ��ֶε��������ͣ�������������AbstractDataObject������
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

        #region XMLOperation//�й�XML�Ĳ���

            public bool IsXMLAttribute(string name)//�ж��ǲ���һ�����Ե�����
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

            public bool IsXMLTag(string name)//�ж��ǲ���һ����ǩ������
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

            public bool IsXMLSubObject(string name)//�ж��ǲ���һ���Ӷ��������
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

            public bool IsXMLSubObjectList(string name)//�ж��ǲ���һ���Ӷ����б������
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

            public bool IsStringList(string name)//�ж��ǲ���һ���ַ��������б������
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

            public AbstractDataObject ParentObject;//��һ��AbstractDataObject����Ҫ��XML֮��Ļ������

            public void getPath(List<AbstractDataObject> PathList)//�õ�����֮��İ�����ϵ
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

            public string WriteXMLDataList(List<AbstractDataObject> l)//�õ�һ�����󼯺ϵ�XML��ʾ
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

            public List<AbstractDataObject> ReadXMLDataList(string XMLTableList)//XMLTableList���б��ַ���������xml��ʽ��i����Ҫ���صڼ�������
            {
                //XmlDocument x = new XmlDocument();
                //x.LoadXml(XMLTableList);
                //XmlNodeList xl = x.GetElementsByTagName(this.GetType().Name);
                //if (xl.Count <= i) { return; }
                //XmlNode n = xl[i];
                //LoadFromXMLString(n.OuterXml);//ֻ��OuterXML���Ǵ�Ԫ�ر�ǩ�ģ�����������
                if (!XMLTableList.StartsWith("<List_" + this.GetType().Name + ">")) { return null; }//�����������List��ǩ
                if (!XMLTableList.EndsWith("</List_" + this.GetType().Name + ">")) { return null; }

                Type t = this.GetType();
                FieldInfo[] fi = t.GetRuntimeFields().ToArray();
                StringReader xs = new StringReader(XMLTableList);
                XmlReader reader = XmlReader.Create(xs);

                bool tb = reader.ReadToFollowing(this.GetType().Name);
                if (!tb) { return null; }
                List<AbstractDataObject> xl = new List<AbstractDataObject>();
                bool IsEnd = false;
                do//����ִ��һ�Σ���Ϊ�Ѿ�������һ������
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

            public string _xa_objectName = "";//���������

            public string WriteToXMLString()//���ݶ�����Ϣ�����µ�xml�ַ�����ͬʱ����Ҳ�Ǵ��л�����ķ���
            //ԭ���ǣ����Թ����ԣ���ǩ���ǩ���Ӷ�����Ӷ������б�����б�
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
                        //������Ӷ����б�ı��洦��
                        List<Object> al;
                        if ((fi[i].GetValue(this) == null) || ((List<Object>)(fi[i].GetValue(this))).Count == 0)//��ʾ����б��ǿյģ�ʲô��û�У����ʱ���ʼ��Ϊ���б�
                        {
                        }
                        else
                        {

                            al = (List<Object>)fi[i].GetValue(this);
                            for (int j = 0; j < al.Count; j++)
                            {
                                //AbstractDataObject ao = (AbstractDataObject)al[j];
                                //ao.xa_objectName = tname.Substring(tname.LastIndexOf("_") + 1);//��ʾ�����͵����ݶ�Ӧ������Ǹ�����
                                string v = al[j].ToString();
                                sInterXML = sInterXML + "<" + getFieldNameWithoutPrefix(tname) + " objectName='" + tname.Substring(tname.LastIndexOf("_") + 1) + "'>" + v + "</" + getFieldNameWithoutPrefix(tname) + ">";
                            }
                        }
                    }
                    if (this.IsXMLSubObjectList(tname))
                    {
                        //������Ӷ����б�ı��洦��
                        List<Object> al;
                        if ((fi[i].GetValue(this) == null) || ((List<Object>)(fi[i].GetValue(this))).Count == 0)//��ʾ����б��ǿյģ�ʲô��û�У����ʱ���ʼ��Ϊ���б�
                        {
                        }
                        else
                        {

                            al = (List<Object>)fi[i].GetValue(this);
                            for (int j = 0; j < al.Count; j++)
                            {
                                AbstractDataObject ao = (AbstractDataObject)al[j];
                                ao._xa_objectName = tname.Substring(tname.LastIndexOf("_") + 1);//��ʾ�����͵����ݶ�Ӧ������Ǹ�����
                                sInterXML = sInterXML + ao.WriteToXMLString();
                            }
                        }
                    }
                    if (this.IsXMLSubObject(tname))
                    {
                        //������Ӷ���ı��洦��,�����Ǵ������Ժ��ܹ����Ӷ����datanode�����Լ���datanode�Ķ�Ӧ�ڵ�
                        //�ȸ����Ӷ����ٻ�ȡ��datanode���и�ֵ
                        if (fi[i].GetValue(this) == null) { }//���ֵ��null��ʾû�и�ֵ��
                        else
                        {
                            AbstractDataObject ao = (AbstractDataObject)fi[i].GetValue(this);
                            ao._xa_objectName = tname.Substring(tname.LastIndexOf("_") + 1);//��ʾ�����͵����ݶ�Ӧ������Ǹ�����
                            sInterXML = sInterXML + ao.WriteToXMLString();
                        }
                    }
                    if (this.IsXMLTag(tname))
                    {
                        //����Ǳ�ǩ�ı��洦��
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
                        //��������Եı��洦��
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

            public void ReadFromXMLString(string s)//���Ч�ʵķ���XML�ı��ķ���SAX�����е�һ���Է��ʣ����Ҽ������е���Ϣ
        {
            Type t = this.GetType();
            FieldInfo[] fi = t.GetRuntimeFields().ToArray();

            //List<FieldInfo> li = new List<FieldInfo>(fi);//���������ȫ�������ֶ�
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
            if (!reader.ReadToFollowing(this.GetType().Name)) { return; }//�ҵ�����ı�ǩ�����û�У���ôû������
            //����ҵ��˸���ı�ǩ����ô�͵�����ĵ�һ��Ԫ��
            if (reader.HasAttributes)//���ظñ�ǩ������
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
            while (IsNotEnd)//���뵱ǰ�Ķ�����ж�ȡ����Ҫ���ӱ�ǩ
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        FieldInfo tf = null;
                        string tag = reader.Name;

                        if (!reader.HasAttributes)//��ʾ����һ����ͨ��Tag
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
                        //��ʾ����һ��xo����xl����sl
                        {
                            reader.MoveToAttribute("objectName");
                            string ObjName = reader.Value;
                            reader.MoveToElement();
                            //if (ObjName == "") { ObjName = "List"; }
                            tf = getXMLObjectFieldByName(tag, ObjName, lixo);//�ȳ�����û���Ӷ���
                            if (tf == null)//��ʾû������Ӷ���
                            {

                                tf = getXMLListFieldByName(tag, ObjName, lixl);//�ٳ�����û�ж����б�
                                if (tf == null)
                                {
                                    tf = getXMLStringListFieldByName(tag, ObjName, lisl);//�ٳ�����û���ַ����б�
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
                                else//��ʾ�Ǹ�xl
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
                        //}//��ʾ��ȻXML�ļ������У����Ƕ�������û�������Ϣ��ֱ�Ӻ���
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
                        //    //Type st = Type.GetType(NameSpace + "." + reader.Name);//�����ƻ��������Ϣ�������ȡʧ��t����null
                        //    //AbstractDataObject xo = (AbstractDataObject)Activator.CreateInstance(st);//���Ѿ����ص�������Ϣ����ʵ��������ʵ��������ʵ��ָ��
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
                        //    //Type st = Type.GetType(NameSpace + "." + reader.Name);//�����ƻ��������Ϣ�������ȡʧ��t����null
                        //    //AbstractDataObject xo = (AbstractDataObject)Activator.CreateInstance(st);//���Ѿ����ص�������Ϣ����ʵ��������ʵ��������ʵ��ָ��
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
                        //if (reader.HasAttributes)//��ʶ���ж��������
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
            //_XMLString = "";//������Ͼ������
            //Changed = false;
        }

            public FieldInfo getXMLTagFieldByName(string Name, List<FieldInfo> fi)//�������Ƶõ�һ��XML�ֶ�
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

            public FieldInfo getXMLListFieldByName(string Name, string objName, List<FieldInfo> fi)//�������Ƶõ�һ��XML�б��ֶ�
            {//objName����Ҳ�Ǵ�AbstractDataObject��̳е�
                if (objName == null) { objName = ""; }
                if ((Name == null) || (Name == "")) { return null; }
                //Type t = this.GetType();
                //FieldInfo[] fi = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Default);
                foreach (FieldInfo f in fi)
                {
                    string FieldType = f.FieldType.Name;
                    string FieldName = f.Name;
                    if (FieldName.Contains("xl_"))//ֻ����ǰ׺�������ж�
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

            public FieldInfo getXMLObjectFieldByName(string Name, string objName, List<FieldInfo> fi)//�������Ƶõ�һ��XML�����ֶ�
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

            public FieldInfo getXMLStringListFieldByName(string Name, string objName, List<FieldInfo> fi)//�õ�һ���ַ����б��ֶ�
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

        #region DatabaseOperation//���ݿ������־��ز��������ݿ�����б�״̬�Ĳ���

            public DataOperation DatabaseFlag;//˵�����ݿ�Ĳ�����־

            //��������Զ����Բ���ʱ��֤������
            Dictionary<FieldInfo, CheckFlag> RelationTableStatus = new Dictionary<FieldInfo, CheckFlag>();//�������б��������ƺ�״̬

            //����Ƿ����
            public bool IsRelationTableTestComplete()//�жϹ������б�����ǲ�������˼��
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

            //����Ƿ�ɹ�
            public bool IsRelationTableTestSuccess()//�жϹ������б��������ɼ���Ժ��ǲ��Ƕ��ǳɹ���
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

            //����һ���б��ʼ������ֶ��б�
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

            //��ȡ�ֺŷָ��ַ��������һ���Ӵ�
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

            //���ûص������ĺ�����
            public void setCallParameterCallFunctionName(Dictionary<string, string> cp, string FunctionName)
            {
                setCallParameterKVPair(cp, "CallFunctionName", FunctionName);
            }
        
            //�õ��ص������������ú�������ͬʱȥ�����һ��������
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

            //���ûص������ľ��������
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
                    cp.Add(Key, Value);//�����Լ��ĵ��ñ�ʶ
                }
            }

            //�õ��ص������������ò�����ͬʱȥ�����һ������
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

            //���ƻص�������������ʵ�������Ⲣ����ʱ����Ϊ���ݵ�ַ��˵Ĵ���
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

            //���øö��������ӳ���
            bool HaveDatabaseTableDetected = false;
            string DefDataTableName = "";//Ĭ�ϵ����ݱ�����
            public string getDatabaseTableName()//����ӳ�����
            {
                if (!HaveDatabaseTableDetected)//ֻ��ѯһ��
                {
                    DefDataTableName = MappingTable.MappingData.getTableName(this);
                    HaveColMapping = MappingTable.MappingData.IsMappingCol(this);
                    HaveColMappingDetected = true;
                    HaveDatabaseTableDetected = true;
                }
                if (DefDataTableName == "")//��Ĭ�ϵ�����
                {
                    DefDataTableName= "Table_" + this.GetType().Name;
                }
                return DefDataTableName;
            }
            public string getDatabaseTableName(string TypeName)
            {
                string s = MappingTable.MappingData.getTableName(TypeName);

                if (s == "")//��Ĭ�ϵ�����
                {
                    s = "Table_" + TypeName;
                }
                return s;
            }
            bool HaveColMappingDetected = false;//�ж��ֶ��Ƿ񱻲��Թ�
            bool HaveColMapping = false;
            public string getDatabaseColName(FieldInfo Field)//����ӳ����ֶ���
            {
                return getDatabaseColName(Field.Name);
            }
            public string getDatabaseColName(string FieldName)//����ӳ����ֶ���
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
            public string getDatabaseColName(string TypeName,string FieldName)//����ӳ����ֶ���
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

        #region DatabaseOperationSuport//�����Ķ����ݿ�Ĳ���֧�ֺ���

            //�Զ��������ݿ�̳е�ID���������������һ��Ķ���ID����һ��������������ӱ�ָ�򸸱�
            public string AutoSetDatabaseInheritID()
            {
                return AutoSetDatabaseInheritID(GetType());
            }
            public string AutoSetDatabaseInheritID(Type t)
            {
                String ClassName = t.Name;
                String SuperClassName = t.GetTypeInfo().BaseType.Name;
                string id = Environment.getGUID();
                if (SuperClassName.StartsWith("Abstract"))//���ʾ�����Ѿ�û�����������
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

            //�Զ�����������ݿ�ģ��̳��ֶε�����
            public void AutoSetDatabaseInheritField()
            {
                AutoSetDatabaseInheritField(GetType());
            }
            public void AutoSetDatabaseInheritField(Type t)//�����Ϊ���������Ե�ʱ������SubType���ԣ������Էǳ����⣬��ʶ��������¼�����ĸ�����ļ�¼
            {//����丸����AbstractDatabaseObject���Ͳ������������䴦����Ϊ���Լ���������ȥ������һ�������Type���Ե�ֵ
                String ClassName = t.Name;
                String SuperClassName = t.GetTypeInfo().BaseType.Name;
                if (SuperClassName.StartsWith("Abstract"))//���ʾ�����Ѿ�û�����������
                {
                }
                else
                {//�ڸ�����ֶ�������Һ�����
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

            //�������ڼ�¼�������������������ֶ�
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
            public FieldInfo getFKIDField(Type t)//����ĳ��������Ҳ�������ڼ�¼ID���ֶ�
            {
                string fName = "ID";
                FieldInfo[] f = t.GetRuntimeFields().ToArray();
                foreach (FieldInfo fi in f)
                {
                    if (fi.Name.EndsWith("_" + fName) && (fi.Name.Contains("_"))) { return fi; }
                }
                return null;
            }
        
            //��һ��������¼�������Ϣ���浽����������
            public void ReadDataTableValue(Dictionary<string, object> DT){//�Զ���Ϊ׼�������Ϣ���Գ�Խ�������Ϣ
                FieldInfo[] fields = this.GetType().GetRuntimeFields().ToArray();
                string ClassName = this.GetType().Name;
                foreach (FieldInfo f in fields)
                { //ע������ļ����������Զ���Ϊ����δ�����еı���������ݶ�������

                    if (IsDataBaseField(f.Name))
                    { //��ʾ�����ݱ����ԣ���Ҫ�������ݱ����������
                        string fieldName = "_" + getFieldNameWithoutPrefix(f.Name);
                        if (DT.ContainsKey(ClassName + fieldName))//��ʾ������������ֶΣ����û�У��򲻼���
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

            //��һ�������ļ�¼��Ϣ���ص��������棬Dictionary<string, object>���Ǵ���һ����¼�ĸ�����
            public void LoadDataTableValue(Dictionary<string, object> DT, AbstractDataObject ao)//��һ��������¼�������Ϣ���浽ָ���Ķ�������
            {
                FieldInfo[] fields = ao.GetType().GetRuntimeFields().ToArray();
                string ClassName = ao.GetType().Name;
                foreach (FieldInfo f in fields)
                { //ע������ļ����������Զ���Ϊ����δ�����еı���������ݶ�������

                    if (IsDataBaseField(f.Name))
                    { //��ʾ�����ݱ����ԣ���Ҫ�������ݱ����������
                        string fieldName = "_" + getFieldNameWithoutPrefix(f.Name);
                        if (DT.ContainsKey(ClassName + fieldName))//��ʾ������������ֶΣ����û�У��򲻼���
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
            public void LoadDataTableValue<T>(Dictionary<string, object> DT, T ao) where T : AbstractDataObject//��һ��������¼�������Ϣ���浽ָ���Ķ�������
            {
                FieldInfo[] fields = ao.GetType().GetRuntimeFields().ToArray();
                string ClassName = ao.GetType().Name;
                foreach (FieldInfo f in fields)
                { //ע������ļ����������Զ���Ϊ����δ�����еı���������ݶ�������

                    if (IsDataBaseField(f.Name))
                    { //��ʾ�����ݱ����ԣ���Ҫ�������ݱ����������
                        string fieldName = "_" + getFieldNameWithoutPrefix(f.Name);
                        if (DT.ContainsKey(ClassName + fieldName))//��ʾ������������ֶΣ����û�У��򲻼���
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

            //�õ������ID�ֶ�����
            private String getSuperClassID(Type t)//������಻��AbstractdatabaseObject���򷵻ظ������IDֵ
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
                        FieldInfo f = getDatabaseBaseClassIDField(t);//��ø���ı�ʶ
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
            public FieldInfo getDatabaseBaseClassIDField(Type t)//���Ҽ�¼�˸���ID���ֶΣ�ֻ�������ݿ����ӳ��淶����Ч
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
        
            //�ж��Ƿ������ݿ���ֶ�
            public bool IsDataBaseField(string name)//����ֶε�ǰ׺������db_��ô��ʶ������Իᱣ�浽���ݿ�����
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

            //�ж������ݿ��������ĳ�����������ǲ���Ҫ�Ӷ���
            public Boolean SQLIsAddComma(Type t)
            //ʵ������SQL����ֻ���ַ���������Ҫ�Ӷ��ţ��������Ͱ�������������Ҫ
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

            //�õ����������б�
            public List<System.Reflection.FieldInfo> getDependenceObjectFieldList()//�õ�������ļ���
            {//ӳ�����һ�Զ�Ĺ�ϵ��������һ���������Ƕ࣬���Զ�Ӧ����������ÿ�����Զ����б��������γ�һ���б�
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
            public bool IsHaveDependenceObjectField()//�õ�һ��Ĭ�ϵĴ������������������
            {//ӳ����Ƕ��һ�Ĺ�ϵ�������Ƕ࣬��������һ�����Զ�Ӧ�����������б�
                List<System.Reflection.FieldInfo> rf = getDependenceObjectFieldList();
                if (rf.Count > 0)
                {
                    return true;
                }
                return false;
            }

            //�õ���Ч����������
            public List<AbstractDataObject> getAvailableDependenceObjectList(List<AbstractDataObject> al)//�õ�һ����ǰ��Ч�ļ��ϣ�Ϊ������ʾʹ�õ�
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
            public List<T> getAvailableDependenceObjectList<T>(List<T> al) where T:AbstractDataObject//�õ�һ����ǰ��Ч�ļ��ϣ�Ϊ������ʾʹ�õ�
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

            //�õ���������
            public List<System.Reflection.FieldInfo> getRelationObjectFieldList()//�õ�һ��Ĭ�ϵĴ������������������
            {//ӳ����Ƕ��һ�Ĺ�ϵ�������Ƕ࣬��������һ�����Զ�Ӧ�����������б�
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
            
            //�ж��Ƿ���ڹ���������ֶ�
            public bool IsHaveRelationObjectField()//�õ�һ��Ĭ�ϵĴ������������������
            {//ӳ����Ƕ��һ�Ĺ�ϵ�������Ƕ࣬��������һ�����Զ�Ӧ�����������б�
                List<System.Reflection.FieldInfo> rf = getRelationObjectFieldList();
                if (rf.Count > 0)
                {
                    return true;
                }
                return false;
            }
        
            //ƴ��ѡ���ѯ��䣬��Ϊ�ض��ļ̳�ӳ���ϵ��������Ҫ�ݹ���ļ̳й�ϵ��ƴ��
            public string getObjectSelectSQL(string ColName, string ColValue, Type ColType)//ֱ�����ɼ򻯰�ĵ�һ��ѯSQL
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
            public string getObjectSelectSQL(Type ObjType, string ColName, string ColValue, Type FieldType)//ֱ�����ɼ򻯰�ĵ�һ��ѯSQL
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
            public string getRelationObjectSelectSQL(Type ObjType, string RelationTableName, string Rid)//ֱ�����ɼ򻯰�ĵ�һ��ѯSQL
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

