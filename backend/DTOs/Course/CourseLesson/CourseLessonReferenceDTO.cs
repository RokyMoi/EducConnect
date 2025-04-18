using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CourseLessonReferenceDTO
    {
        public Guid CourseId { get; set; }

        public Guid CourseLessonId { get; set; }

        public Guid TutorId { get; set; }
    }
}