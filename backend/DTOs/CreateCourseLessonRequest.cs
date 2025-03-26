using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs
{
    public class CreateCourseLessonRequest
    {
        public Guid CourseId { get; set; }

        public Guid? CourseLessonId { get; set; } = null;

        [Required]
        [MinLength(15)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MinLength(45)]
        [MaxLength(250)]
        public string ShortSummary { get; set; }

        [Required]
        [MinLength(70)]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Topic { get; set; }

        [Required]
        [MinLength(100)]
        [MaxLength(100000)]
        public string Content { get; set; }

        [Range(1, int.MaxValue)]
        public int? LessonSequenceOrder { get; set; } = null;

        public bool? PublishedStatus { get; set; } = false;
    }
}