using System.Collections.Generic;
using D3Model.DataContracts;
using D3Monitor.MsmqUtils;
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
        private IApplicationInfoDataAccess applicationInfoDataAccess;
        private readonly IQueueStats queueStats;

        public SubscriptionHub()
        {
            subscriptionServiceWrapper = Bootstrapper.Container.Resolve<ISubscriptionServiceWrapper>();
            applicationInfoDataAccess = Bootstrapper.Container.Resolve<IApplicationInfoDataAccess>();
            
            queueStats = Bootstrapper.Container.Resolve<IQueueStats>();
            Log.Info("SubscriptionHub");
            subscriptionServiceWrapper.SubscriptionsChanged += subscriptionServiceWrapper_SubscriptionsChanged;
        }
        
        public ApplicationsWithLinksDto GetAllApplications()
        {
            try
            {
                var subscriptions = subscriptionServiceWrapper.GetSubscriptions();
                applications = Transform(subscriptions);

                var savedApplications = applicationInfoDataAccess.GetApplicationInfos().ToList();
                
                var applicationsToBeSaved = new List<ApplicationInfoDto>();
                foreach (var applicationDto in applications)
                {
                    var savedApplicationInfo = savedApplications.SingleOrDefault(a => a.ApplicationName == applicationDto.ApplicationName);
                    if (savedApplicationInfo == null)
                    {
                        applicationsToBeSaved.Add(new ApplicationInfoDto()
                        {
                            ApplicationName = applicationDto.ApplicationName
                        });
                    }
                    else
                    {
                        applicationDto.X = savedApplicationInfo.X;
                        applicationDto.Y = savedApplicationInfo.Y;
                        applicationDto.ServiceDown = savedApplicationInfo.ServiceDown;
                    }
                }
                if (applicationsToBeSaved.Any())
                {
                    savedApplications.AddRange(applicationsToBeSaved);
                    applicationInfoDataAccess.SaveApplicationInfos(savedApplications);
                }
                var link1Dto = new LinkDto() { StartApplicationName = "Resulting.MatchStateResulter", EndApplicationName = "InRunning.Football"};
                var link2Dto = new LinkDto() { StartApplicationName = "Resulting.MatchStateResulter", EndApplicationName = "InRunning.Tennis" };
                var link3Dto = new LinkDto(){ StartApplicationName = "RunningBall.Connector", EndApplicationName = "FootballFeed"};
                var link4Dto = new LinkDto() { StartApplicationName = "FootballFeed", EndApplicationName = "InRunning.Football" };
                var applicationsWithLinks = new ApplicationsWithLinksDto()
                {
                    Applications=applications.ToArray(),
                    Links = new[] { link1Dto, link2Dto, link3Dto, link4Dto }
                };
                applications[4].ServiceDown = true;
                return applicationsWithLinks;
            }
            catch (HubException e)
            {
                Log.Error(e);
                throw;
            }
        }

        public void SaveApplicationPosition(string applicationName, int x, int y)
        {
            try
            {
                Log.DebugFormat("Saving Position: {0}, {1}, {2}", applicationName, x, y);
                applicationInfoDataAccess.UpdateApplicationInfo(applicationName,x,y);
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
            applications = new List<ApplicationDto>();
            foreach (var subscription in subscriptions)
            {
                var applicationForSubscription = applications.SingleOrDefault(a => a.ApplicationName == subscription.ApplicationName);
                if (applicationForSubscription == null)
                {
                    applicationForSubscription = new ApplicationDto()
                    {
                        ApplicationName = subscription.ApplicationName,
                        ServiceBuses = new ServiceBusDto[0],
                        Y = 100 * (applications.Count + 1),
                        X = 30 * (applications.Count + 1)
                    };
                    applications.Add(applicationForSubscription);
                }
                var serviceBusForSubscription = applicationForSubscription.ServiceBuses.SingleOrDefault(sb => sb.EndpointUri == subscription.EndpointUri && sb.HostName == subscription.HostName);
                if (serviceBusForSubscription == null)
                {
                    serviceBusForSubscription = new ServiceBusDto()
                    {
                        EndpointUri = subscription.EndpointUri,
                        HostName = subscription.HostName,
                        Subscriptions = new SubscriptionDto[0]
                    };
                    var serviceBuses = applicationForSubscription.ServiceBuses.ToList();
                    serviceBuses.Add(serviceBusForSubscription);
                    applicationForSubscription.ServiceBuses = serviceBuses.ToArray();
                }
                serviceBusForSubscription.ClientId = subscription.ClientId;
                serviceBusForSubscription.ClientName = subscription.ClientName;
                serviceBusForSubscription.MessageCount = queueStats.GetMessageCount(subscription.EndpointUri, subscription.HostName);

                var existingSubscription = serviceBusForSubscription.Subscriptions.SingleOrDefault(s => s.SubscriptionId == subscription.SubscriptionId);
                if (existingSubscription == null)
                {
                    existingSubscription = new SubscriptionDto();
                    var subscriptionsList = serviceBusForSubscription.Subscriptions.ToList();
                    subscriptionsList.Add(existingSubscription);
                    serviceBusForSubscription.Subscriptions = subscriptionsList.ToArray();
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