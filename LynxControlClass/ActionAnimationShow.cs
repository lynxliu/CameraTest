using System;
using System.Net;
using System.Windows;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


using System.Windows.Input;



using System.Collections.Generic;
using Windows.UI.Xaml.Media.Animation;

namespace SilverlightLynxControls
{

    public class ActionAnimationShow
    {
        public ActionAnimationShow(FrameworkElement fo)
        {
            o = fo;
            BackParameter=new LynxAnimationCompleteEventArgs(this,fo);
            
        }

        void AddTransformGroup()//增加变换组
        {
            if (o == null) { return; }
            if (o.RenderTransform == null)
            {
                o.RenderTransform = ActionTransGroup;
            }
            else
            {
                Transform t = o.RenderTransform;
                if (t.GetType().Name == "TransformGroup")
                {
                    TransformGroup tg = t as TransformGroup;
                    tg.Children.Add(ActionTransGroup);
                }
                else
                {
                    TransformGroup tg = new TransformGroup();
                    tg.Children.Add(ActionTransGroup);
                    tg.Children.Add(t);
                    o.RenderTransform = tg;
                }
            }
        }

        void RemoveTransformGroup()
        {
            if (o == null) { return; }
            if (o.RenderTransform == ActionTransGroup)
            {
                o.RenderTransform = null;
            }
            else
            {
                Transform t = o.RenderTransform;
                if (t.GetType().Name == "TransformGroup")
                {
                    TransformGroup tg = t as TransformGroup;
                    if (tg.Children.Contains(ActionTransGroup))
                    {
                        tg.Children.Remove(ActionTransGroup);
                    }
                    if (tg.Children.Count == 1)
                    {
                        Transform xt=tg.Children[0];
                        tg.Children.Remove(xt);
                        o.RenderTransform = xt;
                    }
                }
            }
        }

        LynxAnimationCompleteEventArgs BackParameter=null;
        Storyboard sb = new Storyboard();
        TransformGroup ActionTransGroup = new TransformGroup();
        FrameworkElement o;
        public void InitZoomIn(double ActionLong)
        {
            
            sb.Stop();
            //TransformGroup tg;
            ScaleTransform st = new ScaleTransform();
            ActionTransGroup.Children.Add(st);
            //Transform t = o.RenderTransform;
            //if (t == null)
            //{
            //    tg = new TransformGroup();
            //    tg.Children.Add(st);
            //    o.RenderTransform = tg;
            //    //ActionTransGroup = tg;
            //}
            //else
            //{
            //    if (t.GetType().Name.EndsWith("TransformGroup"))
            //    {
            //        tg = (TransformGroup)t;
            //        tg.Children.Add(st);
                   
            //    }
            //    else
            //    {
            //        tg = new TransformGroup();
            //        tg.Children.Add(t);
            //        tg.Children.Add(st);
            //        o.RenderTransform = tg;
            //    }
            //}
            //ActionTransGroup.Children.Add(st);
            st.CenterX = 0d;
            st.CenterY = 0d;
            DoubleAnimation dax = new DoubleAnimation();
            DoubleAnimation day = new DoubleAnimation();
            sb.Children.Add(dax);
            sb.Children.Add(day);
            dax.Duration = new Duration(TimeSpan.FromMilliseconds(ActionLong));
            day.Duration = new Duration(TimeSpan.FromMilliseconds(ActionLong));
            Storyboard.SetTarget(dax, st);
            Storyboard.SetTarget(day, st);
            Storyboard.SetTargetProperty(dax, "ScaleTransform.ScaleXProperty");
            Storyboard.SetTargetProperty(day, "ScaleTransform.ScaleYProperty");
            dax.From = 0d;
            dax.To = 1d;
            day.From = 0d;
            day.To = 1d;
        }

