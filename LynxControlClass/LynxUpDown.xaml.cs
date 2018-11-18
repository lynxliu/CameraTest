using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using SilverlightLFC.common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace SilverlightLynxControls
{
    public partial class LynxUpDown : UserControl, INotifyPropertyChanged
    {//支持Int，double和decmale三种模式
        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty LongValueProperty =
            DependencyProperty.Register("LongValue", typeof(long), typeof(LynxUpDown), null);
        public static readonly DependencyProperty IntValueProperty =
    DependencyProperty.Register("IntValue", typeof(int), typeof(LynxUpDown), null);

        public static readonly DependencyProperty DoubleValueProperty =
            DependencyProperty.Register("DoubleValue", typeof(double), typeof(LynxUpDown), null);

        public static readonly DependencyProperty DecimalValueProperty =
            DependencyProperty.Register("DecimalValue", typeof(Decimal), typeof(LynxUpDown), null);

        public event LUpDownValueChanged valueChanged;
        Binding b=new Binding();
        public void setBinging()
        {
            b = new Binding();
            if (DataMode == LynxUpDownDataMode.DecimalMode)
            {
                b.Path = new PropertyPath("DecimalValue");
            }
            if (DataMode == LynxUpDownDataMode.DoubleMode)
            {
                b.Path = new PropertyPath("DoubleValue");
            }
            if (DataMode == LynxUpDownDataMode.LongMode)
            {
                b.Path = new PropertyPath("LongValue");
            }
             if (DataMode == LynxUpDownDataMode.IntMode)
            {
                b.Path = new PropertyPath("IntValue");
            }
            b.Mode = BindingMode.TwoWay;
            b.Source = this;
            textBox.SetBinding(TextBox.TextProperty, b);
        }
        public LynxUpDown()
        {
            InitializeComponent();
            setBinging();
            textBox.Background = _BackBrush;
            //textBox.DataContext = this;
        }
        public double controlWidth
        {
            get { return Width; }
            set
            {
                if (value < 20) { return; }//最小是50
                Width = value;
                LayoutRoot.Width = value;

            }
        }

        public double controlHeight
        {
            get { return Height; }
            set
            {
                if (value < 20) { return; }//最小是50
                Height = value;
                LayoutRoot.Height = value;

            }
        }

        void Resized(double w, double h)
        {
            //double w,h;

            //textBox.Width = w * 0.7;
            //textBox.Height = h;
            //Canvas.SetLeft(buttonDown, w * 0.7);
            //Canvas.SetLeft(buttonUp, w * 0.7);
            //Canvas.SetTop(buttonDown, h * 0.5);
            //buttonDown.Width = w * 0.3;
            //buttonDown.Height = h * 0.5;
            //buttonUp.Width = w * 0.3;
            //buttonUp.Height = h * 0.5;

        }

        LynxUpDownDataMode _DataMode = LynxUpDownDataMode.LongMode;//默认模式
        public LynxUpDownDataMode DataMode
        {
            get { return _DataMode; }
            set
            {
                _DataMode = value;
                setBinging();
                ShowValue();
            }
        }

        public void setDefaultRange(string TypeName)
        {
            if (TypeName == "Byte")
            {
                IntMin = Byte.MinValue;
                LongMax = Byte.MaxValue;
            }
            if (TypeName == "Int16")
            {
                LongMin = Int16.MinValue;
                LongMax = Int16.MaxValue;
            }
            if (TypeName == "Int32")
            {
                LongMin = Int32.MinValue;
                LongMax = Int32.MaxValue;
            }
            if (TypeName == "Int64")
            {
                LongMin = Int64.MinValue;
                LongMax = Int64.MaxValue;
            }
            if (TypeName == "Single")
            {
                DoubleMin = Single.MinValue;
                DoubleMin = Single.MaxValue;
            }
            if (TypeName == "Double")
            {
                DoubleMin = Double.MinValue;
                DoubleMin = Double.MaxValue;
            }
            if (TypeName == "Decimal")
            {
                DecimalMin = Decimal.MinValue;
                DecimalMin = Decimal.MaxValue;
            }
        }

        void ShowValue()
        {
            textBox.Foreground = _ForeBrush;

            if (DataMode==LynxUpDownDataMode.LongMode)
            {

                textBox.Text = LongValue.ToString();
                return;
            }
            string s = "0";
            if (DataMode == LynxUpDownDataMode.DecimalMode)
            {
                s = DecimalValue.ToString();
            }
            if (DataMode == LynxUpDownDataMode.DoubleMode)
            {
                s = DoubleValue.ToString();
            }
            int f = s.LastIndexOf('.');
            if (f == -1) { textBox.Text = s; }
            else
            {
                int l = f + _floatNum + 1;
                if (l > s.Length) { l = s.Length; }
                textBox.Text = s.Substring(0, l);
            }
        }


        private void buttonUp_Click(object sender, RoutedEventArgs e)
        {

            if (DataMode == LynxUpDownDataMode.DecimalMode)
            {
                if (DecimalValue <= _DecimalMax - _DecimalStep)
                {
                    DecimalValue = DecimalValue + _DecimalStep;
                }
                else
                {
                    DecimalValue = _DecimalMax;
                }
            }
            if (DataMode == LynxUpDownDataMode.DoubleMode)
            {
                if (DoubleValue <= _DoubleMax - _DoubleStep)
                {
                    DoubleValue = DoubleValue + _DoubleStep;
                }
                else
                {
                    DoubleValue = _DoubleMax;
                }
            }
            if (DataMode == LynxUpDownDataMode.LongMode)
            {
                if (LongValue <= _LongMax - _LongStep)
                {
                    LongValue = LongValue + _LongStep;

                }
                else
                {
                    LongValue = _LongMax;
                }
            }
            ShowValue();
        }

        private void buttonDown_Click(object sender, RoutedEventArgs e)
        {
            if (DataMode == LynxUpDownDataMode.DecimalMode)
            {
                if (DecimalValue >= _DecimalMin + _DecimalStep)
                {
                    DecimalValue = DecimalValue - _DecimalStep;
                }
                else
                {
                    DecimalValue = _DecimalMin;
                }
            }
            if (DataMode == LynxUpDownDataMode.DoubleMode)
            {
                if (DoubleValue >= _DoubleMin + _DoubleStep)
                {
                    DoubleValue = DoubleValue - _DoubleStep;
                }
                else
                {
                    DoubleValue = _DoubleMin;
                }
            }
            if (DataMode == LynxUpDownDataMode.LongMode)
            {
                if (LongValue >= _LongMin + _LongStep)
                {
                    LongValue = LongValue - _LongStep;
                }
                else
                {
                    LongValue = _LongMin;
                }
            }
            if (DataMode == LynxUpDownDataMode.IntMode)
            {
                if (IntValue >= _IntMin + _IntStep)
                {
                    IntValue = IntValue - _IntStep;
                }
                else
                {
                    IntValue = _IntMin;
                }
            } 
            ShowValue();
        }

        //long _LongValue = 0;
        long _LongMin = long.MinValue;
        long _LongMax = long.MaxValue;
        long _LongStep = 1;

        //long _IntValue = 0;
        int _IntMin = int.MinValue;
        int _IntMax = int.MaxValue;
        int _IntStep = 1;

        //decimal _DecimalValue = 0m;
        decimal _DecimalMin = decimal.MinValue;
        decimal _DecimalMax = decimal.MaxValue;
        decimal _DecimalStep = 1m;

        //double _DoubleValue = 0;
        double _DoubleMin = double.MinValue;
        double _DoubleMax = double.MaxValue;
        double _DoubleStep = 1;

        int _floatNum = 2;//浮点类型的时候的小数位数
        Brush _BackBrush = new SolidColorBrush(Colors.White);
        Brush _ForeBrush = new SolidColorBrush(Colors.Black);

        public Brush BackBush
        {
            get
            {
                return _BackBrush;
            }
            set
            {
                _BackBrush = value;
            }
        }
        public Brush ForeBush
        {
            get
            {
                return _ForeBrush;
            }
            set
            {
                _ForeBrush = value;
            }
        }

        public int FloatNum
        {
            get
            {
                return _floatNum;
            }
            set
            {
                _floatNum = value;
                ShowValue();
            }
        }

        public long LongValue
        {
            get
            {
                return (long)GetValue(LongValueProperty); ;
            }
            set
            {
                SetValue(LongValueProperty, value);
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("LongValue"));
                ShowValue();
            }

        }

        public int IntValue
        {
            get
            {
                return (int)GetValue(IntValueProperty); ;
            }
            set
            {
                SetValue(IntValueProperty, value);
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IntValue"));
                ShowValue();
            }
        }

        public double DoubleValue
        {
            get
            {
                return (double)GetValue(DoubleValueProperty); ;
            }
            set
            {
                SetValue(DoubleValueProperty, value);
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DoubleValue"));
                ShowValue();
            }
        }

        public decimal DecimalValue
        {
             get
            {
                return (decimal)GetValue(DecimalValueProperty); ;
            }
            set
            {
                SetValue(DecimalValueProperty, value);
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("DecimalValue"));
                ShowValue();
            }

        }
        public int IntStep
        {
            get
            {
                return _IntStep;
            }
            set
            {
                _IntStep = value;
            }
        }

        public int IntMin
        {
            get
            {
                return _IntMin;
            }
            set
            {
                _IntMin = value;
                if (IntValue < _IntMin) { IntValue = _IntMin; }
            }
        }

        public int IntMax
        {
            get
            {
                return _IntMax;
            }
            set
            {
                _IntMax = value;
                if (IntValue > _IntMax) { IntValue = _IntMax; }
            }
        }
        public long LongStep
        {
            get
            {
                return _LongStep;
            }
            set
            {
                _LongStep = value;
            }
        }
        
        public long LongMin
        {
            get
            {
                return _LongMin;
            }
            set
            {
                _LongMin = value;
                if (LongValue < _LongMin) { LongValue = _LongMin; }
            }
        }
        
        public long LongMax
        {
            get
            {
                return _LongMax;
            }
            set
            {
                _LongMax = value;
                if (LongValue > _LongMax) { LongValue = _LongMax; }
            }
        }
        public double DoubleMin
        {
            get
            {
                return _DoubleMin;
            }
            set
            {
                _DoubleMin = value;
                if (DoubleValue < _DoubleMin) { DoubleValue = _DoubleMin; }
            }
        }

        public double DoubleMax
        {
            get
            {
                return _DoubleMax;
            }
            set
            {
                _DoubleMax = value;
                if (DoubleValue > _DoubleMax) { DoubleValue = _DoubleMax; }
            }
        }

        public double DoubleStep
        {
            get
            {
                return _DoubleStep;
            }
            set
            {
                _DoubleStep = value;
            }
        }

        public decimal DecimalMin
        {
            get
            {
                return _DecimalMin;
            }
            set
            {
                _DecimalMin = value;
                if (DecimalValue < _DecimalMin) { DecimalValue = _DecimalMin; }
            }
        }

        public decimal DecimalMax
        {
            get
            {
                return _DecimalMax;
            }
            set
            {
                _DecimalMax = value;
                if (DecimalValue > _DecimalMax) { DecimalValue = _DecimalMax; }
            }
        }

        public decimal DecimalStep
        {
            get
            {
                return _DecimalStep;
            }
            set
            {
                _DecimalStep = value;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resized(e.NewSize.Width, e.NewSize.Height);
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNumber(textBox.Text))
            {
                if (DataMode == LynxUpDownDataMode.LongMode)
                {
                    long t;

                    if (long.TryParse(textBox.Text, out t))
                    {
                        if ((t >= _LongMin) && (t <= _LongMax))
                        {
                            LongValue = t;
                            if (valueChanged != null)
                            {
                                valueChanged(this, new LUpDownValueChangeArgs(DataMode, LongValue));
                            }
                        }
                    }
                    //ShowValue();
                    //return;
                }
                if (DataMode == LynxUpDownDataMode.IntMode)
                {
                    int t;

                    if (int.TryParse(textBox.Text, out t))
                    {
                        if ((t >= _IntMin) && (t <= _IntMax))
                        {
                            IntValue = t;
                            if (valueChanged != null)
                            {
                                valueChanged(this, new LUpDownValueChangeArgs(DataMode, IntValue));
                            }
                        }
                    }
                    //ShowValue();
                    //return;
                }
                if (DataMode == LynxUpDownDataMode.DecimalMode)
                {
                    decimal t;

                    if (decimal.TryParse(textBox.Text, out t))
                    {
                        if ((t >= _DecimalMin) && (t <= _DecimalMax))
                        {
                            DecimalValue = t;
                            if (valueChanged != null)
                            {
                                valueChanged(this, new LUpDownValueChangeArgs(DataMode, DecimalValue));
                            }
                        }
                    }
                    //ShowValue();
                }
                if (DataMode == LynxUpDownDataMode.DoubleMode)
                {
                    double t;

                    if (double.TryParse(textBox.Text, out t))
                    {
                        if ((t >= _DoubleMin) && (t <= _DoubleMax))
                        {
                            DoubleValue = t;
                            if (valueChanged != null)
                            {
                                valueChanged(this, new LUpDownValueChangeArgs(DataMode, DoubleValue));
                            }
                        }
                    }
                    //ShowValue();
                }
            }
            ShowValue();
        }

        bool IsNumber(string s)//判断是否是合法的数字，使用循环判定
        {//数字可以以正负号开始，可以包含一个小数点
            if (s == null || s == "") { return false; }
            string ts = s;
            int si = s.IndexOf('.');
            int li = s.LastIndexOf('.');
            if (si != li) { return false; }
            if (si > -1)
            {
                if (si == 0) { ts = ts.Substring(1); }
                else
                {
                    ts = s.Substring(0, si);
                    if (s.Length > si)
                    {
                        ts = ts + s.Substring(si + 1);
                    }
                }
            }

            if (ts.StartsWith("+") || ts.StartsWith("-"))
            {
                ts = ts.Substring(1);
            }

            char[] c = ts.ToCharArray();
            foreach (char tc in c)
            {
                if (!char.IsDigit(tc)) { return false; }
            }
            return true;
        }
    }
    public delegate void LUpDownValueChanged(object sender, LUpDownValueChangeArgs e);
    public class LUpDownValueChangeArgs
    {
        public object value;
        public LynxUpDownDataMode CurrentMode;
        public LUpDownValueChangeArgs(LynxUpDownDataMode dm, object v)
        {
            value = v;
            CurrentMode = dm;
        }
    }
    public enum LynxUpDownDataMode//UpDown的工作模式
    {
        LongMode, DoubleMode, DecimalMode,IntMode
    }

}