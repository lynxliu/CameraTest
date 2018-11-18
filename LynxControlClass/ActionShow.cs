using System;
using System.Net;
using System.Windows;
using System.Reflection;


using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation;




namespace SilverlightLynxControls
{
    public class ActionShow
    {
        public static void Close(FrameworkElement uc)
        {
            Panel p = (Panel)uc.Parent;
            if ((p != null)&&(p.Children.Contains(uc)))
            {
                p.Children.Remove(uc);
            }
        }

        public static void ResetZoom(FrameworkElement Control)//取消缩放
        {
            if (Control.RenderTransform == null)//本身没有任何的Transform，完全新建
            {
                return;
            }
            if (Control.RenderTransform is TransformGroup)//最顶层的Transform是一个Group
            {
                TransformGroup t = Control.RenderTransform as TransformGroup;
                foreach (Transform tf in t.Children)//判断是否已经有ScaleTransform
                {
                    if (tf is ScaleTransform)//存在缩放就删除
                    {
                        t.Children.Remove(tf);
                        return;
                    }
                }
            }
            if (Control.RenderTransform is ScaleTransform)//顶层的Transform就是ScaleTransform
            {
                Control.RenderTransform = null;
                return;
            }
        }
        public static void ZoomIn(FrameworkElement Control, double percent)
        {
            ZoomIn(Control, percent, percent);
        }
        public static void ZoomIn(FrameworkElement RootC, double percentX,double percentY)
        {
            if (RootC.RenderTransform == null)//本身没有任何的Transform，完全新建
            {
                TransformGroup t = new TransformGroup();
                ScaleTransform st = new ScaleTransform();
                t.Children.Add(st);
                st.ScaleX = percentX;
                st.ScaleY = percentY;
                RootC.RenderTransform = t;
                return;
            }
            if (RootC.RenderTransform is TransformGroup)//最顶层的Transform是一个Group
            {
                TransformGroup t = RootC.RenderTransform as TransformGroup;
                foreach (Transform tf in t.Children)//判断是否已经有ScaleTransform
                {
                    if (tf is ScaleTransform)//存在就修改其缩放比例
                    {
                        ScaleTransform st = tf as ScaleTransform;
                        st.ScaleX = st.ScaleX* percentX;
                        st.ScaleY = st.ScaleY*percentY;
                        return;
                    }
                }
                ScaleTransform stf = new ScaleTransform();//没有就新建
                t.Children.Add(stf);
                stf.ScaleX = percentX;
                stf.ScaleY = percentY;
                t.Children.Add(stf);
            }
            if (RootC.RenderTransform is ScaleTransform)//顶层的Transform就是ScaleTransform
            {
                ScaleTransform st = RootC.RenderTransform as ScaleTransform;
                st.ScaleX = st.ScaleX * percentX;
                st.ScaleY = st.ScaleY * percentY;
                return;
            }
            TransformGroup pt = new TransformGroup();//顶层有Trandform但是不是ScaleTransform
            ScaleTransform pst = new ScaleTransform();//增加一个Group加入已有的所有的Transform
            pt.Children.Add(pst);
            pst.ScaleX = percentX;
            pst.ScaleY = percentY;
            pt.Children.Add(RootC.RenderTransform);
            RootC.RenderTransform = pt;
            //RootC.Width = RootC.Width * percent;
            //RootC.Height = RootC.Height * percent;
            //RootC.GetValue(UserControl.
        }

        public static void ZoomCanvasIn(Panel RootPanel, double percent)//放大控件到一个比例
        {
            ZoomIn(RootPanel, percent, percent);
            //object po = RootPanel.Parent;
            //if (po.GetType().Name == "UserControl")
            //{
            //    UserControl uc = (UserControl)po;
            //    uc.Width = uc.ActualWidth * percent;
            //    uc.Height = uc.ActualHeight * percent;
            //}

            //Canvas.SetLeft(RootPanel, Canvas.GetLeft(RootPanel) * percent);
            //Canvas.SetTop(RootPanel, Canvas.GetTop(RootPanel) * percent);
            //RootPanel.Width = RootPanel.Width * percent;
            //RootPanel.Height = RootPanel.Height * percent;
            //foreach (UIElement f in RootPanel.Children)
            //{
            //    if (IsPanel(f.GetType()))
            //    {
            //        ZoomCanvasIn((Panel)f, percent);
            //    }
            //    else
            //    {
            //        if (f is Line)
            //        {
            //            Line l = f as Line;
            //            l.X1 = l.X1 * percent;
            //            l.X2 = l.X2 * percent;
            //            l.Y1 = l.Y1 * percent;
            //            l.Y2 = l.Y2 * percent;
            //            continue;//直接进入下一个循环
            //        }
            //        if (f is Path)
            //        {
            //            Path p = f as Path;
            //            Canvas.SetLeft(p, Canvas.GetLeft(p) * percent);
            //            Canvas.SetTop(p, Canvas.GetTop(p) * percent);
            //            p.Width = p.Width * percent;
            //            p.Height = p.Height * percent;
            //            if (p.Data is PathGeometry)
            //            {
            //                foreach (PathFigure pf in (p.Data as PathGeometry).Figures)
            //                {
            //                    pf.StartPoint = new Point(pf.StartPoint.X * percent, pf.StartPoint.Y*percent);
            //                    foreach (PathSegment ps in pf.Segments)
            //                    {
            //                        if (ps is LineSegment)
            //                        {
            //                            (ps as LineSegment).Point = new Point((ps as LineSegment).Point.X * percent, (ps as LineSegment).Point.Y * percent);
            //                        }
            //                        if (ps is ArcSegment)
            //                        {
            //                            (ps as ArcSegment).Point = new Point((ps as LineSegment).Point.X * percent, (ps as LineSegment).Point.Y * percent);

            //                        }
            //                    }
            //                }
            //            }
            //        }

            //        Canvas.SetLeft(f, Canvas.GetLeft(f) * percent);
            //        Canvas.SetTop(f, Canvas.GetTop(f) * percent);
            //        FrameworkElement ff = f as FrameworkElement;

            //        if (ff != null)
            //        {
            //            ff.Width = ff.Width * percent;
            //            ff.Height = ff.Height * percent;
            //        }

            //    }
            //}
        }

