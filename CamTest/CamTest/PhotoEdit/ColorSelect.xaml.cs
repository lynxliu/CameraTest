using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml.Controls.Primitives;

namespace SLPhotoTest.PhotoEdit
{
    public partial class ColorSelect : UserControl
    {
        public ColorSelect()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
            cb.Color = Colors.White;
            RegColor.Fill = cb;
        }
        ActionMove acm;

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(this);
            }
            else
            {
                Popup cw = Parent as Popup;
                cw.IsOpen=false;
            }
        }

        SolidColorBrush cb = new SolidColorBrush();
        private void sliderR_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(sliderR!=null)
            {
            cb.Color =Color.FromArgb(255,Convert.ToByte(sliderR.Value),cb.Color.G,cb.Color.B);
            }
        }

        private void sliderG_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sliderG != null)
            {
                cb.Color = Color.FromArgb(255, cb.Color.R, Convert.ToByte(sliderG.Value), cb.Color.B);
            }
        }

        private void sliderB_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (sliderB != null)
            {
                cb.Color = Color.FromArgb(255, cb.Color.R, cb.Color.G, Convert.ToByte(sliderB.Value));
            }
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Popup f = this.Parent as Popup;
            f.IsOpen=false;
            //Panel p = f.Parent as Panel;
            //p.Children.Remove(f);
        }

        public Color getSelectColor()
        {
            return cb.Color;
        }

    }
}
