using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using Windows.Storage;
using Windows.Storage.Pickers;
using System.Windows.Input;
using Windows.Storage.Streams;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

using SilverlightLynxControls;
using System.IO;
using SilverlightPhotoIO;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using SilverlightLFC.common;
using System.Threading.Tasks;

namespace SLPhotoTest.PhotoEdit
{
    //public delegate PhotoEditProcess;

    public partial class PhotoEditCanvas : UserControl//该类是具备编辑图片功能的通用类，所有的结构都在这个类上面，但是处理代码不在
    {//智能画布，处理的真正代码都集成在这里
        //public event
        public double CanvasWidth = 800;
        public double CanvasHeight = 600;
        public string PhotoName = "UnNamed";
        public bool IsChanged = false;
        PathGeometry pg = new PathGeometry();
        public void setClip()//设置剪裁区
        {
            RectangleGeometry rg = new RectangleGeometry();
            rg.Rect = new Rect(0, 0, PhotoLayers.Width, PhotoLayers.Height);
            //pg.Figures.Clear();
            //PathFigure pf = new PathFigure();
            //pg.Figures.Add(pf);
            //pf.StartPoint = new Point(0, 0);
            //LineSegment tl = new LineSegment();
            //tl.Point = new Point(PhotoLayers.Width, 0);
            //LineSegment rl = new LineSegment();
            //rl.Point = new Point(PhotoLayers.Width, PhotoLayers.Height);
            //LineSegment bl = new LineSegment();
            //bl.Point = new Point(0, PhotoLayers.Height);
            //LineSegment ll = new LineSegment();
            //ll.Point = new Point(0, 0);
            //pf.Segments.Add(tl);
            //pf.Segments.Add(rl);
            //pf.Segments.Add(bl);
            //pf.Segments.Add(ll);
            //pf.IsClosed = true;
            //pf.IsFilled = false;

            PhotoLayers.Clip = rg;
        }

        public PhotoEditCanvas()
        {
            InitializeComponent();
            acm = new ActionMove(this, PhotoTitle);
            acs = new ActionResize(PhotoEditFramework, PhotoEditFramework, new LynxResized(_Resized));
            acs.Enable = true;
            InitLayers();//初始化图层

            


        }

        public void SizedCanvas()//设置画布大小
        {
            double xs = CanvasWidth / this.Width;
            double ys = CanvasHeight / this.Height;
            if (xs >= ys)
            {
                PhotoLayers.Height = CanvasHeight / xs;
                double d = this.Height - PhotoLayers.Height;
                Canvas.SetTop(PhotoLayers, d / 2);
                ScalePercent = Width / CanvasWidth;
            }
            else
            {
                PhotoLayers.Width = CanvasWidth / ys;
                double d = this.Width - PhotoLayers.Width;
                Canvas.SetLeft(PhotoLayers, d / 2);
                ScalePercent = Height / CanvasHeight;
            }
            foreach (PhotoLayer pl in PhotoLayers.Children)
            {
                pl.Height = PhotoLayers.Height;
                pl.Width = PhotoLayers.Width;
            }
            //rectangleBorder.Width = PhotoLayers.Width;
            //rectangleBorder.Height = PhotoLayers.Height;
            ShowTitle();
        }

        public void ShowTitle()//显示标题
        {
            string s = PhotoName;
            if (IsChanged)
            {
                s = s + "(UnSaved)";
            }
            s = s + "--" + (ScalePercent / 100).ToString() + "%";
            textTitle.Text = s;
        }

        public double ScalePercent//缩放比例，纵横都必须锁定缩放比例
        {
            get
            {
                return PhotoLayers.Width / CanvasWidth;
            }
            set
            {
                PhotoLayers.Width = CanvasWidth * value;
                PhotoLayers.Height = CanvasHeight * value;
            }
        }

        public void _Resized()//改变大小以后，图层居中，不改变缩放比例
        {
            PhotoTitle.Width = Width;
            buttonClose.Margin = new Thickness(Width - 20, 0, 0, 0);
            SizedCanvas();
            setClip();
            //PhotoLayers.Height = Height - 20;
            //rectangleBorder.Width=
        }

