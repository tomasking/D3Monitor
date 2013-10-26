using System;
using System.Collections.Generic;
using D3Model.DataContracts;
using D3Monitor.SubscriptionService;
using NUnit.Framework;

namespace D3Monitor.Tests
{
    [TestFixture]
    public class SubscriptionWrapperTests
    {
        private const string DataPath = @"C:\Git\D3Monitor\Source\App\D3Monitor.Web\Data\Subscriptions.xml";

        [Test]
        public void SaveSubscriptionFile()
        {
            var subscriptionInformations = new List<SubscriptionInformation>();

            subscriptionInformations.Add(new SubscriptionInformation()
            {
                ApplicationName = "InRunning.Football",
                ClientId = Guid.NewGuid(),
                ClientName = "FootballMatchStateController",
                CorrelationId=Guid.NewGuid().ToString(),
                EndpointUri = "msmq://localhost/test_queue",
                HostName = "irdev.betgenius.net",
                MessageName = "Betgenius.InRunning.FootballMatchState",
                RoutingDetails=new RoutingDetail[]
                {
                    new RoutingDetail() { Key="SportId",Value= "10" }, 
                },
                SequenceNumber = 1,
                SubscriptionId = Guid.NewGuid(),
                SubscriptionOptions = new SubscriptionOptions()
                {
                    AlwaysPurgeOnStart=true,
                    IntervalBetweenRetries = TimeSpan.FromMinutes(1),
                    MaximumRetriesOnError = 2,
                    MessageExpiryTime = TimeSpan.FromDays(1),
                    QueueExpiryTime = TimeSpan.FromSeconds(1)
                }
            });
            subscriptionInformations.Add(new SubscriptionInformation()
            {
                ApplicationName = "InRunning.Tennis",
                ClientId = Guid.NewGuid(),
                ClientName = "TennisMatchStateController",
                CorrelationId = Guid.NewGuid().ToString(),
                EndpointUri = "msmq://localhost/test_queue",
                HostName = "emdev.betgenius.net",
                MessageName = "Betgenius.InRunning.FootballMatchState",
                RoutingDetails = new RoutingDetail[]
                {
                    new RoutingDetail() { Key="SportId",Value= "24" }, 
                },
                SequenceNumber = 1,
                SubscriptionId = Guid.NewGuid(),
                SubscriptionOptions = new SubscriptionOptions()
                {
                    AlwaysPurgeOnStart = true,
                    IntervalBetweenRetries = TimeSpan.FromMinutes(1),
                    MaximumRetriesOnError = 2,
                    MessageExpiryTime = TimeSpan.FromDays(1),
                    QueueExpiryTime = TimeSpan.FromSeconds(1)
                }
            });

            IXmlFileAccess<SubscriptionInformation[]> xmlFileAccess = new XmlFileAccess<SubscriptionInformation[]>();
            xmlFileAccess.Save(subscriptionInformations.ToArray(), DataPath);

        }
    }
}

/*
public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public string ApplicationName { get; set; }
        public string HostName { get; set; }

        public Guid SubscriptionId { get; set; }
        public int SequenceNumber { get; set; } //incremented per message subscription per client id
        public string EndpointUri { get; set; }
        public string MessageName { get; set; } //for routing
        public RoutingDetail[] RoutingDetails { get; set; } //For content based routing
        public string CorrelationId { get; set; }
        public SubscriptionOptions SubscriptionOptions { get; set; }

*/