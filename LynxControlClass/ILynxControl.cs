using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverlightLynxControls
{
    public interface ILynxControl
    {
        bool IsMove
        {
            get;
            set;
        }

        int getPositionStatus(double x,double y);

        void setClear();
    }
}
