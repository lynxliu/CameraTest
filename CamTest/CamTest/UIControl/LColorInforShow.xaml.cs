using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using DCTestLibrary;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml;

namespace SLPhotoTest.UIControl
{
    public partial class LColorInforShow : UserControl
    {
        public LColorInforShow()
        {
            InitializeComponent();
        }
        private LabMode getCurrentLabMode()
        {
            if (comboBoxLabMode.SelectedIndex == 0)
            {
                return LabMode.CIE;
            }
            else
            {
                return LabMode.Photoshop;
            }
        }
        Color _color;
        public Color color
        {
            get { return _color; }
            set
            {
                _color = value;
                //rectangle1.Fill = new SolidColorBrush(value);
                textRGB_R.Text = value.R.ToString();
                textRGB_G.Text = value.G.ToString();
                textRGB_B.Text = value.B.ToString();
                LColor lc = new LColor(value);
                textHSB_H.Text = lc.HSB_H.ToString();
                textHSB_S.Text = lc.HSB_S.ToString();
                textHSB_B.Text = lc.HSB_B.ToString();
                textLab_L.Text = DCTestLibrary.ColorManager.getLabL(value, getCurrentLabMode()).ToString();
                textLab_a.Text = DCTestLibrary.ColorManager.getLaba(value, getCurrentLabMode()).ToString();
                textLab_b.Text = DCTestLibrary.ColorManager.getLabb(value, getCurrentLabMode()).ToString();


                textBoxLabL1.Text = DCTestLibrary.ColorManager.getLabL(value, getCurrentLabMode()).ToString();
                textBoxLaba1.Text = DCTestLibrary.ColorManager.getLaba(value, getCurrentLabMode()).ToString();
                textBoxLabb1.Text = DCTestLibrary.ColorManager.getLabb(value, getCurrentLabMode()).ToString();
            }
        }

        private void buttonRGB2_Click(object sender, RoutedEventArgs e)
        {
            color = Color.FromArgb(255, Convert.ToByte(textRGB_R.Text), Convert.ToByte(textRGB_G.Text), Convert.ToByte(textRGB_B.Text));
            //LColor lc= DCTestLibrary.ColorManager.RGB2Lab(_color,0);
            //LColor lc2 = DCTestLibrary.ColorManager.RGB2Lab(_color);
            //LColor lc1 = DCTestLibrary.ColorManager.RGB2HSB(_color);
            //textRGB_R.Text = _color.R.ToString();
            //textRGB_G.Text = _color.G.ToString();
            //textRGB_B.Text = _color.B.ToString();
            //textHSB_H.Text = DCTestLibrary.ColorManager.getHue(_color).ToString();
            //textHSB_S.Text = DCTestLibrary.ColorManager.getSaturation(_color).ToString();
            //textHSB_B.Text = DCTestLibrary.ColorManager.getBrightness(_color).ToString();
            //textLab_L.Text = DCTestLibrary.ColorManager.getLabL(_color).ToString();
            //textLab_a.Text = DCTestLibrary.ColorManager.getLaba(_color).ToString();
            //textLab_b.Text = DCTestLibrary.ColorManager.getLabb(_color).ToString();
        }

        private void buttonHSB2_Click(object sender, RoutedEventArgs e)
        {
            //_color = Color.FromArgb(255, Convert.ToByte(textRGB_R.Text), Convert.ToByte(textRGB_G.Text), Convert.ToByte(textRGB_B.Text));
            LColor lc= new LColor();
            lc.setColorByHSB(Convert.ToDouble(textHSB_H.Text), Convert.ToDouble(textHSB_S.Text), Convert.ToDouble(textHSB_B.Text));
            //lc.HSB_H = Convert.ToDouble(textHSB_H.Text);
            //lc.HSB_S = Convert.ToDouble(textHSB_S.Text);
            //lc.HSB_B = Convert.ToDouble(textHSB_B.Text);
            //_color = DCTestLibrary.ColorManager.HSB2RGB(lc);
            //LColor lc2 = DCTestLibrary.ColorManager.HSB2RGB(_color);
            //LColor lc1 = DCTestLibrary.ColorManager.RGB2HSB(_color);
            textRGB_R.Text = lc.ARGB_R.ToString();
            textRGB_G.Text = lc.ARGB_G.ToString();
            textRGB_B.Text = lc.ARGB_B.ToString();
            //textHSB_H.Text = DCTestLibrary.ColorManager.getHue(_color).ToString();
            //textHSB_S.Text = DCTestLibrary.ColorManager.getSaturation(_color).ToString();
            //textHSB_B.Text = DCTestLibrary.ColorManager.getBrightness(_color).ToString();
            textLab_L.Text = DCTestLibrary.ColorManager.getLabL(_color, getCurrentLabMode()).ToString();
            textLab_a.Text = DCTestLibrary.ColorManager.getLaba(_color, getCurrentLabMode()).ToString();
            textLab_b.Text = DCTestLibrary.ColorManager.getLabb(_color, getCurrentLabMode()).ToString();
        }

        private void buttonLab2_Click(object sender, RoutedEventArgs e)
        {
            LColor lc = new LColor();
            lc.setColorByLab(Convert.ToDouble(textLab_L.Text), Convert.ToDouble(textLab_a.Text), Convert.ToDouble(textLab_b.Text), getCurrentLabMode());
            //lc.Lab_L = Convert.ToDouble(textLab_L.Text);
            //lc.Lab_a = Convert.ToDouble(textLab_a.Text);
            //lc.Lab_b = Convert.ToDouble(textLab_b.Text);
            //_color = DCTestLibrary.ColorManager.Lab2RGB(lc);
            //LColor lc2 = DCTestLibrary.ColorManager.RGB2HSB(_color);
            //LColor lc1 = DCTestLibrary.ColorManager.Lab2RGB(_color);
            textRGB_R.Text = _color.R.ToString();
            textRGB_G.Text = _color.G.ToString();
            textRGB_B.Text = _color.B.ToString();
            textHSB_H.Text = DCTestLibrary.ColorManager.getHue(_color).ToString();
            textHSB_S.Text = DCTestLibrary.ColorManager.getSaturation(_color).ToString();
            textHSB_B.Text = DCTestLibrary.ColorManager.getBrightness(_color).ToString();
            //textLab_L.Text = DCTestLibrary.ColorManager.getLabL(_color).ToString();
            //textLab_a.Text = DCTestLibrary.ColorManager.getLaba(_color).ToString();
            //textLab_b.Text = DCTestLibrary.ColorManager.getLabb(_color).ToString();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LColor lc = new LColor();
            //lc.Lab_L = Convert.ToDouble(textBoxLabL1.Text);
            //lc.Lab_a = Convert.ToDouble(textBoxLaba1.Text);
            //lc.Lab_b = Convert.ToDouble(textBoxLabb1.Text);
            //lc.CalculateLab2XYZ();
            lc.CalculateXYZ2RGB();
            textRGB_R.Text = lc.ARGB_R.ToString();
            textRGB_G.Text = lc.ARGB_G.ToString();
            textRGB_B.Text = lc.ARGB_B.ToString();

        }
    }
}