        public Canvas getFramework()
        {
            return this.PhotoEditFramework;
        }
        public Canvas getLayers()
        {
            return PhotoLayers;
        }
        PhotoLayer _SelectedLayer;

        public string Title
        {
            get { return textTitle.Text; }
            set { textTitle.Text = value; }
        }

        public void InitLayers()//如果一个图层都没有，就建立一个默认的图层
        {
            if (PhotoLayers.Children.Count == 0)
            {
                PhotoLayer pl = new PhotoLayer();
                PhotoLayers.Children.Add(pl);
                _SelectedLayer = pl;
            }
        }

        public PhotoLayer SelectLayer//选择的图层
        {
            get
            {
                return _SelectedLayer;
            }
            set
            {
                _SelectedLayer = value;
            }
        }

        public bool CanResize//判断是否可以改变大小，默认都是可以
        {
            get
            {
                return acs.Enable;
            }
            set
            {
                acs.Enable = value;
            }
        }
        ActionMove acm;
        ActionResize acs;

        public async Task<WriteableBitmap> getImage()//得到一个最终拼合图层的位图
        {
            ScaleTransform st=new ScaleTransform();
            st.ScaleX=1 / ScalePercent;
            st.ScaleY=1 / ScalePercent;
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            var tp=PhotoLayers.RenderTransform as TransformGroup;
            if (tp != null) tp.Children.Add(st);
            WriteableBitmap b =await WriteableBitmapHelper.Snapshot(PhotoLayers);
            //WriteableBitmap b = new WriteableBitmap(PhotoLayers, null);
            return b;
        }

        public void Copy()//把图片复制到剪贴板
        {
            //CameraTestDesktop d = CameraTestDesktop.getDesktop();
            if (SelectLayer == null)
            {
                if (PhotoLayers.Children.Count == 0) { return; }
                SelectLayer = PhotoLayers.Children[0] as PhotoLayer;
            }

            PhotoEditManager.Copy(WriteableBitmapHelper.Clone(SelectLayer.getPhoto()));
        }

        public void Paste()//粘贴
        {
            //CameraTestDesktop d = CameraTestDesktop.getDesktop();
            //PhotoLayer pl = new PhotoLayer();
            //pl.Photo.Source = PhotoEditManager.Paste();
            if (SelectLayer == null) { return; }
            SelectLayer.setPhoto(PhotoEditManager.Paste(),ScalePercent);
        }

        public void ReadImageFromFile()//加载照片
        {
            var bl=SilverlightLFC.common.Environment.getEnvironment().OpenImage();
            if (bl == null || bl.Result == null || bl.Result.Count == 0) return;
            var  v=bl.Result.FirstOrDefault();
            if (v == null) return;
            //PhotoLayers.Children.Clear();
            if (SelectLayer == null)
            {
                PhotoLayer pl = new PhotoLayer();
                PhotoLayers.Children.Add(pl);
                SelectLayer = pl;
                //return;
            }

            SelectLayer.setPhoto(WriteableBitmapHelper.Clone(v),ScalePercent);
            setClip();
            //SelectLayer.setScale(ScalePercent);

        }

        public async void WriteImageToFile()//把照片保存下来
        {
            SilverlightLFC.common.Environment.getEnvironment().SaveImage(await getImage());

        }


        public void Clear()
        {
            acs.Clear();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            (this.Parent as Panel).Children.Remove(this);

        }

        public void FlatLayer(PhotoLayer sl,PhotoLayer tl)//拼合图层,包括影像图层拼合,对象影像图层混合拼合和对象图层拼合四种方式
        {//只要有影像图层参加,最终的结果就是影像的

        }

    }
    public delegate void PhotoEditHandler(object sender, PhotoEditEventArgs e);//定义的查询事件
    public class PhotoEditEventArgs : EventArgs//通用的事件类，传送全部的参数
    {
        public string msg;
        public DateTime EventTime = DateTime.Now;//事件发生时刻
        public bool IsSuccess;//标识调用是否成功
        public object ReturnValue;//返回值
        public PhotoEditEventArgs(bool b, string s)
        {
            IsSuccess = b;
            msg = s;
            //ReturnValue = o;
        }
    }
}
