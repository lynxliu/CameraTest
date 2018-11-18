using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using System.Threading.Tasks;

namespace SilverlightPhotoIO
{
    public class PhotoIO
    {
        public static BitmapImage getImageFromURL(string path)
        {//依据一个链接，无论是相对还是绝对加载图片
            Uri u = new Uri(path, UriKind.RelativeOrAbsolute);

            BitmapImage bim = new BitmapImage(u);
            return bim;
        }

        public static void LoadImageFromURL(Image TargetImage, string path)
        {//依据一个链接，无论是相对还是绝对加载图片
            Uri u = new Uri(path, UriKind.RelativeOrAbsolute);

            BitmapImage bim = new BitmapImage(u);
            TargetImage.Source = bim;
        }


        public static BitmapImage LoadImageFromResourStr(string s)
        {
            Uri u = new Uri(s, UriKind.RelativeOrAbsolute);
            return new BitmapImage(u);
            
        }

        public static async Task<List<WriteableBitmap>> ReadMultiImageFromFile()
        {
            var ex= SilverlightLFC.common.Environment.getEnvironment();
            return await ex.OpenImage();
        }


        public static async Task<WriteableBitmap> ReadImageFromFile()
        {
            var ex = SilverlightLFC.common.Environment.getEnvironment();
            var lf= await ex.OpenImage();
            if (lf != null && lf.Count > 0) return lf[0];
            return null;

        }

        public static void WriteImageToFile(WriteableBitmap wb)//把照片保存下来
        {
            var ex = SilverlightLFC.common.Environment.getEnvironment();
            ex.SaveImage(wb);

        }

    }
}
