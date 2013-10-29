using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using D3Monitor.MsmqUtils;
using D3Monitor.SubscriptionService;
using log4net;

namespace D3Monitor
{
    public class Bootstrapper
    {
        public static IWindsorContainer Container;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Bootstrapper));

        static Bootstrapper()
        {
            Container = new WindsorContainer();
        }

         public static IWindsorContainer Bootstrap()
         {
             try
             {
                 Log.Info("Bootstrapping");
                 Container.Register(
                         Component.For<IQueueStats>().ImplementedBy<QueueStats>(),
                         Component.For<IApplicationInfoDataAccess>().ImplementedBy<ApplicationInfoDataAccess>(),
                         Component.For<ISubscriptionServiceWrapper>().ImplementedBy<SubscriptionServiceWrapper>()
                         );
                 return Container;
             }
             catch (Exception e)
             {
                 Log.Error(e);
                 throw;
             }
         }

    }
}