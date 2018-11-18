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
using SLPhotoTest.PhotoInfor;
using SLPhotoTest.UIControl;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.Foundation;

namespace SLPhotoTest.ChartTest
{
    public partial class GrayChartTest : UserControl, IChartTestWindow
    {
        public PhotoEditToolbar PEToolbar { get { return photoEditToolbar; } }
        public PhotoInforToolbar PIToolbar { get { return photoInforTool1; } }
        public LPhotoList PhotoList { get { return lPhotoList1; } }
        public LChartPhoto ChartPhoto { get { return lChartPhoto; } }
        public ProgressBar TestProcessbar { get { return processbar; } }
        public ChartCorrect ChartCorrectToolbar { get { return null; } }
        public AbstractTestChart getChartTest(WriteableBitmap b)
        {
            return new GrayChart(b);
        }
        public List<ParameterTest> GetTestSteps()
        {
            List<ParameterTest> pl = new List<ParameterTest>();
            pl.Add(TestGray);

            return pl;
        }
        ChartTestHelper testHelper;
        public GrayChartTest()
        {
            InitializeComponent();
            testHelper = new ChartTestHelper(this);
            //photoInforTool1.setTarget(ChartPhoto);
            //photoEditToolbar.setChartControl(ChartPhoto);//设置图版和工具栏的关联
            am = new ActionMove(this, Title);
            //ChartPhoto.InitTest(autoTest);
            //photoEditToolbar.saveChartTestResult=WriteToHTML;
            //photoEditToolbar.PhotoOperate += new PhotoTest.PhotoTestOperation(photoEditToolbar_PhotoOperate);
            ChartPhoto.PhotoChanged += new CurrentPhotoChanged(ChartPhoto_PhotoChanged);

            ci = new CameraTestIcon(this,null,"GrayChart","灰度卡亮度一致性测试");
            ci.release = CloseWindow;
            CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            if (!cd.TaskList.Children.Contains(ci))
            {
                cd.TaskList.Children.Add(ci);
            }
            ci.Active();
        }
        bool IsNeedSave = true;
        void ChartPhoto_PhotoChanged(WriteableBitmap current)
        {
            if(IsNeedSave)//自己处理的结果不需要保存
                TempPhoto = WriteableBitmapHelper.Clone(ChartPhoto.getPhoto());//备份原始图像
            //throw new NotImplementedException();
            IsNeedSave = true;//默认都是要存的
        }

        //void photoEditToolbar_PhotoOperate(string OperationName, WriteableBitmap b)
        //{
        //    if (OperationName == "Open")
        //    {
        //        TempPhoto = new WriteableBitmap(ChartPhoto.getPhoto());//备份原始图像
        //    }
        //}
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

        DateTime BeginTime=DateTime.Now;
        DateTime EndTime = DateTime.Now;
        private void buttonInterActiveTest_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            if (!testHelper.TestChartNull()) { return; }
            ChartPhoto.DisableMove();
            buttonInterActiveTest.Foreground = new SolidColorBrush(Colors.Red);
            Canvas c = ChartPhoto.getDrawObjectCanvas();

