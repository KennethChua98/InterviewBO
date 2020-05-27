using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows;
using boostOrder.Drivers;
using System.Linq;
using boostOrder.Model;
using System.Windows.Input;

namespace boostOrder.ViewModel
{
    public class ProductViewModel
    {
        private readonly string clientURL = "https://mangomart-autocount.myboostorder.com/wp-json/wc/v1";
        private readonly string requestPath = "/products";
        private readonly string username = "ck_2682b35c4d9a8b6b6effac126ac552e0bfb315a0";
        private readonly string password = "cs_cab8c9a729dfb49c50ce801a9ea41b577c00ad71";

        public ObservableCollection<Product> Products { get; set; }

        public void LoadProducts()
        {
            int i;
            IList<Product> productList = new List<Product>();

            RestAPIDriver restAPI = new RestAPIDriver(clientURL, requestPath, username, password);
            DatabaseDriver databaseDriver = new DatabaseDriver();
            var response = restAPI.EstCon();
            if (response.IsSuccessful)
            {
                databaseDriver.FreeCartStorage();
                databaseDriver.FreeCatalogStorage();
                productList = restAPI.DeserializeLiveProduct(response);
            }
            else
            {
                //launch DB here
                System.Windows.MessageBox.Show("Offline Mode enabled!");
                productList = databaseDriver.GetProductDB();
            }

            IEnumerable<Product> sortedEnum = productList.OrderBy(p => p.ProductName);
            productList = sortedEnum.ToList();

            Products = new ObservableCollection<Product>();

            for (i = 0; i < productList.Count; i++)
            {
                Products.Add(productList[i]);
            }
        }
    }
}



    

