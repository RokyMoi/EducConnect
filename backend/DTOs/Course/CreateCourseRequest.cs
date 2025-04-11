using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.DTOs.Course
{
    public class CreateCourseRequest
    {
        [Required]
        [MinLength(2)]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MinLength(15)]
        [MaxLength(1000)]
        public string Description { get; set; }

        public Guid CourseCategoryId { get; set; }

        public int LearningDifficultyLevelId { get; set; }

        [Range(typeof(decimal), "0", "999999999999999.99", ErrorMessage = "The value must be greater than 0, or leave 0 for free courses")]
        public decimal Price { get; set; }
        [Range(1, 1000000000, ErrorMessage = "The value must be greater than 0")]
        public int? MinNumberOfStudents { get; set; }

        [Range(1, 1000000000, ErrorMessage = "The value must be greater than 0")]
        public int? MaxNumberOfStudents { get; set; }


    }
}