using System;

namespace D3Model.DataContracts
{
    [Serializable]
    public class SubscriptionInformation
    {
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
    }
}
