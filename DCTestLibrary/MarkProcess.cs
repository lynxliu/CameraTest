using SilverlightLFC.common;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace DCTestLibrary
{
    public class MarkProcess
    {
        public System.Collections.Generic.Dictionary<string, WriteableBitmap> SelectedArea = new System.Collections.Generic.Dictionary<string, WriteableBitmap>();
        public string MarkName;//标版名称
        public string Memo;//标版说明
        public string ProcessWay;//处理方法说明
        public int ProcessMS;//处理的毫秒数
        WriteableBitmap _SourcePhoto;//原始图片
        WriteableBitmap _CorrectPhoto;//矫正以后的图片
        WriteableBitmap _CorrectMovePhoto;//对准
        WriteableBitmap _CorrectRotatePhoto;//对正
        WriteableBitmap _CorrectScalePhoto;//缩放
        WriteableBitmap _CroppedPhoto;//剪裁以后的图片，适用于XMark
        WriteableBitmap _SelectedPhoto;//叠加了选区的原始图片，标识SelectedArea在原始图片上面的位置
        WriteableBitmap _CorrectColorPhoto;//校准颜色的照片
        WriteableBitmap _CorrectBrightPhoto;//校准亮度的照片
        //WriteableBitmap AreaPhoto
        public DateTime LastUpdateTime = DateTime.Now;

        public void Clear()
        {
            SelectedArea.Clear();
            _SourcePhoto = null;
            _CorrectMovePhoto = null;
            _CorrectRotatePhoto = null;
            _CorrectScalePhoto=null;//缩放
             _SelectedPhoto=null;//选区
             _CorrectColorPhoto = null;//校准颜色的照片
             _CorrectBrightPhoto = null;//校准亮度的照片
             _CroppedPhoto = null;
             _CorrectPhoto = null;
        }

        public MarkProcess(WriteableBitmap wb)
        {
            _SourcePhoto = wb;
        }


        public string SourcePhotoName
        {
            get
            {
                return LastUpdateTime.ToString() + "SourcePhoto";
            }
        }
        public string CorrectPhotoName
        {
            get
            {
                return LastUpdateTime.ToString() + "CorrectPhoto";
            }
        }
        public string CorrectMovePhotoName
        {
            get
            {
                return LastUpdateTime.ToString() + "CorrectMovePhoto";
            }
        }

        public string CorrectRotatePhotoName
        {
            get
            {
                return LastUpdateTime.ToString() + "CorrectRotatePhoto";
            }
        }

        public string CorrectScalePhotoName
        {
            get
            {
                return LastUpdateTime.ToString() + "CorrectScalePhoto";
            }
        }

        public string SelectedPhotoName
        {
            get
            {
                return LastUpdateTime.ToString() + "SelectedPhoto";
            }
        }

        public string CorrectColorPhotoName
        {
            get
            {
                return LastUpdateTime.ToString() + "CorrectColorPhoto";
            }
        }

        public string CorrectBrightPhotoName
        {
            get
            {
                return LastUpdateTime.ToString() + "CorrectBrightPhoto";
            }
        }

        public string CroppedPhotoName
        {
            get
            {
                return LastUpdateTime.ToString() + "CroppedPhoto";
            }
        }

        public WriteableBitmap SourcePhoto
        {
            get
            {
                return _SourcePhoto;
            }
            set
            {
                _SourcePhoto = value;
                _SelectedPhoto = WriteableBitmapHelper.Clone(value);
                LastUpdateTime = DateTime.Now;
            }
        }
        public WriteableBitmap CorrectPhoto
        {
            get
            {
                return _CorrectPhoto;
            }
            set
            {
                _CorrectPhoto = value;
                LastUpdateTime = DateTime.Now;
            }
        }
        public WriteableBitmap CorrectMovePhoto
        {
            get
            {
                return _CorrectMovePhoto;
            }
            set
            {
                _CorrectMovePhoto = value;
                LastUpdateTime = DateTime.Now;
            }
        }
        public WriteableBitmap CroppedPhoto
        {
            get
            {
                return _CroppedPhoto;
            }
            set
            {
                _CroppedPhoto = value;
                LastUpdateTime = DateTime.Now;
            }
        }
        public WriteableBitmap CorrectRotatePhoto
        {
            get
            {
                return _CorrectRotatePhoto;
            }
            set
            {
                _CorrectRotatePhoto = value;
                LastUpdateTime = DateTime.Now;
            }
        }
        public WriteableBitmap CorrectScalePhoto
        {
            get
            {
                return _CorrectScalePhoto;
            }
            set
            {
                _CorrectScalePhoto = value;
                LastUpdateTime = DateTime.Now;
            }
        }

        public WriteableBitmap CorrectColorPhoto
        {
            get
            {
                return _CorrectColorPhoto;
            }
            set
            {
                _CorrectColorPhoto = value;
                LastUpdateTime = DateTime.Now;
            }
        }

        public WriteableBitmap CorrectBrightPhoto
        {
            get
            {
                return _CorrectBrightPhoto;
            }
            set
            {
                _CorrectBrightPhoto = value;
                LastUpdateTime = DateTime.Now;
            }
        }
        public WriteableBitmap SelectedPhoto
        {
            get
            {
                return _SelectedPhoto;
            }
            set
            {
                _SelectedPhoto = value;
                LastUpdateTime = DateTime.Now;
            }
        }

    }
}
