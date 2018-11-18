using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;





namespace SLPhotoTest.UIControl
{
    public partial class LMultiPhotoHeader : UserControl
    {
        public LMultiPhotoHeader()
        {
            InitializeComponent();
        }
        public LChartPhoto TargetControl;
        //WriteableBitmap CurrentPhoto=null;
        public void Clear()
        {
            foreach (LPhotoHeaderItem ti in comboBoxControl.Items)
            {
                ti.Clear();
                TargetControl.Clear();
            }
            comboBoxControl.Items.Clear();
        }

        public List<WriteableBitmap> getPhotoList()
        {
            List<WriteableBitmap> PhotoList = new List<WriteableBitmap>();
            foreach (LPhotoHeaderItem ti in comboBoxControl.Items)
            {
                if(ti.TargetPhoto!=null)
                    PhotoList.Add(ti.TargetPhoto);
            }
            return PhotoList;
        }

        public WriteableBitmap getCurrentPhoto()
        {

            return TargetControl.getPhoto();
        }

        string getNextName()
        {
            string s = "UnNamed";
            for (int i = 0; i < 1000; i++)
            {
                LPhotoHeaderItem ti = getItem(s + i.ToString());
                if (ti == null) { return s + i.ToString(); }
            }
            return Guid.NewGuid().ToString();
        }
        public void AddPhoto(WriteableBitmap photo)
        {
            LPhotoHeaderItem LP = new LPhotoHeaderItem();
            LP.TargetPhoto = photo;
            LP.TabName = getNextName();
            //CurrentPhoto = photo;
            TargetControl.setPhoto(photo);
            comboBoxControl.Items.Add(LP);
            comboBoxControl.SelectedItem = LP;

        }


        public void AddPhoto(string n, WriteableBitmap photo)
        {
            LPhotoHeaderItem LP = new LPhotoHeaderItem();
            LP.TargetPhoto = photo;
            LP.TabName = n;
            //CurrentPhoto = photo;
            TargetControl.setPhoto(photo);
            comboBoxControl.Items.Add(LP);
            comboBoxControl.SelectedItem = LP;
        }

        public void RemovePhoto(string n)
        {
            LPhotoHeaderItem ti = getItem(n);
            if (ti != null)
            {
                comboBoxControl.Items.Remove(ti);
                comboBoxControl.SelectedItem = null;
                if (TargetControl.getPhoto() == ti.TargetPhoto)
                {
                    TargetControl.Clear();
                }
                ti.Clear();
            }
        }

        public LPhotoHeaderItem getItem(string n)
        {
            foreach (LPhotoHeaderItem ti in this.comboBoxControl.Items)
            {

                    if (ti.TabName == n) { return ti; }

            }
            return null;
        }

        public void RemovePhoto(WriteableBitmap b)
        {
            if (b == null) { return; }
            LPhotoHeaderItem ti = getItem(b);
            if (ti != null)
            {
                comboBoxControl.Items.Remove(ti);
                comboBoxControl.SelectedItem = null;
                if (TargetControl.getPhoto() == ti.TargetPhoto)
                {
                    TargetControl.Clear();
                }
                ti.Clear();
            }
        }

        public LPhotoHeaderItem getItem(WriteableBitmap b)
        {
            foreach (LPhotoHeaderItem ti in this.comboBoxControl.Items)
            {

                if (ti.TargetPhoto == b) { return ti; }

            }
            return null;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //foreach (TabItem ti in tabControl.Items)
            //{
            //    LChartPhoto lc = ti.Content as LChartPhoto;
            //    lc.Resized(Width, Height);
            //}
        }

        private void comboBoxControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LPhotoHeaderItem ti = e.AddedItems[0] as LPhotoHeaderItem;
            if (ti != null)
            {
                if (ti.TargetPhoto != null)
                {
                    TargetControl.setPhoto(ti.TargetPhoto);
                    
                }
            }
        }

        //private void scrollBarTab_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        //{
        //    foreach (TabItem tb in tabControl.Items)
        //    {
        //        //移动header
        //    }
        //}

    }
}
