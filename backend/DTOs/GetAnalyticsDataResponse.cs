using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAnalyticsDataResponse
    {
        public Guid CourseId { get; set; }
        public int NumberOfUniqueVisitors { get; set; }
        public int TotalViews { get; set; }
        public int CurrentlyViewing { get; set; }
        public double AverageViewDurationInMinutes { get; set; }
    }
}