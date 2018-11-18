using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LynxCameraTest.Model
{
    class TestGroup
    {

        public string Name { get; set; }
        public string Memo { get; set; }

        List<TestItem> _ItemList = new List<TestItem>();
        public List<TestItem> ItemList
        {
            get { return _ItemList; }
        }

        public int Count { get { return ItemList.Count; } }
        public Type TargetPageType { get; set; }
    }
}
