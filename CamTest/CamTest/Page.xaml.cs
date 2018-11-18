using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using System.IO;
using SilverlightLynxControls;
using DCTestLibrary;
using SilverlightDCTestLibrary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using SilverlightLFC.common;

namespace ImageTest
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            //am = new ActionMove(this, TitleArea);
        }
        private DispatcherTimer sw = new DispatcherTimer();
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var bl =SilverlightLFC.common.Environment.getEnvironment().OpenImage();
            if(bl!=null&&bl.Result!=null&&bl.Result.FirstOrDefault()!=null)
                SourcePhoto = bl.Result.FirstOrDefault();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            if (comType.SelectedItem == null)
            {
                return;
            }
            if (comType.SelectionBoxItem.ToString() == "ISO12233测试卡")
            {
                ISO12233ExTest();
            }
            if (comType.SelectionBoxItem.ToString() == "XRite色标")
            {
                XRiteTest();
            }
            if (comType.SelectionBoxItem.ToString() == "XMark测试卡")
            {
                XMarkTest();
            }
        }

        public void ISO12233ExTest()
        {

            processbar.Value = 0;
            ResultPanel.Children.Clear();

            Button li;
            ISO12233ExChart ISOChart = new ISO12233ExChart(SourcePhoto);
            ISOChart.CorrectChart();
            ISOChart.BeginAnalyse();
            processbar.Value = 30;

            sw.Stop();
            sw.Start();

            long r = ISOChart.getLPResoveLines();
            li = new Button();
            li.Name = "ResolvingPower";
            li.Content = "分辨率（中央）："+r.ToString()+" LW/PH";
            li.SetValue(ToolTipService.ToolTipProperty, "利用瑞利判据，直观判断相机分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量");
            li.Click += new RoutedEventHandler(ISO12233MTF_Click);
            ResultPanel.Children.Add(li);
            processbar.Value = 50;

            r = ISOChart.getHEdgeResoveLines();
            li = new Button();
            li.Name = "HResolvingPower";
            li.Content = "分辨率（水平线）：" + r.ToString() + " LW/PH";
            li.SetValue(ToolTipService.ToolTipProperty, "利用水平线条测试分辨率，接近相机的中心分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量");
            li.Click += new RoutedEventHandler(ISO12233MTF_Click);
            ResultPanel.Children.Add(li);
            processbar.Value = 60;

            r = ISOChart.getVEdgeResoveLines();
            li = new Button();
            li.Name = "VResolvingPower";
            li.Content = "分辨率（垂直线）：" + r.ToString() + " LW/PH";
            li.SetValue(ToolTipService.ToolTipProperty, "利用垂直线条测试分辨率，接近相机的边沿分辨率，数据表示在一张照片里面可以分辨的垂直线条的数量");
            li.Click += new RoutedEventHandler(ISO12233MTF_Click);
            ResultPanel.Children.Add(li);
            processbar.Value = 70;

            li = new Button();
            decimal d = ISOChart.getHDispersiveness();
            li.Name = "HDispersiveness";
            li.Content = "色散（水平线）：" + d.ToString() + " pxl";
            li.SetValue(ToolTipService.ToolTipProperty, "利用水平线条测试，接近中央色散程度，数据表示红绿蓝三原色在边界分离的程度，无色散时为0");
            li.Click += new RoutedEventHandler(ISO12233Dispersiveness_Click);
            ResultPanel.Children.Add(li);
            processbar.Value = 85;

            li = new Button();
            d = ISOChart.getVDispersiveness();
            li.Name = "VDispersiveness";
            li.Content = "色散（垂直线）：" + d.ToString() + " pxl";
            li.SetValue(ToolTipService.ToolTipProperty, "利用垂直线条测试，接近边沿色散程度，数据表示红绿蓝三原色在边界分离的程度，无色散时为0");
            li.Click += new RoutedEventHandler(ISO12233Dispersiveness_Click);
            ResultPanel.Children.Add(li);
            processbar.Value = 100;

            sw.Stop();
            li.SetValue(ToolTipService.ToolTipProperty, "测试共花费" + sw.Interval.Milliseconds.ToString() + "ms");

            Logo.Source = ISOChart.AnalysePhoto;

        }
        WriteableBitmap SourcePhoto;
        public void XRiteTest()
        {
            processbar.Value = 0;
            ResultPanel.Children.Clear();

            Button li;
            XRiteColorChart xr = new XRiteColorChart(SourcePhoto);
            //xr.NoiseSwing = XRite_NoiseBrightChange;
            xr.BeginAnalyse();

            sw.Stop();
            sw.Start();

            decimal d = Convert.ToDecimal(xr.getWhiteBanlance()) * 100;
            li = new Button();
            li.Name = "AutoWhiteBalanceDistance";
            li.Content = "白平衡误差：";
            li.Content = li.Content + d.ToString();
            li.Content = li.Content + " %";

            li.Click += new RoutedEventHandler(XRiteWhiteBalance_Click);
            //li.SubItems.Add(sw.ElapsedMilliseconds.ToString());
            li.SetValue(ToolTipService.ToolTipProperty,"数据表示拍摄灰度的时刻偏离灰度的程度，完全准确时为0");

            ResultPanel.Children.Add(li);
            processbar.Value = 30;

            d = xr.getColorDistance();
            li = new Button();
            li.Name = "ColorTrendValue";
            li.Content = "色彩趋向误差："+d.ToString()+" 度";
            li.Click += new RoutedEventHandler(XRiteColorDis_Click);
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示在拍摄标准颜色时候色调的偏差，完全准确时为0");

            ResultPanel.Children.Add(li);
            processbar.Value = 60;

            d = Convert.ToDecimal(xr.getNoiseNum(25)) * 100;
            
            li = new Button();
            li.Name = "Noise";
            li.Content = "噪点："+d.ToString()+" %";
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示在一张照片里面可以分辨的噪点数量");
            ResultPanel.Children.Add(li);
            processbar.Value = 100;

            sw.Stop();
            li.SetValue(ToolTipService.ToolTipProperty, "测试共花费"+sw.Interval.Milliseconds.ToString()+"ms");
            
            Logo.Source = xr.AnalysePhoto;

        }

        public void XMarkTest()//测试XMark卡的各种参数
        {
            processbar.Value = 0;
            ResultPanel.Children.Clear();
            sw.Stop();
            sw.Start();

            Button li;
            XMarkChart xm = new XMarkChart(SourcePhoto);
            xm.CorrectChart();
            xm.BeginAnalyse();
            processbar.Value = 10;

            decimal r;

            r = xm.getAberration() * 100;
            li = new Button();
            li.Click += new RoutedEventHandler(XMarkMTF_Click);
            li.Name = "WAberration";
            li.Content = "广角端畸变："+r.ToString()+" %";
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示照片里面几何变形程度，正表示桶形畸变，负表示枕型畸变，0为无畸变");
            ResultPanel.Children.Add(li);
            processbar.Value = 20;

            r = Convert.ToDecimal(xm.getLatitude());
            li = new Button();
            li.Name = "DynamicLatitude";
            li.Content = "动态范围：" + r.ToString() + " 级";
            li.Click += new RoutedEventHandler(XMarkLatitude_Click);
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示在一张照片里面可以分辨的灰度等级的数量");
            ResultPanel.Children.Add(li);
            processbar.Value = 30;

            r = xm.getColorDis();
            li = new Button();
            li.Name = "ColorTrendValue";
            li.Content = "色彩趋向差异：" + r.ToString() + " 度";
            li.Click += new RoutedEventHandler(XMarkColorDis_Click);
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示在拍摄标准颜色时候色调的偏差，完全准确时为0");
            ResultPanel.Children.Add(li);
            processbar.Value = 40;

            r = xm.getNoiseNum() * 100;
            li = new Button();
            li.Name = "Noise";
            li.Content = "噪点：" + r.ToString() + " %";
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示在一张照片里面可以分辨的噪点数量");
            ResultPanel.Children.Add(li);
            processbar.Value = 50;

            r = xm.getWhiteBanlance() * 100;
            li = new Button();
            li.Name = "AutoWhiteBalanceDistance";
            li.Content = "白平衡能力：" + r.ToString() + " %";
            li.Click += new RoutedEventHandler(XMarkWhiteBalance_Click);
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示拍摄灰度的时刻偏离灰度的程度，完全准确时为0");
            ResultPanel.Children.Add(li);
            processbar.Value = 60;

            r = Convert.ToDecimal(xm.getVEdgeDispersiveness());
            li = new Button();
            li.Name = "Dispersiveness";
            li.Content = "色散：" + r.ToString() + " Pxl";
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示红绿蓝三原色在边界分离的程度，也就是平均分离到几个像素，无色散时为0");
            li.Click += new RoutedEventHandler(XMarkDispersiveness_Click);
            ResultPanel.Children.Add(li);
            processbar.Value = 70;

            r = xm.getBrightChanges() * 100;
            li = new Button();
            li.Name = "TLightingEquality";
            li.Content = "亮度一致性：" + r.ToString() + " %";
            li.Click += new RoutedEventHandler(XMarkBrightChanges_Click);
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示在照片里面中心亮度和四周亮度的差异，无差异时为0");
            ResultPanel.Children.Add(li);
            processbar.Value = 80;

            r = xm.getHEdgeResoveLines();
            li = new Button();
            li.Name = "ResolvingPower";
            li.Content = "分辨率：" + r.ToString() + " lw/ph";
            li.Click += new RoutedEventHandler(XMarkMTF_Click);
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示在一张照片里面可以分辨的水平线条的数量");
            ResultPanel.Children.Add(li);
            processbar.Value = 90;

            r = xm.getWaveQ() * 100;
            li = new Button();
            li.Name = "WaveQ";
            li.Content = "成像一致性：" + r.ToString() + " %";
            li.Click += new RoutedEventHandler(XMarkWaveQ_Click);
            li.SetValue(ToolTipService.ToolTipProperty, "数据表示照片里面还原正弦灰度区域的能力，完全还原时为0，差异越大数据越大");
            ResultPanel.Children.Add(li);
            processbar.Value = 100;

            sw.Stop();
            li.SetValue(ToolTipService.ToolTipProperty, "测试共花费" + sw.Interval.Milliseconds.ToString() + "ms");

            Logo.Source = xm.AnalysePhoto;
        }

        void ISO12233MTF_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            pc.DrawMTFCurve(SourcePhoto,"ISO12233");
            LayoutRoot.Children.Add(pc);
        }

        void ISO12233Dispersiveness_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            pc.DrawDispersivenessCurve(SourcePhoto, "ISO12233");
            LayoutRoot.Children.Add(pc);
        }

        void XMarkMTF_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            pc.DrawMTFCurve(SourcePhoto, "XMark");
            LayoutRoot.Children.Add(pc);
        }
        
        void XMarkDispersiveness_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            pc.DrawDispersivenessCurve(SourcePhoto, "XMark");
            LayoutRoot.Children.Add(pc);
        }

        void XRiteWhiteBalance_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            pc.DrawWhiteBalanceCurve(SourcePhoto, "XRite");
            LayoutRoot.Children.Add(pc);
        }

        void XMarkWhiteBalance_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            pc.DrawWhiteBalanceCurve(SourcePhoto, "XMark");
            LayoutRoot.Children.Add(pc);
        }

        void XRiteLatitude_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            pc.DrawLatitudeCurve(SourcePhoto, "XRite");
            LayoutRoot.Children.Add(pc);
        }

        void XMarkLatitude_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            pc.DrawLatitudeCurve(SourcePhoto, "XMark");
            LayoutRoot.Children.Add(pc);
        }

        void XRiteColorDis_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            for (int i = 0; i < 24; i++)
            {
                ComboBoxItem ci = new ComboBoxItem();
                ci.Content = i.ToString();
                pc.ComIndex.Items.Add(ci);
            }
            pc.ComIndex.SelectionChanged += new SelectionChangedEventHandler(pc.ColorDis_SelectionChanged);
            pc.ComIndex.Visibility = Visibility.Visible;
            
            pc.DrawColorDisCurve(SourcePhoto, "XRite");
            LayoutRoot.Children.Add(pc);
        }

        void XMarkColorDis_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            for (int i = 0; i < 18; i++)
            {
                ComboBoxItem ci = new ComboBoxItem();
                ci.Content = i.ToString();
                pc.ComIndex.Items.Add(ci);
            }
            pc.ComIndex.SelectionChanged += new SelectionChangedEventHandler(pc.ColorDis_SelectionChanged);
            pc.ComIndex.Visibility = Visibility.Visible;
            pc.DrawColorDisCurve(SourcePhoto, "XMark");
            LayoutRoot.Children.Add(pc);
        }

        void XMarkBrightChanges_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            pc.DrawBrightChangesCurve(SourcePhoto, "XMark");
            LayoutRoot.Children.Add(pc);
        }
        void XMarkWaveQ_Click(object sender, RoutedEventArgs e)
        {
            PhotoCanvas pc = new PhotoCanvas(LayoutRoot);
            for (int i = 0; i < 5; i++)
            {
                ComboBoxItem ci = new ComboBoxItem();
                ci.Content = i.ToString();
                pc.ComIndex.Items.Add(ci);
            }
            pc.ComIndex.SelectionChanged += new SelectionChangedEventHandler(pc.WaveQ_SelectionChanged);
            pc.ComIndex.Visibility = Visibility.Visible;
            pc.DrawWaveQCurve(0,SourcePhoto, "XMark");
            LayoutRoot.Children.Add(pc);
        }

        private void Face_Click(object sender, RoutedEventArgs e)
        {
            //FaceTest.FaceTestPage p = new ImageTest.FaceTest.FaceTestPage();
            //LayoutRoot.Children.Add(p);


            //PhotoTest pt = new PhotoTest();
            //WriteableBitmap rb = pt.RotateBitmap(SourcePhoto, 30);
            //img.Source = rb;

            //double d = (double)1m;
            //Face.Content = al[1];
            //FaceTest ft = new FaceTest();
            //LayoutRoot.Children.Add(ft);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            if (p != null) { p.Children.Remove(this); }
        }
    }
}
