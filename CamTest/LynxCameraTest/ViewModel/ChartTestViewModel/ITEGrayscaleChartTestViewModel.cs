using DCTestLibrary;
using LFC.common;
using LynxControls;
using PhotoTestControl;
using PhotoTestControl.Models;
using SilverlightDCTestLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace LynxCameraTest.ViewModel.ChartTestViewModel
{
    public class ITEGrayscaleChartTestViewModel : CommonChartTestViewModel
    {
        public ITEGrayscaleChartTestViewModel()
        {
            TestChart = new ITEGrayscaleChart();
            Test = TestLatitude;
        }
        ObservableCollection<double> _ResultValueList = new ObservableCollection<double>();
        public ObservableCollection<double> ResultValueList
        {
            get { return _ResultValueList; }
        }
        double _ConstL = 5;
        public double ConstL
        {
            get { return _ConstL; }
            set { _ConstL = value; OnPropertyChanged("ConstL"); }
        }
        int _TestAreaWidth = 64;
        public int TestAreaWidth
        {
            get { return _TestAreaWidth; }
            set { _TestAreaWidth = value; OnPropertyChanged("TestAreaWidth"); }
        }
        int _TestAreaHeight = 64;
        public int TestAreaHeight
        {
            get { return _TestAreaHeight; }
            set { _TestAreaHeight = value; OnPropertyChanged("TestAreaHeight"); }
        }
        int _ActiveHeight = 0;
        public int ActiveHeight
        {
            get { return _ActiveHeight; }
            set { _ActiveHeight = value; OnPropertyChanged("ActiveHeight"); }
        }
        int _LatitudeValue = 0;
        public int LatitudeValue
        {
            get { return _LatitudeValue; }
            set { _LatitudeValue = value; OnPropertyChanged("LatitudeValue"); }
        }
        public List<Result> TestLatitude(WriteableBitmap chart)
        {
            try
            {
                var rl = new List<Result>();
                TestChart.setChart(chart);
                (TestChart as ITEGrayscaleChart).ConstGradeL = ConstL;
                LatitudeValue = (TestChart as ITEGrayscaleChart).getGrayGrade(TestAreaHeight, TestAreaWidth);
                Result lp = new Result();
                lp.Name = "Latitude";
                lp.Memo = "数据表示在一张照片里面可以分辨的灰阶亮度级别";
                lp.Dimension = "Grade";
                lp.Value = LatitudeValue;
                rl.Add(lp);

                if (LatitudeValue == 11)
                    ChartTestHelper.setGBSign(true, GBControl);
                else 
                    ChartTestHelper.setGBSign(false, GBControl);
                ShowLatitude();
                return rl;
            }
            catch (Exception xe)
            {
                SilverlightLFC.common.Environment.ShowMessage(xe.Message);
                //testHelper.ProcessError(xe, "灰阶计算错误,请检查照片");
                return null;
            }
        }
        public Panel GBControl { get; set; }

        Canvas _DrawCanvas = null;
        public Canvas DrawCanvas
        {
            get { return _DrawCanvas; }
            set { _DrawCanvas = value; dg = new DrawGraphic(value); }
        }
        DrawGraphic dg;

        void ShowLatitude()
        {
            ResultValueList.Clear();
            DrawCanvas.Children.Clear();

            List<double> dl = (TestChart as ITEGrayscaleChart).getHLineAveL(TestAreaHeight, TestAreaWidth);
            foreach (double d in dl)
            {
                ResultValueList.Add(d);
            }
            dg.DrawBrightHistogram(dl);
        }

        void ShowLatitude(int y)
        {
            ResultValueList.Clear();
            DrawCanvas.Children.Clear();

            List<double> dl = (TestChart as ITEGrayscaleChart).getHLineAveL(y, TestAreaHeight, TestAreaWidth);
            foreach (double d in dl)
            {
                ResultValueList.Add(d);
            }
            dg.DrawBrightHistogram(dl);
        }

        //bool _IsUseConstGradeL = false;
        public bool IsUseConstGradeL
        {
            get { return (TestChart as ITEGrayscaleChart).IsUseConstGradeL; }
            set { (TestChart as ITEGrayscaleChart).IsUseConstGradeL = value; OnPropertyChanged("IsUseConstGradeL"); }
        }
        Brush _ActiveButtonBackground = new SolidColorBrush(Colors.Blue);
        public Brush ActiveButtonBackground
        {
            get { return _ActiveButtonBackground; }
            set { _ActiveButtonBackground = value; OnPropertyChanged("ActiveButtonBackground"); }
        }
        static Brush ActiveBrush = new SolidColorBrush(Colors.Red);
        static Brush DeactiveBrush = new SolidColorBrush(Colors.Blue);
        public DelegateCommand<FlipView> InteractiveTestCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    if (list == null) return;
                    var control = GetCurrentPhotoViewControl(list);
                    if (control == null) return;
                    control.PointerPressed += control_PointerPressed;
                    control.PointerMoved += control_PointerMoved;
                    ActiveButtonBackground = ActiveBrush;
                });
            }
        }
        Line tl = new Line() { StrokeThickness=3,Stroke=new SolidColorBrush(Colors.Red)};
        void control_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var ChartPhoto = sender as LynxPhotoViewControl;
            if (ChartPhoto == null) return;
            if (!ChartPhoto.getDrawObjectCanvas().Children.Contains(tl))
            {
                ChartPhoto.getDrawObjectCanvas().Children.Add(tl);
            }
            tl.X1 = 0;
            tl.X2 = ChartPhoto.Width;
            tl.Y1 = tl.Y2 = e.GetCurrentPoint(ChartPhoto).Position.Y;
        }

        void control_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var control = sender as LynxPhotoViewControl;
            control.PointerPressed -= control_PointerPressed;
            control.PointerMoved -= control_PointerMoved;
            ActiveButtonBackground = DeactiveBrush;
            if (control.getDrawObjectCanvas().Children.Contains(tl)) 
            { control.getDrawObjectCanvas().Children.Remove(tl); }

            (TestChart as ITEGrayscaleChart).ConstGradeL = ConstL;
            double h = e.GetCurrentPoint(control.getDrawObjectCanvas()).Position.Y / control.getImage().Height * control.getPhoto().PixelHeight;
            ActiveHeight = Convert.ToInt32(h);
            int v = (TestChart as ITEGrayscaleChart).getGrayGrade(ActiveHeight, TestAreaHeight, TestAreaWidth);
            LatitudeValue = v;
            ShowLatitude(ActiveHeight);

            if (LatitudeValue == 11)
                ChartTestHelper.setGBSign(true, GBControl);
            else
                ChartTestHelper.setGBSign(false, GBControl);
        }
    }
}
