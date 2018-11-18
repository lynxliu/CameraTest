using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Reflection;

using System.Windows.Input;



using System.Xml;
using System.IO;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Devices.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Input;
using Windows.Foundation;

namespace SilverlightLynxControls
{
    public partial class LynxDesktop : UserControl
    {
        public LynxDesktop()
        {
            InitializeComponent();
            //AppDomain.CurrentDomain.SetData("CurrentDesktop", this);
            //Application.Current.Resources.Add("CurrentDesktop", this);

        }

        public static LynxDesktop FindLynxDesktop(FrameworkElement c)
        {
            UIElement root = Window.Current.Content;
            FrameworkElement tc = (FrameworkElement)c.Parent;
            if (tc == null) { return null; }
            while (tc.GetType().Name != root.GetType().Name)
            {
                if (tc.GetType().Name == "LynxDesktop")
                {
                    return (LynxDesktop)tc;
                }
                tc = (FrameworkElement)tc.Parent;
            }
            return null;
        }

        public void AddControl(Control c, double left, double top)//给桌面增加图标
        {
            Desktop.Children.Add(c);
            c.SetValue(Canvas.TopProperty, 100d);
            c.SetValue(Canvas.LeftProperty, 100d);
            c.PointerPressed += new PointerEventHandler(t_PointerPressed);
            c.PointerReleased += new PointerEventHandler(t_PointerReleased);
            c.PointerMoved += new PointerEventHandler(t_PointerMoved);

        }

        bool isMouseCaptured;
        public double mouseVerticalPosition;//中间的
        public double mouseHorizontalPosition;

        public double LynxMinHeight = 36d;
        public double LynxMinWeith = 37d;


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //EduSilverlightClassLibrary.SLUser u = new EduSilverlightClassLibrary.SLUser();
            //LayoutRoot.Children.Add(u);
            //Login l = new Login();
            //DeskTop.Children.Add(l);
        }

