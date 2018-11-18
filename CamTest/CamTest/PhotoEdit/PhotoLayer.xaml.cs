using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;

using DCTestLibrary;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using SilverlightLFC.common;

namespace SLPhotoTest.PhotoEdit
{
    public partial class PhotoLayer : UserControl//专用的现实照片的图层
    {//support photo load and scale/rotate/transform
        //support draw some geometry
        //mergin layer
        public PhotoLayer()
        {
            InitializeComponent();
            li=  new LayerIcon(this);
            acm = new ActionMove(this, this);
            acm.Enable = false;

            ar = new ActionResize(Layer, Layer, new LynxResized(_Resized));
            ar.Enable = false;//默认不可缩放
        }

        public void _Resized()//只是显示改变不改变比例
        {
            Photo.Width = Width;
            Photo.Height = Height;
            canvasTempObject.Width = Width;
            canvasTempObject.Height = Height;
            if (Layer.Children.Contains(sImg))
            {
                sImg.Width = Width;
                sImg.Height = Height;
            }
            //PhotoLayers.Height = Height - 20;
            //rectangleBorder.Width=
        }
        public void ActiveZoom()//激活可缩放，实际是对照片的拉伸
        {

        }
        ActionMove acm;
        ActionResize ar;
        public Canvas getCanvas()
        {
            return Layer;
        }
        public Canvas getTempObjectCanvas()
        {
            return canvasTempObject;
        }
        public void clearObj()
        {
            canvasTempObject.Children.Clear();
 
        }
        public void clearImg()
        {
            Photo.Source = null; 
        }
        //public void BeginSelect()
        //{
            //Layer.PointerPressed += new MouseButtonEventHandler(Layer_PointerPressed);
            //Layer.PointerReleased += new MouseButtonEventHandler(Layer_PointerReleased);
        //}

        void Layer_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Layer_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #region EditTempObject//编辑矢量对象，矢量对象单独进行选择和编辑
        public bool _IsDrawObject=false;//判断是否是在绘制对象的状态
        public bool _IsEditObject = false;//判断是否处在对象的编辑状态，包括移动和大小
        public void setDrawObject()
        {
            _IsDrawingSelect = false;
            _IsDrawObject = true;
            //_IsSelected = false;
            _IsEditObject = false;
        }
        public void setEditObject()
        {
            _IsDrawingSelect = false;
            _IsDrawObject = false;
            //_IsSelected = false;
            _IsEditObject = true;
        }
        //BorderBrush = new SolidColorBrush(Colors.Blue);
        public int BorderWidth = 3;
        public Brush FillBrush = null;
        Ellipse eo;//需要绘制的椭圆对象
        Rectangle ro;//需要绘制的矩形对象
        Path po;//需要绘制的路径对象
        Point osp;
        public Shape CurrentActiveShap;//当前激活的形状 
        public void DrawElp(Point ocp)
        {
            if (eo == null) { eo = new Ellipse(); }
            if (canvasTempObject.Children.Contains(eo)) { }
            else
            {
                canvasTempObject.Children.Add(eo);
            }
            if (eo.RenderTransform == null)
            {
                eo.RenderTransform = new TransformGroup();
            }
            eo.Fill = FillBrush;
            eo.Stroke = BorderBrush;
            eo.StrokeThickness = BorderWidth;
            eo.Width = Math.Abs(ocp.X - osp.X);
            eo.Height = Math.Abs(ocp.Y - osp.Y);
            Canvas.SetLeft(eo, Math.Min(ocp.X, osp.X));
            Canvas.SetTop(eo, Math.Min(ocp.Y, osp.Y));

            eo.PointerPressed += eo_PointerPressed;

        }

        public void DeActiveShap(Shape s)
        {
            s.StrokeThickness = BorderWidth;
            s.Opacity = 1;
        }

        public void ActiveShap(Shape s)
        {
            s.StrokeThickness = BorderWidth+3;
            s.Opacity = 0.8;
        }

