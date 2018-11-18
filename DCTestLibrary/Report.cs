using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DCTestLibrary
{
    public class Report
    {
        public string TargetInfor;
        public string ParameterName;
        public string TestProjectInfor;
        public Dictionary<string, object> ISO12233TestValue;
        public Dictionary<string, object> XRiteTestValue;
        public Dictionary<string, object> XMarkTestValue;
        public Dictionary<string, object> CommonTestValue;
    }
}
