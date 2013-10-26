using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using D3Model.DataContracts;
using log4net;

namespace D3Monitor.SubscriptionService
{
    public interface ISubscriptionServiceWrapper
    {
        IEnumerable<SubscriptionInformation> GetSubscriptions();
        event EventHandler<List<SubscriptionInformation>> SubscriptionsChanged;
    }

    public class SubscriptionServiceWrapper : ISubscriptionServiceWrapper
    {
        private const string DataPath = @"C:\Git\D3Monitor\Source\App\D3Monitor.Web\Data\";
        private const string Filename = "Subscriptions.xml";
        private readonly IXmlFileAccess<SubscriptionInformation[]> xmlFileAccess;
        private static readonly ILog Log = LogManager.GetLogger(typeof (SubscriptionServiceWrapper));
        private static readonly object FileLock = new object();

        public event EventHandler<List<SubscriptionInformation>> SubscriptionsChanged;

        public SubscriptionServiceWrapper()
        {
            try
            {
                Log.Info("SubscriptionServiceWrapper STARTING");
                xmlFileAccess = new XmlFileAccess<SubscriptionInformation[]>();
                StartWatchingFiles();
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        public IEnumerable<SubscriptionInformation> GetSubscriptions()
        {
            return xmlFileAccess.Load(Path.Combine(DataPath, Filename)).ToList(); ;
        }

        private FileSystemWatcher watcher;

        private void StartWatchingFiles()
        {
            watcher = new FileSystemWatcher {Path = DataPath, NotifyFilter = NotifyFilters.LastWrite, Filter = "*.xml"};
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true; // Begin watching.
        }
        
        private void OnChanged(object sender, FileSystemEventArgs fileSystem)
        {
            try
            {
                lock (FileLock)
                {
                    Thread.Sleep(1000);
                    SubscriptionInformation[] loadedSubscriptions = xmlFileAccess.Load(Path.Combine(DataPath, Filename));
                    
                    if (SubscriptionsChanged != null)
                    {
                        SubscriptionsChanged(this, loadedSubscriptions.ToList());
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}

