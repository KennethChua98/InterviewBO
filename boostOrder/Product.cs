using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace boostOrder
{
    class Product
    {
        [JsonProperty("Product")]
        private string productName;
        private string productCode;
        private double productPrice;
        private BitmapImage productImage;
        private int qty = 0;
        private string uom;
        private string imagePath;
        private JToken attributesHolder;
        private JToken variationHolders;

        public string ProductName { get => productName; set => productName = value; }
        public string ProductCode { get => productCode; set => productCode = value; }
        public double ProductPrice { get => productPrice; set => productPrice = value; }
        public string Uom { get => uom; set => uom = value; }
        public string ImagePath { get => imagePath; set => imagePath = value; }
        public BitmapImage ProductImage { get => productImage; set => productImage = value; }
        public int Qty { get => qty; set => qty = value; }
        public JToken  VariationHolders { get => variationHolders; set => variationHolders = value; }
        public JToken AttributesHolder { get => attributesHolder; set => attributesHolder = value; }

    }
}

