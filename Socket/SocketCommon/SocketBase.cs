using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SocketCommon
{
    public class SocketBase
    {
        #region member
        protected TcpClient tcpClient = new TcpClient();
        protected TcpListener tcpListener = null;//short
        protected TcpListener tcpListener2 = null;//long 
        protected NetworkStream stream;
        public string RemoteEndPointIP { get; set; } = "127.0.0.1";
        public int ShortMesPort { get; set; } = 6666;
        public int LongMesPort { get; set; } = 7777;
        public string Tag { get; set; }
        public int TTL { get; set; } = 3;//when communication fail try times
        public int WaitingTime { get; set; } = 1 * 1000;//when communication fail try times
        public string SplitStr { get; set; } = @"~~~";
        #endregion member


        #region method
        public delegate void DelegatePrint(string message);
        public event DelegatePrint EventPrint;


        public void Print(string str)
        {
            Console.WriteLine(str);
        }

        #region Receive
        public void StartReceiveShortMessage(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            byte[] splitByte = Encoding.UTF8.GetBytes(SplitStr);
            while (true)
            {
                try
                {
                    Socket client = tcpListener.AcceptSocket();
                    int ttl = TTL;
                    byte[] buf = new byte[0];
                    int offset;
                    while (client.Available > 0 && ttl > 0)
                    {
                        int available = client.Available;
                        byte[] array = new byte[available];
                        client.Receive(array, 0, array.Length, SocketFlags.None);
                        offset = buf.Length;
                        buf = new byte[buf.Length + array.Length];
                        Array.Copy(array, 0, buf, offset, array.Length);
                        Thread.Sleep(WaitingTime);
                        ttl--;
                    }
                    byte[] temp = new byte[splitByte.Length];
                    if (temp.Length < buf.Length)
                    {
                        Array.Copy(buf, buf.Length - splitByte.Length, temp, 0, temp.Length);
                        bool canParse = true;
                        for (int i = 0; i < temp.Length; i++)
                        {
                            if (temp[i] != splitByte[i])
                            {
                                canParse = false;
                                break;
                            }
                        }
                        if (canParse)
                        {
                            byte[] byteType = BitConverter.GetBytes((int)CommunicationType.HeartbeatSend);// 4byte
                            Array.Copy(buf, 0, byteType, 0, byteType.Length);
                            byte[] conB = new byte[buf.Length - byteType.Length];
                            Array.Copy(buf, byteType.Length, conB, 0, conB.Length);
                            CommunicationType type = (CommunicationType)BitConverter.ToInt32(byteType, 0);
                            string data = Encoding.UTF8.GetString(conB);
                            Regex regex = new Regex(SplitStr);
                            string content = regex.Split(data)[1];
                            ParseData(client, type, content);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventPrint(ex.Message);
                }


            }
        }

        protected virtual void ParseData(Socket socket, CommunicationType type, string content)
        {
            switch (type)
            {
                case CommunicationType.HeartbeatSend:
                    break;
            }
        }
        #endregion Receive

        #region Send
        protected TcpClient ConnectSocket(string addr, int port)
        {
            try
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                }
                tcpClient = new TcpClient();
                IPAddress ip = IPAddress.Parse(addr);
                tcpClient.Connect(ip, port);
                return tcpClient;
            }
            catch (Exception ex)
            {
                EventPrint(ex.Message);
                return null;
            }
        }

        protected bool SendMessage(CommunicationType communicationType, string content, int ttl = 3)
        {
            if (ttl < 0)
            {
                return false;
            }
            byte[] type = BitConverter.GetBytes((int)communicationType);
            string data = SplitStr + content + SplitStr;
            byte[] buf = Encoding.UTF8.GetBytes(data);
            try
            {
                //if (tcpClient != null)
                //{
                //    tcpClient.Close();
                //    tcpClient = new TcpClient();
                //}
                tcpClient = ConnectSocket(RemoteEndPointIP, ShortMesPort);
                //tcpClient.Client.Send(buf);
                tcpClient.GetStream().Write(type, 0, type.Length);
                tcpClient.GetStream().Write(buf, 0, buf.Length);
                tcpClient.GetStream().Flush();
                //tc.Close();
                return true;
            }
            catch (Exception ex)
            {
                EventPrint?.Invoke(ex.Message);
                return SendMessage(communicationType, content, ttl - 1);

            }
        }
        #endregion Send
        #endregion method
    }
}
