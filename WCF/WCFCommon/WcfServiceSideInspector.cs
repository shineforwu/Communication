using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommon
{
    /// <summary>
    /// The Inspector on server side
    /// </summary>
    public class WcfServiceSideInspector : IDispatchMessageInspector, IClientMessageInspector
    {
       

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            //throw new NotImplementedException();
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            //throw new NotImplementedException();
            string clientMessage = request.Headers.GetHeader<string>("A", "B");
            if(clientMessage=="C")
            {
                Console.WriteLine("clientMessage is OK");
            }
            else
            {
                Console.WriteLine("clientMessage is wrong");
            }
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            //throw new NotImplementedException();
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            //throw new NotImplementedException();
            MessageHeader header = MessageHeader.CreateHeader("A", "B", "C");
            request.Headers.Add(header);
            return null;
        }
    }
}
