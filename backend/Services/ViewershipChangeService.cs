using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.DTOs;
using EduConnect.Entities.Course;
using EduConnect.SignalIR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using MimeKit.Cryptography;
using static EduConnect.SignalIR.CourseAnalyticsHub;

namespace EduConnect.Services
{
    public class ViewershipChangeService(IServiceScopeFactory scopeFactory, IHubContext<CourseAnalyticsHub> hubContext) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        private readonly IHubContext<CourseAnalyticsHub> _hubContext = hubContext;
        private long lastSyncVersion;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("ViewershipChangeService is running.");

            using var scope = _scopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            lastSyncVersion = await dataContext.Database
            .SqlQueryRaw<long>("SELECT CHANGE_TRACKING_CURRENT_VERSION() AS Value")
            .FirstOrDefaultAsync(cancellationToken: stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                using var innerScope = _scopeFactory.CreateScope();
                var db = innerScope.ServiceProvider.GetRequiredService<DataContext>();

                var changedCourseIds = await GetChangedCoursesIds(db, lastSyncVersion);

                foreach (var courseId in changedCourseIds)
                {
                    var analyticsData = await db.CourseViewershipData
                    .Where(x => x.CourseId == courseId)
                    .GroupBy(x => 1)
                    .Select(g => new GetAnalyticsDataResponse

                    {
                        CourseId = courseId,
                        TotalViews = g.Count(),
                        NumberOfUniqueVisitors = g.Select(cvd => cvd.ViewedByPersonId).Distinct().Count(),
                        CurrentlyViewing = g.Count(cvd => cvd.EnteredDetailsAt != null && cvd.LeftDetailsAt == null),
                        AverageViewDurationInMinutes = g
            .Where(cvd => cvd.EnteredDetailsAt != null && cvd.LeftDetailsAt != null)
            .Average(cvd => EF.Functions.DateDiffMinute(cvd.EnteredDetailsAt.Value, cvd.LeftDetailsAt.Value))
                    }).FirstOrDefaultAsync();
                    Console.WriteLine($"Updating analytics data for course {courseId}");
                    await _hubContext.Clients.Group(courseId.ToString()).SendAsync("GetAnalyticsData", analyticsData, cancellationToken: stoppingToken);


                }

                lastSyncVersion = await db.Database
                .SqlQueryRaw<long>("SELECT CHANGE_TRACKING_CURRENT_VERSION() AS Value")
                .FirstAsync(cancellationToken: stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }


        }

        private async Task<List<Guid>> GetChangedCoursesIds(DataContext dbContext, long lastSyncVersion)
        {
            return await dbContext.CourseViewershipData
            .FromSqlRaw($@"
                SELECT DISTINCT CourseId
                FROM Course.CourseViewershipData cvd 
                JOIN CHANGETABLE(CHANGES Course.CourseViewershipData, {lastSyncVersion}) ct
                ON cvd.CourseViewershipDataId = ct.CourseViewershipDataId
            ")
            .Select(x => x.CourseId)
            .ToListAsync();
        }
    }
}
