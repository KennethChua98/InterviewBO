using boostOrder.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Linq;

namespace boostOrder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        private void ProductViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            boostOrder.ViewModel.ProductViewModel productViewModelObject =
               new boostOrder.ViewModel.ProductViewModel();
            productViewModelObject.LoadProducts();
            ProductViewControl.DataContext = productViewModelObject;
        }
    }
}





