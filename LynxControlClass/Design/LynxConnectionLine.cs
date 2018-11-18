using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using SilverlightLFC.common;

namespace SilverlightLynxControls.Design
{
    public class LynxConnectionLine : SilverlightLFC.common.AbstractLFCDataObject
    {
        public LynxConnectionLine()
        {

        }
        public LynxConnectionLine(LynxConnectControl sc, LynxConnectControl tc)
        {
            SourceControl = sc;
            TargetControl = tc;
        }

        #region ID
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
            }
        }
        override public void ClearObjectID()
        {
            this._xt_db_ID = "";
        }
        #endregion

        #region Fields

        public string _xt_db_Name = "UnNamed";
        public string _xt_db_Memo = "UnNamed Relation";

        public LynxConnectControl SourceControl=null;
        public LynxConnectControl TargetControl = null;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                return _xt_db_Name;
            }
            set
            {
                if (_xt_db_Name != value)
                {
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Name");
                }
                _xt_db_Name = value;
                DataFlag = SilverlightLFC.common.DataOperation.Update;
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
                if (_xt_db_Memo != value)
                {
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Memo");
                }
                _xt_db_Memo = value;
                DataFlag = SilverlightLFC.common.DataOperation.Update;
            }
        }
        #endregion
    }
}
