using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLFC.common;
using SilverlightLynxControls.TimeLine;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.Foundation;

namespace SilverlightLynxControls
{
    public delegate void TimeLineResized(double l, double w, LynxTime BeginTime, LynxTime TimeLong);
    public partial class LynxTimeLine : UserControl
    {
        public LynxTimeLine()//可以交互的时间线
        {
            InitializeComponent();
            ShowXUnit();

            canvasTimeLine.PointerPressed += new PointerEventHandler(canvasTimeLine_PointerPressed);
            canvasTimeLine.PointerReleased += new PointerEventHandler(canvasTimeLine_PointerReleased);
            canvasTimeLine.PointerMoved += new PointerEventHandler(canvasTimeLine_PointerMoved);
            RectangleGeometry r = new RectangleGeometry();
            r.Rect = new Rect(0, 0, Width, Height);
            Clip = r;
        }

        public LynxTime XShowBeginTime=new LynxTime(2000,1,1,0,10,0,0);
        public LynxTime XShowEndTime=new LynxTime(2000,1,15);
        public double MinSpace=200;
        //public double MaxSpace = 170;

        #region Zoom

        void ZoomOutScale( double zoomFraction)
        {
            if (zoomFraction > 0.0001 && zoomFraction < 1000.0)
            {
                double range = (XShowEndTime.getDoubleValue() - XShowBeginTime.getDoubleValue()) * zoomFraction / 2.0;

                XShowBeginTime = XShowBeginTime - Convert.ToInt64(range);
                XShowEndTime = XShowEndTime + Convert.ToInt64(range);
                canvasTimeLine.Children.Clear();
                Marks.Clear();
                ll.Clear();
                ShowXUnit();

                foreach (Panel p in SynchronizationPanelList)
                {
                    //Canvas.SetLeft(p, Canvas.GetLeft(p) * (1 - zoomFraction / 2));
                    //p.Width = p.Width * (1 - zoomFraction);
                    foreach (FrameworkElement u in p.Children)
                    {
                        if (u is ITimeLineControl)
                        {
                            //Canvas.SetLeft(u, Canvas.GetLeft(u) * (1 - zoomFraction / 2));
                            SynchronizationControl(u as ITimeLineControl);
                            //u.Width = u.Width * (1 - zoomFraction);
                        }
                    }
                }
            }            
        }
        void ZoomInScale(double zoomFraction)
        {
            if (zoomFraction > 0.0001 && zoomFraction < 1000.0)
            {
                double range = (XShowEndTime.getDoubleValue() - XShowBeginTime.getDoubleValue()) * zoomFraction / 2.0;

                XShowBeginTime = XShowBeginTime + Convert.ToInt64(range);
                XShowEndTime = XShowEndTime - Convert.ToInt64(range);
                canvasTimeLine.Children.Clear();
                Marks.Clear();
                ll.Clear();
                ShowXUnit();
                foreach (Panel p in SynchronizationPanelList)
                {
                    //Canvas.SetLeft(p, Canvas.GetLeft(p) * (1+zoomFraction/2));
                    //p.Width = p.Width * (1 + zoomFraction);
                    foreach (FrameworkElement u in p.Children)
                    {
                        if (u is ITimeLineControl)
                        {
                            SynchronizationControl(u as ITimeLineControl);
                        }
                        //Canvas.SetLeft(u, Canvas.GetLeft(u) * (1 + zoomFraction/2));
                        //u.Width = u.Width * (1 + zoomFraction);
                    }
                }
            }
        }

        public void setTimeLineWidth(double MinWidth, LynxTime MinLong)//通过确定最小宽度来确定时间线的宽度
        {
            double minw = getLynxTimeWidth(MinLong);
            if (minw > MinWidth) { return; }
            canvasTimeLine.Width = (XShowEndTime - XShowBeginTime).getDoubleValue() / MinLong.getDoubleValue() * MinWidth;
            foreach (Panel p in SynchronizationPanelList)
            {
                p.Width = canvasTimeLine.Width;
            }
            ShowXUnit();
        }

        #endregion

