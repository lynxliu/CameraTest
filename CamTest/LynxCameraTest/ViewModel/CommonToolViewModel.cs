using LFC.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace LynxCameraTest.ViewModel
{
    public class CommonToolViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public Visibility IsUseCommonTestButton { get; set; }

        public Action TestAction { get; set; }
        public DelegateCommand TestParameterCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (TestAction != null)
                        TestAction();
                });
            }
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
    }
}
