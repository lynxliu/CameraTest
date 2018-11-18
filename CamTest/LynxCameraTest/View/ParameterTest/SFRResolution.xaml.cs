using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using DCTestLibrary;
using SilverlightLynxControls;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Input;
using LynxCameraTest.ViewModel;
using LynxCameraTest.Model;

namespace SLPhotoTest.PhotoTest
{
    public partial class SFRResolution : UserControl, IParameterTestView
    {
        public SFRResolution()
        {
            InitializeComponent();
            am = new ActionMove(this, this);
        }
        ActionMove am;
        public bool IsV = true;

        PhotoTestParameter ptp = new PhotoTestParameter();
        public void Test(List<WriteableBitmap> bList)
        {
            if ( bList == null||bList.Count==0) { return; }
            var b = bList.FirstOrDefault();
            lChartPhoto1.Photo =(b);
            lChartPhoto1.PointerPressed += (image_MouseLeftButtonDown);
            DrawGraphic dg = new DrawGraphic(SFRCanvas);
            DrawGraphic edg = new DrawGraphic(EdgeBrightCanvas);
            DrawGraphic ldg = new DrawGraphic(LSFCanvas);
            List<double> al = new List<double>();
            List<double> EdgeBrightList;
            List<double> LSF;

            try
            {
                ptp.setEdgeResoveStopFrequency(lynxUpDown1.IntValue);//只要测试就加载参数
                if (IsV)
                {
                    EdgeBrightList = SilverlightLFC.common.Environment.getDoubleList<int>(ptp.getImageGrayHLine(b, b.PixelHeight / 2));
                    LSF = SilverlightLFC.common.Environment.getDoubleList<decimal>(ptp.getDdx(EdgeBrightList));
                    al = ptp.getCurveMTF(b, false);
                }
                else
                {
                    EdgeBrightList = SilverlightLFC.common.Environment.getDoubleList<int>(ptp.getImageGrayVLine(b, b.PixelWidth / 2));
                    LSF = SilverlightLFC.common.Environment.getDoubleList<decimal>(ptp.getDdx(EdgeBrightList));

                    al = ptp.getCurveMTF(b, true);
                }
                edg.DrawLines(EdgeBrightList);
                ldg.DrawLines(LSF);

                dg.DrawX();
                dg.DrawY();
                List<double> dlist = new List<double>();
                dlist.Add(0.0);
                dlist.Add(0.5);
                dlist.Add(1.0);
                dg.DrawYMark(dlist);
                dg.DrawXMark(dlist);
                dg.DrawTitle("MTF 曲线图");
                dg.DrawLines(al);
                textBlockResovLines.Text = ptp.getEdgeResoveEffect(b, !IsV).ToString();
            }
            catch (Exception xe)//未知的异常
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
        }
        Line SelectLine=new Line();
        void image_MouseLeftButtonDown(object sender, PointerRoutedEventArgs e)
        {
            try
            {
                if (SelectLine == null)
                {
                    SelectLine = new Line();
                    SelectLine.StrokeThickness = 3;
                    SelectLine.Stroke = new SolidColorBrush(Colors.Green);
                    if (!lChartPhoto1.getDrawObjectCanvas().Children.Contains(SelectLine))
                    {
                        lChartPhoto1.getDrawObjectCanvas().Children.Add(SelectLine);
                    }
                }
                if (IsV)
                {
                    SelectLine.X1 = 0;
                    SelectLine.X2 = lChartPhoto1.Width;
                    SelectLine.Y1 = e.GetCurrentPoint(lChartPhoto1).Position.Y;
                    SelectLine.Y2 = e.GetCurrentPoint(lChartPhoto1).Position.Y;
                }
                else
                {
                    SelectLine.X1 = e.GetCurrentPoint(lChartPhoto1).Position.X;
                    SelectLine.X2 = e.GetCurrentPoint(lChartPhoto1).Position.X;
                    SelectLine.Y1 = 0;
                    SelectLine.Y2 = lChartPhoto1.Height;
                }
                DrawGraphic dg = new DrawGraphic();
                Point? p = ptp.PointToPix((lChartPhoto1.Photo), (lChartPhoto1.getImage()), e.GetCurrentPoint(lChartPhoto1.getImage()).Position, true);
                if (p == null) { return; }
                double pp;
                if (IsV)
                {
                    pp = p.Value.Y / (lChartPhoto1.Photo).PixelHeight;
                }
                else
                {
                    pp = p.Value.X / (lChartPhoto1.Photo).PixelWidth;
                }

                //int s = ptp.getEdgeResoveLines(cb, !IsV, cb.PixelHeight);
                this.textBlockResovLines.Text = ptp.getEdgeResoveEffect(lChartPhoto1.getPhoto(), !IsV, pp).ToString();
                DrawGraphic sdg = new DrawGraphic(SFRCanvas);
                sdg.ForeColor = Colors.Green;
                DrawGraphic edg = new DrawGraphic(EdgeBrightCanvas);
                edg.ForeColor = Colors.Green;
                DrawGraphic ldg = new DrawGraphic(LSFCanvas);
                ldg.ForeColor = Colors.Green;
                List<double> al = new List<double>();
                List<double> EdgeBrightList;
                List<double> LSF;

                ptp.setEdgeResoveStopFrequency(lynxUpDown1.IntValue);
                if (IsV)
                {
                    EdgeBrightList = SilverlightLFC.common.Environment.getDoubleList<int>(ptp.getImageGrayHLine(lChartPhoto1.getPhoto(), Convert.ToInt32((lChartPhoto1.getPhoto()).PixelHeight * pp)));
                    LSF = SilverlightLFC.common.Environment.getDoubleList<decimal>(ptp.getDdx(EdgeBrightList));
                    al = ptp.getCurveMTF(lChartPhoto1.Photo, false, pp);
                }
                else
                {
                    EdgeBrightList = SilverlightLFC.common.Environment.getDoubleList<int>(ptp.getImageGrayVLine(lChartPhoto1.getPhoto(), Convert.ToInt32((lChartPhoto1.getPhoto()).PixelWidth * pp)));
                    LSF = SilverlightLFC.common.Environment.getDoubleList<decimal>(ptp.getDdx(EdgeBrightList));

                    al = ptp.getCurveMTF(lChartPhoto1.Photo , true, pp);
                }
                edg.DrawLines(EdgeBrightList);
                ldg.DrawLines(LSF);


                sdg.DrawLines(al);
            }
            catch (Exception xe)//未知的异常
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
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            lChartPhoto1.Clear();
            EdgeBrightCanvas.Children.Clear();
            LSFCanvas.Children.Clear();
            SFRCanvas.Children.Clear();
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            lChartPhoto1.Clear();
            if (SelectLine != null)
            {
                //canvasImage.Children.Remove(SelectLine);
                SelectLine = null;
            }
            EdgeBrightCanvas.Children.Clear();
            LSFCanvas.Children.Clear();
            SFRCanvas.Children.Clear();
        }

        private void buttonStandard_Click(object sender, RoutedEventArgs e)
        {
            lynxUpDown1.DoubleValue = 50;
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

        }
        public void ParameterAutoTest()
        {
            WriteableBitmap tb = lChartPhoto1.getPhoto();
            if (tb == null) { return; }
            Test(new List<WriteableBitmap>(){tb});
        }


        public void AddPhoto(WriteableBitmap photo)
        {
            lChartPhoto1.Photo = (photo);
        }

        public void RemovePhoto(WriteableBitmap photo)
        {
            lChartPhoto1.Photo = null;
        }


        public void InitViewModel()
        {
            if (DataContext == null) return;
            var vm = DataContext as CommonToolViewModel;
            if (vm == null) return;
            vm.Title = "SFR Resolution Parameter";
            vm.TestAction = ParameterAutoTest;
        }
    }
}
