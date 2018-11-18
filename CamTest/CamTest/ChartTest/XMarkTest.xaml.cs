using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightDCTestLibrary;
using DCTestLibrary;

using SilverlightLynxControls;
using System.IO;
using System.ComponentModel;
using System.Threading;
using SilverlightLFC.common;
using SLPhotoTest.ChartTest;
using SLPhotoTest.UIControl;
using SLPhotoTest.PhotoInfor;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Input;

namespace SLPhotoTest.PhotoTest
{
    public partial class XMarkTest : UserControl, IChartTestWindow
    {
        public PhotoEditToolbar PEToolbar { get { return photoEditToolbar; } }
        public PhotoInforToolbar PIToolbar { get { return photoInforTool1; } }
        public LPhotoList PhotoList { get { return lPhotoList1; } }
        public LChartPhoto ChartPhoto { get { return lChartPhoto; } }
        public ProgressBar TestProcessbar { get { return processbar; } }
        public ChartCorrect ChartCorrectToolbar { get { return chartCorrect1; } }
        public AbstractTestChart getChartTest(WriteableBitmap b)
        {
            return new XMarkChart(b);
        }
        ChartTestHelper testHelper;
        public List<ParameterTest> GetTestSteps()
        {
            List<ParameterTest> pl = new List<ParameterTest>();
            pl.Add(TestAberration);
            pl.Add(TestBrightChanges);
            pl.Add(TestColorTrend);
            pl.Add(TestHEdgeDispersiveness);
            pl.Add(TestVEdgeDispersiveness);
            pl.Add(TestHEdgeResoveLines);
            pl.Add(TestVEdgeResoveLines);
            pl.Add(TestLatitude);
            pl.Add(TestNoise);
            pl.Add(TestPurplePercent);
            pl.Add(TestWaveQ);
            pl.Add(TestWhiteBanlance);
            return pl;
        }
        DispatcherTimer time = new DispatcherTimer();

        ActionMove am;
        //Dictionary<WriteableBitmap, XMarkChart> tl = new Dictionary<WriteableBitmap, XMarkChart>();

