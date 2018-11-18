using DCTestLibrary;
using SilverlightLynxControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace LynxControls
{
    public partial class LynxPhotoViewControl : UserControl, INotifyPropertyChanged
    {
        public LynxPhotoViewControl()
        {
            this.InitializeComponent();
            //DataContext = this;
            amI = new ActionMove(PhotoBorder, EditLayer);
            amI.Enable = false;
            selectArea.Stroke = new SolidColorBrush(Colors.Red);
            selectArea.StrokeThickness = 2;
            Canvas.SetZIndex(selectArea, 20);
            //Resized(Width, Height);
            //ResetSize();
            CanScale = false;
        }



        ActionMove amI;
        Rectangle selectArea = new Rectangle();
        Point sp = new Point(double.NaN, double.NaN);
        public void setSelectSize(Point ep)
        {
            //Point sp = new Point(Canvas.GetLeft(selectArea), Canvas.GetTop(selectArea));
            if (!EditLayer.Children.Contains(selectArea)) { return ; }
            selectArea.Width = Math.Abs(ep.X - sp.X);
            selectArea.Height = Math.Abs(ep.Y - sp.Y);
            Canvas.SetLeft(selectArea, Math.Min(ep.X, sp.X));
            Canvas.SetTop(selectArea, Math.Min(ep.Y, sp.Y));
        }

        public void BeginSelect(Point p)
        {
            selectArea.Width = 0;
            selectArea.Height = 0;
            if ((p.X == double.NaN) || (p.Y == double.NaN)) { return; }
            sp = p;
            if (!EditLayer.Children.Contains(selectArea)) { EditLayer.Children.Add(selectArea); }
            Canvas.SetLeft(selectArea, sp.X);
            Canvas.SetTop(selectArea, sp.Y);
        }

        public void EndSelect()
        {
            if (selectArea.Width < 3 && selectArea.Height < 3)
            {
                if (EditLayer.Children.Contains(selectArea)) { EditLayer.Children.Remove(selectArea); }
            }
        }

        public WriteableBitmap getSelectArea()
        {
            if (Photo == null) { return null; }
            if (!EditLayer.Children.Contains(selectArea)) { return null; }
            
            DrawGraphic dg = new DrawGraphic();
            Point isp = DrawGraphic.getImagePosition(new Point(Canvas.GetLeft(selectArea), Canvas.GetTop(selectArea)), getImage());

            Point iep = DrawGraphic.getImagePosition(new Point(Canvas.GetLeft(selectArea) + selectArea.Width, Canvas.GetTop(selectArea) + selectArea.Height), getImage());
            DCTestLibrary.PhotoTest pht = new DCTestLibrary.PhotoTest();
            WriteableBitmap sb = pht.getImageArea(Photo, (int)isp.X, (int)isp.Y, (int)(iep.X - isp.X), (int)(iep.Y - isp.Y));
            return sb;
            
        }

        public bool IsSelect
        {
            get
            {
                if (EditLayer.Children.Contains(selectArea)) { return true; }
                return false;
            }
        }

        public void EnableMove()
        {
            amI.Enable = true;
        }
        public void DisableMove()
        {
            amI.Enable = false;
        }

        public void setPhoto(WriteableBitmap b)
        {
            Photo = b;
        }

        public Image getImage()
        {
            return PhotoImage;
        }
        public Canvas getDrawObjectCanvas()
        {
            return EditLayer;
        }
        public WriteableBitmap getPhoto()
        {
            return Photo;
        }

        public void Zoom(double d)
        {
            ActionShow.ZoomIn(PhotoBorder, d);
            //PhotoBorder.Width = canvasEdit.Width+(2*BorderWidth);
            //PhotoBorder.Height = canvasEdit.Height + (2 * BorderWidth);
        }
        double BorderWidth=3;

        public delegate void ChartAutoTest();
        ChartAutoTest at;
        public void InitTest(ChartAutoTest t)
        {
            at = t;
        }
        public void AutoTest()
        {
            if (at != null) { at(); }
        }

        public void ClearDrawObject()
        {
            EditLayer.Children.Clear();
        }

        public void Clear()
        {
            ClearDrawObject();
            Photo = null;
            //ResetSize();
        }
        public Brush ActiveBrush { get; set; }
        public Brush DeActiveBrush { get; set; }
        public void Active()
        {
            if (ActiveBrush != null) { ActiveBrush = new SolidColorBrush(Colors.Red); }
            PhotoBorder.BorderBrush=ActiveBrush;
        }
        public void DeActive()
        {
            PhotoBorder.BorderBrush = DeActiveBrush;
        }

        public Canvas GetSelectLayer() { return EditLayer; }
        public static readonly DependencyProperty PhotoProperty =
DependencyProperty.Register("Photo", typeof(WriteableBitmap),
 typeof(LynxPhotoViewControl), 
 new PropertyMetadata(null,new PropertyChangedCallback(PhotoChangedCallback)));

        private static void PhotoChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var lc = d as LynxPhotoViewControl;
            lc.PhotoImage.Source = e.NewValue as WriteableBitmap; 
        }

        public WriteableBitmap Photo
        {
            get { return (WriteableBitmap)GetValue(PhotoProperty); }
            set { 
                SetValue(PhotoProperty, value); 
                PhotoImage.Source = value; 
                OnPropertyChanged("Photo"); 
            }
        }

        public static readonly DependencyProperty SelectAreaPhotoProperty =
DependencyProperty.Register("SelectedAreaPhoto", typeof(WriteableBitmap),
typeof(LynxPhotoViewControl), new PropertyMetadata(null));

        public WriteableBitmap SelectAreaPhoto
        {
            get { return (WriteableBitmap)GetValue(SelectAreaPhotoProperty); }
            set { SetValue(SelectAreaPhotoProperty, value); OnPropertyChanged("SelectAreaPhoto"); }
        }
        public static readonly DependencyProperty IsSelectAreaProperty =
DependencyProperty.Register("IsSelectArea", typeof(bool),
typeof(LynxPhotoViewControl), new PropertyMetadata(false));

        public bool IsSelectArea
        {
            get { return (bool)GetValue(IsSelectAreaProperty); }
            set { SetValue(IsSelectAreaProperty, value); OnPropertyChanged("IsSelectArea"); }
        }

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanScale { get; set; }
        private void Grid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (CanScale == false) return;
            UIElement element = sender as UIElement;
            
            CompositeTransform transform = element.RenderTransform as CompositeTransform;
            if (transform == null)
            {
                transform = new CompositeTransform();
                element.RenderTransform = transform;
            }
            transform.ScaleX *= e.Delta.Scale;
            transform.ScaleY *= e.Delta.Scale;
            transform.CenterX = element.RenderSize.Width / 2;
            transform.CenterY = element.RenderSize.Height / 2;
            //transform.Rotation += e.Delta.Rotation * 180 / Math.PI;
            transform.TranslateX += e.Delta.Translation.X;
            transform.TranslateY += e.Delta.Translation.Y;

            e.Handled = true;
        }

        private void Grid_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (CanScale == false) return;
            FrameworkElement element = sender as FrameworkElement;
            element.RenderTransform = null;
            element.Width = Width;
            element.Height = Height;
        }
    }
}
