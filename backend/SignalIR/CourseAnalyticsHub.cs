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

            var data = await _dataContext.CourseViewershipData
    .Where(cvd => cvd.CourseId == courseId)
    .GroupBy(cvd => 1) // dummy grouping to enable aggregation
    .Select(g => new
    {
        TotalViews = g.Count(),
        ActiveViewers = g.Count(cvd => cvd.EnteredDetailsAt != null && cvd.LeftDetailsAt == null),
        AverageViewDurationInMinutes = g
            .Where(cvd => cvd.EnteredDetailsAt != null && cvd.LeftDetailsAt != null)
            .Average(cvd => EF.Functions.DateDiffMinute(cvd.EnteredDetailsAt.Value, cvd.LeftDetailsAt.Value))
    })
    .FirstOrDefaultAsync();
            await Groups.AddToGroupAsync(Context.ConnectionId, courseId.ToString());
            Console.WriteLine($"Connection {Context.ConnectionId} subscribed to group {courseId}");
            await Clients.Caller.SendAsync("GetAnalyticsData", data);
        }

        public record CourseViewershipAnalyticsData(Guid CourseId, int TotalViews = 0, int CurrentlyViewing = 0, double AvgTimeSpent = 0);
        public record ViewershipUpdate(
            Guid CourseId,
            int NewViews,
            int ActiveViewersChange,
            double UpdatedAvgTime
        );
    }


}