        public XMarkTest()
        {
            InitializeComponent();
            testHelper = new ChartTestHelper(this);
            
            am = new ActionMove(this, Title);
            //photoInforTool1.setTarget(ChartPhoto);
            //lPhotoList1.TargetControl = ChartPhoto;
            //lPhotoList1.photoOperated += new UIControl.PhotoOperation(lPhotoList1_photoModifyed);
            
            //photoEditToolbar.setChartControl(lPhotoList1);//设置图版和工具栏的关联
            //photoEditToolbar.saveChartTestResult = WriteToHTML;
            //photoEditToolbar.setTestAll(BeginAll);
            //ChartPhoto.InitTest(Begin);

            ci = new CameraTestIcon(this,null,"XMark","XMark测试卡");
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

        //void lPhotoList1_photoModifyed(WriteableBitmap photo, PhotoListOperation po)
        //{
        //    if (po == PhotoListOperation.Add)
        //    {
        //        ProcAdd(photo);
        //    }
        //    if (po == PhotoListOperation.Remove)
        //    {
        //        ProcRemove(photo);
        //    }
        //    if (po == PhotoListOperation.Select)
        //    {
        //        ProcSelect(photo);
        //    }

        //}

        //void ProcAdd(WriteableBitmap b)
        //{
        //    XMarkChart xt = new XMarkChart(b);
        //    tl.Add(b, xt);
        //}

        //void ProcRemove(WriteableBitmap b)
        //{
        //    if (tl.ContainsKey(b)) 
        //    {
        //        tl[b].Clear();
        //        tl.Remove(b); 
        //    }
        //    List<IParameter> r = new List<IParameter>();
            
        //    foreach (IParameter p in ParameterList)
        //    {
        //        if (p.TestChart == b)
        //        {
        //            p.TestChart = null;
        //            r.Add(p);
        //        }

        //    }
        //    foreach (IParameter p in ParameterList)
        //    {
        //        ParameterList.Remove(p);
        //    }
        //    r.Clear();
        //}

        //void ProcSelect(WriteableBitmap b)
        //{
        //    //if (b == null)
        //    //{
        //    //    ShowTestResult();
        //    //}
        //    //else
        //    //{
        //        ShowTestResult(b);
        //        ChartPhoto.setPhoto(b);
        //        chartCorrect1.Init(tl[b], ChartPhoto);//初始化管理按钮
        //    //}
        //}


        //bool TestChartNull()//判断是否有合法的照片
        //{
        //    if (ChartPhoto.getPhoto() == null)
        //    {
        //        SilverlightLFC.common.Environment.ShowMessage("请导入照片");
        //        return false;
        //    }
        //    //t.setChart(ChartPhoto.getPhoto());
        //    return true;
        //}
        CameraTestIcon ci;
        DateTime BeginTime = DateTime.Now;
        DateTime EndTime = DateTime.Now;
        //public List<IParameter> ParameterList = new List<IParameter>();
        //BackgroundWorker bw = new BackgroundWorker();
        //public event DefE ce;
        //void autoTest()
        //{
            //if (!TestChartNull()) { return; }
            //try{
                
            //    Begin();
                //EndTime = DateTime.Now;
            //}
            //catch (Exception e)
            //{
            //    SilverlightLFC.common.Environment.ShowMessage(e.Message);
            //}
        //}

        //void InitChartTestList()
        //{
        //    tl.Clear();
        //    foreach (WriteableBitmap tb in lPhotoList1.PhotoList)
        //    {
        //        XMarkChart t = new XMarkChart(tb);
        //        tl.Add(tb, t);
        //    }
        //}
        //bool IsTesting = false;
        //public void BeginAll()
        //{
        //    if (IsTesting) { return; }
        //    BeginTime = DateTime.Now;
        //    IsTesting = true;
        //    ParameterList.Clear();
        //    time.Interval = TimeSpan.FromSeconds(2);
        //    processbar.Value = 0;
        //    time.Tick += new EventHandler(time_Tick);
        //    TaskList.Clear();
        //    foreach (WriteableBitmap tb in lPhotoList1.PhotoList)
        //    {
        //        TaskList.Add(MoveToNextPhoto);
        //        TaskList.Add(PreProcess);
        //        TaskList.Add(TestAberration);
        //        TaskList.Add(TestBrightChanges);
        //        TaskList.Add(TestColorTrend);
        //        TaskList.Add(TestHEdgeDispersiveness);
        //        TaskList.Add(TestVEdgeDispersiveness);
        //        TaskList.Add(TestHEdgeResoveLines);
        //        TaskList.Add(TestVEdgeResoveLines);
        //        TaskList.Add(TestLatitude);
        //        TaskList.Add(TestNoise);
        //        TaskList.Add(TestPurplePercent);
        //        TaskList.Add(TestWaveQ);
        //        TaskList.Add(TestWhiteBanlance);
        //    }
        //    pi = -1;
        //    PhotoIndex = -1;
        //    time.Start();
        //}
        //public void Begin()
        //{
        //    if (IsTesting) { return; }
        //    BeginTime = DateTime.Now;
        //    IsTesting = true;
        //    ParameterList.Clear();
        //    processbar.Value = 0;
        //    time.Interval = TimeSpan.FromSeconds(2);
        //    time.Tick += new EventHandler(time_Tick);
        //    //PreProcess();
        //    TaskList.Clear();

        //    PhotoIndex = lPhotoList1.getPhotoIndex(ChartPhoto.getPhoto());

        //    CurrentChart = tl[ChartPhoto.getPhoto()];
        //    TaskList.Add(PreProcess);
        //    TaskList.Add(TestAberration);
        //    TaskList.Add(TestBrightChanges);
        //    TaskList.Add(TestColorTrend);
        //    TaskList.Add(TestHEdgeDispersiveness);
        //    TaskList.Add(TestVEdgeDispersiveness);
        //    TaskList.Add(TestHEdgeResoveLines);
        //    TaskList.Add(TestVEdgeResoveLines);
        //    TaskList.Add(TestLatitude);
        //    TaskList.Add(TestNoise);
        //    TaskList.Add(TestPurplePercent);
        //    TaskList.Add(TestWaveQ);
        //    TaskList.Add(TestWhiteBanlance);
        //    pi = -1;
        //    time.Start();
        //}
        //int pi = 0;
        //List<ParameterTest> TaskList = new List<ParameterTest>();
        //double stepPercent;
        //void time_Tick(object sender, EventArgs e)
        //{
        //    time.Stop();
        //    pi++;
        //    if (pi < TaskList.Count )
        //    {
        //        //pi++;
        //        TaskList[pi]();
        //        processbar.Value = processbar.Value + (100d / TaskList.Count);
        //        time.Start();
        //    }
        //    else
        //    {
        //        TestFinish();
        //        ShowTestResult();
        //        return;
        //    }
        //    //if (pi == 1) { TestAberration(); }
        //    //if (pi == 2) { TestBrightChanges(); }
        //    //if (pi == 3) { TestColorTrend(); }
        //    //if (pi == 4) { TestHEdgeDispersiveness(); }
        //    //if (pi == 5) { TestVEdgeDispersiveness(); }
        //    //if (pi == 6) { TestHEdgeResoveLines(); }
        //    //if (pi == 7) { TestVEdgeResoveLines(); }
        //    //if (pi == 8) { TestLatitude(); }
        //    //if (pi == 9) { TestNoise(); }
        //    //if (pi == 10) { TestPurplePercent(); }
        //    //if (pi == 11) { TestWaveQ(); }
        //    //if (pi == 12) { TestWhiteBanlance(); }
        //    //if (pi == 13) { TestFinish(); return; }
        //    //time.Start();
        //}
        //int PhotoIndex = -1;
        //public void MoveToNextPhoto()
        //{
        //    PhotoIndex++;
        //    CurrentChart = tl[lPhotoList1.getPhoto(PhotoIndex)];
        //}
        //void ProcessError(Exception xe, string DefaultStr)
        //{
        //    IsTesting = false;
        //    time.Stop();
        //    time.Tick -= new EventHandler(time_Tick);
        //    if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
        //    {
        //        SilverlightLFC.common.Environment.ShowMessage(xe.Message);
        //    }
        //    else
        //    {
        //        SilverlightLFC.common.Environment.ShowMessage(DefaultStr);
        //    }
        //}
        //XMarkChart CurrentChart;
        //public void PreProcess()
        //{
        //    try
        //    {
        //        //CurrentChart = tl[lPhotoList1.getPhoto(PhotoIndex)];
        //        //t.setChart();
        //        //t.PhotoTestProc += new PhotoTestProcessHandler(t_PhotoTestProc);
        //        CurrentChart.CorrectChart();
        //        CurrentChart.BeginAnalyse();
        //        //processbar.Value = 5;
        //        //time.Start();
        //    }
        //    catch (Exception xe)//未知的异常
        //    {
        //        ProcessError(xe,"测试卡矫正错误，请检查照片");

        //    }
        //}
        //DateTime _BeginTime, _EndTime;
        //LParameter getNewParameter(double r)
        //{
        //    LParameter lp = new LParameter();
        //    lp.TestTime = _EndTime;
        //    lp.SpendTime = (_EndTime - _BeginTime).TotalMilliseconds;
        //    lp.Value = r;
        //    lp.TestChart = testHelper.CurrentChart.ChartPhoto;
        //    lp.TestWay = "XMark测试卡";
        //    return lp;
        //}
        public void TestAberration()
        {
            try
            {
                BeginTime = DateTime.Now;
                //Button li = new Button();
                decimal r = (testHelper.CurrentChart as XMarkChart).getAberration() * 100;
                //ShowTestResult(buttonAberration, "畸变");
                buttonAberration.Content = "畸变：" + r.ToString() + " %";
                buttonAberration.SetValue(ToolTipService.ToolTipProperty, "数据表示照片里面几何变形程度，正表示桶形畸变，负表示枕型畸变，0为无畸变");
                //ResultPanel.Children.Add(li);
                //t.sendProcEvent(true, "");
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "畸变";
                lp.Memo = "数据表示照片里面几何变形程度，正表示桶形畸变，负表示枕型畸变，0为无畸变";
                lp.Dimension = "%";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = _EndTime;
                //lp.SpendTime = (_EndTime - _BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestWay = t.ChartPhoto;
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 10;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "测试畸变错误");
            }
        }
        public void TestBrightChanges()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = (testHelper.CurrentChart as XMarkChart).getBrightChanges() * 100;
                //Button li = new Button();
                //li.Name = "TLightingEquality";
                buttonBrightChanges.Content = "亮度一致性：" + r.ToString() + " %";
                //li.Click += new RoutedEventHandler(XMarkBrightChanges_Click);
                buttonBrightChanges.SetValue(ToolTipService.ToolTipProperty, "数据表示在照片里面中心亮度和四周亮度的差异，无差异时为0");
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "亮度一致性";
                lp.Memo = "数据表示在照片里面中心亮度和四周亮度的差异，无差异时为0";
                lp.Dimension = "%";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = _EndTime;
                //lp.SpendTime = (_EndTime - _BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestWay = t.ChartPhoto;
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 15;
                //time.Start();
                //XMarkTest.px = 30;
                //SendEvent(3);
                //t.sendProcEvent(true, "");
                //bw.ReportProgress(45, li);
                //processbar.Value = 80;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "测试亮度一致性错误");

            }
        }
        public void TestColorTrend()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = (testHelper.CurrentChart as XMarkChart).getColorDis();
                //Button li = new Button();
                //li.Name = "ColorTrendValue";
                buttonColorTrend.Content = "色彩趋向差异：" + r.ToString() + " 度";
                //li.Click += new RoutedEventHandler(XMarkColorDis_Click);
                buttonColorTrend.SetValue(ToolTipService.ToolTipProperty, "数据表示在拍摄标准颜色时候色调的偏差，完全准确时为0");
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "色彩趋向差异";
                lp.Memo = "数据表示在拍摄标准颜色时候色调的偏差，完全准确时为0";
                lp.Dimension = "度";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 20;
                //time.Start();
                //t.sendProcEvent(true, "");
                //SendEvent(4);
                //XMarkTest.px = 20;
                //bw.ReportProgress(20, li);
                //progressBar.Value = 40;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "测试色彩趋向误差测试错误");
            }
        }
        public void TestHEdgeDispersiveness()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = Convert.ToDecimal((testHelper.CurrentChart as XMarkChart).getHEdgeDispersiveness());
                //Button li = new Button();
                //li.Name = "HDispersiveness";
                buttonHDispersiveness.Content = "中央色散：" + r.ToString() + " Pxl";
                buttonHDispersiveness.SetValue(ToolTipService.ToolTipProperty, "数据表示红绿蓝三原色在边界分离的程度，也就是平均分离到几个像素，无色散时为0");
                //li.Click += new RoutedEventHandler(HDispersiveness_Click);
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "中央色散";
                lp.Memo = "利用水平边界测试，数据表示红绿蓝三原色在边界分离的程度，也就是平均分离到几个像素，无色散时为0";
                lp.Dimension = "Pix";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 25;
                //time.Start();
                //t.sendProcEvent(true, "");
                //bw.ReportProgress(40, li);
                //processbar.Value = 70;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "测试中央色散（水平条）测试错误");
            }
        }
        public void TestVEdgeDispersiveness()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = Convert.ToDecimal((testHelper.CurrentChart as XMarkChart).getVEdgeDispersiveness());
                //Button li = new Button();
                //li.Name = "VDispersiveness";
                buttonVDispersiveness.Content = "边缘色散：" + r.ToString() + " Pxl";
                buttonVDispersiveness.SetValue(ToolTipService.ToolTipProperty, "数据表示红绿蓝三原色在边界分离的程度，也就是平均分离到几个像素，无色散时为0");
                //li.Click += new RoutedEventHandler(VDispersiveness_Click);
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "边缘色散";
                lp.Memo = "利用垂直边界测试，数据表示红绿蓝三原色在边界分离的程度，也就是平均分离到几个像素，无色散时为0";
                lp.Dimension = "Pix";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 30;
                //time.Start();
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "测试边缘色散（垂直条）错误");
            }
        }
        public void TestHEdgeResoveLines()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = (testHelper.CurrentChart as XMarkChart).getHEdgeResoveLines();
                //Button li = new Button();
                //li.Name = "HResolvingPower";
                buttonHEdgeResoveLines.Content = "中央分辨率：" + r.ToString() + " lw/ph";
                //li.Click += new RoutedEventHandler(XMarkHMTF_Click);
                buttonHEdgeResoveLines.SetValue(ToolTipService.ToolTipProperty, "数据表示在一张照片里面可以分辨的水平线条的数量");
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "中央分辨率";
                lp.Memo = "利用水平边界测试，数据表示在一张照片里面可以分辨的水平线条的数量";
                lp.Dimension = "lw/ph";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 35;
                //time.Start();
                //t.sendProcEvent(true, "");
                //bw.ReportProgress(50, li);
                //processbar.Value = 90;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "利用水平边界测试中央分辨率错误");
            }
        }

        public void TestVEdgeResoveLines()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = (testHelper.CurrentChart as XMarkChart).getVEdgeResoveLines();
                //Button li = new Button();
                //li.Name = "VResolvingPower";
                buttonVEdgeResoveLines.Content = "边缘分辨率：" + r.ToString() + " lw/ph";
                //li.Click += new RoutedEventHandler(XMarkVMTF_Click);
                buttonVEdgeResoveLines.SetValue(ToolTipService.ToolTipProperty, "数据表示在一张照片里面可以分辨的水平线条的数量");
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "边缘分辨率";
                lp.Memo = "利用垂直边界测试，数据表示在一张照片里面可以分辨的水平线条的数量";
                lp.Dimension = "lw/ph";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 40;
                //time.Start();
                //t.sendProcEvent(true, "");
                //bw.ReportProgress(60, li);
                //processbar.Value = 90;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "利用垂直边界测试边缘分辨率错误");

            }
        }

        public void TestLatitude()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = Convert.ToDecimal((testHelper.CurrentChart as XMarkChart).getLatitude());
                //Button li = new Button();
                //li.Name = "DynamicLatitude";
                buttonLatitude.Content = "动态范围：" + r.ToString() + " 级";
                //li.Click += new RoutedEventHandler(XMarkLatitude_Click);
                buttonLatitude.SetValue(ToolTipService.ToolTipProperty, "数据表示在一张照片里面可以分辨的灰度等级的数量");
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡",Convert.ToDouble(r),BeginTime,EndTime);
                lp.Name = "动态范围";
                lp.Memo = "数据表示在一张照片里面可以分辨的灰度等级的数量";
                lp.Dimension = "级";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 50;
                //time.Start();
                //t.sendProcEvent(true, "");
                //bw.ReportProgress(15, li);
                //progressBar.Value = 30;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "计算动态范围错误");
            }
        }
                
        public void TestNoise()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = (testHelper.CurrentChart as XMarkChart).getNoiseNum() * 100;
                //Button li = new Button();
                //li.Name = "Noise";
                buttonNoise.Content = "噪点：" + r.ToString() + " %";
                //li.Click += new RoutedEventHandler(XMarkNoise_Click);
                buttonNoise.SetValue(ToolTipService.ToolTipProperty, "数据表示在一张照片里面可以分辨的噪点数量");
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "噪点";
                lp.Memo = "数据表示在一张照片里面可以分辨的噪点数量";
                lp.Dimension = "%";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestWay = t.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 60;
                //time.Start();
                //t.sendProcEvent(true, "");
                //bw.ReportProgress(25, li);
                //processbar.Value = 50;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "计算噪点错误");
            }
        }

        public void TestPurplePercent()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = Convert.ToDecimal((testHelper.CurrentChart as XMarkChart).getPurplePercent()) * 100;
                //Button li = new Button();
                //li.Name = "PurplePercent";
                buttonPurplePercent.Content = "紫边像素比例：" + r.ToString() + " %";
                //li.Click += new RoutedEventHandler(PurplePercent_Click);
                buttonPurplePercent.SetValue(ToolTipService.ToolTipProperty, "数据表示照片里面四角的紫色像素比例");
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "紫边像素比例";
                lp.Memo = "数据表示照片里面四角的紫色像素比例";
                lp.Dimension = "%";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestWay = t.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 70;
                //time.Start();
                //t.sendProcEvent(true, "");
                //bw.ReportProgress(100, li);
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "计算紫边像素比例错误");
            }
        }
        
        public void TestWaveQ()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = (testHelper.CurrentChart as XMarkChart).getWaveQ() * 100;
                //Button li = new Button();
                //li.Name = "WaveQ";
                buttonWaveQ.Content = "成像一致性：" + r.ToString() + " %";
                //li.Click += new RoutedEventHandler(XMarkWaveQ_Click);
                buttonWaveQ.SetValue(ToolTipService.ToolTipProperty, "数据表示照片里面还原正弦灰度区域的能力，完全还原时为0，差异越大数据越大");
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡", Convert.ToDouble(r), BeginTime, EndTime);
                lp.Name = "成像一致性";
                lp.Memo = "数据表示照片里面还原正弦灰度区域的能力，完全还原时为0，差异越大数据越大";
                lp.Dimension = "%";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestWay = t.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 80;
                //time.Start();
                //t.sendProcEvent(true, "");
                //bw.ReportProgress(100, li);
                //processbar.Value = 100;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "计算成像一致性错误");
            }
        }

        public void TestWhiteBanlance()
        {
            try
            {
                BeginTime = DateTime.Now;
                decimal r = (testHelper.CurrentChart as XMarkChart).getWhiteBanlance() * 100;
                //Button li = new Button();
                //li.Name = "AutoWhiteBalanceDistance";
                buttonWhiteBanlance.Content = "白平衡能力：" + r.ToString() + " %";
                //li.Click += new RoutedEventHandler(XMarkWhiteBalance_Click);
                buttonWhiteBanlance.SetValue(ToolTipService.ToolTipProperty, "数据表示拍摄灰度的时刻偏离灰度的程度，完全准确时为0");
                //ResultPanel.Children.Add(li);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("XMark测试卡",Convert.ToDouble(r),BeginTime,EndTime);
                lp.Name = "白平衡能力";
                lp.Memo = "数据表示拍摄灰度的时刻偏离灰度的程度，完全准确时为0";
                lp.Dimension = "%";
                //lp.TestWay = "XMark测试卡";
                //lp.TestTime = EndTime;
                //lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
                //lp.Value = Convert.ToDouble(r);
                //lp.TestWay = t.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                //processbar.Value = 95;
                //time.Start();
                //t.sendProcEvent(true, "");
                //bw.ReportProgress(30, li);
                //processbar.Value = 60;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "计算白平衡误差错误");
            }
        }

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

        //public void ShowTestResult()
        //{
        //    ShowTestResult(buttonAberration, "畸变");
        //    ShowTestResult(buttonBrightChanges, "亮度一致性");
        //    ShowTestResult(buttonColorTrend, "色彩趋向差异");
        //    ShowTestResult(buttonHDispersiveness, "中央色散");
        //    ShowTestResult(buttonVDispersiveness, "边缘色散");
        //    ShowTestResult(buttonHEdgeResoveLines, "中央分辨率");
        //    ShowTestResult(buttonVEdgeResoveLines, "边缘分辨率");
        //    ShowTestResult(buttonLatitude, "动态范围");
        //    ShowTestResult(buttonNoise, "噪点");
        //    ShowTestResult(buttonPurplePercent, "紫边像素比例");
        //    ShowTestResult(buttonWaveQ, "成像一致性");
        //    ShowTestResult(buttonWhiteBanlance, "白平衡能力");

        //}

        //public void ShowTestResult(Button b,string Name)
        //{
        //    List<IParameter> pl;
        //    pl = ParameterList.Where(v => v.Name == Name).ToList();
        //    if ((pl != null) && pl.Count > 0)
        //    {
        //        b.Content = Name + pl.Average(v=>v.Value).ToString() + pl[0].Dimension;
        //        b.SetValue(ToolTipService.ToolTipProperty, pl[0].Memo+"(平均耗时："+pl.Average(v=>v.SpendTime).ToString()+")");

        //    }
        //}
        public void ShowTestResult(WriteableBitmap chartPhoto)
        {
            AbstractTestChart ab = null;
            if ((chartPhoto != null) && testHelper.ChartTestList.ContainsKey(chartPhoto))
            {
                ab = testHelper.ChartTestList[chartPhoto];
            }
            testHelper.ShowTestResult(buttonAberration, "畸变", ab);
            testHelper.ShowTestResult(buttonBrightChanges, "亮度一致性", ab);
            testHelper.ShowTestResult(buttonColorTrend, "色彩趋向差异", ab);
            testHelper.ShowTestResult(buttonHDispersiveness, "中央色散", ab);
            testHelper.ShowTestResult(buttonVDispersiveness, "边缘色散", ab);
            testHelper.ShowTestResult(buttonHEdgeResoveLines, "中央分辨率", ab);
            testHelper.ShowTestResult(buttonVEdgeResoveLines, "边缘分辨率", ab);
            testHelper.ShowTestResult(buttonLatitude, "动态范围", ab);
            testHelper.ShowTestResult(buttonNoise, "噪点", ab);
            testHelper.ShowTestResult(buttonPurplePercent, "紫边像素比例", ab);
            testHelper.ShowTestResult(buttonWaveQ, "成像一致性", ab);
            testHelper.ShowTestResult(buttonWhiteBanlance, "白平衡能力", ab);

        }
        //public void ShowTestResult(Button b, string Name,WriteableBitmap wb)
        //{
        //    List<IParameter> pl;
        //    pl = ParameterList.Where(v => (v.Name == Name)&&(v.TestChart==wb)).ToList();
        //    if ((pl != null) && pl.Count > 0)
        //    {
        //        b.Content = Name + pl.Average(v => v.Value).ToString() + pl[0].Dimension;
        //        b.SetValue(ToolTipService.ToolTipProperty, pl[0].Memo + "(平均耗时：" + pl.Average(v => v.SpendTime).ToString() + ")");
        //        b.Tag = wb;
        //    }
        //}

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

        //}

        void HDispersiveness_Click(object sender, RoutedEventArgs e)
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
        void VDispersiveness_Click(object sender, RoutedEventArgs e)
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
        void XMarkNoise_Click(object sender, RoutedEventArgs e)
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
            if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaBW_1"))
            {
                for (int i = 1; i < 23; i++)
                {
                    wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaBW_" + i.ToString()]);
                }
                
            }
            pc.Test(wbl);
        }
        void XMarkLatitude_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            Latitude pc = new Latitude();
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }
            List<WriteableBitmap> wbl = new List<WriteableBitmap>();
            if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaBW_1"))
            {
                for (int i = 1; i < 23; i++)
                {
                    wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaBW_" + i.ToString()]);
                }
            }
            pc.Test(wbl);
        }
        void XMarkWhiteBalance_Click(object sender, RoutedEventArgs e)
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
            if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaBW_1"))
            {
                for (int i = 1; i < 23; i++)
                {
                    wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaBW_" + i.ToString()]);
                }
            }
            pc.Test(wbl);
        }
        void XMarkColorDis_Click(object sender, RoutedEventArgs e)
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
            if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaColor_7"))
            {
                for (int i = 7; i < 19; i++)
                {
                    wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaColor_" + i.ToString()]);
                    sl.Add((testHelper.CurrentChart as XMarkChart).ColorList[i - 1]);
                }
            }
            pc.setStandardColorList(sl);
            pc.Test(wbl);
        }
        void XMarkWaveQ_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            Wave pc = new Wave();
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }
            List<WriteableBitmap> wbl = new List<WriteableBitmap>();
            if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaWave_1"))
            {
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaWave_1"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaWave_2"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaWave_3"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaWave_4"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaWave_5"]);
            }
            pc.Test(wbl);
        }
        void PurplePercent_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            PurplePercent pc = new PurplePercent();
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }
            List<WriteableBitmap> wbl = new List<WriteableBitmap>();
            if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaPurple_1"))
            {
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaPurple_1"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaPurple_2"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaPurple_3"]);
                wbl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaPurple_4"]);
            }
            pc.Test(wbl);
        }
        void XMarkHMTF_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            SFRResolution pc = new SFRResolution();
            pc.IsV = true;
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }
            if (ChartPhoto.IsSelect)
            {
                pc.Test(ChartPhoto.getSelectArea());
            }
            else
            {
                if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaVEdge"))
                {
                    pc.Test(testHelper.CurrentChart.mp.SelectedArea["AreaVEdge"]);
                }
            }
        }
        void XMarkVMTF_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            SFRResolution pc = new SFRResolution();
            pc.IsV = false;
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }
            if (ChartPhoto.IsSelect)
            {
                pc.Test(ChartPhoto.getSelectArea());
            }
            else
            {
                if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaHEdge"))
                {
                    pc.Test(testHelper.CurrentChart.mp.SelectedArea["AreaHEdge"]);
                }
            }
        }
        void XMarkBrightChanges_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            BrightDistance pc = new BrightDistance();
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }

            List<WriteableBitmap> bl = new List<WriteableBitmap>();

            bl.Add((testHelper.CurrentChart as XMarkChart).CropPhoto);
            //t.getCurveBrightnessChange();
            if (testHelper.CurrentChart.mp.SelectedArea.ContainsKey("AreaHBright"))
            {
                bl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaHBright"]);
                bl.Add(testHelper.CurrentChart.mp.SelectedArea["AreaVBright"]);
            }
            pc.Test(bl);
        }
        void XMarkAberration_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            Aberration pc = new Aberration();
            CameraTestDesktop.getDesktopPanel().Children.Add(pc);
            if (testHelper.CurrentChart == null)
            {
                SilverlightLFC.common.Environment.ShowMessage("没有选择测试照片做进一步测试");
                return;
            }
            if (ChartPhoto.IsSelect)
            {
                pc.Test(ChartPhoto.getSelectArea());
            }
            else
            {
                if (testHelper.CurrentChart.getCorrectPhoto() != null)
                {
                    pc.Test(testHelper.CurrentChart.getCorrectPhoto());
                }
            }
        }

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

        //private void buttonTest_Click(object sender, RoutedEventArgs e)
        //{
        //    Begin();
        //}

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
    }
}
