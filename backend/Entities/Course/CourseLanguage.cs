using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Reference.Language;

namespace EduConnect.Entities.Course
{
    [Table("CourseLanguage", Schema = "Course")]
    public class CourseLanguage
    {
        [Key]
        public Guid CourseLanguageId { get; set; }

        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; } = null;

        public Guid LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public Language? Language { get; set; } = null;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;


    }
}