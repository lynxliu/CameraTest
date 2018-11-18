using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using DCTestLibrary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Text;
using Windows.UI.Xaml.Media;

namespace SLPhotoTest.PhotoEdit
{
    public partial class FontSelect : UserControl
    {
        public FontSelect()
        {
            InitializeComponent();
            acm = new ActionMove(this, this);
        }
        ActionMove acm;

        TextFont fs = new TextFont();
        
        public TextFont getSelectFont()
        {
            string sn = comboBox1.SelectedItem.ToString();
            fs.TextFontFamily = new FontFamily(sn);

            fs.Size = lynxUpDown1.DoubleValue;

            
            return fs;
        }

        private void buttonI_Click(object sender, RoutedEventArgs e)
        {
            //fs.TextFontStretch = new FontStretch();
            //fs.TextFontStyle = Italic;
        }

        private void buttonLeft_Click(object sender, RoutedEventArgs e)
        {
            fs.Alignment=TextAlignment.Left;
        }

        private void buttonMiddle_Click(object sender, RoutedEventArgs e)
        {
            fs.Alignment = TextAlignment.Center;
        }

        private void buttonRight_Click(object sender, RoutedEventArgs e)
        {
            fs.Alignment = TextAlignment.Right;
        }

        private void buttonB_Click(object sender, RoutedEventArgs e)
        {
            fs.TextFontWeight = FontWeights.Bold;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            p.Children.Remove(this);
        }

    }


}
