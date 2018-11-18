using DCTestLibrary;
using LFC.common;
using PhotoTestControl.Models;
using PhotoTestControl.Views;
using SilverlightLFC.common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PhotoTestControl.ViewModels
{
    public class LynxPhotoTestViewModel :INotifyPropertyChanged
    {
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
                CurrentResult = ResultList[value];
                OnPropertyChanged("CurrentPhoto"); 
            }
        }

        public DelegateCommand AddPhotoCommand
        {
            get
            {
                return new DelegateCommand(async() => 
                {
                    var photolist =await SilverlightLFC.common.Environment.getEnvironment().OpenImage();
                    foreach (var photo in photolist)
                    {
                        PhotoList.Add(photo);
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
                    if (CurrentPhoto != null&&PhotoList.Contains(CurrentPhoto))
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

        public Func<WriteableBitmap,List<Result>> Test { get; set; } //one photo test multi results
        public DelegateCommand TestAllCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ResultList.Clear();
                    Task.Factory.StartNew(() =>
                    {
                        foreach (var p in PhotoList)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                var rl = Test(p);
                                var or = new ObservableCollection<Result>(rl);
                                ResultList.Add(p, or);
                            });
                        }
                    }).ContinueWith(o =>
                    {
                        GetAverage();
                    });
                });
            }
        }
        public DelegateCommand TestCurrent
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (CurrentPhoto == null) return;
                    ResultList.Remove(currentPhoto);
                    Task.Factory.StartNew(() =>
                    {
                        var rl = Test(currentPhoto);
                        CurrentResult.Clear();
                        foreach (var r in rl) { CurrentResult.Add(r); }
                        ResultList.Add(currentPhoto, CurrentResult);
                        
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

        ObservableCollection<Result> average=new ObservableCollection<Result>();
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
                                Memo=r.Memo,
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
