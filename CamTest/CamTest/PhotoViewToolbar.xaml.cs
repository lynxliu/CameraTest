using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using SilverlightPhotoIO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SLPhotoTest
{
    public partial class PhotoViewToolbar : UserControl
    {
        public PhotoViewToolbar()
        {
            InitializeComponent();
        }

        public void setTarget(LChartPhoto ChartStructure)
        {
            lcp = ChartStructure;
        }
        LChartPhoto lcp;

        private void buttonCopy_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            d.addClip(lcp.getPhoto());
        }

        private void buttonPaste_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            WriteableBitmap wb=d.getClip();
            if(wb==null){return;}
            lcp.setPhoto(wb);
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            var b= PhotoIO.ReadImageFromFile();
            if (b == null) { return; }

            lcp.setPhoto(b.Result);
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }

            lcp.setPhoto(null);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            WriteableBitmap b =lcp.getPhoto();
            if (b != null)
            {
                PhotoIO pi = new PhotoIO();
                PhotoIO.WriteImageToFile(b);
            }
        }

        private void buttonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            lcp.Zoom(1.2);
        }

        private void buttonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            lcp.Zoom(0.8);
        }

        bool _IsMove = false;
        Brush activeBack = new SolidColorBrush(Colors.Red);
        private void buttonMove_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            _IsMove = !_IsMove;
            //ipt.CurrentLPO.Move(_IsMove);
            if (_IsMove)
            {
                MoveButtonBack.Background = activeBack;
                lcp.EnableMove();
            }
            else
            {
                MoveButtonBack.Background = null;
                lcp.DisableMove();
            }
        }

        private void buttonResume_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            lcp.RestTransform();
            lcp.ResetSize();
        }

    }

}
