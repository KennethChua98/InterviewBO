using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace boostOrder
{
    class Cart
    {
        private Product[] products;
        private int qty;
        private double total=0.00;

        public Product[] Products { get => products; set => products = value; }
        public int Qty { get => Qty; set => Qty = value; }

        public double GetTotal()
        {
            int i = 0;
            if (products != null)
            {
                for (i = 0; i < Products.Length; i++)
                { total += Products[i].ProductPrice * Qty; }
               
            }
            return total;
        }

        public void AddProduct(Product product, int qty)
        {
            //insert into database
          

        }




    }


  

  
}


