using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;

namespace SLPhotoTest
{
    public class Product : SilverlightLFC.common.AbstractDataObject
    {
        public DateTime PublcTime;
        public double Price;
        public Company com;

        public string _xt_db_ID;

        override public String ObjectID
        {
            get
            {
                return this._xt_db_ID;
            }
            set
            {
                this._xt_db_ID = value;
                Changed = true;
            }
        }
    }
}
