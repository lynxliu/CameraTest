using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;

using SilverlightLynxControls;
using SilverlightLFC.common;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace SLPhotoTest.PhotoTest
{
    public partial class Dispersiveness : UserControl
    {
        public Dispersiveness()
        {
            InitializeComponent();
            am = new ActionMove(this, Title);
            photoTestToolbar1.setTarget(lChartPhoto1);
            photoTestToolbar1.autoTest = ParameterAutoTest;
            photoTestToolbar1.addPhoto = AddPhoto;
            photoTestToolbar1.removePhoto = RemovePhoto;
        }
        ActionMove am;
        public bool IsV = true;
        WriteableBitmap sb;
        PhotoTestParameter ptp = new PhotoTestParameter();
        public void Test(WriteableBitmap b)
        {
            if (b == null) { return; }
            lChartPhoto1.setPhoto(b);
            sb = b;
            lChartPhoto1.getImage().PointerPressed += (image_PointerPressed);
            int p = 0;
            if (IsV)
            {
                p=b.PixelHeight/2;
            }
            else
            {
                p = b.PixelWidth / 2;
            }
            DrawCurve(p);
        }

        public void DrawCurve(int p)
        {
            if (sb == null) { return; }
            canvasCurve.Children.Clear();
            DrawGraphic dg = new DrawGraphic(canvasCurve);
            List<List<int>> al = new List<List<int>>();

            try
            {
                if (IsV)
                {
                    al = ptp.getCurveVDispersiveness(sb, p);
                }
                else
                {
                    al = ptp.getCurveHDispersiveness(sb, p);
                }
                dg.DrawX();
                List<double> MarkList = new List<double>();
                for (int i = 0; i < 101; i = i + 20)
                {
                    MarkList.Add(i);
                }
                dg.DrawXMark(MarkList);
                MarkList.Clear();
                for (int i = 0; i < 255; i = i + 40)
                {
                    MarkList.Add(i);

                }
                dg.DrawY();
                dg.DrawYMark(MarkList);

                List<double> ral, gal, bal;
                ral = SilverlightLFC.common.Environment.getDoubleList<int>(al[0]);
                gal = SilverlightLFC.common.Environment.getDoubleList<int>(al[1]);
                bal = SilverlightLFC.common.Environment.getDoubleList<int>(al[2]);
                dg.ForeColor = Colors.Blue;
                dg.DrawLines(bal);
                dg.ForeColor = Colors.Red;
                dg.DrawLines(ral);
                dg.ForeColor = Colors.Green;
                dg.DrawLines(gal);
                dg.DrawTitle("色散");
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

        void image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            DrawGraphic dg = new DrawGraphic();
            Point? p = ptp.PointToPix(sb, lChartPhoto1.getImage(), e.GetCurrentPoint(lChartPhoto1.getImage()).Position, false);
            if (p == null) { return; }
            Color c = ptp.GetPixel(sb, Convert.ToInt32(p.Value.X), Convert.ToInt32(p.Value.Y));
            textBlockB.Text = c.B.ToString();
            textBlockG.Text = c.G.ToString();
            textBlockR.Text = c.R.ToString();

            SelectLine.Stroke = new SolidColorBrush(Colors.Red);
            SelectLine.StrokeThickness = 3;
            if (IsV)
            {
                SelectLine.X1 = 0;
                SelectLine.X2 = lChartPhoto1.getImage().Width;
                SelectLine.Y1 = e.GetCurrentPoint(lChartPhoto1.getImage()).Position.Y;
                SelectLine.Y2 = e.GetCurrentPoint(lChartPhoto1.getImage()).Position.Y;
            }
            else
            {
                SelectLine.X1 = e.GetCurrentPoint(lChartPhoto1.getImage()).Position.X;
                SelectLine.X2 = e.GetCurrentPoint(lChartPhoto1.getImage()).Position.X;
                SelectLine.Y1 = 0;
                SelectLine.Y2 = lChartPhoto1.getImage().Height;
            }
            if (lChartPhoto1.getDrawObjectCanvas().Children.Contains(SelectLine)) { }
            else
            {
                lChartPhoto1.getDrawObjectCanvas().Children.Add(SelectLine);
            }
            int tp = 0;
            if (IsV)
            {
                tp = Convert.ToInt32(p.Value.Y);
            }
            else
            {
                tp = Convert.ToInt32(p.Value.X);
            }
            DrawCurve(tp);
        }
        Line SelectLine = new Line();

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            sb = null;
            photoTestToolbar1 = null;
            lChartPhoto1.Clear();
            canvasCurve.Children.Clear();
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            if (sb == null) { return; }
            int p = 0;
            if (IsV)
            {
                p = sb.PixelHeight / 2;
            }
            else
            {
                p = sb.PixelWidth / 2;
            }
            DrawCurve(p);
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ParameterAutoTest()
        {
            Test(lChartPhoto1.getPhoto());
        }

        public void AddPhoto(WriteableBitmap photo)
        {
            lChartPhoto1.setPhoto(photo);
        }

        public void RemovePhoto(WriteableBitmap photo)
        {
            lChartPhoto1.Clear();
        }

    }
}