        public void InitZoomOut(double ActionLong)
        {

            sb.Stop();
            //TransformGroup tg;
            ScaleTransform st = new ScaleTransform();
            ActionTransGroup.Children.Add(st);
            //Transform t = o.RenderTransform;
            //if (t == null)
            //{
            //    tg = new TransformGroup();
            //    tg.Children.Add(st);
            //    o.RenderTransform = tg;
            //}
            //else
            //{
            //    if (t.GetType().Name.EndsWith("TransformGroup"))
            //    {
            //        tg = (TransformGroup)t;
            //        tg.Children.Add(st);
            //    }
            //    else
            //    {
            //        tg = new TransformGroup();
            //        tg.Children.Add(t);
            //        tg.Children.Add(st);
            //        o.RenderTransform = tg;
            //    }
            //}
            st.CenterX = 0d;
            st.CenterY = 0d;
            DoubleAnimation dax = new DoubleAnimation();
            DoubleAnimation day = new DoubleAnimation();
            sb.Children.Add(dax);
            sb.Children.Add(day);
            dax.Duration = new Duration(TimeSpan.FromMilliseconds(ActionLong));
            day.Duration = new Duration(TimeSpan.FromMilliseconds(ActionLong));
            Storyboard.SetTarget(dax, st);
            Storyboard.SetTarget(day, st);
            Storyboard.SetTargetProperty(dax, "ScaleTransform.ScaleXProperty");
            Storyboard.SetTargetProperty(day, "ScaleTransform.ScaleYProperty");
            dax.From = 1d;
            dax.To = 0d;
            day.From = 1d;
            day.To = 0d;
        }

        public void InitZoomInV(double ActionLong)
        {
            sb.Stop();
            ScaleTransform st = new ScaleTransform();
            ActionTransGroup.Children.Add(st);
            //Transform t = o.RenderTransform;
            //if (t == null)
            //{
            //    tg = new TransformGroup();
            //    tg.Children.Add(st);
            //    o.RenderTransform = tg;
            //}
            //else
            //{
            //    if (t.GetType().Name.EndsWith("TransformGroup"))
            //    {
            //        tg = (TransformGroup)t;
            //        tg.Children.Add(st);
            //    }
            //    else
            //    {
            //        tg = new TransformGroup();
            //        tg.Children.Add(t);
            //        tg.Children.Add(st);
            //        o.RenderTransform = tg;
            //    }
            //}
            st.CenterY = 0d;
            DoubleAnimation day = new DoubleAnimation();
            sb.Children.Add(day);
            day.Duration = new Duration(TimeSpan.FromMilliseconds(ActionLong));
            Storyboard.SetTarget(day, st);
            Storyboard.SetTargetProperty(day, "ScaleTransform.ScaleYProperty");
            day.From = 0d;
            day.To = 1d;
        }

        public void InitZoomInH(double ActionLong)
        {
            sb.Stop();
            ScaleTransform st = new ScaleTransform();
            ActionTransGroup.Children.Add(st);
            //Transform t = o.RenderTransform;
            //if (t == null)
            //{
            //    tg = new TransformGroup();
            //    tg.Children.Add(st);
            //    o.RenderTransform = tg;
            //}
            //else
            //{
            //    if (t.GetType().Name.EndsWith("TransformGroup"))
            //    {
            //        tg = (TransformGroup)t;
            //        tg.Children.Add(st);
            //    }
            //    else
            //    {
            //        tg = new TransformGroup();
            //        tg.Children.Add(t);
            //        tg.Children.Add(st);
            //        o.RenderTransform = tg;
            //    }
            //}
            st.CenterX = 0d;
            DoubleAnimation dax = new DoubleAnimation();
            sb.Children.Add(dax);
            dax.Duration = new Duration(TimeSpan.FromMilliseconds(ActionLong));
            Storyboard.SetTarget(dax, st);
            Storyboard.SetTargetProperty(dax, "ScaleTransform.ScaleXProperty");
            dax.From = 0d;
            dax.To = 1d;
        }

        public void InitMove(Point SP, Point EP, double ActionLong)//一个对象的动态移动过程
        {
            Canvas.SetLeft(o, SP.X);//初始化位置，最终变化到终点
            Canvas.SetTop(o,SP.Y);
            BackParameter.EndP = EP;
            BackParameter.StartP = SP;
            sb.Stop();
            TranslateTransform tt = new TranslateTransform();
            ActionTransGroup.Children.Add(tt);
            DoubleAnimation day = new DoubleAnimation();
            DoubleAnimation dax = new DoubleAnimation();
            sb.Children.Add(day);
            sb.Children.Add(dax);
            day.Duration = new Duration(TimeSpan.FromMilliseconds(ActionLong));
            dax.Duration = new Duration(TimeSpan.FromMilliseconds(ActionLong));
            Storyboard.SetTarget(day, tt);
            Storyboard.SetTarget(dax, tt);
            Storyboard.SetTargetProperty(day, "TranslateTransform.YProperty");
            Storyboard.SetTargetProperty(dax, "TranslateTransform.XProperty");
            day.From = 0;
            day.To = EP.Y - SP.Y;
            dax.From = 0;
            dax.To = EP.X - SP.X;
        }

