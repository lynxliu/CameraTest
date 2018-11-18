using DCTestLibrary;
using LynxCameraTest.View;
using LynxCameraTest.ViewModel;
using PhotoTestControl.Models;
using SilverlightDCTestLibrary;
using SLPhotoTest.PhotoTest;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace LynxCameraTest.Model
{
    public class ChartTestItem:TestItem
    {
        public string Key { get; set; }
        public override Func<FrameworkElement> CreateView
        {
            get
            {
                //return ()=> LoadView();
                return null;
            }
            set { }
        }
        //public Func<WriteableBitmap, List<Result>> Test { get; set; } //one photo test multi results
        //FrameworkElement LoadView()
        //{
        //    var view =Activator.CreateInstance(ViewType) as FrameworkElement;
        //    view.DataContext = Activator.CreateInstance(ViewModelType) ;
        //    return view;
        //}
        //public List<Result> Test(WriteableBitmap source)
        //{
        //    if (Key == "ISO12233")
        //    {
        //        return TestISO12233(source);
        //    }

        //    return null;
        //}





    }
}
