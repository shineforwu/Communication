using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MSMQCommon
{
    public class MSMQHelper
    {
        public static string ServerQueuePath { get; private set; }
        public static string ClientQueuePath { get; private set; }
        static string queueName = "Test";

        static string categoryName = "MSMQ Queue";
        static string counterName = "Messages in Queue";
        static string instanceName = Environment.MachineName + "\\private$\\"+ queueName;


        public static bool SetClientQueuePath(string clientQueuePath)
        {
            ClientQueuePath = clientQueuePath;
            return true;
        }
        /// <summary>
        /// CreateQueue
        /// </summary>
        /// <param name="queuePath">QueuePath</param>
        public static void CreateQueue(string queuePath)
        {
            try
            {
                queueName = queuePath;
                string tempPath= @".\private$\" + queuePath;
                ServerQueuePath = tempPath;
                if (!MessageQueue.Exists(tempPath))
                {
                    MessageQueue.Create(@".\private$\"+ queuePath);
                    MessageQueue myQueue = new MessageQueue(ServerQueuePath);
                    myQueue.SetPermissions("Everyone", MessageQueueAccessRights.FullControl);
                }
                else
                {
                    Console.WriteLine("The Queue:"+queuePath + "exists！");
                }
            }
            catch (MessageQueueException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void SendMessage(string mes, bool isServer = true)
        {
            try
            {
                MessageQueue myQueue;
                if (isServer)
                {
                    myQueue = new MessageQueue(ServerQueuePath);
                }
                else
                {
                    myQueue = new MessageQueue(ClientQueuePath);
                }
                Message myMessage = new Message();
                myMessage.Body = mes;
                myMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                myQueue.Send(myMessage);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void ReceiveMessage(bool isServer = true)
        {
            MessageQueue myQueue;
            if (isServer)
            {
                myQueue = new MessageQueue(ServerQueuePath);
            }
            else
            {
                myQueue = new MessageQueue(ClientQueuePath);
            }
            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            try
            {
               
                Message myMessage = myQueue.Receive();
                string context = (string)myMessage.Body;
                Console.WriteLine("Message Information：" + context);
            }
            catch (MessageQueueException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Get Count only server side
        /// </summary>
        /// <returns></returns>
        public static int GetCount()
        {
            PerformanceCounter singleCount = new PerformanceCounter(categoryName, counterName, instanceName);
            int count = (int)singleCount.NextValue();
            return count;
        }
        
    }
}
