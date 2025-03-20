using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class CourseManagementDashboardResponse
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string DifficultyLevel { get; set; }
        public string Category { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsThumbnailAdded { get; set; }
        public DateTime? ThumbnailAddedOn { get; set; }
        public bool IsUsingAzureStorage { get; set; }

    }
}