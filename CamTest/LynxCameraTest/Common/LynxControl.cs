using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace PhotoTestControl
{
    public class LynxControl
    {
        static int getTop(Panel parent)
        {
            if (parent == null) return 0;
            return (int)parent.Children.Max(v => v.GetValue(Canvas.ZIndexProperty));
        }
        public static void FrameworkElement_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;
            element.SetValue(Canvas.ZIndexProperty, getTop(element.Parent as Panel) + 1);
            CompositeTransform transform = element.RenderTransform as CompositeTransform;
            if (transform == null)
            {
                transform = new CompositeTransform();
                element.RenderTransform = transform;
            }
            transform.ScaleX *= e.Delta.Scale;
            transform.ScaleY *= e.Delta.Scale;
            transform.CenterX = element.RenderSize.Width / 2;
            transform.CenterY = element.RenderSize.Height / 2;
            //transform.Rotation += e.Delta.Rotation * 180 / Math.PI;
            transform.TranslateX += e.Delta.Translation.X;
            transform.TranslateY += e.Delta.Translation.Y;


            e.Handled = true;

        }

        public static void FrameworkElement_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;
            Panel p = element.Parent as Panel;
            if (p == null) return;
            element.RenderTransform = null;
            element.Width = p.Width;
            element.Height = p.Height;
        }

        public static void FrameworkElement_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null) return;
            Panel p = element.Parent as Panel;
            if (p == null) return;
            element.SetValue(Canvas.ZIndexProperty, getTop(element.Parent as Panel) + 1);

        }
    }
}
