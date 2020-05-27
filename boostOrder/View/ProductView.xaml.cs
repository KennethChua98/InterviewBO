using boostOrder.Drivers;
using boostOrder.Model;
using boostOrder.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace boostOrder.View
{
    /// <summary>
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : UserControl
    {
        public ProductView()
        {
            InitializeComponent();
          
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {

            var product=(Product) listView.SelectedItem;
            product.Qty +=1;

        }

        private void Button_Click_Minus(object sender, RoutedEventArgs e)
        {
            var product = (Product)listView.SelectedItem;
            if (product.Qty > 0)
            {
                product.Qty -= 1;
            }
        }

        private void Button_Click_Cart(object sender, RoutedEventArgs e)
        {
            var product = (Product)listView.SelectedItem;

            if (product.Qty>0)
            {
                DatabaseDriver databaseDriver = new DatabaseDriver();
                
                databaseDriver.StoreProductCart(product);
                System.Windows.MessageBox.Show(product.Qty+" item(s) has been added into the cart");

            }
            else System.Windows.MessageBox.Show("Minimum Qty to be added is 1");

        }
    }
}
