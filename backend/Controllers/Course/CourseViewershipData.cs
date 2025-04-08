using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Course;
using EduConnect.Enums;
using Microsoft.EntityFrameworkCore.Storage;

namespace EduConnect.Controllers.Course
{
    [Table("CourseViewershipData", Schema = "Course")]
    public class CourseViewershipData
    {
        public Guid CourseViewershipDataId { get; set; } = Guid.NewGuid();
        public Guid CourseId { get; set; }

        [ForeignKey("CourseId")]
        public EduConnect.Entities.Course.Course? Course { get; set; } = null;

        public Guid ViewedByPersonId { get; set; }

        [ForeignKey(nameof(ViewedByPersonId))]
        public EduConnect.Entities.Person.Person? Person { get; set; } = null;

        public DateTime ClickedOn { get; set; }

        public DateTime? EnteredDetailsAt { get; set; } = null;

        public DateTime? LeftDetailsAt { get; set; } = null;

        public UserCourseSourceType UserCameFrom { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;


    }
}