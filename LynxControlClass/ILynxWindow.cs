using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverlightLynxControls
{
    public interface ILynxWindow : ILynxControl
    {
        double CurrentHeight
        {
            get;
            set;
        }

        double CurrentWidth
        {
            get;
            set;
        }

        double BorderWidth
        {
            get;
            set;
        }

        double TitleHeight
        {
            get;
            set;
        }

        bool IsBottomChange
        {
            get;
            set;
        }

        bool IsTopChange
        {
            get;
            set;
        }

        bool IsLeftChange
        {
            get;
            set;
        }

        bool IsRightChange
        {
            get;
            set;
        }
    }
}
