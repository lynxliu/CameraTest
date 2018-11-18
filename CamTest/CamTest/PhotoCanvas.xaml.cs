using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using DCTestLibrary;
using SilverlightDCTestLibrary;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Windows.UI.Xaml;

namespace ImageTest
{
    public partial class PhotoCanvas : UserControl
    {
        public PhotoCanvas(Panel p)
        {
            InitializeComponent();
            ParentPanel = p;
        }
        public PhotoCanvas(Panel p,WriteableBitmap chart,string operation,string chartname)
        {
            InitializeComponent();
            ParentPanel = p;
            ChartBitmap = chart;
            OperatorName = operation;
            ChartType = chartname;
        }
        public Panel ParentPanel;
        public WriteableBitmap ChartBitmap;
        public string OperatorName;
        public string ChartType;
        public ComboBox ComIndex
        {
            get { return comIndex; }
        }

        public void DrawCurve(WriteableBitmap w, string s)
        {
            ChartBitmap = w;
            OperatorName = s;
            DrawCurve();
        }

        public void DrawCurve()
        {
            ComIndex.Visibility = Visibility.Collapsed;
            if (OperatorName == "MTF")
            {
                DrawMTFCurve(ChartBitmap, ChartType);
            }
            if (OperatorName == "BrightChanges")
            {
                DrawBrightChangesCurve(ChartBitmap, ChartType);
            }
            if (OperatorName == "ColorDis")
            {
                ComIndex.Items.Clear();
                for (int i = 0; i < 24; i++)
                {
                    ComboBoxItem ci = new ComboBoxItem();
                    ci.Content = i.ToString();
                    ComIndex.Items.Add(ci);
                }
                ComIndex.SelectionChanged += new SelectionChangedEventHandler(ColorDis_SelectionChanged);
                ComIndex.Visibility = Visibility.Visible;
                DrawColorDisCurve(ChartBitmap, ChartType);
            }
            if (OperatorName == "Dispersiveness")
            {
                DrawDispersivenessCurve(ChartBitmap, ChartType);
            }
            if (OperatorName == "Latitude")
            {
                DrawLatitudeCurve(ChartBitmap, ChartType);
            }
            if (OperatorName == "Rainbow")
            {
                DrawRainbowCurve(ChartBitmap, ChartType);
            }
            if (OperatorName == "WaveQ")
            {
                ComIndex.Items.Clear();
                for (int i = 0; i < 5; i++)
                {
                    ComboBoxItem ci = new ComboBoxItem();
                    ci.Content = i.ToString();
                    ComIndex.Items.Add(ci);
                }
                ComIndex.SelectionChanged += new SelectionChangedEventHandler(WaveQ_SelectionChanged);
                ComIndex.Visibility = Visibility.Visible;
                DrawWaveQCurve(0,ChartBitmap, ChartType);
            }
            if (OperatorName == "WhiteBalance")
            {
                DrawWhiteBalanceCurve(ChartBitmap, ChartType);
            }
        }

        public void ColorDis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem ci = (ComboBoxItem)e.AddedItems[0];
            if (ci == null)
            {
                return;
            }
            DrawColorDisCurve(Convert.ToInt32(ci.Content), ChartBitmap, ChartType);
            //throw new NotImplementedException();
        }

