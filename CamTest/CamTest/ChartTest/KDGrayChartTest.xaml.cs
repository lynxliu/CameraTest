using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using SilverlightLynxControls;
using SilverlightDCTestLibrary;
using DCTestLibrary;
using System.IO;
using SilverlightLFC.common;
using SLPhotoTest.PhotoInfor;
using SLPhotoTest.UIControl;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;

namespace SLPhotoTest.ChartTest
{
    public partial class KDGrayChartTest : UserControl, IChartTestWindow
    {
        public PhotoEditToolbar PEToolbar { get { return photoEditToolbar; } }
        public PhotoInforToolbar PIToolbar { get { return photoInforTool1; } }
        public LPhotoList PhotoList { get { return lPhotoList1; } }
        public LChartPhoto ChartPhoto { get { return lChartPhoto; } }
        public ProgressBar TestProcessbar { get { return processbar; } }
        public ChartCorrect ChartCorrectToolbar { get { return null; } }
        public AbstractTestChart getChartTest(WriteableBitmap b)
        {
            return new KDGrayChart(b);
        }
        public List<ParameterTest> GetTestSteps()
        {
            List<ParameterTest> pl = new List<ParameterTest>();
            pl.Add(TestLatitude);
            pl.Add(DrawLatitude);
            return pl;
        }
        ChartTestHelper testHelper;
        public KDGrayChartTest()
        {
            InitializeComponent();
            testHelper = new ChartTestHelper(this);
            //photoInforTool1.setTarget(ChartPhoto);
            am = new ActionMove(this, Title);

            //photoEditToolbar.setChartControl(ChartPhoto);//设置图版和工具栏的关联
            //photoEditToolbar.saveChartTestResult = WriteToHTML;
            //ChartPhoto.InitTest(autoTest);
            //ChartPhoto.InitSaveTest(WriteToHTML);
            ci = new CameraTestIcon(this,null,"KDGrayChart","柯达灰度卡");
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
        public void TestLatitude()
        {
            if (!testHelper.TestChartNull()) { return; }
            //processbar.Value = 0;
            BeginTime = DateTime.Now;
            try
            {
                //t.setChart(ChartPhoto.getPhoto());
                int h = lynxUpDown1.IntValue;
                if (h == 0) { h = 7; lynxUpDown1.LongValue = 7; }

                double v = (testHelper.CurrentChart as KDGrayChart).getLatitude(h);
                EndTime = DateTime.Now;
                textBlockLatitude.Text = v.ToString();
                LParameter lp = testHelper.getNewParameter("柯达灰阶",v,BeginTime,EndTime);
                lp.Name = "宽容度";
                lp.Memo = "数据表示在一张照片里面可以分辨的亮度级别";
                lp.Dimension = "级";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);

                //List<double> dl = (testHelper.CurrentChart as KDGrayChart).getHLine(ChartPhoto.getPhoto().PixelHeight/2);
                //DrawGraphic dg = new DrawGraphic(canvasLatitude);
                //dg.DrawBrightHistogram(dl);
            }
            catch (Exception xe)
            {
                testHelper.ProcessError(xe, "宽容度计算错误,请检查照片");
            }
            EndTime = DateTime.Now;
            //processbar.Value = 100;
        }
        void DrawLatitude()
        {
            List<double> dl = (testHelper.CurrentChart as KDGrayChart).getHLine(ChartPhoto.getPhoto().PixelHeight / 2);
            DrawGraphic dg = new DrawGraphic(canvasLatitude);
            dg.DrawBrightHistogram(dl);
        }

        //KDGrayChart t = new KDGrayChart();
        //public WriteableBitmap SourcePhoto;
        ActionMove am;

        void CloseWindow()
        {
            testHelper.Clear();
            ChartPhoto.Clear();
            //image.Source = null;
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

        private void buttonInteractiveTest_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            if (!testHelper.TestChartNull()) { return; }
            buttonInteractiveTest.Foreground = new SolidColorBrush(Colors.Red);
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            ChartPhoto.DisableMove();
            c.PointerPressed += (c_PointerPressed);
            //c.PointerReleased += new MouseButtonEventHandler(c_PointerReleased);
            c.PointerMoved += new PointerEventHandler(c_PointerMoved);
        }

        Line tl = new Line();
        void c_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            tl.Stroke = new SolidColorBrush(Colors.Red);
            tl.StrokeThickness = 3;
            if (!ChartPhoto.getDrawObjectCanvas().Children.Contains(tl))
            {
                ChartPhoto.getDrawObjectCanvas().Children.Add(tl);
            }
            tl.X1 = 0;
            tl.X2 = ChartPhoto.Width;
            tl.Y1 = tl.Y2 = e.GetCurrentPoint(ChartPhoto).Position.Y;

        }

        void c_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Canvas c = sender as Canvas;
            try
            {
                canvasLatitude.Children.Clear();
                double h = e.GetCurrentPoint(ChartPhoto.getDrawObjectCanvas()).Position.Y / ChartPhoto.getImage().Height * ChartPhoto.getPhoto().PixelHeight;
                textBlockHeight.Text = Convert.ToInt32(h).ToString();
                textBlockInteractively.Text = (testHelper.CurrentChart as KDGrayChart).getLatitude(Convert.ToInt32(h), lynxUpDown1.IntValue).ToString();

                List<double> dl = (testHelper.CurrentChart as KDGrayChart).getHLine(Convert.ToInt32(h));
                DrawGraphic dg = new DrawGraphic(canvasLatitude);
                dg.DrawBrightHistogram(dl);
                buttonInteractiveTest.Foreground = new SolidColorBrush(Colors.Blue);
                if (c.Children.Contains(tl)) { c.Children.Remove(tl); }
            }
            catch (Exception xe)
            {
                if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
                {
                    SilverlightLFC.common.Environment.ShowMessage(xe.Message);
                }
                else
                {
                    SilverlightLFC.common.Environment.ShowMessage("测试错误,请检查照片");
                }
            }
            finally
            {
                c.PointerPressed -= (c_PointerPressed);
                c.PointerMoved -= (c_PointerMoved);
            }
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
            //LParameter lp = getTestParameter();
            CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            foreach (LParameter lp in testHelper.ParameterList)
            {
                cd.currentProject.AddResult(lp);
            }
        }

        //LParameter getTestParameter()
        //{
        //    LParameter lp = new LParameter();
        //    lp.Name = "动态范围（宽容度）";
        //    lp.Memo = "通过拍摄均匀光照下的灰阶卡测试动态范围";
        //    lp.Dimension = "级";
        //    lp.TestWay = "柯达灰阶卡";
        //    lp.TestTime = EndTime;
        //    lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
        //    lp.Value = Convert.ToDouble(textBlockLatitude.Text);
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
        //    s = s + "指标名称：" + lp.Name;
        //    s = s + "<p class=\"style2\">说明：" + lp.Memo + "</p>";
        //    s = s + "<p>测试时间：" + lp.TestTime.ToString() + "</p>";
        //    if (lp.Value != Double.NaN)
        //    {
        //        s = s + "<p>测试值：" + lp.Value.ToString() + lp.Dimension + "</p>";
        //    }
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
            testHelper.ShowTestResult(textBlockLatitude, "宽容度", ab);

        }
    }
}
