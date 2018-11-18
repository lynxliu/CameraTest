using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Net.NetworkInformation;

namespace SilverlightLFC.common
{
    public class LynxNetService
    {
        public string state;
        public void Init()
        {
            NetworkChange.NetworkAddressChanged += OnNetworkAddressChanged;
            UpdateNetWorkState();
        }
        void UpdateNetWorkState()
        {
            state = NetworkInterface.GetIsNetworkAvailable() ? "Online" : "Offline";
        }
        void OnNetworkAddressChanged(object sender, EventArgs e)
        {
            UpdateNetWorkState();
        }
    }
}
