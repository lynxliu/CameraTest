using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightDCTestLibrary;

using SilverlightLynxControls;
using DCTestLibrary;
using System.IO;
using SilverlightLFC.common;
using SLPhotoTest.ChartTest;
using SLPhotoTest.PhotoInfor;
using SLPhotoTest.UIControl;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.Storage.Pickers;

namespace SLPhotoTest.PhotoTest
{
    public partial class XRiteTest : UserControl, IChartTestWindow
    {
        public PhotoEditToolbar PEToolbar { get { return photoEditToolbar; } }
        public PhotoInforToolbar PIToolbar { get { return photoInforTool1; } }
        public LPhotoList PhotoList { get { return lPhotoList1; } }
        public LChartPhoto ChartPhoto { get { return lChartPhoto; } }
        public ProgressBar TestProcessbar { get { return processbar; } }
        public ChartCorrect ChartCorrectToolbar { get { return chartCorrect1; } }
        public AbstractTestChart getChartTest(WriteableBitmap b)
        {
            return new XRiteColorChart(b);
        }
        public List<ParameterTest> GetTestSteps()
        {
            List<ParameterTest> pl = new List<ParameterTest>();
            pl.Add(TestWhiteBalance);
            pl.Add(TestColorTrendValue);
            pl.Add(TestNoise);
            return pl;
        }
        ChartTestHelper testHelper;
        //public CameraTestProject TestProject;
        //public bool IsTested = false;//判断是否进行过测试
        //public WriteableBitmap ProcPhoto;
        //public WriteableBitmap SourcePhoto;
        ActionMove am;
        //XRiteColorChart t = new XRiteColorChart();
        //private DispatcherTimer sw = new DispatcherTimer();
        public XRiteTest()
        {
            InitializeComponent();
            testHelper = new ChartTestHelper(this);
            //photoInforTool1.setTarget(ChartPhoto);
            am = new ActionMove(this, Title);
            //chartCorrect1.Init(t, ChartPhoto);//初始化管理按钮
            //photoEditToolbar.setChartControl(ChartPhoto);//设置图版和工具栏的关联
            //photoEditToolbar.saveChartTestResult = WriteToHTML;
            //ChartPhoto.InitTest(autoTest);
            //ChartPhoto.InitSaveTest(WriteToHTML);
            ci = new CameraTestIcon(this,null,"XRite","XRite色标卡");
            ci.release = CloseWindow;
            CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            if (!cd.TaskList.Children.Contains(ci))
            {
                cd.TaskList.Children.Add(ci);
            }
            else
            {
                ci.Active();
            }
        }
        //bool TestChartNull()//判断是否有合法的照片
        //{
        //    if (ChartPhoto.getPhoto() == null)
        //    {
        //        SilverlightLFC.common.Environment.ShowMessage("请导入照片");
        //        return false;
        //    }
        //    (testHelper.CurrentChart as XRiteColorChart).setChart(ChartPhoto.getPhoto());
        //    return true;
        //}
        CameraTestIcon ci;
        DateTime BeginTime = DateTime.Now;
        DateTime EndTime = DateTime.Now;
        //public List<IParameter> ParameterList = new List<IParameter>();
        //public void WriteToHTML()
        //{

        //    string s = "";

        //    s = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN/\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\">";
        //    s = s + "<head><meta http-equiv=\"Content-Language\" content=\"zh-cn\" /><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";
        //    s = s + "<title>shiyan</title><style type=\"text/css\">.style1 {text-align: left;}.style2 {margin-left: 40px;}.style3 {	text-decoration: underline;	margin-left: 40px;}</style></head>";
        //    s = s + "<body>";

        //    s = s + "<h2>测试结果信息：</h2>";
        //    foreach (LParameter lp in ParameterList)
        //    {
        //        s = s + "指标名称：" + lp.Name;
        //        s = s + "<p class=\"style2\">说明：" + lp.Memo + "</p>";
        //        s = s + "<p>测试时间：" + lp.TestTime.ToString() + "</p>";
        //        if (lp.Value != Double.NaN)
        //        {
        //            s = s + "<p>测试值：" + lp.Value.ToString() + lp.Dimension + "</p>";
        //        }
        //    }
        //    s = s + "</body></html>";
        //    SilverlightLFC.common.Environment.getEnvironment().SaveFileString(s, "HTML文件|*.html");

