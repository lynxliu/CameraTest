using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Input;
using SLPhotoTest.UIControl;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SLPhotoTest
{
    public partial class LMultiPhoto : UserControl
    {
        public LMultiPhoto()
        {
            InitializeComponent();
            DataContext = new LMutiPhotoViewModel();
        }

        public void Clear()
        {
            var vm = DataContext as LMutiPhotoViewModel;
            if (vm == null) return;
            vm.PhotoList.Clear();
            //foreach (TabItem ti in tabControl.Items)
            //{
            //    LChartPhoto lc = ti.Content as LChartPhoto;
            //    lc.Clear();
            //}
            //tabControl.Items.Clear();
        }

        public List<WriteableBitmap> getPhotoList()
        {
            var vm = DataContext as LMutiPhotoViewModel;
            if (vm == null) return null;
            var pl = new List<WriteableBitmap>();
            foreach (var p in vm.PhotoList)
            {
                pl.Add(p.Photo);
            }
            return pl;
            //List<WriteableBitmap> PhotoList = new List<WriteableBitmap>();
            //foreach (TabItem ti in tabControl.Items)
            //{
            //    LChartPhoto lc = ti.Content as LChartPhoto;
            //    PhotoList.Add(lc.getPhoto());
            //}
            //return PhotoList;
        }

        public WriteableBitmap getCurrentPhoto()
        {
            var vm = DataContext as LMutiPhotoViewModel;
            if (vm == null) return null;
            return vm.CurrentPhoto;
            //if (tabControl.SelectedItem == null) { return null; }
            //TabItem ti = tabControl.SelectedItem as TabItem;
            //return ti.Content as LChartPhoto;
        }

        string getNextName()
        {
            return "Photo" + DateTime.Now.ToString();
            
        }
        public void AddPhoto(WriteableBitmap photo)
        {
            var dc = DataContext as LMutiPhotoViewModel;
            if (dc != null)
            {
                dc.PhotoList.Add(new PhotoInfo() { Name = getNextName(), Photo = photo });
            }
            //TabItem ta = new TabItem();
            //TabHeader th = new TabHeader();
            //th.setTarget(getNextName(), ta, tabControl, TabItemClose);
            //ta.Header = th;
            //LChartPhoto lc = new LChartPhoto();
            //lc.setPhoto(photo);
            //ta.Content = lc;
            //lc.Width = Width - 10;
            //lc.Height = Height - 35;
            //tabControl.Items.Add(ta);
        }


        public void AddPhoto(string n,WriteableBitmap photo)
        {
            var dc = DataContext as LMutiPhotoViewModel;
            if (dc != null)
            {
                dc.PhotoList.Add(new PhotoInfo() { Name = n, Photo = photo });
            }
            //TabItem ta = new TabItem();
            //TabHeader th = new TabHeader();
            //th.setTarget(n, ta, tabControl, TabItemClose);
            //ta.Header = th;
            //LChartPhoto lc = new LChartPhoto();
            //lc.setPhoto(photo);
            //ta.Content = lc;
            //lc.Width = Width - 10;
            //lc.Height = Height - 35;
            //tabControl.Items.Add(ta);
        }

        public void RemovePhoto(string n)
        {
            var dc = DataContext as LMutiPhotoViewModel;
            if (dc != null)
            {
                var dt=dc.PhotoList.FirstOrDefault(v=>v.Name==n);
                dc.PhotoList.Remove(dt);
            }
            //TabItem ti = getTabItem(n);
            //if (ti != null)
            //{
            //    TabHeader ta = (ti.Header) as TabHeader;
            //    ta.CloseTab();
            //}
        }

        public PhotoInfo getTabItem(string n)
        {
            var dc = DataContext as LMutiPhotoViewModel;
            if (dc != null)
            {
                return dc.PhotoList.FirstOrDefault(v=>v.Name==n);
            }
            //foreach (TabItem ti in tabControl.Items)
            //{
            //    if (ti.Header is TabHeader)
            //    {
            //        TabHeader ta=(ti.Header) as TabHeader;
            //        if (ta.TabName == n) { return ti; }
            //    }
            //}
            return null;
        }

        public void RemovePhoto(WriteableBitmap b)
        {
            var dc = DataContext as LMutiPhotoViewModel;
            if (dc != null)
            {
                var dt = dc.PhotoList.FirstOrDefault(v => v.Photo == b);
                dc.PhotoList.Remove(dt);
            }
            //TabItem ti = getTabItem(b);
            //if (ti != null)
            //{
            //    TabHeader ta = (ti.Header) as TabHeader;
            //    ta.CloseTab();
            //}
        }

        public PhotoInfo getTabItem(WriteableBitmap b)
        {
            var dc = DataContext as LMutiPhotoViewModel;
            if (dc != null)
            {
                return dc.PhotoList.FirstOrDefault(v => v.Photo == b);
                //dc.PhotoList.Add(new PhotoInfo() { Name = getNextName(), Photo = photo });
            }

            return null;
        }

        //private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    foreach (TabItem ti in tabControl.Items)
        //    {
        //        LChartPhoto lc = ti.Content as LChartPhoto;
        //        lc.Resized(Width, Height);
        //    }
        //}

        //private void scrollBarTab_Scroll(object sender, ScrollEventArgs e)
        //{
        //    foreach (TabItem tb in tabControl.Items)
        //    {
        //        //移动header
        //    }
        //}
    }
    public class LMutiPhotoViewModel : INotifyPropertyChanged
        
    {
        public WriteableBitmap CurrentPhoto { get; set; }
        ObservableCollection<PhotoInfo> photolist = new ObservableCollection<PhotoInfo>();
        public ObservableCollection<PhotoInfo> PhotoList
        {
            get { return photolist; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class PhotoInfo{
        public WriteableBitmap Photo { get; set; }
        public string Name { get; set; }
    }
}
