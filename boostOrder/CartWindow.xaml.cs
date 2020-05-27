using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace boostOrder
{
    /// <summary>
    /// Interaction logic for CartWindow.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        public CartWindow()
        {
            InitializeComponent();
        }
        public void CartViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            boostOrder.ViewModel.CartViewModel cartViewModelObject =
             new boostOrder.ViewModel.CartViewModel();
            cartViewModelObject.LoadCart();
            CartViewControl.DataContext = cartViewModelObject;

        }
    }
}
