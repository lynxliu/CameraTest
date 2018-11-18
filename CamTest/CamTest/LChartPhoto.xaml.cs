using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using SilverlightLynxControls;
using DCTestLibrary;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.UI;
using Windows.UI.Xaml;

namespace SLPhotoTest
{
    public partial class LChartPhoto : UserControl,ILynxPhotoOperation//定义一个复合结构，支持常见的对图像的操作
    {//最外面是canvas是图片移动的背景，然后是border是给图片镶边，或者代表激活当前，再内部是canva是绘图区，可以绘制选区，然后是image代表显示的图片
        public void MovePhoto(double dx, double dy)
        {
            PhotoTranslate.X = dx;
            PhotoTranslate.Y = dy;
        }

        public void RotatePhoto(double a)
        {
            PhotoRotate.Angle = a;
        }

        public void ScalePhoto(double sx, double sy)
        {
            PhotoScale.ScaleX = sx;
            PhotoScale.ScaleY = sy;
        }

        public void RestTransform()
        {
            PhotoTranslate.X = 0;
            PhotoTranslate.Y = 0;
            PhotoRotate.Angle = 0;
            PhotoScale.ScaleX = 1;
            PhotoScale.ScaleY = 1;
        }

        public LChartPhoto()
        {
            InitializeComponent();
            amI = new ActionMove(PhotoBorder, canvasEdit);
            amI.Enable = false;
            selectArea.Stroke = new SolidColorBrush(Colors.Red);
            selectArea.StrokeThickness = 2;
            Canvas.SetZIndex(selectArea, 20);
            Resized(Width, Height);
            ResetSize();
            //_DefaultWidth = Width;
            //_DefaultHeight = Height;
        }

        public void ResetSize()
        {
            //ImageHeight = _DefaultHeight;
            //ImageWidth = _DefaultWidth;
            Canvas.SetLeft(PhotoBorder, 0);
            Canvas.SetTop(PhotoBorder, 0);
            Canvas.SetLeft(canvasEdit, 0);
            Canvas.SetTop(canvasEdit, 0);
            ActionShow.ResetZoom(PhotoBorder);
            //PhotoBorder.Width = Width;
            //PhotoBorder.Height = Height;
            //double zoomX = canvasEdit.Width / Width;
            //double zoomY = canvasEdit.Height / Height;
            //if (zoomX == 0) { zoomX = 1; }
            //if (zoomY == 0) { zoomY = 1; }
            //ActionShow.XZoomCanvasIn(canvasEdit, 1 / zoomX);
            //ActionShow.YZoomCanvasIn(canvasEdit, 1 / zoomY);


        }

        ActionMove amI;
        WriteableBitmap Chart;

        Rectangle selectArea = new Rectangle();
        Point sp = new Point(double.NaN, double.NaN);
        public void setSelectSize(Point ep)
        {
            //Point sp = new Point(Canvas.GetLeft(selectArea), Canvas.GetTop(selectArea));
            if (!canvasEdit.Children.Contains(selectArea)) { return ; }
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
            if (!canvasEdit.Children.Contains(selectArea)) { canvasEdit.Children.Add(selectArea); }
            Canvas.SetLeft(selectArea, sp.X);
            Canvas.SetTop(selectArea, sp.Y);
        }

        public void EndSelect()
        {
            if (selectArea.Width < 3 && selectArea.Height < 3)
            {
                if (canvasEdit.Children.Contains(selectArea)) { canvasEdit.Children.Remove(selectArea); }
            }
        }

        public WriteableBitmap getSelectArea()
        {
            if (Chart == null) { return null; }
            if (!canvasEdit.Children.Contains(selectArea)) { return null; }
            
            DrawGraphic dg = new DrawGraphic();
            Point isp = DrawGraphic.getImagePosition(new Point(Canvas.GetLeft(selectArea), Canvas.GetTop(selectArea)), imageChart);

            Point iep = DrawGraphic.getImagePosition(new Point(Canvas.GetLeft(selectArea) + selectArea.Width, Canvas.GetTop(selectArea) + selectArea.Height), imageChart);
            DCTestLibrary.PhotoTest pht = new DCTestLibrary.PhotoTest();
            WriteableBitmap sb = pht.getImageArea(Chart, (int)isp.X, (int)isp.Y, (int)(iep.X - isp.X), (int)(iep.Y - isp.Y));
            return sb;
            
        }

