using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using DCTestLibrary;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using LynxCameraTest.View;
using LynxControls;

namespace SLPhotoTest.ChartTest
{
    public partial class ChartCorrect : UserControl
    {
        public ChartCorrect()
        {
            InitializeComponent();
        }

        public void Init(AbstractTestChart Chart, LynxPhotoViewControl tlc)
        {

            lc = tlc;
            o = Chart;
        }

        public AbstractTestChart o = null;
        LynxPhotoViewControl lc=null;

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

        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            if ((o == null) || (lc == null)) { return; }
            if (o.mp.SelectedPhoto == null) { return; }
            lc.setPhoto(o.mp.SelectedPhoto);
        }


    }
}
