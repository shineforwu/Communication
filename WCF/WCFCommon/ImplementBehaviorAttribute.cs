using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommon
{
    public class ImplementBehaviorAttribute : Attribute, IEndpointBehavior
    {
        #region Implement IServiceBehavior

        #endregion Implement IServiceBehavior
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            //throw new NotImplementedException();
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            //throw new NotImplementedException();
            clientRuntime.ClientMessageInspectors.Add(new WcfServiceSideInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            //throw new NotImplementedException();
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new WcfServiceSideInspector());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            //throw new NotImplementedException();
        }
    }
}
