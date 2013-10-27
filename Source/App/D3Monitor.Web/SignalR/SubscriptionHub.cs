using System.Collections.Generic;
using D3Model.DataContracts;
using D3Monitor.SubscriptionService;
using Microsoft.AspNet.SignalR;
using log4net;
using System.Linq;

namespace D3Monitor.SignalR
{
    public class SubscriptionHub : Hub
    {
        private readonly ISubscriptionServiceWrapper subscriptionServiceWrapper;
        private static readonly ILog Log = LogManager.GetLogger(typeof(SubscriptionHub));
        static List<ApplicationDto> applications= new List<ApplicationDto>();

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
            foreach (var subscription in subscriptions)
            {
                var applicationForSubscription = applications.SingleOrDefault(a => a.ApplicationName == subscription.ApplicationName);
                if (applicationForSubscription == null)
                {
                    applicationForSubscription = new ApplicationDto()
                    {
                        ApplicationName = subscription.ApplicationName,
                        ServiceBuses = new ServiceBusDto[0]
                    };
                    applications.Add(applicationForSubscription);
                }
                var serviceBusForSubscription = applicationForSubscription.ServiceBuses.SingleOrDefault(sb => sb.EndpointUri == subscription.EndpointUri);
                if (serviceBusForSubscription == null)
                {
                    serviceBusForSubscription = new ServiceBusDto()
                    {
                        ClientId = subscription.ClientId,
                        ClientName = subscription.ClientName,
                        EndpointUri = subscription.EndpointUri,
                        HostName = subscription.HostName,
                        Subscriptions = new SubscriptionDto[0]
                    };
                }
                var existingSubscription = serviceBusForSubscription.Subscriptions.SingleOrDefault(s => s.SubscriptionId == subscription.SubscriptionId);
                if (existingSubscription == null)
                {
                    existingSubscription = new SubscriptionDto();
                }
                existingSubscription.CorrelationId = subscription.CorrelationId;
                existingSubscription.MessageName = subscription.MessageName;
                existingSubscription.RoutingDetails = subscription.RoutingDetails;
                existingSubscription.SequenceNumber = subscription.SequenceNumber;
                existingSubscription.SubscriptionId = subscription.SubscriptionId;
                existingSubscription.SubscriptionOptions = subscription.SubscriptionOptions;
            }
            return applications;
        }
        
        public override System.Threading.Tasks.Task OnDisconnected()
        {
            Log.Info("OnDisconnected");
            subscriptionServiceWrapper.SubscriptionsChanged -= subscriptionServiceWrapper_SubscriptionsChanged;
            return base.OnDisconnected();
        }
    }
}