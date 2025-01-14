using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CourseLessonDTO
    {
        public Guid CourseLessonId { get; set; }
        public Guid CourseId { get; set; }
        public required string LessonTitle { get; set; }
        public required string LessonDescription { get; set; }
        public required int LessonSequenceOrder { get; set; }
        public string LessonPrerequisites { get; set; }
        public required string LessonObjective { get; set; }
        public required int LessonCompletionTimeInMinutes { get; set; }
        public required string LessonTag { get; set; }

    }
}