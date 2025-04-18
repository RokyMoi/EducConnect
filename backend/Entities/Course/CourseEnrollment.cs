using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{

    [Table("CourseEnrollment", Schema = "Course")]
    public class CourseEnrollment
    {
        [Key]
        public Guid CourseEnrollmentId { get; set; } = Guid.NewGuid();

        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; } = null;

        public Guid StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student.Student? Student { get; set; } = null;

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;
    }
}