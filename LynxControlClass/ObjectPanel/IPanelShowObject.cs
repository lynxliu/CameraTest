using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using SilverlightLFC.common;
using SilverlightLynxControls.TimeLine;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;

namespace SilverlightLynxControls
{
    public interface IPanelShowObject : IAbstractLFCDataObject, ITimeObject
    {
        BitmapImage Icon { get; set; }
        string Title { get; set; }
        FrameworkElement ToolTipInfor { get; set; }
        FrameworkElement PropertyPage { get; set; }
        bool IsActive { get; set; }
    }


}
