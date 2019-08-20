using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketCommon
{
    public enum CommunicationType
    {
        HeartbeatSend = 0,
        ClientTag = 1,
        ShortMessage = 2,
        LongMessage = 3,
    }
}
