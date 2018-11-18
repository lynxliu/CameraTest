using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.IO.Compression;
using Windows.Media;
using System.Reflection;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using System.Runtime;
using Windows.UI;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Windows.Foundation;
using Windows.Web;
using Windows.Networking.Sockets;
using System.Threading;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.UI.Core;

namespace SilverlightLFC.common
{
    public class LFCException : Exception//��׼�����κ��඼����ֱ���׳�����쳣
    {
        string _message;
        public override string Message { get { return _message; } }
        public new object Data { get; set; }
        DateTime _CreateTime = DateTime.Now;
        public DateTime CreateTime { get { return _CreateTime; } }
        public LFCException(string msg, object data)
        {
            _message = msg;
            Data = data;

        }
    }

    public delegate void LFCStandardError(object sender, LFCStandardErrorArgs e);
    public class LFCStandardErrorArgs : EventArgs//��׼�Ĵ��󣬰����ַ�����Ϣ��object��������Ϣ
    {
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
        DateTime _CreateTime = DateTime.Now;
        public DateTime CreateTime { get { return _CreateTime; } }
        public LFCStandardErrorArgs(string msg, object data)
        {
            ErrorMessage = msg;
            Data = data;
        }
    }

    public delegate void LFCObjectChanged(object sender, LFCObjectChangedArgs e);
    public class LFCObjectChangedArgs : EventArgs//ͨ�õ��¼��࣬���Դ�������Ķ��󷵻أ������ػص�����
    {
        public FieldInfo ModifyField;
        public object FieldValue;
        public string PropertyName;
        public LFCObjChanged ChangeType;

        public DateTime EventTime = DateTime.Now;//�¼�����ʱ��
        public LFCObjectChangedArgs(FieldInfo modifyField, object value)
        {
            ModifyField = modifyField;
            FieldValue = value;
        }
        public LFCObjectChangedArgs(LFCObjChanged operation, string propertyName)//�޸�Property��ʱ��
        {
            PropertyName = propertyName;
            ChangeType = operation;
        }
        public LFCObjectChangedArgs(LFCObjChanged operation)//�޷���ȷ����ı������
        {
            ChangeType = operation;
        }
    }

    public enum LFCObjChanged
    {
        ObjectCreated, ObjectDeleted, ObjectChanged
    }

    public delegate void ProcessEventHandler(object sender, LynxProcessCompleteEventArgs e);//�����ϵͳ�¼�
    public class LynxProcessCompleteEventArgs : EventArgs//ͨ�õ��¼��࣬���Դ�������Ķ��󷵻أ������ػص�����
    {
        public object SenderObject;//��������¼��Ķ�����
        public DateTime EventTime = DateTime.Now;//�¼�����ʱ��
        public string CallFunctionName;//���õķ�������
        public bool IsSuccess;//��ʶ�����Ƿ�ɹ�
        public LynxProcessCompleteEventArgs(bool s)
        {
            IsSuccess = s;
        }
        public string Message;//�¼����ӵ���Ϣ
        public string ReturnType;//����ֵ���
        public object ReturnValue;//����ֵ
        public Dictionary<string, string> CallParameterList;//�����б�
    }

    public delegate void ExecuteQueryEventHandler(object sender, LFCExecuteQueryEventArgs e);//����Ĳ�ѯ�¼�
    public class LFCExecuteQueryEventArgs : EventArgs//ͨ�õ��¼��࣬����һ�����ݲ�ѯ����
    {
        public List<Dictionary<string, object>> DataTable;
        public DateTime EventTime = DateTime.Now;//�¼�����ʱ��
        public bool IsSuccess;//��ʶ�����Ƿ�ɹ�
        public LFCExecuteQueryEventArgs(bool s)
        {
            IsSuccess = s;
        }
    }

    public delegate void LynxSelectedObjectChangedHandler(object sender, LynxSelectedObjectChangedEventArgs e);//ѡ�����ı��¼�
    public class LynxSelectedObjectChangedEventArgs : EventArgs//ר�ű�ʾĳ������ѡ�У�ͬʱ�ṩѡ�еĶ���ͱ��滻�Ķ���Ϊ�˽⹹���ϵ�Monitor��
    {
        public string msg;
        public DateTime EventTime = DateTime.Now;//�¼�����ʱ��
        public object OldObject;//���ı����ǰ�Ķ���
        public object SelectedObject;//ѡ��Ķ���
        public LynxSelectedObjectChangedEventArgs(string s, Object selectedObj, object OldObj)
        {
            OldObject = OldObj;
            msg = s;
            SelectedObject = selectedObj;
        }
    }


    public delegate void LynxCommonProcessHandler(object sender, LynxCommonProcessEventArgs e);//�����ͨ�õ����¼�
    public class LynxCommonProcessEventArgs : EventArgs//ͨ�õ��¼��࣬����һ�������һ������˵����Ϣ
    {
        public string msg;
        public DateTime EventTime = DateTime.Now;//�¼�����ʱ��
        public bool IsSuccess;//��ʶ�����Ƿ�ɹ�
        public object ReturnValue;//����ֵ
        public LynxCommonProcessEventArgs(bool b, string s, Object o)
        {
            IsSuccess = b;
            msg = s;
            ReturnValue = o;
        }
    }

