using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;
using System.Linq;
using SilverlightLFC.common;

namespace SilverlightLynxControls.LogicView
{
    public class Diagram : AbstractLFCDataObject//逻辑画布，可以包含在各种应用中，只包含和逻辑视图有关的逻辑可视对象
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
        public string _xt_db_Memo;
        public double _xt_db_Width = 800;
        public double _xt_db_Height = 600;
        public double _xt_db_Zoom = 1;

        public SilverlightLFC.common.LFCObjectList<ViewItem> _xi_db_ItemList = new SilverlightLFC.common.LFCObjectList<ViewItem>();
        public SilverlightLFC.common.LFCObjectList<ViewLineItem> _xi_db_ItemLineList = new SilverlightLFC.common.LFCObjectList<ViewLineItem>();

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
        public double Zoom
        {
            get
            {
                return _xt_db_Zoom;
            }
            set
            {
                if (_xt_db_Zoom != value)
                {
                    _xt_db_Zoom = value;
                    DataFlag = SilverlightLFC.common.DataOperation.Update;
                    sendObjctChanged(LFCObjChanged.ObjectChanged, "Zoom");
                }

            }
        }

        public SilverlightLFC.common.LFCObjectList<ViewItem> NodeItemList
        {
            get
            {
                return _xi_db_ItemList;
            }
        }
        public SilverlightLFC.common.LFCObjectList<ViewLineItem> RelationItemList
        {
            get
            {
                return _xi_db_ItemLineList;
            }
        }
        #endregion

        public List<ViewItem> getLogicItemByID(string id)
        {
            List<ViewItem> lv = new List<ViewItem>();
            foreach (ViewItem vi in _xi_db_ItemList)
            {
                if (vi.DataObjectID == id)
                {
                    lv.Add(vi);
                }
            }
            return lv;
        }

        public List<ViewLineItem> getLogicLineByID(string id)
        {
            List<ViewLineItem> lv = new List<ViewLineItem>();
            foreach (ViewLineItem vi in _xi_db_ItemLineList)
            {
                if (vi.DataObjectID == id)
                {
                    lv.Add(vi);
                }
            }
            return lv;
        }


        public void AddViewItem(ViewItem v)
        {
            if (!_xi_db_ItemList.Contains(v))
            {
                _xi_db_ItemList.Add(v);
            }
        }
        public void DeleteViewItem(ViewItem v)//同时删除关联
        {
            if (_xi_db_ItemList.Contains(v)) { _xi_db_ItemList.Remove(v); }

            List<ViewLineItem> vl = _xi_db_ItemLineList.Where(xv => xv.TargetNodeID == v.ObjectID || xv.SourceNodeID == v.ObjectID).ToList();

            foreach (ViewLineItem tvl in vl)
            {
                _xi_db_ItemLineList.Remove(tvl);
               
            }
            vl.Clear();
        }

        public void AddViewLineItem(ViewLineItem v)
        {
            if (!_xi_db_ItemLineList.Contains(v))
            {
                _xi_db_ItemLineList.Add(v);
            }
        }
        public void DeleteViewLineItem(ViewLineItem v)
        {
            if (_xi_db_ItemLineList.Contains(v))
            {
                _xi_db_ItemLineList.Remove(v);
            }
        }
    }
}
