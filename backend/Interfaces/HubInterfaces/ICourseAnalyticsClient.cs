using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.SignalIR;

namespace EduConnect.Interfaces.HubInterfaces
{
    public interface ICourseAnalyticsClient
    {
        Task ReceiveInitialData(CourseStatistics statistics);

        Task UpdateViewership(ViewershipUpdate update);

        Task BatchUpdateViewership(List<ViewershipUpdate> updates);
    }
}