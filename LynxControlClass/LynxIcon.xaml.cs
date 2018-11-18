using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using System.Reflection;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.Devices.Input;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls.Primitives;

namespace SilverlightLynxControls
{
    public enum LynxIconSizeType
    {
         x10=10,x20=20,x30=30,x40=40,x50=50
    }

    public partial class LynxIcon : UserControl
    {
        public LynxIcon()
        {
            InitializeComponent();
            ToolTipService.SetToolTip(this, Text);
            am = new ActionMove(this, this);
        }
        ActionMove am;
        public LynxIcon(LynxWindow lw)
        {
            InitializeComponent();
            lynxWin = lw;
            //Tip = lw.TitleText.Text;
            am = new ActionMove(this, this);
        }
        public void ActiveWindow(Panel p)
        {
            if (lynxWin == null)
            {
                lynxWin = new LynxWindow();
            }
            if (p != null)
            {
                ActionAnimationShow aas = new ActionAnimationShow(this);
                aas.HideZoomProjection(2000);
                p.Children.Add(lynxWin);
            }
        }
        public void ToWindow()
        {
            if (lynxWin == null)
            {
                lynxWin = new LynxWindow();
            }
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                ActionAnimationShow aas = new ActionAnimationShow(this);
                aas.HideZoomProjection(2000);
                p.Children.Add(lynxWin);
                p.Children.Remove(this);
            }
        }
        LynxWindow lynxWin;
        string Text;
        LynxIconSizeType IconSize = LynxIconSizeType.x30;
        public void ShowIcon()
        {
            this.Width = (double)IconSize;
            this.Height = (double)IconSize;
        }

        public LynxIconSizeType Size
        {
            get
            {
                return IconSize;
            }
            set
            {
                IconSize = value;
            }
        }

        public string Tip
        {
            get
            {
                return Text;
            }
            set
            {
                Text = value;
                ToolTipService.SetToolTip(this, Text);
            }
        }

        public ImageSource Icon
        {
            get
            {
               return IconImage.Source;

            }
            set
            {
                IconImage.Source = value;
            }
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            ChildWindowAbout a = new ChildWindowAbout();
            a.Show();
        }
        public static Panel FindDesktop()
        {
            //UIElement ui = Application.Current.RootVisual;
            return (Panel)VisualTreeHelper.GetChild(Window.Current.Content, 0);
        }
        public static void ArrageIcon(Panel p)
        {
            if (p == null)
            {
                p = FindDesktop();
            }
            double l = 0, t = p.ActualHeight;
            int z = 0;
            for (int i = 0; i < p.Children.Count; i++)
            {
                UIElement ue = p.Children[i];
                if (ue.GetType().Name == "LynxIcon")
                {
                    ue.SetValue(Canvas.LeftProperty, l);
                    ue.SetValue(Canvas.TopProperty, t);
                    ue.SetValue(Canvas.ZIndexProperty, z);
                    l = l + 50;
                    if (l > p.ActualWidth - 50)
                    {
                        t = t - 50;
                        l = 0;
                    }
                    z++;
                }
            }
        }

        private void ArrageButton_Click(object sender, RoutedEventArgs e)
        {
            LynxIcon.ArrageIcon(null);
        }

        private void MenuToIcon_Click(object sender, RoutedEventArgs e)
        {
            ToWindow();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Panel c = (Panel)this.Parent;
            c.Children.Remove(this);
        }

        private void LayoutRoot_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            LynxWindowMenu.IsOpen = !LynxWindowMenu.IsOpen;
        }

        public static void ShowAsIcon(FrameworkElement fe, FrameworkElement wfe)
        {
            LynxWindow lw = new LynxWindow();
            lw.setContent(wfe);
            LynxIcon li = new LynxIcon(lw);
            li.IconMain.Children.Clear();
            li.IconMain.Children.Add(fe);
            li.Width = fe.Width;
            li.Height = li.Height;
        }

        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Active", true);
        }

        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Nomarl", true);
        }
    }
}
