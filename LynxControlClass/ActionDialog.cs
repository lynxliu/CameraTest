using System;
using System.Net;
using System.Windows;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


using System.Windows.Input;
using Windows.UI.Popups;




namespace SilverlightLynxControls
{
    public class ActionDialog
    {
        public static async void ShowInfor(string Msg, bool IsSuccess)
        {
            MessageDialog msg = new MessageDialog(Msg);

            if (IsSuccess)
            {
                msg.Title = "成功";
            }
            else
            {
                msg.Title = "失败";
            }
            msg.Commands.Add(
                new UICommand("OK", (o) =>
                {
                }));
            await msg.ShowAsync();
        }
    }
}
