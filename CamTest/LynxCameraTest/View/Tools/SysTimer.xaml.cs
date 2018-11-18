using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using SilverlightDCTestLibrary;
using DCTestLibrary;
using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using LynxCameraTest.ViewModel;
using LynxCameraTest.Model;

namespace SLPhotoTest
{
    public partial class SysTimer : UserControl, IPhotoTestWindow
    {
        public SysTimer()
        {
            InitializeComponent();
            mt.Interval = TimeSpan.FromMilliseconds(10);
            am = new ActionMove(this,this);
        }
        ActionMove am;
        DispatcherTimer mt = new DispatcherTimer();
        int t=0;
        bool IsBeigin = false;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            IsBeigin = !IsBeigin;
            if (IsBeigin)
            {
                button1.Content = "停止";
                Begin();
            }else{

                button1.Content = "开始";
                End();
            }
        }
        void Begin()
        {
            labelMain.Text = "0";
            t = 0;
            mt.Tick += mt_Tick;
            mt.Start();
        }

        void End()
        {
            mt.Stop();
        }

        void mt_Tick(object sender, object e)
        {
            t++;
            labelMain.Text = t.ToString();
        }

        public string getName()
        {
            throw new NotImplementedException();
        }

        public string getMemo()
        {
            throw new NotImplementedException();
        }

        public string getDimension()
        {
            throw new NotImplementedException();
        }

        public double getValue()
        {
            throw new NotImplementedException();
        }

        public double getScore()
        {
            throw new NotImplementedException();
        }

        public string getReport()
        {
            throw new NotImplementedException();
        }


        public List<IParameter> getSubParameter()
        {
            throw new NotImplementedException();
        }

        public double getProcess()
        {
            throw new NotImplementedException();
        }

        public ParameterState getState()
        {
            throw new NotImplementedException();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }

        public void InitViewModel()
        {
            if (DataContext == null) return;
            var vm = DataContext as CommonToolViewModel;
            if (vm == null) return;
            vm.Title = "Action Speed Test Tool";
            vm.IsUseCommonTestButton = Visibility.Collapsed;
        }
    }
}