        void t_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            ILynxControl ic = sender as ILynxControl;
            ILynxWindow iw = sender as ILynxWindow;
            double deltaV = e.GetCurrentPoint(null).Position.Y - mouseVerticalPosition;
            double deltaH = e.GetCurrentPoint(null).Position.X - mouseHorizontalPosition;
            if (item == null) { return; }
            if (!isMouseCaptured) { return; }
            if (ic == null) { return; }
            if (ic.IsMove)
            {

                moveControl(item, deltaV, deltaH);
            }
            if (iw != null)
            {
                if (iw.IsTopChange)
                {

                    // Calculate the current position of the object.
                    TResizeReg(item, deltaV, deltaH);
                    // Update position global variables.
                }
                if (iw.IsBottomChange)
                {

                    // Calculate the current position of the object.
                    BResizeReg(item, deltaV, deltaH);
                    // Update position global variables.
                }
                if (iw.IsLeftChange)
                {

                    // Calculate the current position of the object.
                    LResizeReg(item, deltaV, deltaH);
                    // Update position global variables.
                }
                if (iw.IsRightChange)
                {

                    // Calculate the current position of the object.
                    RResizeReg(item, deltaV, deltaH);
                    // Update position global variables.
                }
                if (iw.IsMove)
                {

                    moveControl(item, deltaV, deltaH);
                }
            }
            mouseVerticalPosition = e.GetCurrentPoint(null).Position.Y;
            mouseHorizontalPosition = e.GetCurrentPoint(null).Position.X;

        }

        void t_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            ILynxControl ic = sender as ILynxControl;
            ILynxWindow iw = sender as ILynxWindow;

            //LynxWindow item = sender as LynxWindow;
            isMouseCaptured = false;

            mouseVerticalPosition = -1;
            mouseHorizontalPosition = -1;
            if (ic != null) { ic.setClear(); }
            if (iw != null) { iw.setClear(); }
        }

        void t_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FrameworkElement item = sender as FrameworkElement;
            ILynxControl ic = sender as ILynxControl;
            ILynxWindow iw = sender as ILynxWindow;
            mouseVerticalPosition = e.GetCurrentPoint(null).Position.Y;
            mouseHorizontalPosition = e.GetCurrentPoint(null).Position.X;
            Point mp = e.GetCurrentPoint(null).Position;
            if (ic != null)
            {
                int x = ic.getPositionStatus(mp.X, mp.Y);

                isMouseCaptured = true;

            }
            if (iw != null)
            {
                int x = iw.getPositionStatus(mp.X, mp.Y);

                isMouseCaptured = true;

            }
        }

        void at_PointerMoved(object sender, PointerRoutedEventArgs e)
        {

            FrameworkElement item = sender as FrameworkElement;

            IMove im = sender as IMove;


        }

        void at_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

            //FrameworkElement item = sender as FrameworkElement;

            IMove im = sender as IMove;

        }

        void at_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //FrameworkElement item = sender as FrameworkElement;

            IMove im = sender as IMove;
            //im.moveAction.MoveArea_PointerPressed(sender, e);
            //mouseVerticalPosition = e.GetPosition(null).Y;
            //mouseHorizontalPosition = e.GetPosition(null).X;
            //Point mp = e.GetPosition(this);
            //if (im != null)
            //{
            //    int x = im.moveAction.getPositionStatus(item,mp.X, mp.Y);

            //    isMouseCaptured = true;
            //    item.CaptureMouse();
            //}

        }

        public int MaxZIndex = 0;
        public void moveControl(FrameworkElement ew, double deltaV, double deltaH)
        {
            double newTop = deltaV + (double)ew.GetValue(Canvas.TopProperty);
            double newLeft = deltaH + (double)ew.GetValue(Canvas.LeftProperty);

            ew.SetValue(Canvas.TopProperty, newTop);
            ew.SetValue(Canvas.LeftProperty, newLeft);
            ew.SetValue(Canvas.ZIndexProperty, MaxZIndex++);

        }

        public void BResizeReg(FrameworkElement ew, double deltaV, double deltaH)
        {
            double newTop = (double)ew.GetValue(Canvas.TopProperty);
            double newHeight = (double)ew.Height + deltaV;
            if (newTop < 0) { return; }
            if (newHeight < LynxMinHeight) { return; }
            ew.Height = newHeight;
            ew.SetValue(Canvas.ZIndexProperty, MaxZIndex++);

        }

        public void TResizeReg(FrameworkElement ew, double deltaV, double deltaH)
        {
            double newTop = deltaV + (double)ew.GetValue(Canvas.TopProperty);
            double newHeight = (double)ew.Height - deltaV;
            if (newTop < 0) { return; }
            if (newHeight < LynxMinHeight) { return; }
            ew.SetValue(Canvas.TopProperty, newTop);
            ew.Height = newHeight;
            ew.SetValue(Canvas.ZIndexProperty, MaxZIndex++);


        }

        public void LResizeReg(FrameworkElement ew, double deltaV, double deltaH)
        {
            double newLeft = deltaH + (double)ew.GetValue(Canvas.LeftProperty);
            double newWidth = (double)ew.Width - deltaH;
            if (newLeft < 0) { return; }
            if (newWidth < LynxMinWeith) { return; }
            ew.SetValue(Canvas.LeftProperty, newLeft);
            ew.Width = newWidth;
            ew.SetValue(Canvas.ZIndexProperty, MaxZIndex++);

        }

        public void RResizeReg(FrameworkElement ew, double deltaV, double deltaH)
        {
            double newLeft = (double)ew.GetValue(Canvas.LeftProperty);
            double newWidth = (double)ew.Width + deltaH;
            if (newLeft < 0) { return; }
            if (newWidth < LynxMinWeith) { return; }
            ew.SetValue(Canvas.LeftProperty, newLeft);
            ew.Width = newWidth;
            ew.SetValue(Canvas.ZIndexProperty, MaxZIndex++);


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LynxWindow t = new LynxWindow();

            Desktop.Children.Add(t);
            t.SetValue(Canvas.TopProperty, 100d);
            t.SetValue(Canvas.LeftProperty, 100d);
            t.PointerPressed += new PointerEventHandler(t_PointerPressed);
            t.PointerReleased += new PointerEventHandler(t_PointerReleased);
            t.PointerMoved += new PointerEventHandler(t_PointerMoved);
        }

        public void AddNewWindow()
        {
            LynxWindow t = new LynxWindow();

            Desktop.Children.Add(t);
            t.SetValue(Canvas.TopProperty, 100d);
            t.SetValue(Canvas.LeftProperty, 100d);
            t.PointerPressed += new PointerEventHandler(t_PointerPressed);
            t.PointerReleased += new PointerEventHandler(t_PointerReleased);
            t.PointerMoved += new PointerEventHandler(t_PointerMoved);

        }

        public void AddNewWindow(LynxWindow lw)
        {

            Desktop.Children.Add(lw);
            lw.SetValue(Canvas.TopProperty, 100d);
            lw.SetValue(Canvas.LeftProperty, 100d);
            lw.PointerPressed += new PointerEventHandler(t_PointerPressed);
            lw.PointerReleased += new PointerEventHandler(t_PointerReleased);
            lw.PointerMoved += new PointerEventHandler(t_PointerMoved);

        }



        public void AddNewLine(Line l)
        {
            Desktop.Children.Add(l);
        }



        private void Desktop_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            xp.Text = e.GetCurrentPoint(this).Position.X.ToString();
            yp.Text = e.GetCurrentPoint(this).Position.Y.ToString();
        }
        ILine l;
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            l = new ILine();
            l.StartPoint = new Point(20, 70);
            l.EndPoint = new Point(200, 80);
            l.Memo = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            l.Draw(Desktop);


        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            l.Move(new Point(300, 900), new Point(300, 400));
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            ActionLineLink a = new ActionLineLink(this.Client);
            //a.at_PointerPressed += new PointerEventHandler(t_PointerPressed);
            //a.PointerReleased += new PointerEventHandler(t_PointerReleased);
            //a.PointerMoved += new PointerEventHandler(t_PointerMoved);
            //a.ParentPanel = Desktop;
            //PointerPressed += new PointerEventHandler(a.at_PointerPressed);
            //PointerReleased += new PointerEventHandler(a.at_PointerReleased);
            PointerMoved += new PointerEventHandler(a.at_PointerMoved);

        }

        public Dictionary<string, object> ReadFromDataRowXML(string s)//从生成的数据表里面恢复数据
        {
            StringReader xs = new StringReader(s);
            XmlReader reader = XmlReader.Create(xs);
            if (!reader.ReadToFollowing("DataRow")) { return null; }

            Dictionary<string, object> rd = new Dictionary<string, object>();
            bool IsNotEnd = reader.Read();
            while (IsNotEnd)//进入当前的对象进行读取，主要是子标签
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (!reader.HasAttributes)
                        {
                            reader.ReadOuterXml();
                        }
                        else
                        {
                            string an, av;

                            reader.MoveToAttribute("name");
                            an = reader.Value;
                            reader.MoveToAttribute("value");
                            av = reader.Value;
                            //reader.MoveToAttribute("type");

                            rd.Add(an, av);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        IsNotEnd = false;
                        break;
                    case XmlNodeType.Text:
                        reader.ReadOuterXml();
                        break;
                    case XmlNodeType.XmlDeclaration:
                        reader.ReadOuterXml();
                        break;

                    case XmlNodeType.ProcessingInstruction:
                        reader.ReadOuterXml();
                        break;
                    case XmlNodeType.Comment:
                        reader.ReadOuterXml();
                        break;
                    case XmlNodeType.Attribute:
                        reader.Read();
                        break;
                    case XmlNodeType.Whitespace:
                        reader.Read();
                        break;
                }
            }

            return rd;
        }

        public List<Dictionary<string, object>> ReadFromDataTableXML(string s)//从生成的数据表里面恢复数据
        {
            List<Dictionary<string, object>> rs = new List<Dictionary<string, object>>();

            StringReader xs = new StringReader(s);
            XmlReader reader = XmlReader.Create(xs);
            if (!reader.ReadToFollowing("DataTable")) { return null; }//找到该类的标签，如果没有，那么没法加载，返回空
            //如果找到了该类的标签，那么就到该类的第一个元素

            bool IsNotEnd = reader.Read();
            while (IsNotEnd)//进入当前的对象进行读取，主要是子标签
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:

                        string tag = reader.Name;
                        if (tag == "DataRow")
                        {
                            string ss = reader.ReadOuterXml();
                            Dictionary<string, object> td = ReadFromDataRowXML(ss);
                            if (td == null) { }
                            else
                            {
                                rs.Add(td);
                            }
                        }
                        break;
                    case XmlNodeType.Text:
                        reader.ReadOuterXml();
                        break;
                    case XmlNodeType.XmlDeclaration:
                        reader.ReadOuterXml();
                        break;

                    case XmlNodeType.ProcessingInstruction:
                        reader.ReadOuterXml();
                        break;
                    case XmlNodeType.Comment:
                        reader.ReadOuterXml();
                        break;
                    case XmlNodeType.Attribute:
                        reader.Read();
                        break;
                    case XmlNodeType.Whitespace:
                        reader.Read();
                        break;

                    case XmlNodeType.EndElement:
                        IsNotEnd = false;
                        break;
                }
            }
            return rs;
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            List<Dictionary<string, object>> d = ReadFromDataTableXML("");
            //stackPanel1.Children.Clear();
            foreach (Dictionary<string, object> xr in d)
            {
                Button b = new Button();
                b.Content = xr["DC_PublicDate"];
                // stackPanel1.Children.Add(b);
            }
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            //common.DC d = new SilverlightLFC.common.DC();
            //d.LoadDataBaseValue("111");
            //button11.Content = d.Name;
            //for (int i = 0; i < 2100000000; i++)
            //{

            //}
            //button11.Content = "ccc";
        }

        public void Zoom(Panel p, double k)
        {
            foreach (UIElement u in p.Children)
            {
                if (u.GetType().GetTypeInfo().IsSubclassOf(typeof(FrameworkElement)))
                {
                    ((FrameworkElement)u).Width = ((FrameworkElement)u).Width * k;
                    ((FrameworkElement)u).Height = ((FrameworkElement)u).Height * k;
                }
            }
            p.Width = p.Width * k;
            p.Height = p.Height * k;
        }

        private void MoveUp_Click(object sender, RoutedEventArgs e)
        {
            Client.SetValue(Canvas.TopProperty, Convert.ToDouble(Client.GetValue(Canvas.TopProperty)) - 5d);
        }

        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            Client.SetValue(Canvas.LeftProperty, Convert.ToDouble(Client.GetValue(Canvas.LeftProperty)) - 5d);
        }

        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            Client.SetValue(Canvas.LeftProperty, Convert.ToDouble(Client.GetValue(Canvas.LeftProperty)) + 5d);

        }

        private void MoveDown_Click(object sender, RoutedEventArgs e)
        {
            Client.SetValue(Canvas.TopProperty, Convert.ToDouble(Client.GetValue(Canvas.TopProperty)) + 5d);

        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            Zoom(Client, 1.05);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            Zoom(Client, 0.95);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Client.Children.Clear();
        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
