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
using LynxCameraTest.Model;
using Windows.UI.Xaml.Controls;
using LynxCameraTest.ViewModel.ChartTestViewModel;

namespace SLPhotoTest.ChartTest
{
    public partial class AberrationChartTest : UserControl, IPhotoTestWindow
    {
        public AberrationChartTest()
        {
            InitializeComponent();
        }

        public void InitViewModel()
        {
        }
    }
}
