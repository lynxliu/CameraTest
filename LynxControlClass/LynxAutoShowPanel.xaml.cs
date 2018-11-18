using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SilverlightLynxControls
{
    public partial class LynxAutoShowPanel : UserControl
    {
        public LynxAutoShowPanel()
        {
            InitializeComponent();
        }

        public void AddContent(FrameworkElement fe)
        {
            if(!LayoutRoot.Children.Contains(fe))
            LayoutRoot.Children.Add(fe);
        }

        public void RemoveContent(FrameworkElement fe)
        {
            if(LayoutRoot.Children.Contains(fe))
            LayoutRoot.Children.Remove(fe);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LayoutRoot.Width = e.NewSize.Width;
            LayoutRoot.Height = e.NewSize.Height;
        }

        private void LayoutRoot_MouseEntered(object sender, PointerRoutedEventArgs e)
        {
            StoryboardShow.Begin();
        }

        private void LayoutRoot_MouseExited(object sender, PointerRoutedEventArgs e)
        {
            StoryboardHide.Begin();
        }

    }
}
