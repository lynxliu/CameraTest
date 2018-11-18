using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace LynxCameraTest.Model
{
    public interface IParameterTestView:IPhotoTestWindow
    {
        void Test(List<WriteableBitmap> photoList);
    }
}
