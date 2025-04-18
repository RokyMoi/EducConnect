using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.DTOs.Course.CourseLesson;

namespace backend.DTOs.Course.CourseLesson
{
    public class CourseLessonWithCourseLessonContentDTO
    {
        public Guid CourseLessonId { get; set; }

        public Guid CourseId { get; set; }

        public Guid TutorId { get; set; }

        public CourseLessonDTO CourseLesson { get; set; }

        public CourseLessonContentDTO CourseLessonContent { get; set; }
    }
}