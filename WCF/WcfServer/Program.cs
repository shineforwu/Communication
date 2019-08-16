using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCommon;

namespace WcfServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            MyWCF myWCF = new MyWCF();
            myWCF.EventPrint += Write;

            ServiceHost host = myWCF.StartWCF("127.0.0.1", 6666,typeof(Test),typeof(ITest));
            System.Console.ReadKey();
        }

        public static void Write(string str)
        {
            System.Console.WriteLine(str);
        }
    }
}
