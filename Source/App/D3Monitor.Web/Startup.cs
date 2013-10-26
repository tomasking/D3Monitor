
using System;
using System.IO;
using System.Web.Routing;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using D3Monitor.Infrastructure;
using D3Monitor.SignalR;
using D3Monitor.SubscriptionService;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Owin;
using log4net;
using log4net.Config;

namespace D3Monitor
{
    public class Startup
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Startup));

        public void Configuration(IAppBuilder app)
        {
            try
            {
//                GlobalHost.DependencyResolver.Register(
//                 typeof(SubscriptionHub),
//                    () => new SubscriptionHub(Bootstrapper.Container.Resolve<ISubscriptionServiceWrapper>()));
//
//                app.MapSignalR();

                //Log.Info("MapSignalR");
                //app.MapSignalR(new HubConfiguration() {Resolver = new SignalrDependencyResolver(Bootstrapper.Container.Kernel)});
                

                var resolver = new CastleWindsorDependencyResolver(Bootstrapper.Container);
                var config = new HubConfiguration()
                {
                    Resolver = resolver
                };
                GlobalHost.DependencyResolver = resolver;
                
                app.MapSignalR(config);




            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
