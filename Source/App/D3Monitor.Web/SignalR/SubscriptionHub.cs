using D3Model.DataContracts;
using D3Monitor.SubscriptionService;
using Microsoft.AspNet.SignalR;

namespace D3Monitor.SignalR
{
    public class SubscriptionHub : Hub
    {
        private readonly ISubscriptionServiceWrapper subscriptionServiceWrapper;

        //TODO: override hub injection
        public SubscriptionHub()
        {
            subscriptionServiceWrapper = new SubscriptionServiceWrapper();
        }
        public SubscriptionHub(ISubscriptionServiceWrapper subscriptionServiceWrapper )
        {
            this.subscriptionServiceWrapper = subscriptionServiceWrapper;
        }
        
        public ApplicationDto[] GetAllApplications()
        {
            return subscriptionServiceWrapper.GetAllApplications();
        }
    }


}