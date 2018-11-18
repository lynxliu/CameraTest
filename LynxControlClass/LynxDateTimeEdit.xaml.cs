using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using SilverlightLFC.UIHelper;
using SilverlightLFC.common;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

namespace SilverlightLynxControls
{
    public partial class LynxDateTimeEdit : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public LynxDateTimeEdit()
        {
            this.InitializeComponent();
            Value = new LynxTime();
            ts.timeChanged += new TimeChanged(ts_timeChanged);
            p = Parent as Panel;
            popup1.Child = ts;

            textBlock1.DataContext = this;
        }
        public static readonly DependencyProperty LynxTimeValueProperty =
    DependencyProperty.Register("Value", typeof(LynxTime), typeof(LynxDateTimeEdit), null);
        
        Panel p;
        public Panel EditControlPanel
        {
            get { return p; }
            set { p = value; }
        }

        void ts_timeChanged(object sender,LynxTime Time)
        {
            Value = Time;
            if (timeChanged != null)
                timeChanged(this,Time);
        }

        public DateTime DateTimeValue 
        {
            get { return Value.getDateTime(); }
            set 
            {
                LynxTime v = new LynxTime(value);
                Value = v;
            } 
        }

        public LynxTime Value
        {
            get { return (LynxTime)GetValue(LynxTimeValueProperty); }
            set {
                SetValue(LynxTimeValueProperty, value);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
                if(value!=null)
                    ToolTipService.SetToolTip(this, ((LynxTime)GetValue(LynxTimeValueProperty)).getTimeString());
            }
        }

        LynxTimeSelect ts = new LynxTimeSelect();

        void LoadValue()
        {
            ts.ReadTimeValue(Value);
        }

        public LynxTime getValue() { return Value; }
        public event TimeChanged timeChanged;

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            textBlock1.Width = e.NewSize.Width - 25;
        }

        private void image1_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ts.ReadTimeValue(Value);
            popup1.IsOpen = !popup1.IsOpen;
        }
    }


}