        public bool IsSelect
        {
            get
            {
                if (canvasEdit.Children.Contains(selectArea)) { return true; }
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
            //if (canvasEdit.Children.Contains(selectArea)) { canvasEdit.Children.Remove(selectArea); }
            Chart = b;
            imageChart.Source = b;
            if (PhotoChanged != null)
            {
                PhotoChanged(b);
            }
        }

        public Image getImage()
        {
            return imageChart;
        }
        public Canvas getDrawObjectCanvas()
        {
            return this.canvasEdit;
        }
        public WriteableBitmap getPhoto()
        {
            return Chart;
        }

        public void Zoom(double d)
        {
            ActionShow.ZoomIn(PhotoBorder, d);
            //PhotoBorder.Width = canvasEdit.Width+(2*BorderWidth);
            //PhotoBorder.Height = canvasEdit.Height + (2 * BorderWidth);
        }
        double BorderWidth=3;
        public void Resized(double w, double h)
        {
            canvasFrame.Width = w;
            PhotoBorder.Width = w;
            canvasEdit.Width = w - (2 * BorderWidth);
            imageChart.Width = w - (2 * BorderWidth);

            canvasFrame.Height = h;
            canvasEdit.Height = h - (2 * BorderWidth);
            imageChart.Height = h - (2 * BorderWidth);
            PhotoBorder.Height = h;
        }

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

        public Stretch PhotoStretch
        {
            get { return imageChart.Stretch; }
            set { imageChart.Stretch = value; }
        }

        public void ClearDrawObject()
        {
            Image ti = imageChart;
            canvasEdit.Children.Clear();
            canvasEdit.Children.Add(ti);
            //imageChart.Source = null;
            //Chart = null;
            //ResetSize();
            //Resized(400, 300);
        }

        public void Clear()
        {
            Image ti = imageChart;
            canvasEdit.Children.Clear();
            canvasEdit.Children.Add(ti);
            imageChart.Source = null;
            Chart = null;
            ResetSize();
            //Resized(400, 300);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Resized(e.NewSize.Width, e.NewSize.Height);
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

        private void imageMulti_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

        }

        private void imageMulti_PointerExited(object sender, PointerRoutedEventArgs e)
        {

        }

        private void imageMulti_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }
        public event CurrentPhotoChanged PhotoChanged;//显示的照片发生了变化

        #region Cross//支持鼠标移动的十字线

        bool _EnableCross = false;
        public bool EnableCross
        {
            get
            {
                return _EnableCross;
            }
            set
            {
                if (value)
                {
                    if(!_EnableCross)
                        InitCross();
                }
                else
                {
                    if(_EnableCross)
                        ReleaseCross();
                }
                _EnableCross = value;
            }
        }


        Line hl = new Line();
        Line vl = new Line();

        void InitCross()
        {
            Canvas c = getDrawObjectCanvas();
            if (!c.Children.Contains(hl))
            {
                c.Children.Add(hl);
            }
            if (!c.Children.Contains(vl))
            {
                c.Children.Add(vl);
            }
            hl.Stroke = vl.Stroke = new SolidColorBrush(Colors.Red);
            hl.StrokeThickness = vl.StrokeThickness = 1;
            hl.X1 = 0;
            hl.X2 = c.Width;
            vl.Y1 = 0;
            vl.Y2 = c.Height;
            c.PointerMoved += new PointerEventHandler(Cross_PointerMoved);
        }

        void Cross_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Point p=e.GetCurrentPoint(sender as Canvas).Position;
            hl.Y1 = hl.Y2=p.Y;
            vl.X1 = vl.X2 = p.X;
        }

        void ReleaseCross()
        {
            Canvas c = getDrawObjectCanvas();
            if (c.Children.Contains(hl))
            {
                c.Children.Remove(hl);
            }
            if (c.Children.Contains(vl))
            {
                c.Children.Remove(vl);
            }
            c.PointerMoved -= new PointerEventHandler(Cross_PointerMoved);
        }

        #endregion

    }

    public delegate void  CurrentPhotoChanged(WriteableBitmap current);
}
