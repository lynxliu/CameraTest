using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace DCTestLibrary
{
    public class ColorManager
    {
        public static LabMode CurrentLanMode = LabMode.CIE;//整个系统默认的Lab颜色模式
        public static double getB(Color c)//获得标准亮度
        {
            return 0.299 * c.R + 0.587 * c.G + 0.114 * c.B;
        }

        public static double g(double x)
        {
            if (x < 0.018)
            {
                return 4.5318 * x;

            }
            else
            {
                return 1.099 * Math.Pow(x , 0.45) - 0.099;
            }
        }
        //public static Color Lab2RGB1(LColor lc)
        //{
        //    lc.CalculateLab2XYZ();
        //    lc.CalculateXYZ2RGB();

        //    return Color.FromArgb(255, lc.ARGB_R, lc.ARGB_G, lc.ARGB_B);
        //}

        //public static Color Lab2RGB(LColor lc)
        //{
        //    double R, G, B, X, Y, Z;
        //    double L = lc.Lab_L;
        //    double a = lc.Lab_a;
        //    double b = lc.Lab_b;
        //    L /= 2.55;
        //    a -= 128;
        //    b -= 128;

        //    double YYn3 = (L + 16) / 116;
        //    X = Math.Pow(YYn3 + a / 500, 3);
        //    Y = Math.Pow(YYn3, 3);
        //    Z = Math.Pow(YYn3 - b / 200, 3);

        //    R = 3.079978 * X - 1.537150 * Y - 0.542555 * Z;
        //    G = -0.921248 * X + 1.875992 * Y + 0.045225 * Z;
        //    B = 0.052891 * X - 0.204043 * Y + 1.150671 * Z;

        //    if (R > 0.00304)
        //        R = 1.055 * Math.Pow(R, 0.41667) - 0.055;
        //    else
        //        R = 12.92 * R;
        //    if (G > 0.00304)
        //        G = 1.055 * Math.Pow(G, 0.41667) - 0.055;
        //    else
        //        G = 12.92 * G;
        //    if (B > 0.00304)
        //        B = 1.055 * Math.Pow(B, 0.41667) - 0.055;
        //    else
        //        B = 12.92 * B;

        //    R *= 255;
        //    G *= 255;
        //    B *= 255;
        //    return Color.FromArgb(255, (byte)R, (byte)G, (byte)B);
        //}

        //public static LColor RGB2Lab(Color c)
        //{
        //    LColor lc = new LColor(c);
        //    return lc;
        //}

        //public static LColor RGB2HSB(Color c)
        //{
        //    LColor lc = new LColor();
        //    float r = c.R / 255f;
        //    float g = c.G / 255f;
        //    float b = c.B / 255f;
        //    //HSV Hsv = new HSV(0, 0, 0);
        //    float max = Math.Max(r, Math.Max(g, b));
        //    float min = Math.Min(r, Math.Min(g, b));
        //    float dif = max - min;
        //    if (dif == 0)
        //    {
        //        lc.HSB_H = 0;
        //    }
        //    else if (max == r)
        //    {
        //        if (g >= b)
        //        {
        //            lc.HSB_H = (g - b) / dif * 60f;
        //        }
        //        else
        //        {
        //            lc.HSB_H = (g - b) / dif * 60f + 360f;
        //        }
        //    }
        //    else if (max == g)
        //    {
        //        lc.HSB_H = (b - r) / dif * 60f + 120f;
        //    }
        //    else
        //    {
        //        lc.HSB_H = (r - g) / dif * 60f + 240f;
        //    }

        //    lc.HSB_S = max == 0 ? 0 : dif / max;
        //    lc.HSB_B = max;
        //    return lc;
        //}

        //public static LColor RGB2HSL(Color c)
        //{
        //    LColor lc = new LColor();
        //    float r = c.R / 255f;
        //    float g = c.G / 255f;
        //    float b = c.B / 255f;

        //    float max = Math.Max(r, Math.Max(g, b));
        //    float min = Math.Min(r, Math.Min(g, b));
        //    float dif = max - min;
        //    if (dif == 0)
        //    {
        //        lc.HSL_H = 0;
        //    }
        //    else if (max == r)
        //    {
        //        if (g >= b)
        //        {
        //            lc.HSL_H = (g - b) / dif * 60f;
        //        }
        //        else
        //        {
        //            lc.HSL_H = (g - b) / dif * 60f + 360f;
        //        }
        //    }
        //    else if (max == g)
        //    {
        //        lc.HSL_H = (b - r) / dif * 60f + 120f;
        //    }
        //    else
        //    {
        //        lc.HSL_H = (r - g) / dif * 60f + 240f;
        //    }

        //    lc.HSL_L = (max + min) / 2f;
        //    if (lc.HSL_L == 0 || max == min)
        //    {
        //        lc.HSL_S = 0;
        //    }
        //    else if (lc.HSL_L > 0.5f)
        //    {
        //        lc.HSL_S = dif / (2f - (max + min));
        //    }
        //    else
        //    {
        //        lc.HSL_S = dif / (max + min);
        //    }
        //    return lc;
        //}

        //public static Color HSB2RGB(LColor lc)
        //{
        //    int hi = (int)(lc.HSB_H / 60f) % 6;
        //    double f, p, q, t;
        //    f = lc.HSB_H / 60f - hi;
        //    p = lc.HSB_B * (1f - lc.HSB_S);
        //    q = lc.HSB_B * (1f - f * lc.HSB_S);
        //    t = lc.HSB_B * (1f - (1f - f) * lc.HSB_S);

        //    double r = 0f, g = 0f, b = 0f;
        //    switch (hi)
        //    {
        //        case 0:
        //            r = lc.HSB_B;
        //            g = t;
        //            b = p;
        //            break;
        //        case 1:
        //            r = q;
        //            g = lc.HSB_B;
        //            b = p;
        //            break;
        //        case 2:
        //            r = p;
        //            g = lc.HSB_B;
        //            b = t;
        //            break;
        //        case 3:
        //            r = p;
        //            g = q;
        //            b = lc.HSB_B;
        //            break;
        //        case 4:
        //            r = t;
        //            g = p;
        //            b = lc.HSB_B;
        //            break;
        //        case 5:
        //            r = lc.HSB_B;
        //            g = p;
        //            b = q;
        //            break;
        //    }

        //    Color Rgb = Color.FromArgb(255,
        //      (byte)Math.Round(r * 255),
        //      (byte)Math.Round(g * 255),
        //      (byte)Math.Round(b * 255));
        //    return Rgb;
        //}

        //public static Color HSL2RGB(LColor lc)
        //{
        //    if (lc.HSL_S == 0)
        //    {
        //        int l = (int)(lc.HSL_L * 255);
        //        return Color.FromArgb(255,255,255,255);
        //    }

        //    double q, p, hk, tr, tg, tb;
        //    q = lc.HSL_L < 0.5f ? lc.HSL_L * (1f + lc.HSL_S) : lc.HSL_L + lc.HSL_S - (lc.HSL_L * lc.HSL_S);
        //    p = 2f * lc.HSL_L - q;
        //    hk = lc.HSL_H / 360f;
        //    tr = hk + 1f / 3f;
        //    tg = hk;
        //    tb = hk - 1f / 3f;
        //    if (tr < 0) tr += 1f;
        //    if (tr > 1) tr -= 1f;
        //    if (tg < 0) tg += 1f;
        //    if (tg > 1) tg -= 1f;
        //    if (tb < 0) tb += 1f;
        //    if (tb > 1) tb -= 1f;
        //    double colorr, colorg, colorb;
        //    colorr = calc_ColorC(tr, p, q);
        //    colorg = calc_ColorC(tg, p, q);
        //    colorb = calc_ColorC(tb, p, q);

        //    Color Rgb = Color.FromArgb(255,
        //      (byte)Math.Round(colorr * 255),
        //      (byte)Math.Round(colorg * 255),
        //      (byte)Math.Round(colorb * 255));
        //    return Rgb;
        //}
        //static double calc_ColorC(double tc, double p, double q)
        //{
        //    double colorc;
        //    if (6f * tc < 1f)
        //    {
        //        colorc = p + ((q - p) * 6f * tc);
        //    }
        //    else if (6f * tc >= 1f && tc < 0.5f)
        //    {
        //        colorc = q;
        //    }
        //    else if (tc >= 0.5f && 3f * tc < 2f)
        //    {
        //        colorc = p + ((q - p) * 6f * (2f / 3f - tc));
        //    }
        //    else
        //    {
        //        colorc = p;
        //    }
        //    return colorc;
        //}
        public static Color getColorFromInt32(int i)//按照ARGB的十六进制存储的颜色和数字之间转换
        {
            byte[] components = new byte[4];
            components = BitConverter.GetBytes(i);
            Color c = new Color();
            c.B = components[0];
            c.G = components[1];
            c.R = components[2];
            c.A = components[3];
            return c;
        }

        public static int getInt32FromColor(Color c)
        {
            byte[] components = new byte[4];
            components[0] = c.B;
            components[1] = c.G;
            components[2] = c.R;
            components[3] = c.A;
            return BitConverter.ToInt32(components, 0);
        }

        public static float getHue(Color c)
        {
            LColor lc = new LColor(c);
            return Convert.ToSingle(lc.HSB_H);
        }

        public static float getSaturation(Color c)
        {
            LColor lc = new LColor(c);
            return Convert.ToSingle(lc.HSB_S);
        }

        public static float getBrightness(Color c)
        {
            LColor lc = new LColor(c);
            return Convert.ToSingle(lc.HSB_B);
        }

        public static double getLabL(Color c,LabMode m)
        {
            if(m==LabMode.CIE)
                return new LColor(c).CIELab_L;
            return new LColor(c).PhotoShopLab_L;
        }

        public static double getLaba(Color c, LabMode m)
        {
            if (m == LabMode.CIE)
                return new LColor(c).CIELab_a;
            return new LColor(c).PhotoShopLab_a;
        }

        public static double getLabb(Color c, LabMode m)
        {
            if (m == LabMode.CIE)
                return new LColor(c).CIELab_b;
            return new LColor(c).PhotoShopLab_b;
        }

        //public static double getLabL(Color c)
        //{
        //    return 0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B;
        //    //return new LColor(c).Lab_L;
        //}

        //public static double getLaba(Color c)
        //{
        //    return 1.4749 * (0.2213 * c.R - 0.3390 * c.G + 0.1177 * c.B) + 128;
        //    //return new LColor(c).Lab_a;
        //}

        //public static double getLabb(Color c)
        //{
        //    return 0.6245 * (0.1949 * c.R + 0.6057 * c.G - 0.8006 * c.B) + 128;
        //    //return new LColor(c).Lab_b;
        //}

        //public Color getColor(int L, int a, int b)
        //{
        //    Color c=new Color();
        //    c.R = Convert.ToByte(L + (a * 100922 + b * 17790) / 2^23);
        //    c.G = Convert.ToByte(L - (a * 30176 + b * 1481) / 2^23);
        //    c.B = Convert.ToByte(L + (a * 1740 - b * 37719) / 2^23);
        //    return c;
        //}
    }

    public enum LabMode {Undefine, Photoshop, CIE }

    public class LColor//自定义的能够在不同模式下面转换的颜色结构
    {
        public LColor() { }

        public LColor(Color c)
        {
            ARGB_R = c.R;
            ARGB_G = c.G;
            ARGB_B = c.B;
            ARGB_A = c.A;
            CalculateRGB2XYZ();
            CalculateXYZ2CIELab();
            CalculateRGB2PhotoShopLab();
            CalculateRGB2HSB();
            CalculateRGB2HSL();
        }

        public Color getColor()
        {
            return Color.FromArgb((byte)ARGB_A, (byte)ARGB_R, (byte)ARGB_G, (byte)ARGB_B);
        }

        public double getL(LabMode m)
        {
            if (m == LabMode.Photoshop)
                return PhotoShopLab_L;
            return CIELab_L;
        }
        public double geta(LabMode m)
        {
            if (m == LabMode.Photoshop)
                return PhotoShopLab_a;
            return CIELab_a;
        }
        public double getb(LabMode m)
        {
            if (m == LabMode.Photoshop)
                return PhotoShopLab_b;
            return CIELab_b;
        }
        public void setColorByRGB(double r, double g, double b)
        {
            if (r < 0)r=0;
            if (r > 255) r = 255;
            if (g < 0) r = 0;
            if (g > 255) r = 255;
            if (b < 0) r = 0;
            if (b > 255) r = 255;
            ARGB_R=Convert.ToByte(r);
            ARGB_G = Convert.ToByte(g);
            ARGB_B = Convert.ToByte(b);
            CalculateRGB2XYZ();
            CalculateXYZ2CIELab();
            CalculateRGB2PhotoShopLab();
            CalculateRGB2HSB();
            CalculateRGB2HSL();

        }
        public void setColorByLab(double l, double a, double b,LabMode m)
        {
            if (l < 0) l=0;
            if (l > 100) l=100 ;
            if (a < -128) a = -128;
            if (a > 127) a = 127;
            if (b < -128) b = -128;
            if (b > 127) b = 127;
            if (m == LabMode.CIE)
            {
                CIELab_L = l;
                CIELab_a = a;
                CIELab_b = b;
                CalculateCIELab2XYZ();
                CalculateXYZ2RGB();
                CalculateRGB2PhotoShopLab();
            }
            else
            {
                PhotoShopLab_L = l;
                PhotoShopLab_a = a;
                PhotoShopLab_b = b;
                CalculatePhotoShopLab2XYZ();
                CalculateXYZ2RGB();
                CalculateXYZ2CIELab();
            }
            
            CalculateRGB2HSB();
            CalculateRGB2HSL();
        }
        public void setColorByXYZ(double x, double y, double z)
        {
            if (x < 0)x=0;
            if (x > 100)  x=100;
            if (y < 0) y = 0;
            if (y > 100) y = 100;
            if (z < 0) z = 0;
            if (z > 100) z = 100;
            XYZ_X=x;
            XYZ_Y = y;
            XYZ_Z = z;
            CalculateXYZ2CIELab();
            CalculateXYZ2RGB();
            CalculateRGB2HSB();
            CalculateRGB2PhotoShopLab();
            CalculateRGB2HSL();
        }
        public void setColorByHSB(double h, double s, double b)
        {
            if (h < 0 || h > 360) { h = h % 360; if (h < 0)h = h + 360; }
            if (s < 0)s=0;
            if (s > 1) s=1;
            if (b < 0) b = 0;
            if (b > 1) b = 1;
            HSB_H = h;
            HSB_S = s;
            HSB_B = b;
            CalculateHSB2RGB();
            CalculateRGB2XYZ();
            CalculateRGB2PhotoShopLab();
            CalculateXYZ2CIELab();
            CalculateRGB2HSL();
        }
        public void setColorByHSL(double h, double s, double l)
        {
            if (h < 0 || h > 360) { h = h % 360; if (h < 0)h = h + 360; }
            if (s < 0) s = 0;
            if (s > 1) s = 1;
            if (l < 0) l= 0;
            if (l > 1) l = 1;
            HSL_H = h;
            HSL_S = s;
            HSL_L = l;
            CalculateHSL2RGB();
            CalculateRGB2XYZ();
            CalculateXYZ2CIELab();
            CalculateRGB2PhotoShopLab();
            CalculateRGB2HSB();
        }
        public byte ARGB_A=255;
        public byte ARGB_R;
        public byte ARGB_G;
        public byte ARGB_B;

        public double HSB_H;
        public double HSB_S;
        public double HSB_B;

        public double CIELab_L;
        public double CIELab_a;
        public double CIELab_b;

        public double PhotoShopLab_L;
        public double PhotoShopLab_a;
        public double PhotoShopLab_b;

        public double HSL_H;
        public double HSL_S;
        public double HSL_L;

        public double XYZ_X;
        public double XYZ_Y;
        public double XYZ_Z;

        public double YCrCb_Y;
        public double YCrCb_Cr;
        public double YCrCb_Cb;

        public double YPbPr_Y;
        public double YPbPr_Pb;
        public double YPbPr_Pr;

        public void CalculateRGB2YCbCr()
        {
            YCrCb_Y = 0.299 * ARGB_R + 0.587 * ARGB_G + 0.114 * ARGB_B;
            YCrCb_Cb = -0.1687 * ARGB_R - 0.3313 * ARGB_G + 0.500 * ARGB_B+128;
            YCrCb_Cr = 0.500 * ARGB_R - 0.4187 * ARGB_G - 0.0813 * ARGB_B+128;
        }

        public void CalculateYCbCr2RGB()
        {
            ARGB_R = Convert.ToByte(YCrCb_Y +1.4075 *(YCrCb_Cb-128));
            ARGB_G = Convert.ToByte(YCrCb_Y-0.3455 *(YCrCb_Cr-128)-0.7169 *(YCrCb_Cb-128));
            ARGB_B = Convert.ToByte(YCrCb_Y + 1.779 * (YCrCb_Cr - 128));
        }

        public void CalculateRGB2YPbPr()
        {
            YPbPr_Y=0.2126*ARGB_R + 0.7152*ARGB_G + 0.0722*ARGB_B;
            YPbPr_Pb= -0.1146*ARGB_R - 0.3854*ARGB_G + 0.5000*ARGB_B;
            YPbPr_Pr = 0.5000 * ARGB_R - 0.4542 * ARGB_G - 0.0458 * ARGB_B;
        }

        public void CalculateRGB2XYZ()
        {
            var var_R = (ARGB_R / 255d);        //R from 0 to 255
            var var_G = (ARGB_G / 255d);        //G from 0 to 255
            var var_B = (ARGB_B / 255d);        //B from 0 to 255

            if (var_R > 0.04045) var_R = Math.Pow((var_R + 0.055) / 1.055, 2.4);
            else var_R = var_R / 12.92;
            if (var_G > 0.04045) var_G = Math.Pow((var_G + 0.055) / 1.055, 2.4);
            else var_G = var_G / 12.92;
            if (var_B > 0.04045) var_B = Math.Pow((var_B + 0.055) / 1.055, 2.4);
            else var_B = var_B / 12.92;

            var_R = var_R * 100d;
            var_G = var_G * 100d;
            var_B = var_B * 100d;

            //Observer. = 2°, Illuminant = D65
            XYZ_X = var_R * 0.412453 + var_G * 0.357580 + var_B * 0.180423;
            XYZ_Y = var_R * 0.212671 + var_G * 0.715160 + var_B * 0.072169;
            XYZ_Z = var_R * 0.019334 + var_G * 0.119193 + var_B * 0.950227;

        }

        public void CalculateXYZ2RGB()
        {
            var var_X = XYZ_X / 100d;        //X from 0 to  95.047      (Observer = 2°, Illuminant = D65)
            var var_Y = XYZ_Y / 100d;        //Y from 0 to 100.000
            var var_Z = XYZ_Z / 100d;        //Z from 0 to 108.883

            var var_R = var_X * 3.240479 + var_Y * -1.537150 + var_Z * -0.498535;
            var var_G = var_X * -0.969256 + var_Y * 1.875992 + var_Z * 0.041556;
            var var_B = var_X * 0.055648 + var_Y * -0.204043 + var_Z * 1.057311;

            if ( var_R > 0.0031308 ) var_R = 1.055 * ( Math.Pow(var_R , ( 1d / 2.4 )) ) - 0.055;
            else                     var_R = 12.92 * var_R;
            if ( var_G > 0.0031308 ) var_G = 1.055 * ( Math.Pow(var_G , ( 1d / 2.4 )) ) - 0.055;
            else                     var_G = 12.92 * var_G;
            if ( var_B > 0.0031308 ) var_B = 1.055 * ( Math.Pow(var_B , ( 1d / 2.4 )) ) - 0.055;
            else                     var_B = 12.92 * var_B;

            ARGB_R = (byte)Math.Round(var_R * 255);
            ARGB_G = (byte)Math.Round(var_G * 255);
            ARGB_B = (byte)Math.Round(var_B * 255);

        }

        double getF(double t)
        {
            if (t > Math.Pow((6d / 29), 3))
            {
                return Math.Pow(t, 1d / 3d);
            }
            return 1d / 3d * (29d / 6d) * (29d / 6d) * t + 16d / 116d;
        }
        public void CalculateXYZ2CIELab()
        {
            var var_X = XYZ_X / 95.047d;          //ref_X =  95.047   Observer= 2°, Illuminant= D65
            var var_Y = XYZ_Y / 100.000d;          //ref_Y = 100.000
            var var_Z = XYZ_Z / 108.883d;          //ref_Z = 108.883

            CIELab_L = (116d * getF(var_Y)) - 16;
            CIELab_a = 500d * (getF(var_X) - getF(var_Y));
            CIELab_b = 200d * (getF(var_Y) - getF(var_Z));

        }

        public void CalculateCIELab2XYZ()
        {
            var var_Y = (CIELab_L + 16d) / 116d;
            var var_X = CIELab_a / 500d + var_Y;
            var var_Z = var_Y - CIELab_b / 200d;

            if ( Math.Pow(var_Y,3) > 0.008856 ) var_Y = Math.Pow(var_Y,3);
            else                      var_Y = ( var_Y - 16d / 116d ) / 7.787d;
            if ( Math.Pow(var_X,3) > 0.008856 ) var_X = Math.Pow(var_X,3);
            else                      var_X = ( var_X - 16d / 116d ) / 7.787d;
            if ( Math.Pow(var_Z,3) > 0.008856 ) var_Z = Math.Pow(var_Z,3);
            else                      var_Z = ( var_Z - 16d / 116d ) / 7.787d;

            XYZ_X = 95.047d * var_X;     //ref_X =  95.047     Observer= 2°, Illuminant= D65
            XYZ_Y = 100.000d * var_Y;     //ref_Y = 100.000
            XYZ_Z = 108.883d * var_Z;     //ref_Z = 108.883

        }

        public void CalculatePhotoShopLab2XYZ()
        {
            var var_Y = (PhotoShopLab_L + 16d) / 116d;
            var var_X = PhotoShopLab_a / 500d + var_Y;
            var var_Z = var_Y - PhotoShopLab_b / 200d;

            if (Math.Pow(var_Y, 3) > 0.008856) var_Y = Math.Pow(var_Y, 3);
            else var_Y = (var_Y - 16d / 116d) / 7.787d;
            if (Math.Pow(var_X, 3) > 0.008856) var_X = Math.Pow(var_X, 3);
            else var_X = (var_X - 16d / 116d) / 7.787d;
            if (Math.Pow(var_Z, 3) > 0.008856) var_Z = Math.Pow(var_Z, 3);
            else var_Z = (var_Z - 16d / 116d) / 7.787d;

            XYZ_X = 95.047d * var_X;     //ref_X =  95.047     Observer= 2°, Illuminant= D65
            XYZ_Y = 100.000d * var_Y;     //ref_Y = 100.000
            XYZ_Z = 108.883d * var_Z;     //ref_Z = 108.883

        }

        public void CalculateRGB2HSB()
        {
            double r = ARGB_R / 255d;
            double g = ARGB_G / 255d;
            double b = ARGB_B / 255d;
 
            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double dif = max - min;
            if (dif == 0)
            {
                HSB_H = 0;
            }
            else if (max == r)
            {
                if (g >= b)
                {
                    HSB_H = (g - b) / dif * 60d;
                }
                else
                {
                    HSB_H = (g - b) / dif * 60d + 360d;
                }
            }
            else if (max == g)
            {
                HSB_H = (b - r) / dif * 60d + 120d;
            }
            else
            {
                HSB_H = (r - g) / dif * 60d + 240d;
            }

            HSB_S = max == 0 ? 0 : dif / max;
            HSB_B = max;
        }

        public void CalculateRGB2HSL()
        {
            double r = ARGB_R / 255d;
            double g = ARGB_G / 255d;
            double b = ARGB_B / 255d;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double dif = max - min;
            if (dif == 0)
            {
                HSL_H = 0;
            }
            else if (max == r)
            {
                if (g >= b)
                {
                    HSL_H = (g - b) / dif * 60d;
                }
                else
                {
                    HSL_H = (g - b) / dif * 60d + 360d;
                }
            }
            else if (max == g)
            {
                HSL_H = (b - r) / dif * 60d + 120d;
            }
            else
            {
                HSL_H = (r - g) / dif * 60d + 240d;
            }

            HSL_L = (max + min) / 2d;
            if (HSL_L == 0 || max == min)
            {
                HSL_S = 0;
            }
            else if (HSL_L > 0.5)
            {
                HSL_S = dif / (2d - (max + min));
            }
            else
            {
                HSL_S = dif / (max + min);
            }
        }

        public void CalculateHSB2RGB()
        {
            int hi = (int)(HSB_H / 60d) % 6;
            double f, p, q, t;
            f = HSB_H / 60f - hi;
            p = HSB_B * (1f - HSB_S);
            q = HSB_B * (1f - f * HSB_S);
            t = HSB_B * (1f - (1f - f) * HSB_S);

            double r = 0d, g = 0d, b = 0d;
            switch (hi)
            {
                case 0:
                    r = HSB_B;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = HSB_B;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = HSB_B;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = HSB_B;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = HSB_B;
                    break;
                case 5:
                    r = HSB_B;
                    g = p;
                    b = q;
                    break;
            }

            ARGB_R=  (byte)Math.Round(r * 255);
            ARGB_G = (byte)Math.Round(g * 255);
            ARGB_B = (byte)Math.Round(b * 255);

        }

        public void CalculateHSL2RGB()
        {
            if (HSL_S == 0)
            {
                int l = (int)(HSL_L * 255);
                ARGB_R = 255;
                ARGB_G = 255;
                ARGB_B = 255;
            }

            double q, p, hk, tr, tg, tb;
            q = HSL_L < 0.5f ? HSL_L * (1f + HSL_S) : HSL_L + HSL_S - (HSL_L * HSL_S);
            p = 2f * HSL_L - q;
            hk = HSL_H / 360;
            tr = hk + 1f / 3;
            tg = hk;
            tb = hk - 1f / 3;
            if (tr < 0) tr += 1f;
            if (tr > 1) tr -= 1f;
            if (tg < 0) tg += 1f;
            if (tg > 1) tg -= 1f;
            if (tb < 0) tb += 1f;
            if (tb > 1) tb -= 1f;
            double colorr, colorg, colorb;
            colorr = calc_ColorC(tr, p, q);
            colorg = calc_ColorC(tg, p, q);
            colorb = calc_ColorC(tb, p, q);

            ARGB_R = (byte)Math.Round(colorr * 255);
            ARGB_G=  (byte)Math.Round(colorg * 255);
            ARGB_B=  (byte)Math.Round(colorb * 255);
        }
        double calc_ColorC(double tc, double p, double q)
        {
            double colorc;
            if (6f * tc < 1f)
            {
                colorc = p + ((q - p) * 6f * tc);
            }
            else if (6f * tc >= 1f && tc < 0.5f)
            {
                colorc = q;
            }
            else if (tc >= 0.5f && 3f * tc < 2f)
            {
                colorc = p + ((q - p) * 6f * (2f / 3f - tc));
            }
            else
            {
                colorc = p;
            }
            return colorc;
        }

        public void CalculateRGB2PhotoShopLab()
        {
            double X, Y, Z;
            double r = ARGB_R / 255.000; // rgb range: 0 ~ 1
            double g = ARGB_G / 255.000;
            double b = ARGB_B / 255.000;

            // gamma 2.2
            if (r > 0.04045)
                r = Math.Pow((r + 0.055) / 1.055, 2.4);
            else
                r = r / 12.92;
            if (g > 0.04045)
                g = Math.Pow((g + 0.055) / 1.055, 2.4);
            else
                g = g / 12.92;
            if (b > 0.04045)
                b = Math.Pow((b + 0.055) / 1.055, 2.4);
            else
                b = b / 12.92;

            // sRGB
            X = r * 0.436052025 + g * 0.385081593 + b * 0.143087414;
            Y = r * 0.222491598 + g * 0.716886060 + b * 0.060621486;
            Z = r * 0.013929122 + g * 0.097097002 + b * 0.714185470;

            // XYZ range: 0~100
            X = X * 100.000;
            Y = Y * 100.000;
            Z = Z * 100.000;

            // Reference White Point
            double ref_X = 96.4221;
            double ref_Y = 100.000;
            double ref_Z = 82.5211;

            X = X / ref_X;
            Y = Y / ref_Y;
            Z = Z / ref_Z;

            // Lab
            if (X > 0.008856)
                X = Math.Pow(X, 1 / 3.000);
            else
                X = (7.787 * X) + (16 / 116.000);
            if (Y > 0.008856)
                Y = Math.Pow(Y, 1 / 3.000);
            else
                Y = (7.787 * Y) + (16 / 116.000);
            if (Z > 0.008856)
                Z = Math.Pow(Z, 1 / 3.000);
            else
                Z = (7.787 * Z) + (16 / 116.000);

            PhotoShopLab_L = (116.000 * Y) - 16.000;
            PhotoShopLab_a = 500.000 * (X - Y);
            PhotoShopLab_b = 200.000 * (Y - Z);
        }

    }
}
