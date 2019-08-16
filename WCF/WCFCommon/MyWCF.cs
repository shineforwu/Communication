using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommon
{
    public class MyWCF
    {
        public delegate void DelegatePrint(string message);
        public event DelegatePrint EventPrint;
        public ServiceHost StartWCF(string hostAddress,int port, Type serviceType, Type implementedContract)
        {
            try
            {
                Uri baseAddress = new Uri("http://" + hostAddress + ":" + port + "/");
                ServiceHost host = new ServiceHost(serviceType, baseAddress);
                host.AddServiceEndpoint(implementedContract, new WSHttpBinding(SecurityMode.None), baseAddress);
                host.Open();
                return host;
            }
            catch (Exception ex)
            {
                EventPrint?.Invoke(ex.Message);
                return null;
            }
        }

        public Object GetWcfChannel<T>(string hostAddress, int port)
        {
            EndpointAddress edpHttp = new EndpointAddress("http://" + hostAddress + ":" + port + "/");
            WSHttpBinding bind = new WSHttpBinding(SecurityMode.None);
            ChannelFactory<T> factory = new ChannelFactory<T>(bind);
            T channel = factory.CreateChannel(edpHttp);
            return channel;
        }

        public void Close(ServiceHost serviceHost)
        {
            if(serviceHost !=null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
