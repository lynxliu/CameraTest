using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;




namespace SilverlightLynxControls
{
    public partial class LynxTreeItem : UserControl
    {
        public LynxTreeItem()
        {
            InitializeComponent();
            ShowCheck = false;
        }

        public string Text
        {
            get
            {
                return textBlockText.Text;
            }
            set
            {
                textBlockText.Text = value;
            }
        }

        public ImageSource Icon
        {
            get
            {
                return imageIcon.Source;
            }
            set
            {
                imageIcon.Source = value;
            }
        }

        public bool Checked
        {
            get
            {
                if (checkChecked.IsChecked == true) { return true; } else { return false; }
            }
            set
            {
                if (value)
                {
                    checkChecked.IsChecked = true;
                }
                else
                {
                    checkChecked.IsChecked = false;
                }
            }
        }

        public bool ShowCheck
        {
            get
            {
                if (checkChecked.Visibility == Visibility.Visible) { return true; }
                if (checkChecked.Visibility == Visibility.Collapsed) { return false; }
                return false;
            }
            set
            {
                if (value) { checkChecked.Visibility = Visibility.Visible; }
                else
                {
                    checkChecked.Visibility = Visibility.Collapsed;
                }
            }
        }

        object TargetObject=null;

        private void textBlockText_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ActionDragDrop.BeginDrag(this, TargetObject);
        }
    }
}
