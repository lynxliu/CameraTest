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
    public partial class ShutterSpeed : UserControl, IPhotoTestWindow
    {
        public ShutterSpeed()
        {
            InitializeComponent();
            BackTimer.Interval = TimeSpan.FromMilliseconds(1000);
            MainTimer.Interval = new TimeSpan(0,0,0,0,10);
            am = new ActionMove(this, this);
        }
        ActionMove am;
        DispatcherTimer BackTimer = new DispatcherTimer();
        DispatcherTimer MainTimer = new DispatcherTimer();

        int b = 5;
        int t = 0;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            Begin();
        }
        void Clear()
        {
            BackTimer.Stop();
            MainTimer.Stop();
            labelBack.Text = "5";
            textBlockMain.Text = "000";

            b = 5;
            t = 0;
        }

        void Begin()
        {
            BackTimer.Tick += (BackTimer_Tick);
            BackTimer.Start();
        }

        void BackTimer_Tick(object sender, object e)
        {
            b--;
            labelBack.Text = b.ToString();
            if (b == 0)
            {
                BackTimer.Stop();
                BackTimer.Tick -= (BackTimer_Tick);

                MainTimer.Tick += (MainTimer_Tick);
                MainTimer.Start();
            }
        }

        void MainTimer_Tick(object sender, object e)
        {
            t++;
            textBlockMain.Text = t.ToString();
            if (t > 500)
            {
                MainTimer.Stop();
                MainTimer.Tick -= (MainTimer_Tick);
                
            }
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
            vm.Title = "Shutter Speed Test Tool";
            vm.IsUseCommonTestButton = Visibility.Collapsed;
        }
    }
}
