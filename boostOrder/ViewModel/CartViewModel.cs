using boostOrder.Drivers;
using boostOrder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace boostOrder.ViewModel
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            LoadCart();
        }

        public ObservableCollection<Cart> Cart { get; set; }

        public void LoadCart()
        {
            int i;
            DatabaseDriver databaseDriver = new DatabaseDriver();
            Cart = new ObservableCollection<Cart>();
            var cart=databaseDriver.GetCartDB();

            for (i = 0; i < cart.Count; i++)
            {
                Cart.Add(cart[i]);
            }

        }
    }
}
