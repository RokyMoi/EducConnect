using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using backend.Entities.Course;
using backend.Entities.Learning;
using backend.Entities.Reference.Learning;
using MimeKit.Cryptography;

namespace EduConnect.Entities.Course
{
    [Table("CourseDetails", Schema = "Course")]
    public class CourseDetails
    {
        [Key]
        public Guid CourseId { get; set; }

        [ForeignKey("CourseId")]
        //Navigation property
        public Course Course { get; set; }
        public string CourseDescription { get; set; }

        public double Price { get; set; }

        public Guid LearningSubcategoryId { get; set; }

        //Navigation property
        public LearningSubcategory LearningSubcategory { get; set; }

        public int LearningDifficultyLevelId { get; set; }

        //Navigation property
        public LearningDifficultyLevel LearningDifficultyLevel { get; set; }

        public int CourseTypeId { get; set; }

        //Nagivation property
        public CourseType CourseType { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public long? UpdatedAt { get; set; } = null;

    }
}