            c.PointerMoved += c_PointerMoved;
            c.PointerPressed += c_PointerPressed;
        }
        WriteableBitmap TempPhoto = null;
        void c_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!testHelper.TestChartNull()) { return; }
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            if (c.Children.Contains(hl)) { c.Children.Remove(hl); }
            if (c.Children.Contains(vl)) { c.Children.Remove(vl); }
            buttonInterActiveTest.Foreground = new SolidColorBrush(Colors.Blue);
            c.PointerMoved -= new PointerEventHandler(c_PointerMoved);
            c.PointerPressed -= c_PointerPressed;

            DrawGraphic dg=new DrawGraphic();
            Point tp=e.GetCurrentPoint(ChartPhoto).Position;
            Point ip=DrawGraphic.getImagePosition(tp,ChartPhoto.getImage());

            Color cc=testHelper.CurrentChart.GetPixel(ChartPhoto.getPhoto(),(int)ip.X,(int)ip.Y);
            //TempPhoto =new WriteableBitmap( ChartPhoto.getPhoto());//复制一份保留
            try
            {
                WriteableBitmap oi;
                int n = testHelper.CurrentChart.getFloodBrightEdge(ChartPhoto.getPhoto(),out oi, ip, lynxUpDown2.IntValue);
                IsNeedSave = false;
                ChartPhoto.setPhoto(oi);
                textPixNum.Text = n.ToString();
            }
            catch (Exception xe)//未知的异常
            {
                if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
                {
                    SilverlightLFC.common.Environment.ShowMessage(xe.Message);
                }
                else
                {
                    SilverlightLFC.common.Environment.ShowMessage("计算亮度变化错误");
                }
            }
            
        }
        Line hl = new Line();
        Line vl = new Line();
        void c_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            if (!c.Children.Contains(hl))
            {
                c.Children.Add(hl); 
                hl.Stroke = new SolidColorBrush(Colors.Red);
                hl.StrokeThickness = 2;
            }

            hl.X1 = 0;
            hl.X2 = ChartPhoto.Width;
            hl.Y1 = hl.Y2 = e.GetCurrentPoint(ChartPhoto).Position.Y;
            if (!c.Children.Contains(vl))
            {
                c.Children.Add(vl); 
                vl.Stroke = new SolidColorBrush(Colors.Red);
                vl.StrokeThickness = 2;
            }

            vl.Y1 = 0;
            vl.Y2 = ChartPhoto.Height;
            vl.X1 = vl.X2 = e.GetCurrentPoint(ChartPhoto).Position.X;
        }
        ActionMove am;
        //GrayChart t = new GrayChart();

        void CloseWindow()
        {
            try
            {
                //t.Clear();
                testHelper.Clear();
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
            catch (Exception xe)
            {
                SilverlightLFC.common.Environment.ShowMessage(xe.Message);
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        public void TestGray()
        {
            if (!testHelper.TestChartNull()) { return; }
            try{
                //progressbar.Value = 0;
                BeginTime = DateTime.Now;
                //t.setChart(ChartPhoto.getPhoto());
                double d = (testHelper.CurrentChart as GrayChart).getBrightChangesValue();
                //TextValue.Text = d.ToString();
                EndTime = DateTime.Now;

                LParameter lp = testHelper.getNewParameter("灰度测试卡", d, BeginTime, EndTime);
                lp.Name = "亮度变化";
                lp.Memo = "查看照片亮度变化";
                lp.Dimension = "";
                //lp.TestChart = testHelper.CurrentChart.ChartPhoto;
                testHelper.ParameterList.Add(lp);
                testHelper.ShowTestResult(TextValue, "亮度变化", null);
                TestGB();

                //progressbar.Value = 100;
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "灰度计算错误");
            }
        }

        private void buttonBrightCurve_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Standard))
            {
                return;
            }
            if (!testHelper.TestChartNull()) { return; }

            WriteableBitmap b= (testHelper.CurrentChart as GrayChart).getBrightChangesImage(lynxUpDown1.IntValue);
            IsNeedSave = false;
            ChartPhoto.setPhoto(b);
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
            foreach (IParameter lp in testHelper.ParameterList)
            {
                cd.currentProject.AddResult(lp);
            }
        }
        //LParameter getTestParameter()
        //{
        //    LParameter lp = new LParameter();
        //    lp.Name = "亮度一致性";
        //    lp.Memo = "通过拍摄均匀光照下的灰度卡测试成像的亮度一致性";
        //    lp.Dimension = "%";
        //    lp.TestWay = "中调灰卡";
        //    lp.TestTime = EndTime;
        //    lp.SpendTime = (EndTime - BeginTime).TotalMilliseconds;
        //    lp.Value = Convert.ToDouble(TextValue.Text);
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
        //    //fs.Close();
        //}

        private void buttonResumePhoto_Click(object sender, RoutedEventArgs e)
        {
            if (TempPhoto != null)
            {
                ChartPhoto.setPhoto(WriteableBitmapHelper.Clone(TempPhoto));
                //testHelper.CurrentChart = new WriteableBitmap(TempPhoto);
                //TempPhoto = null;
            }
        }

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

        void TestGB()
        {
            double x = (testHelper.CurrentChart as GrayChart).ptp.getGBBrightChangedValue(ChartPhoto.getPhoto(), lynxUpDownAreaNum.IntValue);
            textBlockGBValue.Text = x.ToString();


            if (checkBoxFocus.IsChecked.Value)
            {
                if (x > 0.7) { ChartTestHelper.setGBSign(true, gridGB); }
                else ChartTestHelper.setGBSign(false, gridGB); 

            }
            else
            {
                if (x > 0.5) ChartTestHelper.setGBSign(true, gridGB);
                else ChartTestHelper.setGBSign(false, gridGB);
            }
        }

        public void ShowTestResult(WriteableBitmap chartPhoto)
        {
            AbstractTestChart ab = null;
            if ((chartPhoto != null) && testHelper.ChartTestList.ContainsKey(chartPhoto))
            {
                ab = testHelper.ChartTestList[chartPhoto];
            }

            testHelper.ShowTestResult(TextValue, "亮度变化", ab);

        }

        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lPhotoList1.Visibility = Visibility.Collapsed;
        }

        bool IsShowArea = false;
        private void buttonJBSelectArea_Click(object sender, RoutedEventArgs e)
        {
            if (IsShowArea)
                lChartPhoto.ClearDrawObject();
            else
            {
                for (int i = 0; i < 11; i++)
                {
                    Rectangle r = new Rectangle();
                    r.Stroke = new SolidColorBrush(Colors.Red);
                    r.StrokeThickness = 3;
                    r.Width = lChartPhoto.getDrawObjectCanvas().Width / 11;
                    r.Height = lChartPhoto.getDrawObjectCanvas().Height / 11;
                    Canvas.SetLeft(r, i * r.Width);
                    Canvas.SetTop(r, i * r.Height);
                    lChartPhoto.getDrawObjectCanvas().Children.Add(r);
                }
            }
            IsShowArea = !IsShowArea;
        }

        bool IsShowCurve = false;
        private void buttonJBCurve_Click(object sender, RoutedEventArgs e)
        {
            if (IsShowCurve)
                lChartPhoto.ClearDrawObject();
            else
            {
                List<WriteableBitmap> bl = (testHelper.CurrentChart as GrayChart).ptp.getGBBrightChangedTestArea(ChartPhoto.getPhoto(), lynxUpDownAreaNum.IntValue);
                List<double> l = (testHelper.CurrentChart as GrayChart).ptp.getGBBrightChanged(bl);

                Canvas c = lChartPhoto.getDrawObjectCanvas();
                c.Children.Clear();
                DrawGraphic dg = new DrawGraphic(c);
                dg.ForeColor = Colors.Blue;
                dg.DrawLines(l,0,100);
            }
            IsShowCurve = !IsShowCurve;
        }

        private void buttondEV_Click(object sender, RoutedEventArgs e)
        {
            JBEVTest w = new JBEVTest();
            if (lChartPhoto.getPhoto() != null)
                w.lPhotoList1.AddPhoto(lChartPhoto.getPhoto());
            //w.lChartPhoto1.setPhoto(this.lChartPhoto.getPhoto());
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            ci.ToIcon();
            //Canvas Desk = CameraTestDesktop.getDesktopPanel();
            //if (Desk.Children.Contains(this))
            //{
            //    Desk.Children.Remove(this);
            //}
        }

        private void buttonSNR_Click(object sender, RoutedEventArgs e)
        {
            SNR.Text = (testHelper.CurrentChart as GrayChart).getImageSNR((testHelper.CurrentChart as GrayChart).ChartPhoto).ToString();
        }
    }
}
