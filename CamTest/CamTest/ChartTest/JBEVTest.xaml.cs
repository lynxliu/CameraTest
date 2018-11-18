using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using DCTestLibrary;
using SilverlightDCTestLibrary;
using SLPhotoTest.UIControl;
using SLPhotoTest.PhotoInfor;
using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI;

namespace SLPhotoTest.ChartTest
{
    public partial class JBEVTest : UserControl, IChartTestWindow
    {
        public JBEVTest()
        {
            InitializeComponent();
            testHelper = new ChartTestHelper(this);
            am = new ActionMove(this, Title);
            ci = new CameraTestIcon(this, null, "Aberration", "曝光量误差测试");
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
        public PhotoEditToolbar PEToolbar { get { return photoEditToolbar; } }
        public PhotoInforToolbar PIToolbar { get { return photoInforTool1; } }
        public LPhotoList PhotoList { get { return lPhotoList1; } }
        public LChartPhoto ChartPhoto { get { return lChartPhoto1; } }
        public ProgressBar TestProcessbar { get { return processbar; } }
        public ChartCorrect ChartCorrectToolbar { get { return null; } }
        public AbstractTestChart getChartTest(WriteableBitmap b)
        {
            return new GrayChart(b);
        }
        public List<ParameterTest> GetTestSteps()
        {
            List<ParameterTest> pl = new List<ParameterTest>();
            pl.Add(TestDEV);
            return pl;
        }
        ChartTestHelper testHelper;
        CameraTestIcon ci;
        ActionMove am;
        DateTime BeginTime;//测试起始时间
        DateTime EndTime;//测试完成时间

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

        public void ShowTestResult(WriteableBitmap chartPhoto)
        {
            AbstractTestChart ab = null;
            if ((chartPhoto != null) && testHelper.ChartTestList.ContainsKey(chartPhoto))
            {
                ab = testHelper.ChartTestList[chartPhoto];
            }

            testHelper.ShowTestResult(TextCenterL, "中央平均明度", ab);
            testHelper.ShowTestResult(textBlockDE, "曝光量误差", ab);
        }
        void TestDEV()
        {
            try
            {
                BeginTime = DateTime.Now;
                double r = (testHelper.CurrentChart as GrayChart).ptp.getGBDEv(lChartPhoto1.getPhoto(),lynxUpDownGamma.DoubleValue,lynxUpDownP.DoubleValue);
                EndTime = DateTime.Now;
                LParameter lp = testHelper.getNewParameter("灰度测试卡", r, BeginTime, EndTime);
                lp.Name = "曝光量误差";
                lp.Memo = "计算照片中央明度来确定曝光量";
                lp.Dimension = "";
                testHelper.ParameterList.Add(lp);

                WriteableBitmap cb = (testHelper.CurrentChart as GrayChart).ptp.getImageArea(lChartPhoto1.getPhoto(), lChartPhoto1.getPhoto().PixelWidth / 4, lChartPhoto1.getPhoto().PixelHeight / 4, lChartPhoto1.getPhoto().PixelWidth / 2, lChartPhoto1.getPhoto().PixelHeight / 2);
                double l = (testHelper.CurrentChart as GrayChart).ptp.getAverageColorL(cb);

                lp = testHelper.getNewParameter("灰度测试卡", l, BeginTime, EndTime);
                lp.Name = "中央平均明度";
                lp.Memo = "照片中央明度";
                lp.Dimension = "";
                testHelper.ParameterList.Add(lp);


                if (Math.Abs(r) >= 1)
                {
                    ChartTestHelper.setGBSign(false, gridGB);
                }
                else
                {
                    ChartTestHelper.setGBSign(true, gridGB);
                }
                //testHelper.ShowTestResult(TextAberration, "曝光量", null);
                ShowTestResult(lChartPhoto1.getPhoto());
            }
            catch (Exception xe)//未知的异常
            {
                testHelper.ProcessError(xe, "曝光量计算错误");
            }
        }


        bool IsShowArea = false;
        private void buttonShowArea_Click(object sender, RoutedEventArgs e)
        {
            if (IsShowArea)
                lChartPhoto1.ClearDrawObject();
            else
            {
                
                Rectangle r = new Rectangle();
                r.StrokeThickness = 2;
                r.Stroke = new SolidColorBrush(Colors.Red);
                r.Height = lChartPhoto1.getDrawObjectCanvas().Height / 2;
                r.Width = lChartPhoto1.getDrawObjectCanvas().Width / 2;
                Canvas.SetLeft(r, r.Width / 2);
                Canvas.SetTop(r, r.Height / 2);
                lChartPhoto1.getDrawObjectCanvas().Children.Add(r);
            }
            IsShowArea = !IsShowArea;

        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            if (ChartPhoto.IsSelect)
            {
                double r = (testHelper.CurrentChart as GrayChart).ptp.getDEv(lChartPhoto1.getSelectArea(), lynxUpDownGamma.DoubleValue, lynxUpDownP.DoubleValue);
                double l = (testHelper.CurrentChart as GrayChart).ptp.getAverageColorL(lChartPhoto1.getSelectArea());

                textBlockDE.Text = r.ToString();
                TextCenterL.Text = l.ToString();
                if (Math.Abs(r) >= 1)
                {
                    ChartTestHelper.setGBSign(false, gridGB);
                }
                else
                {
                    ChartTestHelper.setGBSign(true, gridGB);
                }
            }
            else
            {
                TestDEV();
            }
        }

        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lPhotoList1.Visibility = Visibility.Collapsed;
        }

        private void imageMulti_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lPhotoList1.Visibility = Visibility.Visible;
            e.Handled = true;
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
        private void buttonToIcon_Click(object sender, RoutedEventArgs e)
        {
            ci.ToIcon();
        }

    }
}