    public class Environment//���ӻ�����
    {
        public static char SepIDChar = ';';//Ĭ�ϵķָ��ַ����ķ��ţ�����ID�ָ�
        public static string XMarkExtFileName = ".xmk";
        public static string CoefficientExtFileName = ".xco";
        public static string StandardExtFileName = ".xsd";
        public static string XMarkStructureExtFileName = ".xst";
        public static string ProjectExtFileName = ".xpj";
        public static string PeopleLibExtFileName = ".xpl";
        public static string ContractExtFileName = ".xcn";
        public static string StructureModifyExtFileName = ".xsm";

        //public static string StructureManagerExtFileName = ".xsm";

        public static DateTime MinTime = System.Convert.ToDateTime("1773-3-7");
        public static DateTime MaxTime = System.Convert.ToDateTime("2773-3-7");

        public static string SysimageBaseURL = ".\\Images\\";
        public static string SysimageDataBaseURL = ".\\App_Data\\";
        public static string Upload = ".\\UpLoad\\";
        public static string UploadImg = ".\\UpLoad\\Img\\";
        public static string SystemImg = " \\images\\ExpressIMG\\";
        public static string SystemXMLDataURL = "\\Data\\";
        //public DCDevice CurrentDevice;//��¼��ǰ�Ĳ����豸
        //public const string TargetType = "Digital Photo Device";//��¼�ò���Ӧ�ò����ĸ�����
        //private String DatabaseType="access";//��¼ʹ�õ����ݿ�����
        public string SystemKey = "lynxliuVela@2008";//16���ַ�
        public string SystemIV = "lynxliu2002@hotm";//16���ַ�

        public static string CurrentAuthor = "";//��ǰ�Ĵ����û���
        public static string CurrentAuthorEMail = "";//��ǰ�Ĵ����û������ʼ�
        public static string CurrentAuthorPassword = "";//��ǰ�Ĵ����û�����

        public static string CurrentUser = "";//��ǰ�������Ȩ�û���
        public static string CurrentUserEMail = "";//��ǰ�������Ȩ�û������ʼ�
        public static string CurrentUserPassword = "";//��ǰ�������Ȩ�û�����

        public static string getDirectory(string URL)//�õ��ļ���Ŀ¼
        {
            if (URL == null) { return null; }
            int bi = URL.LastIndexOf("\\");
            if (bi == -1) { return URL; }//��ʾ�����ľ���Ŀ¼
            else
            {
                return URL.Substring(0, bi + 1);//ȥ�������ļ���
            }
        }

        public static string getFileName(string URL)//�õ������ļ���
        {
            int i = URL.LastIndexOf("\\");

            return URL.Substring(i + 1);

        }

        public static string getFileNameWithoutExt(string URL)//�õ������ļ���
        {
            int i = URL.LastIndexOf("\\");

            string s = URL.Substring(i + 1);
            int ti = s.LastIndexOf(".");
            if (ti == -1) { return s; }
            return s.Substring(0, ti);
        }

        public static string getFileExtName(string URL)//�õ��ļ�����չ��,���壬������������Ǹ���
        {
            int ti = URL.LastIndexOf(".");
            if (ti != -1)
            {
                string ext = URL.Substring(ti);
                return ext;
            }
            return null;//��ʾû����չ��
        }

        public String ConnectionStr;//���ݿ�����Ӵ�
        public string getConnectionStr()
        {
            return this.ConnectionStr;
        }

        public void setConnectionStr(string s)
        {
            this.ConnectionStr = s;
        }
        private static Environment xEn = new Environment();
        public void ReLoadEnv()
        {

        }
        //public SecurityManager SecurityManager;
        private Environment()
        {  //����ģʽ�����캯����˽�е�
            //this.ConnectionStr="Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"D:\\My Document\\XMark.mdb\";Persist Security Info=True";
            //this.ConnectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"D:\\My Document\\Visual Studio 2005\\Projects\\DCMark\\DCMark\\DCMark.mdb\";Persist Security Info=True;PassWord='lynxliu2002'";
            //ConnectionStr="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\EduProcess.accdb;Persist Security Info=True;Jet OLEDB:Database Password=lynxliu2007";
            //this.SecurityManager = SecurityManager.getInstance();
            //this.CurrentDevice = new DCDevice();
            //ConnectionStr = "Data Source=.\\SQLEXPRESS;AttachDbFilename='D:\\My Document\\Visual Studio 10\\Projects\\LynxSilverlightControls\\WebService1\\App_Data\\Database1.mdf';Integrated Security=True;User Instance=True";
            //ConnectionStr = "Data Source=.\\SQLEXPRESS;AttachDbFilename='|DataDirectory|\\Database1.mdf';Integrated Security=True;User Instance=True";
            ConnectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='D:\\My Document\\DVInfor.mdb'";
        }
        public static Environment getEnvironment()
        {  //����ģʽ��һ����̬��������ȫ��Ψһ��ʵ��
            return xEn;
        }
        public static String getGUID()
        {
            //string RtnStr = "";
            Guid guid = Guid.NewGuid();
            //uint GuidHashCode = (uint)guid.GetHashCode();
            //RtnStr = GuidHashCode.ToString();
            return guid.ToString();
        }
        public void setNewConnectStr(String str)
        {
            this.ConnectionStr = str;
        }

