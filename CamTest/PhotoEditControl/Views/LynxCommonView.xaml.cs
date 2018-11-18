using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PhotoTestControl.Views
{
    public sealed partial class LynxCommonView : UserControl
    {
        public LynxCommonView()
        {
            this.InitializeComponent();
            this.ManipulationDelta += LynxControl.FrameworkElement_ManipulationDelta;
            this.Tapped += LynxControl.FrameworkElement_Tapped;
            this.DoubleTapped += LynxControl.FrameworkElement_DoubleTapped;
        }
    }
}
