using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace boostOrder.Drivers
{
    class DatabaseDriver
    {
        SqlConnection conn;
        public SqlConnection EstDbConn()
        {

            SqlConnection conn = new SqlConnection();

            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            string projectDirectory = path + @"\MallDB.mdf";
            Console.WriteLine(projectDirectory);

            conn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + projectDirectory + ";Integrated Security=True";
            Console.WriteLine("DB Connection OK!");
            conn.Open();
            return conn;
        }

        public void getProductDB()
        {
            int i = 0;
            conn = EstDbConn();
            /*

           try
           {
               
               /*
               string queryString = "SELECT * FROM dbo.Product";
               SqlCommand cmd = new SqlCommand(queryString, conn);
               SqlDataReader dataReader = cmd.ExecuteReader();

               while (dataReader.Read())
               {
                   IList<Product> productList = new List<Product>();
                   productList[i].ProductCode = dataReader.GetString(0);
                   productList[i].ProductName = dataReader.GetString(1);
                   productList[i].ProductPrice = dataReader.GetDouble(2);

                   var image = (byte[])dataReader.GetValue(3);
                   productList[i].ProductImage = ConvertByteToImage(image);
                   productList[i].Uom = dataReader.GetString(4);
                   i++;
               }
           }
           catch(Exception ex) {
               Console.WriteLine(ex);

           }
           finally { conn.Close(); }
               */

        }

        public void StoreProduct(IList<Product> productsList)
        {
            conn = EstDbConn();
            try
            {
                int i = 0;
                

                //Console.WriteLine(bytes[0].ToString());

                for (i = 0; i < productsList.Count; i++)
                {

                    string queryString = "Insert into dbo.Product(ProductCode,ProductName,ProductPrice,ProductImage,Uom) Values (@ProductCode,@ProductName,@ProductPrice,@ProductImage,@Uom);";
                    SqlCommand cmd = new SqlCommand(queryString, conn);
                    cmd.Parameters.AddWithValue("@ProductCode", productsList[i].ProductCode);
                    cmd.Parameters.AddWithValue("@ProductName", productsList[i].ProductName);
                    cmd.Parameters.AddWithValue("@ProductPrice", productsList[i].ProductPrice);
                    var bytes = convertImageToByte(productsList[0].ProductImage);
                    cmd.Parameters.AddWithValue("@ProductImage", bytes);
                    cmd.Parameters.AddWithValue("@Uom", productsList[i].Uom);
                    cmd.ExecuteNonQuery();
                    i++;
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine("storeError:" + ex);


            }

            conn.Close();



        }

        public BitmapImage ConvertByteToImage(Byte[] image)
        {
            MemoryStream stream = new MemoryStream(image);
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            stream.Seek(0, SeekOrigin.Begin);
            bitImage.StreamSource = stream;
            bitImage.EndInit();
            bitImage.EndInit();
            return bitImage;
        }

        public static Byte[] convertImageToByte(BitmapImage image)
        {

            Byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }
    }
}
