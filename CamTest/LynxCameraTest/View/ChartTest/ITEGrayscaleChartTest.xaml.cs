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
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;
using PhotoTestControl.Models;
using LynxCameraTest.ViewModel.ChartTestViewModel;

namespace SLPhotoTest.ChartTest
{
    public partial class ITEGrayscaleChartTest : UserControl, IPhotoTestWindow
    {
        public ITEGrayscaleChartTest()
        {
            InitializeComponent();
        }

        public void InitViewModel()
        {
            var vm = DataContext as ITEGrayscaleChartTestViewModel;
            vm.DrawCanvas = canvasLatitude;
            vm.GBControl = gridGB;
        }
    }
}
