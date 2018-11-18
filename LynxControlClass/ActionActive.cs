using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SilverlightLynxControls
{
    public class ActionActive//激活当前的对象，让其在最前面显示
    {
        
        public static int getMaxZIndex(FrameworkElement o)
        {
            if (o.Parent == null) { return -1; }
            Type ot = o.Parent.GetType();
            int z = 0;
            if (ot.Name == "Canvas")
            {
                Canvas pp = (Canvas)o.Parent;
                
                foreach (FrameworkElement f in pp.Children)
                {
                    if (Canvas.GetZIndex(f) > z)
                    {
                        z = Canvas.GetZIndex(f);
                    }
                }

            }
            return z;
        }

        public static void Active(FrameworkElement o)
        {
            int z = ActionActive.getMaxZIndex(o);
            Canvas.SetZIndex(o, z+1);
        }
    }
}
