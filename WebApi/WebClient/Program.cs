using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Download start");
            //WebApiDownLoad(@"./Test.txt", Guid.NewGuid().ToString() + ".txt");
            RunAsync(@"./Test.txt", Guid.NewGuid().ToString() + ".txt");
            Console.WriteLine("Download over");
            Console.ReadKey();
        }
        private static void WebApiDownLoad(string fliePath, string savePath)
        {
            string url = "http://" +"127.0.0.1" + ":" + 9999 + "/api/Test/Download?filePath=" + fliePath;
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html,application/xhtml+xml,*/*";
            request.ContentType = "application/json";
            request.Method = "GET";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response != null)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            StreamWriter sw = new StreamWriter(fs);
                            int temp = reader.Read();
                            while (-1 != temp)
                            {
                                sw.Write(Convert.ToChar(temp));
                                temp = reader.Read();
                            }

                            sw.Flush();
                            sw.Close();
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Run WebApiDownLoad error:" + ex.Message);
            }
            finally
            {
                
            }

        }

        static async Task RunAsync(string fliePath, string savePath)
        {
            using (var client = new HttpClient())
            {
                string url = "http://" + "127.0.0.1" + ":" + 9999 + "/api/Test/Download2?filePath=" + fliePath;
                HttpResponseMessage response = await client.GetAsync(url);//非阻塞
                //HttpResponseMessage response = client.GetAsync(url).Result; //阻塞
                string responseBody = await response.Content.ReadAsStringAsync();
                using (FileStream fs = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(responseBody);
                    sw.Flush();
                    sw.Close();
                }
            }

        }

    }
}
