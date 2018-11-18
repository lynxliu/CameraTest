using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SilverlightLynxControls
{
    public partial class LynxObjectPanel : UserControl
    {
        public LynxObjectPanel()
        {
            InitializeComponent();
            Uri u = new Uri("//SilverlightLynxControls;component//Images//freemind.png", UriKind.RelativeOrAbsolute);
            _DefaultIcon = new BitmapImage(u);

        }

        public void RemoveAll()
        {
            stackContent.Children.Clear();
        }

        public void HideAll()
        {
            foreach (FrameworkElement fe in stackContent.Children)
            {
                if (fe is LynxObjectPanelItem)
                {
                    (fe as LynxObjectPanelItem).Collapse();
                }
            }
        }

        public LynxObjectPanelItem CurrentItem { get; set; }

        LynxPanelObjectControl _CurrentControl;
        public LynxPanelObjectControl CurrentControl
        {
            get
            {
                return _CurrentControl;
            }
            set
            {
                _CurrentControl = value;
                if (changeCurrentObject != null)
                {
                    changeCurrentObject(value.DataObject);

                }

            }
        }

        public UserControl PropertyControl
        {
            get;
            set;
        }

        public List<LynxObjectPanelItem> getItems()
        {
            List<LynxObjectPanelItem> il = new List<LynxObjectPanelItem>();
            foreach(LynxObjectPanelItem i in stackContent.Children)
            {
                il.Add(i);
            }
            return il;
        }

        public void Active(LynxObjectPanelItem item)
        {
            HideAll();
            item.Expand(Height - stackContent.Children.Count * 27-50);
            CurrentItem = item;
        }

        public void AddNewItem(BitmapImage b, string n, List<IPanelShowObject> ol)
        {
            LynxObjectPanelItem item = new LynxObjectPanelItem(b, n, this);
            stackContent.Children.Add(item);
            Active(item);
        }

        public void AddNewItem()
        {
            if (DefaultObjectList == null)
            {
                DefaultObjectList = new List<IPanelShowObject>();
            }
            AddNewItem(DefaultIcon, DefaultTitle, DefaultObjectList);
        }

        public void AddNewItem(LynxObjectPanelItem item)
        {
            item.ParentPanel = this;
            stackContent.Children.Add(item);
            Active(item);
        }

        public void DeleteCurrent()
        {
            if (CurrentItem == null) { return; }
            if (stackContent.Children.Contains(CurrentItem))
                stackContent.Children.Remove(CurrentItem);
            if (stackContent.Children.Count <= 0)
            {
                CurrentItem = null;
            }
            else
            {
                CurrentItem = stackContent.Children[0] as LynxObjectPanelItem;
            }
        }

        public void AddObject()
        {
            if (CurrentItem == null) { return; }
            if (CurrentControl == null) { return; }
            CurrentItem.AddControl(CurrentControl);
        }

        public void DeleteObject()
        {
            if (CurrentItem == null) { return; }
            if (CurrentControl == null) { return; }
            CurrentItem.DeleteControl(CurrentControl);
        }

        public void LoadAsDefault(LynxObjectPanelItem item)
        {
            
        }

        BitmapImage _DefaultIcon;
        public BitmapImage DefaultIcon { get { return _DefaultIcon; } set { _DefaultIcon = value; } }

        string _DefaultTitle = "Custom";
        public string DefaultTitle { get { return _DefaultTitle; } set { _DefaultTitle = value; } }
        public List<IPanelShowObject> DefaultObjectList { get; set; }

        public LynxPanelObjectControl DragDropControl;
        public LynxObjectPanelItem DragDropControlSourceItem;

        private void buttonAddItem_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }

        private void buttonRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteCurrent();
        }

        private void buttonAddObj_Click(object sender, RoutedEventArgs e)
        {
            AddObject();
        }

        private void buttonRemoveObj_Click(object sender, RoutedEventArgs e)
        {
            DeleteObject();
        }

        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (showData != null) showData();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (saveShowData != null) saveShowData();
        }

        public SavePanel saveShowData;
        public ShowPanel showData;

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            if (saveShowData != null) saveShowData();
            if (Parent is Panel) { (Parent as Panel).Children.Remove(this); }
        }

        public ChangeCurrentObject changeCurrentObject;
    }

    public delegate void SavePanel();
    public delegate void ShowPanel();

    public delegate void ChangeCurrentObject(object o);//改变当前选择对象
}