        public static void XZoomCanvasIn(Panel RootPanel, double percent)//水平放大控件到一个比例
        {
            ZoomIn(RootPanel, percent, 1);
            //object po = RootPanel.Parent;
            //if (po.GetType().Name == "UserControl")
            //{
            //    UserControl uc = (UserControl)po;
            //    uc.Width = uc.ActualWidth * percent;
            //}

            //Canvas.SetLeft(RootPanel, Canvas.GetLeft(RootPanel) * percent);
            //RootPanel.Width = RootPanel.Width * percent;
            //foreach (UIElement f in RootPanel.Children)
            //{
            //    if (IsPanel(f.GetType()))
            //    {
            //        XZoomCanvasIn((Panel)f, percent);
            //    }
            //    else
            //    {
            //        if (f.GetType().Name != "Line")
            //        {
            //            Canvas.SetLeft(f, Canvas.GetLeft(f) * percent);
            //            FrameworkElement ff = f as FrameworkElement;

            //            if (ff != null)
            //            {
            //                ff.Width = ff.Width * percent;
            //            }
            //        }
            //        else
            //        {
            //            Line l = f as Line;
            //            l.X1 = l.X1 * percent;
            //            l.X2 = l.X2 * percent;
            //        }
            //    }
            //}
        }

        public static void YZoomCanvasIn(Panel RootPanel, double percent)//放大控件到一个比例
        {
            ZoomIn(RootPanel,1, percent);
            //object po = RootPanel.Parent;
            //if (po.GetType().Name == "UserControl")
            //{
            //    UserControl uc = (UserControl)po;
            //    uc.Height = uc.Height * percent;
            //}

            //Canvas.SetTop(RootPanel, Canvas.GetTop(RootPanel) * percent);
            //RootPanel.Height = RootPanel.Height * percent;
            //foreach (UIElement f in RootPanel.Children)
            //{
            //    if (IsPanel(f.GetType()))
            //    {
            //        YZoomCanvasIn((Panel)f, percent);
            //    }
            //    else
            //    {
            //        if (f.GetType().Name != "Line")
            //        {
            //            Canvas.SetTop(f, Canvas.GetTop(f) * percent);
            //            FrameworkElement ff = f as FrameworkElement;

            //            if (ff != null)
            //            {
            //                ff.Height = ff.Height * percent;
            //            }
            //        }
            //        else
            //        {
            //            Line l = f as Line;
            //            l.Y1 = l.Y1 * percent;
            //            l.Y2 = l.Y2 * percent;
            //        }
            //    }
            //}
        }

        public static bool IsPanel(Type c)
        {

            string TypeName = c.Name;
            if (TypeName == "Object") { return false; }
            if (TypeName == "DependencyObject") { return false; }
            if (TypeName == "UIElement") { return false; }
            if (TypeName == "FrameworkElement") { return false; }
            if (TypeName == "Panel") { return true; }
            return IsPanel(c.GetTypeInfo().BaseType);
        }

        public static void CenterShow(Panel pc,FrameworkElement fc)
        {
            if (!pc.Children.Contains(fc))
            {
                pc.Children.Add(fc);
            }
            double w, h;
            if (fc.Width > 0)
            {
                w = fc.Width;
            }
            else
            {
                w = fc.ActualWidth;
            }
            if (fc.Height > 0)
            {
                h = fc.Height;
            }
            else
            {
                h = fc.ActualHeight;
            }
            w = (pc.ActualWidth - w) / 2;
            h = (pc.ActualHeight - h) / 2;
            Canvas.SetLeft(fc, w);
            Canvas.SetTop(fc, h);

            ActionActive.Active(fc);
        }

        public static void CenterShow(Canvas pc, FrameworkElement fc,Point sp)
        {
            pc.Children.Add(fc);
            Canvas.SetLeft(fc, sp.X);
            Canvas.SetTop(fc, sp.Y);


        }

    }
}
