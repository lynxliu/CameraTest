using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLPhotoTest
{
    public class Company : SilverlightLFC.common.AbstractDataObject//记录就业的公司
    {
        public string _xa_db_Name;
        public string _xt_db_Memo;
        public string _xt_db_Country;
        public string _xt_db_City;
        public string _xt_db_DocumentID;
        public string _xt_db_Logo;
        public string _xt_db_HomePage;
        public string _xt_db_Type;

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

        public string Name
        {
            get
            {
                return _xa_db_Name;
            }
            set
            {
                _xa_db_Name = value;
            }
        }

        public string Memo
        {
            get
            {
                return _xt_db_Memo;
            }
            set
            {
                _xt_db_Memo = value;
            }
        }

        public string Country
        {
            get
            {
                return _xt_db_Country;
            }
            set
            {
                _xt_db_Country = value;
            }
        }

        public string City
        {
            get
            {
                return _xt_db_City;
            }
            set
            {
                _xt_db_City = value;
            }
        }


        public string LogoLink
        {
            get
            {
                return _xt_db_Logo;
            }
            set
            {
                _xt_db_Logo = value;
            }
        }

        public string HomePage
        {
            get
            {
                return _xt_db_HomePage;
            }
            set
            {
                _xt_db_HomePage = value;
            }
        }

        public string Type
        {
            get
            {
                return _xt_db_Type;
            }
            set
            {
                _xt_db_Type = value;
            }
        }


    }
}
