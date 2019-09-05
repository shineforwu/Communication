using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace WepApiHostSelf
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = new HttpSelfHostConfiguration("http://0.0.0.0:9999");
                config.Routes.MapHttpRoute(
                    "API Default", "api/{controller}/{Action}",
                    new { id = RouteParameter.Optional });
                HttpSelfHostServer server = new HttpSelfHostServer(config);
                config.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());//Add MediaType
                server.OpenAsync().Wait();
                Console.WriteLine("Start Web Api");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
