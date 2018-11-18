using LFC.common;
using LynxCameraTest.Model;
using LynxCameraTest.ViewModel.ChartTestViewModel;
using SLPhotoTest;
using SLPhotoTest.ChartTest;
using SLPhotoTest.PhotoTest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace LynxCameraTest.ViewModel
{
    class MainPageViewModel : ViewModelBase
    {
        ObservableCollection<Page> _RecentView = new ObservableCollection<Page>();

        public ObservableCollection<Page> RecentView { get { return _RecentView; } }
        public static Frame MainFrame { get; set; }
        
        static MainPageViewModel mainVM = new MainPageViewModel();
        public static MainPageViewModel GetInstance() { return mainVM; }

        ObservableCollection<TestItem> _OpenedViewList = new ObservableCollection<TestItem>();
        public ObservableCollection<TestItem> OpenedViewList
        {
            get { return _OpenedViewList; }
        }
        TestItem _CurrentView = null;
        public TestItem CurrentView
        {
            get { return _CurrentView; }
            set
            {
                _CurrentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        TestGroup _CahrtTestGroup = null;
        public TestGroup CahrtTestGroup
        {
            get
            {
                if (_CahrtTestGroup == null)
                {
                    _CahrtTestGroup = new TestGroup()
                    {
                        Name = "Chart View",
                        Memo = "Show the chart for camera can take photos"
                    };
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "ISO12233",
                        Name = "ISO12233 Chart",
                        Memo = "ISO12233 chart is a comprehensive test chart. It test resolution and dispersiveness",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//ISO12233.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(ISO12233Test),
                        ViewModelType=typeof(ISO12233TestViewModel)
                        //Test=new Func<WriteableBitmap,List<PhotoTestControl.Models.Result>>(
                    });
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "ISO12233Ex",
                        Name = "ISO12233Ex Chart",
                        Memo = "ISO12233Ex chart is a comprehensive test chart, base on from ISO12233 chart. It test resolution and dispersiveness",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//ISO12233Ex.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(ISO12233ExTest),
                        ViewModelType = typeof(ISO12233ExTestViewModel)
                    });
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "XRite",
                        Name = "XRite Color Chart",
                        Memo = "Xrite chart is a comprehensive test chart, use color block to test. It test performance about color",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//XRite.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(XRiteTest),
                        ViewModelType = typeof(XRiteChartTestViewModel)
                    });
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "XMark",
                        Name = "XMark Test Chart",
                        Memo = "XMark chart is a comprehensive test chart, can test many performance about camera",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//XMark.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(XMarkTest),
                        ViewModelType = typeof(XMarkChartTestViewModel)
                    });
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Aberration Chart",
                        Name = "Aberration Chart",
                        Memo = "Aberration chart test aberration about camera",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//AberrationChart.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(AberrationChartTest),
                        ViewModelType = typeof(AberrationChartTestViewModel)
                    });
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Gray Chart",
                        Name = "Gray Chart",
                        Memo = "Gray chart test bright changes about camera",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//GrayChart.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(GrayChartTest),
                        ViewModelType = typeof(GrayChartTestViewModel)
                    });
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "KD Gray Chart",
                        Name = "Kodak Gray Chart",
                        Memo = "Kodak Gray chart test bright changes about camera",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//KDGray.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(KDGrayChartTest),
                        ViewModelType = typeof(KDGrayChartTestViewModel)
                    });
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "ITE Gray Chart",
                        Name = "ITE Grayscale Chart",
                        Memo = "ITE Gray chart test bright changes about camera",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//ITEGrayScaleChart.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(ITEGrayscaleChartTest),
                        ViewModelType = typeof(ITEGrayscaleChartTestViewModel)
                    });
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "GB Aberration",
                        Name = "GB Aberration Chart",
                        Memo = "GB aberration chart is a national standard test bright changes about camera",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//AberrationChart.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(JBAberrationTest),
                        ViewModelType = typeof(JBAberrationTestViewModel)
                    });
                    _CahrtTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "GB EV Test",
                        Name = "GB EV Chart",
                        Memo = "GB ev chart is a national standard test ev distance about camera",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Navigator//EVChart.png")),
                        PageType = typeof(ChartTestPage),
                        ViewType = typeof(JBEVTest),
                        ViewModelType = typeof(JBEVTestViewModel)
                    });
                }
                return _CahrtTestGroup;
            }
        }

        TestGroup _ParameterTestGroup = null;
        public TestGroup ParameterTestGroup
        {
            get
            {
                if (_ParameterTestGroup == null)
                {
                    _ParameterTestGroup = new TestGroup()
                    {
                        Name = "Parameter Test",
                        Memo = "Use photo to test camera parameter performance"
                    };
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Aberration Parameter",
                        Name = "Aberration Parameter Test",
                        Memo = "Test camera aberration by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//Aberration.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(Aberration),
                        ViewModelType = typeof(CommonToolViewModel)
                    });

                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Bright changes Parameter",
                        Name = "Bright changes Parameter Test",
                        Memo = "Test camera bright changes by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//BrightChanges.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(BrightDistance),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Color Trend Parameter",
                        Name = "Color trend Parameter Test",
                        Memo = "Test camera color trend by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//ColorTrend.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(ColorTrend),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Dispersiveness Parameter",
                        Name = "Dispersiveness Parameter Test",
                        Memo = "Test camera dispersiveness by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//Disersiveness.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(Dispersiveness),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Latitude Parameter",
                        Name = "Latitude Parameter Test",
                        Memo = "Test camera latitude by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//Latitude.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(Latitude),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Noise Parameter",
                        Name = "Noise Parameter Test",
                        Memo = "Test camera noise by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//Noise.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(Noise),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Purple percent Parameter",
                        Name = "Purplr pixels percent Parameter Test",
                        Memo = "Test camera purple pixel percent at border area by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//PPPercent.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(PurplePercent),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Raylei Parameter",
                        Name = "Paylei resolution Parameter Test",
                        Memo = "Test camera resolution use Raylei limit by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//TestResove.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(RayleiResolution),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "SFR Parameter",
                        Name = "SFR Parameter Test",
                        Memo = "Test camera resolution use SFR curve by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//TestResove.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(SFRResolution),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Wave Parameter",
                        Name = "Wave quality Parameter Test",
                        Memo = "Test camera wave bright bar photo quality",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//Wave.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(Wave),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _ParameterTestGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "White banlance Parameter",
                        Name = "White banlance Parameter Test",
                        Memo = "Test camera white banlance by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Parameter//WhiteBanlance.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(WhiteBanlance),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                }
                return _ParameterTestGroup;
            }
        }

        TestGroup _TestToolGroup = null;
        public TestGroup TestToolGroup
        {
            get
            {
                if (_TestToolGroup == null)
                {
                    _TestToolGroup = new TestGroup()
                    {
                        Name = "Test Tool",
                        Memo = "Support some test tool to test camera performance"
                    };
                    _TestToolGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Shutter delay Parameter",
                        Name = "Shutter delay Parameter Test",
                        Memo = "Test camera shutter delay",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Tools//CurrentTime.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(ShutterSpeed),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _TestToolGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Action Speed Parameter",
                        Name = "Action speed Parameter Test",
                        Memo = "Test camera action speed",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Tools//clock.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(SysTimer),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                    _TestToolGroup.ItemList.Add(new ChartTestItem()
                    {
                        Key = "Bad Pixel Parameter",
                        Name = "Bad Pixel Parameter Test",
                        Memo = "Test camera bad pixel by photo",
                        Icon = new BitmapImage(new Uri("ms-appx:///Assets//Tools//BadPix.png")),
                        PageType = typeof(CommonToolPage),
                        ViewType = typeof(BadPixTest),
                        ViewModelType = typeof(CommonToolViewModel)
                    });
                }
                return _TestToolGroup;
            }
        }


        private MainPageViewModel()
        {
            TestGroupList.Add(CahrtTestGroup);
            TestGroupList.Add(ParameterTestGroup);
            TestGroupList.Add(TestToolGroup);
        }

        ObservableCollection<TestGroup> _TestGroupList = new ObservableCollection<TestGroup>();
        public ObservableCollection<TestGroup> TestGroupList
        {
            get { return _TestGroupList; }
        }
    }
}
