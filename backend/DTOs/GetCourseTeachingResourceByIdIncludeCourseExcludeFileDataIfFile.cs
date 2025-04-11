using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetCourseTeachingResourceByIdIncludeCourseExcludeFileDataIfFile
    {
        public Guid CourseTeachingResourceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ResourceUrl { get; set; }

        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public long? FileSize { get; set; }

        public long CreatedAt { get; set; }

        public long? UpdatedAt { get; set; }

        public required EduConnect.Entities.Course.Course Course { get; set; }
    }
}