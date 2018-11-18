using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SLPhotoTest.UIControl;
using System.Collections.Generic;
using SLPhotoTest.PhotoInfor;

using DCTestLibrary;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;

namespace SLPhotoTest.ChartTest
{
    public interface IChartTestWindow
    {
        PhotoEditToolbar PEToolbar{get;}
        PhotoInforToolbar PIToolbar { get; }
        LPhotoList PhotoList { get; }
        LChartPhoto ChartPhoto { get; }
        ProgressBar TestProcessbar { get; }
        ChartCorrect ChartCorrectToolbar { get; }

        AbstractTestChart getChartTest(WriteableBitmap b);
        void ShowTestResult(WriteableBitmap chartPhoto);//chartPhoto为null是全部照片的平均，否则是具体的照片
        List<ParameterTest> GetTestSteps();

    }
}
