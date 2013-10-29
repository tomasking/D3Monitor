using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using D3Model.DataContracts;
using log4net;

namespace D3Monitor.SubscriptionService
{
    public interface IApplicationInfoDataAccess
    {
        IEnumerable<ApplicationInfoDto> GetApplicationInfos();
        void SaveApplicationInfos(List<ApplicationInfoDto> applicationInfos);
        void UpdateApplicationInfo(string applicationName, int x, int y);
    }

    public class ApplicationInfoDataAccess : IApplicationInfoDataAccess
    {
        private const string DataPath = @"C:\Git\D3Monitor\Source\App\D3Monitor.Web\Data\";
        private const string Filename = "ApplicationInfos.xml";
        private readonly IXmlFileAccess<ApplicationInfoDto[]> xmlFileAccess;
        private static readonly ILog Log = LogManager.GetLogger(typeof(ApplicationInfoDataAccess));
        private static readonly object FileLock = new object();

        public ApplicationInfoDataAccess()
        {
            xmlFileAccess = new XmlFileAccess<ApplicationInfoDto[]>();
        }

        public IEnumerable<ApplicationInfoDto> GetApplicationInfos()
        {
            try
            {
                var apps = xmlFileAccess.Load(Path.Combine(DataPath, Filename));
                return apps;
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        public void SaveApplicationInfos(List<ApplicationInfoDto> applicationInfos)
        {
            try
            {
                xmlFileAccess.Save(applicationInfos.ToArray(), Path.Combine(DataPath, Filename));
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }

        public void UpdateApplicationInfo(string applicationName, int x, int y)
        {
            try
            {
                var apps = GetApplicationInfos().ToList();
                var app = apps.SingleOrDefault(a => a.ApplicationName == applicationName);
                if (app != null)
                {
                    app.X = x;
                    app.Y = y;
                }
                SaveApplicationInfos(apps);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}
