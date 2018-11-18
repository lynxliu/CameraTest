using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;




using SilverlightPhotoIO;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace SLPhotoTest.PhotoTest
{
    public delegate void AutoTestParameter();//自动测试，是针对所有测试照片的综合处理
    public delegate void AddTestPhoto(WriteableBitmap b);
    public delegate void RemoveTestPhoto(WriteableBitmap b);

    public delegate void PhotoTestOperation(string OperationName,WriteableBitmap b);//新完成了一个操作

    public partial class PhotoTestToolbar : UserControl
    {
        public PhotoTestToolbar()
        {
            InitializeComponent();
            //CameraTestDesktop.CheckEnable();
        }
        public event PhotoTestOperation PhotoOperate;
        public AutoTestParameter autoTest;//自动的针对指标进行测试
        public AddTestPhoto addPhoto;//给测试组增加照片
        public RemoveTestPhoto removePhoto;//给测试组删除照片
        public void setTarget(LChartPhoto ChartStructure)
        {
            lcp = ChartStructure;
        }
        LChartPhoto lcp;

        private void buttonCopy_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            ClearActiveAll();
            
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            d.addClip(lcp.getPhoto());
            if (PhotoOperate != null)
            {
                PhotoOperate("Copy", lcp.getPhoto());
            }
        }

        private void buttonPaste_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            ClearActiveAll();
            CameraTestDesktop d = CameraTestDesktop.getDesktop();
            WriteableBitmap wb=d.getClip();
            if(wb==null){return;}
            lcp.setPhoto(wb);
            if (addPhoto != null)
            {
                addPhoto(wb);
            }
            if (PhotoOperate != null)
            {
                PhotoOperate("Paste", wb);
            }
        }

        private void buttonImport_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            ClearActiveAll();
            var b= PhotoIO.ReadImageFromFile();
            if (b == null) { return; }

            lcp.setPhoto(b.Result);
            if (addPhoto != null)
            {
                addPhoto(b.Result);
            }
            if (PhotoOperate != null)
            {
                PhotoOperate("Import", b.Result);
            }
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            ClearActiveAll();
            
            if (removePhoto != null)
            {
                removePhoto(lcp.getPhoto());
            }
            lcp.setPhoto(null);
            if (PhotoOperate != null)
            {
                PhotoOperate("Remove", lcp.getPhoto());
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            ClearActiveAll();
            WriteableBitmap b =lcp.getPhoto();
            if (b != null)
            {
                PhotoIO pi = new PhotoIO();
                PhotoIO.WriteImageToFile(b);
            }
            if (PhotoOperate != null)
            {
                PhotoOperate("Save", b);
            }
        }

        private void buttonZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            ClearActiveAll();
            lcp.Zoom(1.2);
            if (PhotoOperate != null)
            {
                PhotoOperate("ZoomIn", lcp.getPhoto());
            }
        }

        private void buttonZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            ClearActiveAll();
            lcp.Zoom(0.8);
            if (PhotoOperate != null)
            {
                PhotoOperate("ZoomOut", lcp.getPhoto());
            }
        }

        bool _IsMove = false;
        Brush activeBack = new SolidColorBrush(Colors.Red);
        Brush ActiveButton = new SolidColorBrush(Colors.Red);
        void ClearActiveAll()
        {
            _IsMove = false;
            setUnMove();
        }
        public void setUnMove()
        {
            MoveButtonBack.Background = null;
            lcp.DisableMove();
        }
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
            ClearActiveAll();
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }
            lcp.RestTransform();
            lcp.ResetSize();
            if (PhotoOperate != null)
            {
                PhotoOperate("Resume", lcp.getPhoto());
            }
        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            ClearActiveAll();
            if (lcp == null) { SilverlightLFC.common.Environment.ShowMessage("未将按钮和测试对象关联！"); return; }

            if (autoTest != null) { autoTest(); }
            if (PhotoOperate != null)
            {
                PhotoOperate("Test", lcp.getPhoto());
            }
        }
    }
}