        //    //SaveFileDialog of = new SaveFileDialog();
        //    //of.Filter = "HTML文件|*.html";
        //    //of.ShowDialog();
        //    //byte[] sb = System.Text.Encoding.Unicode.GetBytes(s);
        //    //Stream fs = of.OpenFile();
        //    //if (fs != null)
        //    //{
        //    //    fs.Write(sb, 0, sb.Length);
        //    //}
        //}

        //System.Windows.Threading.DispatcherTimer time = new System.Windows.Threading.DispatcherTimer();
        //int pi = 0;
        //void autoTest()
        //{
        //    if (!TestChartNull()) { return; }
        //    BeginTime = DateTime.Now;
        //    Begin();
        //    EndTime = DateTime.Now;
        //}
        //bool IsTesting = false;
        //public void Begin()
        //{
        //    if (IsTesting) { return; }
        //    IsTesting = true;
        //    ParameterList.Clear();
        //    time.Interval = TimeSpan.FromSeconds(2);
        //    time.Tick += new EventHandler(time_Tick);
        //    PreProcess();
        //    pi = 0;
        //    time.Start();
        //}
        //public void PreProcess()
        //{
        //    try
        //    {
        //        t.setChart(ChartPhoto.getPhoto());
        //        t.PhotoTestProc += new PhotoTestProcessHandler(t_PhotoTestProc);
        //        //t.corrCorrectXMark();
        //        t.BeginAnalyse();
        //        processbar.Value = 5;
        //    }
        //    catch (Exception xe)//未知的异常
        //    {
        //        if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
        //        {
        //            SilverlightLFC.common.Environment.ShowMessage(xe.Message);
        //        }
        //        else
        //        {
        //            SilverlightLFC.common.Environment.ShowMessage("测试卡矫正错误，请检查照片");
        //            //throw new SilverlightLFC.common.LFCException("测试卡矫正错误", null);
        //        }
        //    }
        //}
        //public void TestFinish()
        //{
        //    time.Tick -= new EventHandler(time_Tick);
        //    processbar.Value = 100;
        //    IsTesting = false;
        //    EndTime = DateTime.Now;
        //    pi = 0;
        //    ToolTipService.SetToolTip(processbar, "本次测试耗时(ms)：" + (EndTime - BeginTime).TotalMilliseconds.ToString());

