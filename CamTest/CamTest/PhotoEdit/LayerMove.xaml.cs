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
    public partial class LayerMove : UserControl
    {
        public LayerMove()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
        }
        ActionMove acm;
        PhotoEditCanvas TargetCanvas;
        public void setTarget(PhotoEditCanvas pc)//
        {
            TargetCanvas = pc;
            
            //img.PointerPressed += new MouseButtonEventHandler(img_PointerPressed);
            //img.PointerReleased += new MouseButtonEventHandler(img_PointerReleased);
        }

        private void RightStep_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(TargetCanvas.SelectLayer, Canvas.GetLeft(TargetCanvas.SelectLayer) + textBoxMovePix.IntValue);
        }

        private void LeftStep_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(TargetCanvas.SelectLayer, Canvas.GetLeft(TargetCanvas.SelectLayer) - textBoxMovePix.IntValue);

        }

        private void UpStep_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetTop(TargetCanvas.SelectLayer, Canvas.GetTop(TargetCanvas.SelectLayer) - textBoxMovePix.IntValue);

        }

        private void DownStep_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetTop(TargetCanvas.SelectLayer, Canvas.GetTop(TargetCanvas.SelectLayer) + textBoxMovePix.IntValue);

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            TargetCanvas.SelectLayer.LayerScale.ScaleX = textBoxPercent.DoubleValue;

        }

        private void buttonZoomY_Click(object sender, RoutedEventArgs e)
        {
            TargetCanvas.SelectLayer.LayerScale.ScaleY = textBoxPercent.DoubleValue;

        }

        private void buttonRotate_Click(object sender, RoutedEventArgs e)
        {
            TargetCanvas.SelectLayer.LayerRotate.Angle = textBoxAngel.DoubleValue;

        }

        private void buttonSkewX_Click(object sender, RoutedEventArgs e)
        {
            TargetCanvas.SelectLayer.LayerSkew.AngleX = textBoxSkew.DoubleValue;

        }

        private void buttonSkewY_Click(object sender, RoutedEventArgs e)
        {
            TargetCanvas.SelectLayer.LayerSkew.AngleY = textBoxSkew.DoubleValue;

        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }


    }
}
