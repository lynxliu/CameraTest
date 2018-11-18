using System;
using System.Net;
using System.Windows;



using System.Windows.Input;

using System.ComponentModel;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;

namespace SilverlightLynxControls.ObjectPanel
{
    public class ViewModelLynxPanelObjectControl : INotifyPropertyChanged
    {
        public virtual BitmapImage Icon
        {
            get { return DataObject.Icon; }
            set
            {
                DataObject.Icon = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Icon"));

            }
        }
        public virtual string Title
        {
            get { return DataObject.Title; }
            set
            {
                DataObject.Title = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Title"));

            }
        }
        public virtual FrameworkElement ToolTipInfor
        {
            get { return DataObject.ToolTipInfor; }
            set
            {
                DataObject.ToolTipInfor = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ToolTipInfor"));

            }
        }

        public virtual Thickness BorderThickness { get { return new Thickness(0); } }

        public IPanelShowObject DataObject { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
