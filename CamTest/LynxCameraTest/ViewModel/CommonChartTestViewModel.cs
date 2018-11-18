using DCTestLibrary;
using LFC.common;
using LynxCameraTest.Model;
using LynxControls;
using PhotoTestControl.Models;
using SilverlightLFC.common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace LynxCameraTest.ViewModel
{
    public class CommonChartTestViewModel : INotifyPropertyChanged
    {
        public LynxPhotoViewControl GetCurrentPhotoViewControl(FlipView list)
        {
            foreach (var v in list.Items)
            {
                if (v is LynxPhotoViewControl)
                {
                    if ((v as LynxPhotoViewControl).Photo == CurrentPhoto)
                        return v as LynxPhotoViewControl;
                }
            }
            return null;
        }
        public string Title { get; set; }
        public string ChartName { get; set; }

        string _TestInfo = "No photos, no test";
        public string TestInfo
        {
            get { return _TestInfo; }
            set { _TestInfo = value; OnPropertyChanged("TestInfo"); }
        }

        bool _isSelectedArea = false;
        public bool IsSelectedArea
        {
            get { return _isSelectedArea; }
            set { _isSelectedArea = value; OnPropertyChanged("IsSelectedArea"); }
        }

        public WriteableBitmap SelectArea { get; set; }

        public WriteableBitmap Icon { get; set; }

        ObservableCollection<WriteableBitmap> photoList = new ObservableCollection<WriteableBitmap>();
        public ObservableCollection<WriteableBitmap> PhotoList { get { return photoList; } }

        WriteableBitmap currentPhoto;
        public WriteableBitmap CurrentPhoto
        {
            get { return currentPhoto; }
            set
            {
                currentPhoto = value;
                OnPropertyChanged("CurrentPhoto");
                if (ResultList.ContainsKey(value))
                {
                    CurrentResult = ResultList[value];
                    OnPropertyChanged("CurrentResult");
                }
            }
        }
        public ObservableCollection<TestItem> OpenedViewList
        {
            get { return MainPageViewModel.GetInstance().OpenedViewList; }
        }
        public DelegateCommand CloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MainPageViewModel.MainFrame.GoBack();
                });
            }
        }

        public DelegateCommand AddPhotoCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    var photolist = await SilverlightLFC.common.Environment.getEnvironment().OpenImage();
                    foreach (var photo in photolist)
                    {
                        PhotoList.Add(photo);
                    }
                    if (photolist.Count > 0)
                    {
                        CurrentPhoto = photolist.FirstOrDefault();
                    }
                });
            }
        }

        public DelegateCommand RemovePhotoCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (CurrentPhoto != null && PhotoList.Contains(CurrentPhoto))
                    {
                        PhotoList.Remove(CurrentPhoto);
                        CurrentPhoto = PhotoList.FirstOrDefault();
                    }
                });
            }
        }

        bool isShowPhotoListNaviator = false;
        public bool IsShowPhotoListNaviator
        {
            get { return isShowPhotoListNaviator; }
            set { isShowPhotoListNaviator = value; OnPropertyChanged("IsShowPhotoListNaviator"); }
        }

        bool isShowCurrentResult = false;
        public bool IsShowCurrentResult
        {
            get { return isShowCurrentResult; }
            set
            {
                isShowCurrentResult = value;
                if (value)
                {
                    CurrentResult = ResultList[CurrentPhoto];
                }
                else
                {
                    CurrentResult = Average;
                }

                OnPropertyChanged("IsShowCurrentResult");
            }
        }

        AbstractTestChart _testChart = null;
        public AbstractTestChart TestChart
        {
            get { return _testChart; }
            set { _testChart = value; }
        }

        public Func<WriteableBitmap, List<Result>> Test { get; set; } //one photo test multi results
        public DelegateCommand TestAllCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ResultList.Clear();
                    //Task.Factory.StartNew(() =>
                    //{
                        foreach (var p in PhotoList)
                        {
                                var rl = Test(p);
                                var or = new ObservableCollection<Result>(rl);
                                ResultList.Add(p, or);

                        }
                    //}).ContinueWith(o =>
                    //{
                        GetAverage();
                    //});
                });
            }
        }
        public DelegateCommand TestCurrentCommand
        {
            get
            {
                return new DelegateCommand(async () => 
                {
                    if (CurrentPhoto == null) return;
                    ResultList.Remove(currentPhoto);
                        var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;


                        //WriteableBitmap tp = null;
                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            PhotoTest pt=new PhotoTest();
                            CurrentPhoto = pt.getImageArea(currentPhoto, 50, 50, 200, 150);
                            PhotoList.Add(CurrentPhoto);
                            //var rl = Test(currentPhoto);
                            //CurrentResult.Clear();
                            //foreach (var r in rl) { CurrentResult.Add(r); }
                            //ResultList.Add(currentPhoto, CurrentResult);
                        });


                });
            }
        }

        ObservableCollection<Result> currentResult = new ObservableCollection<Result>();
        public ObservableCollection<Result> CurrentResult
        {
            get { return currentResult; }
            set
            {
                currentResult = value;
                OnPropertyChanged("CurrentResult");
            }
        }
        Dictionary<WriteableBitmap, ObservableCollection<Result>> resultList = new Dictionary<WriteableBitmap, ObservableCollection<Result>>();
        public Dictionary<WriteableBitmap, ObservableCollection<Result>> ResultList { get { return resultList; } }

        ObservableCollection<Result> average = new ObservableCollection<Result>();
        public ObservableCollection<Result> Average { get { return average; } }

        public void GetAverage()//auto calculate result average value
        {
            Average.Clear();
            foreach (var rl in ResultList)
            {
                foreach (var r in rl.Value)
                {
                    if (r.CanSupportOperator)
                    {
                        if (Average.Any(v => v.Name == r.Name))
                        {
                            Average.FirstOrDefault(v => v.Name == r.Name).DoubleValue += r.DoubleValue;
                        }
                        else
                        {
                            Average.Add(new Result()
                            {
                                Name = r.Name,
                                Memo = r.Memo,
                                Value = r.Value,
                                Dimension = r.Dimension,
                                TestCount = 1
                            });
                        }
                    }
                }
            }
            foreach (var r in Average)
            {
                r.Value = r.DoubleValue / r.TestCount;
            }
        }

        public double ProcessPercent { get; set; }

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
