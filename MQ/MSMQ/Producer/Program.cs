using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MSMQCommon;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            MSMQHelper.SetClientQueuePath("FormatName:Direct=TCP:xxx.xxx.xxx.xxx\\private$\\Test");
            MSMQHelper.CreateQueue("Test");
            Console.WriteLine("The Queue is running");
            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    MSMQHelper.SendMessage("Mes:" + i + " over");
                    Thread.Sleep(100);
                }
                Thread.Sleep(1000);
            }
        }
    }
}
