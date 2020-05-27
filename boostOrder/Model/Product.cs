using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace boostOrder.Model
{
    public class Product : INotifyPropertyChanged
    {
        public class ProductModel {}
        private string productCode;
        private string productName;
        private double productPrice;
        private BitmapImage productImage;
        private int qty=0;
        private string uom;
        private string imagePath;
        private JToken attributesHolder;
        private JToken variationHolders;

        public string ProductName 
        {
            get 
            { 
                return productName; 
            }
            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
            }
        }
        public string ProductCode
        {
            get
            {
                return productCode;
            }
            set
            {
                productCode = value;
                OnPropertyChanged("ProductCode");
            }
        }
        public double ProductPrice
        {
            get
            {
                return productPrice;
            }
            set
            {
                productPrice = value;
                OnPropertyChanged("ProductPrice");
            }
        }
        public string Uom
        {
            get
            {
                return uom;
            }
            set
            {
                uom = value;
                OnPropertyChanged("Uom");
            }
        }
        public BitmapImage ProductImage
        {
            get
            {
                return productImage;
            }
            set
            {
                productImage = value;
                OnPropertyChanged("ProductImage");
            }
        }
        public int Qty
        {
            get
            {
                return qty;
            }
            set
            {
                qty = value;
                OnPropertyChanged("Qty");
            }
        }

        public JToken VariationHolders { get => variationHolders; set => variationHolders = value; }
        public JToken AttributesHolder { get => attributesHolder; set => attributesHolder = value; }
        public string ImagePath { get => imagePath; set => imagePath = value; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}

