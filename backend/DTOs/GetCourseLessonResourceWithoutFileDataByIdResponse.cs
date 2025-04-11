using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Course;

namespace EduConnect.DTOs
{
    public class GetCourseLessonResourceWithoutFileDataByIdResponse
    {
        public Guid CourseLessonResourceId { get; set; } = Guid.NewGuid();

        public Guid CourseLessonId { get; set; }

        public string Title { get; set; }

        public string? FileName { get; set; }

        public string? ContentType { get; set; }

        public long? FileSize { get; set; }

        public string? ResourceUrl { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        

    }
}