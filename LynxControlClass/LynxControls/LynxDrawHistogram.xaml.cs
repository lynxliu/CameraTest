using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;




namespace SilverlightLynxControls.LynxControls
{
    public partial class LynxDrawHistogram : UserControl
    {
        public LynxDrawHistogram()
        {
            InitializeComponent();
        }

        public double StartX, startY;//绘图区域的起始位置
        public double TotleH, TotleW;//绘图区域的高度
        public List<string> XMarkList;
        public List<double> YMarkList;

        public Canvas PC; 
        public Dictionary<string, Dictionary<string, double>> ObjectValueList;//默认的数据列表，多个对象可以对比表示
        List<Color> lc=new List<Color>();
        public void InitColor()
        {
            for (int i = 0; i < ObjectValueList.Count; i++)
            {
                lc.Add(getRadomColor());
            }
        }

        public Color getRadomColor()
        {
            Byte r, g, b;
            Random rd=new Random(20);
            r =Convert.ToByte( 255 * rd.NextDouble());
            g = Convert.ToByte(255 * rd.NextDouble());
            b = Convert.ToByte(255 * rd.NextDouble());
            return Color.FromArgb(255, r, g, b);
        }

        public void DrawXLine(List<string> nl)
        {
            Line l = new Line();
            l.StrokeThickness = 5;
            l.Stroke = new SolidColorBrush(Colors.Blue);
            l.X1 = this.ActualWidth * 0.05;
            l.X2 = this.ActualWidth * 0.95;
            l.Y1 = this.ActualHeight * 0.95;
            l.Y2 = this.ActualHeight * 0.95;
            LayoutRoot.Children.Add(l);

            for (int i = 0; i < nl.Count; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Text = nl[i];
                tb.SetValue(Canvas.LeftProperty, (this.ActualWidth * 0.9 / nl.Count) * i + this.ActualWidth * 0.05);
                tb.SetValue(Canvas.TopProperty, this.ActualHeight * 0.95 + 5);
                LayoutRoot.Children.Add(tb);
            }
        }

        public void DrawYLine()
        {
            Line l = new Line();
            l.StrokeThickness = 5;
            l.Stroke = new SolidColorBrush(Colors.Blue);
            l.X1 = this.ActualWidth * 0.05;
            l.X2 = this.ActualWidth * 0.05;
            l.Y1 = this.ActualHeight * 0.05;
            l.Y2 = this.ActualHeight * 0.95;
            LayoutRoot.Children.Add(l);

            for (int i = 0; i < 10; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Text = (MinValue+(MaxValue-MinValue)/10*i).ToString();
                tb.SetValue(Canvas.LeftProperty, 0d);
                tb.SetValue(Canvas.TopProperty, (this.ActualHeight * 0.9 / 10) * (10-i) + this.ActualHeight * 0.05);
                LayoutRoot.Children.Add(tb);
            }
        }

        public void AnimotionHis(Rectangle r)
        {
            Storyboard sb = new Storyboard();
            sb.Duration = new Duration(TimeSpan.FromSeconds(2));
            DoubleAnimation da = new DoubleAnimation();
            DoubleAnimation da1 = new DoubleAnimation();
            sb.Children.Add(da);
            sb.Children.Add(da1);
            //ScaleTransform sf = new ScaleTransform();
            //r.RenderTransform = sf;
            da.From = 0;
            da.To = r.Height;
            da1.From = r.Height+(double)r.GetValue(Canvas.TopProperty);
            da1.To = (double)r.GetValue(Canvas.TopProperty);
            //PropertyPath pp = new PropertyPath("(FrameworkElement.Height)");
            //PropertyPath pp1 = new PropertyPath("(Canvas.Top)");
            Storyboard.SetTarget(da, r);
            Storyboard.SetTarget(da1, r);

            Storyboard.SetTargetProperty(da, "(FrameworkElement.Height)");
            Storyboard.SetTargetProperty(da1, "(Canvas.Top)");
            sb.Begin();
        }

        public void AnimotionHis()
        {
            foreach (Rectangle r in rl)
            {
                AnimotionHis(r);
            }
        }

        public void DrawHis(double left, double height, Color c)
        {
            Rectangle r = new Rectangle();
            double h = (height - MinValue) / (MaxValue - MinValue)*this.ActualHeight*0.9;
            r.Height = h;
            r.Width = pw;
            r.Fill = new SolidColorBrush(c);
            r.SetValue(Canvas.LeftProperty, left);
            r.SetValue(Canvas.TopProperty, this.ActualHeight * 0.95 - h);
            rl.Add(r);
            LayoutRoot.Children.Add(r);
        }
        List<Rectangle> rl = new List<Rectangle>();
        public void DrawHis(Dictionary<string, double> ol,int index)
        {
            Color c = lc[index];
            int i=0;
            foreach (KeyValuePair<string, double> vo in ol)
            {
                double left = wi * index + pw * (index + i)+this.ActualWidth*0.1;
                DrawHis(left, vo.Value, c);
                i++;

            }
        }

        public void DrawHis()
        {
            int i = 0;
            foreach (KeyValuePair<string, Dictionary<string, double>> k in ObjectValueList)
            {
                Dictionary<string, double> ol = k.Value;
                DrawHis(ol, i);
                i++;
            }
            
        }



        public List<string> getObjectNameList()//得到要显示的对象数据
        {
            List<string> nl = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, double>> k in ObjectValueList)
            {
                    if (nl.Contains(k.Key)) { }
                    else
                    {
                        nl.Add(k.Key);
                    }

            }
            return nl;
        }

        public List<string> getTitleList()//找到所有的项目标题
        {
            List<string> nl = new List<string>();
            foreach (KeyValuePair<string, Dictionary<string, double>> k in ObjectValueList)
            {

                Dictionary<string, double> ol = k.Value;
                foreach (KeyValuePair<string, double> vo in ol)
                {
                    if(nl.Contains(vo.Key)){}else
                    {
                        nl.Add(vo.Key);
                    }

                }
            }
            return nl;
        }

        public void test()
        {
            ObjectValueList = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, double> ol = new Dictionary<string, double>();
            ol.Add("a", 15);
            ol.Add("b", 17);
            ol.Add("cc", 25);
            ObjectValueList.Add("a", ol);
            Dictionary<string, double> ol1 = new Dictionary<string, double>();
            ol1.Add("a", 15);
            ol1.Add("b", 17);
            ol1.Add("cc", 25);
            ObjectValueList.Add("a1", ol1);
            Draw();
            AnimotionHis();
        }

        public void Draw()
        {
            Init();
            DrawXLine(getTitleList());
            DrawYLine();
            DrawHis();
        }
        public double pw = 0;
        public double wi = 0;
        public double MinValue = 0;
        public double MaxValue = 0;
        public void Init()
        {

            if (ObjectValueList.Count == 0)
            {
                return;
            }
            InitColor();
            pw= this.ActualWidth*0.9 / ObjectValueList.Count;
            wi = pw ;
            pw = pw * 0.8;
            int MaxCount = 0;
            foreach(KeyValuePair<string, Dictionary<string, double>> k in ObjectValueList)
            {
                if (k.Value.Count > MaxCount)
                {
                    MaxCount = k.Value.Count;
                }
                Dictionary<string, double> ol = k.Value;
                foreach (KeyValuePair<string, double> vo in ol)
                {
                    if (vo.Value > MaxValue)
                    {
                        MaxValue = vo.Value;
                    }
                    if (vo.Value < MinValue)
                    {
                        MinValue = vo.Value;
                    }
                }
            }
            pw = pw / MaxCount;
            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            test();
        }


    }
}
