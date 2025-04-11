using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.SignalIR
{
    public class CourseAnalyticsHub(DataContext dataContext) : Hub
    {
        private readonly DataContext _dataContext = dataContext;

        public async Task SendAnalyticsData(Guid courseId)
        {
            Console.WriteLine("Received analytics data for course: " + courseId);

            int count = await _dataContext.CourseViewershipData
            .Where(x => x.CourseId == courseId)
            .CountAsync();

            Console.WriteLine("Count: " + count);

            await Clients.Caller.SendAsync("GetAnalyticsData", count);
        }

        public record CourseViewershipAnalyticsData(Guid courseId, string courseName, int clickedOn);

    }


}