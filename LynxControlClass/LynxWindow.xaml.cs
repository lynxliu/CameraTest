using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;


namespace SilverlightLynxControls
{
    public partial class LynxWindow : UserControl
    {
        public LynxIcon LynxIcon;
        public LynxWindow()
        {
            InitializeComponent();
            am = new ActionMove(this, TitleBar);
            //ar = new ActionResize(LWindow, LWindow);
            ar = new ActionResize(LWindow, LClipWindow);
            //LWindow.Width = CurrentWidth;
            //LWindow.Height = CurrentHeight;
        }
        ActionMove am;
        ActionResize ar;
        public Panel ParentPanel;
        
        public bool IsMove
        {
            get
            {
                return _IsMove;
            }
            set
            {
                _IsMove = value;
            }
        }

        public double BorderWidth
        {
            get
            {
                return _BorderWidth;
            }
            set
            {
                _BorderWidth = value;
            }
        }

        public double CurrentHeight
        {
            get
            {
                return _CurrentHeight;
            }
            set
            {
                _CurrentHeight = value;
            }
        }

        public double CurrentWidth
        {
            get
            {
                return _CurrentWidth;
            }
            set
            {
                _CurrentWidth = value;
            }
        }

        public double TitleHeight
        {
            get
            {
                return _MoveTitleHeight;
            }
            set
            {
                _MoveTitleHeight = value;
            }
        }

        public bool IsBottomChange
        {
            get
            {
                return _IsBottomChange;
            }
            set
            {
                _IsBottomChange = value;
            }
        }

        public bool IsLeftChange
        {
            get
            {
                return _IsLeftChange;
            }
            set
            {
                _IsLeftChange = value;
            }
        }

        public bool IsRightChange
        {
            get
            {
                return _IsRightChange;
            }
            set
            {
                _IsRightChange = value;
            }
        }

        public bool IsTopChange
        {
            get
            {
                return _IsTopChange;
            }
            set
            {
                _IsTopChange = value;
            }
        }

        public bool _IsLeftChange = false;//判断是不是左侧的缩放位置
        public bool _IsRightChange = false;
        public bool _IsTopChange = false;
        public bool _IsBottomChange = false;

        public bool _IsMove = false;

        public double _BorderWidth = 3;//边界的像素数
        public double _MoveTitleHeight = 30;//移动区的像素数

        public double _CurrentWidth = 640d;
        public double _CurrentHeight = 480d;

        public void setCurrentSize(double w, double h)
        {
            CurrentHeight = h;
            CurrentWidth = w;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Panel c = (Panel)this.Parent;
            c.Children.Remove(this);

            //this.Finalize();
        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            Image im = (Image)MinButton.Content;
            BitmapImage b = (BitmapImage)im.Source;
            if (b.UriSource.OriginalString == "Min.png")
            {
                CurrentWidth = this.Width;
                CurrentHeight = this.Height;
                //this.Height = TitleBar.Height + 6;
                Uri uri = new Uri("Resume.png", UriKind.RelativeOrAbsolute);
                b.UriSource = uri;
                ToIcon();
            }
            else
            {
                this.Width = CurrentWidth;
                this.Height = CurrentHeight;
                Uri uri = new Uri("Min.png", UriKind.RelativeOrAbsolute);
                b.UriSource = uri;

            }
        }
        private void LynxWindow_SizeChanged(object sender, SizeChangedEventArgs e)//重新锁定整个的窗体结构
        {
            double w, h;
            if ((this.Width - 30 - 4 - 100) > 10)
            {
                w = this.Width;
            }
            else
            {
                w = 10;
            }
            if (this.Height > 35) { h = this.Height; }
            else
            {
                h = 35;
            }
            Size ws = this.RenderSize;
            LynxFrameWork.Width = ws.Width;
            LynxFrameWork.Height = ws.Height;
            //LynxFrameWork.SetValue(Canvas.LeftProperty, 0d);
            //LynxFrameWork.SetValue(Canvas.TopProperty, 0d);

            LynxForm.Width = ws.Width;
            //LynxForm.SetValue(Canvas.LeftProperty, 3d);
            //LynxForm.SetValue(Canvas.TopProperty, TitleBar.ActualHeight + 3);
            LynxForm.Height = ws.Height - 30;

            TitleBar.Width = ws.Width;
            //TitleBar.SetValue(Canvas.LeftProperty, 0d);
            //TitleBar.SetValue(Canvas.TopProperty, 0d);
            LWindow.Width = ws.Width;
            LWindow.Height = ws.Height;
            LClipWindow.Width = ws.Width;
            LClipWindow.Height = ws.Height;
            //TitleText.Width = ws.Width;

            if ((w - 30 - 4 - 100) > 10)
            {
                TitleText.Width = this.Width - 4 - 100;
            }
            else
            {
                TitleText.Width = 10;
            }
            ar.setSize(ws.Width, ws.Height);
            //RightBorder.Height = this.ActualHeight;
            //RightBorder.SetValue(Canvas.LeftProperty, this.ActualWidth - 3);
            //RightBorder.SetValue(Canvas.TopProperty, 0d);

            //LeftBorder.Height = this.ActualHeight;
            //LeftBorder.SetValue(Canvas.LeftProperty, 0d);
            //LeftBorder.SetValue(Canvas.TopProperty, 0d);

            //TopBorder.Width = this.ActualWidth;
            //TopBorder.SetValue(Canvas.LeftProperty, 0d);
            //TopBorder.SetValue(Canvas.TopProperty, 0d);

            //BottomBorder.Width = this.ActualWidth;
            //BottomBorder.SetValue(Canvas.LeftProperty, 0d);
            //BottomBorder.SetValue(Canvas.TopProperty, this.ActualHeight - 3);


        }

