using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Interfaces.HubInterfaces;
using EduConnect.SignalIR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EduConnect.Services
{
    public class ViewershipChangeService(
        IServiceProvider services,
        IHubContext<CourseAnalyticsHub, ICourseAnalyticsClient> hub
        ) : BackgroundService
    {
        private readonly IServiceProvider _services = services;
        private readonly IHubContext<CourseAnalyticsHub, ICourseAnalyticsClient> _hub = hub;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                using var scope = _services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                var changes = await GetRecentChanges(context);

                foreach (var group in changes.GroupBy(x => x.CourseId))
                {
                    await _hub.Clients.Group(group.Key.ToString())
                    .BatchUpdateViewership([.. group.Select(c => new ViewershipUpdate(
                    c.CourseId,
                    group.Count(),
                    group.Count(v => v.LeftDetailsAt == null),
                    group.Average(v => (v.LeftDetailsAt - v.EnteredDetailsAt)?.TotalMinutes ?? 0)
                ))]);

                }

                await Task.Delay(5000, stoppingToken);
            }

        }

        private async Task<List<CourseViewershipData>> GetRecentChanges(DataContext context)
        {
            return await context.CourseViewershipData
            .FromSqlRaw(@"
             SELECT 
            cvd.CourseViewershipDataId,
            cvd.CourseId,
            cvd.ViewedByPersonId,
            cvd.ClickedOn,
            cvd.EnteredDetailsAt,
            cvd.LeftDetailsAt,
            cvd.UserCameFrom,
            cvd.CreatedAt,
            cvd.UpdatedAt
            FROM CHANGETABLE(CHANGES Course.CourseViewershipData, 0) AS ct
            INNER JOIN Course.CourseViewershipData AS cvd
            ON ct.CourseViewershipDataId = cvd.CourseViewershipDataId
            ")
            .OrderBy(c => c.ClickedOn)
            .Take(100)
            .ToListAsync();
        }
    }
}