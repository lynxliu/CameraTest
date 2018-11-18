using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using SilverlightLynxControls;
using SilverlightDCTestLibrary;
using DCTestLibrary;
using System.IO;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using LynxCameraTest.Model;
using Windows.UI.Xaml.Media.Imaging;
using LynxCameraTest.ViewModel.ChartTestViewModel;

namespace SLPhotoTest.ChartTest
{
    public partial class KDGrayChartTest : UserControl, IPhotoTestWindow
    {
        public KDGrayChartTest()
        {
            InitializeComponent();
        }

        public void InitViewModel()
        {
            var vm = DataContext as KDGrayChartTestViewModel;
            if (vm != null)
                vm.DrawCanvas = canvasLatitude;
        }
    }
}
