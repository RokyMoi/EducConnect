using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{
    [Table("CourseTag", Schema = "Course")]
    public class CourseTag
    {
        [Key]
        public Guid CourseTagId { get; set; } = Guid.NewGuid();

        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; } = null;

        public Guid TagId { get; set; }

        [ForeignKey(nameof(TagId))]
        public Tag? Tag { get; set; } = null;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;
    }
}