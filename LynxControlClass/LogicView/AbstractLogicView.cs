using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using SilverlightLFC.common;

namespace SilverlightLynxControls.LogicView
{
    public abstract class AbstractLogicView : SilverlightLFC.common.AbstractLFCDataObject//所有的逻辑视图对象都从此类继承
    {
        #region ID
        public string _xt_db_ID = SilverlightLFC.common.Environment.getGUID();
        override public String ObjectID
        {
            get
            {
                return this._xt_db_ID;
            }
            set
            {
                this._xt_db_ID = value;
            }
        }
        override public void ClearObjectID()
        {
            this._xt_db_ID = "";
        }      
        #endregion
        

        #region Methods//串行化数据
        public string WriteToXMLString()
        {
            //SilverlightLFC.common.LFCDataService ls = new SilverlightLFC.common.LFCDataService();
            string s = SilverlightLFC.common.Environment.Serialize(this);
            return s;
        }

        public void LoadFromXMLString(string s)
        {
            //SilverlightLFC.common.LFCDataService ls = new SilverlightLFC.common.LFCDataService();
            //ls.ReadFromXMLString(this,s);
            var o = SilverlightLFC.common.Environment.Deserialize(s, this.GetType());
            SilverlightLFC.common.Environment.CopyValue(o, this);
        }        
        #endregion

        #region Fields

        //public string _xt_db_Name;
        //public string _xt_db_Memo;
        public string _xt_db_ControlType;
        public string _xt_db_DataObjectType;
        public string _xt_db_DataObjectID;

        #endregion

        #region Properties
        //public string Name
        //{
        //    get
        //    {
        //        return _xt_db_Name;
        //    }
        //    set
        //    {
        //        if (_xt_db_Name != value)
        //        {
        //            sendObjctChanged(LFCObjChanged.ObjectChanged, "Name");
        //        }
        //        _xt_db_Name=value;
        //        DataFlag = SilverlightLFC.common.DataOperation.Update;
        //    }
        //}

        //public string Memo
        //{
        //    get
        //    {
        //        return _xt_db_Memo;
        //    }
        //    set
        //    {
        //        if (_xt_db_Memo != value)
        //        {
        //            sendObjctChanged(LFCObjChanged.ObjectChanged, "Memo");
        //        }
        //        _xt_db_Memo = value;
        //        DataFlag = SilverlightLFC.common.DataOperation.Update;
        //    }
        //}

        public string ControlType
        {
            get
            {
                return _xt_db_ControlType;
            }
            set
            {
                if (_xt_db_ControlType != value)
                {
                    _xt_db_ControlType = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "ControlType");
                }
            }
        }

        public string DataObjectType
        {
            get
            {
                return _xt_db_DataObjectType;
            }
            set
            {
                if (_xt_db_DataObjectType != value)
                {
                    _xt_db_DataObjectType = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "DataObjectType");
                }

            }
        }

        public string DataObjectID
        {
            get
            {
                return _xt_db_DataObjectID;
            }
            set
            {
                if (_xt_db_DataObjectID != value)
                {
                    _xt_db_DataObjectID = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "DataObjectID");
                }

            }
        }        
        #endregion

    }
}
