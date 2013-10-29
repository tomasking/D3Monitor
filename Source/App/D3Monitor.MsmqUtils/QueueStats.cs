using System;
using System.Diagnostics;
using log4net;

namespace D3Monitor.MsmqUtils
{

    public interface IQueueStats
    {
        long? GetMessageCount(string endpointUri, string hostName);
    }

    public class QueueStats : IQueueStats
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(QueueStats));

        public long? GetMessageCount(string endpointUri, string hostName)
        {
            try
            {
                var queueCounter = new PerformanceCounter(
                    "MSMQ Queue",
                    "Messages in Queue",
                    string.Format("{0}\\{1}", hostName, endpointUri), hostName);
                return queueCounter.RawValue;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
    }
}
