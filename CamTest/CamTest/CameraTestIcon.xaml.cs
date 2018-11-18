using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SilverlightLynxControls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;

namespace SLPhotoTest
{
    public delegate void ReleaseResource();
    public partial class CameraTestIcon : UserControl
    {
        public ReleaseResource release=null;//删除对象时候释放资源
        public FrameworkElement ctp;//对应的项目

        public CameraTestIcon(FrameworkElement cp)
        {
            InitializeComponent();
            ctp = cp;
        }
        public CameraTestIcon(FrameworkElement cp, BitmapSource bs, string title, string memo)
        {
            InitializeComponent();
            ctp = cp;
            if (bs != null)
            {
                bIcon.Source = bs;
            }
            textBlockText.Text = title;
            ToolTipService.SetToolTip(this, memo);
        }

        public void ToIcon()
        {
            if (ctp == null) { return; }
            ActionAnimationShow a = new ActionAnimationShow(ctp);
            CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            StackPanel taskBar = CameraTestDesktop.getTaskBar();
            FormPoint = new Point(Canvas.GetLeft(ctp), Canvas.GetTop(ctp));
            Point ep = CameraTestDesktop.getPosition(this);
            a.AnimationComplete += new AnimationCompleteEventHandler(hidea_AnimationComplete);
            a.HideZoomProjection(500, FormPoint, ep);
        }

        public void ToForm()
        {
            LayoutRoot.Opacity = 1;
            Panel p = CameraTestDesktop.getDesktopPanel();
            if (!p.Children.Contains(ctp))
            {
                p.Children.Add(ctp);
            }
            Point sp = CameraTestDesktop.getPosition(this);
            ActionAnimationShow a = new ActionAnimationShow(ctp);
            a.AnimationComplete += new AnimationCompleteEventHandler(a_AnimationComplete);
            a.ShowZoomProjection(500, sp, FormPoint);
        }
        Point FormPoint = new Point();
        public void Active()//不会激活窗体
        {
            LayoutRoot.Opacity = 1;
            Panel p = CameraTestDesktop.getDesktopPanel();
            if (p.Children.Contains(ctp))
            {
                CameraTestDesktop.ActiveUserControl(ctp);
            }
            if (CameraTestDesktop.SelectedTest != this && CameraTestDesktop.SelectedTest!=null)
            {
                CameraTestDesktop.SelectedTest.DeActive();
                
            }
            CameraTestDesktop.SelectedTest = this;
            //LayoutRoot.Background = new SolidColorBrush(Colors.Yellow);
        }

        void a_AnimationComplete(object sender, LynxAnimationCompleteEventArgs e)
        {
            Canvas.SetTop(ctp, e.EndP.Y);
            Canvas.SetLeft(ctp, e.EndP.X);
            //StackPanel tb = CameraTestDesktop.getTaskBar();
            //tb.Children.Remove(this);
        }
        public void DeActive()
        {
            //if (ctp == null) { return; }
            //ActionAnimationShow a = new ActionAnimationShow(ctp);
            //CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            //StackPanel taskBar = CameraTestDesktop.getTaskBar();
            //FormPoint = new Point(Canvas.GetLeft(ctp), Canvas.GetTop(ctp));
            ////Point ep = new Point(taskBar.Children.Count * Width, CameraTestDesktop.getDesktopPanel().Height);
            //Point ep = CameraTestDesktop.getPosition(this);
            //a.AnimationComplete += new AnimationCompleteEventHandler(hidea_AnimationComplete);
            //a.HideZoomProjection(500, FormPoint, ep);
            //LayoutRoot.Background = new SolidColorBrush(Colors.Transparent);
            LayoutRoot.Opacity = 0.8;
        }

        void hidea_AnimationComplete(object sender, LynxAnimationCompleteEventArgs e)
        {
            Panel p = CameraTestDesktop.getDesktopPanel();
            if (p != null)
            {
                p.Children.Remove(ctp);

            }
            StackPanel tb = CameraTestDesktop.getTaskBar();
            if (!tb.Children.Contains(this))
            {
                tb.Children.Add(this);
            }
        }
        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ToForm();
            //ToForm(e.GetPosition(null));
            //CameraTestDesktop cd = CameraTestDesktop.getDesktop();
            //cd.ActiveCameraTest(this);
        }

        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //Opacity = 1;
            effect.BlurRadius = 75;
            effect.ShadowDepth = 5;
            MiddleColor.Offset = 0.73;
        }

        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Opacity = 0.7;
            effect.BlurRadius = 0;
            effect.ShadowDepth = 0;
            MiddleColor.Offset = 0.37;
        }

        private void image1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            if (ctp != null)
            {
                if (CameraTestDesktop.getDesktopPanel().Children.Contains(ctp))
                {
                    CameraTestDesktop.getDesktopPanel().Children.Remove(ctp);
                }
            }
            if (release != null) { release(); }
            if((Parent!=null)&&((Parent as Panel).Children.Contains(this)))
                (Parent as Panel).Children.Remove(this);
        }

    }
}
