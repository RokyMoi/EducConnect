using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Course.Basic
{
    public class CourseBasicSaveRequestDTO
    {
        [Required]
        public string CourseName { get; set; }

        [Required]
        public string CourseSubject { get; set; }

        [Required]
        public string CourseDescription { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public Guid LearningSubcategoryId { get; set; }

        [Required]
        public int LearningDifficultyLevelId { get; set; }

        [Required]
        public int CourseTypeId { get; set; }





    }
}