        public static Dictionary<string, object> ReadFromDataRowXML(string s)//�����ɵ����ݱ�����ָ�����
        {
            StringReader xs = new StringReader(s);
            XmlReader reader = XmlReader.Create(xs);
            if (!reader.ReadToFollowing("DataRow")) { return null; }

            Dictionary<string, object> rd = new Dictionary<string, object>();
            bool IsNotEnd = reader.Read();
            while (IsNotEnd)//���뵱ǰ�Ķ�����ж�ȡ����Ҫ���ӱ�ǩ
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (!reader.HasAttributes)
                        {
                            reader.ReadOuterXml();
                        }
                        else
                        {
                            string an, av, at;

                            reader.MoveToAttribute("name");
                            an = reader.Value;
                            reader.MoveToAttribute("value");
                            av = reader.Value;
                            av = XmlConvert.DecodeName(av);//����
                            reader.MoveToAttribute("type");
                            at = reader.Value;
                            object tv = getTrueValueByStringValue(av, at);
                            rd.Add(an, tv);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        IsNotEnd = false;
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
                }
            }

            return rd;
        }

        private static object getTrueValueByStringValue(string StringValue, string ValueType)
        {
            if (ValueType == "Int32")
            {
                if (StringValue == null || StringValue == "")
                {
                    return 0;
                }
                return System.Convert.ToInt32(StringValue);
            }
            if (ValueType == "Int64")
            {
                if (StringValue == null || StringValue == "")
                {
                    return 0;
                }
                return System.Convert.ToInt64(StringValue);
            }
            if (ValueType == "Int")
            {
                if (StringValue == null || StringValue == "")
                {
                    return 0;
                }
                return System.Convert.ToInt32(StringValue);
            }
            if (ValueType == "BigInt")
            {
                if (StringValue == null || StringValue == "")
                {
                    return 0;
                }
                return System.Convert.ToInt64(StringValue);
            }
            if (ValueType == "Float")
            {
                if (StringValue == null || StringValue == "")
                {
                    return 0f;
                }
                return System.Convert.ToSingle(StringValue);
            }
            if (ValueType == "Real")
            {
                if (StringValue == null || StringValue == "")
                {
                    return 0d;
                }
                return System.Convert.ToDouble(StringValue);
            }
            if (ValueType == "Decimal")
            {
                if (StringValue == null || StringValue == "")
                {
                    return 0m;
                }
                return System.Convert.ToDecimal(StringValue);
            }
            if (ValueType == "DateTime")
            {
                if ((StringValue == "") || (StringValue == null))
                {
                    return MinTime;
                }
                return System.Convert.ToDateTime(StringValue);
            }

            return StringValue;
        }

        public static List<Dictionary<string, object>> ReadFromDataTableXML(string s)//�����ɵ����ݱ�����ָ�����
        {
            if (s == "")
            {
                return null;
            }
            List<Dictionary<string, object>> rs = new List<Dictionary<string, object>>();

            StringReader xs = new StringReader(s);
            XmlReader reader = XmlReader.Create(xs);
            if (!reader.ReadToFollowing("DataTable")) { return null; }//�ҵ�����ı�ǩ�����û�У���ôû�����أ����ؿ�
            //����ҵ��˸���ı�ǩ����ô�͵�����ĵ�һ��Ԫ��

            bool IsNotEnd = reader.Read();
            while (IsNotEnd)//���뵱ǰ�Ķ�����ж�ȡ����Ҫ���ӱ�ǩ
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:

                        string tag = reader.Name;
                        if (tag == "DataRow")
                        {
                            string ss = reader.ReadOuterXml();
                            Dictionary<string, object> td = ReadFromDataRowXML(ss);
                            if (td == null) { }
                            else
                            {
                                rs.Add(td);
                            }
                        }
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
            return rs;
        }

