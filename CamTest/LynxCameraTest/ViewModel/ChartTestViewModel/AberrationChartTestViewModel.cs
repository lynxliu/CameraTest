using LFC.common;
using LynxCameraTest.Model;
using PhotoTestControl.Models;
using SilverlightDCTestLibrary;
using SLPhotoTest.ChartTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace LynxCameraTest.ViewModel.ChartTestViewModel
{
    public class AberrationChartTestViewModel:CommonChartTestViewModel
    {
        public AberrationChartTestViewModel()
        {
            TestChart = new AberrationChart();
            Test = TestAberration;
        }
        double _AberrationValue = 0;
        public double AberrationValue
        {
            get { return _AberrationValue; }
            set { _AberrationValue = value; OnPropertyChanged("AberrationValue"); }
        }
        double _BottomBlackLineDistance = 0;
        public double BottomBlackLineDistance
        {
            get { return _BottomBlackLineDistance; }
            set { _BottomBlackLineDistance = value; OnPropertyChanged("BottomBlackLineDistance"); }
        }
        double _TopBlackLineDistance = 0;
        public double TopBlackLineDistance
        {
            get { return _TopBlackLineDistance; }
            set { _TopBlackLineDistance = value; OnPropertyChanged("TopBlackLineDistance"); }
        }
        double _MiddleBlackLineDistance = 0;
        public double MiddleBlackLineDistance
        {
            get { return _MiddleBlackLineDistance; }
            set { _MiddleBlackLineDistance = value; OnPropertyChanged("MiddleBlackLineDistance"); }
        }
        int _BottomBlackLineNumber = 0;
        public int BottomBlackLineNumber
        {
            get { return _BottomBlackLineNumber; }
            set { _BottomBlackLineNumber = value; OnPropertyChanged("BottomBlackLineNumber"); }
        }
        int _TopBlackLineNumber = 0;
        public int TopBlackLineNumber
        {
            get { return _TopBlackLineNumber; }
            set { _TopBlackLineNumber = value; OnPropertyChanged("TopBlackLineNumber"); }
        }
        int _MiddleBlackLineNumber = 0;
        public int MiddleBlackLineNumber
        {
            get { return _MiddleBlackLineNumber; }
            set { _MiddleBlackLineNumber = value; OnPropertyChanged("MiddleBlackLineNumber"); }
        }

        List<Result> TestAberration(WriteableBitmap chart)
        {
            var rl = new List<Result>();
            TestChart.setChart(chart);
            var Chart=TestChart as AberrationChart;
            AberrationValue = Chart.getAberration();

            rl.Add(new Result()
            {
                Value=BottomBlackLineDistance = Chart.getBottomBlackLinePix(),
                TestTime = DateTime.Now,
                Name="Bottom black line max distance",
                Memo="",
                TestCount = 1,
                Dimension = "Pix"
            });

            rl.Add(new Result()
            {
                Value = BottomBlackLineNumber = Chart.getBottomBlackLineNum(),
                TestTime = DateTime.Now,
                Name = "Bottom black line number",
                Memo = "",
                TestCount = 1,
                Dimension = ""

            });

            rl.Add(new Result()
            {
                Value = TopBlackLineDistance = Chart.getTopBlackLinePix(),
                TestTime = DateTime.Now,
                Name = "Top black line max distance",
                Memo = "",
                TestCount = 1,
                Dimension = "Pix"
            });

            rl.Add(new Result()
            {
                Value = TopBlackLineNumber = Chart.getTopBlackLineNum(),
                TestTime = DateTime.Now,
                Name = "Top black line number",
                Memo = "",
                TestCount = 1,
                Dimension = ""

            });

            rl.Add(new Result()
            {
                Value = MiddleBlackLineDistance = Chart.getCenterBlackLinePix(),
                TestTime = DateTime.Now,
                Name = "Center black line max distance",
                Memo = "",
                TestCount = 1,
                Dimension = "Pix"
            });

            rl.Add(new Result()
            {
                Value = MiddleBlackLineNumber = Chart.getCenterBlackLineNum(),
                TestTime = DateTime.Now,
                Name = "Bottom black line number",
                Memo = "",
                TestCount = 1,
                Dimension = ""

            });


            rl.Add(new Result()
            {
                Value =AberrationValue= Chart.getAberration(),
                Name = "Aberration percent",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "%"
            });

            return rl;
        }


        public DelegateCommand ShowJBAberationTestCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    JBAberrationTest w = new JBAberrationTest();
                    JBAberrationTestViewModel vm = new JBAberrationTestViewModel();
                    w.DataContext = vm;
                    if (CurrentPhoto != null)
                        vm.PhotoList.Add(CurrentPhoto);
                    CommonHelper.Show(w);
                });
            }
        }

        public DelegateCommand<FlipView>  ClearCommand//删除各种辅助线
        {
            get
            {
                return new DelegateCommand<FlipView>((l) =>
                {
                    var control = GetCurrentPhotoViewControl(l);
                    if (control == null) return;
                    control.Clear();
                });
            }
        }

        Brush _CenterLeftButtonBrush=DeactiveBrush;
        public Brush CenterLeftButtonBrush
        {
            get { return _CenterLeftButtonBrush; }
            set { _CenterLeftButtonBrush = value; OnPropertyChanged("CenterLeftButtonBrush"); }
        }
        Brush _CenterRightButtonBrush = DeactiveBrush;
        public Brush CenterRightButtonBrush
        {
            get { return _CenterRightButtonBrush; }
            set { _CenterRightButtonBrush = value; OnPropertyChanged("CenterRightButtonBrush"); }
        }
        Brush _BorderLeftButtonBrush = DeactiveBrush;
        public Brush BorderLeftButtonBrush
        {
            get { return _BorderLeftButtonBrush; }
            set { _BorderLeftButtonBrush = value; OnPropertyChanged("BorderLeftButtonBrush"); }
        }
        Brush _BorderRightButtonBrush = DeactiveBrush;
        public Brush BorderRightButtonBrush
        {
            get { return _BorderRightButtonBrush; }
            set { _BorderRightButtonBrush = value; OnPropertyChanged("BorderRightButtonBrush"); }
        }
        static Brush ActiveBrush = new SolidColorBrush(Colors.Red);
        static Brush DeactiveBrush = new SolidColorBrush(Colors.Blue);

        Line CenterLeft = new Line() { Stroke = new SolidColorBrush(Colors.Red),StrokeThickness=3 };
        Line CenterRight = new Line() { Stroke = new SolidColorBrush(Colors.Red), StrokeThickness = 3 };
        Line LeftBorder = new Line() { Stroke = new SolidColorBrush(Colors.Blue), StrokeThickness = 3 };
        Line RightBorder = new Line() { Stroke = new SolidColorBrush(Colors.Blue), StrokeThickness = 3 };

        Line CurrentLine = null;
        void CurrentLinePointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (CurrentLine == null) return;
            var layer = sender as Canvas;
            if (layer == null) return;
            CurrentLine.Y1 = 0;
            CurrentLine.Y2 = layer.ActualHeight;
            CurrentLine.X1 = CurrentLine.X2 = e.GetCurrentPoint(layer).Position.X;
        }
        void CurrentLinePointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var layer = sender as Canvas;
            if (layer == null) return;
            ClearActive(layer);
        }
        void ClearActive(Canvas layer)
        {
            CurrentLine = null;
            CenterLeftButtonBrush = DeactiveBrush;
            CenterRightButtonBrush = DeactiveBrush;
            BorderLeftButtonBrush = DeactiveBrush;
            BorderRightButtonBrush = DeactiveBrush;
            layer.PointerMoved -= CurrentLinePointerMoved;
            layer.PointerPressed -= CurrentLinePointerPressed;
        }
        void StratReferenceLine(FlipView list)
        {
            var control = GetCurrentPhotoViewControl(list);
            if (control == null) return;
            var layer = control.getDrawObjectCanvas();
            ClearActive(layer);
            if (!layer.Children.Contains(CurrentLine))
            {
                layer.Children.Add(CurrentLine);
            }
            layer.PointerMoved += CurrentLinePointerMoved;
            layer.PointerPressed += CurrentLinePointerPressed;


        }
        public DelegateCommand<FlipView> ActiveCenterRightCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((c) =>
                {
                    CurrentLine = CenterRight;
                    CenterRightButtonBrush = ActiveBrush;
                    StratReferenceLine(c);
                });
            }
        }


        public DelegateCommand<FlipView> ActiveCenterLeftCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((c) =>
                {
                    CurrentLine = CenterLeft;
                    CenterLeftButtonBrush = ActiveBrush;
                    StratReferenceLine(c);
                });
            }
        }

        public DelegateCommand<FlipView> ActiveLeftBorderLineCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((c) =>
                {
                    CurrentLine = LeftBorder;
                    BorderLeftButtonBrush = ActiveBrush;
                    StratReferenceLine(c);
                });
            }
        }

        public DelegateCommand<FlipView> ActiveRightBorderLineCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((c) =>
                {
                    CurrentLine = LeftBorder;
                    BorderRightButtonBrush = ActiveBrush;
                    StratReferenceLine(c);
                });
            }
        }

        double _ActiveResult = 0;
        public double ActiveResult
        {
            get { return _ActiveResult; }
            set { _ActiveResult = value; OnPropertyChanged("ActiveResult"); }
        }
        double _ActiveCenterDistance = 0;
        public double ActiveCenterDistance
        {
            get { return _ActiveCenterDistance; }
            set { _ActiveCenterDistance = value; OnPropertyChanged("ActiveCenterDistance"); }
        }
        double _ActiveBorderDistance = 0;
        public double ActiveBorderDistance
        {
            get { return _ActiveBorderDistance; }
            set { _ActiveBorderDistance = value; OnPropertyChanged("ActiveBorderDistance"); }
        }
        
        public DelegateCommand CustomeCalculateCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ActiveBorderDistance=RightBorder.X1 - LeftBorder.X1;
                    ActiveCenterDistance = CenterRight.X1 - CenterLeft.X1;
                    ActiveResult = (ActiveCenterDistance - ActiveBorderDistance) / (ActiveCenterDistance);
                });
            }
        }
    }
}
