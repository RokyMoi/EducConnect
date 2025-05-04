using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{
    [Table("CourseThumbnail", Schema = "Course")]
    public class CourseThumbnail
    {
        public Guid CourseThumbnailId { get; set; }

        public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public EduConnect.Entities.Course.Course? Course { get; set; } = null;

        public string ContentType { get; set; }
        public string? ThumbnailUrl { get; set; }

        public byte[]? ThumbnailImageFile { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;

        public Guid? FolderId { get; set; }
        [ForeignKey(nameof(FolderId))]
        public Folder? Folder { get; set; } = null;
    }
}