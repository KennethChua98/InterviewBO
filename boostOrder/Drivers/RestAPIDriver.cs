using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Media.Imaging;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace boostOrder.Drivers
{
    class RestAPIDriver
    {
        private String clientURL;
        private String requestPath;
        private String username;
        private String password;
        private RestRequest request;
        private int totalPage;

        public RestAPIDriver(string clientURL, string requestPath ,string username, string password)
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
            var client=SetUpClient();
            request = new RestRequest(requestPath, Method.HEAD);
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

        public void GoThisPage(int page)
        {
            var client = SetUpClient();
            request = new RestRequest(requestPath+"page?="+page, Method.GET);
            var response = client.Get(request);
        }

        public IList<Product> getProductContent(IRestResponse response)
        {
            JArray jArray = JArray.Parse(response.Content);
            IList<Product> productList = jArray.Select(p => new Product 
            {
                
                ProductName = (string)p["name"],
                ProductCode = (string)p["sku"],
                AttributesHolder = p["default_attributes"],
                VariationHolders= p["variations"],
                ImagePath = (string)p["images"].Last["src"]
                
            }).ToList();

            int i;
            for (i=0;i<productList.Count();i++)
            { 
            
                if (productList[i].AttributesHolder.HasValues && productList[i].VariationHolders.Last.HasValues)
                 {
                    productList[i].Uom = productList[i].AttributesHolder.Last["option"].ToString().ToUpper();
                    productList[i].ProductPrice = Double.Parse(productList[i].VariationHolders.Last["regular_price"].ToString());
                }
                else
                {
                    //Can Introduce Null value handling here
                    productList[i].Uom = "N/A";
                    productList[i].ProductPrice =0;
                }

                productList[i].ProductImage = new BitmapImage();
                productList[i].ProductImage.BeginInit();
                productList[i].ProductImage.UriSource = new Uri(productList[i].ImagePath, UriKind.RelativeOrAbsolute);
                productList[i].ProductImage.CacheOption = BitmapCacheOption.OnLoad;
                productList[i].ProductImage.EndInit();

            }
            //Store productList into DB
            DatabaseDriver databaseDriver = new DatabaseDriver();
            databaseDriver.StoreProduct(productList);

            return productList;
          
        }
    }
}