        #region TimePositionTrans//时间和位置的折算
        public LynxTime getLynxTimeValue(double x)//从位置获取时间
        {
            LynxTime d = (XShowEndTime - XShowBeginTime) * x / canvasTimeLine.Width + XShowBeginTime;
            return d;
        }

        public double getLynxTimeXValue(LynxTime d)//从时间获取位置
        {
            return (d - XShowBeginTime).getDoubleValue() / (XShowEndTime - XShowBeginTime).getDoubleValue() * canvasTimeLine.Width;
        }

        public double getLynxTimeWidth(LynxTime TimeLong)//时间差获取间隔
        {
            LynxTime d = XShowEndTime - XShowBeginTime;
            return Math.Abs(Convert.ToDouble(TimeLong.getDoubleValue()) / d.getDoubleValue()) * canvasTimeLine.Width;
        }

        public double getLynxTimeWidth(LynxTime bt, LynxTime et)//时间差获取间隔
        {
            LynxTime v = et - bt;
            LynxTime d = XShowEndTime - XShowBeginTime;
            return Math.Abs(Convert.ToDouble(v.getDoubleValue()) / d.getDoubleValue()) * canvasTimeLine.Width;
        }

        public double getLynxTimeWidth(TimeDimention td)//时间差获取间隔对应的宽度
        {
            LynxTime v = LynxTime.getUnitSpan(1,td);
            return Math.Abs(Convert.ToDouble(v.getDoubleValue()) / (XShowEndTime.getDoubleValue()-XShowBeginTime.getDoubleValue())) * canvasTimeLine.Width;
        }

        public LynxTime getLynxTimeSpan(double w)//从区间折算时间间隔
        {
            LynxTime d = (XShowEndTime - XShowBeginTime) * (w / canvasTimeLine.Width);
            return d;
        }

        public LynxTime getLynxTimeSpan(double sp, double ep)//从区间折算时间间隔
        {
            LynxTime d = (XShowEndTime - XShowBeginTime)*((ep - sp) / canvasTimeLine.Width);
            return d;
        }

        public void setLynxTimeAtCenter(LynxTime lt)//把特定的时间移动到显示的中央
        {
            double TargetX = canvasTimeLine.Width / 2;
            double HalfTime = (XShowEndTime.getDoubleValue() - XShowBeginTime.getDoubleValue()) / 2;
            XShowBeginTime.setTime( lt.getDoubleValue() - HalfTime);
            XShowEndTime.setTime( lt.getDoubleValue() + HalfTime);
            ShowXUnit();
        }

        #endregion
        
        TimeDimention maxDimention;
        LynxTime FirstTopUnitTime;
        LynxTime LastTopUnitTime;

        #region ShowTime
        
        public void ShowXUnit()
        {
            Marks.Clear();
            canvasTimeLine.Children.Clear();
            maxDimention = LynxTime.getMaxCrossUnit(XShowBeginTime, XShowEndTime);
            FirstTopUnitTime = XShowBeginTime.getPastTime(maxDimention);
            LastTopUnitTime = XShowEndTime.getNextTime(maxDimention);
            TimeUnitNum = LynxTime.getUnitNum(FirstTopUnitTime, LastTopUnitTime, maxDimention);
            long stepUnitNum = Convert.ToInt64(Math.Ceiling(TimeUnitNum / (double)SubNum));
            LynxTime pastTime = FirstTopUnitTime;
            ShowLabel(FirstTopUnitTime, maxDimention);
            for (int i = 1; i < SubNum; i++)
            {
                LynxTime lt = new LynxTime(FirstTopUnitTime);
                lt.AddTimeSpan(stepUnitNum * i, maxDimention);

                ShowLabel(lt, maxDimention);
                LastTopUnitTime = lt;
                double w = getLynxTimeWidth(pastTime, lt);
                if(w>MinSpace)
                    ShowXUnit(pastTime, lt,LynxTime.getLowTimeDim(maxDimention));
                pastTime = lt;
            }
            ShowLabel(LastTopUnitTime, maxDimention);
            //LastTopUnitTime = FirstTopUnitTime + (LastTopUnitTime)XShowEndTime.getNextTime(maxDimention);
            //ShowXUnit(XShowBeginTime, XShowEndTime, canvasTimeLine.Width);
        }
        long TimeUnitNum = 1;//顶级的一次的运动单元数，左右移动的时候，一次显示这个单位
        int SubNum = 3;//每次的下一级别分割区域

