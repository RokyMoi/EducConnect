using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
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

        //Navigation property
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        public string CourseDescription { get; set; }


        public Guid LearningSubcategoryId { get; set; }

        //Navigation property
        [ForeignKey("LearningSubcategoryId")]
        public LearningSubcategory? LearningSubcategory { get; set; }

        public int LearningDifficultyLevelId { get; set; }

        //Navigation property
        [ForeignKey("LearningDifficultyLevelId")]
        public LearningDifficultyLevel? LearningDifficultyLevel { get; set; }

        public DateTime CourseStartsAt { get; set; }

        public DateTime? CourseEndsAt { get; set; }

        public int EstimatedDurationToCompleteTheCourseInHours { get; set; }

        //The price is set in USD, and is converted according to the user specification
        public int Price { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public long? UpdatedAt { get; set; } = null;

    }
}