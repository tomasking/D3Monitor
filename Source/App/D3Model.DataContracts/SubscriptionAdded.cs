using System;

namespace D3Model.DataContracts
{
    [Serializable]
    public class SubscriptionAdded
    {
        public SubscriptionInformation SubscriptionInformation { get; set; }
    }

    [Serializable]
    public class SubscriptionClientAdded
    {
        public SubscriptionClientMessage SubscriptionClientMessage { get; set; }
    }

    [Serializable]
    public class SubscriptionClientRemoved
    {
        public Guid ClientId { get; set; }
        public string EndpointUri { get; set; }
    }

    [Serializable]
    public class SubscriptionRefresh
    {
        public SubscriptionInformation[] SubscriptionInformations { get; set; }
    }

    [Serializable]
    public class SubscriptionRemoved
    {
        public Guid SubscriptionId { get; set; }
        public string EndpointUri { get; set; }
    }

    [Serializable]
    public class SubscriptionClientMessage
    {
        public Guid ClientId { get; set; }
        public string ControlUri { get; set; }
        public string DataUri { get; set; }
    }
}