        void CurrentActiveShap_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Point cp=e.GetCurrentPoint(canvasTempObject).Position;
            Transform t = CurrentActiveShap.RenderTransform;
            if (t.GetType().Name == "TransformGroup")
            {
                TransformGroup tg = t as TransformGroup;
                foreach (Transform tf in tg.Children)
                {
                    if (tf.GetType().Name == "TranslateTransform")
                    {
                        TranslateTransform tt = tf as TranslateTransform;
                        tt.X = tt.X+cp.X - osp.X;
                        tt.Y = tt.Y+cp.Y - osp.Y;
                        osp = cp;
                        return;
                    }
                }
                TranslateTransform ntt = new TranslateTransform();
                ntt.X = ntt.X+cp.X - osp.X;
                ntt.Y = ntt.Y+cp.Y - osp.Y;
                osp = cp;
                tg.Children.Add(ntt);
                return;

            }
            if (t.GetType().Name == "TranslateTransform")
            {
                TranslateTransform tt = t as TranslateTransform;
                tt.X = tt.X + cp.X - osp.X;
                tt.Y = tt.Y + cp.Y - osp.Y;
                osp = cp;
                return;
            }
            TransformGroup ntg = new TransformGroup();
            ntg.Children.Add(t);
            TranslateTransform nt = new TranslateTransform();
            nt.X = nt.X + cp.X - osp.X;
            nt.Y = nt.Y + cp.Y - osp.Y;
            osp = cp;
            ntg.Children.Add(nt);
            //throw new NotImplementedException();
        }