        public void ShowXUnit(LynxTime bt, LynxTime et, TimeDimention ct)
        {
            //TimeDimention cDimention = LynxTime.getMaxCrossUnit(XShowBeginTime, XShowEndTime);
            //FirstTopUnitTime = XShowBeginTime.getPastTime(cDimention);
            //LastTopUnitTime = XShowEndTime.getNextTime(cDimention);
            long tn = LynxTime.getUnitNum(bt, et, ct);
            if (tn < 2) { return; }
            long stepUnitNum = Convert.ToInt64(Math.Ceiling(tn / (double)SubNum));
            LynxTime pastTime = bt;
            //ShowLabel(pastTime, maxDimention);
            for (int i = 1; i < SubNum; i++)
            {
                LynxTime lt = new LynxTime(bt);
                lt.AddTimeSpan(stepUnitNum * i, ct);

                ShowLabel(lt, ct);
                double sw = getLynxTimeWidth(pastTime, lt);
                if (sw > MinSpace)
                    ShowXUnit(pastTime, lt,  LynxTime.getLowTimeDim(maxDimention));
                pastTime = lt;
            }
        }

        //关键在于TimeDimention的长度超过了MaxSpace，就进入下一级别
        //TimeDimention CurrentDimention;//当前的级别
        //public void ShowXUnit(LynxTime bt, LynxTime et, double w)
        //{
        //    //int n = Convert.ToInt32(Math.Floor( w/ MaxSpace));
        //    //if (n < 2) { return; }
        //    TimeDimention CurrentDimention = LynxTime.getMaxCrossUnit(bt, et);
        //    long tu = LynxTime.getUnitNum(bt, et, CurrentDimention);
        //    if (tu < 4)
        //    {
        //        CurrentDimention = LynxTime.getLowTimeDim(CurrentDimention);
        //    }
        //    double tw = w / SubNum;

        //    List<LynxTime> tl = LynxTime.getCoverUnitList(bt, et, CurrentDimention, SubNum);
            
        //    if (tl.Count < 3) { return; }
        //    //FirstTopTime = new LynxTime(tl[0]);
        //    //LastTopTime = new LynxTime(tl[tl.Count - 1]);
        //    //ShowLabel(FirstTopUnitTime, CurrentDimention);

        //    for (int i = 1; i < tl.Count; i++)
        //    {
        //        LynxTime tbt = tl[i-1];
        //        LynxTime tet = tl[i];
        //        ShowLabel(tet, CurrentDimention);
        //        if (tw > MinSpace)
        //        {
        //            ShowXUnit(tbt, tet, tw);
        //        }
        //        //double uw = getLynxTimeWidth(tbt, tet);
        //        //double uw = getLynxTimeWidth(CurrentDimention);
        //        //if (uw > MinSpace)
        //        //{
        //        //    ShowXUnit(tbt, tet, uw, CurrentDimention);
        //        //}
        //    }
            
        //}

        Dictionary<double, LynxTime> Marks = new Dictionary<double, LynxTime>();
        List<TextBlock> ll = new List<TextBlock>();

        void ShowLabel(LynxTime ltime, TimeDimention ctd)
        {
            string TimeText=ltime.getTopFormatStr(ctd);
            //string ShortStr = LynxTime.getShortExpressStr(ltime);

            if (Marks.ContainsValue(ltime)) { return; }
            double lleft = getLynxTimeXValue(ltime);
            if (!Marks.ContainsKey(lleft))
            {
                Marks.Add(lleft, ltime);
                LynxTime lt = new LynxTime(ltime);
                TextBlock l = new TextBlock();
                l.FontSize = 10;
                l.Foreground = new SolidColorBrush(Colors.Blue);
                
                l.Text = TimeText;
                ToolTipService.SetToolTip(l, ltime.getTimeString());

                Canvas.SetLeft(l, lleft);
                Canvas.SetTop(l, 0);
                l.Tag = ltime;
                canvasTimeLine.Children.Add(l);
                ll.Add(l);
            }
        }

