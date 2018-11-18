using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;

using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace SilverlightLynxControls
{
    public partial class LynxTimeSelect : UserControl
    {
        public LynxTimeSelect()
        {
            InitializeComponent();
        }
        //DateTime DateTimeValue=DateTime.Now;
        LynxTime LynxTimeValue=new LynxTime();

        public void ReadTimeValue(LynxTime t)
        {
            LynxTimeValue=new LynxTime(t);
            lynxUpDownHour.LongValue = t.Hour;
            lynxUpDownMinute.LongValue = t.Minute;
            lynxUpDownSecond.LongValue = t.Second;
            lynxUpDownMS.LongValue = t.Millisecond;
            lynxUpDownYear.LongValue = LynxTimeValue.Year;
            lynxUpDownMonth.LongValue = LynxTimeValue.Month;
            lynxUpDownDay.LongValue = LynxTimeValue.Day;
            ShowWeekDay();
        }

        public LynxTime getLynxTimeValue()
        {
            LynxTimeValue.Hour = Convert.ToInt32(lynxUpDownHour.LongValue);
            LynxTimeValue.Minute = Convert.ToInt32(lynxUpDownMinute.LongValue);
            LynxTimeValue.Second = Convert.ToInt32(lynxUpDownSecond.LongValue);
            LynxTimeValue.Millisecond = Convert.ToInt32(lynxUpDownMS.LongValue);
            LynxTimeValue.Year = Convert.ToInt64(lynxUpDownYear.LongValue);
            LynxTimeValue.Month = Convert.ToInt32(lynxUpDownMonth.LongValue);
            LynxTimeValue.Day = Convert.ToInt32(lynxUpDownDay.LongValue);
            LynxTimeValue.CalculateValue();
            return LynxTimeValue;
        }

        void setLynxTimeValue()
        {
            LynxTimeValue.Hour =Convert.ToInt32( lynxUpDownHour.LongValue);
            LynxTimeValue.Minute = Convert.ToInt32(lynxUpDownMinute.LongValue);
            LynxTimeValue.Second = Convert.ToInt32(lynxUpDownSecond.LongValue);
            LynxTimeValue.Millisecond = Convert.ToInt32(lynxUpDownMS.LongValue);
            LynxTimeValue.Year = Convert.ToInt64(lynxUpDownYear.LongValue);
            LynxTimeValue.Month = Convert.ToInt32(lynxUpDownMonth.LongValue);
            LynxTimeValue.Day = Convert.ToInt32(lynxUpDownDay.LongValue);
            LynxTimeValue.CalculateValue();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            setLynxTimeValue();
            (Parent as Popup).IsOpen = false;// Children.Remove(this);
            if (timeChanged != null)
                timeChanged(this,LynxTimeValue);
        }

        private void buttonCanel_Click(object sender, RoutedEventArgs e)
        {
            (Parent as Popup).IsOpen = false;
            //((Panel)Parent).Children.Remove(this);
        }

        private void lynxUpDownMonth_valueChanged(object sender, LUpDownValueChangeArgs e)
        {
            lynxUpDownDay.LongMax = LynxTime.getMonthMaxDay(Convert.ToInt32(lynxUpDownYear.LongValue), Convert.ToInt32(lynxUpDownMonth.LongValue));
            lynxUpDownDay.LongMin = 1;
            ShowWeekDay();
        }

        private void lynxUpDownYear_valueChanged(object sender, LUpDownValueChangeArgs e)
        {
            ShowWeekDay();
        }

        private void lynxUpDownDay_valueChanged(object sender, LUpDownValueChangeArgs e)
        {
            ShowWeekDay();
        }

        void ShowWeekDay()
        {
            textBlockWeekDay.Text = 
                LynxTime.getWeekDay(Convert.ToInt64(lynxUpDownYear.LongValue), Convert.ToInt32(lynxUpDownMonth.LongValue), Convert.ToInt32(lynxUpDownDay.LongValue));
        }
        public event TimeChanged timeChanged;
    }
    public delegate void TimeChanged(object sender,LynxTime Time);
}
