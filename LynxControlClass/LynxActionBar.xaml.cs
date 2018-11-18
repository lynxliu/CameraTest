using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;
using Windows.UI.Xaml.Controls;




namespace SilverlightLynxControls
{
    public partial class LynxActionBar : UserControl
    {
        public LynxActionBar()
        {
            InitializeComponent();
            am = new ActionMove(this, LayoutRoot);
        }
        ActionMove am;
        //public List<LynxIcon> tasklist = new List<LynxIcon>();

        public void AddIcon(LynxButton li)
        {
            //tasklist.Add(li);
            Action.Children.Add(li);
        }

        public void Remove(LynxButton li)
        {
            Action.Children.Remove(li);
        }
    }
}
