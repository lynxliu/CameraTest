using LynxCameraTest.Model;
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

namespace LynxCameraTest.View
{
    public sealed partial class MainControl : UserControl
    {
        public MainControl()
        {
            this.InitializeComponent();
        }

        private void itemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //var item = e.ClickedItem as FrameworkElement;
            //if (item == null) return;
            var data = e.ClickedItem as TestItem;
            if (data != null)
                data.Active();
        }
    }
}