        //}
        //void time_Tick(object sender, EventArgs e)
        //{
        //    time.Stop();
        //    pi++;
        //    if (pi == 1) { TestWhiteBalance(); }
        //    if (pi == 2) { TestColorTrendValue(); }
        //    if (pi == 3) { TestNoise(); }
        //    if (pi == 4) { TestFinish(); return; }
        //    time.Start();
        //}
        void TestWhiteBalance()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal d = Convert.ToDecimal((testHelper.CurrentChart as XRiteColorChart).getWhiteBanlance()) * 100;
                buttonWhiteBalance.Content = "白平衡误差：" + d.ToString();
                buttonWhiteBalance.Content = buttonWhiteBalance.Content + " %";
                buttonWhiteBalance.SetValue(ToolTipService.ToolTipProperty, "数据表示拍摄灰度的时刻偏离灰度的程度，完全准确时为0");
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XRite色标测试卡", Convert.ToDouble(d), BeginTime, EndTime);
                lp.Name = "白平衡误差";
                lp.Memo = "数据表示拍摄灰度的时刻偏离灰度的程度，完全准确时为0";
                lp.Dimension = "%";
                //lp.TestWay = "XRite色标测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(d);
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 20;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "计算白平衡误差错误");
            }
        }

        void TestColorTrendValue()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal d = (testHelper.CurrentChart as XRiteColorChart).getColorDistance();
                buttonColorTrendValue.Content = "色彩趋向误差：" + d.ToString() + " 度";
                buttonColorTrendValue.SetValue(ToolTipService.ToolTipProperty, "数据表示在拍摄标准颜色时候色调的偏差，完全准确时为0");
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XRite色标测试卡", Convert.ToDouble(d), BeginTime, EndTime);
                lp.Name = "色彩趋向差异";
                lp.Memo = "数据表示在拍摄标准颜色时候色调的偏差，完全准确时为0";
                lp.Dimension = "度";
                //lp.TestWay = "XRite色标测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(d);
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 70;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "计算色彩趋向误差错误");
            }
        }

        void TestNoise()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal d = Convert.ToDecimal((testHelper.CurrentChart as XRiteColorChart).getNoiseNum(25)) * 100;
                buttonNoise.Content = "噪点：" + d.ToString() + " %";
                buttonNoise.SetValue(ToolTipService.ToolTipProperty, "数据表示在一张照片里面可以分辨的噪点数量");
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XRite色标测试卡", Convert.ToDouble(d), BeginTime, EndTime);
                lp.Name = "噪点";
                lp.Memo = "数据表示在一张照片里面可以分辨的噪点数量";
                lp.Dimension = "%";
                //lp.TestWay = "XRite色标测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(d);
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 95;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "计算噪点错误");
            }
        }

        void XRiteWhiteBalance_Click(object sender, RoutedEventArgs e)
        {

            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            WhiteBanlance pc = new WhiteBanlance();
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }
            List<WriteableBitmap> wbl = new List<WriteableBitmap>();
            if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("Area_4_1"))
            {
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_1"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_2"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_3"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_4"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_5"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_6"]);
            }
            pc.Test(wbl);
        }
        void XRiteNoise_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }

            Noise pc = new Noise();
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }
            List<WriteableBitmap> wbl = new List<WriteableBitmap>();
            if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("Area_4_1"))
            {
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_1"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_2"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_3"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_4"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_5"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_4_6"]);
            }
            pc.Test(wbl);
        }
        void XRiteColorDis_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            ColorTrend pc = new ColorTrend();
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }
            List<WriteableBitmap> wbl = new List<WriteableBitmap>();
            List<Color> sl = new List<Color>();
            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 7; j++)
                {
                    if ((testHelper.CurrentChart as XRiteColorChart).mp.SelectedArea.ContainsKey("Area_" + i.ToString() + "_" + j.ToString()))
                    {
                        wbl.Add(testHelper.CurrentChart.mp.SelectedArea["Area_" + i.ToString() + "_" + j.ToString()]);
                        sl.Add((testHelper.CurrentChart as XRiteColorChart).ColorList[(i - 1) * 6 + j - 1]);
                    }
                }
            }
            pc.setStandardColorList(sl);
            pc.Test(wbl);
        }

        //private void buttonCopy_Click(object sender, RoutedEventArgs e)
        //{
        //    CameraTestDesktop d = CameraTestDesktop.getDesktop();
        //    d.addClip(ChartPhoto.getPhoto());
        //}

        //private void buttonPaste_Click(object sender, RoutedEventArgs e)
        //{
        //    CameraTestDesktop d = CameraTestDesktop.getDesktop();
        //    ChartPhoto.setPhoto( d.getClip());
        //}

        void CloseWindow()
        {
            testHelper.Clear();
            ChartPhoto.setPhoto(null);
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
            CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            if (cd.TaskList.Children.Contains(ci))
            {
                cd.TaskList.Children.Remove(ci);
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        //private void buttonTest_Click(object sender, RoutedEventArgs e)
        //{
        //    autoTest();
        //}

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            var b = SilverlightLFC.common.Environment.getEnvironment().OpenImage();
            if (b == null || b.Result.Count == 0) return;
            ChartPhoto.setPhoto(WriteableBitmapHelper.Clone(b.Result.FirstOrDefault()));
        }

        private void buttonToIcon_Click(object sender, RoutedEventArgs e)
        {
            ci.ToIcon();
        }

        private void buttonParameterSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (LParameter lp in testHelper.ParameterList)
            {

                CameraTestDesktop cd = CameraTestDesktop.getDesktop();
                cd.currentProject.AddResult(lp);
            }
        }

        private void buttonClearResult_Click(object sender, RoutedEventArgs e)
        {
            testHelper.ParameterList.Clear();
        }
        private void imageMulti_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lPhotoList1.Visibility = Visibility.Visible;
            e.Handled = true;
        }
        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lPhotoList1.Visibility = Visibility.Collapsed;
        }


        public void ShowTestResult(WriteableBitmap chartPhoto)
        {
            AbstractTestChart ab = null;
            if ((chartPhoto != null) && testHelper.ChartTestList.ContainsKey(chartPhoto))
            {
                ab = testHelper.ChartTestList[chartPhoto];
            }
            testHelper.ShowTestResult(buttonColorTrendValue, "色彩趋向差异", ab);
            testHelper.ShowTestResult(buttonNoise, "噪点", ab);
            testHelper.ShowTestResult(buttonWhiteBalance, "白平衡误差", ab);
        }
    }
}
