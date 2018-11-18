using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SLPhotoTest.ChartTest
{
    public partial class ChartCorrect : UserControl
    {
        public ChartCorrect()
        {
            InitializeComponent();
        }

        public void Init(AbstractTestChart Chart, LChartPhoto tlc)
        {
            if (Chart.ChartName != "XMark Chart")
            {
                buttonCrop.IsEnabled = false;
            }
            lc = tlc;
            o = Chart;
        }

        public AbstractTestChart o = null;
        LChartPhoto lc=null;

        private void buttonSource_Click(object sender, RoutedEventArgs e)
        {
            if ((o == null) || (lc == null)) { return; }
            if (o.mp.SourcePhoto == null) { return; }
            lc.setPhoto(o.mp.SourcePhoto);
        }

        private void buttonMove_Click(object sender, RoutedEventArgs e)
        {
            if ((o == null) || (lc == null)) { return; }
            if (o.mp.CorrectMovePhoto == null) { return; }
            lc.setPhoto(o.mp.CorrectMovePhoto);
        }

        private void buttonRotate_Click(object sender, RoutedEventArgs e)
        {
            if ((o == null) || (lc == null)) { return; }
            if (o.mp.CorrectRotatePhoto == null) { return; }
            lc.setPhoto(o.mp.CorrectRotatePhoto);
        }

        private void buttonScale_Click(object sender, RoutedEventArgs e)
        {
            if ((o == null) || (lc == null)) { return; }
            if (o.mp.CorrectScalePhoto == null) { return; }
            lc.setPhoto(o.mp.CorrectScalePhoto);
        }

        private void buttonCrop_Click(object sender, RoutedEventArgs e)
        {
            if ((o == null) || (lc == null)) { return; }
            DCTestLibrary.XMarkChart xc = o as DCTestLibrary.XMarkChart;
            if (xc != null)
            {
                if (xc.CropPhoto == null) { return; }
                lc.setPhoto(xc.CropPhoto);
            }
        }

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            if ((o == null) || (lc == null)) { return; }
            if (o.mp.SelectedPhoto == null) { return; }
            lc.setPhoto(o.mp.SelectedPhoto);
        }


    }
}
