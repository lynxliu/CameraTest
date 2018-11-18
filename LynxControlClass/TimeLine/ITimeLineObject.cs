using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;


namespace SilverlightLynxControls.TimeLine
{
    public enum TimeObjectShowType//显示的模式，代表完全合并，此时允许重叠，完全展开，此时每个对象一行，还有紧凑模式，不许重叠，压缩行数
    {
        Merge, Compact, Expand
    }
    public enum TimeObjectStatus//时间线对象的状态，之前（未发生），发生（正在发生），影响（产生的影响范围），发生（发生过）
    {
        Before,Active,After
    }

    public interface ITimeObject//在时间上展开的对象
    {
        //double MinWidth { get; }//最小宽度
        //TimeObjectShowType ShowType { get; set; }
        event LFCObjectChanged ObjctChanged;
        //LynxTime AffectTimeLong { get; set; }//影响时间长
        //LynxTime StartTime { get; set; }//起始点时间
        TimeObjectStatus getTimeStatus(LynxTime t);//判断时间点的相对关系
    }

    public interface ITimePointObject : ITimeObject//单点的对象
    {
        LynxTime HappenTime { get; set; }
        LynxTime AffectTimeLong { get; set; }
        //event LFCObjectChanged ObjctChanged;
    }

    public interface ITimePeriodObject : ITimeObject
    {
        LynxTime BeginTime { get; set; }
        LynxTime TimeLong { get; set; }
    }


    public class TimeLineLeftConverter : IValueConverter//支持把数据对象的canvas.left属性绑定到beigintime
    {
        public LynxTime BaseTime { get; set; }
        public TimeLineLeftConverter(LynxTimeLine tl)
        {
            TargetTimeLine = tl;
        }
        public LynxTimeLine TargetTimeLine { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (TargetTimeLine == null) { return 0; }
            LynxTime lt = value as LynxTime;
            if (BaseTime != null)
            {
                lt = lt + BaseTime;
            }
            return TargetTimeLine.getLynxTimeXValue(lt);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (TargetTimeLine == null) { return null; }
            double d = System.Convert.ToDouble(value);
            LynxTime lt=TargetTimeLine.getLynxTimeValue(d);
            if (BaseTime != null)
            {
                lt=lt-BaseTime;
            }
            return lt;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeLineWidthConverter : IValueConverter//支持把数据对象的Width属性绑定到TimeLong
    {
        public TimeLineWidthConverter(LynxTimeLine tl)
        {
            TargetTimeLine = tl;
        }
        
        public LynxTimeLine TargetTimeLine { get; set; }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (TargetTimeLine == null) { return 0; }
            LynxTime lt = value as LynxTime;
            return TargetTimeLine.getLynxTimeWidth(lt);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (TargetTimeLine == null) { return null; }
            double lt = System.Convert.ToDouble(value);
            return TargetTimeLine.getLynxTimeSpan(lt);
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeLineHelper//嵌入用户对象，帮助处理和TimeLin之间的关联
    {
        ITimeObject _DataObj;
        public ITimeObject DataObj { get{return _DataObj;}
            set
            {
                if (value != _DataObj)
                {
                    DeWatchITimeLineObject();
                    _DataObj = value;
                    WatchITimeLineObject();
                }
                
            }
        }
        public LynxTime BaseTime { get; set; }//起始时间的相对起点
        public FrameworkElement TimeControl { get; set; }
        public LynxTimeLine timeline { get; set; }

        public double MinWidth
        {
            get { return 50; }
        }

        public void setTimeControlLeft()
        {
            if (DataObj == null || TimeControl == null || timeline == null) { return; }
            if (BaseTime == null)
            {
                if (DataObj is ITimePeriodObject)
                    Canvas.SetLeft(TimeControl, timeline.getLynxTimeXValue((DataObj as ITimePeriodObject).BeginTime));
                if (DataObj is ITimePointObject)
                    Canvas.SetLeft(TimeControl, timeline.getLynxTimeXValue((DataObj as ITimePointObject).HappenTime));

            }
            else
            {
                if (DataObj is ITimePeriodObject)
                    Canvas.SetLeft(TimeControl, timeline.getLynxTimeXValue((DataObj as ITimePeriodObject).BeginTime + BaseTime));
                if (DataObj is ITimePointObject)
                    Canvas.SetLeft(TimeControl, timeline.getLynxTimeXValue((DataObj as ITimePointObject).HappenTime + BaseTime));

            }
        }
        public void setTimeControlWidth()
        {
            if (DataObj == null || TimeControl == null || timeline == null) { return; }
            if (DataObj is ITimePeriodObject)
                TimeControl.Width = timeline.getLynxTimeWidth((DataObj as ITimePeriodObject).TimeLong);

        }
        public void setDataObjBeginTime()
        {
            if (DataObj == null || TimeControl == null || timeline == null) { return; }
            DeWatchITimeLineObject();
            if (DataObj is ITimePeriodObject)
            {
                if (BaseTime == null)
                    (DataObj as ITimePeriodObject).BeginTime = timeline.getLynxTimeValue(Canvas.GetLeft(TimeControl));
                else
                    (DataObj as ITimePeriodObject).BeginTime = timeline.getLynxTimeValue(Canvas.GetLeft(TimeControl)) - BaseTime;
            }
            if (DataObj is ITimePointObject)
            {
                if (BaseTime == null)
                    (DataObj as ITimePointObject).HappenTime = timeline.getLynxTimeValue(Canvas.GetLeft(TimeControl));
                else
                    (DataObj as ITimePointObject).HappenTime = timeline.getLynxTimeValue(Canvas.GetLeft(TimeControl)) - BaseTime;
            }
            WatchITimeLineObject();
        }

        public void setDataObjTimeLong()
        {
            if (DataObj == null || TimeControl == null || timeline == null) { return; }
            if (!(DataObj is ITimePeriodObject)) { return; }
            DeWatchITimeLineObject();
            (DataObj as ITimePeriodObject).TimeLong = timeline.getLynxTimeSpan(TimeControl.Width);
            WatchITimeLineObject();
        }
        int wtime = 0;
        public void WatchITimeLineObject()
        {
            if (DataObj == null) { return; }
            wtime++;
            DataObj.ObjctChanged += new LFCObjectChanged(DataObj_ObjctChanged);
        }
        public void DeWatchITimeLineObject()
        {
            if (DataObj == null) { return; }
            wtime--;
            DataObj.ObjctChanged -= new LFCObjectChanged(DataObj_ObjctChanged);
        }
        void DataObj_ObjctChanged(object sender, LFCObjectChangedArgs e)
        {
            if (e.PropertyName == "BeginTime" || e.PropertyName == "EndTime" || e.PropertyName == "TimeLong" || e.PropertyName == "HappenTime")
            {
                setTimeControlLeft();
                setTimeControlWidth();
            }
        }

        public static LynxTime getPositionTime(ITimeObject o)
        {
            if (o is ITimePeriodObject)
            {
                return (o as ITimePeriodObject).BeginTime;
            } 
            if (o is ITimePointObject)
            {
                return (o as ITimePointObject).HappenTime;
            }
            return null;
        }

        public LynxTime getTimePointObjectTimeLong(double w)//获取点时间对象的等效长度
        {
            return timeline.getLynxTimeSpan(w);
        }
    }
}
