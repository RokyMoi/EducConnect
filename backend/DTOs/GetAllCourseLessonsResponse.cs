using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class GetAllCourseLessonsResponse
    {
        public Guid CourseId { get; set; }
        public Guid CourseLessonId { get; set; }
        public string Title { get; set; }
        public string ShortSummary { get; set; }

        public string Description { get; set; }

        public string Topic { get; set; }

        public int? LessonSequenceOrder { get; set; }

        public bool? PublishedStatus { get; set; }

        public DateTime? StatusChangedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Content { get; set; }



    }
}