using LFC.common;
using LynxCameraTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LynxCameraTest.ViewModel
{
    public class PageNaigatorViewModel:ViewModelBase
    {
        ObservableCollection<TestItem> _TestItemList = new ObservableCollection<TestItem>();
        public ObservableCollection<TestItem> TestItemList
        { get { return _TestItemList; } }

        TestItem _CurrentItem = null;
        public TestItem CurrentItem
        {
            get { return _CurrentItem; }
            set
            {
                _CurrentItem = value;
                _CurrentItem.Active();
                OnPropertyChanged("CurrentItem");

            }
        }

    }
}
