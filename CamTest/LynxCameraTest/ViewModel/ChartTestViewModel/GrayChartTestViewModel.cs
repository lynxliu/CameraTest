using DCTestLibrary;
using LFC.common;
using LynxCameraTest.Model;
using LynxControls;
using PhotoTestControl;
using PhotoTestControl.Models;
using SilverlightDCTestLibrary;
using SilverlightLFC.common;
using SLPhotoTest.ChartTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace LynxCameraTest.ViewModel.ChartTestViewModel
{
    public class GrayChartTestViewModel : CommonChartTestViewModel
    {
        public GrayChartTestViewModel()
        {
            TestChart = new GrayChart();
            Test = TestGray;
        }

        double _BrightChangesValue = 0;
        public double BrightChangesValue
        {
            get { return _BrightChangesValue; }
            set { _BrightChangesValue = value; OnPropertyChanged("BrightChangesValue"); }
        }

        List<Result> TestGray(WriteableBitmap chart)
        {
            var rl = new List<Result>();
            TestChart.setChart(chart);
            var Chart = TestChart as GrayChart;
            BrightChangesValue = Chart.getBrightChangesValue();

            rl.Add(new Result()
            {
                Value = BrightChangesValue,
                TestTime = DateTime.Now,
                Name = "Bright changes",
                Memo = "",
                TestCount = 1,
                Dimension = "%"
            });
            rl.Add(new Result()
            {
                Value =GBValue= Chart.ptp.getGBBrightChangedValue(CurrentPhoto, GBSelectAreaNum),
                TestTime = DateTime.Now,
                Name = "GB bright changes value",
                Memo = "",
                TestCount = 1,
                Dimension = "%"
            });
            TestGB(chart);
            return rl;
        }

        bool _IsLongFocus = true;
        public bool IsLongFocus
        {
            get { return _IsLongFocus; }
            set { _IsLongFocus = value; OnPropertyChanged("IsLongFocus"); }
        }
        public Panel GBControl { get; set; }
        void TestGB(WriteableBitmap b)
        {
            GBValue = (TestChart as GrayChart).ptp.getGBBrightChangedValue(b, GBSelectAreaNum);

            if (IsLongFocus)
            {
                if (GBValue > 0.7) { ChartTestHelper.setGBSign(true, GBControl); }
                else ChartTestHelper.setGBSign(false, GBControl);

            }
            else
            {
                if (GBValue > 0.5) ChartTestHelper.setGBSign(true, GBControl);
                else ChartTestHelper.setGBSign(false, GBControl);
            }
        }

        int _GBSelectAreaNum=11;
        public int GBSelectAreaNum
        {
            get { return _GBSelectAreaNum; }
            set { _GBSelectAreaNum = value; OnPropertyChanged("GBSelectAreaNum"); }
        }
        double _GBValue = 0;
        public double GBValue
        {
            get { return _GBValue; }
            set { _GBValue = value; OnPropertyChanged("GBValue"); }
        }

        bool IsShowArea = false;
        void SwitchShowSelectArea(FlipView control)
        {
            var photoview = GetCurrentPhotoViewControl(control);
            if (photoview == null) return;
            if (IsShowArea)
            {
                photoview.Clear();
            }
            else
            {
                for (int i = 0; i < GBSelectAreaNum; i++)
                {
                    Rectangle r = new Rectangle();
                    r.Stroke = new SolidColorBrush(Colors.Red);
                    r.StrokeThickness = 3;
                    r.Width = photoview.getDrawObjectCanvas().Width / GBSelectAreaNum;
                    r.Height = photoview.getDrawObjectCanvas().Height / GBSelectAreaNum;
                    Canvas.SetLeft(r, i * r.Width);
                    Canvas.SetTop(r, i * r.Height);
                    photoview.getDrawObjectCanvas().Children.Add(r);
                }
            }
            IsShowArea = !IsShowArea;
        }

        public DelegateCommand<FlipView> SwitchShowSelectAreaCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list)=>
                    SwitchShowSelectArea(list));
            }
        }

        bool IsShowCurve = false;
        void SwitchShowBrightCurve(FlipView control)
        {
            var photoview = GetCurrentPhotoViewControl(control);
            if (photoview == null) return;
            if (IsShowCurve)
                photoview.Clear();
            else
            {
                List<WriteableBitmap> bl = (TestChart as GrayChart).ptp.getGBBrightChangedTestArea(CurrentPhoto, GBSelectAreaNum);
                List<double> l = (TestChart as GrayChart).ptp.getGBBrightChanged(bl);

                Canvas c = photoview.getDrawObjectCanvas();
                c.Children.Clear();
                DrawGraphic dg = new DrawGraphic(c);
                dg.ForeColor = Colors.Blue;
                dg.DrawLines(l, 0, 100);
            }
            IsShowCurve = !IsShowCurve;

        }
        public DelegateCommand<FlipView> SwitchShowBrightCurveCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                    SwitchShowBrightCurve(list));
            }
        }

        int _GrayGrade = 5;
        public int GrayGrade
        {
            get { return _GrayGrade; }
            set { _GrayGrade = value; OnPropertyChanged("GrayGrade"); }
        }

        int _BrightGradeDistance = 2;
        public int BrightGradeDistance
        {
            get { return _BrightGradeDistance; }
            set { _BrightGradeDistance = value; OnPropertyChanged("BrightGradeDistance"); }
        }

        int _CustomSelectPointSimularPixelNum = 0;
        public int CustomSelectPointSimularPixelNum
        {
            get { return _CustomSelectPointSimularPixelNum; }
            set { _CustomSelectPointSimularPixelNum = value; OnPropertyChanged("CustomSelectPointSimularPixelNum"); }
        }

        Brush _CustomButtonBrush = DeactiveBrush;
        public Brush CustomButtonBrush
        {
            get { return _CustomButtonBrush; }
            set { _CustomButtonBrush = value; OnPropertyChanged("CustomButtonBrush"); }
        }
        static Brush ActiveBrush = new SolidColorBrush(Colors.Red);
        static Brush DeactiveBrush = new SolidColorBrush(Colors.Blue);
        public DelegateCommand<FlipView> CustomSelectPointCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    var photoview = GetCurrentPhotoViewControl(list);
                    if (photoview == null) return;
                    currentPhotoControl = photoview;
                    CustomButtonBrush = ActiveBrush;

                    Canvas c = photoview.getDrawObjectCanvas();
                    c.PointerMoved -= c_PointerMoved;
                    c.PointerPressed -= c_PointerPressed;
                    c.PointerMoved += c_PointerMoved;
                    c.PointerPressed += c_PointerPressed;

                });
            }
        }
        LynxPhotoViewControl currentPhotoControl = null;
        Line hl = new Line() { Stroke = new SolidColorBrush(Colors.Red), StrokeThickness = 2 };
        Line vl = new Line() { Stroke = new SolidColorBrush(Colors.Red), StrokeThickness = 2 };
        void c_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            
            Canvas c = sender as Canvas;
            if (c.Children.Contains(hl)) { c.Children.Remove(hl); }
            if (c.Children.Contains(vl)) { c.Children.Remove(vl); }
            CustomButtonBrush = DeactiveBrush;
            c.PointerMoved -= c_PointerMoved;
            c.PointerPressed -= c_PointerPressed; 
            if (CurrentPhoto == null) { return; }
            DrawGraphic dg = new DrawGraphic();
            Point tp = e.GetCurrentPoint(c).Position;
            Point ip = DrawGraphic.getImagePosition(tp, currentPhotoControl.getImage());

            Color cc = TestChart.GetPixel(currentPhotoControl.getPhoto(), (int)ip.X, (int)ip.Y);
            //TempPhoto =new WriteableBitmap( ChartPhoto.getPhoto());//复制一份保留
            try
            {
                WriteableBitmap oi;
                CustomSelectPointSimularPixelNum = TestChart.getFloodBrightEdge(currentPhotoControl.getPhoto(), out oi, ip, BrightGradeDistance);
                //IsNeedSave = false;
                currentPhotoControl.setPhoto(oi);
            }
            catch (Exception xe)//未知的异常
            {
                if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
                {
                    SilverlightLFC.common.Environment.ShowMessage(xe.Message);
                }
                else
                {
                    SilverlightLFC.common.Environment.ShowMessage("Calculate error:"+xe.Message);
                    //MessageBox.Show("计算亮度变化错误");
                }
            }
        }

        void c_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Canvas c = sender as Canvas;
            if (c == null) return;
            if (!c.Children.Contains(hl))
            {
                c.Children.Add(hl);
            }

            hl.X1 = 0;
            hl.X2 = c.ActualWidth;
            hl.Y1 = hl.Y2 = e.GetCurrentPoint(c).Position.Y;
            if (!c.Children.Contains(vl))
            {
                c.Children.Add(vl);
            }

            vl.Y1 = 0;
            vl.Y2 = c.ActualHeight;
            vl.X1 = vl.X2 = e.GetCurrentPoint(c).Position.X;

        }

        public DelegateCommand ShowEVCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    JBEVTest w = new JBEVTest();
                    JBEVTestViewModel vm = new JBEVTestViewModel();
                    w.DataContext = vm;
                    if (CurrentPhoto != null)
                        vm.PhotoList.Add(CurrentPhoto);
                    CommonHelper.Show(w);
                });
            }
        }

        double _SNRValue = 0;
        public double SNRValue
        {
            get { return _SNRValue; }
            set { _SNRValue = value; OnPropertyChanged("SNRValue"); }
        }
        public DelegateCommand TestSNRCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SNRValue = (TestChart as GrayChart).getImageSNR((TestChart as GrayChart).ChartPhoto);

                });
            }
        }

        public DelegateCommand<FlipView> ShowBrightCurveCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                    {
                        var photoview = GetCurrentPhotoViewControl(list);
                        if (photoview == null) return;
                        WriteableBitmap b = (TestChart as GrayChart).getBrightChangesImage(GrayGrade);
                        photoview.setPhoto(b);
                    });
            }
        }
        public DelegateCommand<FlipView> ShowSourceCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    var photoview = GetCurrentPhotoViewControl(list);
                    if (photoview == null) return;
                    photoview.setPhoto((TestChart as GrayChart).mp.SourcePhoto);
                });
            }
        }
    }
}