        public void WaveQ_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem ci = (ComboBoxItem)e.AddedItems[0];
            if (ci == null)
            {
                return;
            }
            DrawWaveQCurve(Convert.ToInt32(ci.Content), ChartBitmap, ChartType);
            //throw new NotImplementedException();
        }

        public void DrawMTFCurve(WriteableBitmap b,string ChartType)//绘制MTF的分析图
        {
            this.ChartType = ChartType;
            List<double> al = new List<double>();
            if (ChartType == "XMark")
            {
                XMarkChart xm = new XMarkChart(b);
                al = xm.getCurveMTF();
            }
            if (ChartType == "ISO12233")
            {
                ISO12233ExChart x = new ISO12233ExChart(b);
                al = x.getCurveMTF();
            }
            DrawGraphic dg = new DrawGraphic(DrawCanvas);
            //dg.InitCanvas(picCanvas.Width, picCanvas.Height);//需要先设置画布

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
            //dg.DrawCurve(al);
            //picCanvas.Image = dg.Canvas;

        }

        public void DrawBrightChangesCurve(WriteableBitmap b,string ChartType)//绘制亮度变化的分析图
        {
            this.ChartType = ChartType;
            if (ChartType == "XMark"){
                List<List<int>> al = new List<List<int>>();
                try
                {
                    XMarkChart xm = new XMarkChart(b);
                    al = xm.getCurveBrightnessChange();
                }
                catch (Exception ex)
                {
                    if (ex is SilverlightLFC.common.LFCException)
                    {
                        SilverlightLFC.common.Environment.ShowMessage(ex.Message);
                    }
                    else
                    {
                        SilverlightLFC.common.Environment.ShowMessage("Error Process Image");
                    }
                }
                DrawGraphic dg = new DrawGraphic(DrawCanvas);
                //dg.InitCanvas(picCanvas.Width, picCanvas.Height);
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
                List<int> val, hal;
                val = al[1];
                hal = al[0];
                dg.ForeColor = Colors.Blue;
                dg.DrawLines(SilverlightLFC.common.Environment.getDoubleList<int>(hal));
                dg.ForeColor = Colors.Red;
                dg.DrawLines(SilverlightLFC.common.Environment.getDoubleList<int>(val));
                dg.DrawTitle("亮度一致性变化");
                //picCanvas.Image = dg.Canvas;

            }
        }

        public void DrawColorDisCurve(WriteableBitmap b, string ChartType)//绘制描述色差的图形
        {
            this.ChartType = ChartType;
            List<List<Color>> al = new List<List<Color>>();
            if (ChartType == "XMark")
            {
                XMarkChart xm = new XMarkChart(b);
                al = xm.getCurveColorDis();
            }
            if (ChartType == "XRite")
            {
                XRiteColorChart x = new XRiteColorChart(b);
                al = x.getCurveColorDis();
            }
            DrawGraphic dg = new DrawGraphic(DrawCanvas);

            dg.DrawColorCy(0.9f);
            for (int i = 7; i < 19; i++)
            {
                List<Color> tal = al[i - 7];
                Color c0 = (Color)tal[0];
                Color c1 = (Color)tal[1];
                //picS.BackColor = c0;
                //picV.BackColor = c1;
                dg.DrawColorMoveHue(c0, c1, 0.9f);
            }


        }

        public void DrawColorDisCurve(int cNo, WriteableBitmap b, string ChartType)//绘制单个色差
        {
            this.ChartType = ChartType;
            List<List<Color>> al = new List<List<Color>>();
            if (ChartType == "XMark")
            {
                XMarkChart xm = new XMarkChart(b);
                al = xm.getCurveColorDis();
                cNo = cNo - 7;
            }
            if (ChartType == "XRite")
            {
                XRiteColorChart x = new XRiteColorChart(b);
                al = x.getCurveColorDis();
                cNo = cNo - 1;
            }
            DrawGraphic dg = new DrawGraphic(DrawCanvas);

            dg.DrawColorCy(0.9f);

            List<Color> tal = al[cNo];
            Color c0 = (Color)tal[0];
            Color c1 = (Color)tal[1];
            //picS.BackColor = c0;
            //picV.BackColor = c1;
            dg.DrawColorMoveHue(c0, c1, 0.9f);


        }

        public void DrawDispersivenessCurve(WriteableBitmap b, string ChartType)//绘制色散分析图
        {
            this.ChartType = ChartType;
            List<List<int>> al = new List<List<int>>();
            WriteableBitmap subB;
            if (ChartType == "XMark")
            {
                XMarkChart x = new XMarkChart(b);
                subB = x.getAreaHEdge();
                PhotoTestParameter xm = new PhotoTestParameter();
                al = xm.getCurveVDispersiveness(subB);
            }
            if (ChartType == "ISO12233")
            {
                ISO12233ExChart x = new ISO12233ExChart(b);
                subB = x.getAreaHEdge();
                PhotoTestParameter xm = new PhotoTestParameter();
                al = xm.getCurveHDispersiveness(b);
            }
            DrawGraphic dg = new DrawGraphic(DrawCanvas);

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

        public void DrawLatitudeCurve(WriteableBitmap b, string ChartType)//绘制动态范围的分析图
        {
            this.ChartType = ChartType;
            List<decimal> al = new List<decimal>();
            if (ChartType == "XMark")
            {
                XMarkChart xm = new XMarkChart(b);
                al = xm.getCurveLatitude();
            }
            if (ChartType == "XRite")
            {
                XRiteColorChart xr = new XRiteColorChart(b);
                al = xr.getCurveLatitude();
            }
            DrawGraphic dg = new DrawGraphic(DrawCanvas);

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
            dg.ForeColor = Colors.Red;
            dg.DrawStepPoint(SilverlightLFC.common.Environment.getDoubleList<decimal>(al));
            dg.DrawTitle("宽容度");
            //picCanvas.Image = dg.Canvas;


        }
        public void DrawRainbowCurve(WriteableBitmap b, string ChartType)//绘制连续色阶准确性的分析图
        {
            this.ChartType = ChartType;
            List<Color> al = new List<Color>();
            if (ChartType == "XMark")
            {
                XMarkChart xm = new XMarkChart(b);
                al = xm.getCurveRainbow();
            }
            DrawGraphic dg = new DrawGraphic(DrawCanvas);

            dg.DrawColorCy(0.9f);

            dg.DrawColorList(al, 0.9f);


        }

        public void DrawWaveQCurve(int no, WriteableBitmap b, string ChartType)//绘制成像一致性的分析图
        {
            this.ChartType = ChartType;
            List<List<decimal>> al = new List<List<decimal>>();
            if (ChartType == "XMark")
            {
                XMarkChart xm = new XMarkChart(b);
                al = xm.getCurveWaveQ();
            }
            DrawGraphic dg = new DrawGraphic(DrawCanvas);

            dg.DrawX();
            List<double> MarkList = new List<double>();
            int step = (al[(no - 1) * 2]).Count / 5;
            for (int i = 0; i < (al[(no - 1) * 2]).Count; i = i + step)
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

            List<decimal> Sal, Val;
            Sal = al[(no - 1) * 2];
            Val = al[(no - 1) * 2 + 1];
            dg.ForeColor = Colors.Blue;
            dg.DrawLines(SilverlightLFC.common.Environment.getDoubleList<decimal>(Sal));
            dg.ForeColor = Colors.Red;
            dg.DrawLines(SilverlightLFC.common.Environment.getDoubleList<decimal>(Val));
            dg.DrawTitle("成像品质");

        }

        public void DrawWhiteBalanceCurve(WriteableBitmap b, string ChartType)//绘制白平衡的分析图
        {
            this.ChartType = ChartType;
            List<Color> al = new List<Color>();
            if (ChartType == "XMark")
            {
                XMarkChart xm = new XMarkChart(b);
                al = xm.getCurveWhiteBalance();
            }
            if (ChartType == "XRite")
            {
                XRiteColorChart x = new XRiteColorChart(b);
                al = x.getCurveWhiteBalance();
            }
            DrawGraphic dg = new DrawGraphic(DrawCanvas);
            //dg.InitCanvas(picCanvas.Width, picCanvas.Height);//需要先设置画布
            dg.DrawColorCy(0.9f);

            for (int i = 0; i < al.Count; i++)
            {

                Color c = (Color)al[i];
                dg.DrawColorPoint(c, 0.9f,3);
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ParentPanel.Children.Remove(this);
        }

    }
}
