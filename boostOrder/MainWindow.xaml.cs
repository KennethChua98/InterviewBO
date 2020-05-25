using boostOrder.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace boostOrder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string clientURL = "https://mangomart-autocount.myboostorder.com/wp-json/wc/v1";
        string requestPath = "/products";
        string username= "ck_2682b35c4d9a8b6b6effac126ac552e0bfb315a0";
        string password = "cs_cab8c9a729dfb49c50ce801a9ea41b577c00ad71";

        public MainWindow()
        {
            IList<Product> productList= new List<Product>();
            Product defaultProd= (new Product { ProductName = "DEFAULT_Name", ProductCode = "DEFAULT", ProductPrice = 999.99, Uom = "DEFAULT", Qty = 0 });
            productList.Add(defaultProd);
                //
            RestAPIDriver restAPI = new RestAPIDriver(clientURL, requestPath, username, password);
            var response=restAPI.EstCon();
            if (response.IsSuccessful)
            {
                productList = restAPI.getProductContent(response);
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("This application is in Offline Mode", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Exclamation)),
               DispatcherPriority.Render);
                //launch DB here


            }
            InitializeComponent();
            this.DataContext = productList;
            catalogView.ItemsSource = productList;



        }

        
   

    }
}

      


