using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;




namespace SilverlightLynxControls.Data
{
    public partial class LynxDataView : UserControl
    {
        public LynxDataView()
        {
            InitializeComponent();
        }

        private void buttonQuery_Click(object sender, RoutedEventArgs e)
        {
            
            LynxDataRowList l = new LynxDataRowList();
            LynxWindow.ShowWindow(l);
            l.selectSQL = textBox1.Text;
            if (checkBox1.IsChecked == true)
            {
                l.IsEdit = true;
            }
            l.LoadRecordList();
        }
    }
}
