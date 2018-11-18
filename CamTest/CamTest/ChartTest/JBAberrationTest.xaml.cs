using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SLPhotoTest.PhotoInfor;
using SLPhotoTest.UIControl;
using DCTestLibrary;

using SilverlightDCTestLibrary;
using SilverlightLynxControls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;

namespace SLPhotoTest.ChartTest
{
    public partial class JBAberrationTest : UserControl, IChartTestWindow
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
            pl.Add(Test);
            return pl;
        }
        ChartTestHelper testHelper;
        CameraTestIcon ci;
        ActionMove am;
        DateTime BeginTime;//测试起始时间
        DateTime EndTime;//测试完成时间
        public JBAberrationTest()
        {
            InitializeComponent();
            testHelper = new ChartTestHelper(this);
            am = new ActionMove(this, Title);
            ci = new CameraTestIcon(this, null, "Aberration", "畸变测试");
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

        Button CurrentButton=null;//当前的按钮


        void BeginSelectPoint()
        {
            Canvas c = ChartPhoto.getDrawObjectCanvas();
            lChartPhoto1.EnableCross = true;

            c.PointerPressed += c_PointerPressed;
        }

        void c_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as Canvas).PointerPressed -= c_PointerPressed;
            lChartPhoto1.EnableCross = false;
            if (CurrentButton != null)
            {
                CurrentButton.Tag = e.GetCurrentPoint(sender as Canvas).Position;
                CurrentButton.Content = "(" + e.GetCurrentPoint(sender as Canvas).Position.X.ToString() + "," + e.GetCurrentPoint(sender as Canvas).Position.Y.ToString() + ")";
                CurrentButton = null;
                ShowD();
            }
        }

        double getDistance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        void Test()
        {
            
            if (button1LS.Tag == null) { SilverlightLFC.common.Environment.ShowMessage("请选择0.1半径对角线左边界"); }
            if (button1RS.Tag == null) { SilverlightLFC.common.Environment.ShowMessage("请选择0.1半径对角线右边界"); }
            if (button5LS.Tag == null) { SilverlightLFC.common.Environment.ShowMessage("请选择0.5半径对角线左边界"); }
            if (button5RS.Tag == null) { SilverlightLFC.common.Environment.ShowMessage("请选择0.5半径对角线右边界"); }
            if (button9LS.Tag == null) { SilverlightLFC.common.Environment.ShowMessage("请选择0.9半径对角线左边界"); }
            if (button9RS.Tag == null) { SilverlightLFC.common.Environment.ShowMessage("请选择0.9半径对角线右边界"); }
            BeginTime = DateTime.Now;
            


            double _1l=getDistance((button1LS.Tag as Point?).Value,(button1RS.Tag as Point?).Value);
            double _5l = getDistance((button5LS.Tag as Point?).Value, (button5RS.Tag as Point?).Value);
            double _9l = getDistance((button9LS.Tag as Point?).Value, (button9RS.Tag as Point?).Value);

            double r5 = (testHelper.CurrentChart as AbstractTestChart).ptp.getGBAberration(_5l, _1l, 5);
            double r9 = (testHelper.CurrentChart as AbstractTestChart).ptp.getGBAberration(_9l, _1l, 9);

            

            double h = 0.05;
            if (!checkBoxFocus.IsChecked.Value)
            {
                h = 0.07;
            }
            if (Math.Abs(r5) < h && Math.Abs(r9) < h)
            {
                ChartTestHelper.setGBSign(true, gridGB); 
            }
            else
            {
                ChartTestHelper.setGBSign(false, gridGB);
            }
            EndTime = DateTime.Now;
            LParameter lp = testHelper.getNewParameter("畸变测试卡0.5处", r5, BeginTime, EndTime);
            lp.Name = "国标畸变测试0.5处";
            lp.Memo = "计算照片变形程度测算畸变";
            lp.Dimension = "";
            testHelper.ParameterList.Add(lp);

            lp = testHelper.getNewParameter("畸变测试卡0.9处", r9, BeginTime, EndTime);
            lp.Name = "国标畸变测试0.9处";
            lp.Memo = "计算照片变形程度测算畸变";
            lp.Dimension = "";
            testHelper.ParameterList.Add(lp);

            ShowTestResult(lChartPhoto1.getPhoto());
        }

        public void ShowTestResult(WriteableBitmap chartPhoto)
        {
            AbstractTestChart ab = null;
            if ((chartPhoto != null) && testHelper.ChartTestList.ContainsKey(chartPhoto))
            {
                ab = testHelper.ChartTestList[chartPhoto];
            }

            testHelper.ShowTestResult(textBlock5Aberration, "国标畸变测试0.5处", ab);
            testHelper.ShowTestResult(textBlock9Aberration, "国标畸变测试0.9处", ab);
        }

        void Show1D()
        {
            if (button1RS.Tag == null || button1LS.Tag == null) { return; }
            textBlock1D.Text = getDistance((button1RS.Tag as Point?).Value, (button1LS.Tag as Point?).Value).ToString();
        }

        void Show5D()
        {
            if (button5RS.Tag == null || button5LS.Tag == null) { return; }
            textBlock5D.Text = getDistance((button5RS.Tag as Point?).Value, (button5LS.Tag as Point?).Value).ToString();
        }

        void Show9D()
        {
            if (button9RS.Tag == null || button9LS.Tag == null) { return; }
            textBlock9D.Text = getDistance((button9RS.Tag as Point?).Value, (button9LS.Tag as Point?).Value).ToString();
        }

        void ShowD()
        {
            Show1D();
            Show5D();
            Show9D();
        }

        private void button1RS_Click(object sender, RoutedEventArgs e)
        {
            CurrentButton = button1RS;
            BeginSelectPoint();
            
        }

        private void button1LS_Click(object sender, RoutedEventArgs e)
        {
            CurrentButton = button1LS;
            BeginSelectPoint();
        }

        private void button5RS_Click(object sender, RoutedEventArgs e)
        {
            CurrentButton = button5RS;
            BeginSelectPoint();
        }

        private void button5LS_Click(object sender, RoutedEventArgs e)
        {
            CurrentButton = button5LS;
            BeginSelectPoint();
        }

        private void button9RS_Click(object sender, RoutedEventArgs e)
        {
            CurrentButton = button9RS;
            BeginSelectPoint();
        }

        private void button9LS_Click(object sender, RoutedEventArgs e)
        {
            CurrentButton = button9LS;
            BeginSelectPoint();
        }

        private void buttonCalculate_Click(object sender, RoutedEventArgs e)
        {
            Test();
            
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
