using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace PhotoTestControl
{
    public class ChartTestHelper
    {
        static SolidColorBrush GBFailBrush = new SolidColorBrush(Colors.Red);
        static SolidColorBrush GBSuccessBrush = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));

        public static void setGBSign(bool IsSuccess, Control signControl)
        {
            if (IsSuccess)
            {
                signControl.Background = GBSuccessBrush;
                ToolTipService.SetToolTip(signControl, "该项指标通过国标测试");
            }
            else
            {
                signControl.Background = GBFailBrush;
                ToolTipService.SetToolTip(signControl, "该项指标未通过国标测试");
            }
        }
        public static void setGBSign(bool IsSuccess, Panel signControl)
        {
            if (IsSuccess)
            {
                signControl.Background = GBSuccessBrush;
                ToolTipService.SetToolTip(signControl, "该项指标通过国标测试");
            }
            else
            {
                signControl.Background = GBFailBrush;
                ToolTipService.SetToolTip(signControl, "该项指标未通过国标测试");
            }
        }

    }
}
