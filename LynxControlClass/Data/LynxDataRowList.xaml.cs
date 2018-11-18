using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;




namespace SilverlightLynxControls.Data
{
    public partial class LynxDataRowList : UserControl
    {
        public LynxDataRowList()
        {
            InitializeComponent();
        }
        

        public string selectSQL;
        public int CountPerPage=10;
        public int CurrentPage=1;
        public int TotlePage=-1;
        public int TotleRecords=-1;
        
        public void InitPage()
        {

            SilverlightLFC.common.LFCDataService ls = new SilverlightLFC.common.LFCDataService();
            ls.ProcessQueryComplete += new SilverlightLFC.common.ProcessEventHandler(ls_ProcessQueryComplete);
            //ls.getQueryCount(selectSQL);
        }

        void ls_ProcessQueryComplete(object sender, SilverlightLFC.common.LynxProcessCompleteEventArgs e)
        {
            SilverlightLFC.common.LFCDataService ls = sender as SilverlightLFC.common.LFCDataService;
            ls.ProcessQueryComplete -= new SilverlightLFC.common.ProcessEventHandler(ls_ProcessQueryComplete);

            if (e.IsSuccess)
            {
                TotleRecords = Convert.ToInt32(e.ReturnValue);
                TotlePage = TotleRecords / CountPerPage + 1;
                ShowRowInfor();
            }

        }
        ILynxDataItem TargetItem=null;
        public void setItemControl(ILynxDataItem i)
        {
            TargetItem = i;
        }


        //public void LoadPageRecordList<T>(int cp) where T : UIElement, ILynxDataItem
        //{
        //    CurrentPage = cp;
        //    SilverlightLFC.common.LFCDataService ls = new SilverlightLFC.common.LFCDataService();
        //    ls.ExecuteQuery += new SilverlightLFC.common.ExecuteQueryEventHandler(ls_ExecuteQuery<T>);
        //    ls.AsynchronousExecuteQuery(selectSQL, CurrentPage, CountPerPage);
        //}

        //public void LoadRecordList<T>() where T:UIElement,ILynxDataItem
            public void LoadRecordList()
        {

            SilverlightLFC.common.LFCDataService ls = new SilverlightLFC.common.LFCDataService();
            ls.ExecuteQuery += new SilverlightLFC.common.ExecuteQueryEventHandler(ls_ExecuteQuery);
            //ls.AsynchronousExecuteQuery(selectSQL,CurrentPage,CountPerPage);
        }

        //void ls_ExecuteQuery<T>(object sender, SilverlightLFC.common.LFCExecuteQueryEventArgs e) where T : UIElement, ILynxDataItem
            void ls_ExecuteQuery(object sender, SilverlightLFC.common.LFCExecuteQueryEventArgs e)
        {
            SilverlightLFC.common.LFCDataService ls = sender as SilverlightLFC.common.LFCDataService;
            ls.ExecuteQuery -= new SilverlightLFC.common.ExecuteQueryEventHandler(ls_ExecuteQuery);

            RowList.Children.Clear();
            List<Dictionary<string, object>> d = e.DataTable;
            if (TargetItem == null)
            {
                DefaultShow(d);
                ShowRow();

                return;
            }
            foreach (Dictionary<string, object> r in d)
            {
                Type t = TargetItem.GetType();
                ILynxDataItem ot = (ILynxDataItem)Activator.CreateInstance(t);
                //RowList.Children.Add(ot);
                ot.ShowObject(RowList, r);
            }
            ShowRow();

        }
        public   bool IsEdit = false;

