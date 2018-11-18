using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DCtestLibrary
{
    public class Environment
    {
        public static char SepIDChar=';';//Ĭ�ϵķָ��ַ����ķ��ţ�����ID�ָ�
        public static string XMarkExtFileName = ".xmk";
        public static string CoefficientExtFileName = ".xco";
        public static string StandardExtFileName = ".xsd";
        public static string XMarkStructureExtFileName = ".xst";
        public static string ProjectExtFileName = ".xpj";
        public static string PeopleLibExtFileName = ".xpl";
        public static string ContractExtFileName = ".xcn";
        public static string StructureModifyExtFileName = ".xsm";

        //public static string StructureManagerExtFileName = ".xsm";

        public static DateTime MinTime = Convert.ToDateTime("1773-3-7");
        public static DateTime MaxTime = Convert.ToDateTime("2773-3-7");

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

            string s= URL.Substring(i + 1);
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
            this.ConnectionStr=s;
        }
        private static Environment xEn = new Environment();
        public void ReLoadEnv(){

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
        public static String getGUID(){
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
        public static int getNullObject(int Value)
        {
            return 0;
        }
        public static long getNullObject(long Value)
        {
            return 0;
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
            if (ValueType== "Int32")
            {
                return Convert.ToInt32(StringValue);
            }
            if (ValueType == "Int64")
            {
                return Convert.ToInt64(StringValue);
            }
            if (ValueType == "Int")
            {
                return Convert.ToInt32(StringValue);
            }
            if (ValueType == "BigInt")
            {
                return Convert.ToInt64(StringValue);
            }
            if (ValueType == "Float")
            {
                return Convert.ToSingle(StringValue);
            }
            if (ValueType == "Real")
            {
                return Convert.ToDouble(StringValue);
            }
            if (ValueType == "Decimal")
            {
                return Convert.ToDecimal(StringValue);
            }
            if (ValueType == "DateTime")
            {
                if ((StringValue == "") || (StringValue == null))
                {
                    return MinTime;
                }
                return Convert.ToDateTime(StringValue);
            }

            return StringValue;
        }

        public static List<Dictionary<string, object>> ReadFromDataTableXML(string s)//�����ɵ����ݱ�����ָ�����
        {
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

        //static DispatcherTimer timer = new DispatcherTimer();
        //public static long TimeOutSM = 10000;
        //static bool IsTimeOut = false;
        //static void timer_Tick(object sender, EventArgs e)
        //{
        //    timer.Stop();
        //    IsTimeOut=true;//��ʶ��ʱ
            
        //}

        private static string _ServiceResult = "";//�䵱�м���̵Ļ���
        public static bool HaveRead = false;
        public static string ServiceResult{
            get
            {
                HaveRead = true;
                return _ServiceResult;
            }
            set
            {
                HaveRead = false;
                _ServiceResult = value;
            }
        }

        public String ConvertDBType( Type dbType )
        {

			if(dbType.Name=="VarChar"){return "String";}
			if(dbType.Name=="Text"){return "String";}
            if(dbType.Name=="Int"){return "int";}
            if(dbType.Name=="BigInt"){return "long";}
            if(dbType.Name=="TinyInt"){return "byte";}
            if(dbType.Name=="SmallInt"){return "Int16";}
            if(dbType.Name=="Bit"){return "bool";}
            if(dbType.Name=="DateTime"){return "DateTime";}
            if(dbType.Name=="decimal"){return "decimal";}
            if(dbType.Name=="Float"){return "decimal";}
            if(dbType.Name=="Real"){return "Single";}
            if(dbType.Name=="UniqueIdentifier"){return "Guid";}
            if(dbType.Name=="Binary"){return "Byte[]";}
            if(dbType.Name=="VarBinary"){return "Byte[]";}
            if(dbType.Name=="Image"){return "Byte[]";}
            return "String";
        }

        public String getSystemImgURL()
        {
            return Environment.SystemImg;
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
            if(Filename.EndsWith(".jpg",StringComparison.OrdinalIgnoreCase)){return false;}
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

        //public string EncryptString(string Source, string sKey, string sIV)//�����ı�
        //{
        //    sKey=getSuitedPasswordString(sKey);

        //    RijndaelManaged mobjCryptoService = new RijndaelManaged();
        //    byte[] Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        //    byte[] IV = ASCIIEncoding.ASCII.GetBytes(sIV);
        //    byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
        //    MemoryStream ms = new MemoryStream();
        //    mobjCryptoService.Key = Key;
        //    mobjCryptoService.IV = IV;
        //    ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
        //    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
        //    cs.Write(bytIn, 0, bytIn.Length);
        //    cs.FlushFinalBlock();
        //    ms.Close();
        //    byte[] bytOut = ms.ToArray();
        //    return Convert.ToBase64String(bytOut);
        //}


        //public string DecryptString(string Source, string sKey, string sIV)//�����ı�
        //{
        //    sKey=getSuitedPasswordString(sKey);

        //    RijndaelManaged mobjCryptoService = new RijndaelManaged();
        //    byte[] Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        //    byte[] IV = ASCIIEncoding.ASCII.GetBytes(sIV);
        //    byte[] bytIn = Convert.FromBase64String(Source);
        //    MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
        //    mobjCryptoService.Key = Key;
        //    mobjCryptoService.IV = IV;
        //    ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
        //    CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
        //    StreamReader sr = new StreamReader(cs);
        //    return sr.ReadToEnd();
        //}

        public static string getNameAndVersion(string Name,string Version)//���ڶ��������˵�����ƺͰ汾��Ψһ��ʶ
        {
            if ((Version == "")||(Version==null)) { Version = "0.0"; }
            return Name + "(Version_" + Version + ")";
        }

        public static string getName(string NameAndVersion){
            int index = NameAndVersion.IndexOf("(");
            if (index == -1) { return NameAndVersion; }
            return NameAndVersion.Substring(0, index);
        }

        public static decimal getVersion(string NameAndVersion)
        {
            int index = NameAndVersion.IndexOf("(");
            if (index == -1) { return 0m; }
            string v= NameAndVersion.Substring(index+9);
            v = v.Substring(0, v.Length - 1);
            return Convert.ToDecimal(v);
        }

        //public static Type getTypeByName(string TypeName)//���ո��౾������Ʋ���
        //{
        //    string FilePath = AppDomain.CurrentDomain.BaseDirectory;
        //    string[] files = Directory.GetFiles(FilePath);
        //    //int i = 0;
        //    //PluginInfoAttribute typeAttribute = new PluginInfoAttribute();
        //    foreach (string file in files)
        //    {
        //        string ext = file.Substring(file.IndexOf("."));
        //        if ((ext != ".dll") && (ext != ".exe")) continue;//������չ����dll��
        //        System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(file);
        //        Type[] types = assembly.GetTypes();
        //        foreach (Type t in types)
        //        {
        //            if (t.Name==TypeName)
        //            {
        //                return t;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //public static Type getTypeByFullName(string TypeFullName)//�����������������в��ң����ϸ�
        //{
        //    string FilePath = AppDomain.CurrentDomain.BaseDirectory;
        //    string[] files = Directory.GetFiles(FilePath);
        //    //int i = 0;
        //    //PluginInfoAttribute typeAttribute = new PluginInfoAttribute();
        //    foreach (string file in files)
        //    {
        //        string ext = file.Substring(file.IndexOf("."));
        //        if ((ext != ".dll") && (ext != ".exe")) continue;//������չ����dll��
        //        System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(file);
        //        Type[] types = assembly.GetTypes();
        //        foreach (Type t in types)
        //        {
        //            if (t.FullName == TypeFullName)
        //            {
        //                return t;
        //            }
        //        }
        //    }
        //    return null;
        //}

        public static bool IsRTFString(string s)
        {
            if (s == null) { return false; }
            if(s.StartsWith("{\\rtf")){
                return true;
            }
            return false;
        }

        //public static string Compress(string strSource)//�ַ���ѹ��
        //{
        //    if (strSource == null || strSource.Length > 8 * 1024)
        //        throw new System.ArgumentException("�ַ���Ϊ�ջ򳤶�̫��");

        //    System.Text.Encoding encoding = System.Text.Encoding.Unicode;
        //    byte[] buffer = encoding.GetBytes(strSource);
        //    //byte[] buffer = Convert.FromBase64String(strSource); //������ַ�����һ����Base64String���ͣ�����д����

        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    System.IO.Compression.DeflateStream stream = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Compress, true);
        //    stream.Write(buffer, 0, buffer.Length);
        //    stream.Close();

        //    buffer = ms.ToArray();
        //    ms.Close();

        //    return Convert.ToBase64String(buffer); //��ѹ�����byte[]ת��ΪBase64String
        //}


        //public static string Decompress(string strSource)//��ѹ��
        //{
        //    //System.Text.Encoding encoding = System.Text.Encoding.Unicode;
        //    //byte[] buffer = encoding.GetBytes(strSource);
        //    byte[] buffer = Convert.FromBase64String(strSource);

        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    ms.Write(buffer, 0, buffer.Length);
        //    ms.Position = 0;
        //    System.IO.Compression.DeflateStream stream = new System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress);
        //    stream.Flush();

        //    int nSize = 16 * 1024 + 256; //�ַ������ᳬ��16K
        //    byte[] decompressBuffer = new byte[nSize];
        //    int nSizeIncept = stream.Read(decompressBuffer, 0, nSize);
        //    stream.Close();

        //    return System.Text.Encoding.Unicode.GetString(decompressBuffer, 0, nSizeIncept);//ת��Ϊ��ͨ���ַ���
        //}

        public static List<String> AddList(List<String> a, List<String> b)//a��b�����б�ȥ��һ���ĵ���
        {
            List<String> l = new List<string>();
            bool IsInclude = false;
            for (int i = 0; i < a.Count; i++)
            {
                string t=a[i];
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
                l.Add(Convert.ToDecimal(al[i]));
            }

            return l;
        }

        //public static List<T> getAbstractDataObjectList<T,T1>(List<T1> al) where T1:T
        //{
        //    List<T> l = new List<T>();
        //    //object x = null;
        //    for (int i = 0; i < al.Count;i++ )
        //    {
        //        l.Add((T)(al[i]));
        //    }
        //    return l;
        //}

        public static List<T> getObjectList<T, ST>(List<ST> al)
        {
            List<T> l = new List<T>();
            //object x = null;
            for (int i = 0; i < al.Count; i++)
            {
                object o = al[i];
                l.Add((T)o);

            }
            return l;
        }

        public static List<double> getDoubleList<T>(List<T> al)
        {
            List<double> l = new List<double>();
            //object x = null;
            for (int i = 0; i < al.Count; i++)
            {
                object o = al[i];
                l.Add(Convert.ToDouble(o));

            }
            return l;
        }

        //public string WriteToXML(DataTable dt)//��ѯ�����ݱ��Ϊһ���ַ���
        //{
        //    string s = "";
        //    s = s + "<DataTable>";
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        s = s + "<DataRow>";
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            s = s + "<DataCol name='" + dt.Columns[j].ColumnName + "' type='" + dt.Columns[j].DataType.Name + "' value='" + dt.Rows[i][j].ToString() + "'/>";
        //        }
        //        s = s + "</DataRow>";
        //    }
        //    s = s + "</DataTable>";
        //    return s;
        //}

        //public string WriteToXMLS(DataTable dt)//��ѯ�����ݱ��Ϊһ���ַ���
        //{
        //    string s = "";
        //    s = s + "<DataTable>";
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        s = s + "<DataRow>";
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            s = s + "<DataCol name='" + dt.Columns[j].ColumnName + "' value='" + dt.Rows[i][j].ToString() + "'/>";
        //        }
        //        s = s + "</DataRow>";
        //    }
        //    s = s + "</DataTable>";
        //    return s;
        //}



    }
}
