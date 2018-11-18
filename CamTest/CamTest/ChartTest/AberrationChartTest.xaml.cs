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
using SLPhotoTest.UIControl;
using SLPhotoTest.PhotoInfor;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace SLPhotoTest.ChartTest
{
    public partial class AberrationChartTest : UserControl, IChartTestWindow
    {
        public PhotoEditToolbar PEToolbar { get { return photoEditToolbar; } }
        public PhotoInforToolbar PIToolbar { get { return photoInforTool1; } }
        public LPhotoList PhotoList { get { return lPhotoList1; } }
        public LChartPhoto ChartPhoto { get { return lChartPhoto1; } }
        public ProgressBar TestProcessbar { get { return processbar; } }
        public ChartCorrect ChartCorrectToolbar { get { return null; } }
        public AbstractTestChart getChartTest(WriteableBitmap b)
        {
            return new AberrationChart(b);
        }
        public List<ParameterTest> GetTestSteps()
        {
            List<ParameterTest> pl = new List<ParameterTest>();
            pl.Add(TestAberration);
            pl.Add(TestBlackLineNum);
            pl.Add(TestBlackLinePix);
            pl.Add(TestTopBlackLineNum);
            pl.Add(TestTopBlackLinePix);
            pl.Add(TestBottomBlackLineNum);
            pl.Add(TestBottomBlackLinePix);
            return pl;
        }
        ChartTestHelper testHelper;
        public AberrationChartTest()
        {
            InitializeComponent();
            testHelper = new ChartTestHelper(this);
            //photoInforTool1.setTarget(ChartPhoto);
            //photoEditToolbar.setChartControl(lMultiPhoto1);//设置图版和工具栏的关联
            //lPhotoList1.TargetControl = ChartPhoto;
            //lPhotoList1.photoOperated += new UIControl.PhotoOperation(lPhotoList1_photoModifyed);
            //photoEditToolbar.setTestAll(BeginAll);
            //photoEditToolbar.setChartControl(lPhotoList1);//设置图版和工具栏的关联

            //photoEditToolbar.saveChartTestResult = WriteToHTML;
            //ChartPhoto.InitTest(autoTest);
            //ChartPhoto.InitSaveTest(WriteToHTML);
            am = new ActionMove(this, Title);
            ci = new CameraTestIcon(this,null,"Aberration","畸变测试");
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

        CameraTestIcon ci;
        ActionMove am;
        DateTime BeginTime;//测试起始时间
        DateTime EndTime;//测试完成时间
        //public WriteableBitmap SourcePhoto;
        //AberrationChart t = new AberrationChart();
        //bool TestChartNull()//判断是否有合法的照片
        //{
        //    if (ChartPhoto.getPhoto() == null)//表示没有照片被显示
        //    {
        //        SilverlightLFC.common.Environment.ShowMessage("请导入照片");
        //        return false;
        //    }
        //    t.setChart(ChartPhoto.getPhoto());
        //    return true;
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

        void TestAberration()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as AberrationChart).getAberration();
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("畸变测试卡",r,BeginTime,EndTime);
                lp.Name = "畸变";
                lp.Memo = "查看照片两端垂直黑色线条的弯曲程度";
                lp.Dimension = "";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                testHelper.ShowTestResult(TextAberration, "畸变", null);
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "畸变计算错误");
            }
        }

        void TestBlackLineNum()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as AberrationChart).getCenterBlackLineNum();
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("畸变测试卡", r, BeginTime, EndTime);
                lp.Name = "畸变卡中央线条数";
                lp.Memo = "查看照片两端垂直中央区域的线条数";
                lp.Dimension = "";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                testHelper.ShowTestResult(textBlockCenterBlackLineNum, "畸变卡中央线条数", null);
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "畸变计算错误");
            }
        }

        void TestBlackLinePix()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as AberrationChart).getCenterBlackLinePix();
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("畸变测试卡", r, BeginTime, EndTime);
                lp.Name = "畸变卡中央线条最大距离";
                lp.Memo = "查看照片两端垂直中央区域的线条之间最大距离";
                lp.Dimension = "";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                testHelper.ShowTestResult(textBlockCenterBlackLinePix, "畸变卡中央线条最大距离", null);
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "畸变计算错误");
            }
        }

        void TestTopBlackLineNum()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as AberrationChart).getTopBlackLineNum();
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("畸变测试卡", r, BeginTime, EndTime);
                lp.Name = "畸变卡顶部线条数";
                lp.Memo = "查看照片两端垂直顶部区域的线条数";
                lp.Dimension = "";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                testHelper.ShowTestResult(textBlockTopBlackLineNum, "畸变卡顶部线条数", null);
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "畸变计算错误");
            }
        }

        void TestTopBlackLinePix()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as AberrationChart).getTopBlackLinePix();
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("畸变测试卡", r, BeginTime, EndTime);
                lp.Name = "畸变卡顶部线条最大距离";
                lp.Memo = "查看照片两端垂直顶部区域的线条之间最大距离";
                lp.Dimension = "";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                testHelper.ShowTestResult(textBlockTopBlackLinePix, "畸变卡顶部线条最大距离", null);
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "畸变计算错误");
            }
        }

        void TestBottomBlackLineNum()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as AberrationChart).getBottomBlackLineNum();
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("畸变测试卡", r, BeginTime, EndTime);
                lp.Name = "畸变卡底部线条数";
                lp.Memo = "查看照片两端垂直底部区域的线条数";
                lp.Dimension = "";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                testHelper.ShowTestResult(textBlockBottomBlackLineNum, "畸变卡底部线条数", null);
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "畸变计算错误");
            }
        }

        void TestBottomBlackLinePix()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as AberrationChart).getBottomBlackLinePix();
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("畸变测试卡", r, BeginTime, EndTime);
                lp.Name = "畸变卡底部线条最大距离";
                lp.Memo = "查看照片两端垂直底部区域的线条之间最大距离";
                lp.Dimension = "";

                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                testHelper.ShowTestResult(textBlockBottomBlackLinePix, "畸变卡底部线条最大距离", null);
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "畸变计算错误");
            }
        }

        private void buttonInterActiveTest_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            if (!testHelper.TestChartNull()) { return; }
            ClearInterActive();
            ChartPhoto.DisableMove();
            buttonInterActiveTest.Foreground= new SolidColorBrush(Colors.Red);
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            c.PointerMoved += new PointerEventHandler(c_PointerMoved);
            c.PointerPressed += new PointerEventHandler(c_PointerPressed);
        }

        void c_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            buttonInterActiveTest.Foreground = new SolidColorBrush(Colors.Blue);
            c.PointerMoved -= new PointerEventHandler(c_PointerMoved);
            c.PointerPressed -= new PointerEventHandler(c_PointerPressed);
            if (!testHelper.TestChartNull()) { return; };
            int ph =Convert.ToInt32( e.GetPosition(ChartPhoto).Y / ChartPhoto.Height * ChartPhoto.getPhoto().PixelHeight);
            int n = (testHelper.CurrentChart as AberrationChart).getCenterBlackLineNum();
            textBlackLength.Text = (testHelper.CurrentChart as AberrationChart).getLineLength(ph, 0, n).ToString();
        }
        Line SelectLine = new Line();
        void c_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            if (!c.Children.Contains(SelectLine))
            {
                c.Children.Add(SelectLine); 
                SelectLine.Stroke = new SolidColorBrush(Colors.Red);
                SelectLine.StrokeThickness = 3;
            }

            SelectLine.X1 = 0;
            SelectLine.X2 = c.Width;
            SelectLine.Y1 = SelectLine.Y2 = e.GetCurrentPoint(c).Position.Y;
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonToIcon_Click(object sender, RoutedEventArgs e)
        {
            ci.ToIcon();
        }

        void ClearInterActive()
        {
            SolidColorBrush fb = buttonInterActiveTest.Foreground as SolidColorBrush;
            if (fb.Color == Colors.Red)
            {
                buttonInterActiveTest.Foreground = new SolidColorBrush(Colors.Blue);
                Canvas c = ChartPhoto.getDrawObjectCanvas();
                c.PointerMoved -= new PointerEventHandler(c_PointerMoved);
                c.PointerPressed -= new MouseButtonEventHandler(c_PointerPressed);
            }
            fb = buttonLS.Foreground as SolidColorBrush;
            if (fb.Color == Colors.Red)
            {
                buttonLS.Foreground = new SolidColorBrush(Colors.Blue);
                Canvas c = ChartPhoto.getDrawObjectCanvas();
                c.PointerMoved -= new PointerEventHandler(c_PointerMoved);
                c.PointerPressed -= new MouseButtonEventHandler(c_PointerPressed);
            }
            fb = buttonRS.Foreground as SolidColorBrush;
            if (fb.Color == Colors.Red)
            {
                buttonRS.Foreground = new SolidColorBrush(Colors.Blue);
                Canvas c = ChartPhoto.getDrawObjectCanvas();
                c.PointerMoved -= new PointerEventHandler(c_PointerMoved);
                c.PointerPressed -= new MouseButtonEventHandler(c_PointerPressed);
            }

        }

        Line LL, RL;

        void Clear()//删除各种辅助线
        {
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            if (c.Children.Contains(SelectLine)) { c.Children.Remove(SelectLine); }
            if (c.Children.Contains(LL)) { c.Children.Remove(LL); }
            if (c.Children.Contains(RL)) { c.Children.Remove(RL); }
        }

        private void buttonLS_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            if (!testHelper.TestChartNull()) { return; }
            ClearInterActive();
            ChartPhoto.DisableMove();
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            
            c.PointerMoved+=new PointerEventHandler(LL_PointerMoved);
            c.PointerPressed += new PointerEventHandler(LL_PointerPressed);
        }

        private void buttonRS_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            if (!testHelper.TestChartNull()) { return; }
            ClearInterActive();
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            ChartPhoto.DisableMove();
            c.PointerMoved += new PointerEventHandler(RL_PointerMoved);
            c.PointerPressed += new MouseButtonEventHandler(RL_PointerPressed);
        }
        int ll=0, rl=0;
        void LL_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            //if (c.Children.Contains(LL)) { c.Children.Remove(LL); }
            buttonLS.Foreground = new SolidColorBrush(Colors.Blue);
            c.PointerMoved -= new PointerEventHandler(LL_PointerMoved);
            c.PointerPressed -= new MouseButtonEventHandler(LL_PointerPressed);

            ll = Convert.ToInt32(e.GetPosition(c).X / c.Width * ChartPhoto.getPhoto().PixelWidth);
            ToolTipService.SetToolTip(buttonLS, ll.ToString());

        }

        void LL_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (LL == null)
            {
                LL = new Line();
                LL.Stroke = new SolidColorBrush(Colors.Green);
                LL.StrokeThickness = 2;
            }
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            if (!c.Children.Contains(LL))
            {
                c.Children.Add(LL);
                LL.Stroke = new SolidColorBrush(Colors.Green);
                LL.StrokeThickness = 2;
            }

            LL.Y1 = 0;
            LL.Y2 = c.Height;
            LL.X1 = LL.X2 = e.GetPosition(c).X;
        }

        void RL_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            //if (c.Children.Contains(RL)) { c.Children.Remove(RL); }
            buttonRS.Foreground = new SolidColorBrush(Colors.Blue);
            c.PointerMoved -= new PointerEventHandler(RL_PointerMoved);
            c.PointerPressed -= new PointerEventHandler(RL_PointerPressed);

            rl = Convert.ToInt32(e.GetPosition(c).X / c.Width * ChartPhoto.getPhoto().PixelWidth);
            ToolTipService.SetToolTip(buttonRS, rl.ToString());

        }

        void RL_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (RL == null)
            {
                RL = new Line();
                RL.Stroke = new SolidColorBrush(Colors.Green);
                RL.StrokeThickness = 2;
            }
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            if (!c.Children.Contains(RL))
            {
                c.Children.Add(RL);
                RL.Stroke = new SolidColorBrush(Colors.Green);
                RL.StrokeThickness = 2;
            }

            RL.Y1 = 0;
            RL.Y2 = c.Height;
            RL.X1 = RL.X2 = e.GetPosition(c).X;
        }

        private void buttonGetLength_Click(object sender, RoutedEventArgs e)
        {
            textBlockL.Text = (rl - ll).ToString();
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            buttonInterActiveTest.Foreground = new SolidColorBrush(Colors.Blue);
            buttonRS.Foreground = new SolidColorBrush(Colors.Blue);
            buttonLS.Foreground = new SolidColorBrush(Colors.Blue);
        }

        private void buttonParameterSave_Click(object sender, RoutedEventArgs e)
        {
            //LParameter lp= getTestParameter();
            CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            foreach (LParameter lp in testHelper.ParameterList)
            {
                cd.currentProject.AddResult(lp);
            }
        }

        //LParameter getTestParameter()
        //{
            
        //    LParameter lp = new LParameter();
        //    lp.Name = "畸变";
        //    lp.Memo = "通过拍摄畸变测试卡测试成像的畸变程度";
        //    lp.Dimension = "%";
        //    lp.TestWay = "畸变测试卡";
        //    lp.TestTime = EndTime;
        //    lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
        //    //lp.Value = Convert.ToDouble(TextAberration.Text);
        //    lp.Value = ParameterList.Average(v => v.Value);
        //    return lp;
        //}

        //public void WriteToHTML()
        //{
        //    LParameter lp = getTestParameter();

        //    string s = "";
        //    s = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN/\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\">";
        //    s = s + "<head><meta http-equiv=\"Content-Language\" content=\"zh-cn\" /><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />";
        //    s = s + "<title>shiyan</title><style type=\"text/css\">.style1 {text-align: left;}.style2 {margin-left: 40px;}.style3 {	text-decoration: underline;	margin-left: 40px;}</style></head>";
        //    s = s + "<body>";

        //    s = s + "<h2>测试结果信息：</h2>";
        //    //foreach (IParameter p in ResultList)
        //    //{
        //        s = s + "指标名称：" + lp.Name;
        //        s = s + "<p class=\"style2\">说明：" + lp.Memo + "</p>";
        //        s = s + "<p>测试时间：" + lp.TestTime.ToString() + "</p>";
        //        if (lp.Value != Double.NaN)
        //        {
        //            s = s + "<p>测试值：" + lp.Value.ToString() + lp.Dimension + "</p>";
        //        }
        //    //}

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
        //public List<IParameter> ParameterList = new List<IParameter>();
        private void buttonClearResult_Click(object sender, RoutedEventArgs e)
        {
            testHelper.ParameterList.Clear();
        }
        //bool IsShowList = false;
        private void imageMulti_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lPhotoList1.Visibility = Visibility.Visible;
            e.Handled = true;
            //IsShowList = !IsShowList;
            //if (IsShowList)
            //{
            //    lPhotoList1.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    lPhotoList1.Visibility = Visibility.Collapsed;
            //}
        }


        public void ShowTestResult(WriteableBitmap chartPhoto)
        {
            AbstractTestChart ab = null;
            if ((chartPhoto != null) && testHelper.ChartTestList.ContainsKey(chartPhoto))
            {
                ab = testHelper.ChartTestList[chartPhoto];
            }
            testHelper.ShowTestResult(TextAberration, "畸变", ab);
            testHelper.ShowTestResult(textBlockCenterBlackLineNum, "畸变卡中央线条数", ab);
            testHelper.ShowTestResult(textBlockCenterBlackLinePix, "畸变卡中央线条最大距离", ab);
            testHelper.ShowTestResult(textBlockTopBlackLineNum, "畸变卡顶部线条数", ab);
            testHelper.ShowTestResult(textBlockTopBlackLinePix, "畸变卡顶部线条最大距离", ab);
            testHelper.ShowTestResult(textBlockBottomBlackLineNum, "畸变卡底部线条数", ab);
            testHelper.ShowTestResult(textBlockBottomBlackLinePix, "畸变卡底部线条最大距离", ab);
        }

        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lPhotoList1.Visibility = Visibility.Collapsed;
        }

        private void buttonJB_Click(object sender, RoutedEventArgs e)
        {
            JBAberrationTest w = new JBAberrationTest();
            if(lChartPhoto1.getPhoto()!=null)
                w.lPhotoList1.AddPhoto(lChartPhoto1.getPhoto());
            //w.lChartPhoto1.setPhoto(lChartPhoto1.getPhoto());
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            ci.ToIcon();
            //Canvas Desk = CameraTestDesktop.getDesktopPanel();
            //if (Desk.Children.Contains(this))
            //{
            //    Desk.Children.Remove(this);
            //}
        }
    }
}
