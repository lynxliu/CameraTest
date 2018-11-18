using System;
using System.Net;
using System.Windows;
using System.Linq;
using SLPhotoTest.UIControl;
using DCTestLibrary;
using System.Collections.Generic;
using SilverlightLFC.common;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;

namespace SLPhotoTest.ChartTest
{
    public class ChartTestHelper//包含基本的方法，和数据结构，主要针对多窗体
    {//定义ChartTest窗体的行为

        public DateTime BeginTime, EndTime;
        IChartTestWindow TargetWindow;

        public ChartTestHelper(IChartTestWindow Window)
        {
            TargetWindow = Window;
            TargetWindow.PIToolbar.setTarget(TargetWindow.ChartPhoto);
            TargetWindow.PhotoList.TargetControl = TargetWindow.ChartPhoto;
            TargetWindow.PhotoList.photoOperated += new UIControl.PhotoOperation(PhotoList_photoModifyed);

            TargetWindow.PEToolbar.setChartControl(TargetWindow.PhotoList);//设置图版和工具栏的关联
            TargetWindow.PEToolbar.saveChartTestResult = WriteToHTML;
            TargetWindow.PEToolbar.setTestAll(BeginAll);
            TargetWindow.ChartPhoto.InitTest(Begin);

        }
        public void Clear()
        {
            if (TargetWindow == null) { return; }
            TargetWindow.PIToolbar.setTarget(null);
            TargetWindow.PhotoList.TargetControl = null;
            TargetWindow.PhotoList.photoOperated -= new UIControl.PhotoOperation(PhotoList_photoModifyed);

            TargetWindow.PEToolbar.setChartControl(null);//设置图版和工具栏的关联
            TargetWindow.PEToolbar.saveChartTestResult = null;
            TargetWindow.PEToolbar.setTestAll(null);
            TargetWindow.ChartPhoto.InitTest(null);

            foreach (KeyValuePair<WriteableBitmap, AbstractTestChart> kvp in ChartTestList)
            {
                kvp.Value.Clear();

            }
            ChartTestList.Clear();
        }
        void PhotoList_photoModifyed(WriteableBitmap photo, PhotoListOperation po)
        {
            if (po == PhotoListOperation.Add)
            {
                ProcAdd(photo);
            }
            if (po == PhotoListOperation.Remove)
            {
                ProcRemove(photo);
            }
            if (po == PhotoListOperation.Select)
            {
                ProcSelect(photo);
            }

        }

        void ProcAdd(WriteableBitmap b)
        {
            AbstractTestChart xt = TargetWindow.getChartTest(b);
            ChartTestList.Add(b, xt);
        }

        void ProcRemove(WriteableBitmap b)
        {
            if (ChartTestList.ContainsKey(b))
            {
                ChartTestList[b].Clear();
                List<IParameter> r = ParameterList.Where(v => v.TestChart == ChartTestList[b]).ToList();
                foreach (IParameter ip in r)
                {
                    ParameterList.Remove(ip);
                }
                r.Clear();
                ChartTestList.Remove(b);
            }
            if (ChartTestList.Count > 0)
            {
                ProcSelect(ChartTestList.ElementAt(0).Key);//删除以后选择第一个
            }
            else
            {
                ProcSelect(null);//删光了
            }
        }

        void ProcSelect(WriteableBitmap b)
        {
            //if (b == null)
            //{
            //    TargetWindow.ShowTestResult();
            //}
            //else
            //{
                TargetWindow.ShowTestResult(b);
                TargetWindow.ChartPhoto.setPhoto(b);
                if ((b != null)&&(ChartTestList.ContainsKey(b)))
                {
                    CurrentChart = ChartTestList[b];
                    
                    if (TargetWindow.ChartCorrectToolbar != null)
                    {
                        TargetWindow.ChartCorrectToolbar.Init(ChartTestList[b], TargetWindow.ChartPhoto);//初始化管理按钮
                    }
                }
                else
                {
                    CurrentChart = null;
                }
        }
        void InitChartTestList()
        {
            ChartTestList.Clear();
            foreach (WriteableBitmap tb in TargetWindow.PhotoList.PhotoList)
            {
                ChartTestList.Add(tb, TargetWindow.getChartTest(tb));
            }
        }
        public bool TestChartNull()//判断是否有合法的照片
        {
            if (TargetWindow.ChartPhoto.getPhoto() == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("请导入照片");
                return false;
            }
            return true;
        }

