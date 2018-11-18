using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using DCTestLibrary;
using SilverlightDCTestLibrary;
using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using LynxCameraTest.Model;
using LynxCameraTest.ViewModel.ChartTestViewModel;

namespace SLPhotoTest.ChartTest
{
    public partial class JBEVTest : UserControl, IPhotoTestWindow
    {
        public JBEVTest()
        {
            InitializeComponent();
        }
        
        public void InitViewModel()
        {
            var vm = DataContext as JBEVTestViewModel;
            if (vm != null)
                vm.GBControl = gridGB;
        }
    }
}
