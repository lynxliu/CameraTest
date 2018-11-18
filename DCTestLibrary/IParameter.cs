using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DCTestLibrary
{
    public enum ParameterState{
        Enable,Disable,Prepare,Testing,Finished,UnKnown//有效，无效，准备，测试中，完成，未知
    }
    public interface IParameter//所有可以测试的项目
    {
        string Name{get;set;}//名称
        string Memo { get; set; }
        string Dimension { get; set; }//测试单位
        double Value { get; set; }
        object TestWay { get; set; }//对于测试卡测试来说就是使用的照片
        AbstractTestChart TestChart
        {
            get;
            set;
        }
        DateTime TestTime { get; set; }
        double SpendTime { get; set; }//测试花费时间
        //int TestNum
        //{
        //    get;
        //    set;
        //}
    }

    public class LParameter : IParameter
    {
        public LParameter()
        {
        }
        public LParameter(string testWay, AbstractTestChart chart)
        {
            TestWay = testWay;
            TestChart = chart;
        }
        string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }

        }

        public string Memo
        {
            get;
            set;
        }

        public string Dimension
        {
            get;
            set;
        }

        public double Value
        {
            get;
            set;
        }
        //object _TestWay;
        public object TestWay
        {
            get;
            set;
        }
        //WriteableBitmap _TestChart;
        public AbstractTestChart TestChart
        {
            get;
            set;
        }
        //public int TestNum
        //{
        //    get;
        //    set;
        //}

        public DateTime TestTime
        {
            get;
            set;
        }

        public double SpendTime
        {
            get;
            set;
        }
    }
}
