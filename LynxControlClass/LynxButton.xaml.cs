using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;
using System.IO;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI;

namespace SilverlightLynxControls
{
    public partial class LynxButton : UserControl
    {
        public LynxButton()
        {
            InitializeComponent();
            
        }
        public LynxButton(UserControl u)
        {
            InitializeComponent();
            tu = u;
        }
        UserControl tu;
        Point cp;
        public string ToolTip
        {
            set
            {
                ToolTipService.SetToolTip(this, value);
            }
        }
        public string icon = "/SilverlightLynxControls;component/Icon.png";
        public string text = "";
        public string State = "";
        

        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //this.Width = 40;
            //this.Height = 40;
            //Canvas.SetTop(this, 0);
            //Canvas.SetLeft(this, 5);
            VisualStateManager.GoToState(this, "Active", true);
            LayoutRoot.Background = new SolidColorBrush(Colors.Yellow);
            LayoutRoot.Opacity = 0.3;
            
        }

        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //this.Width = 30;
            //this.Height = 30;
            //Canvas.SetTop(this, 20);
            //Canvas.SetLeft(this, 10);
            VisualStateManager.GoToState(this, "DeActive", true);
            LayoutRoot.Background = new SolidColorBrush(Colors.Transparent);

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            LynxWindow.ShowWindow(tu,FindDesktop(this),cp);
        }

        public Canvas FindDesktop(DependencyObject o)
        {
            //UIElement ui = Application.Current.RootVisual;
            
            DependencyObject po=VisualTreeHelper.GetParent(o);
            if (po == null) { return null; }
            if (po.GetType().Name == "Canvas")
            {
                return po as Canvas;
            }
            return FindDesktop(po);

        }

        private void UserControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            cp = e.GetCurrentPoint(null).Position;
        }
    }
}
