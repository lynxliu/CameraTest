using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls.Primitives;

namespace SLPhotoTest.PhotoEdit
{
    public delegate void LayerEventHandler(object sender, LynxPhotoLayerEventArgs e);//定义的系统事件
    public class LynxPhotoLayerEventArgs
    {
        public PhotoLayer currentLayer;
        public LayerIcon currentIcon;
        public bool IsSelected;
    }

    public partial class LayerIcon : UserControl
    {
        public LayerIcon(PhotoLayer p)
        {
            InitializeComponent();
            //acm = new ActionMove(this, this);
            pl = p;
            ReadInfor();
        }
        public event LayerEventHandler selectLayer;
        public void sendSelectEvent(bool r)//给出简单的查询返回，成功，失败和返回的结果集
        {
            LynxPhotoLayerEventArgs le = new LynxPhotoLayerEventArgs();
            le.currentIcon = this;
            le.currentLayer = pl;
            le.IsSelected = r;
            if (selectLayer != null)
            {
                selectLayer(this, le);
            }
        }

        PhotoLayer pl;
        public PhotoLayer getLayer()
        {
            return pl;
        }

        public void WriteInfor()
        {
            if (pl == null) { return; }
            pl.LayerName = textBoxName.Text;
            pl.Opacity = LayerTrans.Value / 100;
            if (checkBox1.IsChecked == true)
            {
                
                pl.Visibility = Visibility.Visible;
            }
            else
            {
                pl.Visibility = Visibility.Collapsed;
            }
        }

        public void ReadInfor()
        {

            icon.Source = pl.getPhoto();
            textBoxName.Text = pl.LayerName;
            LayerTrans.Value = pl.Opacity * 100;
            if (pl.Visibility == Visibility.Visible)
            {
                checkBox1.IsChecked=true;
            }else
            {
                checkBox1.IsChecked=false;
            }
            icon.Source = pl.Photo.Source;
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            WriteInfor();
        }

        private void LayerTrans_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            WriteInfor();
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            WriteInfor();
        }

        public void Select()
        {
            this.LayoutRoot.Background = new SolidColorBrush(Colors.Gray);
        }

        public void UnSelect()
        {
            this.LayoutRoot.Background = new SolidColorBrush(Colors.White);
        }

        private void LayoutRoot_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
            sendSelectEvent(true);
        }

        private void LayoutRoot_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

            sendSelectEvent(false);
        }
    }
}
