using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;
using ImageTest;
using SilverlightDCTestLibrary;
using System.IO;
using SilverlightLynxControls;
using DCTestLibrary;
using SilverlightLFC.common;
using SLPhotoTest.ChartTest;
using SLPhotoTest.PhotoInfor;
using SLPhotoTest.UIControl;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace SLPhotoTest.PhotoTest
{
    public partial class ISO12233Test : UserControl, IChartTestWindow
    {
        public PhotoEditToolbar PEToolbar { get { return photoEditToolbar; } }
        public PhotoInforToolbar PIToolbar { get { return photoInforTool1; } }
        public LPhotoList PhotoList { get { return lPhotoList1; } }
        public LChartPhoto ChartPhoto { get { return lChartPhoto; } }
        public ProgressBar TestProcessbar { get { return processbar; } }
        public ChartCorrect ChartCorrectToolbar { get { return chartCorrect1; } }
        public AbstractTestChart getChartTest(WriteableBitmap b)
        {
            return new ISO12233Chart(b);
        }
        public List<ParameterTest> GetTestSteps()
        {
            List<ParameterTest> pl = new List<ParameterTest>();
            pl.Add(TestRayleiResolution);
            pl.Add(TestHMTF);
            pl.Add(TestVMTF);
            pl.Add(TestHDispersiveness);
            pl.Add(TestVDispersiveness);
            return pl;
        }
        ChartTestHelper testHelper;
        //public CameraTestProject TestProject;
        //public bool IsTested = false;//判断是否进行过测试
        //public WriteableBitmap ProcPhoto;
        //public WriteableBitmap SourcePhoto;
        public ISO12233Test()
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
            ci = new CameraTestIcon(this,null,"ISO12233","ISO12233测试卡");
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
        //    t.setChart(ChartPhoto.getPhoto());
        //    return true;
        //}
        CameraTestIcon ci;
        DateTime BeginTime = DateTime.Now;
        DateTime EndTime = DateTime.Now;
        //public List<IParameter> ParameterList = new List<IParameter>();
        //System.Windows.Threading.DispatcherTimer time = new System.Windows.Threading.DispatcherTimer();
        //int pi = 0;
        //bool IsTesting = false;
        //public void Begin()
        //{
        //    if (IsTesting) { return; }
        //    IsTesting = true;
        //    time.Interval = TimeSpan.FromSeconds(2);
        //    time.Tick += new EventHandler(time_Tick);
        //    ParameterList.Clear();
        //    PreProcess();
        //    pi = 0;
        //    time.Start();
        //}
        //public void PreProcess()
        //{
        //    try
        //    {
        //        t.setChart(ChartPhoto.getPhoto());
        //        //t.PhotoTestProc += new PhotoTestProcessHandler(t_PhotoTestProc);
        //        t.CorrectChart();
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
        //    //ResultPanel.Height = 25 * 11;
        //    //IsTested = true;
        //    //TestProject.TestHtml.Add(WriteHTML());
        //    time.Tick -= new EventHandler(time_Tick);
        //    processbar.Value = 100;
        //    IsTesting = false;
        //    EndTime = DateTime.Now;
        //    pi = 0;
        //    ToolTipService.SetToolTip(processbar, "本次测试耗时(ms)：" + (EndTime - BeginTime).TotalMilliseconds.ToString());

        //    //time.Start();
        //}
        //void time_Tick(object sender, EventArgs e)
        //{
        //    time.Stop();
        //    pi++;
        //    if (pi == 1) { TestRayleiResolution(); }
        //    if (pi == 2) { TestHMTF(); }
        //    if (pi == 3) { TestVMTF(); }
        //    if (pi == 4) { TestHDispersiveness(); }
        //    if (pi == 5) { TestVDispersiveness(); }
        //    if (pi == 6) { TestFinish(); return; }
        //    time.Start();
        //}
        ActionMove am;
        //public List<string> ParameterHtml = new List<string>();

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

        //ISO12233Chart t = new ISO12233Chart();

        void TestRayleiResolution()
        {
            try
            {
                BeginTime = DateTime.Now;

                long r = (testHelper.CurrentChart as ISO12233Chart).getLPResoveLines();
                buttonRayleiResolution.Content = "分辨率（中央）：" + r.ToString() + " LW/PH";
                buttonRayleiResolution.SetValue(ToolTipService.ToolTipProperty, "利用瑞利判据，直观判断相机分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量");
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("ISO2233测试卡",Convert.ToDouble(r),BeginTime,EndTime);
                lp.Name = "中央瑞利分辨率";
                lp.Memo = "利用瑞利判据，直观判断相机分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量";
                lp.Dimension = "LW/PH";

                //lp.TestWay = "ISO2233测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = r;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 20;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "中央分辨率（瑞利判据）测试错误");
                //if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
                //{
                //    SilverlightLFC.common.Environment.ShowMessage(xe.Message);
                //}
                //else
                //{
                //    SilverlightLFC.common.Environment.ShowMessage("中央分辨率（瑞利判据）测试错误");
                //    //throw new SilverlightLFC.common.LFCException("中央分辨率（瑞利判据）测试错误", null);
                //}
            }
        }

        void TestHMTF()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as ISO12233Chart).getHEdgeResoveLines();
                buttonHMTF.Content = "分辨率（水平线）：" + r.ToString() + " LW/PH";
                buttonHMTF.SetValue(ToolTipService.ToolTipProperty, "利用水平线条测试分辨率，接近相机的中心分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量");
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("ISO2233测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "中央分辨率（水平线）";
                lp.Memo = "利用水平线条测试分辨率，接近相机的中心分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量";
                lp.Dimension = "LW/PH";

                //lp.TestWay = "ISO2233测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = r;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 40;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "中央分辨率（水平线）测试错误");
            }
        }

        void TestVMTF()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as ISO12233Chart).getVEdgeResoveLines();
                buttonVMTF.Content = "分辨率（垂直线）：" + r.ToString() + " LW/PH";
                buttonVMTF.SetValue(ToolTipService.ToolTipProperty, "利用垂直线条测试分辨率，接近相机的边缘分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量");
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("ISO2233测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "中央分辨率（垂直线）";
                lp.Memo = "利用垂直线条测试分辨率，接近相机的边缘分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量";
                lp.Dimension = "LW/PH";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                //lp.TestWay = "ISO2233测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = r;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 60;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "中央分辨率（垂直线线）测试错误");
            }
        }

        void TestHDispersiveness()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal d = (testHelper.CurrentChart as ISO12233Chart).getHDispersiveness();
                buttonHDispersiveness.Content = "色散（水平线）：" + d.ToString() + " pxl";
                buttonHDispersiveness.SetValue(ToolTipService.ToolTipProperty, "利用水平线条测试，接近中央色散程度，数据表示红绿蓝三原色在边界分离的程度，无色散时为0");
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("ISO2233测试卡", Convert.ToDouble(d), BeginTime, EndTime);
                lp.Name = "色散（水平线）";
                lp.Memo = "利用水平线条测试，接近中央色散程度，数据表示红绿蓝三原色在边界分离的程度，无色散时为0";
                lp.Dimension = "Pix";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                //lp.TestWay = "ISO2233测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(d);
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 85;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "色散（水平线）测试错误");
            }
        }

        void TestVDispersiveness()
        {
            try{
                BeginTime = DateTime.Now;
                decimal d = (testHelper.CurrentChart as ISO12233Chart).getVDispersiveness();
                buttonVDispersiveness.Content = "色散（垂直线）：" + d.ToString() + " pxl";
                buttonVDispersiveness.SetValue(ToolTipService.ToolTipProperty, "利用垂直线条测试，接近边缘色散程度，数据表示红绿蓝三原色在边界分离的程度，无色散时为0");
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("ISO2233测试卡", Convert.ToDouble(d), BeginTime, EndTime);
                lp.Name = "色散（垂直线）";
                lp.Memo = "利用垂直线条测试，接近边缘色散程度，数据表示红绿蓝三原色在边界分离的程度，无色散时为0";
                lp.Dimension = "Pix";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                //lp.TestWay = "ISO2233测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(d);
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 95;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "色散（垂直线）测试错误");

            }
        }
        //void autoTest()
        //{
        //    if (!TestChartNull()) { return; }
        //    //try{
        //        BeginTime = DateTime.Now;
        //        Begin();
        //        EndTime = DateTime.Now;
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    SilverlightLFC.common.Environment.ShowMessage(e.Message);
        //    //}
        //}
        //public void Test()
        //{
        //    //mark = new MarkProcess(ProcPhoto);
        //    if (ChartPhoto.getChart() == null) { return; }
        //    processbar.Value = 0;
        //    //ResultPanel.Children.Clear();

        //    //Button li=;
        //    t.setChart(ChartPhoto.getChart());
        //    //t.PhotoTestProc += new PhotoTestProcessHandler(t_PhotoTestProc);
        //    t.CorrectISO12233Chart();
        //    t.BeginAnalyse();
        //    //t.sendProcEvent(true, "");
        //    //processbar.Value = 30;

        //    sw.Stop();
        //    sw.Start();

        //    long r = t.getLPResoveLines();
        //    //li = new Button();
        //    //li.Name = "ResolvingPower";
        //    buttonRayleiResolution.Content = "分辨率（中央）：" + r.ToString() + " LW/PH";
        //    buttonRayleiResolution.SetValue(ToolTipService.ToolTipProperty, "利用瑞利判据，直观判断相机分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量");
        //    //li.Click += new RoutedEventHandler(RayleiResolution_Click);
        //    //ResultPanel.Children.Add(li);
        //    //t.sendProcEvent(true, "");
        //    //processbar.Value = 50;

        //    r = t.getHEdgeResoveLines();
        //    //li = new Button();
        //    //li.Name = "HResolvingPower";
        //    buttonHMTF.Content = "分辨率（水平线）：" + r.ToString() + " LW/PH";
        //    buttonHMTF.SetValue(ToolTipService.ToolTipProperty, "利用水平线条测试分辨率，接近相机的中心分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量");
        //    //li.Click += new RoutedEventHandler(HMTF_Click);
        //    //ResultPanel.Children.Add(li);
        //    //t.sendProcEvent(true, "");
        //    //processbar.Value = 60;

        //    r = t.getVEdgeResoveLines();
        //    //li = new Button();
        //    //li.Name = "VResolvingPower";
        //    buttonVMTF.Content = "分辨率（垂直线）：" + r.ToString() + " LW/PH";
        //    buttonVMTF.SetValue(ToolTipService.ToolTipProperty, "利用垂直线条测试分辨率，接近相机的边沿分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量");
        //    //li.Click += new RoutedEventHandler(VMTF_Click);
        //    //ResultPanel.Children.Add(li);
        //    //t.sendProcEvent(true, "");
        //    //processbar.Value = 70;

        //    //li = new Button();
        //    decimal d = t.getHDispersiveness();
        //    //li.Name = "HDispersiveness";
        //    buttonHDispersiveness.Content = "色散（水平线）：" + d.ToString() + " pxl";
        //    buttonHDispersiveness.SetValue(ToolTipService.ToolTipProperty, "利用水平线条测试，接近中央色散程度，数据表示红绿蓝三原色在边界分离的程度，无色散时为0");
        //    //li.Click += new RoutedEventHandler(VDispersiveness_Click);
        //    //ResultPanel.Children.Add(li);
        //    //t.sendProcEvent(true, "");
        //    //processbar.Value = 85;

        //    //li = new Button();
        //    d = t.getVDispersiveness();
        //    //li.Name = "VDispersiveness";
        //    buttonVDispersiveness.Content = "色散（垂直线）：" + d.ToString() + " pxl";
        //    buttonVDispersiveness.SetValue(ToolTipService.ToolTipProperty, "利用垂直线条测试，接近边沿色散程度，数据表示红绿蓝三原色在边界分离的程度，无色散时为0");
        //    //li.Click += new RoutedEventHandler(HDispersiveness_Click);
        //    //ResultPanel.Children.Add(li);
        //    //t.sendProcEvent(true, "");
        //    //processbar.Value = 100;

        //    sw.Stop();
        //    ResultPanel.SetValue(ToolTipService.ToolTipProperty, "测试共花费" + sw.Interval.Milliseconds.ToString() + "ms");

        //    IsTested = true;
        //    TestProject.TestHtml.Add(WriteHTML());
        //}
        //double PrcoStep = 100 / 6d;
        //void t_PhotoTestProc(object sender, PhotoTestProcessEventArgs e)
        //{
        //    processbar.Value = processbar.Value + PrcoStep;
        //    if (processbar.Value >= 99.5)
        //    {
        //        t.PhotoTestProc -= new PhotoTestProcessHandler(t_PhotoTestProc);
        //    }
        //}
        void RayleiResolution_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            RayleiResolution pc = new RayleiResolution();

            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (ChartPhoto.IsSelect)
            {
                pc.Test(ChartPhoto.getSelectArea());
            }
            else
            {
                if (testHelper.CurrentChart == null)
                {
                    SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                    return;
                }
                if (testHelper.CurrentChart.ProcessInfor.ContainsKey("RayleiResolutionIsLeft"))
                {
                    if (Convert.ToBoolean(testHelper.CurrentChart.ProcessInfor["RayleiResolutionIsLeft"]))
                    {
                        pc.IsLeft = true;
                        pc.Test(testHelper.CurrentChart.mp.SelectedArea["RayleiResolutionLArea"]);
                    }
                    else
                    {
                        pc.IsLeft = false;
                        pc.Test(testHelper.CurrentChart.mp.SelectedArea["RayleiResolutionRArea"]);
                    }
                }
            }
        }
        void HMTF_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            SFRResolution pc = new SFRResolution();
            pc.IsV = true;
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (ChartPhoto.IsSelect)
            {
                pc.Test(ChartPhoto.getSelectArea());
            }
            else
            {
                if (testHelper.CurrentChart == null)
                {
                    SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                    return;
                }
                if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaVEdge"))
                {
                    pc.Test(testHelper.CurrentChart.mp.SelectedArea["AreaVEdge"]);
                }
            }
        }
        void VMTF_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            SFRResolution pc = new SFRResolution();
            pc.IsV = false;
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (ChartPhoto.IsSelect)
            {
                pc.Test(ChartPhoto.getSelectArea());
            }
            else
            {
                if (testHelper.CurrentChart == null)
                {
                    SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                    return;
                }
                if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaHEdge"))
                {
                    pc.Test(testHelper.CurrentChart.mp.SelectedArea["AreaHEdge"]);
                }
            }
        }
        void VDispersiveness_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            Dispersiveness pc = new Dispersiveness();
            pc.IsV = false;
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (ChartPhoto.IsSelect)
            {
                pc.Test(ChartPhoto.getSelectArea());
            }
            else
            {
                if (testHelper.CurrentChart == null)
                {
                    SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                    return;
                }
                if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaHEdge"))
                {
                    pc.Test(testHelper.CurrentChart.mp.SelectedArea["AreaHEdge"]);
                }
            }
        }

        void HDispersiveness_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            Dispersiveness pc = new Dispersiveness();
            pc.IsV = true;
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (ChartPhoto.IsSelect)
            {
                pc.Test(ChartPhoto.getSelectArea());
            }
            else
            {
                if (testHelper.CurrentChart == null)
                {
                    SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                    return;
                }
                if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaVEdge"))
                {
                    pc.Test(testHelper.CurrentChart.mp.SelectedArea["AreaVEdge"]);
                }
            }
        }

        //private void buttonOpen_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog of = new OpenFileDialog();
        //    of.ShowDialog();
        //    if (of.File == null) { return; }
        //    if (!of.File.Exists)
        //    {
        //        return;
        //    }
        //    Stream s = of.File.OpenRead();
        //    BitmapImage bi = new BitmapImage();
        //    bi.SetSource(s);
        //    //ProcPhoto=new WriteableBitmap(bi);
        //    ChartPhoto.setPhoto(new WriteableBitmap(bi));
        //    //ChartPhoto.setImage(SourcePhoto);
        //}

        //private void CorrectRotate_Click(object sender, RoutedEventArgs e)
        //{
        //    ChartPhoto.setPhoto(testHelper.CurrentChart.mp.CorrectRotatePhoto);
        //}

        //private void CorrectScale_Click(object sender, RoutedEventArgs e)
        //{
        //    ChartPhoto.setPhoto(testHelper.CurrentChart.mp.CorrectScalePhoto);
        //}

        //private void select_Click(object sender, RoutedEventArgs e)
        //{
        //    ChartPhoto.setPhoto(testHelper.CurrentChart.mp.SelectedPhoto);
            
        //}

        //private void buttonEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    //ImageEdit ie = new ImageEdit(ChartPhoto.getChart());
        //    //CameraTestDesktop.getDesktopPanel().Children.Add(ie);
        //}
        
        //public string getName()
        //{
        //    throw new NotImplementedException();
        //}

        //public string getMemo()
        //{
        //    throw new NotImplementedException();
        //}

        //public string getDimension()
        //{
        //    throw new NotImplementedException();
        //}

        //public double getValue()
        //{
        //    throw new NotImplementedException();
        //}

        //public double getScore()
        //{
        //    throw new NotImplementedException();
        //}

        //public string getReport()
        //{
        //    throw new NotImplementedException();
        //}


        //public List<IParameter> getSubParameter()
        //{
        //    throw new NotImplementedException();
        //}

        //public double getProcess()
        //{
        //    throw new NotImplementedException();
        //}

        //public ParameterState getState()
        //{
        //    throw new NotImplementedException();
        //}

        //private void buttonTest_Click(object sender, RoutedEventArgs e)
        //{
        //    autoTest();
        //}

        //public void Test(WriteableBitmap b)
        //{
        //    throw new NotImplementedException();
        //}

        //private void buttonCopy_Click(object sender, RoutedEventArgs e)
        //{
        //    CameraTestDesktop d = CameraTestDesktop.getDesktop();
        //    d.addClip(ChartPhoto.getPhoto());
        //}

        //private void buttonPaste_Click(object sender, RoutedEventArgs e)
        //{
        //    CameraTestDesktop d = CameraTestDesktop.getDesktop();
        //    ChartPhoto.setPhoto (d.getClip());
        //}

        void CloseWindow()
        {
            testHelper.Clear();
            //SourcePhoto = null;
            //image.Source = null;
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

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

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
            testHelper.ShowTestResult(buttonRayleiResolution, "中央瑞利分辨率", ab);
            testHelper.ShowTestResult(buttonHMTF, "中央分辨率（水平线）", ab);
            testHelper.ShowTestResult(buttonVMTF, "中央分辨率（垂直线）", ab);
            testHelper.ShowTestResult(buttonHDispersiveness, "色散（水平线）", ab);
            testHelper.ShowTestResult(buttonVDispersiveness, "色散（垂直线）", ab);
        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap xb = (testHelper.CurrentChart as ISO12233Chart).getAreaLResove((testHelper.CurrentChart as ISO12233Chart).ChartPhoto);
            SilverlightPhotoIO.PhotoIO.WriteImageToFile(xb);
        }
    }
}
