using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using System.IO;

using SilverlightLynxControls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SLPhotoTest.PhotoEdit
{
    public partial class PhotoIO : UserControl
    {
        public PhotoIO()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
        }
        ActionMove acm;

        PhotoEditCanvas pc;

        public void setTarget(PhotoEditCanvas p)//
        {
            pc = p;
        }


        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            pc.ReadImageFromFile();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            pc.WriteImageToFile();
        }

        private void buttonCopy_Click(object sender, RoutedEventArgs e)
        {
            pc.Copy();
        }

        private void buttonPaste_Click(object sender, RoutedEventArgs e)
        {
            pc.Paste();
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            PhotoAttributeEdit pa = new PhotoAttributeEdit();
            PhotoEditManager pm = PhotoEditManager.getPhotoEditManager();
            pm.NewPhotoEdit();

        }
    }
}
