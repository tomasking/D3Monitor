﻿using System;

namespace D3Model.DataContracts
{
    public class ApplicationDto
    {
        public string ApplicationName { get; set; }
        public ServiceBusDto[] ServiceBuses { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool ServiceDown { get; set; }
    }

    public class ServiceBusDto
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public string HostName { get; set; }
        public string EndpointUri { get; set; }
        public SubscriptionDto[] Subscriptions { get; set; }
        public long? MessageCount { get; set; }
    }

    public class SubscriptionDto
    {
        public Guid SubscriptionId { get; set; }
        public int SequenceNumber { get; set; } //incremented per message subscription per client id
        public string MessageName { get; set; } //for routing
        public SubscriptionOptions SubscriptionOptions { get; set; }

        public string CorrelationId { get; set; }

        public RoutingDetail[] RoutingDetails { get; set; } //For content based routing
    }


    public class LinkDto
    {
        public string StartApplicationName { get; set; }
        public string EndApplicationName { get; set; }
    }


    public class ApplicationsWithLinksDto
    {
        public ApplicationDto[] Applications { get; set; }
        public LinkDto[] Links { get; set; }
    }

    public class ApplicationInfoDto
    {
        public string ApplicationName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool ServiceDown { get; set; }
    }

}
