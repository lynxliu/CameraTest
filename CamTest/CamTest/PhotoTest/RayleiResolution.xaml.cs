using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using SilverlightDCTestLibrary;
using SilverlightLynxControls;
using DCTestLibrary;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;

namespace SLPhotoTest.PhotoTest
{
    public partial class RayleiResolution : UserControl
    {
        public string TestWay = "ISO12233";
        public RayleiResolution()
        {
            InitializeComponent();
            am = new ActionMove(this, Title);
            photoTestToolbar1.setTarget(lChartPhoto1);
            photoTestToolbar1.autoTest = ParameterAutoTest;
            photoTestToolbar1.addPhoto = AddPhoto;
            photoTestToolbar1.removePhoto = RemovePhoto;

            ll.Fill = ll.Stroke = new SolidColorBrush(Colors.Blue);
            rl.Fill = rl.Stroke = new SolidColorBrush(Colors.Blue);
            pl.Fill = pl.Stroke = new SolidColorBrush(Colors.Red);
            ll.StrokeThickness = 3;
            rl.StrokeThickness = 3;
            pl.StrokeThickness = 3;
            ToolTipService.SetToolTip(ll, "左边界");
            ToolTipService.SetToolTip(rl, "右边界");
            ToolTipService.SetToolTip(pl, "自动极限");

            Selectl.Stroke=Selectl.Fill = new SolidColorBrush(Colors.Orange);
            Selectl.StrokeThickness = 5;
        }
        ActionMove am;
        public bool IsLeft = true;
        WriteableBitmap sb;
        ISO12233ExChart isoc = new ISO12233ExChart();
        public void Test(WriteableBitmap b)
        {
            if ( b == null) { return; }
            
            sb = SilverlightLFC.common.WriteableBitmapHelper.Clone(b);
            if (!IsLeft)
            {
                b = isoc.FlipYImage(b);
            }

            lChartPhoto1.setPhoto(b);
            try
            {
                long l = isoc.getResoveLines(sb, 0.735f, IsLeft);
                if (IsLeft)
                {
                    textBlockIsLeft.Text = "左";
                }
                else
                {
                    textBlockIsLeft.Text = "右";
                }

                testResult.Text = l.ToString() + "LP";

                BorderLeft = Convert.ToDouble(isoc.ptp.ProcessInfor["RayleiResolutionLeftBorder"]);
                BorderRight = Convert.ToDouble(isoc.ptp.ProcessInfor["RayleiResolutionRightBorder"]);
                double p = Convert.ToDouble(isoc.ptp.ProcessInfor["RayleiResolutionRightPosition"]);



                string st = isoc.ptp.ProcessInfor["ISOCardType"].ToString();
                if (st == "100-600")
                {
                    comboBox1.SelectedIndex = 2;
                }
                if (st == "500-2000")
                {
                    comboBox1.SelectedIndex = 0;
                }
                if (st == "1000-4000")
                {
                    comboBox1.SelectedIndex = 1;
                }

                leftBorder.Text = BorderLeft.ToString();
                rightBorder.Text = BorderRight.ToString();
                RayleP.Text = p.ToString();

                ll.Y1 = 0;
                ll.Y2 = lChartPhoto1.Height;
                ll.X1 = BorderLeft / b.PixelWidth * lChartPhoto1.Width;
                ll.X2 = ll.X1;
                if (!lChartPhoto1.getDrawObjectCanvas().Children.Contains(ll))
                {
                    lChartPhoto1.getDrawObjectCanvas().Children.Add(ll);
                }

                rl.Y1 = 0;
                rl.Y2 = lChartPhoto1.Height;
                rl.X1 = BorderRight / b.PixelWidth * lChartPhoto1.Width;
                rl.X2 = rl.X1; 
                if (!lChartPhoto1.getDrawObjectCanvas().Children.Contains(rl))
                {
                    lChartPhoto1.getDrawObjectCanvas().Children.Add(rl);
                }

                pl.Y1 = 0;
                pl.Y2 = lChartPhoto1.Height;
                pl.X1 = p / b.PixelWidth * lChartPhoto1.Width;
                pl.X2 = pl.X1; 
                if (!lChartPhoto1.getDrawObjectCanvas().Children.Contains(pl))
                {
                    lChartPhoto1.getDrawObjectCanvas().Children.Add(pl);
                }

                canvasBright.Children.Clear();
                DrawGraphic dg = new DrawGraphic(canvasBright);
                dg.DrawBrightLines(isoc.getImageGrayVLine(b, Convert.ToInt64(p)));
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
        Line Selectl = new Line();
        Line ll = new Line(), rl = new Line(), pl = new Line();
        private void image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lChartPhoto1.getDrawObjectCanvas().PointerMoved -= new PointerEventHandler(image_PointerMoved);
            lChartPhoto1.getDrawObjectCanvas().PointerPressed -= new PointerEventHandler(image_PointerPressed);
            Select.Foreground = Disactive;

            WriteableBitmap b = lChartPhoto1.getPhoto();
            if (b == null) { return; }
            int p = Convert.ToInt32(e.GetCurrentPoint(lChartPhoto1.getDrawObjectCanvas()).Position.X / lChartPhoto1.getDrawObjectCanvas().ActualWidth * (b).PixelWidth);
            DrawGraphic dg = new DrawGraphic(canvasBright);
            ISO12233ExChart isoc = new ISO12233ExChart();
            canvasBright.Children.Clear();
            List<int> al = isoc.getImageGrayVLine(b, p);
            dg.DrawBrightLines(al);
            if (!lChartPhoto1.getDrawObjectCanvas().Children.Contains(Selectl))
            {
                lChartPhoto1.getDrawObjectCanvas().Children.Add(Selectl);
                Selectl.Fill = new SolidColorBrush(Colors.Green);
                Selectl.StrokeThickness = 3;
                Selectl.Stroke = Selectl.Fill;
            }
            PhotoTestParameter ptp=new PhotoTestParameter();
            textBlockCurrentPercent.Text = ptp.getContrast(al).ToString();
            
        }

        private void imageSetLeft_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lChartPhoto1.getDrawObjectCanvas().PointerMoved -= new PointerEventHandler(image_PointerMoved);
            lChartPhoto1.getDrawObjectCanvas().PointerPressed -= new PointerEventHandler(imageSetLeft_PointerPressed);
            setLeft.Foreground = Disactive;

            WriteableBitmap b = lChartPhoto1.getPhoto();
            if (b == null) { return; }
            int p = Convert.ToInt32(e.GetCurrentPoint(lChartPhoto1.getDrawObjectCanvas()).Position.X / lChartPhoto1.getDrawObjectCanvas().ActualWidth * (b).PixelWidth);

            if (!lChartPhoto1.getDrawObjectCanvas().Children.Contains(ll))
            {
                lChartPhoto1.getDrawObjectCanvas().Children.Add(ll);
            }
            leftBorder.Text = p.ToString();
            BorderLeft = p;
            ll.X1=ll.X2 = e.GetCurrentPoint(lChartPhoto1.getDrawObjectCanvas()).Position.X;
            ll.Y1 = 0;
            ll.Y2 = lChartPhoto1.Height;
        }
        double BorderLeft, BorderRight;
        Brush Disactive = new SolidColorBrush(Colors.Black);
        Brush Active=new SolidColorBrush(Colors.Red);
        private void imageSetRight_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lChartPhoto1.getDrawObjectCanvas().PointerMoved -= new PointerEventHandler(image_PointerMoved);
            lChartPhoto1.getDrawObjectCanvas().PointerPressed -= new PointerEventHandler(imageSetRight_PointerPressed);

