﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCFCommon;

namespace WcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsInsperctor();
            //Run();
            System.Console.ReadKey();
        }
        static void Run()
        {
            MyWCF myWCF = new MyWCF();
            ITest channel = (ITest)myWCF.GetWcfChannel<ITest>("127.0.0.1", 6666);
            for (int i = 0; i < 10; i++)
            {
                channel.Fun1("Hi:" + i.ToString());
                Thread.Sleep(1000);
            }
        }

        static void RunAsInsperctor()
        {
            MyWCF myWCF = new MyWCF();
            ITest channel = (ITest)myWCF.GetWcfChannelByPipe<ITest>("127.0.0.1");
            for (int i = 0; i < 10; i++)
            {
                channel.Fun1("Hi:" + i.ToString());
                Thread.Sleep(1000);
            }
        }
    }
}
