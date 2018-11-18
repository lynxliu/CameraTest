using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;




namespace StoryDesignControls.ProjectUI
{
    public partial class LynxRecordControl : UserControl
    {//用来列表和编辑记录，要求记录都是字符串类型
        public LynxRecordControl()
        {
            InitializeComponent();
        }
        List<string> RecordList=new List<string>();
        //string SelectItem;



        public double controlWidth
        {
            get { return Width; }
            set
            {
                if (value < 50) { return; }//最小是50
                Width = value;
                LayoutRoot.Width = value;

            }
        }

        public double controlHeight
        {
            get { return Height; }
            set
            {
                if (value < 50) { return; }//最小是50
                Height = value;
                LayoutRoot.Height = value;

            }
        }

        public void setName(string name)
        {
            textBlockName.Text = name;
        }
        public void ReadValue(List<string> rl)
        {
            stackRecordList.Children.Clear();
            RecordList = new List<string>(rl);
            foreach (string s in rl)
            {
                Button b = new Button();
                b.Content = s;
                b.Click += new RoutedEventHandler(b_Click);
                b.Width = 305;
                b.Height = 21;
                ToolTipService.SetToolTip(b, s);
                stackRecordList.Children.Add(b);

            }
            textEditRecord.Text = "";
            textEditRecord.Tag = null;
        }

        public List<string> getRecordList()
        {
            return RecordList;
        }

        void b_Click(object sender, RoutedEventArgs e)
        {
            string rs = (sender as Button).Content.ToString();
            int ci = rs.IndexOf("；");
            string ds = rs.Substring(0, ci+1);//时间部分
            rs = rs.Substring(ci + 1);
            textEditRecord.Text = rs;
            ToolTipService.SetToolTip(textEditRecord, ds);
            textEditRecord.Tag = (sender as Button);
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (textEditRecord.Text != "")
            {
                string s =DateTime.Now.ToString("{0:G}")+ "；"+ textEditRecord.Text;
                Button b = new Button();
                b.Content = s;
                b.Click += new RoutedEventHandler(b_Click);
                b.Width = 305;
                b.Height = 21;
                ToolTipService.SetToolTip(b, s);
                stackRecordList.Children.Insert(0,b);
                RecordList.Insert(0, s);
                textEditRecord.Tag = b;
            }
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (textEditRecord.Tag == null) { return; }
            Button b = textEditRecord.Tag as Button;
            if (stackRecordList.Children.Contains(b))
            {
                stackRecordList.Children.Remove(b);
                RecordList.Remove(b.Content.ToString());
            }
            textEditRecord.Tag = null;
            textEditRecord.Text = "";
        }
    }
}
