using System;
using System.IO;
using log4net;
using log4net.Config;

namespace D3Monitor
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            
            string configFile = AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();
            XmlConfigurator.ConfigureAndWatch(new FileInfo(configFile));
            Log.Info("Application_Start");
            Bootstrapper.Bootstrap();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}