using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Data;
using EduConnect.Entities.Course;
using EduConnect.Utilities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms;

namespace EduConnect.Services
{
    public class CourseViewershipDataSnapshotService(IServiceScopeFactory scopeFactory) : IHostedService, IDisposable
    {

        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteTask, null, 0, (int)TimeSpan.FromMinutes(60).TotalMilliseconds);
            return Task.CompletedTask;
        }

        private async void ExecuteTask(object state)
        {
            using var scope = _scopeFactory.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            var courses = await dataContext.Course.ToListAsync();

            foreach (var course in courses)
            {
                if (await dataContext.CourseViewershipData.Where(x => x.CourseId == course.CourseId).AnyAsync())
                {

                    var courseViewershipData = await dataContext.CourseViewershipData
                        .Where(x => x.CourseId == course.CourseId)
                        .ToListAsync();

                    Console.WriteLine($"Course viewership data snapshot for course {course.CourseId}. {courseViewershipData.Count}");

                    var snapshot = new CourseViewershipDataSnapshot
                    {
                        CourseId = course.CourseId,
                        TotalViews = courseViewershipData.Count(),
                        NumberOfUniqueVisitors = courseViewershipData.Select(cvd => cvd.ViewedByPersonId).Distinct().Count(),
                        CurrentlyViewing = courseViewershipData.Count(cvd => cvd.EnteredDetailsAt != null && cvd.LeftDetailsAt == null),
                        AverageViewDurationInMinutes = courseViewershipData
                    .Where(cvd => cvd.EnteredDetailsAt != null && cvd.LeftDetailsAt != null)
                    .DefaultIfEmpty() // To avoid empty sequences
                    .Average(cvd =>
                    {
                        // Calculate the difference in minutes manually if both fields are not null
                        return cvd.EnteredDetailsAt != null && cvd.LeftDetailsAt != null
                            ? (cvd.LeftDetailsAt.Value - cvd.EnteredDetailsAt.Value).TotalMinutes
                            : 0;
                    })

                    };
                    Console.WriteLine($"Snapshot for course {course.CourseId} created.");
                    PrintObjectUtility.PrintObjectProperties(snapshot);

                    await dataContext.CourseViewershipDataSnapshot.AddAsync(snapshot);

                }

            }

            await dataContext.SaveChangesAsync();
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}