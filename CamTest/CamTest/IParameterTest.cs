using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace SLPhotoTest
{
    public interface IParameterTest//所有支持交互式细节测试的窗体需要实现的接口
    {//由于每个指标测试都需要操作多个图像，之间差异很大，因此需要依赖各个指标测试窗体自行实现
        //void ZoomIn();
        //void ZoomOut();
        //void Resume();//不仅仅是恢复大小，也重置测试值
        //void Move(bool IsEnable);

        void ParameterAutoTest();//自动进行的测试，也就是默认的测试，这个测试是窗体自己定义的

        //WriteableBitmap CurrentPhoto { get; set; }
        //Panel CurrentPhotoOperationFramework { get; set; }
        ILynxPhotoOperation CurrentLPO { get; }//当前的操作结构

        void AddPhoto(WriteableBitmap photo);
        void RemovePhoto();
    }
}
