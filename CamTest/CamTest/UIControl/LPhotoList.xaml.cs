using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using DCTestLibrary;
using SilverlightPhotoIO;
using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace SLPhotoTest.UIControl
{
    public partial class LPhotoList : UserControl
    {
        public LPhotoList()
        {
            InitializeComponent();
            //am = new ActionMove(stackBitmapList, stackBitmapList);
            //am.EnableV = false;
            //am.EnableH = true;
            //Resized();
            //Clip = MoveArea.RenderSize; ;
            RectangleGeometry rg = new RectangleGeometry();
            rg.Rect = new Rect(0, 0, ListArea.Width, ListArea.Height);
            ListArea.Clip = rg;
            //Width=ListBorder.Width = ListArea.Width + 17;
            //Height=ListBorder.Height = ListArea.Height + 17;
        }
        //ActionMove am;
        //public void Resized()
        //{
            //scrollViewer.Width = Width;
            //scrollViewer.Height = Height;
            //double cw = (stackBitmapList.Height + 10) * stackBitmapList.Children.Count + 10;
            //stackBitmapList.Width = Math.Max(getPhotoCountWidth(), Width);
            //stackBitmapList.Width = Width-10;
            //stackBitmapList.Height = Height-10;
        //}

        public LChartPhoto TargetControl;
        public Border CurrentImage = null;
        List<WriteableBitmap> bl = new List<WriteableBitmap>();

        public void LoadPhotoList(List<WriteableBitmap> l)
        {
            int i = 0;
            foreach (WriteableBitmap b in l)
            {
                bl.Add(b);
                ShowPhoto(b,i);
                i++;
            }
        }
        public List<WriteableBitmap> PhotoList
        {
            get
            {
                return bl;
            }
        }

        public Border getPhotoBorder(WriteableBitmap photo)
        {
            for (int i = 0; i < ListArea.Children.Count; i++)
            {
                Border br = ListArea.Children[i] as Border;
                Image im = br.Child as Image;
                WriteableBitmap b = im.Source as WriteableBitmap;
                if (photo == b) { return br; }
            }
            return null;
        }

        public int getPhotoIndex(WriteableBitmap photo)
        {
            for (int i = 0; i < ListArea.Children.Count; i++)
            {
                Border br = ListArea.Children[i] as Border;
                Image im = br.Child as Image;
                WriteableBitmap b = im.Source as WriteableBitmap;
                if (photo == b) { return i; }
            }
            return -1;
        }


        public int getActiveIndex()
        {
            for (int i = 0; i < ListArea.Children.Count; i++)
            {
                Border br = ListArea.Children[i] as Border;
                if (br.BorderBrush == ActiveBrush)
                {
                    return i;
                }
            }
            return -1;
        }

        public WriteableBitmap getPhoto(int index)
        {
            if((index<0)||(index>bl.Count-1)){return null;}
            return bl[index];
        }

        void RefreshShow()
        {

            CurrentImage = null;
            ClearShow();
            int i = 0;
            foreach (WriteableBitmap b in bl)
            {
                ShowPhoto(b,i);

                i++;
            }
            AbsLeft = 0;
        }
        Brush ActiveBrush = new SolidColorBrush(Colors.Orange);
        public void ClearActive()
        {
            foreach (Border br in ListArea.Children)
            {
                br.BorderBrush = null;
                br.Background = null;
            }
        }
        public void SelectPhoto(Border br)
        {
            if (br != null)
            {
                Image im = br.Child as Image;
                CurrentImage = br;

                WriteableBitmap tb = im.Source as WriteableBitmap;
                TargetControl.setPhoto(tb);
                ClearActive();
                br.BorderBrush = ActiveBrush;
                br.Background = ActiveBrush;
                if (photoOperated != null)
                {
                    photoOperated(tb, PhotoListOperation.Select);
                }
            }
        }
        public void SelectPhoto(WriteableBitmap ph)
        {
            Border sb = getPhotoBorder(ph);
            if (sb != null)
            {
                ClearActive();
                SelectPhoto(sb);
            }
        }
        void li_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            Border br = sender as Border;
            SelectPhoto(br);
        }

        void SetPostion(FrameworkElement fe, int i)
        {
            Canvas.SetLeft(fe, i * (ListArea.Height + 10));
        }

        public void ShowPhoto(WriteableBitmap b,int i)
        {
            Border br = new Border();
            br.BorderThickness = new Thickness(2);
            br.Width = ListArea.Height;
            br.Height = ListArea.Height;
            //br.Margin = new Thickness(10, 0, 10, 0);
            
            Image li = new Image();
            li.Width = ListArea.Height;
            li.Height = ListArea.Height;

            li.Source = b;
            //li.Tag = b;//附属的是原始的图片
            br.Child = li;
            ListArea.Children.Add(br);

            SetPostion(br, i);

            //double cw = (stackBitmapList.Height + 10) * stackBitmapList.Children.Count + 10;
            //stackBitmapList.Width = Math.Max(cw, Width);
            br.PointerPressed += new PointerEventHandler(li_PointerPressed);
            //double d = Height * stackBitmapList.Children.Count;
            //stackBitmapList.Width = Math.Max(Width-10,d);
        }

        public void AddPhoto(WriteableBitmap b)
        {
            if (bl.Contains(b)) { return; }
            bl.Add(b);
            RefreshShow();
            
            if (photoOperated != null)
            {
                photoOperated(b, PhotoListOperation.Add);
            }
            SelectPhoto(b);
        }

        public void RemovePhoto(WriteableBitmap b)
        {
            Border sb = getPhotoBorder(b);
            if (sb != null)
            {
                //ListArea.Children.Remove(sb);
                bl.Remove(b);
                RefreshShow();
                //double cw = (stackBitmapList.Height + 10) * stackBitmapList.Children.Count + 10;
                //stackBitmapList.Width = Math.Max(getPhotoCountWidth(), Width);
                if (photoOperated != null)
                {
                    photoOperated(b, PhotoListOperation.Remove);
                }
            }
            if (bl.Count > 0)
            {
                SelectPhoto(bl[0]);
            }
        }
        //double getPhotoCountWidth()
        //{
        //    return (stackBitmapList.Height + 20) * stackBitmapList.Children.Count + 10;
        //}
        public void DeleteCurrent()
        {
            if (CurrentImage != null)
            {
                CurrentImage.PointerPressed -= new PointerEventHandler(li_PointerPressed);
                Image im = CurrentImage.Child as Image;
                WriteableBitmap b = im.Source as WriteableBitmap;
                im.Source = null;
                //stackBitmapList.Children.Remove(CurrentImage);
                TargetControl.setPhoto(null);
                CurrentImage = null;
                //double d = 50 * stackBitmapList.Children.Count;
                //stackBitmapList.Width = Math.Max(Width - 10, d);
                //double cw = 
                //stackBitmapList.Width = Math.Max(getPhotoCountWidth(), Width);

                bl.Remove(b);
                RefreshShow();
                if (photoOperated != null)
                {
                    photoOperated(b,PhotoListOperation.Remove);
                }
                if (bl.Count > 0)
                {
                    SelectPhoto(bl[0]);
                }
            }
        }

        public event PhotoOperation photoOperated;//当删除照片时候引发

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RefreshShow();
        }

        double CLeft;
        //private void stackBitmapList_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{
        //    ClearActive();
        //    if (photoOperated != null)
        //    {
        //        photoOperated(null, PhotoListOperation.Select);//代表不再选择具体的
        //    }
        //    stackBitmapList.PointerMoved += new PointerEventHandler(stackBitmapList_PointerMoved);
        //    stackBitmapList.PointerReleased += new MouseButtonEventHandler(stackBitmapList_PointerReleased);
        //    CLeft = e.GetPosition(MoveArea).X;
        //    e.Handled = true;
        //}

        void ListArea_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            ListArea.PointerMoved -= new PointerEventHandler(ListArea_PointerMoved);
            ListArea.PointerReleased -= new PointerEventHandler(ListArea_PointerReleased);

        }
        double AbsLeft = 0;//绝对的最左端
        void ListArea_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            double dx=e.GetCurrentPoint(ListArea).Position.X-CLeft;
            
            //double nl = Canvas.GetLeft(ListArea) + dx;
            double TotleWidth = ListArea.Children.Count * (ListArea.Height + 10);
            if (AbsLeft + dx < 0 && AbsLeft + dx > Width - ListArea.Children.Count * (ListArea.Height + 10))
            {
                AbsLeft += dx;
                foreach (FrameworkElement fe in ListArea.Children)
                {
                    Canvas.SetLeft(fe, Canvas.GetLeft(fe)+dx);
                }
                //Canvas.SetLeft(stackBitmapList, nl);
            }
            CLeft = e.GetCurrentPoint(ListArea).Position.X;
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            (Parent as Panel).Children.Remove(this);
            Clear();
        }

        void ClearShow()
        {
            foreach (Border br in ListArea.Children)
            {
                br.PointerPressed -= new PointerEventHandler(li_PointerPressed);
                Image im = br.Child as Image;
                im.Source = null;
            }
            ListArea.Children.Clear();
        }

        public void Clear()
        {
            bl.Clear();
            ClearShow();

            //stackBitmapList.Width =  Width;
        }

        //private void buttonAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    List<WriteableBitmap> bl = PhotoIO.ReadMultiImageFromFile();
        //    if (bl == null) { return; }
        //    foreach (WriteableBitmap b in bl)
        //    {
        //        AddPhoto(b);
        //    }
        //}

        //private void buttonRemove_Click(object sender, RoutedEventArgs e)
        //{
        //    DeleteCurrent();
        //}

        public static LPhotoList ShowList(Panel p, List<WriteableBitmap> bl)
        {
            foreach (FrameworkElement fe in p.Children)
            {
                if (fe is LPhotoList) { return fe as LPhotoList; }
            }
            LPhotoList lpl = new LPhotoList();
            lpl.LoadPhotoList(bl);
            return lpl;
        }

        private void ListArea_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ClearActive();
            if (photoOperated != null)
            {
                photoOperated(null, PhotoListOperation.Select);//代表不再选择具体的
            }
            ListArea.PointerMoved += new PointerEventHandler(ListArea_PointerMoved);
            ListArea.PointerReleased += new PointerEventHandler(ListArea_PointerReleased);
            CLeft = e.GetCurrentPoint(ListArea).Position.X;
            e.Handled = true;
        }
    }
    public enum PhotoListOperation
    {
        Add,Remove,Select
    }
    public delegate void PhotoOperation(WriteableBitmap rp,PhotoListOperation op);
}
