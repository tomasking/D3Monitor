using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using D3Model.DataContracts;
using log4net;

namespace D3Monitor.SubscriptionService
{
    public interface ISubscriptionServiceWrapper
    {
        IEnumerable<SubscriptionInformation> GetSubscriptions();
    }

    public class SubscriptionServiceWrapper : ISubscriptionServiceWrapper
    {
        private const string DataPath = @"C:\Git\D3Monitor\Source\App\D3Monitor.Web\Data\";
        private const string Filename = "Subscriptions.xml";
        private readonly IXmlFileAccess<SubscriptionInformation[]> xmlFileAccess;
        private readonly List<SubscriptionInformation> subscriptions;
        private static readonly ILog Log = LogManager.GetLogger(typeof(SubscriptionServiceWrapper));

        public SubscriptionServiceWrapper()
        {
            try
            {
                xmlFileAccess = new XmlFileAccess<SubscriptionInformation[]>();
                subscriptions = xmlFileAccess.Load(Path.Combine(DataPath, Filename)).ToList();
                //StartWatchingFiles();
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        public IEnumerable<SubscriptionInformation> GetSubscriptions()
        {
            return subscriptions;
        }

        private void StartWatchingFiles()
        {
            var watcher = new FileSystemWatcher {Path = DataPath, NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite, Filter = "*.xml"};
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true; // Begin watching.
        }

 
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            SubscriptionInformation[] loadedSubscriptions = xmlFileAccess.Load(Path.Combine(DataPath, Filename));
            
            foreach (var loadedSubscription in loadedSubscriptions)
            {
                var existing = subscriptions.SingleOrDefault(s => s.SubscriptionId == loadedSubscription.SubscriptionId);
                if (existing == null)
                {
                    RaiseSubscriptionAdded(loadedSubscription);
                }
            }
            foreach (var subscription in subscriptions)
            {
                var loadedSubscription = loadedSubscriptions.SingleOrDefault(s => s.SubscriptionId == subscription.SubscriptionId);
                if (loadedSubscription == null)
                {
                    RaiseSubscriptionDeleted(subscription);
                }
           }
        }

        private void RaiseSubscriptionAdded(SubscriptionInformation subscription)
        {
            throw new System.NotImplementedException();
        }

        private void RaiseSubscriptionDeleted(SubscriptionInformation subscription)
        {
            throw new System.NotImplementedException();
        }

        
    }
}
