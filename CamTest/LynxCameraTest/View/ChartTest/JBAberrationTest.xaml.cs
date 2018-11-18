using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using DCTestLibrary;
using SilverlightDCTestLibrary;
using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using LynxCameraTest.Model;
using LynxCameraTest.ViewModel.ChartTestViewModel;

namespace SLPhotoTest.ChartTest
{
    public partial class JBAberrationTest : UserControl, IPhotoTestWindow
    {
        public JBAberrationTest()
        {
            InitializeComponent();
        }

        public void InitViewModel()
        {
            var vm = DataContext as JBAberrationTestViewModel;
            vm.GBControl = gridGB;
        }
    }
}
