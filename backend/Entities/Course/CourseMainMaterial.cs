using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Course
{
    [Table("CourseMainMaterial", Schema = "Course")]
    public class CourseMainMaterial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CourseMainMaterialId { get; set; }
        public Guid CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; } = null;
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long ContentSize { get; set; } 

        public byte[] Data { get; set; }

        public DateTime DateTimePointOfFileCreation { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;

    }
}