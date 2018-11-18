using System;
using System.Net;
using System.Windows;



using System.Windows.Input;




using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SLPhotoTest.PhotoEdit
{
    public class PhotoEditManager//管理类，专门管理图像处理
    {
        private PhotoEditManager()
        {

        }
        static PhotoEditManager pem = new PhotoEditManager();
        public static PhotoEditManager getPhotoEditManager()
        {
            return pem;
        }

        List<PhotoEditCanvas> EditPhotoList = new List<PhotoEditCanvas>();//所有的正在编辑的对象

        public void ShowWindow(FrameworkElement fe)//桌面上显示任何一个窗体
        {
            Panel p = CameraTestDesktop.getDesktopPanel();
            if (p.Children.Contains(fe)) { }
            else
            {
                p.Children.Add(fe);
            }
        }

        public void HideWindow(FrameworkElement fe)//关闭单独的窗体
        {
            Panel p = CameraTestDesktop.getDesktopPanel();
            if (p.Children.Contains(fe))
            {
                p.Children.Remove(fe);
            }
        }
        public void setTarget(PhotoEditCanvas pc)//设置某个编辑图片和操控窗体关联
        {
            WindowPhotoIO.setTarget(pc);
            WindowLayerEdit.setTarget(pc);
            WindowLayerDraw.setTarget(pc);
            WindowLayerManager.setTarget(pc);
            WindowLayerMove.setTarget(pc);
            WindowLayerInfor.setTarget(pc);
            WindowPhotoView.setTarget(pc);
        }

        public PhotoIO WindowPhotoIO = new PhotoIO();//主菜单
        public LayerEdit WindowLayerEdit = new LayerEdit();
        public LayerDraw WindowLayerDraw = new LayerDraw();
        public LayerManager WindowLayerManager = new LayerManager();
        public LayerMove WindowLayerMove = new LayerMove();
        public LayerInfor WindowLayerInfor = new LayerInfor();
        public PhotoView WindowPhotoView = new PhotoView();



        public void ShowPhotoEditWindow(PhotoEditCanvas pc)
        {
            double l, t;//默认编辑是在右侧
            ShowWindow(pc);
            Panel p = CameraTestDesktop.getDesktopPanel();

            Canvas.SetLeft(pc, (p.Width - pc.Width) / 2);
            Canvas.SetTop(pc, (p.Height - pc.Height) / 2);

            l = Canvas.GetLeft(pc);
            t = Canvas.GetTop(pc);

            ShowWindow(WindowLayerEdit);
            WindowLayerEdit.setTarget(pc);
            Canvas.SetLeft(WindowLayerEdit, l + pc.Width);
            Canvas.SetTop(WindowLayerEdit, t);

            ShowWindow(WindowLayerDraw);
            WindowLayerDraw.setTarget(pc);
            Canvas.SetLeft(WindowLayerDraw, l);
            Canvas.SetTop(WindowLayerDraw, t - WindowLayerDraw.Height);

            ShowWindow(WindowPhotoIO);
            WindowPhotoIO.setTarget(pc);
            Canvas.SetLeft(WindowPhotoIO, l);
            Canvas.SetTop(WindowPhotoIO,t - WindowPhotoIO.Height - WindowPhotoIO.Height );

            ShowWindow(WindowLayerManager);
            WindowLayerManager.setTarget(pc);
            Canvas.SetLeft(WindowLayerManager, l - WindowLayerManager.Width);
            Canvas.SetTop(WindowLayerManager, t+30);

            ShowWindow(WindowLayerMove);
            WindowLayerMove.setTarget(pc);
            Canvas.SetLeft(WindowLayerMove, l + pc.Width);
            Canvas.SetTop(WindowLayerMove, t - WindowPhotoIO.Height - WindowLayerDraw.Height);

            ShowWindow(WindowLayerInfor);
            WindowLayerInfor.setTarget(pc);
            Canvas.SetLeft(WindowLayerInfor, l);
            Canvas.SetTop(WindowLayerInfor, t + pc.Height);

            ShowWindow(WindowPhotoView);
            WindowPhotoView.setTarget(pc);
            Canvas.SetLeft(WindowPhotoView, l + pc.Width);
            Canvas.SetTop(WindowPhotoView, t + pc.Height + 70);

        }

        public void NewPhotoEdit()//新建
        {
            PhotoEditCanvas pc = new PhotoEditCanvas();
            EditPhotoList.Add(pc);
            ShowPhotoEditWindow(pc);
            //setTarget(pc);
        }

        public void StartPhotoEdit()
        {
            Panel p = CameraTestDesktop.getDesktopPanel();
            PhotoEditCanvas pc = new PhotoEditCanvas();
            pc.Title = "Image Process System";
            p.Children.Add(pc);
            Canvas.SetLeft(pc, (p.ActualWidth - pc.Width) / 2);
            Canvas.SetTop(pc, (p.ActualHeight - pc.Height) / 2);

            ShowPhotoEditWindow(pc);
        }


        public void ClosePhotoEdit()
        {
            HideWindow(WindowPhotoIO);
            HideWindow(WindowLayerEdit);
            HideWindow(WindowLayerDraw);
            HideWindow(WindowLayerManager);
            HideWindow(WindowLayerMove);
            HideWindow(WindowLayerInfor);
            HideWindow(WindowPhotoView);

            foreach (PhotoEditCanvas pc in EditPhotoList)
            {
                HideWindow(pc);
            }
            EditPhotoList.Clear();
        }

        public static void Copy(WriteableBitmap b)
        {
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            d.addClip(b);
        }

        public static WriteableBitmap Paste()
        {
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            return d.getClip();
        }
    }
}
