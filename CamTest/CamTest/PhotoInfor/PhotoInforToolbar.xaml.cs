using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;




namespace SLPhotoTest.PhotoInfor
{
    public partial class PhotoInforToolbar : UserControl
    {
        public PhotoInforToolbar()
        {
            InitializeComponent();
            //CameraTestDesktop.CheckEnable();
        }

        Canvas getCanvas()
        {
            return this.Parent as Canvas;
        }
        LChartPhoto lc;
        StackPanel EditToolStack = new StackPanel();

        void AddToStack(FrameworkElement fe)
        {
            Canvas c = getCanvas();
            if (!c.Children.Contains(EditToolStack)) { c.Children.Add(EditToolStack); }
            EditToolStack.Children.Add(fe);
            Canvas.SetLeft(EditToolStack, Canvas.GetLeft(this) + Width);
            Canvas.SetTop(EditToolStack, Canvas.GetTop(this));
            Canvas.SetZIndex(EditToolStack, 50);
            RefreshStack();
        }

        void RefreshStack()
        {
            double sw = 0, sh = 0;
            foreach (FrameworkElement f in EditToolStack.Children)
            {
                if (f.Width > sw) { sw = f.Width; }
                sh = sh + f.Height;
            }
            EditToolStack.Width = sw;
            EditToolStack.Height = sh;
        }

        public void setTarget(LChartPhoto l)
        {
            lc = l;
        }

        private void buttonColorEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Standard))
            {
                return;
            }
            PixEdit w = new PixEdit();
            w.setTarget(lc);
            AddToStack(w);
        }

        private void buttonColorInfor_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            PhotoColorInfor w = new PhotoColorInfor();
            w.setTarget(lc);
            AddToStack(w);
        }

        private void buttonBrightInfor_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Professional))
            {
                return;
            }
            PhotoBrightCurve w = new PhotoBrightCurve();
            w.setTarget(lc);
            AddToStack(w);
        }

        private void buttonPhotoInfor_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Standard))
            {
                return;
            }
            PhotoInfor w = new PhotoInfor();
            w.setTarget(lc);
            AddToStack(w);
        }

        private void buttonFit_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Standard))
            {
                return;
            }
            PhotoFit w = new PhotoFit();
            w.setTarget(lc);
            AddToStack(w);
        }

        private void buttonPaste_Click(object sender, RoutedEventArgs e)
        {
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            lc.setPhoto(d.getClip());
        }

        private void buttonCopy_Click(object sender, RoutedEventArgs e)
        {
            if (lc.getPhoto() == null) { return; }
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            d.addClip(lc.getPhoto());
        }
    }
}