        //public void ShowXUnit(LynxTime bt, LynxTime et, int SubNum)
        //{
        //    TimeDimention td = LynxTime.getMaxCrossUnit(bt, et);
        //    List<LynxTime> tl = LynxTime.getUnitList(bt, et, SubNum);

        //    if (tl.Count < 2) { return; }
        //    for (int i = 1; i < tl.Count; i++)
        //    {
        //        LynxTime tbt = tl[i - 1];
        //        LynxTime tet = tl[i];
        //        ShowLabel(tet, td);
        //        double uw = getLynxTimeWidth(tbt, tet);
        //        if (uw > MaxWidth)
        //        {
        //            ShowXUnit(tbt, tet, SubNum);
        //        }

        //    }
        //}
        #endregion

        #region Synchronization
        //List<FrameworkElement> SynchronizationControlList = new List<FrameworkElement>();
        //public void AddSynchronizationControl(FrameworkElement fe)
        //{
        //    if (!SynchronizationControlList.Contains(fe))
        //    {
        //        SynchronizationControlList.Add(fe);
        //    }
        //}
        //public void RemoveSynchronizationControl(FrameworkElement fe)
        //{
        //    if (SynchronizationControlList.Contains(fe))
        //    {
        //        SynchronizationControlList.Remove(fe);
        //    }
        //}
        //public void ClearSynchronizationControlList()
        //{
        //    SynchronizationControlList.Clear();
        //}
        List<Panel> SynchronizationPanelList = new List<Panel>();//所有的同步移动面板，此类面板里面的元素都同步移动
        //Panel SynchronizationPanel;
        public void RemoveSynchronizationTargetPanel(Panel TargetPanel)//包含同步控件的面板，直接同步里面的元素
        {
            if (SynchronizationPanelList.Contains(TargetPanel)) { SynchronizationPanelList.Remove(TargetPanel); }
        }

        public void AddSynchronizationTargetPanel(Panel TargetPanel)//包含同步控件的面板，直接同步里面的元素
        {
            if (SynchronizationPanelList.Contains(TargetPanel)) { return; }
            SynchronizationPanelList.Add(TargetPanel);
            Canvas.SetLeft(TargetPanel, Canvas.GetLeft(this));
            TargetPanel.Width = canvasTimeLine.Width;
        }

        public void SynchronizationControl(FrameworkElement fe, LynxTime Begin, LynxTime End)
        {
            fe.Width = getLynxTimeWidth(Begin, End);
            Canvas.SetLeft(fe, getLynxTimeXValue(Begin));

        }

        public void SynchronizationControl(ITimeLineControl itc)
        {
            //itc.TimeControl.Width = getLynxTimeWidth(itc.BeginTime, itc.EndTime);
            //Canvas.SetLeft(itc.TimeControl, getLynxTimeXValue(itc.BeginTime));
            itc.tlhelper.setTimeControlLeft();
            itc.tlhelper.setTimeControlWidth();
        }
        public void SynchronizationControl(FrameworkElement fe, DateTime Begin, DateTime End)
        {
            SynchronizationControl(fe,new LynxTime(Begin), new LynxTime(End));
        }
        #endregion
        //public double MaxWidth = 300;//最大宽度，超过这个宽度是可以的，就使用变换来完成

        //ActionMove am;

        public event TimeLineResized timeLineChanged;//每次修改位置和大小以后，通知

        //public DateTime _BeginTime=new DateTime(1900,1,1);//开始时间
        //public DateTime _EndTime=new DateTime(2020,12,31);//终止时间

        public LynxTime BeginTime
        {
            get
            {
                return XShowBeginTime;
            }
            set 
            { 
                //_BeginTime = value; 
                XShowBeginTime = value;
                ShowXUnit();
                //SynchronizationControl
                if (timeLineChanged != null) 
                { 
                    timeLineChanged(Canvas.GetLeft(canvasTimeLine), canvasTimeLine.Width, BeginTime, EndTime - BeginTime); 
                } 
            }
        }
        public LynxTime EndTime
        {
            get
            {
                return XShowEndTime;
            }
            set
            {
                //_EndTime = value;
                XShowEndTime = value;
                ShowXUnit();
                if (timeLineChanged != null)
                {
                    timeLineChanged(Canvas.GetLeft(canvasTimeLine), canvasTimeLine.Width, BeginTime, EndTime - BeginTime);
                }
            }
        }

