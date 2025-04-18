using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Enums;
using Microsoft.EntityFrameworkCore.Storage;

namespace EduConnect.Entities.Course
{
    [Table("CourseViewershipData", Schema = "Course")]
    public class CourseViewershipData
    {
        [Column("CourseViewershipDataId")]
        public Guid CourseViewershipDataId { get; set; } = Guid.NewGuid();

        [Column("CourseId")]
        public Guid CourseId { get; set; }

        [Column("ViewedByPersonId")]
        public Guid ViewedByPersonId { get; set; }

        [Column("ClickedOn")]
        public DateTime ClickedOn { get; set; }

        [Column("EnteredDetailsAt")]
        public DateTime? EnteredDetailsAt { get; set; }

        [Column("LeftDetailsAt")]
        public DateTime? LeftDetailsAt { get; set; }

        [Column("UserCameFrom")]
        public UserCourseSourceType UserCameFrom { get; set; }

        [Column("CreatedAt")]
        public long CreatedAt { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        [Column("UpdatedAt")]
        public long? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        [ForeignKey("ViewedByPersonId")]
        public Person.Person? Person { get; set; }
    }
}