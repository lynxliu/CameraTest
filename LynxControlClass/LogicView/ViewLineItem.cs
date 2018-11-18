using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using SilverlightLFC.common;
using Windows.Foundation;
using Windows.UI;

namespace SilverlightLynxControls.LogicView
{
    public class ViewLineItem : AbstractLogicView//专门针对Relation的显示
    {
        #region Fields
        public string _xt_db_SourceID = "";
        public string _xt_db_TargetID = "";

        public double _xt_db_StartX = 0;//起始坐标
        public double _xt_db_StartY = 0;
        public double _xt_db_EndX = 0;//终点坐标
        public double _xt_db_EndY = 0;
        public double _xt_db_Width = 3;//线宽
        public string _xt_db_LineTypeName;//线型
        public string _xt_db_Color=SilverlightLFC.common.Environment.ColorToString(Colors.Black);//颜色

        //public string _xt_db_StartItemID;//起始端关联的对象ID
        //public string _xt_db_EndItemID;//中止端关联对象ID
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
        public Point SourcePoint
        {
            get
            {
                return new Point(_xt_db_StartX, _xt_db_StartY);
            }
            set
            {
                _xt_db_StartX = value.X;
                _xt_db_StartY = value.Y;
                DataFlag = SilverlightLFC.common.DataOperation.Update; 
                sendObjctChanged(LFCObjChanged.ObjectChanged, "SourcePoint");
            }
        }
        public Point TargetPoint
        {
            get
            {
                return new Point(_xt_db_EndX, _xt_db_EndY);
            }
            set
            {
                _xt_db_EndX = value.X;
                _xt_db_EndY = value.Y;
                DataFlag = SilverlightLFC.common.DataOperation.Update; ;
                sendObjctChanged(LFCObjChanged.ObjectChanged, "TargetPoint");
            }
        }
        public Color LineColor
        {
            get
            {
                return SilverlightLFC.common.Environment.getColorFromString(_xt_db_Color);
            }
            set
            {
                _xt_db_Color = SilverlightLFC.common.Environment.ColorToString(value);
                DataFlag = SilverlightLFC.common.DataOperation.Update;
                sendObjctChanged(LFCObjChanged.ObjectChanged, "LineColor");
            }
        }
        public string SourceNodeID
        {
            get
            {
                return _xt_db_SourceID;
            }
            set
            {
                if (_xt_db_SourceID != value)
                {
                    _xt_db_SourceID = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "SourceNodeID");
                }

            }
        }
        public string TargetNodeID
        {
            get
            {
                return _xt_db_TargetID;
            }
            set
            {
                if (_xt_db_TargetID != value)
                {
                    _xt_db_TargetID = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "TargetNodeID");
                }

            }
        }
        public string LineTypeName
        {
            get
            {
                return _xt_db_LineTypeName;
            }
            set
            {
                if (_xt_db_LineTypeName != value)
                {
                    _xt_db_LineTypeName = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "LineTypeName");
                }

            }
        }
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

        #endregion
    }
}
