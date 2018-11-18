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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;

namespace SLPhotoTest.ChartTest
{
    public partial class ITEGrayscaleChartTest : UserControl, IChartTestWindow
    {
        public PhotoEditToolbar PEToolbar { get { return photoEditToolbar; } }
        public PhotoInforToolbar PIToolbar { get { return photoInforTool1; } }
        public LPhotoList PhotoList { get { return lPhotoList1; } }
        public LChartPhoto ChartPhoto { get { return lChartPhoto; } }
        public ProgressBar TestProcessbar { get { return processbar; } }
        public ChartCorrect ChartCorrectToolbar { get { return null; } }
        public AbstractTestChart getChartTest(WriteableBitmap b)
        {
            return new ITEGrayscaleChart(b);
        }
        public List<ParameterTest> GetTestSteps()
        {
            List<ParameterTest> pl = new List<ParameterTest>();
            pl.Add(TestLatitude);
            pl.Add(ShowLatitude);
            return pl;
        }
        ChartTestHelper testHelper;
        DrawGraphic dg;
        public ITEGrayscaleChartTest()
        {
            InitializeComponent();
            testHelper = new ChartTestHelper(this);
            //photoInforTool1.setTarget(ChartPhoto);
            am = new ActionMove(this, Title);

            //photoEditToolbar.setChartControl(ChartPhoto);//设置图版和工具栏的关联
            //photoEditToolbar.saveChartTestResult = WriteToHTML;
            //ChartPhoto.InitTest(autoTest);
            //ChartPhoto.InitSaveTest(WriteToHTML);
            ci = new CameraTestIcon(this, null, "ITEGrayscaleChart", "ITE灰阶");
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
            dg = new DrawGraphic(canvasLatitude);
        }

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
                (testHelper.CurrentChart as ITEGrayscaleChart).ConstGradeL = lynxUpDownConstL.DoubleValue;
                int v = (testHelper.CurrentChart as ITEGrayscaleChart).getGrayGrade(lynxUpDownH.IntValue,lynxUpDownW.IntValue);
                EndTime = DateTime.Now;
                //textBlockLatitude.Text = v.ToString();
                LParameter lp = testHelper.getNewParameter("ITE灰阶", v, BeginTime, EndTime);
                lp.Name = "灰阶";
                lp.Memo = "数据表示在一张照片里面可以分辨的灰阶亮度级别";
                lp.Dimension = "级";
                testHelper.ParameterList.Add(lp);
                ShowTestResult(lChartPhoto.getPhoto());
                //List<double> dl = (testHelper.CurrentChart as ITEGrayscaleChart).getHLineAveL(lynxUpDownH.IntValue, lynxUpDownW.IntValue);
                //comboBoxLGrade.Items.Clear();

                //canvasLatitude.Children.Clear();
                //dg.DrawBrightHistogram(dl);

                if (v == 11)
                {
                    ChartTestHelper.setGBSign(true, gridGB);

                }
                else ChartTestHelper.setGBSign(false, gridGB); 
            }
            catch (Exception xe)
            {
                testHelper.ProcessError(xe, "灰阶计算错误,请检查照片");
            }
            EndTime = DateTime.Now;
            //processbar.Value = 100;
        }
        void ShowLatitude()
        {
            comboBoxLGrade.Items.Clear();
            canvasLatitude.Children.Clear();

            List<double> dl = (testHelper.CurrentChart as ITEGrayscaleChart).getHLineAveL(lynxUpDownH.IntValue, lynxUpDownW.IntValue); 
            foreach(double d in dl)
            {
                ComboBoxItem ci = new ComboBoxItem();
                ci.Content = d.ToString();
                comboBoxLGrade.Items.Add(ci);
            }
            dg.DrawBrightHistogram(dl);
        }

        void ShowLatitude(int y)
        {
            comboBoxLGrade.Items.Clear();
            canvasLatitude.Children.Clear();

            List<double> dl = (testHelper.CurrentChart as ITEGrayscaleChart).getHLineAveL(y,lynxUpDownH.IntValue, lynxUpDownW.IntValue);
            foreach (double d in dl)
            {
                ComboBoxItem ci = new ComboBoxItem();
                ci.Content = d.ToString();
                comboBoxLGrade.Items.Add(ci);
            }
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
            c.PointerPressed += c_PointerPressed;
            //c.PointerReleased += new MouseButtonEventHandler(c_PointerReleased);
            c.PointerMoved += c_PointerMoved;
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

                (testHelper.CurrentChart as ITEGrayscaleChart).ConstGradeL = lynxUpDownConstL.DoubleValue;
                double h = e.GetCurrentPoint(ChartPhoto.getDrawObjectCanvas()).Position.Y / ChartPhoto.getImage().Height * ChartPhoto.getPhoto().PixelHeight;
                textBlockHeight.Text = Convert.ToInt32(h).ToString();
                int v = (testHelper.CurrentChart as ITEGrayscaleChart).getGrayGrade(Convert.ToInt32(h), lynxUpDownH.IntValue,lynxUpDownW.IntValue);
                textBlockLatitude.Text = v.ToString();
                ToolTipService.SetToolTip(textBlockLatitude, "交互测试结果：" + v.ToString());
                ShowLatitude(Convert.ToInt32(h));
                //List<double> dl = (testHelper.CurrentChart as ITEGrayscaleChart).getHLineAveL(Convert.ToInt32(h),lynxUpDownH.IntValue,lynxUpDownW.IntValue);
                //canvasLatitude.Children.Clear();
                //dg.DrawBrightHistogram(dl);
                buttonInteractiveTest.Foreground = new SolidColorBrush(Colors.Blue);
                if (c.Children.Contains(tl)) { c.Children.Remove(tl); }
                if (v == 11)
                {
                    ChartTestHelper.setGBSign(true, gridGB);

                }
                else ChartTestHelper.setGBSign(false, gridGB); 
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
                c.PointerPressed -= c_PointerPressed;
                c.PointerMoved -= c_PointerMoved;
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
            testHelper.ShowTestResult(textBlockLatitude, "灰阶", ab);

        }

        private void checkBoxIsConstL_Checked(object sender, RoutedEventArgs e)
        {
            (testHelper.CurrentChart as ITEGrayscaleChart).IsUseConstGradeL = true;
        }

        private void checkBoxIsConstL_UnChecked(object sender, RoutedEventArgs e)
        {

            (testHelper.CurrentChart as ITEGrayscaleChart).IsUseConstGradeL = false;
        }
    }
}
