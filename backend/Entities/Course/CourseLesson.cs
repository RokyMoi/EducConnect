using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Course;

namespace EduConnect.Entities.Course
{
    [Table("CourseLesson", Schema = "Course")]
    public class CourseLesson
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CourseLessonId { get; set; }

        public Guid CourseId { get; set; }

        [ForeignKey("CourseId")]
        public EduConnect.Entities.Course.Course? Course { get; set; } = null;

        public required string LessonTitle { get; set; }

        public required string LessonDescription { get; set; }

        public required int LessonSequenceOrder { get; set; }

        [MaxLength(510)]

        public string LessonPrerequisites { get; set; } = string.Empty;

        [MaxLength(255)]

        public required string LessonObjective { get; set; }

        public required int LessonCompletionTimeInMinutes { get; set; } = 1;

        public required string LessonTag { get; set; }
        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;


    }
}