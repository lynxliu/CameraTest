using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using ImageTest;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SLPhotoTest
{
    public partial class TestToolBox : UserControl,IMove
    {
        public TestToolBox()
        {
            InitializeComponent();
            am = new ActionMove(this, MA);
        }
        #region IMove Members

        private ActionMove am;
        public ActionMove moveAction
        {
            get { return am; }
        }

        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ImageTest.Page p = new ImageTest.Page();

            LynxWindow.ShowWindow(p, this.Parent as Panel);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //FaceTestPage p = new FaceTestPage();
            //Panel pp = this.Parent as Panel;
            //if (pp != null)
            //{
            //    pp.Children.Add(p);
            //}
            //ActionAnimationShow.ShowZoomProjection(p, 1000);
            LynxWindow.ShowWindow(p, this.Parent as Panel);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ShutterSpeed ss = new ShutterSpeed();
            LynxWindow.ShowWindow(ss, this.Parent as Panel);
        }

        private void ButtonTimer(object sender, RoutedEventArgs e)
        {
            SysTimer ss = new SysTimer();
            LynxWindow.ShowWindow(ss, this.Parent as Panel);
        }
    }
}
