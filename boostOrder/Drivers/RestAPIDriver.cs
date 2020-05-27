using boostOrder.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace boostOrder.Drivers
{
    public class RestAPIDriver
    {
        private String clientURL;
        private String requestPath;
        private String username;
        private String password;
        private RestRequest request;
        private int totalPage;

        public RestAPIDriver(string clientURL, string requestPath, string username, string password)
        {
            this.clientURL = clientURL;
            this.requestPath = requestPath;
            this.username = username;
            this.password = password;
        }

        public RestClient SetUpClient()
        {
            var client = new RestClient(clientURL)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
            return client;
        }

        public IRestResponse EstCon()
        {
            var client = SetUpClient();
            request = new RestRequest(requestPath, Method.GET);
            var response = client.Get(request);

            if (response != null)
            {
                totalPage = Convert.ToInt32(response.Headers
               .Where(x => x.Name == "X-WP-TotalPages")
               .Select(x => x.Value)
               .FirstOrDefault());

            }
            return response;
        }


        public void FetchAllPage()
        {
            int i;
            for (i = 1; i <=totalPage; i++)
            {
                var client = SetUpClient();
                request = new RestRequest(requestPath + "page?=" + i, Method.GET);
                var response = client.Get(request);
            }
            //Ideas: Make response [] then deserial each index then store all data into one IList
        }



        public IList<Product> DeserializeLiveProduct(IRestResponse response)
        {

            JArray jArray = JArray.Parse(response.Content);
            IList<Product> productList = jArray.Select(p => new Product
            {

                ProductName = (string)p["name"],
                ProductCode = (string)p["sku"],
                AttributesHolder = p["default_attributes"],
                VariationHolders = p["variations"],
                ImagePath = (string)p["images"].Last["src"]

            }).ToList();

            int i;
            for (i = 0; i < productList.Count(); i++)
            {
                if (productList[i].AttributesHolder.HasValues && productList[i].VariationHolders.Last.HasValues)
                {
                    productList[i].Uom = productList[i].AttributesHolder.Last["option"].ToString().ToUpper();
                    productList[i].ProductPrice = Math.Round(Double.Parse(productList[i].VariationHolders.Last["regular_price"].ToString()),2);
                }
                else
                {
                    //Can Introduce Null value handling here
                    productList[i].Uom = "Promo";
                    productList[i].ProductPrice = 0.99;
                }

                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(productList[i].ImagePath, UriKind.RelativeOrAbsolute);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                productList[i].ProductImage = image;

            }
            //Store productList into DB (Caling DB Driver)
            DatabaseDriver databaseDriver = new DatabaseDriver();
            databaseDriver.StoreProductDB(productList);

            return productList;

        }
    }
}
