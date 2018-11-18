using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightDCTestLibrary;
using DCTestLibrary;
using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SLPhotoTest
{
    public partial class CommonParameter : UserControl
    {
        public CommonParameter()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
        }
        ActionMove acm;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }
    }
}
