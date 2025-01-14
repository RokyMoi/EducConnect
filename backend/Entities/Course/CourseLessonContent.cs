using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{
    [Table("CourseLessonContent", Schema = "Course")]
    public class CourseLessonContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CourseLessonContentId { get; set; } = new Guid();

        public Guid CourseLessonId { get; set; }

        [ForeignKey("CourseLessonId")]
        public CourseLesson? Lesson { get; set; } = null;

        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string Description { get; set; }

        public required string ContentType { get; set; }
        public required string ContentData { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;


    }
}