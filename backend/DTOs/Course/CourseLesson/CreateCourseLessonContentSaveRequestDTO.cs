using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CreateCourseLessonContentSaveRequestDTO
    {
        [Required]
        public required Guid CourseLessonId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string Description { get; set; }

        [Required]
        public required string ContentData { get; set; }
    }
}