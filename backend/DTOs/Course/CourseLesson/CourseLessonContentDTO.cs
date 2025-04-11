using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CourseLessonContentDTO
    {
        public Guid CourseLessonId { get; set; }
        public Guid CourseLessonContentId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }
        public string ContentData { get; set; }
    }
}