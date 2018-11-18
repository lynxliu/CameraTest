using LFC.common;
using LynxCameraTest.ViewModel;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LynxCameraTest.Model
{
    public class TestItem
    {
        public string Name { get; set; }
        public string Memo { get; set; }
        public ImageSource Icon { get; set; }

        public string Title
        {
            get { return Name; }
        }
        public string SubTitle { get { return Memo; } }
        public ImageSource Image { get { return Icon; } }

        public Frame MainFrame { get { return MainPageViewModel.MainFrame; } }

        public FrameworkElement View { get; set; }
        public Type PageType { get; set; }

        public Type ViewType { get; set; }
        public Type ViewModelType { get; set; }

        public void Init(Page p)
        {
            if (View == null)
            {
                if (CreateView != null)
                {
                    View = CreateView();
                }
                else
                {
                    if(ViewType!=null)
                        View = Activator.CreateInstance(ViewType) as FrameworkElement;
                    if(View!=null&&ViewModelType!=null)
                        View.DataContext = Activator.CreateInstance(ViewModelType);
                    if (View.DataContext != null && View.DataContext is ViewModelBase)
                    {
                        var vm = View.DataContext as ViewModelBase;
                        if (vm.Initialize != null)
                            vm.Initialize();
                    }

                    var chartView = View as IPhotoTestWindow;
                    if (chartView != null)
                        chartView.InitViewModel();
                }
                MainPageViewModel.GetInstance().OpenedViewList.Add(this);
                MainPageViewModel.GetInstance().CurrentView = this;
            }
            if(View!=null)
                p.Content = View;
            if(View!=null&&View.DataContext!=null)
                p.DataContext = View.DataContext;//share common data context

        }

        public virtual Func<FrameworkElement> CreateView { get; set; }
        //public Page TargetPage { get; set; }
        public void Active()
        {
            if (MainFrame == null)
            {
                throw new Exception("No avlid frame");
            }
            MainFrame.Navigated += MainFrame_Navigated;
            MainFrame.Navigate(PageType, this);
        }

        void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Page p = e.Content as Page;
            if (p == null) return;
            TestItem titem = e.Parameter as TestItem;
            if (titem == null) return;
            titem.Init(p);
            MainFrame.Navigated -= MainFrame_Navigated;
        }
        public DelegateCommand CloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MainPageViewModel.GetInstance().OpenedViewList.Remove(this);
                    if (MainPageViewModel.GetInstance().OpenedViewList.Count > 0)
                    {
                        var item = MainPageViewModel.GetInstance().OpenedViewList[MainPageViewModel.GetInstance().OpenedViewList.Count - 1];
                        MainPageViewModel.GetInstance().CurrentView = item;
                        item.Active();
                    }
                    else{
                        MainPageViewModel.GetInstance().CurrentView = null;
                        MainFrame.Navigate(typeof(MainPage));
                    }
                });
            }
        }
    }
}
