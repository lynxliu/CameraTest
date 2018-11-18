using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;      
using System.Windows.Input;

namespace SilverlightLynxControls.Design
{
    public partial class LynxDefaultNode : LynxConnectControl
    {
        public LynxDefaultNode()//默认的用于设计器的节点
        {
            InitializeComponent();
            MoveArea = textBlockTitle;
            ActiveArea = image1;
            EnableControlMove();
            EnableActive();
            AnchorPointList.Add(lynxAnchorPoint1);
            lynxAnchorPoint1.ParentConnectControl = this;

        }

    }
}
