using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using SLPhotoTest.PhotoTest;
using SLPhotoTest.ChartTest;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SLPhotoTest
{
    public partial class PhotoTestMenu : UserControl
    {
        public PhotoTestMenu()
        {
            InitializeComponent();
        }

        private void buttonTimer_Click(object sender, RoutedEventArgs e)
        {
            SysTimer w = new SysTimer();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonBadPix_Click(object sender, RoutedEventArgs e)
        {
            BadPixTest w = new BadPixTest();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonShutterSpeed_Click(object sender, RoutedEventArgs e)
        {
            ShutterSpeed w = new ShutterSpeed();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonFace_Click(object sender, RoutedEventArgs e)
        {


        }

        private void buttonXMarkChart_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Standard))
            {
                return;
            }
            XMarkTest w = new XMarkTest();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonAberrationChart_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Standard))
            {
                return;
            }
            AberrationChartTest w = new AberrationChartTest();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonKDGrayChart_Click(object sender, RoutedEventArgs e)
        {
            if (!CameraTestDesktop.CanAccess(Version.Standard))
            {
                return;
            }
            KDGrayChartTest w = new KDGrayChartTest();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonGrayChart_Click(object sender, RoutedEventArgs e)
        {
            GrayChartTest w = new GrayChartTest();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonXRite_Click(object sender, RoutedEventArgs e)
        {
            XRiteTest w = new XRiteTest();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonISO2233Ex_Click(object sender, RoutedEventArgs e)
        {
            ISO12233ExTest w = new ISO12233ExTest();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonISO2233_Click(object sender, RoutedEventArgs e)
        {
            ISO12233Test w = new ISO12233Test();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }

        private void buttonITEGray_Click(object sender, RoutedEventArgs e)
        {
            ITEGrayscaleChartTest w = new ITEGrayscaleChartTest();
            ActionShow.CenterShow(CameraTestDesktop.getDesktopPanel(), w);
            Canvas Desk = CameraTestDesktop.getDesktopPanel();
            if (Desk.Children.Contains(this))
            {
                Desk.Children.Remove(this);
            }
        }
    }
}
