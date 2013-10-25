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

        public SubscriptionHub()
        {
            subscriptionServiceWrapper = Bootstrapper.Container.Resolve<ISubscriptionServiceWrapper>();
            Log.Info("SubscriptionHub");
            subscriptionServiceWrapper.SubscriptionsChanged += subscriptionServiceWrapper_SubscriptionsChanged;
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

        void subscriptionServiceWrapper_SubscriptionsChanged(object sender, List<SubscriptionInformation> subscriptions)
        {
            try
            {
                Clients.Client(Context.ConnectionId).ApplicationsChanged(Transform(subscriptions));
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
                applications.Add(Transform(subscription));
            }
            return applications;
        }

        private static ApplicationDto Transform(SubscriptionInformation subscription)
        {
            return new ApplicationDto()
            {
                Name=subscription.ApplicationName,
                SubscriptionId = subscription.SubscriptionId
            };
        }

        public override System.Threading.Tasks.Task OnDisconnected()
        {
            Log.Info("OnDisconnected");
            

            return base.OnDisconnected();
        }
    }
}