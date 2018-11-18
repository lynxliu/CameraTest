using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;
using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;

namespace SLPhotoTest.PhotoEdit
{
    public partial class LayerEdit : UserControl
    {
        public LayerEdit()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
        }
        ActionMove acm;

        public void setTarget(PhotoEditCanvas pc){
            p = pc;

        }
        PhotoEditCanvas p;
        public void ProcessBlue(int n)
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            PhotoLayer pl = p.SelectLayer;
            WriteableBitmap b = pl.ProcPhoto;
            pl.ProcPhoto = pt.BlueBitmap(b, n);
        }

        public void ProcessGreen(int n)
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            PhotoLayer pl = p.SelectLayer;
            WriteableBitmap b = pl.ProcPhoto;
            pl.ProcPhoto = pt.GreenBitmap(b, n);
        }

        public void ProcessRed(int n)
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            PhotoLayer pl = p.SelectLayer;
            WriteableBitmap b = pl.ProcPhoto;
            pl.ProcPhoto = pt.RedBitmap(b, n);
        }

        public void ProcessBright(int n)
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            PhotoLayer pl = p.SelectLayer;
            WriteableBitmap b = pl.ProcPhoto;
            pl.ProcPhoto = pt.BrightnessBitmap(b, n);
        }

        public void ProcessGray()
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            PhotoLayer pl = p.SelectLayer;
            WriteableBitmap b = pl.ProcPhoto;
            pl.ProcPhoto = pt.Gray(b);
        }

        public void ProcessInvert()
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            PhotoLayer pl = p.SelectLayer;
            WriteableBitmap b = pl.ProcPhoto;
            pl.ProcPhoto = pt.Invert(b);
        }

        public void ProcessBlur(int n)
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            PhotoLayer pl = p.SelectLayer;
            WriteableBitmap b = pl.ProcPhoto;
            pl.ProcPhoto = pt.Blur(b, n);
        }

        public void ProcessMosaic(int n)
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            PhotoLayer pl = p.SelectLayer;
            WriteableBitmap b = pl.ProcPhoto;
            pl.ProcPhoto = pt.Mosaic(b, n);
        }

        public void ProcessContract(int n)
        {
            DCTestLibrary.PhotoTest pt = new DCTestLibrary.PhotoTest();
            PhotoLayer pl = p.SelectLayer;
            WriteableBitmap b = pl.ProcPhoto;
            pl.ProcPhoto = pt.SetContrast(b, n);
        }

        private void buttonRedLess_Click(object sender, RoutedEventArgs e)
        {
            ProcessRed(textBoxRed.IntValue * -1);
        }

        private void buttonRedMore_Click(object sender, RoutedEventArgs e)
        {
            ProcessRed(textBoxRed.IntValue);
        }

        private void buttonGreenLess_Click(object sender, RoutedEventArgs e)
        {
            ProcessGreen(textBoxGreen.IntValue * -1);
        }

        private void buttonGreenMore_Click(object sender, RoutedEventArgs e)
        {
            ProcessGreen(textBoxGreen.IntValue * 1);
        }

        private void buttonContract_Click(object sender, RoutedEventArgs e)
        {
            ProcessContract(textBoxContract.IntValue);
        }

        private void buttonGray_Click(object sender, RoutedEventArgs e)
        {
            ProcessGray();

        }

        private void buttonInvert_Click(object sender, RoutedEventArgs e)
        {
            ProcessInvert();
        }

        private void buttonBlueMore_Click(object sender, RoutedEventArgs e)
        {
            ProcessBlue(textBoxBlue.IntValue);
        }

        private void buttonBlueLess_Click(object sender, RoutedEventArgs e)
        {
            ProcessBlue(textBoxBlue.IntValue * -1);
        }

        private void buttonBrightMore_Click(object sender, RoutedEventArgs e)
        {
            ProcessBright(textBoxBright.IntValue);
        }

        private void button1BrightLess_Click(object sender, RoutedEventArgs e)
        {
            ProcessBright(textBoxBright.IntValue*-1);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            (this.Parent as Panel).Children.Remove(this);
        }
    }
}
