using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SLPhotoTest.PhotoEdit
{
    public partial class LayerInfor : UserControl
    {
        public LayerInfor()
        {
            InitializeComponent();
            am = new ActionMove(this, Title);
            am.Enable = true;
            ar = new ActionResize(LayoutRoot);
        }
        ActionMove am;
        ActionResize ar;
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }

        PhotoEditCanvas pc;
        public void setTarget(PhotoEditCanvas p)
        {
            pc = p;
            photoBrightInfor.setTarget(pc);
            photoColorInfor.setTarget(pc);
            photoInfor.setTarget(pc);
        }

        private void buttonMin_Click(object sender, RoutedEventArgs e)
        {
            this.Height = 55;
        }

        private void buttonResume_Click(object sender, RoutedEventArgs e)
        {
            this.Height = 300;
        }
    }
}