            setRight.Foreground = Disactive;

            WriteableBitmap b = lChartPhoto1.getPhoto();
            if (b == null) { return; }
            int p = Convert.ToInt32(e.GetCurrentPoint(lChartPhoto1.getDrawObjectCanvas()).Position.X / lChartPhoto1.getDrawObjectCanvas().ActualWidth * (b).PixelWidth);

            if (!lChartPhoto1.getDrawObjectCanvas().Children.Contains(rl))
            {
                lChartPhoto1.getDrawObjectCanvas().Children.Add(rl);
            }
            rightBorder.Text = p.ToString();
            BorderRight = p;
            rl.X1 = rl.X2 = e.GetCurrentPoint(lChartPhoto1.getDrawObjectCanvas()).Position.X;
            rl.Y1 = 0;
            rl.Y2 = lChartPhoto1.Height;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            sb = null;
            canvasBright.Children.Clear();
            lChartPhoto1.Clear();
            photoTestToolbar1 = null;
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lChartPhoto1.getPhoto() == null) { return; }

            long l = isoc.getResoveLines(sb, Convert.ToSingle(lynxUpDown1.DoubleValue/100), IsLeft);
            testResult.Text = l.ToString() + "LP";
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonFlip_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap t = lChartPhoto1.getPhoto();
            if (t == null) { return; }
            lChartPhoto1.setPhoto(isoc.FlipYImage(t));
        }

        public void ParameterAutoTest()
        {
            Test(sb);
        }

        public void AddPhoto(WriteableBitmap photo)
        {
            sb = photo;
            lChartPhoto1.setPhoto(photo);
        }

        public void RemovePhoto(WriteableBitmap photo)
        {
            lChartPhoto1.Clear();

        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            lChartPhoto1.getDrawObjectCanvas().PointerMoved += new PointerEventHandler(image_PointerMoved);
            lChartPhoto1.getDrawObjectCanvas().PointerPressed += new PointerEventHandler(image_PointerPressed);
            Select.Foreground = Active;
            MoveLine = Selectl;
        }

        void image_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!lChartPhoto1.getDrawObjectCanvas().Children.Contains(Selectl))
            {
                lChartPhoto1.getDrawObjectCanvas().Children.Add(Selectl);
            }
            WriteableBitmap b = lChartPhoto1.getPhoto();
            if (b == null) { return; }
            int p = Convert.ToInt32(e.GetCurrentPoint(lChartPhoto1.getDrawObjectCanvas()).Position.X / lChartPhoto1.getDrawObjectCanvas().ActualWidth * (b).PixelWidth);


            SelectP.Text = p.ToString();
            int v=0;
            double tv = (p - BorderLeft) / (BorderRight - BorderLeft);
            if(comboBox1.SelectedIndex==0)
            {
                v = Convert.ToInt32( tv* 1500 + 500);

            }
            if (comboBox1.SelectedIndex == 1)
            {
                v = Convert.ToInt32(tv * 3000 + 1000);

            }
            if (comboBox1.SelectedIndex == 2)
            {
                v = Convert.ToInt32(tv * 500+100 );

            }
            testResult.Text = v.ToString();
            if (MoveLine == null)
            {
                MoveLine = Selectl;
            }
            MoveLine.X1 = e.GetCurrentPoint(lChartPhoto1.getDrawObjectCanvas()).Position.X;
            MoveLine.X2 = e.GetCurrentPoint(lChartPhoto1.getDrawObjectCanvas()).Position.X;
            MoveLine.Y1 = 0;
            MoveLine.Y2 = lChartPhoto1.Height;
          
        }

        private void setLeft_Click(object sender, RoutedEventArgs e)
        {
            lChartPhoto1.getDrawObjectCanvas().PointerMoved += new PointerEventHandler(image_PointerMoved);
            lChartPhoto1.getDrawObjectCanvas().PointerPressed += new PointerEventHandler(imageSetLeft_PointerPressed);
            setLeft.Foreground = Active;
            MoveLine = ll;
        }

        private void setRight_Click(object sender, RoutedEventArgs e)
        {
            lChartPhoto1.getDrawObjectCanvas().PointerMoved += new PointerEventHandler(image_PointerMoved);
            lChartPhoto1.getDrawObjectCanvas().PointerPressed += new PointerEventHandler(imageSetRight_PointerPressed);
            setRight.Foreground = Active;
            MoveLine = rl;
        }
        Line MoveLine = null;
        PhotoTestParameter ptp = new PhotoTestParameter();
        private void RaleyPosition_Click(object sender, RoutedEventArgs e)
        {
            
            long l = ptp.getRayleiPosition(sb, lynxUpDown1.DoubleValue);
            RayleP.Text = l.ToString();
        }

        private void buttonChangedType_Click(object sender, RoutedEventArgs e)
        {
            double l = Convert.ToDouble(leftBorder.Text);
            double r = Convert.ToDouble(rightBorder.Text);
            double p = Convert.ToDouble(RayleP.Text);
            double startValue=-1, endValue=-1;
            if (comboBox1.SelectedIndex == 0)
            {
                startValue = 500;
                endValue = 2000;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                startValue = 1000;
                endValue = 4000;
            }
            if (comboBox1.SelectedIndex == 2)
            {
                startValue = 100;
                endValue = 600;
            }
            if (startValue < 0 || endValue < 0||r==l) { return; }
            double tr = (p - l) / (r - l) * (endValue - startValue) + startValue;
            testResult.Text = tr.ToString();
        }

    }
}
