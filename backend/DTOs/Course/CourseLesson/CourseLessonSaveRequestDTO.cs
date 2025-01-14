using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course.CourseLesson
{
    public class CourseLessonSaveRequestDTO
    {

        [Required]
        public Guid CourseId { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(255)]
        public required string LessonTitle { get; set; }

        [Required]
        [MinLength(50)]
        [MaxLength(510)]
        public required string LessonDescription { get; set; }


        [Required]
        public required int LessonSequenceOrder { get; set; }

        [MaxLength(510)]

        public string LessonPrerequisites { get; set; } = string.Empty;

        [Required]
        [MinLength(10)]
        [MaxLength(255)]
        public required string LessonObjective { get; set; }

        [Required]
        public required int LessonCompletionTimeInMinutes { get; set; } = 1;

        [Required]
        [MinLength(1)]
        public required string LessonTag { get; set; }

    }
}