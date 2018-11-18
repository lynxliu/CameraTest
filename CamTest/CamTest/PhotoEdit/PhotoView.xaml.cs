using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SLPhotoTest.PhotoEdit
{
    public partial class PhotoView : UserControl//针对全局的查看，从视口的查看不包括任何的编辑
    {
        public PhotoView()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
        }
        ActionMove acm;

        PhotoEditCanvas Target;
        public void setTarget(PhotoEditCanvas pc)
        {
            Target = pc;

        }

        private void UpStep_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetTop(Target.getLayers(), Canvas.GetTop(Target.getLayers()) - textBoxMovePix.IntValue);
        }

        private void buttonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ActionShow.ZoomCanvasIn(Target.getLayers(), 1+textBoxZoom.DoubleValue);
        }

        private void buttonAuto_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(Target.getLayers(),0);
            Canvas.SetTop(Target.getLayers(), 0);
            double dx,dy;
            dx = Target.getFramework().Width / Target.getLayers().Width;
            dy = Target.getFramework().Height / Target.getLayers().Height;


            ActionShow.XZoomCanvasIn(Target.getLayers(), dx);
            ActionShow.YZoomCanvasIn(Target.getLayers(), dy);
        }

        private void Center_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(Target.getLayers(), Target.getFramework().Width / 2 - Target.getLayers().Width / 2);
            Canvas.SetTop(Target.getLayers(), Target.getFramework().Height / 2 - Target.getLayers().Height / 2);

        }

        private void LeftStep_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(Target.getLayers(), Canvas.GetLeft(Target.getLayers()) - textBoxMovePix.IntValue);

        }

        private void RightStep_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(Target.getLayers(), Canvas.GetLeft(Target.getLayers()) + textBoxMovePix.IntValue);

        }

        private void DownStep_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetTop(Target.getLayers(), Canvas.GetTop(Target.getLayers()) + textBoxMovePix.IntValue);

        }

        private void buttonZoom_Click(object sender, RoutedEventArgs e)
        {
            ActionShow.ZoomCanvasIn(Target.getLayers(), 1-textBoxZoom.DoubleValue);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }


    }
}
