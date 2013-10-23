using System.Collections.Generic;
using D3Model.DataContracts;

namespace D3Monitor.SubscriptionService
{
    public interface ISubscriptionServiceWrapper
    {
        ApplicationDto[] GetAllApplications();
    }

    public class SubscriptionServiceWrapper : ISubscriptionServiceWrapper
    {
        public ApplicationDto[] GetAllApplications()
        {
            var applications = new List<ApplicationDto>
            {
                new ApplicationDto() {Name = "InRunning.Football"},
                new ApplicationDto() {Name = "InRunning.Tennis"},
                new ApplicationDto() {Name = "InRunning.Basketball"},
                new ApplicationDto() {Name = "RunningBall.Connector"}
            };
            return applications.ToArray();
        }
    }
}
