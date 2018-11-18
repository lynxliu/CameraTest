using DCTestLibrary;
using LFC.common;
using PhotoTestControl.Models;
using SilverlightDCTestLibrary;
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
    public class KDGrayChartTestViewModel : CommonChartTestViewModel
    {
        public KDGrayChartTestViewModel()
        {
            TestChart = new KDGrayChart();
        }

        public Canvas DrawCanvas { get; set; }

        int _GradeDis = 7;
        public int GradeDis
        {
            get { return _GradeDis; }
            set { _GradeDis = value; OnPropertyChanged("GradeDis"); }
        }

        double _LatitudeValue = 0;
        public double LatitudeValue
        {
            get { return _LatitudeValue; }
            set { _LatitudeValue = value; OnPropertyChanged("LatitudeValue"); }
        }

        int _ActiveHeight = 0;
        public int ActiveHeight
        {
            get { return _ActiveHeight; }
            set { _ActiveHeight = value; OnPropertyChanged("ActiveHeight"); }
        }
        double _ActiveLatitudeValue = 0;
        public double ActiveLatitudeValue
        {
            get { return _ActiveLatitudeValue; }
            set { _ActiveLatitudeValue = value; OnPropertyChanged("ActiveLatitudeValue"); }
        }

        public List<Result> TestLatitude(WriteableBitmap photo)
        {
            var rl = new List<Result>();
            TestChart.setChart(photo);
            if (GradeDis == 0) { GradeDis = 7;  }
            LatitudeValue = (TestChart as KDGrayChart).getLatitude(GradeDis);
            DrawLatitude(photo);

            var lp = new Result();
            lp.Name = "宽容度";
            lp.Memo = "数据表示在一张照片里面可以分辨的亮度级别";
            lp.Dimension = "Grade";
            lp.Value = LatitudeValue;
            rl.Add(lp);
            return rl;
        }

        Line ReferenceLine = new Line()
        {
            Stroke = new SolidColorBrush(Colors.Red),
            StrokeThickness = 3
        };

        public DelegateCommand<FlipView> ActiveTestCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) => Active(list));
            }
        }

        void Active(FlipView list)
        {
            var control = GetCurrentPhotoViewControl(list);
            if (control == null) return;
            var layer = control.getDrawObjectCanvas();
            layer.PointerMoved -= layer_PointerMoved;
            layer.PointerPressed -= layer_PointerPressed;
            if (!layer.Children.Contains(ReferenceLine))
                layer.Children.Add(ReferenceLine);
            layer.PointerMoved += layer_PointerMoved;
            layer.PointerPressed += layer_PointerPressed;
        }

        void layer_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var layer = sender as Canvas;
            if (layer == null) return;
            if (layer.Children.Contains(ReferenceLine))
                layer.Children.Remove(ReferenceLine);
            layer.PointerMoved -= layer_PointerMoved;
            layer.PointerPressed -= layer_PointerPressed;

            ActiveHeight = Convert.ToInt32(
                e.GetCurrentPoint(layer).Position.Y/layer.ActualHeight*CurrentPhoto.PixelHeight
                );
            TestChart.setChart(CurrentPhoto);
            if (GradeDis == 0) { GradeDis = 7;  }
            ActiveLatitudeValue = (TestChart as KDGrayChart).getLatitude(GradeDis);
            DrawLatitude(CurrentPhoto);
        }

        void layer_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var layer = sender as Canvas;
            if (layer == null) return;
            ReferenceLine.X1 = 0;
            ReferenceLine.X2 = layer.ActualWidth;
            ReferenceLine.Y1 = ReferenceLine.Y2 = e.GetCurrentPoint(layer).Position.Y;
        }

        void DrawLatitude(WriteableBitmap photo)
        {
            DrawCanvas.Children.Clear();
            List<double> dl = (TestChart as KDGrayChart).getHLine(photo.PixelHeight / 2);
            DrawGraphic dg = new DrawGraphic(DrawCanvas);
            dg.DrawBrightHistogram(dl);
        }

    }
}