        public void setContent(FrameworkElement u)
        {
            LynxForm.Children.Clear();
            //u.Arrange(new Rect(0,0,LynxForm.Width,LynxForm.Height));
            if (!Double.IsNaN(u.Width))
            {
                LynxForm.Width = u.Width;
                CurrentWidth = u.Width;
            }
            else
            {
                LynxForm.Width = CurrentWidth;
            }
            if (!Double.IsNaN(u.Height))
            {
                LynxForm.Height = u.Height;
                CurrentHeight = u.Height;
            }
            else
            {
                LynxForm.Height = CurrentHeight;
            }
            
            //DesignWidth

            LynxForm.Children.Add(u);
            // u.Arrange(new Rect(0, 0, this.CurrentWidth, this.CurrentHeight));

            //FrameworkElement tfe= VisualTreeHelper.GetChild(u,0) as FrameworkElement;
            LWindow.Width = LynxForm.Width;
            LWindow.Height = LynxForm.Height;

            LClipWindow.Width = LynxForm.Width;
            LClipWindow.Height = LynxForm.Height+30;

            this.Width = LynxForm.Width;
            this.Height = LynxForm.Height + 30;

            ar.setSize(this.Width, this.Height);
            //u.Height = LynxForm.Height;
            //u.Width = LynxForm.Width;

        }

        public void deleteContent(FrameworkElement u)
        {
            LynxForm.Children.Remove(u);
        }

        public static LynxWindow FindLynxWindow(FrameworkElement c)
        {
            UIElement root = Window.Current.Content;
            FrameworkElement tc = (FrameworkElement)c.Parent;
            if (tc == null) { return null; }
            if (c.Parent != root)
            {
                if (tc.GetType().Name == "LynxWindow")
                {
                    return (LynxWindow)tc;
                }
                return FindLynxWindow(tc);
            }
            return null;
        }

        public static LynxWindow getWindow(FrameworkElement fe)
        {
            LynxWindow lw = new LynxWindow();
            lw.setContent(fe);
            //ActionAnimationShow.ShowZoomProjection(lw, 2000);
            return lw;
        }

        public static void ShowWindow(FrameworkElement fe)
        {
            LynxWindow lw = FindLynxWindow(fe);
            Panel p;
            if (lw != null)
            {
                p = lw.Parent as Panel; ;
            }
            else
            {
                p = FindParentPanel(fe);
            }
            if (p == null) { p = FindDesktop(); }
            ShowWindow(fe, p);
        }

        public static void ShowWindow(FrameworkElement fe,Panel p)
        {
            LynxWindow lw = new LynxWindow();
            //lw.setContent(fe);
            Point sp = new Point(p.ActualWidth / 2, p.ActualHeight / 2);
            Point ep = new Point((p.ActualWidth-lw.Width) / 2, (p.ActualHeight-lw.Height) / 2);
            ShowWindow(fe, p, sp,ep);
            //p.Children.Add(lw);
            //ActionAnimationShow aas = new ActionAnimationShow(lw);
            //aas.ShowZoom(2000);
            //return lw;
        }

        public static void ShowWindow(FrameworkElement fe, Panel p, Point SP)
        {
            LynxWindow lw = new LynxWindow();
            //lw.setContent(fe);
            p.Children.Add(lw);
            Point ep = new Point((p.ActualWidth - lw.Width) / 2, (p.ActualHeight - lw.Height) / 2);
            ActionAnimationShow aas = new ActionAnimationShow(lw);
            aas.ShowZoom( 2000, SP, ep);
        }

