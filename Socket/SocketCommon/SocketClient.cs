using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketCommon
{
    public class SocketClient : SocketBase
    {
        public bool SendMes(string Message)
        {
            return SendMessage(CommunicationType.ShortMessage, Message, 3);
        }
    }
}