        public LynxTime SelectedTime=new LynxTime();//选择的时间

        public double ControlWidth//设置宽度
        {
            get { return Width; }
            set { Width = value; Resized(value); }
        }

        public void Resized(double w)//当父窗口宽度变化后，设置此控件的宽度
        {
            Width = w;
            canvasViewWindow.Width = w;
            canvasTimeLine.Width = w;
            //UperUnit.Width = w;
            //Unit.Width = w;
            //setMove();
            //ShowXUnit();
            //Show();
            if (timeLineChanged != null)
            {
                timeLineChanged(Canvas.GetLeft(canvasTimeLine), canvasTimeLine.Width, BeginTime, TimeLong);
            }
        }
        public LynxTime TimeLong
        {
            get
            {
                return EndTime - BeginTime;
            }
            set
            {
                EndTime = BeginTime + value;
            }
        }

        void InitGradientBrush()
        {
            gb = new LinearGradientBrush();
            GradientStop bc = new GradientStop();
            bc.Color = Colors.Yellow;
            bc.Offset = 0;
            GradientStop mc = new GradientStop();
            mc.Color = Colors.Blue;
            mc.Offset = 0.5;
            GradientStop ec = new GradientStop();
            ec.Color = Colors.Yellow;
            ec.Offset = 1;
            gb.GradientStops.Add(bc);
            gb.GradientStops.Add(mc);
            gb.GradientStops.Add(ec);
            
        }
        GradientBrush gb;

        Line CurrentTimeLine = new Line();//当前时间

        void setCurrentTime(double x)
        {
            if (!canvasTimeLine.Children.Contains(CurrentTimeLine))
            {
                canvasTimeLine.Children.Add(CurrentTimeLine);
                CurrentTimeLine.Stroke = new SolidColorBrush(Colors.Orange);
                CurrentTimeLine.StrokeThickness = 3;
            }

            Canvas.SetLeft(CurrentTimeLine, x);
            //CurrentTimeLine.X1 = CurrentTimeLine.X2 = x;
            //double sv = XShowBeginTime.getDoubleValue() + (XShowEndTime.getDoubleValue() - XShowBeginTime.getDoubleValue()) * x / canvasTimeLine.Width;
            //ToolTipService.SetToolTip(CurrentTimeLine, SelectedTime.ToString());
            SelectedTime=getLynxTimeValue(x);
            this.textBoxCurrentSelect.Text = SelectedTime.getDateTime().ToString();
        }
        private void buttonSelect_Click(object sender, RoutedEventArgs e)
        {
            CurrentTimeLine.Y1 = 0;
            CurrentTimeLine.Y2 = canvasTimeLine.Height;
            canvasTimeLine.PointerPressed += new PointerEventHandler(canvasTimeLine_setCurrentTimePointerPressed);
        }

        void canvasTimeLine_setCurrentTimePointerPressed(object sender, PointerRoutedEventArgs e)
        {
            canvasTimeLine.PointerPressed -= new PointerEventHandler(canvasTimeLine_setCurrentTimePointerPressed);
            setCurrentTime(e.GetCurrentPoint(canvasTimeLine).Position.X);
        }

        private void buttonMin_Click(object sender, RoutedEventArgs e)
        {
            ZoomOutScale(0.2);
        }

        private void buttonMax_Click(object sender, RoutedEventArgs e)
        {
            ZoomInScale(0.2);
        }

        bool _IsMouseDown = false;
        //LynxTime CurrentTime;
        //LynxTime CurrentMinTimeUnit;
        //LynxTime CurrentMaxTimeUnit;

        double CurrentX;
        private void canvasTimeLine_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //tx = e.GetPosition(canvasViewWindow).X;
            CurrentX = e.GetCurrentPoint(canvasTimeLine).Position.X;
            //MouseCurrentTime = getDateTime(e.GetPosition(canvasTimeLine).X);

            //CurrentTime = getLynxTimeValue(CurrentX);
            _IsMouseDown = true;
        }

