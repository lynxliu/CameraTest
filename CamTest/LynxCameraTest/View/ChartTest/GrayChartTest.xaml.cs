using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using SilverlightDCTestLibrary;
using SilverlightLynxControls;
using DCTestLibrary;
using System.IO;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using PhotoTestControl;
using LynxCameraTest.Model;
using LynxCameraTest.ViewModel.ChartTestViewModel;

namespace SLPhotoTest.ChartTest
{
    public partial class GrayChartTest : UserControl, IPhotoTestWindow
    {
        public GrayChartTest()
        {
            InitializeComponent();
        }

        public void InitViewModel()
        {
            var vm = DataContext as GrayChartTestViewModel;
            vm.GBControl = gridGB;
        }
    }
}
