using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.Interfaces.HubInterfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.SignalIR
{
    public class CourseAnalyticsHub(DataContext _dataContext) : Hub<ICourseAnalyticsClient>
    {
        public async Task SubscribeToCourse(Guid courseId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, courseId.ToString());
            var statistics = await GetCourseStatistics(courseId);

        }

        private async Task<CourseStatistics> GetCourseStatistics(Guid courseId)
        {
            return await _dataContext.CourseViewershipData
            .Where(x => x.CourseId == courseId)
            .GroupBy(x => 1)
            .Select(
                x => new CourseStatistics(
                    x.Count(),
                    x.Count(y => y.EnteredDetailsAt != null && y.LeftDetailsAt == null),
                    x.Average(y => y.LeftDetailsAt.HasValue && y.EnteredDetailsAt.HasValue
                        ? (y.LeftDetailsAt.Value - y.EnteredDetailsAt.Value).TotalMinutes
                        : 0)
                )
            )
            .FirstOrDefaultAsync() ?? new CourseStatistics();
        }

    }

    public record CourseStatistics(int TotalViews = 0, int ActiveNow = 0, double AverageTimeSpent = 0);

    public record ViewershipUpdate(Guid CourseId, int NewViews, int ActiveViewersChange, double UpdatedAverageTime);
}