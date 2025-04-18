using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class UpdateCourseLessonAndCourseLessonContentDTO
    {
        [Required]
        public Guid CourseLessonId { get; set; }

        public string? LessonTitle { get; set; } = string.Empty;

        public bool UpdateLessonTitle { get; set; } = false;

        public string? LessonDescription { get; set; }

        public bool UpdateLessonDescription { get; set; } = false;

        public int? LessonSequenceOrder { get; set; }
        public bool UpdateLessonSequenceOrder { get; set; } = false;

        public string? LessonPrerequisites { get; set; } = string.Empty;
        public bool UpdateLessonPrerequisites { get; set; } = false;

        public string? LessonObjective { get; set; }

        public bool UpdateLessonObjective { get; set; } = false;

        public int? LessonCompletionTimeInMinutes { get; set; }

        public bool UpdateLessonCompletionTimeInMinutes { get; set; } = false;

        public string? LessonTag { get; set; }

        public bool UpdateLessonTag { get; set; } = false;

        public string? LessonContentTitle { get; set; } = string.Empty;

        public bool UpdateLessonContentTitle { get; set; } = false;

        public string? LessonContentDescription { get; set; }

        public bool UpdateLessonContentDescription { get; set; } = false;

        public string? LessonContentData { get; set; }

        public bool UpdateLessonContentData { get; set; } = false;


    }
}