        public static void ShowWindow(FrameworkElement fe, Panel p,Point SP,Point EP)
        {
            LynxWindow lw = new LynxWindow();
            lw.ParentPanel = p;
            //lw.LynxForm.Children.Add(fe);
            lw.setContent(fe);
            p.Children.Add(lw);
            Canvas.SetLeft(lw, SP.X);
            Canvas.SetTop(lw, SP.Y);
            ActionAnimationShow aas = new ActionAnimationShow(lw);
            aas.AnimationComplete += new AnimationCompleteEventHandler(aas_AnimationComplete);
            aas.ShowZoom(2000, SP, EP);


            //return lw;
        }
        //static Point ep;
        //static LynxWindow clw;
        static void aas_AnimationComplete(object sender, LynxAnimationCompleteEventArgs e)
        {
            
            Canvas.SetLeft(e.AnimationObject, e.EndP.X);
            Canvas.SetTop(e.AnimationObject, e.EndP.Y);
        }

        public static void HideWindow(LynxWindow lw, Panel p)
        {
            p.Children.Remove(lw);
            ActionAnimationShow aas = new ActionAnimationShow(lw);
            aas.HideZoomProjection( 2000);
            //return lw;
        }

        public static void HideWindow(LynxWindow lw, Panel p, Point SP, Point EP)
        {
            p.Children.Remove(lw);
            ActionAnimationShow aas = new ActionAnimationShow(lw);
            aas.HideZoomProjection( 2000, SP, EP);
            //return lw;
        }

        public static Panel FindDesktop(FrameworkElement fe)
        {
            //UIElement ui = Application.Current.RootVisual;
            return (Panel)VisualTreeHelper.GetChild(Window.Current.Content, 0);

        }

        public static Panel FindParentPanel(FrameworkElement fe)
        {
            //UIElement ui = Application.Current.RootVisual;
            //return (Panel)VisualTreeHelper.GetChild(Application.Current.RootVisual, 0);
            Panel p = fe.Parent as Panel;
            if (p == null)
            {
                FrameworkElement tf = fe.Parent as FrameworkElement;
                if (tf == null) { return null; }
                else
                {
                    return FindParentPanel(tf);
                }
            }
            else
            {
                return p;
            }
        }

        public static Panel FindDesktop()
        {
            //UIElement ui = Application.Current.RootVisual;
            return (Panel)VisualTreeHelper.GetChild(Window.Current.Content, 0);
        }

        public static void ArrageWindow(Panel p)
        {
            if (p == null)
            {
                p = FindDesktop();
            }
            double l=0, t=0;
            int z = 0;
            for (int i = 0; i < p.Children.Count; i++)
            {
                UIElement ue = p.Children[i];
                if (ue.GetType().Name == "LynxWindow")
                {
                    ue.SetValue(Canvas.LeftProperty, l);
                    ue.SetValue(Canvas.TopProperty, t);
                    ue.SetValue(Canvas.ZIndexProperty, z);
                    l = l + 30;
                    t = t + 30;
                    z ++;
                }
            }
        }

        public void ActiveWindow()
        {
            //LynxDesktop ld = LynxDesktop.FindLynxDesktop(this);
            //this.SetValue(Canvas.ZIndexProperty, ld.MaxZIndex++);
            ActionActive.Active(this);
        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            LynxWindowMenu.IsOpen = !LynxWindowMenu.IsOpen;
        }

        private void ArrageButton_Click(object sender, RoutedEventArgs e)
        {

            SilverlightLynxControls.LynxWindow.ArrageWindow(null);
        }

        private void LynxWindowMenu_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            LynxWindowMenu.IsOpen = false;
        }

        public void ToIcon()
        {
            if (LynxIcon == null)
            {
                LynxIcon = new LynxIcon(this);
            }
            Panel p = this.Parent as Panel;
            if (p != null)
            {
                ActionAnimationShow aas = new ActionAnimationShow(this);
                aas.HideZoomProjection( 2000);
                p.Children.Add(LynxIcon);
                p.Children.Remove(this);
            }
        }

        private void MenuToIcon_Click(object sender, RoutedEventArgs e)
        {
            ToIcon();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            ChildWindowAbout a = new ChildWindowAbout();
            a.Show();
        }

        private void IconButton_Click(object sender, RoutedEventArgs e)
        {
            ToIcon();
        }

        private void ZoomInButton_Click(object sender, RoutedEventArgs e)
        {
            ActionShow.ZoomIn(this, 1.2,1.2);
        }

        private void ZoomOutButton_Click(object sender, RoutedEventArgs e)
        {
            ActionShow.ZoomIn(this, 0.8,0.8);
        }


    }
}