        public void InitProjection(double ActionLong)
        {
            sb.Stop();
            PlaneProjection pp = new PlaneProjection();
            o.Projection = pp;

            DoubleAnimation da = new DoubleAnimation();

            sb.Children.Add(da);

            da.Duration = new Duration(TimeSpan.FromMilliseconds(ActionLong));

            Storyboard.SetTarget(da, pp);

            Storyboard.SetTargetProperty(da, "PlaneProjection.RotationYProperty");

            da.From = 0d;
            da.To = 360d;
        }

        public void ShowZoomV(double ActionLong)
        {
            AddTransformGroup(); 
            InitZoomInV(ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();
        }


        public void ShowZoom(double ActionLong)
        {
            AddTransformGroup();
            InitZoomIn(ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();
        }

        public void ShowZoom(double ActionLong, Point SP, Point EP)
        {
            AddTransformGroup();
            InitZoomIn(ActionLong);
            InitMove(SP, EP, ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();
        }

        public void ShowProjection(double ActionLong)
        {
            AddTransformGroup();
            InitProjection(ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();
        }

        public void ShowZoomProjection(double ActionLong)
        {
            AddTransformGroup();
            InitZoomIn(ActionLong);
            InitProjection(ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();
        }
        public void ShowZoomProjection(double ActionLong, Point SP, Point EP)
        {
            AddTransformGroup();
            InitZoomIn(ActionLong);
            InitProjection(ActionLong);
            InitMove(SP, EP, ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();

        }
        public  void HideZoomProjection(double ActionLong)
        {
            AddTransformGroup();
            InitZoomOut(ActionLong);
            InitProjection(ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();

        }

        public  void HideZoomProjection(double ActionLong,Point SP,Point EP)
        {
            AddTransformGroup();
            InitZoomOut(ActionLong);
            InitProjection(ActionLong);
            InitMove(SP, EP, ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();

        }

        public  void HideZoom(double ActionLong)
        {
            AddTransformGroup();
            InitZoomOut(ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();

        }

        public void HideZoom(double ActionLong, Point SP, Point EP)
        {
            AddTransformGroup();
            InitZoomOut(ActionLong);
            InitMove(SP, EP, ActionLong);
            sb.Completed += sb_Completed;
            sb.Begin();

        }
        public event AnimationCompleteEventHandler AnimationComplete;//动画完成

        public void sb_Completed(object sender, object e)
        {
            sb.Stop();
            sb.Children.Clear();
            if (double.IsNaN(BackParameter.EndP.X) || (double.IsNaN(BackParameter.EndP.X)))
            {

            }
            else
            {
                Canvas.SetLeft(o, BackParameter.EndP.X);
                Canvas.SetTop(o, BackParameter.EndP.Y);
            }
            RemoveTransformGroup();
            SendCompleteEvent("Finish");
        }

        public void SendCompleteEvent(string Msg)//引发一个附带数据的失败消息
        {
            BackParameter.Message = Msg;
            if (AnimationComplete != null)
            {
                AnimationComplete(this, BackParameter);
            }
        }
    }

    public delegate void AnimationCompleteEventHandler(object sender, LynxAnimationCompleteEventArgs e);//定义的系统事件
    public class LynxAnimationCompleteEventArgs : EventArgs//通用的事件类，传送全部的参数
    {
        public LynxAnimationCompleteEventArgs(object sender, FrameworkElement Animation)
        {
            SenderObject = sender;
            AnimationObject = Animation;
        }
        public object SenderObject;//产生这个事件的对象句柄
        public DateTime EventTime = DateTime.Now;//事件发生时刻

        public string Message;//事件附加的消息
        public Point StartP = new Point(double.NaN, double.NaN);
        public Point EndP = new Point(double.NaN, double.NaN);
        public FrameworkElement AnimationObject;//参与动画的对象句柄
        public double StartScale;
        public double EndScale;
        public double StartAngle;
        public double EndAngle;

    }
}
