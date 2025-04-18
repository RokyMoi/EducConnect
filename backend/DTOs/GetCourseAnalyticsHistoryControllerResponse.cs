using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetCourseAnalyticsHistoryControllerResponse
    {
        public int UsersCameFromFeedCount { get; set; } = 0;
        public int UsersCameFromSearchCount { get; set; } = 0;
        public List<GetCourseAnalyticsHistoryResponse> CourseAnalyticsHistory { get; set; } = [];
    }
}