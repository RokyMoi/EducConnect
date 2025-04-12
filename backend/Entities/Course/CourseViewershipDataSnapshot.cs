using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{
    [Table("CourseViewershipDataSnapshot", Schema = "Course")]
    public class CourseViewershipDataSnapshot
    {
        [Key]
        public Guid CourseViewershipDataSnapshotId { get; set; } = Guid.NewGuid();
        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; } = null;

        public int TotalViews { get; set; }
        public int NumberOfUniqueVisitors { get; set; }
        public int CurrentlyViewing { get; set; }
        public double AverageViewDurationInMinutes { get; set; }
        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public long? UpdatedAt { get; set; } = null;
    }
}