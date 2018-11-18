using DCTestLibrary;
using LFC.common;
using PhotoTestControl.Models;
using PhotoTestControl.Views;
using SilverlightDCTestLibrary;
using SilverlightLynxControls;
using SLPhotoTest.PhotoTest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PhotoTestControl.ViewModels
{
    public class DesktopViewModel : ViewModelBase
    {
        public Panel Desktop { get; set; }
        ObservableCollection<FrameworkElement> _viewList = new ObservableCollection<FrameworkElement>();
        public ObservableCollection<FrameworkElement> ViewList
        {
            get { return _viewList; }
        }

        FrameworkElement _currentView = null;
        public FrameworkElement CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        public DelegateCommand<string> TestChart
        {
            get
            {
                return new DelegateCommand<string>((s) =>
                {
                    if (s == "ISO12233")
                    {
                        LynxPhotoTestViewModel vm = new LynxPhotoTestViewModel();
                        vm.Test = new Func<WriteableBitmap, List<Result>>(TestISO12233);
                        LynxPhotoTestView v = new LynxPhotoTestView() { DataContext = vm };
                        ActionShow.CenterShow(Desktop, v);
                    }
                });
            }
        }
        List<Result> TestISO12233(WriteableBitmap chart)
        {
            var rl = new List<Result>();
            var Chart = new ISO12233Chart();

            rl.Add(new Result()
            {
                Value = Chart.getLPResoveLines(),
                Name = "Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH",
                TestDetail = (b) =>
                {
                    if (Chart.ProcessInfor.ContainsKey("RayleiResolutionIsLeft"))
                    {
                        if (Convert.ToBoolean(Chart.ProcessInfor["RayleiResolutionIsLeft"]))
                        {
                            ShowRaleiView(true, Chart.mp.SelectedArea["RayleiResolutionLArea"]);
                        }
                        else
                        {
                            ShowRaleiView(false, Chart.mp.SelectedArea["RayleiResolutionRArea"]);
                        }
                    }
                }
            });

            rl.Add(new Result()
            {
                Value = Chart.getHEdgeResoveLines(),
                Name = "Horizontal Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = Chart.getVEdgeResoveLines(),
                Name = "Vertical Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = Chart.getHDispersiveness(),
                Name = "Horizontal Dispersiveness",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "Pix"
            });
            rl.Add(new Result()
            {
                Value = Chart.getVDispersiveness(),
                Name = "Vertical Dispersiveness",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "Pix"
            });

            return rl;
        }

        List<Result> TestISO12233Ex(WriteableBitmap chart)
        {
            var rl = new List<Result>();
            var Chart = new ISO12233ExChart();

            rl.Add(new Result()
            {
                Value = Chart.getLPResoveLines(),
                Name = "Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = Chart.getHEdgeResoveLines(),
                Name = "Horizontal Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = Chart.getVEdgeResoveLines(),
                Name = "Vertical Resolution",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "LW/PH"
            });

            rl.Add(new Result()
            {
                Value = Chart.getHDispersiveness(),
                Name = "Horizontal Dispersiveness",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "Pix"
            });
            rl.Add(new Result()
            {
                Value = Chart.getVDispersiveness(),
                Name = "Vertical Dispersiveness",
                Memo = "",
                TestTime = DateTime.Now,
                TestCount = 1,
                Dimension = "Pix"
            });

            return rl;
        }
        public DelegateCommand<string> ShowChart
        {
            get
            {
                return new DelegateCommand<string>((s) =>
                {

                });
            }
        }

        public DelegateCommand Clock
        {
            get
            {
                return new DelegateCommand(() =>
                {

                });
            }
        }

        public DelegateCommand Speed
        {
            get
            {
                return new DelegateCommand(() =>
                {

                });
            }
        }

        public DelegateCommand ShutterDelay
        {
            get
            {
                return new DelegateCommand(() =>
                {

                });
            }
        }

        public DelegateCommand BadPixel
        {
            get
            {
                return new DelegateCommand(() =>
                {

                });
            }
        }

        void ShowRaleiView(bool IsLeft,WriteableBitmap b)
        {
            if (b == null) return;
            RayleiResolution v = new RayleiResolution();
            ActionShow.CenterShow(Desktop, v);
            v.IsLeft = IsLeft;
            v.Test(b);
        }
    }
}
