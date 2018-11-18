using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using SilverlightLFC.common;

namespace SilverlightLynxControls.LogicView
{
    public class LogicViewAnchorPoint : AbstractLFCDataObject//逻辑锚点，用来链接对象,核心是保存Node在哪里链接对象
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

        #region Fields
        public string _xt_db_Key;
        public double _xt_db_Left;
        public double _xt_db_Top;
        public double _xt_db_Width;
        public double _xt_db_Height;

        public List<string> _sl_db_InLineItemIDList = new List<string>();
        public List<string> _sl_db_OutLineItemIDList = new List<string>();
        #endregion

        #region Properties
        public double Width
        {
            get
            {
                return _xt_db_Width;
            }
            set
            {
                if (_xt_db_Width != value)
                {
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Width");
                }
                _xt_db_Width = value;
                DataFlag = SilverlightLFC.common.DataOperation.Update;
            }
        }
        public double Height
        {
            get
            {
                return _xt_db_Height;
            }
            set
            {
                if (_xt_db_Height != value)
                {
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Height");
                }
                _xt_db_Height = value;
                DataFlag = SilverlightLFC.common.DataOperation.Update;
            }
        }
        public double Top
        {
            get
            {
                return _xt_db_Top;
            }
            set
            {
                if (_xt_db_Top != value)
                {
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Top");
                }
                _xt_db_Top = value;
                DataFlag = SilverlightLFC.common.DataOperation.Update;
            }
        }
        public double Left
        {
            get
            {
                return _xt_db_Left;
            }
            set
            {
                if (_xt_db_Left != value)
                {
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Left");
                }
                _xt_db_Left = value;
                DataFlag = SilverlightLFC.common.DataOperation.Update;
            }
        }
        public string Key
        {
            get
            {
                return _xt_db_Key;
            }
            set
            {
                if (_xt_db_Key != value)
                {
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Key");
                }
                _xt_db_Key = value;
                DataFlag = SilverlightLFC.common.DataOperation.Update;
            }
        }

        public List<string> InRelationIDList { get { return _sl_db_InLineItemIDList; } }
        public List<string> OutRelationIDList { get { return _sl_db_OutLineItemIDList; } }
        #endregion




    }

}
