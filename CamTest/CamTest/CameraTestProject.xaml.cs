using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SilverlightLynxControls;
using SilverlightDCTestLibrary;
using SLPhotoTest.PhotoTest;

using DCTestLibrary;
using System.IO;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SLPhotoTest
{
    public partial class CameraTestProject : UserControl
    {
        public CameraTestProject()
        {
            InitializeComponent();
            am = new ActionMove(this, this.LayoutRoot);
            CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            
        }
        List<IParameter> ResultList = new List<IParameter>();//保留每一项测试结果

        public void AddResult(IParameter p)
        {
            ResultList.Add(p);
            textBlockParameterCount.Text = ResultList.Count.ToString();
        }

        //public void SaveResult()
        //{
        //    OpenFileDialog of = new OpenFileDialog();
        //    of.Filter = "HTML文件|*.html";
        //    of.ShowDialog();
        //    if (of.File != null)
        //    {
        //        string s=WriteToHTML();
        //        byte[] sb= System.Text.Encoding.Unicode.GetBytes(s);

        //        FileStream fs = of.File.OpenWrite();
        //        fs.Write(sb, 0, sb.Length);
        //    }
        //}

        List<IParameter> ProcessResult()
        {
            List<IParameter> pl = new List<IParameter>();
            List<string> nl = ResultList.Select(v => v.Name).Distinct().ToList();
            List<IParameter> tl=new List<IParameter>();
            foreach (string n in nl)
            {
                tl = ResultList.Where(v => v.Name == n).ToList();
                if (tl.Count > 0)
                {
                    IParameter p = new LParameter();
                    p.Name = tl[0].Name+ "一共测试:" + tl.Count.ToString();
                    p.Memo = p.Memo;
                    p.Dimension = tl[0].Dimension;
                    p.SpendTime = tl.Average(pv => pv.SpendTime);
                    p.TestTime = tl.Max(v => v.TestTime);
                    p.TestWay = tl[0].TestWay;
                    p.Value = tl.Average(pv => pv.Value);
                    pl.Add(p);
                }
            }
            return pl;
        }

        public string WriteToHTML()//输出到HTML格式
        {
            string s = "";
            s = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN/\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\">";
            s = s + "<head><meta http-equiv=\"Content-Language\" content=\"zh-cn\" /><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";
            s = s + "<title>shiyan</title><style type=\"text/css\">.style1 {text-align: left;}.style2 {margin-left: 40px;}.style3 {	text-decoration: underline;	margin-left: 40px;}</style></head>";
            s = s + "<body>";

            s = s + "<h1>" + this.TargetName.Text + "</h1>";
            s = s + "<h2 class=\"style1\">" + DateTime.Now.ToString() + "生成</h2>";
            s = s + "<h3>测试对象：" + this.TargetName.Text + "</h3>";
            s = s + "<p>对象类别：数码相机</p>";
            //s = s + "<p>对象基本概况：" +  + "</p>";
            s = s + "<p>测试时间：" + this.TestBeginTime.Text + "</p>";
            s = s + "<p>测试人：" + this.TestPeople.Text + "</p>";
            s = s + "<h2>测试结果信息：</h2>";

            s = s + "<Table style=\"width: 100%\">";
            s = s + "<TR>";
            s = s + "<TD style=\"width: 15%\">指标名称</TD>";
            s = s + "<TD style=\"width: 55%\">指标说明</TD>";
            s = s + "<TD style=\"width: 30%\">测试值</TD>";
            s = s + "</TR>";
            List<IParameter> pl = ProcessResult();
            foreach (IParameter p in pl)
            {
                s = s + "<TR>";
                s = s + "<TD style=\"width: 15%\">" + p.Name + "</TD>";
                s = s + "<TD style=\"width: 55%\">" + p.Memo + "</TD>";
                if (p.Value!= Double.NaN)
                {
                    s = s + "<TD style=\"width: 30%\">" + p.Value.ToString() + p.Dimension + "</TD>";
                }
                s = s + "</TR>";
            }
            s = s + "</Table>";

            s = s + "</body></html>";
            return s;
        }//把测试结果写为html格式
        
        ActionMove am;
        //public List<IParameter> ParameterList = new List<IParameter>();
        //public List<string> TestHtml = new List<string>();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Standard))
            {
                return;
            }
            var filter=new Dictionary<string,IList<string>>();
            filter.Add("html file", new List<string>() { "*.html", "*.htm" });
            SilverlightLFC.common.Environment.getEnvironment().SaveFileString(WriteToHTML(),filter);

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void Begin_Click(object sender, RoutedEventArgs e)
        {
            TestBeginTime.Text = DateTime.Now.ToString();
            ResultList.Clear();//重建
            textBlockParameterCount.Text = "0";
        }

        private void LayoutRoot_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            cd.currentProject = this;
        }
    }
}