        private void canvasTimeLine_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            //tx = e.GetPosition(canvasViewWindow).X;
            _IsMouseDown = false;
            CurrentX = e.GetCurrentPoint(canvasTimeLine).Position.X;
            //MouseCurrentTime = getDateTime(e.GetPosition(canvasTimeLine).X);
        }


        double DeltaMS = 0;

        private void canvasTimeLine_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!_IsMouseDown) { return; }

            double cx = e.GetCurrentPoint(canvasTimeLine).Position.X;

            double dx = cx - CurrentX;
            if ((dx < 0.001)&&(dx>-0.001)) { return; }
            CurrentX = cx;
            double lms = (XShowEndTime - XShowBeginTime).getDoubleValue();
            //if (lms == 0)
            //{
            //    SilverlightLFC.common.Environment.ShowMessage("Error");
            //    return;
            //}
            DeltaMS = dx / canvasTimeLine.Width * lms;

            XShowBeginTime = XShowBeginTime - Convert.ToInt64(DeltaMS);

            XShowEndTime = XShowEndTime - Convert.ToInt64(DeltaMS);



            //textBox1.Text = lms.ToString();
            textBeginTime.Text = XShowBeginTime.getTimeString();
            textEndTime.Text = XShowEndTime.getTimeString();
            //double UnitWidth = getLynxTimeWidth(maxDimention);

            //int num = Convert.ToInt32(Math.Ceiling(MaxSpace / UnitWidth));
            foreach (FrameworkElement u in canvasTimeLine.Children)
            {
                Canvas.SetLeft(u, Canvas.GetLeft(u) + dx);
            }

            foreach (Panel p in SynchronizationPanelList)
            {
                //Canvas.SetLeft(p, Canvas.GetLeft(p) + dx);
                foreach (FrameworkElement u in p.Children)
                {
                    if (u is ITimeLineControl)
                    {
                        Canvas.SetLeft(u, Canvas.GetLeft(u) + dx);
                    }
                }
            }
            if (XShowBeginTime.getDoubleValue() <= FirstTopUnitTime.getDoubleValue())
            {
                LynxTime NFT = new LynxTime(FirstTopUnitTime);
                NFT.AddTimeSpan( -TimeUnitNum, maxDimention);
                double uw = getLynxTimeWidth(NFT, FirstTopUnitTime);
                //ShowLabel(FirstTopUnitTime, maxDimention);
                ShowLabel(NFT, maxDimention);
                if (uw > MinSpace)
                {
                    ShowXUnit(NFT, FirstTopUnitTime, maxDimention);
                }
                FirstTopUnitTime =NFT;
            }
            if (XShowEndTime.getDoubleValue() >= LastTopUnitTime.getDoubleValue())
            {
                LynxTime NFT = new LynxTime(LastTopUnitTime);
                NFT.AddTimeSpan(TimeUnitNum, maxDimention);
                double uw = getLynxTimeWidth(LastTopUnitTime,NFT);
                //ShowLabel(LastTopUnitTime, maxDimention);
                ShowLabel(NFT, maxDimention);
                if (uw > MinSpace)
                {
                    ShowXUnit(LastTopUnitTime, NFT, maxDimention);
                }
                LastTopUnitTime = NFT;
            }


        }

        private void buttonGo_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt=Convert.ToDateTime(textBoxCurrentSelect.Text);
            LynxTime lt=new LynxTime(dt);
            setLynxTimeAtCenter(lt);
            if (!canvasTimeLine.Children.Contains(CurrentTimeLine))
            {
                canvasTimeLine.Children.Add(CurrentTimeLine);

            } 
            Canvas.SetLeft(CurrentTimeLine, canvasTimeLine.Width / 2);
            if (timeLineChanged != null)
            {
                timeLineChanged(Canvas.GetLeft(canvasTimeLine), canvasTimeLine.Width, BeginTime, EndTime - BeginTime);
            }
            foreach (Panel p in SynchronizationPanelList)
            {
                foreach (FrameworkElement f in p.Children)
                {
                    if (f is ITimeLineControl)
                    {
                        SynchronizationControl(f as ITimeLineControl);
                    }
                }
            }
        }

    }

}
