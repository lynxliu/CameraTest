using System;
using System.Net;
using System.Windows;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


using System.Windows.Input;



using SilverlightLFC.common;
using System.Reflection;

namespace SilverlightLynxControls
{
    public class ActionDataShow//自动给特定的数据窗体填充数据和加载数据
    {
        public static bool IsDataLinkArea(FrameworkElement fe)
        {
            if (fe.Name.Contains("_DL_"))
            {
                return true;
            }
            return false;
        }
        public static string getFieldNameWithoutPrefix(string fname)//从对象的属性名得出真实的对象属性名称，也就是去掉所有前缀的名称
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

        public static void ClearDataLinkControl(Panel dp)
        {
            foreach (FrameworkElement fe in dp.Children)
            {
                if (IsDataLinkArea(fe))
                {
                    string tn = getFieldNameWithoutPrefix(fe.Name);
                    Type ft = fe.GetType();
                    if (ft.Name == "TextBox")
                    {
                        TextBox tb = fe as TextBox;
                        tb.Text = "";
                    }
                    if (ft.Name == "TextBlock")
                    {
                        TextBlock tb = fe as TextBlock;
                        tb.Text = "";
                    }
                    if (ft.Name == "DatePicker")
                    {
                        LynxDateTimeSelect tb = fe as LynxDateTimeSelect;
                        tb.Text = "";


                    }
                }
            }
        }

        public static void ReadDadaLinkInfor(Panel dp,AbstractLFCDataObject ao)
        {
            foreach(FrameworkElement fe in dp.Children)
            {
                if(IsDataLinkArea(fe)){
                    string tn = getFieldNameWithoutPrefix(fe.Name);
                    LFCDataService ls = new LFCDataService();
                    FieldInfo fo=ls.getFieldByName(ao,tn);
                    if (fo != null)
                    {
                        ReadDadaLinkInfor(fe, fo, ao);
                    }
                }
            }
        }

        public static void ReadDadaLinkInfor(FrameworkElement fe, FieldInfo f, AbstractLFCDataObject ao)
        {
            Type ft = fe.GetType();
            if (ft.Name == "TextBox")
            {
                TextBox tb = fe as TextBox;
                object v=f.GetValue(ao);
                if (v != null)
                {
                    tb.Text = v.ToString();
                }
                else
                {
                    tb.Text = "";
                }
            }
            if (ft.Name == "TextBlock")
            {
                TextBlock tb = fe as TextBlock;
                object v = f.GetValue(ao);
                if (v != null)
                {
                    tb.Text = v.ToString();
                }
                else
                {
                    tb.Text = "";
                }
            }
            if (ft.Name == "DatePicker")
            {
                LynxDateTimeSelect tb = fe as LynxDateTimeSelect;
                object v = f.GetValue(ao);
                if (v != null)
                {
                    tb.Text = v.ToString();
                    tb.SelectedDate = Convert.ToDateTime(tb.Text);
                }
                else
                {
                    tb.Text = "";
                }

                
            }
            if (ft.Name == "LynxUpDown")
            {
                LynxUpDown t = fe as LynxUpDown;
                object v = f.GetValue(ao);
                if (v != null)
                {
                    t.DoubleValue = Convert.ToDouble(v);
                }
                else
                {
                    t.DoubleValue = 0;
                }
            }
        }

        public static void WriteDadaLinkInfor(Panel dp, AbstractLFCDataObject ao)
        {
            foreach (FrameworkElement fe in dp.Children)
            {
                if (IsDataLinkArea(fe))
                {
                    string tn = getFieldNameWithoutPrefix(fe.Name);
                    LFCDataService ls = new LFCDataService();
                    FieldInfo fo = ls.getFieldByName(ao,tn);
                    if (fo != null)
                    {
                        WriteDadaLinkInfor(fe, fo, ao);
                    }
                }
            }
        }

        public static void WriteDadaLinkInfor(FrameworkElement fe, FieldInfo f, AbstractLFCDataObject ao)
        { 
            Type ft = fe.GetType();
            if ((f.FieldType.Name == "string") || (f.FieldType.Name == "String"))
            {
                if (ft.Name == "TextBox")
                {
                    TextBox tb = fe as TextBox;
                    if (f.GetValue(ao).ToString().Equals(tb.Text)) { }
                    else
                    {
                        f.SetValue(ao, tb.Text);
                        ao.DataFlag = DataOperation.Update;
                    }
                }

            }

            if (f.FieldType.Name == "Int32")
            {
                if (ft.Name == "TextBox")
                {
                    TextBox tb = fe as TextBox;
                    if (f.GetValue(ao).ToString().Equals(tb.Text)) { }
                    else
                    {
                        f.SetValue(ao, Convert.ToInt32(tb.Text));
                        ao.DataFlag = DataOperation.Update;
                    }
                    
                }

                if (ft.Name == "LynxUpDown")
                {
                    LynxUpDown t = fe as LynxUpDown;
                    if (t.IntValue == Convert.ToInt32(f.GetValue(ao))) { }
                    else
                    {
                        f.SetValue(ao, t.IntValue);
                        ao.DataFlag = DataOperation.Update;
                    }
                }
            }

            if (f.FieldType.Name == "Int64")
            {
                if (ft.Name == "TextBox")
                {
                    TextBox tb = fe as TextBox;

                    if (f.GetValue(ao).ToString().Equals(tb.Text)) { }
                    else
                    {
                        f.SetValue(ao, Convert.ToInt64(tb.Text));
                        ao.DataFlag = DataOperation.Update;
                    }
                }

                if (ft.Name == "LynxUpDown")
                {
                    LynxUpDown t = fe as LynxUpDown;
                    if (t.IntValue == Convert.ToInt32(f.GetValue(ao))) { }
                    else
                    {
                        f.SetValue(ao, t.IntValue);
                        ao.DataFlag = DataOperation.Update;
                    }
                }
            }

            if (f.FieldType.Name == "DateTime")
            {
                if (ft.Name == "TextBox")
                {
                    TextBox tb = fe as TextBox;

                    if (f.GetValue(ao).ToString().Equals(tb.Text)) { }
                    else
                    {
                        f.SetValue(ao, Convert.ToDateTime(tb.Text));
                        ao.DataFlag = DataOperation.Update;
                    }
                }

                if (ft.Name == "DatePicker")
                {
                    LynxDateTimeSelect tb = fe as LynxDateTimeSelect;
                    if (f.GetValue(ao).ToString().Equals(tb.SelectedDate.ToString())) { }
                    else
                    {
                        f.SetValue(ao, Convert.ToDateTime(tb.SelectedDate));
                        ao.DataFlag = DataOperation.Update;
                    }

                }
            }

        }
    }
}
