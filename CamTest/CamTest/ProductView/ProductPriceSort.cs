using System;
using System.Net;
using System.Windows;



using System.Windows.Input;



using System.Collections.Generic;

namespace SLPhotoTest
{
    public class ProductPriceSort : IComparer<Product>
    {

        #region IComparer<Product> Members

        public int Compare(Product x, Product y)
        {
            double xx = x.Price - y.Price;
            if (xx > 0) { return 1; }
            if (xx == 0) { return 0; }
            return -1;
        }

        #endregion
    }
}
