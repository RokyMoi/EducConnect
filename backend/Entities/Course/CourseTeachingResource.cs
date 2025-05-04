using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{
    [Table("CourseTeachingResource", Schema = "Course")]
    public class CourseTeachingResource
    {
        [Key]
        public Guid CourseTeachingResourceId { get; set; } = Guid.NewGuid();

        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public Course? Course { get; set; } = null;

        public string Title { get; set; }

        public string? FileName { get; set; }

        public string? ContentType { get; set; }

        public long? FileSize { get; set; }

        public byte[]? FileData { get; set; }

        public string? ResourceUrl { get; set; }

        public string Description { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;

        public Guid? FolderId { get; set; }

        [ForeignKey(nameof(FolderId))]
        public Folder? Folder { get; set; } = null;


    }
}