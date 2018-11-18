using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;




namespace SilverlightLynxControls
{
    public partial class LynxWindowButton : UserControl
    {
        public LynxWindowButton()//包含最小化和关闭按钮
        {
            InitializeComponent();
            

        }
        ActionAnimationShow aas;
        LynxWindowIcon LIcon;
        public FrameworkElement ParentControl//包含此控件的UserControl
        {
            get;
            set;
        }
        //public Panel DeskPanel { get; set; }//桌面
        private void buttonToIcon_Click(object sender, RoutedEventArgs e)
        {
            Panel DeskPanel = ParentControl.Parent as Panel;
            if (ParentControl == null) { return; }
            if (DeskPanel == null) { return; }
            if (LIcon == null)
            {
                LIcon = new LynxWindowIcon();
            }
            LIcon.lynxWin = this.ParentControl;
            aas = new ActionAnimationShow(ParentControl);
            aas.HideZoomProjection(2000, new Point(Canvas.GetLeft(ParentControl), Canvas.GetTop(ParentControl)), new Point(Canvas.GetLeft(LIcon), Canvas.GetTop(LIcon)));
            aas.AnimationComplete += new AnimationCompleteEventHandler(aas_AnimationComplete);

        }

        void aas_AnimationComplete(object sender, LynxAnimationCompleteEventArgs e)
        {
            aas.AnimationComplete -= new AnimationCompleteEventHandler(aas_AnimationComplete);

            Panel DeskPanel = ParentControl.Parent as Panel;
            DeskPanel.Children.Remove(ParentControl);
            if (!DeskPanel.Children.Contains(LIcon))
            {
                DeskPanel.Children.Add(LIcon);
                Canvas.SetLeft(LIcon, Canvas.GetLeft(ParentControl));
                Canvas.SetTop(LIcon, Canvas.GetTop(ParentControl));
            }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            if (ParentControl == null) { return; }
            Panel p = ParentControl.Parent as Panel;
            if (p != null)
            {
                p.Children.Remove(ParentControl);
                if (LIcon != null)
                {
                    if(p.Children.Contains(LIcon))
                    {
                        p.Children.Remove(LIcon);
                    }
                }
            }

        }
    }
}
