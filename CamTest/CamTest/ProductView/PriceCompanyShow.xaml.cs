using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;


using System.Windows.Input;



using SilverlightLynxControls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace SLPhotoTest
{
    public partial class PriceCompanyShow : UserControl,IMove
    {
        public PriceCompanyShow()
        {
            InitializeComponent();
            am = new ActionMove(this, TitleArea);
        }

        List<Company> comList = new List<Company>();
        List<Product> proList = new List<Product>();

        public void InitCompanyList()
        {

        }

        public double MinPrice, MaxPrice;
        public double getY(Product po)
        {
            return this.Height - (po.Price - MinPrice) / (MaxPrice - MinPrice) * this.Height;
        }

        public double getX(Product po)
        {
            return 0;
            //return this.Height - (po.PublcTime. - MinTime) / (MaxPrice - MinPrice) * this.Height;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Panel p = this.Parent as Panel;
            if (p != null) { p.Children.Remove(this); }
        }

        #region IMove Members

        private ActionMove am;
        public ActionMove moveAction
        {
            get { return am; }
        }

        #endregion
    }
}
