using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Interfaces.HubInterfaces;
using EduConnect.SignalIR;
using Microsoft.AspNetCore.SignalR;

namespace EduConnect.Services
{
    public class ViewershipUpdateBufferService(IHubContext<CourseAnalyticsHub, ICourseAnalyticsClient> hub)
    {
        private readonly IHubContext<CourseAnalyticsHub, ICourseAnalyticsClient> _hub = hub;
        private readonly ConcurrentDictionary<Guid, ViewershipUpdate> _buffer = new();

        public void AddUpdate(ViewershipUpdate update)
        {
            _buffer.AddOrUpdate(
                update.CourseId,
                update,
                (_, existing) => existing with
                {
                    NewViews = existing.NewViews + update.NewViews,
                    ActiveViewersChange = existing.ActiveViewersChange + update.ActiveViewersChange,
                    UpdatedAverageTime = (existing.UpdatedAverageTime + update.UpdatedAverageTime) / 2
                }
            );
            Task.Delay(1000).ContinueWith(_ => FlushUpdate(update.CourseId));
        }

        private void FlushUpdate(Guid courseId)
        {
            if (_buffer.TryRemove(courseId, out var update))
            {
                _hub.Clients.Group(courseId.ToString()).BatchUpdateViewership([update]);
            }
        }
    }
}