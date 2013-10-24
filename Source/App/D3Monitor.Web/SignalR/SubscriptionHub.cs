using System.Collections.Generic;
using D3Model.DataContracts;
using D3Monitor.SubscriptionService;
using Microsoft.AspNet.SignalR;
using log4net;

namespace D3Monitor.SignalR
{
    public class SubscriptionHub : Hub
    {
        private readonly ISubscriptionServiceWrapper subscriptionServiceWrapper;
        private static readonly ILog Log = LogManager.GetLogger(typeof(SubscriptionHub));

        //TODO: override hub injection
        public SubscriptionHub() : this(new SubscriptionServiceWrapper()){}
        public SubscriptionHub(ISubscriptionServiceWrapper subscriptionServiceWrapper )
        {
            this.subscriptionServiceWrapper = subscriptionServiceWrapper;
        }
        
        public ApplicationDto[] GetAllApplications()
        {
            try
            {
                var subscriptions = subscriptionServiceWrapper.GetSubscriptions();
                List<ApplicationDto> applications = Transform(subscriptions);
                return applications.ToArray();
            }
            catch (HubException e)
            {
                Log.Error(e);
                throw;
            }
        }

        private List<ApplicationDto> Transform(IEnumerable<SubscriptionInformation> subscriptions)
        {
            var applications = new List<ApplicationDto>();
            foreach (var subscription in subscriptions)
            {
                applications.Add(new ApplicationDto(){ Name=subscription.ApplicationName});
            }
            return applications;
        }
    }


}