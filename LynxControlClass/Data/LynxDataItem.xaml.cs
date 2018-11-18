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
    public partial class LynxDataItem : UserControl,ILynxDataItem
    {
        public LynxDataItem()
        {
            InitializeComponent();
        }

        public void setContent(FrameworkElement fe)
        {
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(fe);
        }

        public void ReadFromObject(SilverlightLFC.common.AbstractLFCDataObject o)
        {

        }

        public void ShowObject(Panel p, Dictionary<string, object> r)
        {
            SilverlightLFC.common.LFCDataService ls = new SilverlightLFC.common.LFCDataService();
            foreach (KeyValuePair<string, object> kv in r)
            {
                TextBlock tb = new TextBlock();
                tb.Text = kv.Value.ToString();
                tb.Tag = kv;
                tb.Width = 20;

            }
            p.Children.Add(this);

        }
    }
}
