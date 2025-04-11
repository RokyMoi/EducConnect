using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CourseLessonContentCreateDTO
    {

        public required Guid CourseLessonId { get; set; }
        public required string Title { get; set; }

        public required string Description { get; set; }
        
        public required string ContentData { get; set; }

    }
}