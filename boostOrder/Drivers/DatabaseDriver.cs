using boostOrder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;

namespace boostOrder.Drivers
{

    public class DatabaseDriver
    {
        SqlConnection conn;
        public SqlConnection EstDbConn()
        {
            SqlConnection conn = new SqlConnection();
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = path + "MallDB.mdf";
            conn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + projectDirectory + ";Integrated Security=True";
            return conn;
        }

        public IList<Product> GetProductDB()
        { 
            conn = EstDbConn();
            conn.Open();
            string queryString = "SELECT * FROM dbo.Product";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            SqlDataReader dataReader = cmd.ExecuteReader();
            IList<Product> productList = new List<Product>();
           

            int i = 0;
            while (dataReader.Read())
            {
                Product product = new Product();
                product.ProductCode = dataReader.GetString(0);
                product.ProductName = dataReader.GetString(1);
                product.ProductPrice = (double)dataReader.GetDecimal(2);

                var image = (byte[])dataReader.GetSqlBinary(3);

                product.ProductImage = ConvertByteToImage(image);

                product.Uom = dataReader.GetString(4);
                i ++;
                productList.Add(product);
            }
            conn.Close();
            return productList;
           
        }

        public IList<Cart> GetCartDB()
        {
            conn = EstDbConn();
            conn.Open();
            string queryString = "SELECT c.ProductCode, p.ProductName, p.ProductPrice, c.Qty FROM dbo.Cart c, dbo.Product p where c.ProductCode=p.ProductCode;";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            SqlDataReader dataReader = cmd.ExecuteReader();

            IList<Cart> cartList = new List<Cart>();


            while (dataReader.Read())
            {
                Cart cartItem = new Cart();
                cartItem.Product = new Product();
                cartItem.Product.ProductCode = dataReader.GetString(0);
                cartItem.Product.ProductName = dataReader.GetString(1);
                cartItem.Product.ProductPrice = (double)dataReader.GetDecimal(2);
                cartItem.Product.Qty = dataReader.GetInt32(3);
                cartList.Add(cartItem);
            }
            conn.Close();
            return cartList;
           
        }

        public void StoreProductDB(IList<Product> productsList)
        {
            conn = EstDbConn();
            conn.Open();
            try
            {
                int i;
 
                for (i = 0; i < productsList.Count; i++)
                {
                    
                    string queryString = "Insert into dbo.Product Values(@ProductCode,@ProductName,@ProductPrice,@ProductImage,@Uom);";
                    SqlCommand cmd = new SqlCommand(queryString, conn);
                    cmd.Parameters.AddWithValue("@ProductCode", productsList[i].ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", productsList[i].ProductName);
                    cmd.Parameters.AddWithValue("@ProductPrice", productsList[i].ProductPrice);
                    cmd.Parameters.AddWithValue("@ProductImage", ConvertImageToByte(productsList[i].ImagePath));
                    cmd.Parameters.AddWithValue("Uom", productsList[i].Uom);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("storeError:" + ex);
            }
            conn.Close();
        }

        public void StoreProductCart(Product product)
        {
            int i, addedQty=0;
            Boolean existed=false;
            var cartList = GetCartDB();

            if(cartList.Count>0)
            {
                for (i = 0; i < cartList.Count; i++)
                {
                    if (cartList[i].Product.ProductCode.Equals(product.ProductCode))
                    {
                        existed = true;
                        addedQty= cartList[i].Product.Qty+product.Qty;
                    }
                }
                if (existed == false)
                {
                    StoreProdCartStmReuse(product);
                }
                else
                {
                    UpdateCrtProdExisted(product, addedQty);
                }
            }
            else
            {
                StoreProdCartStmReuse(product);
            }
        }

        private void StoreProdCartStmReuse(Product product)
        {
            conn = EstDbConn();
            conn.Open();
            string queryString = "Insert into dbo.Cart Values(@ProductCode, @Qty);";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
            cmd.Parameters.AddWithValue("@Qty", product.Qty);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void UpdateCrtProdExisted(Product product, int addedQty)
        {
            conn = EstDbConn();
            conn.Open();
            string queryString = "Update dbo.Cart set Qty=@Qty where ProductCode=@ProductCode;";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
            cmd.Parameters.AddWithValue("@Qty", addedQty);
            cmd.ExecuteNonQuery();
            conn.Close();

        }

        public void FreeCartStorage()
        {
            conn = EstDbConn();
            conn.Open();
            string queryString = "Delete FROM dbo.Cart";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.ExecuteReader();
            conn.Close();

        }

        public void FreeCatalogStorage()
        {
            conn = EstDbConn();
            conn.Open();
            string queryString = "Delete FROM dbo.Product";
            SqlCommand cmd = new SqlCommand(queryString, conn);
            cmd.ExecuteReader();
            conn.Close();

        }

        public BitmapImage ConvertByteToImage(Byte[] array)
        {
            using var ms = new System.IO.MemoryStream(array);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad; // here
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public static Byte[] ConvertImageToByte(string imagePath)
        {
            using WebClient client = new WebClient();
            var Byte = client.DownloadData(imagePath);
            return Byte;

        }
    }
}