        public List<IParameter> ParameterList = new List<IParameter>();
        List<ParameterTest> TaskList = new List<ParameterTest>();
        DispatcherTimer time = new DispatcherTimer();
        public Dictionary<WriteableBitmap, AbstractTestChart> ChartTestList = new Dictionary<WriteableBitmap, AbstractTestChart>();
        public AbstractTestChart CurrentChart;
        bool IsTesting = false;
        int PhotoIndex = -1;
        int pi = -1;
        //public ShowTestResult showTestResult;
        //public GetTestSteps getTestSteps;

        public void BeginAll()
        {
            if (IsTesting) { return; }
            BeginTime = DateTime.Now;
            IsTesting = true;
            ParameterList.Clear();
            time.Interval = TimeSpan.FromSeconds(2);
            TargetWindow.TestProcessbar.Value = 0;
            time.Tick += new EventHandler(time_Tick);
            TaskList.Clear();
            foreach (WriteableBitmap tb in TargetWindow.PhotoList.PhotoList)
            {
                TaskList.Add(MoveToNextPhoto);
                TaskList.Add(PreProcess);
                //if (getTestSteps != null)
                //{
                    List<ParameterTest> spl = TargetWindow.GetTestSteps();
                    foreach (ParameterTest pt in spl)
                    {
                        TaskList.Add(pt);
                    }
                //}
            }
            pi = -1;
            PhotoIndex = -1;
            time.Start();
        }
        public void Begin()
        {
            if (IsTesting) { return; }
            BeginTime = DateTime.Now;
            IsTesting = true;
            ParameterList.Clear();
            TargetWindow.TestProcessbar.Value = 0;
            time.Interval = TimeSpan.FromSeconds(2);
            time.Tick += time_Tick;
            //PreProcess();
            TaskList.Clear();

            PhotoIndex = TargetWindow.PhotoList.getPhotoIndex(TargetWindow.ChartPhoto.getPhoto());
            if(ChartTestList.ContainsKey(TargetWindow.ChartPhoto.getPhoto()))
                CurrentChart = ChartTestList[TargetWindow.ChartPhoto.getPhoto()];
            TaskList.Add(PreProcess);
            if (TargetWindow.GetTestSteps() != null)
            {
                List<ParameterTest> spl = TargetWindow.GetTestSteps();
                foreach (ParameterTest pt in spl)
                {
                    TaskList.Add(pt);
                }
            }
            pi = -1;
            time.Start();
        }
        public void MoveToNextPhoto()
        {
            PhotoIndex++;
            CurrentChart = ChartTestList[TargetWindow.PhotoList.getPhoto(PhotoIndex)];
        }
        public void ProcessError(Exception xe, string DefaultStr)
        {
            IsTesting = false;
            time.Stop();
            time.Tick -= time_Tick;
            if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
            {
                var md=new MessageDialog(xe.Message);
                var x=md.ShowAsync();
            }
            else
            {
                var md = new MessageDialog(DefaultStr);
                var x = md.ShowAsync();
            }
        }
        void time_Tick(object sender,object e)
        {
            time.Stop();
            pi++;
            if (pi < TaskList.Count)
            {
                TaskList[pi]();
                TargetWindow.TestProcessbar.Value = TargetWindow.TestProcessbar.Value + (100d / TaskList.Count);
                time.Start();
            }
            else
            {
                TestFinish();
                TargetWindow.ShowTestResult(null);
                //if (showTestResult != null)
                //{
                //    showTestResult();
                //}
                return;
            }
        }
        public void PreProcess()
        {
            try
            {
                CurrentChart.CorrectChart();
                CurrentChart.BeginAnalyse();
            }
            catch (Exception xe)//未知的异常
            {
                ProcessError(xe, "测试卡矫正错误，请检查照片");

            }
        }
        public void TestFinish()
        {
            //ResultPanel.Height = 25 * 11;
            //IsTested = true;
            //TestProject.TestHtml.Add(WriteHTML());
            time.Tick -= time_Tick;
            TargetWindow.TestProcessbar.Value = 100;
            IsTesting = false;
            EndTime = DateTime.Now;
            pi = 0;
            ToolTipService.SetToolTip(TargetWindow.TestProcessbar, "本次测试耗时(ms)：" + (EndTime - BeginTime).TotalMilliseconds.ToString());

            //time.Start();
        }

