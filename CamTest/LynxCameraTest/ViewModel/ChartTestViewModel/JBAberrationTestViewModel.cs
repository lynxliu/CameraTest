using DCTestLibrary;
using LFC.common;
using PhotoTestControl;
using PhotoTestControl.Models;
using SilverlightDCTestLibrary;
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
    public class JBAberrationTestViewModel : CommonChartTestViewModel
    {
        public JBAberrationTestViewModel()
        {
            TestChart = new AberrationChart();
            Test = TestGBAberration;
        }
        public Panel GBControl { get; set; }

        double _Value10p = 0;
        public double Value10p
        {
            get { return _Value10p; }
            set { _Value10p=value; OnPropertyChanged("Value10p"); }
        }

        double _Value50p = 0;
        public double Value50p
        {
            get { return _Value50p; }
            set { _Value50p = value; OnPropertyChanged("Value50p"); }
        }

        double _Value90p = 0;
        public double Value90p
        {
            get { return _Value90p; }
            set { _Value90p = value; OnPropertyChanged("Value90p"); }
        }


        string _LeftInfo10p = "10% left";
        public string LeftInfo10p
        {
            get { return _LeftInfo10p; }
            set { _LeftInfo10p = value; OnPropertyChanged("LeftInfo10p"); }
        }

        string _RightInfo10p = "10% right";
        public string RightInfo10p
        {
            get { return _RightInfo10p; }
            set { _RightInfo10p = value; OnPropertyChanged("RightInfo10p"); }
        }

        string _LeftInfo50p = "50% left";
        public string LeftInfo50p
        {
            get { return _LeftInfo50p; }
            set { _LeftInfo50p = value; OnPropertyChanged("LeftInfo50p"); }
        }

        string _RightInfo50p = "50% right";
        public string RightInfo50p
        {
            get { return _RightInfo50p; }
            set { _RightInfo50p = value; OnPropertyChanged("RightInfo50p"); }
        }

        string OperationStr = "";

        string _LeftInfo90p = "90% left";
        public string LeftInfo90p
        {
            get { return _LeftInfo90p; }
            set { _LeftInfo90p = value; OnPropertyChanged("LeftInfo90p"); }
        }

        string _RightInfo90p = "90% right";
        public string RightInfo90p
        {
            get { return _RightInfo90p; }
            set { _RightInfo90p = value; OnPropertyChanged("RightInfo90p"); }
        }

        Brush _p10LeftButtonBrush = DeactiveBrush;
        public Brush p10LeftButtonBrush
        {
            get { return _p10LeftButtonBrush; }
            set { _p10LeftButtonBrush = value; OnPropertyChanged("p10LeftButtonBrush"); }
        }
        Brush _p50LeftButtonBrush = DeactiveBrush;
        public Brush p50LeftButtonBrush
        {
            get { return _p50LeftButtonBrush; }
            set { _p50LeftButtonBrush = value; OnPropertyChanged("p50LeftButtonBrush"); }
        }
        Brush _p90LeftButtonBrush = DeactiveBrush;
        public Brush p90LeftButtonBrush
        {
            get { return _p90LeftButtonBrush; }
            set { _p90LeftButtonBrush = value; OnPropertyChanged("p90LeftButtonBrush"); }
        }

        Brush _p10RightButtonBrush = DeactiveBrush;
        public Brush p10RightButtonBrush
        {
            get { return _p10RightButtonBrush; }
            set { _p10RightButtonBrush = value; OnPropertyChanged("p10RightButtonBrush"); }
        }
        Brush _p50RightButtonBrush = DeactiveBrush;
        public Brush p50RightButtonBrush
        {
            get { return _p50RightButtonBrush; }
            set { _p50RightButtonBrush = value; OnPropertyChanged("p50RightButtonBrush"); }
        }
        Brush _p90RightButtonBrush = DeactiveBrush;
        public Brush p90RightButtonBrush
        {
            get { return _p90RightButtonBrush; }
            set { _p90RightButtonBrush = value; OnPropertyChanged("p90RightButtonBrush"); }
        }
        static Brush ActiveBrush = new SolidColorBrush(Colors.Red);
        static Brush DeactiveBrush = new SolidColorBrush(Colors.Blue);

        Point? L10=null;
        Point? L50 = null;
        Point? L90 = null;
        Point? R10 = null;
        Point? R50 = null;
        Point? R90 = null;

        string _Result10 = "0";
        public string Result10
        {
            get { return _Result10; }
            set { _Result10 = value; OnPropertyChanged("Result10"); }
        }
        string _Result50 = "0";
        public string Result50
        {
            get { return _Result50; }
            set { _Result50 = value; OnPropertyChanged("Result50"); }
        }
        string _Result90 = "0";
        public string Result90
        {
            get { return _Result90; }
            set { _Result90 = value; OnPropertyChanged("Result90"); }
        }
        double getDistance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        Line HLine = new Line() { Stroke = new SolidColorBrush(Colors.Red), StrokeThickness = 2 };
        Line VLine = new Line() { Stroke = new SolidColorBrush(Colors.Red), StrokeThickness = 2 };

        void CurrentLinePointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var layer = sender as Canvas;
            if (layer == null) return;
            VLine.Y1 = 0;
            VLine.Y2 = layer.ActualHeight;
            VLine.X1 = VLine.X2 = e.GetCurrentPoint(layer).Position.X;
            HLine.X1 = 0;
            HLine.X2 = layer.ActualWidth;
            HLine.Y1 = HLine.Y2 = e.GetCurrentPoint(layer).Position.Y;
        }
        void CurrentLinePointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var layer = sender as Canvas;
            if (layer == null) return;
            ClearActive(layer);
            var ts="(" + e.GetCurrentPoint(layer).Position.X.ToString() + "," + e.GetCurrentPoint(layer).Position.Y.ToString() + ")";
            if (OperationStr.Contains("10") && OperationStr.Contains("Left"))
            {
                LeftInfo10p = ts;
                L10 = e.GetCurrentPoint(layer).Position;
                if (R10 != null)
                {
                    Result10 = getDistance(L10.Value, R10.Value).ToString();
                }
            }
            if (OperationStr.Contains("50") && OperationStr.Contains("Left"))
            {
                LeftInfo50p = ts;
                L50 = e.GetCurrentPoint(layer).Position;
                if (R50 != null)
                {
                    Result50 = getDistance(L50.Value, R50.Value).ToString();
                }
            }
            if (OperationStr.Contains("90") && OperationStr.Contains("Left"))
            {
                LeftInfo90p = ts; 
                L90 = e.GetCurrentPoint(layer).Position;
                if (R90 != null)
                {
                    Result90 = getDistance(L90.Value, R90.Value).ToString();
                }
            }
            if (OperationStr.Contains("10") && OperationStr.Contains("Right"))
            { 
                RightInfo10p = ts;
                R10 = e.GetCurrentPoint(layer).Position;
                if (L10 != null)
                {
                    Result10 = getDistance(L10.Value, R10.Value).ToString();
                }
            }
            if (OperationStr.Contains("50") && OperationStr.Contains("Right"))
            {
                RightInfo50p = ts;
                R50 = e.GetCurrentPoint(layer).Position;
                if (L50 != null)
                {
                    Result50 = getDistance(L50.Value, R50.Value).ToString();
                }
            }
            if (OperationStr.Contains("90") && OperationStr.Contains("Right"))
            {
                RightInfo90p = ts;
                R90 = e.GetCurrentPoint(layer).Position;
                if (L90 != null)
                {
                    Result90 = getDistance(L90.Value, R90.Value).ToString();
                }
            }
        }
        void ClearActive(Canvas layer)
        {
            layer.Children.Remove(VLine);
            layer.Children.Remove(HLine);
            p10RightButtonBrush = DeactiveBrush;
            p50RightButtonBrush = DeactiveBrush;
            p90RightButtonBrush = DeactiveBrush;
            p10LeftButtonBrush = DeactiveBrush;
            p50LeftButtonBrush = DeactiveBrush;
            p90LeftButtonBrush = DeactiveBrush;
            layer.PointerMoved -= CurrentLinePointerMoved;
            layer.PointerPressed -= CurrentLinePointerPressed;
        }
        void StartActive(FlipView list)
        {
            var control = GetCurrentPhotoViewControl(list);
            if (control == null) return;
            var layer = control.getDrawObjectCanvas();
            ClearActive(layer);
            if (!layer.Children.Contains(VLine))
            {
                layer.Children.Add(VLine);
            }
            if (!layer.Children.Contains(HLine))
            {
                layer.Children.Add(HLine);
            }
            layer.PointerMoved += CurrentLinePointerMoved;
            layer.PointerPressed += CurrentLinePointerPressed;
        }
        public DelegateCommand<FlipView> Active10LeftCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    OperationStr = "10Left";
                    p10LeftButtonBrush = ActiveBrush;
                    StartActive(list);
                });
            }
        }
        public DelegateCommand<FlipView> Active50LeftCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    OperationStr = "50Left";
                    p50LeftButtonBrush = ActiveBrush;
                    StartActive(list);
                });
            }
        }
        public DelegateCommand<FlipView> Active90LeftCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    OperationStr = "90Left";
                    p90LeftButtonBrush = ActiveBrush;
                    StartActive(list);
                });
            }
        }
        public DelegateCommand<FlipView> Active10RightCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    OperationStr = "10Right";
                    p10RightButtonBrush = ActiveBrush;
                    StartActive(list);
                });
            }
        }
        public DelegateCommand<FlipView> Active50RightCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    OperationStr = "50Right";
                    p50RightButtonBrush = ActiveBrush;
                    StartActive(list);
                });
            }
        }
        public DelegateCommand<FlipView> Active90RightCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    OperationStr = "90Right";
                    p90RightButtonBrush = ActiveBrush;
                    StartActive(list);
                });
            }
        }
        public DelegateCommand GBAberrationTestCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {

                    TestGBAberration(CurrentPhoto);
                });
            }
        }
        bool _IsLongFocus = true;
        public bool IsLongFocus
        {
            get { return _IsLongFocus; }
            set { _IsLongFocus = value; OnPropertyChanged("IsLongFocus"); }
        }

        List<Result> TestGBAberration(WriteableBitmap photo)
        {
            var rl = new List<Result>();
            TestChart.setChart(photo);
            var Chart = TestChart as AberrationChart;

            double _1l = getDistance(L10.Value, R10.Value);
            double _5l = getDistance(L50.Value, R50.Value);
            double _9l = getDistance(L90.Value, R90.Value);

            GBResult5 = Chart.ptp.getGBAberration(_5l, _1l, 5);
            GBResult9 = Chart.ptp.getGBAberration(_9l, _1l, 9);

            double h = 0.05;
            if (!IsLongFocus)
            {
                h = 0.07;
            }
            if (Math.Abs(GBResult5) < h && Math.Abs(GBResult9) < h)
            {
                ChartTestHelper.setGBSign(true, GBControl);
            }
            else
            {
                ChartTestHelper.setGBSign(false, GBControl);
            }

            Result lp = new Result();
            lp.Name = "国标畸变测试0.5处";
            lp.Memo = "计算照片变形程度测算畸变";
            lp.Dimension = "";
            lp.Value = GBResult5;
            rl.Add(lp);

            lp = new Result();
            lp.Name = "国标畸变测试0.9处";
            lp.Memo = "计算照片变形程度测算畸变";
            lp.Dimension = "";
            lp.Value = GBResult9;
            rl.Add(lp);

            return rl;
        }
        double _GBResult5 = 0;
        public double GBResult5
        {
            get { return _GBResult5; }
            set { _GBResult5 = value; OnPropertyChanged("GBResult5"); }
        }
        double _GBResult9 = 0;
        public double GBResult9
        {
            get { return _GBResult9; }
            set { _GBResult9 = value; OnPropertyChanged("GBResult9"); }
        }
    }
}
