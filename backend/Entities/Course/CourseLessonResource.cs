using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{
    [Table("CourseLessonResource", Schema = "Course")]
    public class CourseLessonResource
    {
        [Key]
        public Guid CourseLessonResourceId { get; set; } = Guid.NewGuid();

        public Guid CourseLessonId { get; set; }

        [ForeignKey(nameof(CourseLessonId))]
        public CourseLesson? CourseLesson { get; set; } = null;

        public string Title { get; set; }

        public string? FileName { get; set; }

        public string? ContentType { get; set; }

        public long? FileSize { get; set; }

        public byte[]? FileData { get; set; }

        public string? ResourceUrl { get; set; }
        public string Description { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;

    }
}