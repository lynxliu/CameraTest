using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;




namespace SilverlightLynxControls
{
    public partial class LynxWindowIcon : UserControl
    {
        public LynxWindowIcon()
        {
            InitializeComponent();
            ToolTipService.SetToolTip(this, Text);
            am = new ActionMove(this, Title);
            am.Enable = true;
            
        }
        ActionMove am;
        ActionAnimationShow aas;
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
            Panel p = Parent as Panel;
            if (p == null) { return; }
            if (!p.Children.Contains(lynxWin))
            {
                p.Children.Add(lynxWin);
            }
            aas = new ActionAnimationShow(lynxWin);
            aas.ShowZoomProjection(2000,new Point(Canvas.GetLeft(this),Canvas.GetTop(this)),new Point(Canvas.GetLeft(lynxWin),Canvas.GetTop(lynxWin)));
            aas.AnimationComplete += new AnimationCompleteEventHandler(aas_AnimationComplete);

        }

        void aas_AnimationComplete(object sender, LynxAnimationCompleteEventArgs e)
        {
            aas.AnimationComplete -= new AnimationCompleteEventHandler(aas_AnimationComplete);

            Panel p = Parent as Panel;
            p.Children.Remove(this);
            if (!p.Children.Contains(lynxWin))
            {
                p.Children.Add(lynxWin);
            }
        }
        public FrameworkElement lynxWin { get; set; }
        string Text="";


        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {

        }
        public static Panel FindDesktop()
        {
            //UIElement ui = Application.Current.RootVisual;
            return (Panel)VisualTreeHelper.GetChild(Window.Current.Content, 0);
        }
        public void ArrageIcon(Panel p)
        {
            p = Parent as Panel;
            if (p == null)
            {
                p = FindDesktop();
            }
            if (p == null) { return; }
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
