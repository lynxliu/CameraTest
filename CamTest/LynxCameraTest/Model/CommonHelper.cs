using LynxCameraTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace LynxCameraTest.Model
{
    public class CommonHelper
    {
        public static void ShowParameterTestView(IParameterTestView parameterView, WriteableBitmap photo)
        {
            if (photo == null) return;
            ShowParameterTestView(parameterView, new List<WriteableBitmap>() { photo });
        }
        public static void ShowParameterTestView(IParameterTestView parameterView, List<WriteableBitmap> photoList)
        {
            if (parameterView is FrameworkElement)
            {
                Show(parameterView as FrameworkElement);
                parameterView.Test(photoList);
            }
        }

        public static void Show(FrameworkElement control)
        {
            MainPageViewModel.MainFrame.Navigated += MainFrame_Navigated;
            MainPageViewModel.MainFrame.Navigate(typeof(CommonPage), control);
        }
        public static void Show(Type pageType,FrameworkElement control)
        {
            MainPageViewModel.MainFrame.Navigated += MainFrame_Navigated;
            MainPageViewModel.MainFrame.Navigate(pageType, control);
        }

        static void MainFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            MainPageViewModel.MainFrame.Navigated -= MainFrame_Navigated;
            var c=e.Parameter as FrameworkElement;
            if (c != null)
            {
                var target = e.Content as Panel;
                if (target != null)
                    target.Children.Add(c);
            }
        }
    }
}
