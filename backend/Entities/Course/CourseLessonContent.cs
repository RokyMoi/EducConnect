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
        public Guid CourseLessonContentId { get; set; } = new Guid();

        public Guid CourseLessonId { get; set; }

        [ForeignKey("CourseLessonId")]
        public CourseLesson? CourseLesson { get; set; } = null;

        public string Content { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RowGuid { get; set; } = Guid.NewGuid();
    }


}