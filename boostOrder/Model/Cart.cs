using boostOrder.Model;
using System.ComponentModel;

namespace boostOrder
{
    public class Cart : INotifyPropertyChanged
    {
        public class CartModel {}

        private Product product;

        public Product Product
        {
            get
            {
                return product;
            }
            set
            {
                product = value;
                OnPropertyChanged("Product");
            }
        }
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


