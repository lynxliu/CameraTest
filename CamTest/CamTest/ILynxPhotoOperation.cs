using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SLPhotoTest
{
    public interface ILynxPhotoOperation//标准的图像操作结构
    {
        void MovePhoto(double dx, double dy);//移动图像
        void RotatePhoto(double a);//旋转图像
        void ScalePhoto(double sx, double sy);//缩放图像
        void RestTransform();//还原到初始设置

        void BeginSelect(Point p);//开始选择，设置起始点
        void setSelectSize(Point ep);//选择过程里面修改其大小
        void EndSelect();//结束选择

        WriteableBitmap getSelectArea();//把选区变为图片
        bool IsSelect{get;}//是否处于选择状态

        void EnableMove();//允许交互式的移动
        void DisableMove();//停止移动

        void setPhoto(WriteableBitmap b);//设置图片
        Image getImage();//获得保存图片的Image对象
        Canvas getDrawObjectCanvas();//获得选择区对象
        WriteableBitmap getPhoto();//获取当前编辑的照片
        void Clear();//清除照片和选区，还原大小设置

        void Zoom(double d);//缩放，缩放是针对整个对象的，不仅仅是照片
        void Resized(double w, double h);//设置新的大小
        void ResetSize();//恢复为原始的大小和位置

        //double ImageWidth { get; set; }
        //double ImageHeight { get; set; }

        void Active();//激活状态
        void DeActive();

        Brush ActiveBrush { get; set; }
        Brush DeActiveBrush { get; set; }
    }
}