        public ILynxDataEdit ileo;
            void DefaultShow(List<Dictionary<string, object>> d)
            {
                
                RowList.Children.Clear();
                Grid DataGrid = new Grid();
                RowList.Children.Add(DataGrid);

                if (d.Count <= 0) { return; }
                Dictionary<string, object> tc = d[0];
                RowDefinition rd = new RowDefinition();
                DataGrid.RowDefinitions.Add(rd);
                Grid cg = new Grid();
                DataGrid.Children.Add(cg);
                Grid.SetRow(cg, 0);
                int c=0;
                foreach (KeyValuePair<string,object> kv in tc)
                {
                    ColumnDefinition cl = new ColumnDefinition();
                    GridLength gl=new GridLength(35);
                    cl.Width = gl;
                    cg.ColumnDefinitions.Add(cl);
                    Button cb = new Button();
                    cb.Content = kv.Key;
                    cg.Children.Add(cb);
                    Grid.SetColumn(cb, c);
                    Grid.SetRow(cb, 0);

                    c++;
                    
                }
                if (IsEdit)
                {
                    TextBlock tt;
                    DataGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    tt = new TextBlock();
                    tt.Text = "编辑";
                    Grid.SetColumn(tt, c);
                    Grid.SetRow(tt, 0);
                    DataGrid.Children.Add(tt);

                    DataGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    tt = new TextBlock();
                    tt.Text = "新建";
                    Grid.SetColumn(tt, c+1);
                    Grid.SetRow(tt, 0);
                    DataGrid.Children.Add(tt);

                    DataGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    tt = new TextBlock();
                    tt.Text = "删除";
                    Grid.SetColumn(tt, c+2);
                    Grid.SetRow(tt, 0);
                    DataGrid.Children.Add(tt);
                }
                for (int i = 0; i < d.Count; i++)
                {
                    Dictionary<string, object> tr = d[i];
                    RowDefinition trd = new RowDefinition();
                    DataGrid.RowDefinitions.Add(trd);

                    Grid dg = new Grid();
                    dg.PointerPressed += new PointerEventHandler(dg_PointerPressed);
                    //dg.ColumnDefinitions = DataGrid.ColumnDefinitions;
                    DataGrid.Children.Add(dg);
                    Grid.SetRow(dg, i + 1);

                    int j = 0;
                    foreach (KeyValuePair<string, object> kv in tr)
                    {
                        ColumnDefinition cl = new ColumnDefinition();
                        GridLength gl = new GridLength(35);
                        cl.Width = gl;
                        dg.ColumnDefinitions.Add(cl);
                        TextBlock cb = new TextBlock();
                        cb.Text = kv.Value.ToString();
                        dg.Children.Add(cb);
                        Grid.SetColumn(cb, j);
                        
                        Grid.SetRow(cb, 0);

                        j++;
                    }
                    if (IsEdit)
                    {
                        Button eb = new Button();
                        eb.Content = "Edit";
                        eb.Width = 15;
                        eb.Height = 15;
                        eb.Tag = tr;
                        eb.Click += new RoutedEventHandler(eb_Click);
                        Grid.SetColumn(eb, j);

                        Grid.SetRow(eb, i + 1);
                        DataGrid.Children.Add(eb);

                        Button nb = new Button();
                        nb.Content = "New";
                        nb.Width = 15;
                        nb.Height = 15;
                        nb.Tag = tr;
                        nb.Click += new RoutedEventHandler(nb_Click);
                        Grid.SetColumn(nb, j + 1);

                        Grid.SetRow(nb, i + 1);
                        DataGrid.Children.Add(nb);

                        Button db = new Button();
                        db.Content = "Delete";
                        db.Width = 15;
                        db.Height = 15;
                        db.Tag = tr;
                        db.Click += new RoutedEventHandler(db_Click);
                        Grid.SetColumn(db, j + 2);

                        Grid.SetRow(db, i + 1);
                        DataGrid.Children.Add(db);
                    }
                }
            }

 
            void dg_PointerPressed(object sender, PointerRoutedEventArgs e)
            {
                Grid g = sender as Grid;
                g.Background = new SolidColorBrush(Colors.Yellow);
            }

            void eb_Click(object sender, RoutedEventArgs e)
            {
                Button b=sender as Button;
                ileo.EditObject(RowList, (Dictionary<string, object>)b.Tag);
            }
            void nb_Click(object sender, RoutedEventArgs e)
            {
                Button b = sender as Button;
                ileo.CreateObject(RowList, (Dictionary<string, object>)b.Tag);
            }
            void db_Click(object sender, RoutedEventArgs e)
            {
                Button b = sender as Button;
                ileo.DeleteObject(RowList, (Dictionary<string, object>)b.Tag);
            }
            public void ShowRow()
        {
            if (TotleRecords == -1) { InitPage(); }
            else
            {
                ShowRowInfor();
            }
        }

            void ShowRowInfor()
            {
                textBlockInfor.Text = TotleRecords.ToString() + "Records,and " + TotlePage.ToString() + "Pages"; ;
                comboBoxPage.Items.Clear();
                for (int i = 1; i <= TotlePage; i++)
                {
                    ComboBoxItem ci = new ComboBoxItem();
                    ci.Content = i;
                    ci.Tag = i;
                    comboBoxPage.Items.Add(ci);
                    if (i == CurrentPage)
                    {
                        comboBoxPage.SelectedItem = ci;
                    }
                }
            }

        private void buttonFirst_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage=1;
            LoadRecordList();
        }

        private void buttonPrev_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadRecordList();
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage <TotlePage)
            {
                CurrentPage++;
                LoadRecordList();
            }
        }

        private void buttonlast_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = TotlePage;
            LoadRecordList();
        }

        private void comboBoxPage_DropDownClosed(object sender, object e)
        {
            ComboBoxItem ci = comboBoxPage.SelectedItem as ComboBoxItem;
            if (ci != null)
            {
                int t = Convert.ToInt32(ci.Tag);
                if (CurrentPage != t)
                {
                    CurrentPage = t;
                    LoadRecordList();
                }
            }

        }



    }
}