        public static bool ReadFromResultFlagXML(string s)//��ȡ�����־
        {
            if (s == "")
            {
                return false;
            }

            StringReader xs = new StringReader(s);
            XmlReader reader = XmlReader.Create(xs);
            if (!reader.ReadToFollowing("ResultFlag")) { return false; }//�ҵ�����ı�ǩ�����û�У���ôû�����أ����ؿ�
            //����ҵ��˸���ı�ǩ����ô�͵�����ĵ�һ��Ԫ��
            string rs = reader.GetAttribute("value");
            if (rs == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string EncodeCallbackParameter(Dictionary<string, string> Parameter)
        {
            string ss = "<CallbackParameterList>";
            if (Parameter != null)
            {

                foreach (KeyValuePair<string, string> p in Parameter)
                {
                    ss = ss + "<CallbackParameter name='" + p.Key + "'" + " value='" + XmlConvert.EncodeName(p.Value) + "'/>";
                }

            }
            ss = ss + "</CallbackParameterList>";
            return ss;
        }

        public static Dictionary<string, string> DecodeCallbackParameter(string s)
        {
            StringReader xs = new StringReader(s);
            XmlReader reader = XmlReader.Create(xs);
            if (!reader.ReadToFollowing("CallbackParameterList")) { return null; }

            Dictionary<string, string> rd = new Dictionary<string, string>();
            bool IsNotEnd = reader.Read();
            while (IsNotEnd)//���뵱ǰ�Ķ�����ж�ȡ����Ҫ���ӱ�ǩ
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (!reader.HasAttributes)
                        {
                            reader.ReadOuterXml();
                        }
                        else
                        {
                            string an, av;

                            reader.MoveToAttribute("name");
                            an = reader.Value;
                            reader.MoveToAttribute("value");
                            av = XmlConvert.DecodeName(reader.Value);
                            rd.Add(an, av);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        IsNotEnd = false;
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
                }
            }

            return rd;
        }

        public String ConvertDBType(Type dbType)
        {
            if (dbType.Name == "VarChar") { return "String"; }
            if (dbType.Name == "Text") { return "String"; }
            if (dbType.Name == "Int") { return "int"; }
            if (dbType.Name == "BigInt") { return "long"; }
            if (dbType.Name == "TinyInt") { return "byte"; }
            if (dbType.Name == "SmallInt") { return "Int16"; }
            if (dbType.Name == "Bit") { return "bool"; }
            if (dbType.Name == "DateTime") { return "DateTime"; }
            if (dbType.Name == "decimal") { return "decimal"; }
            if (dbType.Name == "Float") { return "decimal"; }
            if (dbType.Name == "Real") { return "Single"; }
            if (dbType.Name == "UniqueIdentifier") { return "Guid"; }
            if (dbType.Name == "Binary") { return "Byte[]"; }
            if (dbType.Name == "VarBinary") { return "Byte[]"; }
            if (dbType.Name == "Image") { return "Byte[]"; }
            return "String";
        }
 
        public String getSystemImgURL()
        {
            return Environment.SystemImg;
        }

        public async void WriteFile(String FullFileName, String Data)
        {//д���ļ�
            StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(FullFileName);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, Data);
        }

        public async Task<string> ReadFile(String FullFileName)
        {//��ȡ�ļ�

            StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
            StorageFile sampleFile =await storageFolder.GetFileAsync(FullFileName);
            var s=await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            return s;
        }

        public decimal ProcessValueList(List<decimal> ValueList)//������Ե����ݣ�����ȥ����ߺ���ͣ����µ�ƽ������������
        {
            decimal x = 0;
            decimal max = 0;
            decimal min = 0;
            if (ValueList.Count == 0)
            {
                return 0m;
            }
            if (ValueList.Count == 1)
            {
                return ValueList[0];
            }
            if (ValueList.Count == 2)
            {
                return (ValueList[0] + ValueList[1]) / 2;
            }
            if (ValueList.Count > 2)
            {
                min = ValueList[0];//��ʼ�����Է�ֹminһֱ����0��С������maxһֱ����0���
                max = ValueList[0];
                for (int i = 0; i < ValueList.Count; i++)
                {
                    decimal tv = ValueList[0];
                    if (tv < min)
                    {
                        min = tv;
                    }
                    if (tv > max)
                    {
                        max = tv;
                    }

                    x = x + ValueList[0];

                }
                x = x - min - max;
                x = x / (ValueList.Count - 2);
                return x;
            }
            return 0m;
        }

