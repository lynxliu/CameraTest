using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using System.IO;

using DCTestLibrary;
using SilverlightPhotoIO;
using SilverlightLFC.common;
using SLPhotoTest.PhotoTest;
using SLPhotoTest.UIControl;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace SLPhotoTest.ChartTest
{
    public partial class PhotoEditToolbar : UserControl
    {
        public PhotoEditToolbar()
        {
            InitializeComponent();
            //CameraTestDesktop.CheckEnable();
        }
        public event PhotoTestOperation PhotoOperate;

        public LChartPhoto ProcessImageCanvas//处理的画布对象，包含一个Image
        {
            get { return TargetControl.TargetControl; }
        }
        //public LMultiPhoto TargetMultiPhoto;
        public LPhotoList TargetControl;
        public void setChartControl(LPhotoList lc)
        {
            TargetControl = lc;
        }

        //ActionMove amI;
        public void ZoomIn()
        {
            ProcessImageCanvas.Zoom(1.2);
            if (PhotoOperate != null)
            {
                PhotoOperate("ZoomIn", ProcessImageCanvas.getPhoto());
            }
        }

        public void ZoomOut()
        {
            ProcessImageCanvas.Zoom(0.8);
            if (PhotoOperate != null)
            {
                PhotoOperate("ZoomOut", ProcessImageCanvas.getPhoto());
            }
        }

        public void setMove()
        {
            ProcessImageCanvas.EnableMove();
            if (PhotoOperate != null)
            {
                PhotoOperate("Move", ProcessImageCanvas.getPhoto());
            }
        }

        public void setUnMove()
        {
            ProcessImageCanvas.DisableMove();
            if (PhotoOperate != null)
            {
                PhotoOperate("UnMove", ProcessImageCanvas.getPhoto());
            }
        }

        public void OpenImage()
        {
            var tb=PhotoIO.ReadMultiImageFromFile();
            if(tb==null) return ;

            List<WriteableBitmap> bl = tb.Result;
            if (bl == null) { return; }
            foreach (WriteableBitmap b in bl)
            {
                TargetControl.AddPhoto(b);
                if (PhotoOperate != null)
                {
                    PhotoOperate("Open", ProcessImageCanvas.getPhoto());
                }
            }


        }

        public void SaveImage()
        {
            if (ProcessImageCanvas.getPhoto() != null)
            {
                PhotoIO pi = new PhotoIO();
                PhotoIO.WriteImageToFile(ProcessImageCanvas.getPhoto());
            }
            if (PhotoOperate != null)
            {
                PhotoOperate("Save", ProcessImageCanvas.getPhoto());
            }
        }
        
        public void setSelect()
        {
            ProcessImageCanvas.getDrawObjectCanvas().PointerPressed += ProcessImageCanvas_PointerPressed;
            ProcessImageCanvas.getDrawObjectCanvas().PointerReleased += ProcessImageCanvas_PointerReleased;
            ProcessImageCanvas.getDrawObjectCanvas().PointerMoved += ProcessImageCanvas_PointerMoved;
        }

        void ProcessImageCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsBeginSelect) 
            ProcessImageCanvas.setSelectSize(e.GetCurrentPoint(ProcessImageCanvas.getDrawObjectCanvas()).Position);
            //sr.Width = Math.Abs(e.GetPosition(ProcessImageCanvas).X - sp.X);
            //sr.Height = Math.Abs(e.GetPosition(ProcessImageCanvas).Y - sp.Y);
            //Canvas.SetLeft(sr,Math.Min(e.GetPosition(ProcessImageCanvas).X, sp.X));
            //Canvas.SetTop(sr,Math.Min(e.GetPosition(ProcessImageCanvas).Y , sp.Y));
        }
        //Point sp;
        bool IsBeginSelect = false;
        Rectangle sr = new Rectangle();
        void setUnSelect()
        {
            IsBeginSelect = false;


            ProcessImageCanvas.getDrawObjectCanvas().PointerPressed -= ProcessImageCanvas_PointerPressed;
            ProcessImageCanvas.getDrawObjectCanvas().PointerReleased -= ProcessImageCanvas_PointerReleased;
            ProcessImageCanvas.getDrawObjectCanvas().PointerMoved -= ProcessImageCanvas_PointerMoved;
        }
        void ProcessImageCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)//区域太小（小于3像素）就是取消选择区
        {
            //IsBeginSelect = false;
            //Canvas c = ProcessImageCanvas.getCanvas();

            setUnSelect();
            ProcessImageCanvas.EndSelect();
            ClearActiveAll();
            if (PhotoOperate != null)
            {
                PhotoOperate("Select", ProcessImageCanvas.getPhoto());
            }
            //Point x = e.GetPosition(ProcessImageCanvas.getCanvas());
            //double dx, dy;
            //dx = Math.Abs(x.X - sp.X);
            //dy = Math.Abs(x.Y - sp.Y);
            //if ((sp.X == Double.NaN) || (sp.Y == Double.NaN)) { return; }
            //if (dx < 3 && dy < 3)
            //{
            //    ProcessImageCanvas.EndSelect(x);
            //}

            //ProcessImageCanvas.PointerPressed -= new MouseButtonEventHandler(ProcessImageCanvas_PointerPressed);
            //ProcessImageCanvas.PointerReleased -= new MouseButtonEventHandler(ProcessImageCanvas_PointerReleased);
            //ProcessImageCanvas.PointerMoved -= new PointerEventHandler(ProcessImageCanvas_PointerMoved);
            //sp.X = Double.NaN;
        }

        void ProcessImageCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsBeginSelect = true;
            //sp = e.GetPosition(ProcessImageCanvas.getCanvas());
            ProcessImageCanvas.BeginSelect(e.GetCurrentPoint(ProcessImageCanvas.getDrawObjectCanvas()).Position);
            //Canvas c = ProcessImageCanvas.getCanvas();
            //if (c.Children.Contains(sr)) { } else { c.Children.Add(sr); }
            
            //sr.Stroke = new SolidColorBrush(Colors.Red);
            //sr.StrokeThickness = 3;
            //DrawGraphic dg = new DrawGraphic();
            // DrawGraphic.getImagePosition(x, ProcessImageCanvas.getImage());
        }

        WriteableBitmap getClip()
        {
            if (tempClip.Source != null)
            {
                return tempClip.Source as WriteableBitmap;
            }
            return null;
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            setSelect();
            stackSelectButton.Background = ActiveButton;
        }

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            OpenImage();

        }

        private void buttonAutoTest_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            ProcessImageCanvas.AutoTest();
            if (PhotoOperate != null)
            {
                PhotoOperate("Tested", ProcessImageCanvas.getPhoto());
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            SaveImage();
            //ProcessImageCanvas.saveChartTest();
        }

        private void buttonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            ZoomOut();
        }

        private void buttonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            ZoomIn();
        }
        Brush ActiveButton = new SolidColorBrush(Colors.Red);
        void ClearActiveAll()
        {
            setUnSelect();
            setUnMove();
            stackMove.Background = null;
            stackSelectButton.Background = null;

        }
        private void buttonMove_Click(object sender, RoutedEventArgs e)
        {


            if (stackMove.Background == null)
            {
                stackMove.Background = ActiveButton;
                setMove();
            }
            else
            {
                ClearActiveAll();
            }
        }

        private void buttonPaste_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            if (d.getClip() == null) { return; }
            ProcessImageCanvas.setPhoto(WriteableBitmapHelper.Clone(d.getClip()));
            if (PhotoOperate != null)
            {
                PhotoOperate("Paste", ProcessImageCanvas.getPhoto());
            }
        }

        private void buttonCopy_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            //if (ProcessImageCanvas.getPhoto() == null) { return; }
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            if (ProcessImageCanvas.IsSelect)
            {
                WriteableBitmap selectArea = ProcessImageCanvas.getSelectArea();
                if (selectArea == null) { return; }
                d.addClip(selectArea);
                if (PhotoOperate != null)
                {
                    PhotoOperate("CopySelect", ProcessImageCanvas.getPhoto());
                }
            }
            else
            {
                SilverlightLFC.common.Environment.ShowMessage("请选择要复制的区域");
            }
            //d.addClip(ProcessImageCanvas.getPhoto());
        }

        private void buttonCrop_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            //if (ProcessImageCanvas.getPhoto() == null) { return; }
            //Canvas c = ProcessImageCanvas.getCanvas();
            //if (!c.Children.Contains(sr)) { return; }
            
            //DrawGraphic dg = new DrawGraphic();
            //Point isp = DrawGraphic.getImagePosition(new Point(Canvas.GetLeft(sr), Canvas.GetTop(sr)), ProcessImageCanvas.getImage());
            
            //Point iep = DrawGraphic.getImagePosition(new Point(Canvas.GetLeft(sr)+sr.Width, Canvas.GetTop(sr)+sr.Height), ProcessImageCanvas.getImage());
            //DCTestLibrary.PhotoTest pht = new DCTestLibrary.PhotoTest();
            if (ProcessImageCanvas.IsSelect)
            {
                //WriteableBitmap selectArea = pht.getImageArea(ProcessImageCanvas.getChart(), (int)isp.X, (int)isp.Y, (int)iep.X, (int)iep.Y);
                WriteableBitmap selectArea = ProcessImageCanvas.getSelectArea();
                if (selectArea == null) { return; }
                ProcessImageCanvas.setPhoto(selectArea);
                if (PhotoOperate != null)
                {
                    PhotoOperate("Crop", ProcessImageCanvas.getPhoto());
                }
            }
            else
            {
                SilverlightLFC.common.Environment.ShowMessage("请选择要剪裁的区域");
            }

            //c.Children.Remove(sr);
        }

        private void buttonSaveTest_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            if (saveChartTestResult != null) 
            { 
                saveChartTestResult();
                if (PhotoOperate != null)
                {
                    PhotoOperate("SaveTest", ProcessImageCanvas.getPhoto());
                }
            }

        }

        public SaveChartTest saveChartTestResult;
        public delegate void SaveChartTest();

        private void buttonPasteAll_Click(object sender, RoutedEventArgs e)
        {
            List<WriteableBitmap> bl = CameraTestDesktop.getDesktop().getClipList();
            foreach (WriteableBitmap b in bl)
            {
                TargetControl.AddPhoto(b);
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            TargetControl.DeleteCurrent();
        }

        public void setTestAll(ParameterTest testAll)
        {
            TestAll = testAll;
        }
        ParameterTest TestAll;
        private void buttonAutoTestAll_Click(object sender, RoutedEventArgs e)
        {
            if (TestAll != null)
            {
                TestAll();
            }
        }

        private void buttonResume_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            if (ProcessImageCanvas == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            ProcessImageCanvas.RestTransform();
            ProcessImageCanvas.ResetSize();
            if (PhotoOperate != null)
            {
                PhotoOperate("Resume", ProcessImageCanvas.getPhoto());
            }
        }
        //System.Windows.Threading.DispatcherTimer time = new System.Windows.Threading.DispatcherTimer();
        //List<ParameterTest> pl;
        //public void Test()
        //{
        //    time.Interval = TimeSpan.FromSeconds(2);
        //    time.Tick += new EventHandler(time_Tick);

        //}
        //int pi = 0;
        //void time_Tick(object sender, EventArgs e)
        //{
        //    time.Stop();
        //    if (pi < pl.Count-1)
        //    {
        //        pi++;
        //        pl[pi]();
        //        time.Start();
        //    }
        //    else
        //    {

        //    }
            
            
            
        //}

    }
    public delegate void ChartParameterTest(AbstractTestChart ct);
    public delegate void ParameterTest();//单独对某个指标进行的测试
}
