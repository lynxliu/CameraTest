using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLFC.common;
using SilverlightLynxControls;
using ImageTest;


using SLPhotoTest.PhotoEdit;
using SLPhotoTest.UIControl;
using System.Net.NetworkInformation;
using System.Threading;
using DCTestLibrary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Popups;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace SLPhotoTest
{
    public partial class CameraTestDesktop : UserControl//桌面，支持基本的定位操作
    {
        private CameraTestDesktop()
        {
            InitializeComponent();
            CurrentVersion = Version.Professional;//默认是专业版
            Init();
        }
        DispatcherTimer desktimer = new DispatcherTimer();//系统的桌面时钟
        public static CameraTestIcon SelectedTest;
        Image SelectedImage;
        public StackPanel ClipList { get { return clipList; } }
        public StackPanel TaskList { get { return taskList; } }
        public List<WriteableBitmap> getClipList()
        {
            List<WriteableBitmap> bl = new List<WriteableBitmap>();
            foreach (Image im in ClipList.Children)
            {
                bl.Add(WriteableBitmapHelper.Clone(im.Source as WriteableBitmap));
            }
            return bl;
        }
        public WriteableBitmap getClip()
        {
            if ((SelectedImage == null) || (SelectedImage.Source == null) || !(SelectedImage.Source is BitmapSource)) { SilverlightLFC.common.Environment.ShowMessage("粘帖图片为空"); return null; }
            return  WriteableBitmapHelper.Clone(SelectedImage.Source as WriteableBitmap);
        }
        public void addClip(WriteableBitmap b)
        {
            if (b == null)
            {
                var md=new MessageDialog("复制图片为空");
                var x = md.ShowAsync();
                return;
            }
            foreach (FrameworkElement fe in ClipList.Children)
            {
                fe.Opacity = 0.65;
            }
            Image im = new Image();
            im.Width = 35;
            im.Height = 35;
            im.Source = b;
            im.Stretch = Stretch.Uniform;
            SelectedImage = im;
            im.Opacity = 1;
            im.Tag = ClipList.Children.Count;
            im.PointerPressed += new PointerEventHandler(im_PointerPressed);
            im.PointerEntered += new PointerEventHandler(im_PointerEntered);
            //im.PointerExited += new PointerEventHandler(im_PointerExited);
            im.PointerMoved += new PointerEventHandler(im_PointerMoved);
            ClipList.Children.Add(im);
        }

        void im_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            cp.Visibility = Visibility.Visible;
            //throw new NotImplementedException();
        }
        ClipView cp = new ClipView();
        void im_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            cp.Visibility = Visibility.Collapsed;
            //throw new NotImplementedException();
        }

        void im_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Image im=sender as Image;
            cp.Visibility = Visibility.Visible;
            double l=PhotoTest.Width+this.TaskList.Width+im.Width*Convert.ToInt32(im.Tag);
            Canvas.SetLeft(cp, l);
            Canvas.SetTop(cp, Desk.Height - cp.Height);
            cp.image1.Source = im.Source;
            cp.Tag = im;
            //throw new NotImplementedException();
        }

        void im_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
            foreach (FrameworkElement fe in ClipList.Children)
            {
                fe.Opacity = 0.65;
            }
            SelectedImage = sender as Image;
            SelectedImage.Opacity = 1;
            //throw new NotImplementedException();
        }
        public void Init()//初始化桌面和任务栏内容
        {
            //LayoutRoot.Children.Add(new PriceCompanyShow());
            //LayoutRoot.Children.Add(new TestToolBox());
            CheckEnable();
            if (!Desk.Children.Contains(cp))
            {
                Desk.Children.Add(cp);
            }
            desktimer.Tick += desktimer_Tick;
            desktimer.Interval = TimeSpan.FromSeconds(1);
            if(!Desk.Children.Contains(currentProject))
                Desk.Children.Add(currentProject);
            //desktimer.Start();
            
            
            //LynxActionBar la = new LynxActionBar();

            //LynxButton lb;
            //lb= new LynxButton(new Page());
            //lb.text = "PhotoAbility";
            //lb.ToolTip = "Test photo ability";
            //la.AddIcon(lb);

            //lb = new LynxButton(new FaceTestPage());
            //lb.text = "FaceRecgonise";
            //lb.ToolTip = "Test Face ability";
            //la.AddIcon(lb);

            //lb = new LynxButton(new ShutterSpeed());
            //lb.text = "ShutterSpeed";
            //lb.ToolTip = "Test ShutterSpeed ability";
            //la.AddIcon(lb);

            //lb = new LynxButton(new SysTimer());
            //lb.text = "SysTimer";
            //lb.ToolTip = "Test SysTimer ability";
            //la.AddIcon(lb);

            //Desk.Children.Add(la);
            //LayoutRoot.Children.Add(li);
            textBlockVersion.Text = getCurrentVersionStr();
        }

        void desktimer_Tick(object sender, object e)
        {
            this.textBlockTime.Text = DateTime.Now.ToString();
            //if (SelectedTest == null)
            //{
            //    TestTime.Content = "0分钟";
            //}
            //else
            //{
            //    TestTime.Content = (DateTime.Now.Hour - SelectedTest.beginTime.Hour) * 60 + DateTime.Now.Minute - SelectedTest.beginTime.Minute;
            //}
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //FileAccess fa = new FileAccess();
            //FileAccess.CreateDirection("", "XXX");

            //Point sp = new Point(20, 30);
            //Point ep = new Point((double)button1.GetValue(Canvas.LeftProperty), (double)button1.GetValue(Canvas.LeftProperty));
            //ep = new Point(200, 500);
            //Canvas.SetLeft(button1, sp.X);
            //Canvas.SetTop(button1, sp.Y);
            //ActionAnimationShow a = new ActionAnimationShow(button1);
            //a.AnimationComplete += new AnimationCompleteEventHandler(a_AnimationComplete);
            //a.ShowZoom(3000, sp, ep);

        }

        void a_AnimationComplete(object sender, LynxAnimationCompleteEventArgs e)
        {

            //Canvas.SetLeft(button1, e.EndP.X);
            //Canvas.SetTop(button1, e.EndP.Y);
            //throw new NotImplementedException();
        }
        private static CameraTestDesktop Desktop = new CameraTestDesktop();
        public static CameraTestDesktop getDesktop()
        {
            return Desktop;
        }
        public static Canvas getDesktopPanel()
        {
            return Desktop.Desk;
        }
        public static StackPanel getTaskBar()
        {
            return Desktop.TaskList;
        }
        public static void ActiveUserControl(FrameworkElement u)
        {
            ActionActive.Active(u);
            //int max = 0;
            //Panel p=getDesktopPanel();
            //foreach (FrameworkElement f in p.Children)
            //{
            //    if (Canvas.GetZIndex(f) > max)
            //    {
            //        max = Canvas.GetZIndex(f);
            //    }

            //}
            //Canvas.SetZIndex(u, max++);
        }

        private void LayoutRoot_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        public  void DeActiveAll()
        {
            SelectedTest = null;
            foreach (FrameworkElement f in TaskList.Children)
            {
                CameraTestIcon tt = f as CameraTestIcon;
                if (tt != null)
                {
                    tt.DeActive();
                }
            }
        }

        private void BeginTest_Click(object sender, RoutedEventArgs e)
        {
            DeActiveAll();
            CameraTestProject cp = new CameraTestProject();
            Desk.Children.Add(cp);
            //cp.Active();
        }

        private void PhotoEditTool_Click(object sender, RoutedEventArgs e)
        {
            //ImageEdit f = new ImageEdit();
            //Desk.Children.Add(f);
            //Canvas.SetLeft(f, (Desk.ActualWidth - f.Width) / 2);
            //Canvas.SetTop(f, (Desk.ActualHeight - f.Height) / 2);
            PhotoEditManager pm = PhotoEditManager.getPhotoEditManager();
            pm.StartPhotoEdit();
        }

        public static Point getPosition(UIElement u)
        {
            GeneralTransform gt = u.TransformToVisual(CameraTestDesktop.getDesktop());
            return gt.TransformPoint(new Point());
        }

        private void buttonNote_Click(object sender, RoutedEventArgs e)
        {
            SilverlightEduProcessManagerUI.Note.LynxNotepad n = new SilverlightEduProcessManagerUI.Note.LynxNotepad();
            Desk.Children.Add(n);
        }

        private void PhotoTest_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            PhotoTest.Height = 50;
            PhotoTest.Margin = new Thickness(0, -15, 0, 0);
        }

        private void PhotoTest_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            PhotoTest.Height = 45;
            PhotoTest.Margin = new Thickness(0, -8, 0, 0);
        }
        PhotoTestMenu ptm = new PhotoTestMenu();
        //CameraTestMenu ctm = new CameraTestMenu();

        private void CameraTest_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CameraTest.Height = 50;
            CameraTest.Margin = new Thickness(0, -15, 0, 0);
        }

        private void CameraTest_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CameraTest.Height = 45;
            CameraTest.Margin = new Thickness(0, -8, 0, 0);
        }

        private void PhotoTest_Click(object sender, RoutedEventArgs e)
        {
            if (Desk.Children.Contains(ptm)) {
                Desk.Children.Remove(ptm);
            }
            else
            {
                Desk.Children.Add(ptm);
                Canvas.SetLeft(ptm,2);
                Canvas.SetTop(ptm, Desk.Height - ptm.Height);
            }
        }

        private void CameraTest_Click(object sender, RoutedEventArgs e)
        {
            if (Desk.Children.Contains(ctm))
            {
                Desk.Children.Remove(ctm);
            }
            else
            {
                Desk.Children.Add(ctm);
                Canvas.SetLeft(ctm, 55);
                Canvas.SetTop(ctm, Desk.Height - ctm.Height);
            }
        }

        private void buttonPhotoEdit_Click(object sender, RoutedEventArgs e)
        {
            PhotoEditManager pm = PhotoEditManager.getPhotoEditManager();
            pm.StartPhotoEdit();
        }

        private void buttonVideoShow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void textBlockTime_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
        }


        public CameraTestProject currentProject = new CameraTestProject();

        private void image1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!Desk.Children.Contains(currentProject))
                Desk.Children.Add(currentProject);
            ActionActive.Active(currentProject);
        }

        private void canvasAbout_MouseRightButtonDown(object sender, PointerRoutedEventArgs e)
        {
            About a = new About();
            ActionShow.CenterShow(Desk, a);
        }

        #region Version//版本相关，分别为免费版，自动测试版，专业版
        static Version CurrentVersion;//默认是专业的全功能版本
        //免费版只能进行简单的指标测试，自动测试版不能修改参数
        public static Version getCurrentVersion()
        {
            return CurrentVersion;
        }
        public static string getCurrentVersionStr()
        {
            if(CurrentVersion==Version.Professional){return "专业版";}
             if(CurrentVersion==Version.Professional){return "简化版";}
            return "共享版";
        }
        #endregion

        private void image2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            textBlockTime.Text = DateTime.Now.ToString();
            textBlockTime.Visibility = Visibility.Visible;
            Canvas.SetTop(textBlockTime, Canvas.GetTop(image2));
            Canvas.SetLeft(textBlockTime, Canvas.GetLeft(image2) + image2.Width + 5);
        }
 
        LynxTimerManager ltm = new LynxTimerManager();

        LynxNetService lns = new LynxNetService();

        DispatcherTimer t = new DispatcherTimer();

        
        public void CheckEnable()//检查是否可以运行
        {
            if (!IsEnableCheck) { return; }
            t.Interval = TimeSpan.FromSeconds(60);//每分钟测试一次
            t.Tick += (t_Tick);
            t.Start();

        }

        void ShowException()//当程序不可使用的时候锁死程序
        {
            Desk.Children.Clear();
            StackBar.Children.Clear();
            ActionShow.CenterShow(Desk,new SystemSupport());
        }

        void t_Tick(object sender, object e)//检测程序在线状态
        {
            bool state = NetworkInterface.GetIsNetworkAvailable() ? true : false;

            if (!state)
            {
                SilverlightLFC.common.Environment.ShowMessage("程序必须在线状态才可使用！");
                ShowException();
            }
            syn = SynchronizationContext.Current;

            ltm.getInternetTime(CheckTime);

        }
        
        SynchronizationContext syn;
 
        void CheckTime(DateTime? dt)
        {
            if (dt == null)
            {
                //SilverlightLFC.common.Environment.ShowMessage("程序必须在线状态才可使用！");
                //App.Current.MainWindow.Close();
                syn.Post(AlertMessage, "程序必须在线状态才可使用！");
                ShowException();
                return;
            }
            if (dt > new DateTime(2012, 1, 1))//最后评估时间
            {
                syn.Post(AlertMessage, "程序以过期，请更新！");
                ShowException();
            }
        }
        static void AlertMessage(object s)
        {
            SilverlightLFC.common.Environment.ShowMessage(s.ToString());
        }
        bool IsEnableCheck = true;//决定是否进行检查，默认是不做检查

        public static bool CanAccess(Version PassVersion)
        {
            Version CurrentV = getCurrentVersion();
            if (CurrentV == Version.Professional) //具备全部的权限
            { return true; }
            if (CurrentV == Version.Standard)
            {
                if (PassVersion == Version.Professional)
                {
                    SilverlightLFC.common.Environment.ShowMessage("当前版本不支持此功能，请升级到专业版！");
                    return false;
                }
                return true;
            }
            if (CurrentV == Version.Free)
            {
                if (PassVersion == Version.Free)
                {
                    return true;
                }
                SilverlightLFC.common.Environment.ShowMessage("当前版本不支持此功能，请升级到标准版或者专业版！");
                return false;
            }
            return true;
        }

        private void comboBoxLabMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxLabMode == null) return;
            if (comboBoxLabMode.SelectedIndex == 0)
            {
                ColorManager.CurrentLanMode = LabMode.CIE;
            }
            if (comboBoxLabMode.SelectedIndex == 1)
            {
                ColorManager.CurrentLanMode = LabMode.Photoshop;
            }
        }

    }
    public enum Version
    {
        Free,Standard,Professional
    }
}
