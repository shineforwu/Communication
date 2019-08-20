using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SocketCommon
{
    public class SocketServer : SocketBase
    {
        public DataTable dtClient;
        public event DelegatePrint Show;

        public SocketServer()
        {
            InitDT();
        }
        public void InitDT()
        {
            dtClient = new DataTable();
            dtClient.Columns.Add("ClientIp", System.Type.GetType("System.String"));//dr0
            dtClient.Columns.Add("Tag", System.Type.GetType("System.String"));//dr1
            dtClient.Columns.Add("IsConnect", System.Type.GetType("System.String"));//dr2
        }

        public void Start()
        {
            StartReceiveShortMessage(ShortMesPort);
        }
        #region Receive
        protected override void ParseData(Socket socket, CommunicationType type, string content)
        {
            base.ParseData(socket, type, content);
            switch (type)
            {
                case CommunicationType.ClientTag:
                    Task.Factory.StartNew(() => { AddRow(socket.RemoteEndPoint.ToString().Split(':')[0], content); });
                    break;
                case CommunicationType.ShortMessage:
                    Task.Factory.StartNew(() => { Show?.Invoke(content); });
                    break;
            }

        }
        private void AddRow(string Ip, string Tag, string isConnect = "1")
        {
            DataRow dr = dtClient.NewRow();
            dr[0] = Ip;
            dr[1] = Tag;
            dr[2] = isConnect;

        }
        #endregion Receive
    }
}