        public bool IsVideo(string Filename)//�����ļ����ж��ǲ�����Ƶ�ļ�
        {
            if (Filename.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".wmv", StringComparison.OrdinalIgnoreCase)) { return true; }
            if (Filename.EndsWith(".mpg", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".mpeg", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".vob", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".avi", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".rm", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".rmvb", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".mov", StringComparison.OrdinalIgnoreCase)) { return false; }
            if (Filename.EndsWith(".asf", StringComparison.OrdinalIgnoreCase)) { return false; }
            return false;
        }

         public string getSuitedPasswordString(string s)//��֤����16λ���ļ����м��ܺͽ���
        {
            s = s.Trim();
            if (s.Length > 16)
            {
                s = s.Substring(0, 16);
            }

            if (s.Length < 16)
            {
                s = s + SystemKey.Substring(0, 16 - s.Length);

            }
            return s;
        }

        public static string getNameAndVersion(string Name, string Version)//���ڶ��������˵�����ƺͰ汾��Ψһ��ʶ
        {
            if ((Version == "") || (Version == null)) { Version = "0.0"; }
            return Name + "(Version_" + Version + ")";
        }

        public static string getName(string NameAndVersion)
        {
            int index = NameAndVersion.IndexOf("(");
            if (index == -1) { return NameAndVersion; }
            return NameAndVersion.Substring(0, index);
        }

        public static decimal getVersion(string NameAndVersion)
        {
            int index = NameAndVersion.IndexOf("(");
            if (index == -1) { return 0m; }
            string v = NameAndVersion.Substring(index + 9);
            v = v.Substring(0, v.Length - 1);
            return System.Convert.ToDecimal(v);
        }

        public static bool IsRTFString(string s)
        {
            if (s == null) { return false; }
            if (s.StartsWith("{\\rtf"))
            {
                return true;
            }
            return false;
        }

        public static List<String> AddList(List<String> a, List<String> b)//a��b�����б�ȥ��һ���ĵ���
        {
            List<String> l = new List<string>();
            bool IsInclude = false;
            for (int i = 0; i < a.Count; i++)
            {
                string t = a[i];
                IsInclude = false;
                for (int j = 0; j < l.Count; j++)
                {
                    if (t == l[j])
                    {
                        IsInclude = true;
                        break;
                    }
                }
                if (!IsInclude)
                {
                    l.Add(a[i]);
                }
            }
            for (int i = 0; i < b.Count; i++)
            {
                string t = b[i];
                IsInclude = false;
                for (int j = 0; j < l.Count; j++)
                {
                    if (t == l[j])
                    {
                        IsInclude = true;
                        break;
                    }
                }
                if (!IsInclude)
                {
                    l.Add(b[i]);
                }
            }
            return l;
        }

        public static List<decimal> getDecimalList<T>(List<T> al)
        {
            List<decimal> l = new List<decimal>();
            for (int i = 0; i < al.Count; i++)
            {
                l.Add(System.Convert.ToDecimal(al[i]));
            }

            return l;
        }

        public static List<T> getObjectList<T, BT>(List<BT> al) where T : BT
        {
            List<T> l = new List<T>();
            for (int i = 0; i < al.Count; i++)
            {
                l.Add((T)(al[i]));
            }
            return l;
        }

        public static List<double> getDoubleList<ST>(List<ST> al)
        {
            List<double> l = new List<double>();
            //object x = null;
            for (int i = 0; i < al.Count; i++)
            {
                object o = al[i];
                l.Add(System.Convert.ToDouble(o));

            }
            return l;
        }

        public static Boolean IsUper(String s, int index)//�ж�ָ���Ĵ�λ���ַ��ǲ��Ǵ�д
        {
            Boolean r;
            string s1, s2;
            s1 = s.Substring(index, 1);
            s2 = s1.ToUpper();
            if (s1 == s2)
            {
                r = true;
            }
            else
            {
                r = false;
            }
            return r;
        }

        public static List<string> getStringListFromCommaString(string s, char? CommaChar)//�ָ�ָ�����ַ��ֿ����ַ���
        {
            char cc=Environment.SepIDChar;
            if (CommaChar != null) { cc = CommaChar.Value; }
            string[] subs = s.Split(CommaChar.Value);
            List<String> l = new List<string>();
            foreach (String ss in subs)
            {
                l.Add(ss);
            }
            return l;
        }

        public static string getCommaString(List<string> l, char? CommaChar)//���ַ����б�õ����ŷָ���ַ���
        {
            char tc = Environment.SepIDChar;
            if (CommaChar != null) { tc=CommaChar.Value; }
            if (l.Count == 0)
            {
                return "";
            }
            if (l.Count == 1)
            {
                return l[0];
            }
            string s = l[0] + tc;
            for (int i = 1; i < l.Count - 1; i++)
            {
                s = s + l[i] + tc;
            }
            s = s + l[l.Count - 1];
            return s;
        }

        public static string EncodeSqlList(List<string> SqlList)
        {
            string s = "";
            foreach (string sql in SqlList)
            {
                s = s + "<SQL>" + sql + "</SQL>";
            }
            s = "<SQLList>" + s + "</SQLList>";
            return s;
        }

        public static List<string> DecodeSqlList(string s)
        {
            StringReader xs = new StringReader(s);
            XmlReader reader = XmlReader.Create(xs);
            if (!reader.ReadToFollowing("SQLList")) { return null; }

            List<string> rd = new List<string>();


            bool IsNotEnd = reader.Read();
            while (IsNotEnd)//���뵱ǰ�Ķ�����ж�ȡ����Ҫ���ӱ�ǩ
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (!reader.HasAttributes)
                        {
                            rd.Add(reader.ReadElementContentAsString());
                        }
                        else
                        {

                        }
                        break;
                    case XmlNodeType.EndElement:
                        IsNotEnd = false;
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
                }
            }

            return rd;
        }

        public static char GetCharSpellCode(char c)//����ת��Ϊƴ����ĸ
        {

            byte[] data = Encoding.GetEncoding("gb2312").GetBytes(c.ToString());

            ushort code = (ushort)((data[0] << 8) + data[1]);

            ushort[] areaCode = {45217,45253,45761,46318,46826,47010,47297,47614,48119,48119,49062,49324,  

             49896,50371,50614,50622,50906,51387,51446,52218,52698,52698,52698,52980,53689,54481, 55290};

            for (int i = 0; i < 26; i++)
            {

                if ((code >= areaCode[i]) && (code <= (ushort)(areaCode[i + 1] - 1)))

                    return (char)('A' + i);

            }

            return c;

        }
        static DateTime _Time = DateTime.Now;
        public static bool IsVirtual = false;//�ж��Ƿ��������ģʽ

        public static void BeginVirtual() { IsVirtual = true; }
        public static void EndVirtual() { IsVirtual = false; _Time = DateTime.Now; }
        public static DateTime CurrentTime
        {
            get
            {
                return _Time;
            }
            set
            {
                if (IsVirtual) { _Time = value; }//������ģʽ��������
            }
        }

        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // ���õ�ǰ����λ��Ϊ���Ŀ�ʼ 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        public async Task<string> LoadFileString(string filter)
        {
            FileOpenPicker openFileDialog = new FileOpenPicker();
            if (filter == "" || filter == null)
            {
                openFileDialog.FileTypeFilter.Add(".txt");
                openFileDialog.FileTypeFilter.Add("*");
            }
            else
            {
                openFileDialog.FileTypeFilter.Add(filter);
                openFileDialog.FileTypeFilter.Add("*");
            }
            openFileDialog.SuggestedStartLocation=PickerLocationId.DocumentsLibrary;
            StorageFile file = await openFileDialog.PickSingleFileAsync();
            return await Windows.Storage.FileIO.ReadTextAsync(file);
        }

        public async void SaveFileString(string s, IDictionary<string,IList<string>> filter=null)
        {

            FileSavePicker saveFileDialog = new FileSavePicker();
            saveFileDialog.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            if (filter == null)
            {
                saveFileDialog.FileTypeChoices.Add("Image", new List<string>() { ".jpg", ".jpeg", ".png", ".bmp", ".gif" });

                saveFileDialog.FileTypeChoices.Add("Text", new List<string>() { ".txt" });
                saveFileDialog.FileTypeChoices.Add("*", new List<string>() { "*.*" });
            }
            else
            {
                foreach (var v in filter)
                {
                    saveFileDialog.FileTypeChoices.Add(v);
                }
            }
             saveFileDialog.SuggestedFileName = DateTime.Now.ToString("yyyyMMddhhmmss");

             StorageFile file =await saveFileDialog.PickSaveFileAsync();
             await Windows.Storage.FileIO.WriteTextAsync(file, s);
        }

        DateTime ZeroTime = new DateTime(1974, 3, 7, 12, 0, 0);
        public long getMS(DateTime d)//ʱ�̵ĺ�����������ν��ʱ�̱Ƚϵ�
        {
            TimeSpan ts = d - ZeroTime;
            return ts.Milliseconds;
        }
        public DateTime getMS(long ms)
        {
            return ZeroTime + TimeSpan.FromMilliseconds(ms);
        }

        #region ImageOperation
        public static string ColorToString(Color color)//colorת��Ϊʮ�������ַ���
        {
            string A = System.Convert.ToString(color.A, 16);
            if (A == "0")
                A = "00";
            string R = System.Convert.ToString(color.R, 16);
            if (R == "0")
                R = "00";
            string G = System.Convert.ToString(color.G, 16);
            if (G == "0")
                G = "00";
            string B = System.Convert.ToString(color.B, 16);
            if (B == "0")
                B = "00";
            string HexColor = "#" + A + R + G + B;
            return HexColor;
        }

        public static Color getColorFromString(string value)//16�����ַ���ת��Ϊ��ɫ
        {
            //RGBʮ����ֵ
            byte r = System.Convert.ToByte("0x" + value.Substring(1, 2), 16);
            byte g = System.Convert.ToByte("0x" + value.Substring(3, 2), 16);
            byte b = System.Convert.ToByte("0x" + value.Substring(5, 2), 16);
            //��ֵ
            return Color.FromArgb((byte)255, r, g, b);
        }

        public async void SaveImage(WriteableBitmap src)

        {

            FileSavePicker save = new FileSavePicker();

           save.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

           save.DefaultFileExtension = ".jpg";

            save.SuggestedFileName ="newimage";

           save.FileTypeChoices.Add(".bmp", new List<string>() { ".bmp" });

           save.FileTypeChoices.Add(".png", new List<string>() { ".png" });

           save.FileTypeChoices.Add(".jpg", new List<string>() { ".jpg", ".jpeg" });

            StorageFile savedItem = await save.PickSaveFileAsync();

            try

            {

                 Guid encoderId;

                 switch (savedItem.FileType.ToLower())

                 {

                     case".jpg":

                        encoderId = BitmapEncoder.JpegEncoderId;

                        break;

                     case".bmp":

                        encoderId = BitmapEncoder.BmpEncoderId;

                        break;

                     case".png":

                     default:

                        encoderId = BitmapEncoder.PngEncoderId;

                        break;

                 }

                 IRandomAccessStream fileStream = await savedItem.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                 BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderId, fileStream);
                
                 Stream pixelStream = src.PixelBuffer.AsStream();

                byte[] pixels = new byte[pixelStream.Length];

               pixelStream.Read(pixels, 0, pixels.Length);

                //pixal format shouldconvert to rgba8 

                 for (int i = 0; i < pixels.Length; i += 4)

                 {

                    byte temp = pixels[i];

                     pixels[i] =pixels[i + 2];

                     pixels[i + 2] =temp;

                 }

               encoder.SetPixelData(

                 BitmapPixelFormat.Rgba8,

                 BitmapAlphaMode.Straight,

                 (uint)src.PixelWidth,

                 (uint)src.PixelHeight,

                 96, // Horizontal DPI 

                 96, // Vertical DPI 

                 pixels

                 );

                 await encoder.FlushAsync();

            }

            catch (Exception e)

            {

                 throw e;

            }

        }
       
        public async Task<List<WriteableBitmap>> OpenImage()

        {

            FileOpenPicker imagePicker = new FileOpenPicker

            {

                 ViewMode = PickerViewMode.Thumbnail,

               SuggestedStartLocation = PickerLocationId.PicturesLibrary,

                 FileTypeFilter = { ".jpg", ".jpeg", ".png", ".bmp" }

            };
            var flist= await imagePicker.PickMultipleFilesAsync();
            var images=new List<WriteableBitmap>();
            foreach (var f in flist)
            {
                var wb = await LoadFile(f);
                if (wb != null)
                    images.Add(wb);
            }
            return images;
        }
        public async Task<WriteableBitmap> LoadFile(StorageFile imageFile)
        {
            Guid decoderId;
            if (imageFile != null)
            {
                 var srcImage = new BitmapImage();
                 using (IRandomAccessStream stream = await imageFile.OpenAsync(FileAccessMode.Read))
                 {
                     srcImage.SetSource(stream);
                     switch (imageFile.FileType.ToLower())
                     {

                        case".jpg":

                        case".jpeg":

                           decoderId = Windows.Graphics.Imaging.BitmapDecoder.JpegDecoderId;

                            break;

                        case".bmp":

                           decoderId = Windows.Graphics.Imaging.BitmapDecoder.BmpDecoderId;

                            break;

                        case".png":

                           decoderId = Windows.Graphics.Imaging.BitmapDecoder.PngDecoderId;

                            break;

                        default:

                             return null;

                     }

                   Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(decoderId, stream);

                     int width = (int)decoder.PixelWidth;

                     int height = (int)decoder.PixelHeight;

                   Windows.Graphics.Imaging.PixelDataProvider dataprovider = await decoder.GetPixelDataAsync();

                     byte[] pixels =dataprovider.DetachPixelData();

                     var wbsrcImage = new WriteableBitmap(width, height);

                     Stream pixelStream =wbsrcImage.PixelBuffer.AsStream();
                     for (int i = 0; i < pixels.Length; i += 4)

                     {

                        byte temp = pixels[i];

                        pixels[i] =pixels[i + 2];

                        pixels[i +2] = temp;

                     }

                   pixelStream.Write(pixels, 0, pixels.Length);

                   pixelStream.Dispose();

                   stream.Dispose();
                   return wbsrcImage;
                 }
            }
            return null;
        }

        public byte[] WriteableBitmapToBytes(WriteableBitmap src)
        {

            Stream temp = src.PixelBuffer.AsStream();

            byte[] tempBytes = new byte[src.PixelWidth * src.PixelHeight * 3];

            for (int i = 0; i < tempBytes.Length; i++)
            {

                temp.Seek(i, SeekOrigin.Begin);

                temp.Write(tempBytes, 0, tempBytes.Length);

            }

            temp.Dispose();

            return tempBytes;

        }

        public WriteableBitmap BytesToImage(byte[] src, int lw, int lh)
        {

            WriteableBitmap wbbitmap = new WriteableBitmap(lw, lh);

            Stream s = wbbitmap.PixelBuffer.AsStream();

            s.Seek(0, SeekOrigin.Begin);

            s.Write(src, 0, lw * lh * 3);

            return wbbitmap;

        }
        #endregion
        
        #region Serialize/Deserialize

        public async static Task Save<T>(string fileName, T data)
        {
            //ȡ�õ�ǰ���������ݵ�Ŀ¼  
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //�����ļ�������ļ����ھ͸���  
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (Stream newFileStream = await file.OpenStreamForWriteAsync())
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(T));
                ser.WriteObject(newFileStream, data);
                newFileStream.Dispose();
            }
        }
        public async static Task<T> Read<T>(string fileName)
        {
            T t = default(T);
            try
            {
                StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile file = await folder.GetFileAsync(fileName);
                if (file == null)
                    return t;
                Stream newFileStream = await file.OpenStreamForReadAsync();
                DataContractSerializer ser = new DataContractSerializer(typeof(T));
                t = (T)ser.ReadObject(newFileStream);
                newFileStream.Dispose();
                return t;
            }
            catch (Exception)
            {
                return t;
            }
        }
        public static string Serialize(object objForSerialization)
        {
            string str = string.Empty;
            if (objForSerialization == null)
            {
                return str;
            }
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(objForSerialization.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, objForSerialization);
                byte[] buffer = new byte[stream.Length];
                stream.Position = 0L;
                stream.Read(buffer, 0, buffer.Length);
                UTF8Encoding us = new UTF8Encoding();
                return us.GetString(buffer, 0, buffer.Length);
                
            }
        }

        public static T Convert<T>(Stream stream)
        {
            try
            {
                return (T)(Deserialize(stream, typeof(T)));

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static object Deserialize(Stream streamObject, Type serializedObjectType)
        {
            if ((serializedObjectType == null) || (streamObject == null))
            {
                return null;
            }
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(serializedObjectType);

            return serializer.ReadObject(streamObject);
        }

        public static object Deserialize(string json, Type serializedObjectType)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(serializedObjectType);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return serializer.ReadObject(ms);
            }

        }
        
        #endregion

        #region Net
        public string InformationOnService { get; set; }
        public async Task<string> GetServiceResult(string url, string parameter)
        {
            var httpClient = new HttpClient();
            // Limit the max buffer size for the response so we don't get overwhelmed
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
            try
            {
                string responseBodyAsText;
                InformationOnService = "Waiting for response ...";

                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                InformationOnService = response.StatusCode + " " + response.ReasonPhrase + System.Environment.NewLine;
                responseBodyAsText = await response.Content.ReadAsStringAsync();
                responseBodyAsText = responseBodyAsText.Replace("<br>", System.Environment.NewLine); // Insert new lines
                InformationOnService = responseBodyAsText;
                return responseBodyAsText;
            }
            catch (HttpRequestException hre)
            {
                InformationOnService = hre.ToString();
            }
            catch (Exception ex)
            {
                // For debugging
                InformationOnService = ex.ToString();
            }
            return "";
        }

        #endregion

        public static bool CopyValue(object source, object target)
        {
            if(source ==null||target==null)
                return false;
            if (source.GetType().AssemblyQualifiedName != target.GetType().AssemblyQualifiedName)
                return false;
            if(source is ICopySupport)
            {
                return (target as ICopySupport).CopyValue(source as ICopySupport);
                
            }
            try
            {
                var fields = target.GetType().GetRuntimeFields();
                var properties = target.GetType().GetRuntimeProperties();
                foreach (var f in fields)
                {
                    if (f.IsPublic)
                        f.SetValue(target, source.GetType().GetRuntimeField(f.Name));
                }
                foreach (var p in properties)
                {
                    if (p.CanWrite)
                        p.SetValue(target, source.GetType().GetRuntimeProperty(p.Name));
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }

        static string AppName="Camera performance test";
        static Version AppVer=new Version("0.7");
        public static void ShowChildWindow(Panel parent, UIElement content, Action<object,object> callback)
        {
            Popup p = new Popup()
            {
                Child = content,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            p.Closed += new EventHandler<object>(callback);
            parent.Children.Add(p);
            p.IsOpen = true;
        }
        public static void ShowAbout(Panel parent)
        {
            Popup p = new Popup()
            {
                Child = new TextBlock() { Text = AppName + "(" + AppVer.ToString() + ")" },
                HorizontalAlignment=HorizontalAlignment.Center,
                VerticalAlignment=VerticalAlignment.Center
            };
            
            parent.Children.Add(p);
            p.IsOpen=true;
        }
        public static void ShowMessage(string message)
        {
            var md = new MessageDialog(message);
            var x = md.ShowAsync();
        }
    }

    public static class WriteableBitmapHelper
    {
        public static byte[] GetPixelsData(this WriteableBitmap source)
        {
            Stream temp = source.PixelBuffer.AsStream();

            byte[] tempBytes = new byte[source.PixelWidth * source.PixelHeight * 3];

            for (int i = 0; i < tempBytes.Length; i++)
            {

                temp.Seek(i, SeekOrigin.Begin);

                temp.Write(tempBytes, 0, tempBytes.Length);

            }

            temp.Dispose();

            return tempBytes;
        }
        public static void SetPixelsData(this WriteableBitmap source, byte[] Data)
        {
            Stream s = source.PixelBuffer.AsStream();

            s.Seek(0, SeekOrigin.Begin);

            s.Write(Data, 0, source.PixelWidth * source.PixelHeight * 3);

        }
        //public static async Task<WriteableBitmap> Snapshot(FrameworkElement control)
        //{
        //    return await WinRTXamlToolkit.Composition.WriteableBitmapRenderExtensions.Render(control);
        //}
        public static WriteableBitmap Clone(WriteableBitmap source)
        {
            
            var target = new WriteableBitmap(source.PixelWidth,source.PixelHeight);
            Stream s = target.PixelBuffer.AsStream();

            s.Seek(0, SeekOrigin.Begin);

            s.Write(source.PixelBuffer.ToArray(), 0, source.PixelWidth * source.PixelHeight * 3);
            return target;
        }
    }

    public interface ICopySupport
    {
        bool CopyValue(ICopySupport obj);
    }
}