        public void ShowTestResult(Button b, string Name, AbstractTestChart wb)
        {
            List<IParameter> pl;
            pl = ParameterList.Where(v => v.Name == Name).ToList();
            if ((pl != null) && pl.Count > 0)
            {
                string tv = "No Value";
                if (wb == null) { tv = pl.Average(v => v.Value).ToString(); }
                else
                {
                    List<IParameter> tpl = pl.Where(v => v.TestChart == wb).ToList();
                    if ((tpl != null) && (tpl.Count > 0))
                        tv = tpl.Average(v => v.Value).ToString();
                }
                b.Content = Name +":"+ tv + pl[0].Dimension;
                b.SetValue(ToolTipService.ToolTipProperty, Name + ":" + tv + pl[0].Dimension+","+pl[0].Memo + "(平均耗时：" + pl.Average(v => v.SpendTime).ToString() + ")");
                //b.Tag = wb;
            }
            else
            {
                b.Content = Name +":无测试数据";
            }
        }

        public void ShowTestResult(TextBlock b, string Name, AbstractTestChart wb)
        {
            List<IParameter> pl;
            pl = ParameterList.Where(v => v.Name == Name).ToList();
            if ((pl != null) && pl.Count > 0)
            {
                string tv = "No Value";
                if (wb == null) { tv = pl.Average(v => v.Value).ToString(); }
                else
                {
                    List<IParameter> tpl = pl.Where(v => v.TestChart == wb).ToList();
                    if ((tpl != null) && (tpl.Count > 0))
                        tv = tpl.Average(v => v.Value).ToString();
                }
                b.Text = tv + pl[0].Dimension;
                b.SetValue(ToolTipService.ToolTipProperty, Name + ":" + tv + pl[0].Dimension+","+pl[0].Memo + "(平均耗时：" + pl.Average(v => v.SpendTime).ToString() + ")");
                //b.Tag = wb;
            }
        }


        public LParameter getNewParameter(string TestWay,double r,DateTime _BeginTime,DateTime _EndTime)
        {
            LParameter lp = new LParameter(TestWay, CurrentChart);
            lp.TestTime = _EndTime;
            lp.SpendTime = (_EndTime - _BeginTime).TotalMilliseconds;
            lp.Value = r;
            //lp.TestChart = CurrentChart.ChartPhoto;
            //lp.TestWay = TestWay;
            return lp;
        }
        public void WriteToHTML()
        {
            string s = "";

            s = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN/\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\">";
            s = s + "<head><meta http-equiv=\"Content-Language\" content=\"zh-cn\" /><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";
            s = s + "<title>shiyan</title><style type=\"text/css\">.style1 {text-align: left;}.style2 {margin-left: 40px;}.style3 {	text-decoration: underline;	margin-left: 40px;}</style></head>";
            s = s + "<body>";

            s = s + "<h2>测试结果信息：</h2>";
            foreach (LParameter lp in ParameterList)
            {
                s = s + "指标名称：" + lp.Name;
                s = s + "<p class=\"style2\">说明：" + lp.Memo + "</p>";
                s = s + "<p>测试时间：" + lp.TestTime.ToString() + "</p>";
                if (lp.Value != Double.NaN)
                {
                    s = s + "<p>测试值：" + lp.Value.ToString() + lp.Dimension + "</p>";
                }
            }
            s = s + "</body></html>";
            var filter = new Dictionary<string, IList<string>>();
            filter.Add("Html file", new List<string>() { "*.html", "*.htm" });
            SilverlightLFC.common.Environment.getEnvironment().SaveFileString(s, filter);

        }

        static SolidColorBrush GBFailBrush = new SolidColorBrush(Colors.Red);
        static SolidColorBrush GBSuccessBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));

        public static void setGBSign(bool IsSuccess, Control signControl)
        {
            if (IsSuccess)
            {
                signControl.Background = GBSuccessBrush;
                ToolTipService.SetToolTip(signControl, "该项指标通过国标测试");
            }
            else
            {
                signControl.Background = GBFailBrush;
                ToolTipService.SetToolTip(signControl, "该项指标未通过国标测试");
            }
        }
        public static void setGBSign(bool IsSuccess, Panel signControl)
        {
            if (IsSuccess)
            {
                signControl.Background = GBSuccessBrush;
                ToolTipService.SetToolTip(signControl, "该项指标通过国标测试");
            }
            else
            {
                signControl.Background = GBFailBrush;
                ToolTipService.SetToolTip(signControl, "该项指标未通过国标测试");
            }
        }

    }

    //public delegate void ShowTestResult();
    //public delegate List<ParameterTest> GetTestSteps();
}
