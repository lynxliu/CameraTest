using DCTestLibrary;
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
    public sealed partial class LynxPhotoTestView : UserControl
    {
        public LynxPhotoTestView()
        {
            this.InitializeComponent();
            this.ManipulationDelta += LynxControl.FrameworkElement_ManipulationDelta;
            this.Tapped += LynxControl.FrameworkElement_Tapped;
            this.DoubleTapped += LynxControl.FrameworkElement_DoubleTapped;
        }

        


        public Point StartPoint { get; set; }
        public Size CurrentSize { get; set; }
        public void Default()
        {
            Canvas.SetLeft(this, StartPoint.X);
            Canvas.SetTop(this, StartPoint.Y);
            Width = CurrentSize.Width;
            Height = CurrentSize.Height;
        }

        public void Max()
        {
            var parent = Parent as Panel;
            if (parent != null)
            {
                StartPoint = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
                CurrentSize = new Size(Width, Height);
                Canvas.SetLeft(this, 0);
                Canvas.SetTop(this, 0);
                Width = parent.Width;
                Height = parent.Height;
            }
        }

        public void Close()
        {
            var parent = Parent as Panel;
            if (parent != null)
                parent.Children.Remove(this);
        }

    }
}
