using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using SilverlightLynxControls.LynxControls;
using SilverlightLFC.common;

namespace SilverlightLynxControls.LogicView
{
    public class ViewItem : AbstractLogicView//所有的可以呈现的对象
    {
        #region Fields
        public double _xt_db_Width = 80;
        public double _xt_db_Height = 60;
        public double _xt_db_Left = 0;
        public double _xt_db_Top = 0;
        //public double _xt_db_BorderWidth = 2;
        #endregion

        #region ID
        public new string _xt_db_ID = SilverlightLFC.common.Environment.getGUID();
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
                    _xt_db_Width = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Width");
                }

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
                    _xt_db_Height = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Height");
                }

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
                    _xt_db_Top = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Top");
                }

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
                    _xt_db_Left = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Left");
                }

            }
        }
        //public double BorderWidth
        //{
        //    get
        //    {
        //        return _xt_db_BorderWidth;
        //    }
        //    set
        //    {
        //        if (_xt_db_BorderWidth != value)
        //        {
        //            sendObjctChanged(LFCObjChanged.ObjectChanged, "BorderWidth");
        //        }
        //        _xt_db_BorderWidth = value;
        //        DataFlag = SilverlightLFC.common.DataOperation.Update;
        //    }
        //}

        #endregion
        
        


        //public List<ViewLineItem> InLineList=new List<ViewLineItem>();//进入的对象
        //public List<ViewLineItem> OutLineList=new List<ViewLineItem>();//引出的对象
    }
}
