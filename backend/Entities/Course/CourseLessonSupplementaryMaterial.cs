using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{
    [Table("CourseLessonSupplementaryMaterial", Schema = "Course")]
    public class CourseLessonSupplementaryMaterial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CourseLessonSupplementaryMaterialId { get; set; }
        public Guid CourseLessonId { get; set; }

        [ForeignKey("CourseLessonId")]
        public CourseLesson? CourseLesson { get; set; } = null;
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long ContentSize { get; set; }

        public byte[] Data { get; set; }

        public long DateTimePointOfFileCreation { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;
    }
}