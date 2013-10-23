using System;

namespace D3Model.DataContracts
{
    [Serializable]
    public class SubscriptionOptions
    {
        public TimeSpan? MessageExpiryTime { get; set; }
        public TimeSpan? QueueExpiryTime { get; set; }
        public TimeSpan IntervalBetweenRetries { get; set; }
        public bool AlwaysPurgeOnStart { get; set; }
        public int MaximumRetriesOnError { get; set; }
    }
}