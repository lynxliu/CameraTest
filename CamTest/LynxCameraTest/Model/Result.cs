using LFC.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace PhotoTestControl.Models
{
    public class Result:ViewModelBase
    {
        public string Name { get; set; }
        public string Memo { get; set; }
        public object Value { get; set; }
        public string Dimension { get; set; }
        public TimeSpan SpendTime { get; set; }
        public DateTime TestTime { get; set; }
        public int TestCount { get; set; }
        public bool CanSupportOperator
        {
            get
            {
                if ((Value is double) || (Value is int) || (Value is long)
                    || (Value is byte) || (Value is bool) || (Value is float)
                    || (Value is decimal))
                    return true;
                return false;
            }
        }
        public double DoubleValue
        {
            get
            {
                if(Value is bool ) if(Convert.ToBoolean(Value)) return 1;else return 0;
                return Convert.ToDouble(Value);
            }
            set { Value = value; }
        }
        //public Action<string> TestDetail { get; set; }

        //public DelegateCommand<string> MoreDetailCommand
        //{
        //    get
        //    {
        //        return new DelegateCommand<string>(
        //            (o) =>
        //            {
        //                if (TestDetail != null) TestDetail(o);
        //            });
        //    }
        //}
    }
}
