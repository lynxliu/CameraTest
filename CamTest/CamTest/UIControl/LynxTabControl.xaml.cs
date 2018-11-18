using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace CamTest.UIControl
{
    public sealed partial class LynxTabControl : UserControl
    {
        public LynxTabControl()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ItemsSourceProperty =
    DependencyProperty.Register("ItemsSource", typeof(object), typeof(LynxTabControl), new PropertyMetadata(null));

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, this); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
DependencyProperty.Register("SelectedItem", typeof(object), typeof(LynxTabControl), new PropertyMetadata(null));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, this); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(LynxTabControl), new PropertyMetadata(null));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, this); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(LynxTabControl), new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, this); }
        }


    }
}
