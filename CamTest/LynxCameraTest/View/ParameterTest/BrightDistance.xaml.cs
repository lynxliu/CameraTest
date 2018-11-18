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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using Windows.UI;
using LynxControls;
using LynxCameraTest.Model;
using LynxCameraTest.ViewModel;

namespace SLPhotoTest.PhotoTest
{
    public partial class BrightDistance : UserControl, IParameterTestView
    {
        public void InitViewModel()
        {
            if (DataContext == null) return;
            var vm = DataContext as CommonToolViewModel;
            if (vm == null) return;
            vm.Title = "Bright Changes Parameter";
            vm.TestAction = ParameterAutoTest;
        }
        public BrightDistance()
        {
            InitializeComponent();
            am = new ActionMove(this, this);
            //photoTestToolbar1.setTarget(HB);
            //photoTestToolbar1.autoTest = ParameterAutoTest;
            //photoTestToolbar1.addPhoto = AddPhoto;
            //photoTestToolbar1.removePhoto = RemovePhoto;
            dg = new DrawGraphic(ImageBrightCurve);

        }
        ActionMove am;
        XMarkChart xt=new XMarkChart();
        DrawGraphic dg;
        List<WriteableBitmap> bl=new List<WriteableBitmap>();
        public void Test(List<WriteableBitmap> b)
        {
            if (b.Count == 0 || b == null) { return; }
            bl = b;
            try
            {
                HB.Photo=(b[1]);
                VB.Photo = (b[2]);
                xt = new XMarkChart();
                decimal d = xt.getBrightChanges(b[0]);//原始
                textBoxBrightChanges.Text = d.ToString();
                textBoxBB.Text = xt.ProcessInfor["BrightChanges_BorderBright"].ToString();
                textBoxCB.Text = xt.ProcessInfor["BrightChanges_CBright"].ToString();
                LT.Text = xt.ProcessInfor["BrightChanges_LT"].ToString();
                LB.Text = xt.ProcessInfor["BrightChanges_LB"].ToString();
                RT.Text = xt.ProcessInfor["BrightChanges_RT"].ToString();
                RB.Text = xt.ProcessInfor["BrightChanges_RB"].ToString();

                List<int> hl, vl;
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                hl = pt.getImageGrayHLine(b[1], b[1].PixelHeight / 2);
                vl = pt.getImageGrayVLine(b[2], b[2].PixelWidth / 2);
                dg.ForeColor = Colors.Blue;
                dg.DrawBrightLines(hl);
                dg.ForeColor = Colors.Red;
                dg.DrawBrightLines(vl);
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
        private void currentBright_MouseMove(object sender, PointerRoutedEventArgs e)
        {
            LynxPhotoViewControl im = sender as LynxPhotoViewControl;
            WriteableBitmap b= im.getPhoto();
            if (b != null)
            {
                Point? p = xt.PointToPix(b, im, e.GetCurrentPoint(im.getImage()).Position, false);
                if (p == null) { return; }
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                Color c = pt.GetPixel(b, Convert.ToInt32(p.Value.X), Convert.ToInt32(p.Value.Y));
                float ph,ps,pb;
                pt.RGB2HSB(Convert.ToInt32(c.R), Convert.ToInt32(c.G), Convert.ToInt32(c.B),out ph,out ps,out pb);
                textBoxCurrentBright.Text = pb.ToString();
            }
        }

        private void VB_MouseLeftButtonDown(object sender, PointerRoutedEventArgs e)
        {
            var im = sender as LynxPhotoViewControl;
            selectedImage = im;
            //VB.Active();
            //HB.DeActive();
            

            WriteableBitmap b = im.Photo;
            if (b != null)
            {
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                Point? p = xt.PointToPix(b, im, e.GetCurrentPoint(im).Position, false);
                if (p == null) { return; }
                dg.DrawBrightLines(pt.getImageGrayVLine(b, Convert.ToInt32(p.Value.X)));
            }

        }

        private void HB_MouseLeftButtonDown(object sender, PointerRoutedEventArgs e)
        {
            var im = sender as LynxPhotoViewControl;
            selectedImage = im;

            //HB.Active();
            //VB.DeActive();
            //photoTestToolbar1.setTarget(HB);
            WriteableBitmap b = im.Photo;
            if (b != null)
            {
                DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
                Point? p = xt.PointToPix(b, im, e.GetCurrentPoint(im).Position,false);
                if (p == null) { return; }
                dg.DrawBrightLines(pt.getImageGrayHLine(b,Convert.ToInt32(p.Value.Y)));
                
            }

        }
        LynxPhotoViewControl selectedImage;//当前选择的

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            bl = null;
            //photoTestToolbar1 = null;
            ImageBrightCurve.Children.Clear();
            VB.Clear();
            HB.Clear();
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            ImageBrightCurve.Children.Clear();
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ParameterAutoTest()
        {
            Test(bl);
        }

        public void AddPhoto(WriteableBitmap photo)
        {
            if (selectedImage == null)
            {
                return;
            }
            if (selectedImage == HB)
            {
                HB.Photo=(photo);
                bl[1] = photo;
            }
            if (selectedImage == VB)
            {
                VB.Photo = (photo);
                bl[2] = photo;
            }

        }

        public void RemovePhoto(WriteableBitmap b)
        {
            if (selectedImage == null)
            {
                return;
            }
            int i = bl.IndexOf(b);
            if (i==1)
            {
                bl.RemoveAt(1);
                HB.Photo = (null);
            }
            if (i == 2)
            {
                bl.RemoveAt(2);
                VB.Photo=(null);
            }

        }
    }
}
