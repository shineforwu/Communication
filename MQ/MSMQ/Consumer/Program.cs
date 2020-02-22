using MSMQCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            MSMQHelper.SetClientQueuePath("FormatName:Direct=TCP:192.168.1.4\\private$\\Test");
            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    MSMQHelper.ReceiveMessage(false);
                    Thread.Sleep(100);
                }
                Thread.Sleep(1000);
            }
        }
    }
}
