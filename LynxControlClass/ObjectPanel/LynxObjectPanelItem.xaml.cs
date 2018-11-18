using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;

using Windows.UI.Xaml;
using System.Windows.Input;
using SilverlightLFC.common;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Input;

namespace SilverlightLynxControls
{
    public partial class LynxObjectPanelItem : UserControl
    {
        public LynxObjectPanelItem()
        {
            InitializeComponent();
        }
        public LynxObjectPanel ParentPanel;


        public bool IsConstant { get; set; }
        public BitmapImage IconSource
        {
            get { return ItemIcon.Source as BitmapImage; }
            set { ItemIcon.Source = value; }
        }

        public string Title
        {
            get { return ItemText.Text; }
            set { ItemText.Text = value; }
        }

        string _Memo = null;
        public string Memo
        {
            get { return _Memo; }
            set
            {
                if (value != null)
                {
                    ToolTipService.SetToolTip(ItemText, value);
                    _Memo = value;
                }
            }
        }

        public LynxObjectPanelItem(BitmapImage b, string s, LynxObjectPanel Parent)
        {
            InitializeComponent();
            
            ItemIcon.Source = b;
            ItemText.Text = s;
            ParentPanel = Parent;
        }

        public void Expand(double h)
        {
            if (h < 27) { return; }
            Height = h;
            stackContent.Height = h - 27;
        }

        public void Collapse()
        {
            stackContent.Height = 0;
            Height = 27;
        }

        LFCObjectList<IPanelShowObject> _DataObject = new LFCObjectList<IPanelShowObject>();
        public LFCObjectList<IPanelShowObject> DataObject
        {
            get { return _DataObject; }
            set
            {
                if (_DataObject != value)
                {
                    if(_DataObject!=null)
                        _DataObject.LFCListObjectChanged -= new LFCListItemChanged(DataObject_LFCListObjectChanged);
                    _DataObject = value;
                    if (_DataObject != null)
                        ReadFromObject();
                    else
                    {
                        stackContent.Children.Clear();
                    }
                }

            }
        }

        public void Clear()
        {
            stackContent.Children.Clear();
        }

        public void ReadFromObject()
        {
            DataObject.LFCListObjectChanged+=new LFCListItemChanged(DataObject_LFCListObjectChanged);

        }

        void DataObject_LFCListObjectChanged(IAbstractLFCDataObject o, LFCObjChanged lo)
        {
            if (lo == LFCObjChanged.ObjectChanged)
            {

            }
            if (lo == LFCObjChanged.ObjectCreated)
            {

            }
            if (lo == LFCObjChanged.ObjectDeleted)
            {

            }
        }

        public List<LynxPanelObjectControl> getShowControl(IPanelShowObject o)
        {
            List<LynxPanelObjectControl> cl = new List<LynxPanelObjectControl>();
            foreach (FrameworkElement fe in stackContent.Children)
            {
                LynxPanelObjectControl lc = fe as LynxPanelObjectControl;
                if (lc != null)
                {
                    if(lc.DataObject==o||(lc.DataObject.ObjectID!=null&&lc.DataObject.ObjectID!=""&&lc.DataObject.ObjectID==o.ObjectID))
                    {
                        cl.Add(lc);
                    }
                }
            }
            return cl;
        }

        public LynxPanelObjectControl ActiveControl
        {
            get
            {
                foreach (FrameworkElement fe in stackContent.Children)
                {
                    LynxPanelObjectControl lc = fe as LynxPanelObjectControl;
                    if (lc != null)
                    {
                        if (lc.IsActive)
                        {
                            return lc;
                        }
                    }
                }
                return null;
            }
        }

        public void ClearActive()
        {
            foreach (FrameworkElement fe in stackContent.Children)
            {
                LynxPanelObjectControl lc = fe as LynxPanelObjectControl;
                if (lc != null)
                {
                    lc.IsActive = false;
                }
            }
        }
        public void AddControl(LynxPanelObjectControl c)
        {
            if (!stackContent.Children.Contains(c))
            {
                stackContent.Children.Add(c);
            }
        }
        public void DeleteControl(LynxPanelObjectControl c)
        {
            if (stackContent.Children.Contains(c))
            {
                stackContent.Children.Remove(c);
            }
        }
        private void UserControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            
            if (ParentPanel.DragDropControl != null && !IsDown)
            {
                ParentPanel.DragDropControlSourceItem.DeleteControl(ParentPanel.DragDropControl);
                AddControl(ParentPanel.DragDropControl);
                ParentPanel.DragDropControlSourceItem = null;
                ParentPanel.DragDropControl = null;
            }
            IsDown = false;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvasTitle.Width = e.NewSize.Width;
            stackContent.Width = e.NewSize.Width;
        }
        bool IsDown = false;
        private void UserControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SaveTitle();
            IsDown = true;
        }
        Brush ActiveBrush = new SolidColorBrush(Colors.Red);
        Brush DeActiveBrush = new SolidColorBrush(Colors.Black);
        private void UserControl_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if(ParentPanel.DragDropControl!=null&&!IsDown)
            ItemText.Foreground = ActiveBrush;
        }

        private void UserControl_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if ((IsDown)&&(ActiveControl!=null))
            {
                ParentPanel.DragDropControl = ActiveControl;
                ParentPanel.DragDropControlSourceItem = this;
            }
            if (ItemText.Foreground == ActiveBrush)
            {
                ItemText.Foreground = DeActiveBrush;
            }
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ParentPanel.Active(this);
        }

        void EditTitle()
        {
            ItemText.Visibility = Visibility.Collapsed;
            ItemTextEdit.Visibility = Visibility.Visible;
            ItemTextEdit.Text = ItemText.Text;
        }

        void SaveTitle()
        {
            ItemText.Visibility = Visibility.Visible;
            ItemTextEdit.Visibility = Visibility.Collapsed;
            ItemText.Text = ItemTextEdit.Text;
        }

        private void ItemText_MouseRightButtonDown(object sender, PointerRoutedEventArgs e)
        {
            EditTitle();
        }

        private void ItemText_Holding(object sender, HoldingRoutedEventArgs e)
        {
            EditTitle();
        }

        private void ItemText_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SaveTitle();
        }

        private void ItemTextEdit_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
