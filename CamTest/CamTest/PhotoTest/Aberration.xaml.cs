using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;
using SilverlightLynxControls;

using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Foundation;

namespace SLPhotoTest.PhotoTest
{
    public partial class Aberration : UserControl
    {
        public Aberration()
        {
            InitializeComponent();
            am = new ActionMove(this, Title);
            //am1 = new ActionMove(PhotoClip, image);
            photoTestToolbar1.setTarget(LChartPhoto);
            photoTestToolbar1.autoTest = TestImage;
            photoTestToolbar1.addPhoto = AddPhoto;
            photoTestToolbar1.removePhoto = RemovePhoto;

            dg = new DrawGraphic(LChartPhoto.getDrawObjectCanvas());
        }
        ActionMove am;
        public string TestWay;
        XMarkChart xt = new XMarkChart();
        DrawGraphic dg;
        void TestImage()
        {
            WriteableBitmap b = LChartPhoto.getPhoto();
            if (b == null) { SilverlightLFC.common.Environment.ShowMessage("没有要测试的照片"); return; }
            decimal d=0m;
            try
            {
                d = xt.getAberration(b);
            }
            catch (Exception xe)//未知的异常
            {
                if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
                {
                    SilverlightLFC.common.Environment.ShowMessage(xe.Message);
                }
                else
                {
                    SilverlightLFC.common.Environment.ShowMessage("广角端畸变测试错误");
                }
            }
            try{
                double leftCirclePoint = Convert.ToDouble(xt.ProcessInfor["AberrationLCP"]);
                double rightCirclePoint = Convert.ToDouble(xt.ProcessInfor["AberrationRCP"]);
                double lv = Convert.ToDouble(xt.ProcessInfor["AberrationLVP"]);
                double rv = Convert.ToDouble(xt.ProcessInfor["AberrationRVP"]);
                leftCirclePoint = leftCirclePoint / b.PixelWidth;
                rightCirclePoint = rightCirclePoint / b.PixelWidth;
                lv = lv / b.PixelWidth;
                rv = rv / b.PixelWidth;

                dg.DrawLine(leftCirclePoint * LChartPhoto.Width, 0, leftCirclePoint * LChartPhoto.Width, LChartPhoto.Height, false, 3, new SolidColorBrush(Colors.Green));
                dg.DrawLine(rightCirclePoint * LChartPhoto.Width, 0, rightCirclePoint * LChartPhoto.Width, LChartPhoto.Height, false, 3, new SolidColorBrush(Colors.Green));
                dg.DrawLine(lv * LChartPhoto.Width, 0, lv * LChartPhoto.Width, LChartPhoto.Height, false, 3, new SolidColorBrush(Colors.Red));
                dg.DrawLine(rv * LChartPhoto.Width, 0, rv * LChartPhoto.Width, LChartPhoto.Height, false, 3, new SolidColorBrush(Colors.Red));

                textBlockAberration.Text = d.ToString();
                textBlockStandardLong.Text = (rightCirclePoint - leftCirclePoint).ToString();
                textBlockTrueLong.Text = (rv - lv).ToString();
            }
            catch (Exception xe)//未知的异常
            {
                if (xe is LFCException)//已经是系统约定的错误类型，直接往上抛
                {
                    SilverlightLFC.common.Environment.ShowMessage(xe.Message);
                }
                else
                {
                    SilverlightLFC.common.Environment.ShowMessage("广角端畸变测试错误");
                }
            }
        }

        public void Test(WriteableBitmap b)
        {
            if (b == null) { return; }
            LChartPhoto.setPhoto(b);
            TestImage();
        }
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            LChartPhoto.Clear();
            photoTestToolbar1 = null;
            dg = null;
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonVLSelect_Click(object sender, RoutedEventArgs e)
        {
            Image im = LChartPhoto.getImage();
            im.PointerPressed += ProcPhoto_PointerPressed;
        }
        Line selectLine = new Line();
        void ProcPhoto_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Image im = LChartPhoto.getImage();
            im.PointerPressed -= ProcPhoto_PointerPressed;
            if (LChartPhoto.getPhoto() == null) { return; }
            Canvas dc = LChartPhoto.getDrawObjectCanvas();
            if (!dc.Children.Contains(selectLine))
            {
                dc.Children.Add(selectLine);
            }
            Point p = e.GetCurrentPoint(im).Position;
            Point? ip = xt.PointToPix(LChartPhoto.getPhoto(), LChartPhoto.getDrawObjectCanvas(), p, false);
            if (ip == null) { return; }
            selectLine.X1 = selectLine.X2 = p.X;
            selectLine.Y1 = 0;
            selectLine.Y2 = LChartPhoto.getDrawObjectCanvas().ActualHeight;
            selectLine.Stroke = new SolidColorBrush(Colors.Blue);
            selectLine.StrokeThickness = 3;
            textBlockSelectPosition.Text = ip.Value.X.ToString();
            ToolTipService.SetToolTip(textBlockSelectPosition, "总宽像素：" + LChartPhoto.getPhoto().PixelWidth.ToString());
        }

        public void AddPhoto(WriteableBitmap photo)
        {
            LChartPhoto.setPhoto(photo);

        }

        public void RemovePhoto(WriteableBitmap photo)
        {
            LChartPhoto.Clear();
        }

    }
}
