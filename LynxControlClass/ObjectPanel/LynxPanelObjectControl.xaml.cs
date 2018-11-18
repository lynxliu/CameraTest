using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls.ObjectPanel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI;

namespace SilverlightLynxControls
{
    public partial class LynxPanelObjectControl : UserControl
    {
        public LynxPanelObjectControl(LynxObjectPanelItem pi)
        {
            InitializeComponent();
            ParentItem = pi;
        }
        LynxObjectPanelItem ParentItem;

        public bool CanModify = true;//判断是否可以修改组的信息

        public void DeleteFromParentItem()
        {
            if (ParentItem == null) { return; }
            ParentItem.DeleteControl(this);
        }



        IPanelShowObject _DataObject;
        public IPanelShowObject DataObject
        {
            get { return _DataObject; }
            set
            {
                _DataObject = value;
            }
        }

        Brush ActiveBrush = new SolidColorBrush(Colors.Red);
        public bool IsActive
        {
            get
            {
                if (ControlBorder.BorderBrush == null) { return false; }
                return true;
            }
            set
            {
                if (value)
                {
                    ControlBorder.BorderBrush = ActiveBrush;
                }
                else
                {
                    ControlBorder.BorderBrush = null;
                }

            }
        }

        public void ReadFromObject(ViewModelLynxPanelObjectControl o)
        {
            DataContext = o;

            ToolTipService.SetToolTip(this, o.ToolTipInfor);
        }

        public void ReadFromObject(IPanelShowObject o)
        {
            DataObject = o;
            DataContext = o;

            //image1.Source = o.Icon;
            //textBlock1.Text = o.Title;
            //ToolTipService.SetToolTip(this, o.ToolTipInfor);
        }

        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            IsActive = true;
        }

        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (_DataObject == null) { return; }

            popup1.Child = _DataObject.ToolTipInfor;
            popup1.IsOpen = true;
        }

        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            popup1.IsOpen = false;
        }

        private void UserControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            IsActive = false;
        }

        private void image1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (ParentItem.ParentPanel.PropertyControl != null)
            {
                ParentItem.ParentPanel.PropertyControl.DataContext = DataObject;
                //ChildWindow c = new ChildWindow();
                //c.Content = ParentItem.ParentPanel.PropertyControl;
                //c.Show();
                LynxWindow.ShowWindow(ParentItem.ParentPanel.PropertyControl);
            }
            //canvasCntrol.Children.Add();
        }

    }
    public enum ControlShowMode { Lardge, Small }
}
