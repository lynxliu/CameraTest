using LFC.common;
using PhotoTestControl;
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
    public class JBEVTestViewModel : CommonChartTestViewModel
    {
        public JBEVTestViewModel()
        {
            TestChart = new GrayChart();
        }
        public Panel GBControl { get; set; }

        double _EVDistance = 0;
        public double EVDistance
        {
            get { return _EVDistance; }
            set { _EVDistance = value; OnPropertyChanged("EVDistance"); }
        }
        double _CenterL = 0;
        public double CenterL
        {
            get { return _CenterL; }
            set { _CenterL = value; OnPropertyChanged("CenterL"); }
        }
        double _gama = 1;
        public double gama
        {
            get { return _gama; }
            set { _gama = value; OnPropertyChanged("gama"); }
        }
        double _TestParameter = 3.322;
        public double TestParameter
        {
            get { return _TestParameter; }
            set { _TestParameter = value; OnPropertyChanged("TestParameter"); }
        }

        List<Result> TestGBEv(WriteableBitmap photo)
        {
            var rl = new List<Result>();
            TestChart.setChart(photo);
            EVDistance = (TestChart as GrayChart).ptp.getGBDEv(photo, gama, TestParameter);
            Result lp = new Result();
            lp.Name = "曝光量误差";
            lp.Memo = "计算照片中央明度来确定曝光量";
            lp.Dimension = "";
            lp.Value = EVDistance;
            rl.Add(lp);

            WriteableBitmap cb = (TestChart as GrayChart).ptp.getImageArea(photo, photo.PixelWidth / 4, photo.PixelHeight / 4, photo.PixelWidth / 2, photo.PixelHeight / 2);
            CenterL = (TestChart as GrayChart).ptp.getAverageColorL(cb);

            lp = new Result();
            lp.Name = "中央平均明度";
            lp.Memo = "照片中央明度";
            lp.Dimension = "";
            lp.Value = CenterL;
            rl.Add(lp);


            if (Math.Abs(EVDistance) >= 1)
            {
                ChartTestHelper.setGBSign(false, GBControl);
            }
            else
            {
                ChartTestHelper.setGBSign(true, GBControl);
            }
            return rl;
        }

        void Calculate(FlipView list)
        {
            var ChartPhoto = GetCurrentPhotoViewControl(list);
            if (ChartPhoto.IsSelect)
            {
                EVDistance = (TestChart as GrayChart).ptp.getDEv(ChartPhoto.getSelectArea(), gama, TestParameter);
                CenterL = (TestChart as GrayChart).ptp.getAverageColorL(ChartPhoto.getSelectArea());

                if (Math.Abs(EVDistance) >= 1)
                {
                    ChartTestHelper.setGBSign(false, GBControl);
                }
                else
                {
                    ChartTestHelper.setGBSign(true, GBControl);
                }
            }
            else
            {
                TestGBEv(CurrentPhoto);
            }

        }

        Rectangle TestArea = new Rectangle() { 
            Stroke=new SolidColorBrush(Colors.Red),
            StrokeThickness=3
        };

        public DelegateCommand<FlipView> SwitchSelectAreaVisibility
        {
            get
            {
                return new DelegateCommand<FlipView>((list) =>
                {
                    var control = GetCurrentPhotoViewControl(list);
                    var cvs = control.getDrawObjectCanvas();
                    if (cvs.Children.Contains(TestArea))
                        cvs.Children.Remove(TestArea);
                    else
                        cvs.Children.Add(TestArea);
                    Canvas.SetLeft(TestArea, cvs.ActualWidth / 4);
                    Canvas.SetTop(TestArea, cvs.ActualHeight / 4);
                    TestArea.Width = cvs.ActualWidth / 2;
                    TestArea.Height = cvs.ActualHeight / 2;
                });
            }
        }

        public DelegateCommand<FlipView> CalculateCommand
        {
            get
            {
                return new DelegateCommand<FlipView>((list) => { Calculate(list); });
            }
        }
    }
}
