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
            //Run();
            RunAsInsperctor();
            System.Console.ReadKey();
        }
        static void Run()
        {
            MyWCF myWCF = new MyWCF();
            myWCF.EventPrint += Write;
            ServiceHost host = myWCF.StartWCF("127.0.0.1", 6666,typeof(Test),typeof(ITest));
            System.Console.ReadKey();
        }
        static void RunAsInsperctor()
        {

            MyWCF myWCF = new MyWCF();
            myWCF.EventPrint += Write;
            ServiceHost host = myWCF.StartWCFByPipe("127.0.0.1", typeof(Test), typeof(ITest));
            
        }



        public static void Write(string str)
        {
            System.Console.WriteLine(str);
        }
    }
}
