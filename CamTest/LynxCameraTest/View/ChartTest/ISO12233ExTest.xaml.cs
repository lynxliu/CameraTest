using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using SilverlightLynxControls;
using System.IO;
using SLPhotoTest.PhotoTest;
using DCTestLibrary;
using SilverlightDCTestLibrary;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using LynxCameraTest.Model;
using LynxCameraTest.View;
using Windows.UI.Xaml.Media.Imaging;

namespace SLPhotoTest.ChartTest
{
    public partial class ISO12233ExTest : UserControl, IPhotoTestWindow
    {
        public ISO12233ExTest()
        {
            InitializeComponent();
        }

        public void InitViewModel()
        {
        }
    }
}
