using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;
using Windows.UI.Xaml.Controls;




namespace SilverlightLynxControls
{
    public partial class LynxDateTimeSelect : UserControl
    {
        public LynxDateTimeSelect()
        {
            InitializeComponent();
        }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public DateTime SelectedDate { get; set; }
        public string Text { get; set; }
        public void ReadValue(DateTime dt)//设置初始值
        {
            year = dt.Year;
            month = dt.Month;
            day = dt.Day;
            comboBoxHour.SelectedIndex = dt.Hour;
            comboBoxMinute.SelectedIndex = dt.Minute;
        }

        public DateTime getValue()//获取用户选择
        {
            return SelectedDate + TimeSpan.FromHours(comboBoxHour.SelectedIndex) + TimeSpan.FromMinutes(comboBoxMinute.SelectedIndex);
        }
    }
}
