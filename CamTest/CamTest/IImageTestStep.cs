using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SLPhotoTest
{
    public class IImageTestStep
    {
        public string ParameterName;//名称
        public string SetpName;
        public WriteableBitmap SourceImage;
        //public void setSource(WriteableBitmap b);
        public UserControl TestForm;
    }
}