        void CurrentActiveShap_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            //throw new NotImplementedException();
            CurrentActiveShap.PointerReleased -= (CurrentActiveShap_PointerReleased);
            CurrentActiveShap.PointerMoved -= new PointerEventHandler(CurrentActiveShap_PointerMoved);

        }

        void eo_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            osp = e.GetCurrentPoint(canvasTempObject).Position;
            DeActiveShap(CurrentActiveShap);
            CurrentActiveShap = sender as Shape;
            ActiveShap(CurrentActiveShap);
            CurrentActiveShap.PointerReleased += (CurrentActiveShap_PointerReleased);
            CurrentActiveShap.PointerMoved +=(CurrentActiveShap_PointerMoved);
            //throw new NotImplementedException();
        }
        public void DrawRec(Point ocp)//绘制指定大小的对象
        {
            if (ro == null) { ro = new Rectangle(); }
            if (canvasTempObject.Children.Contains(ro)) { }
            else
            {
                canvasTempObject.Children.Add(ro);
            }
            if (ro.RenderTransform == null)
            {
                ro.RenderTransform = new TransformGroup();
            }
            ro.Width = Math.Abs(ocp.X - osp.X);
            ro.Height = Math.Abs(ocp.Y - osp.Y);
            Canvas.SetLeft(ro, Math.Min(ocp.X, osp.X));
            Canvas.SetTop(ro, Math.Min(ocp.Y, osp.Y));
            ro.Fill = FillBrush;
            ro.Stroke = BorderBrush;
            ro.StrokeThickness = BorderWidth;
            ro.PointerPressed += (eo_PointerPressed);
        }
        public void DrawPath(Point ocp)
        {
            if (po == null) { po = new Path(); }
            if (canvasTempObject.Children.Contains(po)) { }
            else
            {
                canvasTempObject.Children.Add(po);
            }
            if (po.RenderTransform == null)
            {
                po.RenderTransform = new TransformGroup();
            }
            PathGeometry pg = po.Data as PathGeometry;
            if (pg == null)
            {
                pg = new PathGeometry();
                po.Data = pg;
            }
            if (pg.Figures.Count == 0)
            {
                pg.Figures.Add(new PathFigure());
            }
            PathFigure pf = pg.Figures[0];
            pf.StartPoint = osp;

            LineSegment ls = new LineSegment();
            
            ls.Point = ocp;
            pf.Segments.Add(ls);

            po.Fill = FillBrush;
            po.Stroke = BorderBrush;
            po.StrokeThickness = BorderWidth;
            po.PointerPressed += new PointerEventHandler(eo_PointerPressed);
        }



        #endregion

        #region SelectManager //选区管理，选区只针对照片，绘制选区和剪裁区区域需要不同的Geometry
        //bool _IsSelected = false;//判断是否开始了选择，选择状态下，所有的编辑都是针对选择区的
        Path pt;//选区边界
        Geometry SelectArea = new RectangleGeometry();//选择区的内部，和边界不同，不能够用同一个
        bool _IsDrawingSelect = false;//表示是否正在编辑选区
        Image sImg = new Image();//保留选择区的图像
        public void setDrawSelect()
        {
            _IsDrawingSelect = true;
            _IsDrawObject = false;
            //_IsSelected = false;
            _IsEditObject = false;
        }
        public void setSelected()
        {
            _IsDrawingSelect = false;
            _IsDrawObject = false;
            //_IsSelected = true;
            _IsEditObject = false;
        }
        public void InitSelect()//初始化一个选择层，所有的操作都是针对这个层做的。实际就是复制select图层
        {//清除所有的选择

            //canvasSelect.Clip = null;
            //canvasSelect.Children.Clear();
            sImg.Width = Photo.Width;
            sImg.Height = Photo.Height;
            sImg.Stretch = Stretch.Fill;
            Canvas.SetLeft(sImg, Canvas.GetLeft(Photo));
            Canvas.SetTop(sImg, Canvas.GetTop(Photo));
            sImg.Source = Photo.Source;
            //sImg.Clip = pt.Data as PathGeometry;
            if (Layer.Children.Contains(sImg))
            {            }
            else
            {
                Layer.Children.Add(sImg);
            }
            //canvasSelect.Children.Add(sImg);
            Canvas.SetZIndex(sImg, 10);//放到最上面
        }

        public void BeginSelect()
        {
            //_IsSelected = true;
            InitSelect();
            //Canvas.SetZIndex(canvasSelect, 10);
            sImg.PointerPressed += new PointerEventHandler(sImg_PointerPressed);
            sImg.PointerReleased += new PointerEventHandler(sImg_PointerReleased);
            sImg.PointerMoved += new PointerEventHandler(sImg_PointerMoved);
        }

        public void EndSelect()
        {
            _IsDrawingSelect = false;
            //_IsSelected = true;
        }

        public async void MergineSelect()//合并选择区编辑的结果，实际是sImg和底层的拼合
        {
            Layer.Children.Remove(canvasTempObject);
            
            //Layer.Children.Remove(Photo);

            WriteableBitmap mb =await WriteableBitmapHelper.Snapshot(Layer);
            //canvasSelect.Children.Clear();
            Photo.Source = mb;
            _Photo = mb;
            sImg.Source = null;
            Layer.Children.Remove(sImg);
            Layer.Children.Remove(canvasTempObject);
        }

        void sImg_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_IsDrawingSelect)
            {
                DrawSelectPath(e.GetCurrentPoint(sImg).Position);
            }
        }

        void sImg_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            PathFigure pf = SelectArea.Figures[0];
            
            if (!pf.IsClosed)
            { pf.IsClosed = true; }//选区必须是闭合的

            


            //PathGeometry tpg = pt.Data as PathGeometry;
            //tpg.FillRule = FillRule.EvenOdd;
            sImg.Clip = SelectArea;
            
            //IsSelect = false;
            _IsDrawingSelect = false;
            sImg.PointerMoved -= new PointerEventHandler(sImg_PointerMoved);
            sImg.PointerPressed -= new PointerEventHandler(sImg_PointerPressed);
            sImg.PointerReleased -= new PointerEventHandler(sImg_PointerReleased);

        }

        void sImg_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            sp = e.GetCurrentPoint(sImg).Position;
            InitSelectPath(sp);
            _IsDrawingSelect = true;
            
            //e.Handled = true;
        }
        Point sp=new Point(-1,-1), ep=new Point(-1,-1);

        //PathFigure pf;//当前的选择区域（未来选区可以叠加）
        public void InitSelectPath(Point cp)
        {
            pt = new Path();
            sp = cp;
            PathGeometry pg = new PathGeometry();
            SelectArea.Figures.Clear();

            pg.FillRule = FillRule.Nonzero;
            pt.Data = pg;
            PathFigure pf = new PathFigure();
            PathFigure spf = new PathFigure();

            pt.StrokeDashArray = new DoubleCollection();
            pt.StrokeDashArray.Add(2);
            pt.StrokeDashArray.Add(1);
            pg.Figures.Add(pf);
            pt.Fill = null;
            pt.Stroke = new SolidColorBrush(Colors.Red);
            pt.StrokeThickness = 5;

            pf.StartPoint = cp;
            spf.StartPoint=cp;
            SelectArea.Figures.Add(spf);

            Layer.Children.Add(pt);
            //canvasSelect.Children.Add(pt);
        }
        public void DrawSelectPath(Point cp)
        {
            if (_IsDrawingSelect)//表示是第一次有个起始点
            {
                PathGeometry pg = pt.Data as PathGeometry;

                LineSegment l = new LineSegment();
                LineSegment sl = new LineSegment();
                l.Point = cp;
                sl.Point = cp;
                pg.Figures[0].Segments.Add(l);
                SelectArea.Figures[0].Segments.Add(sl);

            }

        }
        #endregion


        public void setSelect(Geometry sg)
        {
            //SelectPath = sg;
        }
        public PhotoLayer getClip()
        {
            PhotoLayer pl = new PhotoLayer();
            if (Layer.Children.Contains(sImg))
            {
                Rect r = sImg.Clip.Rect;
                WriteableBitmap cb = DrawGraphic.getImageArea(r, sImg);
                pl.setPhoto(cb, ScalePercent,sImg.Clip);
            }
            else
            {
                pl.setPhoto(Photo.Source as WriteableBitmap, ScalePercent);
                
            }
            return pl;
        }

        public PhotoLayer getCopyClip()//找到截图
        {
            PhotoLayer t = getCopy();
            //PathGeometry g=SelectPath.Data as PathGeometry ;

            //t.SelectPath = SelectPath;
            //t.Layer.Clip = SelectPath;
            return t;
        }

        public Canvas getDrawCanvas()//获取允许绘画的画布
        {
            return canvasTempObject;
        }

        //public void select(GeometryGroup gg)//依据一组gg添加一个选区
        //{//在该图层上面复制一个图像，然后设置一个clip
        //    Image si = new Image();
        //    si.Height = Photo.Height;
        //    si.Width = Photo.Width;
        //    gg.FillRule = FillRule.EvenOdd;
        //    PathFigure pf=null;
            
        //    WriteableBitmap wb = new WriteableBitmap(Photo.Source);

        //}

        public PhotoLayer getCopy()
        {
            PhotoLayer t = new PhotoLayer();
            t.setPhoto(Photo.Source as WriteableBitmap,ScalePercent);
            foreach (UIElement ui in canvasTempObject.Children)
            {
                Type ut = ui.GetType();
                if (ut.Name == "Rectangle")
                {
                    Rectangle tr = new Rectangle();
                    t.Layer.Children.Add(tr);
                    tr.Fill = ((Rectangle)ui).Fill;
                    tr.Stroke = ((Rectangle)ui).Stroke;
                    tr.Width = ((Rectangle)ui).Width;
                    tr.Height = ((Rectangle)ui).Height;
                    Canvas.SetLeft(tr, Canvas.GetLeft(ui));
                    Canvas.SetTop(tr, Canvas.GetTop(ui));
                }
                if (ut.Name == "Ellipse")
                {
                    Ellipse tr = new Ellipse();
                    t.Layer.Children.Add(tr);
                    tr.Fill = ((Ellipse)ui).Fill;
                    tr.Stroke = ((Ellipse)ui).Stroke;
                    tr.Width = ((Ellipse)ui).Width;
                    tr.Height = ((Ellipse)ui).Height;
                    Canvas.SetLeft(tr, Canvas.GetLeft(ui));
                    Canvas.SetTop(tr, Canvas.GetTop(ui));
                }
                if (ut.Name == "Path")
                {
                    Path tr = new Path();
                    PathGeometry tp = new PathGeometry();
                    PathGeometry sp = ((Path)ui).Data as PathGeometry;
                    
                    foreach (PathFigure pf in sp.Figures)
                    {
                        PathFigure tpf = new PathFigure();
                        tp.Figures.Add(tpf);
                        tpf.StartPoint = new Point(pf.StartPoint.X, pf.StartPoint.Y);
                        tpf.IsClosed = pf.IsClosed;
                        tpf.IsFilled = pf.IsFilled;
                        foreach (PathSegment ps in pf.Segments)
                        {
                            Type pathtype = ps.GetType();
                            if (pathtype.Name == "LineSegment")
                            {
                                LineSegment ls = new LineSegment();
                                ls.Point = new Point(((LineSegment)ps).Point.X, ((LineSegment)ps).Point.Y);
                                tpf.Segments.Add(ls);
                            }
                            if (pathtype.Name == "ArcSegment")
                            {
                                ArcSegment ls = new ArcSegment();
                                ls.Point = new Point(((LineSegment)ps).Point.X, ((LineSegment)ps).Point.Y);
                                ls.RotationAngle = ((ArcSegment)ps).RotationAngle;
                                ls.Size = new Size(((ArcSegment)ps).Size.Width, ((ArcSegment)ps).Size.Height);
                                ls.SweepDirection = ((ArcSegment)ps).SweepDirection;
                                tpf.Segments.Add(ls);
                            }
                        }
                    }
                    tr.Data = tp;
                    t.Layer.Children.Add(tr);
                    tr.Fill = ((Path)ui).Fill;
                    tr.Stroke = ((Path)ui).Stroke;
                    tr.Width = ((Path)ui).Width;
                    tr.Height = ((Path)ui).Height;
                    Canvas.SetLeft(tr, Canvas.GetLeft(ui));
                    Canvas.SetTop(tr, Canvas.GetTop(ui));
                }
            }
            return t;
        }

        public LayerIcon li;
        //public void setPhoto(WriteableBitmap b)
        //{
        //    double xs = Convert.ToDouble(b.PixelWidth) / this.Width;
        //    double ys = Convert.ToDouble(b.PixelHeight) / this.Height;
        //    if (xs >= ys)
        //    {
        //        Photo.Height = b.PixelHeight / xs;
        //        double d = this.Height - Photo.Height;
        //        Canvas.SetTop(Photo, d / 2);
        //        ScalePercent = Width / b.PixelWidth ;
        //    }
        //    else
        //    {
        //        Photo.Width = b.PixelWidth / ys;
        //        double d = this.Width - Photo.Width;
        //        Canvas.SetLeft(Photo, d / 2);
        //        ScalePercent = Height / b.PixelHeight ;
        //    }
        //    ProcPhoto = b;
        //    Photo.Source = ProcPhoto;
        //}

        public void setScale(double Percent)//设置比例，图层显示照片以后进行的比例缩放
        {
            Width = Width * Percent;
            Height = Height * Percent;
            Photo.Height = Height;
            Photo.Width = Width;
        }

        public void setPhoto(WriteableBitmap b,double per)//必须是统一的缩放比例，这个比例由画布决定而不是图层
        {
            Photo.Width = b.PixelWidth*per;
            Photo.Height = b.PixelHeight * per;
            canvasTempObject.Width = Photo.Width;
            canvasTempObject.Height = Photo.Height;
            ProcPhoto = b;
            Photo.Source = ProcPhoto;
            Layer.Width = Photo.Width;//图层依据照片进行变化
            Layer.Height = Photo.Height;
            this.Width = Photo.Width;
            this.Height = Photo.Height;
        }

        public void setPhoto(WriteableBitmap b, double per,RectangleGeometry pg)//必须是统一的缩放比例，这个比例由画布决定而不是图层
        {
            Photo.Width = b.PixelWidth * per;
            Photo.Height = b.PixelHeight * per;
            canvasTempObject.Width = Photo.Width;
            canvasTempObject.Height = Photo.Height;
            ProcPhoto = b;
            Photo.Source = ProcPhoto;
            Layer.Width = Photo.Width;//图层依据照片进行变化
            Layer.Height = Photo.Height;
            this.Width = Photo.Width;
            this.Height = Photo.Height;
            Photo.Clip = pg;
        }


        public double ScalePercent = 1;

        public WriteableBitmap getPhoto()
        {
            return ProcPhoto;
        }
        public Image getImage()
        {
            return Photo;
        }
        public void Selected()
        {

        }
        WriteableBitmap _Photo;
        public WriteableBitmap ProcPhoto
        {
            get
            {
                return _Photo;
            }
            set
            {
                _Photo = value;
                Photo.Source = value;
            }
        }

        public void MoveX(int n)
        {
            LayerTransform.X = n;
        }
        public void MoveY(int n)
        {
            LayerTransform.Y = n;
        }
        public void ScaleX(double n)
        {
            LayerScale.ScaleX = n;
        }
        public void ScaleY(double n)
        {
            LayerScale.ScaleY = n;
        }
        public void Rotate(double n)
        {
            LayerRotate.Angle = n;
        }
        public void SkewX(double n)
        {
            LayerSkew.AngleX = n;
        }
        public void SkewY(double n)
        {
            LayerSkew.AngleY = n;
        }
        string _Name="UnNamed Layer";
        string _LayerType="common layer";
        string _Memo="";
        public string LayerName
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public string LayerType
        {
            get { return _LayerType; }
            set { _LayerType = value; }
        }
        public string Memo
        {
            get { return _Memo; }
            set { _Memo = value; }
        }

    }
}
